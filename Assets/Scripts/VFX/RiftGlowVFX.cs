using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Runtime particle glow for baked rift-crack lines.
    /// Attach to a child GameObject positioned on the weapon/body crack anchor.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class RiftGlowVFX : MonoBehaviour
    {
        [Header("Glow Color")]
        [SerializeField] private Color glowColor = Color.white;

        [Header("Emission Rates")]
        [SerializeField] private float idleEmissionRate = 3f;
        [SerializeField] private float castEmissionRate = 25f;
        [SerializeField] private float rageEmissionRate = 15f;
        [SerializeField] private float hitSpikeEmissionRate = 40f;

        [Header("Intensity Levels")]
        [SerializeField] private float idleIntensity = 0.25f;
        [SerializeField] private float castIntensity = 1.0f;
        [SerializeField] private float ragePeakIntensity = 0.8f;
        [SerializeField] private float hitSpikeIntensity = 1.2f;

        [Header("Pulse")]
        [SerializeField] private float idlePulseSpeed = 0.8f;
        [SerializeField] private float ragePulseSpeed = 3.0f;

        [Header("Timing")]
        [SerializeField] private float castFlashDuration = 0.15f;
        [SerializeField] private float hitSpikeDuration = 0.08f;
        [SerializeField] private float deathFadeDuration = 1.2f;

        [Header("References")]
        [SerializeField] private RageSystem rageSystem;
        [SerializeField] private Health health;
        [SerializeField] private SkillFlowTracker skillFlow;
        [SerializeField] private ParticleSystem secondaryPS;

        private ParticleSystem primaryPS;
        private bool isCasting;
        private bool rageActive;
        private bool hitSpikeActive;
        private bool isDead;
        private float deathFadeMultiplier = 1f;
        private Coroutine castRoutine;
        private Coroutine hitRoutine;
        private Coroutine deathRoutine;

        private void Awake()
        {
            primaryPS = GetComponent<ParticleSystem>();

            if (rageSystem == null)
                rageSystem = GetComponentInParent<RageSystem>();
            if (health == null)
                health = GetComponentInParent<Health>();
            if (skillFlow == null)
                skillFlow = GetComponentInParent<SkillFlowTracker>();

            ConfigureParticleSystem(primaryPS);
            ConfigureParticleSystem(secondaryPS);
            ApplyVisuals(idleEmissionRate, idleIntensity);
        }

        private void OnEnable()
        {
            if (rageSystem != null)
                rageSystem.OnBloodrageStateChanged.AddListener(OnBloodrageChanged);
            if (health != null)
            {
                health.OnDamageTaken.AddListener(OnDamageTaken);
                health.OnDeath.AddListener(OnDeath);
            }
            if (skillFlow != null)
                skillFlow.OnSkillUsed += OnSkillUsed;
        }

        private void OnDisable()
        {
            if (rageSystem != null)
                rageSystem.OnBloodrageStateChanged.RemoveListener(OnBloodrageChanged);
            if (health != null)
            {
                health.OnDamageTaken.RemoveListener(OnDamageTaken);
                health.OnDeath.RemoveListener(OnDeath);
            }
            if (skillFlow != null)
                skillFlow.OnSkillUsed -= OnSkillUsed;
        }

        private void Update()
        {
            if (isDead)
                return;

            float emissionRate = idleEmissionRate;
            float intensity = idleIntensity;
            float pulseSpeed = idlePulseSpeed;

            if (rageActive)
            {
                emissionRate = rageEmissionRate;
                intensity = ragePeakIntensity;
                pulseSpeed = ragePulseSpeed;
            }

            if (isCasting)
            {
                emissionRate = Mathf.Max(emissionRate, castEmissionRate);
                intensity = Mathf.Max(intensity, castIntensity);
                pulseSpeed = Mathf.Max(pulseSpeed, ragePulseSpeed);
            }

            if (hitSpikeActive)
            {
                emissionRate = Mathf.Max(emissionRate, hitSpikeEmissionRate);
                intensity = Mathf.Max(intensity, hitSpikeIntensity);
            }

            float pulse = Mathf.Lerp(0.75f, 1.15f, Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f);
            ApplyVisuals(emissionRate, intensity * pulse * deathFadeMultiplier);
        }

        public void SetCastState(bool casting)
        {
            isCasting = casting;
        }

        public void SetColor(Color color)
        {
            glowColor = color;
            ApplyVisuals(idleEmissionRate, idleIntensity);
        }

        private void OnSkillUsed(SkillBase _)
        {
            if (castRoutine != null)
                StopCoroutine(castRoutine);

            castRoutine = StartCoroutine(CastFlash());
        }

        private IEnumerator CastFlash()
        {
            SetCastState(true);
            yield return new WaitForSeconds(castFlashDuration);
            SetCastState(false);
            castRoutine = null;
        }

        private void OnDamageTaken(int _)
        {
            if (isDead)
                return;

            if (hitRoutine != null)
                StopCoroutine(hitRoutine);

            hitRoutine = StartCoroutine(HitSpike());
        }

        private IEnumerator HitSpike()
        {
            hitSpikeActive = true;
            yield return new WaitForSeconds(hitSpikeDuration);
            hitSpikeActive = false;
            hitRoutine = null;
        }

        private void OnDeath()
        {
            if (deathRoutine != null)
                StopCoroutine(deathRoutine);

            deathRoutine = StartCoroutine(DeathFade());
        }

        private IEnumerator DeathFade()
        {
            isDead = true;
            float elapsed = 0f;
            while (elapsed < deathFadeDuration)
            {
                elapsed += Time.deltaTime;
                deathFadeMultiplier = Mathf.Clamp01(1f - elapsed / deathFadeDuration);
                ApplyVisuals(idleEmissionRate, idleIntensity * deathFadeMultiplier);
                yield return null;
            }

            ApplyVisuals(0f, 0f);
            StopParticles(primaryPS);
            StopParticles(secondaryPS);
            deathRoutine = null;
        }

        private void OnBloodrageChanged(bool active)
        {
            rageActive = active;
        }

        private void ConfigureParticleSystem(ParticleSystem ps)
        {
            if (ps == null)
                return;

            var main = ps.main;
            main.loop = true;
            main.playOnAwake = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 0.9f);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.03f, 0.06f);
            main.gravityModifier = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.08f;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildFadeGradient(glowColor, idleIntensity);

            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
                renderer.renderMode = ParticleSystemRenderMode.Billboard;

            if (!ps.isPlaying)
                ps.Play();
        }

        private void ApplyVisuals(float emissionRate, float intensity)
        {
            ApplyVisuals(primaryPS, emissionRate, intensity);
            ApplyVisuals(secondaryPS, emissionRate, intensity);
        }

        private void ApplyVisuals(ParticleSystem ps, float emissionRate, float intensity)
        {
            if (ps == null)
                return;

            var emission = ps.emission;
            emission.enabled = emissionRate > 0f && intensity > 0f;
            emission.rateOverTime = emissionRate;

            var main = ps.main;
            Color c = glowColor;
            c.a *= Mathf.Clamp01(intensity);
            main.startColor = c;

            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.color = BuildFadeGradient(glowColor, intensity);
        }

        private static Gradient BuildFadeGradient(Color color, float intensity)
        {
            Color start = color;
            start.a *= Mathf.Clamp01(intensity);

            Color end = color;
            end.a = 0f;

            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(start, 0f), new GradientColorKey(color, 1f) },
                new[] { new GradientAlphaKey(start.a, 0f), new GradientAlphaKey(0f, 1f) });
            return gradient;
        }

        private static void StopParticles(ParticleSystem ps)
        {
            if (ps != null)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
