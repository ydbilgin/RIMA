using UnityEngine;

namespace LaurethStudio.PainterSuite.Runtime
{
    /// <summary>
    /// Origin-based parallax for a single background sprite.
    /// Attach to each child SpriteRenderer of a parallax rig root.
    /// </summary>
    /// <remarks>
    /// Logic extracted from RIMA project (Assets/Scripts/Background/ParallaxLayer.cs)
    /// under namespace rename only. Sort-axis aware via Z preservation.
    /// Pixel snap protects against sub-pixel shimmer at low PPU.
    /// </remarks>
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public sealed class ParallaxLayer : MonoBehaviour
    {
        [Tooltip("Camera the layer follows. Auto-resolved to Camera.main if null.")]
        public Camera target;

        [Tooltip("How much the layer moves relative to the camera. 1=glued to camera, 0=world-static. " +
                 "Typical depth presets: void (0.03, 0.02), nebula (0.05, 0.04), " +
                 "ruins (0.08, 0.05), islands (0.14, 0.08), fog (0.10, 0.06).")]
        public Vector2 factor = new Vector2(0.1f, 0.05f);

        [Tooltip("Snap output position to pixel grid (avoids sub-pixel shimmer at low PPU).")]
        public bool snapToPixel = true;

        [Tooltip("Pixels per unit used for the snap (match Pixel Perfect Camera AssetsPPU).")]
        public int pixelsPerUnit = 64;

        private Vector3 _layerStart;
        private Vector3 _cameraStart;
        private bool _captured;

        private void OnEnable()
        {
            Capture();
        }

        private void Capture()
        {
            if (target == null) target = Camera.main;
            _layerStart = transform.position;
            _cameraStart = target != null ? target.transform.position : Vector3.zero;
            _captured = target != null;
        }

        private void LateUpdate()
        {
            if (target == null) target = Camera.main;
            if (target == null) return;
            if (!_captured) Capture();
            if (!_captured) return;

            Vector3 delta = target.transform.position - _cameraStart;
            Vector3 next = _layerStart + new Vector3(delta.x * factor.x, delta.y * factor.y, 0f);

            if (snapToPixel && pixelsPerUnit > 0)
            {
                float p = pixelsPerUnit;
                next.x = Mathf.Round(next.x * p) / p;
                next.y = Mathf.Round(next.y * p) / p;
            }

            // Preserve original Z (sorting hint, parallax should never alter it)
            next.z = _layerStart.z;
            transform.position = next;
        }

        /// <summary>
        /// Re-capture origin from current transform + camera. Call after manually
        /// moving the rig or camera in the Editor outside Play mode.
        /// </summary>
        [ContextMenu("Recapture origin")]
        public void RecaptureOrigin() => Capture();
    }
}
