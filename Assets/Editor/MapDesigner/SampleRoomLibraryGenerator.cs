#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.MapDesigner
{
    public static class SampleRoomLibraryGenerator
    {
        private const string MenuPath = "RIMA/MapDesigner/Brush/Generate Sample Library v1";
        private const string LibraryPath = "Assets/Data/Rooms/Library";
        private const string BarrelPath = "Assets/Data/Brush/Props/Barrel/barrel_001.asset";
        private const string BiomeId = "ShatteredKeep";

        [MenuItem(MenuPath)]
        public static void GenerateSampleLibraryV1()
        {
            EnsureFolder(LibraryPath);

            string barrelGuid = AssetDatabase.AssetPathToGUID(BarrelPath);
            if (string.IsNullOrEmpty(barrelGuid))
            {
                Debug.LogError($"[SampleRoomLibraryGenerator] Missing barrel prop asset at {BarrelPath}");
                return;
            }

            RoomSpec[] specs = BuildSpecs();
            int count = 0;
            foreach (RoomSpec spec in specs)
            {
                RoomTemplateSO template = BuildTemplate(spec, barrelGuid);
                string assetPath = $"{LibraryPath}/{spec.RoomId}.asset";
                if (AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(assetPath) != null)
                {
                    AssetDatabase.DeleteAsset(assetPath);
                }

                AssetDatabase.CreateAsset(template, assetPath);
                count++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[SampleRoomLibraryGenerator] Generated {count} room templates in Library/");
        }

        private static RoomTemplateSO BuildTemplate(RoomSpec spec, string barrelGuid)
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.schemaVersion = "1.0";
            template.roomId = spec.RoomId;
            template.biomeId = BiomeId;
            template.roomType = spec.RoomType;
            template.bounds = new RectInt(0, 0, spec.Width, spec.Height);
            template.cameraBounds = CameraBounds.FromBounds(template.bounds);
            template.prefabRef = null;
            template.doorSockets = BuildDoorSockets(spec.Doors);
            template.playerSpawn = spec.PlayerSpawn.HasValue
                ? new PlayerSpawnSocket
                {
                    socketId = "player_spawn_01",
                    position = spec.PlayerSpawn.Value.Position,
                    facing = spec.PlayerSpawn.Value.Facing
                }
                : null;
            template.enemySpawnSockets = BuildEnemySpawnSockets(spec.Enemies);
            template.encounterTags = new List<string>(spec.Tags);
            template.difficultyTags = new List<string>();
            template.blockerTags = new List<string>();
            template.props = BuildProps(spec.Props, barrelGuid);
            template.walkableGrid = BuildWalkableGrid(spec, template.doorSockets);
            return template;
        }

        private static bool[] BuildWalkableGrid(RoomSpec spec, List<DoorSocket> doors)
        {
            var walkable = new bool[spec.Width * spec.Height];
            for (int row = 0; row < spec.Layout.Length; row++)
            {
                int y = spec.Height - 1 - row;
                string line = spec.Layout[row];
                for (int x = 0; x < spec.Width; x++)
                {
                    char c = x < line.Length ? line[x] : '.';
                    walkable[(y * spec.Width) + x] = IsWalkableLayoutChar(c);
                }
            }

            foreach (DoorSocket door in doors)
            {
                MarkDoorTiles(walkable, spec.Width, spec.Height, door, true);
            }

            if (spec.PlayerSpawn.HasValue)
            {
                MarkTile(walkable, spec.Width, spec.Height, spec.PlayerSpawn.Value.Position, true);
            }

            foreach (EnemySpec enemy in spec.Enemies)
            {
                MarkTile(walkable, spec.Width, spec.Height, enemy.Position, true);
            }

            foreach (Vector2Int prop in spec.Props)
            {
                MarkTile(walkable, spec.Width, spec.Height, prop, false);
            }

            return walkable;
        }

        private static bool IsWalkableLayoutChar(char c)
        {
            return c == '.' || c == ' ' || c == 'P' || c == 'M' || c == 'E' || c == 'B' || c == 'D';
        }

        private static void MarkDoorTiles(bool[] walkable, int width, int height, DoorSocket door, bool value)
        {
            int dx = door.direction == DoorDirection.North || door.direction == DoorDirection.South ? 1 : 0;
            int dy = door.direction == DoorDirection.East || door.direction == DoorDirection.West ? 1 : 0;
            for (int i = 0; i < door.widthInTiles; i++)
            {
                MarkTile(walkable, width, height, new Vector2Int(door.position.x + (dx * i), door.position.y + (dy * i)), value);
            }
        }

        private static void MarkTile(bool[] walkable, int width, int height, Vector2Int position, bool value)
        {
            if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height)
            {
                return;
            }

            walkable[(position.y * width) + position.x] = value;
        }

        private static List<DoorSocket> BuildDoorSockets(DoorSpec[] doors)
        {
            var sockets = new List<DoorSocket>(doors.Length);
            for (int i = 0; i < doors.Length; i++)
            {
                DoorSpec door = doors[i];
                sockets.Add(new DoorSocket
                {
                    socketId = $"door_{DirectionSuffix(door.Direction)}_{i + 1:00}",
                    position = door.Position,
                    direction = door.Direction,
                    widthInTiles = 2,
                    isExit = true
                });
            }

            return sockets;
        }

        private static string DirectionSuffix(DoorDirection direction)
        {
            switch (direction)
            {
                case DoorDirection.North: return "N";
                case DoorDirection.South: return "S";
                case DoorDirection.East: return "E";
                case DoorDirection.West: return "W";
                default: return "X";
            }
        }

        private static List<EnemySpawnSocket> BuildEnemySpawnSockets(EnemySpec[] enemies)
        {
            var sockets = new List<EnemySpawnSocket>(enemies.Length);
            for (int i = 0; i < enemies.Length; i++)
            {
                sockets.Add(new EnemySpawnSocket
                {
                    socketId = $"enemy_spawn_{i + 1:00}",
                    position = enemies[i].Position,
                    tierHint = enemies[i].TierHint
                });
            }

            return sockets;
        }

        private static List<PropPlacementData> BuildProps(Vector2Int[] positions, string barrelGuid)
        {
            var props = new List<PropPlacementData>(positions.Length);
            foreach (Vector2Int position in positions)
            {
                props.Add(new PropPlacementData
                {
                    propDefinitionGuid = barrelGuid,
                    tilePosition = position,
                    rotationSteps = 0,
                    variantIndex = -1
                });
            }

            return props;
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private static RoomSpec[] BuildSpecs()
        {
            return new[]
            {
                new RoomSpec(
                    "Spawn_01",
                    RoomType.Corridor,
                    8,
                    6,
                    new[]
                    {
                        "########",
                        "#......#",
                        "#.P....D",
                        "#......#",
                        "#......#",
                        "########"
                    },
                    new[] { new DoorSpec(new Vector2Int(7, 3), DoorDirection.East) },
                    new PlayerSpec(new Vector2Int(2, 3), DoorDirection.East),
                    Array.Empty<EnemySpec>(),
                    Array.Empty<Vector2Int>(),
                    new[] { "spawn", "tutorial", "entrance" }),

                new RoomSpec(
                    "Corridor_Linear_01",
                    RoomType.Corridor,
                    12,
                    4,
                    new[]
                    {
                        "############",
                        "D....M.....D",
                        "D..........D",
                        "############"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(0, 1), DoorDirection.West),
                        new DoorSpec(new Vector2Int(11, 1), DoorDirection.East)
                    },
                    null,
                    new[] { new EnemySpec(new Vector2Int(5, 2), "standard") },
                    Array.Empty<Vector2Int>(),
                    new[] { "corridor", "transit" }),

                new RoomSpec(
                    "Corridor_LShape_01",
                    RoomType.Corridor,
                    10,
                    8,
                    new[]
                    {
                        "##########",
                        "#........#",
                        "#........#",
                        "#........#",
                        "#####....#",
                        "####.....D",
                        "####.....#",
                        "####DD####"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(4, 0), DoorDirection.South),
                        new DoorSpec(new Vector2Int(9, 2), DoorDirection.East)
                    },
                    null,
                    new[]
                    {
                        new EnemySpec(new Vector2Int(7, 4), "standard"),
                        new EnemySpec(new Vector2Int(5, 6), "standard")
                    },
                    Array.Empty<Vector2Int>(),
                    new[] { "corridor", "transit", "l-shape" }),

                new RoomSpec(
                    "Combat_Small_01",
                    RoomType.Combat,
                    8,
                    6,
                    new[]
                    {
                        "###DD###",
                        "#......#",
                        "#.M..M.#",
                        "#..b...#",
                        "#.M....#",
                        "###DD###"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(3, 5), DoorDirection.North),
                        new DoorSpec(new Vector2Int(3, 0), DoorDirection.South)
                    },
                    null,
                    new[]
                    {
                        new EnemySpec(new Vector2Int(2, 3), "standard"),
                        new EnemySpec(new Vector2Int(5, 3), "standard"),
                        new EnemySpec(new Vector2Int(2, 1), "standard")
                    },
                    new[] { new Vector2Int(3, 2) },
                    new[] { "combat", "small", "3-enemy" }),

                // K marks temporary column blockers backed by barrel_001 until column props are produced.
                new RoomSpec(
                    "Combat_Medium_01",
                    RoomType.Combat,
                    12,
                    8,
                    new[]
                    {
                        "####DD######",
                        "#..........#",
                        "#.M.K..K.M.#",
                        "#..........#",
                        "#.bb....bb.#",
                        "#..........#",
                        "#.M.K..K.M.#",
                        "####DD######"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(4, 7), DoorDirection.North),
                        new DoorSpec(new Vector2Int(4, 0), DoorDirection.South),
                        new DoorSpec(new Vector2Int(11, 4), DoorDirection.East)
                    },
                    null,
                    new[]
                    {
                        new EnemySpec(new Vector2Int(2, 6), "standard"),
                        new EnemySpec(new Vector2Int(9, 6), "standard"),
                        new EnemySpec(new Vector2Int(2, 1), "standard"),
                        new EnemySpec(new Vector2Int(9, 1), "standard")
                    },
                    new[]
                    {
                        new Vector2Int(3, 6),
                        new Vector2Int(8, 6),
                        new Vector2Int(3, 1),
                        new Vector2Int(8, 1),
                        new Vector2Int(2, 3),
                        new Vector2Int(9, 3)
                    },
                    new[] { "combat", "medium", "4-enemy", "pillars" }),

                new RoomSpec(
                    "Combat_Large_01",
                    RoomType.Combat,
                    16,
                    10,
                    new[]
                    {
                        "########DD######",
                        "D..............#",
                        "D..............#",
                        "#..M.....M....M#",
                        "#..............#",
                        "#......b.......#",
                        "#..M.....M....M#",
                        "#..............D",
                        "#..............D",
                        "########DD######"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(8, 9), DoorDirection.North),
                        new DoorSpec(new Vector2Int(8, 0), DoorDirection.South),
                        new DoorSpec(new Vector2Int(0, 7), DoorDirection.West),
                        new DoorSpec(new Vector2Int(15, 2), DoorDirection.East)
                    },
                    null,
                    new[]
                    {
                        new EnemySpec(new Vector2Int(3, 6), "standard"),
                        new EnemySpec(new Vector2Int(8, 6), "standard"),
                        new EnemySpec(new Vector2Int(13, 6), "standard"),
                        new EnemySpec(new Vector2Int(3, 3), "standard"),
                        new EnemySpec(new Vector2Int(8, 3), "standard"),
                        new EnemySpec(new Vector2Int(13, 3), "standard")
                    },
                    new[] { new Vector2Int(7, 5) },
                    new[] { "combat", "large", "6-enemy", "open-arena" }),

                new RoomSpec(
                    "Elite_01",
                    RoomType.Elite,
                    10,
                    8,
                    new[]
                    {
                        "####DD####",
                        "#........#",
                        "#..M....M#",
                        "#........#",
                        "#...EE...#",
                        "#...EE...#",
                        "#........#",
                        "####DD####"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(4, 7), DoorDirection.North),
                        new DoorSpec(new Vector2Int(4, 0), DoorDirection.South)
                    },
                    null,
                    new[]
                    {
                        new EnemySpec(new Vector2Int(4, 4), "elite"),
                        new EnemySpec(new Vector2Int(3, 6), "standard"),
                        new EnemySpec(new Vector2Int(7, 6), "standard")
                    },
                    Array.Empty<Vector2Int>(),
                    new[] { "elite", "mid-tier", "central-feature" }),

                // Candle placeholders use barrel_001 until candle props are produced.
                new RoomSpec(
                    "Treasure_01",
                    RoomType.Chest,
                    6,
                    6,
                    new[]
                    {
                        "######",
                        "#.b.b#",
                        "#....#",
                        "#....#",
                        "#.b.b#",
                        "###DD#"
                    },
                    new[] { new DoorSpec(new Vector2Int(3, 0), DoorDirection.South) },
                    null,
                    Array.Empty<EnemySpec>(),
                    new[]
                    {
                        new Vector2Int(1, 4),
                        new Vector2Int(3, 4),
                        new Vector2Int(1, 1),
                        new Vector2Int(3, 1)
                    },
                    new[] { "treasure", "chest", "safe" }),

                // Altar pattern uses barrel_001 placeholders until candle and altar props are produced.
                new RoomSpec(
                    "Shrine_01",
                    RoomType.Curse,
                    8,
                    8,
                    new[]
                    {
                        "########",
                        "#......#",
                        "#..bb..#",
                        "D.b..b.D",
                        "D.b..b.D",
                        "#..bb..#",
                        "#......#",
                        "########"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(0, 3), DoorDirection.West),
                        new DoorSpec(new Vector2Int(7, 3), DoorDirection.East)
                    },
                    null,
                    Array.Empty<EnemySpec>(),
                    new[]
                    {
                        new Vector2Int(3, 5),
                        new Vector2Int(4, 5),
                        new Vector2Int(2, 4),
                        new Vector2Int(5, 4),
                        new Vector2Int(2, 3),
                        new Vector2Int(5, 3),
                        new Vector2Int(3, 2),
                        new Vector2Int(4, 2)
                    },
                    new[] { "shrine", "curse", "no-combat" }),

                new RoomSpec(
                    "Boss_Intro_01",
                    RoomType.Boss,
                    14,
                    10,
                    new[]
                    {
                        "######DD######",
                        "#............#",
                        "#............#",
                        "#.....BB.....#",
                        "#.....BB.....#",
                        "#............#",
                        "#............#",
                        "#.....P......#",
                        "#............#",
                        "######DD######"
                    },
                    new[]
                    {
                        new DoorSpec(new Vector2Int(6, 9), DoorDirection.North),
                        new DoorSpec(new Vector2Int(6, 0), DoorDirection.South)
                    },
                    new PlayerSpec(new Vector2Int(6, 2), DoorDirection.North),
                    new[] { new EnemySpec(new Vector2Int(6, 6), "boss") },
                    Array.Empty<Vector2Int>(),
                    new[] { "boss", "intro", "dramatic" })
            };
        }

        private readonly struct RoomSpec
        {
            public readonly string RoomId;
            public readonly RoomType RoomType;
            public readonly int Width;
            public readonly int Height;
            public readonly string[] Layout;
            public readonly DoorSpec[] Doors;
            public readonly PlayerSpec? PlayerSpawn;
            public readonly EnemySpec[] Enemies;
            public readonly Vector2Int[] Props;
            public readonly string[] Tags;

            public RoomSpec(
                string roomId,
                RoomType roomType,
                int width,
                int height,
                string[] layout,
                DoorSpec[] doors,
                PlayerSpec? playerSpawn,
                EnemySpec[] enemies,
                Vector2Int[] props,
                string[] tags)
            {
                RoomId = roomId;
                RoomType = roomType;
                Width = width;
                Height = height;
                Layout = layout;
                Doors = doors;
                PlayerSpawn = playerSpawn;
                Enemies = enemies;
                Props = props;
                Tags = tags;
            }
        }

        private readonly struct DoorSpec
        {
            public readonly Vector2Int Position;
            public readonly DoorDirection Direction;

            public DoorSpec(Vector2Int position, DoorDirection direction)
            {
                Position = position;
                Direction = direction;
            }
        }

        private readonly struct PlayerSpec
        {
            public readonly Vector2Int Position;
            public readonly DoorDirection Facing;

            public PlayerSpec(Vector2Int position, DoorDirection facing)
            {
                Position = position;
                Facing = facing;
            }
        }

        private readonly struct EnemySpec
        {
            public readonly Vector2Int Position;
            public readonly string TierHint;

            public EnemySpec(Vector2Int position, string tierHint)
            {
                Position = position;
                TierHint = tierHint;
            }
        }
    }
}
#endif
