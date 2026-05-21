using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Map
{
    public static class RoomLoader
    {
        public static void LoadJsonToScene(string jsonPath, Tilemap floorMap, Transform propParent, MaterialVariantPoolSO pool)
        {
            LoadJsonToScene(jsonPath, floorMap, propParent, pool, null);
        }

        public static RoomInstance LoadJsonToScene(string jsonPath, Tilemap floorMap, Transform propParent, MaterialVariantPoolSO pool, WallPrefabRegistry wallRegistry)
        {
            if (floorMap == null)
            {
                Debug.LogError("[RoomLoader] Missing floor Tilemap.");
                return null;
            }

            string text = ReadJsonText(jsonPath);
            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogError("[RoomLoader] JSON file is empty or missing: " + jsonPath);
                return null;
            }

            RoomLayoutJson room = JsonUtility.FromJson<RoomLayoutJson>(text);
            if (room == null || string.IsNullOrEmpty(room.room_id))
            {
                Debug.LogError("[RoomLoader] Failed to parse room JSON: " + jsonPath);
                return null;
            }

            Transform root = propParent != null ? propParent : floorMap.transform;
            RoomInstance instance = root.GetComponent<RoomInstance>();
            if (instance == null) instance = root.gameObject.AddComponent<RoomInstance>();
            instance.roomId = room.room_id;
            instance.mobInstances.Clear();
            instance.doors.Clear();

            PaintFloor(room, floorMap, pool);
            InstantiateWalls(room, floorMap, root, wallRegistry);
            InstantiateProps(room, root);
            PlaceMobSpawnMarkers(room, root);
            PlaceDoorTriggers(room, root, instance);
            PlacePlayerSpawnMarkers(room, root);

            Debug.Log("[RoomLoader] Loaded " + room.room_id + " from " + jsonPath);
            return instance;
        }

        private static string ReadJsonText(string jsonPath)
        {
            if (string.IsNullOrWhiteSpace(jsonPath)) return string.Empty;

            string path = jsonPath;
            if (jsonPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
            {
                path = Path.Combine(Application.dataPath, jsonPath.Substring("Assets/".Length));
            }

            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        private static void PaintFloor(RoomLayoutJson room, Tilemap floorMap, MaterialVariantPoolSO pool)
        {
            if (room.floor == null)
            {
                Debug.LogWarning("[RoomLoader] Room has no floor block: " + room.room_id);
                return;
            }

            floorMap.ClearAllTiles();
            for (int x = 0; x < room.width; x++)
            {
                for (int y = 0; y < room.height; y++)
                {
                    string material = ResolveMaterial(room.floor, x, y);
                    TileBase tile = pool != null ? pool.GetVariant(material, Hash(room.room_id, x, y)) : null;
                    if (tile != null) floorMap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private static string ResolveMaterial(RoomFloor floor, int x, int y)
        {
            string material = floor.default_material;
            if (floor.zones != null)
            {
                for (int i = 0; i < floor.zones.Length; i++)
                {
                    RoomZone zone = floor.zones[i];
                    if (zone == null || zone.rect == null) continue;
                    if (x >= zone.rect.x && x < zone.rect.x + zone.rect.width &&
                        y >= zone.rect.y && y < zone.rect.y + zone.rect.height)
                    {
                        material = zone.material;
                    }
                }
            }

            if (floor.accents != null)
            {
                for (int i = 0; i < floor.accents.Length; i++)
                {
                    RoomAccent accent = floor.accents[i];
                    if (accent != null && accent.x == x && accent.y == y) material = accent.material;
                }
            }

            return material;
        }

        private static void InstantiateWalls(RoomLayoutJson room, Tilemap floorMap, Transform parent, WallPrefabRegistry registry)
        {
            if (room.walls == null || registry == null) return;

            for (int i = 0; i < room.walls.Length; i++)
            {
                RoomWall wall = room.walls[i];
                if (wall == null) continue;

                GameObject prefab = registry.GetPrefab(wall.prefab);
                if (prefab == null) continue;

                Vector3 position = floorMap.GetCellCenterWorld(new Vector3Int(wall.x, wall.y, 0));
                Quaternion rotation = Quaternion.Euler(0f, 0f, wall.rotation);
                GameObject instance = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
                instance.name = "Wall_" + wall.prefab;
            }
        }

        private static void InstantiateProps(RoomLayoutJson room, Transform parent)
        {
            if (room.props == null) return;

            for (int i = 0; i < room.props.Length; i++)
            {
                RoomProp prop = room.props[i];
                if (prop == null || string.IsNullOrEmpty(prop.prefab)) continue;

                GameObject prefab = Resources.Load<GameObject>(prop.prefab);
                if (prefab == null) continue;

                GameObject instance = UnityEngine.Object.Instantiate(prefab, new Vector3(prop.x, prop.y, 0f), Quaternion.Euler(0f, 0f, prop.rotation), parent);
                instance.name = "Prop_" + prop.prefab;
                float scale = prop.scale <= 0f ? 1f : prop.scale;
                instance.transform.localScale = new Vector3(prop.flip_x ? -scale : scale, scale, scale);
            }
        }

        private static void PlaceMobSpawnMarkers(RoomLayoutJson room, Transform parent)
        {
            if (room.mob_spawns == null) return;

            for (int i = 0; i < room.mob_spawns.Length; i++)
            {
                RoomMobSpawn spawn = room.mob_spawns[i];
                if (spawn == null) continue;

                GameObject marker = new GameObject("MobSpawn_" + spawn.mob_id + "_W" + Mathf.Max(1, spawn.wave));
                marker.transform.SetParent(parent, false);
                marker.transform.position = new Vector3(spawn.x, spawn.y, 0f);
            }
        }

        private static void PlaceDoorTriggers(RoomLayoutJson room, Transform parent, RoomInstance roomInstance)
        {
            if (room.doors == null) return;

            for (int i = 0; i < room.doors.Length; i++)
            {
                RoomDoor door = room.doors[i];
                if (door == null) continue;

                GameObject go = new GameObject("Door_" + door.direction + "_to_" + (string.IsNullOrEmpty(door.target_room_id) ? "Exit" : door.target_room_id));
                go.transform.SetParent(parent, false);
                go.transform.position = new Vector3(door.x, door.y, 0f);

                BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                collider.size = new Vector2(1.5f, 1.5f);

                global::RIMA.DoorTrigger trigger = go.AddComponent<global::RIMA.DoorTrigger>();
                SetDoorDirection(trigger, door.direction);
                SetDoorCollider(trigger, collider);
                trigger.SetActive(!door.locked_initial);
                roomInstance.doors.Add(trigger);
            }
        }

        private static void PlacePlayerSpawnMarkers(RoomLayoutJson room, Transform parent)
        {
            if (room.spawn_points == null) return;

            for (int i = 0; i < room.spawn_points.Length; i++)
            {
                RoomSpawnPoint spawn = room.spawn_points[i];
                if (spawn == null) continue;

                GameObject marker = new GameObject("PlayerSpawn_" + spawn.id);
                marker.transform.SetParent(parent, false);
                marker.transform.position = new Vector3(spawn.x, spawn.y, 0f);
            }
        }

        private static void SetDoorDirection(global::RIMA.DoorTrigger trigger, string direction)
        {
            global::RIMA.DoorDirection value = global::RIMA.DoorDirection.North;
            if (!string.IsNullOrEmpty(direction))
            {
                Enum.TryParse(CultureName(direction), out value);
            }

            FieldInfo field = typeof(global::RIMA.DoorTrigger).GetField("direction", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null) field.SetValue(trigger, value);
        }

        private static void SetDoorCollider(global::RIMA.DoorTrigger trigger, BoxCollider2D collider)
        {
            FieldInfo field = typeof(global::RIMA.DoorTrigger).GetField("col", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null) field.SetValue(trigger, collider);
        }

        private static string CultureName(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToUpperInvariant(value[0]) + value.Substring(1).ToLowerInvariant();
        }

        private static int Hash(string roomId, int x, int y)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (roomId != null ? roomId.GetHashCode() : 0);
                hash = hash * 31 + x;
                hash = hash * 31 + y;
                return hash;
            }
        }
    }
}
