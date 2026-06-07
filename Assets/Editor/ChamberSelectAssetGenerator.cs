using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public static class ChamberSelectAssetGenerator
    {
        private const string SourcePedestalPath = "STAGING/imagegen/echo_pedestal.png";
        private const string PedestalSpritePath = "Assets/Sprites/Environment/Props/echo_pedestal.png";
        private const string EchoPedestalPath = "Assets/Data/Props/EchoPedestal.asset";
        private const string ArchGatePath = "Assets/Data/Props/ArchGate.asset";
        private const string RegistryPath = "Assets/Data/Props/PropRegistry.asset";
        private const string RoomPath = "Assets/Data/Rooms/Special/Chamber_CharSelect.asset";

        private const string FloorTilePath = "Assets/Resources/ChamberSelect/Tiles/ChamberFloor.asset";
        private const string CollisionTilePath = "Assets/Resources/ChamberSelect/Tiles/ChamberCollision.asset";
        private const string OverlayTilePath = "Assets/Resources/ChamberSelect/Tiles/ChamberOverlayPath.asset";

        private static readonly Vector2Int[] Pedestals =
        {
            new(6, 6), new(8, 8), new(11, 10), new(14, 11), new(17, 10),
            new(19, 8), new(17, 6), new(14, 5), new(11, 4), new(8, 4)
        };

        [MenuItem("RIMA/Character Select/Generate Attunement Chamber Assets")]
        public static void Generate()
        {
            EnsureFolders();
            CopyPedestal();
            PropDefinitionSO echoPedestal = EnsureEchoPedestal();
            PropDefinitionSO archGate = EnsureArchGate();

            EnsureTileAssets();
            UpdatePropRegistry(echoPedestal, archGate);
            BuildRoom(echoPedestal, archGate);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[ChamberSelectAssetGenerator] Generated EchoPedestal, registry entry, tiles, and Chamber_CharSelect room asset.");
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Sprites/Environment", "Props");
            EnsureFolder("Assets/Data/Rooms", "Special");
            EnsureFolder("Assets/Resources", "ChamberSelect");
            EnsureFolder("Assets/Resources/ChamberSelect", "Tiles");
        }

        private static void EnsureFolder(string parent, string child)
        {
            string full = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(full))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static void CopyPedestal()
        {
            if (!File.Exists(SourcePedestalPath))
            {
                Debug.LogError($"[ChamberSelectAssetGenerator] Missing provided pedestal source: {SourcePedestalPath}");
                return;
            }

            File.Copy(SourcePedestalPath, PedestalSpritePath, true);
            AssetDatabase.ImportAsset(PedestalSpritePath, ImportAssetOptions.ForceUpdate);

            TextureImporter importer = AssetImporter.GetAtPath(PedestalSpritePath) as TextureImporter;
            if (importer == null) return;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 64f;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }

        private static PropDefinitionSO EnsureEchoPedestal()
        {
            PropDefinitionSO prop = AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(EchoPedestalPath);
            if (prop == null)
            {
                prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                AssetDatabase.CreateAsset(prop, EchoPedestalPath);
            }

            Sprite sprite = LoadRequired<Sprite>(PedestalSpritePath);
            prop.displayName = "EchoPedestal";
            prop.description = "Attunement chamber echo pedestal.";
            prop.worldSprite = sprite;
            prop.icon = sprite;
            prop.footprintSize = new Vector2Int(2, 2);
            prop.spriteAnchor = Vector2Int.zero;
            prop.blocksWalkable = true;
            prop.blocksMovement = true;
            prop.colliderShape = PropDefinitionSO.ColliderShape.Box;
            prop.colliderFootprintRatio = 0.82f;
            prop.requiresWalkableTile = false;
            prop.respectsWalkableMask = true;
            prop.sortingMode = PropDefinitionSO.PropSortingMode.YPosition;
            EditorUtility.SetDirty(prop);
            prop.propId = AssetDatabase.AssetPathToGUID(EchoPedestalPath);
            return prop;
        }

        private static PropDefinitionSO EnsureArchGate()
        {
            PropDefinitionSO prop = AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(ArchGatePath);
            if (prop == null)
            {
                prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                AssetDatabase.CreateAsset(prop, ArchGatePath);
            }

            Sprite sprite = LoadRequired<Sprite>("Assets/Resources/Environment/Gate/gate_arch.png");
            prop.displayName = "ArchGate";
            prop.description = "Ruined Keep rift gate for chamber exits.";
            prop.worldSprite = sprite;
            prop.icon = sprite;
            prop.footprintSize = new Vector2Int(2, 1);
            prop.spriteAnchor = Vector2Int.zero;
            prop.blocksWalkable = false;
            prop.blocksMovement = false;
            prop.colliderShape = PropDefinitionSO.ColliderShape.None;
            prop.requiresWalkableTile = false;
            prop.respectsWalkableMask = true;
            prop.sortingMode = PropDefinitionSO.PropSortingMode.YPosition;
            EditorUtility.SetDirty(prop);
            prop.propId = AssetDatabase.AssetPathToGUID(ArchGatePath);
            return prop;
        }

        private static void EnsureTileAssets()
        {
            Sprite floor = LoadRequired<Sprite>("Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png");
            Sprite overlay = LoadRequired<Sprite>("Assets/Sprites/Environment/PixelLabFloor451/floor451_15.png");
            EnsureTile(FloorTilePath, floor, Color.white);
            EnsureTile(CollisionTilePath, floor, new Color(0.22f, 0.07f, 0.08f, 0.65f));
            EnsureTile(OverlayTilePath, overlay, Color.white);
        }

        private static void EnsureTile(string path, Sprite sprite, Color color)
        {
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                AssetDatabase.CreateAsset(tile, path);
            }

            tile.sprite = sprite;
            tile.color = color;
            tile.colliderType = Tile.ColliderType.None;
            EditorUtility.SetDirty(tile);
        }

        private static void UpdatePropRegistry(params PropDefinitionSO[] props)
        {
            PropRegistrySO registry = LoadRequired<PropRegistrySO>(RegistryPath);
            foreach (PropDefinitionSO prop in props)
            {
                registry.EditorAddProp(prop);
            }

            EditorUtility.SetDirty(registry);
        }

        private static void BuildRoom(
            PropDefinitionSO echoPedestal,
            PropDefinitionSO archGate)
        {
            RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(RoomPath);
            if (room == null)
            {
                room = ScriptableObject.CreateInstance<RoomTemplateSO>();
                AssetDatabase.CreateAsset(room, RoomPath);
            }

            RectInt bounds = new RectInt(0, 0, 22, 16);
            room.schemaVersion = "1.0";
            room.roomId = "Chamber_CharSelect";
            room.biomeId = "attunement_chamber";
            room.roomType = RoomType.Event;
            room.bounds = bounds;
            room.playerSpawn = new PlayerSpawnSocket
            {
                socketId = "playerSpawn",
                position = new Vector2Int(3, 3),
                facing = DoorDirection.North
            };
            room.doorSockets = new List<DoorSocket>
            {
                new DoorSocket
                {
                    socketId = "riftExit",
                    position = new Vector2Int(20, 13),
                    direction = DoorDirection.North,
                    widthInTiles = 2,
                    isExit = true
                }
            };
            room.enemySpawnSockets = new List<EnemySpawnSocket>();
            room.cameraBounds = CameraBounds.FromBounds(bounds);
            room.prefabRef = null;
            room.backgroundLayers = new List<BackgroundLayerData>();
            room.encounterTags = new List<string> { "character_select", "attunement_chamber" };
            room.difficultyTags = new List<string>();
            room.blockerTags = new List<string>();
            room.walkableGrid = BuildWalkable(bounds);
            room.overlayMask = BuildOverlay(bounds);
            room.props = BuildProps(
                AssetDatabase.AssetPathToGUID(EchoPedestalPath),
                AssetDatabase.AssetPathToGUID(ArchGatePath));

            EditorUtility.SetDirty(room);
        }

        private static bool[] BuildWalkable(RectInt bounds)
        {
            bool[] grid = new bool[bounds.width * bounds.height];
            Vector2 start = new Vector2(2.5f, 3.0f);
            Vector2 end = new Vector2(20.0f, 13.0f);
            Vector2 axis = end - start;
            float axisLenSq = axis.sqrMagnitude;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    Vector2 p = new Vector2(x + 0.5f, y + 0.5f);
                    float t = Mathf.Clamp01(Vector2.Dot(p - start, axis) / axisLenSq);
                    Vector2 closest = start + axis * t;
                    float crescentWidth = Mathf.Lerp(4.2f, 2.7f, t) + Mathf.Sin(t * Mathf.PI) * 2.15f;
                    bool walkable = Vector2.Distance(p, closest) <= crescentWidth;

                    if (x >= 1 && x <= 6 && y >= 2 && y <= 7)
                    {
                        walkable = true;
                    }

                    if (x >= 17 && x <= 21 && y >= 11 && y <= 15)
                    {
                        walkable = true;
                    }

                    grid[(y - bounds.yMin) * bounds.width + (x - bounds.xMin)] = walkable;
                }
            }

            return grid;
        }

        private static int[] BuildOverlay(RectInt bounds)
        {
            int[] mask = new int[bounds.width * bounds.height];
            Vector2Int[] path =
            {
                new(3, 3), new(4, 4), new(5, 4), new(6, 5), new(7, 6), new(8, 6),
                new(9, 7), new(10, 8), new(11, 8), new(12, 9), new(13, 9), new(14, 9),
                new(15, 9), new(16, 10), new(17, 11), new(18, 12), new(19, 13), new(20, 13)
            };

            foreach (Vector2Int cell in path)
            {
                if (!bounds.Contains(cell)) continue;
                int idx = (cell.y - bounds.yMin) * bounds.width + (cell.x - bounds.xMin);
                mask[idx] = 1;
            }

            return mask;
        }

        private static List<PropPlacementData> BuildProps(
            string echoGuid,
            string archGuid)
        {
            var props = new List<PropPlacementData>();

            foreach (Vector2Int pedestal in Pedestals)
            {
                props.Add(new PropPlacementData(echoGuid, pedestal) { placedByUser = "chamber_generator" });
            }

            props.Add(new PropPlacementData(archGuid, new Vector2Int(20, 13)) { placedByUser = "chamber_generator" });
            return props;
        }

        private static T LoadRequired<T>(string path) where T : Object
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                Debug.LogError($"[ChamberSelectAssetGenerator] Missing required asset: {path}");
            }

            return asset;
        }
    }
}
