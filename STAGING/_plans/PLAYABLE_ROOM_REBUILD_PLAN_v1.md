# Playable Room Rebuild — Plan v1

**Status:** PLAN DRAFT, user guidance bekler.
**Date:** 2026-05-21
**Hedef:** Reference image (`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`) seviyesinde atmospheric playable room.
**Filozofi:** Basit tut, layer layer ilerle, her layer onaylanmadan sonrakine geçme.

---

## 1. Reference Image Analizi

**Görsel özellikler:**
- 30-35° angled isometric perspective (RIMA canonical)
- **TALL perimeter walls** — dış dungeon sınırı, ~2-tile yüksek, kalın stone block
- **LOW inner walls** — multi-room dividers, ~1-tile yüksek, ince stone ramparts
- Wall'lar üzerinde dekorasyon (banner, chain, candle, kemikler, yosun)
- Multi-room layout (oda içi 3-4 chamber, archway ile bağlı)
- Flat iso floor (granite stone, cyan rift cracks scatter)
- Lighting: torch glow + cyan rift accent
- Karakter + mob clearly visible (camera framing breathing room veriyor)

**Çıkarım:**
- 2 farklı wall tipi gerek: PERIMETER (tall) + INNER (low)
- Walls'ın üst kısmı flat top (decoration üstüne konacak yer)
- Floor base + crack overlay layer ayrı

---

## 2. Mevcut Durum (Verified Assets)

