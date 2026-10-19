Shader "Rugy/UniversalEnvironmentFX"
{
    Properties
    {
        [MainTexture] _BaseMap ("Texture", 2D) = "white" {}
        [MainColor] _Tint ("Tint", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,0.5,0.1,1)
        _GlowStrength ("Glow Strength", Range(0,3)) = 0.5
        _WaveAmplitude ("Wave Amplitude", Range(0,0.2)) = 0.02
        _WaveFrequency ("Wave Frequency", Range(0,8)) = 1
        _ScrollSpeed ("Scroll Speed", Range(-2,2)) = 0
        _PulseSpeed ("Pulse Speed", Range(0,5)) = 1
        _DistortionStrength ("Distortion", Range(0,0.2)) = 0
        _AlphaClip ("Alpha Clip", Range(0,1)) = 0.01
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
        }

        Pass
        {
            Name "UniversalEnvironmentFX"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _Tint;
                half4 _GlowColor;
                half _GlowStrength;
                half _WaveAmplitude;
                half _WaveFrequency;
                half _ScrollSpeed;
                half _PulseSpeed;
                half _DistortionStrength;
                half _AlphaClip;
            CBUFFER_END

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 position = input.positionOS.xyz;
                float time = _Time.y;
                float wave = sin((position.x + time * _ScrollSpeed) * _WaveFrequency) * _WaveAmplitude;
                float drift = cos((position.y - time * _ScrollSpeed) * (_WaveFrequency + 0.5)) * _WaveAmplitude * 0.5;
                position.xy += float2(wave, drift);
                output.positionHCS = TransformObjectToHClip(position);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float time = _Time.y;
                float2 shiftedUV = uv + float2(time * _ScrollSpeed * 0.05, 0);
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, shiftedUV);
                float noise = hash21(floor(uv * 18 + time * _PulseSpeed));
                float pulse = 0.5 + 0.5 * sin(time * _PulseSpeed + uv.x * 6.2831);
                half glow = saturate((_GlowStrength * 0.35) + pulse * _GlowStrength * 0.35 + noise * _GlowStrength * 0.15);
                half4 color = tex * _Tint * input.color;
                color.rgb += _GlowColor.rgb * glow;
                color.a *= _Tint.a * input.color.a;
                clip(color.a - _AlphaClip);
                return color;
            }
            ENDHLSL
        }
    }
}
