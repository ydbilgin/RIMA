using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Composition;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Composition
{
    public class WangContextResolverTests
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
        public void ResolveCaseAt_FullySurroundedCorners_ReturnsAllOnes()
        {
            var tilemap = CreateTilemap("Wang_Probe1");
            var tile = ScriptableObject.CreateInstance<Tile>();
            _cleanup.Add(tile);

            // pos (5,5) itself must be a wall cell per spec contract
            tilemap.SetTile(new Vector3Int(5, 5, 0), tile);
            // Place walls at all 4 corners around (5, 5)
            tilemap.SetTile(new Vector3Int(6, 6, 0), tile); // NE
            tilemap.SetTile(new Vector3Int(4, 6, 0), tile); // NW
            tilemap.SetTile(new Vector3Int(6, 4, 0), tile); // SE
            tilemap.SetTile(new Vector3Int(4, 4, 0), tile); // SW

            var resolver = new WangContextResolver();
            string result = resolver.ResolveCaseAt(new Vector2Int(5, 5), tilemap);
            Assert.AreEqual("wang_1111", result);
        }

        [Test]
        public void ResolveCaseAt_LShapeCorner_ReturnsExpectedCase()
        {
            var tilemap = CreateTilemap("Wang_LShape");
            var tile = ScriptableObject.CreateInstance<Tile>();
            _cleanup.Add(tile);

            // pos (1,1) itself must be a wall cell per spec contract
            tilemap.SetTile(new Vector3Int(1, 1, 0), tile);
            // L-shape: SE and SW corners have walls, NE+NW empty
            tilemap.SetTile(new Vector3Int(2, 0, 0), tile); // SE corner of (1,1)
            tilemap.SetTile(new Vector3Int(0, 0, 0), tile); // SW corner of (1,1)

            var resolver = new WangContextResolver();
            string result = resolver.ResolveCaseAt(new Vector2Int(1, 1), tilemap);
            // NE=0, NW=0, SE=1, SW=1
            Assert.AreEqual("wang_0011", result);
        }

        [Test]
        public void ResolveCaseAt_PosNotWallCell_ReturnsNull()
        {
            var tilemap = CreateTilemap("Wang_NonWallPos");
            var resolver = new WangContextResolver();
            // pos (2,2) itself has no tile → spec contract: return null
            string result = resolver.ResolveCaseAt(new Vector2Int(2, 2), tilemap);
            Assert.IsNull(result, "Spec: returns null if pos itself is not a wall cell.");
        }

        [Test]
        public void ResolveCaseAt_NullTilemap_ReturnsNull()
        {
            var resolver = new WangContextResolver();
            Assert.IsNull(resolver.ResolveCaseAt(new Vector2Int(0, 0), null));
        }

        [Test]
        public void PickVariantForCase_MatchByVariantId_ReturnsExactMatch()
        {
            var resolver = new WangContextResolver();
            var v1 = new BrushAssetVariant { variantId = "wang_0000" };
            var v2 = new BrushAssetVariant { variantId = "wang_1111" };
            var v3 = new BrushAssetVariant { variantId = "wang_0011" };
            var candidates = new List<BrushAssetVariant> { v1, v2, v3 };

            var picked = resolver.PickVariantForCase("wang_1111", candidates);
            Assert.AreSame(v2, picked);

            var picked2 = resolver.PickVariantForCase("wang_0011", candidates);
            Assert.AreSame(v3, picked2);
        }

        [Test]
        public void PickVariantForCase_NoMatch_ReturnsFirstCandidate()
        {
            var resolver = new WangContextResolver();
            var v1 = new BrushAssetVariant { variantId = "wang_0000" };
            var v2 = new BrushAssetVariant { variantId = "wang_1111" };
            var candidates = new List<BrushAssetVariant> { v1, v2 };

            var picked = resolver.PickVariantForCase("wang_0011", candidates);
            Assert.AreSame(v1, picked, "Fallback to first candidate when no match.");
        }

        [Test]
        public void PickVariantForCase_EmptyCandidates_ReturnsNull()
        {
            var resolver = new WangContextResolver();
            Assert.IsNull(resolver.PickVariantForCase("wang_0011", new List<BrushAssetVariant>()));
            Assert.IsNull(resolver.PickVariantForCase("wang_0011", null));
        }

        private Tilemap CreateTilemap(string name)
        {
            var gridGO = new GameObject(name + "_Grid");
            _cleanup.Add(gridGO);
            gridGO.AddComponent<Grid>();
            var tmGO = new GameObject(name);
            _cleanup.Add(tmGO);
            tmGO.transform.SetParent(gridGO.transform, false);
            var tilemap = tmGO.AddComponent<Tilemap>();
            tmGO.AddComponent<TilemapRenderer>();
            return tilemap;
        }
    }
}
