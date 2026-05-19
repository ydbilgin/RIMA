# Codex Task — Boona Tweet Independent Review (for 3-agent map composition decision)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Scope
**NO CODE — design review only.** Read the analysis package and produce an independent written review.

## Input
- Read: `STAGING/boona_analysis_package.md` (full context: tweet, video frames analysis, 24 replies, 8 hypotheses H1-H8)
- Read for context: `MEMORY/feedback_blueprint_first_map_design.md`, `MEMORY/feedback_layered_terrain_mandatory.md`, `MEMORY/project_room_composer_paint_intent_lock.md`, `MEMORY/project_brush_v1_manual_composition_system.md`
- Optional reference for current state: `Assets/Editor/MapDesigner/Blueprint/` (AutoPopulator + BlueprintZoneTypeSO if you want to see what's wired today)
- Current "messy" screenshot: `Assets/Screenshots/PlayableRoom_combat_v15c_8layer.png` (you can describe what you see)

## Output
Write to `STAGING/CODEX_DONE_BOONA_REVIEW.md` (single file).

Structure:
```
# Codex Independent Review — Boona Tweet → RIMA Map Composition

## H1-H8 verdicts
For each hypothesis: ACCEPT / MODIFY / REJECT + 1-2 sentence reasoning.
Be opinionated — don't hedge.

## Proposed v15d numbers (concrete)
- Negative space %: X (range or absolute)
- Dominant floor cell %: X / secondary %: Y / accent %: Z
- Hero prop cluster cap per room: N
- Palette cap (distinct colors per zone): N
- Path cells minimum: N or X% of room area

## v15d code change scope (high-level, no implementation)
What field/method changes go where. 3-5 bullet points max. Do NOT write code — just point to files + describe.

## What you'd push BACK on vs my analysis
Things in boona_analysis_package.md you think are wrong or overstated.

## Confidence
For each verdict: HIGH / MEDIUM / LOW confidence + why.
```

Keep total output ≤ 500 lines. Be direct, opinionated, evidence-based. The orchestrator (Opus) will synthesize Codex+Gemini+self into a final decision.

## What NOT to do
- No code edits
- No new ScriptableObject design (just point to where to extend)
- No tests
- No imagegen
- Don't repeat my analysis — give your independent take
