using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Sword slash arc VFX — LineRenderer ile yay geometrisi cizer.
    /// Ortasi kalin, uclari ince, additive fade: klasik ARPG slash gorunumu.
    /// PlayerAttack.Emit(direction, comboStep) ile tetiklenir.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class SlashArcVFX : MonoBehaviour
    {
        [Header("Arc Geometry")]
        [SerializeField] private int   segments  = 24;   // yay nokta sayisi (kalite)
        [SerializeField] private float arcAngle  = 130f; // derece cinsinden yay acisi

        [Header("Per Combo Step — radius / width / duration")]
        [SerializeField] private float[] radius   = { 1.1f, 1.25f, 1.55f };
        [SerializeField] private float[] maxWidth = { 0.18f, 0.22f, 0.32f };
        [SerializeField] private float[] duration = { 0.13f, 0.15f, 0.20f };

        [Header("Colors (additive — parlaklik = alpha)")]
        [SerializeField] private Color colorCore = new Color(1.00f, 1.00f, 1.00f, 1.0f); // beyaz core
        [SerializeField] private Color colorRim  = new Color(0.45f, 0.75f, 1.00f, 0.7f); // soguk mavi kenar

        [Header("Material")]
        [SerializeField] private Material lineMaterial;

        private LineRenderer lr;
        private Coroutine    fadeRoutine;

        // ─── Init ────────────────────────────────────────────────────────────

        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
            ConfigureRenderer();
        }

        private void ConfigureRenderer()
        {
            lr.useWorldSpace    = true;
            lr.loop             = false;
            lr.positionCount    = segments + 1;
            lr.numCapVertices   = 4;   // yuvarlak uclar
            lr.numCornerVertices = 4;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows   = false;
            lr.sortingLayerName = "VFX";
            lr.sortingOrder     = 5;

            if (lineMaterial != null)
                lr.material = lineMaterial;

            // Baslangi cta gizle
            lr.enabled = false;
        }

        // ─── Public API ──────────────────────────────────────────────────────

        public void Emit(Vector2 worldDirection, int comboStep)
        {
            int step = Mathf.Clamp(comboStep, 0, 2);

            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            DrawArc(worldDirection, radius[step], maxWidth[step]);
            fadeRoutine = StartCoroutine(FadeOut(duration[step], maxWidth[step]));
        }

        // ─── Arc Geometry ────────────────────────────────────────────────────

        private void DrawArc(Vector2 direction, float r, float width)
        {
            float centerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float halfArc     = arcAngle * 0.5f;
            float startAngle  = centerAngle - halfArc;
            float endAngle    = centerAngle + halfArc;

            Vector3 origin = transform.position;

            // Noktalari hesapla
            for (int i = 0; i <= segments; i++)
            {
                float t     = (float)i / segments;
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;
                lr.SetPosition(i, origin + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
            }

            // Width curve: 0 → max → 0  (uclari ince, ortasi kalin)
            var widthCurve = new AnimationCurve();
            widthCurve.AddKey(new Keyframe(0.0f, 0.0f));
            widthCurve.AddKey(new Keyframe(0.1f, width));
            widthCurve.AddKey(new Keyframe(0.5f, width));
            widthCurve.AddKey(new Keyframe(0.9f, width));
            widthCurve.AddKey(new Keyframe(1.0f, 0.0f));
            lr.widthCurve = widthCurve;

            // Renk gradyani: core beyaz ortada, rim mavisi kenarlarda
            var grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(colorRim,  0.0f),
                    new GradientColorKey(colorCore, 0.3f),
                    new GradientColorKey(colorCore, 0.7f),
                    new GradientColorKey(colorRim,  1.0f),
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(0.0f, 0.0f),
                    new GradientAlphaKey(1.0f, 0.15f),
                    new GradientAlphaKey(1.0f, 0.85f),
                    new GradientAlphaKey(0.0f, 1.0f),
                }
            );
            lr.colorGradient = grad;

            lr.enabled = true;
        }

        // ─── Fade ────────────────────────────────────────────────────────────

        private IEnumerator FadeOut(float dur, float startWidth)
        {
            float elapsed = 0f;

            while (elapsed < dur)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dur;

                // Width sifirlanir (tum curve scale edilir)
                float w = Mathf.Lerp(startWidth, 0f, t * t);  // ease-in: sona dogru hizlanir
                lr.widthMultiplier = w / startWidth;

                yield return null;
            }

            lr.enabled    = false;
            lr.widthMultiplier = 1f;
            fadeRoutine   = null;
        }
    }
}
