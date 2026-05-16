#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Auto;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class BridsonPoissonAutoPlacerTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (Object obj in created)
            {
                if (obj != null) Object.DestroyImmediate(obj);
            }
            created.Clear();
        }

        [Test]
        public void Generate_NullTemplate_ReturnsEmpty()
        {
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            List<BridsonPoissonAutoPlacer.PlacementCandidate> result = placer.Generate(null, null, new List<PropDefinitionSO> { CreateProp("a") }, 0, 0.5f);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Generate_ZeroDensity_ReturnsEmpty()
        {
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            RoomTemplateSO template = CreateTemplate(10, 10);
            List<PropDefinitionSO> pool = new List<PropDefinitionSO> { CreateProp("z") };
            List<BridsonPoissonAutoPlacer.PlacementCandidate> result = placer.Generate(template, null, pool, 42, 0f);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Generate_DeterministicSeed_SameOutput()
        {
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            RoomTemplateSO template = CreateTemplate(12, 10);
            List<PropDefinitionSO> pool = new List<PropDefinitionSO> { CreateProp("det") };
            List<BridsonPoissonAutoPlacer.PlacementCandidate> run1 = placer.Generate(template, null, pool, 99, 1f);
            List<BridsonPoissonAutoPlacer.PlacementCandidate> run2 = placer.Generate(template, null, pool, 99, 1f);
            Assert.AreEqual(run1.Count, run2.Count);
            for (int i = 0; i < run1.Count; i++)
            {
                Assert.AreEqual(run1[i].tilePos, run2[i].tilePos);
                Assert.AreEqual(run1[i].rotationSteps, run2[i].rotationSteps);
            }
        }

        [Test]
        public void Generate_RespectsMinDistance()
        {
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            RoomTemplateSO template = CreateTemplate(20, 15);
            PropDefinitionSO prop = CreateProp("distance");
            prop.distanceFromOtherProps = 3f;
            List<PropDefinitionSO> pool = new List<PropDefinitionSO> { prop };

            List<BridsonPoissonAutoPlacer.PlacementCandidate> result = placer.Generate(template, null, pool, 17, 1f);
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    float d = Vector2.Distance(result[i].tilePos, result[j].tilePos);
                    Assert.GreaterOrEqual(d, prop.distanceFromOtherProps - 0.01f, $"Pair {i}-{j} distance {d} less than {prop.distanceFromOtherProps}");
                }
            }
        }

        [Test]
        public void Generate_FocalClusterRole_HigherDensityThanCleanCenter()
        {
            BridsonPoissonAutoPlacer placer = new BridsonPoissonAutoPlacer();
            RoomTemplateSO templateCenter = CreateTemplate(15, 15);
            RoomTemplateSO templateFocal = CreateTemplate(15, 15);
            List<PropDefinitionSO> pool = new List<PropDefinitionSO> { CreateProp("cluster") };

            CompositionRoleMap clusterMap = new CompositionRoleMap(templateFocal.bounds);
            for (int y = templateFocal.bounds.yMin; y < templateFocal.bounds.yMax; y++)
            {
                for (int x = templateFocal.bounds.xMin; x < templateFocal.bounds.xMax; x++)
                {
                    clusterMap.SetRoleAt(new Vector2Int(x, y), CompositionRole.FocalCluster);
                }
            }

            CompositionRoleMap cleanMap = new CompositionRoleMap(templateCenter.bounds);
            for (int y = templateCenter.bounds.yMin; y < templateCenter.bounds.yMax; y++)
            {
                for (int x = templateCenter.bounds.xMin; x < templateCenter.bounds.xMax; x++)
                {
                    cleanMap.SetRoleAt(new Vector2Int(x, y), CompositionRole.CleanCenter);
                }
            }

            int focalTotal = 0;
            int cleanTotal = 0;
            for (int seed = 1; seed <= 5; seed++)
            {
                focalTotal += placer.Generate(templateFocal, clusterMap, pool, seed, 0.5f).Count;
                cleanTotal += placer.Generate(templateCenter, cleanMap, pool, seed, 0.5f).Count;
            }
            Assert.Greater(focalTotal, cleanTotal, $"FocalCluster ({focalTotal}) should out-density CleanCenter ({cleanTotal}).");
        }

        [Test]
        public void DensityForRole_MatchesDocumentedTable()
        {
            Assert.AreEqual(0.1f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.CleanCenter));
            Assert.AreEqual(1.0f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.DecoratedEdge));
            Assert.AreEqual(2.0f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.FocalCluster));
            Assert.AreEqual(0f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.WallBand));
            Assert.AreEqual(0f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.DoorSafety));
            Assert.AreEqual(0f, BridsonPoissonAutoPlacer.DensityForRole(CompositionRole.Empty));
        }

        private PropDefinitionSO CreateProp(string id)
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = id;
            prop.footprintSize = new Vector2Int(1, 1);
            prop.distanceFromOtherProps = 1f;
            prop.forbiddenRoles = new CompositionRole[]
            {
                CompositionRole.WallBand,
                CompositionRole.DoorSafety,
                CompositionRole.Empty
            };
            created.Add(prop);
            return prop;
        }

        private RoomTemplateSO CreateTemplate(int w, int h)
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, w, h);
            template.props = new List<PropPlacementData>();
            created.Add(template);
            return template;
        }
    }
}
#endif
