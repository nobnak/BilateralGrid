Shader "Custom/Pixel" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GridSize ("Grid size", Vector) = (1, 1, 1, 1)
		_RcpGrid ("Reciprocal of grid", Vector) = (1, 1, 1, 1)
		_RcpSigma ("Reciprocal of sigma", Vector) = (1, 1, 1, 1)
		_PSize ("Point Size", Float) = 1
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
			float _PSize;
			float4 _RcpGrid;
			float4 _RcpSigma;
			float4 _GridSize;
			
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
				float3 xyzOnGrid = round(xyzOnImage * _RcpSigma);
				float2 xyzOn01 = xyzOnGrid.xy + float2(0.0, xyzOnGrid.z * _GridSize.y);
				
				vsout o;
				o.vertex = mul(UNITY_MATRIX_MVP, float4(xyzOn01, 0.0, 1.0));
				//o.vertex = float4(xyzOnGrid, 1.0);
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
