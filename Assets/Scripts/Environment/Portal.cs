using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Placeholder portal. Cyan square SpriteRenderer + trigger collider.
    /// Swap visualSprite later when the rift sprite ships from PixelLab.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class Portal : MonoBehaviour
    {
        public enum DestinationType { Combat, Treasure, Ritual, BossApproach, Bridge }

        [Tooltip("Where this portal leads. Drives spawn weighting on the next room.")]
        public DestinationType destination = DestinationType.Combat;

        [Tooltip("Optional override sprite (rift art). If null a procedural cyan square is used.")]
        public Sprite visualSprite;

        [Tooltip("Tag the player GameObject uses. Trigger fires only for this tag.")]
        public string playerTag = "Player";

        public System.Action<Portal> OnEntered;

        private void Reset()
        {
            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.4f;
        }

        private void OnEnable()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (visualSprite != null)
            {
                sr.sprite = visualSprite;
                sr.color = Color.white;
            }
            else
            {
                if (sr.sprite == null) sr.sprite = BuildPlaceholderSprite();
                sr.color = new Color(0f, 1f, 0.8f, 1f); // cyan #00FFCC (matches yarık palette).
            }
            sr.sortingOrder = 5;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
            if (col.radius < 0.05f) col.radius = 0.4f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            Debug.Log($"[Portal] Entered: {destination} @ {transform.position}");
            OnEntered?.Invoke(this);
            // TODO: hook to scene transition / NextRoomLoader on a later dispatch.
        }

        // 4x4 white square - keeps allocation tiny and survives without art assets.
        private static Sprite _cachedPlaceholder;
        private static Sprite BuildPlaceholderSprite()
        {
            if (_cachedPlaceholder != null) return _cachedPlaceholder;
            const int size = 4;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "PortalPlaceholderTex",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };
            var pixels = new Color32[size * size];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(pixels);
            tex.Apply(false, true);
            _cachedPlaceholder = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            _cachedPlaceholder.name = "PortalPlaceholderSprite";
            _cachedPlaceholder.hideFlags = HideFlags.DontSave;
            return _cachedPlaceholder;
        }
    }
}
