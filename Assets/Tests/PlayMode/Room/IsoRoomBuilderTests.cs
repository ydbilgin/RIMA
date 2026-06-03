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
            return template;
        }
    }
}
