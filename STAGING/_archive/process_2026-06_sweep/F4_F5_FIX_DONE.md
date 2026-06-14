# F4 + F5 Fix Done

**Date:** 2026-05-27 LATE
**Agent:** Sonnet (impl)
**Input:** `STAGING/CLIFF_F1_F4_F5_REVIEW.md` (Opus CONDITIONAL × 2)
**Compile:** 0 err / 0 warn (Unity MCP refresh force + read_console verified)

---

## Changed Files

### 1. `Assets/Scripts/Environment/CliffEdgeDustEmitter.cs` (F4 — 3 surgical patches)

| Blocker | Fix | Lines |
|---|---|---|
| MAJOR: URP magenta — ParticleSystemRenderer.material unset | `rend.sharedMaterial = Resources.GetBuiltinResource<Material>("Sprites-Default.mat")` inside CreateEmitter, guarded by `if (rend.sharedMaterial == null)` | ~136-140 |
| MAJOR: Camera.main re-fetch missing | `if (_cam == null) _cam = Camera.main;` added as first line of Update() | ~148 |
| MINOR: per-emitter maxParticles = global cap → transient overshoot | `main.maxParticles = Mathf.Max(1, settings.maxTotalParticles / Mathf.Max(1, _emitters.Count + 1))` | ~98 |

### 2. `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs` (F5 — full revision ~200 LOC)

| Blocker | Fix |
|---|---|
| MAJOR: Plain Tile pool — transformOffset + direction-aware sprite LOST | Replaced `Tile[] _variantTiles` (spritesS-only, no TRS) with `Dictionary<CliffDir, Tile[]> _pool` (8-direction). Each Tile gets `t.transform = Matrix4x4.TRS(cliffTileSource.transformOffset, Quaternion.identity, new Vector3(spriteScale.x, spriteScale.y, 1f))` and `flags = LockTransform | LockColor`. |
| MAJOR: CollectCliffCells skips neither cliffTileSource check nor ManualPaintedCells | Added `_tilemap.GetTile(pos) != cliffTileSource` early-continue + `cliffAutoPlacer.ManualPaintedCells.Contains(pos)` skip. |

Additional fixes applied (Opus MINOR + cross-cutting):
- `ComputeCliffDir()` method added — mirrors DirectionalCliffTile.GetTileData priority (hasN→S, hasNW→SE, etc.) using iso vectors.
- `_cellDir` cache dictionary populated at CollectCliffCells time so per-cell direction is O(1) at animation time.
- `GetCellCenterWorld` replaces `CellToWorld` for accurate frustum check.
- `OnDestroy` cleans up ScriptableObject pool instances (`DestroyImmediate`).
- `cliffAutoPlacer` field is optional (inspector-assignable); if null, direction defaults to S (safe).
- Direction fallback: if pool missing for computed direction, falls back to `CliffDir.S` pool.

---

## NOT Changed (YASAK respected)

- `CliffAutoPlacer.cs` — LIVE, untouched
- `DirectionalCliffTile.cs` — LIVE, untouched
- `DirectionalCliffTile_Hades.asset` — LIVE, untouched
- F1 files (`CliffAutoPlacerEditor.cs` etc.) — PASS, untouched
- No new .cs files created

---

## Opus Re-Review Checklist

### F4
- [ ] PlayMode: ParticleSystem visible (no magenta squares) — Sprites-Default.mat assigned
- [ ] PlayMode: Camera.main null on Start → lazy re-fetch in Update() works after scene reload
- [ ] PlayMode: emitter maxParticles ~ (200 / emitterCount) per emitter, not 200 each
- [ ] LOD: emission disabled for emitters >20u from camera

### F5
- [ ] PlayMode Start: CollectCliffCells only picks up cells where `GetTile == cliffTileSource`
- [ ] PlayMode Start: cells in ManualPaintedCells (D5.5 LIVE) are excluded from `_cliffCells`
- [ ] Cycle fires: cliff sprites swap using same-direction variant (S cell stays S, SE stays SE)
- [ ] After swap: no cliff cell "jumps" — transformOffset preserved via Tile.transform TRS
- [ ] After swap: spriteScale preserved via Tile.transform TRS
- [ ] Frustum: only visible cells animate (GetCellCenterWorld used)
- [ ] OnDestroy: no ScriptableObject leak (DestroyImmediate pool)
- [ ] Direction fallback: cell with no matching floor neighbor uses S pool (no NullRef)

### Cross-cutting
- [ ] 0 compile errors / 0 compile warnings after Unity refresh
- [ ] CliffAutoPlacer.ManualPaintedCells public property confirmed accessible (it is: line 47)
- [ ] Pre-existing PlayerAttack NullRef (PlayerAttack.cs:142) unrelated — ignore
