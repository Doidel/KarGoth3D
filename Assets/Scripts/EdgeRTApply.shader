Shader "Custom/EdgeRTApply" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_SensitivityX ("SensitivityX", Range(0,5)) = 3.75
		_SensitivityY ("SensitivityY", Range(0,5)) = 0.82
		_SampleDistance ("SampleDistance", Range(0,2)) = 1
		_Falloff ("Falloff", Range(0, 100)) = 10.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows finalcolor:edgecolor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		uniform float4 _MainTex_TexelSize;
		uniform float4 _CameraDepthNormalsTexture_TexelSize;
		sampler2D _CameraDepthNormalsTexture;
		uniform sampler2D _GlobalEdgeTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform half _SensitivityX;
		uniform half _SensitivityY;
		uniform half _SampleDistance;
		uniform half _Falloff;
		uniform half _Draw;

		inline half CheckSame(half2 centerNormal, float centerDepth, half4 theSample)
		{
			// difference in normals
			// do not bother decoding normals - there's no need here
			half2 diff = abs(centerNormal - theSample.xy) * _SensitivityY;
			int isSameNormal = (diff.x + diff.y) * _SensitivityY < 0.1;
			// difference in depth
			float sampleDepth = DecodeFloatRG(theSample.zw);
			float zdiff = abs(centerDepth - sampleDepth);
			// scale the required threshold by the distance
			int isSameDepth = zdiff * _SensitivityX < 0.09 * centerDepth;

			// return:
			// 1 - if normals and depth are similar enough
			// 0 - otherwise

			return isSameNormal * isSameDepth ? 1.0 : 0.0;
		}

		/*inline half QuantizeChange(half2 centerNormal, float centerDepth, half4 theSample)
		{
			half2 diff = abs(centerNormal - theSample.xy) * _SensitivityY;
			float sampleDepth = DecodeFloatRG(theSample.zw);
			float zdiff = abs(centerDepth - sampleDepth);

			return (diff.x + diff.y) * _SensitivityY + zdiff * _SensitivityX * 10.0;
		}*/

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_CBUFFER_END

		void edgecolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

			float sampleSizeX = _CameraDepthNormalsTexture_TexelSize.x; // _CameraDepthNormalsTexture_TexelSize.x;
			float sampleSizeY = _CameraDepthNormalsTexture_TexelSize.y; // _CameraDepthNormalsTexture_TexelSize.y;
			float2 _uv2 = screenUV + float2(-sampleSizeX, -sampleSizeY) * _SampleDistance;
			float2 _uv3 = screenUV + float2(+sampleSizeX, -sampleSizeY) * _SampleDistance;


			half4 center = tex2D(_CameraDepthNormalsTexture, screenUV);
			half4 sample1 = tex2D(_CameraDepthNormalsTexture, _uv2);
			half4 sample2 = tex2D(_CameraDepthNormalsTexture, _uv3);

			// encoded normal
			half2 centerNormal = center.xy;
			// decoded depth
			float centerDepth = DecodeFloatRG(center.zw);

			float d = clamp(centerDepth * _Falloff - 0.05, 0.0, 1.0);
			half4 depth = half4(d, d, d, 1.0);

			half edge = 1.0;
			edge *= CheckSame(centerNormal, centerDepth, sample1);
			edge *= CheckSame(centerNormal, centerDepth, sample2);

			color = edge * color + (1.0 - edge) * depth * color;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			/*float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

			float sampleSizeX = _CameraDepthNormalsTexture_TexelSize.x; // ;
			float sampleSizeY = _CameraDepthNormalsTexture_TexelSize.y; // _CameraDepthNormalsTexture_TexelSize.y;
			float2 _uv2 = screenUV + float2(-sampleSizeX, -sampleSizeY) * _SampleDistance;
			float2 _uv3 = screenUV + float2(+sampleSizeX, -sampleSizeY) * _SampleDistance;


			half4 center = tex2D(_CameraDepthNormalsTexture, screenUV);
			half4 sample1 = tex2D(_CameraDepthNormalsTexture, _uv2);
			half4 sample2 = tex2D(_CameraDepthNormalsTexture, _uv3);

			// encoded normal
			half2 centerNormal = center.xy;
			// decoded depth
			float centerDepth = DecodeFloatRG(center.zw);

			half edge = 1.0;
			edge *= CheckSame(centerNormal, centerDepth, sample1);
			edge *= CheckSame(centerNormal, centerDepth, sample2);

			float d = clamp(centerDepth * _Falloff - 0.05, 0.0, 1.0);
			half4 depth = half4(d, d, d, 1.0);

			o.Albedo = edge * o.Albedo + (1.0 - edge) * depth.xyz * o.Albedo;*/
		}
		ENDCG
	}
	FallBack "Diffuse"
}
