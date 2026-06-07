# PixelLab Weapon Method — ax Gemini 3.1 Pro High (recommendation)

⚠️ NOTE: ax-3.1-Pro CLAIMED to write this file but did NOT (file was missing). Content below is reconstructed by the orchestrator from ax-3.1-Pro's stdout. Treat its specifics as advisory; the substantive grounded doc is cx's `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md`.

## Recommendation: Method B (exact-size item generation)
Strongly recommends exact-size item generation over "Create Image Pro":
- Generating directly at exact dimensions (e.g. 64x16) inherently preserves the 1:1 pixel-grid mapping → prevents scaling artifacts.
- Avoids the messy transparency extraction that plagues composite-grid generation (Method A's 16-variation grid).

## Batching
- Different aspect ratios cannot be combined smoothly in a single item-generation batch.
- → Generate Rune Disc, Ranger Bow, Shadowblade Dagger as 3 SEPARATE batches (1-8 variations each).

## PPU & pivots
- Exact pixel sizes aligned to the live greatsword (64x16) scale.
- Grip pivots placed on the LEFT for horizontal-right assets.

## VERIFY LIVE (ax could not locate exact endpoints in workspace)
- Exact item endpoint name (e.g. `/v2/generate/item`), `image_size`/dimension params, sub-32 height handling, candidate count with style refs.

## Convergence note
Agrees with cx (which recommends hybrid: Method B via `create_1_direction_object` for production, Create Image Pro fallback for exploration / sub-32px-tall sprites like the 64x16 greatsword).
