using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Composition
{
    public class CompositionPainterIntegrationTests
    {
        private List<Object> _cleanup;

        [SetUp]
        public void SetUp() { _cleanup = new List<Object>(); }

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _cleanup)
            {
                if (o != null) Object.DestroyImmediate(o);
            }
        }

        [Test]
        public void ContextAware_WallBand_PlacesSprite()
        {
            var painter = CreatePainter();
            var room = CreateRoom();
            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);
            var resolver = new WangContextResolver();
            var candidates = CreateCandidates(out Sprite expectedSprite);

            // (0, 0) is a perimeter / WallBand cell in the 10x10 room
            var wall = painter.PlaceWallSprite_ContextAware_WithCandidates(
                new Vector2Int(0, 0),
                map,
                resolver,
                walkableMask: null,
                candidates: candidates,
                baseTilemap: null);

            Assert.IsNotNull(wall, "Wall sprite GO must be placed at WallBand cell.");
            _cleanup.Add(wall);

            var sr = wall.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr);
            Assert.AreSame(expectedSprite, sr.sprite);
        }

        [Test]
        public void ContextAware_CleanCenter_DoesNotPlace()
        {
            var painter = CreatePainter();
            var room = CreateRoom();
            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);
            var resolver = new WangContextResolver();
            var candidates = CreateCandidates(out _);

            // (5, 5) is CleanCenter for a 10x10 room
            var wall = painter.PlaceWallSprite_ContextAware_WithCandidates(
                new Vector2Int(5, 5),
                map,
                resolver,
                walkableMask: null,
                candidates: candidates,
                baseTilemap: null);

            Assert.IsNull(wall, "CleanCenter cell must NOT trigger wall placement.");
        }

        [Test]
        public void ContextAware_EmptyCandidates_ReturnsNull()
        {
            var painter = CreatePainter();
            var room = CreateRoom();
            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);
            var resolver = new WangContextResolver();

            var wall = painter.PlaceWallSprite_ContextAware_WithCandidates(
                new Vector2Int(0, 0),
                map,
                resolver,
                walkableMask: null,
                candidates: new List<BrushAssetVariant>(),
                baseTilemap: null);

            Assert.IsNull(wall);
        }

        private WallOverlayPainter CreatePainter()
        {
            var go = new GameObject("WallOverlayPainter_Composition_Test");
            _cleanup.Add(go);
            return go.AddComponent<WallOverlayPainter>();
        }

        private RoomTemplateSO CreateRoom()
        {
            var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
            _cleanup.Add(room);
            room.roomId = "composition_test_001";
            room.biomeId = "TestBiome";
            room.bounds = new RectInt(0, 0, 10, 10);
            room.doorSockets = new List<DoorSocket>();
            return room;
        }

        private List<BrushAssetVariant> CreateCandidates(out Sprite expected)
        {
            var tex = new Texture2D(2, 2);
            _cleanup.Add(tex);
            expected = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 32f);
            _cleanup.Add(expected);

            var variant = new BrushAssetVariant
            {
                variantId = "wang_0000",
                sprite = expected,
                bucket = SizeBucket.Medium,
                weight = 1f
            };

            return new List<BrushAssetVariant> { variant };
        }
    }
}
