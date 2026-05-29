using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public class HitFlashDriver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float flashDuration = 0.08f;
        [SerializeField] private Color flashColor = Color.white;

        private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private MaterialPropertyBlock propertyBlock;
        private Coroutine flashCoroutine;
        private Color originalColor;
        private Health health;

        private enum FlashMode
        {
            None,
            FlashColor,
            ColorOverride
        }

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            propertyBlock = new MaterialPropertyBlock();

            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }

            health = GetComponentInParent<Health>() ?? GetComponent<Health>();
        }

        // Self-wire to the universal damage event so every damage path (not just
        // BasicAttack/CombatEventBus) triggers the hit-confirm white flash.
        private void OnEnable()
        {
            if (health != null) health.OnDamageTaken.AddListener(OnDamaged);
        }

        private void OnDisable()
        {
            if (health != null) health.OnDamageTaken.RemoveListener(OnDamaged);
        }

        private void OnDamaged(int amount) => Flash();

        public void Flash()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            flashCoroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            FlashMode flashMode = TrySetFlashUniform(flashColor);
            if (flashMode == FlashMode.None)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = flashColor;
            }

            yield return new WaitForSecondsRealtime(Mathf.Max(0f, flashDuration));

            if (flashMode == FlashMode.FlashColor)
            {
                SetPropertyBlockColor(FlashColorId, Color.clear);
            }
            else if (flashMode == FlashMode.ColorOverride)
            {
                SetPropertyBlockColor(ColorId, originalColor);
            }
            else if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            flashCoroutine = null;
        }

        private FlashMode TrySetFlashUniform(Color color)
        {
            if (spriteRenderer == null || spriteRenderer.sharedMaterial == null)
            {
                return FlashMode.None;
            }

            Material material = spriteRenderer.sharedMaterial;
            if (material.HasProperty(FlashColorId))
            {
                SetPropertyBlockColor(FlashColorId, color);
                return FlashMode.FlashColor;
            }

            if (material.HasProperty(ColorId))
            {
                originalColor = spriteRenderer.color;
                SetPropertyBlockColor(ColorId, color);
                return FlashMode.ColorOverride;
            }

            return FlashMode.None;
        }

        private void SetPropertyBlockColor(int propertyId, Color color)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(propertyId, color);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
