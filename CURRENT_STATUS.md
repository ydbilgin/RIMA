# CURRENT STATUS
**2026-05-06 - S43 - Phase 1**

## Active Block
UI/minimap/combat-camera visual acceptance still OPEN -- requires Play Mode screenshot QA.
Full skill tree: LOCKED v0.1 -- TASARIM/SKILL_TREE_10CLASSES_CANONICAL.md
Skill pool doc: NOT written yet -- Codex dispatch failed repeatedly, retry next session.

## Locked This Session (2026-05-06)

### Design Systems (all LOCKED)
- Full skill tree 10x8: `TASARIM/SKILL_TREE_10CLASSES_CANONICAL.md`
- Basic Attack Contract 8-class: `TASARIM/BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md`
- RMB Redesign (all 10 classes): `TASARIM/CLASS_RMB_REDESIGN_2026-05-06.md`
- Summoner + Hexer full design: `TASARIM/SUMMONER_HEXER_CLASS_DESIGN.md`
- Cross-Class Proc System: `TASARIM/CROSS_CLASS_PROC_SYSTEM.md`
- Shadowblade Echo System: `TASARIM/SHADOWBLADE_ECHO_SYSTEM.md`
- Aim Shot + Boss Weak Spot + Area Skill Placement: `TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md`
- Rift Portal Opportunity: `TASARIM/RIFT_PORTAL_OPPORTUNITY.md`
- Makeup VFX Contract: `TASARIM/MAKEUP_VFX_CONTRACT.md`
- Dev Tool Plan: `TASARIM/DEV_TOOL_PLAN.md`

### Code (DONE this session)
- BasicAttackProfile infrastructure: commit 280a637 (laurethayday) -- 4 files created
- BuildFloorMask rect-first refactor: commit d9f08bd (laurethgame) -- all 16 layouts rewritten, architectural masonry aesthetic
- PlayerAttack.cs refactor: dispatched earlier, status UNKNOWN -- needs QC

### Doc (PENDING)
- Skill pool alternatives (10 classes): Codex running, not committed yet -- TASARIM/SKILL_POOL_ALTERNATIVES_2026-05-06.md

## Working Rules
- Record concrete results and unresolved complaints here.
- Keep details in linked files; this file stays compact.

## Done This Session (2026-05-05 evening)

### God Object Refactor
- `LargeDungeonMapPainterBase` extracted from RuntimeRoomManager.cs to own file (1481 lines)
- RuntimeRoomManager.cs trimmed from 2604 to 1132 lines
- Deleted 6 legacy/one-time scripts: _Legacy/RoomManager, _Legacy/HUDManager, IsoDummyRenderer, FixTextureImport, FixTilemapMaterials, TestSwitcher
- _Legacy folder removed entirely

### Graphify Chunk 3 (COMPLETE)
- Extracted 82 semantic nodes + 138 AST nodes from GUIDES/ + Tools/ + root docs
- Merged into main graph: 2862 nodes, 4683 edges, 129 communities
- graph.html + GRAPH_REPORT.md updated

### UI Overlay Decision (LOCKED)
- 3-layer UI approved: Combat HUD (always) / TAB Quick Overlay (semi-pause) / ESC Full Menu (pause)
- "Run Codex Build Overlay" concept demoted to reference-only
- character_menu_concept.png REJECTED (equipment grid violates RIMA rules)
- Next: Codex wireframe mockup for TAB overlay -> implement piece by piece

### Cleanup & Structure (earlier)
- Removed 435MB bloat (discord media, youtube video, old screenshots, artifacts)
- CODEX.md removed -> replaced by ANTIGRAVITY.md (studio agent stack definition)
- CLAUDE.md = universal rules for ALL agents (Claude, Codex, Gemini)
- AGENTS.md updated with Antigravity + mutual QC rule
- MEMORY/INDEX.md rebuilt: categorized, orphan-free
- .gitignore updated to prevent future bloat
- Two git checkpoints: `ad8d2c1` (full S43 state) + `c59fbb9` (cleanup)

### Graphify (partial)
- Ran on 355 files (Scripts + TASARIM + GUIDES + STAGING + MEMORY + Tools)
- 4/5 semantic chunks complete (chunk 3 = GUIDES + root docs + Tools hit daily token limit)
- Result: 2780 nodes, 4625 edges, 99 communities
- Output: `graphify-out/graph.html` (interactive), `graphify-out/GRAPH_REPORT.md`
- God objects found: `LargeDungeonMapPainterBase` (74 edges), `RuntimeRoomManager` (53 edges)

### Branding
- Logo transparent PNG: `TASARIM/UI_CONCEPTS/BRANDING/rima_logo_final_transparent_2026-05-05.png`
- Brand prompts written (title screen, steam capsule, thumbnail, social, loading screen, discord)

### UI Concept
- Run Codex / character build overlay concept added for Claude review and iteration:
  `TASARIM/UI_CONCEPTS/rima_run_codex_build_overlay_concept_2026-05-05.png`
