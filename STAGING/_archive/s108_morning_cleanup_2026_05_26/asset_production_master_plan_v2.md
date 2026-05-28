# Asset Production Master Plan v2

**File:** `STAGING/asset_production_master_plan_v2.md`
**Last updated:** 2026-05-22
**Supersedes:** `STAGING/asset_production_master_plan_v1.md` (v1 retained as historical context -- do NOT delete)
**Owner:** Orchestrator reads this before any asset dispatch. Do NOT modify decisions; update Status column only.

## Change log (v1 -> v2)

| Change | Detail |
|---|---|
| L1 REVERTED from LOCKED to PENDING_LOCK | User introduced Track B (Hades-iso ~70-75 deg) as preferred direction; prototype-first before final pivot lock |
| Section 2 expanded | Two-track parameter table: Track A pure top-down vs Track B Hades-iso; key insight on native `high top-down` angle |
| Section 4 expanded | Wall render difference per track: Track A 16-32px height, Track B 64-96px height |
| Section 5 restructured | New Item 0 (pivot prototype) gates all others; Item 2 merged into Item 0 |
| Section 5.5 added | Outpainting / inpaint stitching research flag |
| Section 8 repurposed | Image #3 reference attribute list for production targeting (was open risks -- risks moved to Section 11) |
| Section 9 added | Two-track production strategy side-by-side |
| Section 10 note added | Hades-iso tall canvas (64x96 / 64x128) batch slot impact |
| Section 11 added | Open risks (moved from old Section 8) |

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

**v2 critical note:** Item 0 (pivot prototype) is the gate. Nothing dispatches until Item 0 delivers comparison screenshots and user locks Track A or Track B. User explicit preference is Track B (Hades-iso) if technically viable.

---

## Section 1 -- Locked decisions (do not re-open unless noted)

