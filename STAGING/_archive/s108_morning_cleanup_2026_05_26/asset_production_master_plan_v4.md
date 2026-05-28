# Asset Production Master Plan v4

**File:** `STAGING/asset_production_master_plan_v4.md`
**Last updated:** 2026-05-22
**Supersedes (canonical):** `STAGING/asset_production_master_plan_v3.md`
**v1, v2, v3 retained as historical context -- do NOT delete.**
**Owner:** Orchestrator reads this before any asset dispatch. Do NOT modify decisions; update Status column only.

---

## Change log (v3 -> v4)

| Change | Detail |
|---|---|
| L10 ADDED | Parca-parca (piece-by-piece) stitching pipeline -- mandatory for any asset >= 128px in either dimension |
| L11 ADDED | Boss canvas hard cap at 256x256 max -- no exceptions |
| L12 ADDED | Tile seam strategy refined to HYBRID: Wang16 for generic material transitions + Inpaint v3 for special seams |
| L13 ADDED | Natural-look layered floor 6-layer strategy -- decal layer is MANDATORY, not optional |
| Format | Entire Section 5 rewritten as declarative RULE N list ("boyle olacak boyle" format per user direction 2026-05-22) |
| Section 4 | Budget table added as new Section 4, replacing open-ended estimate table from v3 Section 7 |
| Section 5 revised | v3 Item list replaced with RULE 1-13 sequential declarative rules |
| Boss policy updated | v3 HYBRID outpaint "boss >256px" case REMOVED. L11 closes it: boss never exceeds 256. Parca-parca pipeline does NOT apply to boss. |
| Wall policy updated | Parca-parca (L10) now applies to walls >= 128px (hero arch, large pillar), not just tiles |
| Dungeon edge walls | Covered under RULE 4 with explicit parca-parca trigger at >= 128px |

---

## Lock status legend

| Symbol | Meaning |
|---|---|
| PENDING_DECISION | User must choose between listed options |
| PENDING_LOCK | Decision reached, awaiting explicit user confirmation |
| LOCKED | User confirmed. Do not re-decide. |
| IN_PROGRESS | Active dispatch running |
| DONE | Shipped and committed |

---

## Section 0 -- Read-first context

v4 is the canonical production document. v3 remains for lineage. All decisions from v3 carry forward unchanged unless explicitly revised here.

Three new locks (L10/L11/L12/L13) change how large assets and boss sprites are produced. They do NOT change the dispatch order. The first executable item is still RULE 3 (wall prototype), which gates all object production.

No item may be dispatched autonomously unless its RULE status = LOCKED.

---

## Section 1 -- All locks (13 total)

| # | Lock | Rule | Status |
|---|---|---|---|
| L1 | Pivot -- Hades-iso | ~70-75 deg tilt. create_object view="high top-down". Pure top-down REVOKED. | LOCKED |
| L2 | Weapon decouple arch | Weaponless body + Child SR + HandAnchor. 12 weapon sprites incl. Elementalist orb. | LOCKED |
| L3 | No PixelLab gen at night | create_tiles_pro / create_object / animate_* blocked during autonomous night runs. get_* / list_* read-only only. | LOCKED |
| L4 | Hibrit variant strategy C | Act 2-4 hero anchors = user manual Edit Image Pro. Small props + decals = orchestrator create_object_state autonomous. | LOCKED |
| L5 | Tile pipeline: Mod B | create_tiles_pro Mod B + style_images transition. Wang autotile deprecated for standard floor. | LOCKED |
| L6 | Wall + decoration pipeline | create_object n_frames=4/16/64 by canvas size. GameObject placement (NOT tile). canvas 64x96 / 64x128 Hades-iso. | LOCKED |
| L7 | Tile size | 64x64 px production. Matches char 64 PPU = 1 cell. | LOCKED |
| L8 | Painter default | RimaWorldPainterWindow PaintMode.TopDown primary. | LOCKED |
| L9 | Tile seam base | create_tiles_pro style_images anchor for palette consistency on every subsequent tile batch. | LOCKED |
| L10 | Parca-parca stitching pipeline | Any asset >= 128px in either dimension is produced as separate pieces then stitched. NOT a single large create_object call. See Section 2. | LOCKED (NEW) |
| L11 | Boss canvas 256 max | Boss sprites never exceed 256x256. Animation drawn at same canvas. No extend, no scale-up. No parca-parca for boss -- native 256 create_character_pro direct. | LOCKED (NEW) |
| L12 | Tile seam HYBRID | Wang16 for generic material transitions (granite<->rubble, walkway<->rift). Inpaint v3 manual for special seams (boss arena entry, lore room glow, ritual circle boundary). NOT pure Wang only. NOT pure Inpaint only. | LOCKED (NEW, refines L9) |
| L13 | Natural-look 6-layer floor | Every floor area uses 6-layer overlay. Decal layer (L4) is MANDATORY. See Section 3. | LOCKED (NEW) |

