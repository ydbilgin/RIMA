# CURRENT STATUS
**2026-05-08 - S45 - Phase 1 (Tile Pipeline + DungeonWorldBuilder)**

## Active Block

### Tile Pipeline (2026-05-08)
- **F2 sliced** → `Assets/Art/Tiles/Act1/F2/` (16 tile, 64×64). F2 tile 13-16 → prop/overlay kullan, Random Tile pool'a koyma.
- **F3 sliced** → `Assets/Art/Tiles/Act1/F3/` (12 tile, 64×64). Zone isolation zorunlu — F1/F2 ile aynı pool'a girmesin.
- **W1 ✅ DONE** (ChatGPT, commit 2026-05-08) → `STAGING/tiles_raw/style_anchor_W1_wall_PRIMARY.png`. PRIMARY style anchor for all ChatGPT tiles.
- **W2 ✅ DONE** (ChatGPT, commit 2026-05-08) → slice via batch_tiles.ps1 → `Assets/Art/Tiles/Act1/W2/`
- **OBW ✅ DONE** (ChatGPT, commit 2026-05-08) → slice via batch_tiles.ps1 → `Assets/Art/Tiles/Act1/OBW/`
- **F3 ❌ REGEN PENDING** → Wrong size from ChatGPT (1254×1254, should 1024×1536). Regeneration: `STAGING/CHATGPT_REMAINING_TILES.md` PROMPT 1
- **Trans F1→F2 ❌ REGEN PENDING** → Wrong size (1774×887). Regeneration: `STAGING/CHATGPT_REMAINING_TILES.md` PROMPT 2
- **Trans F2→F3 ❌ REGEN PENDING** → Wrong size (1774×887). Regeneration: `STAGING/CHATGPT_REMAINING_TILES.md` PROMPT 3

### Tile Grid Math Kuralı (LOCKED)
- Floor 64×64: 1024×1024, 4×N grid — N sadece 1/2/4/8 (1024÷N integer olmalı, 3 YASAK)
- Wall 64×96: 1024×1536, 4×4 grid → 256×384 hücre
- Tall wall 64×128: 1024×1536, 4×3 grid → 256×512 hücre
- Codex $imagegen: tile için KULLANMA — smooth 3D render üretiyor. ChatGPT > Codex for pixel art.
- $imagegen syntax: `$imagegen "prompt"` (Codex task içinde). Pixel art için "pixel clusters min 4px, no gradients" ekle.

### WallOcclusionFader (Hades stili saydamlaşma)
- `Assets/Scripts/Core/WallOcclusionFader.cs` → KOD HAZIR, değişiklik yok.
- Unity'de yapılacak: Wall Tilemap → Add Component → WallOcclusionFader. fadeRadius 2.2, minAlpha 0.38, fadeSpeed 10.

### Sıradaki Tile Üretimleri
1. **W1 ✅** (DONE, `style_anchor_W1_wall_PRIMARY.png`)
2. **W2 ✅** (DONE, awaiting batch_tiles.ps1)
3. **OBW ✅** (DONE, awaiting batch_tiles.ps1)
4. **F3 + Trans tiles ❌** (regen via CHATGPT_REMAINING_TILES.md — size constraints)

### Act Tile Progression (LOCKED — memory'de tam plan var)
- 4 act × derinlik bandı tile seti planı → `MEMORY/project_act_tile_progression.md`
- Act 1: F1(temiz)→F2(çatlak)→F3(yosun)→F4-rift(yapılmadı), zona göre ayrı Random Tile pool

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
- Dash-Cancel on Attack Recovery: per-class cancel windows (Ravager/Shadow 15-25%, Ranger/Gunslinger 35-50%, Warblade/Brawler 60-75%, Casters windup not cancellable). Hook: BasicAttackProfile.cancelWindowFraction + PlayerController.HandleDash
- OnDash Passive Proc: 4th passive type added to taxonomy. Shadowblade/Ronin primary. CrossClassEffectType.OnDash enum + CrossClassSkillManager.OnDash() method. Complexity S.
- Boss Posture/Stagger: universal meter, break window 3s +50% dmg. Pairs with Fracture Echoes. StatusEffectSystem coordination required before implementation. Complexity L.

