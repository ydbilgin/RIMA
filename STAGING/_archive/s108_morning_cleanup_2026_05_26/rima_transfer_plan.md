# RIMA Transfer Plan — RIMA_HD2D → RIMA
**Authored:** 2026-05-23 | **Session:** S103 pivot-back
**Status:** Cleanup + Direct copies EXECUTED. Adapted scripts await new-session user decision (Q1-Q5).

**Total source artifacts surveyed:** 100
- PNG textures: 6
- Prefabs: 44
- Materials: 6 (all SKIP)
- ScriptableObjects: 16 (11 footprints + 5 wall libs)
- C# scripts: 4
- URP settings: 1
- STAGING docs: 22
- MEMORY files: 1 scoped

**RIMA current state:** `Assets/` contained only `_Archive_2026-05-23/` Kenney iso assets. No scripts, no prefabs, no materials before transfer. Zero conflicts.

---

## Section 1 — Direct Transfer (executed)

29 items copied 1:1 (no conversion needed):
- 6 PNG textures (`Assets/Art/Environment/Walls/*.png`, `Floor/*.png`)
- 11 RoomFootprint .asset (`Assets/Data/Environment/Footprints/RF_*.asset`)
- 2 scripts (`RoomFootprint.cs`, `WallModuleLibrary.cs`)
- 10 STAGING docs (architecture_decision, procgen_*, opus_gap_analysis, modular_wall_*, phase4/5 plans, iqg_dual_grid_2d_research)
- `MEMORY/feedback_camera_lock_hd2d.md`

## Section 2 — Adapt-and-Transfer (NOT EXECUTED — awaits Q1-Q5)

### 2A Scripts
- **RoomShellBuilder.cs** → `RoomShellBuilder2D.cs` (7 edits: XZ→XY axis, MeshRenderer→SpriteRenderer in TintRenderers, Y-rotation→Z-rotation for west walls, pillar offset axes)
- **CameraLockController.cs** → `CameraLockController2D.cs` (3 edits: position, euler, drop orthographic.set)

### 2B Wall/Floor Prefabs (11 items — need SpriteRenderer + BoxCollider2D replace)
WallSegment_Straight/Cracked/Niche, PillarSegment, FloorTile_2x2/Sigil, WallSegment_NE_OuterCorner, WallSegment_NW_InnerCorner, WallSegment_Breach, WallSegment_Toppled, WallSegment_Heavy

### 2C Lighting (2 items)
- TorchLight.prefab → TorchLight2D.prefab (URP Point Light 3D → Light2D Point)
- RiftLight.prefab → RiftLight2D.prefab

### 2D Camera Rig (1 item)
- CameraRig_HD2D.prefab → CameraRig_2D.prefab (CameraLockController → 2D variant, Camera.orthographic stays, position/rotation = Q3 decision)

### 2E Materials — **SKIP ALL 6** (URP Lit 3D incompatible with SpriteRenderer)

### 2F URP Volume
- `DungeonVolume_Profile.asset` — copy with editor-open verification; if Missing overrides, recreate Bloom/Vignette/ColorGrading manually

### 2G WallLib SOs (5 items — copy + Inspector re-link)
WallLib_Standard/Damaged/Rift/Ritual/ShatteredKeep — copy structure, re-link prefab slots in Inspector after Section 2B prefabs exist

## Section 3 — DOC + MEMORY Transfer (executed direct + archive partial)

Direct annotated copies under `STAGING/`: architecture_decision, procgen_design_verdict, opus_gap_analysis (with HD2D→2D note), phase4/5 plans (with Physics→Physics2D notes), modular_wall_*, iqg_dual_grid_2d_research

Archive copies under `STAGING/archive_hd2d/` (for reference/history): codex_*_task.md, hd2d_hybrid_design_verdict, codex_hd2d_tech_review, LORA_TRAINING_GUIDE, phase1_qc_dispatch, codex_arch_review, autonomous_session_report

## Section 4 — Skip List (not transferred)

- All 6 .mat files (URP 3D Lit incompatible)
- `Prefabs/Environment/Walls/Act1_ShatteredKeep/` (16+5+5 prefabs) — dep on AssetPacks/ not in RIMA
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/` (third-party PixelLab pack)
- HD2D scenes (3D layout, not transferable)
- HD2D-build QC/review docs (not actionable in 2D)
- CameraRig_HD2D.prefab raw (references 3D controller)

## Section 5 — Order of Operations

**Phase A: Infrastructure** ✅ executed (directories created in RIMA)
**Phase B: Direct copies** ✅ executed (29 items + docs)
**Phase C: Verify Console** — user does in new session opening RIMA
**Phase D: Adapted scripts** ⏸ waits user Q1-Q5
**Phase E: 2D prefabs** ⏸ waits user
**Phase F: WallLib re-link** ⏸ waits user
**Phase G: Smoke test** ⏸ waits user

## Section 6 — Decision Points for User (NEW SESSION)

**Q1 — cellSize unit scale**
RoomFootprint.cellSize was `2f` (3D units). RIMA 2D PPU=64, tile=32px. Options:
- Keep `2f` → 12-cell room = 24 Unity units
- Change to `1f` → 12-cell room = 12 Unity units
- Recommend: depends on PPU lock + room visible-area preference

**Q2 — West-wall rotation in 2D**
HD2D rotated west walls `Quaternion.Euler(0,90,0)`. In 2D top-down no Y-rotation. Options:
- A) Separate west sprite (2 extra PNGs)
- B) Sprite flipX (free, identical appearance)
- C) Single sprite for both N+W (Hammerwatch style)
- **Recommend C for MVP**

**Q3 — Camera lock values 2D**
HD2D `(12,8,-12)` rot `(35,315,0)` ortho 9 invalid. Suggest:
- Position `(0, 0, -10)`, Rotation `(0,0,0)`, OrthoSize ~8 (12-cell room + padding)
- User confirms visible-area pref

**Q4 — Corner/variant prefabs day-1**
NE_OuterCorner, NW_InnerCorner, Breach, Toppled, Heavy have NO PNG in HD2D (used 3D mesh shapes). Options:
- A) Skip (null slots) — visually incomplete
- B) Placeholder `stone_wall_b_cracked.png` for all variant slots
- C) PixelLab batch FIRST then build
- **Recommend B as day-1 placeholder, dispatch PixelLab batch next**

**Q5 — RoomShellBuilder vs existing RIMA system**
RIMA Assets/Scripts/ was empty pre-transfer (confirmed). RoomShellBuilder2D is clean addition. No merge conflict.

---

## What ORCHESTRATOR executed in this transfer

✅ Section 1 (29 items)
✅ Section 3 direct + archive doc copies
✅ Section 5 Phase A + B
⏸ Section 2 (Phase D-G) — awaits Q1-Q5 user decisions in new session

**Next session quick start:** Open RIMA in Unity → Console check → answer Q1-Q5 → orchestrator dispatches Codex for Phase D-G adaptation.