- Intent: visual reference only, not final static UI. Use it to discuss HUD/build overlay composition,
  reusable panel language, route strip, skill bar, passive/echo rows, and RIMA-specific UI identity.
- Constraints to preserve: no equipment grid, no backpack-first RPG screen, no full route reveal,
  no baked final text. Runtime UI/minimap/combat-camera acceptance remains OPEN.

## LOCKED
- UI: No generic RPG equipment grid. RIMA-run-first.
- UI: HUD minimal (HP/resource top-left, skills bottom, minimap top-right).
- UI: In-world gate thresholds, color-coded.
- UI: 3-choice draft reward (Hades pattern).
- Act 1 name: Shattered Keep.
- Room gen: authored combat skeleton + connected naturalization.
- Gate sockets: blueprint-defined.
- PixelLab floor: Create Image Pro, 64px, 16 variations, isometric.
- Logo: Cyan rift wordmark = PRIMARY.
- Thumbnail: `dark_primary` direction (1 char + ghost echoes + cyan rift).

## Tooling Added (2026-05-06)
- `/p` skill: ~/.claude/commands/p.md -- Gemini 2.5 Flash prompt beautifier (Claude prompting best practices baked in)
- `/ask_gemini` skill: ~/.claude/commands/ask_gemini.md -- inline Gemini query
- NotebookLM MCP: added via `claude mcp add`, package installed, nlm login done (yasinderyabilgin@gmail.com)
- cx laurethayday exec syntax confirmed: `Set-Location <dir>; cx laurethayday exec -s danger-full-access -m o4-mini "prompt"`

## Next Priorities (Next Session)

### Immediate
1. Skill pool doc: Codex failed 3x -- retry with `cx laurethayday exec` directly (syntax now confirmed)
2. NotebookLM: feed RIMA docs (TASARIM/ + MEMORY/) into a notebook via nlm source add
3. PlayerAttack.cs: QC whether laurethgame refactor committed (git log check)
4. Tile generation: 8 undercroft tiles -- prompts ready at STAGING/PIXELLAB_TILESET_UNDERCROFT_2026-05-06.md

### Block 1 — Code
4. BasicAttackProfile OnCommitBeat event (needed for cross-class proc system)
5. Area Skill Placement input system
6. Unity compile check (LargeDungeonMapPainterBase + BuildFloorMask changes)

### Block 2 — Unity
7. TAB Overlay wireframe (Codex)
8. HUD implementation (HP bar, skill slots, minimap, buff icons)
9. Play Mode UI/minimap/gate QA

### Block 3 — Animation Production Prep
10. Movement sheet prompts: LMB + RMB per class (10 classes, 4 directions, frame spec)
11. Boss pose sheet spec: canvas, PPU, attack list per boss

## Latest Verification
- EditMode: 134/134 PASS.
- Script validation: HUDController, MiniMap, RuntimeRoomManager, SettingsMenuUI, MainMenuScreen, RoomPreviewPanel all PASS.
- PlayMode: smoke test may hang due to Time.timeScale=0 in main menu.

## Current Risks
- PlayMode smoke tests need Time.timeScale-aware waits.
- Graphify chunk 3 missing (not critical, add with --update).
- God objects (LargeDungeonMapPainterBase, RuntimeRoomManager) -- technical debt, Phase 1 acceptable.
- PixelLab 127px bug (128px outputs 127px) -- QC during floor test.
- Basic attack identity gap: only Warblade has full 3-step LMB combo in code; see `MEMORY/feedback_basic_attack_combo_identity.md`.
- BasicAttackProfile Codex task not yet started; Warblade still has no ScriptableObject boundary.
- Movement sheet prompts not yet written for any class.

## Key Pointers
- Graphify: `graphify-out/graph.html` + `graphify-out/GRAPH_REPORT.md`
- Logo: `TASARIM/UI_CONCEPTS/BRANDING/rima_logo_final_transparent_2026-05-05.png`
- Brand prompts: in conversation (title screen x6 variants)
- PixelLab external workflow review: `STAGING/PIXELLAB_MOVEMENT_SHEET_AND_MAP_WORKSHOP_REVIEW_2026-05-05.md`
- PixelLab Map Workshop isometric usage: `STAGING/PIXELLAB_MAP_WORKSHOP_ISOMETRIC_USAGE_NOTE_2026-05-06.md`
- 8-class basic attack contract (LOCKED): `TASARIM/BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md`
- Rift Portal design (LOCKED): `TASARIM/RIFT_PORTAL_OPPORTUNITY.md`
- Makeup VFX contract (LOCKED): `TASARIM/MAKEUP_VFX_CONTRACT.md`
- Dev Tool plan (LOCKED): `TASARIM/DEV_TOOL_PLAN.md`
- Elementalist matrix: `STAGING/ELEMENTALIST_FIRE_FROST_LIGHTNING_BUILD_MATRIX_2026-05-04.md`
- Act 1 room catalogue: `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md`
