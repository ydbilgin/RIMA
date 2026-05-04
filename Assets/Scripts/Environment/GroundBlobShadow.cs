using UnityEngine;

namespace RIMA.Environment
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class GroundBlobShadow : MonoBehaviour
    {
        [SerializeField] private Color color = new Color(0f, 0f, 0f, 0.32f);
        [SerializeField] private Vector2 size = new Vector2(0.95f, 0.34f);
        [SerializeField] private Vector3 localOffset = new Vector3(0f, -0.18f, 0f);
        [SerializeField] private string sortingLayerName = "Ground";
        [SerializeField] private int sortingOrder = 120;

        private static Sprite blobSprite;
        private SpriteRenderer spriteRenderer;

        public static GroundBlobShadow Ensure(Transform owner, Vector2 shadowSize, float alpha = 0.32f)
        {
            if (owner == null) return null;

            const string shadowName = "GroundBlobShadow";
            Transform existing = owner.Find(shadowName);
            GameObject shadowObject = existing != null
                ? existing.gameObject
                : new GameObject(shadowName);

            if (existing == null)
                shadowObject.transform.SetParent(owner, false);

            var shadow = shadowObject.GetComponent<GroundBlobShadow>();
            if (shadow == null) shadow = shadowObject.AddComponent<GroundBlobShadow>();
            shadow.Configure(shadowSize, alpha);
            return shadow;
        }

        public void Configure(Vector2 shadowSize, float alpha)
        {
            size = shadowSize;
            color = new Color(0f, 0f, 0f, alpha);
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

            spriteRenderer.sprite = GetBlobSprite();
            spriteRenderer.color = color;
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortingOrder;
            transform.localPosition = localOffset;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(size.x, size.y, 1f);
        }

        private static Sprite GetBlobSprite()
        {
            if (blobSprite != null) return blobSprite;

            const int sizePx = 96;
            var texture = new Texture2D(sizePx, sizePx, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Vector2 center = new Vector2((sizePx - 1) * 0.5f, (sizePx - 1) * 0.5f);
            float radius = sizePx * 0.46f;

            for (int y = 0; y < sizePx; y++)
            {
                for (int x = 0; x < sizePx; x++)
                {
                    float t = Mathf.Clamp01(Vector2.Distance(new Vector2(x, y), center) / radius);
                    float a = Mathf.Pow(1f - t, 2.2f);
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }

            texture.Apply();
            blobSprite = Sprite.Create(texture, new Rect(0, 0, sizePx, sizePx), new Vector2(0.5f, 0.5f), sizePx);
            blobSprite.name = "RIMA_Ground_Blob_Shadow";
            return blobSprite;
        }
    }
}
