namespace RIMA.Tests.Editor
{
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner;
    using RIMA.RoomDesigner.Core;
    using RIMA.Runtime.Rooms;
    using RIMA.Systems.Map;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.Tilemaps;

    public sealed class PipelineSmokeTest
    {
        private GameObject gridRoot;
        private GameObject stageRoot;
        private Tilemap baseTilemap;
        private Tilemap decalsTilemap;
        private Tilemap wallsTilemap;
        private RoomBlueprint bp;

        [TearDown]
        public void TearDown()
        {
            if (bp != null)
            {
                Object.DestroyImmediate(bp);
            }

            if (gridRoot != null)
            {
                Object.DestroyImmediate(gridRoot);
            }

            if (stageRoot != null)
            {
                Object.DestroyImmediate(stageRoot);
            }
        }

        [Test]
        public void PipelineSmokeTest_F1Template_FullPipeline_NoExceptions()
        {
            RimaRoomBaselineTemplate template = Resources.Load<RimaRoomBaselineTemplate>("__missing__");
            template = UnityEditor.AssetDatabase.LoadAssetAtPath<RimaRoomBaselineTemplate>("Assets/Art/Templates/F1_ShatteredRuins.asset");
            Assert.NotNull(template, "F1 template asset must exist.");

            CreateTilemaps();
            bp = ScriptableObject.CreateInstance<RoomBlueprint>();
            bp.biomeType = template.biome;
            bp.noiseSeed = 42;
            bp.roomWidth = 15;
            bp.roomHeight = 12;
            bp.roomOrigin = Vector3Int.zero;

            var input = new GenerationInput(42, template.biome.ToString(), template.archetypeId, 15, 12, template.generatorVersion);

            Assert.DoesNotThrow(() =>
            {
                GridSnapshot snapshot = RunCoreRoomBaseline(new RimaRoomBaselineGenerator(), input, baseTilemap, wallsTilemap);
                bp.roomWidth = snapshot.width;
                bp.roomHeight = snapshot.height;
                bp.roomOrigin = snapshot.origin;
            });

            LogAssert.Expect(LogType.Warning, "DecalPainter: decalSprites is empty.");
            Assert.DoesNotThrow(() => DecalPainter.PaintDecals(decalsTilemap, bp, template.decalSprites, 42, template.decalDensity));

            LogAssert.Expect(LogType.Warning, "PropPlacer: propSpecs is empty.");
            Assert.DoesNotThrow(() =>
            {
                AnchorZone[] anchors = RimaArchetypeGenerators.GetDefaultAnchorZones(template.archetypeId, 42, 15, 12);
                PropPlacer.PlaceProps(stageRoot, anchors, template.propSpecs, bp, 42);
            });
        }

        private void CreateTilemaps()
        {
            gridRoot = new GameObject("PipelineSmokeGrid");
            gridRoot.AddComponent<Grid>();
            baseTilemap = CreateTilemap("BaseTilemap");
            decalsTilemap = CreateTilemap("DecalsTilemap");
            wallsTilemap = CreateTilemap("WallsTilemap");
            wallsTilemap.gameObject.AddComponent<TilemapCollider2D>();

            stageRoot = new GameObject("StageRoot");
            stageRoot.AddComponent<Grid>();
        }

        private Tilemap CreateTilemap(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(gridRoot.transform, false);
            var tilemap = go.AddComponent<Tilemap>();
            go.AddComponent<TilemapRenderer>();
            return tilemap;
        }

        private static GridSnapshot RunCoreRoomBaseline(RoomBaselineGeneratorBase generator, GenerationInput input, Tilemap floor, Tilemap wall)
        {
            System.Type runnerType = System.Type.GetType("RIMA.RoomDesigner.Core.Editor.CoreRoomBaselineRunner, RIMA.RoomDesigner.Core.Editor");
            Assert.NotNull(runnerType, "CoreRoomBaselineRunner type must be loaded.");
            System.Reflection.MethodInfo run = runnerType.GetMethod("Run", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Assert.NotNull(run, "CoreRoomBaselineRunner.Run must exist.");
            return (GridSnapshot)run.Invoke(null, new object[] { generator, input, floor, wall });
        }
    }
}
