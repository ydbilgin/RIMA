# S98 Autonomous Roadmap — EXPANDED

> **Owner:** Orchestrator (Claude Opus 4.7) dispatches; mutual QC enforced.
> **QC rule:** Codex output → Opus review. Opus/Sonnet output → Codex review.
> **Created:** 2026-05-22 (S97 LATE NIGHT 2 → S98 handoff)
> **Pivot LOCK:** Top-down + fake-iso (Children of Morta model)
> **PixelLab policy:** Sadece deneme — büyük gen batch'leri suspended. Wall/decoration sonraki sprint.

---

## Phase Overview

| Phase | Focus | Agent | PixelLab | Status |
|---|---|---|---|---|
| 0 | Modular tile transition test verdict | Orchestrator review | (Codex done) | b7nbaad9s in-flight |
| E | Unity setup + Warblade prefab + WASD wire | Codex → Opus QC | NO | Task ready |
| F | Cleanup (wang archive, painter scan paths) | Codex → Opus QC | NO | Pending |
| G | MapDesigner audit + iso-era purge | Codex → Opus QC | NO | Pending |
| H | JSON map loader + RoomManifest SO | Codex → Opus QC | NO | Pending |
| I | Act 1 Shattered Keep 6-room layout | Orchestrator design + Codex impl | Deferred | Pending |
| J | Door transitions polish (fade, mid-fight lock, save state) | Codex → Opus QC | NO | Pending |
| K | Full vertical slice test + verdict | Orchestrator + Codex | NO | Pending |

**Total scope:** ~8-12 saat autonomous. PixelLab budget korunuyor (~2,400 reserve).

---

## PHASE 0 — Modular Transition Verdict (15 dk, manual review)

**Inputs (Codex b7nbaad9s will deliver):**
- `STAGING/modular_test_verdict.md` — written verdict
- `Assets/Screenshots/ModularTest_v1.png` — paint test screenshot
- `Assets/Art/Tiles/Act1_ShatteredKeep/modular_pack_v1/transition_granite_rubble/` — transition PNG
- 16 + ~4-8 transition tiles in `Assets/Data/Tiles/Act1_ShatteredKeep/modular_v1/`

**Orchestrator verdict:**
- **PASS** (smooth granite↔rubble transition, no sharp seam, natural blend) → Phase E
- **TWEAK** (style_images approach works but quality issue) → defer transition expansion, proceed Phase E
- **FAIL** (no improvement vs Wang) → kullanıcıya sun, ChatGPT painted bg fallback değerlendir

**No new PixelLab gen here.** Just review + decision.

---

## PHASE E — Unity Setup + Warblade Prefab + WASD Wire (Codex, ~45 dk)

**Goal:** TopDownTest_Map1 sahnesinde Warblade WASD ile yürüsün, kamera takip etsin, console temiz olsun.

**Task file:** `STAGING/codex_task_phase_E_unity_setup.md`

**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_task_phase_E_unity_setup.md --effort high --profile yasinderyabilgin` (background)

**Mutual QC:** Codex done → orchestrator runs Opus review (read commit + check console + verify play mode behavior).

**Sub-tasks:**
1. Project Settings → Transparency Sort Axis = (0, 1, 0)
2. SpriteRenderer.sortPoint = Pivot (per-renderer enforce, scene-wide)
3. Verify/build Warblade prefab from current `TopDownTest_Map1` GameObject → `Assets/Prefabs/Characters/Warblade.prefab`
4. PlayerMovementController.cs (existing) WASD bind verify
5. CameraFollow.cs target = Warblade prefab instance
6. IsoSortingOrder.cs confirm `#if false` wrap (already done)
7. Play mode test (WASD 8-dir movement, camera follow, no console error)
8. Screenshot post-play: `Assets/Screenshots/Phase_E_warblade_wasd.png`

**Success criteria:**
- WASD movement smooth (no sliding, no input lag)
- Camera follow lerp natural (no jitter)
- 0 console error during 30s play test
- Sort order correct (Warblade behind back walls, in front of floor)
- Verdict file: `STAGING/phase_E_verdict.md`