| Asset | Durum | Boyut | Lokasyon |
|---|---|---|---|
| **Iso Floor (granite)** | ✅ 3 variant LIVE | 64×64 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` |
| **Rift overlay** | ✅ 1 variant LIVE | (büyük) | `rift_accents/act1_rift_fracture_overlay_v01.png` |
| **Walls (Pilot A 7-piece)** | ⚠️ TALL only, 1 variant per type | 128×128 | `walls/pilot_a_test/` |
| **Pillars** | ✅ 3 variant | 64×96 / 128×128 | `pillars/` |
| **Braziers** | ✅ 2 variant (cyan + orange) | TBD | `props/` |
| **Statues** | ✅ 3 variant | TBD | `statues/` |
| **Ritual** | ✅ 5 variant | TBD | `ritual/` |
| **Wall decorations** | ✅ 10 variant (banner/chain/cage/skeleton/ivy/lantern/grate) | TBD | `wall_decoration/` |
| **Decals** | ✅ bone chip × 4 + crack × 4 (dust archived) | 32-64 | `decals/` |
| **Cross junction wall** | ❌ YOK | — | inner wall corners için lazım |
| **Low wall set** | ❌ YOK | — | inner room dividers için lazım |
| **Damaged/cracked/mossy wall variants** | ❌ YOK | — | variation system için lazım |

---

## 3. Eksikler

### A. Wall Production (PixelLab `create_object` 64×128 base)

Memory `project-wall-object-production-plan-s95` LOCK + variation plan synthesis:

#### A1. PERIMETER Wall Set (TALL) — dungeon dış sınır
Already mostly have Pilot A 7-piece. Eksik:
- **damaged variant** her piece (kırık taş hissi)
- **cracked variant** her piece (cyan rift çatlak)
- **mossy variant** her piece (yıkık eski yosunlu)
- **cross junction (X)** piece (iç-iç duvar kesişimi)
- 4 type × 3 yeni variant + 1 yeni type × 4 variant = **16 yeni piece**, ~250 gen

#### A2. INNER Wall Set (LOW) — multi-room dividers
**Tamamen yeni production:**
- face_LOW_NS / face_LOW_EW
- corner_LOW_outer / corner_LOW_inner
- end_cap_LOW (1 yön broken end)
- gap_LOW (kırık parça, geçilemez ama görünür)
- 6 type × 4 variant (intact/damaged/cracked/mossy) = **24 piece**, ~300 gen

#### A3. Wall-Mount Decoration (Pure Attachment per [[feedback-wall-decoration-pure-attachment-only]])
Decoration ayrı sprite, wall üstüne overlay:
- Banner small (3 renk: purple/red/teal) ✅ mevcut
- Chain hanging short/long ✅ mevcut
- Lantern hanging ✅ mevcut
- Candle small (3 variant: lit/unlit/cyan) ❌ YENİ (~30 gen)
- Wall sigil (cyan rune symbol) ❌ YENİ (~20 gen)
- Skull mount ❌ YENİ (~20 gen)
- **Toplam: ~70 gen yeni decoration**

### B. Floor Layer Polish

Memory `alabaster-dawn-pipeline-lock` (Karar #143):
- Layer 1 base = ✅ mevcut (3 granite variant, ideal 8-16 olsa daha iyi variety)
- Layer 2 organic patches = ❌ YOK (256² soft alpha overlay, doğal görünüm secret)
- Layer 5 decals = ✅ mevcut (bone + crack)
- **Eksik organic patches:** moss patch + dirt patch + rubble area + rift seepage → **6 patch × 1 gen = ~50 gen REST endpoint**

### C. Brazier Light Standard (Void Flame canonical)
S95 plan: 32×64 px, state lit/unlit. Mevcut brazier asset'lerin boyutunu doğrulamamız + state spec gen gerek.

---

## 4. Boyut Standardı (LOCKED per memory S95)

| Kategori | Boyut | Örnek |
|---|---|---|
| Floor tile | 64×64 (iso diamond) | granite_clean / worn / chiseled |
| Floor patch (organic overlay) | 256×256 (soft alpha) | moss / dirt / rubble |
| Floor decal | 32×32 | bone chip / crack chip / blood spatter |
| **Inner LOW wall** | **64×64** (1-tile) | face / corner / end_cap |
| **Perimeter TALL wall** | **128×128** (2-tile) | Pilot A 7-piece existing |
| Wall decoration (attachment) | 32×64 | candle / banner / sigil |
| Light source (brazier) | 32×64 | Void Flame canonical |
| Small prop | 64×64 | barrel / crate / urn |
| Medium prop | 64×96 | broken pillar / small statue |
| Vertical prop | 64×128 | intact pillar / altar / obelisk |
| Boss landmark | 128×128 | tomb headstone / ritual statue |
| Mob (small) | 64×64 | bone walker / goblin |
| Mob (medium) | 64×96 | husk / bone archer |
| Player chibi | 64×96 (visible) | Warblade canonical south |

**Hierarchy:** Player ≈ medium mob < pillar/altar < tall wall = perimeter < boss landmark.

---

## 5. PixelLab Endpoint Doğru Eşleştirme

User'ın haklı uyarısı: `create_tiles_pro` SADECE floor. Walls farklı endpoint:

| Asset | Endpoint | Mode |
|---|---|---|
| Floor tile (Layer 1) | `mcp__pixellab__create_tiles_pro` | shape mode, tile_type=isometric, tile_view_angle=55, tile_size=64 |
| Organic patches (Layer 2) | REST `/generate-with-style-v2` | style_image=base floor, 256² |
| Wall pieces (Layer 3) | `mcp__pixellab__create_object` | individual per piece, style_image shared across batch |
| Wall mount decoration | `mcp__pixellab__create_object` | 32×64, shared style |
| Decals (Layer 5) | `mcp__pixellab__create_object` | 32×32 transparent |
| Brazier light | `mcp__pixellab__create_object` + state animation | 32×64, 2 state (lit/unlit) |

**Önemli:** Walls için single-batch consistency `style_image` reference paylaşımı ile sağlanır (create_tiles_pro 16-batch gibi olmasa da pseudo-batch).

---

## 6. Unity Isometric Tilemap Setup

Memory `project-isometric-floor-pivot-s95` LOCK:
- Grid GameObject: Cell Layout = **Isometric** (cellLayout=3) veya **Isometric Z As Y** (cellLayout=4)
- Cell Size: (1, 0.5, 1) — diamond ratio 2:1
- Tilemap component: TilemapRenderer (sortingOrder Layer "Floor")
- Transparency Sort Mode: Custom Axis (0, 1, 0) — Y dominant

**Mevcut sorun:** Manual GameObject wall'lar 2D top-down render edildi → Scene view iso görünmüyor → Painter doğru çalışmıyor.

**Düzeltme:** 
- WallTilemap GameObject (Cell Layout = Isometric)
- Wall sprite'ları TilemapRenderer ile yerleşir
- Painter Wall RuleTile mode → Tilemap'e paint
- Scene view 2D toggle → iso projection görünür

---

## 7. Production Sırası (Layer 1 → Layer 6)

### Phase 0 — Scene Cleanup (önce burası)
1. Mevcut `IsoShowcaseRoom_S95.unity` ayıkla → tüm manual GameObject sil
2. Empty scene'e Grid + WallTilemap + FloorTilemap + Props_Root + Lighting_Root
3. Isometric Tilemap parametreleri set
4. Görsel test: empty iso grid Scene view'da iso projection ile görünmeli

### Phase 1 — Floor Base (mevcut + organic patches)
1. Floor tile pool: mevcut 3 granite variant → Unity Tile Palette
2. (Opsiyonel) Floor variant expansion: 3 → 8 (5 yeni gen, ~50 gen)
3. Organic patches: 6 patch generation (moss/dirt/rubble/rift seepage)
4. Test scene: 16×10 flat floor + 4-6 organic patch scatter

### Phase 2 — Wall Production
1. **TALL perimeter walls** (Pilot A 7-piece KORUNUR): damaged/cracked/mossy variants gen (~250 gen)
2. **LOW inner walls** (yeni set): 6 type × 4 variant gen (~300 gen)
3. Cross junction (X) piece × 4 variant (~50 gen)
4. RuleTile asset her set için: `Act1_TallWallRuleTile.asset`, `Act1_LowWallRuleTile.asset`
5. Painter mode toggle: "Perimeter" / "Inner" wall layer seçim

### Phase 3 — Wall Decoration
1. Wall-mount decorations: candle/sigil/skull mount yeni gen (~70 gen)
2. Existing decoration (banner/chain/lantern) reuse
3. Painter mode: "Wall Decoration Scatter" — per cell %X şansla decoration spawn

### Phase 4 — Lighting
1. Void Flame brazier state: lit/unlit gen
2. Unity Point Light 2D child (Act 1 cyan #00FFCC)
3. Place 2-3 brazier (1 center + 2 corner)
4. Ambient: dark global, torch contrast

### Phase 5 — Objects (Pillars + Statues + Ritual)
1. Mevcut asset reuse (3 pillar + 3 statue + 5 ritual)
2. Painter object placement mode

### Phase 6 — Player + Movement
1. Warblade canonical sprite
2. PlayerMovementController WASD
3. Camera follow
4. Y-sort with IsoSortingOrder.cs

---

## 8. Production Budget Estimate

| Phase | PixelLab Gen | Notlar |
|---|---:|---|
| Phase 1 Floor expansion + patches | ~100 | 5 floor var + 6 organic patches |
| Phase 2 Walls (tall variants + low set + cross) | ~600 | 16+24+4 piece, ~15-20 gen each via create_object |
| Phase 3 Decoration | ~70 | candle/sigil/skull mount |
| Phase 4 Lighting | ~30 | brazier state lit/unlit |
| Phase 5 Objects | 0 | mevcut reuse |
| Phase 6 Player | 0 | mevcut |
| **TOPLAM** | **~800 gen** | mevcut budget 2,473 → rahatça sığar |

**Alternative scope (minimum viable):**
- Phase 1 floor expansion **SKIP** (3 variant şimdilik yeterli)
- Phase 2 walls **PARTIAL** (sadece LOW set + Pilot A variants kısmen)
- Phase 3 decoration **SKIP** (mevcut decoration yeterli)
- Phase 4 lighting **PARTIAL** (mevcut brazier'lar yeterli, state gen sonra)
- **Minimum: ~350-400 gen, 2-3 saat Codex + PixelLab work**

---

## 9. User Guidance Points (Sen Karar Vereceksin)

| # | Karar | Seçenekler |
|---|---|---|
| Q1 | Scene scope | (A) Tek atmospheric oda MVP, (B) Multi-room mini-dungeon (reference matching), (C) Full Act 1 vertical slice |
| Q2 | Wall scope | (A) Sadece TALL perimeter (mevcut Pilot A variants gen), (B) Sadece LOW inner (yeni set), (C) HER İKİSİ (full reference match) |
| Q3 | Floor patch scope | (A) Skip (sadece base 3 variant), (B) Generate 6 organic patches (Alabaster Dawn full) |
| Q4 | Production öncelik | (A) Hız (minimum scope, hızlı playable), (B) Görsel kalite (full scope, reference quality) |
| Q5 | Şu an phase'i | (A) Phase 0 cleanup ile başla, (B) Önce yeni wall asset gen et sonra clean scene, (C) Plan onayla, sonra all-phases sequence dispatch |

---

## 10. Recommended Path (Orchestrator Önerim)

**Hızlı + reference-aligned dengeli yol:**

1. **Phase 0 Cleanup** — boş iso scene + Tilemap setup (1 Codex dispatch, 1 saat)
2. **Phase 1 Floor placement** — mevcut 3 granite variant + Tile Palette + 16×10 paint (Codex, 30 dk)
3. **Phase 2 PARTIAL** — sadece **LOW wall set** dispatch (yeni 24 piece + 4 cross = ~350 gen)
   - TALL Pilot A mevcut variant şimdilik kalır (damaged/cracked sonra)
4. **Phase 3 SKIP** — mevcut decoration yeterli demo için
5. **Phase 4 SKIP** — mevcut 2 brazier yeterli
6. **Phase 5 Objects** — mevcut asset placement
7. **Phase 6 Player** — Warblade + WASD

**Sonuç:** Multi-room görünüm (LOW walls inner divide) + TALL perimeter (mevcut Pilot A) + atmospheric lighting. Reference'a yakın AMA scope dar.

**PixelLab cost:** ~350 gen (LOW wall set 24 piece × ~15 gen avg)
**Süre:** 2 saat Codex Unity work + ~45 dk PixelLab async gen

---

## 11. İlk Soru (Senin Yönlendirmen)

Bu plan'a verdict — kabul mü, hangi maddede değişiklik istersin?

Section 9 Q1-Q5'teki kararlar veya Section 10 recommended path bana yön verir. Karar verince ilgili Codex dispatch'leri başlatırım.
