# Object Üretim Master Spec — v2 LIVE (2 iter Codex review tamamlandı)

## Verdict
**LIVE** — Codex Iter 2 PASS_WITH_MINOR_REVISIONS, no Iter 3 required. Spec, ilk pilot dispatch için hazır (Template A v2, wall face 4-piece, 25-40 gen reserve 40).

## Iter Log
- **v1:** Opus draft (9 karar, prompt templates) → Codex Iter 1 fired
- **Codex Iter 1 verdict:** PASS_WITH_REVISIONS — 4 karar LIVE (2, 3, 7, 8), 5 NEEDS_REVISION (1, 4, 5, 6, 9), 0 blocker
- **v2:** Opus revize — `item_descriptions` API entegre, L2a/L2b same-dispatch claim çıkarıldı, state_of % limit heuristic'e indirildi, object_view+side blend pilot-gated yapıldı, bütçe range'lere çekildi
- **Codex Iter 2 verdict:** PASS_WITH_MINOR_REVISIONS — 5 karar RESOLVED, item_descriptions VALID, pilot gates ACTIONABLE, range budget ACTIONABLE, Open Question #5 ve #7 cevaplı (aşağıda entegre edildi), no Iter 3
- **v2 LIVE inline edits:** Açık Soru #5 ve #7 Codex cevaplarıyla resolve edildi, dispatch-path caveat eklendi

---

## Master Kararlar (v2)

### 1. L2a Wall Base Tool

**KARAR v2: Pilot-gated, iki yol açık. Default: `create_object` 128 view="low top-down" tek dispatch (n_frames=1 veya 4). Fallback: `create_isometric_tile` size=64 thin tile.**

**Codex feedback entegrasyonu:** L2a + L2b aynı dispatch'te bundle iddiası geçersiz çünkü `view` parametresi tek bir değer alır — L2a "low top-down", L2b "side" istiyor. Same-dispatch stil bundle argümanı çürük.

| Aday | Verdict | Sebep |
|---|---|---|
| `create_object` 128 view="low top-down" n_frames=1 | **DEFAULT PILOT** | Ref 65c99904 ispatlı flat block + 4×4 stone. L2a runtime'da L2b overlay'in altında — görsel önemli değil, footprint kritik. Tek piece yeterli. |
| `create_isometric_tile` size=64 thin tile | **FALLBACK** | Eğer L2a runtime'da partially visible (yarı saydam wall) gereği çıkarsa, gerçek izometrik footprint için bu yol daha doğru. Size 64 sınırı L2b 128 ile mismatch yapar — pivot Unity'de ayarlanır. |
| `create_object` 128 n_frames=4 batch | **REJECT (bu karar için)** | L2b ile aynı dispatch impossible. L2a tek piece yeter, n_frames=4 buffer drift riski. |

**Karar gate:**
- L2a sprite ASLA runtime'da görülecek mi? → User cevabı bekleniyor (Açık Soru #1).
- Hayır → `create_object` 128 single dispatch
- Evet → `create_isometric_tile` 64 thin tile + pivot ayarı

**Bu spec varsayımı:** L2a runtime'da görünmez (collider source). Default seçim `create_object` 128.

---

### 2. L2b Wall Face Tool

**KARAR v2: `create_object` size=128 SQUARE view="side" directions=1 n_frames=4 + `item_descriptions` ile 4-piece batch. PILOT GATE: ilk batch sonrası stil tutarlılığı doğrulanmalı.**

**Codex feedback entegrasyonu:** Karar LIVE ama prompt'a "tall vertical wall billboard, sprite fills canvas height" eklenmeli. `create_tiles_pro` 64×128 yolu fallback olarak kalır, low-cost test ile karşılaştırılır.

| Aday | Verdict | Sebep |
|---|---|---|
| **A. `create_object` 128 side n_frames=4 + item_descriptions** | **DEFAULT PILOT** | API-valid, item_descriptions per-frame control, stil bundle 4 piece tek dispatch. Prompt'a canvas-fill cue eklenir. |
| **B. `create_tiles_pro` tile_size=64 tile_height=128 tile_view_angle=0** | **FALLBACK / A/B TEST CANDIDATE** | Non-square 64×128 API-valid. Ama "tile" kategorisinde çıkma riski (billboard vs tile görselliği). Bir düşük cost test worth. |
| Hibrit (face → tiles_pro, corner → object) | **REJECT** | İki tool = iki stil dispatch = drift. |

