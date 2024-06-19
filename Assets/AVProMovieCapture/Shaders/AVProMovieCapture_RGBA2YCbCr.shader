// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AVProMovieCapture/RGBA2YCbCr" 
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
#include "AVProMovieCapture_Shared.cginc"

uniform sampler2D _MainTex;
float4 _MainTex_ST;
float4 _MainTex_TexelSize;
float flipY;

struct v2f {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

v2f vert( appdata_img v )
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord.xy;//TRANSFORM_TEX(v.texcoord, _MainTex);
		
	// Often in YCbCr modes we need to flip Y
	if (flipY > 0.0)
	{
		o.uv.y = 1-o.uv.y;	
	}

	return o;
}

float4 frag (v2f i) : COLOR
{
	float2 uv = i.uv;
	
	//float4 col = tex2D(_MainTex, uv.xy ).bgra;
	
	float2 texel = float2(_MainTex_TexelSize.x, 0.0);
	float4 col = tex2D(_MainTex, uv);
	float4 col2 = tex2D(_MainTex, uv + texel);
	float4 yuv1 = rgb2yuv(col);
	float4 yuv2 = rgb2yuv(col2);

	float y1 = yuv1.x;
	float v = saturate((yuv1.y + yuv2.y) * 0.5 + 0.5);
	float y2 = yuv2.x;
	float u = saturate((yuv1.z + yuv2.z) * 0.5 + 0.5);

	//UYVY
	//float4 oCol = float4(u, y1, v, y2);	
	
	// YUY2
	float4 oCol = float4(y1, v, y2, u);
		
				
	return oCol;
} 
ENDCG
		}
	}
	
	FallBack Off
}