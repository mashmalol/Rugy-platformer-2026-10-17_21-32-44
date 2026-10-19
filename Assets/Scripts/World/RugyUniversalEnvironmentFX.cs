using System;
using UnityEngine;

/// <summary>
/// Universal environment effects controller for Rugy's 2.5D world.
/// It randomizes shader motion, tint, glow, particle behavior, light mood,
/// object sway, and effect profiles for water, fire, sky, fog, clouds,
/// aurora, motes, and other environmental eye-candy assets.
///
/// Add this component to one scene-level FX object, assign the renderers,
/// particle systems, lights, and transforms, then connect RandomizeNow()
/// to a Unity UI Button for a live mood-randomizer effect.
/// </summary>
public class RugyUniversalEnvironmentFX : MonoBehaviour
{
    public enum EffectProfile
    {
        CalmCourtyard,
        WarmEmbers,
        MoonlitVoid,
        ThreadStorm,
        DreamBloom,
        Random
    }

    [Serializable]
    private class RendererEntry
    {
        public Renderer renderer;
        [Range(0f, 1f)] public float influence = 1f;
        public bool randomizeTint = true;
        public bool randomizeGlow = true;
        public bool randomizeMotion = true;
    }

    [Serializable]
    private class ParticleEntry
    {
        public ParticleSystem particleSystem;
        public float minimumRate = 2f;
        public float maximumRate = 20f;
        public float minimumLifetime = 1f;
        public float maximumLifetime = 4f;
        public bool randomizeColor = true;
        public bool randomizeSize = true;
    }

    [Header("Randomizer")]
    [SerializeField] private EffectProfile profile = EffectProfile.Random;
    [SerializeField] private bool randomizeOnStart = true;
    [SerializeField] private bool autoRandomize;
    [SerializeField] private float randomizeInterval = 12f;
    [SerializeField] private int seed = 0;
    [SerializeField] private bool useNewSeedEachTime = true;

    [Header("Environmental renderers")]
    [SerializeField] private RendererEntry[] renderers;

    [Header("Particle systems")]
    [SerializeField] private ParticleEntry[] particleSystems;

    [Header("Lights")]
    [SerializeField] private Light[] moodLights;
    [SerializeField] private bool randomizeLightIntensity = true;

    [Header("Sway objects")]
    [SerializeField] private Transform[] swayObjects;
    [SerializeField] private float maximumSwayAngle = 5f;
    [SerializeField] private float maximumSwayScale = 0.04f;

    [Header("Runtime palette")]
    [SerializeField] private Color primaryColor = new Color(0.62f, 0.19f, 0.25f);
    [SerializeField] private Color accentColor = new Color(0.12f, 0.65f, 0.63f);
    [SerializeField] private Color glowColor = new Color(0.90f, 0.66f, 0.23f);
    [SerializeField] private Color shadowColor = new Color(0.09f, 0.14f, 0.28f);

    private static readonly int TintId = Shader.PropertyToID("_Tint");
    private static readonly int GlowColorId = Shader.PropertyToID("_GlowColor");
    private static readonly int GlowStrengthId = Shader.PropertyToID("_GlowStrength");
    private static readonly int WaveAmplitudeId = Shader.PropertyToID("_WaveAmplitude");
    private static readonly int WaveFrequencyId = Shader.PropertyToID("_WaveFrequency");
    private static readonly int ScrollSpeedId = Shader.PropertyToID("_ScrollSpeed");
    private static readonly int PulseSpeedId = Shader.PropertyToID("_PulseSpeed");
    private static readonly int DistortionId = Shader.PropertyToID("_DistortionStrength");

    private System.Random random;
    private float nextRandomizeTime;
    private readonly System.Collections.Generic.Dictionary<Transform, Vector3> originalScales = new();
    private readonly System.Collections.Generic.Dictionary<Transform, Quaternion> originalRotations = new();

    public EffectProfile CurrentProfile => profile;

    private void Awake()
    {
        CacheOriginalTransforms();
        InitializeRandom();
    }

    private void Start()
    {
        if (randomizeOnStart)
            RandomizeNow();
    }

    private void Update()
    {
        if (!autoRandomize || Time.unscaledTime < nextRandomizeTime)
            return;

        RandomizeNow();
    }

    /// <summary>Public button-friendly action that creates a new environmental mood.</summary>
    public void RandomizeNow()
    {
        if (useNewSeedEachTime)
            InitializeRandom();

        EffectProfile activeProfile = profile == EffectProfile.Random
            ? ChooseRandomProfile()
            : profile;

        ApplyProfilePalette(activeProfile);
        ApplyRendererState(activeProfile);
        ApplyParticleState(activeProfile);
        ApplyLightState(activeProfile);
        ApplySwayState();

        nextRandomizeTime = Time.unscaledTime + Mathf.Max(0.2f, randomizeInterval);
    }

    /// <summary>Sets the profile and immediately applies it.</summary>
    public void SetProfile(int profileIndex)
    {
        profile = (EffectProfile)Mathf.Clamp(profileIndex, 0, Enum.GetValues(typeof(EffectProfile)).Length - 1);
        RandomizeNow();
    }

    /// <summary>Enables or disables automatic mood changes.</summary>
    public void SetAutoRandomize(bool enabled)
    {
        autoRandomize = enabled;
        nextRandomizeTime = Time.unscaledTime + randomizeInterval;
    }

    /// <summary>Use this from a UI button to set the randomizer interval.</summary>
    public void SetRandomizeInterval(float seconds)
    {
        randomizeInterval = Mathf.Max(0.2f, seconds);
    }