**Codex önerilen ek prompt cue (entegre edildi Karar #7'ye):**
> "tall vertical wall billboard, sprite fills most of the 128px canvas height, narrow transparent margins"

**Pilot karar gate:**
- İlk 4-piece batch fire (Template A v2)
- `get_object` → 4 candidate inline preview
- 4 piece stil tutarlı + dikey occupy → A LOCK
- Stil drift / tile görselliği → B test (tiles_pro 64×128) low cost dispatch ile karşılaştır
- Her ikisi de zayıf → Plan C: 4 ayrı `create_object` 128 n_frames=1 dispatch (60 gen cost ama %100 control)

---

### 3. Size × n_frames Stratejisi

**KARAR v2:** Codex confirmed natural threshold'ları — kullanıcının matrix'i doğru, sadece "off-natural" use case'leri "drift-prone unless tested" olarak işaretlenir.

| Object size | n_frames seçimleri (legal) | **Önerilen (natural)** | Toplam canvas | Use case |
|---|---|---|---|---|
| 32 | 1, 4, 16, 64 | **64** (8×8) | 256×256 | Floor clutter (potion, key, gem, coin, rune, candle_stub, blood_splatter, small_skull) |
| 64 | 1, 4, 16, 64 (legal ama off-natural) | **16** (4×4) | 256×256 | Orta items + mob silhouette (chest, barrel, small_statue, void_flame_torch, rubble_sm) |
| 128 | 1, 4, 16 (legal ama off-natural) | **4** (2×2) | 256×256 | Büyük items (wall_face, throne, altar, archway, sarcophagus, rubble_lg) |
| 256 | 1 | **1** | 256×256 | Boss prop / landmark tek piece (no review mode) |

**Codex'ten doğrulanan natural threshold'lar (api source):**
- ≤42px → 64 frames
- ≤85px → 16 frames
- ≤170px → 4 frames
- > 170px → 1 frame

**Off-natural use:** 64×64 n_frames=64 *legal* ama API natural thresh 16. Drift-prone unless tested. Production'da natural değer kullan.

**Düzeltme v1'den:** "n_frames=8 YOK" notu doğru — enum sadece [1, 4, 16, 64].

---

### 4. Grouping Kuralı (Numbered Prompt + item_descriptions API)

**KARAR v2: `item_descriptions` API kullan multi-frame pack için. Max 16 unique item per dispatch. n_frames=64'te ya 64 explicit text entry, ya 16 base + variant strategy via item_descriptions.**

**Codex feedback entegrasyonu:** API'nin `item_descriptions` field'i var — per-frame description support. v1'deki "17-64). variants of items above" range shorthand zayıf. Production'da YASAK.

| n_frames | Max unique | Strateji (CONTROL EDİLEN) |
|---|---|---|
| 4 | 4 | `item_descriptions=[d1, d2, d3, d4]` her piece için açık ifade. |
| 16 | 8-16 | `item_descriptions=[d1, ..., d16]` her item explicit. |
| 64 | 8-16 unique × 4 variant | `item_descriptions` 64 entry: ya 64 unique explicit, ya 16 base × 4 variant açık ifade. |

**Variant strategy (n_frames=64) revize:**
```python
item_descriptions = [
  # Base 16 items
  "1). gold coin pile small",
  "2). silver coin pile small",
  ...
  "16). cyan mineral chunk small",
  # Variant pass 1 (color shift)
  "17). gold coin pile, weathered tarnish",
  "18). silver coin pile, blood stain",
  ...
  "32). cyan mineral chunk, violet glow variant",
  # Variant pass 2 (rotation)
  "33). gold coin pile, rotated 45 degrees",
  ...
  "64). cyan mineral chunk, fragmented variant"
]
```

**Theme coherence rule (v1'den korundu):**
- Aynı dispatch'teki tüm item'lar **aynı act material** (Act 1 = granite #3A3D42 + cyan #00FFCC)
- Cross-act batch YASAK

**Codex Iter 2 cevap (Açık Soru #5 RESOLVED):**
> "Main `description` should be shared style anchor (palette, camera, canvas occupancy, transparent background). Per-frame details belong in `item_descriptions`. Do not duplicate a full numbered list in main description. Numbering inside `item_descriptions` is optional; explicit one-entry-per-frame wording is the important part."

**Production rule:**
- `description` = ortak stil + palette + perspective + style anchor + background
- `item_descriptions` = her frame için unique geometry/identity (numbered olabilir veya olmayabilir)
- **YASAK:** Main description'da "1). ... 2). ..." numbered list + item_descriptions parallel kullanımı (redundancy)
- `item_descriptions` array uzunluğu = `n_frames` (deterministic kontrol için)

---

### 5. state_of vs Yeni Object

**KARAR v2: Görsel heuristic — küçük overlay edit'leri state_of, geometry/silhouette/material değişimleri yeni object. % piksel limit API resmi değil, RIMA heuristic.**

**Codex feedback entegrasyonu:** API'de "%30 pixel change" limit YOK. v1'deki bu cümle RIMA üretim disiplini, official constraint değil.

**Görsel heuristic table:**

| Edit türü | state_of | Yeni object | Sebep |
|---|---|---|---|
| Crack overlay (intact → cracked) | **state_of** | | Aynı silhouette, overlay edit. |
| Moss / vine overgrowth | **state_of** | | Surface overlay. |
| Glow / dim flicker (sconce lit ↔ unlit) | **state_of** | | Color edit. |
| Small missing stones / chip | **state_of** | | Local edit, silhouette korunur. |
| **Collapsed silhouette (üst kısmı kırık)** | | **yeni object** | Silhouette değişiyor — outline farklı. |
| **Broken top profile (kırık tepe)** | | **yeni object** | Geometry edit, state_of overlay yetmez. |
| **Arch damage (kemer çatlama → kemer çökmüş)** | | **yeni object** | Structural geometry. |
| **Rubble field change (yığın yapısı)** | | **yeni object** | Tamamen farklı item. |
| Cross-act material variant | | **yeni object** | Color palette + texture büyük değişim. |
| Cross-perspective (NS → EW) | | **yeni object** | Geometry farklı. |
| Cross-size (128 → 64) | | **yeni object** | Size param state_of'ta locked. |
| Rift state (closed → opening → open) | **state_of** | | Glow + overlay edit, S94 ispatlı. |
| Void Flame (mounted_lit → mounted_dim) | **state_of** | | Brightness edit. |

**RIMA prod kuralı:**
- Wall damage = "üst kırık + taş dağılmış" → **yeni object dispatch**, state_of değil.
- Wall crack = sadece çatlak overlay → state_of OK.

**Production impact:** Sıra 1 wall queue revize:
- Önceki: 6 base + 6 damaged state = 12 sub-dispatch (60 + 60 = 120 gen)
- Yeni: 6 base + 6 damaged yeni object = 12 base dispatch (her biri 128 n_frames=1 ≈ 20 gen × 12 = 240 gen)
- Trade-off: +120 gen cost ama her damaged piece kendi silhouette'iyle gelir
- Alternatif: 3-piece damaged batch (face_NS_damaged + face_EW_damaged + corner_damaged) tek dispatch n_frames=4 ≈ 40 gen (Codex pro tool ranges'den)

---

### 6. View Parametresi Mapping

**KARAR v2: `view` ve `object_view` ayrı pilot. `view="side"` + `object_view="top-down"` blend YASAK (untested). Side wall billboard'lar için `object_view=null` default.**

**Codex feedback entegrasyonu:** `object_view` = default-style category (style_images yokken). `view` = camera direction. Bu iki param farklı semantik, kör combine etme.

| Use case | view | object_view | Sebep |
|---|---|---|---|
| L2a flat floor base (collider source) | `"low top-down"` | `"top-down"` | 65c99904 örneği, top-down style category mantıklı. |
| L2b dikey wall face (side billboard) | `"side"` | **`null`** (default) | Side + top-down blend untested. Default category yeterli. |
| Floor clutter (potion, rune, coin) | `"low top-down"` | `"top-down"` | Karar #100 35° dünya, top-down category. |
| Mob silhouette | `"low top-down"` | `"top-down"` | Karar #100 35° chibi. |
| Tall prop (statue, altar, pillar) | `"high top-down"` | `"top-down"` | 15% top-down depth + top-down category. |
| Wall-mounted item (sconce, plaque) | `"side"` | **`null`** | Side + top-down untested. |
| Boss prop landmark (throne) | `"high top-down"` | `"top-down"` | Yükseklik + top-down style. |
| **Pilot test:** Side wall + `object_view="sidescroller"` | `"side"` | `"sidescroller"` | A/B test (Codex önerisi) — eğer null default zayıf çıkarsa. |

**Pilot karar gate (yeni):**
- İlk L2b wall batch (Template A v2) `object_view=null` ile fire
- Sonuç stil drift → A/B test: aynı prompt `object_view="sidescroller"` ile
- En iyi default category lock, future wall batch'leri o config ile

---

### 7. Description Prompt Formülü

**KARAR v2 (Codex revisions entegre):**

```
[BASE GEOMETRY], [MATERIAL + COLOR (name + HEX where palette critical)],
[SECONDARY DETAIL + ACCENT HEX], [PERSPECTIVE CUE INCLUDING CANVAS OCCUPANCY],
[STYLE ANCHOR], [BACKGROUND]
```

**Side wall için canvas occupancy cue MANDATORY (Codex önerisi):**
- "tall vertical wall billboard, sprite fills most of the 128px canvas height, narrow transparent margins"

**Codex'in hard kuralları:**
- Genre label YASAK ("dark fantasy", "grimdark", "horror") — confirmed
- "Hades-style" YASAK (third-party game name) — confirmed
- Color HEX **strongly recommended** (zorunlu değil, palette critical olduğunda)
- Color name (granite gray) + HEX (#3A3D42) kombinasyon en güvenli
- Numbered list batch control için zorunlu — confirmed
- `item_descriptions` API ile per-frame detay daha güçlü (Karar #4)

**Slot doldurma örneği (Act 1 wall face) revize:**
```
[BASE GEOMETRY]: ancient stone keep wall facing south
[MATERIAL + COLOR]: granite gray #3A3D42 base
[SECONDARY DETAIL + ACCENT HEX]: cyan #00FFCC mineral veins in cracks, weathered mortar lines, exposed lighter stone #5A5F66
[PERSPECTIVE CUE + CANVAS OCCUPANCY]: tall vertical wall billboard from side perspective, sprite fills most of 128px canvas height, narrow transparent margins
[STYLE ANCHOR]: painterly pixel art, no outline
[BACKGROUND]: isolated on transparent background
```

**Birleştirilmiş prompt (v2):**
```
ancient stone keep wall facing south, granite gray #3A3D42 base, cyan #00FFCC mineral veins in cracks, weathered mortar lines, exposed lighter stone #5A5F66, tall vertical wall billboard from side perspective filling most of 128px canvas height with narrow transparent margins, painterly pixel art, no outline, isolated on transparent background
```

---

### 8. 4-Piece Wall Batch (n_frames=4 + item_descriptions)

**KARAR v2: `create_object` n_frames=4 + `item_descriptions` API kullan. PILOT GATE — ilk batch visual review sonrası karar.**

**Codex feedback entegrasyonu:** API'de `item_descriptions` field'i var multi-frame pack için. v1'deki long description shorthand yerine per-frame array kullan.

```python
mcp__pixellab__create_object(
  description=(
    "Act 1 Shattered Keep ancient stone keep wall pieces, "
    "granite gray #3A3D42 base with cyan #00FFCC mineral veins, "
    "weathered mortar lines, exposed lighter stone #5A5F66, "
    "tall vertical wall billboard from side perspective filling most of 128px canvas height with narrow transparent margins, "
    "painterly pixel art, no outline, isolated on transparent background"
  ),
  item_descriptions=[
    "wall face south view, flat single facet, granite weathered, cyan veins along mortar",
    "wall face east view, perpendicular flat single facet, granite weathered, cyan veins along mortar",
    "outer corner piece, two granite facets meeting at 90 degrees both visible, weathered cyan",
    "arched doorway opening, rough archway through granite wall with stone keystone, cyan accent"
  ],
  size=128,
  view="side",
  directions=1,
  n_frames=4,
  object_view=None  # null default — Karar #6 pilot gate
)
```

**Post-dispatch flow:**
1. `get_object(object_id=...)` → 4 candidate inline preview
2. Visual review:
   - 4 piece stil tutarlı + canvas dolu → **A LOCK** → `select_object_frames(indices=[0,1,2,3], common_tag="act1_wall_base_s95")`
   - Bir veya iki piece zayıf → `select_object_frames` ile iyileri al + zayıf olanı ayrı n_frames=1 dispatch ile redo
   - Hepsi drift → `dismiss_review` + Plan C (4 ayrı 128 n_frames=1 dispatch)

**Cost (Codex range-based):**
- A path n_frames=4 batch: 25-40 gen (range — pro tool batch memory'de 40-credit run ispatlı)
- Plan C 4 ayrı dispatch: 4 × ~20 gen = 80 gen
- Pilot cost: 25-40 gen (acceptable test)

---

### 9. Bütçe Plan (Range-Based, Codex Revision)

**KARAR v2:** Tüm cost'lar **range** olarak verilir. Her dispatch sonrası gerçek `usage` log'lanır.

**Mevcut bütçe:** ~2,500 PixelLab gen kaldı (5,000 total). S94 batch log'larından evidence: 64×64 base ≈ 20 gen, state ≈ 10 gen.

**Sıra 1-3 plan (range):**

| Batch | Tool + Mode | Gen range (per dispatch) | Adet | Range subtotal |
|---|---|---|---|---|
| Wall L2b face batch (face_NS + face_EW + corner + arch) | `create_object` 128 n_frames=4 + item_descriptions | **25-40** | 1 | 25-40 |
| Wall L2a base (collider source) | `create_object` 128 n_frames=1 | **15-25** | 1 | 15-25 |
| Wall L2b inner_corner + end_cap_NS + end_cap_EW + ruined_half_wall | `create_object` 128 n_frames=4 + item_descriptions | **25-40** | 1 | 25-40 |
| Wall damaged variants (silhouette değişim → YENİ object batch) | `create_object` 128 n_frames=4 | **25-40** | 1-2 | 25-80 |
| Wall crack/moss state'leri (overlay only) | `create_object_state` | **8-15** | 4-6 | 32-90 |
| Void Flame Act 1 base | `create_object` 64 n_frames=1 | **15-25** | 1 | 15-25 |
| Void Flame state (mounted_dim, floor_stand_lit) | `create_object_state` | **8-15** | 2 | 16-30 |
| Floor clutter 32px batch (16 unique + variants) | `create_object` 32 n_frames=64 + item_descriptions | **30-50** | 1 | 30-50 |
| Wall-mounted 64px batch (8-16 items) | `create_object` 64 n_frames=16 + item_descriptions | **25-40** | 1 | 25-40 |
| **Sıra 1-3 total range** | | | | **~210-420 gen** |

**Buffer + reserve:**
- Mob 64px batch (Sıra 4): ~50-100 gen
- Boss prop (256×256) tek piece × Act = 25-40 gen × 3 = 75-120 gen
- Cross-act variants (Act 2 + Act 3): ~400-600 gen
- Reserve drift retry: ~200 gen

**Toplam plan range:** ~935-1,440 gen → 2,500 bütçeden ~%37-58 kullanım → **GÜVENLİ**

**Üretim sırası (revize):**

1. **PILOT (BU HAFTA):** Wall L2b 4-piece face batch (25-40 gen) → stil tutarlı doğrulanırsa devam
2. **HAFTA 1:** Wall L2a single + inner_corner/end_cap batch + crack state'leri (80-160 gen)
3. **HAFTA 1:** Wall damaged variants (silhouette değişim batch, 25-80 gen)
4. **HAFTA 2:** Void Flame Act 1 (31-55 gen)
5. **HAFTA 2:** Floor clutter 32px + wall-mounted 64px (55-90 gen)
6. **HAFTA 3+:** Mob batch + cross-act variants

**Trigger gate (Codex önerisi):** Her batch sonrası **STAGING/RIMA_PixelLab_BalanceLog.md** veya PIXELLAB_OBJECT_PRODUCTION.md güncellenir gerçek `usage` ile.

---

## Prompt Templates (Ready-to-Use, v2)

### Template A v2 — L2b Wall Face Batch (128px n_frames=4 + item_descriptions)

```python
mcp__pixellab__create_object(
  description=(
    "Act 1 Shattered Keep ancient stone keep wall pieces, "
    "granite gray #3A3D42 base with cyan #00FFCC mineral veins, "
    "weathered mortar lines, exposed lighter stone #5A5F66, "
    "tall vertical wall billboard from side perspective filling most of 128px canvas height with narrow transparent margins, "
    "painterly pixel art, no outline, isolated on transparent background"
  ),
  item_descriptions=[
    "wall face south view, flat single facet, granite weathered, cyan veins along mortar",
    "wall face east view, perpendicular flat single facet, granite weathered, cyan veins along mortar",
    "outer corner piece, two granite facets meeting at 90 degrees both visible, weathered cyan",
    "arched doorway opening, rough archway through granite wall with stone keystone, cyan accent"
  ],
  size=128,
  view="side",
  directions=1,
  n_frames=4,
  object_view=None
)
```

**Beklenen cost:** 25-40 gen

**Post-dispatch:**
1. `get_object` → 4 candidate preview
2. Stil tutarlı: `select_object_frames(indices=[0,1,2,3], common_tag="act1_wall_base_s95")`
3. Drift: `dismiss_review` + Plan C (4 ayrı n_frames=1 dispatch) veya tek piece redo

---

### Template B v2 — Floor Clutter Batch (32px n_frames=64 + item_descriptions)

```python
mcp__pixellab__create_object(
  description=(
    "Act 1 Shattered Keep dropped items and floor clutter on stone floor, "
    "cyan #00FFCC + gold #C9A227 + crimson #8C1F1F accent palette, "
    "weathered painterly pixel art, no outline, "
    "isolated on transparent background, "
    "top-down view dropped on stone floor, small footprint sprite"
  ),
  item_descriptions=[
    # Base 16 unique items
    "gold coin pile small, golden #C9A227 stack",
    "silver coin pile small, silvery shine",
    "red health potion vial, crimson #8C1F1F liquid",
    "blue mana potion vial, deep blue liquid",
    "green herbal potion vial, sage green liquid",
    "ancient rune stone with cyan #00FFCC glow",
    "ancient rune stone with violet glow",
    "ancient rune stone with gold #C9A227 glow",
    "rusty iron key, brown corroded",
    "brass ornate key, golden ornate",
    "small skull fragment, bone white",
    "broken pottery shard, terracotta",
    "dropped torch stub extinguished, charred wood",
    "bloodstain dark crimson #8C1F1F",
    "small cyan #00FFCC mineral chunk",
    "weathered scroll rolled, parchment beige",
    # Variant pass 1 (color/state shift, items 17-32)
    "gold coin single coin, tarnished",
    "silver coin single coin, blood stained",
    "red potion bottle larger, sealed",
    "blue potion bottle larger, sealed",
    "green potion bottle larger, sealed",
    "rune fragment broken cyan",
    "rune fragment broken violet",
    "rune fragment broken gold",
    "iron key broken half",
    "brass key broken half",
    "skull fragment larger piece",
    "broken pottery shard larger",
    "ash pile from torch, gray powder",
    "smaller bloodstain spatter",
    "cyan mineral cluster larger",
    "scroll unrolled partially, parchment with writing",
    # Variant pass 2 (rotation/orientation, items 33-48)
    "gold coin pile rotated, side view",
    "silver coin pile rotated, side view",
    "red potion vial knocked over, spilled",
    "blue potion vial knocked over, spilled",
    "green potion vial knocked over, spilled",
    "rune stone flat lying, cyan",
    "rune stone flat lying, violet",
    "rune stone flat lying, gold",
    "iron key on side, rust pattern",
    "brass key on side, ornate detail",
    "skull viewed from above, eye sockets",
    "pottery shard pile multiple pieces",
    "torch stub on side, splintered",
    "blood streak elongated",
    "cyan mineral shard fragment small",
    "scroll case cylindrical, brass cap",
    # Variant pass 3 (size/material variation, items 49-64)
    "tiny gold coin single small",
    "tiny silver coin single small",
    "tiny health potion vial small",
    "tiny mana potion vial small",
    "tiny herb potion vial small",
    "rune dust pile cyan glow",
    "rune dust pile violet glow",
    "rune dust pile gold glow",
    "iron lock fragment broken",
    "brass lock fragment broken",
    "bone fragment small white",
    "ceramic dust pile gray",
    "wood splinter pile brown",
    "blood drop single droplet",
    "cyan dust speck tiny",
    "parchment scrap torn small"
  ],
  size=32,
  view="low top-down",
  directions=1,
  n_frames=64,
  object_view="top-down"
)
```

**Beklenen cost:** 30-50 gen

**Post-dispatch:** `select_object_frames` ile 32-48 best frame'i seç, kalan'ı dismiss et. Wall_decoration_pure_attachment_only LOCK ile ilgili değil (floor item).

---

### Template C v2 — Wall-Mounted 64px Batch (n_frames=16 + item_descriptions)

```python
mcp__pixellab__create_object(
  description=(
    "Act 1 Shattered Keep wall-mounted decorations, "
    "iron #2A2D32 + bronze #8B7355 + cyan accent #00FFCC palette, "
    "weathered painterly pixel art, no outline, "
    "isolated on transparent background, "
    "side perspective wall-attached items with mounting hardware, "
    "decoration only without wall background"
  ),
  item_descriptions=[
    "iron sconce empty bracket, wrought iron #2A2D32 mount",
    "iron chain hook short, weathered wrought iron",
    "torch wall bracket weathered, iron #2A2D32 with rust",
    "small ceremonial pennant cyan #00FFCC trim, cloth banner",
    "bronze #8B7355 decorative plaque, embossed sun motif",
    "iron #2A2D32 candle holder twin arms, drip catcher",
    "hanging chain link short, wrought iron #2A2D32 links",
    "small stone gargoyle head mount, weathered granite",
    "iron #2A2D32 ring door knocker, ornate edge",
    "bronze #8B7355 ceremonial mask wall mount, blank eyes",
    "small wooden shield round wall display, iron rim",
    "iron #2A2D32 weapon rack empty, two hooks",
    "hanging skull trophy small, bone white with iron chain",
    "bronze #8B7355 sun emblem wall plaque, radiant rays",
    "small lantern hook unlit, iron #2A2D32 with chain",
    "cyan #00FFCC rift crystal wall growth small, void mineral"
  ],
  size=64,
  view="side",
  directions=1,
  n_frames=16,
  object_view=None  # pilot gate Karar #6
)
```

**Beklenen cost:** 25-40 gen

**Post-dispatch:** `select_object_frames(indices=[...])` — Karar #149 wall_decoration_pure_attachment_only LOCK uyumlu (sadece mounting + decoration).

---

## Codex Review Excerpts

### Iter 2 (PASS_WITH_MINOR_REVISIONS, no Iter 3)

> "All five Iter 1 NEEDS_REVISION items are RESOLVED. The damaged-wall split is correct: crack/moss/glow/chip overlays can use `state_of`; broken tops, collapsed silhouettes, arch collapse, rubble fields, and cross-material/cross-perspective changes should be new object dispatches." (Karar #5)

> "`item_descriptions` should carry per-frame object identity. The main `description` should carry shared style, palette, camera, canvas occupancy, and transparent-background constraints. ... For deterministic packs, array length should equal `n_frames`." (Karar #4)

> "Do not add `object_view='top-down'` back into side billboard prompts unless a pilot proves it is visually superior." (Karar #6)

> "Keep the upper bound, not the average, as the production reservation number. ... 'reserve 420 gen for Sira 1-3; expected 210-420.'" (Karar #9)

> "Pilot gates are not too heavy. They are placed at high-risk boundaries. ... Keep the review burden bounded: one Template A pilot first, then only branch if it fails. Do not run all A/B gates up front."

> "Open Question #7: use 64 explicit entries for first production. Do not A/B this unless the first 64-frame batch quality is poor."

> "Implementation caveat: current production dispatch must be checked to ensure it supports forwarding `item_descriptions`. If direct MCP wrapper schema does not expose the field, use the REST dispatch path that Iter 1 validated."

### Iter 1 (PASS_WITH_REVISIONS)

**Overall verdict:** PASS_WITH_REVISIONS — 4 LIVE (2, 3, 7, 8), 5 NEEDS_REVISION (1, 4, 5, 6, 9), 0 blocker.

**Önemli alıntılar:**

> "L2a wants `view='low top-down'`; L2b wants `view='side'`; one `create_object` dispatch has one shared `view`. ... Remove same-dispatch-with-L2b as the justification." (Karar #1)

> "API support is better than the draft states because `item_descriptions` gives per-frame control for multi-frame packs." (Karar #8)

> "For side walls, add 'tall vertical wall billboard, fills most of the 128px canvas height, narrow transparent margins'. HEX colors should be strongly recommended, not treated as the only valid color language." (Karar #7)

> "`object_view='top-down'` should not be blindly applied to `view='side'` wall billboards. ... Mixing `view='side'` with `object_view='top-down'` is an untested category blend." (Karar #6)

> "The `<=30% pixel change` threshold is a RIMA heuristic, not an official API constraint. Wall damage with broken tops and scattered stones can cross from state edit into new geometry." (Karar #5)

> "Memory also records Pro tool batch sizes as 40-credit runs, which conflicts with the 25-gen estimate. ... Present budget as ranges until actual `usage` data is logged." (Karar #9)

**Hard API confirmations:**
- `n_frames` enum = [1, 4, 16, 64] (size 32 only allows up to 64)
- `n_frames>1` → review status
- `item_descriptions` field exists for per-frame multi-frame packs
- `tile_size` 16-256 (live REST), `tile_height` 16-256 non-square supported
- `create_isometric_tile` MAX 64×64
- `create_object_state` no published % pixel change limit

---

## Dispatch-Path Caveat (Codex Iter 2 önerisi)

**Implementation check (kod tarafında):**
`item_descriptions` field'i PixelLab REST API'de doğrulandı (Iter 1) ve MCP `create_object` tool schema'sında doğrulandı (description-only parametre listesinde gözükmüyor ama API contract'ında var). İlk pilot dispatch ÖNCE:

- **Eğer MCP wrapper `item_descriptions` field'i forward etmiyorsa:** Direct REST dispatch path kullan (Iter 1'de validated)
- **Eğer MCP wrapper forward ediyorsa:** Normal MCP dispatch
- **Verify step:** İlk dispatch'te `get_object` response'u her frame için description'ın doğru göründüğünü check et

**Aksiyon:** Kullanıcı/orchestrator ilk Template A v2 dispatch'inde MCP path verify etsin. Eğer field forward edilmiyorsa Codex Iter 3 gerek (dispatch path fix). Spec içeriği aynı kalır.

---

## Açık Sorular (Gemini 3.5 Flash review için + user gate)

Codex Iter 2'de cevaplanan sorular (#5, #7) entegre edildi. Kalan açık sorular **pilot-gated** veya **user decision** kategorisinde:

1. **L2a runtime'da görünür mü?** [User decision]
   - Hayır → `create_object` 128 single (default)
   - Evet → `create_isometric_tile` 64 thin tile (visual pilot olarak, sadece pivot adjustment değil — Codex Iter 2 not)
   - Bu spec'in varsayımı: L2a runtime'da görünmez (collider source)

2. **`object_view` side wall'lar için: null mu, sidescroller mı?** [Pilot gate — Karar #6]
   - İlk dispatch `object_view=None` ile fire
   - Stil drift varsa A/B test `object_view="sidescroller"` ile
   - **YASAK:** `object_view="top-down"` side wall'larda (untested blend)

3. **`item_descriptions` n_frames=4 batch'inde "4 farklı geometry" stil tutarlı çıkıyor mu?** [Pilot gate — Karar #2, #8]
   - İlk pilot Template A v2 fire, visual review sonra karar
   - Drift → Plan C (4 ayrı n_frames=1 dispatch)

4. **128 n_frames=4 wall batch gerçek `usage` cost'u kaç?** [Pilot usage log — Karar #9]
   - Reserve 40 gen (upper bound)
   - Beklenen range 25-40
   - Log gerçek değeri `STAGING/RIMA_PixelLab_BalanceLog.md`'a

5. **Wall damaged variants (silhouette değişim) yeni object batch'lere taşındı — bütçe etki kabul edilebilir mi?** [User production budget — Karar #5, #9]
   - +100-200 gen ekstra (Codex Iter 2 acceptable verdict)
   - Toplam 2,500 bütçeden %4-8 ekstra → acceptable per Codex

**Codex Iter 2'de cevaplanmış sorular (sadece referans için):**
- ~~#5 v1 (item_descriptions redundancy)~~: Main = style anchor, item_descriptions = per-frame identity. No duplicate numbering. (Karar #4'e entegre edildi)
- ~~#7 v1 (Template B 64 explicit vs 16 base+variant)~~: Use 64 explicit entries for first production. Do not A/B unless 64-frame quality poor. (Template B v2 + Karar #4'e entegre edildi)

---

## Hard Constraints (Hatırlatma)

- **PixelLab MCP dispatch BU SPEC'TE YAPILMAYACAK.** Sadece hazır prompt template'leri.
- **Gerçek üretim:** User onayı sonrası Codex/Sonnet dispatch eder, MCP via `cx_dispatch.py` veya direct.
- **Codex Iter 2 review zorunlu:** Bu v2 → Codex re-validate → user onay → ilk pilot dispatch fire.
- **Pilot gate first:** İlk gerçek dispatch = Template A v2 (wall face 4-piece, 25-40 gen) — sonuç visual review sonra production scale up.
