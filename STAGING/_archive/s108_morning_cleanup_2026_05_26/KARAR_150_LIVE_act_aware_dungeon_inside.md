# Karar #150 LIVE — Act-Aware Dungeon-Inside Architecture

**Status:** **LIVE** (Codex APPROVE_WITH_REVISIONS applied)
**Date:** 2026-05-19 S94 LATE NIGHT
**Codex verdict:** `STAGING/CODEX_DONE_karar_150_review.md` — 8 revisions integrated below
**Supersedes:** v3 diamond-corner-cut constraint (kaldırıldı)
**Concept reference:** `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`

---

## 1. What was decided

RIMA dungeon görünümü **"fake isometric + dungeon-inside"** tekniği ile yapılır. Her Act bu mimari paterni kendi tematik palet/material ile yorumlar. Diamond room constraint kaldırıldı — yerine **internal-architecture primary, irregular layout** kuralı LOCK.

### 1.0 Karar #149 vs #150 conflict reconciliation (Codex revision #1)

**Karar #150 OVERRIDES** Karar #149'un yalnızca **default visual canvas size**'ını (16×10 → ~32×22). Karar #149'un encounter semantics'i **PRESERVED**:

| Karar #149 semantics | Status |
|---|---|
| Combat/Elite macro node = 1 EncounterTemplateSO × 3-5 RoomTemplateSO sub-room | ✅ Preserved |
| Sub-room transitions = internal fade-to-black, NO DungeonGraph advance | ✅ Preserved |
| No per-sub-room reward (no skill draft/map fragment/Echo Imprint) | ✅ Preserved |
| Macro reward fires only after final required sub-room cleared | ✅ Preserved |
| EncounterBank holds enemy identity + threat budget (RoomTemplateSO holds sockets/tags only) | ✅ Preserved |
| Save/load: no mid-encounter save in MVP | ✅ Preserved |
| **Default sub-room canvas: 16×10** | **❌ OVERRIDDEN → ~32×22 (v4 evidence)** |
| 12×8 allowed for connectors / ambush pockets / low-threat transitions | ✅ Preserved (still valid for connector slot) |

### 1.1 Core visual rules (all acts)

