# TILE vs CHARACTER ANGLE — Architectural Verdict (Opus, S93)

Generated 2026-05-19 for morning review. Honest verdict, no padding.

---

# VERDICT

**Branch D + Branch E hybrid, weighted toward D.**

Keep current PURE top-down (90°) PixelLab tile output as the **base floor layer**, but commit fully to the Hades model: the floor is structurally a flat 2D plane and **the eye is intentionally NOT drawn there**. Visual depth comes from L3 walls (35° rendered), L4 large organic patches (soft-alpha painted with implied depth), L5 scatter props (35° angled), and L6 rift accents. Floor sits at the bottom of the visual hierarchy as low-contrast, low-saturation, low-detail texture. Additionally apply a small camera tilt (Branch E, ~8-12° on Camera transform, NOT 35°) so that the floor reads as receding rather than as a wall-facing grid. This is cheap (single Camera component edit), reversible, and untested but standard practice in Unity top-down workflows.

**Why this over A (regen all tiles at 35°):** Branch A is a trap. PixelLab's `low top-down` view does not actually give you a coherent angled tile — it gives you a tile that *implies* an angle, which then *fights* the character's true 35° projection because the tile angle is baked at a different focal point than the character. You would burn 50-100 gens to discover the mismatch is not gone, just repainted in a new color. Children of Morta hit exactly this wall and the fix was to abandon the grid (their words: "too flat... adjustable connections that can be even less than one tile"), which is precisely what the LOCKED Paint-Brush Architecture already does. The angle mismatch is not the structural problem the user thinks it is — it is **a hierarchy and contrast problem dressed as an angle problem**. Branches D + E fix it without touching Karar #100 or the 10 locked anchors.

---

# 1. Hades art pipeline reverse-engineering

## Evidence collected

From MCV/Develop interview with Jen Zee + Game Developer article on Hades characters + Steam/community references:

- Hades' camera is described variously as "isometric" or "~30° angled top-down." It is closer to 30° than to true 30/60° isometric — it is the same family RIMA is in.
- Environments: "1,400 environment textures" shipped. Photoshop + Maya. **Environments are hand-painted 2D artwork** (gamedeveloper.com article). Some 3D animated effects layered in.
- Characters: 3D sculpts in ZBrush → renders → hand-painted black line work on top to match 2D look.
- **No source describes Hades floors as flat top-down tiles painted square.** No source describes them as a tile grid at all. The pipeline is: artist paints a **room region in Photoshop with the 30° projection baked in by hand**, then it ships as a near-monolithic background image (with separate light/shadow passes and animated effect overlays).

## Inference (high confidence)

Hades' "floor" is not a tile. It is a hand-painted bitmap region authored at the target camera angle. The room is monolithic-ish, not tile-composed. Detail is concentrated at the boundary (walls, props, edges of the painted patch) where the angle reads. The "interior" of the floor is mostly low-contrast wash — exactly because a flat-feeling interior reads fine when the walls and props carry the depth.

## Implication for RIMA

This matches the **Multi-Layer Painter v1 LOCK** (memory: `project_multilayer_painter_v1_lock.md`) and **Map Plan v1 LOCK** ("Hibrit monolithic painted background + gameplay overlay") already in MEMORY. The user already locked the Hades model at S91. The current crisis is that the L1/L2 floor layer is being asked to *carry* the perspective, which is the opposite of what Hades does. Hades' floor doesn't carry perspective — walls and props do.

---

# 2. Comparative analysis — other angled-top-down games

| Game | Camera | Character | Floor | Match? | Lesson |
|---|---|---|---|---|---|
| **Hades** | ~30° isometric-leaning | Hand-painted ink-over-3D, 30° | Hand-painted monolithic 2D regions, perspective baked by artist | YES (both authored to angle) | Don't tile the floor. Paint it. Depth lives in walls/props. |
| **Children of Morta** | Isometric | Pixel art, isometric | Started as tile grid, **abandoned** for "adjustable connections < 1 tile" — i.e. overlapping painted pieces | YES (after pivot) | Grid floors look flat. They explicitly broke grid. Confirms RIMA's paint-brush LOCK. |
| **Death's Door** | Fixed isometric | 3D low-poly | 3D low-poly diorama | YES (full 3D) | Not applicable — they sidestepped 2D entirely. |
| **Hyper Light Drifter** | **Pure top-down** | Top-down sprite | Flat colors + gradient overlays | YES (both flat) | This is one of two valid solutions: commit to NO angle. RIMA rejected this at S86. |
| **Stardew Valley** | Orthographic | Front-facing sprite | Flat top-down tile | YES (both flat ortho) | Same — commit to one projection. |
| **Wizard of Legend** | 3/4 top-down (~45-60°) | 3/4 sprite | 3/4 painted tile (NOT pure 90°) | YES (both 3/4) | Confirms: tile MUST share the character's projection. |
| **Crawl (Powerhoof)** | Top-down with slight angle | Top-down sprite | Top-down tile | YES (both flat) | Same pattern. |

