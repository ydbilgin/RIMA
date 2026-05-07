# CURRENT STATUS
**2026-05-07 - S43 - Phase 1**

## Active Block
- UI performance fix + visual cleanup DONE (Antigravity 2026-05-07). Play Mode screenshot QA still OPEN.
- UI full rebuild: given to Antigravity -- Ashen Glyph spec, 3-layer architecture, placeholder art. All existing UI screens rebuilt from scratch.

### Skill Files (RAW — old Q/E/R format, will be revised)
- 10-class wrongly-generated roster (Ironclad/Arcanist/Riftstalker/Vanguard/Specter): ON HOLD
- SKILL_TREE_10CLASSES_CANONICAL.md -- wrong roster, reference only
- SKILL_POOL_ALTERNATIVES_2026-05-06.md -- wrong roster, reference only
- SKILL_TREE_5CLASS_MISSING_2026-05-06.md -- commit 1bbed80, raw material
- SKILL_POOL_ALTERNATIVES_5CLASS_MISSING_2026-05-06.md -- commit 1bbed80, raw material
- PixelLab animation prompts (correct S41 roster): STAGING/PIXELLAB_ANIMATION_PROMPTS_10CLASS_2026-05-06.md

## cx exec Syntax (CONFIRMED 2026-05-06)
Correct: `$prompt | cx <account> exec -s danger-full-access -m gpt-5.5`
Wrong:   `cx <account> exec ... $prompt` (hangs -- stdin stays open in background PS, codex waits for EOF)
Detail: MEMORY (feedback_codex_dispatch_strategy.md)

## NotebookLM (NEW - 2026-05-06)
- Notebook: RIMA Game Design Knowledge Base (ID: ed3c8952-417c-4988-84a7-425d25ba3b08)
- 89 sources total (80 bootstrap + 9 updates: RULES/AGENTS/CLAUDE/CODEX + 5 MEMORY updates)
- Sync tag: nlm-sync-20260506
- HARD RULE: Claude never reads files except CURRENT_STATUS.md -- all context via NotebookLM query
- Detail: MEMORY/notebooklm_workflow.md

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
- Skill System Taxonomy: `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md` -- 4 aktif tip, 3 pasif tip, upgrade sistemi, Identity Passive, Cross-Family Carrier
- Skill System Taxonomy (4 tip / 3 pasif / upgrade / Identity Passive): `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md`
- Skill Pools 10-class: `TASARIM/SKILL_POOLS_10CLASS_2026-05-07.md`
- CLASS_IDENTITY_CONSTRAINTS (OWNS/AVOIDS per class): taxonomy §8

### Code (DONE this session)

#### Contract Test Suite (Codex -- task addf8a5cda39113d9)
- 10 new contract test files: TimeScaleContract, BootstrapContract, CombatContract, UIFlowContract + EditMode/PlayMode test classes
- EditMode: 10/10 PASS
- PlayMode: 4/5 PASS -- 1 genuine bug caught (TimeScale=0 on boot)
- Files: Assets/Tests/Contracts/ + Assets/Tests/EditMode/Contracts/ + Assets/Tests/PlayMode/

#### TimeScale Boot Fix (Codex -- commit b343d4c)
- Root cause: MainMenuScreen.AutoInit() fired in _IsoGame via [RuntimeInitializeOnLoadMethod]
- Fix: scene guard added -- if (SceneManager.GetActiveScene().name == "_IsoGame") return;
- Duplicate EventSystem warning also eliminated
- File: Assets/Scripts/UI/MainMenuScreen.cs

#### HeatGaugeBehavior + MarkPulseBehavior (Antigravity -- commit f8abe30)
- HeatGaugeBehavior.cs: Gunslinger ranged attack, Heat resource, dual pistol cadence
- MarkPulseBehavior.cs: Ravager melee, Fury buildup, Blood Pact RMB
- BasicAttackProfile.cs: factory updated, no more MeleeChain fallback for these two
- BasicAttack strategy pattern NOW COMPLETE (all 6 behaviors implemented)

#### AreaSkillPlacer (Antigravity -- commit 41818de)
- 262 lines, AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md contract fulfilled
- RMB hold -> indicator -> release -> cast, ESC/LMB cancels, max 6 tile range, red clamp
- File: Assets/Scripts/Combat/Skills/AreaSkillPlacer.cs

#### GameViewSetup (Codex -- commit 3869efb)
- Maximize on Play enabled via EditorPrefs on every project open
- MenuItem: RIMA/Setup Game View (1080p + Maximize)
- File: Assets/Editor/DevTools/GameViewSetup.cs

