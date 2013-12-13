Shader "Custom/Depth" {

	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float depth : TEXCOORD1;
			};

			v2f vert(appdata_base v) {
				float3 eyeSpacePos = mul(UNITY_MATRIX_MV, v.vertex);
			
				v2f o;
				o.vertex = mul(UNITY_MATRIX_P, float4(eyeSpacePos, 1.0));
				o.texcoord = v.texcoord.xy;
				o.depth = -eyeSpacePos.z;
				return o;
			}

			fixed4 frag(v2f p) : COLOR {
				return p.depth;
			}

			ENDCG
		}
	}
}
