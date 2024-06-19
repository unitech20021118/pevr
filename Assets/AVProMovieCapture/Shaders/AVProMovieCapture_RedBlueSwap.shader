// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AVProMovieCapture/CompositeSwapChannelsRB" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Pass
		{ 
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
		
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers flash xbox360 ps3 gles
//#pragma fragmentoption ARB_precision_hint_fastest 
#pragma fragmentoption ARB_precision_hint_nicest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
float4 _MainTex_ST;
float4 _MainTex_TexelSize;

struct v2f {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

v2f vert( appdata_img v )
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord.xy;//TRANSFORM_TEX(v.texcoord, _MainTex);
	
	return o;
}

float4 frag (v2f i) : COLOR
{
	float4 col = tex2D(_MainTex, i.uv).bgra;
	
	return col;
} 
ENDCG
		}
	}
	
	FallBack Off
}