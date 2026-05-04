using UnityEngine;

namespace RIMA
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerStartMarker : MonoBehaviour
    {
        [SerializeField] private bool visibleInPlay;
        [SerializeField, Range(0.25f, 2f)] private float markerScale = 0.72f;

        private const string MarkerSpritePath = "Environment/Markers/RIMA_PlayerStartMarker";
        private SpriteRenderer spriteRenderer;

        public Vector3 SpawnPosition => transform.position;

        private void Awake()
        {
            ApplyVisual();
        }

        private void OnEnable()
        {
            ApplyVisual();
        }

        private void ApplyVisual()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = RimaGeneratedSpriteCache.Load(MarkerSpritePath, 128f);
            spriteRenderer.sortingLayerName = "VFX";
            spriteRenderer.sortingOrder = -20;
            spriteRenderer.enabled = !Application.isPlaying || visibleInPlay;
            transform.localScale = new Vector3(markerScale, markerScale, 1f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.25f, 0.95f, 1f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, 0.45f);
            Gizmos.DrawLine(transform.position + Vector3.left * 0.8f, transform.position + Vector3.right * 0.8f);
            Gizmos.DrawLine(transform.position + Vector3.down * 0.8f, transform.position + Vector3.up * 0.8f);
        }
    }
}
