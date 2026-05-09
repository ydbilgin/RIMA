# CURRENT STATUS
**2026-05-10 - S47 DAY - PixelLab production setup (klasör + playbook), NLM orphan cleanup, 4 ana class anim önceliği LOCKED**

## S47 Day Session (2026-05-10)

### PixelLab Klasör Yapısı + Tek-Dosya Playbook
- **`STAGING/PIXELLAB/` kuruldu** — numaralı üretim klasörleri (üretim sırasına göre):
  - `00_README.md` — master index, kategori tablosu, klasör akışı
  - `01_NEXT_walls/` — HOWTO + W1/W2/OBW PROMPT.md + outputs/{w1,w2,obw}
  - `02_NEXT_floors/` — HOWTO + F1/F2/F3/TRANS PROMPT.md + outputs/{f1,f2,f3,trans}
  - `03_NEXT_obstacles/` — HOWTO + 8 obje (pillar/rubble/torch/crack/barrel/bone/stump/altar)
  - `04_NEXT_Warblade_anim/` — Simetrik (3 yön + W flip)
  - `05_NEXT_Ranger_anim/` — Asimetrik (4 yön)
  - `06_NEXT_Shadowblade_anim/` — Asimetrik
  - `07_NEXT_Elementalist_anim/` — Asimetrik, silahsız (07_weapon_pass YOK)
  - `_DONE/` + `_ARCHIVE/` (12 eski v1/v2 prompt dosyası taşındı)
- **`STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md`** — tek dosya, 51 adımlı sıralı playbook. Her adımda: tool, ayarlar, kopyala-yapıştır prompt, output path, process komutu, QC checklist. Açıp checkbox'la ilerleme.
- Her anim klasöründe `outputs/01_base_4dir → 07_weapon_pass` Master Pipeline 5 adımına göre alt-klasörlü.

### 4 Ana Class Anim Önceliği LOCKED (2026-05-10)
- **Faz 1-2 ana class:** Warblade, Ranger, Shadowblade, Elementalist (BasicAttackProfile contract'ı bu 4'te tanımlı)
- Önceki Animation Bible önceliği (Warblade > Ronin > Shadowblade > Ranger > Brawler > GS > Elem > Ravager > Hexer > Summoner) → SUPERSEDED. v1 sprintinde sadece bu 4 sınıfa odaklan.
- Kalan 6 sınıf (Ronin/Brawler/Gunslinger/Ravager/Hexer/Summoner) → v2 sprint'e ertelendi
- Memory: `project_v1_anim_4classes.md`

### NLM Orphan Cleanup
- **Script güncellendi** (`.claude/commands/nlm-sync.md`):
  - `/nlm-sync --cleanup-dry` → orphan'ları listele (silmez)
  - `/nlm-sync --cleanup` → orphan'ları NLM'den + state'ten sil
  - `/nlm-sync` (batch) → sonunda otomatik orphan uyarısı
  - `/nlm-sync --status` → orphan'ları da gösterir
- **Güvenlik:** Sadece `nlm_sync_state.txt`'te izi olan + local'de yok olan dosyalar silinir. Manuel yüklenmiş GUIDES/PDF kaynaklara dokunulmaz.
- **İlk cleanup yapıldı:** 21 orphan NLM kaynağı silindi (hepsi `_ARCHIVE/` veya `ARCHIVE/SUPERSEDED/`'e taşınmış eski PixelLab/animation prompt'ları), state 153→130 satır (2 bozuk satır da temizlendi).

### Acık Isler
- **PixelLab üretimine başla** — `STAGING/PIXELLAB/PRODUCTION_PLAYBOOK.md` Adım 1 (W1 wall) ile aç
- **NLM sync** — yeni `STAGING/PIXELLAB/00_README.md` ve PRODUCTION_PLAYBOOK.md'yi NLM'e yüklemek istersen alt klasör scope'u eklemek lazım (mevcut script `STAGING/` root maxdepth=1)

---

**2026-05-09 - S46 NIGHT SESSION - Tile pipeline complete, Alabaster Dawn analysis, Opus design/mob/asset decisions + YENİ TILE SET ÜRETİLDİ (green chromakey, 6 sheet)**

## S46 Night Session (2026-05-09 — night continuation)

