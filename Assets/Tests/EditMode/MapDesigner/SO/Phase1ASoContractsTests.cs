using NUnit.Framework;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Tests.SO
{
    public class Phase1ASoContractsTests
    {
        [Test]
        public void TerrainDef_Defaults_Sensible()
        {
            var t = ScriptableObject.CreateInstance<TerrainDefinitionSO>();
            Assert.IsTrue(t.walkable);
            Assert.IsFalse(t.blocksMovement);
            Assert.That(t.defaultDecalDensity, Is.InRange(0f, 1f));
            Object.DestroyImmediate(t);
        }

        [Test]
        public void PatchAtlas_Roles_Cover_AllSlots()
        {
            Assert.AreEqual(5, System.Enum.GetValues(typeof(PatchRole)).Length);
        }

        [Test]
        public void ImportAssetRole_Matches_ChatGPTLock()
        {
            var required = new[]
            {
                "Terrain32",
                "MacroPatch64_128",
                "OrganicDecal",
                "DetailScatter",
                "Accent",
                "Prop",
                "Character",
                "TierBBackground",
                "LightSource"
            };

            foreach (var n in required)
            {
                Assert.IsTrue(
                    System.Enum.IsDefined(typeof(ImportAssetRole), n),
                    $"ImportAssetRole missing: {n}");
            }
        }

        [Test]
        public void RoomVisualProfile_Modes_Cover_AllChatGPTModes()
        {
            Assert.AreEqual(6, System.Enum.GetValues(typeof(RoomVisualMode)).Length);
        }

        [Test]
        public void PropDef_Shadow_Defaults()
        {
            var p = ScriptableObject.CreateInstance<PropDefinitionSO>();
            Assert.That(p.shadowAlpha, Is.InRange(0f, 1f));
            Assert.AreEqual(new Vector2(0f, -0.08f), p.shadowOffset);
            Object.DestroyImmediate(p);
        }
    }
}
