Shader "Custom/GaussianZ" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RcpTile ("Rcp of tile size", Vector) = (1, 1, 1, 1)
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
			
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _RcpTile;
			
			struct vsout {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			vsout vert(appdata_base i) {
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.texcoord = i.texcoord.xy;
				return o;
			}
			
			float4 frag(vsout i) : COLOR {
				float4 c0 = tex2D(_MainTex, i.texcoord - float2(0.0, 2.0 * _RcpTile.z));
				float4 c1 = tex2D(_MainTex, i.texcoord - float2(0.0, 1.0 * _RcpTile.z));
				float4 c2 = tex2D(_MainTex, i.texcoord);
				float4 c3 = tex2D(_MainTex, i.texcoord + float2(0.0, 1.0 * _RcpTile.z));
				float4 c4 = tex2D(_MainTex, i.texcoord + float2(0.0, 2.0 * _RcpTile.z));
				
				return 0.054488684549643 * c0
					+ 0.244201342003233 * c1
					+ 0.402619946894247 * c2
					+ 0.244201342003233 * c3
					+ 0.054488684549643 * c4;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
