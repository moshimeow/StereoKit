#ifndef SK_NO_FLATSCREEN
#ifndef WINDOWS_UWP

#include "win32.h"
#include "flatscreen_input.h"

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <dxgi1_2.h>

#include "../../stereokit.h"
#include "../../_stereokit.h"
#include "../../asset_types/texture.h"
#include "../render.h"
#include "../d3d.h"
#include "../input.h"
#include "../hand/input_hand.h"

namespace sk {

///////////////////////////////////////////

HWND             win32_window    = nullptr;
tex_t            win32_target    = {};
IDXGISwapChain1 *win32_swapchain = {};
float            win32_scroll    = 0;

// For managing window resizing
bool win32_check_resize = true;
bool win32_resize_drag  = false;
UINT win32_resize_x     = 0;
UINT win32_resize_y     = 0;

///////////////////////////////////////////

void win32_resize(int width, int height) {
	if (width == sk_info.display_width && height == sk_info.display_height)
		return;
	sk_info.display_width  = width;
	sk_info.display_height = height;
	log_diagf("Resized to: %d<~BLK>x<~clr>%d", width, height);
	
	if (win32_swapchain != nullptr) {
		tex_releasesurface(win32_target);
		win32_swapchain->ResizeBuffers(0, (UINT)sk_info.display_width, (UINT)sk_info.display_height, DXGI_FORMAT_UNKNOWN, 0);
		ID3D11Texture2D *back_buffer;
		win32_swapchain->GetBuffer(0, IID_PPV_ARGS(&back_buffer));
		tex_setsurface(win32_target, back_buffer, DXGI_FORMAT_R8G8B8A8_UNORM_SRGB);
		back_buffer->Release();
	}

	render_update_projection();
}

///////////////////////////////////////////

bool win32_init(const char *app_name) {
	sk_info.display_width  = sk_settings.flatscreen_width;
	sk_info.display_height = sk_settings.flatscreen_height;
	if (!d3d_init(nullptr))
		return false;

	sk_info.display_type = display_opaque;

	WNDCLASS wc = {0}; 
	wc.lpfnWndProc = [](HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
		switch(message) {
		case WM_CLOSE:     sk_run     = false; PostQuitMessage(0); break;
		case WM_SETFOCUS:  sk_focused = true;  break;
		case WM_KILLFOCUS: sk_focused = false; break;
		case WM_MOUSEWHEEL:win32_scroll += (short)HIWORD(wParam); break;
		case WM_SYSCOMMAND: {
			// Has the user pressed the restore/'un-maximize' button?
			// WM_SIZE happens -after- this event, and contains the new size.
			if (wParam == SC_RESTORE) {
				win32_check_resize = true;
			}
			// Disable alt menu
			if ((wParam & 0xfff0) == SC_KEYMENU) 
				return (LRESULT)0; 
		} return DefWindowProc(hWnd, message, wParam, lParam); 
		case WM_SIZE: {
			win32_resize_x = (UINT)LOWORD(lParam);
			win32_resize_y = (UINT)HIWORD(lParam);

			// Don't check every time the size changes, this can lead to ugly memory alloc.
			// If a restore event, a maximize, or something else says we should resize, check it!
			if (win32_check_resize || wParam == SIZE_MAXIMIZED) { 
				win32_check_resize = false; 
				win32_resize(win32_resize_x, win32_resize_y); 
			}
		} return DefWindowProc(hWnd, message, wParam, lParam);
		case WM_EXITSIZEMOVE: {
			// If the user was dragging the window around, WM_SIZE is called -before- this 
			// event, so we can go ahead and resize now!
			if (win32_resize_drag) {
				win32_resize_drag = false;
				win32_resize(win32_resize_x, win32_resize_y);
			}
		} return DefWindowProc(hWnd, message, wParam, lParam);
		case WM_ENTERSIZEMOVE: win32_resize_drag = true; return DefWindowProc(hWnd, message, wParam, lParam);
		default: return DefWindowProc(hWnd, message, wParam, lParam);
		}
		return (LRESULT)0;
	};
	wc.hInstance     = GetModuleHandle(NULL);
	wc.hbrBackground = (HBRUSH)(COLOR_BACKGROUND);
	wc.lpszClassName = app_name;
	if( !RegisterClass(&wc) ) return false;
	win32_window = CreateWindow(
		wc.lpszClassName, 
		app_name, 
		WS_OVERLAPPEDWINDOW | WS_VISIBLE, 
		sk_settings.flatscreen_pos_x, 
		sk_settings.flatscreen_pos_y, 
		sk_info.display_width, 
		sk_info.display_height, 
		0, 0, 
		wc.hInstance, 
		nullptr);
	if( !win32_window ) return false;

	// Create a swapchain for the window
	DXGI_SWAP_CHAIN_DESC1 sd = { };
	sd.BufferCount = 2;
	sd.Width       = sk_info.display_width;
	sd.Height      = sk_info.display_height;
	sd.Format      = DXGI_FORMAT_R8G8B8A8_UNORM;
	sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	sd.SwapEffect  = DXGI_SWAP_EFFECT_FLIP_DISCARD;
	sd.SampleDesc.Count = 1;
	
	IDXGIDevice2  *dxgi_device;  d3d_device  ->QueryInterface(__uuidof(IDXGIDevice2),  (void **)&dxgi_device);
	IDXGIAdapter  *dxgi_adapter; dxgi_device ->GetParent     (__uuidof(IDXGIAdapter),  (void **)&dxgi_adapter);
	IDXGIFactory2 *dxgi_factory; dxgi_adapter->GetParent     (__uuidof(IDXGIFactory2), (void **)&dxgi_factory);

	dxgi_factory->CreateSwapChainForHwnd(d3d_device, win32_window, &sd, nullptr, nullptr, &win32_swapchain);
	ID3D11Texture2D *back_buffer;
	win32_swapchain->GetBuffer(0, IID_PPV_ARGS(&back_buffer));

	win32_target = tex_create(tex_type_rendertarget, tex_format_rgba32_linear);
	tex_set_id     (win32_target, "stereokit/system/rendertarget");
	tex_setsurface (win32_target, back_buffer, DXGI_FORMAT_R8G8B8A8_UNORM_SRGB);
	tex_add_zbuffer(win32_target, tex_format_depth16);

	back_buffer ->Release();
	dxgi_factory->Release();
	dxgi_adapter->Release();
	dxgi_device ->Release();

	flatscreen_input_init();

	return true;
}

///////////////////////////////////////////

void win32_shutdown() {
	flatscreen_input_shutdown();
	tex_release(win32_target);
	win32_swapchain->Release();

	d3d_shutdown();
}

///////////////////////////////////////////

void win32_step_begin() {
	d3d_update();

	MSG msg = {0};
	if (PeekMessage(&msg, nullptr, 0U, 0U, PM_REMOVE)) {
		TranslateMessage(&msg);
		DispatchMessage (&msg);
	}
	flatscreen_input_update();
}

///////////////////////////////////////////

void win32_step_end() {
	// Set up where on the render target we want to draw, the view has a 
	D3D11_VIEWPORT viewport = CD3D11_VIEWPORT(0.f, 0.f, (float)sk_info.display_width, (float)sk_info.display_height);
	d3d_context->RSSetViewports(1, &viewport);

	// Wipe our swapchain color and depth target clean, and then set them up for rendering!
	tex_rtarget_clear(win32_target, render_get_clear_color());
	tex_rtarget_set_active(win32_target);

	input_update_predicted();

	matrix view = render_get_cam_root  ();
	matrix proj = render_get_projection();
	matrix_inverse(view, view);
	render_draw_matrix(&view, &proj, 1);
	render_clear();
}

///////////////////////////////////////////

void win32_vsync() {
	win32_swapchain->Present(1, 0);
}

///////////////////////////////////////////

void *win32_hwnd() {
	return win32_window;
}

} // namespace sk

#endif // WINDOWS_UWP
#endif // SK_NO_FLATSCREEN