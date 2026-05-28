# Asset Production Master Plan v3

**File:** `STAGING/asset_production_master_plan_v3.md`
**Last updated:** 2026-05-22
**Supersedes:** `STAGING/asset_production_master_plan_v2.md` (v2 retained as historical context -- do NOT delete)
**Supersedes:** `STAGING/asset_production_master_plan_v1.md` (v1 retained as historical context -- do NOT delete)
**Owner:** Orchestrator reads this before any asset dispatch. Do NOT modify decisions; update Status column only.

## Change log (v2 -> v3)

| Change | Detail |
|---|---|
| L1 LOCKED Hades-iso | Pure top-down track REVOKED. Pivot resolved: Hades-iso ~70-75 deg is the production camera angle. Confirmed by 4 reference images (Image #3 RIMA + 3 ChatGPT mockups shared 2026-05-22). |
| L9 ADDED (NEW LOCK) | Tile seam strategy: create_tiles_pro style_images anchor + Wang16 corner transition (Karar #131 reactivated for material transitions). |
| Section 1 revised | L1 status changed PENDING_LOCK -> LOCKED. Pure top-down column/fallback text removed everywhere. L9 row added. |
| Section 2 revised | Single-track table (Track B only). Track A column removed. create_object view="high top-down" LOCKED. |
| Section 4 revised | Wall canvas: 64x96 or 64x128 LOCKED (front-face mandatory). Two-track height comparison table removed. |
| Section 5 Item 0 revised | Single-track Hades-iso prototype only. ~40-80 gen (half of v2 estimate, one track). |
| Section 5.5 revised | Outpainting policy resolved: HYBRID verdict. Native create_object first; outpaint for boss/hero > 256px only. Aseprite palette-snap pass mandatory. |
| Section 7 revised | Budget table updated: Track A column removed, single Track B projection. |
| Section 8 expanded | 4 reference images annotated: Image #3 RIMA combat, Catlak Mezarlik combat, Planning UI (Class Arsenal), Rift Sanctum boss. |
| Section 9 REPLACED | v2 two-track strategy table removed. v3: three production tracks (Asset Production, Combat Showcase Phase L, Planning UI Phase M). |
| Section 10 revised | Tile seam quality strategy: style_images anchor + Wang16 corner transitions. |
| Section 11 revised | New risk row: PixelLab "high top-down" actual output match to ChatGPT mockup quality level. L1 memory stale risk resolved. |
| Section 12 ADDED (NEW) | Execution order summary post-LOCK. |

---

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

**v3 critical note:** L1 pivot is now LOCKED Hades-iso. The two-track prototype test from v2 is revised to a single-track Hades-iso wall prototype (Item 0). Item 0 still gates all production items 1-10 -- it validates seam quality + front-face rendering + PixelLab view output match, not track selection.

---

## Section 1 -- Locked decisions (do not re-open unless noted)

| # | Decision | Detail | Status | Source |
|---|---|---|---|---|
| L1 | Pivot -- Hades-iso LOCKED | ~70-75 deg tilt. create_object view="high top-down" native match. Pure top-down track REVOKED. Reference: 4 images (Image #3 RIMA + ChatGPT mockups 2026-05-22). | LOCKED | User direction 2026-05-22; reference images STAGING/CHATGPT_MOCKUPS/ |
| L2 | Weapon decouple arch (Karar #123/#144/#146) | Body weaponless + Child SR + HandAnchor; 12 weapon sprites incl. Elementalist orb | LOCKED | STAGING/weapon_pipeline_v1.md, STAGING/weapon_web_prompts_v1.md |
| L3 | No PixelLab gen during autonomous night runs | create_tiles_pro / create_object / animate_* blocked; get_* / list_* read-only only | LOCKED | feedback_no_pixellab_night_autonomous.md |
| L4 | Hibrit variant strategy (C) | Act 2-4 anchor hero sprites = user manual Edit Image Pro; small props + decals = orchestrator state pipeline autonomous | LOCKED | 2026-05-22 user direction |
| L5 | Tile pipeline: Modular (Mod B) | create_tiles_pro Mod B + manual style_images transition; Wang autotile deprecated for standard floor | LOCKED | project_modular_pipeline_lock.md |
| L6 | Wall + decoration pipeline | create_object n_frames=16 (or 4 for tall canvas) GameObject placement (NOT tile); canvas 64x96/64x128 Hades-iso | LOCKED | project_modular_pipeline_lock.md; v3 pivot lock |
| L7 | Tile size | 64x64 px production (matches char 64 PPU = 1 cell) | LOCKED | S98 baseline |
| L8 | Painter default | RimaWorldPainterWindow PaintMode.TopDown primary | LOCKED | S98 |
| L9 | Tile seam strategy | create_tiles_pro style_images anchor for palette consistency + Wang16 corner tiles for material transitions (Karar #131 reactivated). 1 transition test batch before full production. | LOCKED | User direction 2026-05-22; Karar #131 |

**Memory file update required:** `project_topdown_fakeiso_pivot_lock.md` must be updated to reflect L1 LOCKED Hades-iso and pure top-down REVOKED. Orchestrator handles post-Item 0 if any adjustments needed after prototype validates; core pivot is already resolved.

---

## Section 2 -- PixelLab tool parameter LOCK status

### Tile parameters (floor -- Hades-iso, flat projection)

| Tool | Parameter | Value | Status |
|---|---|---|---|
| create_tiles_pro | tile_type | square_topdown | LOCKED |
| create_tiles_pro | tile_view | top-down (PURE) | LOCKED |
| create_tiles_pro | tile_view_angle | 90 | LOCKED (floor stays flat-projected in Hades-iso) |
| create_tiles_pro | tile_depth_ratio | 0 | LOCKED |
| create_tiles_pro | outline_mode | segmentation | LOCKED |
| create_tiles_pro | style_images | First batch anchor; pass as reference on all subsequent calls | LOCKED (L9) |

### Object parameters (walls + props -- Hades-iso single track)

| Tool | Parameter | Value | Status |
|---|---|---|---|
| create_object | view | "high top-down" | LOCKED -- native Hades-iso match, no prompt workaround needed |
| create_object | directions | 1 | LOCKED (static prop) |
| create_object | n_frames | 4 for 88-168px canvas (walls + large props); 16 for 48-80px; 64 for 32px | LOCKED |
| create_object | object_view | top-down | LOCKED |

**Key note:** PixelLab `view: "high top-down"` produces ~30-35 deg tilt which is the native Hades-iso camera angle. No prompt workaround required. This was the decisive technical factor in locking Track B.

### n_frames rule (canvas size -> batch capacity)

| Canvas size (px) | n_frames | Items per call | Hades-iso use |
|---|---|---|---|
| 32x32 | 64 | 64 | Decals + small scatter only |
| 48-80 | 16 | 16 | Medium props (torch, banner, urn) |
| 88-168 | 4 | 4 | Walls (64x96/128) + large props |

---

## Section 3 -- Pricing reference (PixelLab v2 API)

| Tool | Gen cost / call | Output per call | Notes |
|---|---|---|---|
| create_object 32px | 20 gen | 64 items | n_frames=64 |
| create_object 48-80px | 30 gen | 16 items | n_frames=16 |
| create_object 88-168px | 40 gen | 4 items | n_frames=4; wall tier |
| create_object_state | 20-40 gen | 1 variant of existing | Act 2-4 small variants (autonomous) |
| create_tiles_pro | 25 gen | 16 tiles (4x4) | per call, 64px tile |
| Edit Image Pro (USER MANUAL, NOT MCP) | 20/25/40 by output size | 1 sprite | up to 256/314/512 px; Act 2-4 hero anchors |
| Inpaint v3 (USER MANUAL, NOT MCP) | 20 gen flat | 1 masked area | palette-locking pass; outpaint extension for boss/hero >256px |
| create_character | out of scope (user web UI only) | -- | separate phase |
| animate_* | out of scope (separate phase) | -- | after all statics done |

**Budget as of 2026-05-22 S98:** 2265 / 5000 gen remaining.

---

## Section 4 -- Asset taxonomy + canonical Act 1 needs

### Act 1 Shattered Keep -- ~110 asset target

| Layer | Type | Count | Notes |
|---|---|---|---|
| L1 Floor Base | Cool Granite 16-tile set | 16 | create_tiles_pro Mod B; flat-projected 90 deg |
| L2 Floor Variation | Worn Stone Path 16-tile set | 16 | create_tiles_pro Mod B |
| L3 Wall Overlay | 8 class x 3 var = 24 sprite | 24 | create_object 64x96/64x128; front-face mandatory |
| L4 Large Patches | 3 materials x 8 var = 24 sprite | 24 | Cave Moss + Dust Drift + Cracked Rubble |
| L5 Scatter | ~18 sprite | 18 | small stones, chain remnants, bone fragments |
| L6 Accent/Hero | ~12 sprite | 12 | cyan rift, brazier, banner, column hero, sarcophagus |

### Wall class list (Hades-iso, front-face mandatory)

| Class | Canvas | Notes |
|---|---|---|
| straight_wall_n | 64x96 | Primary corridor wall |
| corner_wall_convex | 64x96 | Outer corner |
| corner_wall_concave | 64x96 | Inner corner |
| wall_end_stub | 64x64 | Short wall cap |
| collapsed_stub | 64x96 | Ruined partial wall |
| archway_n | 64x128 | Hero prop double-duty as wall class |
| pillar_hero | 64x96 | Standalone column acting as wall break |
| pillar_broken | 64x96 | Variant |

**8 wall classes x 3 variants = 24 sprites.** Canvas 64x96 or 64x128 = 88-168px batch tier, n_frames=4, 1 call per wall class. Total wall calls: 8+ calls, ~320 gen.

### Prefab names in use (from act1_shattered_keep_layout_v1.json)

walls[]: `archway_n`, `column_hero`, `column_broken`
props[]: `torch_wall`, `banner_tattered`, `rubble_pile_small`, `rubble_pile_large`, `shrine_pedestal`, `throne_dais`, `skull_pile`

### Outpainting candidates (Section 5.5 HYBRID policy)

Boss sprite, throne_dais (if > 192px), large archway. All others: native create_object.

---

## Section 5 -- Sequential decision list

Rules:
- Items are ordered. Do not skip ahead.
- Every item stays at its current status until user says "bunu yap" or "lock et" or equivalent.
- Orchestrator presents options for PENDING_DECISION items; user picks; orchestrator updates to PENDING_LOCK; user confirms; status becomes LOCKED.
- **Item 0 gates Items 1-10.** Item 0 validates seam quality + front-face rendering + style output -- track selection is already resolved (Hades-iso LOCKED).

---

### Item 0 -- Hades-iso wall prototype (GATES ALL OTHERS)

**Purpose:** Validate PixelLab `view: "high top-down"` output quality matches Hades-iso reference. Confirm seam quality, front-face rendering, and style against Image #3 / ChatGPT mockup targets. Track selection is NOT the question -- Hades-iso is LOCKED. This is a quality gate.

**Action required:**
1. Dispatch 1-2 wall sprites (e.g., `straight_wall_n`, `corner_wall_convex`) with `view: "high top-down"`, canvas 64x96 or 64x128
2. Prompt injection: "pixel art dungeon wall, front face visible, Hades-style isometric view, warm torch orange light from left, cold cyan rift glow accent, cracked stone texture, dark atmosphere"
3. Place in Unity scene (existing Phase I room or new test room)
4. Screenshot for user review
5. User confirms PASS (proceed to Items 1/3/4/5/6) or REWORK (adjust prompt + retry)

**Gen estimate:** 40-80 gen (1-2 wall calls at 40 gen each; single track, half of v2 estimate)
**Status:** PENDING_LOCK (user confirmed Hades-iso lock; awaiting dispatch confirmation)
**Depends on:** none (first action)

---

### Item 1 -- Floor batch structure

**Decision:** How to split create_tiles_pro calls for Act 1 floor tiles.

| Option | Description | Gen cost | Tiles |
|---|---|---|---|
| i | 1 call: 4-tile mix (granite + rubble + walkway + rift), Act 1 only | 25 gen | 16 mixed |
| ii | 2 calls: Granite 16-var + Path 16-var separately | 50 gen | 32 pure-material |
| iii | 1 call: 4-tile mix -- 2 Act 1 materials + 2 Act 2 materials in same batch | 25 gen | 16 (cross-act risk -- see Section 6 WARNING) |

**Note (v3):** First call output becomes style_images anchor for all subsequent tile calls (L9 seam strategy). Choose the most palette-representative tile set as first call.

**Status:** PENDING_DECISION
**Gen estimate:** 25-50 gen depending on option
**Depends on:** Item 0 (seam quality confirmed first)

---

### Item 2 -- create_object view parameter test

**RESOLVED BY ITEM 0 (v2 change carried forward).** Item 0 wall prototype directly validates the `view` parameter. Item 2 is retired.

**Status:** RESOLVED BY ITEM 0
**Gen estimate:** 0

---

### Item 3 -- 32x32 batch composition

**Decision:** What to put in the 32px n_frames=64 batch (small props + decals).

| Slot range | Items |
|---|---|
| 1-8 | small stones (granite, rubble, loose, cracked x4 variants) |
| 9-16 | chain remnants (link, coil, spike anchor, rusted ring x4) |
| 17-24 | bone fragments (skull shard, rib, joint, finger pile x4 variants) |
| 25-32 | rubble scatter (small chip, dust pile, pebble cluster, mortar crumble) |
| 33-40 | decals -- blood smear, scorch mark, moss patch, footprint set |
| 41-48 | decals -- summon circle faint, arcane etching, cyan crack hairline, rift dust |
| 49-50 | misc -- key shard placeholder, relic shard placeholder |

**Remaining 14 slots (Act 2-4 fill TBD):** Small props for Act 2-4 biomes sharing same canvas size.

**Note (v3 -- Hades-iso atmosphere):** blood_smear and rift_crack_hairline are atmospheric density targets per Image #3 / ChatGPT mockup reference. Retain both regardless of floor tile coverage.

**Status:** PENDING_DECISION
**Gen estimate:** 20 gen per call; 1-3 calls
**Depends on:** Item 0, Item 7 (decal narrowing)

---

### Item 4 -- 48-80px batch composition

**Decision:** What to put in the 48-80px n_frames=16 batch (medium props).

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
| 9-16 | Act 2-4 fill (TBD at lock time) | 48-80px |

**Status:** PENDING_DECISION
**Gen estimate:** 30 gen per call; likely 1-2 calls
**Depends on:** Item 0

---

### Item 5 -- 88-168px batches (large hero props)

**Decision:** What to put in each 4-slot 88-168px batch.

| Batch | Items | Canvas |
|---|---|---|
| 5A | column_hero, column_broken, archway_n, pillar_stub | 128px / 128px / 168px / 88px |
| 5B | shrine_pedestal, throne_dais, sarcophagus, altar_stone | 128px each |
| 5C | skull_pile, rubble_pile_large, rift_crystal, gate_arch | 88-128px |

**Outpaint note (v3 -- HYBRID policy):** sarcophagus and throne_dais are outpaint candidates if they read below 192px native. Pipeline: base 128 -> Aseprite canvas extend -> Inpaint v3 mask transparent area -> palette snap -> seam polish. See Section 5.5.

**Status:** PENDING_DECISION
**Gen estimate:** 40 gen per call; 3 calls = 120 gen
**Depends on:** Item 0, Section 5.5 policy (RESOLVED -- HYBRID)

---

### Item 6 -- Wall production batch (LOCKED track, pending dispatch)

**v3 note:** Track selection is resolved (Hades-iso LOCKED). After Item 0 validates prototype quality, Item 6 becomes full wall library dispatch.

**Content (Hades-iso LOCKED):** 8 wall classes x 3 variants, canvas 64x96/64x128, 88-168px tier. Each call = 4 wall variants. ~6-8 calls total.

**Status:** PENDING_LOCK (awaits Item 0 quality pass)
**Gen estimate:** 240-320 gen (full wall library, Track B)
**Depends on:** Item 0

---

### Item 7 -- Decal categorical narrowing

**Decision:** Which decal categories to keep after floor tile mix covers base materials.

**Proposed keep list:** moss_patch, blood_smear, rift_crack_hairline, bone_scatter_decal, footprint_set, scorch_mark, summon_circle_faint, dust_drift_decal
**Proposed drop (covered by tile):** crack_decal (covered by cracked rubble tile)

**Note (v3):** cyan_crack retained (atmospheric per Image #3/mockup). Atmospheric density is a design target -- Hades-iso requires layered decal depth for scene quality.

**Status:** PENDING_DECISION
**Gen estimate:** 0 gen (decision only; affects Item 3 slot allocation)
**Depends on:** Item 0, Item 1

---

### Item 8 -- Hero anchor list for Hibrit Act 2-4 (user manual Edit Image Pro)

**Decision:** Which assets get user-manual Edit Image Pro treatment for Act 2-4 biome shift.

**Candidate hero assets:** column_hero, archway_n, throne_dais, sarcophagus, shrine_pedestal, banner_tattered, brazier_floor, urn_whole, rift_crystal, altar_stone

User selects 5-10 of these. Autonomous pipeline handles everything else via create_object_state.

**Status:** PENDING_DECISION
**Gen estimate:** 20-40 gen per user manual call; not counted against autonomous budget
**Depends on:** Item 5 (Act 1 hero sprites first)

---

### Item 9 -- Small variant scope for state pipeline (Act 2-4 autonomous)

**Decision:** Which decals + small props get create_object_state variants for Act 2-4.

Autonomous state pipeline generates biome-shift variants (recolor, texture swap) without user. Must be restricted to items that do NOT appear in hero anchor list (Item 8).

**Proposed in-scope:** small stones, chain remnants, generic rubble scatter, moss/blood/scorch decals
**Proposed exclude:** whatever user selects in Item 8

**Status:** PENDING_DECISION
**Gen estimate:** 20-40 gen per state call
**Depends on:** Item 3, Item 8

---

### Item 10 -- Production trigger order

**Decision:** Confirm final dispatch sequence.

**Proposed order (v3 -- single track):**

1. Item 0 (Hades-iso wall prototype) -- quality gate; confirms seam + front-face
2. Item 1 (floor tiles) -- foundation; first batch becomes style_images anchor
3. Item 6 (wall library full batch) -- geometry confirmed by Item 0
4. Item 4 (medium props: torch/banner) -- highest visual impact per gen
5. Item 3 (32px scatter/decal) -- fill density
6. Item 5 (hero props) -- large canvas, expensive; after structure confirmed
7. Item 8 (user manual Act 2-4 heroes) -- user-paced, off-band
8. Item 9 (state pipeline Act 2-4 variants) -- after Act 1 complete

**Status:** PENDING_LOCK
**Gen estimate:** N/A (ordering decision, no gen cost)
**Depends on:** Items 0-9

---

## Section 5.5 -- Outpainting / inpaint stitching policy (RESOLVED -- HYBRID)

**Research dispatched to:** `STAGING/research_outpainting_inpaint_stitching.md`

**Verdict: HYBRID**

| Policy | Rule |
|---|---|
| Default | Native create_object 256x256 FIRST for all assets |
| Outpaint trigger | Only for boss sprite (>256px), throne_dais if it scales above 192px, large dramatic archway |
| Tool | Inpaint v3 (user manual, NOT MCP) -- 20 gen flat per masked area |
| Palette discipline | Aseprite palette-snap manual pass MANDATORY after every inpaint round |
| Pipeline | base 128 -> Aseprite canvas extend -> Inpaint v3 mask transparent area -> palette snap -> seam polish |
| Cost | Inpaint v3 = 20 gen flat (cheaper than additional create_object calls for size extension) |
| Palette lock quality | Inpaint v3 community-validated better than SD/ComfyUI for pixel art palette consistency |

**RIMA policy:** Default = native create_object. Outpaint only for boss and hero items explicitly listed above. Do not generalize outpainting without explicit user approval per asset.

---

## Section 6 -- Batch fill rule (no waste)

Every batch must be filled to capacity. Do not ship a half-full batch.

| Batch type | Capacity | Act 1 fill | Remaining for Act 2-4 |
|---|---|---|---|
| 32px n_frames=64 | 64 | ~50 Act 1 small items | ~14 slots Act 2-4 small props |
| 48-80px n_frames=16 | 16 per call | 8 Act 1 medium items | 8 slots Act 2-4 medium props |
| 88-168px n_frames=4 | 4 per call | 3-4 Act 1 hero items | 0-1 slots per batch |
| create_tiles_pro | 16 tiles per call | depends on Item 1 option | N/A |

**Hades-iso wall batch note (v3):** Wall sprites at 64x96/64x128 fit 88-168px tier (n_frames=4). 1 call per wall class (4 variants per call). 8 wall classes = 8+ calls. Do not mix walls with props -- separate calls.

**Fill priority:** Act 1 items first. Cross-act fill uses same-canvas-size biome-adjacent assets. See Section 11 for palette bleed risk.

**WARNING -- style/palette consistency risk:** Mixing Act 1 + Act 2-4 assets risks PixelLab defaulting to a shared palette that washes out biome identity. Test with 1 mixed batch (Item 3 first call) before committing to cross-act fill strategy.

---

## Section 7 -- Total gen budget tracking

Updated as items lock and dispatch.

| Item | Description | Gen | Cumulative spent |
|---|---|---|---|
| (all pending) | -- | -- | 0 |

**Budget remaining:** 2265 / 5000 gen as of 2026-05-22 S98.

**Projected spend (Hades-iso single track, Act 1 full production):**

| Phase | Calls | Gen est. |
|---|---|---|
| Item 0: Hades-iso wall prototype | 1-2 | 40-80 |
| Floor tiles (Mod B, 1-2 calls) | 1-2 | 25-50 |
| Wall library full (8 classes, 88-168px, ~8 calls) | 8 | 320 |
| Medium props (48-80px, 1-2 calls) | 2 | 60 |
| Small props/decals (32px, 1-2 calls) | 2 | 40-60 |
| Hero props (88-168px, 3 calls) | 3 | 120 |
| Outpaint stitch rounds (HYBRID -- 2-3 hero assets) | 4-6 | 40-60 |
| Wang16 transition test batch | 1 | 25 |
| **Act 1 total estimate** | | **~670-775 gen** |
| Reserve after Act 1 | | **~1490-1595 gen** |
| Act 2-4 state variants (autonomous, TBD) | TBD | ~300-500 |
| Phase L boss sprite batch | 1-2 | 80-160 |

---

## Section 8 -- Reference image annotations (4 images, 2026-05-22)

**Reference set location:** `STAGING/CHATGPT_MOCKUPS/` (orchestrator / user to copy shared images to this folder)

### Image 1 -- RIMA earlier combat reference (Yikilmis Gecit)

| Attribute | Value |
|---|---|
| Camera tilt | ~70-75 deg (Hades-iso confirmed) |
| Scene | Hooded warrior + 4 mob + rift portal right |
| Floor | Cracked granite + blood decals |
| Wall | Front-face visible, strong perspective depth |
| Lighting | Warm torch orange + cold cyan rift dual-tone |
| VFX | Sword arc trail |
| UI | Corner HP/MP + mini-map + objective panel, skill bar 1/2/3/4/Q/R |
| Production use | Primary visual target for wall + floor atmosphere |

### Image 2 -- Catlak Mezarlik combat (ChatGPT mockup)

| Attribute | Value |
|---|---|
| Camera tilt | ~70-75 deg (same projection, consistent) |
| Scene | 5 mob + player, rift crystal right, ritual stone circle |
| Floor | Cracked granite, blood decals, ritual stone circle overlay |
| Lighting | Torch warm + rift cyan + subtle purple ambient |
| Risk badge | Risk I top-right |
| Production use | Confirms atmospheric decal density target; ritual circle = Item 3 decal candidate |

### Image 3 -- Planning UI / Class Arsenal (ChatGPT mockup)

| Attribute | Value |
|---|---|
| Type | Full-screen UI reference |
| Class sidebar | 10 classes listed (Revenant, Void Knight, Riftlancer, Blade Harrow, Soul Hunter, Boneweaver, Rift Seer, Gravebinder, Chainshaper, Exorcist) -- INSPIRATION ONLY, not RIMA canonical |
| Loadout | Build name + 3 loadout slots |
| Skill grid | 5 Active + 5 Passive + Ultimate + Keystone |
| Equipment | 10-slot grid |
| Mission planning | Right panel: Act + recommended level + objectives + modifiers |
| Inventory sections | Inventory + Consumables + Rift Tools + Resources |
| Production use | Phase M (Planning UI) design pattern reference. NOT immediate scope. |

### Image 4 -- Rift Sanctum boss combat (ChatGPT mockup)

| Attribute | Value |
|---|---|
| Boss | "KEMURLUK RIFTBORN HERALD" -- sprite ~256-384px |
| Combat moment | Combo Executed panel left ("RIFT PURGE Q->E->Q->R") |
| Damage numbers | Large color-coded: 4286/5032/2710/1685/2147/1838/2301 |
| Status labels | Frozen, Shattered, Chained, Combo |
| VFX | Chain projector cyan radiating, chain link FX |
| Risk badge | Risk III top-right |
| Production use | Phase L (Combat Showcase) visual target. Boss sprite gen + chain VFX + damage system. |

---

## Section 9 -- Production tracks (v3 revised)

Three tracks replace v2 two-track A/B prototype strategy.

### Track 1: Asset Production (ACTIVE -- Items 0-10)

Current 10-item sequential decision list. Hades-iso single track. See Section 5.

### Track 2: Combat Showcase (Phase L -- depends on Track 1)

Boss-scale feature sprint targeting Image 4 (Rift Sanctum) visual quality.

| Step | Task | Type | Estimate |
|---|---|---|---|
| L.1 | Boss sprite gen (KEMURLUK RIFTBORN HERALD, 256-384px, multi-phase) | PixelLab (1 batch) | 80-160 gen |
| L.2 | Boss arena scene polish (Phase I Throne room, lighting + atmosphere) | Codex | 1 day |
| L.3 | Damage number system (TextMeshPro Canvas + DOTween float, color-coded) | Codex | 1 day |
| L.4 | Chain projector VFX (URP Sprite Shader + Bezier curve, cyan radiating) | Codex | 2 days |
| L.5 | Status FX labels (Particle + Canvas combo: Frozen/Shattered/Chained/Combo) | Codex | 1 day |
| L.6 | Atmospheric lighting profile (URP 2D Light dual-tone + post-process) | Codex | 1 day |
| L.7 | Combat UI (boss HP top + skill bar + combo panel) | Codex | 2 days |
| L.8 | Sword arc + spell trail VFX (Trail Renderer + Sprite Anim) | Codex | 2 days |

**Total Phase L estimate:** ~2-3 weeks dev + 1 PixelLab batch.
**Dependency:** L.1 (boss sprite) blocks L.2 boss scene polish. L.3-L.8 are independent of boss sprite -- can parallel.

### Track 3: Planning UI (Phase M -- deferred)

Image 3 (Class Arsenal) is design reference. Full-screen planning UI: class selector, build loadout, mission planning panel, inventory. Separate sprint, post-Phase L. No scope or timeline set yet.

---

## Section 10 -- Tile seam quality strategy (v3 -- L9 LOCKED)

### create_tiles_pro style_images anchor

- First call generates the foundational granite tile set
- That output is immediately stored as the style_images reference for all subsequent tile calls
- Pass style_images on every subsequent tile batch to lock palette consistency
- If a batch produces palette deviation, re-run with style_images anchor before proceeding

### Wang16 corner transition (Karar #131 reactivated)

| Use case | Tool | Rule |
|---|---|---|
| Standard floor tile set | create_tiles_pro Mod B | No Wang needed; pure material tiles |
| Material transitions (granite <-> rubble, walkway <-> rift) | Wang16 corner tiles | 1 transition test batch before production |
| Painter placement | Transform.y = 0.5 cell-fit (memory: pixellab-production-knowledge S95 lock) | LOCKED |

**Transition test sequence:**
1. Complete at least 1 floor tile call (Item 1)
2. Dispatch 1 Wang16 corner transition test batch (granite <-> rubble boundary)
3. Review seam in Unity painter scene
4. PASS: dispatch remaining transition batches. REWORK: adjust corner tile prompts.

### Aseprite seam polish

Manual Aseprite pass if tile seam review reveals 1-2 pixel mismatches. Not expected to be systematic -- PixelLab segmentation_outline mode handles most cases. Reserve as fallback.

---

## Section 11 -- Open risks + flags

| Risk | Severity | Mitigation |
|---|---|---|
| PixelLab "high top-down" actual output quality does not match ChatGPT mockup detail level | HIGH | Item 0 prototype mandatory. ChatGPT mockups are AI-generated targets -- PixelLab may differ in detail. If quality gap is large, adjust prompt or reduce scene complexity expectations. |
| Item 0 front-face rendering incorrect (flat blobs vs volume depth) | HIGH | Item 0 mandatory quality gate. REWORK loop until front-face reads correctly before proceeding. |
| Cross-act batch palette bleed | MEDIUM | 1 mixed batch test (Item 3 first call) before committing cross-act fill strategy |
| Act 1 hero list vs layout mismatch | LOW | layout_v1.json walls[] + props[] are source of truth |
| Pickup / key shard / relic / gate socket gap | MEDIUM | Not in current taxonomy; add to Item 3 slot list or separate 32px batch |
| Hazard / dash-only telegraph missing | MEDIUM | Not in current taxonomy; add as Item 3 overflow or new item |
| Outpaint stitch seam quality (Inpaint v3) | LOW | HYBRID policy limits to 2-3 hero assets. Aseprite palette-snap mandatory. Community-validated for pixel art. |
| Hades-iso wall height causes painter z-order issues | MEDIUM | Test in Unity painter after Item 0; 64x96/64x128 sprite pivot must be at tile base |
| Wang16 corner tile batch style consistency vs floor anchor | MEDIUM | Pass style_images from floor anchor call into Wang16 batch |
| Phase L VFX scope creep | MEDIUM | Phase L steps L.3-L.8 are independent Codex tasks; track separately from Track 1 asset production |

---

## Section 12 -- Execution order summary (post-LOCK v3)

Sequential steps after v3 file is accepted:

1. **User onay** for Item 0 dispatch -- Hades-iso wall prototype, ~40-80 gen
2. **Item 0 prototype review** -- seam quality + front-face check vs Image 1/2 reference; PASS or REWORK loop
3. **PASS** -> orchestrator presents Items 1 / 3 / 4 / 5 batch composition choices one-by-one; user picks options
4. **Item 1 locked** -> floor tiles dispatched; first output becomes style_images anchor
5. **Item 6 locked** -> full wall library dispatch (8 calls, ~320 gen)
6. **Items 3/4/5** -> medium + small + hero prop batches dispatch (pending user option choices)
7. **Wang16 transition test batch** -> 1 call; seam review; PASS/REWORK
8. **Parallel (Track 2, Phase L):** Codex tasks L.3-L.8 start immediately (independent of Track 1 completion). L.1 boss sprite gen scheduled with user approval.
9. **Vertical slice REWORK fix** -- Phase K open bugs resolved with new assets + VFX in place
10. **Phase K verdict re-run** -- playtest with new assets + Track 2 VFX/UI combined; evaluate slice quality

---

## Confirmation summary (v3)

| Metric | Count | Detail |
|---|---|---|
| LOCK count | 9 | L1 (pivot), L2 (weapon decouple), L3 (no night gen), L4 (hibrit C), L5 (tile modular), L6 (wall pipeline), L7 (tile size), L8 (painter), L9 (tile seam) |
| New locks vs v2 | +2 | L1 resolved PENDING_LOCK -> LOCKED; L9 added (tile seam strategy) |
| PENDING_DECISION items | 6 | Items 1, 3, 4, 5, 7, 8 |
| PENDING_LOCK items | 3 | Items 0, 6, 10 |
| RESOLVED items | 1 | Item 2 (merged into Item 0) |
| New sections vs v2 | 3 | Section 8 (expanded -- 4 images annotated); Section 9 (replaced -- 3 tracks); Section 12 (execution order) |
| Significantly revised vs v2 | 5 | Section 1 (L1 locked, L9 added); Section 2 (single track); Section 5.5 (outpaint resolved); Section 10 (tile seam strategy); Section 11 (new risks) |
| Pure top-down track | REVOKED | All Track A references removed from active sections |
| v1, v2 files | RETAINED | Do not delete; historical context |

---

## Section 9 (roster supplement) -- Character roster reconciliation

**ChatGPT mockup class names** (from Image 3 sidebar): Revenant, Void Knight, Riftlancer, Blade Harrow, Soul Hunter, Boneweaver, Rift Seer, Gravebinder, Chainshaper, Exorcist -- **INSPIRATION ONLY for visual atmosphere and Rift theme. Do not adopt names.**

**RIMA canonical roster** (from `project_canonical_character_roster_v2.md`, LOCKED):
Warblade, Ronin, Gunslinger, Ranger, Elementalist, Shadowblade, Ravager, Hexer, Brawler, Summoner

No rename. ChatGPT mockup informs Rift visual atmosphere (heavy Rift theme) which already aligns with Act 1 Shattered Keep lore lock. Class identity and naming remain canonical.