---

## Section 2 -- Parca-parca pipeline specification (L10)

**Trigger rule:** ANY asset >= 128px in either dimension. This includes dungeon edge walls, hero arch, large pillar, sarcophagus, throne_dais, boss arena gate.

**Step-by-step (mandatory sequence):**

1. Plan the division. Example: 128x128 wall = top-half 64x128 + bottom-half 64x128, OR left+right halves 64x128 each. For dungeon edge walls (horizontal run): plan as repeating 64x96 segments with defined seam points.
2. Generate piece 1 via create_object (small canvas, high detail, n_frames=4).
3. Generate piece 2 via create_object with style_image=piece_1 output OR create_object_state referencing piece 1. This anchors palette and texture language.
4. Combine in Aseprite: place pieces on extended canvas, leave seam line transparent or overlap 8-16px.
5. Mask seam region (~16-24px wide).
6. Inpaint v3 seam region with prompt: "seamless natural blend between [piece descriptions], painterly pixel art, organic edge, [RIMA atmosphere: warm torch orange + cold cyan, cracked stone]".
7. Palette snap: Aseprite Indexed Mode, snap to original palette.
8. Optional: spot-fix seam pixels manually if 1-2px artifacts visible.
9. Export final combined PNG -> Unity import as single sprite.

**Cost per large asset (parca-parca):**
- 2x create_object @ 20-40 gen each = 40-80 gen
- 1x Inpaint v3 @ 20 gen = 20 gen
- Total: 60-100 gen per large asset

**Why parca-parca is better for large assets:**
- Higher per-pixel detail (each piece generated at smaller canvas = more detail density)
- Better palette consistency (each piece anchored to prior via style_image)
- Cheaper per detail unit vs single large canvas
- User controls seam quality via Aseprite palette snap

**What parca-parca does NOT apply to:**
- Tiles (create_tiles_pro handles 64x64 natively)
- Props < 128px in both dimensions (native create_object)
- Boss sprite (L11 hard cap at 256 -- native create_character_pro, no stitching)

---

## Section 3 -- 6-layer floor specification (L13)

**Rule:** Every floor area in RIMA uses all 6 layers. No single-layer floor. Decal layer (L4) is mandatory in every room.

| Layer | Content | Tool | Note |
|---|---|---|---|
| L1 | Base tile -- 8-12 variants, random rot/flip per cell | create_tiles_pro Mod B | Granite or primary material |
| L2 | Path/secondary tile -- 4-8 variants | create_tiles_pro Mod B | Walkway or secondary zone material |
| L3 | Transition -- Wang16 for material seams OR Inpaint v3 for special seams | Per L12 HYBRID rule | Between L1/L2 zones |
| L4 | Decal overlay -- moss, dust, crack, blood, debris, random scatter | create_object 32px n_frames=64 | MANDATORY. Never skip. |
| L5 | Scatter prop -- small stones, bones, chains | create_object 32px n_frames=64 | High scatter density |
| L6 | Prop overlay -- columns, urns, banners, vertical/horizontal grid breakers | create_object 48-80px and 88-168px | URP 2D Light variation on top |

**Lighting variation:** Asymmetric URP 2D Lights placed above L6. Warm torch orange + cold cyan rift dual-tone. Not a separate layer -- it is the lighting pass on top of all 6 layers.

---

## Section 4 -- PixelLab parameter table (final locked values)

### Tile parameters

| Tool | Parameter | Value | Status |
|---|---|---|---|
| create_tiles_pro | tile_type | square_topdown | LOCKED |
| create_tiles_pro | tile_view | top-down | LOCKED |
| create_tiles_pro | tile_view_angle | 90 | LOCKED |
| create_tiles_pro | tile_depth_ratio | 0 | LOCKED |
| create_tiles_pro | outline_mode | segmentation | LOCKED |
| create_tiles_pro | style_images | Pass first batch output on all subsequent calls | LOCKED (L9) |

