# Opus Wall Production FINAL (Wave D — 2026-05-19)

## 1. Scale + math (corrected from Wave B)

### Room geometry (CORRECTED)
- World bounds: **18 × 12 world units** (was wrongly 3×2 in Wave B)
- L1_BaseFloor tilemap origin (0,0) → (18,12)
- Gate centers (from scene state): **LEFT (0.5, 4.91)**, **RIGHT (17.5, 4.91)**
- Each cell = 1 × 1 world unit (NOT 0.1667)

### PPU decisions
| Asset | Native px | PPU | World size | Role |
|---|---|---|---|---|
| `pixellab_wall_section_horizontal` | 384×216 | **64** | 6.0 × 3.375 | Hero top/bottom edges |
| `pixellab_wall_arch_section` | 192×128 | **64** | 3.0 × 2.0 | Gate cutouts |
| `gate_arch` | 192×128 | **64** | 3.0 × 2.0 | Corner pillars + side fillers |
| `painterly_prop_06_burning_brazier` | ~192×192 | **64** | ~3.0 × 3.0 | Brazier (scale 0.4 → 1.2 world) |
| `wall_decoration_vines` | overlay | **64** | scale to taste | Seam decals |
| `painterly_decal_xx` | varies | **64** | scale 0.3-0.5 | Seam decals |

### Sprite count + overlap
- Top/bottom edge = 18u wide, hero = 6u wide, 10% overlap = 5.4u spacing
- **4 sprites per long edge** (centers x=3.0, 7.5, 12.0, 15.0)
- Left/right edges (12u tall): see Section 2

### Wall vs character ratio
- Wall height = 3.375u
- Warblade scale 0.85 → 2.0u → 1.7× ratio (FAIL concept 3-4×)
- **DECISION: Warblade scale 0.85 → 0.5** in Spawn rooms only → 1.2u → **2.8× ratio**
- Combat rooms preserve 0.85 (combat readability)

---

## 2. Side wall problem + LOCKED resolution

**CRITICAL DISCOVERY:** Hero `pixellab_wall_section_horizontal` is baked with HORIZONTAL 35° perspective. Rotating 90° BREAKS the perspective illusion (bricks sideways, shadow inverted).

**LOCKED — TWO-PHASE side walls:**

**Phase 1 (THIS execution, zero new gen):**
- Side edges (x=0, x=18) use **stacked `gate_arch` columns** at PPU 64 (3w × 2h each)
- 5 columns per side + gate (~10h coverage with arch at 4.91 spanning 2h)
- `gate_arch` IS rotation-tolerant (columnar geometry)
- Reads as "ornate buttressed side wall"

**Phase 2 (deferred contingent):**
- Mini-regen 2-3 vertical wall sprites (424×632 per existing prod spec)
- Use `b2703abf` as style ref
- Replace gate_arch column stacks
- Cost: 2-3 PixelLab gens

**Phase 1 ships first.** Phase 2 triggered if visual test <80%.

---

## 3. Composition plan (FULLY RECOMPUTED D6)

### Hierarchy
```
Walls/
  Top/        (SO 100, parent y=12.0)
  Bottom/     (SO 100, parent y=0.0)
  Left/       (SO 100, parent x=0.0)
  Right/      (SO 100, parent x=18.0)
  Corners/    (SO 101)
  Gates/      (SO 102)
  Braziers/   (SO 110)
  SeamDecals/ (SO 105)
```

Pivot=BottomCenter (horizontals), BottomLeft (corners + verticals). PPU 64. SortingLayer=Walls.

### Spawn_01 — Top edge (y=11.0, sprite extends to y=14.375)
| # | Sprite | World (x,y) | flipX | SO |
|---|---|---|---|---|
| T1 | `pixellab_wall_section_horizontal` | (3.0, 11.0) | false | 100 |
| T2 | same | (7.5, 11.0) | true | 100 |
| T3 | same | (12.0, 11.0) | false | 100 |
| T4 | same | (15.0, 11.0) | true | 100 |

