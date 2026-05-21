# CURRENT_STATUS
> Archive: `STAGING/_archive/current_status_pre_s97_late.md`
> Bu session: **S97 LATE NIGHT (2026-05-21)** — Wall Pack v3 LIVE + 22 tile asset + paint test PASS
> Önceki session pickup için bu dosyayı oku.

---

## 🟢 LIVE — Major Deliverables Bu Session

### Wall Pack v3 — 22 Tile Asset PRODUCTION-READY 🌟

- **Path:** `Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/`
- **22 unique tile:** archway_NE/SE (multi-direction!), column, wall_hero, straight NE/SE, 4 outer corner, 2 inner corner, 4 T-junction, low walls, foundation, floor_edge
- **Paint test PASS:** PlayableRoom_v2'de wall_v3 ile rectangular room + south archway, screenshot kaydedildi
- **Scene saved:** corruption prevention HARD RULE
- Memory: `project_wall_pack_v3_live.md`

### Production Workflow LOCKED — Workflow F Hybrid

- **Path:** `STAGING/_research/PIXELLAB_WORKFLOW_INSIGHTS.md` (288 satır)
- Memory: `feedback_chatgpt_pixellab_hybrid_workflow.md`
- ChatGPT concept → PixelLab Create Image Pro refine → Codex slice → UnityMCP import
- **%93 cost saving** vs naive per-piece (50 gen vs 660 gen for 22 walls)

### Codex Multi-Source Imagegen Done

3 farklı pack üretildi, comparison verdict alındı:
- `act1_wall_modular_pack_codex_v1.png` (Codex imagegen v1, 512×512 RGBA)
- `act1_wall_modular_pack_codex_v2.png` (Codex imagegen v2, RGBA transparent, no text)
- `act1_wall_modular_pack_chatgpt_v1.png` (ChatGPT, 1254×1254 RGB, alpha eksik)
- `act1_wall_pure_pixellab_v3_clean.png` (PixelLab pure prompt, WINNER) ⭐

Comparison: `STAGING/_research/MASTER_SHEET_REVIEW_VERDICT.md` + `MASTER_SHEET_V2_COMPARISON.md`

### Door Choice Mechanic — 3 Visualization Done

- **Path:** `STAGING/_pixellab_outputs/door_choice/`
  - `proposal_A_echo_loom_fractures.png` (cyan rift seams + glyph icons)
  - `proposal_B_mirror_remnant_triptych.png` (3 cracked mirrors with reflections)
  - `proposal_C_defender_echo_silhouettes.png` (3 ghost defenders + footprint trail)