- BasicAttackProfile infrastructure: commit 280a637 (laurethayday) -- 4 files created
- BuildFloorMask rect-first refactor: commit d9f08bd (laurethgame) -- all 16 layouts rewritten, architectural masonry aesthetic
- PlayerAttack + BasicAttackProfile + SkillSlot QC + FIX: DONE (Antigravity 2026-05-07)
  - 2 blocker duzeltildi: classType int->enum, God-Object strategy pattern'e cevrildi
  - 7 warn duzeltildi: OnCommitBeat silindi, duplicate SkillData->ActiveSkillData, silent fallback->LogError, ClassType enum 10 sinifa tamamlandi, SkillSlotIndex Q/E/R->Slot1-4
  - 6 yeni dosya: IBasicAttackBehavior, BasicAttackBehaviorBase, MeleeChainBehavior, CastRhythmBehavior, ShotCadenceBehavior, VeilStrikeBehavior
  - BasicAttackProfile: 398 satirdan 94 satir saf data SO'ya indi
  - commit'e hazir (laurethgame)
- Unity compile check: CLEAN (Antigravity 2026-05-07) -- 0 error, sadece pre-existing TMP/FindObjectOfType warning'leri
- BasicAttackProfile .asset dosyalari: DONE -- Assets/Resources/Combat/BasicAttack/
  - BasicAttackProfile_Warblade.asset (Melee)
  - BasicAttackProfile_Elementalist.asset (CastRhythm)
  - BasicAttackProfile_Ranger.asset (ShotCadence)
  - BasicAttackProfile_Shadowblade.asset (VeilStrike)
- SkillDraftSystem.cs iskelet: DONE -- Assets/Scripts/Combat/Skills/SkillDraftSystem.cs
  - Hades-style 3-choice draft, taxonomy soft-guidance weight table, TriggerDraft(roomNumber) + SelectSkill(data) API

#### Performance Deep-Fix Pass (Antigravity 2026-05-07)
- 11 per-frame Find/Alloc calls eliminated: one-shot cache + interval scan + reusable buffers
- CPU frame time: 99ms -> 0.11ms (~900x). 8 files changed. PerformanceAntiPatternTests added.

### Doc (DONE)
- Skill pool alternatives (10 classes): commit 048a14c -- TASARIM/SKILL_POOL_ALTERNATIVES_2026-05-06.md

## Working Rules
- Record concrete results and unresolved complaints here.
- Keep details in linked files; this file stays compact.
- Earlier session history (2026-05-05): see git log (commits ad8d2c1, c59fbb9, d9f08bd).

## LOCKED
- Map editor approach: Unity Editor Game View + Maximize on Play. NO standalone build for editing. NO separate EditorWindow tool. Runtime overlay (F9) remains for gameplay tools only. Detail: TASARIM/DEV_TOOL_PLAN.md
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

## Next Priorities
1. BasicAttack .asset'lerini Inspector'da PlayerAttack'e assign et
2. SkillDraftSystem -> SkillOfferUI: TriggerDraft oda gecisinde bagla
3. Identity Passive system kodu (BasicAttackProfile OnCommitBeat -> class pasif tetik)
4. TAB Overlay wireframe (Codex) -- 3-layer UI
5. Undercroft tile seti -- PixelLab (prompts: STAGING/PIXELLAB_TILESET_UNDERCROFT_CONNECTED_2026-05-07.md)
6. Movement sheet generation (prompts ready: STAGING/WARBLADE_ANIMATION_PROMPTS_2026-05-07.md)

## Latest Verification
- EditMode: 144/144 PASS (10 new contract tests added, all pass).
- PlayMode: 4/5 PASS -- TimeScale=0 boot bug caught and fixed (commit b343d4c).
- Script validation: HUDController, MiniMap, RuntimeRoomManager, SettingsMenuUI, MainMenuScreen, RoomPreviewPanel all PASS.
- Performance: CPU frame time 99ms -> 0.11ms after deep-fix pass.
- BasicAttack strategy pattern: all 6 behaviors implemented and compile-clean.

## Current Risks
- BasicAttack .asset'leri Inspector'da PlayerAttack'e henüz assign edilmedi.
- SkillDraftSystem -> SkillOfferUI hook baglandi, TriggerDraft hala oda gecisinde cagirilmiyor.
- UI rebuild in progress (Antigravity).
- Movement sheet prompts now written, generation pending.
- Graphify chunk 3 missing (not critical, add with --update).
- God objects (LargeDungeonMapPainterBase, RuntimeRoomManager) -- technical debt, Phase 1 acceptable.
- PixelLab 127px bug (128px outputs 127px) -- QC during floor test.
- Imagen tile ciktilari kalite yetersiz -- undercroft tile seti PixelLab'da yeniden uretilecek.

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
- Skill taxonomy (LOCKED): `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md`
- Skill pools 10-class (LOCKED): `TASARIM/SKILL_POOLS_10CLASS_2026-05-07.md`
- Undercroft connected tile prompts: `STAGING/PIXELLAB_TILESET_UNDERCROFT_CONNECTED_2026-05-07.md`
- Warblade animation prompts: `STAGING/WARBLADE_ANIMATION_PROMPTS_2026-05-07.md`
- Dungeon asset prompts (tile/wall/objects): `STAGING/PIXELLAB_DUNGEON_ASSETS_PROMPTS_2026-05-07.md`
