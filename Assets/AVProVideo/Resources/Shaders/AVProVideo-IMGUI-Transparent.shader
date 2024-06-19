Shader "AVProVideo/IMGUI/Texture Transparent"
{
	Properties
	{
		_MainTex("Texture", any) = "" {}
		_ChromaTex("Chroma", any) = "" {}
		_VertScale("Vertical Scale", Range(-1, 1)) = 1.0
		[KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack("Alpha Pack", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}

	SubShader
	{
		Tags { "ForceSupported" = "True" "RenderType" = "Overlay" }

		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile ALPHAPACK_NONE ALPHAPACK_TOP_BOTTOM ALPHAPACK_LEFT_RIGHT

			// TODO: Change XX_OFF to __ for Unity 5.0 and above
			// this was just added for Unity 4.x compatibility as __ causes
			// Android and iOS builds to fail the shader
			#pragma multi_compile APPLY_GAMMA_OFF APPLY_GAMMA
			#pragma multi_compile USE_YPCBCR_OFF USE_YPCBCR

			#include "UnityCG.cginc"
			#include "AVProVideo.cginc"

			struct appdata_t 
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};

			uniform sampler2D _MainTex;
#if USE_YPCBCR
			uniform sampler2D _ChromaTex;
#endif
			uniform float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			uniform float _VertScale;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = XFormObjectToClip(v.vertex);
				o.color = v.color;
				o.texcoord = OffsetAlphaPackingUV(_MainTex_TexelSize.xy, TRANSFORM_TEX(v.texcoord, _MainTex), _VertScale < 0.0);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
#if USE_YPCBCR
	#if SHADER_API_METAL || SHADER_API_GLES || SHADER_API_GLES3
				float3 ypcbcr = float3(tex2D(_MainTex, i.texcoord.xy).r, tex2D(_ChromaTex, i.texcoord.xy).rg);
	#else
				float3 ypcbcr = float3(tex2D(_MainTex, i.texcoord.xy).r, tex2D(_ChromaTex, i.texcoord.xy).ra);
	#endif
				fixed4 col = fixed4(Convert420YpCbCr8ToRGB(ypcbcr), 1.0);
#else
				// Sample RGB
				fixed4 col = tex2D(_MainTex, i.texcoord.xy);
#endif
#if APPLY_GAMMA
				col.rgb = LinearToGamma(col.rgb);
#endif
#if ALPHAPACK_TOP_BOTTOM | ALPHAPACK_LEFT_RIGHT
				// Sample the alpha
	#if USE_YPCBCR
				col.a = tex2D(_MainTex, i.texcoord.zw).r;	
	#else
				fixed4 alpha = tex2D(_MainTex, i.texcoord.zw);
		#if APPLY_GAMMA
				alpha.rgb = LinearToGamma(alpha.rgb);
		#endif
				col.a = (alpha.r + alpha.g + alpha.b) / 3.0;
	#endif
#endif
				return col * i.color;
			}
			ENDCG
		}
	}

	Fallback off
}