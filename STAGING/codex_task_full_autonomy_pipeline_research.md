# Codex Task — Full Autonomy Wang16 Pipeline (User Cannot Draw)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## CRITICAL NEW CONSTRAINT

User CANNOT draw in Aseprite, Pixelorama, or any pixel art editor. **Every step of the pipeline must be AI-driven or scripted.** No manual cell painting. No hand-drawn corner masks. No motif touch-ups.

Your previous verdicts (`CODEX_DONE_topdown_floor_pipeline_decision.md`, `CODEX_DONE_wang16_compositor_review.md`) suggested "Aseprite cleanup" or "manual mask authoring" as fallback. **REVOKE those fallbacks.** Any plan that requires user to hand-paint anything is invalid.

User feedback memory: `memory/feedback_user_cannot_draw_full_autonomy_required.md` (full autonomy LOCK).

## Mission

Investigate open source Wang tile assembly tools + design a **100% AI/script-driven** Wang16 pipeline. No user pixel art skill required at any step.

Web research evidence already gathered (orchestrator search):
- **Wangscape** (`Wangscape/Wangscape`) — Procjam 2016, "Convert terrain tiles to procedural corner Wang tilesets" with JSON config + smooth transitions + variant generation
- **IJDykeman/wangTiles** (158 stars) — Wang-like tile experiments
- **samsartor/content_aware_tiles** (22 stars) — **Training-free diffusion-based Wang tile generator** (recent, May 2026 update)
- **fogleman/WangTiling** (12 stars) — Weighted Wang tiling
- **Gollum999/perlin-wang** (20 stars) — Perlin noise Wang tile generator
- **OpenGameArt Wang 'Blob' Tileset** — free reference 47-blob template

## Research questions

1. **Wangscape deep-dive** — Is it actually usable for RIMA 2026?
   - Language, build status, license, JSON config format
   - Does it accept ONLY base terrain tiles (no manual mask) and generate Wang16 autonomously?
   - If yes: how do we drive it from Python/Codex script?
   - If no (e.g. abandoned, missing features): score and move on
   
2. **content_aware_tiles diffusion approach** — Is this the dream solution?
   - Reads as: feed one image, get full Wang tile set via diffusion
   - Compatibility with RIMA constraints (32px chunky pixel art, no smooth gradients, palette control)
   - GPU requirements (we have RTX 5080), Python integration

3. **Procedural corner mask generation** — Can we kill the "user draws mask" problem with deterministic noise?
   - Perlin / cellular / blue noise generated mask
   - Pillow + numpy implementation feasibility
   - Quality vs hand-painted mask trade-off

4. **Hybrid recommendation** — Given user-cannot-draw constraint, what's the actual best pipeline?
   - Option A: Wangscape direct integration (if mature)
   - Option B: content_aware_tiles diffusion (if it produces RIMA-acceptable output)
   - Option C: Custom Python with procedural mask + AI-generated base + AI-generated edge motif
   - Option D: Something else you find during research

5. **Codex implementation plan** — Once you pick the path, write a step-by-step build plan.
   - File structure (`STAGING/wang16_compositor/...` or `tools/...`)
   - Dependencies (Pillow, numpy, requests for PixelLab, etc.)
   - Fully autonomous workflow: Codex/Claude can run end-to-end without user touching pixels
   - Acceptance gates that are AI-checkable (no human "looks good" needed)

## Available tools (re-list with autonomy lens)

- PixelLab MCP (`create_tiles_pro`, `create_object`, `create_map_object`, `create_topdown_tileset`) — prompt-driven, AI runs
- Codex `gpt-image-1` — prompt-driven, AI runs
- Local RTX 5080 — ComfyUI/FLUX/SD for fully local generation
- Python + Pillow + numpy — Codex writes, Codex runs
- UnityMCP — Codex/Claude drive for import + scene composition

## Required output structure

`STAGING/CODEX_DONE_full_autonomy_pipeline.md`:

```
# VERDICT
[Best pipeline given user-cannot-draw constraint. Concrete tool choice.]

# 1. Wangscape evaluation
[Mature / abandoned / usable / not? Concrete reasons. License? Driver script feasibility?]

# 2. content_aware_tiles evaluation  
[Diffusion approach: works for chunky 32px pixel art? GPU requirements? Output quality vs RIMA constraints?]

# 3. Procedural mask alternative
[Can we generate corner masks deterministically with noise? Code sketch.]

# 4. Selected pipeline
[Step-by-step, fully autonomous. Each step says "Codex runs X" or "Claude dispatches Y". NO user pixel art step.]

# 5. Implementation plan
[Concrete next dispatch: what Codex script to write, what scope, what acceptance gates. Single-pair MVP first per prior verdict.]

# 6. Risks specific to "user cannot draw"
[What happens if AI output is borderline acceptable — what's our iteration loop without falling back to "user touches up"?]
```

Effort: high. This is foundational — we cannot dispatch the compositor build until this answers.

Web research aggressively. Read README/wiki of Wangscape + content_aware_tiles. Compare honest options.