### Tile Pipeline — COMPLETE (F1/F2 now clean)
- **F1 re-processed**: `STAGING/IMAGEGEN_OUTPUTS/f1.png` (1254×1254, proper magenta bg, 4×4). Old green-chromakey F1 fully replaced.
- **F2 re-processed**: `tiles_raw/style_anchor_F2_floor.png` (canonical magenta source, 1024×1024, 4×4). `IMAGEGEN_OUTPUTS/f2.png` REJECTED (dark purple bg (47,0,45) — unprocessable).
- **process_tiles.py fix APPLIED**: Broader chromakey `(r>140) & (b>120) & (g<55) & ((ri+bi)>(gi*3+150))` + binary alpha snap `np.where(alpha<128, 0, 255)`. Both fixes live in `STAGING/process_tiles.py`.
- **WB folder DELETED**: Duplicate of OBW. Removed via AssetDatabase.DeleteAsset.
- **Trans old tiles DELETED**: 32 stale `tf12_*` / `tf23_*` .asset files removed. New `trans_f1f2_00..07` and `trans_f2f3_00..07` in place.
- **DoorEast / DoorWest REMOVED from scene**: Dungeon flows North/South only (Hades-style).
- **DemoRoomPainter.cs CREATED** (`Assets/Editor/DevTools/`): MenuItem `RIMA/Paint Demo Room` — fills 14×10 floor rect with random F1 tiles + W1 walls north/south.
- **Unity import sequence**: ImportAll (force-reimport PNGs) → FixTileSprites (rebuild sprite refs). alreadyOk=108, Created=12 (OBW).

### Opus Dungeon Design Decisions (LOCKED)
- **F1** (Entry rooms): Cold gray stone, sparse moss, dim torchlight. Oppressive, uniform.
- **F2** (Mid): Cracked walls, deeper shadows, faint bioluminescent lichen. Pressure builds.
- **F3** (Deep): Dark volcanic stone, pulsing energy cracks, corrupted glyphs. Boss territory.
- **Hybrid tile pipeline** (LOCKED): ChatGPT for structural tiles (floors, walls, door arches, floor decals). PixelLab for decorative props (torches, rubble, crystals, AO shadows) — 16-variation feature valuable for props.
- **Missing tile sets identified**: Corner tiles, door arches, floor decals, AO shadow tiles.

### Opus Mob/Boss Roster Expansion (LOCKED 2026-05-09)
- **New mob proposals (M09-M13)**: Resonance blocker, Rage drain, Posture specialist, Wound stacker, Keystone disruptor.
- **New elite proposals**: E01 Vow-Hammer Confessor (posture-break punisher), E02 The Wound (Wound Field stacker).
- **M09 Toll Hound + M10 Vow Speaker**: Act 1 rostere EKLENDI (LOCKED).
- **Remaining open**: Resonance 2-tag named outcomes (v2 sprint), Elite posture at 120?, Wound Field x Mob Armor interaction rule, Sovereign arena conflict, Cross-class secondary unlock timing.

### Opus Asset Pipeline Final Decisions (LOCKED)
- Canonical F2 source = `tiles_raw/style_anchor_F2_floor.png`.
- `IMAGEGEN_OUTPUTS/f2.png` and `rima_act1_environment_sheet_alpha/chromakey_2026_05_04.png` → ARCHIVE (pre-style-lock sheets, never use again).
- 3 production sessions planned to complete Act 1 (corner tiles, door arches, floor decals/AO props).
- PixelLab prompt specs drafted for torch/rubble/crystal variants.

### Alabaster Dawn `terra/` Analysis
- **Standard tile format**: 672px width, variable height. Per-tileset JSON metadata (`tilesets.json`, `nyx-tiles.json`).
- **Chromakey**: Bright yellow (255,255,0) — reliable, no stone color conflicts.
- **Autotile connectivity**: JSON border/connected rules define which tile types can neighbor each other.
- **Transferable to RIMA**: JSON metadata schema (`Assets/Data/TileSets.json`), autotile connectivity for F1→W1 transitions, `swapImg` pattern for tile variants, region-clip format `[px, py, pw, ph]`.
- **Not applicable**: 672px width (RIMA uses 64px base unit), RPG Maker engine specifics.

### Preview Images (for visual QC)
- `tiles_raw/preview/` — checkerboard-background PNGs for all source sheets + F1 sliced preview (4×4 grid, 4× scale).

---

## S46 Tile Pipeline Session (2026-05-09 — evening/night)

### Tile Pipeline Root Cause Found + Fixed
- **Root cause**: F1/F2 tiles were never run through `process_tiles.py`. They were manually split (wrong tool/approach), bypassing the chromakey removal step entirely. That's why colored artifacts remained — raw source pixels were present.
- **Chromakey filter gap**: process_tiles.py's filter `(r>225) & (g<30) & (b>225)` was too narrow for ChatGPT output which produces R≈159-248, G≈0-47, B≈130-247. **Fix pending** — needs HSV-based detection + binary alpha snap (0 or 255 only, from Alabaster Dawn analysis).
- **HSV overreach incident**: Attempted HSV hue 270-360 filter on existing tiles — destroyed wall art (blue-gray stone tones fell in same range). Reverted via `git checkout HEAD -- "Assets/Art/Tiles/Act1/"`.