### DungeonWorldBuilder (Architecture LOCKED — Codex in progress, laurethgame)
- Phase 1: `LayoutKind` public + `PaintTemplateAtOffset` on `LargeDungeonMapPainterBase`
- Phase 2: New SOs + builder — `DungeonWorldBuilder.cs`, `RoomTemplate.cs`, `DepthBandTileSet.cs`
- Phase 3: `RuntimeRoomManager.StartRoom` rewired → `worldBuilder.GetRoomBounds`
- Grid: lane×roomStride.x, depth×roomStride.y; corridorWidth=8; depth bands 0-2→F1, 3-5→F2, 6+→F3
- All 13 DungeonGraph nodes painted once at build time; `LargeDungeonMapPainterBase` = single-room renderer wrapped by builder
- New files: `Assets/Scripts/Systems/Map/DungeonWorldBuilder.cs`, `RoomTemplate.cs`, `DepthBandTileSet.cs`

### HUD Pixel Art Assets (ChatGPT — planned, after tile batch)
- Skill slot frames: LMB/RMB 72×72, Q-4/5 56×56, stone-carved, cyan rift glyph inlay
- HP bar frame: 220×32px gothic stone arch; Resource bar: same style, class-agnostic crystal icon
- Minimap border: 128×128px stone/parchment; Room name banner: 320×36px stone tablet
- Palette: #1A1A2E/#2D2D4E/#00FFCC/#C8A96E

### Code (DONE this session)

#### batch_tiles.ps1 (commit 9e647c7)
- `STAGING/batch_tiles.ps1` — batch process W1/W2/OBW/F3/Trans tiles via single command
- Slices generated sheets (1024×1536 or 1024×1024) into per-tile 64×64 or 64×96 via `process_tiles.py`
- Output: `Assets/Art/Tiles/Act1/{W1,W2,OBW,F3,Trans_*}/`

#### F1TileSetup editor tool (commit ac426bd)
- `Assets/Editor/DevTools/F1TileSetup.cs` — RIMA/Setup F1 Tiles menu item
- Loads 16×64px F1 floor tiles from `Assets/Art/Tiles/Act1/F1/` → `DungeonLayerManager.f1FloorTiles` TileBase[]

#### DungeonWorldBuilder — Phase 1-3 Complete (commits 670fce3, e8f13dd, 1ab62a3)
- **Phase 1** (commit 670fce3): `LargeDungeonMapPainterBase.LayoutKind` public, `PaintTemplateAtOffset(LayoutKind, Vector3Int)` added
- **Phase 2** (commit e8f13dd): New SOs — `RoomTemplate.cs`, `DepthBandTileSet.cs`; `DungeonWorldBuilder.cs` (main builder)
- **Phase 3** (commit 1ab62a3): `RuntimeRoomManager.StartRoom()` → `worldBuilder.GetRoomBounds()` wired (null-guarded)
- Grid: lane×roomStride.x, depth×roomStride.y; corridorWidth=8; depth bands 0-2→F1, 3-5→F2, 6+→F3
- **GAP: DepthBandTileSet painter hookup pending** — depth-band tile swap not yet wired to painter

#### tiles_raw cleanup (commit a86d1c3)
- Style anchor files renamed for clarity: `style_anchor_W1_wall_PRIMARY.png`, `style_anchor_W2_wall.png`, etc.
- Old ARCHIVE/ files removed from staging area

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

#### HUD sprite cleanup (Codex -- laurethgame, dispatched)
- Remove last sprite asset ref: `HUDController.cs ~385` `bgImg.sprite = RimaUITheme.PromptFrame`
- Makes HUD fully procedural (no PNG dependencies)

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

#### Full UI Architecture Rebuild (Antigravity session)
**Phase 1 -- Opus 4.6:**
- UIManager.cs: Singleton, mutual exclusion for TAB/ESC/SkillOffer overlays, single source of truth for Time.timeScale
- RimaUITheme.cs: expanded -- procedural 9-slice frames at runtime, all palette constants (no baked PNG panels)
- HUDController.cs: rewritten -- procedural non-scaling bars, pulse effects, transient room name banners
- SkillBarUI.cs: rewritten -- 7-slot hexagonal layout (LMB/RMB/1-5), legacy drag-drop removed
- CharacterSheetUI.cs: rebuilt -- TAB overlay, dark-glass procedural panel via UIManager
- SettingsMenuUI.cs: rebuilt -- ESC overlay, procedural panel via UIManager
- MiniMap.cs: rebuilt -- flat-grid node map using DungeonGraph
- SkillOfferUI.cs: rebuilt -- Hades-style 3-card draft, slide-in animations, tier color coding

