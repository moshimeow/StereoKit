﻿namespace StereoKit
{
	/// <summary>This is a collection of StereoKit default assets that are
	/// created or loaded by StereoKit during its initialization phase! Feel
	/// free to use them or Copy them, but be wary about modifying them, 
	/// since it could affect many things throughout the system.</summary>
	public static class Default
	{
		/// <summary>The default material! This is used by any model or mesh
		/// rendered from within StereoKit. Its shader may change based on 
		/// system performance characteristics, so it can be great to copy 
		/// this one when creating your own materials!</summary>
		public static Material Material         { get; private set; }
		/// <summary>This material is used for projecting equirectangular 
		/// textures into cubemap faces. It's probably not a great idea to 
		/// change this one much!</summary>
		public static Material MaterialEquirect { get; private set; }
		/// <summary>This material is used as the default for rendering text!
		/// By default, it uses the 'default/shader_font' shader, which is a 
		/// two-sided alpha-clip shader. This also turns off backface culling.
		/// </summary>
		public static Material MaterialFont     { get; private set; }
		/// <summary>This is the default material for rendering the hand! 
		/// It's a copy of the default material, but set to transparent, and 
		/// using a generated texture.</summary>
		public static Material MaterialHand     { get; private set; }
		/// <summary>The material used by the UI! By default, it uses a shader 
		/// that creates a 'finger shadow' that shows how close the finger is 
		/// to the UI.</summary>
		public static Material MaterialUI       { get; private set; }

		/// <summary>Default 2x2 white opaque texture, this is the texture 
		/// referred to as 'white' in the shader texture defaults.</summary>
		public static Tex Tex      { get; private set; }
		/// <summary>Default 2x2 black opaque texture, this is the texture 
		/// referred to as 'black' in the shader texture defaults.</summary>
		public static Tex TexBlack { get; private set; }
		/// <summary>Default 2x2 middle gray (0.5,0.5,0.5) opaque texture, 
		/// this is the texture referred to as 'gray' in the shader texture 
		/// defaults.</summary>
		public static Tex TexGray  { get; private set; }
		/// <summary>Default 2x2 flat normal texture, this is a normal that 
		/// faces out from the, face, and has a color value of (0.5,0.5,1). 
		/// This is the texture referred to as 'flat' in the shader texture 
		/// defaults.</summary>
		public static Tex TexFlat  { get; private set; }
		/// <summary>Default 2x2 roughness color (0,0,1) texture, this is the
		/// texture referred to as 'rough' in the shader texture defaults.
		/// </summary>
		public static Tex TexRough { get; private set; }
		/// <summary>The default cubemap that StereoKit generates, this is 
		/// the cubemap that's visible as the background and initial scene 
		/// lighting.</summary>
		public static Tex Cubemap  { get; private set; }

		/// <summary>A default quad mesh, 2 triangles, 4 verts, from (-1,-1,0) 
		/// to (1,1,0) and facing forward on the Z axis (0,0,-1). White 
		/// vertex colors, and UVs from (0,0) at vertex (-1,-1,0) to (1,1) at 
		/// vertex (1,1,0).</summary>
		public static Mesh MeshQuad   { get; private set; }
		/// <summary>A cube with dimensions of (1,1,1), this is equivalent to
		/// Mesh.GenerateCube(Vec3.One).</summary>
		public static Mesh MeshCube   { get; private set; }
		/// <summary>A sphere mesh with a diameter of 1. This is equivalent 
		/// to Mesh.GenerateSphere(1,4).</summary>
		public static Mesh MeshSphere { get; private set; }

		/// <summary>This is a fast, general purpose shader. It uses a
		/// texture for 'diffuse', a 'color' property for tinting the 
		/// material, and a 'tex_scale' for scaling the UV coordinates. For 
		/// lighting, it just uses a lookup from the current cubemap.
		/// </summary>
		public static Shader Shader         { get; private set; }
		/// <summary>A physically based shader.</summary>
		public static Shader ShaderPbr      { get; private set; }
		/// <summary>Sometimes lighting just gets in the way! This is an
		/// extremely simple and fast shader that uses a 'diffuse' texture 
		/// and a 'color' tint property to draw a model without any lighting
		/// at all!</summary>
		public static Shader ShaderUnlit    { get; private set; }
		/// <summary>A shader for text! Right now, this will render a font
		/// atlas texture, and perform alpha-testing for transparency, and 
		/// super-sampling for better readability. It also flips normals of
		/// the back-face of the surface, so  backfaces get lit properly when 
		/// backface culling is turned off, as it is by default for text.
		/// </summary>
		public static Shader ShaderFont     { get; private set; }
		/// <summary>A shader for projecting equirectangular textures onto 
		/// cube faces! This is for equirectangular texture loading.</summary>
		public static Shader ShaderEquirect { get; private set; }
		/// <summary>A shader for UI or interactable elements, this'll be the
		/// same as the Shader, but with an additional finger 'shadow' and 
		/// distance circle effect that helps indicate finger distance from 
		/// the surface of the object.</summary>
		public static Shader ShaderUI       { get; private set; }

		/// <summary>A default click sound that lasts for 300ms. It's a 
		/// procedurally generated sound based on a mouse press, with extra 
		/// low frequencies in it.</summary>
		public static Sound SoundClick   { get; private set; }
		/// <summary>A default click sound that lasts for 300ms. It's a 
		/// procedurally generated sound based on a mouse release, with extra 
		/// low frequencies in it.</summary>
		public static Sound SoundUnclick { get; private set; }

		internal static void Initialize()
		{
			Material         = Material.Find(DefaultIds.material);
			MaterialEquirect = Material.Find(DefaultIds.materialEquirect);
			MaterialFont     = Material.Find(DefaultIds.materialFont);
			MaterialHand     = Material.Find(DefaultIds.materialHand);
			MaterialUI       = Material.Find(DefaultIds.materialUI);

			Tex      = Tex.Find(DefaultIds.tex);
			TexBlack = Tex.Find(DefaultIds.texBlack);
			TexGray  = Tex.Find(DefaultIds.texGray);
			TexFlat  = Tex.Find(DefaultIds.texFlat);
			TexRough = Tex.Find(DefaultIds.texRough);
			Cubemap  = Tex.Find(DefaultIds.cubemap);

			MeshQuad   = Mesh.Find(DefaultIds.meshQuad);
			MeshCube   = Mesh.Find(DefaultIds.meshCube);
			MeshSphere = Mesh.Find(DefaultIds.meshSphere);

			Shader         = Shader.Find(DefaultIds.shader);
			ShaderPbr      = Shader.Find(DefaultIds.shaderPbr);
			ShaderUnlit    = Shader.Find(DefaultIds.shaderUnlit);
			ShaderFont     = Shader.Find(DefaultIds.shaderFont);
			ShaderEquirect = Shader.Find(DefaultIds.shaderEquirect);
			ShaderUI       = Shader.Find(DefaultIds.shaderUI);

			SoundClick   = Sound.Find(DefaultIds.soundClick);
			SoundUnclick = Sound.Find(DefaultIds.soundUnclick);
		}

		internal static void Shutdown()
		{
			Material         = null;
			MaterialEquirect = null;
			MaterialFont     = null;
			MaterialHand     = null;
			MaterialUI       = null;

			Tex      = null;
			TexBlack = null;
			TexGray  = null;
			TexFlat  = null;
			TexRough = null;
			Cubemap  = null;

			MeshQuad   = null;
			MeshCube   = null;
			MeshSphere = null;

			Shader         = null;
			ShaderPbr      = null;
			ShaderUnlit    = null;
			ShaderFont     = null;
			ShaderEquirect = null;
			ShaderUI       = null;

			SoundClick   = null;
			SoundUnclick = null;
		}
	}
}
