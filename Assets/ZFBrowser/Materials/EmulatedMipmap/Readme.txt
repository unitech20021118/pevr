
This folder contains special shaders for rendering mimmap-free textures as if they had mipmaps.

Typically it's better to use mipmaps, but sometimes you only have the texture for one frame and it's cheaper to fake mipmaps in the shader than to generate a new mipmap every frame.

If you need a shader that's not provided here to work with emulated mipmaps you should be able to follow roughly this procedure:
	- Copy the shader in question to this folder (so it can see our .cginc)
		- You can download a copy of Unity's built-in shaders from https://unity3d.com/get-unity/download/archive
	- Change the shader name so it's in the "Emulated Mipmap" folder
	- Add `#include "./MipSample.cginc"`
	- For every "sampler2D _MyTex" you want to emulate mipmaps on, add a "float4 _MyTex_TexelSize"
		- Unity will magically fill this variable with (texel size x, texel size y, texture size x, texture size y)
	- Replace tex2D() calls with mipSample() calls.
		- For an explanation of how to use mipSample, check out the docblock in MipSample.gcinc
