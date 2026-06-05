using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class EchoWalletTests
    {
        private GameObject runStatsObject;
        private RunStats runStats;

        [SetUp]
        public void SetUp()
        {
            runStatsObject = new GameObject("RunStats_Test");
            runStats = runStatsObject.AddComponent<RunStats>();
            runStats.StartNewRun();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(runStatsObject);
        }

        [Test]
        public void ComputeRunAward_ClampsToMinimum()
        {
            SetRunStats(0, 0);

            Assert.AreEqual(EchoWallet.MinRunAward, EchoWallet.ComputeRunAward(runStats));
        }

        [Test]
        public void ComputeRunAward_ClampsToMaximum()
        {
            SetRunStats(40, 300);

            Assert.AreEqual(EchoWallet.MaxRunAward, EchoWallet.ComputeRunAward(runStats));
        }

        [Test]
        public void ComputeRunAward_UsesRoomsAndKills()
        {
            SetRunStats(8, 24);

            Assert.AreEqual(28, EchoWallet.ComputeRunAward(runStats));
        }

        private void SetRunStats(int roomsCleared, int kills)
        {
            SetPrivateField("roomsCleared", roomsCleared);
            SetPrivateField("kills", kills);
        }

        private void SetPrivateField(string fieldName, int value)
        {
            FieldInfo field = typeof(RunStats).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"{fieldName} field must exist.");
            field.SetValue(runStats, value);
        }
    }
}
