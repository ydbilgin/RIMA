using UnityEngine;

namespace RIMA.Walls.V2
{
    [ExecuteAlways]
    public class WallPiece : MonoBehaviour
    {
        public WallPieceData data;

        [Header("Refs (auto-wired in prefab)")]
        public SpriteRenderer visual;
        public BoxCollider2D footprintCollider;
        public Transform footprintAnchor;
        public Transform leftSocket;
        public Transform rightSocket;
        public Transform seamSocketLeft;
        public Transform seamSocketRight;

        public void Initialize(WallPieceData d)
        {
            data = d;
            if (data == null) return;
            ApplyMetadata();
        }

        public void ApplyMetadata()
        {
            if (footprintCollider != null)
            {
                footprintCollider.size = data.colliderSize;
                footprintCollider.offset = data.colliderOffset;
            }
            if (visual != null)
            {
                visual.color = data.placeholderColor;
                if (data.spriteRef != null && visual != null)
                {
                    visual.sprite = data.spriteRef;
                    visual.color = Color.white;
                }
                Vector3 scale = new Vector3(data.footprintSize.x, data.footprintSize.y, 1f);
                if (visual.sprite != null)
                {
                    Vector3 size = visual.sprite.bounds.size;
                    if (size.x > 0.0001f) scale.x = data.footprintSize.x / size.x;
                    if (size.y > 0.0001f) scale.y = data.footprintSize.y / size.y;
                }
                visual.transform.localScale = scale;
                visual.transform.localPosition = new Vector3(data.anchorOffset.x, data.anchorOffset.y, 0f);
            }
            if (leftSocket != null) leftSocket.localPosition = data.leftSocketLocal;
            if (rightSocket != null) rightSocket.localPosition = data.rightSocketLocal;
            if (seamSocketLeft != null) seamSocketLeft.localPosition = data.seamSocketLeftLocal;
            if (seamSocketRight != null) seamSocketRight.localPosition = data.seamSocketRightLocal;
        }

        void OnDrawGizmosSelected()
        {
            if (data == null) return;
            var pos = transform.position;
            // Footprint outline (yellow)
            Gizmos.color = Color.yellow;
            Vector3 size = new Vector3(data.footprintSize.x, data.footprintSize.y, 0.01f);
            Gizmos.DrawWireCube(pos + new Vector3(data.anchorOffset.x, data.anchorOffset.y, 0), size);

            // Collider bounds (green)
            Gizmos.color = Color.green;
            Vector3 cSize = new Vector3(data.colliderSize.x, data.colliderSize.y, 0.01f);
            Gizmos.DrawWireCube(pos + new Vector3(data.colliderOffset.x, data.colliderOffset.y, 0), cSize);

            // Sockets (cyan dots)
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pos + new Vector3(data.leftSocketLocal.x, data.leftSocketLocal.y, 0), 0.06f);
            Gizmos.DrawSphere(pos + new Vector3(data.rightSocketLocal.x, data.rightSocketLocal.y, 0), 0.06f);
            // Seam sockets (magenta dots)
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(pos + new Vector3(data.seamSocketLeftLocal.x, data.seamSocketLeftLocal.y, 0), 0.05f);
            Gizmos.DrawSphere(pos + new Vector3(data.seamSocketRightLocal.x, data.seamSocketRightLocal.y, 0), 0.05f);
        }
    }
}