### Object parameters

| Tool | Parameter | Value | Status |
|---|---|---|---|
| create_object | view | "high top-down" | LOCKED |
| create_object | directions | 1 | LOCKED (static prop) |
| create_object | n_frames | 64 for 32px / 16 for 48-80px / 4 for 88-168px | LOCKED |
| create_object | object_view | top-down | LOCKED |

### Pricing reference

| Tool | Gen cost | Output | Notes |
|---|---|---|---|
| create_object 32px | 20 gen | 64 items | n_frames=64 |
| create_object 48-80px | 30 gen | 16 items | n_frames=16 |
| create_object 88-168px | 40 gen | 4 items | n_frames=4; wall tier |
| create_object_state | 20-40 gen | 1 variant | Act 2-4 small variants autonomous |
| create_tiles_pro | 25 gen | 16 tiles | 64px tile, per call |
| Inpaint v3 (user manual, NOT MCP) | 20 gen flat | 1 masked area | Parca-parca seam stitch + L12 special seams |
| Edit Image Pro (user manual, NOT MCP) | 20/25/40 by output size | 1 sprite | up to 256/314/512px; Act 2-4 hero anchors |
| create_character_pro | 40-80 gen | 1 character base | Boss base sprite (256x256 max per L11) |
| create_character_state | 20-40 gen | 1 phase variant | Boss phase variants |

**Budget as of 2026-05-22 S98:** 2265 / 5000 gen remaining.

---

## Section 5 -- Production rules (RULE 1-13, declarative)

These are LOCKED production decisions, not options. PENDING items are clearly marked. The sequence is the execution order.

---

### RULE 1 -- Floor base

create_tiles_pro 4-mix batch. Materials: Granite (L1 base) + Worn Stone Path (L2 secondary) + Cracked Rubble + Rift Fissure.

Parameters: tile_view_angle=90, tile_depth_ratio=0, outline_mode=segmentation.

Output: 16-tile sprite sheet, 64x64 each.

First batch output becomes style_images anchor for all subsequent tile calls (L9).

Cost: 25 gen.

Status: PENDING_LOCK (awaits user dispatch confirmation after RULE 3 prototype pass)

---

### RULE 2 -- Floor transition (HYBRID per L12)

Wang16 corner transition for generic material pairs. Two batches:
- Batch A: granite <-> cracked rubble boundary
- Batch B: walkway <-> rift fissure boundary

Both batches pass style_images from RULE 1 output to maintain palette lock.

PLUS Inpaint v3 spot-polish on 3 special seams: boss arena entry, lore room boundary, ritual circle edge.

Cost: Wang batches 2x25 = 50 gen. Inpaint special seams 3x20 = 60 gen. Total: 110 gen.

Status: PENDING_LOCK (after RULE 1 output confirmed)

---

### RULE 3 -- Wall prototype (GATES ALL OBJECT PRODUCTION)

create_object view="high top-down", canvas 64x96, n_frames=16.

1 batch: straight_wall_n edge variant pool, 16 candidates generated, 3 selected.

Prompt: "pixel art dungeon wall, front face visible, Hades-style isometric view, warm torch orange light from left, cold cyan rift glow accent, cracked stone texture, dark atmosphere, Shattered Keep Act 1"

Place in Unity test room. Screenshot for user review. PASS = proceed to RULE 4 full library dispatch. REWORK = adjust prompt, retry.

Cost: 20-40 gen.

Status: PENDING_LOCK -- first action item, unblocks all object production. No dependency except user confirmation.

---

### RULE 4 -- Wall full library (24 sprites total, parca-parca for >= 128px)

8 wall classes x 3 variants = 24 sprites.

Wall classes:
- straight_wall_n (64x96) -- primary corridor wall, dungeon edge N
- straight_wall_e (64x96) -- dungeon edge E
- corner_wall_convex (64x96) -- outer corner
- corner_wall_concave (64x96) -- inner corner
- wall_end_stub (64x64) -- short wall cap, dead end
- collapsed_stub (64x96) -- ruined partial wall
- archway_n (64x128) -- hero prop, wall class for room entry
- pillar_hero (64x96) -- standalone column, wall break

For dungeon edges (long runs of straight_wall_n and straight_wall_e): produce as modular 64x96 segments with defined seam points. Wall variants tile horizontally. NO single wide canvas for a wall run -- produce segment units, assemble in Unity.

