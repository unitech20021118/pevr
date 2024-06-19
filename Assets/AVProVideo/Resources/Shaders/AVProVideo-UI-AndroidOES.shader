Shader "AVProVideo/UI/AndroidOES"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _ChromaTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[KeywordEnum(None, Top_Bottom, Left_Right)] Stereo("Stereo Mode", Float) = 0
		[Toggle(STEREO_DEBUG)] _StereoDebug("Stereo Debug Tinting", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Fog{ Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
			GLSLPROGRAM
			#pragma only_renderers gles gles3

			#pragma multi_compile MONOSCOPIC STEREO_TOP_BOTTOM STEREO_LEFT_RIGHT STEREO_CUSTOM_UV
			#pragma multi_compile STEREO_DEBUG_OFF STEREO_DEBUG			

			#extension GL_OES_EGL_image_external : require
			#extension GL_OES_EGL_image_external_essl3 : enable

			precision mediump float;
	

#ifdef VERTEX

#include "UnityCG.glslinc"
#define SHADERLAB_GLSL
#include "AVProVideo.cginc"
	uniform mat4 _ViewMatrix;
	uniform vec3 _cameraPosition;
	varying vec2 texVal;

#if defined(STEREO_DEBUG)
	varying vec4 tint;
#endif

	void main()
	{
		gl_Position = XFormObjectToClip(gl_Vertex);
		texVal = gl_MultiTexCoord0.xy;
		texVal.y = 1.0 - texVal.y;

#if defined(STEREO_TOP_BOTTOM) | defined(STEREO_LEFT_RIGHT)
		bool isLeftEye = IsStereoEyeLeft(_cameraPosition, _ViewMatrix[0].xyz);

		vec4 scaleOffset = GetStereoScaleOffset(isLeftEye, false);

		texVal.xy *= scaleOffset.xy;
		texVal.xy += scaleOffset.zw;
#elif defined (STEREO_CUSTOM_UV)
		if (!IsStereoEyeLeft(_cameraPosition, _ViewMatrix[0].xyz))
		{
			texVal = gl_MultiTexCoord1.xy;
			texVal = vec2(1.0, 1.0) - texVal;
		}
#endif

#if defined(STEREO_DEBUG)
		tint = GetStereoDebugTint(IsStereoEyeLeft(_cameraPosition, _ViewMatrix[0].xyz));
#endif

	}
#endif

#ifdef FRAGMENT
	varying vec2 texVal;

#if defined(STEREO_DEBUG)
	varying vec4 tint;
#endif

	uniform vec4 _Color;
	uniform samplerExternalOES _MainTex;

	void main()
	{
#if defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)

#if __VERSION__ < 300
		gl_FragColor = texture2D(_MainTex, texVal.xy) * _Color;
#else
		gl_FragColor = texture(_MainTex, texVal.xy) * _Color;
#endif

#else
		gl_FragColor = vec4(1.0, 1.0, 0.0, 1.0);
#endif

#if defined(STEREO_DEBUG)
		gl_FragColor *= tint;
#endif
	}

#endif

	ENDGLSL
	}
	}

	Fallback "AVProVideo/UI/Stereo"
}
