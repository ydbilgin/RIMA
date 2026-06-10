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

        [MenuItem("RIMA/Rooms/Populate Generated Props")]
        public static void PopulateGeneratedProps()
        {
            IReadOnlyList<PropDefinitionSO> pool = RoomTemplateAutoPropsUtility.LoadPropPool();
            if (pool == null || pool.Count == 0)
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

                    RoomTemplateAutoPropsUtility.Result result =
                        RoomTemplateAutoPropsUtility.Populate(room, pool, RoomTemplateAutoPropsUtility.StableSeed(room.name), true, "Populate Generated Props");
                    totalProps += result.afterCount;
                    totalRooms++;
                    Debug.Log($"[PopulateGeneratedProps] {room.name}: {result.afterCount} props placed.");
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

    public static class RoomTemplateAutoPropsUtility
    {
        private const string PropRegistryPath = "Assets/Resources/Props/PropRegistry.asset";

        // Density fills edges/clusters and keeps the center clean.
        private const float TargetDensity = 0.30f;
        private const int SeedTries = 8;
        private const int MaxPropsPerRoom = 45;

        public readonly struct Result
        {
            public readonly int beforeCount;
            public readonly int afterCount;
            public readonly int seed;

            public Result(int beforeCount, int afterCount, int seed)
            {
                this.beforeCount = beforeCount;
                this.afterCount = afterCount;
                this.seed = seed;
            }
        }

        public static IReadOnlyList<PropDefinitionSO> LoadPropPool()
        {
            PropRegistrySO registry = AssetDatabase.LoadAssetAtPath<PropRegistrySO>(PropRegistryPath);
            if (registry == null)
            {
                Debug.LogError($"[PopulateGeneratedProps] PropRegistry not found at {PropRegistryPath}");
                return new List<PropDefinitionSO>();
            }

            registry.RebuildIndex();
            IReadOnlyList<PropDefinitionSO> allProps = registry.AllProps;
            List<PropDefinitionSO> pool = new List<PropDefinitionSO>();
            if (allProps == null)
            {
                return pool;
            }

            for (int i = 0; i < allProps.Count; i++)
            {
                PropDefinitionSO prop = allProps[i];
                if (prop != null && prop.worldSprite != null)
                {
                    pool.Add(prop);
                }
            }

            return pool;
        }

        public static Result Populate(RoomTemplateSO room, IReadOnlyList<PropDefinitionSO> pool, int seed, bool recordUndo, string undoLabel)
        {
            if (room == null)
            {
                return new Result(0, 0, seed);
            }

            int beforeCount = room.props != null ? room.props.Count : 0;
            if (recordUndo)
            {
                Undo.RecordObject(room, undoLabel);
            }

            if (room.props == null)
            {
                room.props = new List<PropPlacementData>();
            }
            else
            {
                room.props.Clear();
            }

            if (pool == null || pool.Count == 0)
            {
                EditorUtility.SetDirty(room);
                return new Result(beforeCount, room.props.Count, seed);
            }

            CompositionRoleMap roleMap = CompositionRoleMapGenerator.GenerateFromRoom(room);
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            List<BridsonPoissonAutoPlacer.PlacementCandidate> candidates = null;
            for (int attempt = 0; attempt < SeedTries; attempt++)
            {
                List<BridsonPoissonAutoPlacer.PlacementCandidate> tryResult =
                    placer.Generate(room, roleMap, pool, seed + attempt * 13, TargetDensity);
                if (tryResult.Count > 0)
                {
                    candidates = tryResult;
                    break;
                }
            }

            if (candidates == null)
            {
                candidates = new List<BridsonPoissonAutoPlacer.PlacementCandidate>();
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                if (room.props.Count >= MaxPropsPerRoom) break;

                BridsonPoissonAutoPlacer.PlacementCandidate candidate = candidates[i];
                if (candidate.prop == null) continue;
                if (roleMap != null && roleMap.GetRoleAt(candidate.tilePos) == CompositionRole.CleanCenter) continue;

                string propAssetPath = AssetDatabase.GetAssetPath(candidate.prop);
                string propGuid = string.IsNullOrEmpty(propAssetPath) ? string.Empty : AssetDatabase.AssetPathToGUID(propAssetPath);
                if (string.IsNullOrEmpty(propGuid)) continue;

                PropPlacementData placement = new PropPlacementData(propGuid, candidate.tilePos)
                {
                    rotationSteps = candidate.rotationSteps,
                    flipX = candidate.flipX,
                    variantIndex = candidate.variantIndex,
                    placedByUser = "auto"
                };
                room.props.Add(placement);
            }

            EditorUtility.SetDirty(room);
            return new Result(beforeCount, room.props.Count, seed);
        }

        public static int StableSeed(string value)
        {
            unchecked
            {
                int hash = 17;
                if (!string.IsNullOrEmpty(value))
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        hash = hash * 31 + value[i];
                    }
                }

                return hash;
            }
        }
    }
}
#endif
