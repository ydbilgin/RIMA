// C9 — Runtime JSON model (F5).
// Pure runtime: NO AssetDatabase, NO Editor APIs. JsonUtility-compatible.
// Mirrors the schema written by RoomLayoutSerializer (Editor-side C2).
// Used by LiveRoomReloader (C10) to deserialize room_current.json at runtime.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Live
{
    // ── Top-level document ─────────────────────────────────────────────────────

    [Serializable]
    public sealed class RoomLayoutData
    {
        public string schema_version;
        public string room_id;
        public RoomLayoutMeta metadata;
        public List<FloorTileData> floor_tiles   = new List<FloorTileData>();
        public List<CliffCellData> cliff_cells   = new List<CliffCellData>();
        public List<PropData>      prop_instances = new List<PropData>();
        public List<ColliderOverrideData> collider_overrides = new List<ColliderOverrideData>();

        /// <summary>
        /// Parse JSON produced by RoomLayoutSerializer.
        /// JsonUtility requires a concrete class wrapping the list — we use a
        /// thin helper below so the public API stays clean.
        /// Returns null and logs an error on failure (graceful-degrade).
        /// </summary>
        public static RoomLayoutData FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("[RoomLayoutData] FromJson called with empty string.");
                return null;
            }
            try
            {
                RoomLayoutData data = JsonUtility.FromJson<RoomLayoutData>(json);
                if (data == null)
                    Debug.LogError("[RoomLayoutData] JsonUtility.FromJson returned null — malformed JSON?");
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[RoomLayoutData] JSON parse error: {ex.Message}");
                return null;
            }
        }
    }

    // ── Sub-models ─────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class RoomLayoutMeta
    {
        public string name;
        public string created;
        public string modified;
    }

    [Serializable]
    public sealed class FloorTileData
    {
        /// <summary>cell[0]=x, cell[1]=y, cell[2]=z</summary>
        public int[] cell;
        public string tile_guid;
    }

    [Serializable]
    public sealed class CliffCellData
    {
        /// <summary>cell[0]=x, cell[1]=y, cell[2]=z</summary>
        public int[] cell;
        public string tile_guid;
        public bool is_decor;
    }

    [Serializable]
    public sealed class PropData
    {
        public string prefab_guid;
        /// <summary>position[0]=x, [1]=y, [2]=z</summary>
        public float[] position;
        public float rotation;
        // Instance id is derived from prefab_guid + index for diffing.
        public string instance_id;
    }

    [Serializable]
    public sealed class ColliderOverrideData
    {
        public string instance_id;
        public float[] size;
        public float[] offset;
        public string shape;
    }
}
