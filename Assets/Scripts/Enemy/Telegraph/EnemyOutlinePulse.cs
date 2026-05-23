using System.Collections;
using UnityEngine;

namespace RIMA.Enemy.Telegraph
{
    [DisallowMultipleComponent]
    public class EnemyOutlinePulse : MonoBehaviour
    {
        private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");
        private static readonly int OutlineAlphaId = Shader.PropertyToID("_OutlineAlpha");
        private static readonly int OutlineThicknessId = Shader.PropertyToID("_OutlineThickness");
        private static readonly int OutlineWidthId = Shader.PropertyToID("_OutlineWidth");

        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private SpriteRenderer fallbackSpriteRenderer;
        [SerializeField] private Color pulseColor = new Color(1f, 0.08f, 0.04f, 1f);
        [SerializeField] private float maxOutlineThickness = 4f;

        private MaterialPropertyBlock propertyBlock;
        private Coroutine pulseRoutine;
        private Color originalSpriteColor;
        private bool hasOriginalSpriteColor;
        private bool supportsOutlineAlpha;
        private bool supportsOutlineThickness;
        private bool supportsOutlineWidth;
        private bool supportsOutlineColor;

        private void Awake()
        {
            ResolveTargets();
            CacheShaderSupport();
        }

        private void OnDisable()
        {
            TelegraphEnd();
        }

        public void TelegraphStart(float duration)
        {
            ResolveTargets();
            CacheShaderSupport();

            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
            }

            pulseRoutine = StartCoroutine(PulseRoutine(Mathf.Max(0.01f, duration)));
        }

        public void TelegraphEnd()
        {
            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
                pulseRoutine = null;
            }

            ApplyPulse(0f);
        }

        private IEnumerator PulseRoutine(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float alpha = Mathf.Sin(t * Mathf.PI);
                ApplyPulse(alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }

            ApplyPulse(0f);
            pulseRoutine = null;
        }

        private void ApplyPulse(float alpha)
        {
            if (targetRenderer != null && (supportsOutlineAlpha || supportsOutlineThickness || supportsOutlineWidth || supportsOutlineColor))
            {
                if (propertyBlock == null)
                {
                    propertyBlock = new MaterialPropertyBlock();
                }

                targetRenderer.GetPropertyBlock(propertyBlock);

                if (supportsOutlineColor)
                {
                    Color color = pulseColor;
                    color.a *= alpha;
                    propertyBlock.SetColor(OutlineColorId, color);
                }

                if (supportsOutlineAlpha)
                {
                    propertyBlock.SetFloat(OutlineAlphaId, alpha);
                }

                if (supportsOutlineThickness)
                {
                    propertyBlock.SetFloat(OutlineThicknessId, maxOutlineThickness * alpha);
                }

                if (supportsOutlineWidth)
                {
                    propertyBlock.SetFloat(OutlineWidthId, maxOutlineThickness * alpha);
                }

                targetRenderer.SetPropertyBlock(propertyBlock);
                return;
            }

            if (fallbackSpriteRenderer == null)
            {
                return;
            }

            if (!hasOriginalSpriteColor)
            {
                originalSpriteColor = fallbackSpriteRenderer.color;
                hasOriginalSpriteColor = true;
            }

            fallbackSpriteRenderer.color = Color.Lerp(originalSpriteColor, pulseColor, Mathf.Clamp01(alpha));
        }

        private void ResolveTargets()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<Renderer>();
            }

            if (fallbackSpriteRenderer == null)
            {
                fallbackSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (!hasOriginalSpriteColor && fallbackSpriteRenderer != null)
            {
                originalSpriteColor = fallbackSpriteRenderer.color;
                hasOriginalSpriteColor = true;
            }
        }

        private void CacheShaderSupport()
        {
            supportsOutlineAlpha = false;
            supportsOutlineThickness = false;
            supportsOutlineWidth = false;
            supportsOutlineColor = false;

            Material material = targetRenderer != null ? targetRenderer.sharedMaterial : null;
            if (material == null)
            {
                return;
            }

            supportsOutlineAlpha = material.HasProperty(OutlineAlphaId);
            supportsOutlineThickness = material.HasProperty(OutlineThicknessId);
            supportsOutlineWidth = material.HasProperty(OutlineWidthId);
            supportsOutlineColor = material.HasProperty(OutlineColorId);
        }
    }
}
