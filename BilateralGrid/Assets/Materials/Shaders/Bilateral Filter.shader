﻿Shader "Custom/Bilateral Filter" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BilateralGridTex ("Bilateral grid", 2D) = "black" {}
		_RcpTile ("Rcp of tile size", Vector) = (1, 1, 1, 1)
		_GridSize ("Grid size", Vector) = (1, 1, 1, 1)
		_Normalize ("Normalize", Range(0.0, 1.0)) = 1
		_Effect ("Effect", Range(0, 1)) = 1
		_Gamma ("Gamma", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "BilateralCommon.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _BilateralGridTex;
			float4 _BilateralGridTex_TexelSize;
			float4 _GridSize;
			float4 _RcpTile;
			float _Normalize;
			int _TexType;
			float _Effect;
			float _Gamma;

			struct appdata_custom {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			struct vsout {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			vsout vert(appdata_custom i) {
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.texcoord = i.texcoord;
				return o;
			}
			
			fixed4 frag(vsout i) : COLOR {
				if (_TexType == 1)
					_MainTex_TexelSize.xy /= _MainTex_TexelSize.zw;
			
				float4 c = tex2D(_MainTex, i.texcoord);
				c = gamma2linear(c, _Gamma);
				float l = rgb2luminance(c);
				float3 xyzOnGrid = uv2gridFloat(i.texcoord - 0.5 * _MainTex_TexelSize.xy, l, _GridSize);
				float t = frac(xyzOnGrid.z);
				xyzOnGrid.z = floor(xyzOnGrid.z);
				float3 tile0 = xyzOnGrid * _RcpTile;
				float3 tile1 = (xyzOnGrid + float3(0, 0, 1)) * _RcpTile;
				float2 uv0 = float2(tile0.x, tile0.y + tile0.z) + 0.5 * _BilateralGridTex_TexelSize.xy;
				float2 uv1 = float2(tile1.x, tile1.y + tile1.z) + 0.5 * _BilateralGridTex_TexelSize.xy;
				float4 c0 = tex2D(_BilateralGridTex, uv0);
				float4 c1 = tex2D(_BilateralGridTex, uv1);
								
				float4 c01 = (1.0 - t) * c0.a * c0 + t * c1.a * c1;
				float4 nc01 = c01 / (c01.a <= 0 ? 1 : c01.a);
				float4 filtered = (1 - _Normalize) * c01 + _Normalize * nc01;
				return (1.0 - _Effect) * c + _Effect * filtered;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
