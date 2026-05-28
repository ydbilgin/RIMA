---
status: PROPOSED
faz: 1-1.5+
tarih: 2026-05-14
ozet: "ChatGPT Room Designer feedback değerlendirme + 5 yeni Karar (#126-130) Organic Room Dressing Pipeline + Stamp Library + WangResolver + Biome Preset + Validator"
source: rima-design Opus (orchestrator direct karar, user güveni)
---

# Organic Room Dressing Pipeline — Karar #126-#130 Proposal

ChatGPT feedback (2026-05-14) RIMA Room Designer için kapsamlı bir "Organic Room Dressing Pipeline" önerdi. Bu doküman feedback'i değerlendirir, mevcut RIMA Karar'larıyla overlap'i çıkarır, 5 yeni Karar önerisi sunar.

## 1. ChatGPT Feedback Özeti

**Önerilen sistem (BEŞLİ):**
1. PixelLab = tile/prop görseli üretir
2. Unity RuleTile/Wang Resolver = doğru tile'ı seçer (Unity-side logic)
3. Room Dressing Pipeline = doğallık post-process
4. **Stamp/Cluster Library = elle yapılmış hissi** ← ChatGPT'nin "en kritik" işaretlediği
5. Naturalness Validator = QA

**9-Stage Pipeline (A-J):**
- A. Base Terrain (47-tile autotile)
- B. Edge Naturalization (40%+ ragged, Perlin)
- C. Cliff (Wall Front/Top + drop shadow)
- D. Decal (distance-based: edge/cliff/prop)
- E. Scatter (blue-noise/Poisson)
- F. **Stamp/Cluster** (handcrafted mini-layouts SO)
- G. Path/Readability (main path clear, combat clear)
- H. Shadow Grounding
- I. Y-Sort Integration
- J. Naturalness Validator

## 2. RIMA Mevcut Karar Overlap

