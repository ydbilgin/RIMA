using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests
{
    /// <summary>
    /// Validates the 4-way diagonal facing snap used by PlayerAnimator.
    /// Uses reflection because SnapToFourDiagonal is private static.
    /// </summary>
    public class PlayerAnimatorDirectionTests
    {
        private static Vector2 Snap(Vector2 dir, Vector2 previousFacing)
        {
            var method = typeof(PlayerAnimator).GetMethod("SnapToFourDiagonal",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method, "SnapToFourDiagonal method not found in PlayerAnimator");
            return (Vector2)method.Invoke(null, new object[] { dir, previousFacing });
        }

        [Test] public void PureRight_KeepsPreviousVerticalAxis()
            => Assert.AreEqual(new Vector2(1, -1), Snap(new Vector2(1, 0), new Vector2(-1, -1)));

        [Test] public void PureLeft_KeepsPreviousVerticalAxis()
            => Assert.AreEqual(new Vector2(-1, 1), Snap(new Vector2(-1, 0), new Vector2(1, 1)));

        [Test] public void PureUp_KeepsPreviousHorizontalAxis()
            => Assert.AreEqual(new Vector2(-1, 1), Snap(new Vector2(0, 1), new Vector2(-1, -1)));

        [Test] public void PureDown_KeepsPreviousHorizontalAxis()
            => Assert.AreEqual(new Vector2(1, -1), Snap(new Vector2(0, -1), new Vector2(1, 1)));

        [Test] public void DiagonalSE_SnapsToSE()
            => Assert.AreEqual(new Vector2(1, -1), Snap(new Vector2(1, -1), new Vector2(-1, 1)));

        [Test] public void DiagonalNE_SnapsToNE()
            => Assert.AreEqual(new Vector2(1, 1), Snap(new Vector2(1, 1), new Vector2(-1, -1)));

        [Test] public void DiagonalSW_SnapsToSW()
            => Assert.AreEqual(new Vector2(-1, -1), Snap(new Vector2(-1, -1), new Vector2(1, 1)));

        [Test] public void DiagonalNW_SnapsToNW()
            => Assert.AreEqual(new Vector2(-1, 1), Snap(new Vector2(-1, 1), new Vector2(1, -1)));

        [Test] public void SlightlyRightOfNorthEast_SnapsToNE()
            => Assert.AreEqual(new Vector2(1, 1), Snap(new Vector2(0.7f, 0.9f), new Vector2(-1, -1)));

        [Test] public void ZeroInput_PreservesPreviousFacing()
            => Assert.AreEqual(new Vector2(-1, 1), Snap(Vector2.zero, new Vector2(-1, 1)));

        [Test]
        public void FourDiagonalTransitionMatrix_HasOnlyFourOppositeRiskPairs()
        {
            var method = typeof(PlayerAnimator).GetMethod("IsOppositeFacing",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.IsNotNull(method, "IsOppositeFacing method not found in PlayerAnimator");

            Vector2[] dirs =
            {
                new(1, -1),  // SE
                new(1, 1),   // NE
                new(-1, 1),  // NW
                new(-1, -1), // SW
            };

            int oppositeCount = 0;
            foreach (var from in dirs)
            foreach (var to in dirs)
            {
                bool opposite = (bool)method.Invoke(null, new object[] { from, to });
                if (opposite) oppositeCount++;
            }

            Assert.AreEqual(4, oppositeCount);
        }

        [Test]
        public void WarbladeController_CombatFacingNeedsIdleSpeedToChangeDirectionWithoutRunStates()
        {
            var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Animations/Characters/Warblade/Warblade.controller");
            Assert.IsNotNull(controller, "Warblade animator controller missing");

            var go = new GameObject("WarbladeControllerCombatFacingTest");
            try
            {
                var animator = go.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
                animator.Rebind();
                animator.Update(0f);

                animator.SetFloat("DirX", -1f);
                animator.SetFloat("DirY", -1f);
                animator.SetFloat("Speed", 1f);
                animator.Update(0.2f);
                string movingClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

                animator.SetFloat("Speed", 0f);
                animator.Update(0.2f);
                string combatStartupClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

                Assert.AreNotEqual("warblade_idle_SW", movingClip);
                Assert.AreEqual("warblade_idle_SW", combatStartupClip);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void CombatFacing_ReturnsToHeldMovementFacingWhenOverrideEnds()
        {
            var controllerAsset = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Animations/Characters/Warblade/Warblade.controller");
            Assert.IsNotNull(controllerAsset, "Warblade animator controller missing");

            var root = new GameObject("CombatFacingReturnTest");
            var sprite = new GameObject("Sprite");
            sprite.transform.SetParent(root.transform, false);
            sprite.AddComponent<SpriteRenderer>();
            var unityAnimator = sprite.AddComponent<Animator>();
            unityAnimator.runtimeAnimatorController = controllerAsset;

            try
            {
                root.AddComponent<Rigidbody2D>();
                root.AddComponent<BoxCollider2D>();
                var controller = root.AddComponent<PlayerController>();
                var playerAnimator = root.AddComponent<PlayerAnimator>();

                InvokePrivate(controller, "Awake");
                InvokePrivate(playerAnimator, "Awake");
                unityAnimator.Rebind();
                unityAnimator.Update(0f);

                SetPrivateField(controller, "moveInput", new Vector2(1f, 0f));
                SetPrivateField(controller, "lastMoveDir", new Vector2(1f, 0f));
                SetPrivateField(controller, "movementFacingDir", new Vector2(1f, 0f));
                InvokePrivate(playerAnimator, "Update");

                controller.FaceCombatDirection(new Vector2(-1f, 0f), 1f);
                InvokePrivate(playerAnimator, "Update");

                Assert.AreEqual(0f, unityAnimator.GetFloat("Speed"));
                Assert.AreEqual(-1f, unityAnimator.GetFloat("DirX"));

                SetPrivateField(controller, "combatFacingLockedUntil", Time.time - 0.1f);
                SetPrivateField(controller, "moveInput", new Vector2(1f, 0f));
                SetPrivateField(controller, "movementFacingDir", new Vector2(1f, 0f));
                InvokePrivate(playerAnimator, "Update");

                Assert.AreEqual(1f, unityAnimator.GetFloat("Speed"));
                Assert.AreEqual(1f, unityAnimator.GetFloat("DirX"));
                Assert.AreEqual(-1f, unityAnimator.GetFloat("DirY"));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field, $"Missing field {fieldName}");
            field.SetValue(target, value);
        }

        private static void InvokePrivate(object target, string methodName)
        {
            var method = target.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method, $"Missing method {methodName}");
            method.Invoke(target, null);
        }
    }
}