## Universal rule extracted

**In every shipped 2D angled-camera game with strong visual cohesion, the floor and the character are authored at the same projection — OR the floor is so visually de-emphasized that its projection becomes invisible.**

RIMA today violates rule 1 (different projections) AND violates rule 2 (the L1/L2 floor is being asked to carry visual weight via tile variety, tile color, tile pattern). Either fix makes the problem go away. Doing both is the Hades approach.

## Critical secondary insight

PixelLab cannot author a true 35° angled top-down tile. The `low top-down` view setting in PixelLab gives you a sprite-frame projection (good for objects with implied front faces like crates, walls, characters), NOT a ground-plane projection. A ground plane at 35° is a **trapezoid in screen space**, not a square. PixelLab will always paint a square texture and call it angled. This is why 1200+ gens haven't fixed it — the tool is structurally incapable of producing what the user is asking for. **Branch A is dead on arrival.**

---

# 3. RIMA branches re-evaluated

## A — Regen all tiles at "low top-down" 35° angle

- **Cost:** 50-100 gen + a week of integration + 10 Wang tilesets to re-author + scrap of v15c painted top-down pool.
- **Fidelity gain:** Near zero. See "Critical secondary insight" above. PixelLab can't paint a true ground-plane trapezoid. You will get tiles that *imply* angle by adding pseudo-shading, which the eye reads as "tile with shadow" not "tile receding into screen." The mismatch will reappear in a different guise.
- **Risk:** Burns the 5000-gen budget reserve (Memory: `project_5000_pixellab_allocation_lock.md`) on a structural dead-end. Reproduces the Children of Morta "still too flat" problem.
- **Verdict:** **REJECT.** Highest-cost lowest-payoff branch.

## B — Regen all character anchors at 90° pure top-down

