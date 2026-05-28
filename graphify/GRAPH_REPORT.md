# Graph Report - .  (2026-05-26)

## Corpus Check
- Corpus is ~30,486 words - fits in a single context window. You may not need a graph.

## Summary
- 366 nodes · 383 edges · 39 communities detected
- Extraction: 85% EXTRACTED · 15% INFERRED · 0% AMBIGUOUS · INFERRED: 57 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Memory + Cleanup + Project Index|Memory + Cleanup + Project Index]]
- [[_COMMUNITY_Cliff System S107|Cliff System S107]]
- [[_COMMUNITY_PixelLab Tools + Animation API|PixelLab Tools + Animation API]]
- [[_COMMUNITY_Act 1 Rooms + Blueprints|Act 1 Rooms + Blueprints]]
- [[_COMMUNITY_PixelLab Pro Tool Set|PixelLab Pro Tool Set]]
- [[_COMMUNITY_Animation + Encounter Rules|Animation + Encounter Rules]]
- [[_COMMUNITY_Status Format + Skill Audits|Status Format + Skill Audits]]
- [[_COMMUNITY_Path C Hybrid Pipeline|Path C Hybrid Pipeline]]
- [[_COMMUNITY_Agent Routing + Authority|Agent Routing + Authority]]
- [[_COMMUNITY_Direction System (8-way)|Direction System (8-way)]]
- [[_COMMUNITY_PixelLab Halt + Warblade|PixelLab Halt + Warblade]]
- [[_COMMUNITY_PixelLab Budget + Costs|PixelLab Budget + Costs]]
- [[_COMMUNITY_Sub-Room Tags + EncounterTemplate|Sub-Room Tags + EncounterTemplate]]
- [[_COMMUNITY_Discord Research + Risk|Discord Research + Risk]]
- [[_COMMUNITY_NotebookLM Auth + Sources|NotebookLM Auth + Sources]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]
- [[_COMMUNITY_Community 20|Community 20]]
- [[_COMMUNITY_Community 21|Community 21]]
- [[_COMMUNITY_Community 22|Community 22]]
- [[_COMMUNITY_Community 23|Community 23]]
- [[_COMMUNITY_Community 24|Community 24]]
- [[_COMMUNITY_Community 25|Community 25]]
- [[_COMMUNITY_Community 26|Community 26]]
- [[_COMMUNITY_Community 27|Community 27]]
- [[_COMMUNITY_Community 28|Community 28]]
- [[_COMMUNITY_Community 29|Community 29]]
- [[_COMMUNITY_Community 30|Community 30]]
- [[_COMMUNITY_Community 31|Community 31]]
- [[_COMMUNITY_Community 32|Community 32]]
- [[_COMMUNITY_Community 33|Community 33]]
- [[_COMMUNITY_Community 34|Community 34]]
- [[_COMMUNITY_Community 35|Community 35]]
- [[_COMMUNITY_Community 36|Community 36]]
- [[_COMMUNITY_Community 37|Community 37]]
- [[_COMMUNITY_Community 38|Community 38]]

## God Nodes (most connected - your core abstractions)
1. `RIMA Memory Index` - 25 edges
2. `Creator Tool URL Mapping Table` - 14 edges
3. `PixelLab Prompt Grammar Reference` - 10 edges
4. `PixelLab Tool Guide (canonical)` - 9 edges
5. `EncounterTemplateSO (Karar #149)` - 9 edges
6. `Path C Hybrid Production Pipeline Lock` - 9 edges
7. `Sub-Room Canonical Tags Lock (5 strings)` - 9 edges
8. `Canonical Direction System (2026-05-03)` - 8 edges
9. `Act 1 Shattered Keep Envanter (Karar #150)` - 8 edges
10. `Behavioral Contract Testing System` - 8 edges

## Surprising Connections (you probably didn't know these)
- `Walkability + Dash MVP (S107)` --semantically_similar_to--> `DashTraverseGap candidate mask`  [INFERRED] [semantically similar]
  s107_obsidian_notes/Walkability_Dash.md → MEMORY/project_room_blueprints.md
- `animate_character MCP Forbidden` --semantically_similar_to--> `PixelLab MCP Halt Rule (HARD)`  [INFERRED] [semantically similar]
  MEMORY/feedback_animate_character.md → CURRENT_STATUS.md
