# Next Session Pickup (S94 mid-day handoff)

**Reason for restart:** UnityMCP MCP server disconnected mid-session; `.claude/settings.json` had `disabledMcpjsonServers: ["UnityMCP", "pixellab"]` which blocked reconnect. Settings now empty `[]` — MCP servers should re-register on Claude Code restart.

---

## Immediate next action (Wave E)

**Dispatch Sonnet UnityMCP agent to execute `STAGING/OPUS_WALL_FINAL_DECISION.md` Phase 1-10.**

Phase summary:
1. Asset import (PPU 64 config for 3 PixelLab walls in `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/`)
2. Scene cleanup (delete Wave 4 painterly_wall children from L3_Walls, KEEP Gate_Entry/Exit)
3. Spawn_01 composition (~30 SR per Opus Section 3 tables)
4. Warblade scale 0.85 → 0.5
5. Brazier cyan conversion (Light2D + FlameAdditive, Path A locked Karar #98 expansion)
6. Spawn_02 variant (+25 X offset, single brazier, no LG arch)
7. File pack cleanup (archive wall_edge_stone.png, delete STAGING test PNGs)
8. Screenshot `STAGING/walls_v3_spawn01.png` + `walls_v3_spawn02.png`
9. Test pass (read_console clean check)
10. Visual gate — ≥80% concept fidelity → COMMIT, <80% → Phase 2 mini-regen

ETA: ~90 min Sonnet UnityMCP execution.

## State of play (what's done)

### Files in pack (Assets/Art/AssetPacks/Act1_ShatteredKeep/)
- ✅ 3 PixelLab walls downloaded: `walls/pixellab_wall_section_horizontal.png` (b2703abf), `pixellab_wall_corner.png` (a3f9fcf1 — rejected purple-tinted), `pixellab_wall_arch_section.png` (1d73e775)
- ✅ Existing painterly_wall_01-12 set (Wave 4 still in scene)
- ✅ wall_edge_stone.png (Wave 4 superseded, archive target)
- ✅ gate_arch.png (active hero)
- ✅ wall_decoration_vines.png (accent + seam decal)

### Scripts created (Codex Step 1)
- `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomEntry.cs`
- `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs`
- `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs`
- `Assets/Editor/MapDesigner/EncounterMenu.cs`
- `Assets/Editor/MapDesigner/EncounterTemplateSOEditor.cs`
- dotnet build PASS, validator works

### Karar updates
- ✅ Karar #149 LIVE (sub-room encounter system, MASTER_KARAR_BELGESI)
- ✅ Karar #98 expanded via Path A — cyan #00FFCC also for braziers + containment lamps, behavior discipline (brazier 2s breath, rift 0.4s pulse)

### Memory persisted
- `feedback_token_management_weekly_2026_05.md`
- `feedback_object_state_animation_workflow_split.md`
- `project_karar_149_subroom_encounter_lock.md`
- MEMORY.md index updated

### Specs/research in STAGING/
- `OPUS_WALL_FINAL_DECISION.md` — execution-ready wall spec
- `OPUS_WALL_PRODUCTION_DESIGN.md` — Wave B (superseded)
- `RIMA_WALL_INVENTORY_AND_CANON.md` — Wave A inventory + canon
- `SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` — Codex implementation spec for Step 2-7
- `PIXELLAB_OBJECT_PRODUCTION.md` — Rift Batch 1 + Batch 2 IDs (duplicate, decide which to keep)
- `CODEX_DONE_subroom_encounter_review.md` — Codex APPROVE_WITH_REVISIONS verdict
- `codex_task_encounter_template_step1.md` — already dispatched DONE

### Rift objects (PixelLab, V3 web UI animation pending by user)
- Batch 1: wall_rift_small (22e83d26), wall_rift_large (c3729267), floor_rift_scar (24f477a0) + states
- Batch 2 duplicate: 2356cebc / d536f49c / c88143c5 + states (rima-asset agent fired same set)
- Decision needed: keep one batch, delete_object the other

### Patches Wave 5 (RIMA-theme)
- 3 patches: rift_seepage (7219918c) ✅, corrupted_moss (607e0f2d), void_ash (c8e3ba1a) ✅
- Need: download to file pack + L4_Patches rebuild dispatch (after Wave E)

## Open items / next waves

| Wave | Status |
|---|---|
| **Wave E** wall execution | **NEXT — dispatch after restart** |
| Wave F file pack cleanup follow-up | After Wave E |
| Wave G L4_Patches rebuild | Cleanup unused + use 3 RIMA-theme patches |
| Wave H Codex Step 2 dispatch (SubRoomSequenceController) | Independent, can dispatch parallel to Wave E |
| Wave I Rift Batch duplicate cleanup | After user V3 interpolation comparison |

## Token + budget state

- PixelLab balance: ~3500 / 5000 (340 gen spent on rift duplicate, 60 on patches)
- Codex: yasinderyabilgin profile silent-failed twice, laurethayday OR laurethgame fresh
- Sonnet weekly: liberal use, no concern
- Opus: 4× used this morning (Karar #149 spec, wall production B, final D, brazier scope)

## Session restart instructions for user

1. Save any unsaved work in Unity (just in case)
2. Close Claude Code
3. Re-open Claude Code in this project directory
4. New session start protocol: read `CURRENT_STATUS.md` + `.claude/PROJECT_RULES.md`
5. Read `STAGING/NEXT_SESSION_PICKUP.md` (this file) for handoff context
6. Say "Wave E dispatch" or paste this file
7. Verify UnityMCP MCP tools available via testing 1 simple call
8. Dispatch Wave E execution per Opus FINAL spec
