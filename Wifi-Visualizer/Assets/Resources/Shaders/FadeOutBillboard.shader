Shader "Custom/FadeOutBillboard" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Position("Position", Vector) = (0,0,0,1)
		_Transparency("Transparency", Range(0,5)) = 2
		_Size("Size", Range(0,5)) = 2
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

		Cull Front

			CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha:fade


		struct Input {
			float3 worldPos;
		};

		float4 _Color;
		float4 _Position;
		float _Transparency;
		float _Size;
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _Color.rgb;

			float3 diff = (IN.worldPos - _Position.xyz);
			float dist = _Size - sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);

			o.Alpha = dist * _Transparency;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
