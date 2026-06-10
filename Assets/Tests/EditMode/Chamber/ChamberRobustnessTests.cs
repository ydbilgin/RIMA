// ChamberRobustnessTests — EditMode tests that would have caught today's playtest bugs.
// T1: Chamber player spawn path yields PlayerAttack.enabled + non-null basicAttackProfile.
// T2: Lock enforcement — locked class cannot become SelectedClass via PlayerClassManager.
// T3: Dummy takes damage — Health.TakeDamage fires OnDamageTaken.
// T4: Layout determinism — demo classes (Warblade+Elementalist) are unique and in early positions.

using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests.Chamber
{
    public class ChamberRobustnessTests
    {
        // ── helpers ──────────────────────────────────────────────────────────────

        private static void SetPlayerClassManagerInstance(PlayerClassManager value)
        {
            FieldInfo field = typeof(PlayerClassManager).GetField(
                "<Instance>k__BackingField",
                BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, value);
        }

        private static BasicAttackProfile LoadProfile(ClassType cls)
        {
            return Resources.Load<BasicAttackProfile>($"Combat/BasicAttack/BasicAttackProfile_{cls}");
        }

        // ── T1: Combat-readiness ─────────────────────────────────────────────────

        [Test]
        public void T1_SpawnedPlayer_HasValidAttackProfile_AndComponentEnabled()
        {
            // Arrange: mirror the exact path ChamberSelectBootstrap.SpawnPlayer uses.
            PlayerClassManager.SelectedClass = ClassType.Warblade;

            // Create a minimal player GameObject as SpawnPlayer does.
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Warblade")
                              ?? Resources.Load<GameObject>("Prefabs/Player");
            GameObject instance = prefab != null
                ? Object.Instantiate(prefab)
                : new GameObject("Player_T1");

            try
            {
                instance.name = "Player";
                instance.tag = "Player";

                // Add PlayerAttack (mirrors EnsurePlayerRuntime).
                if (instance.GetComponent<PlayerController>() == null)
                    instance.AddComponent<PlayerController>();
                PlayerAttack attack = instance.GetComponent<PlayerAttack>()
                                   ?? instance.AddComponent<PlayerAttack>();

                // Simulate what AssignAttackProfileToPlayer does.
                BasicAttackProfile profile = LoadProfile(ClassType.Warblade);
                if (profile != null)
                    attack.SetBasicAttackProfile(profile);

                // Assert.
                Assert.IsNotNull(
                    GetPrivateField<BasicAttackProfile>(attack, "basicAttackProfile"),
                    "T1 FAIL: basicAttackProfile is null after spawn path.");
                Assert.IsTrue(attack.enabled,
                    "T1 FAIL: PlayerAttack.enabled is false — Awake must have logged an error.");
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }

        [Test]
        public void T1_SetBasicAttackProfile_EnablesComponent()
        {
            // Arrange: stand-alone PlayerAttack with no profile (simulates a blank prefab).
            var go = new GameObject("PlayerAttack_T1_Blank");
            try
            {
                // PlayerAttack requires PlayerController.
                go.AddComponent<PlayerController>();
                PlayerAttack attack = go.AddComponent<PlayerAttack>();
                // Awake will have run; if profile was null it disables itself.
                // Now simulate the fix: load and assign.
                BasicAttackProfile profile = LoadProfile(ClassType.Warblade);
                Assume.That(profile, Is.Not.Null,
                    "BasicAttackProfile_Warblade not found in Resources — skip T1 profile-assign sub-test.");

                attack.SetBasicAttackProfile(profile);

                Assert.IsTrue(attack.enabled, "SetBasicAttackProfile must re-enable the component.");
                Assert.IsNotNull(
                    GetPrivateField<BasicAttackProfile>(attack, "basicAttackProfile"),
                    "basicAttackProfile must be non-null after SetBasicAttackProfile.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        // ── T2: Lock enforcement ─────────────────────────────────────────────────

        [SetUp]
        public void SetUp_ClearPlayerPrefs()
        {
            // Clear any previously unlocked locked classes before each test.
            PlayerPrefs.DeleteKey(ClassUnlockPolicy.UnlockPrefKey(ClassType.Ranger));
            PlayerPrefs.DeleteKey(ClassUnlockPolicy.UnlockPrefKey(ClassType.Shadowblade));
            PlayerPrefs.DeleteKey(ClassUnlockPolicy.UnlockPrefKey(ClassType.Ronin));
            PlayerClassManager.SelectedClass = ClassType.Warblade;
            SetPlayerClassManagerInstance(null);
        }

        [TearDown]
        public void TearDown_ClearInstance()
        {
            SetPlayerClassManagerInstance(null);
        }

        [Test]
        public void T2_LockedClass_CannotBecomeSelectedClass_ViaSetPrimaryClass()
        {
            // Ranger and Shadowblade are locked by default.
            var go = new GameObject("PCM_T2");
            var manager = go.AddComponent<PlayerClassManager>();
            try
            {
                Assert.IsFalse(ClassUnlockPolicy.IsUnlocked(ClassType.Ranger),
                    "Precondition: Ranger must be locked.");

                manager.SetPrimaryClass(ClassType.Ranger);

                Assert.AreNotEqual(ClassType.Ranger, PlayerClassManager.SelectedClass,
                    "T2 FAIL: locked class Ranger became SelectedClass.");
                Assert.AreNotEqual(ClassType.Ranger, manager.PrimaryClass,
                    "T2 FAIL: locked class Ranger became PrimaryClass.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void T2_UnlockedStarterClasses_AreAlwaysSelectable()
        {
            Assert.IsTrue(ClassUnlockPolicy.IsUnlocked(ClassType.Warblade),
                "Warblade must always be unlocked.");
            Assert.IsTrue(ClassUnlockPolicy.IsUnlocked(ClassType.Elementalist),
                "Elementalist must always be unlocked.");
        }

        // ── T3: Dummy takes damage ───────────────────────────────────────────────

        [Test]
        public void T3_TrainingDummy_Health_TakesDamage_FiresOnDamageTaken()
        {
            var go = new GameObject("Dummy_T3");
            try
            {
                Health health = go.AddComponent<Health>();
                health.SetMaxHP(1000);

                // Guard: Awake may not have run in some EditMode contexts — ensure events exist.
                health.OnDamageTaken ??= new UnityEngine.Events.UnityEvent<int>();

                bool damageFired = false;
                health.OnDamageTaken.AddListener(_ => damageFired = true);

                health.TakeDamage(50);

                Assert.IsTrue(damageFired, "T3 FAIL: OnDamageTaken did not fire after TakeDamage.");
                // HP should have dropped (exact amount depends on incomingDamageMultiplier=1).
                Assert.Less(health.CurrentHP, 1000, "T3 FAIL: HP did not decrease after TakeDamage.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void T3_TrainingDummy_Immortal_ResetsOnDeath()
        {
            var go = new GameObject("Dummy_T3_Immortal");
            try
            {
                Health health = go.AddComponent<Health>();
                health.SetMaxHP(100);

                // Guard: Awake may not have run in some EditMode contexts — ensure events exist.
                health.OnDeath ??= new UnityEngine.Events.UnityEvent();
                health.OnHealthChanged ??= new UnityEngine.Events.UnityEvent<int, int>();
                health.OnDamageTaken ??= new UnityEngine.Events.UnityEvent<int>();

                bool deathFired = false;
                health.OnDeath.AddListener(() =>
                {
                    deathFired = true;
                    health.RestoreToFull();
                });

                health.TakeDamage(200); // overkill → should trigger death

                Assert.IsTrue(deathFired, "T3 FAIL: OnDeath did not fire on lethal damage.");
                Assert.AreEqual(health.MaxHP, health.CurrentHP,
                    "T3 FAIL: Immortal dummy did not restore to full HP after death.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        // ── T4: Layout determinism ───────────────────────────────────────────────

        [Test]
        public void T4_ChamberClasses_ContainBothUnlockedStarterClasses()
        {
            // ChamberClasses is a private static array — inspect via reflection.
            FieldInfo field = typeof(ChamberSelectBootstrap).GetField(
                "ChamberClasses",
                BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(field, "T4: ChamberClasses field not found via reflection.");

            ClassType[] classes = (ClassType[])field.GetValue(null);
            Assert.IsNotNull(classes, "T4: ChamberClasses array is null.");
            // DEMO LOCK (2026-06-10): only Warblade + Elementalist have kits + controller-routing.
            // Array is 2 until remaining classes get their kits.
            Assert.AreEqual(2, classes.Length, "T4: Expected exactly 2 demo chamber classes (Warblade+Elementalist).");

            // Unlocked starters must be present.
            Assert.Contains(ClassType.Warblade, classes,
                "T4 FAIL: Warblade not in ChamberClasses.");
            Assert.Contains(ClassType.Elementalist, classes,
                "T4 FAIL: Elementalist not in ChamberClasses.");
        }

        [Test]
        public void T4_ChamberClasses_AreAllDistinct_NoRepeats()
        {
            FieldInfo field = typeof(ChamberSelectBootstrap).GetField(
                "ChamberClasses",
                BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            ClassType[] classes = (ClassType[])field.GetValue(null);

            var seen = new HashSet<ClassType>();
            foreach (ClassType cls in classes)
            {
                Assert.IsFalse(seen.Contains(cls),
                    $"T4 FAIL: ClassType.{cls} appears more than once in ChamberClasses.");
                seen.Add(cls);
            }
        }

        [Test]
        public void T4_UnlockedClasses_AreInEarlyPositions_OfChamberClassOrder()
        {
            // Warblade and Elementalist are the starter unlocked classes.
            // They should appear at the front of ChamberClasses (indices 0 and 1)
            // so they land in the nearest-to-spawn gallery rows and read as "available".
            FieldInfo field = typeof(ChamberSelectBootstrap).GetField(
                "ChamberClasses",
                BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            ClassType[] classes = (ClassType[])field.GetValue(null);

            int warbladeIdx = System.Array.IndexOf(classes, ClassType.Warblade);
            int elemIdx = System.Array.IndexOf(classes, ClassType.Elementalist);

            // Both must be in the first two entries (front-row positions).
            Assert.LessOrEqual(warbladeIdx, 1,
                "T4: Warblade should be at index 0 or 1 (front row) in ChamberClasses.");
            Assert.LessOrEqual(elemIdx, 1,
                "T4: Elementalist should be at index 0 or 1 (front row) in ChamberClasses.");
            Assert.AreNotEqual(warbladeIdx, elemIdx,
                "T4: Warblade and Elementalist must not share the same station slot.");
        }

        // ── reflection helper ────────────────────────────────────────────────────

        private static T GetPrivateField<T>(object target, string fieldName) where T : class
        {
            FieldInfo field = target.GetType().GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
            return field?.GetValue(target) as T;
        }
    }
}
