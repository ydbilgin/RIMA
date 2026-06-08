using NUnit.Framework;
using System.Reflection;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public sealed class ClassUnlockPolicyTests
    {
        [SetUp]
        public void SetUp()
        {
            PlayerPrefs.DeleteKey(ClassUnlockPolicy.UnlockPrefKey(ClassType.Ranger));
            PlayerPrefs.DeleteKey(ClassUnlockPolicy.UnlockPrefKey(ClassType.Shadowblade));
            PlayerClassManager.SelectedClass = ClassType.Warblade;
            SetPlayerClassManagerInstance(null);
        }

        [TearDown]
        public void TearDown()
        {
            SetPlayerClassManagerInstance(null);
        }

        [Test]
        public void StarterClassesAreUnlockedAndLegacyStartersAreLockedByDefault()
        {
            Assert.IsTrue(ClassUnlockPolicy.IsUnlocked(ClassType.Warblade));
            Assert.IsTrue(ClassUnlockPolicy.IsUnlocked(ClassType.Elementalist));
            Assert.IsFalse(ClassUnlockPolicy.IsUnlocked(ClassType.Shadowblade));
            Assert.IsFalse(ClassUnlockPolicy.IsUnlocked(ClassType.Ranger));
        }

        [Test]
        public void PlayerClassManagerRejectsLockedPrimaryClass()
        {
            var go = new GameObject("PlayerClassManager_Test");
            var manager = go.AddComponent<PlayerClassManager>();

            manager.SetPrimaryClass(ClassType.Shadowblade);

            Assert.AreNotEqual(ClassType.Shadowblade, PlayerClassManager.SelectedClass);
            Assert.AreNotEqual(ClassType.Shadowblade, manager.PrimaryClass);

            Object.DestroyImmediate(go);
        }

        private static void SetPlayerClassManagerInstance(PlayerClassManager value)
        {
            FieldInfo field = typeof(PlayerClassManager).GetField("<Instance>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, value);
        }
    }
}
