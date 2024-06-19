Shader "Unlit/Self_Illumination"
{
	Properties
	{
	_IlluminCol("Self-Illumination color (RGB)",Color)=(0.64,0.64,0.64,1)
	_MainTex("Base (RGB) Self-Illumination (A)",2D)="white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry+1000" "IGNOREPROJECTOR"="false" }
		ZTest Always

		Pass
		{
			Tags{"Queue"="Geometry" "IGNOREPROJECTOR"="false" }
			ZTest Always
			Material{
			 Ambient(1,1,1,1)
			 Diffuse(1,1,1,1)
			}
			SetTexture[_MainTex]{ConstantColor[_IlluminCol] combine constant* texture}
			SetTexture[_MainTex]{combine previous+texture}
			SetTexture[_MainTex]{ConstantColor[_IlluminCol] combine previous* constant}

		}
	}
}