| Kural | Spec |
|---|---|
| **Map silhouette** | Irregular rectangular, ~32×22 default sub-room (16×10 was v3 fail). Karar #149 16×10 sub-room SUPERSEDED by larger 32×22 inside-feel canvas |
| **Perimeter walls** | Sadece 1 köşe/edge'de görünür VEYA off-screen — frame'in %75'i internal architecture |
| **Internal walls primary** | Min 2 free-standing pillar + 1 archway connector + 1 collapsed stub per sub-room |
| **Wall depth** | Fake isometric — top cap + front face + base shadow gradient. Flat sprite YASAK |
| **Floor angle** | 35° tilt (Karar #100) + 0° Y rotation. Tilt camera DEĞİL, art baked |
| **Character orientation** | 8-dir (Karar #114 LOCK 2026-05-13): 5 sprite üret (S, SE, E, NE, N) + 3 mirror (W←E, SW←SE, NW←NE) |
| **Diamond constraint** | KALDIRILDI — irregular layout primary, octagonal silhouette opsiyonel |
| **Door connection** | Sağ-alt / sol-üst archway → sub-room transition (Karar #149 fade-to-black) |

### 1.1.5 MapLayerOrchestrator API — actual vs claimed (Codex revision #2)

**Doğru API:** `MapLayerOrchestrator.Paint(Tilemap floorTilemap, RimaBiomePreset biome, RoomData room, int seed)` — NOT `Paint(RoomTemplateSO)`.

**Mevcut SubRoomSequenceController (Karar #149 shipped):** `RoomTemplateSO.backgroundLayers` (Karar #147 Multi-Layer Painter) ile authored composition kullanıyor. **6-layer procedural paint DEĞİL.**

**Karar #150 MVP path (LOCKED):**
- Her sub-room → **authored `BackgroundLayerData[]`** (Karar #147 LIVE)
- L1-L6 6-layer procedural painting **deferred** until `RoomTemplateSO → RoomData` adapter shipped
- Faz 1-5 (skeleton/floor/decoration/lighting/post-FX) iş authored data üretir
- Faz 6 (variant) procedural compose adapter introduce edilebilir

**Karar #143 6-layer painter** asset üretim taxonomy'sini guide eder (16+16+24+24+18+12 spec) ama runtime'da MVP authored composition uses these assets directly.

### 1.2 Karar #149 sub-room integration (door-through mechanic)

User direktifi: *"sağ alttaki kapıdan geçince o mapin 2. bölümü açılacak ama tutarlı birbirine bağlantılı şekilde olmalı"*

**Mekanik:**
- Combat/Elite macro node = 1 `EncounterTemplateSO` containing 3-5 ordered `RoomTemplateSO` sub-rooms
- Sub-room 1 cleared → archway gate glow → player walks through → fade-to-black → sub-room 2 loads
- DungeonGraph DOES NOT advance — same encounter, internal continuation
- Final sub-room cleared → reward sequence (skill draft / map fragment) fires

**Visual consistency (tutarlılık):**
- Same Act material (Act 1 → cool granite, Act 2 → corrupted bog, Act 3 → void substrate)
- Same ambient lighting palette (Act 1 cyan, Act 2 violet/rust, Act 3 gold/void)
- Same wall class set (5 classes per Act)
- Same scatter/decal asset pool
- Variant via composition + density + accent placement

**Logical connection (birbirine bağlantılı):**
- Sub-room 1 archway exit → sub-room 2 archway entry MATCH (mirror placement)
- Storytelling continuity: same architectural complex (Act 1 = keep, Act 2 = hive, Act 3 = sanctum)
- Optional debris trail / blood trail / footprints between sub-rooms (authored L5 decals in `RoomTemplateSO.props` or `BackgroundLayerData` — NOT a new `EncounterTrailSO`)

### 1.2.5 Archway mirror geometry — validator rules (Codex revision #3)

`EncounterTemplateValidator` mevcut: `Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs`. Mevcut check'ler: socket resolution + reachability. **Eksik:** mirror geometry.

**Ekle (one code edit, no new architecture):**

| Validator rule | Severity |
|---|---|
| `SubRoomLink.fromDoorSocket.direction` ile `toEntryDoorSocket.direction` **inverse** olmalı (N↔S, E↔W) | Error |
| İki socket compatible edge'lerde olmalı (top↔bottom, left↔right) | Error |
| `widthInTiles` match (tolerance ±1) | Warning |
| Relative placement mirrored (sub-room N exit X-pos ≈ sub-room N+1 entry X-pos, tolerance ±2 cell) | Warning |

Bu kural archway exit→entry "sağ-alt → sol-üst" mirror discipline'ı authoring time'da enforce eder.

### 1.2.6 Sub-room slot grammar — `subRoomTag` canonical strings (Codex revision #4)

Mevcut `SubRoomEntry.subRoomTag` field **string**, ayrı enum YOK. Karar #150 enum eklemez (premature) — bunun yerine canonical string values lock:

| Canonical tag | Slot purpose |
|---|---|
| `entry_chamber` | Player spawn, low threat or 1st wave |
| `pillar_arena` | Combat zone, 2-3 internal pillars |
| `collapse_corridor` | Connector, ambush pocket, narrow chokepoint |
| `ritual_hall` | Boss sub-room, hero rift accent |
| `crypt_cell` | Optional reward / mystery, small 12×8 |

`RoomTemplateSO.roomType` macro taxonomy (Combat/Elite/Boss/etc.) DEĞİŞMEZ — slot grammar `EncounterTemplateSO.sequence[i].subRoomTag` üzerinden. Production authoring friction çıkarsa Phase 2'de `SubRoomSlotType` enum eklenebilir.

### 1.3 Layout grammar (sub-room types within encounter)

Karar #149 sub-room sequence = composed of typed sub-room slots:

| Slot type | Purpose | Layout cue |
|---|---|---|
| **Entry chamber** | Player spawn, low threat or 1st wave | Wide opening, single archway to next |
| **Pillar arena** | Combat zone, 2-3 internal pillars for cover | Open center, perimeter pillars |
| **Collapse corridor** | Connector, ambush pocket | Narrow, partial wall stubs, debris piles |
| **Ritual hall** | Boss sub-room, hero rift accent | Symmetric layout, hero L6 rift in center |
| **Crypt cell** | Optional reward / mystery | Small 12×8, single chest socket |

Encounter = 3-5 slots concatenated. Example:
- Combat encounter: Entry → Pillar arena → Collapse corridor → Pillar arena (reward)
- Elite encounter: Entry → Ritual hall (single intense fight)
- Boss encounter: Entry → Collapse corridor → Ritual hall

---

## 2. Per-Act material adaptation

Same architectural grammar, different visual identity per Act.

### 2.1 Act 1: Shattered Keep (Sunken Keep)

**Theme:** Fragmented ancient order, cold stone, ritual catastrophe. Order broken by The Fracturing.

**Architectural cue:**
- Walls: stacked cool granite blocks, vine creep
- Pillars: rectangular columns, chain motifs hanging
- Archways: granite arch + cyan rift glow inside
- Floor: cool granite, worn stone path variations
- Storytelling debris: skeletons in chains, broken banners, candles

**Palette (NLM canonical):**
- Granite dominant: `#3A3D42` – `#4E5260`
- Cool shadow: `#252830`
- Ice blue cracks: `#7BA7BC`
- Torch orange: `#C4682A`
- **Cyan rift accent (Karar #98 LOCK): `#00FFCC`**
- BANNED: warm brown, generic bright green, geometric grid

**L1-L6 asset breakdown:**
| Layer | Material | Variants | Endpoint |
|---|---|---|---|
| L1 Floor base | Cool Granite | 16 | `create_tiles_pro` (PURE top-down) |
| L2 Floor variation | Worn Stone Path | 16 | `create_tiles_pro` |
| L3 Wall overlay | **5 classes** (top hero / bottom hero / side L / side R / corner) + arch + pillar + collapsed_stub | 5×3 variants + 3 hero | `create_object` (isometric depth) |
| L4 Large patches | Cave Moss / Dust Drift / Cracked Rubble | 8 each | `create_map_object` |
| L5 Scatter | Small stones (universal) + Chain debris + Skeleton fragments | 4-8 each | `create_object` |
| L6 Accent | Cyan rift fracture (hero) + Brazier candle (point light) + Banner hanging | 1-2 hero + 4-6 medium | `create_object` |

### 2.2 Act 2: Bleeding Wastes

**Theme:** Living corrupted wound, dark bog, ossuary. Architecture **eaten by biological wound**. The Fracturing made flesh.

**Architectural cue:**
- Walls: granite blocks + dark roots crawling, dried blood vein trace
- Pillars: ossified bone twists, root-wrapped stone
- Archways: bone arch + dried-blood rust glow inside
- Floor: corrupted bog (dark violet-purple), bone fragment scatter
- Storytelling debris: rib-cages embedded in floor, root tendrils, blood pools

**Palette (NLM canonical):**
- Bog dominant: `#3A2840`
- Corrupted moss: `#5A4870`
- Weathered bone: `#A89880`
- Dried blood: `#5E2A35`
- Dark roots: `#3A2820`
- **Rust ember accent (Karar #98 Act 2 variant): `#C8502A`**
- BANNED: generic swamp green, bright clean palette

**L1-L6 asset breakdown:**
| Layer | Material | Variants | Endpoint |
|---|---|---|---|
| L1 Floor base | Corrupted Bog | 16 | `create_tiles_pro` |
| L2 Floor variation | Dried Blood Channel | 16 | `create_tiles_pro` |
| L3 Wall overlay | Same 5 classes, **bone-wrapped granite variant** (palette swap + root overlay) | 5×3 | `create_object` |
| L4 Patches | Corrupted Moss / Dried Blood / Dark Roots | 8 each | `create_map_object` |
| L5 Scatter | Bone fragments / Rib-cages / Skull clusters | 4-8 each | `create_object` |
| L6 Accent | Rust ember rift (hero) + Blood-soaked banner + Bone brazier | 1-2 hero + 4-6 medium | `create_object` |

### 2.3 Act 3: Core Approach

**Theme:** Transcendental cosmic, thinning reality, voids. Architecture **dissolving into void**. The Fracturing's origin layer.

**Architectural cue:**
- Walls: black void-stone + incandescent gold sigil carvings (half-erased)
- Pillars: floating fragmented stone, gold rune bands
- Archways: void arch + gold sigil glow inside
- Floor: void substrate (near-black with star dust)
- Storytelling debris: cosmic dust trails, gold rune fragments, void tear seams

**Palette (NLM canonical):**
- Void dominant: `#0A0810`
- Incandescent gold: `#FFD700`
- Star fragments: `#E8DFC0`
- Void bleed: `#4F2A6B`
- **Gold sigil accent (Karar #98 Act 3 variant): `#FFD700`**
- BANNED: warm/cozy palette, organic earth tones

**L1-L6 asset breakdown:**
| Layer | Material | Variants | Endpoint |
|---|---|---|---|
| L1 Floor base | Void Substrate | 16 | `create_tiles_pro` |
| L2 Floor variation | Star Dust Trail | 16 | `create_tiles_pro` |
| L3 Wall overlay | Same 5 classes, **void-stone with gold sigil variant** | 5×3 | `create_object` |
| L4 Patches | Void Bleed / Gold Sigil Pool / Cosmic Dust | 8 each | `create_map_object` |
| L5 Scatter | Star fragments / Gold rune chips / Void crystal | 4-8 each | `create_object` |
| L6 Accent | Gold incandescent sigil (hero) + Floating rune lantern + Cosmic banner | 1-2 hero + 4-6 medium | `create_object` |

---

## 3. Asset pack structure (Codex review hedef)

Mevcut yapı `Assets/Art/AssetPacks/{_Universal, Act1, Act2, Act3, Special}` korunur. Karar #150 LIVE her Act'e aşağıdaki minimum production library'i ekler.

### 3.1 Asset count per Act

| Layer | Asset class count | Variants per class | Total assets |
|---|---|---|---|
| L1 Floor base | 1 material (16-tile Wang set) | 16 | 16 |
| L2 Floor variation | 1 material (16-tile Wang set) | 16 | 16 |
| L3 Wall overlay | **5 wall classes + arch + pillar + collapsed_stub = 8** | 3 each | 24 |
| L4 Large patches | 3 materials | 8 each | 24 |
| L5 Scatter | 3 categories | 4-8 each | ~18 |
| L6 Accent | 1 hero + 3 medium + 2 point-light sources | 1-2 + 4-6 | ~12 |
| **Per-Act total** | | | **~110 assets** |

**Asset economics caveat (Codex revision #5):**

- 110 planned × 3 Acts = 330 base gen
- NLM-observed regen rate 25-35% → 330 × 1.35 = **~446 effective gens**
- Current PixelLab reserve 3500/5000 → leaves ~3054 credits after all 3 Acts
- **Scope:** Bu math YALNIZ environment package için. NLM full-art budget 21000-22000 credits (whole game art) — Karar #150 environment scope, char/mob/VFX/UI budget AYRI.

**Act 1 already produced:** ~35-50 (from MEMORY asset pack inventory)
**Act 1 remaining (Karar #150 Faz 1):** ~24 L3 wall sprite (5 class + arch + pillar + stub × 3 variant). **Faz 1 ÖNCE inventory verify**: mevcut wall_edge_stone / painterly_wall_* / wall_decoration_vines fake-iso depth karşılıyor mu? Çoğu flat → regen gerek.

### 3.2 Cross-Act reuse rules (Karar updated)

| Asset | Universal? | Why |
|---|---|---|
| Small stones (4 variants) | ✅ Universal | Neutral gray, color-tintable |
| VFX atomic (sparks, dust puffs) | ✅ Universal | Class-color-tintable |
| Floor decals (generic chips) | ✅ Universal | Cross-Act safe |
| L1/L2 floor materials | ❌ Per-Act | Palette + texture biome-specific |
| L3 wall classes | ❌ Per-Act | Material visually distinct per Act |
| L4 patches | ❌ Per-Act | Color/organic-shape biome-specific |
| L5 scatter (debris) | Mixed | Generic = universal, Act-themed (bone, sigil) = per-Act |
| L6 hero rift accent | ❌ Per-Act | Color + motif Act-specific |
| Brazier / Lantern (point light) | Mixed | Act 1 = candle, Act 2 = bone-brazier, Act 3 = floating-rune lantern |

**HARD RULE:** Yeni asset üretmeden ÖNCE check `_Universal` veya başka Act'te benzer var mı? Varsa palette tint / scale ile reuse. Sıfırdan üretme.

**Palette override caveat (Codex revision #6):**

Runtime tint Unity-side (`BackgroundLayerData.tintColor`, `PatchEntry`, `BiomeSkin`) **works** — Unity SpriteRenderer tint.

PixelLab `create_object` palette override (Act 2 bone-wrapped granite gen'i Act 1 granite'tan palette swap ile) **verified DEĞİL**. Karar #150 stance: **"reuse/tint where safe; regen or image-edit for material identity changes unless PixelLab palette override is verified."**

- Act 2 wall material variants → BUDGET regen olarak treat et (24 sprite × 3 Act = full per-Act gen)
- Act 2 floor (corrupted bog) → tint Act 1 granite YASAK (palette identity material identity = bog ≠ granite)
- Sadece neutral/grayscale assets (`_Universal/small_stones`, `vfx_atomic`) runtime tint ile reuse

---

## 4. Sub-room connection design (door-through mechanic detail)

### 4.1 Visual connection rules

Her sub-room transition için:

1. **Exit archway placement match** — sub-room N'in çıkış arch'ı sub-room N+1'in giriş arch'ı ile aynı kenarda olur (sağ → sol mirror). Player gate'ten geçer, fade-to-black, yeni sub-room'da aynı arch'tan giriyor olarak spawn.
2. **Material continuity** — aynı L3 wall class set kullanılır. Sub-room 1 granite duvar → sub-room 2 granite duvar (Act 1 case).
3. **Palette consistency** — same ambient light color, same accent color. Sub-room varyasyon density + composition'da, palette'te değil.
4. **Storytelling trail** — opsiyonel debris breadcrumb: sub-room 1 sonunda boss-imp ölüsü L5 decal → sub-room 2 başında same imp tribe scatter (continuity hint).

### 4.2 Logical interconnection (bol tasarım)

**Encounter narrative arc örneği (Act 1 Shattered Keep, Combat encounter):**

| Sub-room | Architectural cue | Combat | Reward |
|---|---|---|---|
| 1. Entry chamber | Wide opening, broken portcullis behind player | Player spawn, 0 enemies (entry breather) | — |
| 2. Pillar arena | 3 granite pillars in center, banners hanging | Wave 1: 3 imps emerge from pillar shadows | — |
| 3. Collapse corridor | Partial walls + debris piles, narrow chokepoint | Wave 2: 1 elite + 2 imps, ambush from collapsed wall gaps | — |
| 4. Ritual hall | Symmetric pillar layout, hero cyan rift in center | Wave 3: encounter boss-imp + 2 swarm | Map fragment + skill draft (encounter-final reward) |

**4 sub-room (~%70 of encounters) veya 3 sub-room (~%30) variants.** Vertical slice = 2 sub-room.

### 4.3 Asset pool reuse per encounter

Tek bir encounter (4 sub-room) kullanır:
- 1 L1 floor material (Wang set)
- 1 L2 variation
- 5 wall classes + arch + pillar + stub (composed differently per sub-room)
- L4 patches algorithmically distributed (each sub-room different density)
- L5 scatter (each sub-room different cluster pattern)
- L6 accent (each sub-room different placement — final sub-room has hero rift)

**Tek bir 110-asset Act pack → tüm Act 1 (60-80 sub-room across 13-node DungeonGraph)** composition rules ile rendered.

---

## 5. Production roadmap integration

Karar #150 LIVE → Roadmap (`STAGING/ROADMAP_dungeon_buildup.md`) Faz 1 trigger.

**Faz 1' (revize):** Act 1 skeleton (5 isometric wall classes) PixelLab gen.

**Faz sırası (Act 1 only, then repeat for Act 2/3):**
1. **Faz 1 — Skeleton:** 5 wall classes + arch + pillar + collapsed_stub (8 classes × 3 variants = 24 sprites)
2. **Faz 2 — Floor:** L1 + L2 Wang sets (already have 16+2 = need 16 more L2)
3. **Faz 3 — Wall decoration:** Chain/banner/candle motifs (L6 + animation-ready)
4. **Faz 4 — Lighting:** Global Light2D + brazier/candle point light + cyan rift glow
5. **Faz 5 — Post-FX:** Vignette + bloom + contact shadow blob + 8px floor overlap
6. **Faz 6 — Variant + sub-room sequence:** 3 sub-room compose (Karar #149 vertical slice)

Her faz Antigravity Opus 4.6 veya Sonnet UnityMCP dispatch ile yapılır. Karar #150 LIVE Faz 0 satisfy ediyor.

### 5.1 Faz 1 dispatch trigger criteria (Codex revision #7)

**v4 PASS = sufficient trigger.** Stop concept iteration. Faz 1 skeleton screenshot review aşağıdaki 5 gate'te FAIL ederse regen tek-tip:

1. **Silhouette:** Internal architecture primary, perimeter minimal/off-screen — pass
2. **Inside-feel:** Multi-chamber feeling, not arena — pass
3. **Wall depth:** Fake-iso top cap + front face + base shadow okunaklı — pass
4. **Arch readability:** Archway portals açık silhouette, gate identity okunabilir — pass
5. **35° compatibility:** Floor tilt + 8-dir character alignment (Karar #114) görsel olarak tutarlı — pass

5/5 PASS → Faz 2 dispatch. 1-2 FAIL → regen failed gate sprite class only. 3+ FAIL → root cause investigation (yeni concept iteration ihtiyacı?).

### 5.2 Sustainability — Act 2/3 asset strategy (Codex revision)

**Karar #150 stance:** Act 1 ship FIRST. Act 2/3 asset gen budget save için:
- ❌ Sub-room count azaltma (encounter quality drop)
- ✅ **Unique asset class azaltma** (110 asset = ceiling, not quota)
- ✅ **Aggressive `_Universal` reuse** (small stones, neutral chips, contact shadows, dust puffs, sparks, generic crack masks, neutral VFX atoms)
- ✅ **Tintable grayscale decal masks** for compatible scatter
- ✅ **Selective hero regen** — L6 hero accent + 1-2 signature wall class per Act, rest composed via tint + composition

---

## 6. Codex review questions (for dispatch)

1. **Architecture feasibility:** v4 dungeon-inside style → mevcut RoomTemplateSO + MapLayerOrchestrator + multi-layer painter ile tutarlı mı? Implementation gap var mı?
2. **Asset count realism:** 110 asset per Act × 3 Act = 330 total. PixelLab 5000 budget ile uyumlu mu? Hidden cost var mı (regen iteration)?
3. **Sub-room connection:** Karar #149 fade-to-black + archway match rule implementation feasibility. CameraFollow SetBounds API gap var mı?
4. **Cross-Act reuse rules:** Universal vs per-Act split mantıklı mı? Daha aggressive universal kullanım önerisi var mı (asset budget save)?
5. **Layout grammar (5 sub-room types):** Entry/Pillar/Collapse/Ritual/Crypt slot system mevcut DungeonGraph + EncounterTemplate ile çakışıyor mu?
6. **Per-Act adaptation:** Act 2 bone-wrapped variant + Act 3 void-substrate variant Act 1 wall class'larından **palette swap + overlay** ile mi yapılır, yoksa **sıfırdan regen** mi? Hangisi daha ucuz?
7. **Scope creep risk:** Roadmap Faz 1-6 disiplini × 3 Act × 6 faz = 18 faz cycle. Her faz tek-tip iş kuralı bu scale'de sürdürülebilir mi?

---

## 7. Drift / conflict notes

**NLM staleness flag (CRITICAL):**
NLM canonical hâlâ Karar #149 öncesi "1 oda = 1 arena, wave-based" modeli söylüyor. Karar #149 (S94 LIVE) bunu süpersede etti. NLM sync gerek (`/nlm-sync` dispatch). Bu Karar #150 dokümanı + Karar #149 memory NLM'e push edilmeli.

**Preserved (no conflict):**
- Karar #98 cyan rift accent (per-Act color variant: cyan / rust ember / gold)
- Karar #100 35° tilt + Karar #114 8-dir LOCK (5 produce + 3 mirror)
- Karar #143 6-layer painter
- Karar #147 Multi-Layer Painter
- Karar #148 Branch D+E (floor de-emphasis + camera tilt)

**Updated (Karar #150 LIVE açıklığa kavuşturuyor):**
- Karar #149 sub-room sizing: 16×10 default → **32×22 default** (v4 PASS evidence)
- v3 diamond constraint → KALDIRILDI

---

## 8. Next actions after LIVE (Codex revision #8 NLM sync mandatory)

1. ✅ MASTER_KARAR_BELGESI.md Karar #150 LIVE wording (Codex-suggested) entegre
2. ✅ Memory `project_karar_150_fake_isometric_lock.md` LIVE
3. ⏳ **NLM sync push MANDATORY** — Karar #143/147/148/149/150 + memory dosyaları. NLM hâlâ pre-#149 single-arena canon dönüyor, sub-agent dispatch'leri yanlış routing yapar.
4. ⏳ EncounterTemplateValidator mirror geometry rules (one code edit, Codex Step 3 dispatch)
5. ⏳ Roadmap Faz 1' dispatch hazırla — Act 1 L3 skeleton ONLY (5 wall classes + arch + pillar + collapsed_stub, 3 variants each, NO floor/deco/lighting creep)
6. ⏳ Vertical slice authoring: 2 sub-room (matching DoorSocket ids, fade transition, archway mirror, final reward only)

---

## Related

- [[project-karar-150-fake-isometric-lock]] (candidate, this doc supersedes to LIVE)
- [[project-karar-149-subroom-encounter-lock]] (sub-room integration)
- [[project-roadmap-dungeon-buildup-lock]] (6-faz discipline)
- [[project-asset-pack-organization-lock]] (folder hierarchy)
- [[project-alabaster-dawn-pipeline-lock]] (Karar #143)
- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png` (v4 PASS reference)
