#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Props;
using UnityEngine;

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// Build Mode PHASE A — the data-driven ASSET CATALOG that feeds the asset browser
    /// (design spec STAGING/LEVEL_EDITOR_UI_DESIGN_2026-06-13.md section 1; framework decision
    /// STAGING/LEVEL_EDITOR_FRAMEWORK_DECISION_2026-06-13.md "AL — demo cekirdegi" row 1).
    ///
    /// SCOPE (framework decision, ANA KARAR): RIMA-coupled for the demo — NO formal interfaces yet,
    /// but the SEAMS are kept so the post-demo package extraction (IAssetCatalog 2-layer) is a
    /// retrofit, not a rewrite:
    ///   - <see cref="AssetEntry"/> already carries the eventual contract fields (id / displayName /
    ///     icon / category / payload) so a future IAssetCatalog can wrap this 1:1.
    ///   - <see cref="AssetCategory"/> is EXTENSIBLE: adding a category = adding a builder method that
    ///     appends an <see cref="AssetCategoryGroup"/> to <see cref="Categories"/> (data, not new UI).
    ///   - The browser NEVER duplicates placement logic: an entry's <see cref="AssetEntry.payload"/>
    ///     is what the existing Phase 2 prop tool / Phase 3 tile brush consume on SELECTION.
    ///
    /// Pure data assembly: no scene/prefab edits, no gameplay logic, DisableDomainReload-safe (no
    /// statics — a fresh catalog is built per Build Mode enter and discarded on exit).
    /// </summary>
    internal sealed class BuildModeAssetCatalog
    {
        /// <summary>
        /// The four demo categories. EXTENSIBLE: a new category is a new enum value + a builder that
        /// appends a group; the tab bar + grid are generic over <see cref="AssetCategoryGroup"/> so
        /// no UI changes. Lights / Decals ship as empty (stub) groups for the demo.
        /// </summary>
        public enum AssetCategory
        {
            Props,
            Tiles,
            Lights,
            Decals
        }

        /// <summary>
        /// What the browser shows + selects. The fields ARE the eventual IAssetCatalog core contract
        /// (id / displayName / preview / category / payload), kept RIMA-typed for the demo.
        /// </summary>
        public sealed class AssetEntry
        {
            /// <summary>Stable identity (prop GUID, tile mode key, ...). Future: IAssetCatalog id.</summary>
            public string id;

            /// <summary>Human label shown on the card (Jersey10 CardName).</summary>
            public string displayName;

            /// <summary>Card thumbnail. Reuses an EXISTING game sprite — never new art (see spec sec 5).</summary>
            public Sprite icon;

            /// <summary>Which group/tab this entry lives under.</summary>
            public AssetCategory category;

            /// <summary>
            /// SEAM (framework "Custom Data slot" tohum, lighter form): the typed thing the existing
            /// tool consumes on selection. Props -> PropDefinitionSO; Tiles -> a BrushMode int boxed.
            /// The browser only ROUTES this to the right tool; it owns NO placement logic.
            /// </summary>
            public object payload;

            /// <summary>True when the asset is selectable (has a usable icon / payload).</summary>
            public bool enabled = true;
        }

        /// <summary>A category and its entries — the unit the tab bar + grid render generically.</summary>
        public sealed class AssetCategoryGroup
        {
            public AssetCategory category;
            public string label;            // tab label (uppercased by the tab bar)
            public Color dot;               // optional category accent dot (spec 2.5.1)
            public readonly List<AssetEntry> entries = new List<AssetEntry>();
            public int Count => entries.Count;
        }

        private readonly List<AssetCategoryGroup> categories = new List<AssetCategoryGroup>();

        /// <summary>All category groups, tab order. Read by the browser to build the data-driven tabs.</summary>
        public IReadOnlyList<AssetCategoryGroup> Categories => categories;

        /// <summary>
        /// Build the demo catalog from the LIVE project data. PropRegistry -> Props; the brush
        /// sub-modes -> Tiles; Lights / Decals are empty stub groups (extension points).
        /// Re-callable: clears and rebuilds (cheap; the demo catalog is small).
        /// </summary>
        public void Build(PropRegistrySO registry)
        {
            categories.Clear();
            BuildProps(registry);
            BuildTiles();
            BuildLights();
            BuildDecals();
        }

        // --- PROPS: every PropDefinitionSO in the registry, thumbnail = its existing sprite. -------
        private void BuildProps(PropRegistrySO registry)
        {
            AssetCategoryGroup group = new AssetCategoryGroup
            {
                category = AssetCategory.Props,
                label = "PROPS",
                dot = BuildModeUiStyle.Ember
            };
            categories.Add(group);

            if (registry == null) return;
            registry.RebuildIndex();
            IReadOnlyList<PropDefinitionSO> all = registry.AllProps;
            for (int i = 0; i < all.Count; i++)
            {
                PropDefinitionSO def = all[i];
                if (def == null) continue;
                Sprite icon = PropThumbnail(def);
                if (icon == null) continue;
                group.entries.Add(new AssetEntry
                {
                    id = !string.IsNullOrEmpty(def.propId) ? def.propId : def.name,
                    displayName = PropLabel(def),
                    icon = icon,
                    category = AssetCategory.Props,
                    payload = def,                 // the prop tool consumes the SO on selection.
                    enabled = true
                });
            }
        }

        // --- TILES: the brush sub-modes (FLOOR / WALKABLE / OVERLAY). The browser SELECTION drives
        // the existing Phase 3 tile brush — it does NOT duplicate any painting logic. Thumbnails are
        // null for now (procedural fallback glyph); SEAM = drop tile-mode preview sprites in later. ---
        private void BuildTiles()
        {
            AssetCategoryGroup group = new AssetCategoryGroup
            {
                category = AssetCategory.Tiles,
                label = "TILES",
                dot = BuildModeUiStyle.Ember
            };
            categories.Add(group);

            AddTile(group, "FLOOR", BuildTileBrushController.BrushMode.FloorPaint);
            AddTile(group, "WALKABLE", BuildTileBrushController.BrushMode.WalkableToggle);
            AddTile(group, "OVERLAY", BuildTileBrushController.BrushMode.OverlayPaint);
        }

        private static void AddTile(AssetCategoryGroup group, string label, BuildTileBrushController.BrushMode mode)
        {
            group.entries.Add(new AssetEntry
            {
                id = "tile_" + label.ToLowerInvariant(),
                displayName = label,
                icon = null,                        // SEAM: tile-mode preview sprites (see asset_seams).
                category = AssetCategory.Tiles,
                payload = (int)mode,                // the tile brush consumes the BrushMode on selection.
                enabled = true
            });
        }

        // --- LIGHTS: empty stub group (extension point for a later phase). -------------------------
        private void BuildLights()
        {
            categories.Add(new AssetCategoryGroup
            {
                category = AssetCategory.Lights,
                label = "LIGHTS",
                dot = BuildModeUiStyle.VoidPurple
            });
        }

        // --- DECALS: empty stub group (extension point for a later phase). -------------------------
        private void BuildDecals()
        {
            categories.Add(new AssetCategoryGroup
            {
                category = AssetCategory.Decals,
                label = "DECALS",
                dot = BuildModeUiStyle.VoidPurple
            });
        }

        // --- helpers ------------------------------------------------------------------------------

        /// <summary>Find a category group by enum (null if absent).</summary>
        public AssetCategoryGroup Group(AssetCategory category)
        {
            for (int i = 0; i < categories.Count; i++)
                if (categories[i].category == category) return categories[i];
            return null;
        }

        private static Sprite PropThumbnail(PropDefinitionSO def)
        {
            if (def == null) return null;
            if (def.icon != null) return def.icon;            // authored icon wins.
            if (def.worldSprite != null) return def.worldSprite;
            if (def.variantSprites != null && def.variantSprites.Length > 0 && def.variantSprites[0] != null)
                return def.variantSprites[0];
            return null;                                       // browser shows the procedural empty glyph.
        }

        private static string PropLabel(PropDefinitionSO def)
        {
            if (def == null) return "Prop";
            return !string.IsNullOrEmpty(def.displayName) ? def.displayName :
                (!string.IsNullOrEmpty(def.name) ? def.name : "Prop");
        }
    }
}
#endif
