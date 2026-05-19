# Codex Review Task — RIMA Fluid Transition Architecture (Opus Verdict)

You are a senior Unity/C# game-systems reviewer. Read the design at
`STAGING/RIMA_FLUID_TRANSITION_DESIGN.md` end-to-end, then verify it
against the project's locked constraints.

## Authoritative source documents to cross-check against

1. `STAGING/CHATGPT_AUTHORITATIVE_CORRECTION.md` — 15 critical fixes
   (PPU=32, Wang16 corner-mask, JSON-driven import with Y-flip,
   WallKitSO mandatory, Tier B pixel-art-compatible, dual-use Wang
   atlases, props tiered n_frames, same-family terrain pair rule,
   runtime vs editor modes, LoRA secondary, vertical slice first,
   L0–L11 layer stack, required SO list).
2. `STAGING/CHATGPT_PHASE0_REVIEW.md` — Phase 0 results + open
   questions (Q1 floor variant strategy, Q2 decal density,
   Q3 Y-flip rule, Q4 PatchAtlas defaults, Q5 Phase 1 plan,
   Q6 style drift).
3. `CURRENT_STATUS.md` — Sprint 13 LIVE state, 321/321 EditMode tests
   passing, Brush V1 ship-ready.
4. `Assets/Scripts/Rima/MapDesigner/**` — current Brush V1 source
   tree (~115 source + 26 test files).
5. `.claude/PROJECT_RULES.md` — repo-wide rules.

## Review questions (give a verdict on each)

### A. Architectural fit with ChatGPT lock
- A1. Does Opus's "hybrid zones + stamps + stacked tiles" model
  honor every numbered ChatGPT correction? Flag any direct conflict.
- A2. Wang16 scope: Opus says elevation/feature edges only. Does any
  code skeleton (Section 1–6) accidentally route same-elevation
  terrain blending through Wang16? Quote line if so.
- A3. PPU=32 lock: any place where Opus's decal/tile sizes silently
  imply PPU=64 or half-cell math?
- A4. WallKitSO: ChatGPT says mandatory for Image 1 quality. Opus
  defers WallKitSO to Phase 1B. Is the deferral safe given Phase 1A
  ships a "Type A enclosed dungeon"? What's the minimum WallKit
  surface Phase 1A actually needs?

### B. SO scaffolding completeness
- B1. Opus lists 9 Phase 1A SOs (Section 7). Cross-check against
  ChatGPT's required-SO list (correction memo §"Critical fix #15").
  Anything ChatGPT named that Opus omitted?
- B2. `ImportAssetRole` enum: does Opus's set match ChatGPT's
  enum (Terrain32 / Wang16_32 / FloorMacro64 / Decal / Scatter /
  Prop / Character / TierBBackground / UI)? Note divergence.
- B3. `Wang16AtlasSO.usageRole` (ChatGPT critical fix #7) — does
  Opus's design preserve dual-use separation, or fold it into
  TerrainDefinitionSO?
- B4. Field-additive policy on `PatchAtlasSO` / `PropDefinitionSO`:
  do Opus's extensions break the 321 existing tests? Compile-only
  guess is OK; cite which test would fail if any.

### C. Brush V1 extension safety
- C1. Opus says "extend, do not rewrite" Brush V1. Read
  `Assets/Scripts/Rima/MapDesigner/Editor/Brush*.cs` and identify
  the integration seams. Is `BrushStrokeWindow` a new window or a
  tab inside the existing window? Opus contradicts itself between
  Section 1 (separate EditorWindow) and Section 10 Risk 8 (single
  RoomComposerWindow with tabs). Pick the right answer.
- C2. Determinism: Risk 7 says all sub-grid placements seeded
  through single System.Random per room. Is the current Brush V1
  seeding architecture compatible? Where does it currently seed?

### D. Layer stack & rendering
- D1. Tile stacking via separate Tilemaps (Section 6). Is sortingLayer
  configuration (`-200, -100, ..., +500`) feasible without changing
  TagManager / SortingLayer asset? Cite if a project-wide change is
  needed.
- D2. L9 sprite-based shadows vs URP ShadowCaster2D: does the
  performance argument hold? Quick math: 100 props × 5 lights = 500
  caster evals/frame. Is 500 actually a problem on RTX 5080 / 9800X3D?
- D3. L10 8-emitter budget per room: any room in current RoomBank
  exceed this? (Read room bank if it exists.)

### E. Phase 1A dispatch list
- E1. 17 PixelLab dispatches. Compare against ChatGPT review §Q5.
  Match? Any tool-signature mismatch (e.g., create_tiles_pro vs
  create_object choice)?
- E2. Brazier (#15) added by Opus to validate L10 — agree? Or is
  L10 validation premature in Phase 1A?
- E3. Wall kit 4 modules (#11-14). Is "straight + outer corner +
  doorway + pillar" the minimum, or is inner-corner / top-cap also
  required for any Type A enclosed dungeon shape?

### F. Implementation order (Section 9)
- F1. Step 1 (SO scaffolding) before Step 2 (3-call pipe validation).
  Is the order right, or should Step 2 happen first to validate
  PixelLab style lock before sinking SO design effort?
- F2. Step 7 (Brush V1 extensions) parallel-able to Steps 4-6.
  Verify no file-overlap between the parallel tracks.

### G. Risk analysis completeness
- G1. Opus lists 8 risks. What's missing? (Consider: PixelLab MCP
  rate limits, Backblaze CDN reliability per past incident, palette
  drift across multi-day dispatches.)

## Output format

Return a markdown report with the following structure:

```
# Codex Review — Fluid Transition Design

## Verdict: GO / GO-WITH-FIXES / NO-GO

## Section-by-section findings
- A1, A2, A3, A4 ...
- B1, B2, B3, B4 ...
- (continue for C, D, E, F, G)

## Top 5 blockers (in priority order)

## Top 5 nits (won't block Phase 1A but should fix in Phase 1B)

## Recommended next concrete action
(one sentence — what should the orchestrator do tomorrow)
```

Save the report to `STAGING/CODEX_REVIEW_fluid_transition_DONE.md`.

Do not modify the source design doc. Do not touch any `Assets/`
script. Read-only review.