- **Brainstorm doc:** `STAGING/_research/BUFFER_FILL_DOOR_CHOICE_BRAINSTORM.md`
- **LOCK status:** PENDING (Phase 2 task #8)

### Phase 2 Mega-Batch Plan READY

3 batch identified, **~100-150 gen total** for ALL objects + overlays + pickups:
- Batch 1: Medium Decorations (size 64, n_frames=16) → 16 sprite
- Batch 2: Large Features (size 128, n_frames=4) → 4 sprite
- Batch 3: Tiny Overlays + Pickups (size 32, n_frames=64) → ~30 sprite

Prompts hazır, paste-ready (see chat history).

### Discord Insights System

- Folder: `STAGING/_research/discord_archives/` (manual screenshot dump)
- Doc: `STAGING/_research/PIXELLAB_WORKFLOW_INSIGHTS.md` (auto-update via Codex)

---

## 📁 Yeni Dosya Inventory (Bu Session)

### Memory (`~/.claude/projects/.../memory/`)
- `project_wall_pack_v3_live.md` — 22 tile inventory + production workflow
- `feedback_chatgpt_pixellab_hybrid_workflow.md` — Workflow F lock

### Research (`STAGING/_research/`)
- `PIXELLAB_WORKFLOW_INSIGHTS.md` — 8 tool analysis + 6 patterns
- `MASTER_SHEET_REVIEW_VERDICT.md` — v1 comparison
- `MASTER_SHEET_V2_COMPARISON.md` — v1/v2/ChatGPT comparison
- `BUFFER_FILL_DOOR_CHOICE_BRAINSTORM.md` — Echo Loom / Mirror / Silhouettes

### Codex Tasks (`STAGING/codex_task_*`)
- 10+ task dispatch files (all completed, archived in STAGING)

### Assets
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/` — 22 sliced PNGs + contact sheet
- `Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/` — 22 Unity Tile assets

### PixelLab Outputs (`STAGING/_pixellab_outputs/`)
- `walls/v2/` — 4 master sheet versions (3 source comparison)
- `door_choice/` — 3 proposal visualizations
- `walls/v2/act1_wall_tall_straight_v2.png` — Sprint 1 single wall test

---

## 🎯 NEXT SESSION — Yapılacaklar

### A) Scene Cleanup + Setup (Önce)
1. Eski Dungeon A/B/C kalıntıları zaten temizlendi (cleanup yapıldı)
2. **Sorting layer reorder** gerekli — Floor şu an LAST (üstte), Wall ARKADA kalıyor
   - Doğru sıra: Default → Floor → Walls → Entities → VFX
   - Şu an: Default → Ground → Walls → Entities → VFX → Floor (yanlış)
   - Unity Edit → Project Settings → Tags & Layers → Sorting Layers
3. Scene view iso lock — boşluğa tıklayınca top-down'a dönmesin
   - Unity Scene View'da rotate/perspektif gizmo'su var
   - **2D toggle** açık olmalı (sol üstte "2D" butonu) → iso projeksiyon kalır
   - Veya custom scene camera lock script

### B) Multi-Room Painting (Asıl iş)
3-4 farklı oda shape paint et, hepsi proper iso:
1. **Rectangle room** (basit, 8×6)
2. **L-shape room** (corner_inner kullan, oda cutout)
3. **Multi-chamber** (T-junction ile 2 oda bağlantısı)
4. **Boss antechamber** (büyük 12×10, archway exit + columns)

Wall_v3 22 tile + Unity flipX ile yeterli coverage.

### C) Phase 2 Mega-Batch Dispatch
Sıralama:
1. **Mega-Batch 1** — Medium Decorations (~30-50 gen)
2. **Mega-Batch 2** — Large Features (~30-40 gen)
3. **Mega-Batch 3** — Tiny Overlays + Pickups (~40-60 gen)

Prompts kaydedildi (chat history + memory).

### D) Door Choice LOCK (Memory)
3 visualization mevcut, LOCK pending:
- Phase 1 standard = A (Echo Loom Fractures)
- Phase 2 rare event = C (Echo Silhouettes)
- Act 4 reveal = B (Mirror Triptych)

Hibrit lock memory'e yazılır + Phase 2 production'a girer.

### E) Git Commit (Önce git status)
Bu session'da büyük asset gen yapıldı:
- 22 sliced PNG (`wall_pack_v3/`)
- 22 Unity tile asset (`walls_v3/`)
- 4 master sheet PNG (comparison)
- 3 door choice visualization
- Multiple research docs (~5 markdown)
- Updated PlayableRoom_v2 scene

Commit önerisi: `[S97-LATE] Wall pack v3 LIVE + Workflow F + door choice viz`

---

## 🛠️ Tools Used (Bu Session)

| Tool | Use case |
|---|---|
| ChatGPT/DALL-E (manual) | Initial wall pack concepts |
| PixelLab Create Image Pro (Web UI) | Pure pixel art wall pack v3 |
| Codex imagegen (cx_dispatch.py) | Door choice visualizations + comparison |
| Codex Python PIL (cx_dispatch.py) | Master sheet init grids + slicing |
| UnityMCP execute_code | Sprite import + tile asset creation + scene paint |
| UnityMCP manage_scene | Scene view framing + save |
| UnityMCP manage_camera | Scene screenshots |

---

## ⚠️ Bilinen Problemler / Notlar

### Sorting Layers Order Yanlış
Sahnede Floor en üstte render oluyor (görünür kapatıyor). Düzeltme: Floor → Ground gibi erkene taşı.

### Scene View Iso Locking
Boşluğa tıklayınca Unity Scene View top-down'a dönüyor. Workaround:
- 2D toggle hep açık olsun
- Veya Scene View → Rotate gizmo lock

### Wall Sprite Pivot
Walls BottomCenter pivot ile import edildi. Doğru oturuyor mu test: yan yana paint'te kontrol.

### Codex Dispatch Profile
Çoğu dispatch laurethgame/laurethayday profile'ları kullanıyor. Yasinderyabilgin profil dengeli kalsın.

---

## 📊 PixelLab Budget Status

- **Önceki:** 2,820 / 5,000
- **Bu session spend:** ~150-200 gen (master sheets + 3 door choice + comparison)
- **Mevcut tahmin:** ~2,400-2,500 / 5,000
- **Reserve:** ~2,500 gen (Phase 2-6 için bol)

---

## 🚦 Yeni Session İlk Adımlar

1. `.claude/PROJECT_RULES.md` + bu CURRENT_STATUS.md oku
2. **Sorting layer fix** — Project Settings → Sorting Layers (Floor'u Ground'a taşı veya yeniden sırala)
3. **Scene view 2D toggle** kontrol et (iso kalsın)
4. PlayableRoom_v2 aç, mevcut state'i gör
5. **Multi-room paint** — 3-4 farklı shape (rectangle, L, multi-chamber, boss)
6. **Phase 2 Mega-Batch 1** (Medium Decorations) dispatch

---

## 🔗 Quick Reference

- Wall pack: `Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/` (22 tile)
- Floor tiles: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/` (16 tile)
- Scene: `Assets/Scenes/Demo/PlayableRoom_v2.unity` (LIVE, committed)
- Lore: `memory/project_act1_shattered_keep_lore_lock.md`
- Workflow lock: `memory/feedback_chatgpt_pixellab_hybrid_workflow.md`
- Wall pack lock: `memory/project_wall_pack_v3_live.md`