**Phase 2 -- Gemini 3.1 Pro:**
- MainMenuScreen.cs: rewritten -- 100% procedural, RimaUITheme constants, no legacy dungeon background images
- CharacterSelectScreen.cs: rewritten -- procedural, proper scene transition cleanup
- MovementDiagnostic.cs: repaired -- old reflection queries removed, re-routed to UIManager.Instance (IsTabOpen/IsSettingsOpen/IsSkillOfferOpen)

**Result:** All old UI prefabs/monolithic update loops deprecated. UI is code-driven, procedural, mutual-exclusion safe, Ashen Glyph spec compliant.

#### Performance Deep-Fix Pass (Antigravity 2026-05-07)
- 11 per-frame Find/Alloc calls eliminated: one-shot cache + interval scan + reusable buffers
- CPU frame time: 99ms -> 0.11ms (~900x). 8 files changed. PerformanceAntiPatternTests added.

### Doc (DONE)
- Skill pool alternatives (10 classes): commit 048a14c -- TASARIM/SKILL_POOL_ALTERNATIVES_2026-05-06.md
- Dungeon Lighting + Generation Research (commit f457edb): `STAGING/DUNGEON_LIGHTING_GENERATION_RESEARCH.md` — physical lighting + dungeon gen synthesis

## Working Rules
- Record concrete results and unresolved complaints here.
- Keep details in linked files; this file stays compact.
- Earlier session history (2026-05-05): see git log (commits ad8d2c1, c59fbb9, d9f08bd).

## LOCKED
- Yükseklik sistemi: Hades approach — kamera açısı sabit, yükseklik farkı IsometricZAsY Z-offset + görsel gölge/kenar ile anlatılır. Kamera tilt yok.
- Tile üretim yaklaşımı: ChatGPT (GPT-4o) > PixelLab isometrik floor için. Prompt şablonu: STAGING/CHATGPT_PROMPT_FLOOR_TILES.md. Unity side face çözümü: pivot top-center + Y-sort.
- 3-katman dungeon render sistemi: Structural (Rule Tile) + Detail (Random Tile scatter) + Entity (Y-sorted props). AO shadow sprite duvar-zemin birleşiminde.


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
### Immediate next session:
1. **ChatGPT: F3 + Trans F1→F2 + Trans F2→F3 regen** — `STAGING/CHATGPT_REMAINING_TILES.md` (size constraints)
2. **batch_tiles.ps1 run** — process W1/W2/OBW sheets → `Assets/Art/Tiles/Act1/{W1,W2,OBW}/`
3. **DungeonWorldBuilder DepthBandTileSet hookup** — wire depth-band tile swap to painter
4. **DUNGEON_LIGHTING_GENERATION_RESEARCH.md review** → design session → PropSpec implementation
5. **WallOcclusionFader attach** → Wall Tilemap + Add Component (Unity side)
6. **Dash-Cancel** (Sonnet, Antigravity) — BasicAttackProfile.cancelWindowFraction + PlayerController event
7. **OnDash Proc** (Sonnet, Antigravity) — CrossClassEffectType.OnDash + HandleDash call site
8. Boss Posture system -- after StatusEffectSystem unstaged changes resolved

### Backlog:
- BasicAttack .asset'lerini Inspector'da PlayerAttack'e assign et
- SkillDraftSystem -> SkillOfferUI: TriggerDraft oda gecisinde bagla
- Identity Passive system kodu (BasicAttackProfile OnCommitBeat -> class pasif tetik)
- TAB Overlay wireframe (Codex) -- 3-layer UI
- Undercroft tile seti -- PixelLab (prompts: STAGING/PIXELLAB_TILESET_UNDERCROFT_CONNECTED_2026-05-07.md)