### New Source Sheets Processed (all from tiles_raw/ with proper magenta backgrounds)
- **F3** (16 tiles, 64×64) — `tiles_raw/yeni/ChatGPT Image 9 May 2026 16_44_47.png` (1254×1254, 4×4 mossy stone)
- **Trans_F1F2** (8 tiles, 64×64) — `tiles_raw/yeni/ChatGPT Image 9 May 2026 16_44_52.png` (1774×887, 4×2)
- **Trans_F2F3** (8 tiles, 64×64) — `tiles_raw/yeni/ChatGPT Image 9 May 2026 16_44_55.png` (1774×887, 4×2)
- **W1** (16 tiles, 64×96) — `tiles_raw/w1_sheet_v2.png` (1024×1536, 4×4 dark stone bricks)
- **W2** (16 tiles, 64×96) — `tiles_raw/w2_sheet_v2.png` (1024×1536, 4×4)
- **OBW** (12 tiles, 64×128) — `tiles_raw/obw_sheet.png` (1024×1536, 4×3)
- All processed via `process_tiles.py` (current narrow magenta filter). Binary alpha fix pending.

### Unity Import
- `RIMA/Import Act1 Tiles` → Created: 12 (OBW new .asset files), Skipped: 108. All tiles valid.
- `RIMA/Fix Tile Sprites` → alreadyOk=108 (all sprites ≥32px, no stale sub-assets).

### Alabaster Dawn Tile Analysis (Opus)
- **Binary alpha rule**: Alpha must be 0 or 255 only — partial transparency from ChatGPT causes edge artifacts
- **Broader magenta tolerance**: R>200, B>200, G<60 minimum (HSV approach safer for stone tiles)
- **`_alt_N` naming**: use for atmospheric tile variants

### Pending
- **F1 new source sheet NEEDED**: User will provide clean magenta-background ChatGPT sheet. Run: `python STAGING/process_tiles.py --source <path> --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_`
- **F2 new source sheet NEEDED**: Same — `style_anchor_F2_floor.png` is a reference, not a proper magenta-bg source sheet.
- **process_tiles.py fix**: Broader magenta detection (R>200, B>200, G<60) + binary alpha snap to 0/255
- **Commit pending**: All tile PNGs + .meta + .asset files (large commit)

---

## S46 Evening Session (2026-05-09 — Yasin + Opus orkestra)

### Onaylanan Tasarım Kararları (MASTER #71 + 7 destekleyici, hepsi LOCKED v1)
- **Karar #71** — Silah hep elde / single-state, Ronin istisna (sheath/draw kimliği). Alabaster Dawn 3-state "puf" rejected — 2D pixel art teleport bug + scope kabul edilemez. Detay: `TASARIM/MAKEUP_BACKLOG.md` + `TASARIM/CINEMATIC_LAYER_v1.md`.
- **Karar #62-70** — S46 sabah dispatch (Map Fragment + Kırık Taş Tablet + Act 1 15-node + AD v1 sprint) MASTER_KARAR_BELGESI'ne eklendi.

### Yeni Dosyalar (4 yeni, hepsi NLM sync edildi)
- `TASARIM/MAKEUP_BACKLOG.md` — 8 eksiklik HIGH/MEDIUM/LOW (Hub rest pose, HP düşük yorgun idle, boss intro freeze-frame, run start ritual, death+Mühür sequence, skill draft drama, echo imprint reveal, NPC interaction zoom)
- `TASARIM/CINEMATIC_LAYER_v1.md` — 4 katman: A camera-driven, B environmental, C diegetic UI, D manuel cinematic frames (~30 frame total). Faz 2-5 dağıtım.
- `GUIDES/RIMA_MASTER_ART_PIPELINE.md` — Tek canonical art pipeline (Create Character Pro New → Animate with Text NEW v3 → Interpolate NEW → Edit Image Pro). Canvas: **128 native + 252 v3 render canvas** (v3 büyük canvas otomatik, weapon headroom için). Tier 2 abonelik kredi tahmini: ~750-1100 cred/sınıf, 10 sınıf ~4-5 ay.
- `TASARIM/FAZLAR/FAZ_DETAILED_SCOPE.md` — 8-haftalık Faz 1 detay + Faz 2-5 hafta-bütçe + sistem-faz matrisi + kritik patika (~10-12K kredit total).

