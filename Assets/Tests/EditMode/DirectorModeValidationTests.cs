using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.Balance;
using UnityEngine;

namespace RIMA.Tests
{
    public sealed class DirectorModeValidationTests
    {
        private readonly List<GameObject> created = new List<GameObject>();
        private PlayerClassManager createdClassManager;

        [SetUp]
        public void SetUp()
        {
            Time.timeScale = 1f;
            CleanupDirectorResidue();
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                    Object.DestroyImmediate(created[i]);
            }

            created.Clear();
            if (createdClassManager != null || PlayerClassManager.Instance == null)
            {
                typeof(PlayerClassManager)
                    .GetProperty(nameof(PlayerClassManager.Instance), BindingFlags.Public | BindingFlags.Static)
                    ?.SetValue(null, null);
            }

            createdClassManager = null;
            CleanupDirectorResidue();
            PlayerClassManager.DirectorBypassClassUnlock = false;
            Time.timeScale = 1f;
        }

        [Test]
        public void PropValidationPlacesAndErasesSelectedProp()
        {
            DirectorMode director = CreateDirector();

            Assert.IsTrue(director.SelectFirstPropForValidation());
            Assert.IsTrue(director.HasPropGhostForValidation());

            Assert.IsTrue(director.PlaceSelectedPropAtForValidation(new Vector2(2.3f, 3.2f)));
            Assert.AreEqual(1, director.DirectorPlacedPropCountForValidation());

            Assert.IsTrue(director.ErasePlacedPropAtForValidation(new Vector2(2f, 3f)));
            Assert.AreEqual(0, director.DirectorPlacedPropCountForValidation());
        }

        [Test]
        public void SpawnValidationStopsAtDirectorCap()
        {
            DirectorMode director = CreateDirector();

            Assert.IsTrue(director.SelectFirstSpawnEnemyForValidation());

            for (int i = 0; i < 12; i++)
            {
                director.SpawnSelectedEnemyAtForValidation(new Vector2(i * 2f, 0f));
            }

            Assert.AreEqual(10, director.DirectorSpawnedEnemyCountForValidation());
        }

        [Test]
        public void SetStatForValidationUpdatesRuntimeStatsThroughClampedSliderPath()
        {
            PlayerClassManager manager = EnsureClassManager();
            DirectorMode director = CreateDirector();

            Assert.IsTrue(director.SetStatForValidation("physPower", 177f));
            Assert.AreEqual(177f, manager.EnsureCurrentPrimaryStats().physPower, 0.001f);

            Assert.IsTrue(director.SetStatForValidation("debugGlobalDamageMult", 50f));
            Assert.AreEqual(5f, manager.EnsureCurrentPrimaryStats().debugGlobalDamageMult, 0.001f);
        }

        private DirectorMode CreateDirector()
        {
            GameObject go = new GameObject("DirectorMode_Test");
            created.Add(go);
            return go.AddComponent<DirectorMode>();
        }

        private PlayerClassManager EnsureClassManager()
        {
            if (PlayerClassManager.Instance != null)
                return PlayerClassManager.Instance;

            GameObject go = new GameObject("PlayerClassManager_Test");
            created.Add(go);
            PlayerClassManager manager = go.AddComponent<PlayerClassManager>();
            typeof(PlayerClassManager)
                .GetProperty(nameof(PlayerClassManager.Instance), BindingFlags.Public | BindingFlags.Static)
                ?.SetValue(null, manager);
            createdClassManager = manager;
            return manager;
        }

        private static void CleanupDirectorResidue()
        {
            GameObject[] all = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = all.Length - 1; i >= 0; i--)
            {
                GameObject go = all[i];
                if (go == null)
                    continue;

                bool residue =
                    go.name == "DirectorSpawnGhost" ||
                    go.name == "DirectorPropGhost" ||
                    go.name == "DirectorMode_Test" ||
                    go.name == "PlayerClassManager_Test" ||
                    go.name.EndsWith("_Director") ||
                    go.name.EndsWith("_DirectorProp");

                if (residue)
                    Object.DestroyImmediate(go);
            }
        }
    }
}
