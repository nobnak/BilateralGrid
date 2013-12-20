Shader "Custom/Bilateral Grid View" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Brightness ("Brightness", Float) = 0
		_Contrast ("Contrast", Float) = 1
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
			float _Brightness;
			float _Contrast;

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
				return tex2D(_MainTex, i.texcoord);
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
