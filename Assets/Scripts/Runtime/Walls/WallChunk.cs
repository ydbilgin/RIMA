using UnityEngine;

namespace RIMA.Walls
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WallChunk : MonoBehaviour
    {
        [SerializeField] private WallChunkData data;
        [SerializeField] private SpriteRenderer visualRenderer;
        [SerializeField] private BoxCollider2D footprintCollider;

        [Header("Socket Anchors (auto-populated)")]
        [SerializeField] private Transform footprintAnchor;
        [SerializeField] private Transform leftSocket;
        [SerializeField] private Transform rightSocket;
        [SerializeField] private Transform torchSocket;
        [SerializeField] private Transform bannerSocket;
        [SerializeField] private Transform seamSocket;
        [SerializeField] private Transform seamSocketLeft;
        [SerializeField] private Transform seamSocketRight;
        [SerializeField] private Transform optionalPropSocket;

        public WallChunkData Data => data;
        public Transform FootprintAnchor => footprintAnchor;

        private void OnValidate()
        {
            if (data == null)
            {
                return;
            }

            ApplyData();
        }

        public void Initialize(WallChunkData chunkData)
        {
            data = chunkData;
            ApplyData();
        }

        public void ApplyData()
        {
            if (visualRenderer == null)
            {
                visualRenderer = GetComponent<SpriteRenderer>();
            }

            visualRenderer.sprite = data.visual;

            if (footprintCollider == null)
            {
                footprintCollider = GetComponent<BoxCollider2D>();
            }

            if (footprintCollider != null)
            {
                footprintCollider.size = data.colliderSize;
                footprintCollider.offset = data.colliderOffset;
            }

            ApplySockets();
        }

        private void ApplySockets()
        {
            foreach (SocketDef socket in data.sockets)
            {
                Transform target = socket.socketName.ToLowerInvariant() switch
                {
                    "torch" => torchSocket,
                    "banner" => bannerSocket,
                    "left" => leftSocket,
                    "right" => rightSocket,
                    "seam" => seamSocket,
                    "seam_left" => seamSocketLeft != null ? seamSocketLeft : seamSocket,
                    "seam_right" => seamSocketRight != null ? seamSocketRight : seamSocket,
                    "prop" => optionalPropSocket,
                    _ => null
                };

                if (target != null)
                {
                    target.localPosition = socket.localPosition;
                }
            }
        }
    }
}
