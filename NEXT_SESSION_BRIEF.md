# NEXT SESSION BRIEF — RIMA (2026-05-23 gece geç)

**Read this FIRST when starting next session in RIMA project.**

## TL;DR

S103 gece: HD-2D Hybrid pivotu (RIMA_HD2D) **geri çekildi**. Mevcut RIMA 2D combat sistemi korunuyor. RIMA_HD2D'den değerli asset/script/plan'lar RIMA'ya **transfer edildi**. RIMA cleanup'ı **kısmen yapıldı** (Library/Logs/Temp Unity açık olduğu için silinemedi — sen yeni session'da Unity kapatıp tekrar dene). 5 hard decision (Q1-Q5) seni bekliyor — sonra Codex Phase D-G dispatch.

## ⚡ İlk yapılacaklar (sırasıyla)

1. **Unity RIMA'yı kapat** (eğer açıksa)
2. Bu briefi oku
3. `STAGING/rima_transfer_plan.md` Section 6 — **5 karar verilecek soru** (Q1-Q5)
4. `STAGING/rima_cleanup_plan.md` Section 1 — **3 REVIEW item** (ARCHIVE/STAGING_OLD, RIMA_AssetParts v2/v3, PIXELLAB_OUTPUTS)
5. Library/Logs/Temp eğer hala duruyorsa sil:
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Library"
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Logs"
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Temp" -ErrorAction SilentlyContinue
```
6. Unity'i RIMA'da aç — Library regen (~15-20 dk). Console'a bak, error varsa raporla
7. Q1-Q5 cevapla → Codex Phase D-G dispatch eder

## Bu gece ne yapıldı (S103 özet)

### RIMA_HD2D'de (alternatif proje, archive olarak duruyor)
HD-2D Hybrid yapıldı, 5 commit (Phase 1-4a):
- b502fb79 — 6 texture + 5 prefab + 6 material + Room_Sample
- dd6cf34 — RoomFootprint+ShellBuilder+WallModuleLibrary + 7 footprint + 2 corner
- d926f735 — URP volume + 4 wall lib + 3 wall variant + 2 light prefab + atmosphere
- 56dab735 — CameraLockController + 11 irregular footprint + camera lock 10 scene
- 89b9a737 — "Make It Glow": scene light placement + pillar offset + sigil emission + bloom

**Pivot sebebi:** chatgpt_ref aslında 2D fake-iso art. HD-2D 3D mimari overkill. RIMA'da combat zaten 2D + çalışıyor. PlayerController port'una gerek yok.

### RIMA'ya transfer edilenler
- ✅ `RoomDecorationSpawner.cs` Random ambigüite fix (line 20 + 54)
- ✅ 6 PNG texture (`Assets/Art/Environment/Walls,Floor/`)
- ✅ 11 RoomFootprint .asset (`Assets/Data/Environment/Footprints/`)
- ✅ 2 script: `RoomFootprint.cs`, `WallModuleLibrary.cs` (3D referansı yok, direkt çalışır)
- ✅ 1 MEMORY: `feedback_camera_lock_hd2d.md`
- ✅ 11 STAGING aktif doc (architecture_decision, procgen_*, opus_gap_analysis, phase4/5 plans, vs.)
- ✅ 15 archive doc → `STAGING/archive_hd2d/` (referans için)
- ✅ `rima_cleanup_plan.md`, `rima_transfer_plan.md`

### RIMA cleanup yapıldı (partial)
**Moved to SCRATCH** (`F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/`):
- twitter_research (464 MB)
- tweet_research (83 MB)
- CHATGPTSPRITESHEETS (24 MB)
- _reference_packs_raw (15 MB)

**Deleted:**
- boona_frames (20 MB)
- _DISCARDED_codex_v1_* subfolder
- tmp/ (33 MB)
- Temp/

**FAIL (Unity açıktı, file lock):**
- Library/ (8.1 GB) — sen Unity kapatıp tekrar sil
- Logs/ (8.7 MB) — sen sil
- (Bunları silmeden Unity reimport hızlı olmaz)

**Disk kazancı:** ~640 MB (Library + Logs eklenince ~8.7 GB)

## 5 KARAR VERİLECEK SORU (Q1-Q5)

### Q1 — `RoomFootprint.cellSize` unit ölçek
HD2D'de `2f` (3D units). RIMA 2D PPU=64, 1 unit = 64px = 1 tile. Seçim:
- (A) Keep `2f` → 12-cell oda = 24 Unity unit
- (B) Change to `1f` → 12-cell oda = 12 Unity unit (tile = 1 unit, daha kontrollü)
- **Öneri:** B (PPU lock + okunabilirlik için)

### Q2 — West-wall rotation
HD2D `Quaternion.Euler(0,90,0)` rotate. 2D top-down'da Y-rotate yok.
- (A) Ayrı west sprite (2 PNG ekstra)
- (B) Sprite flipX (free)
- (C) Tek sprite N+W (Hammerwatch style)
- **Öneri:** C MVP, ileride B

### Q3 — Camera lock değerleri 2D
HD2D `(12,8,-12)` rot `(35,315,0)` ortho 9 — invalid 2D'de. Öneri:
- Position `(0, 0, -10)`, Rotation `(0,0,0)`, OrthoSize `~8` (12-cell oda + padding)
- User confirm

### Q4 — Corner/variant prefab day-1
NE_OuterCorner, NW_InnerCorner, Breach, Toppled, Heavy için HD2D'de PNG yok (3D mesh shape kullanılmış). Seçim:
- (A) Skip slots (null) — incomplete
- (B) `stone_wall_b_cracked.png` placeholder hepsine
- (C) PixelLab batch FIRST
- **Öneri:** B day-1, sonra PixelLab batch

### Q5 — Room system merge?
RIMA'nın eski `RoomBuilder.cs`, `RoomBlueprint.cs`, `RoomTemplate.cs` var (Assets/Scripts/...). RIMA_HD2D'den gelen `RoomFootprint.cs` + `WallModuleLibrary.cs` paralel sistem. Seçim:
- Eskileri tut + yeniyi yanına koy (parallel)
- Eskileri sil, yeniyi kullan
- Merge et
- **Öneri:** Eski sistemleri keşfedip karşılaştır, then decide

## 3 CLEANUP REVIEW (cleanup_plan.md Section 1)

1. **ARCHIVE/STAGING_OLD/** (~400 MB Ranger anim PNG'ler) — SCRATCH'a taşı mı?
2. **RIMA_AssetParts_v2/v3/** — Assets/'a import edildi mi, STAGING redundant?
3. **PIXELLAB_OUTPUTS/** — SCRATCH'a (history) mı projede mi?

## Önemli paths

| Path | İçerik |
|---|---|
| `STAGING/rima_transfer_plan.md` | Transfer detayları + Q1-Q5 + Section 5 phase D-G order |
| `STAGING/rima_cleanup_plan.md` | Cleanup detayları + 3 REVIEW question |
| `STAGING/opus_gap_analysis.md` | ChatGPT_ref karşı eksikler + roadmap (Opus deep) |
| `STAGING/phase4_decor_plan.md` | 18 decor item + RoomDecorator spec |
| `STAGING/phase5_room_connector_plan.md` | RoomConnector + chain builder |
| `STAGING/architecture_decision.md` | Option C Template+Decor (RIMA core arch) |
| `STAGING/procgen_design_verdict.md` | L1 random-walk dungeon algo |
| `STAGING/concepts/chatgpt_ref/` | 8 PNG visual target (atmosfer ref) |
| `STAGING/archive_hd2d/` | HD2D Phase 1-4a task specs (referans/history) |
| `MEMORY/feedback_camera_lock_hd2d.md` | Camera lock pattern (2D adapt lazım) |
| `RIMA_HD2D/` (parent project kuzeni) | Alternatif HD-2D 3D yapı, archive olarak duruyor — silmem |

## Mimari hatırlatma — RIMA 2D

- **Combat:** mevcut PlayerController.cs (Rigidbody2D, dash + combat aim + facing + status effects), PlayerAnimator (4-way diagonal), PlayerAttack — **HAZIR + ÇALIŞIYOR**
- **Rendering:** URP 2D Renderer + Light2D + sprites in XY plane
- **PPU:** 64 (Karar #100 chibi 64px characters)
- **Camera:** orthographic, Z=-10, rot=(0,0,0), orthoSize=~8 (Q3'te netleşir)
- **Z-sorting:** sortingOrder by Y position (2D standart)
- **PixelLab:** `create_tiles_pro` (Discord notu, full tileset batch, single tile değil)

## Yaygın hatırlatmalar

- **Codex via cx_dispatch.py background**, orchestrator bulk yazmaz
- **NLM:** `30ddffa5-292f-4248-8e77-68074af901be` — design context burada
- **Karar #100 chibi 64px LOCKED** — character pivot ayak tabanı, sprite üst y'da
- **Camera lock HARD RULE** — CameraLockController2D adapt edildiğinde dokunulmaz

---

**Sessionun başlangıcında: bu briefi oku → Q1-Q5 cevapla → Library/Logs sil → Unity aç → Codex Phase D-G dispatch ederim.**
