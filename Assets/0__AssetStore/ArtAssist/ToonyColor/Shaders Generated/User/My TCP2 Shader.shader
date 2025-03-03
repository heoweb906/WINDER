// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Toony Colors Pro+Mobile 2
// (c) 2014-2023 Jean Moreno


Shader "Toony Colors Pro 2/User/My TCP2 Shader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0

		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		[NoScaleOffset] _MetallicGlossMap("Metallic", 2D) = "black" {}

		// [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		// [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax("Height Scale", Range (0.005, 0.08)) = 0.02
		_ParallaxMap("Height Map", 2D) = "black" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}


		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}
		_DetailMask("Detail Mask", 2D) = "white" {}
		//[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0

		// Blending state
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0

		//TOONY COLORS PRO 2 ----------------------------------------------------------------
		_HColor("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
		_HighlightMultiplier ("Highlight Multiplier", Range(0,4)) = 1
		_ShadowMultiplier ("Shadow Multiplier", Range(0,4)) = 1
		_DiffTint ("Diffuse Tint", Color) = (0.7,0.8,1,1)

	[Header(Ramp Shading)]
		_RampThreshold("Threshold", Range(0,1)) = 0.5
		_RampSmooth("Main Light Smoothing", Range(0,1)) = 0.2
		_RampSmoothAdd("Other Lights Smoothing", Range(0,1)) = 0.75

	[Header(Threshold Texture)]
		[NoScaleOffset]
		_ThresholdTex ("Texture (Alpha)", 2D) = "gray" {}
		_ThresholdStrength ("Strength", Range(0,1)) = 1
		_ThresholdScale ("Scale", Float) = 4

	[Header(Sketch)]
		//SKETCH
		_SketchTex ("Sketch (Alpha)", 2D) = "white" {}
		_SketchSpeed ("Sketch Anim Speed", Range(1.1, 10)) = 6
		_SketchStrength ("Sketch Strength", Range(0,1)) = 1

	[Header(HSV Controls)]
		_HSV_H ("Hue", Range(-360,360)) = 0
		_HSV_S ("Saturation", Range(-1,1)) = 0
		_HSV_V ("Value", Range(-1,1)) = 0

	[Header(Subsurface Scattering)]
		_SSDistortion ("Distortion", Range(0,2)) = 0.2
		_SSPower ("Power", Range(0.1,16)) = 3.0
		_SSScale ("Scale", Float) = 1.0
		_SSColor ("Color (RGB)", Color) = (0.5,0.5,0.5,1)
		_SSAmbColor ("Ambient Color (RGB)", Color) = (0.5,0.5,0.5,1)

	[Header(Silhouette)]
		_SilhouetteColor ("Color (RGB) Opacity (A)", Color) = (0,0,0,0.5)
	[TCP2Separator]

	[Header(Outline)]
		//OUTLINE
		[HDR] _OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
		_Outline ("Outline Width", Float) = 1

		//Outline Textured
		[Toggle(TCP2_OUTLINE_TEXTURED)] _EnableTexturedOutline ("Color from Texture", Float) = 0
		[TCP2KeywordFilter(TCP2_OUTLINE_TEXTURED)] _TexLod ("Texture LOD", Range(0,10)) = 5

		//Constant-size outline
		[Toggle(TCP2_OUTLINE_CONST_SIZE)] _EnableConstSizeOutline ("Constant Size Outline", Float) = 0

		//ZSmooth
		[Toggle(TCP2_ZSMOOTH_ON)] _EnableZSmooth ("Correct Z Artefacts", Float) = 0
		//Z Correction & Offset
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _ZSmooth ("Z Correction", Range(-3.0,3.0)) = -0.5
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset1 ("Z Offset 1", Float) = 0
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset2 ("Z Offset 2", Float) = 0

		//This property will be ignored and will draw the custom normals GUI instead
		[TCP2OutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0

		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("__unused__", Float) = 0
	}

	SubShader
	{
		//Make sure that the objects are rendered later to avoid sorting issues with the transparent silhouette
		Tags { "Queue"="Geometry+10" }
		
		//Silhouette Pass
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Greater
			ZWrite Off			

			Stencil
			{
				Ref 1.0
				Comp NotEqual
				Pass Replace
				ReadMask 1.0
				WriteMask 1.0
			}
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"

				fixed4 _SilhouetteColor;

				struct appdata_sil
				{
					float4 vertex : POSITION;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f_sil
				{
					float4 vertex : SV_POSITION;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f_sil vert (appdata_sil v)
				{
					v2f_sil o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				fixed4 frag (v2f_sil i) : COLOR
				{
					return _SilhouetteColor;
				}
			ENDCG
		}

		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]
		//Outline
		Tags { "Queue"="Transparent" }
		UsePass "Hidden/Toony Colors Pro 2/Outline Only Behind/OUTLINE"
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }

		CGPROGRAM

		#pragma surface surf StandardTCP2 vertex:vert keepalpha exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
		#pragma shader_feature _EMISSION

		//================================================================================================================================
		// STRUCTS

		struct Input
		{
			float2 uv_MainTex;
			#define uv_TexturedThreshold uv_MainTex
			float2 uv_DetailAlbedoMap;
			#define uv_Detail uv_DetailAlbedoMap
			float3 viewDir;
			half2 sketchUV;
		};

		//================================================================================================================================
		// VERTEX FUNCTION

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 tangent : TANGENT;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			float4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
	
		//Vertex function
		void vert (inout appdata_tcp2 v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			
			float4 pos = UnityObjectToClipPos(v.vertex);
			float4 screenCoords = ComputeScreenPos(pos);
			
			//Sketch
			float2 screenUV = screenCoords.xy / screenCoords.w;
			float screenRatio = _ScreenParams.y / _ScreenParams.x;
			screenUV.y *= screenRatio;
			o.sketchUV.xy = screenUV;
			o.sketchUV.xy = TRANSFORM_TEX(o.sketchUV.xy, _SketchTex);
			float2 random = round(float2(_Time.z, -_Time.z) * _SketchSpeed) / _SketchSpeed;
			o.sketchUV.xy += frac(random.xy);
		}

		//================================================================================================================================
		// LIGHTING FUNCTION

		inline half4 LightingStandardTCP2(SurfaceOutputStandardTCP2 s, half3 viewDir, UnityGI gi)
		{
			s.Normal = normalize(s.Normal);

			half oneMinusReflectivity;
			half3 specColor;
			s.Albedo = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, /*out*/ specColor, /*out*/ oneMinusReflectivity);

			// shader relies on pre-multiply alpha-blend (_SrcBlend = One, _DstBlend = OneMinusSrcAlpha)
			// this is necessary to handle transparency in physically correct way - only diffuse component gets affected by alpha
			half outputAlpha;
			s.Albedo = PreMultiplyAlpha(s.Albedo, s.Alpha, oneMinusReflectivity, /*out*/ outputAlpha);

		#if defined(UNITY_PASS_FORWARDBASE)
			fixed atten = s.atten;
		#else
			fixed atten = 1;
		#endif

			half4 c = TCP2_BRDF_PBS(s.Albedo, specColor, oneMinusReflectivity, s.Smoothness, s.Normal, viewDir, gi.light, gi.indirect, /* TCP2 */ atten, s

				,s.texThresholdTexcoords
				,s.sketchUV
				);
			c.a = outputAlpha;
			return c;
		}

		inline void LightingStandardTCP2_GI(inout SurfaceOutputStandardTCP2 s, UnityGIInput data, inout UnityGI gi)
		{
			Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(s.Smoothness, data.worldViewDir, s.Normal, lerp(unity_ColorSpaceDielectricSpec.rgb, s.Albedo, s.Metallic));
			gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);

			s.atten = data.atten;				//transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//================================================================================================================================
		// SURFACE FUNCTION

		void surf (Input IN, inout SurfaceOutputStandardTCP2 o)
		{
			//Parallax Offset
			fixed height = tex2D(_ParallaxMap, IN.uv_MainTex).a;
			float2 offset = ParallaxOffset(height, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;

			fixed4 mainTex = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = mainTex.rgb;
			o.Alpha = mainTex.a;

		#if _ALPHATEST_ON
			clip(o.Alpha - _Cutoff);
		#endif

			
			//Detail texture
			half detailMask = tex2D(_DetailMask, IN.uv_MainTex.xy).a;
			half3 detailAlbedo = tex2D (_DetailAlbedoMap, IN.uv_Detail.xy).rgb;
			o.Albedo *= LerpWhiteTo (detailAlbedo * unity_ColorSpaceDouble.rgb, detailMask);

			//Metallic Workflow
			fixed4 metalGlossMap = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			half2 metallicGloss = MetallicGloss(mainTex.a, metalGlossMap);
			half metallic = metallicGloss.x;
			half smoothness = metallicGloss.y;
			o.Metallic = metallic;
			o.Smoothness = smoothness;


			o.texThresholdTexcoords = IN.uv_TexturedThreshold;

			//Albedo HSV
			float3 diffHSV = rgb2hsv(o.Albedo.rgb);
			diffHSV += float3(_HSV_H/360,_HSV_S,_HSV_V);
			o.Albedo.rgb = hsv2rgb(diffHSV);

			//Bump/normal map
			half4 normalMap = tex2D(_BumpMap, IN.uv_MainTex.xy);
			o.Normal = UnpackScaleNormal(normalMap, _BumpScale);
			half3 detailNormalTangent = UnpackScaleNormal(tex2D(_DetailNormalMap, IN.uv_Detail.xy), _DetailNormalMapScale);
			o.Normal = lerp(o.Normal, BlendNormals(o.Normal, detailNormalTangent), detailMask);

			//Emission
		#if _EMISSION
			o.Emission += tex2D(_EmissionMap, IN.uv_MainTex.xy) * _EmissionColor.rgb;
		#endif

			//Occlusion
			o.Occlusion = LerpOneTo(tex2D(_OcclusionMap, IN.uv_MainTex.xy).g, _OcclusionStrength);
			o.sketchUV = IN.sketchUV;
		#ifdef _ALPHABLEND_ON
			o.Albedo *= o.Alpha;
		#endif
		}
		ENDCG


		//Outline - Depth Pass Only
		Pass
		{
			Name "OUTLINE_DEPTH"

			Cull Off
			Offset [_Offset1],[_Offset2]
			Tags { "LightMode"="ForwardBase" }

			//Write to Depth Buffer only
			ColorMask 0
			ZWrite On

			CGPROGRAM

			#include "UnityCG.cginc"
			#include "../Shaders/Include/TCP2_Outline_Include.cginc"

			#pragma vertex TCP2_Outline_Vert
			#pragma fragment TCP2_Outline_Frag

			#pragma multi_compile TCP2_NONE TCP2_ZSMOOTH_ON
			#pragma multi_compile TCP2_NONE TCP2_OUTLINE_CONST_SIZE
			#pragma multi_compile TCP2_NONE TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV2_AS_NORMALS
			//#pragma multi_compile TCP2_NONE TCP2_OUTLINE_TEXTURED		//Not needed for depth
			#pragma multi_compile_instancing

			#pragma multi_compile EXCLUDE_TCP2_MAIN_PASS

			#pragma target 3.0

			ENDCG
		}
	}

	CGINCLUDE

	#if !defined(EXCLUDE_TCP2_MAIN_PASS)
		#include "Lighting.cginc"

		//================================================================================================================================
		// STRUCT

		struct SurfaceOutputStandardTCP2
		{
			fixed3 Albedo;      // base (diffuse or specular) color
			fixed3 Normal;      // tangent space normal, if written
			half3 Emission;

			half Metallic;      // 0=non-metal, 1=metal

			//Smoothness is the user facing name, it should be perceptual smoothness but user should not have to deal with it.
			// Everywhere in the code you meet smoothness it is perceptual smoothness
			half Smoothness;    // 0=rough, 1=smooth
			half Occlusion;     // occlusion (default 1)
			fixed Alpha;        // alpha for transparencies

			fixed atten;
			float2 texThresholdTexcoords;
			half2 sketchUV;
		};

		//================================================================================================================================
		// VARIABLES

		sampler2D _MainTex;
		fixed4 _Color;
		half _Cutoff;
		half _Glossiness;
		half _GlossMapScale;
		half _Metallic;
		sampler2D _MetallicGlossMap;
		half _BumpScale;
		sampler2D _BumpMap;
		sampler2D _ParallaxMap;
		half _Parallax;
		sampler2D _DetailAlbedoMap;
		half _DetailNormalMapScale;
		sampler2D _DetailNormalMap;
		sampler2D _DetailMask;
		half4 _EmissionColor;
		sampler2D _EmissionMap;
		half _OcclusionStrength;
		sampler2D _OcclusionMap;

		//-------------------------------------------------------------------------------------
		//TCP2 Params

		fixed4 _HColor;
		fixed4 _SColor;
		fixed _HighlightMultiplier;
		fixed _ShadowMultiplier;
		sampler2D _Ramp;
		fixed _RampThreshold;
		fixed _RampSmooth;
		fixed _RampSmoothAdd;
		float _HSV_H;
		float _HSV_S;
		float _HSV_V;
		half _SSDistortion;
		half _SSPower;
		half _SSScale;
		fixed4 _SSColor;
		fixed4 _SSAmbColor;
			fixed4 _DiffTint;
		sampler2D _ThresholdTex;
		fixed _ThresholdScale;
		fixed _ThresholdStrength;
		sampler2D _SketchTex;
		float4 _SketchTex_ST;
		fixed _SketchStrength;
		fixed _SketchSpeed;

		//================================================================================================================================
		// LIGHTING / BRDF

		//-------------------------------------------------------------------------------------
		// TCP2 Tools

		inline half WrapRampNL(half nl, fixed threshold, fixed smoothness)
		{
			nl = saturate(nl);
			nl = smoothstep(threshold - smoothness*0.5, threshold + smoothness*0.5, nl);
			return nl;
		}

		// HSV HELPERS
		// source: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
		float3 rgb2hsv(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		float3 hsv2rgb(float3 c)
		{
			c.g = max(c.g, 0.0); //make sure that saturation value is positive
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}

		//-------------------------------------------------------------------------------------
		// Standard Shader inputs


		half2 MetallicGloss(float mainTexAlpha, fixed4 metalGlossMap)
		{
			half2 mg;
			mg = metalGlossMap.ra;
			mg.g *= _GlossMapScale;
			return mg;
		}

		//-------------------------------------------------------------------------------------

		// Note: BRDF entry points use oneMinusRoughness (aka "smoothness") and oneMinusReflectivity for optimization
		// purposes, mostly for DX9 SM2.0 level. Most of the math is being done on these (1-x) values, and that saves
		// a few precious ALU slots.

		// Main Physically Based BRDF
		// Derived from Disney work and based on Torrance-Sparrow micro-facet model
		//
		//   BRDF = kD / pi + kS * (D * V * F) / 4
		//   I = BRDF * NdotL
		//
		// * NDF (depending on UNITY_BRDF_GGX):
		//  a) Normalized BlinnPhong
		//  b) GGX
		// * Smith for Visiblity term
		// * Schlick approximation for Fresnel
		half4 TCP2_BRDF_PBS(half3 diffColor, half3 specColor, half oneMinusReflectivity, half smoothness, half3 normal, half3 viewDir, UnityLight light, UnityIndirect gi,
			/* TCP2 */ half atten, SurfaceOutputStandardTCP2 s
			,half2 texThresholdTexcoords
			,half2 sketchUV
			)
		{
			half perceptualRoughness = SmoothnessToPerceptualRoughness (smoothness);
			half3 halfDir = Unity_SafeNormalize (light.dir + viewDir);

			// NdotV should not be negative for visible pixels, but it can happen due to perspective projection and normal mapping
			// In this case normal should be modified to become valid (i.e facing camera) and not cause weird artifacts.
			// but this operation adds few ALU and users may not want it. Alternative is to simply take the abs of NdotV (less correct but works too).
			// Following define allow to control this. Set it to 0 if ALU is critical on your platform.
			// This correction is interesting for GGX with SmithJoint visibility function because artifacts are more visible in this case due to highlight edge of rough surface
			// Edit: Disable this code by default for now as it is not compatible with two sided lighting used in SpeedTree.
			#define TCP2_HANDLE_CORRECTLY_NEGATIVE_NDOTV 0 

	#if TCP2_HANDLE_CORRECTLY_NEGATIVE_NDOTV
			// The amount we shift the normal toward the view vector is defined by the dot product.
			half shiftAmount = dot(normal, viewDir);
			normal = shiftAmount < 0.0f ? normal + viewDir * (-shiftAmount + 1e-5f) : normal;
			// A re-normalization should be applied here but as the shift is small we don't do it to save ALU.
			//normal = normalize(normal);

			half nv = saturate(dot(normal, viewDir)); // TODO: this saturate should no be necessary here
	#else
			half nv = abs(dot(normal, viewDir));	// This abs allow to limit artifact
	#endif

			half nl = dot(normal, light.dir);

			half3 diffTint = saturate(_DiffTint.rgb + nl);

		#if !defined(UNITY_PASS_FORWARDADD)
	
			half2 thresholdUv = texThresholdTexcoords.xy * _ThresholdScale;
			half texThreshold = tex2D(_ThresholdTex, thresholdUv).a - 0.5;
			nl += texThreshold * _ThresholdStrength;
		#endif

		#if defined(UNITY_PASS_FORWARDADD)
			#define RAMP_SMOOTH _RampSmoothAdd
		#else
			#define RAMP_SMOOTH _RampSmooth
		#endif

			//TCP2 Ramp N.L
			nl = WrapRampNL(nl, _RampThreshold, RAMP_SMOOTH);

			half nh = saturate(dot(normal, halfDir));

			half lv = saturate(dot(light.dir, viewDir));
			half lh = saturate(dot(light.dir, halfDir));

			// Diffuse term
			half diffuseTerm = DisneyDiffuse(nv, nl, lh, perceptualRoughness) * nl;

			// Specular term
			// HACK: theoretically we should divide diffuseTerm by Pi and not multiply specularTerm!
			// BUT 1) that will make shader look significantly darker than Legacy ones
			// and 2) on engine side "Non-important" lights have to be divided by Pi too in cases when they are injected into ambient SH
			half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
	#if UNITY_BRDF_GGX
			half V = SmithJointGGXVisibilityTerm (nl, nv, roughness);
			half D = GGXTerm (nh, roughness);
	#else
			// Legacy
			half V = SmithBeckmannVisibilityTerm (nl, nv, roughness);
			half D = NDFBlinnPhongNormalizedTerm (nh, PerceptualRoughnessToSpecPower(perceptualRoughness));
	#endif
			half specularTerm = 0.0;

			// surfaceReduction = Int D(NdotH) * NdotH * Id(NdotL>0) dH = 1/(roughness^2+1)
			half surfaceReduction;
	#ifdef UNITY_COLORSPACE_GAMMA
			surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;		// 1-0.28*x^3 as approximation for (1/(x^4+1))^(1/2.2) on the domain [0;1]
	#else
			surfaceReduction = 1.0 / (roughness*roughness + 1.0);			// fade \in [0.5;1]
	#endif

			// To provide true Lambert lighting, we need to be able to kill specular completely.
			specularTerm *= any(specColor) ? 1.0 : 0.0;

	//TCP2 Colored Highlight/Shadows
			_SColor = lerp(_HColor, _SColor, _SColor.a * _ShadowMultiplier);	//Shadows intensity through alpha
			_HColor.rgb *= _HighlightMultiplier;

	//light attenuation already included in light.color for point lights
	#if !defined(UNITY_PASS_FORWARDADD)
			diffuseTerm *= atten;
	#endif
			half3 diffuseTermRGB = lerp(_SColor.rgb, _HColor.rgb, diffuseTerm);
			diffuseTermRGB *= diffTint;
			half3 diffuseTCP2 = diffColor * (gi.diffuse + light.color * diffuseTermRGB);
			//original: diffColor * (gi.diffuse + light.color * diffuseTerm)

	//light attenuation already included in light.color for point lights
	#if !defined(UNITY_PASS_FORWARDADD)
			//TCP2: atten contribution to specular since it was removed from light calculation
			specularTerm *= atten;
	#endif

			half grazingTerm = saturate(smoothness + (1-oneMinusReflectivity));
			half3 color =	diffuseTCP2
							+ specularTerm * light.color
							+ surfaceReduction * gi.specular
							* specColor;

		#if defined(UNITY_PASS_FORWARDADD)
			//Subsurface Scattering
			half3 ssLight = light.dir + normal * _SSDistortion;
			half ssDot = pow(saturate(dot(viewDir, -ssLight)), _SSPower) * _SSScale;
		  #if defined(UNITY_PASS_FORWARDADD)
			half ssAtten = atten * 2;
		  #else
			half ssAtten = 1;
		  #endif
			half3 ssColor = ssAtten * ((ssDot * _SSColor.rgb) + _SSAmbColor.rgb);
			ssColor.rgb *= light.color.rgb;
			color.rgb *= diffColor * ssColor;
		#endif
			//Sketch

			fixed sketch = tex2D(_SketchTex, sketchUV).a;
			sketch = lerp(sketch, 1, nl * atten);	//Regular sketch overlay
			color.rgb = lerp(color.rgb, sketch * color.rgb, _SketchStrength);
			return half4(color, 1);
		}

		//================================================================================================================================

	#endif
	ENDCG

	FallBack "VertexLit"
	CustomEditor "TCP2_MaterialInspector_SurfacePBS_SG"
}

