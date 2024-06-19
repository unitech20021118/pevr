Shader "AVProVideo/Lit/Diffuse (texture+color+fog+stereo support)" 
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ChromaTex("Chroma", 2D) = "white" {}

		[KeywordEnum(None, Top_Bottom, Left_Right, Custom_UV)] Stereo("Stereo Mode", Float) = 0
		[Toggle(STEREO_DEBUG)] _StereoDebug("Stereo Debug Tinting", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}

	SubShader
	{
		Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Geometry" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:VertexFunction
		#pragma multi_compile MONOSCOPIC STEREO_TOP_BOTTOM STEREO_LEFT_RIGHT STEREO_CUSTOM_UV

		// TODO: Change XX_OFF to __ for Unity 5.0 and above
		// this was just added for Unity 4.x compatibility as __ causes
		// Android and iOS builds to fail the shader
		#pragma multi_compile APPLY_GAMMA_OFF APPLY_GAMMA
		#pragma multi_compile STEREO_DEBUG_OFF STEREO_DEBUG			
		#pragma multi_compile USE_YPCBCR_OFF USE_YPCBCR

		#include "AVProVideo.cginc"

		uniform sampler2D _MainTex;
#if USE_YPCBCR
		uniform sampler2D _ChromaTex;
#endif
		uniform fixed4 _Color;
		uniform float3 _cameraPosition;

		struct Input 
		{
			float2 uv_MainTex;
#if STEREO_DEBUG
			float4 color;
#endif
		};

		void VertexFunction(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

#if STEREO_TOP_BOTTOM | STEREO_LEFT_RIGHT
			float4 scaleOffset = GetStereoScaleOffset(IsStereoEyeLeft(_cameraPosition, UNITY_MATRIX_V[0].xyz), true);
			o.uv_MainTex = v.texcoord.xy *= scaleOffset.xy;
			o.uv_MainTex = v.texcoord.xy += scaleOffset.zw;
#elif STEREO_CUSTOM_UV
			o.uv_MainTex = v.texcoord.xy;
			if (!IsStereoEyeLeft(_cameraPosition, UNITY_MATRIX_V[0].xyz))
			{
				o.uv_MainTex = v.texcoord1.xy;
			}
#endif

#if STEREO_DEBUG
			o.color = GetStereoDebugTint(IsStereoEyeLeft(_cameraPosition, UNITY_MATRIX_V[0].xyz));
#endif
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
#if USE_YPCBCR
	#if SHADER_API_METAL || SHADER_API_GLES || SHADER_API_GLES3
			float3 ypcbcr = float3(tex2D(_MainTex, IN.uv_MainTex).r, tex2D(_ChromaTex, IN.uv_MainTex).rg);
	#else
			float3 ypcbcr = float3(tex2D(_MainTex, IN.uv_MainTex).r, tex2D(_ChromaTex, IN.uv_MainTex).ra);
	#endif
			fixed4 c = fixed4(Convert420YpCbCr8ToRGB(ypcbcr), 1.0);
#else
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
#endif
#if APPLY_GAMMA
			c.rgb = GammaToLinear(c.rgb);
#endif			
#if STEREO_DEBUG
			c *= IN.color;
#endif
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}