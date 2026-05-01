using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Small runtime hand/orb particle effect for class identity VFX.
    /// Attach to a child GameObject positioned at the relevant hand/orb anchor.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class HandGlowVFX : MonoBehaviour
    {
        [Header("Glow")]
        [SerializeField] private Color glowColor = Color.white;

        [Header("Emission")]
        [SerializeField] private float idleEmissionRate = 8f;
        [SerializeField] private float castEmissionRate = 40f;

        [Header("Intensity")]
        [SerializeField] private float idleIntensity = 0.3f;
        [SerializeField] private float castIntensity = 1.0f;
        [SerializeField] private float castFlashDuration = 0.18f;

        [Header("References")]
        [SerializeField] private SkillFlowTracker skillFlow;

        private ParticleSystem particleSystemRef;
        private bool casting;
        private Coroutine castRoutine;

        private void Awake()
        {
            particleSystemRef = GetComponent<ParticleSystem>();
            if (skillFlow == null)
                skillFlow = GetComponentInParent<SkillFlowTracker>();

            ConfigureParticleSystem();
            ApplyVisuals(idleEmissionRate, idleIntensity);
        }

        private void OnEnable()
        {
            if (skillFlow != null)
                skillFlow.OnSkillUsed += OnSkillUsed;
        }

        private void OnDisable()
        {
            if (skillFlow != null)
                skillFlow.OnSkillUsed -= OnSkillUsed;
        }

        public void SetCastState(bool isCasting)
        {
            casting = isCasting;
            ApplyVisuals(casting ? castEmissionRate : idleEmissionRate,
                         casting ? castIntensity : idleIntensity);
        }

        public void SetColor(Color color)
        {
            glowColor = color;
            ApplyVisuals(casting ? castEmissionRate : idleEmissionRate,
                         casting ? castIntensity : idleIntensity);
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

        private void ConfigureParticleSystem()
        {
            var main = particleSystemRef.main;
            main.loop = true;
            main.playOnAwake = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.4f, 0.7f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.4f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.08f);
            main.gravityModifier = -0.1f;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            var shape = particleSystemRef.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
            shape.radius = 0.05f;

            var colorOverLifetime = particleSystemRef.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = BuildFadeGradient(glowColor, idleIntensity);

            if (!particleSystemRef.isPlaying)
                particleSystemRef.Play();
        }

        private void ApplyVisuals(float emissionRate, float intensity)
        {
            if (particleSystemRef == null)
                return;

            var emission = particleSystemRef.emission;
            emission.enabled = emissionRate > 0f;
            emission.rateOverTime = emissionRate;

            var main = particleSystemRef.main;
            Color c = glowColor;
            c.a *= Mathf.Clamp01(intensity);
            main.startColor = c;

            var colorOverLifetime = particleSystemRef.colorOverLifetime;
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
    }
}
