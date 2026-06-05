using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Room
{
    public class IsoRoomBuilderOverlayTests
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
        public void OverlayMask_RoundTripsLikeWalkableGrid()
        {
            RoomTemplateSO template = CreateTemplate(3, 2);
            template.walkableGrid = new[] { true, false, true, true, true, false };
            template.overlayMask = new[] { 0, 1, 0, 2, 0, 1 };

            string json = EditorJsonUtility.ToJson(template);
            RoomTemplateSO copy = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(copy);
            EditorJsonUtility.FromJsonOverwrite(json, copy);

            Assert.AreEqual(template.walkableGrid, copy.walkableGrid);
            Assert.AreEqual(template.overlayMask, copy.overlayMask);
            Assert.AreEqual(2, copy.GetOverlayTileIndex(new Vector2Int(0, 1)));

            RoomTemplateSO defaultTemplate = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(defaultTemplate);
            Assert.AreEqual(0, defaultTemplate.GetOverlayTileIndex(Vector2Int.zero));
        }

        [Test]
        public void Build_EmptyOverlayMask_DoesNotCreateOverlayTilemap()
        {
            IsoRoomBuilder builder = CreateRig(out Grid grid, out _, out _);
            SetPrivate(builder, "overlayTiles", new TileBase[] { CreateTile("overlay") });
            RoomTemplateSO template = CreateTemplate(3, 3);
            template.overlayMask = null;

            builder.Build(template);

            Assert.IsNull(FindTilemap(grid, "OverlayTilemap"));
        }

        [Test]
        public void Build_MaskedOverlay_PaintsCellsAboveGround()
        {
            IsoRoomBuilder builder = CreateRig(out Grid grid, out _, out TilemapRenderer groundRenderer);
            Tile overlayTile = CreateTile("overlay");
            SetPrivate(builder, "overlayTiles", new TileBase[] { overlayTile });
            groundRenderer.sortingOrder = 7;
            RoomTemplateSO template = CreateTemplate(3, 3);
            template.overlayMask = new[]
            {
                0, 0, 0,
                0, 1, 0,
                1, 0, 0
            };

            builder.Build(template);

            Tilemap overlayTilemap = FindTilemap(grid, "OverlayTilemap");
            Assert.IsNotNull(overlayTilemap);
            Assert.AreSame(overlayTile, overlayTilemap.GetTile(new Vector3Int(1, 1, 0)));
            Assert.AreSame(overlayTile, overlayTilemap.GetTile(new Vector3Int(0, 2, 0)));
            Assert.IsNull(overlayTilemap.GetTile(new Vector3Int(2, 2, 0)));

            TilemapRenderer overlayRenderer = overlayTilemap.GetComponent<TilemapRenderer>();
            Assert.AreEqual(groundRenderer.sortingLayerID, overlayRenderer.sortingLayerID);
            Assert.AreEqual(groundRenderer.sortingOrder + 1, overlayRenderer.sortingOrder);
        }

        private IsoRoomBuilder CreateRig(out Grid grid, out Tilemap groundTilemap, out TilemapRenderer groundRenderer)
        {
            GameObject gridObject = new GameObject("TestGrid");
            created.Add(gridObject);
            grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = new GameObject("GroundTilemap");
            created.Add(groundObject);
            groundObject.transform.SetParent(gridObject.transform, false);
            groundTilemap = groundObject.AddComponent<Tilemap>();
            groundRenderer = groundObject.AddComponent<TilemapRenderer>();

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
            SetPrivate(builder, "floorTile", CreateTile("floor"));
            SetPrivate(builder, "collisionTile", CreateTile("collision"));

            return builder;
        }

        private Tile CreateTile(string name)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = name;
            created.Add(tile);
            return tile;
        }

        private RoomTemplateSO CreateTemplate(int width, int height)
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(template);
            template.roomId = "overlay_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, width, height);
            template.walkableGrid = null;
            template.playerSpawn = new PlayerSpawnSocket { position = Vector2Int.zero };
            return template;
        }

        private static Tilemap FindTilemap(Grid grid, string tilemapName)
        {
            Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>(true);
            for (int i = 0; i < tilemaps.Length; i++)
            {
                if (tilemaps[i].name == tilemapName)
                {
                    return tilemaps[i];
                }
            }

            return null;
        }

        private static void SetPrivate(object target, string fieldName, object value)
        {
            target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }
    }
}
