using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.Environment
{
    /// <summary>
    /// F2: Procedural cliff drop-shadow placer.
    /// Hooks into CliffAutoPlacer.Regenerate via [ContextMenu] + OnEnable auto-sync.
    /// Mirrors every cliff cell into CliffDropShadowTilemap using a procedural
    /// alpha-gradient sprite tile (CliffDropShadowGenerator).
    /// Sorting: Decor_Cliff layer, order -20 (behind cliff base at -1).
    /// Pattern: DecorCliffPainter.cs / CliffAutoPlacer.cs approach.
    /// </summary>
    [ExecuteAlways]
    public sealed class CliffDropShadowPlacer : MonoBehaviour
    {
        [Header("Required refs")]
        public CliffAutoPlacer cliffAutoPlacer;
        public Tilemap         shadowTilemap;

        [Header("Runtime tile (auto-created)")]
        [SerializeField, HideInInspector]
        private Sprite _shadowSprite;

        // ------------------------------------------------------------------
        // Unity lifecycle
        // ------------------------------------------------------------------

        private void OnEnable()
        {
            Regenerate();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Defer so scene is fully loaded; avoids SendMessage warnings.
            EditorApplication.delayCall += () =>
            {
                if (this != null) Regenerate();
            };
        }
#endif

        // ------------------------------------------------------------------
        // Public API — call after CliffAutoPlacer.Regenerate()
        // ------------------------------------------------------------------

        [ContextMenu("Regenerate Shadow Tilemap")]
        public void Regenerate()
        {
            if (!IsReady()) return;

            shadowTilemap.ClearAllTiles();
            CliffDropShadowGenerator.InvalidateCache();
            _shadowSprite = CliffDropShadowGenerator.GetSprite();

            // Build a temporary runtime tile that wraps the procedural sprite.
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite    = _shadowSprite;
            tile.color     = Color.white;
            tile.colliderType = Tile.ColliderType.None;
            tile.hideFlags = HideFlags.DontSave;

            // Mirror each cliff cell into the shadow tilemap.
            Tilemap cliffTm = cliffAutoPlacer.cliffTilemap;
            if (cliffTm == null) return;

            foreach (Vector3Int cell in cliffTm.cellBounds.allPositionsWithin)
            {
                if (!cliffTm.HasTile(cell)) continue;
                shadowTilemap.SetTile(cell, tile);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(shadowTilemap.gameObject);
            }
#endif
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        private bool IsReady()
        {
            if (cliffAutoPlacer == null)
            {
                Debug.LogWarning("[CliffDropShadowPlacer] cliffAutoPlacer is not assigned.", this);
                return false;
            }
            if (shadowTilemap == null)
            {
                Debug.LogWarning("[CliffDropShadowPlacer] shadowTilemap is not assigned.", this);
                return false;
            }
            return true;
        }
    }
}
