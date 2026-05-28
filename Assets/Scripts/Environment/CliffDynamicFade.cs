using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    [ExecuteAlways]
    [RequireComponent(typeof(Tilemap))]
    public sealed class CliffDynamicFade : MonoBehaviour
    {
        [Header("Components")]
        public Tilemap cliffTilemap;
        public Camera targetCamera;

        [Header("Settings")]
        public float minZoom = 3.0f; // Zoomed in (cliffs fully visible/white tint)
        public float maxZoom = 6.0f; // Zoomed out (cliffs faded/darkened)

        [Header("Colors")]
        [ColorUsage(true, false)]
        public Color closeColor = Color.white;
        [ColorUsage(true, false)]
        public Color farColor = new Color(0.2f, 0.2f, 0.2f, 1.0f); // 80% dimmed

        private void Awake()
        {
            if (cliffTilemap == null)
            {
                cliffTilemap = GetComponent<Tilemap>();
            }

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }
        }

        private void Update()
        {
            if (cliffTilemap == null) return;

            // In editor mode targetCamera might be null if not playing, try to find main camera
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
                if (targetCamera == null) return;
            }

            // Lerp based on camera's orthographic size (zoom level)
            float t = Mathf.InverseLerp(maxZoom, minZoom, targetCamera.orthographicSize);
            cliffTilemap.color = Color.Lerp(farColor, closeColor, t);
        }
    }
}
