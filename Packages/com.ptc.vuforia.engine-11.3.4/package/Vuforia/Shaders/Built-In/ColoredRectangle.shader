//===============================================================================
//Copyright (c) 2022 PTC Inc. All Rights Reserved.
//
//Confidential and Proprietary - Protected under copyright and other laws.
//Vuforia is a trademark of PTC Inc., registered in the United States and other
//countries.
//===============================================================================

Shader "Vuforia/Built-In/ColoredRectangle" 
{
	Properties 
	{
		_Thickness ("Thickness in meters (e.g. 0.001 => 1 mm", Float) = 0.001
		_Color ("Color", Color) = (1,1,1,1)
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"

	struct VertexInput 
	{
		float4 position : POSITION;
		float3 normal : NORMAL;
	};
	
	struct FragmentInput 
	{
		float4 position : POSITION;
		float2 uv : TEXCOORD0;
	};

	uniform float _Thickness;
	uniform float4 _Color;
	
	ENDCG

	SubShader 
	{
		Tags { "Queue" = "Geometry" }
		
		Pass 
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vertexShader
			#pragma fragment fragmentShader

			FragmentInput vertexShader(VertexInput input) 
			{
				FragmentInput output;
				output.position = UnityObjectToClipPos(input.position);
				output.uv = input.position.xy + 0.5f;

				return output;
			}

			float4 fragmentShader(FragmentInput input) : COLOR 
			{
				float3 objectScale = float3(
				    length(unity_ObjectToWorld._m00_m10_m20),
				    length(unity_ObjectToWorld._m01_m11_m21),
				    length(unity_ObjectToWorld._m02_m12_m22)
				);

				float2 scaledThickness = float2(_Thickness, _Thickness) / objectScale.xy;

				if (_Thickness == 0 ||
					(input.uv.x >= scaledThickness.x && input.uv.x <= (1 - scaledThickness.x) &&
					input.uv.y >= scaledThickness.y && input.uv.y <= (1 - scaledThickness.y)))
				{
					discard;
				}
				
				return _Color;
			}
			ENDCG
		}
	}
	
	Fallback "Diffuse"
}
