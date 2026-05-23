using System.Collections;
using UnityEngine;

namespace RIMA.Enemy.Telegraph
{
    [DisallowMultipleComponent]
    public class TelegraphGroundMarker : MonoBehaviour
    {
        private const int TextureSize = 64;
        private const float PixelsPerUnit = 64f;
        private const string DefaultVfxSortingLayer = "VFX";

        [SerializeField] private SpriteRenderer markerRenderer;
        [SerializeField] private Sprite markerSprite;
        [SerializeField] private Color markerColor = new Color(1f, 0.05f, 0.02f, 0.55f);
        [SerializeField] private string sortingLayerName = DefaultVfxSortingLayer;
        [SerializeField] private int sortingOrder = 10;

        private static Sprite generatedCircleSprite;

#if UNITY_EDITOR
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearStaticCache()
        {
            if (generatedCircleSprite != null)
            {
                UnityEngine.Object.DestroyImmediate(generatedCircleSprite.texture);
                UnityEngine.Object.DestroyImmediate(generatedCircleSprite);
                generatedCircleSprite = null;
            }
        }
#endif

        private Coroutine markerRoutine;

        private void Awake()
        {
            EnsureRenderer();
            HideMarker();
        }

        private void OnDisable()
        {
            TelegraphEnd();
        }

        public void TelegraphStart(float duration, float radius)
        {
            EnsureRenderer();

            if (markerRoutine != null)
            {
                StopCoroutine(markerRoutine);
            }

            float diameter = Mathf.Max(0.01f, radius) * 2f;
            markerRenderer.transform.localScale = new Vector3(diameter, diameter, 1f);
            markerRenderer.enabled = true;
            markerRoutine = StartCoroutine(MarkerRoutine(Mathf.Max(0.01f, duration)));
        }

        public void TelegraphEnd()
        {
            if (markerRoutine != null)
            {
                StopCoroutine(markerRoutine);
                markerRoutine = null;
            }

            HideMarker();
        }

        private IEnumerator MarkerRoutine(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float pulse = 0.35f + Mathf.Sin(t * Mathf.PI) * 0.65f;
                ApplyAlpha(markerColor.a * pulse);
                elapsed += Time.deltaTime;
                yield return null;
            }

            HideMarker();
            markerRoutine = null;
        }

        private void EnsureRenderer()
        {
            if (markerRenderer == null)
            {
                Transform existing = transform.Find("TelegraphGroundMarker");
                GameObject markerObject = existing != null ? existing.gameObject : new GameObject("TelegraphGroundMarker");
                markerObject.transform.SetParent(transform, false);
                markerObject.transform.localPosition = Vector3.zero;
                markerRenderer = markerObject.GetComponent<SpriteRenderer>();
                if (markerRenderer == null)
                {
                    markerRenderer = markerObject.AddComponent<SpriteRenderer>();
                }
            }

            markerRenderer.sprite = markerSprite != null ? markerSprite : GetGeneratedCircleSprite();
            markerRenderer.color = markerColor;
            markerRenderer.sortingLayerName = sortingLayerName;
            markerRenderer.sortingOrder = sortingOrder;
        }

        private void HideMarker()
        {
            if (markerRenderer != null)
            {
                ApplyAlpha(0f);
                markerRenderer.enabled = false;
            }
        }

        private void ApplyAlpha(float alpha)
        {
            if (markerRenderer == null)
            {
                return;
            }

            Color color = markerColor;
            color.a = Mathf.Clamp01(alpha);
            markerRenderer.color = color;
        }

        private static Sprite GetGeneratedCircleSprite()
        {
            if (generatedCircleSprite != null)
            {
                return generatedCircleSprite;
            }

            Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            texture.name = "GeneratedTelegraphCircle";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            float center = (TextureSize - 1) * 0.5f;
            float radius = center;
            for (int y = 0; y < TextureSize; y++)
            {
                for (int x = 0; x < TextureSize; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    float edge = Mathf.InverseLerp(radius, radius - 2f, distance);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, Mathf.Clamp01(edge)));
                }
            }

            texture.Apply();
            generatedCircleSprite = Sprite.Create(texture, new Rect(0f, 0f, TextureSize, TextureSize), new Vector2(0.5f, 0.5f), PixelsPerUnit);
            generatedCircleSprite.name = "GeneratedTelegraphCircleSprite";
            return generatedCircleSprite;
        }
    }
}
