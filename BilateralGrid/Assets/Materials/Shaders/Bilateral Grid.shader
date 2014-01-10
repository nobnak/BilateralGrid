Shader "Custom/Bilateral Grid" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RcpTile ("Rcp of tile size", Vector) = (1, 1, 1, 1)
		_GridSize ("Grid size", Vector) = (1, 1, 1, 1)
		_PSize ("Point Size", Float) = 1
		_Gamma ("Gamma", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		Blend One One ZTest Always ZWrite Off 
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma glsl
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "BilateralCommon.cginc"
	
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _PSize;
			float4 _RcpTile;
			float4 _GridSize;
			int _TexType;
			float _Gamma;
			
			struct appdata_custom {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct vsout {
				float4 vertex : POSITION;
				float psize : PSIZE;
				float4 color : TEXCOORD0;
			};

			vsout vert(appdata_custom i) {
				if (_TexType == 1)
					_MainTex_TexelSize.xy /= _MainTex_TexelSize.zw;
				
				float4 c = tex2Dlod(_MainTex, float4(i.texcoord, 0.0, 0.0));
				c = gamma2linear(c, _Gamma);
				float l = rgb2luminance(c);
				float3 xyzOnGrid = uv2gridInt(i.vertex.xy * _MainTex_TexelSize.xy, l, _GridSize);
				float3 xyzOnTile = xyzOnGrid * _RcpTile;
				float2 xyzOnClip = float2(xyzOnTile.x, xyzOnTile.y + xyzOnTile.z) * 2.0 - 1.0 + _RcpTile.xy;
				
				#if UNITY_UV_STARTS_AT_TOP
				xyzOnClip.y *= -1;
				#endif
				
				vsout o;
				o.vertex = float4(xyzOnClip, 0.0, 1.0);
				o.psize = _PSize;
				o.color = float4(c.rgb, 1.0);
				return o;
			}
			
			float4 frag(vsout i) : COLOR {
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
