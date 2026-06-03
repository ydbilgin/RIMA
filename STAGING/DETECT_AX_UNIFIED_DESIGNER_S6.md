ACTIVE RULES: (1) think before answering (2) concise (3) no code, no file edits (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# TASK: DETECTION / DESIGN-FINDINGS ONLY — Unified Designer UX + organization + references

**FINDINGS-ONLY. DO NOT WRITE OR EDIT CODE OR SCENES. Output = a written report to your AGY_DONE file. No production.**

## Context
RIMA has many overlapping in-editor and in-game level/room authoring tools. The user
wants them consolidated into ONE tool that:
- works on BOTH surfaces — in Unity Editor AND in-game (F2 overlay) — with the SAME data,
- lets the user SELECT a room and EDIT it on either surface,
- feels like a REAL in-game level-editor tool (not a clunky Inspector): wide, good UI/UX, easy to use,
- keeps asset-pack + room library ORGANIZED and tidy,
- builds CLEAN iso floating-island rooms (seamless PixelLab floor + auto cliff under floor +
  placed objects + light + void), layers SEPARATE (no bleed).

Read `STAGING/UNIFIED_DESIGNER_TASK_S6.md` (full spec), `STAGING/ROOMTOOL_UX_SPEC_S6.md`,
`STAGING/ROOMTOOL_IMPROVEMENT_PLAN_S6.md`. Perspective is ISOMETRIC (PM·5 final lock).

## What to research & report (design + UX + reference — NO code)

### 1. REFERENCE TOOLS (how the pros do dual-surface / in-game level editors)
- Name 4-6 concrete reference tools/games with strong, EASY level editors that work
  in-game or feel like an in-game tool (e.g. Townscaper, Super Mario Maker 2, Tiled,
  Hades' room authoring approach, RPG Maker, Dreams, Core/UEFN). For each: the ONE UX
  idea worth stealing for RIMA's iso room tool.
- Specifically: how do the best ones handle TILE PALETTE + OBJECT PLACEMENT + LAYER
  selection + ROOM SELECT/SWITCH in a single uncluttered screen?

### 2. UX LAYOUT for RIMA's unified tool (concrete, describable)
- Propose the on-screen layout that works on BOTH surfaces (Editor window + in-game overlay):
  where the room-library browser goes, where the asset/tile palette goes (floor/wall/cliff/
  decor/object buckets), where layer toggles go, where the canvas is, mode switching,
  brush vs drag vs single-place. Keep it ONE screen, uncluttered, big canvas.
- How to make it feel like a real in-game tool: hotkeys, ghost-preview on hover, drag-place,
  undo, snap-to-iso-grid, minimal modal dialogs.
- How room SELECT+EDIT should flow: browse thumbnails → pick → load into canvas → edit →
  save, identically on both surfaces.

### 3. ORGANIZATION (asset-pack + rooms tidy)
- Recommend how to present + organize the AssetPack in the palette (buckets, search/filter,
  thumbnails) so "everything is organized".
- Recommend room-library organization (naming, folders/tags, duplicate/rename, thumbnails).

### 4. CLEAN-ROOM AUTHORING FLOW
- Describe the ideal step-by-step a user follows to build ONE clean iso floating-island room
  in this tool (floor paint → auto-cliff → place objects → light → done), and what guard-rails
  keep it tidy (layer separation, seamless floor, no gaps, no overlap bleed).

### 5. CONSOLIDATION OPINION
- Given there are many scattered windows (MapDesigner Brush / Visual Map Editor / Blueprint /
  AssetPackBrowser / RoomPainter / LiveTool), give your design opinion on what the ONE tool's
  identity should be, and what to drop. (cx covers the code side; you cover the UX/design side.)

## Output format
Write a concise structured report (sections 1-5) to your AGY_DONE file. Reference-driven,
opinionated, concrete. NO code, NO file edits, NO asset production.
