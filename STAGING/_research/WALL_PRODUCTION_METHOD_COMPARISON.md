# RIMA Wall Production Method Comparison

## 1. Executive Summary

- **PRIMARY: Method A -> targeted Method C repair.** Use a hand-authored 3D box outline sheet to lock silhouette, iso angle, and target crop boxes, then use Web UI Edit Image Pro to fill the forms. Repair only failed pieces with Same Style Pro from the best accepted wall reference.
- **SECONDARY: Method G, but only after Method A has produced a style seed.** Big pack canvas is useful for variants and Act 2-4 expansion, not for the first locked Phase 1 wall set where exact 96x160 / 128x160 / 48x160 dimensions matter.
- **Do not use MCP direct generation for this Phase 1 wall set.** `create_map_object` square clamp breaks the tall wall requirement; current MCP object routes are useful for square props/decals but not for the requested non-square wall sprites.
- **Avoid pure prompt grid methods as the first production path.** Method B and raw Method G can be cheap, but their failure mode is expensive: wrong grid interpretation, merged objects, inconsistent wall grammar, or unusable crops.
- **Best practical sprint plan:** 2 controlled outline sheets via Method A, then Same Style Pro redo for failed pieces, then a G-style variant sheet for damaged/Act variants once the wall grammar is locked.

Cost estimates below use a nominal Pro dispatch cost of **30 generations** with **+/-20% = 24-36 gen** unless local evidence suggests a wider band. Existing RIMA log evidence: a 128px 4-frame wall object pilot cost 20 gen, but Web UI Pro cost should still be reserved at 24-36 until actual usage is logged.

## 2. Method-by-method analysis tablo

| Method | Cost estimate (+/-20%) | Style consistency (1-10) | Dimension control (1-10) | Multi-Act expansion fit | Failure mode | Recovery cost | RIMA-specific fit |
|---|---:|---:|---:|---|---|---:|---|
| A - 3D Box Outline + Edit Image Pro | 2 sheets recommended: nominal 60 gen, range 48-72. Single 8-slot sheet: 24-36 but higher quality risk. | 8 | 9 | Strong if outline templates are reusable per Act; material prompt can retheme same geometry. | AI ignores some guide lines, overpaints slot boundaries, turns silhouettes into generic ruins, or loses archway negative space. | Low-medium: redo failed slots with Same Style Pro, 24-36 each. | **Best fit.** Exact wall footprints, iso 30 degree cue, and crop discipline are built into the source image instead of trusted to prompt alone. |
| B - Plain Canvas + Multi-Cell Prompt | Nominal 30 gen, range 24-36 per attempt. Realistic 2-4 attempts: 48-144. | 5 | 3 | Weak; prompt-only grid has to be relearned per Act. | One object instead of grid, cells merged, wrong sizes, wrong count, inconsistent camera angle. | High: likely discard and restart or switch to A. | Poor for Phase 1. It is cheap only if the first shot works, and the known object-tool history suggests grid compliance is unreliable. |
| C - Same Style Pro Iteration | Seed 24-36 + 7 derivatives 168-252 = 192-288. Task nominal ~240 matches. | 9 | 6 | Good for reusing visual language across Acts if each Act has a locked seed. | Style sticks but geometry drifts; tall pieces can become square-ish; arch/endcap silhouette may mutate. | Medium: individual redo 24-36 each. | Strong as repair/fallback. Too expensive and less dimension-locked as the primary for all 8 pieces. |
| D - Kenney Pack Re-Theme | One bundle pass 24-36, plus likely 2-4 corrections 48-144. Total realistic 72-180. | 7 | 5 | Medium; can retheme the same source pack per Act, but proportions stay source-biased. | Kenney anatomy dominates, sandstone/block proportions survive, RIMA iso wall scale mismatches, derivative look feels external. | Medium-high: requires manual resizing or A/C after re-theme. | Useful only as anatomy bootstrap/reference. Not recommended for final locked RIMA wall sprites. |
| E - Iterative Reference Build | 8 chained calls nominal 240, range 192-288; more if chain drift requires rollback. | 8 | 5 | Medium; chain can carry style but also carries errors. | Early bad wall contaminates later outputs; each step compounds silhouette and palette drift. | High if drift found late: rollback to last good reference and redo downstream pieces. | Good art-direction workflow, weak production workflow for an 8-piece locked set. Better as selective C-style repair, not full chain. |
| F - create_map_object x8 Direct | 8 calls nominal 240, range 192-288. | 4 | 1 | Weak for walls; square output breaks canonical sizes. | Known square clamp: 96x160 becomes 96x96; style drift across 8 independent calls; no batch consistency. | Very high: output cannot satisfy target dimensions without repaint/extending. | Reject for Phase 1 wall pieces. Keep MCP map objects for square decals/props only. |
| G - Big Asset Pack Canvas + Iterative Reference | Initial 1-2 pack calls 24-72; 2-4 iterative passes 48-144. Realistic 72-216. | 7 raw, 8 if seeded by A | 5 raw, 7 if using visible crop/slot guides | **Strongest expansion path** after a locked seed; supports variants, broken pieces, Act rethemes, and larger packs. | Multi-cell handling uncertain; model may prioritize beauty over slot exactness; small pieces can be under-detailed in 4x4 layout. | Medium: accept good cells, redo bad cells with C or smaller G/A sheet. | Good secondary system. Risky as first source of canonical Phase 1 dimensions. |