Seams: x=5.4, 9.9, 13.5, 16.5

### Spawn_01 — Bottom edge (y=0.0)
| # | Sprite | World (x,y) | flipX | SO |
|---|---|---|---|---|
| B1 | `pixellab_wall_section_horizontal` | (3.0, 0.0) | true | 100 |
| B2 | same | (7.5, 0.0) | false | 100 |
| B3 | same | (12.0, 0.0) | true | 100 |
| B4 | same | (15.0, 0.0) | false | 100 |

### Spawn_01 — Left edge (x=0.0, gate at y=4.91)
| # | Sprite | World (x,y) | SO |
|---|---|---|---|
| L1 | `gate_arch` (column) | (0.0, 0.0) | 99 |
| L2 | `gate_arch` (column) | (0.0, 2.0) | 99 |
| **LG** | `pixellab_wall_arch_section` | (0.0, 3.91) | 99 |
| L3 | `gate_arch` (column) | (0.0, 5.91) | 99 |
| L4 | `gate_arch` (column) | (0.0, 7.91) | 99 |
| L5 | `gate_arch` (column) | (0.0, 9.91) | 99 |

### Spawn_01 — Right edge (x=18.0)
| # | Sprite | World (x,y) | flipX | SO |
|---|---|---|---|---|
| R1 | `gate_arch` (column) | (18.0, 0.0) | true | 99 |
| R2 | `gate_arch` (column) | (18.0, 2.0) | true | 99 |
| **RG** | `pixellab_wall_arch_section` | (18.0, 3.91) | true | 99 |
| R3 | `gate_arch` (column) | (18.0, 5.91) | true | 99 |
| R4 | `gate_arch` (column) | (18.0, 7.91) | true | 99 |
| R5 | `gate_arch` (column) | (18.0, 9.91) | true | 99 |

### Corner pillars (overlay)
| # | Sprite | World (x,y) | flipX | flipY | SO |
|---|---|---|---|---|---|
| C-TL | `gate_arch` | (0.0, 11.0) | false | false | 102 |
| C-TR | `gate_arch` | (18.0, 11.0) | true | false | 102 |
| C-BL | `gate_arch` | (0.0, 0.0) | false | true | 102 |
| C-BR | `gate_arch` | (18.0, 0.0) | true | true | 102 |

### Braziers (Path A LOCKED — cyan #00FFCC)
| # | Sprite | World (x,y) | scale | SO |
|---|---|---|---|---|
| BR-L1 | `painterly_prop_06_burning_brazier` | (1.2, 3.5) | 0.4 | 110 |
| BR-L2 | same | (1.2, 6.3) | 0.4 | 110 |
| BR-R1 | same | (16.8, 3.5) | 0.4 | 110 |
| BR-R2 | same | (16.8, 6.3) | 0.4 | 110 |

### Vine accents
| # | Sprite | World (x,y) | flipX | SO |
|---|---|---|---|---|
| V1 | `wall_decoration_vines` | (5.0, 12.5) | false | 103 |
| V2 | same | (13.0, 12.5) | true | 103 |

**Total Spawn_01: ~30 SpriteRenderers** (was 16 in wrong-scale Wave B).

### Spawn_02 deltas
- Drop LG arch → replace with stacked `gate_arch` (closed-off feel)
- Drop BR-L1 + BR-L2 (single brazier set, right side only)
- Add bottom vines V3+V4 at (5.0, -0.3) and (13.0, -0.3)
- No top vines
- Mirror all flipX vs Spawn_01

**Total Spawn_02: ~28 SpriteRenderers**

---

## 4. Wall connection plan (HYBRID D+A LOCKED)

### Top edge seams
| Seam x | Hide decal | Position | Scale |
|---|---|---|---|
| 5.4 | `wall_decoration_vines` | (5.4, 11.5) | 0.5 |
| 9.9 | `painterly_decal_moss_01` | (9.9, 11.3) | 0.4 |
| 13.5 | `wall_decoration_vines` (flipX) | (13.5, 11.5) | 0.5 |
| 16.5 | `painterly_decal_crack_01` | (16.5, 11.4) | 0.3 |