IF wall sprite <= 128px in both dimensions: single create_object n_frames=4.
IF wall sprite > 128px (archway_n at 64x128 height = 128px: borderline -- use parca-parca for archway and any wall exceeding 128px): apply L10 parca-parca pipeline.

Cost: ~5 batches x 25-40 gen = 125-200 gen (mix of native and parca-parca).

Status: PENDING_LOCK (after RULE 3 PASS)

---

### RULE 5 -- Prop 32x32 batch (Act 1 small props + decals, L4/L5 layers)

create_object directions=1 size=32 n_frames=64 (64-capacity slot, fill to capacity).

Items (50 Act 1 + 14 Act 2-4 anchor small items = 64 total):
- skull pile, bone heap (x4 var)
- chest x4 tiers x2 states (closed/open) = 8 slots
- pickup shard, key shard, relic shard = 3 slots
- footprint set, dust pile, ash pile, scorch mark = 4 slots
- bone fragment scatter, chain remnant, iron ring, spike anchor = 4 slots
- small moss patch, crack pattern, blood splatter x4 var = 6 slots
- rift glyph faint, summon circle faint, arcane etching = 3 slots
- water puddle, rift dust scatter, cyan crack hairline = 3 slots
- small rubble chip, pebble cluster, mortar crumble = 3 slots
- hazard telegraph (dash-only zone marker), socket placeholder = 2 slots
- (remaining 14 slots: Act 2-4 canvas-compatible small props -- fill at RULE 5 dispatch time)

Cost: 30 gen single batch.

Status: PENDING_DECISION (need final item list approval from user)

---

### RULE 6 -- Prop 48-80px batch (medium props, L6 layer)

create_object directions=1 size=48-80 n_frames=16 (16-capacity per call).

Items:
- banner_tattered x4 var (64px)
- torch_wall x4 var (48px)
- wall_sconce x2 var (48px)
- brazier_floor x2 var (64px)
- urn_broken x2 var (64px)
- urn_whole x2 var (64px)
= 16 items = 1 batch exactly

If more needed: split into 2 calls, 2nd call fills Act 2-4 medium props.

Cost: 30-60 gen (1-2 calls).

Status: PENDING_DECISION

---

### RULE 7 -- Prop 88-168px batches (hero props, parca-parca per L10 if >= 128px)

create_object directions=1 n_frames=4 per call (4-capacity).

Batches:
- Batch 7A: column_hero (128px), column_broken (128px), pillar_stub (88px), wall_torch_hero (96px)
- Batch 7B: shrine_pedestal (128px), throne_dais (128px), sarcophagus (128px), altar_stone (88px)
- Batch 7C: skull_pile_large (88px), rubble_pile_large (96px), rift_crystal (128px), gate_arch_half (128px)
- Batch 7D (if archway hero needs 2-piece): produce top-half 64x128 + bottom-half 64x128, stitch per L10
- Batch 7E (4-5 additional hero items based on act1_shattered_keep_layout_v1.json audit)

For every item >= 128px in either dimension: apply L10 parca-parca pipeline. Column_hero (128px height) is the first parca-parca candidate. Gate_arch_half and sarcophagus are candidates depending on final canvas.

Cost: 3-5 batches x 40 gen native + parca-parca overhead (60-100 gen each) = 120-300 gen total.

Status: PENDING_DECISION

---

### RULE 8 -- Decal layer production (L13 mandatory, L4 layer categorical batches)

Two categories per Karar #150 and L13:

Category A -- Large patches (L4 base decal):
- Cave Moss x8 var
- Dust Drift x8 var
- Cracked Rubble patch x8 var
= 3 batches x 25-30 gen = 75-90 gen

Category B -- Hero accents (L6 accent overlay):
- Cyan rift hairline x4 var
- Brazier glow splash x2 var
- Banner shadow x2 var
- Ritual circle faint x4 var
= 1 batch 30 gen

Total: 4 batches, 105-120 gen.

Status: PENDING_DECISION (categorical narrowing: which variants to include per batch)

---

### RULE 9 -- Hero anchor list (Hibrit Track C, user manual Edit Image Pro)

After RULE 7 Act 1 hero props are complete, user selects 5-10 for manual Edit Image Pro Act 2-4 biome shift.

Candidates: column_hero, archway_n, throne_dais, sarcophagus, shrine_pedestal, banner_tattered, brazier_floor, urn_whole, rift_crystal, altar_stone.

