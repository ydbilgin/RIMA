# Sprite Strategy FINAL LOCK — RIMA Brush V1 Single Source of Truth

**Date:** 2026-05-16 S86 PREP-3
**Status:** LOCK — bu dosya Brush V1 sprite stratejisinin TEK YETKİ kaynağıdır. Çelişen önceki tüm önerileri override eder.
**Authority chain:** ChatGPT (mimari + UX) + Codex initial review (production cost + bucket) + Codex meta-review (sprint sequencing + risk) → Opus harmanlama.
**Replaces / supersedes:**
- `STAGING/sprite_strategy_FINAL_PLAN.md` (ilk konsolide — bu LOCK ile güncellenir, plan dosyası kalır referans olarak)
- `STAGING/codex_sprite_strategy_review.md` (kaynak)
- `STAGING/codex_meta_review.md` (kaynak)
- Eski Sprint 9 plan'ı (importer-only) — yeni: importer + RoomTemplate stub
- Eski "L3 wall semantic 7 type" planı — yeni: **Wang Full 16 corner set**

---

## 0. CORE LOCKS (one-liner reference)

| Karar | LOCK |
|---|---|
| Stratejy | **Hybrid Auto-Slice** (Strategy A semantic L3 wall + Strategy B atlas slice L4-L6) |
| L3 wall topoloji | **Wang Full 16 corner set** (eski 7-type semantic plan iptal) |
| Variant pick | **Native size bucket** — runtime non-integer scale YASAK |
| Bucket isim | **Micro / Small / Medium / Large / Hero** |
| Radius mapping | **Soft overlap** (Aseprite/Polybrush feel, BrushRadiusProfileSO table-driven) |
| Karakter yön | **8-direction** (5 produce + 3 mirror — body simetrik, weapons child) |
| Composition primary model | **Composition Roles** (clean center / decorated edge / focal cluster / wall band / door safety / encounter avoid) |
| Room storage | **Editor-only RoomBank** (NO runtime procedural) |
| Room MVP target | **20-30 oda/tip** (40-60 combat sonraki gate, 100 hedef YOK V1) |
| RoomTemplate model | **SO + Prefab hybrid** (pure SO YASAK, scene-per-room YASAK) |
| Validator severity | **Error / Warning / Info** (art-quality FAIL blocking YASAK) |
| Sprint sequence | **9 → 10 (RoomBank vertical slice) → 11 (Natural Engine) → 12 (Props) → 13 (Hardening)** |
| Markov clustering | **DEFER V2** (Spelunky sub-templates yeterli) |
| AI tag suggestion | **DEFER** (template tags + Preview override) |
| SpriteAtlas | **DEFER V2** (folder/label structure per-biome packing-ready) |
| 2 P0 retrofit | **scaleRange runtime non-integer scale + sorting layer mismatch** (Patch+Scatter only, Detail/Accent/Props/Entities eksik) |
| Encounter ownership | **RoomTemplate ≠ Encounter** (room expose sockets/tags, EncounterBank ayrı combine) |
| Deterministic IDs | **Variant + Room + Prop + Socket IDs stable + human-readable** |
| Workflow (16-18 May) | **Opus implement → Codex review** (limit yüksek, Codex idle) |

---

## 1. STRATEGY: HYBRID AUTO-SLICE

| Layer | Üretim modeli | Slice modu | Variant kaynağı | Output |
|---|---|---|---|---|
| L1 floor base | mevcut (Tilemap 32×32) | — | mevcut spritesheet | unchanged |
| L2 floor variation | mevcut (Tilemap 32×32) | — | mevcut spritesheet | unchanged |
| **L3 wall overlay** | **Strategy A semantic** | TemplateRect (named cells + tags) | PixelLab `create_topdown_tileset` (Wang chain) | **Wang Full 16 corner set** (16 base tile) |
| **L4 transition** | **Strategy B organic** | TemplateRect → AlphaIsland (V2) | Master atlas (Create Image Pro) → multi-bucket slice | 17-25 variants/master |
| **L5 detail decal** | **Strategy B organic** | TemplateRect → AlphaIsland (V2) | Master atlas → high variant count | 32 variants/master |
| **L6 rift accent** | **Strategy B + curated hero** | TemplateRect | Master atlas → 1-2 hero + medium + small | 11-13 variants/master |

