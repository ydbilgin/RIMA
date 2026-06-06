using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.DebugTools;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace RIMA.Tests
{
    public sealed class ScreenshotModePlayModeTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            ScreenshotMode.SetEnabled(false);
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                {
                    Object.Destroy(created[i]);
                }
            }
            created.Clear();
        }

        [UnityTest]
        public IEnumerator IsoRoomBuilder_RuntimeMarkersRegisterWithScreenshotMode()
        {
            int before = ScreenshotMode.RegisteredCount;
            IsoRoomBuilder builder = CreateBuilder(out _);
            RoomTemplateSO template = CreateTemplate();

            builder.Build(template);
            yield return null;

            Assert.Greater(ScreenshotMode.RegisteredCount, before);
            ScreenshotMode.SetEnabled(true);
            Assert.IsFalse(builder.PlayerSpawnMarker.gameObject.activeInHierarchy);

            ScreenshotMode.SetEnabled(false);
            Assert.IsTrue(builder.PlayerSpawnMarker.gameObject.activeInHierarchy);
        }

        private IsoRoomBuilder CreateBuilder(out Tilemap groundTilemap)
        {
            GameObject gridObject = new GameObject("ScreenshotModePlayModeTestGrid");
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
            template.roomId = "screenshot_mode_playmode_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 4, 4);
            template.playerSpawn = new PlayerSpawnSocket { position = new Vector2Int(1, 1) };
            template.enemySpawnSockets = new List<EnemySpawnSocket>
            {
                new EnemySpawnSocket { socketId = "a", position = new Vector2Int(2, 2) }
            };
            return template;
        }

        private static void SetPrivate(object target, string fieldName, object value)
        {
            target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(target, value);
        }
    }
}
