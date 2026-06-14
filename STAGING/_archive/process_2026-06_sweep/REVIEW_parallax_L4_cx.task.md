ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical - only the listed/own files (4) write BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

Amaç: Review the just-implemented "parallax_L4" uncommitted changes for correctness bugs, RIMA convention violations, and the parallax authoring design focus below. This is a READ-ONLY review — do NOT modify any code.

## Step 1 — Inspect the changes
Run this to see the uncommitted diff:

```
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" diff -- Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs Assets/Scripts/Background/ParallaxLayer.cs
```

The two changed files:
- `Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs` (editor-only, inside Editor asmdef)
- `Assets/Scripts/Background/ParallaxLayer.cs` (runtime)

## Step 2 — Review focus
Check for correctness bugs + RIMA convention violations, with this design focus on parallax authoring correctness:

(a) The toggle maps: stick-to-camera -> factor 0 / stick-to-map -> factor 1.0 / custom in between.
(b) It writes the EXISTING factor field (no duplicate / parallel data model introduced).
(c) The canonical 0.05-1.10 range is honored (camera-lock may legitimately clamp to 0).
(d) Editor-only code stays in the Editor asmdef (no runtime leakage of editor types/usings).
(e) No scene-breaking (no serialized-field rename/removal without migration, no broken references).
(f) RIMA conventions (PPU 64, paths, naming, surgical change, min code).

## Step 3 — Output format
Output a concise findings list, one finding per line:
`severity / file:line / issue / fix`
(severity = BLOCKER | MAJOR | MINOR | NIT)

End with exactly one final line:
`STATUS: PASS`  (no blocking/major issues)
or
`STATUS: CONCERNS`  (one or more issues that should be addressed)

## Step 4 — Write result
Write your full review (the findings list + the final STATUS line) to a file named CODEX_DONE in the repo root:
`F:/Antigravity Projeler/2d roguelite/RIMA/CODEX_DONE`
Also print the same review to stdout.
