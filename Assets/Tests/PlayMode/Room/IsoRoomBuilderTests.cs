using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Room
{
    public class IsoRoomBuilderTests
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
        public void Build_CodeBuiltGrid_PaintsFloorAndSpawnMarker()
        {
            IsoRoomBuilder builder = CreateRig(out Tilemap groundTilemap);
            RoomTemplateSO template = CreateTemplate();

            builder.Build(template);

            Assert.IsNotNull(builder.LastFloorCells);
            Assert.AreEqual(25, builder.LastFloorCells.Count);
            Assert.IsNotNull(groundTilemap.GetTile(new Vector3Int(2, 2, 0)));
            Assert.IsNotNull(builder.PlayerSpawnMarker);
        }

        [Test]
        public void BuildExitDoors_ReturnsOneObjectPerDoorType()
        {
            IsoRoomBuilder builder = CreateRig(out _);
            RoomTemplateSO template = CreateTemplate();
            var texture = new Texture2D(4, 4);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
            created.Add(sprite);
            created.Add(texture);
            SetPrivate(builder, "gateNorthSprite", sprite);

            builder.Build(template);
            List<GameObject> doors = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Combat, RIMA.RoomType.Elite });

            Assert.AreEqual(2, doors.Count);
            Assert.AreEqual("ExitDoor_0_Combat", doors[0].name);
            Assert.AreEqual("ExitDoor_1_Elite", doors[1].name);
            Assert.AreEqual(0.5f, doors[0].transform.position.x, 0.01f);
            Assert.AreEqual(4.5f, doors[1].transform.position.x, 0.01f);
            Assert.That(doors[0].transform.position.x, Is.Not.EqualTo(2.5f).Within(0.01f));
            Assert.That(doors[1].transform.position.x, Is.Not.EqualTo(2.5f).Within(0.01f));
        }

        [Test]
        public void BuildExitDoors_UsesAuthoredSlotMappingForOneTwoAndThreeDoors()
        {
            IsoRoomBuilder builder = CreateRig(out _);
            RoomTemplateSO template = CreateTemplate();
            var texture = new Texture2D(4, 4);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
            created.Add(sprite);
            created.Add(texture);
            SetPrivate(builder, "gateNorthSprite", sprite);

            builder.Build(template);

            List<GameObject> oneDoor = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Combat });
            Assert.AreEqual(1, oneDoor.Count);
            Assert.AreEqual(2.5f, oneDoor[0].transform.position.x, 0.01f);

            List<GameObject> twoDoors = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Combat, RIMA.RoomType.Elite });
            Assert.AreEqual(2, twoDoors.Count);
            Assert.AreEqual(0.5f, twoDoors[0].transform.position.x, 0.01f);
            Assert.AreEqual(4.5f, twoDoors[1].transform.position.x, 0.01f);

            List<GameObject> threeDoors = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Combat, RIMA.RoomType.Elite, RIMA.RoomType.Event });
            Assert.AreEqual(3, threeDoors.Count);
            Assert.AreEqual(0.5f, threeDoors[0].transform.position.x, 0.01f);
            Assert.AreEqual(2.5f, threeDoors[1].transform.position.x, 0.01f);
            Assert.AreEqual(4.5f, threeDoors[2].transform.position.x, 0.01f);
        }

        private static void SetPrivate(object o, string field, object val)
        {
            o.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(o, val);
        }

        private IsoRoomBuilder CreateRig(out Tilemap groundTilemap)
        {
            GameObject gridObject = new GameObject("TestGrid");
            created.Add(gridObject);
            Grid grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = new GameObject("GroundTilemap");
            created.Add(groundObject);
            groundObject.transform.SetParent(gridObject.transform, false);
            groundTilemap = groundObject.AddComponent<Tilemap>();

            GameObject collisionObject = new GameObject("CollisionTilemap");
            created.Add(collisionObject);
            collisionObject.transform.SetParent(gridObject.transform, false);
            Tilemap collisionTilemap = collisionObject.AddComponent<Tilemap>();

            GameObject builderObject = new GameObject("IsoRoomBuilder");
            created.Add(builderObject);
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
            created.Add(template);
            template.roomId = "test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 5, 5);
            template.walkableGrid = null;
            template.playerSpawn = new PlayerSpawnSocket { position = new Vector2Int(2, 2) };
            template.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthWestId, position = new Vector2Int(0, 4), direction = RIMA.DoorDirection.North, widthInTiles = 2, isExit = true },
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthId, position = new Vector2Int(2, 4), direction = RIMA.DoorDirection.North, widthInTiles = 2, isExit = true },
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthEastId, position = new Vector2Int(4, 4), direction = RIMA.DoorDirection.North, widthInTiles = 2, isExit = true }
            };
            return template;
        }
    }
}