### Detailed notes by method

**Method A - 3D Box Outline + Edit Image Pro**

This method gives the model a visible production contract: slot boundary, wall height, footprint, arch cutout, corner mass, and iso angle. For RIMA Phase 1 this is more important than pure style novelty because the eight pieces must assemble in Unity with consistent pivots and dimensions. The safest layout is not one 16-cell sheet. Use two 512x512 sheets with 4 pieces each, or one tall/low split:

- Sheet 1 P0 tall: `wall_tall_straight` 96x160, `wall_tall_corner` 96x160, `wall_archway` 128x160, `wall_endcap_column` 48x160.
- Sheet 2 P1 low: `wall_low_straight` 96x96, `wall_low_corner` 96x96, `wall_low_endcap` 48x96, `wall_low_T_junction` 96x96.

The source sheet should include crop labels outside the slots, light gray construction lines, transparent/flat neutral background, and no texture. Prompt should explicitly say to preserve each slot's bounding box and transparent margins. This makes cropping deterministic after generation.

**Method B - Plain Canvas + Multi-Cell Prompt**

This is the lowest setup cost but has the worst control profile. The model has no geometry anchor, so it has to infer grid, count, wall taxonomy, dimensions, perspective, material, and sprite isolation from text. That is too many simultaneous constraints. It is only worth a one-call experiment after the production path is already secured, not as the startup decision.

**Method C - Same Style Pro Iteration**

Same Style Pro is the best targeted repair tool. It should not carry the whole production load because seven independent derivative calls cost more and can still miss exact dimensions. Its best role is: pick the strongest accepted Method A output as the reference, then generate only failed or missing pieces with precise prompts and the reference attached.

**Method D - Kenney Pack Re-Theme**

This can provide an anatomy sanity check for modular stone wall pieces, but it creates a style ownership problem: even if CC0 is acceptable, RIMA's wall language may inherit source pack proportions. The current target is dark granite, cyan rift accents, 30 degree iso, and custom dimensions; a re-themed generic pack is more likely to be a sketch/reference than final production art.

**Method E - Iterative Reference Build**

The chain approach is attractive for art direction, but production should avoid dependency chains where a late asset inherits every prior mistake. If used, keep a stable base reference set of 2-3 accepted walls instead of a single previous output. That turns E into a controlled C variant, not a fragile linear chain.

**Method F - create_map_object x8 Direct**

The known square clamp is decisive. A method that cannot emit 96x160, 128x160, or 48x160 is not viable for the canonical wall set. Even if the art is good, it creates downstream manual extension work and breaks the main reason to produce a wall set now: consistent assembly dimensions.

**Method G - Big Asset Pack Canvas + Iterative Reference**

G is valuable, but its role is expansion, not first lock. After A produces an accepted wall grammar, use that image as a reference for a larger 512x512 or 688x384 pack containing damaged variants, moss/rift variants, and Act rethemes. A 4x4 grid is efficient for concept expansion, but for final sprites use 2x4 or 4x2 layouts with visible slot guides when exact cropping matters.

