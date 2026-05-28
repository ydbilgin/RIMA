using UnityEngine;

namespace RIMA.Background
{
    /// <summary>
    /// Origin-based parallax for a single background sprite.
    /// Attach to each child SpriteRenderer of RoomBackgroundRig.
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public sealed class ParallaxLayer : MonoBehaviour
    {
        // --- Canonical parallax-factor anchors (Sang-Hendrix-L4 authoring convenience) ---
        // The runtime math is pos = layerStart + cameraDelta * factor, so:
        //   factor 1.0 => tracks the camera 1:1 (HUD-like)      = "Stick to Camera"
        //   factor 0.0 => stays fixed in world space (no drift) = "Stick to Map"
        // Anything between is the parallax depth. Canonical authoring range is 0.05..1.10;
        // "Stick to Map" is allowed to clamp fully to 0.

        /// <summary>Factor that pins the layer to the world (no parallax drift). "Stick to Map".</summary>
        public const float StickToMapFactor = 0f;

        /// <summary>Factor that glues the layer to the camera (HUD-like). "Stick to Camera".</summary>
        public const float StickToCameraFactor = 1.0f;

        /// <summary>Lower bound of the canonical authoring range (excludes the special 0 lock).</summary>
        public const float CanonicalMinFactor = 0.05f;

        /// <summary>Upper bound of the canonical authoring range.</summary>
        public const float CanonicalMaxFactor = 1.10f;

        [Tooltip("Camera the layer follows. Auto-resolved to Camera.main if null.")]
        public Camera target;

        [Tooltip("How much the layer moves relative to the camera. 1=glued to camera, 0=world-static. " +
                 "Suggested per Codex verdict: L0 void (0.03, 0.02), L1 nebula (0.05, 0.04), " +
                 "L2 ruins (0.08, 0.05), L3 islands (0.14, 0.08), L4 fog (0.10, 0.06).")]
        public Vector2 factor = new Vector2(0.1f, 0.05f);

        [Tooltip("Snap output position to pixel grid (avoids sub-pixel shimmer at low PPU).")]
        public bool snapToPixel = true;

        [Tooltip("Pixels per unit used for the snap (match Pixel Perfect Camera AssetsPPU).")]
        public int pixelsPerUnit = 64;

        /// <summary>
        /// Editor-only virtual camera offset used by the Room Painter Preview Pan scrub.
        /// Set by <c>ParallaxSection</c> in edit mode; ignored at runtime.
        /// Reset to zero when the scrub is released.
        /// </summary>
        public static Vector2 EditorPreviewOffset;

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
            _captured = (target != null) || !Application.isPlaying;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            // Edit-mode preview: use EditorPreviewOffset as virtual camera delta.
            if (!Application.isPlaying)
            {
                ApplyEditorPreview();
                return;
            }
#endif
            ApplyRuntimeParallax();
        }

#if UNITY_EDITOR
        private void ApplyEditorPreview()
        {
            if (!_captured)
            {
                _layerStart = transform.position;
                _captured = true;
            }

            Vector2 off = EditorPreviewOffset;
            Vector3 pos = _layerStart + new Vector3(off.x * factor.x, off.y * factor.y, 0f);

            if (snapToPixel && pixelsPerUnit > 0)
            {
                float p = pixelsPerUnit;
                pos.x = Mathf.Round(pos.x * p) / p;
                pos.y = Mathf.Round(pos.y * p) / p;
            }

            pos.z = _layerStart.z;
            transform.position = pos;
        }
#endif

        private void ApplyRuntimeParallax()
        {
            if (target == null) target = Camera.main;
            if (target == null) return;
            if (!_captured) Capture();
            if (!_captured) return;

            Vector3 delta = target.transform.position - _cameraStart;
            Vector3 pos = _layerStart + new Vector3(delta.x * factor.x, delta.y * factor.y, 0f);

            if (snapToPixel && pixelsPerUnit > 0)
            {
                float p = pixelsPerUnit;
                pos.x = Mathf.Round(pos.x * p) / p;
                pos.y = Mathf.Round(pos.y * p) / p;
            }

            // Preserve original Z (sorting hint, parallax should never alter it)
            pos.z = _layerStart.z;
            transform.position = pos;
        }

        /// <summary>
        /// Re-capture origin from current transform + camera. Call after manually
        /// moving the rig or camera in the Editor outside Play mode.
        /// In edit mode, first restores transform to the stored <c>_layerStart</c>
        /// (removing any preview offset) before re-capturing, so origins do not drift.
        /// </summary>
        [ContextMenu("Recapture origin")]
        public void RecaptureOrigin()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && _captured)
            {
                // Restore to true rest position before we snapshot it.
                transform.position = _layerStart;
            }
#endif
            Capture();
        }
    }
}
