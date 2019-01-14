﻿Shader "Unlit/Tetrahedron"
{
	Properties
	{
	}

	SubShader
	{
		Tags { "Queue" = "Geometry"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = i.color;
				return col;
			}
			ENDCG
		}
	}
}