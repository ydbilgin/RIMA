using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using RIMA.Tests.Contracts;

namespace RIMA.Tests.PlayMode
{
    /// <summary>
    /// Verifies that the game scene boots to a valid state within the bootstrap deadline.
    /// These tests require the "_IsoGame" scene to be included in Build Settings.
    /// </summary>
    public class SceneBootstrapTests
    {
        [UnityTest]
        [Category("Bootstrap")]
        [Category("PlayMode")]
        public IEnumerator GameScene_TimeScale_IsOne_AfterBoot()
        {
            SceneManager.LoadScene(BootstrapContract.GameSceneName);
            yield return null; // one frame to start the load

            yield return new WaitForSecondsRealtime(BootstrapContract.MaxBootstrapTime);

            Assert.AreEqual(
                TimeScaleContract.GameRunning,
                Time.timeScale,
                $"Time.timeScale should be {TimeScaleContract.GameRunning} after " +
                $"{BootstrapContract.MaxBootstrapTime}s. " +
                "Check RuntimeInitializeOnLoadMethod handlers — something left timeScale=0.");
        }

        [UnityTest]
        [Category("Bootstrap")]
        [Category("PlayMode")]
        public IEnumerator GameScene_Player_HasRequiredComponents()
        {
            SceneManager.LoadScene(BootstrapContract.GameSceneName);
            yield return new WaitForSecondsRealtime(0.3f);

            var player = Object.FindFirstObjectByType<PlayerController>();
            Assert.IsNotNull(player,
                "PlayerController not found in scene. " +
                "The Player prefab must have a PlayerController component.");
            Assert.IsTrue(player.enabled,
                "PlayerController is disabled. This usually means a missing required component.");

            var pa = player.GetComponent<PlayerAttack>();
            Assert.IsNotNull(pa,
                "PlayerAttack missing on Player. Add PlayerAttack component or check RequireComponent chain.");
            Assert.IsTrue(pa.enabled,
                "PlayerAttack is disabled. Most likely cause: basicAttackProfile is null in Inspector. " +
                "Assign a BasicAttackProfile asset to the Player prefab.");
        }
    }
}
