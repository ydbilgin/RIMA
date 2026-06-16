# AUDIT — REWARD-02 fix (RewardPickup.cs) — auditor — 2026-06-16

VERDICT: **PASS** (PASS-with-nits; nits are MINOR, none block demo)

Method: READ-ONLY static + git-diff. Unity NOT run (compile already 0-error per brief).
Scope audited: RewardPickup.cs diff, RoomRunDirector.SpawnRewardPickup/ResolveRewardSpawnPosition,
RuntimeRoomManager spawn path, RoomClearVictoryTrigger spawn path, PlayerController RB2D setup,
ProjectSettings/Physics2DSettings.asset, TagManager.asset.

## Q2 — Awake-timing reliability of OverlapCircleAll (the brutal one): RELIABLE
Decisive facts:
- Physics2DSettings: `m_AutoSyncTransforms: 0` (off). BUT in Unity 6 (6000.3.6f1) physics QUERIES
  (OverlapCircleAll/Raycast/...) internally flush dirty transforms before executing even when
  auto-sync is off. So the freshly-set reward `transform.position` IS seen by the query. No manual
  `Physics2D.SyncTransforms()` exists in the codebase — and none is needed for the query side.
- Ordering in SpawnRewardPickup: position set (1381) → CircleCollider2D added (1417) → 
  AddComponent<RewardPickup>() (1421) fires Awake synchronously → CheckInitialPlayerOverlap.
  The reward's own collider is registered BEFORE the query runs. Good.
- `m_QueriesHitTriggers: 1` and `m_QueriesStartInColliders: 1` → trigger colliders are detected
  and a query starting inside a collider reports it. So the initial overlap WILL be found.
- Player Transform is authoritative-synced after each FixedUpdate (Dynamic RB2D moved by
  linearVelocity, writeback to Transform), so its position is current at clear-time.
CONCLUSION: the OverlapCircleAll leg is NOT a no-op. It is the primary catch. OnTriggerStay2D is a
correct belt-and-suspenders backup (fires while overlapping; player carries a Dynamic RB2D so the
contact pair stays awake) but is not load-bearing here. Both legs are valid; redundancy is fine.

## Q1 Correctness: PASS. Overlap-at-spawn → CheckInitialPlayerOverlap sets playerInRange + prompt
in Awake → G works same frame. Soft-lock premise confirmed real: RewardAutoCollectTimeoutSec=0f
(RoomRunDirector:1194) so there is NO timeout fallback on that path — the overlap fix is the only
recovery. Fix is on the correct seam.

## Q3 Regression / double-ShowPrompt / double-Collect: PASS.
- `playerInRange` guard + `collected` guard gate every entry (Enter/Stay/initial). Stay & initial
  both early-return once playerInRange is true, so no repeated work.
- ShowPrompt is idempotent (HUDController.SetInteractionPrompt just sets text+activates panel;
  promptCanvas.enabled=true). Double-call = harmless.
- Collect() is self-guarded (`if (collected) return`) and disables its own collider, so no double
  Collect. ClearPlayerRange early-returns when !playerInRange — unaffected, still correct.

## Q4 Edge cases: PASS.
- lossyScale at Awake = pickupVisualScale(1.1) before RewardSpawnPop shrinks to 0 then re-grows.
  The radius computed in CheckInitialPlayerOverlap uses the at-Awake scale; clamped to >=0.1. The
  pop animation runs AFTER Awake, so it cannot affect the one-shot check. No issue.
- OverlapCircleAll uses no layer mask (all layers) + CompareTag("Player") filter → non-player hits
  (incl. the reward's own trigger) are ignored. Correct.
- Alloc: OverlapCircleAll allocates once per reward spawn (~once/room). Negligible.

## Q5 Coverage: PASS, and BROADER than asked.
Fix lives in the RewardPickup component, so it covers ALL spawn paths:
  (1) RoomRunDirector.SpawnRewardPickup (the bug source — center spawn),
  (2) RuntimeRoomManager.SpawnRewardInFrontOfPlayer (prefab, in-front — overlap unlikely but safe),
  (3) RoomClearVictoryTrigger.SpawnRewardPickup:155 (3rd path, NOT named in brief; same
      collider-before-AddComponent pattern → also covered).

## Q6 Decision-doc compliance: PASS. Pure code change; touches no prefab, no collider asset, no
G-input flow contract. Matches "asset production must not change prefab collider / G-input flow."

## NITS (MINOR — optional, do not block):
- N1 [MINOR] CheckInitialPlayerOverlap radius default 0.5f only applies if the collider is NOT a
  CircleCollider2D. All 3 spawn paths use CircleCollider2D, so the branch is always taken in
  practice; the 0.5 default is dead-but-safe. Fine as defensive code.
- N2 [MINOR] Belt-and-suspenders: if you ever want zero reliance on the (correct) query-syncs-
  dirty-transform behavior, you could add `Physics2D.SyncTransforms()` immediately before the
  OverlapCircleAll. NOT required for Unity 6 — query auto-flush already guarantees it. Listed only
  for completeness; do not add for the demo (no value, extra cost).
- N3 [MINOR] No forbidden patterns found: no network calls, no Stop-Process, no path traversal,
  no out-of-bounds writes (review-only target was a single .cs anyway).

BINDING FIX: none required. Ship as-is.
