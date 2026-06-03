// TARGET: Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs
#if RIMA_LIVE_TOOL
using System.Collections.Generic;
using UnityEngine;
using RIMA.Live;
using RIMA.RoomPainter;

namespace RIMA.LiveTool
{
    /// <summary>
    /// C6 - RuntimeBrushPalette (F3).
    /// Stateful palette model: holds filtered view of registry entries + selected brush.
    /// Consumed by LiveToolPaletteWindow (C5) for rendering.
    /// No IMGUI/UI Toolkit calls here - pure data/filter logic.
    /// </summary>
    public sealed class RuntimeBrushPalette
    {
        // -- State --------------------------------------------------------------

        private RuntimeAssetRegistry _registry;

        /// <summary>Current mode filter (matches RoomPainterMode values).</summary>
        public PaletteMode ActiveMode { get; private set; } = PaletteMode.All;

        /// <summary>Active layer filter. Null means no layer filter (show all).</summary>
        public RoomLayer? LayerFilter { get; private set; } = null;

        /// <summary>Whether a layer filter is currently active.</summary>
        public bool LayerFilterActive => LayerFilter.HasValue;

        /// <summary>Free-text search filter (lowercased displayName match).</summary>
        public string SearchText { get; private set; } = string.Empty;

        /// <summary>Currently selected entry. Null when nothing is selected.</summary>
        public RegistryEntry SelectedEntry { get; private set; }

        // -- Filtered view (rebuilt on demand) ---------------------------------

        private List<RegistryEntry> _filtered = new List<RegistryEntry>();
        private bool _dirty = true;

        // -- Public API ---------------------------------------------------------

        /// <summary>
        /// Bind (or re-bind after bake) the backing registry.
        /// Passing null is valid - palette will show "bake required" state.
        /// </summary>
        public void SetRegistry(RuntimeAssetRegistry registry)
        {
            _registry = registry;
            _dirty = true;
        }

        /// <summary>Whether a registry is loaded and has entries.</summary>
        public bool HasEntries => _registry != null && _registry.Count > 0;

        /// <summary>Change the mode filter and mark filtered list dirty.</summary>
        public void SetMode(PaletteMode mode)
        {
            if (ActiveMode == mode) return;
            ActiveMode = mode;
            _dirty = true;
        }

        /// <summary>Set a layer filter to show only entries matching <paramref name="layer"/>.</summary>
        public void SetLayerFilter(RoomLayer layer)
        {
            if (LayerFilter == layer) return;
            LayerFilter = layer;
            _dirty = true;
        }

        /// <summary>Remove the layer filter so all layers are shown.</summary>
        public void ClearLayerFilter()
        {
            if (!LayerFilter.HasValue) return;
            LayerFilter = null;
            _dirty = true;
        }

        /// <summary>Change the search string and mark filtered list dirty.</summary>
        public void SetSearch(string text)
        {
            string safe = text ?? string.Empty;
            if (SearchText == safe) return;
            SearchText = safe;
            _dirty = true;
        }

        /// <summary>
        /// Select a brush entry. Pass null to clear.
        /// F4/F5 consumers read SelectedEntry to know which asset to paint.
        /// </summary>
        public void Select(RegistryEntry entry)
        {
            SelectedEntry = entry;
        }

        /// <summary>
        /// Deselect if the given entry is currently selected.
        /// Used when the selected entry disappears from the filtered view after a filter change.
        /// </summary>
        public void DeselectIfCurrent(RegistryEntry entry)
        {
            if (SelectedEntry == entry) SelectedEntry = null;
        }

        /// <summary>
        /// Returns the current filtered list, rebuilding it if needed.
        /// Always returns a non-null list (may be empty).
        /// </summary>
        public IReadOnlyList<RegistryEntry> GetFiltered()
        {
            if (_dirty) RebuildFilter();
            return _filtered;
        }

        // -- Filter logic -------------------------------------------------------

        private void RebuildFilter()
        {
            _filtered.Clear();
            _dirty = false;

            if (_registry == null) return;

            string lowerSearch = SearchText.ToLowerInvariant();
            bool hasSearch = !string.IsNullOrEmpty(lowerSearch);
            bool hasLayerFilter = LayerFilter.HasValue;

            foreach (RegistryEntry e in _registry.Entries)
            {
                // Mode filter
                if (!PassesModeFilter(e, ActiveMode)) continue;

                // Layer filter (null = no filter, any non-null value filters to that layer)
                if (hasLayerFilter && e.layer != LayerFilter.Value) continue;

                // Search filter
                if (hasSearch && !e.displayName.ToLowerInvariant().Contains(lowerSearch)) continue;

                _filtered.Add(e);
            }
        }

        private static bool PassesModeFilter(RegistryEntry e, PaletteMode mode)
        {
            switch (mode)
            {
                case PaletteMode.All:    return true;
                case PaletteMode.Tile:   return e.kind == AssetKind.Tile || e.kind == AssetKind.TileAndSprite;
                case PaletteMode.Cliff:  return e.tag == "cliff";
                case PaletteMode.Decor:  return e.layer == RoomLayer.Decals || e.tag == "prop" || e.tag == "parallax";
                case PaletteMode.Object: return e.kind == AssetKind.Prefab;
                default:                 return true;
            }
        }
    }

    /// <summary>Top-level paint mode for the palette, matching spec F3 DropdownField.</summary>
    public enum PaletteMode
    {
        All    = 0,
        Tile   = 1,
        Cliff  = 2,
        Decor  = 3,
        Object = 4,
    }
}
#endif
