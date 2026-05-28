# Asset Production Master Plan v1

**File:** `STAGING/asset_production_master_plan_v1.md`
**Last updated:** 2026-05-22
**Owner:** Orchestrator reads this before any asset dispatch. Do NOT modify decisions; update Status column only.

## Lock status legend

| Symbol | Meaning |
|---|---|
| PENDING_DECISION | User must choose between listed options |
| PENDING_LOCK | Decision reached, awaiting explicit user confirmation ("bunu yap") |
| PENDING_TEST | 1-2 test assets required before locking |
| LOCKED | User confirmed. Do not re-decide. |
| IN_PROGRESS | Active dispatch running |
| DONE | Shipped and committed |

---

## Section 0 -- Read-first context

This file is the single source of truth for asset production execution order.
Every item is PENDING_LOCK or PENDING_DECISION until user gives explicit approval.
Orchestrator reads this file at session start, finds the first non-DONE item, and acts.
No item may be dispatched autonomous unless status = LOCKED.

---

## Section 1 -- Locked decisions (do not re-open)

| # | Decision | Detail | Status | Source |
|---|---|---|---|---|
| L1 | Pure top-down pivot | Map view ~85-90 deg ortho, ref STAGING/CHATGPT_TOPDOWN/*.png | LOCKED | project_topdown_pivot_lock.md |
| L2 | Weapon decouple arch (Karar #123/#144/#146) | Body weaponless + Child SR + HandAnchor; 12 weapon sprites incl. Elementalist orb | LOCKED | STAGING/weapon_pipeline_v1.md, STAGING/weapon_web_prompts_v1.md |
| L3 | No PixelLab gen during autonomous night runs | create_tiles_pro / create_object / animate_* blocked; get_* / list_* read-only only | LOCKED | feedback_no_pixellab_night_autonomous.md |
| L4 | Hibrit variant strategy (C) | Act 2-4 anchor hero sprites = user manual Edit Image Pro; small props + decals = orchestrator state pipeline autonomous | LOCKED | 2026-05-22 user direction |
| L5 | Tile pipeline: Modular (Mod B) | create_tiles_pro Mod B + manual style_images transition; Wang autotile deprecated for RIMA | LOCKED | project_modular_pipeline_lock.md |
| L6 | Wall + decoration pipeline | create_object n_frames=16 GameObject placement (NOT tile) | LOCKED | project_modular_pipeline_lock.md |
| L7 | Tile size | 64x64 px production (matches char 64 PPU = 1 cell) | LOCKED | S98 baseline |
| L8 | Painter default | RimaWorldPainterWindow PaintMode.TopDown primary | LOCKED | S98 |

---

## Section 2 -- PixelLab tool parameter LOCK status

| Tool | Parameter | Locked value | Status | Source |
|---|---|---|---|---|
| create_tiles_pro | tile_type | square_topdown | LOCKED | NLM S95, reference_pixellab_create_tiles_pro_4type memory |
| create_tiles_pro | tile_view | top-down (PURE, not "high") | LOCKED | NLM S95 |
| create_tiles_pro | tile_view_angle | 90 | LOCKED | NLM S95, S98 band bug derived |
| create_tiles_pro | tile_depth_ratio | 0 | LOCKED | NLM S95, S98 band bug derived |
| create_tiles_pro | outline_mode | segmentation | LOCKED | NLM S95 |
| create_object | view | PENDING_TEST -- `high top-down` ~30-35 deg; PixelLab exposes no pure-90 option; workaround: `high top-down` + prompt force "PURE 90 DEGREE OVERHEAD VIEW" | PENDING_TEST | Gap identified 2026-05-22; need 1-2 test prop to validate |
| create_object | directions | 1 | LOCKED | static prop |
| create_object | n_frames | rule-locked by canvas size | LOCKED | see rule below |
| create_object | object_view | top-down | LOCKED | static |

### n_frames rule (canvas size -> batch capacity)

| Canvas size (px) | n_frames | Items per call |
|---|---|---|
| 32x32 | 64 | 64 |
| 48-80 | 16 | 16 |
| 88-168 | 4 | 4 |

---

## Section 3 -- Pricing reference (PixelLab v2 API)

| Tool | Gen cost / call | Output per call | Notes |
|---|---|---|---|
| create_object 32px | 20 gen | 64 items | n_frames=64 |
| create_object 48-80px | 30 gen | 16 items | n_frames=16 |
| create_object 88-168px | 40 gen | 4 items | n_frames=4 |
| create_object_state | 20-40 gen | 1 variant of existing | used for Act 2-4 small variants (autonomous) |
| create_tiles_pro | 25 gen | 16 tiles (4x4) | per call, 64px tile |
| Edit Image Pro (USER MANUAL, NOT MCP) | 20 / 25 / 40 by output size | 1 sprite | up to 256 / 314 / 512 px; Act 2-4 hero anchors |
| Inpaint v3 (USER MANUAL, NOT MCP) | 20 gen flat | 1 masked area | repairs / color shift |
| create_character | out of scope (user web UI only) | -- | separate phase |
| animate_* | out of scope (separate phase) | -- | after all statics done |

**Budget as of 2026-05-22 S98:** 2265 / 5000 gen remaining.

---

## Section 4 -- Asset taxonomy + canonical Act 1 needs

### Act 1 Shattered Keep -- ~110 asset target

| Layer | Type | Count | Notes |
|---|---|---|---|
| L1 Floor Base | Cool Granite 16-tile set | 16 | create_tiles_pro Mod B |
| L2 Floor Variation | Worn Stone Path 16-tile set | 16 | create_tiles_pro Mod B |
| L3 Wall Overlay | 8 class x 3 var = 24 sprite | 24 | create_object; straight/corner/hero/broken/arch/pillar/collapsed_stub |
| L4 Large Patches | 3 materials x 8 var = 24 sprite | 24 | Cave Moss + Dust Drift + Cracked Rubble |
| L5 Scatter | ~18 sprite | 18 | small stones, chain remnants, bone fragments |
| L6 Accent/Hero | ~12 sprite | 12 | cyan rift, brazier, banner |

**Total floor call target:** 4 create_tiles_pro calls @ 25 gen each = 100 gen for 64 tiles.

### Prefab names in use (from act1_shattered_keep_layout_v1.json)

walls[]: `archway_n`, `column_hero`, `column_broken`
props[]: `torch_wall`, `banner_tattered`, `rubble_pile_small`, `rubble_pile_large`, `shrine_pedestal`, `throne_dais`, `skull_pile`

### Codex review gap items (STAGING/asset_pipeline_codex_review.md)

- pickup / key shard / relic (schema rewards.pickups[])
- gate socket variants: arch / breach / chained / rift / locked / bridge
- hazard / dash-only telegraph

---

## Section 5 -- Sequential decision list

Rules:
- Items are ordered. Do not skip ahead.
- Every item stays at its current status until user says "bunu yap" or "lock et" or equivalent.
- Orchestrator presents options for PENDING_DECISION items; user picks; orchestrator updates to PENDING_LOCK; user confirms; status becomes LOCKED.

---

### Item 1 -- Floor batch structure

**Decision:** How to split create_tiles_pro calls for Act 1 floor tiles.

| Option | Description | Gen cost | Tiles |
|---|---|---|---|
| i | 1 call: 4-tile mix (granite + rubble + walkway + rift), Act 1 only | 25 gen | 16 mixed |
| ii | 2 calls: Granite 16-var + Path 16-var separately | 50 gen | 32 pure-material |
| iii | 1 call: 4-tile mix -- 2 Act 1 materials + 2 Act 2 materials in same batch | 25 gen | 16 (cross-act risk -- see Section 6 WARNING) |

**Status:** PENDING_DECISION
**Gen estimate:** 25-50 gen depending on option
**Depends on:** none
**Source:** Modular Mod B pipeline lock; user "mantikli doldur" direction

---

### Item 2 -- create_object view parameter test

**Decision:** Lock `view` parameter for all create_object calls.

Current gap: PixelLab does not expose a pure 90-deg overhead option for objects (only `high top-down` ~30-35 deg). Proposed workaround: use `high top-down` + inject prompt text "PURE 90 DEGREE OVERHEAD VIEW, looking straight down, no perspective tilt" and evaluate output.

**Action required:** Dispatch 1-2 test prop calls (e.g., `rubble_pile_small` + `torch_wall`) and screenshot for user review before locking.

**Status:** PENDING_TEST
**Gen estimate:** 20-40 gen (1-2 calls at 32px)
**Depends on:** none (can run parallel to Item 1 decision)
**Source:** Gap identified 2026-05-22

---

### Item 3 -- 32x32 batch composition

**Decision:** What to put in the 32px n_frames=64 batch (small props + decals).

**Candidate list (Act 1, ~50 slots):**

| Slot range | Items |
|---|---|
| 1-8 | small stones (granite, rubble, loose, cracked x4 variants) |
| 9-16 | chain remnants (link, coil, spike anchor, rusted ring x4) |
| 17-24 | bone fragments (skull shard, rib, joint, finger pile x4 variants) |
| 25-32 | rubble scatter (small chip, dust pile, pebble cluster, mortar crumble) |
| 33-40 | decals -- blood smear, scorch mark, moss patch, footprint set |
| 41-48 | decals -- summon circle faint, arcane etching, cyan crack hairline, rift dust |
| 49-50 | misc -- key shard placeholder, relic shard placeholder |

**Remaining 14 slots (Act 2-4 fill TBD at lock time):** Small props for Act 2-4 biomes that share same canvas size. User confirms fill list at lock.

**Status:** PENDING_DECISION
**Gen estimate:** 20 gen per call; 1-3 calls depending on how many 32px batches approved
**Depends on:** Item 2 (view param test must pass first)
**Source:** Batch fill rule Section 6; user "batch doldur" direction

---

### Item 4 -- 48-80px batch composition

**Decision:** What to put in the 48-80px n_frames=16 batch (medium props).

**Candidate list (Act 1, up to 16 slots per batch):**

| Slot | Item | Canvas |
|---|---|---|
| 1 | torch_wall | 48px |
| 2 | banner_tattered | 64px |
| 3 | candle_tall | 48px |
| 4 | brazier_floor | 64px |
| 5 | urn_broken | 64px |
| 6 | urn_whole | 64px |
| 7 | lantern_hanging | 48px |
| 8 | wall_sconce | 48px |
| 9-16 | Act 2-4 fill (TBD at lock time; user approves) | 48-80px |

**Note:** If Act 1 needs exceed 16 slots, dispatch 2 batches. Fill any extra slots in batch 2 with Act 2-4 medium props.

**Status:** PENDING_DECISION
**Gen estimate:** 30 gen per call; likely 1-2 calls
**Depends on:** Item 2 (view param test)
**Source:** Batch fill rule Section 6

---

### Item 5 -- 88-168px batches (large hero props)

**Decision:** What to put in each 4-slot 88-168px batch.

**Act 1 hero asset list (each batch = 4 items):**

| Batch | Items | Canvas |
|---|---|---|
| 5A | column_hero, column_broken, archway_n, pillar_stub | 128px / 128px / 168px / 88px |
| 5B | shrine_pedestal, throne_dais, sarcophagus, altar_stone | 128px each |
| 5C | skull_pile, rubble_pile_large, rift_crystal, gate_arch | 88-128px |

**Remaining slots in each batch for Act 2-4 fill:** If a batch is under 4 Act 1 items, user confirms which Act 2-4 hero counterpart fills the gap.

**Status:** PENDING_DECISION
**Gen estimate:** 40 gen per call; 3 calls = 120 gen
**Depends on:** Item 2 (view param test)
**Source:** act1_shattered_keep_layout_v1.json walls[] and props[]

---

### Item 6 -- Wall geometry decision

**Decision:** Flat pure-90 wall tiles vs front-face depth sprites vs prototype-first.

User earlier indicated "prototype-first." Proposed lock: dispatch 1 wall prototype batch (4-slot 128px call: straight_wall_n, corner_wall_nw, corner_wall_ne, wall_end_n). Evaluate output. Lock geometry approach based on verdict.

**Status:** PENDING_LOCK
**Gen estimate:** 40 gen (1 call, 4 items)
**Depends on:** Item 2 (view param test)
**Source:** User "prototype-first" direction; wall library Phase B roadmap

---

### Item 7 -- Decal categorical narrowing

**Decision:** Which decal categories to keep after floor tile mix covers Rubble + Rift materials.

**Proposed keep list:** moss_patch, blood_smear, bone_scatter_decal, footprint_set, scorch_mark, summon_circle_faint, dust_drift_decal

**Proposed drop (covered by tile):** crack_decal (covered by cracked rubble tile), cyan_accent_decal (covered by rift tile)

User must confirm keep/drop list before any decal gen.

**Status:** PENDING_DECISION
**Gen estimate:** 0 gen (decision only; affects Item 3 slot allocation)
**Depends on:** Item 1 (to know which materials are in floor tile)
**Source:** L4/L5 taxonomy; Codex review gap analysis

---

### Item 8 -- Hero anchor list for Hibrit Act 2-4 (user manual Edit Image Pro)

**Decision:** Which assets get user-manual Edit Image Pro treatment for Act 2-4 biome shift.

**Candidate hero assets (user manually recolors / repaints via web UI):**

column_hero, archway_n, throne_dais, sarcophagus, shrine_pedestal, banner_tattered, brazier_floor, urn_whole, rift_crystal, altar_stone

User selects 5-10 of these. Autonomous pipeline handles everything else via create_object_state.

**Status:** PENDING_DECISION
**Gen estimate:** 20-40 gen per user manual call (Edit Image Pro); not counted against autonomous budget
**Depends on:** Item 5 (must have Act 1 hero sprites generated first)
**Source:** L4 Hibrit strategy (C) lock

---

### Item 9 -- Small variant scope for state pipeline (Act 2-4 autonomous)

**Decision:** Which decals + small props get create_object_state variants for Act 2-4.

Autonomous state pipeline generates biome-shift variants (recolor, texture swap) without user.
Must be restricted to items that do NOT appear in hero anchor list (Item 8).

**Proposed in-scope:** small stones, chain remnants, generic rubble scatter, moss/blood/scorch decals
**Proposed exclude (user-manual):** whatever user selects in Item 8

User confirms scope at lock time.

**Status:** PENDING_DECISION
**Gen estimate:** 20-40 gen per state call; number of calls = (items in scope) / 1 (1 state variant per call)
**Depends on:** Item 3, Item 8 (scope defined after hero list locked)
**Source:** Hibrit strategy (C); user "kucuk objeler autonomous" direction

---

### Item 10 -- Production trigger order

**Decision:** Which batch ships first to vertical slice after all locks confirmed.

**Proposed order:**
1. Item 1 (floor tiles) -- foundation; painter cannot function without floor material
2. Item 2 test (view param) -- gate for all create_object calls
3. Item 6 (wall prototype) -- one call to validate geometry before bulk wall gen
4. Item 4 (medium props: torch/banner) -- highest visual impact per gen spent
5. Item 3 (32px scatter/decal) -- fill density; lower priority than structure
6. Item 5 (hero props) -- large canvas, expensive; after structure confirmed
7. Item 8 (user manual Act 2-4 heroes) -- user-paced, off-band
8. Items 9 (state pipeline Act 2-4 variants) -- after Act 1 complete

User confirms or reorders.

**Status:** PENDING_LOCK
**Gen estimate:** N/A (ordering decision, no gen cost)
**Depends on:** Items 1-9
**Source:** Vertical slice first principle; "oynanabilir plan" user direction

---

## Section 6 -- Batch fill rule (no waste)

Every batch must be filled to capacity. Do not ship a half-full batch.

| Batch type | Capacity | Act 1 fill | Remaining for Act 2-4 |
|---|---|---|---|
| 32px n_frames=64 | 64 | ~50 Act 1 small items | ~14 slots Act 2-4 small props |
| 48-80px n_frames=16 | 16 per call | 8 Act 1 medium items | 8 slots Act 2-4 medium props |
| 88-168px n_frames=4 | 4 per call | 3-4 Act 1 hero items | 0-1 slots per batch |
| create_tiles_pro | 16 tiles per call | depends on option chosen in Item 1 | N/A (tile, not object) |

**Fill priority:** Act 1 items first. If capacity remains, Act 2-4 items of same canvas size that share biome-adjacent palette (cold stone, moss, wood, metal).

**WARNING -- style/palette consistency risk:** Mixing Act 1 + Act 2-4 assets in one batch risks PixelLab defaulting to a shared palette that washes out individual biome identity. Mitigation: test with 1 mixed batch (Item 3 first call) before committing to cross-act fill strategy. If palette bleeds, switch to same-act-only batches and accept partial fill waste.

---

## Section 7 -- Total gen budget tracking

Updated as items lock and dispatch.

| Item | Description | Gen | Cumulative spent |
|---|---|---|---|
| (all pending) | -- | -- | 0 |

**Budget remaining:** 2265 / 5000 gen as of 2026-05-22 S98.

**Projected spend when all Act 1 items locked (estimate):**

| Phase | Calls | Gen est. |
|---|---|---|
| Floor tiles (4 Mod B calls) | 4 | 100 |
| create_object view param test | 2 | 40-60 |
| Wall prototype | 1 | 40 |
| Wall library full (5 batches 88-128px) | 5 | 200 |
| Medium props (48-80px, 2 calls) | 2 | 60 |
| Small props/decals (32px, 1-2 calls) | 2 | 40-60 |
| Hero props (88-168px, 3 calls) | 3 | 120 |
| **Act 1 total estimate** | **19-21 calls** | **~620-640 gen** |
| Reserve after Act 1 | | ~1620 / 5000 |
| Act 2-4 state variants (autonomous) | TBD | ~300-500 |
| User manual Edit Image Pro (off-budget) | TBD | user-paced |

**Budget is comfortable for Act 1 + Act 2-4 autonomous variants. Animate phase is separate and not counted here.**

---

## Section 8 -- Open risks + flags

| Risk | Severity | Mitigation |
|---|---|---|
| create_object view param unvalidated | HIGH | Item 2 test mandatory before bulk dispatch |
| Cross-act batch palette bleed | MEDIUM | 1 mixed batch test before committing cross-act fill |
| Act 1 hero list vs layout mismatch | LOW | layout_v1.json walls[] + props[] are source of truth; all hero assets must map to a prefab name in that file |
| Pickup / key shard / gate socket gap | MEDIUM | Codex review flagged; add to Item 3 slot list or create separate 32px batch |
| Hazard / dash telegraph missing | MEDIUM | Not in current taxonomy; add as Item 3 overflow or new item |
