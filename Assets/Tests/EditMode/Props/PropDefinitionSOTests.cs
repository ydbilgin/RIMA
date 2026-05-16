#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropDefinitionSOTests
    {
        private PropDefinitionSO prop;

        [SetUp]
        public void SetUp()
        {
            prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
        }

        [TearDown]
        public void TearDown()
        {
            if (prop != null) Object.DestroyImmediate(prop);
        }

        [Test]
        public void Defaults_FootprintIsOneByOne()
        {
            Assert.AreEqual(Vector2Int.one, prop.footprintSize);
        }

        [Test]
        public void Defaults_RespectsWalkableMaskTrue()
        {
            Assert.IsTrue(prop.respectsWalkableMask);
        }

        [Test]
        public void Defaults_ForbiddenRolesContainsDoorSafetyAndWallBand()
        {
            CollectionAssert.Contains(prop.forbiddenRoles, CompositionRole.DoorSafety);
            CollectionAssert.Contains(prop.forbiddenRoles, CompositionRole.WallBand);
        }

        [Test]
        public void Defaults_PreferredRolesEmpty()
        {
            Assert.IsTrue(prop.preferredRoles == null || prop.preferredRoles.Length == 0);
        }
    }
}
#endif
