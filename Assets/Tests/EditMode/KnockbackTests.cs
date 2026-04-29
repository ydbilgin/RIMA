using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// KnockbackReceiver/Component EditMode testleri.
    /// ApplyKnockback Coroutine kullanıyor → bileşen varlık testleri.
    /// </summary>
    public class KnockbackTests
    {
        private GameObject target;
        private Rigidbody2D rb;

        [SetUp]
        public void SetUp()
        {
            target = new GameObject("Knockback_Test");
            rb = target.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(target);
        }

        [Test]
        public void Receiver_CanBeAdded()
        {
            var receiver = target.AddComponent<KnockbackReceiver>();
            Assert.IsNotNull(receiver, "KnockbackReceiver eklenebilmeli.");
        }

        [Test]
        public void Receiver_HasRigidbody()
        {
            target.AddComponent<KnockbackReceiver>();
            Assert.IsNotNull(target.GetComponent<Rigidbody2D>(), "Rigidbody2D olmalı.");
        }

        [Test]
        public void KnockbackComponent_CanBeAdded()
        {
            var attacker = new GameObject("Attacker");
            var comp = attacker.AddComponent<KnockbackComponent>();
            Assert.IsNotNull(comp, "KnockbackComponent eklenebilmeli.");
            Object.DestroyImmediate(attacker);
        }
    }
}
