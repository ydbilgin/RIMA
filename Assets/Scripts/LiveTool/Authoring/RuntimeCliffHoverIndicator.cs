// TARGET: Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs
// ============================================================================
// C8 — RuntimeCliffHoverIndicator (RIMA Live Editor T3 Standalone Tool)
// ----------------------------------------------------------------------------
// Runtime replacement for the SceneView-only CliffHoverIndicator.cs.
// A single SpriteRenderer GameObject that snaps to the cursor's grid cell and
// shows the cliff/brush sprite that WOULD be placed there. Updated every frame
// from previewCamera + cursor position.
//
// Constraint (contract §C8): ZERO UnityEditor / Handles / Gizmos. Pure
// SpriteRenderer + Transform. This is what makes it runtime-legal vs the
// SceneView original, so it can ship inside Tool.exe.
//
// Compile boundary (contract §1, §2 LiveToolBuildProcessor): the whole
// RIMA.LiveTool assembly is gated behind RIMA_LIVE_TOOL via defineConstraints,
// so this #if is belt-and-suspenders — it keeps the file inert if it is ever
// pulled into an assembly that does NOT carry the define.
// ============================================================================
#if RIMA_LIVE_TOOL

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Mouse.current — contract §8 (no legacy Input.Get*)
using RIMA.Live;               // RuntimeAssetRegistry, RegistryEntry

