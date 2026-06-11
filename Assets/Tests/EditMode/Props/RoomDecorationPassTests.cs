#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Props
{
    public sealed class RoomDecorationPassTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                {
                    Object.DestroyImmediate(created[i]);
                }
            }

            created.Clear();
        }

        [Test]
        public void Build_DefaultAutoDecorationFlagOff_SpawnsNoDecor()
        {
            IsoRoomBuilder builder = CreateBuilderRig();
            SetPrivate(builder, "decorationRegistry", CreateRegistry());

            builder.Build(CreateTemplate());

            Transform decorations = builder.transform.Find("Decorations");
            Assert.IsNotNull(decorations);
            Assert.AreEqual(0, decorations.childCount);
        }

        [Test]
        public void Apply_SpawnsNonBlockingDecorAndKeepsDoorAndSpawnClear()
        {
            RoomTemplateSO template = CreateTemplate();
            PropRegistrySO registry = CreateRegistry();
            GameObject parent = CreateGameObject("DecorParent");
            HashSet<Vector3Int> floorCells = CreateFloorCells(template);
            bool[] originalWalkable = (bool[])template.walkableGrid.Clone();

            int placed = RoomDecorationPass.Apply(template, floorCells, null, parent.transform, registry, 12345, 1f);

            Assert.Greater(placed, 0);
            Assert.AreEqual(placed, parent.transform.childCount);
            CollectionAssert.AreEqual(originalWalkable, template.walkableGrid);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject decor = parent.transform.GetChild(i).gameObject;
                Assert.IsNotNull(decor.GetComponent<SpriteRenderer>());
                Assert.IsNotNull(decor.GetComponent<PropSorterRuntime>());
                Assert.IsNull(decor.GetComponent<Collider2D>());

                Vector2Int tile = ToTile(decor.transform.localPosition);
                Assert.IsFalse(IsInsideDoorSafety(template, tile), $"Decor spawned in DoorSafety at {tile}.");
                Assert.Greater(Mathf.Abs(tile.x - template.playerSpawn.position.x) + Mathf.Abs(tile.y - template.playerSpawn.position.y), 1);
                Assert.Greater(Mathf.Abs(tile.x - template.enemySpawnSockets[0].position.x) + Mathf.Abs(tile.y - template.enemySpawnSockets[0].position.y), 2);
            }
        }

        [Test]
        public void Apply_SameSeedProducesSamePositions()
        {
            RoomTemplateSO template = CreateTemplate();
            PropRegistrySO registry = CreateRegistry();
            HashSet<Vector3Int> floorCells = CreateFloorCells(template);
            GameObject parentA = CreateGameObject("DecorA");
            GameObject parentB = CreateGameObject("DecorB");

            int countA = RoomDecorationPass.Apply(template, floorCells, null, parentA.transform, registry, 9001, 1f);
            int countB = RoomDecorationPass.Apply(template, floorCells, null, parentB.transform, registry, 9001, 1f);

            Assert.AreEqual(countA, countB);
            Assert.AreEqual(Signature(parentA.transform), Signature(parentB.transform));
        }

        [Test]
        public void Apply_BlockingPropDefinitionDoesNotAddCollidersOrChangeWalkableGrid()
        {
            RoomTemplateSO template = CreateTemplate();
            PropRegistrySO registry = CreateRegistry();
            GameObject parent = CreateGameObject("DecorParent");
            HashSet<Vector3Int> floorCells = CreateFloorCells(template);
            bool[] originalWalkable = (bool[])template.walkableGrid.Clone();

            int placed = RoomDecorationPass.Apply(template, floorCells, null, parent.transform, registry, 77, 1f);

            Assert.Greater(placed, 0);
            CollectionAssert.AreEqual(originalWalkable, template.walkableGrid);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Assert.IsNull(parent.transform.GetChild(i).GetComponent<Collider2D>());
            }
        }

        private IsoRoomBuilder CreateBuilderRig()
        {
            GameObject gridObject = CreateGameObject("TestGrid");
            Grid grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = CreateGameObject("GroundTilemap");
            groundObject.transform.SetParent(gridObject.transform, false);
            Tilemap groundTilemap = groundObject.AddComponent<Tilemap>();

            GameObject collisionObject = CreateGameObject("CollisionTilemap");
            collisionObject.transform.SetParent(gridObject.transform, false);
            Tilemap collisionTilemap = collisionObject.AddComponent<Tilemap>();

            GameObject builderObject = CreateGameObject("IsoRoomBuilder");
            IsoRoomBuilder builder = builderObject.AddComponent<IsoRoomBuilder>();
            SetPrivate(builder, "grid", grid);
            SetPrivate(builder, "groundTilemap", groundTilemap);
            SetPrivate(builder, "collisionTilemap", collisionTilemap);

            Tile floorTile = ScriptableObject.CreateInstance<Tile>();
            Tile collisionTile = ScriptableObject.CreateInstance<Tile>();
            created.Add(floorTile);
            created.Add(collisionTile);
            SetPrivate(builder, "floorTile", floorTile);
            SetPrivate(builder, "collisionTile", collisionTile);

            return builder;
        }

        private RoomTemplateSO CreateTemplate()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.roomId = "decoration_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 14, 14);
            template.walkableGrid = new bool[template.bounds.width * template.bounds.height];
            for (int i = 0; i < template.walkableGrid.Length; i++)
            {
                template.walkableGrid[i] = true;
            }

            template.props = new List<PropPlacementData>();
            template.playerSpawn = new PlayerSpawnSocket { position = new Vector2Int(7, 3) };
            template.enemySpawnSockets = new List<EnemySpawnSocket>
            {
                new EnemySpawnSocket { socketId = "enemy_a", position = new Vector2Int(4, 6), avoidRadius = 2f }
            };
            template.doorSockets = new List<DoorSocket>
            {
                new DoorSocket
                {
                    socketId = RoomTemplateSO.ExitSlotNorthId,
                    position = new Vector2Int(7, 13),
                    direction = RIMA.DoorDirection.North,
                    widthInTiles = 2,
                    isExit = true
                }
            };
            created.Add(template);
            return template;
        }

        private PropRegistrySO CreateRegistry()
        {
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "decor_test_prop";
            prop.footprintSize = Vector2Int.one;
            prop.blocksWalkable = true;
            prop.requiresWalkableTile = true;
            prop.colliderShape = PropDefinitionSO.ColliderShape.Box;
            prop.distanceFromOtherProps = 1f;
            prop.worldSprite = CreateSprite();
            registry.EditorAddProp(prop);
            registry.RebuildIndex();
            created.Add(prop);
            created.Add(registry);
            return registry;
        }

        private Sprite CreateSprite()
        {
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.SetPixel(1, 0, Color.white);
            texture.SetPixel(0, 1, Color.white);
            texture.SetPixel(1, 1, Color.white);
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 16f);
            created.Add(sprite);
            created.Add(texture);
            return sprite;
        }

        private static HashSet<Vector3Int> CreateFloorCells(RoomTemplateSO template)
        {
            HashSet<Vector3Int> cells = new HashSet<Vector3Int>();
            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int tile = new Vector2Int(x, y);
                    if (template.IsWalkable(tile))
                    {
                        cells.Add(new Vector3Int(x, y, 0));
                    }
                }
            }

            return cells;
        }

        private static bool IsInsideDoorSafety(RoomTemplateSO template, Vector2Int tile)
        {
            for (int i = 0; i < template.doorSockets.Count; i++)
            {
                Vector2Int door = template.doorSockets[i].position;
                if (Mathf.Abs(tile.x - door.x) + Mathf.Abs(tile.y - door.y) <= 3)
                {
                    return true;
                }
            }

            return false;
        }

        private static string Signature(Transform parent)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < parent.childCount; i++)
            {
                Vector3 pos = parent.GetChild(i).localPosition;
                builder.Append(Mathf.RoundToInt(pos.x)).Append(',').Append(Mathf.RoundToInt(pos.y)).Append('|');
            }

            return builder.ToString();
        }

        private static Vector2Int ToTile(Vector3 position)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private GameObject CreateGameObject(string name)
        {
            GameObject go = new GameObject(name);
            created.Add(go);
            return go;
        }

        private static void SetPrivate(object target, string field, object value)
        {
            target.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }
    }
}
#endif
