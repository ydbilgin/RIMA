// T1 — Map Boundary Tests
// Verifies that the boundary physics blocks prevent the player from leaving walkable area.
// Strategy: teleport player to a known walkable position (0,0), apply an outward velocity
// impulse for 1.5s, then assert the final position is still walkable.
// Repeats for all 4 cardinal directions.

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RIMA.Environment;

namespace RIMA.Tests.Phase1Demo
{
    public class T1_MapBoundaryTest : Phase1TestHarness
    {
        // Direction name + velocity to apply (50 u/s outward impulse).
        private static readonly (string dir, Vector2 velocity)[] PushDirections = new[]
        {
            ("North", new Vector2( 0f,  50f)),
            ("South", new Vector2( 0f, -50f)),
            ("East",  new Vector2( 50f,  0f)),
            ("West",  new Vector2(-50f,  0f)),
        };

        [UnityTest]
        public IEnumerator AllFourDirections_BoundaryKeepsPlayerWalkable()
        {
            var wmap = WalkabilityMap.Instance;
            if (wmap == null)
            {
                RegisterResult("T1_MapBoundary_AllFourDirections", false,
                    "WalkabilityMap.Instance is null — scene may not contain one.");
                Assert.Inconclusive("WalkabilityMap.Instance is null in scene. Test skipped.");
                yield break;
            }

            var player = GetPlayer();
            if (player == null)
            {
                RegisterResult("T1_MapBoundary_AllFourDirections", false,
                    "Player GameObject not found in scene.");
                Assert.Fail("Player GameObject not found in scene.");
                yield break;
            }

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                RegisterResult("T1_MapBoundary_AllFourDirections", false,
                    "Player has no Rigidbody2D — cannot apply velocity impulse.");
                Assert.Fail("Player Rigidbody2D not found.");
                yield break;
            }

            foreach (var (dir, velocity) in PushDirections)
            {
                // 1. Place player at known walkable position and clear velocity.
                TeleportPlayer(Vector2.zero);
                yield return null; // one frame to let physics settle after teleport

                // 2. Apply outward velocity impulse.
                rb.linearVelocity = velocity;

                // 3. Simulate physics for 1.5s while boundary colliders push back.
                yield return new WaitForSeconds(1.5f);

                // 4. Stop player so next iteration starts clean.
                rb.linearVelocity = Vector2.zero;

                // 5. Assert final position is still walkable.
                Vector3 finalPos = player.transform.position;
                bool walkable    = wmap.IsWalkableWorld(finalPos);

                if (!walkable)
                {
                    RegisterResult($"T1_MapBoundary_{dir}", false,
                        $"Player ended at {finalPos} which is NOT walkable after 1.5s outward push. " +
                        "Boundary collider did not contain the player.");
                    Assert.Fail(
                        $"[T1] {dir}: player position {finalPos} is not walkable " +
                        "after 1.5s outward push — boundary not enforced.");
                }
                else
                {
                    RegisterResult($"T1_MapBoundary_{dir}", true,
                        $"Player at {finalPos} — walkable confirmed after {dir} push.");
                }

                // Brief pause between directions to let physics settle.
                yield return WaitFrames(2);
            }
        }
    }
}
