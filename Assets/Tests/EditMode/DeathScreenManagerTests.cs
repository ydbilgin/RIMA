using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class DeathScreenManagerTests
    {
        private GameObject go;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("DSM_Test");
        }

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f; // restore after any test that may have changed it
            Object.DestroyImmediate(go);
        }

        [Test]
        public void DeathScreenManager_CanBeInstantiated()
        {
            Assert.DoesNotThrow(() => go.AddComponent<DeathScreenManager>());
        }

        [Test]
        public void RegisterKill_DoesNotThrow()
        {
            var dsm = go.AddComponent<DeathScreenManager>();
            Assert.DoesNotThrow(() => dsm.RegisterKill());
        }

        [Test]
        public void RegisterKill_MultipleCallsDoNotThrow()
        {
            var dsm = go.AddComponent<DeathScreenManager>();
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 10; i++)
                    dsm.RegisterKill();
            });
        }

        [Test]
        public void RestartRun_ResetsTimeScale()
        {
            Time.timeScale = 0f;
            var dsm = go.AddComponent<DeathScreenManager>();
            // RestartRun resets timeScale to 1 before reloading scene.
            // Scene reload is tested manually; here we just verify timeScale is restored
            // by calling the relevant line directly via the public method.
            // NOTE: SceneManager.LoadScene will NOP in EditMode test context.
            dsm.RestartRun();
            Assert.AreEqual(1f, Time.timeScale, 0.001f, "Time.timeScale must be reset to 1 on restart.");
        }
    }
}
