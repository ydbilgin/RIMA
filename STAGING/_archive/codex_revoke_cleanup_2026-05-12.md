# RIMA 2.5D Revoke Cleanup Raporu (2026-05-12)

## 1. MEMORY/ Celiksi Tablosu
| Dosya | Satir | Icerik | Onerilen Aksiyon |
|---|---:|---|---|
| MEMORY/discord_pipeline.md | 67 | Priority: isometric tile workflow, object/icon pipeline, mob sprites. | REWRITE |
| MEMORY/feedback_animate_character.md | 19 | - MCP is allowed ONLY for: create_isometric_tile, create_object, create_map_object, create_tiles_pro (tiles and objects, NOT characters) | UPDATE_LINE |
| MEMORY/INDEX.md | 70 | - [project_pure_2d_topdown_pivot_2026-05-12.md](project_pure_2d_topdown_pivot_2026-05-12.md) -- WHEN: 2.5D vs 2D karar, chibi 64px, asset boyutlari, sprite/tile/VFX spec, REVOKE listesi | KEEP_AS_HISTORY |
| MEMORY/PIXELLAB_TOOL_GUIDE.md | 204 | - You need all 8 directions for top-down or isometric gameplay. | UPDATE_LINE |
| MEMORY/PIXELLAB_TOOL_GUIDE.md | 227 | - You need isometric, hex, octagon, or square top-down tile shapes. | UPDATE_LINE |
| MEMORY/pixellab_master_pipeline.md | 19 | ## S60 OVERRIDE - Pure 2D Top-Down (2026-05-13 LOCKED, supersedes 2.5D rules below) | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 21 | > Bu dosyanin geri kalanindaki "8 direction mandatory", "252x252 canvas", "128px karakter", "body-only anchor", "2.5D mimari" referanslari REVOKED. | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 23 | \| Konu \| Eski (2.5D / Faz 1 ChatGPT) \| S60 LOCKED (2026-05-13) \| | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 25 | \| **Karakter sprite** \| 128-252px native + body-only anchor + WeaponAnchorMap \| **64x64 chibi + silahli 1-piece**, PixelLab Create Character (Pixen) \| | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 34 | \| **Renderer** \| URP 3D + Billboard (2.5D) \| **URP 2D Renderer + Pixel Perfect Camera + 2D Lights** \| | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 54 | - "Body-only anchor" -> silahli 1-piece | KEEP_AS_HISTORY |
| MEMORY/pixellab_master_pipeline.md | 157 | \| **Silah sprite (2.5D)** \| **Create Image S-XL (new)** + DirectionNone + init image \| 64/128/256 native \| 1 canonical \| Bolum 9 \| | UPDATE_LINE |
| MEMORY/pixellab_master_pipeline.md | 262 | - 1 Direction yeterli (top-down isometric) | UPDATE_LINE |
| MEMORY/pixellab_master_pipeline.md | 426 | RIMA kendi Unity-based isometric design tool'unu gelistiriyor. | UPDATE_LINE |
| MEMORY/project_player_hades_system.md | 31 | **Why:** Hades 1/2 pattern proven for isometric action: character visually faces movement direction, but skills/attacks snap to aim direction at cast moment. | UPDATE_LINE |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 2 | name: Pure 2D Top-Down Pivot - 2.5D Detour Reverted | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 3 | description: 2026-05-12 - RIMA mimari pivotu. 2.5D denemesi (3D env + 2D billboard) terkedildi, pure 2D top-down chibi 64px secildi. | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 22 | **2.5D detour (S57-S58) basarisiz:** | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 25 | 3. KayKit chibi 3D + Blender render = AD aesthetic'e uymadi (cok cute, dark fantasy degil) | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 26 | 4. Reallusion CC Bases alternatifi icin Blender modelleme + texture workflow gerek - solo dev'e fazla | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 27 | 5. 3D environment + 2D sprite billboard = 3 sistem ayni anda (production cost x3) | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 42 | - **3D pipeline (Blender / KayKit) artik gereksiz:** Mob poz referansi icin opsiyonel. | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 60 | \| Render mimarisi: 2.5D (3D env + 2D Billboard) \| 2026-05-12 \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 63 | \| Dungeon mimari: 2.5D kare grid arena \| 2026-05-12 \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 64 | \| Body-only anchor 128px (V1 ChatGPT 30 deg ref pipeline) \| 2026-05-12 (chibi 64px native uretim) \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 65 | \| KayKit/Blender pre-render pipeline \| 2026-05-12 (gereksiz) \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 66 | \| Duvar boyutu 64x128 (isometric) \| 2026-05-12 (32x32 top-down) \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 67 | \| Zemin boyutu 64x64 (isometric) \| 2026-05-12 (32x32 top-down) \| | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 69 | ## RIMA_2.5D Nested Folder | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 71 | `F:\Antigravity Projeler\2d roguelite\RIMA\RIMA_2.5D\` - tasinacak: | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 72 | - Hedef: `F:\Antigravity Projeler\2d roguelite\RIMA\_ARCHIVE\RIMA_2.5D_attempt_2026-05-11\` | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 73 | - Rationale: 1 gunluk 2.5D denemesi, ders cikarildi, 2D'ye geri donuldu | KEEP_AS_HISTORY |
| MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md | 75 | Ayrica sibling `F:\Antigravity Projeler\2d roguelite\RIMA_2.5D\` da arsivlenecek. | KEEP_AS_HISTORY |

## 2. TASARIM/ Celiksi Tablosu
| Dosya | Satir | Icerik | Onerilen Aksiyon |
|---|---:|---|---|
| TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLOCKOUT_SET_2026-05-04.md | 18 | - floor tile: `128x64` isometric diamond | UPDATE_LINE |
| TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLOCKOUT_SET_2026-05-04.md | 348 | - chunky boardgame isometric block | UPDATE_LINE |
| TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md | 109 | - North-facing almost never visible to player in isometric view -> omitted | UPDATE_LINE |
| TASARIM/BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md | 56 | - LMB/RMB must be readable at RIMA isometric scale. | UPDATE_LINE |
| TASARIM/chibi_lore_integration_decision_2026-05-13.md | 81 | - 2D portrait Karar #72'yi ihlal etmiyor (3D/billboard/2.5D degil) | KEEP_AS_HISTORY |
| TASARIM/chibi_lore_integration_decision_2026-05-13.md | 106 | Orchestrator -> rima-asset: Decal pack brief. PixelLab MCP create_isometric_tile veya manuel pixel art. | UPDATE_LINE |
| TASARIM/GDD.md | 552 | **Teknik:** Unity 6.3 LTS, URP 2D, Global Light 2D + Point Light 2D. Pixel art isometric - karakter 128x128 canvas, zemin tile 64x32, duvar tile 64x96. | UPDATE_LINE |
| TASARIM/HUD_DESIGN_SPEC.md | 207 | - X Yamuk minimap (isometric diamond) | KEEP_AS_HISTORY |
| TASARIM/MASTER_KARAR_BELGESI.md | 82 | \| 72 \| **S59 Pivot - Pure 2D Top-Down LOCKED** \| 2.5D mimari (3D env + 2D billboard) REVOKED... \| | KEEP_AS_HISTORY |
| TASARIM/ROOM_DESIGN_PHILOSOPHY.md | 40 | > Tilemap birimi: 1 tile = 0.5x0.5 Unity unit (isometric grid, cellSize 1x0.5x1) | UPDATE_LINE |
| TASARIM/room_designer_f2_ux.md | 35 | 1. Wall corridor stamp -> otomatik isometric kose/duz baglanti (aninda) | REWRITE |
| TASARIM/room_designer_topdown_enhancement_2026-05-13.md | 22 | Onceki S58'de 2.5D refactor pending vardi -> S59'da REVOKED, pure 2D top-down geri. | KEEP_AS_HISTORY |
| TASARIM/STYLE_BIBLE.md | 55 | facing southeast, 3/4 ARPG view, 35 degree angle, isometric pixel art, orthographic projection, | UPDATE_LINE |
| TASARIM/STYLE_BIBLE.md | 97 | - **Side view YASAK** - isometric/3/4 ARPG kurali. | UPDATE_LINE |
| TASARIM/UNITY_STATE_OVERLAY_SPEC.md | 5 | Applies to: Unity 2D URP, isometric scene `Assets/Scenes/_IsoGame.unity`, Namespace RIMA. | REWRITE |
| TASARIM/UNITY_STATE_OVERLAY_SPEC.md | 329 | - Does the isometric camera rig ever rotate the parent enemy object? | REWRITE |
| TASARIM/UNITY_STATE_OVERLAY_SPEC.md | 345 | **EF-03: PipGroupRenderer arc positioning in isometric view** | REWRITE |
| TASARIM/UNITY_STATE_OVERLAY_SPEC.md | 347 | Issue: In an isometric 2D scene, "above the enemy head" in world space may drift visually. | REWRITE |
| TASARIM/FAZLAR/FAZ_MASTER.md | 28 | - **S59 Pivot LOCKED:** Pure 2D top-down + 64x64 chibi... Eski 2.5D mimari + 128px native + KayKit/Blender pipeline REVOKED. | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2_5D_TRANSITION_LOCKED.md | 1 | # RIMA 2.5D Gecis - LOCKED Kararlar (2026-05-11) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2_5D_TRANSITION_LOCKED.md | 30 | - Karakter view acisi (~30-40 deg) - AYNI, 2.5D uyumlu | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2_5D_TRANSITION_LOCKED.md | 45 | - 1 oda (3D Plane + Cube) + 1 billboard karakter + 1 point light | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 1 | # RIMA 2.5D Script Salvage Manifest | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 4 | **Hedef:** `F:\Antigravity Projeler\2d roguelite\RIMA_2.5D\Assets\Scripts\` | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 36 | ## ADAPT - Kopyala + 2.5D icin degistir (~30 script) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 42 | \| Player/PlayerAnimator.cs \| Billboard 4yon+flip, facing 3D vektorden hesapla \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 45 | \| Player/ShadowRecall.cs \| Pos snapshot COPY, silhouette spawn 3D billboard prefab \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 46 | \| Player/ShadowSilhouette.cs \| Sprite kopya stratejisi 3D billboard \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 53 | \| CrossClass/CrossClassGhostEffect.cs \| Billboard sprite kopya stratejisi \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 59 | \| Core/HitImpact.cs \| VFX spawn pos 3D, sprite VFX -> billboard quad \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 68 | \| VFX/* (tumu) \| Sprite VFX -> billboard quad / VFX Graph; mantik COPY \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 70 | \| UI/HoverOutline.cs \| URP outline billboard sprite'a \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 105 | 1. **Skill VFX hooks:** Skill logic COPY ama VFX prefab'lar 2D sprite -> 2.5D billboard quad donusumu ayri is | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/2.5d_salvage_manifest.md | 106 | 2. **Billboard yon mantigi:** PlayerAnimator + Mob animator'lari ayri hesapliyor -> `BillboardFacing` util tek noktaya cek | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 1 | # RIMA - Body-Only Anchor Uretim Spesifikasyonu (V1) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 4 | **Baglam:** 2.5D mimari + Silah Anchor sistemi + 4 yon flip kurali | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 37 | Karakter yatay merkezde, alt %10 bos (billboard pivot bottom-center). | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 111 | no weapon visible, no props, body-only anchor pose, RIMA character anchor reference | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 125 | shading, no sword visible, body-only anchor, RIMA Warblade reference | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 134 | pixel art, bold style, single color outline, body-only anchor, RIMA Ranger reference | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 143 | color outline, no daggers visible, body-only anchor, RIMA Shadowblade reference | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 152 | single color outline, no staff visible, no orb visible, body-only anchor, | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 202 | - Billboard 35 deg kamerada siluet okunabilir mi? | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/anchor_body_only_spec.md | 259 | \| 2.5D billboard 35 deg ortho \| project_2.5d_architecture.md \| | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 30 | 4. Project name: `RIMA_2.5D` | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 60 | - Billboard.cs (SpriteRenderer her frame kameraya doner) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 70 | Validation: Console'da hata yok. `Assets/Scripts/Runtime/Billboard.cs` gorunuyor | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 82 | - Empty GameObject "Player" + Billboard component | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 92 | ### Adim 0.5 - Billboard Test | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 97 | - Billboard.cs component'i ekle | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 104 | Validation: Sprite kamera nereye baksa hep cepheden gorunuyor, billboard calisiyor | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 149 | Validation: Sahnede 1 oda + 1 billboard karakter + isik calisiyor, pixel-perfect | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 186 | ### Adim 1.2 - Weapon Removal (Body-Only Anchor) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 214 | Validation: 3 yon body-only anchor, eller "bos kavrama" pozunda | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/USER_WORKFLOW_GUIDE.md | 231 | - Start Frame: body-only anchor (Adim 1.2'den) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/WEAPON_PRODUCTION_GUIDE.md | 1 | # RIMA - Silah Uretim Guide (2.5D Ayrik Katman Sistemi) | KEEP_AS_HISTORY |
| TASARIM/_ARCHIVE_2.5D_2026-05-12/WEAPON_PRODUCTION_GUIDE.md | 339 | RIMA 2.5D ayrik silah katmani sistemine gecti. | KEEP_AS_HISTORY |

## 3. STAGING/ Obsolete Dosyalar
- STAGING/cinderia_research_prompt.txt: One-off research prompt tied to isometric/top-down comparison; DELETE candidate after Claude confirms no reuse.
- STAGING/concept_art/_DISCARDED_2026-05-10_painterly/REASON.md: Already discarded concept-art reason file references old 128px character target; DELETE candidate if discarded folder is no longer needed.
- STAGING/refs/infamous_keepers/analysis.md: Reference note is only a single high top-down isometric camera note; KEEP_AS_HISTORY unless reference pack is being pruned.
- STAGING/cinderia_deep_research_2026-05-12.md: Research artifact with one 2.5D mention; KEEP_AS_HISTORY.
- STAGING/cinderia_research_2026-05-12.md: Research artifact with 3 old-style references; KEEP_AS_HISTORY.
- STAGING/hero_siege_hammerwatch_research_2026-05-12.md: Research comparison mentions 2.5D but remains historical context; KEEP_AS_HISTORY.
- STAGING/nlm_boss_design_2026-05-12.md: NLM dump contains revoked terms and locked pivot citations; KEEP_AS_HISTORY.
- STAGING/nlm_mob_design_2026-05-12.md: NLM dump contains revoked terms and locked pivot citations; KEEP_AS_HISTORY.
- STAGING/pixellab_videos_synthesis_2026-05-13.md: Current S60 synthesis explicitly warns not to revive old 2.5D anchors; KEEP_AS_HISTORY.
- STAGING/concept_art/INDEX.md: Failed concept-art index contains one isometric scale note; KEEP_AS_HISTORY or UPDATE_LINE if concept_art staging remains active.

## 4. .claude/ Eski Promptlar
- .claude/codex_arch_review_prompt.txt: Old architecture review prompt for 2.5D + 3D + billboard pipeline; DELETE candidate.
- .claude/codex_chibi_lore_review_output.txt: Contains current ban/context language for 3D, billboard, and 2.5D; KEEP_AS_HISTORY.
- .claude/codex_chibi_lore_review_prompt.txt: Contains current ban/context language for 3D, billboard, and 2.5D; KEEP_AS_HISTORY.
- .claude/codex_concept_art_f1_2_prompt.txt: Only matched "3d render" in negative prompt; KEEP_AS_HISTORY.
- .claude/PROJECT_RULES.md: Active rule says 2.5D/3D/billboard are forbidden; KEEP_AS_HISTORY.

## 5. PIXELLAB_OUTPUTS/ Yapisi (ozet)
- .: 3 files
- _ARCHIVE: 12 files
- concept_art: 16 files
- elementalist: 2 files
- floors: 6 files
- obstacles: 3 files
- ranger: 2 files
- shadowblade: 2 files
- walls: 4 files
- warblade: 2 files
- elementalist/outputs: 0 files
- elementalist/outputs/01_base_4dir: 0 files
- elementalist/outputs/02_idle_hit_death: 0 files
- elementalist/outputs/03_run_cycle: 0 files
- elementalist/outputs/04_attack_LMB: 0 files
- elementalist/outputs/05_attack_RMB: 0 files
- elementalist/outputs/06_dash: 0 files
- floors/outputs: 0 files
- floors/outputs/f1: 0 files
- floors/outputs/f2: 0 files
- floors/outputs/f3: 0 files
- floors/outputs/trans: 0 files
- obstacles/outputs: 0 files
- ranger/outputs: 0 files
- ranger/outputs/01_base_4dir: 0 files
- ranger/outputs/02_idle_hit_death: 0 files
- ranger/outputs/03_run_cycle: 0 files
- ranger/outputs/04_attack_LMB: 0 files
- ranger/outputs/05_attack_RMB: 0 files
- ranger/outputs/06_dash: 0 files
- ranger/outputs/07_weapon_pass: 0 files
- shadowblade/outputs: 0 files
- shadowblade/outputs/01_base_4dir: 0 files
- shadowblade/outputs/02_idle_hit_death: 0 files
- shadowblade/outputs/03_run_cycle: 0 files
- shadowblade/outputs/04_attack_LMB: 0 files
- shadowblade/outputs/05_attack_RMB: 0 files
- shadowblade/outputs/06_dash: 0 files
- shadowblade/outputs/07_weapon_pass: 0 files
- walls/outputs: 0 files
- walls/outputs/obw: 0 files
- walls/outputs/w1: 0 files
- walls/outputs/w2: 0 files
- warblade/outputs: 0 files
- warblade/outputs/01_base_4dir: 0 files
- warblade/outputs/02_idle_hit_death: 0 files
- warblade/outputs/03_run_cycle: 0 files
- warblade/outputs/04_attack_LMB: 0 files
- warblade/outputs/05_attack_RMB: 0 files
- warblade/outputs/06_dash: 0 files
- warblade/outputs/07_weapon_pass: 0 files

## 6. CODEX_TASK / CODEX_DONE Durumu
- CODEX_TASK.md: Contains this S59 revoke cleanup scan task, allowed read/write boundaries, pattern list, required report structure, and no-git instruction.
- CODEX_DONE.md: Previous content was cleared at task start per CODEX.md automation; file was absent/blank during the scan and will be rewritten with final status.

## 7. RIMA_2.5D Nested Proje
Exists: no. File count: 0. Estimated size: 0 MB.

## OZET TABLOSU
- Toplam celiksi: 101 scan items (90 MEMORY/TASARIM line hits + 10 STAGING file candidates + 1 obsolete .claude prompt)
- DELETE oneri: 3 dosya
- UPDATE oneri: 14 dosya
- REWRITE oneri: 4 dosya
- Toplam tahmini bosaltilacak alan: 0 MB

STATUS: DONE
COMPLETED:
- Scanned MEMORY markdown files for revoked 2.5D patterns.
- Scanned TASARIM recursively for revoked 2.5D patterns.
- Scanned STAGING recursively and summarized obsolete-file candidates.
- Scanned .claude markdown/text prompts for 2.5D, 3D, and billboard terms.
- Listed PIXELLAB_OUTPUTS folder structure to three levels with per-folder direct file counts.
- Checked CODEX_TASK.md, CODEX_DONE.md status, and RIMA_2.5D nested project metadata.
ERRORS: NONE
FILES_TOUCHED:
- STAGING/codex_revoke_cleanup_2026-05-12.md
NEXT_SIGNAL: "revoke_cleanup_scan_done"
