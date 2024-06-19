Shader "Emulated Mipmap/Standard Transparent" {
	Properties {
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0, 1)) = 0.5
		_Metallic("Metallic", Range(0, 1)) = 0.0
		_SampleMultiplier("Sample Multiplier", Range(0, 8)) = 1.0
		_MaxSamplesPerLine("Line Sample Limit", Range(1, 50)) = 10
	}
	SubShader {
		Tags {
			"RenderType" = "Transparent"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _SampleMultiplier;
		int _MaxSamplesPerLine;
		fixed4 _Color;

		#include "./MipSample.cginc"

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = mipSample(
				_MainTex, IN.uv_MainTex,
				_MainTex_TexelSize.zw,
				_MaxSamplesPerLine, _SampleMultiplier
			);

			o.Albedo = _Color * c;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}

	FallBack "Standard"
}