## 3. Combination recommendations

### Recommended hybrid: A -> C -> G

1. **A first:** create two controlled outline sheets and run Web UI Edit Image Pro.
2. **C second:** accept the strongest pieces, then Same Style Pro only for failed pieces or missing silhouettes.
3. **G third:** once Phase 1 base style is locked, use a big canvas to generate variants: damaged tops, moss, rift-open, Act 2 bog, Act 3 alabaster, Act 4 void.

Why this is strongest: A handles geometry, C handles local repair, G handles expansion. Each method covers the previous method's weakest point without making the whole pipeline expensive.

### Backup hybrid: D -> A -> C

Use Kenney or another clean wall pack only as a shape/anatomy reference while generating the outline sheet. Do not directly ship the re-themed result unless it passes RIMA scale and silhouette QC. This is useful if hand-authored box outlines look too abstract for Edit Image Pro.

### Rejected primary combinations

- **B -> C:** too much cleanup. If B makes one attractive wall but fails the grid, it simply becomes a seed for C, not a production method.
- **F -> C:** starts from wrong dimensions. Same Style cannot reliably fix a square-clamped source into tall locked sprites.
- **E full chain:** too fragile. Use stable multi-reference Same Style instead of one-reference chain dependency.
- **Raw G first:** good experiment, weak canonical lock. It should come after a seed, not before.

## 4. Final production sequence

### Sprint 1 - Geometry lock and P0 wall sheet

**Method:** A.

**Steps:**

1. Generate a 512x512 source sheet locally with four labeled construction slots: tall straight 96x160, tall corner 96x160, archway 128x160, endcap column 48x160.
2. Upload to Web UI Edit Image Pro.
3. Prompt for RIMA Shattered Keep dark granite, cyan rift accents, painterly pixel art, iso 30 degree, preserve slot bounds, transparent background, no extra objects.
4. Export, crop the four target rectangles, and run visual QC against dimension, pivot, silhouette, and style.

**Cost:** nominal 30 gen, range 24-36. Reserve one retry: total reserve 48-72.

**Expected output:** 3-4 usable P0 pieces. If one piece fails, do not rerun whole sheet unless style is globally wrong.

### Sprint 2 - P1 low wall sheet

**Method:** A.

**Steps:**

1. Generate second 512x512 source sheet with low straight 96x96, low corner 96x96, low endcap 48x96, low T junction 96x96.
2. Use the accepted Sprint 1 output as visual reference in Web UI if possible.
3. Repeat Edit Image Pro with same palette and material prompt.
4. Crop and QC.

**Cost:** nominal 30 gen, range 24-36. Reserve one retry: total reserve 48-72.

**Expected output:** 3-4 usable P1 pieces that match P0 material but read as lower cover.

### Sprint 3 - Targeted repair pass

**Method:** C.

**Steps:**

1. Pick the best accepted wall as reference; preferably include one tall face plus one corner if Web UI supports multiple refs.
2. Same Style Pro each failed piece with a strict target prompt and target canvas size.
3. For archway failures, explicitly preserve negative opening and keystone silhouette.
4. For endcap failures, explicitly require narrow column mass and transparent side margins.

**Cost:** 24-36 per failed piece. Expected 1-3 repairs = 24-108.

**Expected output:** all 8 Phase 1 pieces accepted and crop-ready.

### Sprint 4 - Assembly QC and Unity-facing normalization

**Method:** no AI unless repair needed.

**Steps:**

1. Normalize PNG canvas sizes exactly to target dimensions.
2. Set consistent pivot rules: bottom center for tall walls/endcaps, bottom center or footprint center for low walls depending on collider use.
3. Build a quick contact sheet with checker/grid background.
4. Place pieces in a simple room test to check wall joins, corner grammar, archway clearance, and cyan accent density.

**Cost:** 0 gen unless repair needed.

**Expected output:** final Phase 1 wall base set, import-ready.

### Sprint 5 - Variant and Act expansion

**Method:** G seeded by accepted Phase 1 sheet, then C for high-value corrections.