- **Cost:** YASAK (Karar #100 LOCK, 5000+ gen, 10 anchors scrapped, 8-dir mirror strategy invalidated, weeks of rework).
- **Fidelity gain:** Would solve the mismatch by removing the angle entirely. Becomes Hyper Light Drifter or Stardew.
- **Risk:** Loses Hades-style visual identity (LOCKED at S91 as Map Plan v1) AND violates 4+ MASTER_KARAR items. User has been explicit this is non-negotiable.
- **Verdict:** **REJECT — out of scope.** Listed only for completeness.

## C — Subtle perspective overlay on flat tiles (shadow gradient, fake depth)

- **Cost:** 1-2 days shader work. Stackable on top of current floor.
- **Fidelity gain:** Modest. A vignette/gradient/fake-AO pass darkens the floor edges and brightens near the camera, giving an implied light direction. Hyper Light Drifter does exactly this and it works for them — but they don't have an angled character to fight.
- **Risk:** A gradient overlay on a 90° tile floor under a 35° character does not reconcile the angle; it only de-emphasizes the floor. That de-emphasis IS the win, but you can achieve the same thing cheaper by lowering floor contrast/saturation in the painted tiles themselves.
- **Verdict:** **WEAK YES, SECONDARY.** Worth adding as L6.5 (vignette pass) but not load-bearing. Bundle with D.

## D — Hades model: flat tiles + 3D-look walls/props + de-emphasized floor

- **Cost:** Mostly LIVE already. L3 wall sprites (35° from Hades reference, memory: `project_alabaster_dawn_pipeline_lock.md`) exist. L4 large organic patches LIVE. L5 scatter LIVE. What's missing: deliberate **contrast/saturation reduction on L1/L2 floor** so the eye stops trying to read the floor's angle.
- **Fidelity gain:** Large. This is what the user already locked at S91. The problem is execution drift — recent gens have produced high-contrast, high-detail, high-saturation floors that *invite* the eye to inspect the projection. Fix: paint floors as low-contrast washes (Hades does this — interiors are muted, edges are punchy).
- **Risk:** Requires aesthetic discipline (gen budget for floors must be capped LOW, not high) and possibly 1-2 floor regen passes to dial down contrast. Not architectural risk — execution risk.
- **Verdict:** **STRONG YES, PRIMARY.** Confirms existing LOCKs. The fix is to actually do what was already decided, with discipline.

## E — Camera tilt: Unity Camera angled, slight perspective

- **Cost:** Single component edit. Camera.transform.eulerAngles.x = 8-12°. Possibly switch to perspective camera (instead of ortho) with very narrow FOV. ~30 min to test.
- **Fidelity gain:** Surprisingly large for the cost. A small physical tilt makes flat-painted ground tiles read as receding because the projection math is doing it for you in hardware. The floor is *literally* angled away from camera. This is the secret of Octopath Traveler and many 2D-3D hybrids — tilt the camera, paint the assets flat-ish, let projection do the work. **However**, the character sprite (authored at 35°) is on a flat billboard quad; tilting the camera tilts the character too, which would skew the sprite. Fix: keep character sprite on a vertical billboard that auto-faces camera, OR use a sprite-specific shader that compensates. Standard Unity 2D top-down trick.
- **Fidelity gain (continued):** Even at 8°, this aligns the floor's apparent projection toward the character's baked 35°. The tiles don't need to BE 35°; they just need to APPEAR receding.
- **Risk:** Untested in RIMA. Pixel art readability at tilted angles can break at non-integer angles or non-PPU-aligned camera transforms. Mitigation: only test at angles that preserve pixel-grid alignment (e.g. exact arctan ratios that snap to pixel rows). Reversible if it breaks — revert one transform.
- **Verdict:** **YES, COMPLEMENTARY.** Single-line config change with potentially large payoff. Test before committing. Worst case: revert.

## F — Mixed (D + E + selective angled tiles in key areas)

- **Cost:** D + E + ~20 hand-curated "feature tiles" at 35° for hero spots (room entrances, boss arenas, focal pads).
- **Fidelity gain:** Highest possible without violating locks.
- **Risk:** Complexity. Selective angled tiles only work if they're hand-placed by RoomTemplate authoring, not random procedural. This pushes toward more manual room composition — which is fine because the user already locked manual composition for MVP (memory: `project_brush_v1_manual_composition_system.md`).
- **Verdict:** **HOLD.** Adopt D + E first. F is a Phase 2 polish, not an MVP fix. Mark as backlog.

---

# 4. Concrete next step (morning user can execute)

**Single dispatch, sequential.** Do these in order, stop at any failure:

## Step 1 — Camera tilt smoke test (15 min, orchestrator-runnable, no gen)

Dispatch to orchestrator or rima-codex:
> Open `Assets/Scenes/Demo/RoomPipelineTest.unity`. Locate the main Camera. Add a child wrapper or directly set `Camera.transform.eulerAngles = (10, 0, 0)`. Keep camera orthographic. Verify pixel art readability: take a screenshot before and after. Test with a Warblade sprite in scene. If floor reads more "receding" without character distortion, keep the change. If character distorts, switch to billboard-facing-camera shader for sprites. If neither works at 10°, try 6°. If 6° still distorts, revert to 0° and skip to Step 2.

Acceptance: visual A/B screenshot for morning review. Single-line config change, no risk.

## Step 2 — Floor de-emphasis pass (no gen, data only)

Dispatch to rima-codex or rima-doc:
> For all `BlueprintZoneTypeSO` floor zones (`zone_grass`, `zone_stone`, `zone_path`), reduce tint saturation by 20% and add `floorContrastMultiplier = 0.7` to the SO (new field, default 1.0). For Combat Room profile, set floor zones to use this multiplier. Test in RoomPipelineTest. Goal: floor reads as background wash, walls/props pop.

Acceptance: side-by-side before/after of a Combat Room. Floor visually recedes.

## Step 3 — Architecture decision LOCK update

Dispatch to rima-doc:
> Append to `TASARIM/MASTER_KARAR_BELGESI.md` as Karar #148:
> "Floor is structurally background. PixelLab pure-top-down tiles ACCEPTED as floor base. Floor de-emphasized via low contrast/saturation. Perspective carried by L3 walls (35°) + L4 patches + L5 props. Optional camera tilt 6-10° if visual A/B passes. Branch A (regen tiles at 35°) REJECTED — PixelLab structurally cannot author ground-plane trapezoidal projection."

Acceptance: locked Karar #148 in MASTER_KARAR_BELGESI.

## Step 4 (optional, only if Step 1+2 don't fully solve)

Defer to Phase 2: Branch F selective angled feature tiles for hero spots. Not MVP.

**Time budget for full sequence:** 90 minutes orchestrator + agent. Zero PixelLab credit spend.

---

# 5. Locks to revoke or update

## No revokes needed

- **Karar #100 (character 35° anchor LOCK):** UNTOUCHED. Verdict reinforces it.
- **Karar #143 / S92 6-layer pipeline:** UNTOUCHED. Verdict reinforces it.
- **Karar #147 / Multi-Layer Painter v1:** UNTOUCHED.
- **Karar #143-context Alabaster Dawn pipeline:** UNTOUCHED. Floor remains "PURE top-down" per the memory — that was the correct call.
- **Map Plan v1 (Hades model):** UNTOUCHED. Verdict explicitly enforces it.

## Updates required

- **Add Karar #148** (text in Step 3 above) — formalizes "floor is background, perspective lives in walls/props, Branch A rejected" so the next time someone proposes regen-tiles-at-35° the lock catches it.
- **Update memory `project_tile_character_angle_mismatch_s93.md`** with the verdict outcome: "Resolved S93 Opus verdict — Branch D primary + Branch E test. Floor de-emphasis pass scheduled. Branch A permanently rejected (PixelLab cannot author ground-plane trapezoid)."
- **Optional: Update `feedback_pixellab_sxl_low_detail.md` or related** with the new finding: "PixelLab `low top-down` view authors sprite-frame projection, not ground-plane projection. Cannot produce true 35° receding floor. Use only for objects, never for floor tiles."

---

# CONFIDENCE & CAVEATS

- **High confidence:** Branch A rejection. The PixelLab structural limitation is the core argument and it's well-supported by both the tool's documented behavior and by the Children of Morta postmortem ("grid floors too flat → abandon grid").
- **Medium-high confidence:** Branch D as primary fix. Matches Hades reverse-engineering and matches user's own S91 lock. Risk is execution discipline, not architecture.
- **Medium confidence:** Branch E (camera tilt). Untested in RIMA. The recommendation is to spend 15 minutes verifying empirically rather than committing blind. If it works, free win. If not, no loss.
- **Caveat:** I did not read the current floor tile output sprites directly. The verdict assumes the user's diagnosis ("floors look flat against 35° character") is accurate. If on inspection the floors already read as receding (e.g. because of v15c painted-top-down execution being better than described), Branch D's "floor de-emphasis" pass may be unnecessary and only Branch E remains. Morning A/B screenshot from Step 1 resolves this.
- **Out of scope:** Did not evaluate whether RoomTemplate authoring should shift to monolithic Photoshop-painted regions (full Hades pipeline). That's a Phase 2+ question. Current paint-brush stamp architecture is fine for MVP under Branches D + E.

# CONFLICTS WITH LOCKED RULES

NONE. All recommendations operate within Karar #100, #143, #147, Map Plan v1, Paint-Brush Architecture, and Alabaster Dawn pipeline. Branch B (the one that would conflict) is explicitly rejected. The new Karar #148 is additive, not contradictory.

---

# Sources consulted

- MCV/Develop — Behind the art of Hades (Jen Zee interview)
- Game Developer — Learn how Supergiant brought Hades' hand-painted characters to life
- Game Developer — Postmortem: Children of Morta
- TechRaptor — How Dead Mage Created the Art for Children of Morta
- Gemsbok / Xbox Tavern — Death's Door art style reviews
- Game Developer — Hyper Light Drifter ultra-modern stylings
- Slynyrd Pixelblog 22 + 55 — Top-down character/animation pixel art conventions
- Retronator Magazine Part 2 — Multiview / projection comparison
- RIMA memory: `project_alabaster_dawn_pipeline_lock`, `project_paint_brush_architecture_lock`, `project_multilayer_painter_v1_lock`, `project_map_plan_v1_lock`, `project_canonical_character_roster_v2`, `project_5000_pixellab_allocation_lock`, `feedback_blueprint_first_map_design`, `feedback_layered_terrain_mandatory`
