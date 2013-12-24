Shader "Custom/NormalView" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
		
		Pass {
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			
			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert(appdata_base i) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.texcoord = i.texcoord.xy;
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR {
				float3 n = tex2D(_MainTex, i.texcoord).rgb;
				return float4(n * 0.5 + 0.5, 1.0);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
