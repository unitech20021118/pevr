Shader "AVProVideo/Lit/Transparent Diffuse (texture+color+fog+packed alpha)" 
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "black" {}
		_ChromaTex("Chroma", 2D) = "black" {}

		[KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack("Alpha Pack", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:VertexFunction alpha
		#pragma multi_compile ALPHAPACK_NONE ALPHAPACK_TOP_BOTTOM ALPHAPACK_LEFT_RIGHT

		// TODO: Change XX_OFF to __ for Unity 5.0 and above
		// this was just added for Unity 4.x compatibility as __ causes
		// Android and iOS builds to fail the shader
		#pragma multi_compile APPLY_GAMMA_OFF APPLY_GAMMA
		#pragma multi_compile USE_YPCBCR_OFF USE_YPCBCR

		#include "AVProVideo.cginc"

		uniform sampler2D _MainTex;
		//uniform float4 _MainTex_ST;
		uniform float4 _MainTex_TexelSize;
#if USE_YPCBCR
		uniform sampler2D _ChromaTex;
#endif
		uniform fixed4 _Color;
		uniform float3 _cameraPosition;

		struct Input 
		{
			float4 texcoords;
		};

		void VertexFunction(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.texcoords = OffsetAlphaPackingUV(_MainTex_TexelSize.xy, v.texcoord.xy, true);// _MainTex_ST.y < 0.0);
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
#if USE_YPCBCR
	#if SHADER_API_METAL || SHADER_API_GLES || SHADER_API_GLES3
			float3 ypcbcr = float3(tex2D(_MainTex, IN.texcoords.xy).r, tex2D(_ChromaTex, IN.texcoords.xy).rg);
	#else
			float3 ypcbcr = float3(tex2D(_MainTex, IN.texcoords.xy).r, tex2D(_ChromaTex, IN.texcoords.xy).ra);
	#endif
			fixed4 col = fixed4(Convert420YpCbCr8ToRGB(ypcbcr), 1.0);
#else
			fixed4 col = tex2D(_MainTex, IN.texcoords.xy);
#endif
#if APPLY_GAMMA
			col.rgb = GammaToLinear(col.rgb);
#endif
			
#if ALPHAPACK_TOP_BOTTOM | ALPHAPACK_LEFT_RIGHT
				// Sample the alpha
	#if USE_YPCBCR
				col.a = tex2D(_MainTex, IN.texcoords.zw).r;
	#else
				fixed4 alpha = tex2D(_MainTex, IN.texcoords.zw);
		#if APPLY_GAMMA
				alpha.rgb = GammaToLinear(alpha.rgb);
		#endif
				col.a = (alpha.r + alpha.g + alpha.b) / 3.0;
	#endif
#endif
				col *= _Color;
				o.Albedo = col.rgb;
				o.Alpha = col.a;
		}
		ENDCG
	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}