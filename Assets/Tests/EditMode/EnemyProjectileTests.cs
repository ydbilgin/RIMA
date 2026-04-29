using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class EnemyProjectileTests
    {
        private GameObject projectileObject;

        [TearDown]
        public void TearDown()
        {
            if (projectileObject != null)
                Object.DestroyImmediate(projectileObject);
        }

        [Test]
        public void Projectile_Init_ConfiguresPhysicsForTriggerProjectile()
        {
            projectileObject = new GameObject("Projectile_Test");
            var rb = projectileObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 3f;
            var col = projectileObject.AddComponent<CircleCollider2D>();
            col.isTrigger = false;
            var projectile = projectileObject.AddComponent<Projectile>();

            projectile.Init(12, 2f);

            Assert.AreEqual(0f, rb.gravityScale);
            Assert.IsTrue(col.isTrigger);
        }

        [Test]
        public void EnemyProjectileDamage_Init_ToleratesMissingCollider()
        {
            projectileObject = new GameObject("ProjectileDamage_Test");
            var damage = projectileObject.AddComponent<EnemyProjectileDamage>();

            Assert.DoesNotThrow(() => damage.Init(10, 1f));
        }
    }
}
