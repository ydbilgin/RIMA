using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.Systems.Map;
using UnityEngine;

namespace RIMA.Tests.Editor
{
    public sealed class Karar143Asama1Tests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null)
                {
                    Object.DestroyImmediate(cleanup[i]);
                }
            }

            cleanup.Clear();
        }

        [Test]
        public void ProceduralRoomGenerator_OutputsExpectedPerimeterWallEdges()
        {
            RoomRecipe recipe = CreateRecipe(8, 6);

            RoomData room = ProceduralRoomGenerator.Generate(recipe, 123);

            Assert.NotNull(room.wallEdges);
            Assert.AreEqual(20, room.wallEdges.Count);
        }

        [Test]
        public void WallOverlayPainter_PlacesWallSpritesOutsideWalkableCells()
        {
            RoomData room = CreateSimpleRoom(5, 5);
            WallBrushSetSO brushSet = ScriptableObject.CreateInstance<WallBrushSetSO>();
            cleanup.Add(brushSet);
            Sprite sprite = CreateSprite();
            brushSet.horizontal.Add(sprite);
            brushSet.vertical.Add(sprite);
            brushSet.corner.Add(sprite);

            GameObject host = new GameObject("WallOverlayPainter_Test");
            cleanup.Add(host);
            WallOverlayPainter painter = host.AddComponent<WallOverlayPainter>();

            painter.PaintWalls(room, brushSet, null, 42);

            SpriteRenderer[] renderers = host.GetComponentsInChildren<SpriteRenderer>();
            Assert.Greater(renderers.Length, 0);
            foreach (SpriteRenderer renderer in renderers)
            {
                int x = Mathf.FloorToInt(renderer.transform.position.x);
                int y = Mathf.FloorToInt(renderer.transform.position.y);
                bool onWalkable = x >= 0 && y >= 0 && x < room.walkable.GetLength(0) && y < room.walkable.GetLength(1) && room.walkable[x, y];
                Assert.IsFalse(onWalkable, "Wall sprite landed on walkable cell " + new Vector2Int(x, y));
            }
        }

        [Test]
        public void DetailDecalPainter_DoesNotPaintNonWalkableCells()
        {
            RoomData room = CreateSimpleRoom(4, 4);
            PatchAtlasSO atlas = ScriptableObject.CreateInstance<PatchAtlasSO>();
            cleanup.Add(atlas);
            atlas.patches.Add(new PatchEntry
            {
                sprite = CreateSprite(),
                density = 1f
            });

            GameObject host = new GameObject("DetailDecalPainter_Test");
            cleanup.Add(host);
            DetailDecalPainter painter = host.AddComponent<DetailDecalPainter>();

            painter.PaintDetails(null, room, atlas, 77);

            SpriteRenderer[] renderers = host.GetComponentsInChildren<SpriteRenderer>();
            Assert.AreEqual(4, renderers.Length);
            foreach (SpriteRenderer renderer in renderers)
            {
                int x = Mathf.FloorToInt(renderer.transform.position.x);
                int y = Mathf.FloorToInt(renderer.transform.position.y);
                Assert.IsTrue(room.walkable[x, y], "Decal landed on non-walkable cell " + new Vector2Int(x, y));
            }
        }

        [Test]
        public void EdgeBiasedDensity_WallNearCellsBeatCenterCells()
        {
            RoomData room = CreateSimpleRoom(10, 10);

            float nearWall = DetailDecalPainter.DensityForCell(new Vector2Int(1, 1), room, 0.08f);
            float center = DetailDecalPainter.DensityForCell(new Vector2Int(5, 5), room, 0.08f);

            Assert.Greater(nearWall, center);
            Assert.AreEqual(0f, DetailDecalPainter.DensityForCell(new Vector2Int(0, 0), room, 0.08f));
        }

        private RoomRecipe CreateRecipe(int width, int height)
        {
            RoomRecipe recipe = ScriptableObject.CreateInstance<RoomRecipe>();
            cleanup.Add(recipe);
            recipe.size = new Vector2Int(width, height);
            recipe.allowedTerrains.Add(CreateTerrainDefinition(1, true));
            recipe.allowedTerrains.Add(CreateTerrainDefinition(3, true));
            recipe.allowedTerrains.Add(CreateTerrainDefinition(2, false));
            return recipe;
        }

        private TerrainDefinition CreateTerrainDefinition(int id, bool walkable)
        {
            TerrainDefinition terrain = ScriptableObject.CreateInstance<TerrainDefinition>();
            cleanup.Add(terrain);
            terrain.terrainId = id;
            terrain.walkable = walkable;
            terrain.collisionType = walkable ? TerrainCollisionType.None : TerrainCollisionType.Wall;
            return terrain;
        }

        private RoomData CreateSimpleRoom(int width, int height)
        {
            bool[,] walkable = new bool[width, height];
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    walkable[x, y] = true;
                }
            }

            return new RoomData
            {
                size = new Vector2Int(width, height),
                walkable = walkable,
                wallEdges = ProceduralRoomGenerator.BuildWallEdges(walkable, width, height)
            };
        }

        private Sprite CreateSprite()
        {
            Texture2D texture = new Texture2D(2, 2);
            cleanup.Add(texture);
            texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 32f);
            cleanup.Add(sprite);
            return sprite;
        }
    }
}
