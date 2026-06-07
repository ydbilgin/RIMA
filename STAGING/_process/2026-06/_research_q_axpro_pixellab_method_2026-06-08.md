# RIMA — DEEP RESEARCH (ax Gemini 3.1 Pro High): PixelLab weapon-production METHOD

You are an independent deep-research advisor. Read these repo files for ground truth, then ALSO use any knowledge you have about PixelLab's current capabilities. Be decisive and lean — no essay.

Ground-truth files (read):
- `STAGING/PIXELLAB_API_V2_REFERENCE.md` (+ `PIXELLAB_API_V2_LLMS.md` if needed) — PixelLab tool/endpoint capabilities
- `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md` §3 (locked decisions)
- `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md` (the 3 immediate weapons)
- `STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md` (live Warblade_Greatsword = 64x16, PPU64, Point, grip pivot)

QUESTION — for STATIC weapon sprites of VARIED aspect (greatsword 64x16, bow tall/thin, rune disc ~square), which PixelLab method is best:
- **A:** "Create Image Pro" at 64px → ~16 variations to pick from (discovery/iteration)
- **B:** item/object generation at exact item size (e.g. 64x64) → production-ready single asset
- or a hybrid?

Locked constraints (from the audit): PPU64 + Point, TARGET-SIZE native (NO big-canvas→downscale; 128→64 damages the pixel grid), transparent alpha, directional weapons horizontal-right (grip left/tip right), ≤8 items/batch, style-ref = live Warblade_Greatsword 64x16.

Deliver:
1. Recommended method (A / B / hybrid) + WHY (verify each candidate's claim: does Create Image Pro really do ~16 variations? is its output transparent + Point + non-square-aspect capable? can item-gen specify non-square size + transparency + grip pivot?).
2. Exact tool/endpoint name + key params to set.
3. One-weapon workflow (prompt → gen → pick → import-ready PNG at PPU64 + grip pivot).
4. Batch strategy (≤8/batch; can different aspects share a batch? how to group the 3 weapons?).
5. Per-weapon size table: Elementalist rune disc / Ranger bow / Shadowblade dagger (px W×H + aspect + grip point), consistent with the 64x16 greatsword scale.
6. Flag anything uncertain as "VERIFY LIVE".

Write your answer to `STAGING/_process/2026-06/_research_axpro_pixellab_method_2026-06-08.md`.
