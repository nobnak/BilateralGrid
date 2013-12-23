Shader "Custom/Bilateral Grid" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PSize ("Point Size", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		//Blend One One ZTest Always ZWrite Off 
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma glsl
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "BilateralCommon.cginc"
	
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _PSize;
			int _TextureType;
			
			struct appdata_custom {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct vsout {
				float4 vertex : POSITION;
				float psize : PSIZE;
				float4 color : TEXCOORD0;
			};

			vsout vert(appdata_custom i) {
				if (_TextureType == 1)
					_MainTex_TexelSize.xy /= _MainTex_TexelSize.zw;
				
				float4 c = tex2Dlod(_MainTex, float4(i.texcoord, 0.0, 0.0));
				float2 xy = i.texcoord - 0.5 * _MainTex_TexelSize.xy;
				float2 xyzOnClip = float4(xy * 2.0 - 1.0, 0.0, 1.0);
				
				vsout o;
				//o.vertex = mul(UNITY_MATRIX_MVP, float4(xy * 2.0, 0.0, 1.0));
				o.vertex = float4(2.0 * xy - 1.0, 0.0, 1.0);
				o.psize = _PSize;
				o.color = float4(c.rgb, 1.0);
				return o;
			}
			
			float4 frag(vsout i) : COLOR {
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
