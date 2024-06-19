Shader "AVProVideo/Unlit/Transparent (texture+color+fog+packed alpha)"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "black" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_ChromaTex("Chroma", 2D) = "gray" {}

		[KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack("Alpha Pack", Float) = 0
		[Toggle(APPLY_GAMMA)] _ApplyGamma("Apply Gamma", Float) = 0
		[Toggle(USE_YPCBCR)] _UseYpCbCr("Use YpCbCr", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent" }
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile ALPHAPACK_NONE ALPHAPACK_TOP_BOTTOM ALPHAPACK_LEFT_RIGHT

			// TODO: Change XX_OFF to __ for Unity 5.0 and above
			// this was just added for Unity 4.x compatibility as __ causes
			// Android and iOS builds to fail the shader
			#pragma multi_compile APPLY_GAMMA_OFF APPLY_GAMMA
			#pragma multi_compile USE_YPCBCR_OFF USE_YPCBCR

			#include "UnityCG.cginc"
			#include "AVProVideo.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION; 
				float4 uv : TEXCOORD0;

#if UNITY_VERSION >= 500
				UNITY_FOG_COORDS(1)
#endif
			};

			uniform sampler2D _MainTex;
#if USE_YPCBCR
			uniform sampler2D _ChromaTex;
#endif
			uniform float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			uniform fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = XFormObjectToClip(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

				// Horrible hack to undo the scale transform to fit into our UV packing layout logic...
				if (_MainTex_ST.y < 0.0)
				{
					o.uv.y = 1.0 - o.uv.y;
				}

				o.uv = OffsetAlphaPackingUV(_MainTex_TexelSize.xy, o.uv.xy, _MainTex_ST.y < 0.0);

#if UNITY_VERSION >= 500
				UNITY_TRANSFER_FOG(o, o.vertex);
#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
#if USE_YPCBCR
	#if SHADER_API_METAL || SHADER_API_GLES || SHADER_API_GLES3
				float3 ypcbcr = float3(tex2D(_MainTex, i.uv.xy).r, tex2D(_ChromaTex, i.uv.xy).rg);
	#else
				float3 ypcbcr = float3(tex2D(_MainTex, i.uv.xy).r, tex2D(_ChromaTex, i.uv.xy).ra);
	#endif
				fixed4 col = fixed4(Convert420YpCbCr8ToRGB(ypcbcr), 1.0);
#else
				// Sample RGB
				fixed4 col = tex2D(_MainTex, i.uv.xy);
#endif

#if APPLY_GAMMA
				col.rgb = GammaToLinear(col.rgb);
#endif

#if ALPHAPACK_TOP_BOTTOM | ALPHAPACK_LEFT_RIGHT
				// Sample the alpha
	#if USE_YPCBCR
				col.a = tex2D(_MainTex, i.uv.zw).r;
	#else
				fixed4 alpha = tex2D(_MainTex, i.uv.zw);
		#if APPLY_GAMMA
				alpha.rgb = GammaToLinear(alpha.rgb);
		#endif
				col.a = (alpha.r + alpha.g + alpha.b) / 3.0;
				//col.a = (alpha.r + alpha.g + alpha.g + alpha.b) / 4.0;
				//clip(col.a - 0.01);
	#endif
#endif
				col *= _Color;

#if UNITY_VERSION >= 500
				UNITY_APPLY_FOG(i.fogCoord, col);
#endif

				return col;
			}
			ENDCG
		}
	}
}
