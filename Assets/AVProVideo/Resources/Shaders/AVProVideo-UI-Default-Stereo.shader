Shader "AVProVideo/UI/Stereo"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _ChromaTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[KeywordEnum(None, Top_Bottom, Left_Right)] Stereo("Stereo Mode", Float) = 0
		[Toggle(STEREO_DEBUG)] _StereoDebug("Stereo Debug Tinting", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile MONOSCOPIC STEREO_TOP_BOTTOM STEREO_LEFT_RIGHT

			// TODO: Change XX_OFF to __ for Unity 5.0 and above
			// this was just added for Unity 4.x compatibility as __ causes
			// Android and iOS builds to fail the shader
			#pragma multi_compile APPLY_GAMMA_OFF APPLY_GAMMA
			#pragma multi_compile STEREO_DEBUG_OFF STEREO_DEBUG			
			#pragma multi_compile USE_YPCBCR_OFF USE_YPCBCR

			#include "UnityCG.cginc"
			#include "AVProVideo.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
#if USE_YPCBCR
			uniform sampler2D _ChromaTex;
#endif
			uniform float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			uniform float3 _cameraPosition;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = XFormObjectToClip(IN.vertex);

#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
#endif

				OUT.texcoord.xy = IN.texcoord.xy;

#if STEREO_TOP_BOTTOM | STEREO_LEFT_RIGHT
				float4 scaleOffset = GetStereoScaleOffset(IsStereoEyeLeft(_cameraPosition, UNITY_MATRIX_V[0].xyz), _MainTex_ST.y < 0.0);
				OUT.texcoord.xy *= scaleOffset.xy;
				OUT.texcoord.xy += scaleOffset.zw;
#endif

				OUT.color = IN.color * _Color;

#if STEREO_DEBUG
				OUT.color *= GetStereoDebugTint(IsStereoEyeLeft(_cameraPosition, UNITY_MATRIX_V[0].xyz));
#endif			
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
#if USE_YPCBCR
	#if SHADER_API_METAL || SHADER_API_GLES || SHADER_API_GLES3
				float3 ypcbcr = float3(tex2D(_MainTex, IN.texcoord).r, tex2D(_ChromaTex, IN.texcoord).rg);
	#else
				float3 ypcbcr = float3(tex2D(_MainTex, IN.texcoord).r, tex2D(_ChromaTex, IN.texcoord).ra);
	#endif
				half4 color = half4(Convert420YpCbCr8ToRGB(ypcbcr), 1.0);
#else
				half4 color = tex2D(_MainTex, IN.texcoord.xy);
#endif
#if APPLY_GAMMA
				color.rgb = GammaToLinear(color.rgb);
#endif

				color *= IN.color;
				return color;
			}

		ENDCG
		}
	}
}
