# Codex Review - Object Production Master Spec v1

## Overall Verdict
PASS_WITH_REVISIONS

The live PixelLab OpenAPI schema was re-confirmed on 2026-05-20 from https://api.pixellab.ai/v2/openapi.json and matches the saved May 18 reference for the relevant endpoints. The draft is directionally usable, but several "LOCK" claims should be downgraded to pilot-gated rules because the API confirms capability, not output quality or cost.

Hard API confirmations:
- `POST /objects`: directions `1 | 8`; image_size width/height `32-256`; view `low top-down | high top-down | side`; `n_frames` must be one of `{1, 4, 16, 64}` and is only for `directions=1`; natural values are `<=42px -> 64`, `<=85px -> 16`, `<=170px -> 4`, else `1`; `n_frames>1` returns review status; `item_descriptions` exists for per-frame multi-frame packs; `object_view` is only the default-style category when no style_images exist.
- `POST /create-tiles-pro`: tile_type includes `hex`, `hex_pointy`, `isometric`, `octagon`, `square_topdown`; tile_size is `16-256` in live REST docs; tile_height supports non-square `16-256`; tile_view_angle is `0-90`, with `0=side`, `90=top-down`.
- `POST /create-isometric-tile`: image_size max is `64x64`; tile shape supports `thin tile`, `thick tile`, `block`.
- `POST /create-object-state`: edit_description is `1-1000` chars and creates a grouped state; the API does not publish a percent-pixel-change limit.

## Per-Karar Verdict

### Karar 1: L2a tool
- Verdict: NEEDS_REVISION
- Comment: `create_object` 128 low top-down is API-valid, but the draft's "same dispatch with L2b" rationale is not valid as written. L2a wants `view="low top-down"`; L2b wants `view="side"`; one `create_object` dispatch has one shared `view`. Also, if L2a is hidden collider source, style matching with L2b is less important than footprint correctness.
- Suggested change: Make L2a a separate pilot choice: default `create_object` 128 low top-down if the 65c99904-style flat block is acceptable; fallback `create_isometric_tile` 64 thin tile if collider footprint/isometric clarity matters. Remove same-dispatch-with-L2b as the justification.

### Karar 2: L2b tool
- Verdict: LIVE
- Comment: `create_object` 128 side, directions=1, n_frames=4 is API-valid and is the most practical first wall-face pilot. `create_tiles_pro` 64x128 is also API-valid by REST docs, but there is no local proof that it yields a wall billboard rather than tile-shaped output.
- Suggested change: Keep `create_object` route as first pilot, but add explicit prompt language: "tall vertical wall billboard, sprite fills canvas height, transparent padding allowed". Keep tiles_pro 64x128 as fallback only after one low-cost test.

### Karar 3: Size x n_frames matrix
- Verdict: LIVE
- Comment: The matrix aligns with the live API's natural n_frames thresholds: 32->64, 64->16, 128->4, 256->1. The API does not say `n_frames=64` is illegal at 64px, but it does say the natural value at <=85px is 16, so the spec should phrase this as "legal but not recommended" rather than hard illegal.
- Suggested change: Replace "64 NOT 64" language with "use 16 for 64px; higher counts are off-natural and drift-prone unless explicitly tested."

### Karar 4: Grouping kural?
- Verdict: NEEDS_REVISION
- Comment: The max 16 unique-item rule is good production discipline, but the `17-64). variants of items above` shorthand is weaker than the API affordance. `item_descriptions` exists specifically for per-frame descriptions in consistent-style multi-frame packs.
- Suggested change: For n_frames=64, provide either 64 explicit numbered text entries or use `item_descriptions` with 16 base items repeated as controlled variants. Avoid range shorthand in production prompts.

### Karar 5: state_of vs yeni object
- Verdict: NEEDS_REVISION
- Comment: The direction is right: use object states for small variants and new objects for new geometry/perspective/material families. However, the `<=30% pixel change` threshold is a RIMA heuristic, not an official API constraint. Wall damage with broken tops and scattered stones can cross from state edit into new geometry.
- Suggested change: Reword the threshold as a visual heuristic. For wall damaged variants, use state_of only for cracks, moss, glow, chips, and small missing stones; use new object dispatch for collapsed silhouette, broken top profile, arch damage, or rubble field changes.

### Karar 6: View parametresi mapping
- Verdict: NEEDS_REVISION
- Comment: The main view mapping is plausible, but `object_view="top-down"` should not be blindly applied to `view="side"` wall billboards. API docs define `object_view` as the default-style category when no style_images exist, not as camera direction. Mixing `view="side"` with `object_view="top-down"` is an untested category blend.
- Suggested change: Use `object_view="top-down"` for low/high top-down objects. For side wall billboards, start with `object_view=null`; if default style drifts, A/B test `object_view="sidescroller"` versus `top-down` with one pilot.

### Karar 7: Description prompt formula
- Verdict: LIVE
- Comment: The formula matches local PixelLab prompt rules: numbered lists improve batch control, HEX palette improves consistency, and genre labels / third-party style names should be avoided. The wall prompt is under-specified for canvas occupancy.
- Suggested change: For side walls, add "tall vertical wall billboard, fills most of the 128px canvas height, narrow transparent margins". HEX colors should be strongly recommended, not treated as the only valid color language; use color names plus HEX where palette lock matters.

### Karar 8: 4-piece wall batch
- Verdict: LIVE
- Comment: API support is better than the draft states because `item_descriptions` gives per-frame control for multi-frame packs. A 4-piece wall batch is realistic enough for a pilot. Still, face, perpendicular face, corner, and arch are different geometry classes, so this should not be treated as proven until one batch is visually reviewed.
- Suggested change: Use `item_descriptions` instead of relying only on one long description. Keep the gate: if the 4-piece pilot drifts, switch wall pieces to separate 128 n_frames=1 dispatches or two smaller 2-piece batches.

### Karar 9: Budget plan
- Verdict: NEEDS_REVISION
- Comment: The API docs do not publish exact endpoint pricing. Local evidence supports 64/128 base `create_object` around 20 gen and object states around an estimated 10 gen, but the draft's `128 n_frames=4 ~=25 gen` is not proven. Memory also records Pro tool batch sizes as 40-credit runs, which conflicts with the 25-gen estimate.
- Suggested change: Present budget as ranges until actual `usage` data is logged. For first wall pilot, reserve 40 gen for one 128 n_frames=4 batch, plus retry buffer. Update `STAGING/RIMA_PixelLab_BalanceLog.md` or the object production log after every dispatch with real usage.

## Open Questions for Iter 2
- Should L2a be visible at runtime? If no, footprint clarity beats style bundle; if yes, it needs its own visual QC path.
- For side wall objects, which `object_view` default category works best: null, top-down, or sidescroller?
- Does a 128 n_frames=4 wall batch cost 20, 25, 40, or another amount in actual `usage` metadata?
- Does `item_descriptions` materially improve 64-frame clutter control compared with numbered prompt text alone?
- What is the acceptance threshold for damaged walls: crack overlay state versus changed silhouette new object?

## Critical Blockers (if any)
None. The spec can move to v2 without asset generation, but it should remove unproven LOCK language around same-dispatch L2a/L2b, object_view defaults, state edit percent limits, and exact costs.
