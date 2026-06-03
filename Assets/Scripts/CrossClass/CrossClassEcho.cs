using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Feature B (B2) — the transient "Shadow / Sundered Echo" actor.
    ///
    /// A black silhouette of a guest class that puffs in, moves into position by archetype,
    /// performs the guest skill (via <see cref="SkillBase.ExecuteAt"/> on the player-parented guest
    /// component — the SAME damage/break logic, fired from the echo's position), then puffs out and
    /// despawns. It is purely cosmetic + a positional anchor: it never takes damage and does not persist.
    ///
    /// Visual: guest idle sprite (Resources/Characters/&lt;Class&gt;/&lt;class&gt;_idle_south) tinted
    /// BLACK with a CYAN rim (a slightly larger cyan duplicate behind), reconciling the user's "black
    /// silhouette" with NLM canon's "translucent cyan, ~0.3 alpha". Sorted on layer "Entities" with
    /// spriteSortPoint = Pivot so the project's Custom-Axis Y-sort orders it — NEVER manual sortingOrder.
    /// </summary>
    [DisallowMultipleComponent]
    public class CrossClassEcho : MonoBehaviour
    {
        private const float FormTime = 0.4f;       // puff-in / dissolve duration (canon ~0.4s)
        private const float BodyAlpha = 0.6f;      // black body alpha
        private const float RimAlpha = 0.45f;      // cyan rim alpha (canon 0.3–0.7 band)
        private const float RimScale = 1.12f;      // rim sprite slightly larger than body
        private const float MoveSpeed = 22f;       // dash speed for melee echoes
        private const float StrikeTail = 0.35f;    // visible time after the strike before puff-out
        private const float ShoulderOffset = 0.4f; // ranged: ~24px over shoulder @ PPU64 ≈ 0.4u
        private const float NearCursorRadius = 6f; // melee target search radius around cursor/aim
        private static readonly Color CyanRim = new Color(0f, 1f, 0.8f, RimAlpha); // #00FFCC

        private SkillBase guestSkill;
        private CrossClassSkillData binding;
        private PlayerController player;
        private SpriteRenderer body;
        private SpriteRenderer rim;

        /// <summary>
        /// Spawn + run a Shadow Echo for the given guest skill. Static factory so callers
        /// (PlayerCrossClassBinding) need not assemble the GO. Null-guarded throughout.
        /// </summary>
        public static CrossClassEcho Spawn(PlayerCrossClassBinding owner, SkillBase guestSkill,
                                           CrossClassSkillData binding, PlayerController player)
        {
            if (guestSkill == null || binding == null) return null;

            var go = new GameObject($"ShadowEcho_{binding.guestSkillName}");
            var echo = go.AddComponent<CrossClassEcho>();
            echo.guestSkill = guestSkill;
            echo.binding = binding;
            echo.player = player;
            echo.StartCoroutine(echo.Run());
            return echo;
        }

        private IEnumerator Run()
        {
            // The strike phase can throw inside a guest SkillBase. A coroutine cannot yield inside a
            // try/catch, so we drive the strike phase through an inner enumerator wrapped in a manual
            // step loop that swallows + logs exceptions. Teardown (PuffOut + Destroy) then ALWAYS runs,
            // guaranteeing the silhouette despawns even if the guest skill faulted.
            Vector2 aim = ResolveAim();
            Health target = FindEchoTarget(aim);
            Vector3 spawnPos = ResolveSpawnPosition(binding.archetype, aim, target);

            transform.position = spawnPos;
            BuildSilhouette();

            yield return RunGuarded(RunStrike(aim, target));

            // Guaranteed teardown.
            yield return PuffOut();
            Destroy(gameObject);
        }

        /// <summary>Strike phase that may fault (puff-in → reposition → guest skill → tail).</summary>
        private IEnumerator RunStrike(Vector2 aim, Health target)
        {
            // Puff-in
            yield return PuffIn();

            // Melee echoes dash next to the target before striking; ranged stay over-shoulder.
            if (binding.archetype == EchoArchetype.Melee && target != null)
                yield return MoveTo(target.transform.position);

            // Perform the guest skill FROM the echo, aimed at the target.
            PerformGuestSkill(aim, target);

            // Visible tail before puff-out.
            yield return new WaitForSeconds(StrikeTail);
        }

        /// <summary>Pump an inner enumerator, logging (not propagating) any exception so the caller's
        /// teardown still runs. yield return cannot live in a try/catch, hence this manual MoveNext loop.</summary>
        private static IEnumerator RunGuarded(IEnumerator routine)
        {
            while (true)
            {
                object current;
                try
                {
                    if (!routine.MoveNext()) yield break;
                    current = routine.Current;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[Echo] guest skill threw — despawning silhouette anyway: {e}");
                    yield break;
                }
                yield return current;
            }
        }

        // ── Targeting / positioning ──────────────────────────────────────────────

        private Vector2 ResolveAim()
        {
            // Cursor-aim (project rule): aim toward the mouse from the player, fall back to facing.
            if (player != null)
            {
                var cam = Camera.main;
                if (cam != null && UnityEngine.InputSystem.Mouse.current != null)
                {
                    Vector2 mouseWorld = cam.ScreenToWorldPoint(
                        UnityEngine.InputSystem.Mouse.current.position.ReadValue());
                    Vector2 toMouse = mouseWorld - (Vector2)player.transform.position;
                    if (toMouse.sqrMagnitude > 0.01f) return toMouse.normalized;
                }
                if (player.FacingDirection.sqrMagnitude > 0.01f) return player.FacingDirection;
            }
            return Vector2.right;
        }

        private Health FindEchoTarget(Vector2 aim)
        {
            Vector2 playerPos = player != null ? (Vector2)player.transform.position : (Vector2)transform.position;
            // Search around a point out along the aim (matches cursor-aim intent).
            Vector2 searchPoint = playerPos + aim * NearCursorRadius * 0.5f;
            Health nearCursor = SkillRuntime.FindNearestEnemy(searchPoint, NearCursorRadius);
            return nearCursor != null ? nearCursor : SkillRuntime.FindNearestEnemy(playerPos, 20f);
        }

        private Vector3 ResolveSpawnPosition(EchoArchetype archetype, Vector2 aim, Health target)
        {
            Vector3 playerPos = player != null ? player.transform.position : transform.position;
            switch (archetype)
            {
                case EchoArchetype.Melee:
                    // Spawn ON the target (canon); fall back to over-shoulder if none.
                    return target != null ? target.transform.position : playerPos + (Vector3)(aim * ShoulderOffset);
                case EchoArchetype.Zone:
                    // At the aimed floor point.
                    return target != null ? target.transform.position : playerPos + (Vector3)(aim * NearCursorRadius);
                case EchoArchetype.SelfBuff:
                    return playerPos;
                case EchoArchetype.Ranged:
                default:
                    // ~0.4u over the player's shoulder toward the cursor.
                    Vector2 perp = new Vector2(-aim.y, aim.x);
                    return playerPos + (Vector3)(perp * ShoulderOffset);
            }
        }

        private IEnumerator MoveTo(Vector3 destination)
        {
            // Stop just short of the target center so the silhouette stands "next to" the mob.
            while ((transform.position - destination).sqrMagnitude > 0.25f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, destination, MoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // ── Guest skill invocation (B3) ───────────────────────────────────────────

        private void PerformGuestSkill(Vector2 aim, Health target)
        {
            if (guestSkill == null) return;

            // Aim the guest skill at the chosen target if we have one (so on-enemy / from-afar
            // strikes converge); otherwise keep the cursor aim.
            Vector2 effectiveAim = aim;
            if (target != null)
            {
                Vector2 toTarget = (Vector2)target.transform.position - (Vector2)transform.position;
                if (toTarget.sqrMagnitude > 0.0001f) effectiveAim = toTarget.normalized;
            }

            if (guestSkill.SupportsEchoOrigin)
            {
                // Fires from the echo's world position with the SAME damage/break/state logic.
                guestSkill.ExecuteAt(transform.position, effectiveAim);
            }
            else
            {
                // B3 FALLBACK: the guest skill hardcodes player origin (e.g. self-positioning skills
                // were curated out, but a non-migrated skill could be bound). Land a safe basic hit
                // on the chosen target from the echo so the action still reads as a strike.
                FallbackBasicHit(target);
            }
        }

        /// <summary>Safe fallback when a guest skill does not honor an origin override: a single
        /// modest hit on the target. Uses layer-agnostic Health (target already filtered to enemies).</summary>
        private void FallbackBasicHit(Health target)
        {
            if (target == null || target.IsDead) return;
            int dmg = Mathf.Max(1, binding != null ? Mathf.RoundToInt(binding.primaryValue) : 0);
            if (dmg <= 1) dmg = 20; // sensible default if binding has no value
            SkillRuntime.DealDamage(target, dmg, popup: true, attacker: player != null ? player.gameObject : gameObject,
                hitDirection: ((Vector2)target.transform.position - (Vector2)transform.position).normalized);
            Debug.Log($"[Echo] '{binding?.guestSkillName}' does not support origin override — used fallback basic hit.");
        }

        // ── Silhouette visual ─────────────────────────────────────────────────────

        private void BuildSilhouette()
        {
            Sprite sprite = LoadGuestIdleSprite(binding.sourceClass);

            // Cyan rim (drawn first / behind, slightly larger) — soft glow, no hard outline.
            var rimGo = new GameObject("Rim");
            rimGo.transform.SetParent(transform, false);
            rimGo.transform.localScale = Vector3.one * RimScale;
            rim = rimGo.AddComponent<SpriteRenderer>();
            rim.sprite = sprite;
            rim.color = CyanRim;
            ConfigureSort(rim);

            // Black body on top.
            body = gameObject.AddComponent<SpriteRenderer>();
            body.sprite = sprite;
            body.color = new Color(0f, 0f, 0f, 0f); // start invisible; PuffIn fades up
            ConfigureSort(body);
        }

        private static Sprite LoadGuestIdleSprite(SourceClass cls)
        {
            string folder = cls.ToString();           // SourceClass names match Resources folder names
            string lower = folder.ToLowerInvariant();
            // Resources.Load path is relative to a Resources/ folder, no extension.
            var sprite = Resources.Load<Sprite>($"Characters/{folder}/{lower}_idle_south");
            if (sprite == null)
            {
                // Fallback to a generic circle so the echo is still visible if art is missing.
                sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            }
            return sprite;
        }

        private static void ConfigureSort(SpriteRenderer sr)
        {
            // Custom-Axis Y-sort per project rule: single "Entities" layer, order 0, sort by Pivot.
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 0;
            sr.spriteSortPoint = SpriteSortPoint.Pivot;
        }

        // ── Puff in / out (minimal; B4 polishes) ──────────────────────────────────

        private IEnumerator PuffIn()
        {
            SpawnPuff(dissolve: false);
            float t = 0f;
            while (t < FormTime)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / FormTime);
                SetAlpha(BodyAlpha * k, RimAlpha * k);
                yield return null;
            }
            SetAlpha(BodyAlpha, RimAlpha);
        }

        private IEnumerator PuffOut()
        {
            SpawnPuff(dissolve: true);
            float t = 0f;
            while (t < FormTime)
            {
                t += Time.deltaTime;
                float k = 1f - Mathf.Clamp01(t / FormTime);
                SetAlpha(BodyAlpha * k, RimAlpha * k);
                yield return null;
            }
            SetAlpha(0f, 0f);
        }

        private void SetAlpha(float bodyA, float rimA)
        {
            if (body != null) { var c = body.color; c.a = bodyA; body.color = c; }
            if (rim != null) { var c = rim.color; c.a = rimA; rim.color = c; }
        }

        private void SpawnPuff(bool dissolve)
        {
            // B4: pixelated black-smoke + cyan-edge burst (procedural, point-filtered, palette-locked,
            // 12fps stepped) — reads as the echo materializing / dissolving. Replaces the two flat circles.
            EchoPuffBurst.Spawn(transform.position, FormTime, dissolve);
        }
    }
}
