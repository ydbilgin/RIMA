# Wall Production v2 — FINAL Web UI Batch Plan

> **Tarih:** 2026-05-22 (S99 LATE)
> **Mode:** User web UI manual production (HARD RULE: no autonomous PixelLab gen).
> **Hedef açı:** Hades-iso ~70-75° (PixelLab `view: high top-down`).
> **Strateji:** Option A — Create Object n_frames=4 + flipX mirror (state pipeline ban).

---

## TL;DR — sadece bunu okumak yeterli

**4 web UI call yap, ~100-140 gen harca, sonra haber ver.** Detay aşağıda.

| # | Call | Canvas | Sprite | Neden |
|---|---|---:|---|---|
| 1 | W01 wall_straight_n RE-ROLL | 96×96 | back wall | mevcut wall_n_v1 height 64 → corner 96 ile uyumsuz, yeniden 96×96 üret |
| 2 | W07 corner_SE | 96×96 | bottom-right corner | KRİTİK (W08 SW flipX için kaynak) |
| 3 | W09 wall_collapsed_stub | 96×96 | mid-room blocker | combat cover |
| 4 | W10 wall_archway_n | 128×128 | transition portal | hero piece (veya Codex 02cee97d kullan, skip) |

**Free flip (üretim YOK):** W04 E ← W03 flipX, W06 NW ← W05 flipX, W08 SW ← W07 flipX.
**Deferred:** W02 wall_short_edge_s → sonraki 64px tileset batch (16-item pack).
**Keep from Batch 1:** W03 wall_w_v1 + W05 corner_NE_v1.

---

## 1. Final wall envanteri (10 library entry)

| ID | Asset adı | Kaynak | Canvas | Status |
|---|---|---|---:|---|
| W01 | wall_straight_n | **RE-ROLL** (mevcut wall_n_v1 height mismatch) | 96×96 | 🔴 PRODUCE Call 1 |
| W02 | wall_short_edge_s | Sonraki 64px tileset batch | 64×64 | ⏸️ DEFERRED |
| W03 | wall_straight_w | Batch 1 `wall_w_v1.png` | 64×96 | ✅ KEEP |
| W04 | wall_straight_e | W03 + SpriteRenderer.flipX | — | ✅ FREE |
| W05 | corner_NE | Batch 1 `corner_NE_v1.png` | 96×96 | ✅ KEEP |
| W06 | corner_NW | W05 + SpriteRenderer.flipX | — | ✅ FREE |
| W07 | corner_SE | **YENİ ÜRET** | 96×96 | 🔴 PRODUCE Call 2 |
| W08 | corner_SW | W07 + SpriteRenderer.flipX | — | ✅ FREE |
| W09 | wall_collapsed_stub | **YENİ ÜRET** | 96×96 | 🔴 PRODUCE Call 3 |
| W10 | wall_archway_n | **YENİ ÜRET** veya Codex `02cee97d` | 128×128 | 🔴 PRODUCE Call 4 / OPSIYON |

**Discard:** `wall_s_v1.png` (Batch 1 — N kopyası, foreground silhouette yanlış).

---

## 2. Web UI ortak ayarlar (TÜM call'larda aynı)

PixelLab web UI: https://pixellab.ai/app/create → **Create Object** tool.

