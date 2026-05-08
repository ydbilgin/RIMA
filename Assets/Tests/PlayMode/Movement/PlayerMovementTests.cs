using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using RIMA.Tests.Contracts;

namespace RIMA.Tests.PlayMode
{
    /// <summary>
    /// Verifies player movement component state after scene bootstrap.
    /// Does NOT simulate actual input (Unity Test Framework restriction).
    /// Tests component state rather than input response.
    /// </summary>
    public class PlayerMovementTests
    {
        private PlayerController _player;

        [UnitySetUp]
        public IEnumerator LoadScene()
        {
            SceneManager.LoadScene(BootstrapContract.GameSceneName);
            yield return new WaitForSecondsRealtime(0.4f);

            _player = Object.FindFirstObjectByType<PlayerController>();
            Assume.That(_player, Is.Not.Null,
                "PlayerController not found — SceneBootstrapTests must pass first.");
        }

        [UnityTest]
        [Category("Movement")]
        [Category("PlayMode")]
        public IEnumerator PlayerController_MoveAction_IsEnabled()
        {
            // PlayerController stores moveAction as a private InputAction field.
            // We verify it is enabled, which means the InputSystem integration is wired correctly.
            var field = typeof(PlayerController).GetField("moveAction",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(field,
                "PlayerController.moveAction private field not found. " +
                "If the field was renamed, update this test.");

            var action = field.GetValue(_player) as UnityEngine.InputSystem.InputAction;
            Assert.IsNotNull(action, "moveAction is null on PlayerController at runtime.");
            Assert.IsTrue(action.enabled,
                "PlayerController.moveAction is not enabled. " +
                "Ensure PlayerController.OnEnable() calls moveAction.Enable().");

            yield return null;
        }

        [UnityTest]
        [Category("Movement")]
        [Category("PlayMode")]
        public IEnumerator PlayerRigidbody_IsSimulated()
        {
            var rb = _player.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rb, "Rigidbody2D missing on Player GameObject.");

            Assert.IsTrue(rb.simulated,
                "Rigidbody2D.simulated is false. Physics will not run for this object.");

            Assert.AreEqual(RigidbodyType2D.Dynamic, rb.bodyType,
                "Rigidbody2D.bodyType must be Dynamic for player physics.");

            Assert.AreEqual(0f, rb.gravityScale,
                "Rigidbody2D.gravityScale must be 0 for a top-down game (no gravity).");

            yield return null;
        }

        [UnityTest]
        [Category("Movement")]
        [Category("PlayMode")]
        public IEnumerator TimeScale_IsNotZero_WhenNoUIOpen()
        {
            // Verify no pausing UI is active at start
            var skillOffer = Object.FindFirstObjectByType<SkillOfferUI>();
            var settings   = Object.FindFirstObjectByType<SettingsMenuUI>();

            bool skillOfferActive  = skillOffer  != null && skillOffer.gameObject.activeSelf;
            bool settingsActive    = settings    != null && settings.gameObject.activeSelf;

            Assume.That(!skillOfferActive && !settingsActive,
                "A pausing UI panel is active at scene start — cannot test baseline timeScale. " +
                "Ensure SkillOfferUI and SettingsMenuUI start hidden.");

            Assert.AreNotEqual(0f, Time.timeScale,
                "Time.timeScale is 0 at scene start with no UI open. " +
                "Check Awake()/Start() methods for accidental Time.timeScale = 0 calls.");

            yield return null;
        }
    }
}