---

## PHASE F — Cleanup (Codex, ~30 dk)

**Goal:** Wang artifact archive + painter scan path refactor + folder hygiene.

**Sub-tasks:**
1. **Archive Wang artifacts** (no delete):
   - `Assets/Art/Tiles/Act1_ShatteredKeep/wang_pack/` → `Assets/_ARCHIVE/Tiles/wang_pack_pre_modular/`
   - `Assets/Data/Tiles/Act1_ShatteredKeep/wang_rules/` → `Assets/_ARCHIVE/Tiles/wang_rules_pre_modular/`
2. **Painter scan paths refactor** (`Assets/Editor/RimaWorldPainterWindow.cs`):
   - Remove `wang_rules` scan path
   - Add `Assets/Data/Tiles/Act1_ShatteredKeep/modular_v1/` floor scan
   - Add `Assets/Prefabs/Environment/Walls/` (Phase B output target — empty for now)
   - Add `Assets/Prefabs/Environment/Decorations/` (Phase C output target — empty for now)
3. **Codex `*.corrupted_2026_05_21` cleanup** (b488445c residual)
4. **Empty folders prune:**
   - `Assets/Resources/Characters/extglob/` (if empty, delete)
   - `Assets/Prefabs/Rooms 1/` (suspected typo of `Rooms/` — confirm + delete if empty)
5. **STAGING archive sweep:**
   - Sprint S97 completed task reports → `STAGING/_archive/s97_completed/`
6. Verdict: `STAGING/phase_F_verdict.md`

**Mutual QC:** Codex → Opus review (verify no live ref broken, painter still scans, scene loads).

---

## PHASE G — MapDesigner Audit + Iso-Era Purge (Codex + rima-sonnet, ~1 saat)

**Goal:** Mevcut MapDesigner kod kapsamı tara — top-down/projection-agnostic kalanları LIVE, iso-era assumptions barındıranları `#if RIMA_ISO_LEGACY` wrap veya `_Archive/` taşı.

**Sub-tasks:**

**G.1 (rima-sonnet, analysis only):**
- `Assets/Scripts/MapDesigner/**` ve `Assets/Scripts/Map/**` audit
- Her .cs için: LIVE / NEEDS_ADAPT / ARCHIVE klasifikasyonu
- Rapor: `STAGING/mapdesigner_audit_report.md`

**G.2 (Codex, mechanical):**
- ARCHIVE listesindeki dosyaları `Assets/Scripts/MapDesigner/_Archive_iso_pre_topdown/` altına taşı (`git mv`)
- NEEDS_ADAPT listesindekiler için Codex'e tek tek dispatch (sub-batch)
- Compile check + console clean verify after each move batch

**G.3 (Codex):**
- `LegacyRuntimeRoomManager.cs` rename → `RuntimeRoomManager.cs` (legacy etiketi kalkacak)
- `IsoSortingOrder.cs` → `_Archive_iso_pre_topdown/` (zaten `#if false`, archive)
- `RoomBuilder.cs` (Hades-style 32×24 procedural) → KEEP (top-down compatible, primary builder)

**Mutual QC:** Codex done → Opus review (compile + console + scene load test on TopDownTest_Map1).

---

## PHASE H — JSON Map Loader + RoomManifest SO (Codex, ~1.5 saat)

**Goal:** Kullanıcı talebi: "JSON olarak çizersek yapılabilir değil mi?" — Evet. Room layout JSON-driven olacak.

**Architecture (detay: `STAGING/map_system_design_v1.md`):**

**H.1 — Data layer (Codex):**
- `Assets/Scripts/Map/Data/RoomManifestSO.cs` (ScriptableObject — id, dimensions, theme, connections array, json layout path)
- `Assets/Scripts/Map/Data/RoomLayoutJson.cs` (DTO — width/height, floor[][], walls[][], props[], mobs[], doors[], lighting, music_track)
- `Assets/Scripts/Map/Data/MapManifestSO.cs` (ScriptableObject — Act manifest: ordered room list, connection graph, save checkpoints)

