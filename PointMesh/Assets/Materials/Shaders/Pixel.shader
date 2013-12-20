Shader "Custom/Pixel" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RcpTile ("Rcp of tile size", Vector) = (1, 1, 1, 1)
		_RcpSigma ("Rcp of sigma", Vector) = (1, 1, 1, 1)
		_PSize ("Point Size", Float) = 1
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
	
			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			float _PSize;
			float4 _RcpTile;
			float4 _RcpSigma;
			
			struct appdata_custom {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vsout {
				float4 vertex : POSITION;
				float psize : PSIZE;
				float4 color : TEXCOORD0;
			};

			vsout vert(appdata_custom i) {
				float4 c = tex2Dlod(_MainTex, i.texcoord);
				float3 xyzOnImage = float3(i.vertex.xy, c);
				float3 xyzOnGrid = 1 + floor(xyzOnImage * _RcpSigma);
				float3 xyzOn01 = xyzOnGrid * _RcpTile;
				float2 xyzOnClip = float2(xyzOn01.x, xyzOn01.y + xyzOn01.z) * 2.0 - 1.0 + _RcpTile.xy;
				
				vsout o;
				//o.vertex = mul(UNITY_MATRIX_MVP, float4(xyzOn01.x, xyzOn01.y + xyzOn01.z, 0.0, 1.0));
				o.vertex = float4(xyzOnClip, 0.0, 1.0);
				o.psize = _PSize;
				o.color = c;
				return o;
			}
			
			fixed4 frag(vsout i) : COLOR {
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
