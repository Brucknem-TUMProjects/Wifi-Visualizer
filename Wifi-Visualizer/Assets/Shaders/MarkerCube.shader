Shader "Custom/MarkerCube" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha:fade

		struct Input {
			float2 uv_MainTex;
		};

		half _Transparency;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color;
			o.Emission = _Color;
			o.Alpha = _Transparency;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