**H.2 — Loader (Codex):**
- `Assets/Scripts/Map/Runtime/RoomLoader.cs` — JSON parse → Tilemap fill (Floor/Walls) + prop GameObject instantiate + door positioning
- `Assets/Scripts/Map/Runtime/RoomInstance.cs` — Runtime room state (mobs alive, doors locked, decals modified)
- Material→TileBase lookup: use modular_v1/ tile assets (granite/rubble/walkway/rift)

**H.3 — Editor tool (Codex):**
- `Assets/Editor/Map/RoomLayoutValidator.cs` — JSON schema validation, missing references warn
- Menu item: `RIMA ▸ Map ▸ Load Room JSON to Scene` (test loader manually)

**H.4 — Schema lock:**
- `STAGING/map_schema_v1.json` schema dosyası → `Assets/Data/Map/Schemas/room_v1.schema.json` (Unity'ye al)

**Mutual QC:** Codex done → Opus review (schema sanity + loader edge cases: malformed JSON, missing tile material, oversized grid).

---

## PHASE I — Act 1 Shattered Keep 6-Room Vertical Slice (Orchestrator design + Codex impl, ~2 saat)

**Goal:** İlk gerçek Act 1 oyun deneyimi — 6-7 oda, kapı geçişli, modular tile + Warblade.

**Layout (full detail: `STAGING/act1_shattered_keep_layout_v1.json`):**

| # | Room | Size | Theme | Connections | Notes |
|---|---|---|---|---|---|
| 1 | Entry Hall | Large 32×24 | Granite + archway center | S→Outside, N→2, E→3 | Boss giriş mahmuzu, atmospheric |
| 2 | West Chamber | Medium 24×18 | Rubble dominant, broken pillars | S→1, N→5 | First combat encounter (3 mobs) |
| 3 | East Corridor | Narrow 8×24 | Walkway + lateral rift | W→1, E→4 | Tight space, single-file enemies |
| 4 | Treasure Vault | Small 16×12 | Granite + rift accent center | W→3, N→5 | Optional, single chest + 1 elite |
| 5 | North Antechamber | Medium 20×16 | Mixed granite/rubble + rift veins | S→2/4, N→6 | Narrative beat, lore note + 0-1 mob |
| 6 | Shattered Throne (Boss) | Large 40×30 | Rift center radial, granite outer | S→5 | Act 1 boss arena |

**Sub-tasks:**

**I.1 (Orchestrator):**
- `STAGING/act1_shattered_keep_layout_v1.json` written — 6 room JSON
- Door connection graph diagram (ASCII): Entry→W/E/Throne flow

**I.2 (Codex):**
- 6 `*.asset` RoomManifestSO created in `Assets/Data/Map/Act1_ShatteredKeep/`
- 6 `*.json` room layout files in `Assets/Data/Map/Act1_ShatteredKeep/json/`
- 1 `MapManifest_Act1.asset` linking 6 rooms with door graph
- Tilemap pre-paint each room via RoomLoader manual call (Phase H tool)
- Save 6 scenes? OR 1 scene with Additive load? — **DECISION:** Use 1 scene `Assets/Scenes/Act1_ShatteredKeep.unity` with Room GameObject parent activated/deactivated per transition (cheaper than scene additive).

**I.3 (Codex):**
- Wire each room's DoorTrigger to MapManifest connections
- Wire RuntimeRoomManager to load next Room GameObject on door enter
- Camera bounds per room (CameraFollow.bounds = current room rect)

**Mutual QC:** Codex done → Opus review (walk through 6 rooms in play mode, verify transitions, no leak/double-load).

---

## PHASE J — Door Transitions Polish (Codex, ~1 saat)

**Goal:** Hades-tier oda geçiş hissi — fade, mid-fight lock, save state.

**Sub-tasks:**

**J.1 — Fade transition (Codex):**
- `Assets/Scripts/Core/RoomTransitionFX.cs` extend — black fade 0.3s out → load → 0.3s in
- Audio ducking + footstep mute during fade

**J.2 — Mid-fight door lock (Codex):**
- Door enters room → enemies in-room "claim" lock
- All enemies dead → doors unlock with VFX (glow + sound)
- `RuntimeRoomManager.OnEnemyDeath` → lock check
- Hades model — already in `LegacyRuntimeRoomManager` partially, polish + integrate

**J.3 — Save state (Codex):**
- Door cross → checkpoint save (room id, character state, inventory)
- Reload from checkpoint on death/exit
- `Assets/Scripts/Core/CheckpointSystem.cs` (new file, lightweight, JSON serialize to `PersistentDataPath/checkpoint_act1.json`)

**J.4 — Door visualization (Phase G+ deferred, but task file ready):**
- 3 door visualization variants exist (`STAGING/_pixellab_outputs/door_choice/`)
- Visual lock CHOICE pending — kullanıcıya sun, decision sonrası 1 visual canonical

**Mutual QC:** Codex done → Opus review (test 6-room walk: enter combat room → lock → kill mobs → unlock → next room → checkpoint reload).

---

## PHASE K — Full Vertical Slice Test + Verdict (Orchestrator + Codex, ~30 dk)

**Goal:** Act 1 baştan sona oyna, Hades-tier dungeon hissi verdict ver.

**Sub-tasks:**

**K.1 (Codex):**
- Play mode automated test (input replay if possible, otherwise manual checklist):
  - Spawn Entry Hall → walk to N door → unlock → enter West Chamber → 3 mob combat → unlock → North Antechamber → ... → Throne Room boss
  - 60 FPS verify (URP profiler frame check)
  - 0 console error verify (run_in_background output check)
- Screenshot per room: `Assets/Screenshots/phase_K_room_<n>.png`

**K.2 (Orchestrator + user):**
- Verdict: `STAGING/phase_K_verdict.md`
- 6 screenshot comparison vs reference (Children of Morta, Hades)
- Quality flags:
  - Tile material consistency (modular OK?)
  - Wall placeholder OK or breaks immersion?
  - Combat feel intact in top-down?
  - Transition juice OK or jank?
- **Stop conditions:** Console error 3+, frame drop <50 FPS, transition broken, save state lost → user sun.

---

## PixelLab Deferred Production Plan (next sprint, post-K)

When Phase K verdict PASS → wall + decoration PixelLab batch:
- Phase B (Walls): 5 batch ~150 gen `create_object` n_frames=16 → wall prefabs
- Phase C (Decorations): 3 batch ~100 gen → props, debris, lore objects
- Phase D infrastructure (Wang chain): SKIP — modular pipeline ↔ Wang autotile reduplicated effort, not needed
- Phase G-J production for Act 2-3 — deferred

---

## Autonomous Execution Protocol

Per phase:
1. Orchestrator: read prior phase verdict → confirm GO / NO-GO
2. Codex dispatch (background): `python cx_dispatch.py --task-file <task>.md --effort high`
3. Codex done notify → Opus review (orchestrator triggers `Agent` with `rima-design` or direct read)
4. Opus review verdict → if PASS commit, if FAIL repair task → Codex
5. CURRENT_STATUS.md update per phase
6. MEMORY.md index touch if new lock
7. Next phase trigger

**STOP conditions:**
- Console error 3+ ardışık → user sun
- Compile fail can't auto-fix → user sun
- Verdict FAIL → user sun
- Codex profile rate-limit → next profile fallback, else user sun
- Unity crash → STOP

**User authorization scope:** Phase E-K autonomous. Post-K (PixelLab production) ayrı session.

---

## Quick Reference

| File | Purpose |
|---|---|
| `STAGING/S98_autonomous_roadmap_expanded.md` | This file — master roadmap |
| `STAGING/map_system_design_v1.md` | Phase G-J architectural design |
| `STAGING/map_schema_v1.json` | Phase H JSON schema |
| `STAGING/act1_shattered_keep_layout_v1.json` | Phase I 6-room layout |
| `STAGING/codex_task_phase_E_unity_setup.md` | Phase E Codex dispatch task |
| `CURRENT_STATUS.md` | Update per phase |
| `MEMORY.md` | Touch per new lock |
