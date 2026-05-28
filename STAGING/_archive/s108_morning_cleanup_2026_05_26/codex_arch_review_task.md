# Codex Architecture Review — RIMA Room Generation Pipeline

## Amaç
Independent technical/Unity-engineering review of three room generation architectures. Complement Opus design judgment. Decision drives Phase 2 production in ~3 hours.

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Context (read-only, do not modify)
- Custom RIMA pixel art Flux LoRA training in progress (335 images, ETA 2-4h)
- RIMA = top-down roguelite, 64px chibi characters, 32x32 tiles
- URP 2D Renderer + Pixel Perfect Camera
- Existing tool: `Assets/Editor/.../RimaWorldPainterWindow.cs` (primary composition tool)
- Solo dev, prefers minimum engineering

## Three options to evaluate (CODE/UNITY perspective only — design covered by Opus)

### A) Modular asset pack
Generate wall pieces, floor tiles, decor objects separately. Unity uses Tilemap + sprite overlays.

### B) Full room paintings
Generate complete rooms 1024-2048px. Use as single sprite + Polygon Collider 2D + spawn point transforms.

### C) Hybrid: template + decor
5-8 room template paintings + decor object library. Template sprite as base + decor sprites overlaid via spawn points.

## Your task — Unity/code review

For each option, evaluate:
1. **Unity integration complexity** (LOC estimate, new systems needed)
2. **Compatibility with existing RimaWorldPainterWindow** (extend? replace? bypass?)
3. **Procgen architecture** (room selection, encounter spawning)
4. **Performance** (sprite count, draw calls, collider complexity)
5. **Implementation time estimate** for solo dev
6. **Iteration friendliness** (how easy to add new rooms/assets)

## Output

Write report to: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/codex_arch_review.md`

Structure:
```
# Codex Architecture Review

## Verdict
Choice: A/B/C
Confidence: low/med/high
Rationale (3 bullets, code-perspective):
- ...

## Per-option analysis
### A) Modular
[criteria 1-6 above]
### B) Full rooms
[criteria 1-6]
### C) Hybrid
[criteria 1-6]

## Implementation outline (chosen)
Step-by-step Unity code changes needed.

## Code-perspective risks
What could go wrong from engineering side.
```

Concrete, ~400-500 words. No code in this report — just architecture analysis.

## Constraints
- NLM ACCESS: query RIMA design context via `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"` if needed
- Read RimaWorldPainterWindow.cs briefly to understand existing tool
- Read CURRENT_STATUS.md for project state
- Do NOT write code, do NOT modify Unity scenes/prefabs
- Output the report file only
