Shader "Custom/SphereSurf" {
	Properties {
	//	_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(0,2)) = 1.2
		_Falloff("Falloff", Range(2,20)) = 2
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		//ZWrite Off

		LOD 10000

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		//#pragma target 3.0

		struct Input {
			float3 viewDir;
		};

		fixed4 _Color;
		float _Transparency;
		float _Falloff;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = /*tex2D(_MainTex, IN.uv_MainTex) **/ _Color;
			float vDotN = dot(IN.viewDir, o.Normal);
			o.Alpha = saturate(pow(vDotN, _Falloff) * _Transparency);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
