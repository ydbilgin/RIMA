using NUnit.Framework;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

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
    }
}
