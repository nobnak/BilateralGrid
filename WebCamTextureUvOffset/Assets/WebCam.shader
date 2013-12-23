Shader "Custom/WebCam" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;

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
				o.texcoord = i.texcoord.xy;
				return o;
			}
			fixed4 frag(vsout i) : COLOR {
				float4 c = tex2D(_MainTex, i.texcoord);
				return float4(saturate(i.texcoord), 0.0, 1.0);
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
