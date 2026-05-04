# CURRENT STATUS
**2026-05-05 - S43 - Phase 1**

## Active Block
UI/minimap/combat-camera visual acceptance still OPEN — requires Play Mode screenshot QA.
Full skill tree (10 class x 8 skills) definition needed before production moves forward.

## Working Rules
- Record concrete results and unresolved complaints here.
- Keep details in linked files; this file stays compact.

## Decisions Made This Session (2026-05-05)

### LOCKED
- UI: No generic RPG equipment grid. RIMA-run-first (active kit, passives/echoes, route/room, reward).
- UI: HUD minimal (HP/resource top-left, skills bottom, minimap top-right). Hades reference.
- UI: In-world gate thresholds, not UI arrows. Gate types color-coded.
- UI: 3-choice draft reward (Hades pattern).
- Act 1 name: Shattered Keep.
- Act form: Act1 controlled ruins, Act2 living wound (body-horror), Act3 void-gold architecture, Final mirror Nexus Core.
- Room gen: authored combat skeleton + connected naturalization. No random scatter.
- Room gen: code-side descriptor prototype first, LDtk/Tiled later (Phase 2-3).
- Route map: forward branching from prepared pool. Fragments reveal next 1-2 nodes only.
- Gate sockets: blueprint-defined. Sabit DoorEast/West/North/South = placeholder only.
- PixelLab floor REVISED: Create Tiles PRO only supports 16x16/32x32 (top-down/sidescroller).
  For isometric 64px floor: use Create Image Pro, 64px, 16 variations, isometric prompt.
  128px tile test still valid as style mining source. Downscale or use as reference.
- Flat top-surface: LOCKED.
- Logo: Cyan rift wordmark = PRIMARY. Amber = secondary/thematic only.
- Thumbnail direction: `dark_primary` (1 character + ghost echoes + cyan rift). Party thumbnail REJECTED.
- Output Economy: terse for mechanical work, nuanced for design. Auto-toggle.
- Context Mode MCP: installed, active next session.
- Multi-account: Bedrock (Opus 4.6 main) + laurethgame (Opus 4.7 heavy tasks, separate terminal).

### PROPOSED (awaiting implementation)
- Elementalist fire/frost/lightning matrix — pure lanes stronger, mixed lanes utility.
- Full skill tree: 10 class x 8 skills draft (Codex + Claude review).
- 7 RIMA skills to create: room-blueprinter, ui-designer, pixellab-module-planner, skill-scaffold, sprite-import, anim-wire, session-bridge.

## Latest Pointers
- UI template concept: `TASARIM/UI_CONCEPTS/rima_ui_template_concept_2026-05-04.png`
- Gate socket concept: `TASARIM/UI_CONCEPTS/rima_gate_socket_concept_2026-05-04.png`
- Branding: `TASARIM/UI_CONCEPTS/BRANDING/` (logo, key art, thumbnail)
- Elementalist matrix: `STAGING/ELEMENTALIST_FIRE_FROST_LIGHTNING_BUILD_MATRIX_2026-05-04.md`
- Act 1 room catalogue: `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md`
- Room connected gen proposal: `TASARIM/ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md`
- PixelLab env notes: `TASARIM/PIXELLAB_ENVIRONMENT_MODULE_NOTES_PENDING_CLAUDE_2026-05-03.md`
- UI/QA/AI skill notes: `TASARIM/UI_QA_AND_AI_SKILL_RECOMMENDATIONS_2026-05-04.md`
- UI state blueprint: `TASARIM/RIMA_UI_STATE_BLUEPRINT_2026-05-04.md`
- UI production rules: `TASARIM/UI_PRODUCTION_RULES_FROM_OPUS_REVIEW_2026-05-04.md`
- Discord analysis (PixelLab): from Antigravity session, captured in this status
- Detailed pre-compact snapshot: `MEMORY/status_snapshot_current_2026_05_03_before_compact.md`

## Latest Verification
- EditMode: 134/134 PASS.
- Script validation: HUDController, MiniMap, RuntimeRoomManager, SettingsMenuUI, MainMenuScreen, RoomPreviewPanel all PASS.
- PlayMode: smoke test may hang due to Time.timeScale=0 in main menu. Test-design issue.

## Current Risks
- PlayMode smoke tests need Time.timeScale-aware waits.
- Worktree very dirty — commit frequently.
- Some room preview names mapped to approximate layouts, not exact Act 1 masks.
- PixelLab 127px bug (128px outputs 127px) — QC during floor test.

## Next Priorities
1. PixelLab floor test: Create Image Pro, 64px, 16 variations, isometric flat stone prompt.
2. ChatGPT logo redo: FT/RCH integration failed — needs better prompt (see below).
   Base file: `TASARIM/UI_CONCEPTS/BRANDING/rima_logo_wordmark_stone_rift_source_2026-05-04.png`
   Problem: ChatGPT placed FT/RCH as separate floating letters, not integrated into crack edges.
   Next prompt must: embed letters INTO the rift crack edges as carved/etched stone relief, not overlay.
3. Full skill tree draft (10 class x 8 skills).
4. Play Mode UI/minimap/gate QA (screenshot-by-screenshot).
5. Room descriptor prototype (8 rooms in code).
6. 7 RIMA skills creation.
7. CURRENT_STATUS archived snapshot + lint.