## Latest Verification
- EditMode: 144/144 PASS (10 new contract tests added, all pass).
- PlayMode: 4/5 PASS -- TimeScale=0 boot bug caught and fixed (commit b343d4c).
- Script validation: HUDController, MiniMap, RuntimeRoomManager, SettingsMenuUI, MainMenuScreen, RoomPreviewPanel all PASS.
- Performance: CPU frame time 99ms -> 0.11ms after deep-fix pass.
- BasicAttack strategy pattern: all 6 behaviors implemented and compile-clean.

## Current Risks
- BasicAttack .asset'leri Inspector'da PlayerAttack'e henüz assign edilmedi.
- SkillDraftSystem -> SkillOfferUI hook baglandi, TriggerDraft hala oda gecisinde cagirilmiyor.
- UI rebuild needs QC + PlayMode visual verification (no PlayMode screenshot test yet).
- Compile check on new UI files not yet confirmed.
- Movement sheet prompts now written, generation pending.
- Graphify chunk 3 missing (not critical, add with --update).
- God objects (LargeDungeonMapPainterBase, RuntimeRoomManager) -- technical debt, Phase 1 acceptable.
- PixelLab 127px bug (128px outputs 127px) -- QC during floor test.
- Imagen tile ciktilari kalite yetersiz -- undercroft tile seti PixelLab'da yeniden uretilecek.
- ChestUI.cs:43,50 + ForgeUI.cs:72,93,100 — direct timeScale writes, pre-existing, need UIManager routing (follow-up)
- **DungeonWorldBuilder DepthBandTileSet hookup PENDING** — depth-band tile swap not yet wired to painter; currently uses Inspector tiles
- **F3 + 2 transition tiles REGEN PENDING** — ChatGPT gave wrong dimensions; regeneration via CHATGPT_REMAINING_TILES.md

## Key Pointers
- UIManager.cs: `Assets/Scripts/UI/UIManager.cs` -- singleton, owns all timeScale + overlay state
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
- ChatGPT floor tile prompt (LOCKED): `STAGING/CHATGPT_PROMPT_FLOOR_TILES.md`
- ChatGPT batch prompts (wall+floor): `STAGING/CHATGPT_BATCH_PROMPTS.md`
- **ChatGPT remaining tiles (F3 + transitions)**: `STAGING/CHATGPT_REMAINING_TILES.md` — 3 prompts, size constraints
- **batch_tiles.ps1**: `STAGING/batch_tiles.ps1` — batch slice W1/W2/OBW/F3 sheets
- **Dungeon lighting research**: `STAGING/DUNGEON_LIGHTING_GENERATION_RESEARCH.md` — physical lighting + dungeon gen synthesis
- **W1 style anchor**: `STAGING/tiles_raw/style_anchor_W1_wall_PRIMARY.png`
- DungeonWorldBuilder (Phase 1-3 DONE, hookup PENDING): `Assets/Scripts/Systems/Map/DungeonWorldBuilder.cs`
- DungeonLayerManager.cs (3-katman sistem): `Assets/Scripts/Systems/Map/DungeonLayerManager.cs`
- F1TileSetup editor tool (DONE): `Assets/Editor/DevTools/F1TileSetup.cs`
- F1 floor tile PixelLab prompt (WORKING): `STAGING/PIXELLAB_PROMPT_F1_FLOOR_TILES.md`
- F1 tile sheet source: `C:\Users\ydbil\Downloads\pixellab-Seamless-isometric-pixel-art-d-1778183060391.png` → target: `Assets/Art/Tiles/Act1/f1variants.png`
- Warblade animation guide (step-by-step): `STAGING/PIXELLAB_PRODUCTION_GUIDE_WARBLADE_ANIMATIONS.md`
- Dungeon asset guide (tile/wall/objects, step-by-step): `STAGING/PIXELLAB_PRODUCTION_GUIDE_DUNGEON_ASSETS.md`
- PixelLab prompt template ([CHARACTER]/[ACTION]/[CONSTRAINTS]): `STAGING/PIXELLAB_PROMPT_TEMPLATE.md`
- Combat fluidity decisions: dash-cancel + OnDash + posture (LOCKED this session, see CURRENT_STATUS)