namespace RIMA.LiveTool
{
    /// <summary>
    /// C8 — Camera + cursor driven ghost preview.
    ///
    /// Owns one child SpriteRenderer GameObject that:
    ///   • snaps to the integer grid cell under the cursor
    ///     (Mathf.RoundToInt(worldPos.x/.y) — parity with the BrushExecutorRouter
    ///      snap math and the LiveRoomReloader cell math),
    ///   • shows the sprite of the currently selected variant (cliff or any brush
    ///     entry) as a semi-transparent cyan ghost
    ///     (mirror of VisualEditorScenePainter.UpdateGhostObject line 223:
    ///      new Color(0.6f, 0.9f, 1f, 0.6f)),
    ///   • renders above world content at the cliff-face sorting band (-10, per
    ///     project_3kit_bg_architecture_lock / spec §0.5 L3 6-layer stack).
    ///
    /// Driven by ToolBootstrap (C5): Initialize once, SetActive(mode == Cliff),
    /// SetVariant when the palette selection changes. CurrentCell is read back by
    /// the bootstrap to feed BrushExecutorRouter.Paint.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RuntimeCliffHoverIndicator : MonoBehaviour
    {
        // ── Tunables (contract-locked values) ────────────────────────────────
        // Cliff face sorting band (spec §0.5 L3; project_3kit_bg_architecture_lock).
        private const int CliffFaceSortingOrder = -10;
        // Ghost tint — verbatim parity with VisualEditorScenePainter.cs:223.
        private static readonly Color GhostTint = new Color(0.6f, 0.9f, 1f, 0.6f);
        // Distance from camera at which to place the ghost when using an ortho cam
        // (ScreenToWorldPoint needs a z; for ortho the value is irrelevant to x/y
        //  but we keep it positive-in-front for safety on a perspective fallback).
        private const float GhostDepth = 10f;

        // ── Injected dependencies ────────────────────────────────────────────
        private Camera _previewCamera;
        private RuntimeAssetRegistry _registry;

        // ── Owned view object ────────────────────────────────────────────────
        private GameObject _ghostObject;
        private SpriteRenderer _ghostRenderer;
        private bool _active;

        // ── Variant cache ────────────────────────────────────────────────────
        // Pre-loaded set of all cliff variants (spec §4.1 R10 "Pre-load all 8
        // cliff variants") so SetVariant / cycling never hits a cold Resources
        // path mid-drag. Indexed for optional cycling by the bootstrap.
        private readonly List<RegistryEntry> _cliffVariants = new List<RegistryEntry>();
        private RegistryEntry _currentVariant;

        // ── Public state ─────────────────────────────────────────────────────
        /// <summary>Snapped integer cell currently under the cursor.</summary>
        public Vector3Int CurrentCell { get; private set; }

        /// <summary>All pre-loaded cliff variants (read-only, for bootstrap cycling).</summary>
        public IReadOnlyList<RegistryEntry> CliffVariants => _cliffVariants;

        /// <summary>The variant currently shown as the ghost (may be null).</summary>
        public RegistryEntry CurrentVariant => _currentVariant;

        // ── Lifecycle / API ──────────────────────────────────────────────────

        /// <summary>
        /// One-time setup. Creates the ghost SpriteRenderer child and pre-loads
        /// every cliff variant from the registry (registry.GetByTag("cliff")).
        /// Safe to call again to re-inject a different camera/registry.
        /// </summary>
        public void Initialize(Camera previewCamera, RuntimeAssetRegistry registry)
        {
            _previewCamera = previewCamera;
            _registry = registry;

            EnsureGhostObject();
            PreloadCliffVariants();

            // Default-hidden until the bootstrap flips into Cliff mode.
            SetActive(false);
        }

        /// <summary>
        /// Toggle visibility. ToolBootstrap calls SetActive(CurrentMode == Cliff).
        /// While inactive the ghost object is disabled (no per-frame work matters
        /// either way — Update early-returns).
        /// </summary>
        public void SetActive(bool on)
        {
            _active = on;
            if (_ghostObject != null)
                _ghostObject.SetActive(on && _ghostRenderer != null && _ghostRenderer.sprite != null);
        }

        /// <summary>
        /// Set the sprite shown at the cursor. Pass the palette's currently
        /// selected RegistryEntry (cliff or any brush). Null clears/hides the ghost.
        /// </summary>
        public void SetVariant(RegistryEntry cliffEntry)
        {
            _currentVariant = cliffEntry;

            if (_ghostRenderer == null)
                EnsureGhostObject();

            if (_ghostRenderer == null)
                return;

            Sprite sprite = cliffEntry != null ? cliffEntry.sprite : null;
            _ghostRenderer.sprite = sprite;

            // If active but the new variant has no sprite, hide; otherwise reflect _active.
            _ghostObject.SetActive(_active && sprite != null);
        }

        /// <summary>
        /// Per-frame: read the cursor, project through the preview camera, snap to
        /// the grid cell, and position the ghost SpriteRenderer. Updates CurrentCell.
        /// </summary>
        private void Update()
        {
            if (!_active || _ghostObject == null || _ghostRenderer == null)
                return;
            if (_previewCamera == null || _ghostRenderer.sprite == null)
                return;

            // contract §C8: previewCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue())
            Mouse mouse = Mouse.current;
            if (mouse == null)
                return; // no pointer device this frame

            Vector2 screen = mouse.position.ReadValue();
            Vector3 screenPoint = new Vector3(screen.x, screen.y, GhostDepth);
            Vector3 w = _previewCamera.ScreenToWorldPoint(screenPoint);

            // contract §C8 snap: new Vector3Int(RoundToInt(w.x), RoundToInt(w.y), 0)
            Vector3Int cell = new Vector3Int(
                Mathf.RoundToInt(w.x),
                Mathf.RoundToInt(w.y),
                0);
            CurrentCell = cell;

            // Place the ghost at the cell center on the world plane (z = 0 so it
            // sorts purely by sortingOrder, matching tilemap/prop placement).
            _ghostObject.transform.position = new Vector3(cell.x, cell.y, 0f);
        }

        private void OnDisable()
        {
            // Hide the ghost if the indicator component is disabled out from under us.
            if (_ghostObject != null)
                _ghostObject.SetActive(false);
        }

        // ── Internals ────────────────────────────────────────────────────────

        private void EnsureGhostObject()
        {
            if (_ghostObject != null && _ghostRenderer != null)
                return;

            _ghostObject = new GameObject("CliffHoverGhost");
            _ghostObject.transform.SetParent(transform, worldPositionStays: false);
            _ghostObject.transform.localPosition = Vector3.zero;
            // Tool-only scratch object: do not persist into any saved scene/asset.
            _ghostObject.hideFlags = HideFlags.DontSave;

            _ghostRenderer = _ghostObject.AddComponent<SpriteRenderer>();
            _ghostRenderer.color = GhostTint;                 // semi-transparent cyan ghost
            _ghostRenderer.sortingOrder = CliffFaceSortingOrder; // cliff-face band, spec §0.5 L3
            _ghostObject.SetActive(false);
        }

        private void PreloadCliffVariants()
        {
            _cliffVariants.Clear();
            if (_registry == null)
                return;

            // contract §C8: registry.GetByTag("cliff") (IReadOnlyList<RegistryEntry>).
            // RuntimeAssetRegistry.GetByTag never returns null (returns Array.Empty
            // when no match) — RuntimeAssetRegistry.cs:83-89.
            IReadOnlyList<RegistryEntry> cliffs = _registry.GetByTag("cliff");
            for (int i = 0; i < cliffs.Count; i++)
            {
                RegistryEntry e = cliffs[i];
                if (e != null && e.sprite != null)
                    _cliffVariants.Add(e);
            }

            // If nothing has been chosen yet, default the ghost to the first cliff
            // variant so Cliff mode shows something on first hover.
            if (_currentVariant == null && _cliffVariants.Count > 0)
                SetVariant(_cliffVariants[0]);
        }
    }
}

#endif // RIMA_LIVE_TOOL
