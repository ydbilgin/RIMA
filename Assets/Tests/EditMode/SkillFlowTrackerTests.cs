using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class SkillFlowTrackerTests
    {
        private class StubSkill : SkillBase { protected override void Execute() { } }

        private GameObject go;
        private SkillFlowTracker sut;
        private StubSkill skillA;
        private StubSkill skillB;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("SFT_Test");
            sut    = go.AddComponent<SkillFlowTracker>();
            skillA = go.AddComponent<StubSkill>();
            skillB = go.AddComponent<StubSkill>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void InitialState_NotChained()
        {
            Assert.IsFalse(sut.IsChainedToBasic);
            Assert.IsFalse(sut.IsChainedToSkill);
        }

        [Test]
        public void InitialState_LastSkillIsNull()
        {
            Assert.IsNull(sut.LastSkillUsed);
        }

        [Test]
        public void NotifySkillUsed_SetsBothChainFlags()
        {
            sut.NotifySkillUsed(skillA);
            Assert.IsTrue(sut.IsChainedToBasic);
            Assert.IsTrue(sut.IsChainedToSkill);
        }

        [Test]
        public void NotifySkillUsed_SetsLastSkill()
        {
            sut.NotifySkillUsed(skillA);
            Assert.AreEqual(skillA, sut.LastSkillUsed);
        }

        [Test]
        public void NotifySkillUsed_UpdatesLastSkillOnSecondCall()
        {
            sut.NotifySkillUsed(skillA);
            sut.NotifySkillUsed(skillB);
            Assert.AreEqual(skillB, sut.LastSkillUsed);
        }

        [Test]
        public void ConsumeBasicChain_WhenChained_ReturnsBonusMultiplier()
        {
            sut.NotifySkillUsed(skillA);
            float mult = sut.ConsumeBasicChain();
            Assert.AreEqual(1.2f, mult, 0.001f, "Chained basic attack should have 20% bonus.");
        }

        [Test]
        public void ConsumeBasicChain_WhenChained_ResetsFlag()
        {
            sut.NotifySkillUsed(skillA);
            sut.ConsumeBasicChain();
            Assert.IsFalse(sut.IsChainedToBasic, "Chain consumed — flag must reset.");
        }

        [Test]
        public void ConsumeBasicChain_WhenNotChained_Returns1()
        {
            float mult = sut.ConsumeBasicChain();
            Assert.AreEqual(1f, mult, 0.001f);
        }

        [Test]
        public void OnSkillUsed_FiresOnNotify()
        {
            int count = 0;
            sut.OnSkillUsed += _ => count++;
            sut.NotifySkillUsed(skillA);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void OnSkillUsed_FiresWithCorrectSkill()
        {
            SkillBase received = null;
            sut.OnSkillUsed += s => received = s;
            sut.NotifySkillUsed(skillA);
            Assert.AreEqual(skillA, received);
        }

        [Test]
        public void OnSkillChain_FiresWhenDifferentSkillUsedWhileChained()
        {
            SkillBase prev = null, curr = null;
            sut.OnSkillChain += (p, c) => { prev = p; curr = c; };

            sut.NotifySkillUsed(skillA);
            sut.NotifySkillUsed(skillB);

            Assert.AreEqual(skillA, prev);
            Assert.AreEqual(skillB, curr);
        }

        [Test]
        public void OnSkillChain_DoesNotFireForSameSkill()
        {
            bool fired = false;
            sut.OnSkillChain += (_, __) => fired = true;

            sut.NotifySkillUsed(skillA);
            sut.NotifySkillUsed(skillA);

            Assert.IsFalse(fired, "Same skill back-to-back should not trigger chain.");
        }

        [Test]
        public void OnSkillChain_DoesNotFireWhenNotYetChained()
        {
            bool fired = false;
            sut.OnSkillChain += (_, __) => fired = true;

            // Only one call — IsChainedToSkill was false before this
            sut.NotifySkillUsed(skillA);

            Assert.IsFalse(fired, "First skill use cannot chain.");
        }
    }
}
