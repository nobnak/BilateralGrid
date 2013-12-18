Shader "Custom/Pixel" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		Blend One One
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

			struct vsout {
				float4 vertex : POSITION;
				float4 color : TEXCOORD0;
			};

			vsout vert(appdata_full i) {
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.color = tex2Dlod(_MainTex, i.texcoord);
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