### Güncellenenler
- `MASTER_KARAR_BELGESI.md` — Karar #71 ekleme + #29 override işareti + S46 sprint kararları
- `GDD.md` — STALE flag yumuşatıldı ("REFERENCE — S43+")
- `ANIMATION_REDESIGN.md` — "MOB-only" header (class anim Master Pipeline'a yönlendirildi)
- `TASARIM/FAZLAR/FAZ_MASTER.md` — S46 sprint kararları + 4 yeni satır (Map Fragment / Combat Feel AD v1 / Cinematic Layer / Asset Pipeline) + ASSET PIPELINE INTEGRATION bölümü
- `memory/` — 4 yeni feedback (user_profile.md, feedback_orchestrator_responsibility.md, feedback_design_originality_principle.md, feedback_nlm_login_automation.md)

### Test Pipeline (EditMode GREEN, PlayMode 4 fail kaldı)
**B8 fix:** RimaUITheme.MakeRoundedRect try-catch wrapper (EditMode-safe).
**B9 fix:** CharacterSelectScreen Awake → Start ayrımı + 3 testte reflection invoke.
**B10 fix (Codex):** EventSystem dup cleanup (FindObjectsByType + sceneLoaded hook), UIManager Time.timeScale=1f zorla, MainMenuScreen `_IsoGame`'de self-destroy, SkillOfferUI button name "Btn" + StopAllCoroutines, SettingsMenu hidden button rename.
- **EditMode: 148/148 PASS** ✅
- **PlayMode önceki 2 fail (timeScale=0 + DraftManager hide):** ÇÖZÜLDÜ ✅
- **PlayMode yeni 4 fail (B10 ile alakasız, room/enemy spawn):**
  1. `MultiRoom_ClearRoom1_NavigateThenClear` — oda boş (0 düşman)
  2. `RewardPickup_Interact_MarksCollected` — aynı sebep
  3. `LegacyRuntimeRoomManager_Room1Starts` — CurrentRoom=0 (1 olmalı)
  4. `RoomLoop_KillAllEnemies_RoomClears` — ilk oda boş
  - **Tahmin:** RoomLoader / DungeonWorldBuilder mob spawn flow eksik. CURRENT_STATUS önceki sürümünde "Pilot validation BLOCKED" notu vardı — bu görev hâlâ açık.

### Knowledge Base Temizlik
- 8 stale PixelLab dosyası → `ARCHIVE/SUPERSEDED/` (PIXELLAB_ENVIRONMENT_MODULE_NOTES, PIXELLAB_MOVEMENT_SHEET_AND_MAP_WORKSHOP_REVIEW, PIXELLAB_MAP_WORKSHOP_ISOMETRIC_USAGE_NOTE, PIXELLAB_RESEARCH_SYNTHESIS, ANIMATION_PRODUCTION_GUIDE, WARBLADE_ANIMATION_PIPELINE, ISOMETRIC_PRODUCTION_GUIDE, PIXELLAB_PROMPT_TEMPLATE)
- 6 Codex output artifact silindi (codex_b7/b8/b9/b10_*.md/.txt)
- `__pycache__/` silindi
- `STAGING/OVERNIGHT_REPORT.md` → `ARCHIVE/SESSION_REPORTS/OVERNIGHT_REPORT_2026-05-09.md`

### Knowledge Graph (graphify TASARIM)
- **490 nodes / 624 edges / 24 communities**
- God nodes: GDD (22 edges), Map+Item System (20), Class State Contract (18), Combat Roster (15), MASTER_KARAR (15)
- Surprising connections: Karar #18 → Map+Item (rationale), Hades 3-choice ↔ Faz 1, Pixel Art Constraint ↔ Animation Redesign
- Çıktı: `graphify-out/graph.html` + `GRAPH_REPORT.md`

### NLM Sync (8 dosya)
- Batch (5): MASTER_KARAR, GDD, ANIMATION_REDESIGN, MAKEUP_BACKLOG, CINEMATIC_LAYER_v1
- Manual subfolder (3): FAZ_MASTER, FAZ_DETAILED_SCOPE, RIMA_MASTER_ART_PIPELINE
- Stop hook memory dosyalarını otomatik sync edecek

### Lore Audit (rima-design + 8 NLM sorgu) — KEEP verdict
4 Act yapısı (Keep → Wastes → Core → Nexus) **olağanüstü tutarlı**. Lore-mekanik-boss-map UI 6 eksen kilitli. 3 alternatif (kale-only / element / spatial) hepsi REJECT.

### ⚠️ Güvenlik Bulgusu
`TASARIM/UI_CONCEPTS/` SVG dosyalarından birinde **prompt injection** tespit edildi (fake MCP Server Instructions). Agent reddetti. Kaynak muhtemelen ChatGPT export — UI_CONCEPTS klasörü manuel review önerilir.

---

## 🎮 Yeni Session Başlangıç Tercihi (LOCKED 2026-05-09)

- **Default model:** Sonnet 4.6 (kullanıcı seçer)
- **Opus 4.7 1M:** Sadece tasarım kararı / lore sentez / çelişki audit / kritik karar gerektiğinde — orchestra "Opus'a geç" önerir
- **Codex çıktısı QC zorunlu** — dispatch sonrası mantık check + gerekirse re-dispatch
- **Save-session pas geç** — sadece bu CURRENT_STATUS güncel tut

## ⏳ AÇIK İŞLER (Sıradaki Session İçin)

### Siradaki: Animasyon + PixelLab
- **Opus skill review sonuclarina gore animasyon oncelik sirasini belirle**
- **PixelLab W1 duvar testi** (Create Tileset Pro, Transition Height 1.0)
- **Ilk animasyon uretimine basla** (Animation Bible: 7 anim x 10 sinif)

### Siradaki: PixelLab Tile Uretimi
- **W1 duz duvar tile'lari** (16 variation, 64x96) — `STAGING/PIXELLAB_PROMPT_W1_WALL_v2.md` prompt hazir
- **F1 floor tile'lari yenile** (PixelLab, 16 variation, 64x64) — `STAGING/PIXELLAB_PROMPT_F1_FLOOR_v2.md` hazir
- `w1_conn` connector tile'lari random kullanımda duzgun gorunmuyor — duz duvar tile'lari gelince DemoRoomPainter duzeltilecek
- process_tiles.py komutu: `--cols 4 --rows 4 --width 64 --height 96 --prefix w1_`

### 🔴 v1 Sprint Öncesi Kritik Design Kararlar (Opus Review 2026-05-09)
- **DONE ✅ KARAR A — Boss Posture Reset Politikası:** Faz 1=700, Faz 2=850, Faz 3=1000 (LOCKED). Faz 3 yeni mekanik içermez.
- **DONE ✅ KARAR B — Summoner Soul Bond:** i-frame → Posture Guard %50 hasar azalt 1.2s (LOCKED). Identity Passive değişti.
- **DONE ✅ KARAR C — Hexer Stack Pressure ICD:** 4s ICD (LOCKED). Identity Passive değişti.
- **DONE ✅ — DAMAGE_CALCULATION.md:** `TASARIM/DAMAGE_CALCULATION.md` oluşturuldu (multiplier kategorileri + x3.0 cap).
- **DONE ✅ — MOB_COMPOSITION_RULES.md:** `TASARIM/MOB_COMPOSITION_RULES.md` oluşturuldu (M06+M04 yasak, M08+M04 yasak, M07 telegraph trainer TODO).
- **DONE ✅ — Shadowblade Scar Memory:** Pencere 2s → 1.2s (LOCKED kararı değil, rafine).

---

### Opus Mob/Boss Tasarım Review (2026-05-09)

#### Sprite Boyut Hiyerarşisi
Sprite boyut tablosu mob-spesifik versiyona güncellendi — detay: MEMORY/project_mob_boss_sizes.md

#### Yasin Onayi Bekleyen Kararlar (Yuksek Oncelik)
1. [x] LOCKED — The Witness mekanigi → gorus-hatti-bazli (oldur degil, sirtini don — buff her saniye katlanir ama sadece gorus hattindayken)
2. [x] LOCKED — Echo Twin Faz 2 → "Resonance Rift" firsati (iki kimlik carpısınca alan acilir, oyuncu icinde cross-class kullanabilir — zorunlu degil, odulli)
3. [x] LOCKED — Architect Faz 4 → mekanik gradient 4s/mekanik (hepsi ayni anda degil, 16s'de birikerek)
4. [x] LOCKED — Adaptive Posture Cap (faz 25s'den hizli bittiyse bir sonraki faz posture'u +%20)
5. [x] LOCKED — Final boss canavar boyutu: 96px, oyuncudan kucuk (Architect gercek form)

#### Yasin Onayi Bekleyen Kararlar (Orta Oncelik)
6. [x] LOCKED — M09 Toll Hound (Act 1 kovalayici — rota-takibi, 5s gecikme) Act 1 rostere EKLENDI
7. [x] LOCKED — M10 Vow Speaker (Act 1 skill yasakci — son 3s kullanilan skill'i 4s ICD ile cezalandirir) Act 1 rostere EKLENDI
8. [x] LOCKED — M04 aura %50 → %35 (Summoner/Brawler dengesi)
9. [x] LOCKED — M06 kalkan 3-stack cap (Gunslinger dengesi)
10. [x] LOCKED — Penitent Faz 1 "Kefaret" imza mekanigi (boss kendi posture'unun %20'sini kirar, pencere 2x uzar)

#### Yasin Onayi Bekleyen Kararlar (Dusuk Oncelik)
11. [x] LOCKED — M03 gorsel revizyon (yer alti → tile dikis cizgisi hareketi)
12. [x] LOCKED — M07 slow uzaklik-bazli (%30 yakin / %15 orta / %0 uzak)
13. [x] LOCKED — Mob posture bari UI'dan kaldir (sadece stagger aninda kirmizi titreme)

#### Act 1 Uygunluk Bulgulari
- Act 1 EKSIK: hiz/rota-baskisi tehdidi yok — M09/M10 onerileri bu boslugu dolduruyor
- The Witness mevcut tasarimi behavioral question sormuyor — revizyon KARAR bekliyor (yukarda #1)
- Carrion Weaver → Diablo Fallen Shaman benzerligi var; diferansiyasyon gerekiyor (backlog)

**2026-05-09 — Tum Opus kararlari Yasin tarafindan onaylandi (LOCKED)**
Extended fikirler: Opus'un ilerideki tasarim oturumlarinda proaktif oneri getirebilecegi alanlar acik birakildi — kilitlenen kararlari ihlal etmemek sartiyla.

Kilitlenen kararlar ozeti:
- The Witness → gorus-hatti-bazli buff (oldur degil, sirtini don)
- Echo Twin Faz 2 → Resonance Rift firsati (zorunlu degil, odulli)
- Architect Faz 4 → 4s/mekanik gradient (16s'de full)
- Adaptive Posture Cap → faz 25s altinda bittiyse +%20
- M09 Toll Hound + M10 Vow Speaker → Act 1 rostere EKLENDI
- M04 Anti-Heal aura → %50 → %35
- M06 kalkan → 3-stack cap
- M07 slow → uzaklik-bazli (%30/%15/%0)
- Penitent Faz 1 → "Kefaret" imza mekanigi
- M03 → tile dikis cizgisi gorsel revizyonu
- Mob posture bari → UI'dan kaldir (boss bari kalir)
- Carrion Weaver → diferansiyasyon gerekiyor (backlog)
- Architect gercek form → 96px, oyuncudan kucuk (LOCKED)
- Echo Twin cross-class trigger → Resonance Rift (iki kimlik carpısınca alan acilir)

---

### Yüksek Öncelik
0. **Tile pipeline — YENİ SHEETS ÜRETİLDİ, PROCESS + IMPORT BEKLİYOR (2026-05-09)**
   - 6 sheet üretildi: f1_sheet, f2_sheet, f3_sheet, w1_connector_set, w2_connector_set, decal_f1_sheet
   - Lokasyon: `STAGING/tiles_raw/yeni/`
   - Chromakey: #00FF00 (yeşil) — temiz çalıştı, pipeline onaylandı
   - QC PASS: F1/F2/F3 progression net, W1/W2 connector set tam (outer×2, inner×2, door arch, 2× endcap, variant), decal ×4 okunabilir
   - Dikkat: Wall tile'lar smooth 3D render görünümü — mevcut W1/W2 straight tile'larla Unity'de karşılaştır
   - **Aksiyon (sıradaki session):**
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f1_sheet.png --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_`
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f2_sheet.png --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_`
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/f3_sheet.png --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_`
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/w1_connector_set.png --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 96 --prefix w1_conn_`
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/w2_connector_set.png --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 96 --prefix w2_conn_`
     - `python STAGING/process_tiles.py --source STAGING/tiles_raw/yeni/decal_f1_sheet.png --output Assets/Art/Tiles/Act1/Decal --cols 2 --rows 2 --width 64 --height 64 --prefix decal_f1_`
     - Unity reimport → DemoRoomPainter görsel QC → W1 connector style uyumu kontrol
1. **DONE ✅ PlayMode 4 fail (room/enemy spawn)** — `#if UNITY_EDITOR` stub spawn + 2s timeout fix (Codex commits a5c261f + 78af4a9). LegacyRuntimeRoomManager.SpawnEnemies now creates TestEnemy_Stub gameobjects in editor/test builds.
2. **Lore REWORK 4 dosya** (lore audit raporu — Game Communication Requirement risk):
   - `TASARIM/STORY_RUN_PROGRESSION.md` (yeni) — 9-run NPC tanışma + lore drip tablosu
   - `TASARIM/HUB_DESIGN_v1.md` (yeni) — Hub mimari + 4 NPC yerleşimi + palet
   - 3-ending detay genişlet (KAL/KIR/TAŞI cutscene + NPC silme listesi — KIR için)
   - Lore drip pipeline (death recap'te run-by-run mini reveal — 9 satır mini-arc)
3. **Lore REVISE 4 küçük edit:**
   - Architect Faz 1 dialog netleştir ("İlk engelin bendim" → "Onları buraya ben yerleştirdim")
   - Sınıf seçim ekranı arketip disclaimer (Gunslinger ≠ kovboy, Ronin ≠ samuray)
   - Skill Taxonomy lore-bridge paragrafı (KEYSTONE/MODIFIER/RESONANCE = anılar metaforu)
   - Death recap "MÜHÜR" frame UX label

### Orta Öncelik
4. **Commit hazırlığı** — ~153 working tree dosyası, mantıklı gruplar (B8/B9/B10 fix, doc updates, archive moves)
5. **DONE ✅ B5.1 visual regression** — F1 tile sprites were 8×8 (stale sub-assets). Act1TileImporter.FixTileSprites patched to check dimensions (≥32px valid), stale sub-assets purged, 16 F1 tiles reimported as 64×64. Play mode verified: sampled=50 valid=50 bad=0.
6. **Map Fragment + DungeonGraph implementation** — spec hazır, kod görevleri Codex'e dispatch edilebilir

### Düşük Öncelik / Backlog
7. **MAKEUP_BACKLOG 8 eksiklik** (Hub rest pose, HP yorgun idle, boss intro, run start, death+Mühür, skill draft drama, echo imprint reveal, NPC zoom) — polish phase
8. **Cinematic Layer A/B/C/D** — Faz 2-5 dağıtımı

---

## Active Block (önceki içerik korunur)

## S47 Session (2026-05-09 — PixelLab + Animation + Design)

### PixelLab Pipeline — LOCKED (2026-05-09)
- **Tool haritasi kesinlesti**: Create Tiles Pro (floor), Create Tileset Pro+Transition Height 1.0 (wall), Create Tileset Standard+Upload Image (transitions), Create Image S-XL new (obstacles)
- **Duvar boyutu duzeltmesi**: 64x96 PixelLab'da yok -> **64x128** (Transition Full 32x64 -> 2x upscale)
- **Create Tileset Pro Transition Height=1.0**: 32px'den otomatik 23-tile kose+junction ailesi uretir -> 2x = 64x128 duvar tam boyutu
- **Create Tileset Standard Upload Image**: kendi tile'ini yukle -> Wang 16-tile gecis seti (F1->F2, F2->F3 icin mukemmel)
- **2x Upscale kurali**: nearest-neighbor, Unity Filter Mode=Point, Compression=None — kalite kaybi yok
- **Create Image S-XL (new)**: transparent bg ON, Low top-down (3D objeler), High top-down (flat objeler)
- **Shared palette Act 1 LOCKED**: #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575
- **Yeni dosyalar**: `GUIDES/PIXELLAB_PRODUCTION_GUIDE_v1.md` + `STAGING/PIXELLAB_PROMPT_FLOORS_v3.md` + `STAGING/PIXELLAB_PROMPT_WALLS_v3.md` + `STAGING/PIXELLAB_PROMPT_OBSTACLES_v1.md`
- NLM sync: PIXELLAB_PRODUCTION_GUIDE_v1.md done, ANIMATION_BIBLE.md done

### Animation Bible — LOCKED (2026-05-09, Opus karari)
- **View**: High top-down (~30-35 deg, Hades match) — tum assetler uniform
- **Yonler**: 4 yonlu + yatay ayna = 6 unique (N/NE/E/SE/S/SW), W=mirror E
- **Default facing**: South (kameraya dogru)
- **Oyuncu v1**: 7 anim (Idle/Walk/Attack_LMB/Attack_RMB/Dash/Hurt/Death) + Summoner Summon_Cast
- **Mob v1**: 4 anim (Walk/Attack/Hurt/Death), Elite +Telegraph, Boss 8 anim
- **Toplam v1 butce**: ~179 animasyon
- **Dosya**: `TASARIM/ANIMATION_BIBLE.md`

### Dungeon vs Open World — FINAL KARAR (LOCKED 2026-05-09, Opus+Codex+NLM)
- **Mimari KORUNUR**: prefab-per-room, duvar boundary var
- **Terrain obstacles (pillar/rubble/chasms) -> oda ICI content** olarak eklenir — boundary replacement degil
- **Genis odalar + ic objeler** = Hades modeli (kapali arena + dolu icerik)
- Duvar tile art sorunu -> PixelLab ile cozulur, mimari degismez
- NLM'den 9 kaynak bu karari destekledi

### PixelLab Tool Arastirmasi — Netlesti
- Create Tileset Standard/Pro max 32px (16x16 "not supported yet")
- Standard: Wang/dual-grid/3x3 export, Top-down veya Sidescroller, Upload Image terrain
- Pro: Shape Controls (preset sekil secici, angular dungeon icin), Transition Height 0-1.0+, Gemini kalitesi, 20 gen maliyet
- Create Image S-XL (new): View dropdown (High/Low top-down), Direction (None for objects), Outline, Detail, 32-768px square, transparent bg
- Prompt dosyalari: STAGING/PIXELLAB_PROMPT_*.md

### Skill System v2 — LOCKED (2026-05-09, Opus+Codex+Yasin sentezi)
8 LOCKED karar + Shadow Echo cross-class sistemi + final keybind. Detay: `TASARIM/SKILL_SYSTEM_v2.md`
- **Karar 1** Warblade Iron Verdict -> **Verdict Ledger** (3 stack -> herhangi skill Sunder, oda cikisi reset)
- **Karar 2** Ranger Distance Discipline -> **Range Bands** (<5 tile -%15, 5+ tile +%15+crit)
- **Karar 3** Ravager Carnage Pulse -> **Blood Tide** (hit->stack +%6, max 5, oda min 2 floor, refresh on hit)
- **Karar 4** Summoner Soul Bond -> **Necrotic Toll** (i-frame KAL + 3s pencerede skill +%30 AoE)
- **Karar 5** Z/X kaldirildi -> **Shadow Echo sistemi** (3 katman: aura+phantom+UI flash, pozisyon skill tipine gore)
- **Karar 6** Upgrade slot label-only Shape/Edge/Twist (zorla yok)
- **Karar 7** Resonance pasifi Altar selection (1/Act, ana guzergah, 3 secenek)
- **Karar 8** Ult Decay -%10/oda, floor -%40, gorunur HUD bar
- **Hold mekanik YASAK** v1'de (ActionCommitProfile ihlali)
- **Final Keybind**: WASD + LMB/RMB + Q/E/R/F + V (ult) + Space (dash) + G/M/C/Esc — toplam 8 combat input (Hades 2 seviyesi)
- **Shadow Echo havuzu**: 10 sinif x 5 = 50 Echo, ~30 mevcut skill reuse + ~20 yeni shadow-spesifik
- **Pozisyon kurali**: Melee->hedef, Ranged->player yani, Zone->cursor, Buff->player ustunde
- **Gorsel spek**: alpha 0.3, cyan #00FFCC tint, 0.4s sure
- **Animation onceligi**: Warblade -> Ronin -> Shadowblade -> Ranger -> Brawler -> GS -> Elem -> Ravager -> Hexer -> Summoner
- **Yeni dosyalar**: `TASARIM/SKILL_SYSTEM_v2.md` + `TASARIM/SHADOW_ECHO_MATRIX.md`
- **Memory**: `project_skill_system_v2_locked.md` + `project_shadow_echo_system.md`
- **NLM sync**: PENDING bu session sonu

### Open World vs Dungeon — Geri-Kontrol
Dungeon mimari KORUNUR (Hades-style closed arena + intra-room terrain obstacles). NLM'den 9 kaynak destekledi. Detay: bu sessionin alt boluumlerinde.

### Skill v2 NLM Audit — 6 Cakisma COZULDU (2026-05-09)
NLM tutarsizlik sorgusu 6 conflict tespit etti, hepsi cozuldu:
1. **Hold yasak vs Aim Shot:** Aim Shot TAP-MODE'a gecti (2-stage). `AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` UPDATED.
2. **Z/X kaldirilmasi vs Secondary Class:** Z/X **Secondary Class slotu** olarak korundu. Act 1=8 input, Act 2 boss sonrasi=10 input. Shadow Echo ayri sistem (Q/E/R/F evolution).
3. **Altar vs Map:** Altar = yeni Node 12.5 (pre-boss "Altar of Resonance", ana guzergah, atlanamaz). Map 15->16 node. dungeon_act1_map.md update sonraki sprint.
4. **Verdict Ledger vs Broken/Sundered:** Parallel paths (klasik Broken stack KORUNUR + Warblade fast lane Verdict Ledger).
5. **Blood Tide vs Bleed:** Stack'e +%5 Bleed tick eklendi (cross-class Hexer/Brawler bleed sinerji korundu).
6. **Cyan Shadow vs Violet Shadowblade Echo:** Renk dili (cyan=cross-class, violet=Shadowblade kendi).
**Sonuc:** 8 LOCKED karar tutarsiz degil — eski dokumanlarla harmonize edildi. Detay: `TASARIM/SKILL_SYSTEM_v2.md` "CONFLICT RESOLUTIONS" bolumu.

### Acik Isler (Siradaki)
- **Opus skill review sonucu** -> Animation oncelik listesi gelecek
- **PixelLab'da W1 duvar testi**: Create Tileset Pro -> Transition Height 1.0 -> F1 floor style ref yukle -> 4 var test
- **Animasyonlara baslama**: Animation Bible'a gore, Opus oncelik sirasina gore

---

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
- PixelLab üretim limiti: hareketli max 256×256px (confirmed), statik max 512×512px. Tüm moblar mob-spesifik boyutlarla üretilecek, bosslarda Unity scale kullanılacak.
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
- **Tile pipeline CLEAN**: 0 chromakey artigi (G>R+B hard removal + spill suppression — two-pass LOCKED)
- **Unity IsometricZAsY**: gaps giderildi, player Entities layer'inda, duvarlar Walls layer'inda
- **Tile envanteri**: F1/F2/F3 (16 each), W1_conn/W2_conn (8 each), Decal (4), OBW (12), Trans (8+8)
- **PixelLab prompts**: STAGING/PIXELLAB_PROMPT_F1_FLOOR_v2.md + STAGING/PIXELLAB_PROMPT_W1_WALL_v2.md hazir
- EditMode: 148/148 PASS. PlayMode: 4 fail kaldi (room/enemy spawn — ayri gorev).
- Performance: CPU frame time 99ms -> 0.11ms after deep-fix pass.

## Current Risks
- **Act1TileImporter DONE** — F3/Trans/W1/W2/OBW tüm tile'lar import edildi (alreadyOk=108, Created=12). Tile sprite'ları geçerli.
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
- **Tile pipeline PROCESS+IMPORT PENDING** — 6 yeni sheet STAGING/tiles_raw/yeni/ içinde hazır. process_tiles.py + Unity reimport + DemoRoomPainter QC gerekli.
- **process_tiles.py binary alpha fix pending** — Current magenta filter too narrow; partial alpha from ChatGPT causes edge artifacts. Fix: R>200, B>200, G<60 + snap alpha to 0/255.
- **F3/Trans tile QC pending** — sliced from non-standard ChatGPT dimensions (1254×1254, 1774×887); visual QC in Unity needed.
- **RRM tile painting bagimliliklar** -- Task B'de soküm yapilacak; simdilik paralel calisiyor
- **Tile commit pending** — F3/Trans/W1/W2/OBW PNGs + .meta + .asset files not yet committed this session.

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
