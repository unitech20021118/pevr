Shader "AVProVideo/VR/InsideSphere Unlit (stereo) - Android OES ONLY" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_Color("Color", Color) = (0.0, 1.0, 0.0, 1.0)
		[KeywordEnum(None, Top_Bottom, Left_Right, Custom_UV)] Stereo("Stereo Mode", Float) = 0
		[KeywordEnum(None, EquiRect180)] Layout("Layout", Float) = 0
		[Toggle(STEREO_DEBUG)] _StereoDebug("Stereo Debug Tinting", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
	}
	SubShader 
	{
		Tags{ "Queue" = "Geometry" }
		Pass
		{ 
			Cull Front
			//ZTest Always
			ZWrite On
			Lighting Off

			GLSLPROGRAM

			#pragma only_renderers gles gles3
			#pragma multi_compile MONOSCOPIC STEREO_TOP_BOTTOM STEREO_LEFT_RIGHT STEREO_CUSTOM_UV
			#pragma multi_compile __ STEREO_DEBUG
			#pragma multi_compile LAYOUT_NONE LAYOUT_EQUIRECT180
			//#pragma multi_compile __ GOOGLEVR

			#extension GL_OES_EGL_image_external : require
			#extension GL_OES_EGL_image_external_essl3 : enable
			precision mediump float;

			#ifdef VERTEX

#include "UnityCG.glslinc"
#define SHADERLAB_GLSL
#include "AVProVideo.cginc"

		varying vec2 texVal;
		uniform vec3 _cameraPosition;
		uniform mat4 _ViewMatrix;

#if defined(STEREO_DEBUG)
		varying vec4 tint;
#endif

			void main()
			{
				gl_Position = XFormObjectToClip(gl_Vertex);
				texVal = gl_MultiTexCoord0.xy;
				texVal = vec2(1.0, 1.0) - texVal;
#if defined(EQUIRECT180)
				texVal.x = ((texVal.x - 0.5) * 2.0) + 0.5;
#endif
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
	
	Fallback "AVProVideo/VR/InsideSphere Unlit (stereo+fog)"
}