### Bottom edge seams
| Seam x | Hide decal | Position | Scale |
|---|---|---|---|
| 5.4 | `painterly_decal_moss_02` | (5.4, 0.7) | 0.4 |
| 9.9 | `painterly_decal_crack_02` | (9.9, 0.8) | 0.3 |
| 13.5 | `painterly_decal_moss_01` (flipX) | (13.5, 0.7) | 0.4 |
| 16.5 | `wall_decoration_vines` | (16.5, 0.8) | 0.4 |

### Side seams (gate_arch stacks)
8 seams (4 per side at y=2.0, 5.91, 7.91, 9.91), small decal at each (alpha ≥ 0.7, scale 0.2-0.3).

**Total: 16 seam decals.**

### Implementation rule
For each SR pair, verify:
1. 10-15% overlap (spacing = sprite_width × 0.85 to 0.90)
2. Seam decal at SO 105 (between walls 99-102 and braziers 110)
3. Decal alpha ≥ 0.7

---

## 5. Character scale adjustment

**LOCKED:** Warblade scale **0.85 → 0.5** in Spawn rooms ONLY.

- Modify Warblade instance in Spawn_01.unity + Spawn_02.unity
- DO NOT modify Warblade prefab (Combat rooms keep 0.85)
- Alt: RoomEntry script trigger based on RoomTemplateSO.roomType

Ratio after fix: 2.8× character (concept 3-4× still 10-15% short, Phase 2 regen pushes to 3×+).

---

## 6. Cyan brazier conversion (Path A LOCKED)

Per brazier (×4 Spawn_01, ×2 Spawn_02):

