using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class BossAITests
    {
        private GameObject go;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("BossAI_Test");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void BossAI_CanBeInstantiated()
        {
            Assert.DoesNotThrow(() => go.AddComponent<BossAI_PenitentSovereign>());
        }

        [Test]
        public void RequiredComponent_Health_AutoAdded()
        {
            go.AddComponent<BossAI_PenitentSovereign>();
            Assert.IsNotNull(go.GetComponent<Health>());
        }

        [Test]
        public void RequiredComponent_Rigidbody2D_AutoAdded()
        {
            go.AddComponent<BossAI_PenitentSovereign>();
            Assert.IsNotNull(go.GetComponent<Rigidbody2D>());
        }

        [Test]
        public void RequiredComponent_KnockbackReceiver_AutoAdded()
        {
            go.AddComponent<BossAI_PenitentSovereign>();
            Assert.IsNotNull(go.GetComponent<KnockbackReceiver>());
        }

        [Test]
        public void AllFourRequiredComponents_PresentAfterAddComponent()
        {
            go.AddComponent<BossAI_PenitentSovereign>();
            Assert.IsNotNull(go.GetComponent<BossAI_PenitentSovereign>());
            Assert.IsNotNull(go.GetComponent<Health>());
            Assert.IsNotNull(go.GetComponent<Rigidbody2D>());
            Assert.IsNotNull(go.GetComponent<KnockbackReceiver>());
        }
    }
}