- `Karpathy 4 Universal Coding Principles` --semantically_similar_to--> `forrestchang/karpathy-skills assessment`  [INFERRED] [semantically similar]
  PROJECT_RULES.md → MEMORY/agent_context_economy.md
- `cx wrapper 3 profil (laurethgame primary)` --semantically_similar_to--> `cx_dispatch.py routing`  [INFERRED] [semantically similar]
  MEMORY/feedback_codex_task_routing.md → PROJECT_RULES.md
- `Shared Memory Only (no local auto-memory)` --semantically_similar_to--> `Drift Hierarchy (NLM > local > prompt)`  [INFERRED] [semantically similar]
  MEMORY/feedback_memory_system.md → PROJECT_RULES.md

## Hyperedges (group relationships)
- **Walkability + Dash MVP Subsystem** — current_status_walkability_map, current_status_iobstacle, current_status_player_controller, current_status_void_blocker [EXTRACTED 1.00]
- **Codex Dispatch Chain (orchestrator -> cx -> rima-codex)** — project_rules_orchestrator_context, project_rules_cx_dispatch, feedback_codex_task_cx_wrapper, feedback_codex_task_rima_codex_agent, concept_codex_gpt55 [EXTRACTED 0.95]
- **NLM Canonical Context Flow** — concept_notebooklm, project_rules_nlm_hard_rule, project_rules_subagent_nlm_access, project_rules_nlm_sync_policy, project_rules_drift_hierarchy [EXTRACTED 0.90]
- **PixelLab Structured Prompt Consensus** — feedback_pixellab_prompt_structure_rule, pixellab_sprites_sjalsol_template, pixellab_pipeline_workflows_sjalsol_3x3, pixellab_prompt_rules_techniques [INFERRED 0.85]
- **Act 1 Encounter Production Stack** — project_karar_149_encounter_template_so, project_karar_150_act1_envanter, project_karar_149_karar_147 [EXTRACTED 0.90]
- **NLM Notebook Lifecycle (active + deprecated + workflow)** — project_nlm_notebook_id_active, project_nlm_notebook_id_deprecated, notebooklm_workflow_hard_rule, notebooklm_workflow_bootstrap [EXTRACTED 0.90]
- **Path C 4-Layer Asset Stack (Base/Object/Gameplay/Collision)** — path_c_codex_imagegen, path_c_pixellab_inventory_119, path_c_map_designer_brush_v1, path_c_layer_architecture [EXTRACTED 0.95]
- **Sub-Room Encounter Authoring Stack (tags + validator + EncounterTemplateSO)** — subroom_canonical_tags, subroom_mirror_validator, subroom_encounter_template_so, subroom_encounter_proposal [EXTRACTED 0.90]
- **S107 Overnight Deliverables (Cliff + Walkability + Reward Flow + Cleanup)** — cliff_system_s107, walkability_dash_mvp, reward_portal_flow, s107_cleanup_chain, s107_overnight_log [EXTRACTED 0.95]

## Communities

### Community 0 - "Memory + Cleanup + Project Index"
Cohesion: 0.05
Nodes (40): _archive_overnight_2026_05_26 archive, Auto-Memory Canonical MEMORY.md, RIMA Memory Index, [ACTION] Block, [CHARACTER] Block, [CONSTRAINTS] Block, PixelLab 3-Block Prompt Structure, PROMPT_TEMPLATE.md (+32 more)

### Community 1 - "Cliff System S107"
Cohesion: 0.09
Nodes (31): CliffAutoPlacer.cs (neighbor edge detection), DeterministicVariantTile (offset.y=1.5, PPU 64), KitB_Cliff sprite pack (9 PNG, 128x192), CliffPlacementRules_Hades.asset preset, Cliff System S107 FINAL (3-direction), RIMA Obsidian Notes README (S107), Obsidian vault = RIMA project root, Decision 3: Cliff Sprite v2 Web UI route (+23 more)

### Community 2 - "PixelLab Tools + Animation API"
Cohesion: 0.07
Nodes (30): animate_with_text tool, animate_with_text_pro tool, Character Creator area, create_8_directional_pro tool, create_from_style_pro tool, create_image_pixen tool, create_image_pro tool (Pro), create_m_xl_image tool (+22 more)

