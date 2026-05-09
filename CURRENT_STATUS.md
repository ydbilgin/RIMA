# CURRENT STATUS
**2026-05-09 - S46 - Regresyon fix'leri + dungeon mimari karar + map fragment spec — verification pending**

## S46 Late Session (2026-05-09 — autonomous, user uyurken)
- **Dungeon mimari KARAR doğrulandı (KEEP Hades-style ayrık oda)**: combat v1 + AD v2 eklemeleri açık dünya yönüne çekmiyor, tam tersi Hades modelini zorunlu kılıyor. Detay: `TASARIM/dungeon_act1_map.md`.
- **Act 1 map paketi LOCKED v1**: 15 node (13 ana hat + 2 dal) — Entry / 6 Combat / 2 Elite / 2 Rest (transition) / 1 Shop / 1 Curse Gate dal / 1 Mystery dal / 1 Boss. Procedural per-run (topoloji sabit, içerik random). Detay: `TASARIM/dungeon_act1_map.md`.
- **Map fragment system spec LOCKED v1**: StS2-style node reveal (current+1 default, +1 daha pickup ile = 2 ileri tip görünür), boss kapısı 8 fragment, hibrit MapPanel(TAB)+MiniMap(sol-üst). Detay: `TASARIM/map_fragment_system.md`.
- **Q1-Q5 açık soruları kapatıldı (Opus karar)**: Rest fragment YOK / Mystery AUTHORED events / Spirit v1'de YOK / Map UI HİBRİT / Boss kapısı 8 fragment.
- **Kod fix'leri uygulandı (5 edit)**:
  - `UIManager.cs` AutoInit reset → B4.2 timeScale=0 PlayMode test order pollution fix
  - `MainMenuScreen.cs` `OnNewRun` → `OnPlayClicked` rename (button delegate dahil) → B4.1 OnPlayClicked_Exists test
  - `CharacterSelectTests.cs` 3 yer reflection invoke kaldırıldı (Awake auto-runs) → B4.1 CharSelect NullRef
  - `RewardPickup.cs` Update lazy-find → Start+Invoke pattern → B4.1 PerformanceAntiPattern direct
- **Verification BLOCKED — manuel test gerekli**: Unity MCP 3 dk timeout boyunca yanıt vermedi (5 dosya değişikliği sonrası muhtemelen domain reload sürüyor veya MCP bridge disconnect). Sabah Unity Editor açıldığında:
  1. Compile bittiğini doğrula (sağ alt spinner yok)
  2. `Assets → Reimport All` (3-5 dk) — B5.1 visual fix
  3. `Window → General → Test Runner` → EditMode → Run All — B4.1/B4.2 verify
  4. Hala fail varsa: en muhtemel kalıntı **MainMenuScreen_CreatesCanvasWithGraphicRaycaster** ve **_CanvasSortOrderIsHigh** (rename sonrası geçer mi belirsiz, RimaUITheme.ResourceFrame null hipotezi var). PerformanceAntiPattern direct artık geçmeli; indirect path için RewardPickup.LateAcquirePlayer Update'ten doğrudan çağrılmıyor (Invoke ile çağrılıyor) → indirect tester yakalamamalı.