    private void InitializeRandom()
    {
        int chosenSeed = useNewSeedEachTime || seed == 0
            ? Environment.TickCount ^ GetInstanceID()
            : seed;
        random = new System.Random(chosenSeed);
    }

    private EffectProfile ChooseRandomProfile()
    {
        int value = random.Next(0, 5);
        return (EffectProfile)value;
    }

    private void ApplyProfilePalette(EffectProfile activeProfile)
    {
        switch (activeProfile)
        {
            case EffectProfile.CalmCourtyard:
                primaryColor = Hex("9E3040");
                accentColor = Hex("1FA6A0");
                glowColor = Hex("E6A83B");
                shadowColor = Hex("172447");
                break;
            case EffectProfile.WarmEmbers:
                primaryColor = Hex("9E3040");
                accentColor = Hex("E6A83B");
                glowColor = new Color(1f, 0.27f, 0.06f);
                shadowColor = Hex("531D35");
                break;
            case EffectProfile.MoonlitVoid:
                primaryColor = Hex("531D35");
                accentColor = Hex("8C64C7");
                glowColor = Hex("F1D9A6");
                shadowColor = Hex("172447");
                break;
            case EffectProfile.ThreadStorm:
                primaryColor = Hex("9E3040");
                accentColor = Hex("1FA6A0");
                glowColor = Hex("8C64C7");
                shadowColor = Hex("172447");
                break;
            case EffectProfile.DreamBloom:
                primaryColor = Hex("9E3040");
                accentColor = Hex("277C62");
                glowColor = Hex("E6A83B");
                shadowColor = Hex("8C64C7");
                break;
        }
    }

    private void ApplyRendererState(EffectProfile activeProfile)
    {
        float intensity = activeProfile == EffectProfile.WarmEmbers ? 1.2f : 1f;
        foreach (RendererEntry entry in renderers)
        {
            if (entry == null || entry.renderer == null)
                continue;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            entry.renderer.GetPropertyBlock(block);

            if (entry.randomizeTint)
                block.SetColor(TintId, Blend(primaryColor, accentColor, Next01()) * entry.influence);
            if (entry.randomizeGlow)
            {
                block.SetColor(GlowColorId, glowColor);
                block.SetFloat(GlowStrengthId, Mathf.Lerp(0.15f, 1.4f, Next01()) * intensity);
            }
            if (entry.randomizeMotion)
            {
                block.SetFloat(WaveAmplitudeId, Mathf.Lerp(0.005f, 0.08f, Next01()));
                block.SetFloat(WaveFrequencyId, Mathf.Lerp(0.3f, 3f, Next01()));
                block.SetFloat(ScrollSpeedId, Mathf.Lerp(-0.35f, 0.35f, Next01()));
                block.SetFloat(PulseSpeedId, Mathf.Lerp(0.2f, 2.5f, Next01()));
                block.SetFloat(DistortionId, Mathf.Lerp(0f, 0.08f, Next01()));
            }

            entry.renderer.SetPropertyBlock(block);
        }
    }

    private void ApplyParticleState(EffectProfile activeProfile)
    {
        foreach (ParticleEntry entry in particleSystems)
        {
            if (entry == null || entry.particleSystem == null)
                continue;

            ParticleSystem.MainModule main = entry.particleSystem.main;
            ParticleSystem.EmissionModule emission = entry.particleSystem.emission;
            float rate = Mathf.Lerp(entry.minimumRate, entry.maximumRate, Next01());
            float lifetime = Mathf.Lerp(entry.minimumLifetime, entry.maximumLifetime, Next01());

            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime * 0.75f, lifetime * 1.25f);

            if (entry.randomizeColor)
                main.startColor = Blend(accentColor, glowColor, Next01());
            if (entry.randomizeSize)
                main.startSize = Mathf.Lerp(0.06f, 0.32f, Next01());

            if (activeProfile == EffectProfile.ThreadStorm)
                entry.particleSystem.Play();
        }
    }

    private void ApplyLightState(EffectProfile activeProfile)
    {
        foreach (Light lightSource in moodLights)
        {
            if (lightSource == null)
                continue;

            lightSource.color = Blend(glowColor, accentColor, Next01());
            if (randomizeLightIntensity)
            {
                float multiplier = activeProfile == EffectProfile.MoonlitVoid ? 0.55f : 1f;
                lightSource.intensity = Mathf.Lerp(0.4f, 2.2f, Next01()) * multiplier;
            }
        }
    }

    private void ApplySwayState()
    {
        foreach (Transform target in swayObjects)
        {
            if (target == null)
                continue;

            if (!originalScales.ContainsKey(target))
                originalScales[target] = target.localScale;
            if (!originalRotations.ContainsKey(target))
                originalRotations[target] = target.localRotation;

            target.localScale = originalScales[target] * (1f + Mathf.Lerp(-maximumSwayScale, maximumSwayScale, Next01()));
            target.localRotation = originalRotations[target] * Quaternion.Euler(0f, 0f, Mathf.Lerp(-maximumSwayAngle, maximumSwayAngle, Next01()));
        }
    }

    private void CacheOriginalTransforms()
    {
        foreach (Transform target in swayObjects)
        {
            if (target == null) continue;
            originalScales[target] = target.localScale;
            originalRotations[target] = target.localRotation;
        }
    }

    private float Next01() => (float)random.NextDouble();

    private static Color Blend(Color a, Color b, float amount)
    {
        return Color.Lerp(a, b, Mathf.Clamp01(amount));
    }

    private static Color Hex(string value)
    {
        if (ColorUtility.TryParseHtmlString("#" + value, out Color color))
            return color;
        return Color.white;
    }
}