### Community 3 - "Act 1 Rooms + Blueprints"
Cohesion: 0.08
Nodes (24): Project Room Blueprints, Act 1 30-Room Blueprint Catalogue, CombatQuestion (per room), DashTraverseGap candidate mask, Camera Clamp to Playable Floor + Shell, Connected generation (semantic skeleton first), GateSockets (blueprint-defined, not cardinal), PlayableFloorMask + VisualShellMask (+16 more)

### Community 4 - "PixelLab Pro Tool Set"
Cohesion: 0.11
Nodes (22): PixelLab tools/consistent-style, PixelLab Create 8-Rotations Pro, PixelLab Create Tiles Pro tool, MCP create_topdown_tileset (Wang16), PixelLab Inpaint v3, Negative Prompt Library (RIMA defaults), DO NOT: Cell 1: NAME format (baked label risk), PixelLab options/guidance (description, negative, weight) (+14 more)

### Community 5 - "Animation + Encounter Rules"
Cohesion: 0.12
Nodes (18): animate_character MCP FORBIDDEN Rule, Claude Role for Character Anims, Codex APPROVE_WITH_REVISIONS, EncounterTemplateSO (Karar #149), RoomTransitionFX fade-to-black, Karar #143 (6-layer Painter), Karar #147 (Multi-Layer Painter), Karar #27 (Echo/Death Imprint) (+10 more)

### Community 6 - "Status Format + Skill Audits"
Cohesion: 0.12
Nodes (17): CURRENT_STATUS leanness rule, Agent Context Economy (compact always-loaded), forrestchang/karpathy-skills assessment, mksglu/context-mode assessment, Root Junk Archive Cleanup, Graphify Focused Scope (S107 notes), NLM+Graphify+Obsidian Rebuild, NLM Sync (S108 morning) (+9 more)

### Community 7 - "Path C Hybrid Pipeline"
Cohesion: 0.14
Nodes (17): Codex image_gen (L1 floor + wall base), Dead Cells Workflow Reference, Hades Quality Reference, Path C Hybrid Production Pipeline Lock, Karar #149 sub-room sequence, 4-Layer Architecture (Base/Object/Gameplay/Collision), Map Designer Brush V1, PixelLab 119 PNG Inventory (L2 overlays) (+9 more)

### Community 8 - "Agent Routing + Authority"
Cohesion: 0.12
Nodes (16): Authority Order (Claude > Codex > Antigravity > Gemini > Gemma), Core Routing Rules, Delegation Gate, Codex (GPT-5.5), Opus rima-design, Sonnet Orchestrator, Sonnet Skill Capability Matrix, Reasoning Agents (rima-design, doc, qc, asset) (+8 more)

### Community 9 - "Direction System (8-way)"
Cohesion: 0.15
Nodes (13): Canonical Direction System (2026-05-03), flipX Bug Fix (2026-05-03), 8-Way Idle States, Direction Pattern A (Ele/Ranger/Shadow), Direction Pattern B (Warblade), PlayerAnimator Snapping, 4-Diagonal Run System, STYLE_BIBLE.md (truth source) (+5 more)

### Community 10 - "PixelLab Halt + Warblade"
Cohesion: 0.22
Nodes (11): PixelLab, Warblade (class), PixelLab MCP Halt Rule (HARD), animate_character MCP Forbidden, PixelLab > Autosprite Production, ClassType enum (10 classes), Basic Attack Combo Identity Lock, BasicAttackProfile.cs strategy pattern (+3 more)

### Community 11 - "PixelLab Budget + Costs"
Cohesion: 0.22
Nodes (10): Animation to Animation Transfer, animate-with-text-v3 Cost (v0.4.92), PixelLab Gen Budget, May 18 Cycle 5000 Gen, 2026-05-02 Session Allocation, Elementalist (core 4), Ranger (core 4), Class Integration Order (Core 4 first) (+2 more)

### Community 12 - "Sub-Room Tags + EncounterTemplate"
Cohesion: 0.22
Nodes (10): Sub-Room Canonical Tags Lock (5 strings), Dead Cells Biome Model Reference, Sub-Room Encounter System Proposal (SUPERSEDED), EncounterTemplateSO, EncounterTemplateValidator Archway Mirror, Tag: collapse_corridor, Tag: crypt_cell, Tag: entry_chamber (+2 more)

### Community 13 - "Discord Research + Risk"
Cohesion: 0.22
Nodes (9): Playwright Risky (webdriver flag), PyAutoGUI + Real Chrome (Safe), #announcements, #api-and-sdk, #pixellab-art-gallery, #help-questions-support, #mcp-and-vibe-coding, #share-your-tips-and-tricks (+1 more)

### Community 14 - "NotebookLM Auth + Sources"
Cohesion: 0.25
Nodes (9): Session Start Auth Check Sequence, Bootstrap 80 Sources, NotebookLM Hard Rule (2026-05-07), NotebookLM MCP Tools, Deprecated Notebook ID ed3c8952, Update Loop (list/delete/add), RIMA Notebook ID 06a27df3 (active), nlm login Auth Recovery (+1 more)

### Community 15 - "Community 15"
Cohesion: 0.32
Nodes (8): DISCORD_INSIGHTS_S43.md, Discord Pipeline Model Stack, Discord Pipeline Analysis Complete (S43), YouTube PixelLab pipeline (in-progress), Installed Ollama Models Table, Gemma 4 MTP Drafter (3x speed), Ollama Task Routing, Do NOT Unload gemma4:26b When Idle

### Community 16 - "Community 16"
Cohesion: 0.29
Nodes (7): NotebookLM, nlm login Recovery, NLM Auth Recovery (nlm login), NotebookLM HARD RULE (Context Source), NLM Live Notebook 30ddffa5, NLM Sync Policy Table, Sub-Agent NLM Access Mandatory

### Community 17 - "Community 17"
Cohesion: 0.29
Nodes (7): 2.5D Detour S57-S58 (REVOKED), Chibi 64x64 sprite + 32x32 tile spec, Hammerwatch Reference, Hyper Light Drifter Reference, KayKit/Blender Pre-Render Pipeline REVOKED, Pure 2D Top-Down Pivot 2026-05-12 (S59), URP 2D Renderer + Pixel Perfect Camera + 2D Lights

### Community 18 - "Community 18"
Cohesion: 0.33
Nodes (6): CliffAutoPlacer.CollectCliffCells, Cliff System v3 (Hybrid A+B), DeterministicVariantTile, Kit B Auto-Placement System, ParallaxLayer.cs, PlayableArena.unity Scene

### Community 19 - "Community 19"
Cohesion: 0.53
Nodes (6): IObstacle.cs interface, PlayerController, S107 Overnight (Opus orchestrator), VoidBlocker Tilemap, Walkability + Dash MVP, WalkabilityMap.cs

### Community 20 - "Community 20"
Cohesion: 0.33
Nodes (6): Dispatch Phase to rima-codex, Opus Design Judgment Phase, rima-codex agent, rima-qc agent, Orchestra Discipline Rule, 5-Edit Threshold for Dispatch

### Community 21 - "Community 21"
Cohesion: 0.4
Nodes (6): general-purpose agent, rima-asset Sonnet 4.6 agent (STAGING only), rima-design Opus 4.7 agent, rima-doc Sonnet 4.6 agent, rima-qc Sonnet 4.6 agent, Claude Sub-Agent Table

### Community 22 - "Community 22"
Cohesion: 0.4
Nodes (5): animate-with-text v3 Only Rule, Canvas Auto-Expansion (256 -> 168), Known Bugs Table (May 2026), 10-Minute Polling Timeout Minimum, Transient Errors Retry Rule

### Community 23 - "Community 23"
Cohesion: 0.6
Nodes (5): AttackAimMode (SON YON / MOUSE), PlayerController.FaceCombatTarget(), Player Hades-Style Movement and Combat Aim, Movement Facing (SE/NE/NW/SW diagonals), SettingsMenuUI runtime overlay

### Community 24 - "Community 24"
Cohesion: 0.5
Nodes (4): Agentic Safety Rules, External Tools (cleanEdge, Aseprite), Ultimatefrisbie1 Agentic Workflow, Python PNG Stitching Pattern

### Community 25 - "Community 25"
Cohesion: 0.5
Nodes (4): Final boss as lock/warden hook, Project Story / Lore, Phase 1-5 narrative split, RIMA / Rift March meaning

### Community 26 - "Community 26"
Cohesion: 0.67
Nodes (3): 3-Kit BG Architecture Lock, S104-S106 HARD RULES, L3 Island Large Boss Spawn

### Community 27 - "Community 27"
Cohesion: 0.67
Nodes (3): Shared Memory Only (no local auto-memory), Drift Hierarchy (NLM > local > prompt), Iteration Cleanup One LIVE Version

### Community 28 - "Community 28"
Cohesion: 0.67
Nodes (3): CameraLockController.cs, Camera Lock HARD RULE (S103), CameraRig_HD2D.prefab

### Community 29 - "Community 29"
Cohesion: 0.67
Nodes (3): Unity MCP Stuck Recovery, CoplayDev uvx MCP Unity Server, run_tests Syntax (PlayMode/EditMode)

### Community 30 - "Community 30"
Cohesion: 1.0
Nodes (2): Tile Painter v4 (MinimalTilePainter), Unified Map Designer

### Community 31 - "Community 31"
Cohesion: 1.0
Nodes (2): ASCII-only Internal Docs, Double-encoding Mojibake Rationale

### Community 32 - "Community 32"
Cohesion: 1.0
Nodes (2): Attribution Prefixes (CODEX:, ANTIGRAVITY:), Commit Frequency Rule

### Community 33 - "Community 33"
Cohesion: 1.0
Nodes (2): snowli_on 2-Step Batch Workflow, snowli_on 2-Step Pose Batch

### Community 34 - "Community 34"
Cohesion: 1.0
Nodes (1): Kaninen (PixelLab MEGA staff)

### Community 35 - "Community 35"
Cohesion: 1.0
Nodes (1): NikolaiPatricioStar

### Community 36 - "Community 36"
Cohesion: 1.0
Nodes (1): xjon

### Community 37 - "Community 37"
Cohesion: 1.0
Nodes (1): YumYum

### Community 38 - "Community 38"
Cohesion: 1.0
Nodes (1): Claude Code statusline configuration

## Knowledge Gaps
- **192 isolated node(s):** `Root Junk Archive Cleanup`, `STAGING 708-entry Cleanup`, `DeterministicVariantTile`, `NLM Sync (S108 morning)`, `Obsidian Vault = RIMA Root` (+187 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **Thin community `Community 30`** (2 nodes): `Tile Painter v4 (MinimalTilePainter)`, `Unified Map Designer`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 31`** (2 nodes): `ASCII-only Internal Docs`, `Double-encoding Mojibake Rationale`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 32`** (2 nodes): `Attribution Prefixes (CODEX:, ANTIGRAVITY:)`, `Commit Frequency Rule`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 33`** (2 nodes): `snowli_on 2-Step Batch Workflow`, `snowli_on 2-Step Pose Batch`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 34`** (1 nodes): `Kaninen (PixelLab MEGA staff)`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 35`** (1 nodes): `NikolaiPatricioStar`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 36`** (1 nodes): `xjon`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 37`** (1 nodes): `YumYum`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 38`** (1 nodes): `Claude Code statusline configuration`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `RIMA Memory Index` connect `Memory + Cleanup + Project Index` to `PixelLab Tools + Animation API`, `Animation + Encounter Rules`, `Direction System (8-way)`, `PixelLab Budget + Costs`, `NotebookLM Auth + Sources`, `Community 20`?**
  _High betweenness centrality (0.104) - this node is a cross-community bridge._
- **Why does `Walkability + Dash MVP (S107)` connect `Cliff System S107` to `Act 1 Rooms + Blueprints`, `Path C Hybrid Pipeline`?**
  _High betweenness centrality (0.051) - this node is a cross-community bridge._
- **Why does `Path C Hybrid Production Pipeline Lock` connect `Path C Hybrid Pipeline` to `Sub-Room Tags + EncounterTemplate`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **What connects `Root Junk Archive Cleanup`, `STAGING 708-entry Cleanup`, `DeterministicVariantTile` to the rest of the system?**
  _192 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Memory + Cleanup + Project Index` be split into smaller, more focused modules?**
  _Cohesion score 0.05 - nodes in this community are weakly interconnected._
- **Should `Cliff System S107` be split into smaller, more focused modules?**
  _Cohesion score 0.09 - nodes in this community are weakly interconnected._
- **Should `PixelLab Tools + Animation API` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._