| Field | Value |
|---|---|
| Tool | Create Object |
| Direction | 1 (static) |
| **n_frames** | **4** (4 candidate al — Strateji B: hepsini KEEP, variant olarak slice) |
| **Image size** | **96** (Call 1-3 için), **128** (Call 4 archway) |
| View | **High top-down** (~30-35° slider = Hades-iso match) |
| Object view | Top-down |
| Background | Transparent / No background |
| Outline | None |
| **Style reference image** | **`Assets/Art/Walls/Act1_ShatteredKeep/corner_NE_v1.png`** (TÜM 4 call'da aynı — style chain pin) |

### Strateji B — 4 candidate'ı variant olarak KEEP

n_frames=4 → output strip (256×96 veya 512×128). 4 candidate'ın hepsi benzer kalitede çıkarsa:
- 1'ini base PNG yap (örn `corner_SE_v1.png`)
- Kalan 3'ü `corner_SE_v2.png`, `_v3.png`, `_v4.png` olarak kaydet (random material variation için Unity'de pool'a düşer)
- Eğer 4 candidate'tan sadece 1-2'si OK, kalanlar bozuksa sadece OK olanları al

Bu strateji 4× bedava variant kazandırır → odalarda tile repetition kırılır.

### Universal style prefix (her prompt'tan ÖNCE yapıştır)

```
Pixel art dungeon wall asset, RIMA Act 1 Shattered Keep aesthetic.
View: Hades-iso top-down at ~70-75 degree tilt (high top-down).
The wall MUST show BOTH its TOP masonry surface (from above) AND its
FRONT-FACE stone surface (player-facing vertical face) — NOT pure flat
top-down 90 degrees, NOT side-view 0 degrees.
Style: dark weathered stone block construction, painterly pixel art,
moss creep at base, hairline cracks, gritty palette base RGB(42,45,52),
subtle cyan rift #5DEFFF accent hint (sparse, not dominant), torch warm
glow ambient implied. Light source from above (top-front).
NO outline frame, NO black borders, NO UI, NO character, NO weapon,
transparent background.
```

---

## 3. Prompts — 4 call

### CALL 1 — W01 `wall_straight_n` RE-ROLL

**Filename:** `Assets/Art/Walls/Act1_ShatteredKeep/wall_n_v2.png`
**Canvas:** 96×96 | **n_frames:** 4

```
[Universal style prefix]

NORTH WALL STRAIGHT TILE — the BACK wall of a dungeon room (the wall
behind the player when player faces south/down). Hades-iso 70-75° tilt
strict requirements:
- TOP edge of the sprite shows the upper masonry SURFACE (about top
  25-30% of sprite — player sees the top of the wall from above)
- BOTTOM 70-75% of sprite shows the wall's FRONT FACE material
  (player-facing stone blocks with clear vertical presence)
- NOT a flat horizontal strip — clear vertical wall mass required
- Tile-mate left-right edges: seamless side-by-side placement (4-6
  instances of this sprite will be repeated along the entire north
  room boundary, so left edge pattern must continue into right edge)
Same dark weathered stone block construction as existing W wall and
corner_NE references. This is the MOST REPEATED wall tile in any room.
```

---

### CALL 2 — W07 `corner_SE` 🔴 KRİTİK

**Filename:** `Assets/Art/Walls/Act1_ShatteredKeep/corner_SE_v1.png`
**Canvas:** 96×96 | **n_frames:** 4

```
[Universal style prefix]

SOUTHEAST OUTER CORNER — bottom-right corner of a dungeon room where
the SOUTH wall and EAST wall meet at an outer turn. Anatomy:
- SOUTH face on the LEFT-BOTTOM of the sprite, exposed toward camera
  (front-facing perspective, player sees this directly)
- EAST face on the TOP-RIGHT of the sprite, fading right with side
  perspective
- Masonry blocks turn around the corner angle (continuous stone)
- Same dark weathered stone style and palette as existing NE corner
  reference (Batch 1 `corner_NE_v1.png`)
- Edge-mate: bottom-left edge connects to SOUTH wall straight tile,
  top-right edge connects to EAST wall straight tile (W03 flipX)
The corner block is the focal point — visible mass should anchor the
foreground-right of the room.
```

---

### CALL 3 — W09 `wall_collapsed_stub`

**Filename:** `Assets/Art/Walls/Act1_ShatteredKeep/collapsed_stub_v1.png`
**Canvas:** 96×96 | **n_frames:** 4

```
[Universal style prefix]

COLLAPSED WALL STUB — a partial ruined wall blocker that sits in the
MIDDLE of a dungeon room (NOT on perimeter, NOT tile-mate). Combat
cover / line-of-sight blocker. Properties:
- Roughly 60-70% the height of intact perimeter walls (mid-height)
- Jagged broken top profile where the wall has crumbled
- Visible front-face stone (player-facing) and a bit of top masonry
- Significant rubble base spreading outward at the bottom (loose
  stones scattered on the floor around the base)
- Asymmetric silhouette — NOT a clean wall segment
- Free-standing — does NOT need to tile-mate to other walls
- Same dark weathered stone style as existing N/W walls
Single hero blocker piece, used 1-3 per room placement.
n_frames=4 will give 4 ruin pattern variants — Strateji B: keep all 4
as v1-v4 for random room placement (asymmetric variation valuable).
```

---

### CALL 4 — W10 `wall_archway_n` 🔴 / OPSIYON

**ALTERNATIF:** Codex'in ürettiği archway state output `02cee97d-0c81-4a2c-9735-34df9127b9f9` PixelLab cloud'da var. Önce indir + incele — kabul edilebilirse Call 4'ü SKIP.

**Filename:** `Assets/Art/Walls/Act1_ShatteredKeep/archway_n_v1.png`
**Canvas:** 128×128 | **n_frames:** 4

```
[Universal style prefix]

DRAMATIC STONE ARCHWAY PORTAL — entry/transition piece between dungeon
rooms, north-facing orientation (player walks through from south to
north). Anatomy:
- Two thick stone pillars flanking an open gap (each pillar shows
  masonry blocks + weathered stone matching adjacent wall style)
- Curved stone arch top connecting the two pillars
- Central keystone at the top of the arch with a faint cyan rift
  #5DEFFF glow embedded (subtle hint, not overwhelming)
- The arch opening is the focal area — wide enough for a 64px chibi
  character to walk through (centered gap under the arch)
- Pillars sit on a small stone base (1-2 block deep)
- Hades-iso ~70-75° tilt: top of pillars and arch keystone show upper
  masonry surface partially (top-down perspective hint)
- Same dark weathered stone style and palette as N/W walls and corners
The cyan keystone glow is the ONLY color accent — everything else stays
in the dark gritty palette.
```

---

## 4. Per-call workflow (her sprite için)

1. https://pixellab.ai/app/create → "Create Object" tool seç
2. Universal style prefix + spesifik prompt yapıştır
3. **Image size:** 96 (Call 1-3) veya 128 (Call 4)
4. **n_frames:** 4
5. View slider: "High top-down" (~30-35°)
6. Background: Transparent
7. **Style reference upload:** `Assets/Art/Walls/Act1_ShatteredKeep/corner_NE_v1.png` (TÜM call'larda aynı)
8. Generate
9. Output strip'i incele:
   - 4 candidate başarılıysa hepsini al → 4 farklı v1/v2/v3/v4 dosyası
   - 1-2'si OK kalanlar bozuksa sadece OK olanları al
   - Hepsi bozuksa re-roll (style ref farklı bir Batch 1 sprite ile dene)
10. Download → her variant'ı target filename ile kaydet
11. `Assets/Art/Walls/Act1_ShatteredKeep/` klasörüne koy
12. Sonraki call'a geç

---

## 5. Modüler bağlama — Unity'de nasıl çalışacak

### Pivot + import (her PNG için)
- PPU 64
- Filter Mode: Point (no filter)
- Compression: None
- Pivot: **Custom (0.5, 0.0) = bottom-center** (Y-sort için zorunlu)
- Sprite Mode: Single

### Modular tile placement (3D LEGO benzeri)

| Bağlantı | Çalışır mı? | Risk |
|---|---|---|
| W01 ↔ W01 yan yana (N row repeat) | ✅ pixel-identical | YOK |
| W01 ↔ W05 corner_NE (N row sonu) | ✅ style chain match | DÜŞÜK |
| W05 ↔ W06 flipX (NE↔NW mirror) | ✅ simetrik | DÜŞÜK |
| W03 ↔ W04 flipX (W wall ↔ E wall) | ✅ simetrik | DÜŞÜK |
| W07 ↔ W08 flipX (SE↔SW) | ✅ simetrik | DÜŞÜK |
| W10 archway ↔ W01 N (N row'a yerleşim) | ⚠️ asimetrik silhouette | ORTA |
| W09 stub ↔ floor (mid-room) | ✅ free-standing | YOK |

### Seam gizleme stratejisi (Hades approach)

AI gen tile-mate **asla pixel-perfect değil** — seam'lerde 1-3px stone block kayması olur. Çözüm:

| Seam noktası | Prop overlay |
|---|---|
| N row W01 her 2-3 instance | **P01 column** veya **P04 wall_torch** yerleştir → seam'i göz algılamaz |
| Köşeler (W05/W07 mate) | **P05 floor_brazier** köşe yanına → corner mate gizlenir |
| W10 archway ↔ W01 birleşim | **P03 tattered_banner** archway yanına |

Worst seam'ler için sonradan **PixelLab Inpaint** ile per-room düzeltme (sadece 2-3 seam için, ~5-10 gen each).

---

## 6. Composition test (production sonrası tek oda kanıtı)

8×6 cell test odası (64px PPU):

```
W05 W01 W01 W10 W10 W01 W01 W06         (N row: corner_NE + 2×W01 + archway + 2×W01 + corner_NW[flipX])
W03                              W04     (W↔E side walls, W04 = W03 flipX)
W03               W09            W04     (W09 mid-room collapsed_stub)
W03                              W04
W03                              W04
W07 (W02) (W02) (W02) (W02) (W02) W08   (S row: corner_SE + 5×W02[DEFERRED edge_lip] + corner_SW[flipX])
```

Bu kompozisyon doğrularsa modular bağlama PASS. W02 deferred olduğu için S row test'i sonraki batch'te yapılır.

---

## 7. Üretim sonrası — Codex Unity import dispatch

User 4 call bittiğinde orchestrator'a bildir. Codex task dispatch edilecek:

1. PNG'leri Unity import (PPU 64, Point, No compression, bottom-center pivot)
2. Her base + variant için Prefab build (`Assets/Prefabs/Walls/Act1_ShatteredKeep/`)
   - SpriteRenderer + BoxCollider2D (collider size content bbox'a göre)
   - W04/W06/W08 prefab'ları flipX=true (no extra PNG)
3. `WallPrefabRegistry_Act1.asset` güncelle — 9 entry (W01, W03-W10) + W02 deferred
4. `Assets/Scenes/Demo/TopDownTest_Map1.unity` composition test odası kur
5. Screenshot + verdict raporu

---

## 8. Budget özeti

| Call | Tahmini gen |
|---|---:|
| Call 1 W01 N re-roll | 25-35 |
| Call 2 W07 corner_SE | 25-35 |
| Call 3 W09 collapsed_stub | 25-35 |
| Call 4 W10 archway (skip if 02cee97d) | 0-35 |
| **Toplam** | **~75-140 gen** |

Mevcut bütçe ~2050-2100/5000 → kalan ~1910-2025/5000 (rahat).

---

## 9. Strateji B notu — "4'ün hepsi variant"

n_frames=4 → çıktı 4-frame strip. Her frame potansiyel bağımsız variant:
- W01 N: 4 frame KEEP → odada random N tile placement (repetition kırılır)
- W09 stub: 4 frame KEEP → asimetrik ruin variant 4× artar (HIGHLY recommended)
- W07 corner_SE: 1 KEEP yeterli (corner variant az değerli, simetrik kullanım)
- W10 archway: 1 KEEP (hero piece, en iyi olan)

Bu strateji 4× bedava material variation kazandırır, ek gen yok.