**Rationale (üç perspektif birleşimi):**
- Codex (initial): L3 explicit/A, L4-L6 master+slice/B → reviewable structural pieces + low-cost decorative variety
- ChatGPT: aynı sonuç, soft overlap + Micro/.../Hero naming + BrushAtlasImporter explicit
- Codex meta: **L3 wall semantic = Tiled Wang numerology** → 7-type plan eksik, 16-case zorunlu. UX promise tek bir master + auto-slice ile korunur (user slice rect çizmez)

---

## 2. L3 WALL — WANG FULL 16 (TOPOLOGY LOCK)

**Eski plan (REJECTED):** 7 semantic type — horizontal / vertical / corner_NE / corner_NW / corner_SE / corner_SW / doorway

**Yeni LOCK:** Wang Full 16 corner set — her hücre 4 köşesinin (NE/NW/SE/SW) wall|floor durumuna göre kodlanır → 2^4 = 16 case.

```
Wang corner numerology (4-bit, NE-NW-SE-SW):
0000 = all floor (skip, L1 zaten halleder)
0001 = SW corner only
0010 = SE corner only
0011 = south edge (S wall)
0100 = NW corner only
0101 = west edge (W wall)
0110 = diagonal NW+SE (rare, anti-diagonal)
0111 = U-shape open N (S+E+W walls, N floor)
1000 = NE corner only
1001 = diagonal NE+SW (rare, diagonal)
1010 = east edge (E wall)
1011 = U-shape open W
1100 = north edge (N wall)
1101 = U-shape open E
1110 = U-shape open S
1111 = all wall (enclosed cell, rare)
```

**Production via PixelLab MCP:**
- Tool: `mcp__pixellab__create_topdown_tileset`
- Mode: **Lower (floor) → Upper (wall) chain** — `transition_size 1.0` (full wall transition)
- tile_size: **32**
- **Output reality (S86 PREP-3 verified dispatch):** PixelLab `tileset15` format → 15 unique tile (4×4 PNG grid 128×128, format includes all-floor reference). L3 AssetPool imports **15 wall/transition variant**, 0000 (all-floor) skipped as L1 reference.
- Eski "16 base tile + transition slice" wording İPTAL (Codex review M5 finding) — actual PixelLab output esas alınır.
- 1 tileset = 1 biome wall set (ShatteredKeep first, sonra 4 biome daha — V2 chain)

