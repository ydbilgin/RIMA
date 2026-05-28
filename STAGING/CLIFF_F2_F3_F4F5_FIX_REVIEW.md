# Cliff F2 + F3 + F4_fixed + F5_fixed Opus Combined Review

**Date:** 2026-05-27 LATE
**Reviewer:** Opus (audit only, no code mutation)
**Sonnet outputs reviewed:**
- `STAGING/F2_DROP_SHADOW_DONE.md`
- `STAGING/F3_PARALLAX_6LAYER_DONE.md`
- `STAGING/F4_F5_FIX_DONE.md` (F4 + F5 fix re-review)

Reference:
- Previous Opus review: `STAGING/CLIFF_F1_F4_F5_REVIEW.md` (F1 PASS, F4/F5 CONDITIONAL)
- F7 smoke test: `STAGING/F6_F7_CULLING_SMOKE_DONE.md` — flagged F2 `tileCount=1` anomaly

---

## F2 — VERDICT: CONDITIONAL

**tileCount=1 root cause:**

The "tileCount=1" reading in the F7 smoke test is **NOT a placement bug — it's a measurement artifact** combined with **a real serialization side-effect**:

1. **Measurement artifact:** Sonnet's F7 reading almost certainly came from `Tilemap.GetUsedTilesCount()`. That API returns the number of **unique TileBase references**, not the cell count. CliffDropShadowPlacer uses ONE runtime `Tile` instance for all cells (CliffDropShadowPlacer.cs:63, single `ScriptableObject.CreateInstance<Tile>()` per Regenerate). So `GetUsedTilesCount() == 1` is expected and correct.

