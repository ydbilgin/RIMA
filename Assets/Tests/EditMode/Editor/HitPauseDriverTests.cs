using System.Collections;
using System.Reflection;
using NUnit.Framework;
using RIMA.Combat;
using RIMA.Combat.Juice;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests.Editor
{
    public sealed class HitPauseDriverTests
    {
        private GameObject host;
        private HitPauseDriver driver;
        private float originalTimeScale;

        [SetUp]
        public void SetUp()
        {
            originalTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            FeelToggleSettings.HitstopEnabled = true;

            host = new GameObject("HitPauseDriver_Test");
            driver = host.AddComponent<HitPauseDriver>();
            host.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (host != null)
            {
                Object.DestroyImmediate(host);
            }

            Time.timeScale = originalTimeScale;
        }

        [Test]
        public void TriggerPause_ZerosTimeScaleImmediately()
        {
            driver.TriggerPause(0.05f);
            Assert.AreEqual(0f, Time.timeScale);
        }

        [UnityTest]
        public IEnumerator PauseRoutine_RestoresTimeScaleAfterDurationEnumeratorCompletes()
        {
            MethodInfo routineMethod = typeof(HitPauseDriver).GetMethod("PauseRoutine", BindingFlags.Instance | BindingFlags.NonPublic);
            IEnumerator routine = (IEnumerator)routineMethod.Invoke(driver, new object[] { 0f });

            Assert.IsTrue(routine.MoveNext());
            Assert.AreEqual(0f, Time.timeScale);

            yield return null;

            Assert.IsFalse(routine.MoveNext());
            Assert.AreEqual(1f, Time.timeScale);
        }

        [Test]
        public void FeelToggleDisabled_OnHitEventLeavesTimeScaleUnchanged()
        {
            FeelToggleSettings.HitstopEnabled = false;

            CombatEventBus.PublishHit(new HitEvent
            {
                worldPos = Vector3.zero,
                isCrit = false
            });

            Assert.AreEqual(1f, Time.timeScale);
        }
    }
}
