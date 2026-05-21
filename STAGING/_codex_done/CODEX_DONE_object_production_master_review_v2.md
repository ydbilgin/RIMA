# Codex Re-Validate - Object Production Master Spec v2

## Overall Verdict
PASS_WITH_MINOR_REVISIONS

No Iter 3 is needed. The v2 revision resolves the five Iter 1 NEEDS_REVISION items well enough for first pilot dispatch, but two open questions should be answered inside the spec instead of left to user/pilot, and one implementation caveat should be made explicit: the dispatch path must pass `item_descriptions` through to the PixelLab REST API or equivalent wrapper.

## Iter 1 Revisions Status

### Karar 1: L2a - RESOLVED
The same-dispatch L2a/L2b rationale was removed, and the replacement is implementable: default `create_object` 128 low top-down for hidden collider source, fallback `create_isometric_tile` 64 thin tile if L2a becomes visibly important. The runtime-visibility gate is the right decision point.

Remaining note: if L2a is visible, the fallback should be treated as a visual pilot, not just a pivot adjustment, because 64px iso tile and 128px wall billboard may differ stylistically.

### Karar 4: Grouping - RESOLVED
The v1 range shorthand problem is fixed. Template B v2 gives 64 explicit `item_descriptions`, which is the correct controlled form for `n_frames=64` based on Iter 1 API evidence.

Production rule should say: `item_descriptions` is optional at API level but required by RIMA production for multi-frame control. For deterministic packs, array length should equal `n_frames`; shorter arrays may be API-accepted but leave unspecified frames uncontrolled.

### Karar 5: state_of - RESOLVED
The official-looking pixel percent rule was removed and replaced with a visual heuristic. The damaged-wall split is correct: crack/moss/glow/chip overlays can use `state_of`; broken tops, collapsed silhouettes, arch collapse, rubble fields, and cross-material/cross-perspective changes should be new object dispatches.

The extra +100-200 generation budget is acceptable against the stated 2500 remaining budget, especially because damaged pieces are silhouette-bearing gameplay/readability assets.

### Karar 6: View - RESOLVED
`object_view=null` for side wall billboards is safer than forcing `top-down`. The spec now separates camera `view` from default-style `object_view`, and the A/B fallback to `sidescroller` is sufficient.

Do not add `object_view="top-down"` back into side billboard prompts unless a pilot proves it is visually superior.

### Karar 9: Budget - RESOLVED
The range-based plan is consistent with the Iter 1 concern that exact prices are not published and that previous evidence included 40-credit pro-tool runs. The 25-40 range for 128 `n_frames=4`, 15-25 for 128 single, and 8-15 for states are actionable planning ranges.

The usage-log gate after every dispatch is the right way to refine estimates. Keep the upper bound, not the average, as the production reservation number.

## New Content Validation

### item_descriptions API integration: VALID
Template A with 4 entries, Template B with 64 entries, and Template C with 16 entries match the intended per-frame-control model from Iter 1 API confirmation.

`item_descriptions` should carry per-frame object identity. The main `description` should carry shared style, palette, camera, canvas occupancy, and transparent-background constraints. Numbering inside `item_descriptions` is optional; explicit one-entry-per-frame wording is the important part.

Implementation caveat: current production dispatch must be checked to ensure it supports forwarding `item_descriptions`. If direct MCP wrapper schema does not expose the field, use the REST dispatch path that Iter 1 validated.

### Pilot gates: ACTIONABLE
The gates are not too heavy. They are placed at high-risk boundaries: L2a visibility, first wall style consistency, side-wall `object_view`, and first 4-piece wall review. This is appropriate because production scale-up depends on those outputs.

Keep the review burden bounded: one Template A pilot first, then only branch if it fails. Do not run all A/B gates up front.

### Range-based budget: ACTIONABLE
The range budget is more honest and more useful than a fake exact value. For user-facing approval, show upper-bound reservation and likely range together, e.g. "reserve 420 gen for Sira 1-3; expected 210-420." A single conservative value alone would hide useful retry/headroom information.

## Open Questions Assessment

Questions that are reasonable user/pilot gates:
- #1 L2a runtime visibility: user/gameplay decision.
- #2 side-wall `object_view` result: pilot output decision.
- #3 `item_descriptions` geometry separation: pilot output decision.
- #4 real 128 n_frames=4 usage: pilot usage-log decision.
- #6 damaged-wall budget acceptance: user production budget decision.

Questions Codex should answer now, not leave open:
- #5 `item_descriptions` redundancy: main description should be shared style anchor; per-frame details belong in `item_descriptions`. Do not duplicate a full numbered list in main description.
- #7 Template B 64 explicit vs 16 base + AI variants: use 64 explicit entries for first production. Do not A/B this unless the first 64-frame batch quality is poor.

## Final Recommendation
- Mark the spec LIVE after minor inline edits for Open Questions #5 and #7 plus the dispatch-path caveat for `item_descriptions` support.
- First pilot dispatch remains Template A v2 only.
- Reserve upper-bound cost for planning, then replace estimates with real `usage` after the pilot.
- No Iter 3 required unless the dispatcher cannot send `item_descriptions`.

## Critical Blockers (if any)
None.
