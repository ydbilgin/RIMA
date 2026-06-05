#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Auto;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.Map
{
    public static class PopulateGeneratedPropsMenu
    {
        private const string GeneratedRoomsFolder = "Assets/Data/Rooms/Generated";
        private const string PropRegistryPath     = "Assets/Data/Props/PropRegistry.asset";

        // Density: fills edges/clusters, keeps center clean. (cx review: 0.55 cok yogundu -> 0.30)
        private const float TargetDensity = 0.30f;
        // Try multiple seeds and keep the best (non-empty) result.
        private const int SeedTries = 8;
        // cx review: buyuk odalarda 90 prop'a cikti -> sert tavan.
        private const int MaxPropsPerRoom = 45;

        [MenuItem("RIMA/Rooms/Populate Generated Props")]
        public static void PopulateGeneratedProps()
        {
            PropRegistrySO registry = AssetDatabase.LoadAssetAtPath<PropRegistrySO>(PropRegistryPath);
            if (registry == null)
            {
                Debug.LogError($"[PopulateGeneratedProps] PropRegistry not found at {PropRegistryPath}");
                return;
            }

            registry.RebuildIndex();
            IReadOnlyList<PropDefinitionSO> allProps = registry.AllProps;
            if (allProps == null || allProps.Count == 0)
            {
                Debug.LogWarning("[PopulateGeneratedProps] PropRegistry has no props — nothing to place.");
                return;
            }

            // Build a pool of props suitable for auto-placement (no forbidden-all props).
            List<PropDefinitionSO> pool = new List<PropDefinitionSO>();
            for (int i = 0; i < allProps.Count; i++)
            {
                PropDefinitionSO p = allProps[i];
                if (p != null && p.worldSprite != null)
                {
                    pool.Add(p);
                }
            }

            if (pool.Count == 0)
            {
                Debug.LogWarning("[PopulateGeneratedProps] No props with worldSprites in registry — nothing to place.");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { GeneratedRoomsFolder });
            if (guids == null || guids.Length == 0)
            {
                Debug.LogWarning($"[PopulateGeneratedProps] No RoomTemplateSO assets found in {GeneratedRoomsFolder}");
                return;
            }

            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            int totalRooms = 0;
            int totalProps  = 0;

            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                    if (room == null) continue;

                    Undo.RecordObject(room, "Populate Generated Props");

                    // Clear existing auto-placed props so re-running is idempotent.
                    if (room.props == null)
                    {
                        room.props = new List<PropPlacementData>();
                    }
                    else
                    {
                        room.props.Clear();
                    }

                    CompositionRoleMap roleMap = CompositionRoleMapGenerator.GenerateFromRoom(room);

                    // Try multiple seeds; keep first non-empty result for robustness.
                    int baseSeed = room.name.GetHashCode();
                    List<BridsonPoissonAutoPlacer.PlacementCandidate> candidates = null;
                    for (int attempt = 0; attempt < SeedTries; attempt++)
                    {
                        var tryResult = placer.Generate(room, roleMap, pool, baseSeed + attempt * 13, TargetDensity);
                        if (tryResult.Count > 0)
                        {
                            candidates = tryResult;
                            break;
                        }
                    }
                    if (candidates == null) candidates = new List<BridsonPoissonAutoPlacer.PlacementCandidate>();

                    foreach (BridsonPoissonAutoPlacer.PlacementCandidate c in candidates)
                    {
                        if (c.prop == null) continue;
                        if (room.props.Count >= MaxPropsPerRoom) break;

                        // cx review: CleanCenter (combat alani) prop'suz kalsin.
                        if (roleMap != null && roleMap.GetRoleAt(c.tilePos) == CompositionRole.CleanCenter) continue;

                        // Resolve the asset GUID for this prop so IsoRoomBuilder can look it up.
                        string propAssetPath = AssetDatabase.GetAssetPath(c.prop);
                        string propGuid      = AssetDatabase.AssetPathToGUID(propAssetPath);
                        if (string.IsNullOrEmpty(propGuid)) continue;

                        PropPlacementData placement = new PropPlacementData(propGuid, c.tilePos)
                        {
                            rotationSteps = c.rotationSteps,
                            flipX         = c.flipX,
                            variantIndex  = c.variantIndex,
                            placedByUser  = "auto"
                        };
                        room.props.Add(placement);
                    }

                    EditorUtility.SetDirty(room);
                    int count = room.props.Count;
                    totalProps += count;
                    totalRooms++;
                    Debug.Log($"[PopulateGeneratedProps] {room.name}: {count} props placed.");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[PopulateGeneratedProps] Done — {totalRooms} rooms, {totalProps} total props ({(totalRooms > 0 ? (float)totalProps / totalRooms : 0f):F1} avg/room).");
        }
    }
}
#endif
