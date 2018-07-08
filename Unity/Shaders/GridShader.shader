Shader "Unlit/GridShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HorisontalCoef("Horisontal coef", float) = 1
		_VerticalCoef("Vertical coef", float) = 1
		_sideAlphaCoef("Side alpha coef", float) = 0.1
	}
	SubShader
	{
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _HorisontalCoef;
			float _VerticalCoef;
			float _sideAlphaCoef;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    float2 coords = float2(i.uv.x * _HorisontalCoef, i.uv.y * _VerticalCoef);
			    
			    float alpha = 1;
			    
			    if (i.uv.x < _sideAlphaCoef / _HorisontalCoef)
			        alpha *= _HorisontalCoef * i.uv.x / _sideAlphaCoef;
			        
			    if (1 - i.uv.x < _sideAlphaCoef / _HorisontalCoef)
			        alpha *= _HorisontalCoef * (1 - i.uv.x) / _sideAlphaCoef;
			        
			        
			    if (i.uv.y < _sideAlphaCoef / _VerticalCoef)
			        alpha *= _VerticalCoef * i.uv.y / _sideAlphaCoef;
			        
			    if (1 - i.uv.y < _sideAlphaCoef / _VerticalCoef)
			        alpha *= _VerticalCoef * (1 - i.uv.y) / _sideAlphaCoef;
			    
				float4 color = tex2D(_MainTex, coords);
				
				color.a *= alpha;
				
				return color;
			}
			ENDCG
		}
	}
}
