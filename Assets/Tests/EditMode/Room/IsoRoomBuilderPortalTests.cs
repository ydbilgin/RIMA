#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Room
{
    public sealed class IsoRoomBuilderPortalTests
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
        public void BuildExitDoors_MapsSlotsToFacingAndOnlyFlipsAngledVisual()
        {
            IsoRoomBuilder builder = CreateRig();
            Sprite combatFrontal = CreateSprite("combat_frontal");
            Sprite combatAngled = CreateSprite("combat_angled");
            Sprite eliteAngled = CreateSprite("elite_angled");
            SetPrivate(builder, "portalCombatFrontalSprite", combatFrontal);
            SetPrivate(builder, "portalCombatAngledSprite", combatAngled);
            SetPrivate(builder, "portalEliteAngledSprite", eliteAngled);
            SetPrivate(builder, "runeCombatSprite", CreateSprite("rune_combat"));
            SetPrivate(builder, "runeEliteSprite", CreateSprite("rune_elite"));

            builder.Build(CreateTemplate());

            List<GameObject> doors = builder.BuildExitDoors(new List<RIMA.RoomType>
            {
                RIMA.RoomType.Combat,
                RIMA.RoomType.Combat,
                RIMA.RoomType.Elite
            });

            Assert.AreEqual(3, doors.Count);
            AssertDoorVisual(doors[0], combatAngled, false);
            AssertDoorVisual(doors[1], combatFrontal, false);
            AssertDoorVisual(doors[2], eliteAngled, true);

            SpriteRenderer runeRenderer = doors[2].transform.Find("Rune").GetComponent<SpriteRenderer>();
            Assert.IsFalse(runeRenderer.flipX);
            Assert.AreEqual(Vector3.one, runeRenderer.transform.localScale);
        }

        [Test]
        public void BuildExitDoors_DefaultsUnsupportedRoomTypeToCombatVisual()
        {
            IsoRoomBuilder builder = CreateRig();
            Sprite combatFrontal = CreateSprite("combat_frontal");
            SetPrivate(builder, "portalCombatFrontalSprite", combatFrontal);

            builder.Build(CreateTemplate());

            List<GameObject> doors = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Event });

            Assert.AreEqual(1, doors.Count);
            AssertDoorVisual(doors[0], combatFrontal, false);
        }

        [Test]
        public void VisualScaleChange_DoesNotChangeRootScaleOrColliderBounds()
        {
            IsoRoomBuilder builder = CreateRig();
            SetPrivate(builder, "portalCombatFrontalSprite", CreateSprite("combat_frontal"));

            builder.Build(CreateTemplate());
            GameObject door = builder.BuildExitDoors(new List<RIMA.RoomType> { RIMA.RoomType.Combat })[0];
            BoxCollider2D collider = door.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(1.25f, 0.75f);
            Physics2D.SyncTransforms();

            Vector3 rootScale = door.transform.localScale;
            Bounds bounds = collider.bounds;

            Transform visual = door.transform.Find("Visual");
            visual.localScale = new Vector3(1.5f, 1.5f, 1f);
            Physics2D.SyncTransforms();

            Assert.AreEqual(rootScale, door.transform.localScale);
            Assert.AreEqual(bounds.center, collider.bounds.center);
            Assert.AreEqual(bounds.size, collider.bounds.size);
        }

        private static void AssertDoorVisual(GameObject door, Sprite expectedSprite, bool expectedFlipX)
        {
            Transform visual = door.transform.Find("Visual");
            Assert.IsNotNull(visual);
            SpriteRenderer renderer = visual.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            Assert.AreEqual(expectedSprite, renderer.sprite);
            Assert.AreEqual(expectedFlipX, renderer.flipX);
        }

        private Sprite CreateSprite(string name)
        {
            Texture2D texture = new Texture2D(4, 4);
            texture.name = name + "_texture";
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
            sprite.name = name;
            created.Add(sprite);
            created.Add(texture);
            return sprite;
        }

        private static void SetPrivate(object target, string field, object value)
        {
            target.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }

        private IsoRoomBuilder CreateRig()
        {
            GameObject gridObject = new GameObject("TestGrid");
            created.Add(gridObject);
            Grid grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = new GameObject("GroundTilemap");
            created.Add(groundObject);
            groundObject.transform.SetParent(gridObject.transform, false);
            Tilemap groundTilemap = groundObject.AddComponent<Tilemap>();

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
            template.roomId = "portal_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 5, 5);
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
#endif