## Overnight Run (2026-05-09 ~02:50–04:10) — READ FIRST
- **Full report:** `STAGING/OVERNIGHT_REPORT.md` (per-task PASS/FAIL/BLOCKED + investigation list + commit plan)
- **17/21 PASS/PARTIAL** (B1 cleanup ×8, B2 pipeline ×1, B3 wiring ×5 — except Inspector wiring of DungeonWorldBuilder which is deferred), **3 REGRESSION**, **1 BLOCKED**
- **Sub-agent pattern verified:** 6 Codex (laurethgame+laurethayday) + 8 rima-qc + 1 Gemini audit. Cross-review caught real issues (dead "Rest" branch, redundant SetTileFlags, wrong file path).
- **NO COMMIT MADE** (visual regression risk — ~141 dosya working tree'de, selectively stage)

### Critical regressions to debug (priority order)
1. **B5.1 Visual:** Play mode tiles render as Unity "missing sprite" placeholders despite Editor showing 108/108 valid sprite refs. Hypotheses (in order): TilemapRenderer Individual mode mismatch, "Entities" sorting layer absence, YSortBehaviour vs IsoSorter LateUpdate race, asset DB desync.
2. **B4.2 timeScale=0 boot:** Was fixed in commit b343d4c, re-broken tonight. Suspect: B1.4 ChestUI/ForgeUI PauseForMenu sequence or scene state.
3. **B4.1 EditMode 142/148:** 6 fails — CharacterSelect×3 NullRef (likely pre-existing), MainMenu_OnPlayClicked test (test/code drift), PerformanceAntiPattern×2 (re-run with verbose to capture report).

### Tonight's clean wins (safe to keep)
- B1.1 CS0618 migration (4 sites) — Codex+QC PASS
- B1.3 HUD PromptFrame removal — Codex+QC PASS
- B1.4 ChestUI/ForgeUI timeScale routing — Codex+QC PASS *(but suspect for B4.2)*
- B1.5 Obstacle TilemapRenderer pattern added to Setup script
- B1.6 DepthBandTileSet audit — Gemini found shared-state bug (TODO comment placed)
- B2.1 process_tiles.py IEND root fix — Codex+QC PASS (BytesIO + explicit format="PNG" + post-write _verify_iend)
- B3.5 DepthBand SOs — 3 SOs created+populated at `Assets/Resources/Map/DepthBands/`, editor menu `RIMA/Create DepthBand SOs` shipped
- B3.4 DraftManager → RoomLoader.OnRoomCleared subscription (PARTIAL: dead "Rest" branch, harmless)
- B3.1 YSortBehaviour added to Player *(but suspect for B5.1)*

### Inventory: not committed yet
~141 files in working tree. **Do not commit `Assets/Scenes/_IsoGame.unity` or `YSortBehaviour.cs` Player attach until B5.1 root cause identified** — they may be the regression source.

### rima-research model (correction from earlier tonight)
- Gemini 3.1 Pro Preview IS available (`~/.gemini/settings.json` already pins it as default).
- Earlier "3.1 unavailable, fallback 2.5 Pro" was WRONG — actual cause was HTTP 429 (rate limit), not unavailability.
- `.claude/agents/rima-research.md` updated: bare `gemini -p` (no `-m`); on 429 → retry-after-30s, not panic-fallback. Detail: `MEMORY/feedback_rima_research_model.md`.

---

## Active Block

### Tile Pipeline (2026-05-08)
- **F2 sliced** → `Assets/Art/Tiles/Act1/F2/` (16 tile, 64×64). F2 tile 13-16 → prop/overlay kullan, Random Tile pool'a koyma.
- **F3 sliced** → `Assets/Art/Tiles/Act1/F3/` (12 tile, 64×64). Zone isolation zorunlu — F1/F2 ile aynı pool'a girmesin.
- **W1 ✅ DONE** (ChatGPT, commit 2026-05-08) → `STAGING/tiles_raw/style_anchor_W1_wall_PRIMARY.png`. PRIMARY style anchor for all ChatGPT tiles.
- **W2 ✅ DONE** (ChatGPT, commit 2026-05-08) → slice via batch_tiles.ps1 → `Assets/Art/Tiles/Act1/W2/`
- **OBW ✅ DONE** (ChatGPT, commit 2026-05-08) → slice via batch_tiles.ps1 → `Assets/Art/Tiles/Act1/OBW/`
- **F3 ✅ DONE** → `Assets/Art/Tiles/Act1/F3/` 16 tile 64×64 (commit eb037a3). ChatGPT output 1254×1254 (non-standard), process_tiles.py cell-resize handled it automatically.
- **Trans F1→F2 ✅ DONE** → `Assets/Art/Tiles/Act1/Trans_F1F2/` 8 tile 64×64 (commit eb037a3). ChatGPT output 1774×887, same auto-resize.
- **Trans F2→F3 ✅ DONE** → `Assets/Art/Tiles/Act1/Trans_F2F3/` 8 tile 64×64 (commit eb037a3). Same.

### Room Scene Authoring (2026-05-08 — Task A DONE)
- **Mimari karar**: Prefab-per-room (scene degil). DungeonWorldBuilder -> replace.
- **RoomLoader event API**: `OnRoomLoaded(RoomConfig, GameObject)` + `OnRoomCleared` static events
- **RRM**: Simdilik dokunulmadi -- Task B'de `LegacyRuntimeRoomManager` rename + event subscribe
- **Task A ✅ DONE** (commit 3d64bab): RoomConfig.cs + RoomRegistry.cs + RoomLoader.cs + 3 pilot prefab placeholder (combat_01 / reward_01 / corridor_01)
- **Task B (pending)**: RRM rename + event subscribe baglantisi
- **Task C (pending B)**: 3 pilot prefab tile paint (F1 tile kullanimi)
- **Spec**: `TASARIM/room_authoring.md`
- **Pilot validation kriteri**: 3 prefab Play mode'da Instantiate -> event fire -> console log

### Tile Grid Math Kuralı (LOCKED)
- Floor 64×64: 1024×1024, 4×N grid — N sadece 1/2/4/8 (1024÷N integer olmalı, 3 YASAK)
- Wall 64×96: 1024×1536, 4×4 grid → 256×384 hücre
- Tall wall 64×128: 1024×1536, 4×3 grid → 256×512 hücre
- Codex $imagegen: tile için KULLANMA — smooth 3D render üretiyor. ChatGPT > Codex for pixel art.
- $imagegen syntax: `$imagegen "prompt"` (Codex task içinde). Pixel art için "pixel clusters min 4px, no gradients" ekle.
- **ChatGPT canvas boyut fix**: ChatGPT'de canvas açıkken "canvası tam olarak eşit X×Y hücreli grid olarak böl" şeklinde iste → doğru pixel boyutlarını üretebilir. (process_tiles.py arbitrary boyutu handle eder ama non-standard = hafif kalite kaybı)

### WallOcclusionFader (Hades stili saydamlaşma)
- `Assets/Scripts/Core/WallOcclusionFader.cs` → KOD HAZIR, değişiklik yok.
- Unity'de yapılacak: Wall Tilemap → Add Component → WallOcclusionFader. fadeRadius 2.2, minAlpha 0.38, fadeSpeed 10.

### Sıradaki Tile Üretimleri — ALL DONE (commit eb037a3)
1. **W1 ✅** (16t 64×96, `Assets/Art/Tiles/Act1/W1/`)
2. **W2 ✅** (16t 64×96, `Assets/Art/Tiles/Act1/W2/`)
3. **OBW ✅** (12t 64×128, `Assets/Art/Tiles/Act1/WB/`)
4. **F3 ✅** (16t 64×64) + **Trans F1→F2 ✅** (8t) + **Trans F2→F3 ✅** (8t) — non-standard ChatGPT input, process_tiles.py auto-handled

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

## NotebookLM (UPDATED - 2026-05-09)
- Notebook: RIMA Game Design Knowledge Base (ID: ed3c8952-417c-4988-84a7-425d25ba3b08)
- ~200 sources (full batch sync 2026-05-09: 89 yeni dosya — tüm TASARIM/MEMORY/STAGING)
- Last sync: 2026-05-09 | Last commit: b058c0a (2026-05-08)
- Stop hook: find+hash tabanlı — committed dosyaları da yakalar, git status'tan bağımsız
- /nlm-sync: hash karşılaştırmalı, NLM listesi bir kez çekilir, paralel sync
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

### Alabaster Dawn Araştırması + Opus Değerlendirmesi (2026-05-09 — TAMAMLANDI)
- **Araştırma**: Gemini (rima-research) + Codex GPT-5.5 High + ChatGPT PDF (STAGING/RIMA_Alabaster_Dawn_Expanded_Claude_Review_Pack.pdf)
- **Opus değerlendirmesi**: 9 öneri değerlendirildi, 10 LOCKED kural ihlali tespit edildi
- **v1 Sprint Paketi (LOCKED)**:
  1. ActionCommitProfile 5 alan (windupMs, recoveryMs, dashCancelStartFraction, hitstopMs, cancelOnWhiff) → BasicAttackProfile'a ekle
  2. 3-katman feedback hiyerarşi (Normal/Commit/Break) — Named outcome glyph v1'de YOK
  3. Rarity Common/Rare/Epic/Legendary ağırlık tablosu — Wild v3'e ertelendi, Epic korundu
  4. Sınıf ses imzası 10 sınıf (sadece SES, görsel v2)
  5. Rift Fracture isimlendirmesi (mevcut Rift Meta-Family üstüne sadece ad)
  6. Boolean hasInterruptArmor flag — sayısal poise meter v2
  7. Boss posture kalibrasyon 450/850 (2 boss tipi v1)
- **Reddedilen / ertelenen**:
  - Wild rarity → v3 (LOCKED dash-cancel + ICD + upgrade slot ihlali)
  - 5 Rift Portal türü → v2 (LOCKED %4 spawn ile aritmetik uyumsuz)
  - 10 named outcome → 4 outcome v2, 6'sı OWNS/AVOIDS çakışması
  - Tile Room Memory Overlay → v2 (önce DepthBandTileSet hookup + F4 Rift)
  - Sayısal poise meter → v2 (boolean armor flag v1)
- **Alabaster Dawn'dan RIMA'ya taşınan prensip**: animation commitment + 3-tier feedback + named outcome + rarity layer. Setting/narrative/2.5D/kit-swap taşınmaz.
- **STAGING dosyası**: STAGING/RIMA_Alabaster_Dawn_Expanded_Claude_Review_Pack.pdf (ChatGPT'nin 9 önerisi)

### Alabaster Dawn v2 — Opus Web Araştırması + Çarpışma Analizi (2026-05-09 — KİLİTLENDİ)
- **Araştırma**: Opus kendi araştırdı (Steam, xmodhub, Game8, RPGFan, Power Up Gaming, wiki) — önceki pas'taki PDF özetinin ötesine geçti
- **Yeni AD bulgular**: 8-frame (~133ms) parry penceresi, stamina-gate sistemi, blade-weaving (light/heavy alternasyon → defensive multiplier), hidden poise bar, element reaction naming (Magma/Arc/Shatter = 2 element → named outcome), Juno Identity Passive (upgrade edilemez), mid-dungeon rest break
- **v1 Sprint'e eklendi (LOCKED)**:
  1. `showPostureMeter` boolean toggle → Game Feel Toggles listesine eklendi (default ON)
  2. Ranger/Gunslinger dash-cancel fraction genişletildi: %35-50 → %30-55 (playtest kalibrasyonu)
  3. Cross-Class Proc tetiklenince sigil glyph üstüne 1 satır 12px text ("Tremor!" / "Severance!") + SFX
  4. Death recap + next-run hint UX layer (opsiyonel, boss yenilgisi sonrası)
- **v2 Sprint adayları (tasarım onayı gerekir)**:
  1. Resonance ara kademesi: 2-tag named outcome (10 pair: Tremor/Bloodveil/Severance/Crushblood/Resonant Hold/Lockedge/Splinter/Phantom Pulse/Hammerwound/Hemorrhage). Rift 3-tag kuralı KORUNUR
  2. DepthBand transition rest room (F1→F2 ve F2→F3 geçişinde 1 combat-yok oda)
  3. Boss weak-point sprite spawn (posture break 3s penceresinde spatial feedback)
  4. Reactive adaptation boss design prensip: telegraph ±10-15% timing varyans
  5. Brawler Identity Passive: LMB/RMB alternation reward (AD blade-weaving, class-spesifik)
  6. Ronin REACTIVE skill: 8-frame (~133ms) parry penceresi (class-spesifik, global parry yok)
- **Kesin reddedilen**: Stamina-Gate (hız LOCKED'ı kırar), global parry, companion sistemi, 2-element global reaction
- **v3'te kalan**: Wild rarity (AD'da karşılığı yok), 5 Rift Portal türü (Resonance v2'den sonra netleşir)
- **Önceki pas delta**: v1'e 4 yeni ekleme, v2'ye 6 aday, 0 yeni LOCKED ihlal

### Oyunu Kullanıcıya Anlatabilme (2026-05-09 — AKTİF GEREKSİNİM)
- Her sprint sonucunun teknik kararların yanı sıra "oyuncuya nasıl hissettiriyor" katmanıyla belgelenmesi gerekiyor
- Hedef: early access / playtester / pazarlama için sade dil özet
- **Combat sistemi kullanıcı özeti**: Her vuruş bir sözdür (commit). Saldırıya başlayınca belirli bir noktaya kadar devam etmek zorundasın. Zamanla dash ederek daha hızlı zincir kurarsın. Düşmanların gizli bir denge barı var — bu barı kırınca 3 saniyelik avantaj penceresi açılır. 10 sınıfın her birinin farklı vuruş ritmi ve kaynak döngüsü var. Farklı sınıflardan yetenek kullandığında sahada etiketler birikir; 3 farklı etiket aynı hedefte buluşunca büyük Rift patlaması tetiklenir.

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

### Isometric Z-Sort + Tile Sprite Fix (2026-05-09 — KOD HAZIR, Unity execution pending)
- **IsometricSortSetup.cs** (`Assets/Editor/DevTools/IsometricSortSetup.cs`) — menu: `RIMA/Setup Isometric Sorting`. Camera'ya CustomAxis Y-sort atar, Ground/Wall sorting layer oluşturur, tüm TilemapRenderer'ları Individual moda geçirir.
- **YSortBehaviour.cs** (`Assets/Scripts/Core/YSortBehaviour.cs`) — runtime Y-sort component. SpriteRenderer veya TilemapRenderer'a ekle; LateUpdate'te `sortingOrder = baseOrder - RoundToInt(y * yMultiplier)` hesaplar.
- **Act1TileImporter.cs** — `RIMA/Fix Tile Sprites (Sub-Asset Embed)` menu item eklendi. NULL sprite'lı tile'ları düzeltir (sub-asset embed, F1'e uygulanan pattern).
- **Execution sırası (Unity MCP ile)**: 1) `RIMA/Fix Tile Sprites` → 2) `RIMA/Setup Isometric Sorting` → 3) screenshot QC

### Code (DONE this session)

#### /nlm, /nlm-sync, /commit skill'leri (2026-05-08)
- `/query` skill silindi (outdated)
- `/nlm "soru"` -> NotebookLM query (`.claude/commands/nlm.md`)
- `/nlm-sync` -> NLM batch/single sync (`.claude/commands/nlm-sync.md`). Global template rename: `~/.claude/commands/nlm-sync-template.md` (artık çakışma yok)
- `/commit` -> uncommitted dosyaları gruplara ayırıp commit et (`.claude/commands/commit.md`). `/commit` preview, `/commit --yes` direkt commit.
- **NLM sync state tracking**: `.claude/nlm_sync_state.txt` — her sync sonrası content hash kaydedilir. Stop hook hash karşılaştırır, sadece gerçekten sync edilmemiş dosyaları gösterir.
- **Stop hook timestamp**: her session sonunda `[NLM] sync:MM/DD HH:MM | commit:MM/DD HH:MM` gösterir. Commit = içerik güncelliği, sync = NLM güncelliği.

#### room_authoring.md spec (2026-05-08)
- `TASARIM/room_authoring.md` -- Prefab-per-room kontrat, RoomConfig schema, render contract checklist, migration plani

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
- **DepthBandTileSet hookup DONE** — SetTilePool(TileBase[] floorTiles, TileBase[] wallTiles) lines 327-334'te mevcut

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
- **Mob Ideas Research (S45)**: `STAGING/MOB_IDEAS_GPT.md` (Codex/GPT-5.5, 10 proposals) + `STAGING/MOB_IDEAS_GEMINI.md` (Gemini web research, 10 proposals + 15 gap analysis). Act 2-3 enemy archetypes, Last Epoch/Dead Cells/Hades/RoR2/PoE kaynaklı. Design session bekliyor.

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

## Tooling Added (2026-05-09)
- **UnityMCP standardized**: tüm CCS instance'larına (`laurethgame`, `yasinderyabilgin`, `ydbilginn`, `ydbilgin`) `UnityMCP` eklendi / `mcp-unity` renamed. Tool prefix: `mcp__UnityMCP__*`. Node: `C:/Users/ydbil/mcp-unity/Server~/build/index.js`.
- **Not**: Yeni CCS account eklenince UnityMCP manuel eklenmesi gerekiyor (CCS custom MCP'leri kopyalamıyor).

## Tooling Added (2026-05-06)
- `/p` skill: ~/.claude/commands/p.md -- Gemini 2.5 Flash prompt beautifier (Claude prompting best practices baked in)
- `/ask_gemini` skill: ~/.claude/commands/ask_gemini.md -- inline Gemini query
- NotebookLM MCP: added via `claude mcp add`, package installed, nlm login done (yasinderyabilgin@gmail.com)
- cx laurethayday exec syntax confirmed: `Set-Location <dir>; cx laurethayday exec -s danger-full-access -m o4-mini "prompt"`

## Next Priorities
### Immediate next session:
0. **Verification agent sonuçları kontrol et** — Reimport All + EditMode test sonuçları. Eğer hala fail varsa ek fix (özellikle MainMenuScreen_CreatesCanvasWithGraphicRaycaster ve _CanvasSortOrderIsHigh için neden belirsizdi; rename sonrası geçer mi gör).
0. **Map fragment + DungeonGraph implementasyonu** — `TASARIM/map_fragment_system.md` ve `TASARIM/dungeon_act1_map.md` spec'lerine göre kod görevleri Codex'e dispatch edilebilir: MapFragment.cs (mevcut?), MapPanel.cs (yeni), DungeonGraph node visibility flags, RoomRegistry pool populate.
0. **Unity MCP — Tile Fix + Z-Sort** (MCP artık aktif, session restart sonrası): `RIMA/Fix Tile Sprites` → `RIMA/Setup Isometric Sorting` → screenshot QC. YSortBehaviour'ı Player'a ekle.
0. **v1 Combat Feel Sprint** — ActionCommitProfile 5 alan + 3-layer feedback + Rarity tier + Ses imzası + **AD v2 eklemeleri**: showPostureMeter toggle, Ranger/GS fraction %30-55, proc text feedback, death recap hint (bkz. Alabaster Dawn v2 bölümü)
0. **v2 Sprint tasarım oturumu** — Resonance 2-tag named outcome listesi onayı (10 pair), DepthBand rest room spec, boss weak-point sprite, Ronin parry penceresi
0. **F3/Trans tile import** — Unity Editor'da `RIMA/Import Act1 Tiles` menu item çalıştır (Act1TileImporter.cs pre-pass fix commit 75cf298 hazır; sadece execution gerekiyor)
1. **Pilot room validation** — Play mode: 3 prefabs Instantiate via RoomLoader → event fires → console log
2. **Task B**: LegacyRuntimeRoomManager rename + event subscribe
3. **F3/Trans tile QC** — Unity görsel kontrol
4. **DungeonWorldBuilder DepthBandTileSet hookup DONE** — SetTilePool lines 327-334
5. **WallOcclusionFader attach** → Wall Tilemap + Add Component
6. **Mob production** — PixelLab create_character + animate_character (8-dir, 48-52px); start with Act 1 mob
7. **Dash-Cancel** — BasicAttackProfile.cancelWindowFraction + PlayerController event
8. **OnDash Proc** — CrossClassEffectType.OnDash + HandleDash call site

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
- **F1 floor tiles VISIBLE in Play mode** (2026-05-08) — 30×30 painted at IsoGrid/Ground. PNG import pipeline was broken (stale DefaultAsset cache); fixed by embedding Sprites as sub-assets in each Tile .asset via Texture2D.LoadImage()+Sprite.Create()+AssetDatabase.AddObjectToAsset(). Player centered, CameraFollow working, Global Light2D intensity=1.0.

## Current Risks
- **Act1TileImporter fix hazır (commit 75cf298)** — F3/Trans sprite'ları için Unity'de `RIMA/Import Act1 Tiles` henüz çalıştırılmadı; tile'lar hâlâ null sprite ref içeriyor olabilir.
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
- **Room authoring Task A DONE** (commit 3d64bab) -- rima-qc review pending
- **F3/Trans_F1F2/Trans_F2F3 tile sprites NULL** -- same broken PNG import cache; need sub-asset fix (same pattern as F1)
- **RRM tile painting bagimliliklar** -- Task B'de soküm yapilacak; simdilik paralel calisiyor
- **F3/Trans tile QC pending** — sliced from non-standard ChatGPT dimensions (1254×1254, 1774×887); visual QC in Unity needed to confirm quality acceptable

## Key Pointers
- **Alabaster Dawn Opus Eval**: STAGING/RIMA_Alabaster_Dawn_Expanded_Claude_Review_Pack.pdf — 9 öneri, 10 LOCKED çakışma tespiti, v1 sprint paketi
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
- **batch_tiles.ps1**: `STAGING/batch_tiles.ps1` — batch slice W1/W2/OBW/F3 sheets (ALL DONE eb037a3)
- **Mob ideas research**: `STAGING/MOB_IDEAS_GPT.md` (GPT-5.5) + `STAGING/MOB_IDEAS_GEMINI.md` (Gemini web) — Act 2-3 enemy proposals
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
- Room authoring spec (LOCKED): `TASARIM/room_authoring.md`
- RoomLoader (Task A, pending): `Assets/Scripts/Systems/Map/RoomLoader.cs`
- RoomConfig (Task A, pending): `Assets/Scripts/Systems/Map/RoomConfig.cs`
- RoomRegistry (Task A, pending): `Assets/Scripts/Systems/Map/RoomRegistry.cs`
- Pilot prefabs (Task A, pending): `Assets/Prefabs/Rooms/Act1/`
