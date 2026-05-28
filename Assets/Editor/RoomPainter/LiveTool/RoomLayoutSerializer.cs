using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace RIMA.Editor.RoomPainter.LiveTool
{
    public static class RoomLayoutSerializer
    {
        public const string SchemaVersion = "1.0";
        public static string CurrentJsonPath => Path.Combine(Application.streamingAssetsPath, "live", "room_current.json");

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void Serialize(Scene scene, out string json)
        {
            if (!scene.IsValid()) throw new ArgumentException("Scene is not valid.", "scene");

            string now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            Layout doc = new Layout
            {
                schema_version = SchemaVersion,
                room_id = string.IsNullOrEmpty(scene.name) ? "unsaved_room" : scene.name,
                metadata = new Meta { name = string.IsNullOrEmpty(scene.name) ? "Unsaved Room" : scene.name, created = now, modified = now }
            };

            List<GameObject> roots = new List<GameObject>();
            scene.GetRootGameObjects(roots);
            foreach (GameObject root in roots)
            {
                foreach (Tilemap tilemap in root.GetComponentsInChildren<Tilemap>(true)) AddTiles(tilemap, doc.floor_tiles);
                foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
                {
                    GameObject go = transform.gameObject;
                    if (go.GetComponentInParent<Tilemap>() != null || !PrefabUtility.IsAnyPrefabInstanceRoot(go)) continue;
                    AddProp(go, doc);
                }
            }

            json = JsonConvert.SerializeObject(doc, JsonSettings);
            Directory.CreateDirectory(Path.GetDirectoryName(CurrentJsonPath));
            File.WriteAllText(CurrentJsonPath, json);
            AssetDatabase.Refresh();
        }

        public static void Deserialize(string json, Scene targetScene)
        {
            if (!targetScene.IsValid()) throw new ArgumentException("Target scene is not valid.", "targetScene");

            Layout doc = JsonConvert.DeserializeObject<Layout>(json);
            if (doc == null) throw new ArgumentException("JSON did not contain a room layout document.", "json");
            if (doc.schema_version != SchemaVersion)
            {
                throw new NotSupportedException("Room layout schema " + doc.schema_version + " is not compatible with " + SchemaVersion + ".");
            }

            GameObject root = FindRoot(targetScene, "RoomLayout_Deserialized");
            if (root != null) Object.DestroyImmediate(root);
            root = new GameObject("RoomLayout_Deserialized");
            SceneManager.MoveGameObjectToScene(root, targetScene);

            GameObject grid = new GameObject("Grid");
            grid.transform.SetParent(root.transform, false);
            grid.AddComponent<Grid>();

            GameObject floor = new GameObject("Floor_Tilemap");
            floor.transform.SetParent(grid.transform, false);
            Tilemap tilemap = floor.AddComponent<Tilemap>();
            floor.AddComponent<TilemapRenderer>();

            foreach (FloorTile tile in doc.floor_tiles)
            {
                TileBase asset = Load<TileBase>(tile.tile_guid);
                if (asset != null && tile.cell != null && tile.cell.Length >= 3)
                {
                    tilemap.SetTile(new Vector3Int(tile.cell[0], tile.cell[1], tile.cell[2]), asset);
                }
            }

            foreach (Prop prop in doc.prop_instances)
            {
                GameObject prefab = Load<GameObject>(prop.prefab_guid);
                GameObject instance = prefab == null ? null : PrefabUtility.InstantiatePrefab(prefab, targetScene) as GameObject;
                if (instance == null) continue;
                instance.transform.SetParent(root.transform, true);
                instance.transform.position = ToVector3(prop.position);
                instance.transform.rotation = Quaternion.Euler(0f, 0f, prop.rotation);
            }
        }

        private static void AddTiles(Tilemap tilemap, List<FloorTile> tiles)
        {
            tilemap.CompressBounds();
            foreach (Vector3Int cell in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(cell);
                if (tile != null) tiles.Add(new FloorTile { cell = new[] { cell.x, cell.y, cell.z }, tile_guid = GuidOrPath(tile) });
            }
        }

        private static void AddProp(GameObject go, Layout doc)
        {
            doc.prop_instances.Add(new Prop
            {
                prefab_guid = GuidOrPath(PrefabUtility.GetCorrespondingObjectFromSource(go)),
                position = new[] { go.transform.position.x, go.transform.position.y, go.transform.position.z },
                rotation = go.transform.eulerAngles.z
            });

            foreach (BoxCollider2D box in go.GetComponentsInChildren<BoxCollider2D>(true))
            {
                doc.collider_overrides.Add(new ColliderOverride
                {
                    instance_id = GlobalObjectId.GetGlobalObjectIdSlow(box).ToString(),
                    size = new[] { box.size.x, box.size.y },
                    offset = new[] { box.offset.x, box.offset.y },
                    shape = "Box"
                });
            }
        }

        private static string GuidOrPath(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(path)) return string.Empty;
            string guid = AssetDatabase.AssetPathToGUID(path);
            return string.IsNullOrEmpty(guid) ? path : guid;
        }

        private static T Load<T>(string guidOrPath) where T : Object
        {
            if (string.IsNullOrEmpty(guidOrPath)) return null;
            string path = AssetDatabase.GUIDToAssetPath(guidOrPath);
            if (string.IsNullOrEmpty(path) && guidOrPath.StartsWith("Assets/", StringComparison.Ordinal)) path = guidOrPath;
            return string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<T>(path);
        }

        private static GameObject FindRoot(Scene scene, string name)
        {
            List<GameObject> roots = new List<GameObject>();
            scene.GetRootGameObjects(roots);
            return roots.Find(root => root.name == name);
        }

        private static Vector3 ToVector3(float[] value)
        {
            return value == null || value.Length < 3 ? Vector3.zero : new Vector3(value[0], value[1], value[2]);
        }

        private sealed class Layout
        {
            public string schema_version;
            public string room_id;
            public Meta metadata;
            public List<FloorTile> floor_tiles = new List<FloorTile>();
            public List<CliffCell> cliff_cells = new List<CliffCell>();
            public List<Prop> prop_instances = new List<Prop>();
            public List<ColliderOverride> collider_overrides = new List<ColliderOverride>();
        }

        private sealed class Meta { public string name; public string created; public string modified; }
        private sealed class FloorTile { public int[] cell; public string tile_guid; }
        private sealed class CliffCell { public int[] cell; public bool is_decor; }
        private sealed class Prop { public string prefab_guid; public float[] position; public float rotation; }
        private sealed class ColliderOverride { public string instance_id; public float[] size; public float[] offset; public string shape; }
    }
}
