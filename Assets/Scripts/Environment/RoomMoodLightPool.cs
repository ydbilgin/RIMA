using UnityEngine;

namespace RIMA.Environment
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoomMoodLightPool : MonoBehaviour
    {
        [SerializeField] private Color color = new Color(1f, 0.45f, 0.18f, 1f);
        [SerializeField, Range(0.5f, 16f)] private float radius = 5f;
        [SerializeField, Range(0.02f, 0.6f)] private float alpha = 0.18f;
        [SerializeField] private string sortingLayerName = "VFX";
        [SerializeField] private int sortingOrder = -100;

        private static Sprite radialSprite;
        private SpriteRenderer spriteRenderer;

        public void Configure(Color poolColor, float poolRadius, float poolAlpha, string layerName = "VFX", int order = -100)
        {
            color = poolColor;
            radius = poolRadius;
            alpha = poolAlpha;
            sortingLayerName = layerName;
            sortingOrder = order;
            Apply();
        }

        private void OnEnable()
        {
            Apply();
        }

        private void OnValidate()
        {
            Apply();
        }

        private void Apply()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            spriteRenderer.sprite = GetRadialSprite();
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortingOrder;
            transform.localScale = new Vector3(radius, radius, 1f);
        }

        private static Sprite GetRadialSprite()
        {
            if (radialSprite != null) return radialSprite;

            const int size = 96;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Vector2 center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            float maxDistance = size * 0.5f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float t = Mathf.Clamp01(Vector2.Distance(new Vector2(x, y), center) / maxDistance);
                    float a = Mathf.Pow(1f - t, 1.8f);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }

            texture.Apply();
            radialSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size * 0.5f);
            radialSprite.name = "RIMA_Room_Mood_Light_Pool";
            return radialSprite;
        }
    }
}
