Shader "Custom/Normal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MaxDepth ("Max depth", Float) = 1
		_Fov ("Field of View", Vector) = (1, 1, 1, 1)
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
			float4 _MainTex_TexelSize;
			float _MaxDepth;
			float4 _Fov;
			
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
			
			float3 uvToEye(float2 uv, float depth) {
				return float3((uv*2.0 - 1.0) * _Fov.xy, -1.0) * depth;
			}
			
			float3 getEyePos(float2 uv) {
				float depth = tex2D(_MainTex, uv).r;
				return uvToEye(uv, depth);
			}
			
			fixed4 frag(v2f i) : COLOR {
				float depth = tex2D(_MainTex, i.texcoord).r;
				if (depth > _MaxDepth) discard;
				
				float3 eyeSpacePos = uvToEye(i.texcoord, depth);
				
				float3 dx = getEyePos(i.texcoord + float2(_MainTex_TexelSize.x, 0.0)) - eyeSpacePos;
				float3 dx2 = eyeSpacePos - getEyePos(i.texcoord - float2(_MainTex_TexelSize.x, 0.0));
				dx = (abs(dx.z) > abs(dx2.z)) ? dx2 : dx;
				
				float3 dy = getEyePos(i.texcoord + float2(0.0, _MainTex_TexelSize.y)) - eyeSpacePos;
				float3 dy2 = eyeSpacePos - getEyePos(i.texcoord - float2(0.0, _MainTex_TexelSize.y));
				dy = (abs(dy.z) > abs(dy2.z)) ? dy2 : dy;
				
				float3 n = normalize(cross(dx, dy));
				
				return float4(n, 1.0);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
