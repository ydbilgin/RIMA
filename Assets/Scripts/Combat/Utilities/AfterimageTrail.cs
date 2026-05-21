using System;
using System.Collections;
using UnityEngine;

namespace RIMA.Combat
{
    /// <summary>
    /// Captures and fades sprite copies behind dash or teleport movement.
    /// </summary>
    public class AfterimageTrail : MonoBehaviour
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        public void SpawnTrail(Transform source, AfterimageConfig config)
        {
            if (source == null || config == null || config.afterimageCount <= 0)
            {
                return;
            }

            StartCoroutine(SpawnTrailRoutine(source, config));
        }

        private static IEnumerator SpawnTrailRoutine(Transform source, AfterimageConfig config)
        {
            for (int i = 0; i < config.afterimageCount; i++)
            {
                if (source == null)
                {
                    yield break;
                }

                SpriteRenderer sourceRenderer = source.GetComponentInChildren<SpriteRenderer>();
                if (sourceRenderer != null && sourceRenderer.sprite != null)
                {
                    SpawnAfterimage(sourceRenderer, config);
                }

                if (i < config.afterimageCount - 1 && config.intervalSeconds > 0f)
                {
                    yield return new WaitForSeconds(config.intervalSeconds);
                }
            }
        }

        private static void SpawnAfterimage(SpriteRenderer sourceRenderer, AfterimageConfig config)
        {
            GameObject go = new GameObject("Afterimage");
            go.transform.SetPositionAndRotation(sourceRenderer.transform.position, sourceRenderer.transform.rotation);
            go.transform.localScale = sourceRenderer.transform.lossyScale;

            SpriteRenderer copy = go.AddComponent<SpriteRenderer>();
            copy.sprite = sourceRenderer.sprite;
            copy.flipX = sourceRenderer.flipX;
            copy.flipY = sourceRenderer.flipY;
            copy.sortingLayerID = sourceRenderer.sortingLayerID;
            copy.sortingOrder = sourceRenderer.sortingOrder - 1;

            Material material = config.afterimageMaterial != null
                ? new Material(config.afterimageMaterial)
                : new Material(Shader.Find("Sprites/Default"));
            copy.material = material;
            copy.color = config.tintColor;

            AfterimageFader fader = go.AddComponent<AfterimageFader>();
            fader.Initialize(copy, material, config.tintColor, Mathf.Max(0.01f, config.fadeDuration));
        }
    }

    /// <summary>
    /// Runtime configuration for afterimage copy count, timing, and fade style.
    /// </summary>
    [Serializable]
    public class AfterimageConfig
    {
        public int afterimageCount = 5;
        public float intervalSeconds = 0.05f;
        public float fadeDuration = 0.4f;
        public Color tintColor = new Color(0.5f, 0.5f, 1f, 0.5f);
        public Material afterimageMaterial;
    }

    /// <summary>
    /// Fades and destroys one spawned afterimage sprite copy.
    /// </summary>
    internal class AfterimageFader : MonoBehaviour
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private SpriteRenderer spriteRenderer;
        private Material material;
        private Color startColor;
        private float fadeDuration;
        private float startTime;

        public void Initialize(SpriteRenderer spriteRenderer, Material material, Color startColor, float fadeDuration)
        {
            this.spriteRenderer = spriteRenderer;
            this.material = material;
            this.startColor = startColor;
            this.fadeDuration = fadeDuration;
            startTime = Time.time;
        }

        private void Update()
        {
            float t = Mathf.Clamp01((Time.time - startTime) / fadeDuration);
            Color color = startColor;
            color.a *= 1f - t;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }

            if (material != null && material.HasProperty(ColorId))
            {
                material.SetColor(ColorId, color);
            }

            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (material != null)
            {
                Destroy(material);
            }
        }
    }
}