Each selected asset gets Edit Image Pro treatment per act transition: granite->moss (Act 2), granite->wood (Act 3), granite->crystal (Act 4).

Cost (user manual, not autonomous budget): 5-10 selected x 20-40 gen x 3 acts = 300-600 gen user-paced.

Status: PENDING_DECISION (which assets get manual variant; decided after RULE 7 complete)

---

### RULE 10 -- Small variant scope (state pipeline autonomous, Act 2-4)

create_object_state autonomous pipeline generates biome-shift variants for decals + small props NOT in RULE 9 hero anchor list.

In-scope (autonomous): small stones, chain remnants, rubble scatter, generic moss/blood/scorch decals.
Exclude: everything user selects in RULE 9.

Selective scope: only 3 decal categories get state variants (moss / rift glyph / blood). Others reused as-is across acts.

Cost: ~10 state calls x 20-40 gen = 200-400 gen (autonomous, orchestrator-run).

Status: PENDING_DECISION (scope narrowing after RULE 5/9 decisions)

---

### RULE 11 -- Boss sprite (Phase L scope, post-asset-base)

create_character_pro 256x256 base body. HARD CAP: 256x256, no exceptions (L11).

Animation drawn in-place at same canvas in PixelLab. User cannot hand-correct animation at sizes > 256.

Boss Unity math: PPU 64. 256x256 sprite = 4x4 Unity units. Warblade 64x64 = 1x1 unit. Boss = 4x character scale. This matches KEMURLUK ratio from Image 4 reference.

