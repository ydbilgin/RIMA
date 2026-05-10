using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using RIMA.Editor.RoomDesigner;
using RIMA.Editor.RoomDesigner.Brushes;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RIMA.Tests.Editor
{
    internal class FakeContext : IRoomDesignerContext
    {
        public Tilemap FloorTilemap { get; set; }
        public Tilemap WallsTilemap { get; set; }
        public Tilemap DecalsTilemap { get; set; }
        public RoomLayer ActiveLayer { get; set; } = RoomLayer.Floor;
        public TileBase ActiveTile { get; set; }
        public BrushMode ActiveBrush { get; set; }
        public Vector3Int HoveredCell { get; set; }
        public bool IsCanvasHovered => true;
        private VisualElement _left = new VisualElement();
        private VisualElement _right = new VisualElement();
        public VisualElement LeftPanel => _left;
        public VisualElement RightPanel => _right;
        public Tilemap GetActiveTilemap()
        {
            switch (ActiveLayer)
            {
                case RoomLayer.Walls:
                    return WallsTilemap;
                case RoomLayer.Decals:
                    return DecalsTilemap;
                case RoomLayer.Floor:
                default:
                    return FloorTilemap;
            }
        }

        public void InvokeBrush(int mouseButton, Vector3Int cell) { }
        public void MarkDirty() { }
    }

    public class BrushTests
    {
        private static Tilemap MakeTilemap(out GameObject go)
        {
            go = new GameObject("TilemapGO");
            go.AddComponent<Grid>();
            var child = new GameObject("TilemapChild");
            child.transform.SetParent(go.transform);
            return child.AddComponent<Tilemap>();
        }

        [Test]
        public void StampBrush_HappyPath()
        {
            var tilemap = MakeTilemap(out var go);
            var tile = ScriptableObject.CreateInstance<Tile>();
            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tile };
            _ = new BrushController();

            var brush = new StampBrush();
            brush.OnStrokeBegin(ctx, Vector3Int.zero, 0);
            brush.OnStrokeEnd(ctx);

            Assert.IsNotNull(tilemap.GetTile(Vector3Int.zero));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tile);
        }

        [Test]
        public void EraserBrush_ClearsCell()
        {
            var tilemap = MakeTilemap(out var go);
            var tile = ScriptableObject.CreateInstance<Tile>();
            tilemap.SetTile(Vector3Int.zero, tile);
            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = null };
            _ = new BrushController();

            var brush = new EraserBrush();
            brush.OnStrokeBegin(ctx, Vector3Int.zero, 0);
            brush.OnStrokeEnd(ctx);

            Assert.IsNull(tilemap.GetTile(Vector3Int.zero));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tile);
        }

        [Test]
        public void BucketFill_SmallArea()
        {
            var tilemap = MakeTilemap(out var go);
            var tileA = ScriptableObject.CreateInstance<Tile>();
            var tileB = ScriptableObject.CreateInstance<Tile>();
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileA);

            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tileB };
            _ = new BrushController();

            new BucketFillBrush().OnStrokeBegin(ctx, Vector3Int.zero, 0);

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    Assert.AreEqual(tileB, tilemap.GetTile(new Vector3Int(x, y, 0)));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tileA);
            Object.DestroyImmediate(tileB);
        }

        [Test]
        public void BucketFill_LargeAreaAbortsGracefully()
        {
            var tilemap = MakeTilemap(out var go);
            var tileA = ScriptableObject.CreateInstance<Tile>();
            var tileB = ScriptableObject.CreateInstance<Tile>();
            for (int x = 0; x < 150; x++)
                for (int y = 0; y < 150; y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileA);

            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tileB };
            _ = new BrushController();

            Assert.DoesNotThrow(() =>
                new BucketFillBrush().OnStrokeBegin(ctx, Vector3Int.zero, 0));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tileA);
            Object.DestroyImmediate(tileB);
        }

        [Test]
        public void MultiTilemap_ApplyStrokeSetsGroupName()
        {
            var tm1 = MakeTilemap(out var go1);
            var tile = ScriptableObject.CreateInstance<Tile>();
            var ctx = new FakeContext { FloorTilemap = tm1, ActiveTile = tile };
            var ctrl = new BrushController();

            var edits = new List<CellEdit>
            {
                new CellEdit(tm1, Vector3Int.zero, tile)
            };

            Assert.DoesNotThrow(() => ctrl.ApplyStroke(ctx, edits, "TestStroke"));

            Object.DestroyImmediate(go1);
            Object.DestroyImmediate(tile);
        }

        [Test]
        public void PickerBrush_PicksTileAndSwitchesToStamp()
        {
            var tilemap = MakeTilemap(out var go);
            var tile = ScriptableObject.CreateInstance<Tile>();
            tilemap.SetTile(Vector3Int.zero, tile);
            var ctx = new FakeContext
            {
                FloorTilemap = tilemap,
                ActiveLayer = RoomLayer.Floor,
                ActiveBrush = BrushMode.Picker
            };
            _ = new BrushController();

            new PickerBrush().OnStrokeBegin(ctx, Vector3Int.zero, 0);

            Assert.AreEqual(tile, ctx.ActiveTile);
            Assert.AreEqual(BrushMode.Stamp, ctx.ActiveBrush);

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tile);
        }
    }
}
