ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself. DO NOT use Unity MCP (another agent owns Unity). DO NOT call PixelLab generation tools (production is user-GATED). You MAY read PixelLab MCP help/docs if cheap, but prefer the local API docs.

# Amaç
Decide WHICH PixelLab tool/method + workflow to use for producing static WEAPON sprites for RIMA, grounded in the local API docs + the just-finished audit decisions. ANALYSIS ONLY — no generation, no code. Output a recommendation doc.

## Read these (ground truth)
- `STAGING/PIXELLAB_API_V2_REFERENCE.md` (+ `PIXELLAB_API_V2_LLMS.md` / `PIXELLAB_API_V2_RAW.md` as needed) — authoritative tool/endpoint capabilities.
- `STAGING/PIXELLAB_KNOWLEDGE_BASE.md` + `STAGING/PIXELLAB_SYNTHESIS_S114.md` — prior PixelLab learnings.
- `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md` §3 (decision table: PPU64+Point, TARGET-SIZE native not downscale, horizontal-right, ≤8/batch).
- `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md` (the 3 immediate weapons + canon).
- `STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md` (live Warblade_Greatsword = 64x16, PPU64, Point, pivot grip).

## THE QUESTION (user's two candidate methods — VERIFY each against the API docs, don't assume)
- **Method A:** "Create Image Pro" at 64px → produces ~16 variations to pick from (iteration/discovery).
- **Method B:** Item/object generation specifying exact item size (e.g. 64x64) → production-ready single asset.

Answer with API-doc evidence (endpoint/param names):
1. Does "Create Image Pro" actually output ~16 variations at low res? Is its output transparent (alpha), Point/pixel-perfect, and can it target a NON-square aspect (weapons are not square: greatsword 64x16, bow tall/thin, disc ~square)? What's the exact endpoint/tool name (MCP: is it `create_tiles_pro` or another)?
2. For item/object generation: which exact tool (`create_map_object` / `create_1_direction_object` / `create_object_state` / other)? Can it specify non-square target size? Transparent? Point? grip pivot control?
3. TARGET-SIZE vs downscale: the audit locked target-size native. Which method honors that without 128->64 grid damage?
4. style_images reference: can we pass the live `Warblade_Greatsford` (64x16) as a style ref so new weapons match? Does the ref's size constrain the batch (the session plan claims style refs set the batch size)?
5. Variations (pick-best) vs single production asset — which fits our pipeline (HandAnchorAttach needs ONE clean transparent sprite with a grip pivot at PPU64)? Is a hybrid right (use Pro/variations to explore, then lock the chosen one at target size)?

## OUTPUT
Write to `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md`:
- Recommended method (A / B / hybrid) + WHY, with API-doc evidence.
- Exact tool name(s) + the key params to set (size, transparency, Point, style_images, n_variations if any).
- Step-by-step workflow for ONE weapon (from prompt → variations/gen → pick → import-ready PNG at PPU64 + grip pivot).
- Batch strategy (≤8/batch; how to group the 3 gated weapons by aspect/size; can different aspects share a batch?).
- Per-weapon size table for the 3 gated weapons: Elementalist rune disc, Ranger bow, Shadowblade dagger (px W×H + aspect + grip point), consistent with canon + live 64x16 greatsword scale.
- Flag anything where the API docs are ambiguous as "VERIFY LIVE" (a question to confirm against the real PixelLab UI/API).
Write a short pointer + the recommended method to CODEX_DONE.md. NO generation, NO code.