| # | Decision | Detail | Status | Source |
|---|---|---|---|---|
| L1 | Pivot -- PROTOTYPE FIRST | Track A: pure top-down ~85-90 deg (ChatGPT_TOPDOWN ref) vs Track B: Hades-iso ~70-75 deg (Image #3, Hades / Children of Morta match) -- PENDING_LOCK pending prototype result; was LOCKED pure top-down in v1, REVERTED | PENDING_LOCK | User direction 2026-05-22; v2 revert |
| L2 | Weapon decouple arch (Karar #123/#144/#146) | Body weaponless + Child SR + HandAnchor; 12 weapon sprites incl. Elementalist orb | LOCKED | STAGING/weapon_pipeline_v1.md, STAGING/weapon_web_prompts_v1.md |
| L3 | No PixelLab gen during autonomous night runs | create_tiles_pro / create_object / animate_* blocked; get_* / list_* read-only only | LOCKED | feedback_no_pixellab_night_autonomous.md |
| L4 | Hibrit variant strategy (C) | Act 2-4 anchor hero sprites = user manual Edit Image Pro; small props + decals = orchestrator state pipeline autonomous | LOCKED | 2026-05-22 user direction |
| L5 | Tile pipeline: Modular (Mod B) | create_tiles_pro Mod B + manual style_images transition; Wang autotile deprecated for RIMA | LOCKED | project_modular_pipeline_lock.md |
| L6 | Wall + decoration pipeline | create_object n_frames=16 GameObject placement (NOT tile); canvas height TBD by Track lock | LOCKED | project_modular_pipeline_lock.md |
| L7 | Tile size | 64x64 px production (matches char 64 PPU = 1 cell) | LOCKED | S98 baseline |
| L8 | Painter default | RimaWorldPainterWindow PaintMode.TopDown primary | LOCKED | S98 |

**Note on L1 memory file:** `project_topdown_pivot_lock.md` will require revision once Item 0 prototype comparison locks the final track. Do NOT revise that memory file until the lock arrives -- orchestrator handles post-prototype.

---

## Section 2 -- PixelLab tool parameter LOCK status

### Tile parameters (floor -- both tracks identical)

| Tool | Parameter | Track A: Pure top-down | Track B: Hades-iso | Status |
|---|---|---|---|---|
| create_tiles_pro | tile_type | square_topdown | square_topdown | LOCKED |
| create_tiles_pro | tile_view | top-down (PURE) | top-down (PURE) | LOCKED |
| create_tiles_pro | tile_view_angle | 90 | 90 | LOCKED (floor stays flat both tracks) |
| create_tiles_pro | tile_depth_ratio | 0 | 0 | LOCKED |
| create_tiles_pro | outline_mode | segmentation | segmentation | LOCKED |

### Object parameters (walls + props -- differ by track)

| Tool | Parameter | Track A: Pure top-down | Track B: Hades-iso | Status |
|---|---|---|---|---|
| create_object | view | "high top-down" + prompt force "PURE 90 DEGREE OVERHEAD VIEW, no perspective tilt" | "high top-down" (native Hades-iso match -- NO workaround needed) | PENDING_TEST (resolved by Item 0) |
| create_object | directions | 1 | 1 | LOCKED (static prop) |
| create_object | n_frames | rule-locked by canvas size | rule-locked by canvas size | LOCKED |
| create_object | object_view | top-down | top-down | LOCKED |

**Key insight (v2):** PixelLab `view: "high top-down"` produces ~30-35 deg tilt which IS the native Hades-iso camera angle. Track B requires NO prompt workaround. Track A requires aggressive prompt-force to suppress front-face rendering. This makes Track B the technically simpler PixelLab path.

### n_frames rule (canvas size -> batch capacity)

| Canvas size (px) | n_frames | Items per call | Track B wall canvas note |
|---|---|---|---|
| 32x32 | 64 | 64 | Decals only |
| 48-80 | 16 | 16 | Small props |
| 88-168 | 4 | 4 | Large props; Track B walls may need 64x96/64x128 -- fits this tier |

---

## Section 3 -- Pricing reference (PixelLab v2 API)

| Tool | Gen cost / call | Output per call | Notes |
|---|---|---|---|
| create_object 32px | 20 gen | 64 items | n_frames=64 |
| create_object 48-80px | 30 gen | 16 items | n_frames=16 |
| create_object 88-168px | 40 gen | 4 items | n_frames=4 |
| create_object_state | 20-40 gen | 1 variant of existing | Act 2-4 small variants (autonomous) |
| create_tiles_pro | 25 gen | 16 tiles (4x4) | per call, 64px tile |
| Edit Image Pro (USER MANUAL, NOT MCP) | 20/25/40 by output size | 1 sprite | up to 256/314/512 px; Act 2-4 hero anchors |
| Inpaint v3 (USER MANUAL, NOT MCP) | 20 gen flat | 1 masked area | repairs / color shift; see Section 5.5 for outpainting note |
| create_character | out of scope (user web UI only) | -- | separate phase |
| animate_* | out of scope (separate phase) | -- | after all statics done |

**Budget as of 2026-05-22 S98:** 2265 / 5000 gen remaining.

---

## Section 4 -- Asset taxonomy + canonical Act 1 needs

### Act 1 Shattered Keep -- ~110 asset target

| Layer | Type | Count | Notes |
|---|---|---|---|
| L1 Floor Base | Cool Granite 16-tile set | 16 | create_tiles_pro Mod B; same both tracks |
| L2 Floor Variation | Worn Stone Path 16-tile set | 16 | create_tiles_pro Mod B; same both tracks |
| L3 Wall Overlay | 8 class x 3 var = 24 sprite | 24 | create_object; height + canvas differ by track (see below) |
| L4 Large Patches | 3 materials x 8 var = 24 sprite | 24 | Cave Moss + Dust Drift + Cracked Rubble |
| L5 Scatter | ~18 sprite | 18 | small stones, chain remnants, bone fragments |
| L6 Accent/Hero | ~12 sprite | 12 | cyan rift, brazier, banner |

### Wall canvas height by track

| Track | Wall render | Sprite height | Prefab canvas | Front-face visible |
|---|---|---|---|---|
| Track A Pure top-down | Top-profile only, minimal depth | 16-32 px | 64x64 | No |
| Track B Hades-iso | Front-face + top-profile both visible, dramatic depth | 64-96 px | 64x96 or 64x128 | Yes -- required for Hades-iso reading |

**Impact:** Track B wall sprites are taller -- fit the 88-168px n_frames=4 batch tier. Track A walls can fit 48-80px tier. Total batch count and gen cost differs by ~40-60 gen for full wall library. Track B wall class count may be reduced (8 classes -> 6) to offset taller-canvas cost.

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
- **Item 0 gates everything.** Do not dispatch Items 1-10 until Item 0 prototype comparison is reviewed and Track A or B locked.

---

### Item 0 -- Pivot prototype test (NEW -- GATES ALL OTHERS)

**Decision:** Lock Track A (pure top-down) or Track B (Hades-iso) as production camera angle.

**Action required:**
1. Dispatch Track A: 1-2 wall sprites (e.g., `straight_wall_n`, `corner_wall_nw`) with `view: "high top-down"` + prompt "PURE 90 DEGREE OVERHEAD VIEW"
2. Dispatch Track B: 1-2 wall sprites (same names) with `view: "high top-down"` (native angle, no workaround)
3. Place both sets in Unity scene (same room layout)
4. Screenshot both for user comparison
5. User confirms Track A or Track B -- this becomes the new L1 lock

**Gen estimate:** 80-160 gen (2 batches of 1-2 wall calls each; 128px canvas = 40 gen per call)
**Status:** PENDING_LOCK (user confirmed prototype-first approach; awaiting dispatch confirmation)
**Depends on:** none (first action)
**Source:** User direction 2026-05-22 -- "prototype-first (C option)"; Image #3 reference; Section 9

---

### Item 1 -- Floor batch structure

**Decision:** How to split create_tiles_pro calls for Act 1 floor tiles.

| Option | Description | Gen cost | Tiles |
|---|---|---|---|
| i | 1 call: 4-tile mix (granite + rubble + walkway + rift), Act 1 only | 25 gen | 16 mixed |
| ii | 2 calls: Granite 16-var + Path 16-var separately | 50 gen | 32 pure-material |
| iii | 1 call: 4-tile mix -- 2 Act 1 materials + 2 Act 2 materials in same batch | 25 gen | 16 (cross-act risk -- see Section 6 WARNING) |

**Note (v2):** Floor tile parameters are identical for both tracks (tile_view_angle=90, tile_depth_ratio=0). Item 1 does not hard-block on Item 0; however, chosen wall track affects how floor tiles read visually in the scene -- confirm Item 0 first for visual consistency check before final floor batch lock.

**Status:** PENDING_DECISION
**Gen estimate:** 25-50 gen depending on option
**Depends on:** Item 0 preferred (visual match to wall track matters)
**Source:** Modular Mod B pipeline lock; user "mantikli doldur" direction

---

### Item 2 -- create_object view parameter test

**MERGED INTO ITEM 0 (v2 change).** The pivot prototype (Item 0) uses both Track A and Track B wall calls -- this directly tests and resolves the `view` parameter question for both tracks simultaneously. Item 2 is retired as a standalone item; its outcome is delivered by Item 0.

**Status:** RESOLVED BY ITEM 0
**Gen estimate:** 0 (covered by Item 0 budget)

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
**Depends on:** Item 0 (track lock), Item 7 (decal narrowing)
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

**Note:** If Act 1 needs exceed 16 slots, dispatch 2 batches. Fill extra slots in batch 2 with Act 2-4 medium props.

**Status:** PENDING_DECISION
**Gen estimate:** 30 gen per call; likely 1-2 calls
**Depends on:** Item 0 (track lock gates view parameter for all create_object calls)
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

**Note (v2 -- outpainting flag):** sarcophagus and throne_dais are candidates for outpaint/stitch workflow if research (Section 5.5) returns GO verdict. Large hero assets that exceed 256px native cap may use outpaint. Final call pending Section 5.5 research resolution.

**Remaining slots in each batch for Act 2-4 fill:** If a batch is under 4 Act 1 items, user confirms which Act 2-4 hero counterpart fills the gap.

**Status:** PENDING_DECISION
**Gen estimate:** 40 gen per call; 3 calls = 120 gen
**Depends on:** Item 0 (track lock), Section 5.5 (outpaint policy for hero props)
**Source:** act1_shattered_keep_layout_v1.json walls[] and props[]

---

### Item 6 -- Wall production batch (FINAL LOCK post Item 0)

**v2 note:** In v1 this was "Wall geometry decision." That decision is now RESOLVED by Item 0 prototype comparison. After Item 0 locks a track, Item 6 becomes "Final wall production batch dispatch."

**Pending lock content:**
- If Track A locked: wall class 8 x 3 var, canvas 64x64, 48-80px tier, ~180 gen total
- If Track B locked: wall class 6-8 x 3 var, canvas 64x96 or 64x128, 88-168px tier, ~200-240 gen total; front-face mandatory

**Status:** PENDING_LOCK (awaits Item 0 outcome)
**Gen estimate:** 180-240 gen (full wall library)
**Depends on:** Item 0
**Source:** User prototype-first direction; wall library Phase B roadmap; Section 9 track strategy

---

### Item 7 -- Decal categorical narrowing

**Decision:** Which decal categories to keep after floor tile mix covers Rubble + Rift materials.

**Proposed keep list:** moss_patch, blood_smear, bone_scatter_decal, footprint_set, scorch_mark, summon_circle_faint, dust_drift_decal
**Proposed drop (covered by tile):** crack_decal (covered by cracked rubble tile), cyan_accent_decal (covered by rift tile)

**Note (v2 -- Image #3 reference):** Track B (Hades-iso) visual target includes blood/debris/rift-crack decals as atmospheric layer. If Track B is locked, retain blood_smear and rift_crack_hairline even if floor tile partially covers. Atmospheric density is a design target per Image #3.

User must confirm keep/drop list before any decal gen.

**Status:** PENDING_DECISION
**Gen estimate:** 0 gen (decision only; affects Item 3 slot allocation)
**Depends on:** Item 0 (track affects decal density target), Item 1 (to know which materials are in floor tile)
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

**Proposed order (v2 -- updated for Item 0 gate):**

1. Item 0 (pivot prototype) -- Track A vs B comparison; gates everything
2. Item 1 (floor tiles) -- foundation; painter cannot function without floor material
3. Item 6 (wall production batch) -- geometry confirmed by Item 0; one full dispatch
4. Item 4 (medium props: torch/banner) -- highest visual impact per gen spent
5. Item 3 (32px scatter/decal) -- fill density; lower priority than structure
6. Item 5 (hero props) -- large canvas, expensive; after structure confirmed
7. Item 8 (user manual Act 2-4 heroes) -- user-paced, off-band
8. Item 9 (state pipeline Act 2-4 variants) -- after Act 1 complete

User confirms or reorders.

**Status:** PENDING_LOCK
**Gen estimate:** N/A (ordering decision, no gen cost)
**Depends on:** Items 0-9
**Source:** Vertical slice first principle; "oynanabilir plan" user direction

---

## Section 5.5 -- Outpainting / inpaint stitching policy (NEW)

**Flag raised (2026-05-22 user):** "buyuk objeleri inpaint ile birlestirebiliriz... outpainting / stitching strategy"

**Context:** PixelLab create_object native cap is ~256px output size. Hero props like sarcophagus (~300-400px in game world) and large archways may benefit from outpaint/stitch workflow: generate core sprite, then extend canvas via inpaint rounds.

**Research file pending:** `STAGING/research_outpainting_inpaint_stitching.md` (not yet written at v2 time -- orchestrator dispatches)

**Pipeline policy: PENDING_RESEARCH**

| Verdict | Action |
|---|---|
| GO | Large hero assets (sarcophagus, throne_dais, archway) use outpaint workflow; add stitch step to Item 5 dispatch |
| HYBRID | Outpaint only for assets > 256px native; standard flow for rest |
| NO-GO | Stick to native 256px create_object cap; accept size constraint |

**Note:** Inpaint v3 cost is 20 gen flat per masked area. Each stitch round = 1 Inpaint v3 call. A 400px sprite from 256px base = approximately 2 stitch rounds = 40 additional gen per hero asset. Budget impact: manageable if limited to 3-4 hero props.

---

## Section 6 -- Batch fill rule (no waste)

Every batch must be filled to capacity. Do not ship a half-full batch.

| Batch type | Capacity | Act 1 fill | Remaining for Act 2-4 |
|---|---|---|---|
| 32px n_frames=64 | 64 | ~50 Act 1 small items | ~14 slots Act 2-4 small props |
| 48-80px n_frames=16 | 16 per call | 8 Act 1 medium items | 8 slots Act 2-4 medium props |
| 88-168px n_frames=4 | 4 per call | 3-4 Act 1 hero items | 0-1 slots per batch |
| create_tiles_pro | 16 tiles per call | depends on option chosen in Item 1 | N/A (tile, not object) |

**Track B wall canvas note (v2):** Hades-iso wall sprites at 64x96 or 64x128 fit the 88-168px tier (n_frames=4). This changes wall batch count from Track A (48-80px tier, n_frames=16, 1 call per ~3 wall classes) to Track B (88-168px tier, n_frames=4, 1 call per wall class). Total wall calls: Track A ~3-4 calls, Track B ~6-8 calls. Gen delta: ~80-120 gen more for Track B wall library.

**Fill priority:** Act 1 items first. If capacity remains, Act 2-4 items of same canvas size that share biome-adjacent palette (cold stone, moss, wood, metal).

**WARNING -- style/palette consistency risk:** Mixing Act 1 + Act 2-4 assets in one batch risks PixelLab defaulting to a shared palette that washes out individual biome identity. Mitigation: test with 1 mixed batch (Item 3 first call) before committing to cross-act fill strategy. If palette bleeds, switch to same-act-only batches and accept partial fill waste.

---

## Section 7 -- Total gen budget tracking

Updated as items lock and dispatch.

| Item | Description | Gen | Cumulative spent |
|---|---|---|---|
| (all pending) | -- | -- | 0 |

**Budget remaining:** 2265 / 5000 gen as of 2026-05-22 S98.

**Projected spend when all Act 1 items locked (estimate, Track B preferred):**

| Phase | Calls | Gen est. (Track A) | Gen est. (Track B) |
|---|---|---|---|
| Item 0: Pivot prototype | 4 | 80-160 | 80-160 |
| Floor tiles (4 Mod B calls) | 4 | 100 | 100 |
| Wall prototype (covered by Item 0) | 0 | 0 | 0 |
| Wall library full (Track A ~3-4 calls 48-80px) | 3-4 | 90-120 | -- |
| Wall library full (Track B ~6-8 calls 88-168px) | 6-8 | -- | 240-320 |
| Medium props (48-80px, 2 calls) | 2 | 60 | 60 |
| Small props/decals (32px, 1-2 calls) | 2 | 40-60 | 40-60 |
| Hero props (88-168px, 3 calls) | 3 | 120 | 120 |
| Outpaint stitch rounds (if GO, 3 hero props) | 6 | 0 | ~60 |
| **Act 1 total estimate** | | **~490-520 gen** | **~700-880 gen** |
| Reserve after Act 1 | | ~1745 | ~1385 |
| Act 2-4 state variants (autonomous) | TBD | ~300-500 | ~300-500 |

**Track B costs ~200-360 gen more than Track A for Act 1. Budget remains comfortable for either track. Animate phase separate.**

---

## Section 8 -- Image #3 reference attributes (production targeting)

**Reference:** Darkest Dungeon x Hades hybrid shared by user 2026-05-22. This defines Track B visual target.

| Attribute | Value | Production impact |
|---|---|---|
| Camera tilt | ~70-75 deg (NOT pure 90) | Track B lock; PixelLab "high top-down" native match |
| Wall height | ~80-128px with visible front-face | 64x96 / 64x128 canvas; 88-168px batch tier |
| Floor | Cracked granite with blood, debris, rift-crack decals layered | Decal density target -- retain blood_smear + rift_crack in Item 7 narrowing |
| Lighting | Dual-tone: warm torch orange + cold rift cyan | Prompt: "warm torchlight from left, cold cyan rift glow, atmospheric shadow" |
| Enemies | 3D-ish chibi pixel sprites with HP bars overhead | create_character compatible; separate phase |
| VFX | Sword arc trail, gold pickup floating text | Separate VFX phase; not in this asset plan scope |
| UI | Corner HP/MP bars, mini-map + objective panel, skill bar 1/2/3/4/Q/R | UI phase; separate from world asset plan |
| Style reference | Children of Morta for wall front-face depth; Hades for lighting atmosphere | Inject both as style reference in PixelLab prompts |

**Prompt injection template for Track B (object calls):**
"pixel art dungeon wall, front face visible, Hades-style isometric view, warm torch orange light from one side, cold cyan rift glow accent, cracked stone texture, dark atmosphere, [asset name]"

---

## Section 9 -- Two-track production strategy (NEW)

| Dimension | Track A: Pure Top-down | Track B: Hades-iso |
|---|---|---|
| Camera angle | ~85-90 deg ortho | ~70-75 deg tilt |
| PixelLab view param | "high top-down" + prompt force | "high top-down" (native) |
| Wall render | Top-profile only, minimal height | Front-face + top-profile, tall sprite |
| Wall canvas | 64x64 (48-80px batch tier) | 64x96 or 64x128 (88-168px batch tier) |
| Wall call count for full library | ~3-4 calls | ~6-8 calls |
| Gen cost premium vs Track A | baseline | +200-360 gen for Act 1 |
| Visual impact | Minimalist dungeon, clean grid reading | Dramatic depth, atmospheric, high art bar |
| Production complexity | Lower -- simpler sprites, fewer wall classes | Higher -- tall sprites, front-face render discipline |
| Act 2-4 biome shift | Straightforward recolor | Front-face must shift by biome; more variant work |
| User preference | Fallback | PREFERRED ("Hades yapilabilirse boyle yapalim") |
| Verdict | Fallback if Track B prototype fails | Primary target -- proceed unless prototype shows fatal issue |

**Decision rule:** Item 0 prototype is go/no-go for Track B. If Track B prototype output reads correctly in Unity scene (walls read as depth volumes, not flat blobs), lock Track B and proceed. If front-face renders incorrectly or style is incompatible, fall back to Track A and lock. User makes final call from screenshot comparison.

---

## Section 10 -- Batch fill rule note: tall canvas impact

**v2 addition:** Hades-iso (Track B) wall sprites use 64x96 or 64x128 canvas -- taller than standard 64x64. This affects batch slot assignment:

- Track A walls: fit 48-80px tier (n_frames=16, 16 walls per call -- high batch efficiency)
- Track B walls: fit 88-168px tier (n_frames=4, 4 walls per call -- lower batch efficiency)
- Impact: Track B wall library requires ~2x more calls for same wall class count
- Mitigation option: reduce wall class count from 8 to 6 for Track B to keep total call count similar to Track A

**Batch fill reminder for Track B wall calls:** 4 slots per call. Fill with 4 different wall classes (straight, corner_convex, corner_concave, wall_end) per call. Do not mix with props -- wall and prop batches separate.

---

## Section 11 -- Open risks + flags

| Risk | Severity | Mitigation |
|---|---|---|
| Item 0 Track B prototype may not render front-face correctly in PixelLab | HIGH | Item 0 mandatory comparison test; Track A fallback ready |
| create_object view param unvalidated (both tracks) | HIGH | Resolved by Item 0 -- do not bypass |
| Cross-act batch palette bleed | MEDIUM | 1 mixed batch test before committing cross-act fill |
| Act 1 hero list vs layout mismatch | LOW | layout_v1.json walls[] + props[] are source of truth |
| Pickup / key shard / gate socket gap | MEDIUM | Codex review flagged; add to Item 3 slot list or separate 32px batch |
| Hazard / dash telegraph missing | MEDIUM | Not in current taxonomy; add as Item 3 overflow or new item |
| Outpaint stitch pipeline unvalidated | MEDIUM | Section 5.5 research pending; do not use until GO verdict |
| Track B wall height causes painter z-order issues | MEDIUM | Test in Unity painter after Item 0; 64x96/64x128 sprite pivot must be at tile base |
| L1 memory file (project_topdown_pivot_lock.md) is stale until Item 0 resolves | LOW | Do not reference that memory file as LOCKED until post-Item 0 update |
