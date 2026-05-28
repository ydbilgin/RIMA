# Sprite Strategy FINAL PLAN — RIMA Brush V1 Asset Pipeline

**Date:** 2026-05-16 S86
**Decision authority:** ChatGPT + Codex cross-review harmanlaması (her ikisi Hybrid Auto-Slice'a yakınsadı, divergence noktaları aşağıda çözüldü).
**Status:** LOCK — batch dosyaları + memory + Sprint 6/7 adjustment'lar bu plana göre revize edilecek.

---

## 1. CORE DECISION (LOCK)

**Strategy: HYBRID AUTO-SLICE**

| Layer | Üretim modeli | Slice modu | Variant kaynağı |
|---|---|---|---|
| L1 floor base | mevcut (Tilemap 32×32) | — | mevcut spritesheet |
| L2 floor variation | mevcut (Tilemap 32×32) | — | mevcut spritesheet |
| **L3 wall overlay** | **Strateji A semantic** | TemplateRect (named cells + tags) | Master sheet → semantic slice (gutter ile) |
| **L4 transition** | **Strateji B organic** | TemplateRect → AlphaIsland (V2) | Master atlas → multi-bucket slice |
| **L5 detail decal** | **Strateji B organic** | TemplateRect → AlphaIsland (V2) | Master atlas → high variant count |
| **L6 rift accent** | **Strateji B + curated hero** | TemplateRect | Master atlas → 1-2 hero + medium + small |

**Brush size kuralı (LOCK):**
> Brush radius **bir sprite'ı scale etmez** — native-size variant'lar arasından bucket-weighted pick yapar.
> Runtime non-integer scale **YASAK**. Integer scale **kaçınılmalı**.

**UX promise (LOCK):**
> User: "Paint gibi boyar." Sistem: bucket selection + atlas rules + minDistance + flip + weighted variation + composite layer coordination + walkable filter + edge bias — hepsini backend yapar. User Sprite Editor açmaz, slice rect çizmez, variant listesi yönetmez.

---

## 2. SIZE BUCKET SYSTEM (LOCK)

**Enum: `SizeBucket`** (5 değer)

| Bucket | Native size | Tipik kullanım |
|---|---|---|
| **Micro** | 32×32 | L5 ufak chip, pebble, küçük çatlak |
| **Small** | 64×64 | L4 küçük moss patch, L5 crack cluster |
| **Medium** | 128×128 | L4 orta patch, L3 corner cell, L5 medium debris |
| **Large** | 192×192 | L4 büyük patch, L3 corner large variant |
| **Hero** | 256×256 (veya 256-320 stretch) | L4 hero patch, L6 hero rift, L3 wall section |

> **Why these names?** ChatGPT'nin Micro/Small/Medium/Large/Hero adlandırması Codex'in XS/S/M/L/Hero'sundan daha okunabilir + asset isimlendirmesinde anlamı net (Micro=tek tile, Hero=focal piece).

### Brush Radius → Bucket Mapping (LOCK — ChatGPT soft overlap)

`BrushRadiusProfileSO` → table-driven, **soft overlap** (Aseprite/Polybrush hissi):

| Radius | Bucket weights |
|---|---|
| 1 | Micro 100% |
| 2 | Micro 70%, Small 30% |
| 3 | Small 80%, Micro 20% |
| 4 | Small 50%, Medium 50% |
| 5 | Medium 80%, Small 20% |
| 6 | Medium 50%, Large 50% |
| 7 | Large 80%, Medium 20% |
| 8 | Large 60%, Hero 40% |
| 9 | Hero 80%, Large 20% |
| 10 | Hero 100% |

**Hero gating:** `BrushAssetVariant.heroAllowed = true` ve `AssetPoolSO.heroLayerWhitelist` kontrolü zorunlu — Hero bucket sadece L4 (hero patch), L6 (hero rift), opsiyonel L3 wall section'da. L5'te Hero genelde KAPALI.

**Resolved divergence:** Codex hard cutoff (1→XS, 2→XS, 3→S, ..., 10→Hero) önerdi; ChatGPT soft overlap önerdi. **ChatGPT seçildi** çünkü:
1. Aseprite/Krita brush size davranışıyla uyumlu (smooth size transition)
2. Radius 4 ve 6'da yumuşak geçiş ani sıçramayı önler
3. Codex'in cutoff'u stress test'e tabi tutulduğunda radius 5 ile 6 arası pop'unu gizleyemez

---

## 3. DATA MODEL (LOCK)

### `BrushAssetVariant` (yeni — Sprint 6 BrushPack içinde)

```csharp
[Serializable]
public class BrushAssetVariant
{
    public Sprite sprite;
    public string variantId;          // unique within pool
    public SizeBucket bucket;
    public float weight;              // weighted random pick
    public TargetLayer targetLayer;   // L3/L4/L5/L6
    public Vector2Int nativeSize;
    public RectInt sourceRect;        // for sliced sprites; full for separate PNG
    public Vector2 pivot;
    public float footprintRadius;     // world-space placement radius
    public bool allowFlipX;
    public bool allowFlipY;
    public bool allowRotation;
    public float rotationSnapDegrees;
    public string[] tags;             // semantic tags for L3 (e.g. "corner_NE", "wall_horizontal")
    public bool respectsWalkableMask; // Karar #143-D enforcement
    public float minDistance;
    public float encounterAvoidRadius; // Karar #143-P
    public bool edgeBiased;
    public float wallProximityFactor;  // Karar #143-K
    public FeatureMaskSO featureMaskMultiplier; // optional
    public bool heroAllowed;          // gating for Hero bucket pick
}
```

### `AssetPoolSO` (extend existing — Sprint 1 data layer)

```csharp
public class AssetPoolSO : ScriptableObject
{
    public string poolName;
    public AssetCategory category;
    public List<BrushAssetVariant> variants;   // PRIMARY pool (replaces List<Sprite>)
    public Texture2D sourceMasterTexture;       // master sheet ref (null for separate PNG pools)
    public SliceLayoutTemplateSO importTemplate;
    public string namespacePrefix;              // V2 marketplace forward-compat
    public TargetLayer defaultTargetLayer;
    public BucketDefaultRules defaultBucketRules;
    public WeightDefaultRules defaultWeights;
    public bool[] heroLayerWhitelist;           // hero bucket gating per layer
}
```

### `SliceLayoutTemplateSO` (yeni)

Template-based slicing manifest. Importer bu template'e bakarak master texture'ı parçalar.

```csharp
public class SliceLayoutTemplateSO : ScriptableObject
{
    public string templateName;
    public Vector2Int masterSize;
    public List<SliceCell> cells;
    public int gutterSize;          // padding between slices, default 16-32
    public Vector2 defaultPivot;    // 0.5/0.5 center, can override per cell
}

[Serializable]
public class SliceCell
{
    public string cellName;          // semantic name → variantId source
    public RectInt rect;
    public SizeBucket bucket;        // can derive from rect.size, but explicit overrides
    public string[] tags;            // copied to BrushAssetVariant
    public Vector2? pivotOverride;
    public bool heroAllowed;
}
```

**Why SliceLayoutTemplateSO ScriptableObject?** Editable in Unity, version-controlled, can be reused (e.g. `L4_Organic_512` template uses for 5 different L4 master sheets — moss/dirt/erosion/broken-stone/wet-stain).

---

## 4. SLICE LAYOUT TEMPLATES (V1 STARTER SET)

### `L3_Horizontal_512x288.asset`
- Master: 512×288 (16:9)
- Cells: 2× horizontal wall pieces (256×288 each)
- Gutter: 16px horizontal padding between cells
- Tags: `wall_horizontal`, `wall_horizontal_A`, `wall_horizontal_B`
- Pivot: 0.5/0.5 (center) — orchestrator places mid-segment

**Why 256×288 cell (not 256×128)?** Master 512×288 ÷ 2 cell = 256×288 each. Logical wall sprite ~256×128 occupies bottom half of cell; top half is "ledge" + transparency padding (gutter against next cell's silhouette). Gutter prevents painter brush bleed at slice boundary.

### `L3_Vertical_512x512.asset` (alternative: 384×512)
- Master: 512×512
- Cells: 2× vertical wall pieces (256×512 each, gutter 16px between)
- Tags: `wall_vertical`, `wall_vertical_A`, `wall_vertical_B`

### `L3_Corners_512x512.asset` ⚠ GUTTER ZORUNLU
- Master: 512×512 with 4× 240×240 corner cells + 16px gutter cross
- Cells:
  - `corner_NE` rect(0, 0, 240, 240)
  - `corner_NW` rect(272, 0, 240, 240)
  - `corner_SE` rect(0, 272, 240, 240)
  - `corner_SW` rect(272, 272, 240, 240)
- Tags per cell: `corner_NE`, `corner_NW`, `corner_SE`, `corner_SW`
- Pivot: per-corner (NE pivot 1.0/0.0 = top-right, NW 0.0/0.0, etc.)
- **Validation rule:** gutter cross zone must be fully transparent. If validation fails → fallback to 4 separate 256×256 PNG dispatches (Strategy A explicit).

**Resolved divergence:** Codex "gutter şart yoksa A" uyarısı + ChatGPT "tek master 512×512" önerisi → **gutter zorunlu LOCK + fallback to A explicit on validation FAIL**. Validation tool bu durumu yakalar, user manuel müdahale gerekmez.

### `L3_Doorway_512x288.asset`
- Master: 512×288 (16:9)
- Cells: 2× doorway gap pieces (240×288 each, gutter 32px between)
- Tags: `doorway`, `doorway_A`, `doorway_B`
- Pivot: 0.5/0.0 (top-center) — doorway cap from above

### `L4_Organic_512.asset` ⭐ HERO TEMPLATE
- Master: 512×512
- Cells:
  - 1× hero patch 256×256 (top-left quadrant) — bucket Hero, tag `patch_hero`
  - 4× medium patches 128×128 (top-right + bottom-left quadrants split into 4) — bucket Medium, tag `patch_medium_{1..4}`
  - 8-12× small patches 64×64 (bottom-right quadrant + filler) — bucket Small, tag `patch_small_{1..12}`
  - Optional: 4-8× micro 32×32 fillers — bucket Micro, tag `patch_micro_{1..8}`
- Gutter: 16px between all cells
- Pivot: 0.5/0.5
- heroAllowed: TRUE (L4)
- **Total: 17-25 variants from 1 master = high variant density**

### `L5_Detail_512.asset`
- Master: 512×512
- Cells:
  - 4× medium 128×128 (top half) — bucket Medium, tag `detail_medium_{1..4}`
  - 12× small 64×64 (mid section, 4×3 grid) — bucket Small, tag `detail_small_{1..12}`
  - 16× micro 32×32 (bottom section, 8×2 grid) — bucket Micro, tag `detail_micro_{1..16}`
- Gutter: 8-16px (smaller than L4 because cells are denser)
- heroAllowed: FALSE (L5 doesn't use Hero bucket)
- **Total: 32 variants**

### `L6_Accent_512.asset`
- Master: 512×512
- Cells:
  - 1× hero rift fracture 320×320 (centered or offset) — bucket Hero, tag `rift_hero`
  - 4× medium rift cracks 96×96 (corners) — bucket Medium, tag `rift_medium_{1..4}`
  - 6-8× small sparks 48×48 (edges) — bucket Small, tag `rift_spark_{1..8}`
- Gutter: 16-24px (rift sprites need clear separation, no bleed)
- heroAllowed: TRUE (L6 hero is the design intent)
- **Total: 11-13 variants, sparse use**

---

## 5. PRODUCTION COST (LOCK)

### Eski plan (ayrı sprite per type)
- L3: 7 type × 2-4 pick = ~21 credit
- L4-L6: 5 call × variant pick = ~15 credit
- **Total: ~36 credit, 29 sprite dosyası**

### Yeni plan (Hybrid Auto-Slice master + slice)

| Layer | Master count | PixelLab dispatch | Variant output | Notes |
|---|---|---|---|---|
| L3 horizontal wall | 1-2 | 2 dispatch (1 master + 1 regen QC) | 4 named variants | TemplateRect 512×288 |
| L3 vertical wall | 1-2 | 2 dispatch | 4 named variants | TemplateRect 512×512 |
| L3 corners | 1-2 | 2 dispatch | 4 corners (NE/NW/SE/SW) | TemplateRect 512×512 + gutter validation |
| L3 doorway | 1 | 1 dispatch | 2 variants (A/B) | TemplateRect 512×288 |
| L4 moss | 1-2 | 2 dispatch | 17-25 variants | L4_Organic_512 template |
| L4 dirt | 1-2 | 2 dispatch | 17-25 variants | L4_Organic_512 template |
| L4 erosion/broken | 1-2 | 2 dispatch | 17-25 variants | L4_Organic_512 template |
| L5 cracks | 1 | 1 dispatch | 32 variants | L5_Detail_512 template |
| L5 rubble | 1 | 1 dispatch | 32 variants | L5_Detail_512 template |
| L6 rift | 1-2 | 2 dispatch | 11-13 variants | L6_Accent_512 template |
| **TOTAL minimum** | **10 master** | **~12-14 dispatch + QC** | **~150-200 variant** | |
| **TOTAL with QC regen** | — | **~18-24 dispatch** | — | Conservative estimate |

**Variant density artışı:** ~150-200 variant (eski plan 29) — **5-7x daha fazla görsel çeşitlilik aynı dispatch sayısında**.

**Credit tahmini:** Create Image Pro web UI dispatch maliyeti henüz user tarafında bilinen değil; şu an MCP credit cinsinden tahmin yok. Eski 36 credit ölçeği yerine, **dispatch sayısı (12-24)** metriği baz alınmalı.

---

## 6. IMPORTER SPEC — `BrushAtlasImporter`

**Yeni Editor utility.** Sprint 6'ya ek olarak (veya ayrı mini-sprint) implement edilecek.

### Workflow (user-facing)
1. User PixelLab web UI'da master üretir (örn. L4 moss 512×512)
2. PNG'yi `Assets/Art/BrushAtlas/Intake/L4_moss_master.png`'ye drop eder
3. Unity menü: **RIMA → Brush → Import Atlas** (veya right-click PNG → Import as Brush Atlas)
4. Açılan dialog:
   - Pool name (auto-suggest from filename: "L4_moss")
   - SliceLayoutTemplateSO seçimi (dropdown: L4_Organic_512, L5_Detail_512, ...)
   - Target layer (auto from template)
   - Namespace prefix (default: empty for V1)
5. Importer çalışır:
   - Texture import settings'i lock'lar (Point, no compression, no mipmaps, PPU=32)
   - Sprite Mode = Multiple
   - Slice rect'leri SliceLayoutTemplateSO'dan üretir
   - Her slice için BrushAssetVariant oluşturur
   - AssetPoolSO'yu yazar/günceller
   - Validation çalıştırır
6. Sonuç dialog:
   - "✅ 17 variant imported, AssetPool 'L4_moss' updated" veya
   - "⚠ 3 validation warnings (see console)"

### Importer responsibilities (LOCK)

```csharp
public static class BrushAtlasImporter
{
    public static ImportResult Import(
        string pngPath,
        SliceLayoutTemplateSO template,
        string poolName,
        TargetLayer layer,
        string namespacePrefix);

    // 1. Validate PNG exists and is readable
    // 2. Set TextureImporter settings:
    //    - textureType = Sprite
    //    - spriteImportMode = Multiple
    //    - filterMode = Point
    //    - textureCompression = None
    //    - mipmapEnabled = false
    //    - spritePixelsPerUnit = 32
    //    - spritePivot from template default
    // 3. Reimport texture (forces meta update)
    // 4. Build SpriteMetaData[] from template.cells
    // 5. Apply via TextureImporter.spritesheet
    // 6. AssetDatabase.ImportAsset (refresh)
    // 7. Load resulting Sprite assets
    // 8. Build BrushAssetVariant per cell:
    //    - sprite = loaded Sprite
    //    - variantId = $"{namespacePrefix}{poolName}_{cell.cellName}"
    //    - bucket from cell.bucket (or derive from rect.size if null)
    //    - tags from cell.tags
    //    - pivot from cell.pivotOverride or template default
    //    - sourceRect from cell.rect
    //    - nativeSize from rect.size
    //    - heroAllowed from cell.heroAllowed
    //    - footprintRadius = max(rect.width, rect.height) * 0.5f / PPU
    //    - default weight = 1.0
    //    - allowFlipX/Y from layer defaults
    //    - respectsWalkableMask = true (Karar #143-D)
    //    - minDistance/edgeBiased/wallProximityFactor from layer presets
    // 9. Find or create AssetPoolSO at $"Assets/Art/BrushAtlas/Pools/{poolName}.asset"
    // 10. Update AssetPoolSO:
    //     - sourceMasterTexture = loaded texture
    //     - importTemplate = template ref
    //     - variants = built list
    //     - SetDirty + SaveAssets
    // 11. Run BrushAtlasValidator (separate utility)
    // 12. Return ImportResult { variantCount, warnings, errors }
}
```

### Validation checklist (LOCK — `BrushAtlasValidator`)

13 check (ChatGPT'den + Codex'in gutter check'i ile birleşik):

1. ✅ Transparent background exists (sample 4 corner pixels alpha < 8)
2. ✅ No sprite touches atlas border (rect inset ≥ gutter on all sides)
3. ✅ Minimum padding between sprite islands/cells (≥ template.gutterSize)
4. ✅ No visible rectangular frame (cell border pixels alpha < 16)
5. ✅ All variants have bucket assigned
6. ✅ All variants have targetLayer
7. ✅ All variants have weight > 0
8. ✅ All variants have pivot
9. ✅ All variants have footprintRadius > 0
10. ✅ No duplicate variant IDs within pool
11. ✅ Texture import: filterMode = Point
12. ✅ Texture import: textureCompression = None, mipmapEnabled = false
13. ✅ Gutter cross zone (for L3 corner template) is fully transparent — fallback hint if FAIL

**FAIL → console error + warning dialog.** User decides: regenerate master, edit template, or accept (override).

### Preview Tool — `BrushVariantPreviewWindow`

Editor window:
- Pool dropdown (all AssetPoolSO in project)
- Variant grid grouped by bucket (Micro/Small/Medium/Large/Hero rows)
- Per variant card: sprite preview, weight, pivot dot, footprint radius circle, tags
- "Sample stroke preview" button → simulates 50 picks with current radius profile, shows distribution histogram
- "Test brush" button → opens brush window with this pool selected

**Why preview window?** User'ın 32-200 variant'ı manuel inspect etmesi YASAK. Validation pass etse bile görsel doğrulama için tek tıkla sample.

---

## 7. KARAR #143 INTEGRATION (LOCK — execution order)

`CompositeStrokeExecutor` (Sprint 6 LIVE) içinde her brush operation için:

```
1. Stroke point list al (click veya drag)
2. BrushPreset'ten target operations resolve et
3. Her operation için:
   a. Walkable mask check (Karar #143-D) — wall cell'e patch düşmez
   b. encounterAvoidRadius check (Karar #143-P) — combat alanına düşmez
   c. wallProximityFactor / edgeBiased weight (Karar #143-K)
   d. FeatureMaskSO multiplier (Aşama 2)
   e. density evaluation (AnimationCurve from preset, Q3 LOCK)
   f. BrushRadiusProfileSO resolve → bucket weights
   g. Allowed buckets filter (heroAllowed gate)
   h. Weighted pick BrushAssetVariant
   i. minDistance / footprintRadius rejection check
   j. flip policy (allowFlipX/Y) random apply
   k. rotation policy (allowRotation, rotationSnapDegrees) random apply
   l. Place sprite via existing painter (WallOverlayPainter / TransitionBrushPainter / DetailDecalPainter / AccentPainter)
4. Undo group register (single undo per stroke)
```

**No painter rewrite.** Mevcut Karar #143 LIVE painter'lar dokunulmaz; executor sadece BrushAssetVariant'ı painter'ın beklediği signature'a adapt eder.

---

## 8. V1 IMPLEMENT IMPACT (LOCK — Sprint impact analizi)

### Sprint 1 (Data Layer) — değişiklik gerekiyor
- `AssetPoolSO` extend: `List<Sprite>` yerine `List<BrushAssetVariant>` (backwards compat: V0 List<Sprite> → migration utility)
- `SizeBucket` enum yeni
- `TargetLayer` enum yeni (L3/L4/L5/L6)
- `BrushRadiusProfileSO` yeni
- `SliceLayoutTemplateSO` yeni
- `BrushAssetVariant` class yeni

**Effort: 1 sprint (Codex task — spec-following).**

### Sprint 6 (BrushPack + Composite Executor) — adjust gerekiyor
- Mevcut 12 default brush'ın AssetPool referansları yeni `BrushAssetVariant` yapısına migrate
- `CompositeStrokeExecutor` adımları yukarıdaki Karar #143 sıralamasına göre yeniden sıralanır
- Bucket-aware variant pick logic (weighted from BrushRadiusProfileSO)
- minDistance / footprintRadius spatial rejection
- Hero gating

**Effort: 1 sprint (Codex task + Opus QC — spec-following with judgment for Hero gating decisions).**

### Sprint 7 (Auto-Dress + Regenerate + Smart Fill) — adjust gerekiyor
- Auto-Dress: aynı bucket weights kullanır, hand stroke ile auto stroke arası tutarlılık
- Regenerate: stroke seed + radius bucket + layer role koru, variant swap
- Smart Fill: bucket presets + wallProximityFactor signal kullanır (raw radius değil)

**Effort: 0.5 sprint adjustment.**

### Sprint 8 (BiomeSkin) — değişiklik YOK
- BiomeSkin material/shader katmanı, BrushAssetVariant ile etkileşimi sadece source texture sample seviyesinde

### Yeni Sprint 9 (BrushAtlasImporter + Validator + Preview) — eklenecek
- `BrushAtlasImporter` static utility (yukarıda spec)
- `BrushAtlasValidator` static utility (13 check)
- `BrushVariantPreviewWindow` editor window
- 5 starter SliceLayoutTemplateSO asset (L3_Horiz, L3_Vert, L3_Corners, L3_Doorway, L4_Organic, L5_Detail, L6_Accent)
- Right-click PNG context menu integration

**Effort: 1.5 sprint (Opus implement — UI + import logic, taste calls for default values).**

**Total V1 adjustment: ~3 sprint additional (1 Sprint 1 retrofit, 1 Sprint 6 retrofit, 0.5 Sprint 7 retrofit, 1.5 new Sprint 9). Existing V1 LIVE work NOT discarded.**

---

## 9. V1 / V2 SPLIT (LOCK)

### V1 (RIMA shipping)
- Hybrid Auto-Slice (this plan)
- 5 size buckets + soft overlap radius mapping
- BrushAssetVariant data model
- BrushAtlasImporter + Validator + Preview window
- 7 starter SliceLayoutTemplateSO
- TemplateRect slicing only
- Manual master generation (PixelLab web UI Create Image Pro)
- No runtime scale, no SpriteAtlas batching yet

### V2 (post-ship ecosystem)
- AlphaIsland slicing (organic auto-detect)
- SpriteAtlas integration for batching
- Brushpack import/export (folder + zip)
- Namespace prefix conflict resolver
- Marketplace-ready brush packs
- Biome library (cross-biome reuse)
- Standalone migration (RIMA-independent brush tool)
- Automated PixelLab dispatch via MCP (when 512px endpoint lands)

---

## 10. UX PROMISE GUARANTEE (LOCK — non-negotiable)

User workflow (post-V1):
1. PixelLab web UI'da Create Image Pro ile master üret (örn. L4 moss 512×512, SliceLayoutTemplateSO `L4_Organic_512`'ye uygun layout)
2. PNG'yi `Assets/Art/BrushAtlas/Intake/` altına drop et
3. Unity'de PNG'ye sağ tık → "Import as Brush Atlas" → template seç → done
4. Brush window'da pool'u seç → hotkey B → sahnede boyamaya başla
5. Hotkey `[` `]` ile brush size değiştir → uygun bucket variant'ları otomatik pick
6. Composite brush seç ("Mossy Broken Edge") → tek stroke ile L2+L4+L5 aynı anda boyanır

**User'ın YAPMADIĞI:**
- ❌ Slice rect çizmek
- ❌ Variant listesi yönetmek
- ❌ Layer routing seçmek
- ❌ Asset bucket atamak
- ❌ Density/edge bias hesaplamak
- ❌ minDistance ayarlamak
- ❌ Flip / rotation policy ayarlamak
- ❌ Walkable mask kontrolü yapmak
- ❌ Sprite Editor açmak
- ❌ TextureImporter ayarlarına dokunmak
- ❌ AssetPoolSO'yu manuel düzenlemek

**Backend'in YAPTIĞI:**
- ✅ Texture import settings auto-lock
- ✅ Slice rect template'den auto-generate
- ✅ Bucket assignment from rect.size
- ✅ Variant metadata auto-build
- ✅ AssetPoolSO auto-populate
- ✅ Validation auto-run + warning dialog
- ✅ Brush radius → bucket weights resolve (BrushRadiusProfileSO)
- ✅ Karar #143 atlas rules application (walkable, encounterAvoid, edgeBias, minDistance)
- ✅ Composite layer coordination
- ✅ Undo group registration

---

## 11. CONVERGENCE / DIVERGENCE LOG

### ChatGPT + Codex AGREED (LOCK)
- Hybrid Auto-Slice strategy
- L3 = structural/explicit (semantic), L4-L6 = master + slice (organic)
- Runtime scale FORBIDDEN
- Brush size = bucket pick (NOT scale)
- 5 size buckets
- BrushAssetVariant metadata pattern
- Backend metadata extension (Sprint 6 BrushPack)
- UX promise: user paints, system handles
- Sprite Editor not exposed to user
- Validation required
- Texture import preset lock (Point/no compression/no mipmaps)
- V2 forward-compat (marketplace, namespace)
- Karar #143 painter integration (no painter rewrite)
- BrushAtlasImporter pattern

### DIVERGENCE — RESOLVED
| Konu | Codex | ChatGPT | LOCK |
|---|---|---|---|
| Bucket isim | XS/S/M/L/Hero | Micro/Small/Medium/Large/Hero | **Micro/Small/Medium/Large/Hero** (okunabilir) |
| Radius mapping | Hard cutoff (1=XS, 5=M, 10=Hero) | Soft overlap (radius 4 = 50/50) | **Soft overlap** (Aseprite/Polybrush feel) |
| L3 corner master | "Gutter şart, yoksa A explicit" (temkinli) | "Tek master 512×512 + 4 cell + tag" (optimist) | **Tek master + gutter zorunlu + Validator gutter check + FAIL fallback to A** |
| Importer adı | implied "deterministic manifest" | explicit "BrushAtlasImporter" | **BrushAtlasImporter** (explicit kazanır) |
| Slice template | implied | explicit "SliceLayoutTemplateSO" | **SliceLayoutTemplateSO** (explicit kazanır) |
| Validation | "validate" generic | 13-item checklist + "Validate" button | **13-item checklist + button** (concrete) |
| Preview | mentioned passing | explicit "BrushVariantPreviewWindow" | **Explicit window** (UX promise için şart) |
| L5 layout | "24-40 slices" range | 4× 128 + 12× 64 + 16× 32 = 32 explicit | **32 explicit slices** (template-driven) |
| Production cost | 9-14 dispatch (biome starter) | 10-12 minimum, 18-24 with QC | **12-14 minimum, 18-24 with QC** (orta) |
| Reference patterns | shipped games (Hades, Dead Cells, HLD) | Unity tools (Random Brush, Polybrush, Tiled, LDtk) + games | **Both** (game examples + tool patterns) |

### DIVERGENCE — UNRESOLVED (need user call)
- **L3 horizontal wall master boyutu:** Codex "688×384 OR 512×288"; ChatGPT "512×288 OR 688×384". **Default önerisi: 512×288** (4 variant slice'a daha rahat sığar). User L3 wall görsel detay isterse 688×384 (3 variant slice).
- **L4 master count per biome:** Codex "1-2 dispatch per biome material pair"; ChatGPT "1 dispatch per biome master, 1-2 with regen". **Default: 1 dispatch per material type (moss, dirt, erosion separate). 1 biome starter = 3 L4 master = 3 dispatch.**
- **PixelLab Create Image Pro credit cost per dispatch:** unknown. User confirm needed. Eski MCP credit ölçeği geçersiz.

---

## 12. NEXT STEPS (action list)

### Bu plan onaylanırsa:

1. **Memory update:**
   - `project_brush_tool_v1.md` → "Sprint 9 added: BrushAtlasImporter + Validator + Preview" + V1 retrofit notu
   - `reference_pixellab_create_image_pro.md` → master template önerileri tablosu güncelle
   - Yeni: `project_hybrid_auto_slice_strategy.md` → bu doc'un memory özeti

2. **Batch dosya revize:**
   - `STAGING/pixellab_l3_wall_batch.md` → master + template formatına revize (4 master dispatch yerine 7 sprite type)
   - `STAGING/pixellab_l4_l5_l6_batch.md` → master atlas formatına revize (10 master toplam)
   - Her batch'te: Create Image Pro prompt formülü + slice template referansı

3. **Codex task spec yaz (Sprint 1 retrofit):**
   - `STAGING/codex_brush_sprint9_atlas_importer.md`:
     - BrushAssetVariant + AssetPoolSO migration
     - SliceLayoutTemplateSO 7 starter asset
     - BrushAtlasImporter + Validator + Preview window
     - 5 size bucket enum + BrushRadiusProfileSO default presets

4. **User üretim sırası:**
   - L3 horizontal master ilk (production gate — oda kapanmadan brush V1 test edilemez)
   - Sonra L3 vertical → L3 corner → L3 doorway
   - L4-L6 paralel (composite brush daha sonra)

5. **QC gate:**
   - Her master üretim sonrası BrushAtlasImporter çalıştır → BrushAtlasValidator otomatik check
   - Preview window'da gözle doğrula
   - FAIL → master regen (Create Image Pro)
   - PASS → BrushPack'e ekle, brush window'dan test paint

---

## 13. AUTHORITATIVE REFERENCES

- ChatGPT cevabı (kullanıcıdan paste, 2026-05-16 S86 turn)
- Codex cevabı: `STAGING/codex_sprite_strategy_review.md`
- Original prompt: `STAGING/sprite_strategy_review_prompt.md`
- Brush V1 design spec: `STAGING/map_designer_unified_brush_design.md`
- Karar #143 pipeline: [[karar-143-layered-pipeline]] memory
- Brush V1 progress: [[brush-tool-v1-design]] memory
- PixelLab Create Image Pro spec: [[pixellab-create-image-pro]] memory
- PixelLab MCP tool inventory: [[pixellab-tool-inventory]] memory

---

**LOCK seal:** Bu plan ChatGPT + Codex cross-review convergence'ı + Opus harmanlaması sonucu. Aksi karar verilmedikçe Sprint 1 retrofit + Sprint 9 implement bu spec'e göre yapılacak.