**SliceLayoutTemplate:** `L3_Wang16_Topdown.asset`
- Wang generator template-driven (BrushAtlasImporter Wang-aware slicing)
- **Canonical bit order: NE-NW-SE-SW** (corner-major)
- Variant tag pattern: **`wang_{ne}{nw}{se}{sw}`** (örn. `wang_0011` = NE=0, NW=0, SE=1, SW=1 → south edge wall)
- Bucket: tüm Wang tiles **Micro** (32×32) → L3 tile-grid placement
- heroAllowed: FALSE (L3 Wang non-hero)
- **Import scope:** 15 wall+transition variant (0000 all-floor case → `all_floor_reference` tag, AssetPool'a girmez, L1 zaten sağlar)

**Vertical slice scope (M5-M6):** Sadece **1 Wang tileset** üretilecek (ShatteredKeep biome, Floor → Wall basic). 4 ek biome chain DEFER → vertical slice loop PASS sonrası.

**FALLBACK (Wang generator FAIL):** Strategy A explicit — 16 ayrı 32×32 PNG dispatch. BrushAtlasValidator FAIL detector tetiklenirse otomatik fallback hint user'a gösterilir.

---

## 3. SIZE BUCKET SYSTEM (LOCK)

**Enum: `SizeBucket`** (5 değer)

| Bucket | Native size | Tipik kullanım |
|---|---|---|
| **Micro** | 32×32 | L3 Wang tile, L5 ufak chip, küçük çatlak |
| **Small** | 64×64 | L4 küçük moss patch, L5 crack cluster |
| **Medium** | 128×128 | L4 orta patch, L5 medium debris |
| **Large** | 192×192 | L4 büyük patch |
| **Hero** | 256×256 (256-320 stretch) | L4 hero patch, L6 hero rift |

### Brush Radius → Bucket Mapping (LOCK — Soft overlap)

`BrushRadiusProfileSO` table-driven:

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

**Hero gating:** `BrushAssetVariant.heroAllowed = true` AND `AssetPoolSO.heroLayerWhitelist[layer] = true`. L5 default Hero KAPALI.

---

## 4. DATA MODEL (LOCK)

### `BrushAssetVariant` (Sprint 9)

```csharp
[Serializable]
public class BrushAssetVariant
{
    public Sprite sprite;
    public string variantId;          // unique, human-readable: {pool}_{tag} (e.g. "L4_moss_patch_hero")
    public SizeBucket bucket;
    public float weight;              // default 1.0
    public TargetLayer targetLayer;   // L3/L4/L5/L6
    public Vector2Int nativeSize;
    public RectInt sourceRect;        // for sliced sprites; full for separate PNG
    public Vector2 pivot;
    public float footprintRadius;     // world-space placement radius
    public bool allowFlipX;
    public bool allowFlipY;
    public bool allowRotation;
    public float rotationSnapDegrees;
    public string[] tags;             // semantic tags (e.g. "wang_0011", "corner_NE")
    public bool respectsWalkableMask; // Karar #143-D enforcement
    public float minDistance;
    public float encounterAvoidRadius; // Karar #143-P
    public bool edgeBiased;
    public float wallProximityFactor;  // Karar #143-K
    public FeatureMaskSO featureMaskMultiplier; // optional
    public bool heroAllowed;
    public string schemaVersion;      // V1: "1.0"
}
```

### `AssetPoolSO` (extend existing)

```csharp
public class AssetPoolSO : ScriptableObject
{
    public string poolName;
    public AssetCategory category;
    public List<BrushAssetVariant> variants;   // PRIMARY (replaces legacy List<Sprite> — migration utility provided)
    public List<Sprite> legacySprites;          // backwards compat, V1 deprecation warning
    public Texture2D sourceMasterTexture;       // master sheet ref (null for separate PNG pools)
    public SliceLayoutTemplateSO importTemplate;
    public string namespacePrefix;              // V2 marketplace forward-compat
    public TargetLayer defaultTargetLayer;
    public BucketDefaultRules defaultBucketRules;
    public WeightDefaultRules defaultWeights;
    public bool[] heroLayerWhitelist;           // per-layer hero gating
    public string assetGuid;                    // stable GUID, importer preserves on reimport
}
```

### `SliceLayoutTemplateSO`

```csharp
public class SliceLayoutTemplateSO : ScriptableObject
{
    public string templateName;
    public Vector2Int masterSize;
    public List<SliceCell> cells;
    public int gutterSize;          // padding between slices
    public Vector2 defaultPivot;
    public bool wangAware;          // L3 Wang 16 template flag
}

[Serializable]
public class SliceCell
{
    public string cellName;          // semantic name → variantId source
    public RectInt rect;
    public SizeBucket bucket;
    public string[] tags;
    public Vector2? pivotOverride;
    public bool heroAllowed;
}
```

### `RoomTemplateV1` (Sprint 9 stub, Sprint 10 full)

```csharp
public class RoomTemplateSO : ScriptableObject
{
    public string schemaVersion;        // "1.0"
    public string roomId;               // human-readable stable: "combat_shatteredkeep_001"
    public string biomeId;
    public RoomType roomType;           // Combat / Shop / Shrine / Elite / Boss
    public RectInt bounds;              // tile-space room bounds
    public List<DoorSocket> doorSockets;
    public PlayerSpawnSocket playerSpawn;
    public List<EnemySpawnSocket> enemySpawnSockets;
    public CameraBounds cameraBounds;
    public GameObject prefabRef;        // hybrid: SO + prefab
    public List<string> encounterTags;   // RoomTemplate ≠ Encounter; tags only
    public List<string> difficultyTags;
    public List<string> blockerTags;
}
```

**Encounter decoupling:** RoomBank ∪ EncounterBank → runtime combine. 40 oda × N encounter = combinatorial variety. Yeni room kategorisi eklenirse encounter logic'i kırılmaz.

---

## 5. SLICE LAYOUT TEMPLATES (V1 STARTER SET)

| Template | Master | Cell count | Layer | Notes |
|---|---|---|---|---|
| `L3_Wang16_Topdown.asset` | PixelLab tileset output | 16 base | L3 | Wang corner numerology, Micro bucket, 32×32 |
| `L4_Organic_512.asset` ⭐ | 512×512 | 1 Hero 256 + 4 Medium 128 + 8-12 Small 64 + 4-8 Micro 32 = 17-25 variants | L4 | gutter 16, heroAllowed TRUE |
| `L5_Detail_512.asset` | 512×512 | 4 Medium 128 + 12 Small 64 + 16 Micro 32 = 32 variants | L5 | gutter 8-16, heroAllowed FALSE |
| `L6_Accent_512.asset` | 512×512 | 1 Hero 320 + 4 Medium 96 + 6-8 Small 48 = 11-13 variants | L6 | gutter 16-24, heroAllowed TRUE |

**Validation rule (Wang template special):** Each Wang variant cell border must be transparent (gutter inset ≥ 1px) so tile-grid adjacency doesn't bleed visually. FAIL → user dialog with regen prompt.

---

## 6. PRODUCTION COST (VERTICAL SLICE → FULL)

### Vertical slice (M5-M8 scope)
- 1 Wang tileset (ShatteredKeep, Floor + Wall basic) — PixelLab `create_topdown_tileset` 1 dispatch
- Optional: 1 L4 master if loop test needs decoration (DEFER, vertical slice = walkable+exit yeter)
- **Total:** ~1-2 dispatch

### Full V1 (vertical slice PASS sonrası)

| Layer | Master count | Dispatch | Variant output |
|---|---|---|---|
| L3 Wang | 5 biome × 1 tileset | 5 + QC regen | 16 × 5 = 80 Wang tiles |
| L4 moss + dirt + erosion | 3 master per biome | 3 + QC | 17-25 × 3 = 51-75 variants |
| L5 cracks + rubble | 2 master per biome | 2 + QC | 32 × 2 = 64 variants |
| L6 rift | 1-2 master per biome | 1-2 + QC | 11-13 × 1-2 = 11-26 variants |
| **PER BIOME (5 biome total)** | ~10 master | **12-15 dispatch** | **~200-250 variant/biome** |

**Variant density:** Eski plan ~29 sprite. Yeni ~200-250 variant/biome → **~8x görsel çeşitlilik** aynı dispatch cost'unda.

---

## 7. BRUSH ATLAS IMPORTER (Sprint 9)

### Workflow (user-facing)
1. PixelLab web UI'da master üret (örn. L4 moss 512×512) VEYA MCP'den Wang tileset üret
2. PNG `Assets/Art/BrushAtlas/Intake/L4_moss_master.png`'ye drop
3. Unity menü: **RIMA → Brush → Import Atlas** (veya right-click PNG)
4. Dialog: Pool name + SliceLayoutTemplateSO + Target layer + Namespace prefix
5. Importer:
   - Texture import settings LOCK (Point, no compression, no mipmaps, PPU=32)
   - Sprite Mode = Multiple
   - Slice rect'leri template'den üret
   - BrushAssetVariant'lar oluştur
   - AssetPoolSO yaz/güncelle (existing GUID preserve)
   - Validation çalıştır (severity: Error/Warning/Info)
6. Sonuç dialog: variant count + severity list

### Importer Responsibilities (LOCK)

```csharp
public static class BrushAtlasImporter
{
    public static ImportResult Import(
        string pngPath,
        SliceLayoutTemplateSO template,
        string poolName,
        TargetLayer layer,
        string namespacePrefix);

    // 1. Validate PNG readable
    // 2. Set TextureImporter (Sprite Multiple, Point, no compression, no mipmaps, PPU=32)
    // 3. Build SpriteMetaData[] from template.cells (Wang generator if template.wangAware)
    // 4. Apply via TextureImporter.spritesheet
    // 5. AssetDatabase.ImportAsset (refresh)
    // 6. Build BrushAssetVariant per cell with deterministic variantId
    // 7. Find or create AssetPoolSO at "Assets/Art/BrushAtlas/Pools/{poolName}.asset"
    //    — preserve existing GUID, in-place update
    // 8. Update AssetPoolSO + SetDirty + SaveAssets
    // 9. Run BrushAtlasValidator
    // 10. Return ImportResult { variantCount, errors, warnings, infos }
}
```

### Validator (LOCK — severity-driven)

**Error (BLOCK import):**
- Unreadable PNG
- Wrong master size for template
- Duplicate variant IDs within pool
- Missing bucket / targetLayer
- Non-Point filterMode
- textureCompression ≠ None or mipmapEnabled = true
- L3 Wang gutter cross fail (corner template — auto-fallback hint)

**Warning (advise, allow import):**
- Palette drift detected
- AA suspicion (alpha gradient on outline)
- Outline drift between variants
- Low alpha noise inside cells
- Border proximity (variant within 4px of master edge but technically usable)
- Weight = 0 (variant will never pick)

**Info (advisory):**
- Variant count per bucket
- Estimated memory footprint (texture size × variants)
- Hero/non-hero ratio
- Tag coverage report

### Preview Window — `BrushVariantPreviewWindow`

- Pool dropdown
- Variant grid grouped by bucket (Micro/Small/Medium/Large/Hero rows)
- Per variant: sprite preview, weight, pivot dot, footprint radius circle, tags
- "Sample stroke preview" button → 50 picks at radius 1/4/7/10 → distribution histogram
- "Test brush" button → opens brush window with this pool selected

---

## 8. KARAR #143 INTEGRATION (LOCK — execution order)

`CompositeStrokeExecutor` (Sprint 6 LIVE) içinde, her brush operation için:

```
1. Stroke point list al
2. BrushPreset → target operations resolve
3. Per operation:
   a. Walkable mask check (Karar #143-D)
   b. encounterAvoidRadius check (Karar #143-P)
   c. wallProximityFactor / edgeBiased weight (Karar #143-K)
   d. FeatureMaskSO multiplier
   e. density evaluation (AnimationCurve)
   f. BrushRadiusProfileSO resolve → bucket weights
   g. Allowed buckets filter (heroAllowed gate)
   h. Weighted pick BrushAssetVariant
   i. minDistance / footprintRadius spatial rejection
   j. flip policy random apply
   k. rotation policy random apply
   l. Place via existing painter (no painter rewrite)
4. Undo group register (single undo per stroke)
```

**No painter rewrite.** Existing painters (WallOverlayPainter / TransitionBrushPainter / DetailDecalPainter / AccentPainter) dokunulmaz; executor BrushAssetVariant'ı painter signature'ına adapt eder.

---

## 9. P0 RETROFITS (Sprint 9 içinde — BLOCKING)

### R1: scaleRange runtime non-integer scale violation

**Current bug:** `BrushLayerOperation.scaleRange = 0.85..1.15` ve `DecorativeExecutorUtility.PlaceAt()` arbitrary scale uyguluyor → pixel art blur, bucket strategy invalidate.

**Fix (Sprint 9 scope):**
- Yeni variant path: native sprite size kullan, **scale uygulama**
- `scaleRange` legacy-only field, yeni path bypass eder
- New path: bucket selection only (radius → bucket weights → pick variant)
- Editor warning: "scaleRange deprecated for new variant path" (legacy migration için)

### R2: Sorting layer mismatch

**Current state:** `RimaSortingLayerValidator` sadece `Patch` ve `Scatter` valide ediyor. Decorative executors `Patch`, `Detail`, `Accent` emit ediyor → mismatch.

**Fix (Sprint 9 scope):**
- Validator extend: `Patch`, `Detail`, `Accent`, `Props`, `Entities`, `UI`, `Lights` (full ordering rules)
- All decorative executors validate against extended layer list
- Editor menu: "RIMA → Brush → Validate Sorting Layers" — fail-fast on missing layer

---

## 10. SPRINT 9-13 SEQUENCE (LOCK)

### Sprint 9 — Importer + Metadata Retrofit + Room Contract Stub
- **Owner:** Opus implement → Codex review (16-18 May window)
- **Estimate:** 1.5-2 day
- **Scope:**
  - SizeBucket + BrushAssetVariant + BrushRadiusProfileSO + SliceLayoutTemplateSO + SliceCell + ImportResult + ValidationIssueSeverity enums/classes
  - AssetPoolSO extend (variants list + legacy sprites + GUID preserve)
  - BrushAtlasImporter (Wang-aware slicing for L3 template)
  - BrushAtlasValidator (Error/Warning/Info)
  - BrushVariantPreviewWindow
  - 4 starter SliceLayoutTemplateSO (L3_Wang16 + L4_Organic_512 + L5_Detail_512 + L6_Accent_512)
  - RoomTemplateV1 contract stub (no impl, just data model)
  - 2 P0 retrofits (scaleRange + sorting layer)
  - EditMode tests (importer → variant pick path, validator severity, no runtime scale)
- **Exit criteria:**
  - 1 test master → AssetPoolSO with variants (no compile error, dotnet build PASS)
  - Validator returns severity list
  - Bucket pick path has no runtime scale
  - All existing Brush V1 tests still pass (37 currently green)
- **Forbidden:** RoomBank impl, save/load impl, full Natural Engine (deferred to Sprint 10+)

### Sprint 10 — Minimal RoomTemplate + RoomBank Vertical Slice
- **Owner:** Opus implement → Codex review
- **Estimate:** 1-1.5 day
- **Scope:**
  - RoomTemplateSO V1 full (yukarıda spec)
  - RoomBankSO (room type lists, deterministic random pick by seed, validation report)
  - Save current authoring root → room prefab + metadata asset (deterministic child naming)
  - Load RoomTemplate into clean authoring scene
  - Runtime test loader: RoomBank.Pick → spawn player + 1 placeholder enemy + exit valid
  - Validation: exactly 1 player spawn + ≥1 exit + camera bounds contains walkable + no enemy in prop footprint + dependencies exist
- **Exit criteria:**
  - 1 room → paint/import → save → reload → RoomBank pick → PlayMode load → exit valid
  - Save/load roundtrip test passes
  - No editor-only class runtime dependency

### Sprint 11 — Natural Placement Engine MVP
- **Owner:** Codex implement, Opus composition-role presets
- **Estimate:** 1-1.5 day
- **Scope:**
  - CompositionRoleMap (cleanCenter / decoratedEdges / focalZones / doorSafety / encounterAvoid / wallBand)
  - Bridson Poisson (bounded room grid, cell size r/√2, 3×3 neighbor query)
  - Density mask sampling (reuse FeatureMaskSO)
  - Anti-repetition variant history per layer/pool
  - Exact-count placement for Hero/Focal ops
  - Debug overlays (density heatmap, accepted/rejected points, minDistance circles, role zones)
- **Forbidden:** Markov clustering, AI tag suggestion
- **Exit criteria:** Auto-Dress room with no grid cadence, clean combat center, readable exits, deterministic seed replay

### Sprint 12 — Props Mode MVP
- **Owner:** Codex implement, User/Opus prop categories
- **Estimate:** 1-1.5 day
- **Scope:**
  - PropDefinitionSO (sprite/prefab + propCategory + footprint + pivot/sorting anchor + snap mode + collision + interactable type + combatBlocker + destroyable + lootSource + spawnAvoidRadius + biome tags)
  - Props tab in Brush window (palette + ghost preview + place/select/move/delete/duplicate/flip/rotate)
  - Props save into RoomTemplate prefab hierarchy with stable instance IDs
  - Footprint preview + spawn/camera validation integration
- **Exit criteria:** 3 prop categories placed + save/load PASS + enemy/player spawn avoids footprints

### Sprint 13 — Production Hardening + Batch Gate
- **Owner:** Codex tooling/tests, User first batch, Opus final go/no-go
- **Estimate:** 1 day initial (extend if metrics fail)
- **Scope:**
  - Performance smoke (200/500/1000 decorative sprites)
  - Undo stress (100+ placement composite stroke)
  - Dependency report (missing sprites/templates/GUID/sorting/scale/props)
  - Room thumbnail generation
  - ID generator for rooms/variants/props/sockets
  - First production batch: 5 combat + 1 shop + 1 shrine + 1 elite + 1 boss placeholder
- **Exit criteria:** 10-room library playable through RoomBank, time per room documented

---

## 11. V1 / V2 SPLIT (LOCK)

### V1 (RIMA shipping)
- Hybrid Auto-Slice
- 5 size buckets + soft overlap
- BrushAssetVariant data model
- BrushAtlasImporter + Validator + Preview
- 4 starter SliceLayoutTemplateSO
- TemplateRect slicing (+ Wang-aware for L3)
- Manual master generation (PixelLab web UI / MCP)
- RoomTemplateSO + RoomBank vertical slice
- Composition Roles + Poisson + density mask + anti-repeat + exact-count
- Props Mode MVP
- 20-30 oda/tip starter library
- 2 P0 retrofits done

### V2 (post-ship)
- AlphaIsland slicing (organic auto-detect)
- SpriteAtlas per-biome
- Markov clustering (if needed)
- AI tag suggestion
- Brushpack import/export (folder + zip)
- Namespace prefix conflict resolver
- Marketplace-ready brush packs
- Biome library cross-biome reuse
- Standalone migration (RIMA-independent)
- Automated PixelLab MCP dispatch
- 40-60 combat rooms / 100 total

---

## 12. UX PROMISE GUARANTEE (LOCK — non-negotiable)

### User workflow (post-V1)
1. PixelLab MCP / web UI'da master üret
2. PNG `Assets/Art/BrushAtlas/Intake/` altına drop
3. Unity'de right-click → "Import as Brush Atlas" → template seç → done
4. Brush window'da pool seç → hotkey B → boya
5. `[` `]` brush size → bucket variants auto pick
6. Composite brush → tek stroke ile L2+L4+L5 boyanır

### User YAPMADIĞI
- ❌ Slice rect çizmek
- ❌ Variant listesi yönetmek
- ❌ Layer routing seçmek
- ❌ Asset bucket atamak
- ❌ Density/edge bias hesaplamak
- ❌ minDistance ayarlamak
- ❌ Flip/rotation policy ayarlamak
- ❌ Walkable mask kontrolü yapmak
- ❌ Sprite Editor açmak
- ❌ TextureImporter ayarlarına dokunmak
- ❌ AssetPoolSO manuel düzenlemek
- ❌ Wang corner case'i hesaplamak

### Backend YAPTIĞI
- ✅ Texture import settings auto-lock
- ✅ Slice rect template'den (+ Wang generator) auto-generate
- ✅ Bucket assignment from rect.size
- ✅ Variant metadata auto-build
- ✅ AssetPoolSO auto-populate + GUID preserve
- ✅ Validation auto-run + severity dialog
- ✅ Brush radius → bucket weights resolve
- ✅ Karar #143 atlas rules (walkable, encounterAvoid, edgeBias, minDistance)
- ✅ Composite layer coordination
- ✅ Undo group register
- ✅ Wang 16 case auto-resolve from tile-grid context

---

## 13. KARAKTER 8-DIRECTION LOCK (S86 PIVOT — daha önceki 4-dir lock OVERRIDE)

**Production:**
- 5 sprite ÜRET: S, N, E, SE, NE
- 3 yön MIRROR (Unity): W = E flipX, SW = SE flipX, NW = NE flipX
- Body **simetrik** (asymmetric features YASAK — gözbağı, tek omuz pad, asymmetric capes vb. KAPALI V1)
- Weapons **ayrı child SpriteRenderer** attach edilir → asymmetric weapon OK (silah child, mirror'la birlikte taşınır)

**Karakter durumu (Glob audit S86):**
- Warblade / Elementalist / Ranger / Shadowblade → 4-yön idle var (legacy, **8-yön regen için işaretli**)
- Gunslinger → 8-yön zaten var (KEEP)
- Ravager / Brawler / Ronin / Summoner / Hexer → sprite eksik (üretim listesi)

**Regen audit (DEFER vertical slice sonrası):** 5 batch × ~9 sprite (idle + run + 3 attack frame × 5 yön çoğunlukla).

---

## 14. WORKFLOW OVERRIDE (16-18 May 2026 ONLY)

**Reason:** Claude Code Opus limit aşırı yüksek, Codex idle. Geçici inversion.

| Normal | Override (16-18 May) |
|---|---|
| Codex implement, Opus review | **Opus implement, Codex review** |
| Codex spec-following | Codex final-pass review |
| Opus judgment/architecture | Opus implement + judgment |

**Cutoff:** 18 May 2026 sonrası → eski routing'e geri dön ([[codex-vs-opus-split]]).

**Codex review dispatch:** `cx_dispatch.py --task-file STAGING/codex_review_*.md --effort high` (background) → CODEX_DONE_*.md'den sonuç oku.

---

## 15. UNRESOLVED / USER CONFIRM NEEDED

| Konu | Status | User input gerekli |
|---|---|---|
| PixelLab `create_topdown_tileset` credit cost | unknown | Vertical slice dispatch sonrası user reporting |
| L3 Wang biome chain → 5 mi 3 mü? | tentative 5 | Vertical slice loop PASS sonrası karar |
| ShatteredKeep biome ilk mi? | LOCK | — |
| L4 master count per biome | tentative 3 (moss/dirt/erosion) | Sprint 11 öncesi |
| Karakter regen schedule | DEFER | Vertical slice + Sprint 13 sonrası |

---

## 16. AUTHORITATIVE REFERENCES

- `STAGING/sprite_strategy_FINAL_PLAN.md` (kaynak — bu LOCK ile birlikte yaşar, çatışmada bu LOCK kazanır)
- `STAGING/codex_sprite_strategy_review.md` (Codex initial cross-review)
- `STAGING/codex_meta_review.md` (Codex meta + sprint sequencing)
- `STAGING/sprite_strategy_review_prompt.md` (original prompt to ChatGPT/Codex)
- Memory: [[brush-tool-v1-design]] [[karar-143-layered-pipeline]] [[camera-angle-revisit-s43]] [[8dir-mirror-production-strategy]] [[codex-as-reviewer-until-may18]] [[pixellab-tool-inventory]] [[pixellab-create-image-pro]] [[pixellab-character-via-web-ui-v3]]
- Sprint task specs (yazılacak): `STAGING/codex_brush_sprint9_atlas_importer.md`, `STAGING/codex_brush_sprint10_room_template_bank.md`
- PixelLab L3 Wang spec (yazılacak): `STAGING/pixellab_l3_wang_chain.md`

---

## 17. LOCK SEAL

Bu doküman Brush V1 sprite stratejisinin **TEK YETKİ KAYNAĞI**dır. Aksi karar verilmedikçe (user override veya yeni meta-review), tüm Sprint 9-13 implementation + asset üretimi bu spec'e göre yapılacak.

**Codex review required:** Bu LOCK doc'unun kendisi Codex'e review gönderilecek (M1 PASS gate). Codex'ten BLOCKING concern dönerse → revize + Opus final decision.

**Non-negotiable:**
- Hybrid Auto-Slice
- Wang 16 L3 wall
- Native size bucket (no runtime scale)
- Sprint 10 RoomBank vertical slice BEFORE Natural Engine
- Severity-driven validator
- RoomTemplate ≠ Encounter
- 8-dir character lock
