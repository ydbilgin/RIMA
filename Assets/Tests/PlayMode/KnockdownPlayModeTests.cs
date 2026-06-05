using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests
{
    public class KnockdownPlayModeTests
    {
        private GameObject target;

        [TearDown]
        public void TearDown()
        {
            if (target != null)
                Object.Destroy(target);
        }

        [UnityTest]
        public IEnumerator BrokenHeavyImpulse_ArcsLocksDamageThenGetsUp()
        {
            Time.timeScale = 1f;
            target = new GameObject("Knockdown_PlayMode_Target");
            target.tag = "Enemy";
            target.transform.position = Vector3.zero;

            var sr = target.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f, 1f);

            var rb = target.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            target.AddComponent<BoxCollider2D>();

            var health = target.AddComponent<Health>();
            health.SetMaxHP(20);
            var enemyAI = target.AddComponent<EnemyAI>();
            var tracker = target.AddComponent<SkillStateTracker>();
            tracker.Apply(SkillStateTracker.Broken, 2f);

            var receiver = target.AddComponent<KnockbackReceiver>();
            var profile = ScriptableObject.CreateInstance<KnockdownProfile>();
            profile.launchDuration = 0.10f;
            profile.arcHeight = 0.40f;
            profile.downTime = 0.12f;
            profile.getUpDuration = 0.10f;
            profile.getUpIFrame = 0.05f;
            profile.bounceCount = 1;
            profile.bounceDuration = 0.05f;
            var impulse = new HitImpulse(Vector2.right, 2f, 0.10f, canKnockdown: true, profile);

            receiver.ApplyImpulse(impulse);
            yield return WaitRealtime(0.05f);

            var driver = target.GetComponent<KnockdownDriver>();
            Assert.IsNotNull(driver);
            Assert.IsTrue(driver.IsDownOrGettingUp);
            Assert.IsTrue(health.IsImmune);
            Assert.IsFalse(enemyAI.enabled);

            health.TakeDamage(5);
            Assert.AreEqual(20, health.CurrentHP, "Downed target must be immune to damage.");

            yield return WaitRealtime(0.65f);

            Assert.IsFalse(driver.IsDownOrGettingUp);
            Assert.IsFalse(health.IsImmune);
            Assert.IsTrue(enemyAI.enabled);

            health.TakeDamage(5);
            Assert.AreEqual(15, health.CurrentHP, "Target must take damage again after get-up i-frame.");

            Object.Destroy(profile);
        }

        private static IEnumerator WaitRealtime(float seconds)
        {
            float end = Time.realtimeSinceStartup + seconds;
            while (Time.realtimeSinceStartup < end)
                yield return null;
        }
    }
}