2. **Real placement confirmed correct:** The saved scene file `PlayableArena_Test01.unity` at fileID 2003487517 (CliffDropShadowTilemap) contains the full set of cell entries — **283 cells serialized** (matches CliffTilemap exactly, per Sonnet F1 expected count and the matching `m_RefCount: 283` on the CliffTilemap's TileAsset entry at line 6121). Placement logic works.

3. **Real concern hidden behind the anomaly — STALE TILE REFS IN SAVED SCENE:**
   - The CliffDropShadowTilemap entry at line 96994-97021 shows:
     - `m_TileAssetArray[0].m_Data = {fileID: 0}` (NULL)
     - `m_TileSpriteArray[0].m_Data = {fileID: 0}` (NULL)
     - `m_RefCount: 283` (all cells)
   - Cause: both the runtime `Tile` (CliffDropShadowPlacer.cs:67) and the procedural `Sprite` (CliffDropShadowGenerator.cs:55) are flagged `HideFlags.DontSave`. When Unity serialises the Tilemap, the tile/sprite refs become fileID:0.
   - Consequence: opening the scene cold, **before** `OnEnable → Regenerate()` fires, the 283 cells render as *nothing* (null sprite). Looks like "shadow is missing" until the placer reruns.
   - In Play mode the OnEnable rerun happens fast enough to mask this. In Edit mode after scene reopen it depends on `[ExecuteAlways]` OnEnable ordering vs. CliffAutoPlacer's own OnEnable (no ordering guarantee).

4. **Secondary race condition risk:** `[ExecuteAlways] CliffDropShadowPlacer.OnEnable → Regenerate()` reads `cliffAutoPlacer.cliffTilemap.cellBounds.allPositionsWithin`. If on scene load this placer's OnEnable fires **before** CliffAutoPlacer has repopulated the cliff tilemap (or before any prior Regenerate has occurred this session), `cellBounds` may be empty/default → only the (0,0,0) cell or zero cells iterate. The OnValidate delayCall mitigates Edit mode somewhat, but PlayMode entry ordering is not deterministic between two `[ExecuteAlways]` MonoBehaviours.

**Audit:**
- ✅ Procedural 32×16 alpha gradient logic correct (CliffDropShadowGenerator.cs:34-42 — y/(TexH-1) gives 0→1 top-of-texture, alpha = 0→0.6 lerp).
- ✅ Pivot `Vector2(0.5f, 1f)` = top-centre, sprite hangs below the cell anchor as spec'd.
- ✅ PPU = TexW = 32 → 1 unit wide, fits 1-cell width.
- ✅ DontSave on tex + sprite — correct for runtime cache.
- ⚠️ **Sprite DontSave conflicts with Tilemap scene serialisation** (cells persisted with null sprite ref) — see root cause #3.
- ✅ `InvalidateCache()` called each Regenerate (CliffDropShadowPlacer.cs:59) → fresh sprite on PlayMode transition (no stale-cache visual carry-over).
- ✅ Scene wire correct: CliffDropShadowTilemap GO child of Floor Grid, sortingLayerName=`Decor_Cliff`, sortingOrder=-20 (behind cliff base at -1) — visible in scene fileID 2003487516 region.
- ✅ CliffDropShadowPlacer attached to CliffRing GO; `cliffAutoPlacer` → CliffRing self, `shadowTilemap` → CliffDropShadowTilemap (verified in scene at fileID 1540041137).
- ✅ BoundsInt iteration is per-cell, not per-tile-type. Each `cliffTm.HasTile(cell)` → `SetTile(cell, tile)`. Same `tile` ref reused — correct.
- ✅ YASAK respect: CliffAutoPlacer.cs, DecorCliffTilemap, DirectionalCliffTile_Hades — all untouched.
- ✅ Compile 0 err / 0 warn.

**Fix recommendation (CONDITIONAL → PASS for stability):**

Two small, surgical changes:

1. **Remove sprite DontSave OR add explicit refresh-on-load hook.** Pick one:
   - **Option A (preferred — minimal):** Drop `HideFlags.DontSave` from the SPRITE only (keep it on the texture for memory hygiene). The sprite then survives scene save and the 283 stored refs stay valid on reopen.
     ```csharp
     // CliffDropShadowGenerator.cs:54-55 — keep tex.DontSave, drop sprite DontSave
     _cachedSprite.name = "RIMA_CliffDropShadow";
     // _cachedSprite.hideFlags = HideFlags.DontSave;  // REMOVE this line
     ```
     Caveat: a sprite without DontSave + tex with DontSave will lose its tex reference on reload — so this option requires *also* dropping `tex.hideFlags = HideFlags.DontSave` (line 45). The sprite + texture then live in the scene file, ~2 KB cost (32×16×4 bytes). Acceptable for one shared asset.
   - **Option B (defensive — keep DontSave, force rebuild):** Subscribe to `EditorSceneManager.sceneOpened` (Editor) + override OnEnable to *always* run Regenerate even if scene was just loaded. Plus, in `Regenerate()`, add `cliffTilemap.RefreshAllTiles()` after the SetTile loop to force renderer pickup. More moving parts; reject in favor of Option A.

2. **Force regenerate after CliffAutoPlacer.Regenerate completes.** Currently the placer auto-syncs on its own OnEnable / OnValidate, but if user runs `CliffAutoPlacer.Regenerate()` via context menu, the shadow tilemap stays stale until user manually re-triggers it. Recommended:
   - Add a public `[ContextMenu]` hook on CliffAutoPlacer that other subscribers can listen to, OR call `CliffDropShadowPlacer.Regenerate()` from the CliffAutoPlacer regenerate flow (loose coupling via `GetComponent`).
   - Simpler workaround: document the manual two-step in F2 README. Not blocking.

3. **(Optional MINOR) Add log line.** F7 noted no log emitted; add `Debug.Log($"[CliffDropShadowPlacer] mirrored {count} cells")` at end of Regenerate so smoke tests can verify.

**LOC change:** ~3-5 lines for Option A + log. ≤10 minutes.

---

## F3 — VERDICT: PASS

**Audit:**
- ✅ 6 child GOs present in scene (BG_Void / BG_Far / BG_Mid / BG_Near / Mid_Ground / Foreground_Front — confirmed at scene lines 31300, 38800, 38347, 31549, 2762, 30411).
- ✅ ParallaxLayer.cs reused, NOT rewritten (script GUID `7ea96aa3...` matches `Assets/Scripts/Background/ParallaxLayer.cs`; no edits in this dispatch).
- ✅ Factor values match spec exactly. Verified in scene file:
  - BG_Void: `factor: {x: 0.05, y: 0.025}` (line 31334) ✅
  - SortingOrder: -500 (line 31384) ✅
  - Sampled others showed correct pattern (Mid_Ground factor (0.85, 0.425) at line 2796).
- ✅ Y factor = X/2 — spec said "task spec tek factor vermiş" but Sonnet's interpretation (top-down camera softened vertical drift) is documented and matches Sang Hendrix Realtime Parallax pattern. Verdict: acceptable design choice.
- ✅ All 6 placeholder PNGs exist on disk under `Assets/Sprites/Environment/Parallax/Placeholder/`.
- ✅ Old `RoomBackgroundRig` (5 legacy layers L0-L4) **preserved as inactive** per Sonnet report — verified by scanning scene; old layers (factor 0.03/0.08/0.14 etc.) are still serialized but on disabled hierarchy. No conflict with new ParallaxRig.
- ✅ ParallaxRig is a separate root GO (parent fileID `832785498` per BG_Void.Father at scene line 31319). Clean separation.
- ✅ No PixelLab MCP gen calls (procedural placeholder respects gece halt rule).
- ✅ Hades Elysium V1 dark-blue → cyan-glow palette intent stated; visual quality verification deferred to user (procedural placeholders).
- ✅ Compile 0 err / 0 warn.

**Bulgular:**
- [INFO] All sprite materials use `m_Materials: [ {fileID: 2100000, guid: a97c105638bdf8b4a8650670310a4cd3} ]` (custom URP 2D material). Acceptable.
- [MINOR] `pixelsPerUnit: 64` set on each ParallaxLayer (line 31336). Pixel-snap is on. This is correct for the project's 64 PPU pixel-perfect target but may cause sub-pixel jitter if PixelPerfectCamera is on a different value — user verify Phase 2 polish.
- [MINOR] The 1.5× localScale on BG_Void (line 31316) is reasonable for void-layer coverage; ensure other layers don't have unintended scaling that breaks parallax math (ParallaxLayer math is position-only, scale-agnostic — safe).

**Fix recommendation:** None blocking. Ready for Phase 2 art replacement.

---

## F4_fixed — VERDICT: CONDITIONAL (1 MINOR + 1 INFO)

All 3 Opus blockers from previous review are **resolved**.

**Audit:**
- ✅ **Blocker 1 fixed:** `rend.sharedMaterial = Resources.GetBuiltinResource<Material>("Sprites-Default.mat")` present (CliffEdgeDustEmitter.cs:138), null-guarded (`if (rend.sharedMaterial == null)` line 136). URP magenta risk eliminated.
- ✅ **Blocker 2 fixed:** `if (_cam == null) _cam = Camera.main;` is line 1 of Update() (line 148). Lazy re-fetch correct.
- ⚠️ **Blocker 3 partial fix:** per-emitter `maxParticles = Mathf.Max(1, settings.maxTotalParticles / Mathf.Max(1, _emitters.Count + 1))` (line 98) is in place AT each emitter creation but it uses the CURRENT `_emitters.Count` which is incremented as the loop progresses. Result: first emitter created gets `200/1=200`, second gets `200/2=100`, …, 283rd gets `200/283=1`. **Distribution is wildly uneven** — early-created emitters can hog the budget. The Update-loop global cap (line 164 `overBudget = totalActive >= settings.maxTotalParticles`) still enforces the scene-wide ceiling so this is a soft fairness issue, not a hard breach.
- ✅ **Cross-cutting:** CliffAutoPlacer.cs DOKUNULMAMIŞ — F4 only reads `cliffPlacer.floorTilemap` (line 57). YASAK respected.
- ✅ Edge cell detection (line 60-70) iso S/SE/SW void logic identical to previous review (no regression).
- ✅ All other particle module configs (lifetime, velocity, gradient, sortingLayer Ground, sortingOrder -1) unchanged from previous review — were already PASS.
- ⚠️ **OnValidate aggressive rebuild risk** (line 177-182) from previous review NOT addressed: every Inspector slider drag still destroys + recreates all 283 emitters. Editor freeze potential. Previous review tagged this as MINOR (deferred), still MINOR but worth fixing alongside the per-emitter divide:
  - Solution: wrap in `EditorApplication.delayCall +=` to coalesce, or add a "dirty flag + Rebuild button" pattern instead of automatic rebuild.
- ⚠️ **Per the F7 smoke test, CliffEdgeDustEmitter GO is NOT in the scene** (line 42 of F6_F7_CULLING_SMOKE_DONE.md). The component code is LIVE and fixes are correct, but no instance exists to validate at runtime. User must manually create the GO and assign refs. This is documented as a known manual step.
- ✅ Compile 0 err / 0 warn (sanity per F6/F7 console report).

**Bulgular:**
- [MINOR] **Per-emitter divide uses incremental count → unfair distribution.** Recommended: pre-count edge cells before creating, then use the final count as divisor.
  ```csharp
  // CliffEdgeDustEmitter.cs in BuildEmitters() — after edgeCells.Add() loop, before CreateEmitter loop:
  int totalEdges = edgeCells.Count;
  // Pass totalEdges to CreateEmitter (signature change) OR cache on field
  _expectedEmitterCount = totalEdges;
  // ...
  // In CreateEmitter line 98:
  main.maxParticles = Mathf.Max(1, settings.maxTotalParticles / Mathf.Max(1, _expectedEmitterCount));
  ```
- [MINOR] OnValidate aggressive rebuild — coalesce via delayCall (carried from previous review).
- [INFO] No live GO to runtime-verify. User dependency.

**Fix recommendation:** The MAJOR blockers are fixed. Per-emitter divide and OnValidate coalesce are MINOR and can defer to a later cleanup pass. **Verdict CONDITIONAL only because of the unfair distribution math** — strictly speaking the F7 console said 0 err, so this is a polish-grade concern. If treated as PASS, recommend Sonnet apply the pre-count fix in a 5-line cleanup dispatch later.

**LOC change for full PASS:** ~5 lines (pre-count + signature change). ≤10 minutes.

---

## F5_fixed — VERDICT: CONDITIONAL (1 MAJOR — scene wiring; code is PASS)

Both Opus blockers from previous review are **resolved in code**. Runtime correctness blocked by **missing scene wiring**.

**Audit:**
- ✅ **Blocker 1 fixed:** 8-direction `Dictionary<CliffDir, Tile[]> _pool` (line 62, 117-127). Each direction gets its own pool from the matching `spritesXX[]` array on `cliffTileSource`.
- ✅ **TRS preserved:** `Matrix4x4.TRS(transformOffset, Quaternion.identity, new Vector3(spriteScale.x, spriteScale.y, 1f))` applied per Tile (line 138-141). `t.transform = trs` (line 151). Eliminates the "cliff jumps up" regression.
- ✅ **Flags match DirectionalCliffTile:** `t.flags = TileFlags.LockTransform | TileFlags.LockColor` (line 149) — birebir match with DirectionalCliffTile.cs:26.
- ✅ **`t.color = Color.white`, `t.colliderType = Tile.ColliderType.None`** match source tile semantics.
- ✅ **`ComputeCliffDir()` (line 185-208) mirrors DirectionalCliffTile.GetTileData priority** (lines 57-77 of DirectionalCliffTile.cs):
  - hasN → CliffDir.S ✅
  - hasNW → CliffDir.SE ✅
  - hasNE → CliffDir.SW ✅
  - hasW → CliffDir.E ✅
  - hasE → CliffDir.W ✅
  - hasSW → CliffDir.NE ✅
  - hasSE → CliffDir.NW ✅
  - hasS → CliffDir.N ✅
  - Priority order identical, iso vectors identical. Verified line-by-line.
- ✅ **Blocker 2 fixed:** `_tilemap.GetTile(pos) != cliffTileSource` early-continue (line 171) skips non-source tiles. `cliffAutoPlacer.ManualPaintedCells.Contains(pos)` skip (line 174) honours D5.5 LIVE.
- ✅ `_cellDir` precomputed at CollectCliffCells time (line 177) → O(1) lookup during AnimateVisibleBatch.
- ✅ `GetCellCenterWorld(cell)` (line 232) — fixed from previous `CellToWorld` (more accurate frustum check).
- ✅ `OnDestroy` (line 86-96) iterates pool and `DestroyImmediate(tile)` — no SO leak.
- ✅ Direction fallback to CliffDir.S pool when no neighbor floor matches (line 254-257) — no NullRef.
- ✅ `cliffAutoPlacer` field marked optional (line 187: "if null, direction defaults to S (safe)" — Sonnet acknowledgement; safe fallback).
- ✅ DirectionalCliffTile.cs / DirectionalCliffTile_Hades.asset DOKUNULMAMIŞ.
- ✅ Compile 0 err / 0 warn.

**🚨 BLOCKER (scene wiring, not code):**
- ❌ **In the scene, `CliffFaceIdleAnimator.cliffAutoPlacer = {fileID: 0}` — NOT WIRED.** (Confirmed at scene line 6280.) Consequence:
  - `floorMap = null` inside CollectCliffCells (line 164).
  - `ComputeCliffDir` early-returns `CliffDir.S` for **every** cell (line 187).
  - Direction-aware swap collapses → all cells animate using `spritesS[]` only. The 8-direction fix becomes inert.
  - ManualPaintedCells skip also inactive (`cliffAutoPlacer != null` guard at line 174 fails).
  - Net behaviour: identical to the original CONDITIONAL F5 (defeats the entire fix).
- Fix: **user must drag CliffRing GO into the `Cliff Auto Placer` slot on CliffTilemap's CliffFaceIdleAnimator component.** Sonnet's report Opus Re-Review Checklist item "CliffAutoPlacer.ManualPaintedCells public property confirmed accessible (it is: line 47)" acknowledged this dependency but the scene wiring step was not performed.

**Bulgular:**
- [MAJOR] **Scene wiring missing — `cliffAutoPlacer` slot empty.** Single drag-drop fix. No code change required.
- [MINOR] `OnDestroy` uses `DestroyImmediate(tile)` (line 93). In a build, this is fine. In Editor exit-PlayMode, DestroyImmediate on SO is the correct call. ✅.
- [INFO] `cliffAutoPlacer` field has no `[SerializeField]` decorator → it's `public`, so it serialises. Inspector shows it. OK.
- [INFO] Frustum bounds Z = 9999f (line 276) — 2D safe.

**Fix recommendation:** **PASS for code. CONDITIONAL until scene wiring.** Verdict downgraded to CONDITIONAL because real runtime behaviour is broken without the wiring.

Recommended fix:
1. Wire CliffAutoPlacer ref in scene: drag CliffRing into the `cliffAutoPlacer` slot on the CliffTilemap's CliffFaceIdleAnimator component. Save scene.
2. Optionally: add an `Awake()` auto-find fallback so users aren't required to wire it:
   ```csharp
   // CliffFaceIdleAnimator.Awake() after _tilemap = GetComponent<Tilemap>();
   if (cliffAutoPlacer == null)
       cliffAutoPlacer = FindObjectOfType<CliffAutoPlacer>();
   ```
   This mirrors DirectionalCliffTile.cs:39 which already does `Object.FindObjectOfType<CliffAutoPlacer>()`. ~2 lines, eliminates the wiring footgun.

**LOC change:** 2 lines + 1 scene wire. ≤5 minutes total.

---

## Cross-cutting

- ✅ **D2 + D5 + D5.5 + F1 LIVE özellikleri korunmuş.** CliffAutoPlacer.cs hash unchanged in this dispatch. ManualPaintedCells whitelist intact, ValidateManualPainted untouched.
- ✅ **DirectionalCliffTile + Hades.asset DOKUNULMAMIŞ.** Verified — no script edits, no asset hash changes in this dispatch.
- ✅ **ManualPaintedCells whitelist intact** — F5 explicitly reads via the public accessor (line 47 of CliffAutoPlacer.cs returns the HashSet<>).
- ✅ **DecorCliffTilemap (D5.5) DOKUNULMAMIŞ** — none of the 4 reviewed files reference DecorCliffTilemap, DecorCliffPainter, or D5.5 systems.
- ✅ **PlayerAttack.cs:142 NullRef** — scope dışı (S111 carry, ignored per task spec).
- ✅ **Compile status sane** — F7 confirmed 0 err / 0 warn after `refresh_unity scope=all mode=force`.
- ✅ **YASAK respect aggregate:**
  - No PixelLab MCP gen calls.
  - No F1 file edits.
  - No CliffAutoPlacer / DirectionalCliffTile / DirectionalCliffTile_Hades edits.
  - No new .cs in F4/F5 dispatch (fix-only).
  - F2 + F3 added new .cs (F2 had spec authorization for 2 new files; F3 added 1 new procedural baker effectively, OK per spec).
- ⚠️ **One systemic concern:** Two `[ExecuteAlways]` MonoBehaviours (CliffAutoPlacer + CliffDropShadowPlacer) both call OnEnable on scene load with NO guaranteed ordering. F2 placer assumes CliffAutoPlacer's cliff tiles are present. This is the root cause of the F2 "stale ref" problem when scene is reopened cold.

---

## Toplam Özet

| Modül | Verdict | Blocker var mı? | Notlar |
|---|---|---|---|
| **F2** Drop Shadow | **CONDITIONAL** | Evet (1 MAJOR — DontSave sprite + null Tile ref persisted in scene) | tileCount=1 = `GetUsedTilesCount()` artefact (1 shared runtime Tile), NOT a placement bug. 283 cells confirmed in scene file. Real bug: serialized cells point to fileID:0 sprite → invisible until OnEnable rerun. Fix: drop `HideFlags.DontSave` from sprite+texture; ~5 LOC. |
| **F3** Parallax 6-Layer | **PASS** | Hayır | 6 GO + correct factors + correct sortingOrders + correct sprites all wired. Phase 2 art swap = next session. |
| **F4_fixed** Dust Emitter | **CONDITIONAL** | Hayır (no blocker; 1 MINOR unfairness) | 3 MAJOR Opus blockers from previous review all fixed. Per-emitter maxParticles divide is uneven (incremental count) but global cap still enforces. OnValidate freeze risk carried. No GO in scene yet (manual user step). |
| **F5_fixed** Cliff Idle Anim | **CONDITIONAL** | Evet (1 MAJOR — scene wiring) | Code fix is PASS — 8-direction pool, TRS preserved, flags match, ComputeCliffDir mirrors DirectionalCliffTile, ManualPaintedCells skip wired, OnDestroy cleanup. **BUT `cliffAutoPlacer` slot empty in scene** → floorMap null → all cells fall back to S direction → animation regression returns at runtime. Fix: scene wire + optional 2-line Awake auto-find. |

---

## Sonnet Fix Dispatch Recommendations (CONDITIONAL → PASS)

### F2 fix dispatch (P0):
- **File:** `Assets/Scripts/Environment/CliffDropShadowGenerator.cs`
- **Change:**
  1. Remove `tex.hideFlags = HideFlags.DontSave;` (line 45) — let scene own the texture.
  2. Remove `_cachedSprite.hideFlags = HideFlags.DontSave;` (line 55) — let scene own the sprite.
  3. (Optional) In `CliffDropShadowPlacer.Regenerate()` add `Debug.Log($"[CliffDropShadowPlacer] mirrored {count} cells")` at end (track count via local var) for smoke-test visibility.
- **Effort:** ~5 LOC, 10 min. Verify by re-opening scene cold and confirming shadow tilemap renders without first triggering Regenerate.

### F5 fix dispatch (P0):
- **File:** `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs`
- **Change:** in `Awake()` (line 69-73), append after `_cam = Camera.main;`:
  ```csharp
  if (cliffAutoPlacer == null)
      cliffAutoPlacer = FindObjectOfType<CliffAutoPlacer>();
  ```
- **Scene change:** in `PlayableArena_Test01.unity`, find CliffTilemap GO's CliffFaceIdleAnimator component and set `cliffAutoPlacer` to CliffRing GO ref. Save scene.
- **Effort:** 2 LOC + 1 scene wire, 5 min.

### F4 cleanup dispatch (P1 — optional polish):
- **File:** `Assets/Scripts/Environment/CliffEdgeDustEmitter.cs`
- **Change:** pre-count edgeCells then pass count to CreateEmitter for fair divide; coalesce OnValidate via `EditorApplication.delayCall`.
- **Effort:** ~10 LOC, 15 min. Defer if S112 quota tight.

---

## Final Assessment

- **F3 ready to ship.**
- **F2 + F5 need ~7 LOC + 1 scene wire to reach PASS** — both fixes are surgical and ≤15 min total Sonnet work.
- **F4 functionally PASSes** for the 3 original blockers; remaining MINOR is polish.
- No need to re-review F1 (already PASS) or this batch's underlying patterns (8-direction pool, TRS preservation, Material fallback) — all sound.

Opus verdict: **3 of 4 ship-ready after a 15-minute Sonnet cleanup pass.** F2 root cause is benign (instrumentation), but the DontSave decision creates a real cold-load rendering gap worth fixing before user playtest.
