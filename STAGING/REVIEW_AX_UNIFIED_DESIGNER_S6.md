ACTIVE RULES: (1) think before reviewing (2) concise (3) findings only, no code/scene edits (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# TASK: DESIGN/UX REVIEW (findings-only) — Unified Designer

**REVIEW ONLY. No code/scene edits, no production. Write a report to your AGY_DONE file.**

## Context
A unified room/level designer was built (Opus, ultracode). Goal (user): consolidate ~12
scattered editor windows into ONE tabbed tool that works on BOTH surfaces (Unity Editor
window + in-game F2 overlay), shares the same RoomData, supports categories
(Floor/Cliff/Object/Portal/Light), a logical Generate-Cliff, a shiftable depth-layer stack
(L1 floor / L2 cliff / preview islands / L3 backdrop), organized asset-pack + room library,
and feels like a real in-game tool. Build is compile-green, all 363 EditMode tests pass.

Read: `STAGING/UNIFIED_DESIGNER_ARCHITECTURE_LOCK_S6.md`, `STAGING/UNIFIED_DESIGNER_TASK_S6.md`,
`STAGING/DETECT_AX_RESULT_S6.md` (your prior detection), and skim
`Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` + `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs`.

## Review focus (design/UX, NOT code mechanics — cx covers code)
1. **Does the tabbed UnifiedMapDesigner match the single-screen UX you recommended** (Library / category tabs / Layers panel / palette + search)? What's the biggest UX gap vs your earlier layout (top-bar Playtest, room thumbnail drawer, MRU, ghost-preview, hotkeys)?
2. **Dual-surface parity:** the Editor window has 7 tabs; the F2 overlay still has its older Floor/Cliff/Prop + Floor/Wall/Decor browser (now also a Generate-Cliff button). Is "good enough parity" acceptable for the demo, or is the divergence a real problem? What's the minimum to make them feel like the same tool?
3. **Categories:** Floor/Cliff/Object/Portal/Light — right set for "place things for game flow"? Anything missing (e.g. Enemy-spawn, Decal)?
4. **Depth stack UX:** the Layers tab currently shows read-only sorting slots. The user wants them SHIFTABLE. What's the simplest UX to let them nudge layer order without breaking the floating-island look?
5. **Clean-room flow:** is the floor-paint -> Generate-Cliff -> place-objects -> light flow discoverable in this UI? What one change most improves "tertemiz oda" authoring?
6. **Prioritize:** give a ranked list of the top 3-5 follow-ups (effort S/M/L) that would most move this toward the user's vision.

## Output
Concise structured report (sections 1-6) to your AGY_DONE file. Opinionated, reference-aware. NO code, NO production.
