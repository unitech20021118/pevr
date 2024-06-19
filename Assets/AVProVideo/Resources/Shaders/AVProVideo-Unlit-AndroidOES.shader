Shader "AVProVideo/Unlit/Opaque (texture+color support) - Android OES ONLY"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_ChromaTex("Chroma", 2D) = "gray" {}
		_Color("Main Color", Color) = (1,1,1,1)

		[KeywordEnum(None, Top_Bottom, Left_Right)] Stereo("Stereo Mode", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="False" "Queue"="Geometry" }
		LOD 100
		Lighting Off
		Cull Off

		Pass
		{
			GLSLPROGRAM

			#pragma only_renderers gles gles3
			#extension GL_OES_EGL_image_external : require
			#extension GL_OES_EGL_image_external_essl3 : enable
			precision mediump float;

			#ifdef VERTEX

			#include "UnityCG.glslinc"
			#define SHADERLAB_GLSL
			#include "AVProVideo.cginc"
		
			varying vec2 texVal;
			uniform vec4 _MainTex_ST;

			/// @fix: explicit TRANSFORM_TEX(); Unity's preprocessor chokes when attempting to use the TRANSFORM_TEX() macro in UnityCG.glslinc
			/// 	(as of Unity 4.5.0f6; issue dates back to 2011 or earlier: http://forum.unity3d.com/threads/glsl-transform_tex-and-tiling.93756/)
			vec2 transformTex(vec4 texCoord, vec4 texST) 
			{
				return (texCoord.xy * texST.xy + texST.zw);
			}

			void main()
			{
				gl_Position = XFormObjectToClip(gl_Vertex);
				texVal = transformTex(gl_MultiTexCoord0, _MainTex_ST);
				//texVal.x = 1.0 - texVal.x;
				texVal.y = 1.0 - texVal.y;
            }
            #endif  

			#ifdef FRAGMENT

			varying vec2 texVal;

			uniform samplerExternalOES _MainTex;

            void main()
            {          
#if defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
				gl_FragColor = texture2D(_MainTex, texVal.xy);
#else
				gl_FragColor = vec4(1.0, 1.0, 0.0, 1.0);
#endif
			}
            #endif       
				
			ENDGLSL
		}
	}
	
	Fallback "AVProVideo/Unlit/Opaque (texture+color+fog+stereo support)"
}