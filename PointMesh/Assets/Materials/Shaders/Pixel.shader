Shader "Custom/Pixel" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
	
			sampler2D _MainTex;
			float2 _MainTex_TexelSize;

			struct vsout {
				float4 vertex : POSITION;
				float4 color : TEXCOORD0;
			};

			vsout vert(appdata_full i) {
				vsout o;
				o.vertex = i.vertex; //mul(UNITY_MATRIX_MVP, i.vertex);
				o.color = tex2D(_MainTex, i.vertex.xy + 0.5 * _MainTex_TexelSize);
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
