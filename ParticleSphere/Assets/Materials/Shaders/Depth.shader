Shader "Custom/Depth" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius ("Radius", Float) = 1
		_Color ("Color", Color) = (1, 1 ,1, 1)
		_LightDir ("Light dir", Vector) = (1, 0, 0, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float _Radius;
			float4 _Color;
			float4 _LightDir;
			
			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 eyeSpaceCenter : TEXCOORD1;
			};
			
			struct psout {
				float4 color : COLOR;
				float depth : DEPTH;
			};
			
			v2f vert(appdata_base i) {
				float3 eyeSpaceCenter = mul(UNITY_MATRIX_MV, i.vertex).xyz;
				float2 eyeSpaceOffset = (i.texcoord.xy * 2.0 - 1.0) * _Radius;
				float3 eyeSpacePos = eyeSpaceCenter + float3(eyeSpaceOffset, 0.0);
				
				v2f o;
				o.vertex = mul(UNITY_MATRIX_P, float4(eyeSpacePos, 1.0));
				o.texcoord = i.texcoord.xy;
				o.eyeSpaceCenter = eyeSpaceCenter;
				return o;
			}
			
			psout frag(v2f i) {
				float3 eyeSpaceNormal;
				eyeSpaceNormal.xy = i.texcoord * 2.0 - 1.0;
				float r2 = dot(eyeSpaceNormal.xy, eyeSpaceNormal.xy);
				if (r2 > 1.0) discard;
				eyeSpaceNormal.z = sqrt(1.0 - r2);
				
				float4 eyeSpacePos = float4(i.eyeSpaceCenter + eyeSpaceNormal * _Radius, 1.0);
				float4 clipSpacePos = mul(UNITY_MATRIX_P, eyeSpacePos);
				float depth = clipSpacePos.z / clipSpacePos.w;
				
				psout o;
#ifdef DEBUG_DIFFUSE
				float3 eyeSpaceLightDir = normalize(_LightDir);
				float diffuse = max(0.0, dot(eyeSpaceNormal, eyeSpaceLightDir));
				o.color = diffuse;
#else
				o.color = -eyeSpacePos.z;
#endif
				o.depth = depth;
				return o;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
