Shader "Unlit/Tri-Planar"
{
	Properties
	{
		_DiffuseMap("Main Map ", 2D) = "white" {} //不命名为_DiffuseMap无法在Unity地形中使用
		_TextureScale("Texture Scale",float) = 1
		_TriplanarBlendSharpness("Blend Sharpness",float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal:NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
					float3 normal:NORMAL;
					float4 wVertex : FLOAT;
				};

				float _TextureScale;
				sampler2D _DiffuseMap;
				float4 _DiffuseMap_ST;
				float _TriplanarBlendSharpness;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _DiffuseMap);
					UNITY_TRANSFER_FOG(o,o.vertex);
					o.normal = mul(unity_ObjectToWorld,v.normal);
					o.wVertex = mul(unity_ObjectToWorld,v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					float3 bWeight = (pow(abs(normalize(i.normal)),_TriplanarBlendSharpness));
					//example: sharpness=2:  0,0.25,1--->0,0.0625,1
					bWeight = bWeight / (bWeight.x + bWeight.y + bWeight.z);
					float4 xaxis_tex = tex2D(_DiffuseMap,i.wVertex.zy / _TextureScale);
					float4 yaxis_tex = tex2D(_DiffuseMap,i.wVertex.xz / _TextureScale);
					float4 zaxis_tex = tex2D(_DiffuseMap,i.wVertex.xy / _TextureScale);
					fixed4 tex = xaxis_tex * bWeight.x + yaxis_tex * bWeight.y + zaxis_tex * bWeight.z;
					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, tex);
					return tex;
				}
				ENDCG
			}
		}
}

