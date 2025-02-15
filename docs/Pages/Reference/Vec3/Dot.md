---
layout: default
title: Vec3.Dot
description: The dot product is an extremely useful operation! One major use is to determine how similar two vectors are. If the vectors are Unit vectors (magnitude/length of 1), then the result will be 1 if the vectors are the same, -1 if they're opposite, and a gradient in-between with 0 being perpendicular. See [Freya Holmer's excellent visualization](https.//twitter.com/FreyaHolmer/status/1200807790580768768) of this concept
---
# [Vec3]({{site.url}}/Pages/Reference/Vec3.html).Dot

<div class='signature' markdown='1'>
static float Dot(Vec3& a, Vec3& b)
</div>

|  |  |
|--|--|
|Vec3& a|First vector.|
|Vec3& b|Second vector.|
|RETURNS: float|The dot product!|

The dot product is an extremely useful operation! One major use is to determine
how similar two vectors are. If the vectors are Unit vectors (magnitude/length of 1), then
the result will be 1 if the vectors are the same, -1 if they're opposite, and a gradient
in-between with 0 being perpendicular. See [Freya Holmer's excellent visualization](https://twitter.com/FreyaHolmer/status/1200807790580768768)
of this concept




## Examples

```csharp
Pose head        = Input.Head;
Vec3 objPosition = new Vec3(0,0,-1);
bool visible     = Vec3.Dot(head.Forward, objPosition-head.position) > 0;
if (visible)
    Log.Info("Object's position is in front of the user!");
```

