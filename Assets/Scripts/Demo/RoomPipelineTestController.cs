namespace RIMA.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using RIMA.RoomDesigner.Core;
    using RIMA.Runtime.Rooms;
    using RIMA.Systems.Map;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class RoomPipelineTestController : MonoBehaviour
    {
        [SerializeField] private RimaRoomBaselineTemplate template;
        [SerializeField] private Tilemap baseTilemap;
        [SerializeField] private Tilemap decalsTilemap;
        [SerializeField] private Tilemap wallsTilemap;
        [SerializeField] private GameObject stageRoot;
        [SerializeField] private int seed = 42;

        private RoomBlueprint bp;

        [ContextMenu("1. Generate")]
        public void GenerateRoom()
        {
            if (!EnsureTemplateAndTilemaps())
            {
                return;
            }

            int width = Mathf.Clamp(15, Mathf.Max(3, template.minWidth), Mathf.Max(template.minWidth, template.maxWidth));
            int height = Mathf.Clamp(12, Mathf.Max(3, template.minHeight), Mathf.Max(template.minHeight, template.maxHeight));
            var input = new GenerationInput(seed, template.biome.ToString(), template.archetypeId, width, height, template.generatorVersion);
            GridSnapshot snapshot = RunCoreRoomBaseline(new RimaRoomBaselineGenerator(), input, baseTilemap, wallsTilemap);

            if (bp == null)
            {
                bp = ScriptableObject.CreateInstance<RoomBlueprint>();
                bp.hideFlags = HideFlags.HideAndDontSave;
            }

            bp.roomId = "room_pipeline_test";
            bp.roomType = RoomType.Combat.ToString();
            bp.biomeType = template.biome;
            bp.noiseSeed = seed;
            bp.roomWidth = snapshot.width;
            bp.roomHeight = snapshot.height;
            bp.roomOrigin = snapshot.origin;
            bp.floorVariantIndex = new byte[snapshot.width * snapshot.height];
            bp.wallVariantIndex = new byte[snapshot.width * snapshot.height];
            bp.overrideVariantIndex = new bool[snapshot.width * snapshot.height];

            if (!InvokeBool("RIMA.Editor.RoomDesigner.FloorVariantPainter, RIMA.RoomDesigner.Editor", "BakeVariants", baseTilemap, bp, template.floorVariants))
            {
                Debug.LogWarning("RoomPipelineTestController: floorVariants is empty.");
            }

            List<Vector3Int> wallCells = CollectOccupiedCells(wallsTilemap);
            if (template.wallVariants == null || template.wallVariants.Length == 0)
            {
                Debug.LogWarning("RoomPipelineTestController: wallVariants is empty.");
            }
            else
            {
                InvokeVoid("RIMA.Editor.RoomDesigner.WallAutoConnect, RIMA.RoomDesigner.Editor", "RefreshNeighborhood", wallsTilemap, wallCells, template.wallVariants, bp);
                InvokeBool("RIMA.Editor.RoomDesigner.WallAutoConnect, RIMA.RoomDesigner.Editor", "BakeWallVariants", wallsTilemap, bp, template.wallVariants);
            }
        }

        [ContextMenu("2. Paint Decals")]
        public void PaintDecals()
        {
            if (!EnsureGenerated())
            {
                return;
            }

            InvokeBool("RIMA.Editor.RoomDesigner.DecalPainter, RIMA.RoomDesigner.Editor", "PaintDecals", decalsTilemap, bp, template.decalSprites, seed, template.decalDensity);
        }

        [ContextMenu("3. Place Props")]
        public void PlaceProps()
        {
            if (!EnsureGenerated())
            {
                return;
            }

            AnchorZone[] anchors = RimaArchetypeGenerators.GetDefaultAnchorZones(template.archetypeId, seed, bp.roomWidth, bp.roomHeight);
            InvokeObject("RIMA.Editor.RoomDesigner.PropPlacer, RIMA.RoomDesigner.Editor", "PlaceProps", stageRoot, anchors, template.propSpecs, bp, seed);
        }

        [ContextMenu("4. Save + Load Round-Trip")]
        public void SaveAndLoad()
        {
            if (!EnsureGenerated())
            {
                return;
            }

#if UNITY_EDITOR
            try
            {
                string roomId = $"room_pipeline_test_{seed}";
                object result = InvokeObject("RIMA.Editor.RoomDesigner.RoomSaver, RIMA.RoomDesigner.Editor", "Save", stageRoot, roomId, template.biome, seed, bp.roomWidth, bp.roomHeight);
                FieldInfo prefabPathField = result.GetType().GetField("Item1");
                string prefabPath = prefabPathField != null ? prefabPathField.GetValue(result) as string : string.Empty;
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogError($"RoomPipelineTestController: saved prefab missing at {prefabPath}");
                    return;
                }

                GameObject instance = Instantiate(prefab, new Vector3(bp.roomWidth + 2f, 0f, 0f), Quaternion.identity);
                instance.name = prefab.name + "_RoundTrip";
                Debug.Log($"RoomPipelineTestController: saved and loaded {prefabPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"RoomPipelineTestController: save/load failed: {ex.Message}");
            }
#else
            Debug.LogWarning("RoomPipelineTestController: save/load is editor-only.");
#endif
        }

        private bool EnsureGenerated()
        {
            if (!EnsureTemplateAndTilemaps())
            {
                return false;
            }

            if (bp != null)
            {
                return true;
            }

            Debug.LogWarning("RoomPipelineTestController: generate a room first.");
            return false;
        }

        private bool EnsureTemplateAndTilemaps()
        {
            if (template == null)
            {
                Debug.LogError("RoomPipelineTestController: assign a RimaRoomBaselineTemplate.");
                return false;
            }

            if (baseTilemap == null || decalsTilemap == null || wallsTilemap == null || stageRoot == null)
            {
                Debug.LogError("RoomPipelineTestController: tilemaps or stage root are not assigned.");
                return false;
            }

            return true;
        }

        private static GridSnapshot RunCoreRoomBaseline(RoomBaselineGeneratorBase generator, GenerationInput input, Tilemap floor, Tilemap wall)
        {
            return (GridSnapshot)InvokeObject("RIMA.RoomDesigner.Core.Editor.CoreRoomBaselineRunner, RIMA.RoomDesigner.Core.Editor", "Run", generator, input, floor, wall);
        }

        private static bool InvokeBool(string typeName, string methodName, params object[] args)
        {
            object result = InvokeObject(typeName, methodName, args);
            return result is bool value && value;
        }

        private static void InvokeVoid(string typeName, string methodName, params object[] args)
        {
            InvokeObject(typeName, methodName, args);
        }

        private static object InvokeObject(string typeName, string methodName, params object[] args)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                throw new InvalidOperationException($"{typeName} was not found.");
            }

            MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new MissingMethodException(type.FullName, methodName);
            }

            return method.Invoke(null, args);
        }

        private static List<Vector3Int> CollectOccupiedCells(Tilemap tilemap)
        {
            var cells = new List<Vector3Int>();
            if (tilemap == null)
            {
                return cells;
            }

            BoundsInt bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var cell = new Vector3Int(x, y, 0);
                    if (tilemap.GetTile(cell) != null)
                    {
                        cells.Add(cell);
                    }
                }
            }

            return cells;
        }
    }
}
