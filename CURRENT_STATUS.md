# CURRENT STATUS
**2026-05-10 — S48 | Aktif Sprint: Faz 1-2**

> **S48 bu session özet:**
> - **PlayMode 4 fail ÇÖZÜLDÜ (commit 88f084d):** Root cause = scene'de class adı yanlış (RuntimeRoomManager→LegacyRuntimeRoomManager) + m_Enabled=0. YAML fix + Codex coroutine lifecycle (roomRunId) commit edildi. QC: PARTIAL PASS — Unity'de verify gerekiyor.
> - **Statusline dinamik fix:** terminal genişliğine göre adaptif (>=110 full, >=90 medium, <80 compact). Küçük pencerede artık taşmıyor.

> **Sıradaki session:**
> 1. Unity'de PlayMode testleri çalıştır (4 fail → 0 verify)
> 2. tiles_raw/yeni/ 6 sheet process + Unity import (komutlar aşağıda)
> 3. Asset üretimi başlat — 4 base prompt padding-ready
> 4. Wall Adım 2-3 (W2/OBW) düzeltme

---

## Açık İşler (Öncelik Sırasına Göre)

### 🔴 Yüksek Öncelik

1. **PixelLab üretim — TEK GİRİŞ DOSYASI:** `GUIDES/PIXELLAB_PRODUCTION_GUIDE_v2.md`
   - **Bölüm 0** = kanonik kurallar (252×252 canvas, pixel budget formülü, MCP yasakları)
   - **Bölüm 11** = adım adım tıklama rehberi (Faz A: tile/obstacle → Faz B: 4 sınıf anim)
   - Detaylı prompt'lar için → `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md` (51 adım)
   - Tool detayı için → `PixelLabDocs/<tool>.md` (57 dosya)
   - **MCP kuruldu:** `pixellab` server — tile/static prop için OK; karakter animasyonu YASAK
   - **Üretim sırası:** F1→F2→F3 → Transitions → W1→W2→OBW → 8 obstacle → 4 sınıf anim sırayla

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

3. **PlayMode 4 fail — VERIFY GEREKİYOR** — Root cause fix commit edildi (88f084d). Unity'de `Run Tests > PlayMode` ile doğrula. 4 test: MultiRoom_ClearRoom1, RewardPickup_Interact, LegacyRuntimeRoomManager_Room1Starts, RoomLoop_KillAllEnemies.

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
