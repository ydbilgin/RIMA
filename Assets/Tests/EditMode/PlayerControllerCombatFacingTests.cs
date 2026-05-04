using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests
{
    public class PlayerControllerCombatFacingTests
    {
        private GameObject player;
        private PlayerController controller;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject("Player");
            player.AddComponent<Rigidbody2D>();
            player.AddComponent<BoxCollider2D>();
            controller = player.AddComponent<PlayerController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(player);
        }

        [Test]
        public void CombatFacingOverrideWinsOverMovementFacingWhileLocked()
        {
            SetPrivateVector("movementFacingDir", Vector2.right);
            SetPrivateVector("lastMoveDir", Vector2.right);

            controller.FaceCombatDirection(Vector2.left, 5f);
            SetPrivateVector("movementFacingDir", Vector2.right);

            Assert.IsTrue(controller.HasCombatFacingOverride);
            Assert.Less(controller.FacingDirection.x, -0.99f);
        }

        [Test]
        public void MovementFacingReturnsAfterCombatFacingLockExpires()
        {
            SetPrivateVector("movementFacingDir", Vector2.right);
            controller.FaceCombatDirection(Vector2.left, 5f);
            SetPrivateFloat("combatFacingLockedUntil", Time.time - 0.1f);

            Assert.IsFalse(controller.HasCombatFacingOverride);
            Assert.Greater(controller.FacingDirection.x, 0.99f);
        }

        private void SetPrivateVector(string fieldName, Vector2 value)
        {
            var field = typeof(PlayerController).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Missing PlayerController field: {fieldName}");
            field.SetValue(controller, value);
        }

        private void SetPrivateFloat(string fieldName, float value)
        {
            var field = typeof(PlayerController).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Missing PlayerController field: {fieldName}");
            field.SetValue(controller, value);
        }
    }
}
