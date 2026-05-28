using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Live
{
    /// <summary>
    /// C4 — RuntimeAssetRegistry (F2).
    /// Baked ScriptableObject that maps GUID strings to assets (Sprite / TileBase / GameObject).
    /// Loaded from Resources at runtime by both Tool.exe and Game.exe.
    /// Populated by RuntimeAssetRegistryBaker (C3, Editor-only).
    /// API frozen for C6/C9/C10 downstream dependencies.
    /// </summary>
    [CreateAssetMenu(fileName = "RuntimeAssetRegistry", menuName = "RIMA/Live Tool/Runtime Asset Registry")]
    public sealed class RuntimeAssetRegistry : ScriptableObject
    {
        // ── Data ───────────────────────────────────────────────────────────────

        [SerializeField] private List<RegistryEntry> _entries = new List<RegistryEntry>();

        // ── Runtime lookup tables (built on first access) ──────────────────────

        private Dictionary<string, RegistryEntry> _byGuid;
        private Dictionary<string, List<RegistryEntry>> _byTag;
        private bool _initialized;

        // ── Frozen public API ──────────────────────────────────────────────────

        /// <summary>Total entry count (useful for validation / unit tests).</summary>
        public int Count => _entries.Count;

        /// <summary>All entries (read-only view for palette population in C6).</summary>
        public IReadOnlyList<RegistryEntry> Entries => _entries;

        /// <summary>
        /// Resolve an entry by its AssetDatabase GUID string.
        /// Returns null if the GUID is not registered.
        /// O(1) after first call.
        /// </summary>
        public RegistryEntry Get(string guid)
        {
            EnsureInitialized();
            if (string.IsNullOrEmpty(guid)) return null;
            _byGuid.TryGetValue(guid, out RegistryEntry entry);
            return entry;
        }

        /// <summary>
        /// Convenience: resolve the Sprite for a GUID. Returns null if missing.
        /// Used by C6 (RuntimeBrushPalette) thumbnail rendering.
        /// </summary>
        public Sprite GetSprite(string guid)
        {
            RegistryEntry e = Get(guid);
            return e?.sprite;
        }

        /// <summary>
        /// Convenience: resolve the TileBase for a GUID. Returns null if missing.
        /// Used by C10 (LiveRoomReloader) floor/cliff tile instantiation.
        /// </summary>
        public TileBase GetTile(string guid)
        {
            RegistryEntry e = Get(guid);
            return e?.tile;
        }

        /// <summary>
        /// Convenience: resolve the prefab GameObject for a GUID. Returns null if missing.
        /// Used by C9 (RuntimeAssetLoader) + C10 (LiveRoomReloader) prop instantiation.
        /// </summary>
        public GameObject GetPrefab(string guid)
        {
            RegistryEntry e = Get(guid);
            return e?.prefab;
        }

        /// <summary>
        /// Filter entries by a freeform tag (e.g. "floor", "cliff", "wall").
        /// Returns empty list if no entries match — never null.
        /// Used by C6 palette filtering.
        /// </summary>
        public IReadOnlyList<RegistryEntry> GetByTag(string tag)
        {
            EnsureInitialized();
            if (string.IsNullOrEmpty(tag)) return _entries;
            if (_byTag.TryGetValue(tag.ToLowerInvariant(), out List<RegistryEntry> list)) return list;
            return System.Array.Empty<RegistryEntry>();
        }

        /// <summary>
        /// Filter entries by RoomLayer enum.
        /// Used by C6 palette layer-filter UI.
        /// </summary>
        public List<RegistryEntry> GetByLayer(RoomPainter.RoomLayer layer)
        {
            EnsureInitialized();
            List<RegistryEntry> result = new List<RegistryEntry>();
            foreach (RegistryEntry e in _entries)
                if (e.layer == layer) result.Add(e);
            return result;
        }

        /// <summary>
        /// Check whether a GUID is registered (O(1) after first call).
        /// Used by F2 bake verification + C10 graceful-degrade path.
        /// </summary>
        public bool Contains(string guid)
        {
            EnsureInitialized();
            return !string.IsNullOrEmpty(guid) && _byGuid.ContainsKey(guid);
        }

        // ── Resources loader ───────────────────────────────────────────────────

        private static RuntimeAssetRegistry _instance;

        /// <summary>
        /// Singleton accessor: loads from Resources/Live/RuntimeAssetRegistry.
        /// Suitable for Tool.exe + Game.exe startup. Editor code should use AssetDatabase instead.
        /// Returns null if the asset has not been baked yet.
        /// </summary>
        public static RuntimeAssetRegistry Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Resources.Load<RuntimeAssetRegistry>("Live/RuntimeAssetRegistry");
                if (_instance != null) _instance.EnsureInitialized();
                return _instance;
            }
        }

        // ── Editor-only mutation (C3 baker calls these) ────────────────────────
        // Guarded so they compile-out in non-Editor builds via the Baker being in Editor/ only.

        /// <summary>
        /// Replace all entries. Called by RuntimeAssetRegistryBaker after scan.
        /// Do NOT call at runtime — Editor baker only.
        /// </summary>
        public void SetEntries(List<RegistryEntry> entries)
        {
            _entries = entries ?? new List<RegistryEntry>();
            _initialized = false; // force rebuild
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private void OnEnable()
        {
            // Reset on domain reload so dictionaries are rebuilt fresh.
            _initialized = false;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            _byGuid = new Dictionary<string, RegistryEntry>(_entries.Count);
            _byTag  = new Dictionary<string, List<RegistryEntry>>();

            foreach (RegistryEntry e in _entries)
            {
                if (!string.IsNullOrEmpty(e.guid))
                    _byGuid[e.guid] = e;

                if (!string.IsNullOrEmpty(e.tag))
                {
                    string key = e.tag.ToLowerInvariant();
                    if (!_byTag.TryGetValue(key, out List<RegistryEntry> bucket))
                    {
                        bucket = new List<RegistryEntry>();
                        _byTag[key] = bucket;
                    }
                    bucket.Add(e);
                }
            }

            _initialized = true;
        }
    }

    // ── Entry struct ───────────────────────────────────────────────────────────

    [System.Serializable]
    public sealed class RegistryEntry
    {
        /// <summary>AssetDatabase GUID (from .meta file). Primary key — never changes.</summary>
        public string guid;

        /// <summary>Human-readable asset name (filename without extension).</summary>
        public string displayName;

        /// <summary>
        /// Freeform tag derived from keyword-matching (same logic as RoomPainterPhysicsRules).
        /// Examples: "floor", "cliff", "wall", "prop", "parallax".
        /// </summary>
        public string tag;

        /// <summary>Sorting layer / painting layer for palette filter.</summary>
        public RoomPainter.RoomLayer layer;

        /// <summary>Sprite reference (may be null for prefab-only entries).</summary>
        public Sprite sprite;

        /// <summary>TileBase reference (floor/cliff tiles; may be null for prop entries).</summary>
        public TileBase tile;

        /// <summary>Prefab reference (prop/wall prefabs; may be null for tile-only entries).</summary>
        public GameObject prefab;

        /// <summary>Asset type classification — drives icon and palette slot.</summary>
        public AssetKind kind;
    }

    /// <summary>Classifies the primary runtime role of a registered asset.</summary>
    public enum AssetKind
    {
        Tile,
        Sprite,
        Prefab,
        TileAndSprite,
    }
}