**SpriteRenderer:** sprite unchanged (color #FFFFFF), Material Sprite-Lit-Default, SO 110

**Light2D (child):** Point, Color `#00FFCC`, Intensity 1.2 (animated 1.0→1.4 over 2.0s), Outer 2.0, Inner 0.5, Falloff 0.8, Blend Style Multiply

**FlameAdditive (child SR):** sprite = top half of brazier (just flame) or noise quad, Color `#00FFCC` alpha 0.7, Material Additive, SO 111, local pos (0, 0.6, 0)

**Animator BrazierBreath:** Idle loop, Light2D.intensity curve 1.0→1.4→1.0 over 2.0s (smooth Hermite), FlameAdditive alpha 0.6→0.8→0.6 same timing

**Discipline:** brazier 2.0s slow breath, rift cracks 0.4s fast pulse + particles (visual distinction via behavior, same color).

---

## 7. Concept fidelity prediction

**Phase 1: 80-85%**

| Dimension | Score | Notes |
|---|---|---|
| Wall tone | 90% | pixellab_wall_section matches |
| Wall verticality | 75% | 2.8× ratio, concept 3-4× still 10-15% short |
| Cyan braziers | 95% | Light2D + additive strong match |
| Gate arches | 90% | arch_section near-perfect |
| Seam invisibility | 85% | Overlap+decals hide 90% |
| Side wall fidelity | 60% | gate_arch columns interim |
| Corner ornamentation | 85% | gate_arch corner reads strong |
| Floor-wall transition | 75% | Sprite-baked rubble carries |

**Push to 95%+ (Phase 2):** 5-9 PixelLab gens — vertical hero walls + dedicated seam decals + 1 taller hero variant.

---

## 8. File pack cleanup checklist

| Asset | Action | Reason |
|---|---|---|
| `wall_edge_stone.png` | **ARCHIVE** → `_archive_faz1/walls_superseded/` | Superseded |
| `wall_decoration_vines.png` | **KEEP** | Active accent + seam decal |
| `painterly_wall_01-12` | **KEEP + tag WARM_TONE_VARIANT** | Act 2/3 candidate |
| `gate_arch.png`, `gate_spikes.png` | **KEEP** | Active hero |
| `painterly_prop_06_burning_brazier.png` | **KEEP** | Light2D overlay base |
| `STAGING/spawn01_faz1_polish_v1/v2/v3.png` | **DELETE** | Git covers |
| `STAGING/test_pixellab.png`, `test_api_dl.png` | **DELETE** | Test artifacts |
| 8× 32×32 PixelLab tiles | **DEFER IMPORT** | Future tilemap rooms |
| `ab0f5ab4` alabaster_layered | **SKIP IMPORT** | Incompatible structure |

Total: 1 archive move, 5 deletions, 0 git rm (GUID preserved).

---

## 9. Integration plan (Sonnet UnityMCP execution)

### Phase 1 — Asset import (10 min)
Already done by orchestrator: pixellab_wall_section_horizontal + corner + arch_section in walls folder. Configure import: PPU=64, Filter=Point, Compression=None, Pivot=BottomCenter (horizontal) / BottomLeft (arch).

### Phase 2 — Scene cleanup (5 min)
Delete all 14× painterly_wall_xx SR under Spawn_01_NewTileSystem/L3_Walls AND Spawn_02_NewTileSystem/L3_Walls. KEEP Gate_Entry + Gate_Exit GameObjects.

### Phase 3 — Spawn_01 composition (30 min)
- Create empty parents under L3_Walls (or rebuild as new Walls/ hierarchy)
- Instantiate per Section 3 tables (4+4+6+6 edges + 4 corners + 4 braziers + 2 vines = 30 SR)
- Verify positions match EXACTLY (no procedural placement)

### Phase 4 — Warblade scale (2 min)
Spawn_01/02: Warblade `transform.localScale = (0.5, 0.5, 1.0)`. Save scenes.

### Phase 5 — Brazier cyan (15 min × 4-2 braziers)
Light2D + FlameAdditive + Animator per Section 6.

### Phase 6 — Spawn_02 variant (15 min)
Repeat with deltas (single brazier, no LG arch, bottom vines, mirror flipX).

### Phase 7 — Cleanup (5 min)
Archive wall_edge_stone + delete test/old screenshots + tag painterly_wall.

### Phase 8 — Screenshot verify (5 min)
Camera frame to concept ratio. Save `STAGING/walls_v3_spawn01.png` + `walls_v3_spawn02.png`. Side-by-side compare.

### Phase 9 — Test pass (5 min)
RoomFlowTests + new WallCompositionTests.cs (verify corners, gates, braziers, decals exist).

### Phase 10 — Visual gate
- ≥80% fidelity: COMMIT
- <80%: dispatch Phase 2 mini-regen (2-3 vertical walls + dedicated seam decals)

**Total Phase 1: ~90 minutes execution.**

---

## 10. Conflict check

| Karar | Status |
|---|---|
| #98 cyan | **EXPANDED** (Path A lock, behavior discipline) |
| #100 35° | ✅ PRESERVED |
| #143 6-layer | ✅ PRESERVED |
| #147 Multi-Layer Painter | ✅ PRESERVED |
| #148 Branch D+E | ✅ PRESERVED |
| 5000 PixelLab alloc | ✅ Phase 1 zero gen, Phase 2 fits 2150 reserve |
| Asset Pack Org | ✅ Archive preserves GUIDs |
| User-cannot-draw autonomy | ✅ Sprite import + scene placement only |

---

## Decision summary

**DECISION:** Hybrid wall composition. Top/bottom = 4× hero horizontal with 10-15% overlap + seam decals. Sides = stacked `gate_arch` columns (Phase 1) + mini-regen Phase 2 contingent. Gate cutouts = `pixellab_wall_arch_section`. Corners = `gate_arch`. Braziers cyan Light2D + additive per Path A. Warblade 0.5 scale in spawn rooms. 16 seam decals.

**Phase 1 ships 80-85% fidelity, zero new gen, ~90 min execution.**

**Phase 2 contingent: 5-9 PixelLab gens for 95%+** if visual test fails.

**Orchestrator next step:** Sonnet UnityMCP dispatch when user opens Unity. Execute Phase 1-10 sequentially.
