using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Brush
{
    public static class BrushSampleAssetCreator
    {
        private const string OutputFolder = "Assets/Data/Brush";
        private const string PoolPath = OutputFolder + "/AssetPool_Floor_ShatteredKeep.asset";
        private const string CleanBrushPath = OutputFolder + "/Brush_CleanStoneFloor.asset";
        private const string CompositeBrushPath = OutputFolder + "/Brush_MossyCorner_Composite.asset";
        private const string Act1FloorTilePrefix = "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_tile_";

        public static void CreateSamples()
        {
            CreateSamples(OutputFolder);
        }

        public static void CreateSamples(string outputFolder)
        {
            if (string.IsNullOrEmpty(outputFolder) || !outputFolder.StartsWith("Assets/", StringComparison.Ordinal))
            {
                throw new ArgumentException("Output folder must be under Assets/.", nameof(outputFolder));
            }

            List<Sprite> floorSprites = LoadRequiredFloorSprites();
            Directory.CreateDirectory(outputFolder);

            AssetDatabase.StartAssetEditing();
            try
            {
                AssetPoolSO pool = LoadOrCreate<AssetPoolSO>(PoolPath);
                pool.poolName = "Floor_ShatteredKeep";
                pool.category = AssetCategory.Floor;
                pool.sprites = floorSprites;
                pool.spriteWeights = new List<float>();
                pool.tiles = new List<UnityEngine.Tilemaps.TileBase>();
                pool.prefabs = new List<GameObject>();
                pool.nativeSize = new Vector2Int(32, 32);
                pool.supportsRotation = false;
                pool.supportsFlip = false;
                pool.isSoftEdge = false;
                EditorUtility.SetDirty(pool);

                MapDesignerBrushPresetSO clean = LoadOrCreate<MapDesignerBrushPresetSO>(CleanBrushPath);
                clean.brushName = "Clean Stone Floor";
                clean.category = BrushCategory.Floor;
                clean.paintMode = PaintMode.GridTile;
                clean.operations = new List<BrushLayerOperation>
                {
                    new BrushLayerOperation
                    {
                        targetLayer = TargetLayer.L1,
                        assetPool = pool,
                        density = 1.0f,
                        probability = 1.0f,
                        respectsWalkableMask = true,
                        affectsCollision = true
                    }
                };
                clean.previewIcon = null;
                clean.showInPalette = true;
                clean.description = "Base floor tile, structural";
                clean.hotkeyIndex = 1;
                EditorUtility.SetDirty(clean);

                MapDesignerBrushPresetSO composite = LoadOrCreate<MapDesignerBrushPresetSO>(CompositeBrushPath);
                composite.brushName = "Mossy Broken Edge";
                composite.category = BrushCategory.Composite;
                composite.paintMode = PaintMode.CompositeStroke;
                composite.operations = new List<BrushLayerOperation>
                {
                    new BrushLayerOperation
                    {
                        targetLayer = TargetLayer.L2,
                        density = 0.35f,
                        probability = 1.0f,
                        respectsWalkableMask = true
                    },
                    new BrushLayerOperation
                    {
                        targetLayer = TargetLayer.L4,
                        density = 0.45f,
                        probability = 0.85f,
                        scaleRange = new Vector2(0.85f, 1.15f),
                        allowRotation = true,
                        respectsWalkableMask = true,
                        wallProximityCurve = new AnimationCurve(
                            new Keyframe(0f, 1.0f),
                            new Keyframe(1f, 0.6f),
                            new Keyframe(2f, 0.3f),
                            new Keyframe(3f, 0.1f))
                    },
                    new BrushLayerOperation
                    {
                        targetLayer = TargetLayer.L5,
                        density = 0.20f,
                        probability = 0.6f,
                        minDistance = 32f,
                        respectsWalkableMask = true
                    }
                };
                composite.previewIcon = null;
                composite.showInPalette = true;
                composite.description = "Natural moss erosion near walls - L2 dark + L4 patch + L5 cracks";
                composite.hotkeyIndex = 9;
                EditorUtility.SetDirty(composite);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
        }

        private static List<Sprite> LoadRequiredFloorSprites()
        {
            var sprites = new List<Sprite>();
            for (int i = 1; i <= 5; i++)
            {
                string path = Act1FloorTilePath(i);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite == null)
                {
                    throw new InvalidOperationException($"Missing required floor sprite: {path}");
                }

                sprites.Add(sprite);
            }

            return sprites;
        }

        private static string Act1FloorTilePath(int index)
        {
            return $"{Act1FloorTilePrefix}{index:00}.png";
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }

    public class BrushDataTests
    {
        private const string KnownSpritePath = "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_tile_01.png";
        private const string Act1FloorTilePrefix = "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_tile_";
        private const string CleanBrushPath = "Assets/Data/Brush/Brush_CleanStoneFloor.asset";
        private const string CompositeBrushPath = "Assets/Data/Brush/Brush_MossyCorner_Composite.asset";

        [Test]
        public void DefaultValues_BrushLayerOperation()
        {
            var operation = new BrushLayerOperation();

            Assert.IsTrue(operation.respectsWalkableMask);
            Assert.NotNull(operation.wallProximityCurve);
            Assert.GreaterOrEqual(operation.wallProximityCurve.keys.Length, 2);
        }

        [Test]
        public void AssetPoolSO_RoundTrip_Json()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.poolName = "JsonTestPool";
            pool.category = AssetCategory.Floor;
            pool.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(Act1FloorTilePath(1)));
            pool.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(Act1FloorTilePath(2)));
            pool.sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(Act1FloorTilePath(3)));

            string json = BrushJsonSerializer.ExportAssetPoolToJson(pool);
            AssetPoolDTO dto = BrushJsonSerializer.ImportAssetPoolFromJson(json);

            Assert.AreEqual(3, dto.spritePaths.Count);
            Assert.AreEqual(Act1FloorTilePath(1), dto.spritePaths[0]);
            Assert.AreEqual(Act1FloorTilePath(2), dto.spritePaths[1]);
            Assert.AreEqual(Act1FloorTilePath(3), dto.spritePaths[2]);
        }

        private static string Act1FloorTilePath(int index)
        {
            return $"{Act1FloorTilePrefix}{index:00}.png";
        }

        [Test]
        public void MapDesignerBrushPresetSO_Composite_HasThreeOperations()
        {
            MapDesignerBrushPresetSO brush = AssetDatabase.LoadAssetAtPath<MapDesignerBrushPresetSO>(CompositeBrushPath);

            Assert.NotNull(brush, "Run BrushSampleAssetCreator.CreateSamples before this test.");
            Assert.AreEqual(3, brush.operations.Count);
            Assert.AreEqual(TargetLayer.L2, brush.operations[0].targetLayer);
            Assert.AreEqual(TargetLayer.L4, brush.operations[1].targetLayer);
            Assert.AreEqual(TargetLayer.L5, brush.operations[2].targetLayer);
        }

        [Test]
        public void BrushJsonSerializer_AnimationCurve_RoundTrip()
        {
            var brush = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brush.brushName = "Curve Brush";
            brush.operations.Add(new BrushLayerOperation
            {
                targetLayer = TargetLayer.L4,
                wallProximityCurve = new AnimationCurve(
                    new Keyframe(0f, 1f),
                    new Keyframe(1f, 0.6f),
                    new Keyframe(2f, 0.3f),
                    new Keyframe(3f, 0.1f))
            });

            string json = BrushJsonSerializer.ExportBrushToJson(brush);
            BrushPresetDTO dto = BrushJsonSerializer.ImportBrushFromJson(json);

            Assert.AreEqual(4, dto.operations[0].wallProximityCurve.keys.Length);
            Assert.AreEqual(1f, dto.operations[0].wallProximityCurve.keys[0].value, 0.001f);
            Assert.AreEqual(0.6f, dto.operations[0].wallProximityCurve.keys[1].value, 0.001f);
            Assert.AreEqual(0.3f, dto.operations[0].wallProximityCurve.keys[2].value, 0.001f);
            Assert.AreEqual(0.1f, dto.operations[0].wallProximityCurve.keys[3].value, 0.001f);
        }

        [Test]
        public void SpritePathResolution_Existing_ReturnsSprite()
        {
            Sprite sprite = BrushJsonSerializer.ResolveSpritePath(KnownSpritePath);

            Assert.NotNull(sprite);
        }

        [Test]
        public void SpritePathResolution_Missing_ReturnsNull()
        {
            Assert.DoesNotThrow(() =>
            {
                Sprite sprite = BrushJsonSerializer.ResolveSpritePath("Assets/Art/Tiles/Keep/Floor/missing_sprite_for_test.png");
                Assert.IsNull(sprite);
            });
        }

        [Test]
        public void BrushPackSO_PreservesOrder_OnRoundTrip()
        {
            var brushA = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brushA.brushName = "A";
            var brushB = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brushB.brushName = "B";
            var pack = ScriptableObject.CreateInstance<BrushPackSO>();
            pack.brushes.Add(brushA);
            pack.brushes.Add(brushB);

            string json = BrushJsonSerializer.ExportBrushPackToJson(pack);
            BrushPackDTO dto = BrushJsonSerializer.ImportBrushPackFromJson(json);

            Assert.AreEqual("A", dto.brushes[0].brushName);
            Assert.AreEqual("B", dto.brushes[1].brushName);
        }

        [Test]
        public void CleanStoneFloor_RoundTrip_Json_FieldByField()
        {
            MapDesignerBrushPresetSO brush = AssetDatabase.LoadAssetAtPath<MapDesignerBrushPresetSO>(CleanBrushPath);

            Assert.NotNull(brush, "Run BrushSampleAssetCreator.CreateSamples before this test.");
            string json = BrushJsonSerializer.ExportBrushToJson(brush);
            BrushPresetDTO dto = BrushJsonSerializer.ImportBrushFromJson(json);

            Assert.AreEqual(1, dto.schemaVersion);
            Assert.AreEqual("Clean Stone Floor", dto.brushName);
            Assert.AreEqual(BrushCategory.Floor, dto.category);
            Assert.AreEqual(PaintMode.GridTile, dto.paintMode);
            Assert.AreEqual(1, dto.operations.Count);
            Assert.AreEqual(TargetLayer.L1, dto.operations[0].targetLayer);
            Assert.AreEqual(1.0f, dto.operations[0].density, 0.001f);
            Assert.AreEqual(1.0f, dto.operations[0].probability, 0.001f);
            Assert.IsTrue(dto.operations[0].respectsWalkableMask);
            Assert.IsTrue(dto.operations[0].affectsCollision);
            Assert.AreEqual("Base floor tile, structural", dto.description);
            Assert.AreEqual(1, dto.hotkeyIndex);
        }
    }
}