Multi-phase production: create_character_state for 3-4 phases (idle / damaged / enraged / on-fire per Karar #145).

NO parca-parca for boss. Native 256 create_character_pro direct.

Cost: ~80-160 gen per boss (base + 3-4 state calls).

Status: PENDING (Phase L, after vertical slice unlock)

---

### RULE 12 -- Combat showcase Codex tasks (Phase L)

| Step | Task | Estimate |
|---|---|---|
| L.1 | Damage number system (TextMeshPro Canvas + DOTween float, color-coded) | 1 day |
| L.2 | Chain projector VFX (URP Sprite Shader + Bezier curve, cyan radiating) | 2 days |
| L.3 | Status FX labels (Particle + Canvas: Frozen/Shattered/Chained/Combo) | 1 day |
| L.4 | Atmospheric lighting profile (URP 2D Light dual-tone + post-process) | 1 day |
| L.5 | Combat UI Canvas (boss HP top + skill bar + combo panel) | 2 days |
| L.6 | Sword arc + spell trail VFX (Trail Renderer + Sprite Anim) | 2 days |

Total Phase L dev: ~2-3 weeks. L.1-L.6 are independent of boss sprite -- can parallel with RULE 11.

Status: PENDING (Phase L)

---

### RULE 13 -- Planning UI (Phase M, deferred)

Image 3 reference (Class Arsenal, Build Loadout, Mission Planning UI) is the design target. Full-screen planning UI: class selector, build loadout, mission planning panel, inventory grid.

Separate sprint, post-Phase L. No scope or timeline set.

Status: PENDING (Phase M)

---

## Section 6 -- Budget table

Running total against 2265 gen budget. All figures are estimates pre-dispatch.

| Rule | Gen low | Gen high | Cumulative low | Cumulative high |
|---|---:|---:|---:|---:|
| 3 Wall prototype | 20 | 40 | 20 | 40 |
| 1 Floor base | 25 | 25 | 45 | 65 |
| 2 Floor transition | 110 | 110 | 155 | 175 |
| 4 Wall full library | 125 | 200 | 280 | 375 |
| 5 Prop 32x32 | 30 | 30 | 310 | 405 |
| 6 Prop 48-80px | 30 | 60 | 340 | 465 |
| 7 Prop 88-168px hero | 120 | 300 | 460 | 765 |
| 8 Decal layer | 105 | 120 | 565 | 885 |
| 9 Hero anchor manual (user-paced) | 300 | 600 | 865 | 1485 |
| 10 Small variant state (autonomous) | 200 | 400 | 1065 | 1885 |
| 11 Boss Phase L | 80 | 160 | 1145 | 2045 |
| Spare for re-gen + Acts 2-4 expansion | -- | -- | 220 | 1120 |

**Budget ceiling:** 2265 gen remaining. All RULES fit within budget. Spare at worst case: ~220 gen.

---

## Section 7 -- Decision lock order (what gets asked next, sequentially)

1. RULE 3 (wall prototype) -- first dispatch. Unblocks all object production. No dependency.
2. RULE 1 (floor base) -- small commit, low risk, runs after RULE 3 confirmed.
3. RULE 5 item list approval -- 32px batch final slot list.
4. RULE 6 batch composition -- medium prop list confirmation.
5. RULE 7 batch composition -- hero prop list + parca-parca candidates identified.
6. RULE 8 decal categorical narrowing -- which variants per category.
7. RULE 9 hero anchor selection -- after RULE 7 sprites exist.
8. RULE 10 small variant scope -- after RULE 5 and RULE 9 decisions.
9. RULE 2 floor transition -- after RULE 1 style anchor confirmed.
10. RULE 4 full wall library -- after RULE 3 prototype PASS.
11. RULE 11/12/13 -- Phase L/M scope, post-asset-base vertical slice.

---

## Section 8 -- Risks and open questions

| Risk | Severity | Mitigation |
|---|---|---|
| Parca-parca Inpaint v3 seam quality untested at PixelLab | HIGH | RULE 3 prototype validates PixelLab output first. Parca-parca is first applied on archway_n after RULE 3 passes. If Inpaint v3 seam fails: fallback = single native 256 create_object for that asset, accept lower detail. |
| Aseprite manual seam labor per large asset | MEDIUM | Estimate 5-15 min per seam. If > 20 assets need stitching total labor becomes significant. Mitigation: keep parca-parca only for assets truly >= 128px; native single-canvas for 88-127px range. |
| PixelLab "high top-down" output quality vs ChatGPT mockup reference | HIGH | RULE 3 mandatory quality gate. REWORK loop until front-face reads correctly before RULE 4. |
| Dungeon edge wall seam on horizontal run | MEDIUM | Produce modular segments (64x96 each). Seam handled at Unity painter level by abutting sprites. Inpaint stitch not required for tiling segments -- only for hero/accent walls that cross natural stitch boundaries. |
| Boss 256 scale vs screen real estate | LOW | L11 math: 256 = 4x4 Unity units at PPU 64. Matches KEMURLUK reference ratio. If screen feels crowded, scale boss in Unity (uniform scale, no new sprite). |
| Wang16 batch palette consistency vs RULE 1 anchor | MEDIUM | Pass style_images from RULE 1 floor output into every Wang16 transition batch. |
| Cross-act batch palette bleed | MEDIUM | Test 1 mixed batch (RULE 5 first call) before committing cross-act fill. |
| Phase L VFX scope creep | MEDIUM | L.1-L.6 are discrete Codex tasks. Track separately from asset production track. |

---

## Section 9 -- Image reference summary (pointer to v3)

Full annotations for 4 reference images (Image 1 RIMA combat / Image 2 Catlak Mezarlik / Image 3 Class Arsenal / Image 4 Rift Sanctum boss) are in v3 Section 8. v4 does not duplicate them.

Key production uses:
- Image 1/2: wall + floor atmosphere target for RULE 3/4 quality gate
- Image 3: Phase M Planning UI reference only
- Image 4: Phase L combat showcase target (boss sprite + chain VFX + damage numbers)

---

## Section 10 -- Character roster lock (carried from v3)

RIMA canonical roster (LOCKED): Warblade, Ronin, Gunslinger, Ranger, Elementalist, Shadowblade, Ravager, Hexer, Brawler, Summoner.

ChatGPT mockup class names (Image 3) are atmosphere reference only. No rename.

---

## Confirmation summary (v4)

| Metric | Count | Detail |
|---|---|---|
| LOCK count | 13 | L1-L13; +4 new vs v3 |
| New locks vs v3 | +4 | L10 (parca-parca), L11 (boss 256 cap), L12 (HYBRID seam), L13 (6-layer floor) |
| PENDING_DECISION rules | 6 | RULE 5, 6, 7, 8, 9, 10 |
| PENDING_LOCK rules | 4 | RULE 1, 2, 3, 4 |
| PENDING (future phase) | 3 | RULE 11, 12, 13 |
| Format change vs v3 | Full | Section 5 replaced with declarative RULE 1-13 list |
| v3 outpaint policy | REVISED | Boss >256 outpaint case REMOVED by L11. Parca-parca (L10) is the new large-asset path -- does not apply to boss. |
| v1, v2, v3 files | RETAINED | Do not delete |