**Steps:**

1. Create a 512x512 or 688x384 big pack prompt using the accepted wall sheet as reference.
2. Generate damaged, mossed, rift-open, dusted, and collapsed variants.
3. For Act 2-4, reuse the same geometry sheet and retheme material/accent palette instead of changing silhouette.
4. Promote only cells that pass crop and grammar QC.

**Cost:** 2-4 big pack passes = 48-144. Same Style corrections as needed: 24-36 each.

**Expected output:** 8 base walls plus 8-24 controlled variants without reopening the whole Phase 1 decision.

### Total recommended reserve

| Stage | Expected gen | Reserve gen |
|---|---:|---:|
| Sprint 1 P0 A sheet | 24-36 | 72 |
| Sprint 2 P1 A sheet | 24-36 | 72 |
| Sprint 3 targeted repairs | 24-108 | 108 |
| Sprint 4 normalization | 0 | 0 |
| Sprint 5 variants/expansion | 48-180 | 180 |
| **Phase 1 base only** | **72-180** | **252** |
| **Base + first variant expansion** | **120-360** | **432** |

This is materially cheaper than full C/E/F-style 8 independent calls while preserving stronger dimension control.

## 5. Risk catalog

| Risk | A | B | C | D | E | F | G | Mitigation |
|---|---:|---:|---:|---:|---:|---:|---:|---|
| Wrong final dimensions | Low | High | Medium | Medium | Medium | **Certain** | Medium | Use visible slot boxes, crop guides, and post-export canvas normalization. Reject F for canonical walls. |
| AI merges multiple cells | Medium | High | Low | Medium | Low | Low | High | Prefer 4-cell sheets over 16-cell sheets. Add gutters and labels outside crop zones. |
| Style drift across pieces | Low-medium | High | Low | Medium | Medium | High | Medium | Generate related pieces in the same A sheet; use accepted sheet as reference for C/G. |
| Archway loses opening | Medium | High | Medium | Medium | Medium | Medium | Medium | Use explicit outline with empty arch cutout; prompt "preserve transparent open doorway." Repair arch separately if needed. |
| Corner does not connect to straight wall | Medium | High | Medium | Medium | Medium | High | Medium | Include corner and straight on same sheet; QC by placing adjacent in Unity/contact sheet. |
| Cyan accent overpowers readability | Medium | Medium | Medium | Medium | Medium | Medium | Medium | Prompt "small cyan rift accent, 5-10% of surface, not full glow"; QC at gameplay zoom. |
| Big canvas under-details small pieces | Low for 4-cell A | Medium | Low | Medium | Low | Low | High | Use 4-cell or 8-cell max for final pieces; reserve 16-cell G for variants/concepts. |
| Source-pack look dominates | None | None | None | High | None | None | None | Use D only as reference/anatomy input, not direct final output. |
| Chain contamination | None | None | Low | None | High | None | Medium | Keep stable accepted references; do not use a single latest output as the only source. |
| MCP/tool limitation blocks target | None for Web UI A | None | None | None | None | **High** | None | Use Web UI for non-square; use MCP only for square decals/props or future API after non-square support is verified. |

### QC gates

- **Dimension gate:** every cropped sprite must exactly match target W x H before import.
- **Silhouette gate:** tall straight, corner, archway, and endcap must be distinguishable in grayscale.
- **Assembly gate:** straight + corner + endcap must look joined, not merely adjacent.
- **Perspective gate:** all pieces must share the same iso 30 degree top/face relationship.
- **Palette gate:** dark granite remains dominant; cyan rift accent is a controlled accent, not a second base material.
- **Expansion gate:** Act rethemes may change material and accent palette, but must not change the locked wall grammar unless explicitly producing a new Act-specific variant family.

## Final Recommendation

Use **Method A as the production spine**: two small outline sheets, Edit Image Pro fill, deterministic crop and QC. Use **Method C as the repair tool** for any failed piece. Promote **Method G only after the base set is accepted**, where its pack efficiency is valuable for variants and Act rethemes.

Do not spend Phase 1 production budget on Method F, and do not start with Method B. They are cheap only on paper; their recovery path is the actual cost.
