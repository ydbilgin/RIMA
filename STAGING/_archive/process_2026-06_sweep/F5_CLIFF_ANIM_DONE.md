# F5 Cliff Face Idle Animator — DONE

Date: 2026-05-27

## Deliverables

| Item | Status |
|---|---|
| `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs` | NEW — 0 err / 0 warn |
| `Assets/Scenes/Test/PlayableArena_Test01.unity` | CliffTilemap component attached + wired |

## What was built

`CliffFaceIdleAnimator` (MonoBehaviour, ~120 LOC) attaches to any CliffTilemap GameObject.

**Startup:**
- Validates `cliffTileSource.spritesS` has ≥ 2 sprites (warns + disables otherwise).
- Builds a pool of plain `Tile` ScriptableObjects (one per spritesS variant) at Start — no per-frame allocation.
- Scans the tilemap's cellBounds once to collect all occupied cells into `_cliffCells`.

**Animation loop (Coroutine):**
- Waits `Random.Range(cycleIntervalMin, cycleIntervalMax)` seconds (default 3–5 s) between cycles.
- Camera frustum check (orthographic bounds, world-space) — only visible cells are candidates.
- Fisher-Yates shuffle of visible candidates, then takes up to `maxAnimatedCells` (default 20).
- Each selected cell: deterministic hash(cell.x, cell.y) → pick next variant index → `tilemap.SetTile(cell, variantTile)`.

**Per-cell phase offset:** hash function identical to `DirectionalCliffTile.DeterministicSeed` — different cells advance to different variants, preventing uniform lockstep.

**Constraints respected:**
- `DirectionalCliffTile.cs` — not touched.
- `DirectionalCliffTile_Hades.asset` — not touched.
- No AnimatedTile assets created.
- `mcp__UnityMCP__refresh_unity scope=all mode=force` — executed after script creation.

## Inspector fields (CliffTilemap → CliffFaceIdleAnimator)

| Field | Value set in scene |
|---|---|
| Cliff Tile Source | DirectionalCliffTile_Hades |
| Cycle Interval Min | 3 s |
| Cycle Interval Max | 5 s |
| Max Animated Cells | 20 |

## Verify checklist

- [ ] Enter PlayMode → cliff sprite cells periodically swap to different spritesS variants
- [ ] No stutter / FPS drop (max 20 SetTile calls per 3-5 s interval)
- [ ] Off-screen cells are skipped (frustum check)
- [ ] Console: 0 err / 0 warn

## Next

Opus visual coherence review (F5 PASS criterion per task spec).
