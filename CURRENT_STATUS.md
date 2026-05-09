# CURRENT STATUS
**2026-05-10 — S47 | Aktif Sprint: Faz 1-2 | PixelLab pipeline kanonik + Infamous Keepers analiz**

> **Son session (2026-05-10):** PixelLab MCP eklendi, 57 dosyalık docs konsolide, NLM sync. PRODUCTION_PLAYBOOK kanonik kararlara revize (252×252 canvas, pixel budget formülü, 8-frame attack, animate_character MCP yasak). Infamous Keepers Steam screenshot karşılaştırması — %92 sistem eşleşme, 16 aksiyon + 4 design decision tetiklendi.

---

## Açık İşler (Öncelik Sırasına Göre)

### 🔴 Yüksek Öncelik

1. **PixelLab tile üretimi** — `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md` Adım 1 ile başla (W1 wall)
   - Tool: Create Tile — Isometric, 64×128, 8 var, bg #00FF00
   - Sonraki: W2 → OBW → F1-F3 → transitions → obstacles
   - **Playbook revizyon kararı verildi (2026-05-10):** Eski Interpolation ÖLÜ → Interpolate NEW (v2, 252×252) + Animation-to-Animation Bridging Mode. animate_character MCP KALICI YASAK. Etkilenen adımlar: 17/21c/22-23/26/30-32/35/39-41/44/48-50
   - **MCP kuruldu:** `pixellab` MCP server aktif — tile/static prop için OK; karakter animasyonu için YASAK
   - Referans: `PixelLabDocs/` (57 dosya, NLM sync edildi)

2. **tiles_raw/yeni/ sheets process + import** — 6 sheet hazır, process edilmedi:
   ```
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f1_sheet.png --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f2_sheet.png --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f3_sheet.png --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/w1_connector_set.png --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 96 --prefix w1_conn_
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/w2_connector_set.png --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 96 --prefix w2_conn_
   python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/decal_f1_sheet.png --output Assets/Art/Tiles/Act1/Decal --cols 2 --rows 2 --width 64 --height 64 --prefix decal_f1_
   ```
   Sonra: Unity reimport → DemoRoomPainter görsel QC → w1_conn style uyumu kontrol

3. **PlayMode 4 fail** — RoomLoader/DungeonWorldBuilder mob spawn eksik (S46'dan açık):
   - MultiRoom_ClearRoom1_NavigateThenClear
   - RewardPickup_Interact_MarksCollected
   - LegacyRuntimeRoomManager_Room1Starts
   - RoomLoop_KillAllEnemies_RoomClears

### 🟡 Orta Öncelik

4. **NLM sync** — `STAGING/PIXELLAB/00_README.md` henüz NLM'de yok
   - `PRODUCTION_PLAYBOOK.md` ve `PixelLabDocs/index.md` sync edildi (2026-05-10)
   - Mevcut script `STAGING/` root maxdepth=1; alt klasör scope eklenmesi lazım

5. **Animasyonlara başla** (tile üretimi sonrası) — 4 ana class, v1 sprint:
   - Warblade (simetrik, 3 yön+flip) → Ranger → Shadowblade → Elementalist
   - Her class için: `STAGING/PIXELLAB/04-07_NEXT_*_anim/` klasörü hazır

6. **Lore rework** (4 yeni dosya + 4 küçük edit):
   - `TASARIM/STORY_RUN_PROGRESSION.md` — 9-run NPC tanışma + lore drip
   - `TASARIM/HUB_DESIGN_v1.md` — Hub mimari + 4 NPC
   - 3-ending cutscene detayı (KAL/KIR/TAŞI)
   - Lore drip death recap pipeline

7. **Infamous Keepers analizinden çıkan kritik aksiyonlar** (Faz 1-2'ye eklendi):
   - Damage number visual hierarchy spec (Normal/Commit/Break)
   - Damage number color coding (element/status'a göre renk)
   - FloatingTextSpawner anti-overlap + tick stacking
   - Character shadow spec (runtime drop shadow önerisi)
   - Detay: `STAGING/refs/infamous_keepers/self_review.md` E bölümü

### 🟠 Karar Bekleyen Design Decisions (Infamous Keepers tetikledi)

D1. **Mob faction system / infighting** var mı? (Hollow Knight tarzı)
D2. **Terrain hazard system** (spike trap, acid pool, crumbling floor)?
D3. **Adjacent room peek visibility** — kapı arkası rendering?
D4. **Hub combat sub-area** — Hub'ın yan odası combat olur mu?

### 🔧 Tooling Sorunları

T1. **cx wrapper non-interactive Codex** — uzun prompt'larda takılıyor. Partial fix var (`--prompt-file` flag, `codex_raw.ps1`). Codex CLI'ın kendisi de hang ediyor — terminal debug gerekli (auth/model/TTY hangisi). Detay: `memory/feedback_cx_wrapper_long_prompt.md`

### 🟢 Düşük Öncelik / Backlog

8. **Map Fragment + DungeonGraph** — spec hazır, Codex dispatch edilebilir (cx fix sonrası)
9. **MAKEUP_BACKLOG 8 eksiklik** — polish phase
10. **Cinematic Layer A/B/C/D** — Faz 2-5
11. **Wall architectural features** — Production Playbook'a Adım 17 (gargoyle/forge/niche)
12. **Chokepoint Combat room template** — Room template kataloğuna variant
13. **Background NPC layer in hub** — 5-8 silüet (yaşam hissi)

---

## LOCKED Kararlar Özeti (referans)

| Alan | Karar |
|---|---|
| Tile chromakey | #00FF00, filter: G>200 AND R<60 AND B<60, binary alpha snap |
| Duvar boyutu (PixelLab) | 64×128 (32×64 base + 2x nearest-neighbor upscale) |
| Zemin boyutu | 64×64, 16 var |
| Anim view | High top-down (~30-35°, Hades match) |
| Anim yönler | 4 yönlü + yatay mirror = 6 unique |
| v1 sprint sınıfları | Warblade / Ranger / Shadowblade / Elementalist (kalan 6 → v2) |
| Dungeon mimari | Prefab-per-room, Hades-style closed arena |
| Skill keybind | LMB/RMB + Q/E/R/F + V(ult) + Space(dash) |
| Shadow Echo | 3 katman (aura+phantom+UI flash), cyan #00FFCC, 50 havuz |
| Act 1 map | 15 node procedural (topoloji sabit, içerik random) |
| Boss posture | Faz1=700 / Faz2=850 / Faz3=1000 |

---

## Mimari Referanslar

- Tile pipeline: `STAGING/process_tiles.py`
- PixelLab playbook: `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md`
- Animation Bible: `TASARIM/ANIMATION_BIBLE.md`
- Skill System v2: `TASARIM/SKILL_SYSTEM_v2.md`
- Shadow Echo: `TASARIM/SHADOW_ECHO_MATRIX.md`
- Act 1 map: `TASARIM/dungeon_act1_map.md`
- Damage calc: `TASARIM/DAMAGE_CALCULATION.md`
- Mob rules: `TASARIM/MOB_COMPOSITION_RULES.md`
- Art pipeline: `GUIDES/RIMA_MASTER_ART_PIPELINE.md`
