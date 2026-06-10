using System;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace RIMA.Tests.Room
{
    public class RoomRunDirectorTests
    {
        // Logged by RoomRunDirector.BuildCurrentRoom when no IsoRoomBuilder is wired.
        // Scene-less navigation tests expect this exact error each build call.
        private const string MissingBuilderError = "[RoomRunDirector] Missing IsoRoomBuilder reference.";

        private GameObject directorObject;
        private RoomRunDirector director;

        [SetUp]
        public void SetUp()
        {
            directorObject = new GameObject("RoomRunDirectorTest");
            director = directorObject.AddComponent<RoomRunDirector>();
        }

        [TearDown]
        public void TearDown()
        {
            if (directorObject != null)
            {
                Object.DestroyImmediate(directorObject);
            }
        }

        [Test]
        public void BeginRun_WithMissingBuilder_GeneratesNavigableGraph()
        {
            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.BeginRun();

            Assert.IsNotNull(director.Graph);
            Assert.AreEqual(director.Graph.startId, director.CurrentNodeId);
            Assert.Greater(director.CurrentChoices.Count, 0);
            Assert.IsFalse(director.IsRunComplete);
        }

        [Test]
        public void AdvanceTo_ValidChoice_MovesToChild()
        {
            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.BeginRun();
            int childId = director.CurrentChoices[0].id;

            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.AdvanceTo(0);

            Assert.AreEqual(childId, director.CurrentNodeId);
        }

        [Test]
        public void AdvanceTo_InvalidIndex_IsNoOp()
        {
            // Only BeginRun's build logs an error; invalid AdvanceTo calls log warnings
            // (which do not fail the test) and never reach a build.
            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.BeginRun();
            int before = director.CurrentNodeId;

            director.AdvanceTo(-1);
            Assert.AreEqual(before, director.CurrentNodeId);

            director.AdvanceTo(999);
            Assert.AreEqual(before, director.CurrentNodeId);
        }

        [Test]
        public void IsRunComplete_FalseAtBoss()
        {
            // Boss is no longer terminal — it has a child (post-boss Combat).
            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.BeginRun();

            int guard = 0;
            while (director.CurrentRoomType != RIMA.RoomType.Boss && guard++ < 20)
            {
                LogAssert.Expect(LogType.Error, MissingBuilderError);
                director.AdvanceTo(0);
            }

            Assert.AreEqual(RIMA.RoomType.Boss, director.CurrentRoomType, "Should be on Boss node.");
            Assert.IsFalse(director.IsRunComplete, "Boss node must NOT be terminal (has post-boss child).");
            Assert.AreEqual(1, director.CurrentChoices.Count, "Boss node must have exactly 1 choice.");
        }

        [Test]
        public void IsRunComplete_TrueAtPostBoss()
        {
            // Post-boss Combat (node 5) is the real terminal.
            LogAssert.Expect(LogType.Error, MissingBuilderError);
            director.BeginRun();

            int guard = 0;
            while (!director.IsRunComplete && guard++ < 20)
            {
                LogAssert.Expect(LogType.Error, MissingBuilderError);
                director.AdvanceTo(0);
            }

            Assert.IsTrue(director.IsRunComplete, "Post-boss Combat must be terminal.");
            Assert.IsNotNull(director.CurrentNode);
            Assert.AreEqual(RIMA.RoomType.Combat, director.CurrentRoomType, "Terminal node must be post-boss Combat.");
            Assert.AreEqual(0, director.CurrentChoices.Count);
        }

        [Test]
        public void Lifecycle_CombatClearRewardDoorAdvance_FollowsExpectedOrder()
        {
            RoomRunLifecycle lifecycle = new RoomRunLifecycle();

            lifecycle.BeginCombat();
            Assert.AreEqual(RoomRunLifecycleState.Combat, lifecycle.State);

            Assert.IsTrue(lifecycle.MarkCleared());
            Assert.AreEqual(RoomRunLifecycleState.Cleared, lifecycle.State);

            Assert.IsTrue(lifecycle.MarkRewardTaken());
            Assert.AreEqual(RoomRunLifecycleState.RewardTaken, lifecycle.State);

            Assert.IsTrue(lifecycle.MarkDoorsOpened());
            Assert.AreEqual(RoomRunLifecycleState.DoorOpen, lifecycle.State);

            Assert.IsTrue(lifecycle.MarkAdvancing());
            Assert.AreEqual(RoomRunLifecycleState.Advancing, lifecycle.State);
        }

        [Test]
        public void Lifecycle_DoorCannotOpenBeforeRewardTaken()
        {
            RoomRunLifecycle lifecycle = new RoomRunLifecycle();

            lifecycle.BeginCombat();
            Assert.IsFalse(lifecycle.MarkDoorsOpened());
            Assert.AreEqual(RoomRunLifecycleState.Combat, lifecycle.State);

            Assert.IsTrue(lifecycle.MarkCleared());
            Assert.IsFalse(lifecycle.MarkDoorsOpened());
            Assert.AreEqual(RoomRunLifecycleState.Cleared, lifecycle.State);
        }

        [Test]
        public void Lifecycle_VictoryCanTerminateAfterClear()
        {
            RoomRunLifecycle lifecycle = new RoomRunLifecycle();

            lifecycle.BeginCombat();
            Assert.IsTrue(lifecycle.MarkCleared());
            lifecycle.MarkVictory();

            Assert.AreEqual(RoomRunLifecycleState.Victory, lifecycle.State);
            Assert.IsFalse(lifecycle.MarkRewardTaken());
        }

        [Test]
        public void SelectSecondaryClass_RejectsPrimaryClass()
        {
            // Primary-class reject guard in PlayerClassManager. A fresh manager defaults to
            // PrimaryClass = Warblade, so no SetPrimaryClass call (and no Player lookup) is needed.
            var go = new GameObject("PCM_Test");
            var manager = go.AddComponent<PlayerClassManager>();

            Assert.AreEqual(ClassType.Warblade, manager.PrimaryClass, "Fresh manager must default to Warblade primary.");

            manager.SelectSecondaryClass(ClassType.Warblade); // must be rejected (matches primary)

            Assert.AreEqual(ClassType.None, manager.SecondaryClass,
                "SelectSecondaryClass must reject ClassType equal to PrimaryClass.");

            Object.DestroyImmediate(go);
        }

        // ── Unlock-draft race condition contract tests ───────────────────────────

        [Test]
        [Category("Contract")]
        public void DraftManager_HasIsDraftPendingProperty_PublicReadable()
        {
            // Contract: IsDraftPending must exist as a public bool property so
            // RoomClearSequence can include the 2 s ShowDraftDelayed window in its wait.
            // Without this property the race condition (door opens before draft appears) recurs.
            Assembly rimaAssembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetType("RIMA.DraftManager") != null)
                {
                    rimaAssembly = asm;
                    break;
                }
            }
            Assert.IsNotNull(rimaAssembly, "Could not locate RIMA runtime assembly.");

            var type = rimaAssembly.GetType("RIMA.DraftManager");
            Assert.IsNotNull(type, "RIMA.DraftManager not found.");

            var prop = type.GetProperty("IsDraftPending", BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(prop,
                "DraftManager.IsDraftPending public property not found. " +
                "This property bridges the 2 s ShowDraftDelayed gap so RoomClearSequence " +
                "waits for the unlock draft before opening the exit door.");
            Assert.AreEqual(typeof(bool), prop.PropertyType,
                "DraftManager.IsDraftPending must be bool.");
        }

        [Test]
        [Category("Contract")]
        public void DraftManager_IsDraftPending_DefaultsFalse()
        {
            // A newly created DraftManager (no MonoBehaviour lifecycle) must start with
            // IsDraftPending = false so the WaitWhile loop exits immediately on a fresh run.
            Assembly rimaAssembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetType("RIMA.DraftManager") != null)
                {
                    rimaAssembly = asm;
                    break;
                }
            }
            Assert.IsNotNull(rimaAssembly, "Could not locate RIMA runtime assembly.");

            var type = rimaAssembly.GetType("RIMA.DraftManager");
            Assert.IsNotNull(type, "RIMA.DraftManager not found.");

            var go = new GameObject("DraftManager_ContractTest");
            var dm = go.AddComponent(type) as MonoBehaviour;
            Assert.IsNotNull(dm, "Could not add DraftManager as MonoBehaviour.");

            var prop = type.GetProperty("IsDraftPending", BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(prop, "IsDraftPending property missing — run preceding test first.");

            bool value = (bool)prop.GetValue(dm);
            Assert.IsFalse(value,
                "DraftManager.IsDraftPending must be false on a fresh instance. " +
                "A true default would cause RoomClearSequence to stall forever.");

            Object.DestroyImmediate(go);
        }
    }
}
