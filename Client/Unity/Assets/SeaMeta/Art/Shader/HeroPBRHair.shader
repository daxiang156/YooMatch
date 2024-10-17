Shader "SG/Hero/PBRHair"
{
    Properties
    {
[Header(Main Setting)] _ElementViewEleID("Element ID", Float) = 0
[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
[MainTexture][NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
[NoScaleOffset]_BumpMap("Main Normal Map", 2D) = "bump" { }
[NoScaleOffset]_DetailMask("Detail Mask", 2D) = "black" {}
_Metallic("Metallic", Float) = 0
_RoughnessScale("RoughnessScale", Float) = 0.86
_OcclusionStrength("OcclusionStrength", Float) = 0.50
_FresnelIntensity("FresnelIntensity", Float) = 1
[HDR] _IndirectColor("IndirectColor", Color) = (1, 1, 1, 1.00)
[HDR] _DirectColor("DirectColor", Color) = (1.0, 1.0, 1.0, 1.00)

[Header(Anisotropy)]_Anisotropy_base("Anisotropy_base", Range(-1 , 1)) = -1
_R1_Color("R1_Color", Color) = (0.9632353,0.9038391,0.8499135,0)
[HDR]_R2_Color("R2_Color", Color) = (0.9632353,0.9038391,0.8499135,0)
_NiGuangClo("NiGuangClo", Color) = (1,1,1,0)
_NiGuang("NiGuang", Range(0 , 1)) = 0.5234948
_Anisotropy_R1("Anisotropy_R1", Range(1 , 40)) = 40
_Anisotropy_R2("Anisotropy_R2", Range(1 , 100)) = 50
_RapMap("RapMap", 2D) = "white" {}
_strength("strength", Range(0 , 1)) = 1
_MaskPower("MaskPower", Range(0 , 5)) = 0
[Header(DecalsTex)]
_DecalsTex("DecalsTex", 2D) = "black" {}

[Header(GAMEPLAY)]
[Toggle(_GAMEPLAY)] _GAMEPLAY("GAMEPLAY", Float) = 0
_Hit("Hit", Float) = 0
_Disslove("Disslove", Range(0 , 1)) = 0
_Occluded("Occluded", Range(0 , 1)) = 0
[ASEEnd]_OccludedColor("OccludedColor", Color) = (1,1,1,0)
    }
    SubShader
    {
        
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Lit"  }

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

        TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex);
        TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DecalsTex);        SAMPLER(sampler_DecalsTex);

        CBUFFER_START(UnityPerMaterial)

        half4 _BaseColor;// = { 0.91509, 0.91509, 0.91509, 1.00 };
        half _Metallic;// = -0.21;
        half _RoughnessScale;// = 0.86;
        half _OcclusionStrength;// = 0.50;
        half _FresnelIntensity;// = 0.00;
        half4 _IndirectColor;// = { 1.43396, 1.43396, 1.43396, 1.00 };
        half4 _DirectColor;// = { 0.92215, 0.92215, 0.92215, 1.00 };

        half _SSSIntensity;

        half4 _AnisoControl;
        half4 _DecalsTex_ST;

        half4 _NiGuangClo;
        float4 _RapMap_ST;
        half4 _R1_Color;
        half4 _R2_Color;
        float4 _MaskMap_ST;
        half _NiGuang;
        half _strength;
        half _Anisotropy_base;
        half _Anisotropy_R1;
        half _Anisotropy_R2;
        half _MaskPower;
        half _Smoothness;

        float _Disslove;
        half _Hit;
        float _Occluded;
        half4 _OccludedColor;
        CBUFFER_END

        ENDHLSL

        Pass
        {
             Name "ForwardLit"

            Tags { "LightMode" = "UniversalForward" }

            Cull Back
            ZTest LEqual
            ZWrite On

            Stencil {
                Ref 10
                Comp always
                Pass replace
            }

            HLSLPROGRAM

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _UNIQUE_SHADOW

            //#pragma shader_feature_local_fragment _SKIN_SSS
 			#pragma multi_compile _ _CONST_POINT_LIT
            #pragma shader_feature_local_fragment _GAMEPLAY
            //#pragma shader_feature_local_fragment _ANIS_SPECULAR

            #pragma vertex vert
            #pragma fragment frag

#if _GAMEPLAY
            #define _ADDITIONAL_LIGHTS  0
            #define TONEMAP            0
            #define DISSLOVE 1
            #define NEEDHIT           1
#else
            #define _ADDITIONAL_LIGHTS 1
            #define TONEMAP           1
            #define DISSLOVE          0
            #define NEEDHIT           0
#endif

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION0;

            float3 L2Term : TEXCOORD0;
            float4 normalWS : TEXCOORD1;
            float4 tangentWS : TEXCOORD2;
            float4 bitangentWS : TEXCOORD3;
            float4 shadowCoord : TEXCOORD4;
            float4 texcoord : TEXCOORD5;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

#if  _CONST_POINT_LIT
        static const real4 Const_SHAr = { 0.00, 0.13982, 0.00, 0.1702 };
        static const real4 Const_SHAg = { 0.00, 0.09701, 0.00, 0.13578 };
        static const real4 Const_SHAb = { 0.00, 0.09701, 0.00, 0.13578 };
        static const real4 Const_SHBr = { 0.00, 0.00, -0.02826, 0.00 };
        static const real4 Const_SHBg = { 0.00, 0.00, -0.0168, 0.00 };
        static const real4 Const_SHBb = { 0.00, 0.00, -0.0168, 0.00 };
        static const real4 Const_SHC = { -0.02826, -0.0168, -0.0168, 1.00 };

#endif
        Varyings vert(Attributes input)
        {
            Varyings output = (Varyings)0;

            UNITY_SETUP_INSTANCE_ID(i);
            UNITY_TRANSFER_INSTANCE_ID(i, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

            // normalWS and tangentWS already normalize.
            // this is required to avoid skewing the direction during interpolation
            // also required for per-vertex lighting and SH evaluation
            VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
            output.positionCS = vertexInput.positionCS;

#if  _CONST_POINT_LIT && !_GAMEPLAY
            output.L2Term.xyz = SHEvalLinearL2(normalInput.normalWS, Const_SHBr, Const_SHBg, Const_SHBb, Const_SHC);
#else
            output.L2Term.xyz = SHEvalLinearL2(normalInput.normalWS, unity_SHBr, unity_SHBg, unity_SHBb, unity_SHC);
#endif


            output.normalWS.xyz = normalInput.normalWS.xyz;
            output.tangentWS.xyz = normalInput.tangentWS.xyz;
            output.bitangentWS.xyz = normalInput.bitangentWS.xyz;

            output.normalWS.w = vertexInput.positionWS.x;
            output.tangentWS.w = vertexInput.positionWS.y;
            output.bitangentWS.w = vertexInput.positionWS.z;


            output.shadowCoord = GetShadowCoord(vertexInput);
            output.texcoord.xy = input.texcoord.xy;
            output.texcoord.zw = input.texcoord1.xy;
            return output;
        }


         half3 EnvironmentBRDFSpecular(half roughness2, half3 specular, half grazingTerm, half fresnelTerm)
        {
            float surfaceReduction = 1.0 / (roughness2 + 1.0);
            return surfaceReduction * lerp(specular, grazingTerm, fresnelTerm);
        }

        // Computes the scalar specular term for Minimalist CookTorrance BRDF
// NOTE: needs to be multiplied with reflectance f0, i.e. specular color to complete
        half DirectBRDFSpecular(half normalizationTerm, half roughness2MinusOne, half roughness2, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
        {
            float3 halfDir = SafeNormalize(float3(lightDirectionWS)+float3(viewDirectionWS));

            float NoH = saturate(dot(normalWS, halfDir));
            half LoH = saturate(dot(lightDirectionWS, halfDir));

            // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
            // BRDFspec = (D * V * F) / 4.0
            // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
            // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
            // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
            // https://community.arm.com/events/1155

            // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
            // We further optimize a few light invariant terms
            // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
            float d = NoH * NoH * roughness2MinusOne + 1.00001f;

            half LoH2 = LoH * LoH;
            half specularTerm = roughness2 / ((d * d) * max(0.1h, LoH2) * normalizationTerm);

            // On platforms where half actually means something, the denominator has a risk of overflow
            // clamp below was added specifically to "fix" that, but dx compiler (we convert bytecode to metal/gles)
            // sees that specularTerm have only non-negative terms, so it skips max(0,..) in clamp (leaving only min(100,...))
        //#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
            specularTerm = specularTerm - HALF_MIN;
            specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
        //#endif

            return specularTerm;
        }

        TEXTURE2D(_SssLut);        SAMPLER(sampler_SssLut);

        TEXTURE2D(_RapMap);         SAMPLER(sampler_RapMap);

        half3 GetSkinColorSp(half ndl_no_clamp, half sss_curvature)
        {
            half3 skin = SAMPLE_TEXTURE2D(_SssLut, sampler_SssLut, half2(clamp(ndl_no_clamp * 0.5 + 0.5, 0.0, 1.0), sss_curvature)).xyz;
            skin = skin * skin;
            return skin * _SSSIntensity;
        }

        half3 LightingPhysicallyBased(half3 brdfdiffuse, half3 brdfspecular, half normalizationTerm, half roughness2MinusOne, half roughness2,
            half3 lightColor, half3 lightDirectionWS, half lightAttenuation,
            half3 normalWS, half3 viewDirectionWS, half3 vertexNormalDir, half skin)
        {
            half NdotL = saturate(dot(normalWS, lightDirectionWS));
            lightAttenuation = 0.7 * lightAttenuation + 0.30;
#if _SKIN_SSS
            if (skin > 0.01)
            {
                half3 attenColor = lightColor * lightAttenuation;
                half3 smoothNormal = lerp(normalWS, vertexNormalDir, 0.2);
                half smooth_nol = dot(smoothNormal, lightDirectionWS);
                half3 skin_nol = GetSkinColorSp(smooth_nol, skin);
                half3 directDiffuse = brdfdiffuse * skin_nol * attenColor;
                return directDiffuse + brdfspecular * DirectBRDFSpecular(normalizationTerm, roughness2MinusOne, roughness2, normalWS, lightDirectionWS, viewDirectionWS) * attenColor * NdotL;
            }
            else
#endif
            {
                half3 radiance = lightColor * (lightAttenuation * NdotL);

                half3 brdf = brdfdiffuse;

                brdf += brdfspecular * DirectBRDFSpecular(normalizationTerm, roughness2MinusOne, roughness2, normalWS, lightDirectionWS, viewDirectionWS);
                return brdf * radiance;
            }
        }


#if _ANIS_SPECULAR
        
        half3 Fresnel(half3 SpecularColor, half VoH)
        {
            return F_Schlick(SpecularColor, VoH);
        }

        // Anisotropic GGX
        // [Burley 2012, "Physically-Based Shading at Disney"]
        half D_GGXaniso(half RoughnessX, half RoughnessY, half NoH, half3 H, half3 X, half3 Y)
        {
            half mx = RoughnessX * RoughnessX;
            half my = RoughnessY * RoughnessY;
            half XoH = dot(X, H);
            half YoH = dot(Y, H);
            half d = XoH * XoH / (mx * mx) + YoH * YoH / (my * my) + NoH * NoH;
            return 1.0 / (mx * my * d * d);
        }

        half3 Speuclar(half3 specularColor, half roughness, half NoH, half NoV, half NoL, half VoH, half3 H, half3 T, half3 B, half3 anisoCtrl)
        {
            return D_GGXaniso(anisoCtrl.x, anisoCtrl.y, NoH, H, T, B) * Fresnel(specularColor, VoH) * anisoCtrl.z;
        }

        half3 GetBRDFSpecular(half3 specularColor, half roughness, half NoH, half NoV, half NoL, half VoH, half3 H, half3 T, half3 B, half3 anisoCtrl)
        {
            return  Speuclar(specularColor, roughness, NoH, NoV, NoL, VoH, H, T, B, anisoCtrl) * roughness;
        }
#else
        //half3 GetBRDFSpecular(half3 specularColor, half roughness, half NoH, half NoV, half NoL, half VoH)
        //{
        //    return  Speuclar(specularColor, roughness, NoH, NoV, NoL, VoH) * roughness;
        //}
#endif


        // Matches Unity Vanila attenuation
// Attenuation smoothly decreases to light range.
        float DistanceAttenuation1(float distanceSqr, half2 distanceAttenuation)
        {
            // We use a shared distance attenuation for additional directional and puctual lights
            // for directional lights attenuation will be 1
            float lightAtten = rcp(distanceSqr);

#if 0
            // Use the smoothing factor also used in the Unity lightmapper.
            half factor = distanceSqr * distanceAttenuation.x;
            half smoothFactor = saturate(1.0h - factor * factor);
            smoothFactor = smoothFactor * smoothFactor;
#else
            // We need to smoothly fade attenuation to light range. We start fading linearly at 80% of light range
            // Therefore:
            // fadeDistance = (0.8 * 0.8 * lightRangeSq)
            // smoothFactor = (lightRangeSqr - distanceSqr) / (lightRangeSqr - fadeDistance)
            // We can rewrite that to fit a MAD by doing
            // distanceSqr * (1.0 / (fadeDistanceSqr - lightRangeSqr)) + (-lightRangeSqr / (fadeDistanceSqr - lightRangeSqr)
            // distanceSqr *        distanceAttenuation.y            +             distanceAttenuation.z
            half smoothFactor = saturate(distanceSqr * distanceAttenuation.x + distanceAttenuation.y);
#endif

            return lightAtten * smoothFactor;
        }

        float3 mod2D289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float2 mod2D289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float3 permute(float3 x) { return mod2D289(((x * 34.0) + 1.0) * x); }
        float snoise(float2 v)
        {
            const float4 C = float4(0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439);
            float2 i = floor(v + dot(v, C.yy));
            float2 x0 = v - i + dot(i, C.xx);
            float2 i1;
            i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
            float4 x12 = x0.xyxy + C.xxzz;
            x12.xy -= i1;
            i = mod2D289(i);
            float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));
            float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
            m = m * m;
            m = m * m;
            float3 x = 2.0 * frac(p * C.www) - 1.0;
            float3 h = abs(x) - 0.5;
            float3 ox = floor(x + 0.5);
            float3 a0 = x - ox;
            m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
            float3 g;
            g.x = a0.x * x0.x + h.x * x0.y;
            g.yz = a0.yz * x12.xz + h.yz * x12.yw;
            return 130.0 * dot(m, g);
        }


        // Brian Karis' modification of Dimitar Lazarov's Environment BRDF.
// cf https://www.unrealengine.com/blog/physically-based-shading-on-mobile
// [Lazarov 2013, "Getting More Physical in Call of Duty: Black Ops II"]
        half3 EnvBRDFApprox1(half3 SpecularColor, half Roughness, half NoV)
        {
            // Approximate version, base for pre integrated version
            half4 c0 = { (-(1)),(-(0.0275)),(-(0.572)),0.022 };
            half4 c1 = { 1,0.0425,1.04,(-(0.04)) };
            half4 r = ((((Roughness) * (c0))) + (c1));
            half a004 = ((((min(((r.x) * (r.x)), (half)exp2((((-(9.28))) * (NoV))))) * (r.x))) + (r.y));
            half2 AB = ((((half2((-(1.04)), 1.04)) * (a004))) + (r.zw));
            return ((((SpecularColor) * (AB.x))) + (AB.y));
        }

        half4 frag(Varyings input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
            float4 ImmCB_0[4];
            ImmCB_0[0] = float4(1.0, 0.0, 0.0, 0.0);
            ImmCB_0[1] = float4(0.0, 1.0, 0.0, 0.0);
            ImmCB_0[2] = float4(0.0, 0.0, 1.0, 0.0);
            ImmCB_0[3] = float4(0.0, 0.0, 0.0, 1.0);


            half3 vertexNormalDir = normalize(input.normalWS.xyz);



            float3 positionWS;
            positionWS.x = input.normalWS.w;
            positionWS.y = input.tangentWS.w;
            positionWS.z = input.bitangentWS.w;

  
            float3 V = normalize((-positionWS.xyz) + _WorldSpaceCameraPos.xyz);

            half4 albedo = SAMPLE_TEXTURE2D_BIAS(_MainTex, sampler_MainTex, input.texcoord.xy, -1) * _BaseColor;

#if NEEDHIT
            albedo.xyz += _Hit;
#endif

            half2 uv2 = input.texcoord.zw * _DecalsTex_ST.xy + _DecalsTex_ST.zw;
            half4 decalCol = SAMPLE_TEXTURE2D(_DecalsTex, sampler_DecalsTex, uv2);
            albedo.xyz = lerp(albedo.xyz, decalCol.xyz, decalCol.w);

            half3 tNormal = UnpackNormal(SAMPLE_TEXTURE2D_BIAS(_BumpMap, sampler_BumpMap, input.texcoord.xy,-1)).xyz;

            half4 detailMask = SAMPLE_TEXTURE2D_BIAS(_DetailMask, sampler_DetailMask, input.texcoord.xy, -1);
            

            half smoothness = detailMask.y;

            half metallic = saturate(detailMask.x + _Metallic);

            smoothness = smoothness * _RoughnessScale;

            smoothness = saturate(smoothness);

            half occlusion = (-detailMask.z) + 1.0;
            occlusion = occlusion * _OcclusionStrength;

            occlusion = 1.0 - saturate(occlusion);

#if DISSLOVE
            half3 Emisson184 = half3(0, 0, 0);
            if (_Disslove > 0)
            {
                float2 texCoord124 = input.texcoord.xy;
                float simplePerlin2D130 = snoise(texCoord124 * 30.0);
                simplePerlin2D130 = simplePerlin2D130 * 0.5 + 0.5;
                float4 transform163 = float4(UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w, 0);
                float4 break167 = (float4(positionWS, 0.0) - transform163);
                float2 appendResult114 = (float2(break167.x, break167.y));
                float simplePerlin2D125 = snoise(appendResult114 * 30.0);
                simplePerlin2D125 = simplePerlin2D125 * 0.5 + 0.5;
                float DissloveMask139 = (1.0 - (simplePerlin2D130 * saturate(simplePerlin2D125)));
                float4 color148 = float4(0, 0.08349621, 1, 0);
                float4 transform117 = transform163;
                float temp_output_132_0 = (((positionWS.y - transform117.y) + (-5.0 + ((_Disslove * 1.35) - 0.0) * (0.0 - -5.0) / (1.0 - 0.0))) + 0.5);
                float temp_output_133_0 = (temp_output_132_0 + 0.8);
                float4 lerpResult151 = lerp(float4(0, 0, 0, 0), color148, saturate((temp_output_133_0 + 1.0)));
                float4 color154 =  float4(0, 1.793486, 1.406099, 1);
                float L2Mask138 = saturate(temp_output_133_0);
                float4 lerpResult158 = lerp(lerpResult151, color154, L2Mask138);
                float4 color156 = float4(31.98566, 227.9413, 236.02, 0);
                float4 lerpResult159 = lerp(lerpResult158, color156, saturate(temp_output_132_0));
                Emisson184 = (DissloveMask139 * lerpResult159);

                float OffsetY160 = temp_output_132_0;
                float AlphaClip155 = saturate(((1.0 - (DissloveMask139 * L2Mask138)) * (1.0 - saturate(OffsetY160))));

                clip(AlphaClip155 - 0.5);
            }
#endif

            half3 N = input.tangentWS.xyz * tNormal.xxx + input.bitangentWS.xyz * tNormal.yyy + vertexNormalDir * tNormal.zzz;
            N = normalize(N);

#if  _CONST_POINT_LIT && !_GAMEPLAY
            half3 L0L1Term = SHEvalLinearL0L1(N, Const_SHAr, Const_SHAg, Const_SHAb);
#else
            half3 L0L1Term = SHEvalLinearL0L1(N, unity_SHAr, unity_SHAg, unity_SHAb);
#endif
            half3 shGI = L0L1Term + input.L2Term;


            shGI.xyz = max(shGI.xyz, half3(0.0, 0.0, 0.0));
            shGI.xyz = shGI.xyz * 1.35000002;
            half oneMinusReflectivity = OneMinusReflectivityMetallic(metallic);// (-metallic) * 0.959999979 + 0.959999979;
            half reflectivity = 1.0 - oneMinusReflectivity;
            half grazingTerm = saturate(smoothness + reflectivity);

            half3 brdfDiffuse  = albedo.xyz * oneMinusReflectivity;
            
            half3 brdfSpecular = lerp(kDieletricSpec.xyz, albedo.xyz, metallic);

            half perceptualRoughness = (-smoothness) + 1.0;
            half roughness = max(PerceptualRoughnessToRoughness(perceptualRoughness), HALF_MIN_SQRT);

            half roughness2 = roughness * roughness;
            half normalizationTerm = roughness * 4.0 + 2.0;
            half roughness2MinusOne = roughness * roughness + -1.0;

#if  _CONST_POINT_LIT && !_GAMEPLAY
            half shadowAttenuation = MainLightRealtimeShadow(input.shadowCoord);
#else
            half shadowAttenuation = min(MainLightRealtimeShadow(input.shadowCoord), unity_ProbesOcclusion.x);
#endif
 
            half3 envColor = shGI.xyz +  (-1.0);
            envColor.xyz = envColor.xyz * 0.5 + 1.0;

            half NoV = saturate(dot(N.xyz, V.xyz));

            half fresnelTerm = Pow4(1.0 - NoV);

            fresnelTerm = fresnelTerm * _FresnelIntensity;

            half3 indirectDiffuse;
            indirectDiffuse.xyz = shGI.xyz * occlusion;
            half luminance = dot(shGI.xyz * occlusion, half3(0.298999995, 0.587000012, 0.114));
            indirectDiffuse.xyz = shGI.xyz * occlusion - luminance;
            indirectDiffuse.xyz = _IndirectColor.www * indirectDiffuse.xyz + luminance;
            indirectDiffuse.xyz = indirectDiffuse.xyz * _IndirectColor.xyz;


            half3 reflectVector = reflect(-V, N);
            half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, perceptualRoughness, occlusion);

            indirectSpecular.xyz = envColor.xyz * indirectSpecular;

            indirectSpecular.xyz = indirectSpecular.xyz * EnvironmentBRDFSpecular(roughness2, brdfSpecular, grazingTerm, fresnelTerm);


            half3 indirectColor = indirectDiffuse.xyz * brdfDiffuse.xyz + indirectSpecular.xyz;
            
            luminance = dot(_MainLightColor.xyz, half3(0.298999995, 0.587000012, 0.114));


            half3 lightColor = (_DirectColor.www * (_MainLightColor.xyz - luminance) + luminance) * _DirectColor.xyz;

            half lightAttenuation = shadowAttenuation * unity_LightData.z;

            half skin = 1 - detailMask.w;
            half4 finalColor;
   
            half3 t = input.tangentWS.xyz;
            half3 b = input.bitangentWS.xyz;
#if _ANIS_SPECULAR
            // ShaderX: Per-Pixel Strand Based Anisotropic Lighting
            t = normalize(t - dot(t, N) * N); // Graham-Schmidt Orthonormalization
            b = normalize(cross(t, N));
            half3 aniso_ctrl = half3(_AnisoControl.x * perceptualRoughness, _AnisoControl.y * perceptualRoughness, _AnisoControl.z);
            aniso_ctrl.xy = max(aniso_ctrl.xy, (half2)0.1);

            half3 L = _MainLightPosition.xyz;
            half3 H = normalize(V + L);
            half3 NoH = saturate(dot(N.xyz, H.xyz));
            half3 NoL = saturate(dot(N.xyz, L.xyz));
            half3 VoH = saturate(dot(V.xyz, H.xyz));
            half3 directSpecular = GetBRDFSpecular(brdfSpecular, perceptualRoughness, NoH, NoV, NoL, VoH, H, t, b, aniso_ctrl);

            half3 radiance = lightColor * (lightAttenuation * NoL);

            finalColor.xyz = (brdfDiffuse + directSpecular) * radiance;
#else
            finalColor.xyz = indirectColor.xyz + LightingPhysicallyBased(brdfDiffuse,brdfSpecular, normalizationTerm,roughness2MinusOne,roughness2, lightColor,
                _MainLightPosition.xyz, lightAttenuation,
                N, V, vertexNormalDir, skin);
#endif

#if _ADDITIONAL_LIGHTS
#if _CONST_POINT_LIT
            const uint lightsCount = 2;
            float4 AdditionalLightsPosition[2] = { float4(-14,0,6.2 + 0.66,1.0), float4(-0.3,0,-1.5 + 0.66, 1.0) };
            half4 AdditionalLightsColor[2] = { half4(1.2,1.2,1.2,1.2), half4(0.35,0.67,1.1,1.5) };
            half4 DistanceAndSpotAttenuation[2] = { half4(0.0031, 2.8, 0, 1.0), half4(0.11,2.8,0,1) };

            float4 transformPos = { UNITY_MATRIX_M[0].w, UNITY_MATRIX_M[1].w, UNITY_MATRIX_M[2].w, 0 };
#else
            uint lightsCount = min(GetAdditionalLightsCount(), 2);
#endif
            for (uint u_xlatu_loop_1 = uint(0u); u_xlatu_loop_1 < lightsCount; u_xlatu_loop_1++)
            {   
#if _CONST_POINT_LIT
                float4 lightPositionWS = AdditionalLightsPosition[u_xlatu_loop_1] +transformPos;
                half3 lightColor = AdditionalLightsColor[u_xlatu_loop_1].rgb;
                half4 distanceAndSpotAttenuation = DistanceAndSpotAttenuation[u_xlatu_loop_1];
#else
                int u_xlati20 = int(uint(u_xlatu_loop_1 & 3u));
                int u_xlatu40 = uint(u_xlatu_loop_1 >> 2u);
                int perObjectLightIndex = int(dot(unity_LightIndices[int(u_xlatu40)], ImmCB_0[u_xlati20]));
    #if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
                    float4 lightPositionWS = _AdditionalLightsBuffer[perObjectLightIndex].position;
                    half3 lightColor = _AdditionalLightsBuffer[perObjectLightIndex].color.rgb;
                    half4 distanceAndSpotAttenuation = _AdditionalLightsBuffer[perObjectLightIndex].attenuation;
    #else
                    float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
                    half3 lightColor = _AdditionalLightsColor[perObjectLightIndex].rgb;
                    half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
    #endif
#endif

                float3 lightVector = lightPositionWS.xyz - positionWS * lightPositionWS.w;
                float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);

                half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
                half attenuation = DistanceAttenuation1(distanceSqr, distanceAndSpotAttenuation.xy);

#if _ANIS_SPECULAR

                half3 L = lightDirection.xyz;
                half3 H = normalize(V + L);
                half3 NoH = saturate(dot(N.xyz, H.xyz));
                half3 NoL = saturate(dot(N.xyz, L.xyz));
                half3 VoH = saturate(dot(V.xyz, H.xyz));
                half3 directSpecular = GetBRDFSpecular(brdfSpecular, perceptualRoughness, NoH, NoV, NoL, VoH, H, t, b, aniso_ctrl);

                half3 radiance = lightColor * (lightAttenuation * NoL);

                finalColor.xyz += (brdfDiffuse + directSpecular) * radiance;
#else
                finalColor.xyz += LightingPhysicallyBased(brdfDiffuse, brdfSpecular, normalizationTerm, roughness2MinusOne, roughness2, lightColor,
                    lightDirection.xyz, attenuation,
                    N, V, vertexNormalDir, skin);
#endif

            }
#endif
            NoV = max(dot(V, vertexNormalDir), 0.0001);
            half back = smoothstep(_NiGuang, 1.0, 1.0 - NoV );
            half LoN = dot(_MainLightPosition.xyz, vertexNormalDir);
            float2 uv_RapMap = input.texcoord.xy * _RapMap_ST.xy + _RapMap_ST.zw;
            half2 rap = SAMPLE_TEXTURE2D(_RapMap, sampler_RapMap, uv_RapMap).rg;
            half offset = clamp((rap.r + ((rap.g * _Anisotropy_base) + -0.2)), 0.0, 1.0);
            half ToV = dot((input.bitangentWS.xyz + (vertexNormalDir * offset)), V);
            half rToV = max(sqrt((1.0 - (ToV * ToV))), 0.0001);

            half4 anisotropyCol = saturate(((_NiGuangClo * back * (LoN * 1.0 + 0.2)) + (_strength * (((pow(rToV, _Anisotropy_R1) * _R1_Color) + (pow(rToV, _Anisotropy_R2) * _R2_Color)) * pow(NoV, _MaskPower)))));

            finalColor.xyz += anisotropyCol.xyz * detailMask.w;
#if DISSLOVE
            finalColor.xyz += Emisson184.xyz;
#endif
            finalColor = max(finalColor, half4(0.0, 0.0, 0.0, 0.0));

#if TONEMAP
            finalColor.xyz = finalColor.xyz * 1.46166837;
            half3 color1 = finalColor.xyz * 0.85946101 + 0.121230006;
            color1.xyz = finalColor.xyz * color1.xyz + 0.0504000038;
            half3 color2 = finalColor.xyz * 0.85946101 + 0.449000001;
            finalColor.xyz = finalColor.xyz * color2.xyz + 0.316799998;
            finalColor.xyz = color1.xyz / finalColor.xyz;
            finalColor.xyz = finalColor.xyz + (-0.159090906);
            finalColor.xyz = finalColor.xyz * 1.46166837;
#endif
            finalColor.w = 1.0;

            return finalColor;
        }

        ENDHLSL

        }
       
        Pass
        {
           Name "Occluded"
           Tags { "LightMode" = "Occluded" }

            Blend One Zero
            Cull Back
            ZWrite Off
            ZTest Greater

            Stencil {
                Ref 10
                Comp NotEqual
                Pass Replace
                Fail Keep
            }


            HLSLPROGRAM

             #pragma multi_compile_instancing

            #pragma vertex LowVertex
            #pragma fragment LowFragment

             struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings LowVertex(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_TRANSFER_INSTANCE_ID(i, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz + lerp(10000.0, 0.0, _Occluded).xxx);
                output.positionCS = vertexInput.positionCS;
                return output;
            }

            half4 LowFragment(Varyings i) : SV_Target
            {
                return  _OccludedColor;
            }
            ENDHLSL
        }


        Pass
        {
           Name "ShadowCaster"
           Tags { "LightMode" = "ShadowCaster" }

           ZWrite On
           ZTest LEqual
           ColorMask 0

           HLSLPROGRAM
           
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            float3 _LightDirection;

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };

            float4 GetShadowPositionHClip(Attributes input)
            {
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

            #if UNITY_REVERSED_Z
                positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
            #else
                positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
            #endif

                return positionCS;
            }

            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);

                output.positionCS = GetShadowPositionHClip(input);
                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_TARGET
            {
                return 0;
            }



           ENDHLSL

        }
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off

            HLSLPROGRAM

             #pragma multi_compile_instancing

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            

                struct Attributes
                {
                    float4 position     : POSITION;
                    float2 texcoord     : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4 positionCS   : SV_POSITION;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                Varyings DepthOnlyVertex(Attributes input)
                {
                    Varyings output = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(input);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                    output.positionCS = TransformObjectToHClip(input.position.xyz);
                    return output;
                }

                half4 DepthOnlyFragment(Varyings input) : SV_TARGET
                {
                    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                    return 0;
                }

            ENDHLSL

        }
    }

    CustomEditor "SimpleShaderGUI"
}
