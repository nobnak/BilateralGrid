Shader "Custom/Gamma" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Gamma ("Gamma", Float) = 2.2
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
			float _Gamma;

			struct appdata_custom {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct vsout {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			vsout vert(appdata_custom i) {
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv = i.uv;				
				return o;
			}
			
			fixed4 frag(vsout i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				c.rgb = pow(c.rgb, _Gamma);
				return c;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
