// T3 — Combat Readiness Test
// Verifies: (1) PlayerAttack has a basicAttackProfile assigned,
//            (2) a basic attack triggered via InputTestFixture deals damage to a
//                Health-bearing target placed in front of the player.

using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace RIMA.Tests.Phase1Demo
{
    public class T3_CombatReadinessTest : Phase1TestHarness
    {
        // Test mob root GO reference — destroyed in teardown.
        private GameObject _testMob;

        /// <summary>
        /// Guard: restore Time.timeScale=1 in case a previous test (e.g. T2 DraftUI)
        /// left timeScale=0, which would freeze all WaitForSeconds in T3.
        /// </summary>
        [SetUp]
        public void T3RestoreTimeScale()
        {
            Time.timeScale = 1f;
        }

        [TearDown]
        public void T3CleanupMob()
        {
            if (_testMob != null)
                UnityEngine.Object.Destroy(_testMob);
            _testMob = null;
            Time.timeScale = 1f; // restore in case test left it modified
        }

        [UnityTest]
        public IEnumerator Combat_BasicAttack_Hits_Target()
        {
            const string TestName = "T3_Combat_BasicAttack_Hits_Target";

            // ── Step 1: find PlayerAttack ─────────────────────────────────────────
#if UNITY_2023_1_OR_NEWER
            var playerAttack = UnityEngine.Object.FindFirstObjectByType<PlayerAttack>();
#else
            var playerAttack = UnityEngine.Object.FindObjectOfType<PlayerAttack>();
#endif
            if (playerAttack == null)
            {
                RegisterResult(TestName, false,
                    "PlayerAttack component not found in scene.");
                Assert.Fail("[T3] PlayerAttack not found in scene.");
                yield break;
            }

            // ── Step 2: assert basicAttackProfile is assigned ─────────────────────
            // Field is SerializeField private — access via reflection.
            var profileField = typeof(PlayerAttack).GetField(
                "basicAttackProfile",
                BindingFlags.Instance | BindingFlags.NonPublic);

            object profileValue   = profileField?.GetValue(playerAttack);
            bool   profileAssigned = profileValue != null;

            if (!profileAssigned)
            {
                RegisterResult(TestName, false,
                    "PlayerAttack.basicAttackProfile is null. " +
                    "Assign a BasicAttackProfile in the Inspector. " +
                    "BLOCKED: runtime hook needed — PlayerAttack disables itself when profile is null.");
                Assert.Fail("[T3] PlayerAttack.basicAttackProfile is null (BLOCKED — needs Inspector assignment).");
                yield break;
            }

            Debug.Log($"[T3] basicAttackProfile assigned: {profileValue}");

            // ── Step 3: ensure player is at known position ────────────────────────
            var playerGO = GetPlayer();
            if (playerGO == null)
            {
                RegisterResult(TestName, false, "Player GameObject not found.");
                Assert.Fail("[T3] Player not found.");
                yield break;
            }

            TeleportPlayer(Vector2.zero);

            // Force CharacterFacing mode so attack direction = movementFacingDir (deterministic).
            // TowardsMouse reads Mouse.current.position which is unpredictable in PlayMode tests.
            var ctrl2 = playerGO.GetComponent<PlayerController>();
            if (ctrl2 != null) ctrl2.AttackAimMode = CombatAimMode.CharacterFacing;

            yield return WaitFrames(2);

            // ── Step 4: find or create a Health-bearing target ────────────────────
            Health targetHealth = null;
            var allHealth = UnityEngine.Object.FindObjectsByType<Health>(FindObjectsSortMode.None);
            foreach (var h in allHealth)
            {
                if (h.gameObject == playerGO) continue;
                if (h.IsDead) continue;
                targetHealth = h;
                break;
            }

            if (targetHealth == null)
            {
                _testMob             = new GameObject("T3_TestMob");
                _testMob.tag         = "Untagged";
                targetHealth         = _testMob.AddComponent<Health>();
                yield return WaitFrames(1); // ensure Awake runs
                Debug.Log("[T3] No live enemy found — created runtime TestMob.");
            }

            int initialHP = targetHealth.CurrentHP;
            if (initialHP <= 0)
            {
                if (_testMob != null)
                {
                    targetHealth.RestoreToFull();
                    initialHP = targetHealth.CurrentHP;
                }
                else
                {
                    RegisterResult(TestName, false,
                        "Target Health.CurrentHP == 0 before attack — already dead.");
                    Assert.Fail("[T3] Target already dead before attack.");
                    yield break;
                }
            }

            // ── Step 5: place target at hit-center along CharacterFacing direction ─
            // CharacterFacing mode was set above, so FaceCombatTarget() will use
            // movementFacingDir (default (1,-1).normalized = (0.707,-0.707)).
            // Place mob at that exact hit-center so it is guaranteed inside hitRadius.
            Vector3 playerPos = playerGO.transform.position;

            var profile = profileValue as BasicAttackProfile;
            Vector2 facing   = new Vector2(1f, -1f).normalized;  // default movementFacingDir
            float   hitRange = profile != null && profile.hitRange != null && profile.hitRange.Length > 0
                                   ? profile.hitRange[0] : 1.2f;
            float   hitRadiusVal = profile != null && profile.hitRadius != null && profile.hitRadius.Length > 0
                                   ? profile.hitRadius[0] : 0.75f;

            // Position target at hitCenter (playerPos + facing * hitRange).
            // Give it a CircleCollider2D with radius slightly smaller than hitRadiusVal
            // so it is always fully inside the overlap sphere.
            targetHealth.transform.position = (Vector2)playerPos + facing * hitRange;

            var existingCol = targetHealth.GetComponent<Collider2D>();
            if (existingCol == null)
            {
                var circle    = targetHealth.gameObject.AddComponent<CircleCollider2D>();
                circle.radius = Mathf.Min(0.4f, hitRadiusVal * 0.5f);
            }

            yield return WaitFrames(3); // 3 frames: physics registers new collider
            Physics2D.SyncTransforms(); // ensure new collider is in the physics world

            // ── Step 6: invoke basic attack via test hook (bypasses InputAction binding) ──
            // PlayerAttack.InvokeBasicAttackForTest() calls behavior.OnLMBInput directly,
            // so virtual input device mismatch is irrelevant.
            playerAttack.InvokeBasicAttackForTest();
            yield return new WaitForSeconds(0.5f);

            // ── Step 7: assert health decreased ───────────────────────────────────
            int  finalHP = targetHealth.CurrentHP;
            bool damaged = finalHP < initialHP;

            if (damaged)
            {
                RegisterResult(TestName, true,
                    $"Target HP {initialHP} → {finalHP} (delta {initialHP - finalHP}).");
            }
            else
            {
                RegisterResult(TestName, false,
                    $"Target HP unchanged: {initialHP} → {finalHP}. " +
                    "InvokeBasicAttackForTest() was called but no damage dealt. " +
                    "Possible causes: mob outside hitRadius, behavior returned early " +
                    "(CommitTimer > 0), or Health.TakeDamage has a guard condition.");
                Assert.Fail(
                    $"[T3] Target HP unchanged at {finalHP} after InvokeBasicAttackForTest().");
            }
        }
    }
}