| ChatGPT spec | RIMA mevcut Karar | Overlap |
|---|---|---|
| Base 47-tile autotile | #118 Hybrid Tile Composition + #115 deterministic generator | ✅ 80% |
| Edge Naturalization 40%+ | #116(a) Raggedness ≥40% | ✅ 95% |
| Cliff Wall Front/Top | Antigravity 4 P0.3 + #116(f) | ✅ 90% (dispatch'te) |
| Decal Pass | #118 Decal layer + #116(d) | ✅ 70% (distance-based logic eksik) |
| Scatter Brush | #121 Scatter Brush (Perlin, 4 kategori) | ✅ 85% (Poisson eksik) |
| Shadow Grounding | Antigravity 4 P0.2 Drop Shadow Layer 1.5 | ✅ 95% (dispatch'te) |
| Y-Sort | Antigravity 4 P0.1 (0,1,0) | ✅ 100% (dispatch'te) |
| **Stamp Library** | — | ❌ **TAMAMEN YENİ** |
| **Tile Metadata SO** | Kısmen #118 (4-layer label) | ⚠️ 30% |
| **WangResolver Unity-side** | #115 deterministic + #116 Wang | ⚠️ 50% (Unity-side custom logic eksik) |
| **Path/Readability Pass** | — | ❌ **YENİ** |
| **Biome Preset SO** | #115 biome enum (RimaBiomeType) | ⚠️ 40% (enum→SO yükseltme gerek) |
| **Naturalness Validator** | — | ❌ **YENİ** |

**Sonuç:** %60-70 mevcut Karar'larla kapsanmış, %30-40 önemli yeni ekleme.

## 3. 5 Yeni Karar Önerisi

### Karar #126 — Organic Room Dressing Pipeline (UMBRELLA) — PROPOSED 2026-05-14

**Konu:** RoomGenerator/RoomDesigner output'una post-process 9-stage organic dressing pipeline. Tek umbrella karar, alt-stage'leri Karar #127-#130 ve mevcut #115-#121 implement eder.

**9 Stage:**
1. **Base Terrain Pass** — Karar #115 + #118 Base layer + WangResolver (Karar #128)
2. **Edge Naturalization Pass** — Karar #116(a) Raggedness 40%+ + Perlin variant select (Karar #128 logic)
3. **Cliff Pass** — Antigravity 4 P0.3 Wall Front/Top + cliff edge shadow decals
4. **Decal Pass** — Karar #118 Decal layer + distance-based density (edge/cliff/prop yakın yüksek)
5. **Scatter Pass** — Karar #121 Scatter Brush + Poisson/blue-noise upgrade
6. **Stamp/Cluster Pass** — Karar #127 handcrafted mini-layouts
7. **Path/Readability Pass** — Karar #130 main path/combat zone clear validator
8. **Shadow Grounding Pass** — Antigravity 4 P0.2 Drop Shadow + per-prop oval shadow
9. **Y-Sort Integration** — Antigravity 4 P0.1 (0,1,0) sort axis
10. **Naturalness Validator** — Karar #130 QA pass (repeated tile detect, straight border, dense scatter, missing shadow)

**Faz mapping:**
- **Faz 1 P0:** Stage 1, 2, 3, 8, 9 (mevcut Karar'lar + Antigravity dispatch)
- **Faz 1.5 P1:** Stage 4 (distance Decal), 5 (Poisson Scatter), 6 (Stamp scaffold), 7 (Path), 10 (Validator basic)
- **Faz 2+ P2:** Full validator, advanced stamps, biome preset matrix

**Implementation:** Sequential pipeline, each stage gated. Stage output → next input. Seed-deterministic for reproducibility.

### Karar #127 — Stamp/Cluster Library — PROPOSED 2026-05-14

**Konu:** Handcrafted mini-layout ScriptableObject sistemi. ChatGPT'nin "elle yapılmış hissi"nin tek kaynağı dedi.

**Stamp SO içeriği:**
- Local tile overrides (terrain layer)
- Decals
- Props (with collision)
- Scatter points
- Optional sockets (spawn-anchor for mob/loot)
- Allowed biome (multi-select)
- Allowed terrain
- Allowed elevation condition
- Min/max distance from room center
- Min/max distance from path
- Weight + rarity
- Footprint size (clear area requirement)

**F1 Faz 1.5 Stamp seti (8 minimum):**
1. Cliff corner rubble cluster
2. Mossy stone patch
3. Broken pillar with debris
4. Dead tree with root/moss base
5. Crystal bloom (Karar #98 cyan+violet rift)
6. Ruined altar corner
7. Cracked path segment
8. Empty breathing-space marker (NO stamp, gameplay clarity)

**Placement logic (RoomGenerator post-process):**
- Roll N stamps per room (biome-dependent density)
- For each: pick valid stamp (filter by biome/terrain/elevation), find valid position (footprint clear, path clear), place
- Avoid blocking doors, spawn zones, combat center

**Asset path:** `Assets/RoomDesigner.Core/Stamps/` (generic SO) + `Assets/Scripts/Systems/Map/RimaStamps/` (F1-specific instances)
**Karar #117 Portable Core compliance:** Stamp SO definition in Core, F1 instances in Game layer.

### Karar #128 — Tile Asset Metadata SO + WangTileResolver — PROPOSED 2026-05-14

**Konu:** PixelLab tile sheet'lerine Unity-side metadata + autotile resolver. ChatGPT: "PixelLab visual üretir, Unity logic karar verir."

**TileAssetMetadata SO:**
```csharp
public class TileAssetMetadata : ScriptableObject {
  public string tileId;
  public TerrainType terrainType;   // F1: Stone, Moss, Cracked, Corruption
  public BiomeType biomeType;       // RimaBiomeType #115
  public EdgeSignature north, east, south, west;  // Wang seam pattern
  public EdgeSignature ne, se, sw, nw;             // corner signatures
  public string variantGroup;       // weighted variant set
  public float weight;
  public bool supportsCollision;
  public bool isCliffTop;
  public bool isCliffFront;
  public bool isTransition;
  public bool decalAllowed;
  public bool scatterAllowed;
  public bool shadowRequired;
  public int sortingLayer;
}
```

**WangTileResolver:**
- Input: cell position + neighbor terrain types + seed
- Output: tile candidate + variant index
- Logic:
  1. Compute edge mask (NSEW + corners) from neighbors
  2. Filter TileAssetMetadata candidates by terrain + edge signature
  3. Weighted random select by `variantGroup` + `weight` (deterministic seed)
  4. Return tile + sprite index
- Support 47-tile blob (single-terrain), 16-tile NSEW (basic Wang), 256-tile NSEW+corners (advanced)
- Karar #115 deterministic compliance: same seed → same output

**Karar #118 TileImportWizard entegrasyon:**
- TileImportWizard (mevcut dispatch'te) sheet slice + sprite generate
- Yeni step: TileAssetMetadata SO auto-create per sheet entry (PixelLab JSON'dan edge signature parse)
- WangResolver runtime'da metadata lookup

**Faz 1 P0 scope:** Metadata SO + 16-tile NSEW resolver (Karar #118 base level)
**Faz 1.5 P1:** 47-tile blob extension
**Faz 2 P2:** 256-tile corner + decal/scatter allowed filtering

### Karar #129 — Biome Preset SO — PROPOSED 2026-05-14

**Konu:** Karar #115 `RimaBiomeType` enum'unu ScriptableObject preset sistemine yükselt. ChatGPT'nin Biome Preset eklentisi.

**BiomePreset SO içeriği:**
- Allowed base terrains (TileAssetMetadata refs)
- Transition tile sets
- Decal sets
- Scatter sets (Karar #121 categories)
- Prop cluster library (Karar #127 stamp refs)
- Color mood tags (palette anchor)
- Density ranges (decal/scatter/stamp per cell)
- Cliff style preset
- Moss/lichen behavior
- Corruption/magic effects

**F1 başlangıç preset'leri:**
1. **Shattered Keep** (mevcut F1 main) — stone + moss + cracked, cliff-heavy, Penitent palette
2. **Mossy Ruins** — moss-dominant variant, low cliff
3. **Ashen Cliff** — Ravager hint biome, vertical cliff focus

**F2-F4 preset'leri (Faz 2+):** Rift-Corrupted Stone, Overgrown Path, Crystal Wound

**Asset path:** `Assets/RoomDesigner.Core/BiomePresets/` (generic) + `Assets/Scripts/Systems/Map/RimaBiomes/` (RIMA F1-F4)
**Karar #117 Portable Core compliance:** SO definition Core, F1-F4 concrete Game layer.

### Karar #130 — Naturalness Validator + Path Readability — PROPOSED 2026-05-14

**Konu:** Editor QA tool ve Path/Readability Pass. ChatGPT'nin "saçmalıkları yakala" sistemi.

**Naturalness Validator detects:**
- Repeated identical tiles >N in same neighborhood (radius 3)
- Straight terrain borders >N tiles linear (no Raggedness)
- Missing transition tiles (terrain mismatch hard cut)
- Missing cliff fronts (elevation drop no Wall_Front)
- Props blocking critical paths
- Scatter density >N% in playable zone
- Decals >N% in playable zone
- Empty rooms with <N visual interest score (variety count)
- Overdecorated rooms (>N% coverage)
- Missing shadows under major props

**Debug overlays (Scene view gizmo):**
- Terrain type map (color-coded)
- Elevation map (heatmap)
- Transition mask (highlight Wang seams)
- Scatter density mask
- Prop collision footprint
- Path clearance (main path overlay)
- Y-sort pivot visualization

**Path Readability Pass:**
- Main door-to-door path detect (A* shortest path)
- Combat center detect (largest open area)
- Clear minimum walkable width (>= 2 tiles)
- Enemy spawn/arena zones clear
- Stamp/scatter blocking → reject placement (Karar #127 footprint validation)
- Sample 10 random walks from entry → exit, all valid

**Asset path:** `Assets/Editor/Tools/NaturalnessValidator.cs` + `Assets/RoomDesigner.Core/PathReadability.cs`
**Faz 1.5 P1 scope:** Basic validator (repeated tile, straight border, missing transition) + Path Pass main door-to-door
**Faz 2 P2:** Full validator + debug overlays + advanced naturalness scoring

## 4. Cross-System Conflict Audit

| Mevcut Karar | Yeni #126-#130 | Conflict |
|---|---|---|
| #115 Map Builder deterministic | #126 pipeline + #128 WangResolver | NONE — seed-deterministic preserved |
| #116 Tile Transition Quality | #126 Edge Naturalization Pass + #128 metadata | NONE — Raggedness 40% enforced |
| #117 Portable Core | All #126-#130 | OK — Core/Game ayrımı uygulanmalı (her SO için Core abstract + Game concrete) |
| #118 4-layer Tilemap | #126 stage 1-6 + #128 metadata | NONE — layer mantığı korunur, metadata layer üzerinde |
| #119 AI ASCII Parser | #126 pipeline | NONE — ASCII parser pre-pipeline (room layout), pipeline post (dressing) |
| #121 Scatter Brush | #126 stage 5 + #128 metadata | OK — Scatter Brush retained, Poisson/blue-noise upgrade |
| Antigravity 4 P0 | #126 stage 3, 8, 9 | NONE — Antigravity P0 = pipeline stages |

**Conflict: NONE.** Tüm yeni Karar'lar mevcut spec'leri extends, breaks none.

## 5. Implementation Priority

### P0 — Faz 1 (mevcut dispatch'lerle paralel)
- **Karar #128 base** — TileAssetMetadata SO + 16-tile NSEW WangResolver
  - Codex dispatch: Karar #118 TileImportWizard (mevcut, dispatch ediliyor) — metadata SO entegrasyon EKLE
- **Antigravity 4 P0** (Karar #126 stage 3, 8, 9) — Codex dispatch (mevcut, dispatch ediliyor)
- **Karar #129 F1 preset** — 1 BiomePreset SO (Shattered Keep) — manuel SO create, 1 saat

### P1 — Faz 1.5 polish
- **Karar #127 Stamp Library** scaffold + 8 F1 stamp
  - Codex dispatch ~6-8h
- **Karar #128 47-tile blob** extension
  - Codex dispatch ~3-4h
- **Karar #130 Path Readability Pass** basic
  - Codex dispatch ~4-5h
- **Karar #129 Mossy Ruins + Ashen Cliff** preset

### P2 — Faz 2+
- Karar #130 full Validator + debug overlays
- Karar #128 256-tile corner Wang
- F2-F4 biome presets (Karar #129)

## 6. Sonraki Adım Önerisi

**Şimdi (paralel devam):**
- 3 Codex dispatch (Animation Step 2 + Antigravity 4 P0 + Karar #118 TileImportWizard) — running

**Sonraki turn (P0 tamamlama):**
- **Codex dispatch Karar #128 entegrasyon:** TileAssetMetadata SO + WangResolver (Karar #118 dispatch'inin extension'ı). ~4-5h.

**Faz 1.5 başlangıcında:**
- **Codex dispatch Karar #127 Stamp Library** + 8 F1 stamp scaffold (P1 öncelik — ChatGPT en kritik dedi)
- **rima-design Opus** F1 8 stamp visual brief (3 mob brief gibi)
- **rima-asset** stamp asset PixelLab prompt

**Karar #126-#130 LOCK için:**
- User onayı sonrası MASTER_KARAR_BELGESI.md entry ekle (Codex lint pattern)
- FAZ_MASTER sync update
- Memory file: user-memory `project_organic_pipeline.md` + `project_stamp_library.md`

## 7. rima-design Opus Final Önerisi

**LOCK önerisi:**
- Karar #126 (Pipeline umbrella) — LOCK
- Karar #127 (Stamp Library) — LOCK (ChatGPT haklı, en yüksek görsel ROI Faz 1.5'te)
- Karar #128 (Metadata + Resolver) — LOCK (Karar #118 extension olarak natural fit)
- Karar #129 (Biome Preset) — LOCK (Karar #115 enum yükseltmesi)
- Karar #130 (Validator + Path) — LOCK (Faz 1.5 P1, Faz 2 polish)

**Faz 1 25-gün scope etkisi:**
- Karar #128 P0 base ~4-5h Codex Faz 1'e sığar (Karar #118 zaten dispatch'te, extension)
- Karar #129 F1 preset 1 saat manual
- **Karar #127 ve #130 Faz 1.5 P1** — Faz 1 deadline'a etki YOK
- ChatGPT'nin önerdiği tüm sistem ~30-40 saat Faz 1.5 sprint (P1 öncelik)

**Conflict:** NONE — 5 yeni Karar mevcut #115-#121 + Antigravity 4 P0 spec'lerini extend, kırmaz.

**rima-design VERDICT (Opus, orchestrator direct):** LOCK 5 karar, ama Faz 1 deadline koruma için #127 ve #130 Faz 1.5'e bırak. User onayı sonrası MASTER_KARAR entry + Codex dispatch P0 (#128).
