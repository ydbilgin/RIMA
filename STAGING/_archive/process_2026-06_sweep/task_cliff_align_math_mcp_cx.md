# CX TASK #2 — Cliff alignment (mathematically correct) + UnityMCP reload robustness

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

STANDING DIRECTIVE (user, this session): UnityMCP keeps disconnecting on domain reload ("Cannot access a disposed object"). EVERY task must (a) tolerate it — after any recompile, poll `editor_state.isCompiling` until false, then reconnect/retry the MCP call instead of reporting failure; and (b) where this task touches it (Part B), actually HARDEN the reconnect. Unity is OPEN. Do NOT treat a post-reload disposed-object error as a real failure.

Run this ONLY AFTER cx task #1 (`STAGING/task_iso_paint_cliff_zoom_cx.md`) is fully done + scene saved. Both touch `_IsoGame` cliffs — sequential, not parallel.

---

## PART A — Cliff vertical alignment, mathematically correct (user: "cliffler kaymış, eşitlemedi")
Opus aligned `CliffTilemap.tileAnchor` to `(0.5,0,0)` (matches Ground) — that fixed the half-cell float but the cliff `transformOffset.y` is still wrong, leaving a gap below the floor edge.

**Geometry (measured by Opus, verify before trusting):**
- Grid `cellSize = (0.96, 0.585, 1)` Isometric → iso diamond half-height = `cellSize.y/2 = 0.2925`.
- Floor sprite `floor451_0`: 64x64px @ppu64 = 1x1 world u, pivot CENTER (0.5,0.5), tileAnchor (0.5,0).
- Cliff sprite `cliff_*`: 128x192px @ppu64 = 2x3 world u, pivot TOP-CENTER (0.5,1.0), tileAnchor (0.5,0).
- Both tilemaps share `IsoGrid` and now share tileAnchor (0.5,0) → the floor sprite CENTER and the cliff sprite TOP land at the SAME cell anchor point (before transformOffset).
- Floor diamond's front (camera-facing, lower) vertex is `0.2925` BELOW the floor center.

**Derivation:** For the cliff TOP edge to meet the floor's front diamond edge, `transformOffset.y` must = `-(cellSize.y/2)` = **−0.2925**. Currently it is **−0.47** (≈0.18u too low → visible gap). To TUCK the cliff top slightly under the floor (cliff renders behind, sortLayer Ground order −50, so a small tuck hides the seam), a touch higher is better: **−0.25** (range −0.29 .. −0.20).

**DO — empirically verify, don't trust the number blind:**
1. With Unity in EDIT mode on `_IsoGame`, for one south-facing cliff cell, compute the ACTUAL world Y of (i) the cliff sprite top (pivot) and (ii) the floor diamond front vertex, using the tile transform + sprite pivot. Confirm the current gap ≈ 0.18u (validates the model).
2. Set `DirectionalCliffTile_Hades.transformOffset` to `(0, -0.25, 0)` (keep x=0, keep spriteScale 1,1). `EditorUtility.SetDirty` + `AssetDatabase.SaveAssets()`.
3. `CliffAutoPlacer.Regenerate()` + `CliffTilemap.RefreshAllTiles()`.
4. **VISUAL VERIFY (required):** zoom tight onto a cliff-floor junction and confirm the cliff top sits flush on the floor front edge — NO gap, NO climbing onto the floor top surface. If not flush, adjust transformOffset.y within [-0.29,-0.18] and re-shoot until flush. Capture BOTH: (a) `capture_source=scene_view` AND (b) Play-mode `capture_source=game_view` (LIT) of the same junction — the scene view is unlit and exaggerates the cliff's brown/ref_kit tone; Opus needs the lit game_view to judge final cliff COLOR (should read dark-slate, not brown) + alignment. Report all screenshot paths.
5. Save `_IsoGame.unity`.

DO NOT change spriteScale, cliff art, sprite pivots, or the placement algorithm. Only `transformOffset.y` + the already-applied tileAnchor. Cliff is still 3u tall (deep hang) — leaving that for Opus to judge from your screenshot.

---

## PART B — UnityMCP domain-reload robustness (user priority: "her agent unitymcplerini çözmeye odaklansın")
**Problem:** When a script compiles (e.g. creating CameraZoom.cs), Unity domain-reloads, the MCP-for-Unity bridge drops, editor windows close, and tool calls return "Cannot access a disposed object". This breaks autonomous runs.

**Investigate + propose (and implement if low-risk + surgical):**
1. Find the MCP-for-Unity bridge/connection code in the project (likely under `Packages/` or `Assets/` — search `MCPForUnity`, `McpForUnity`, `disposed`, `IConnectionState`, `[InitializeOnLoad]`, `AssemblyReloadEvents`). Identify WHY the bridge does not auto-reconnect after `AssemblyReloadEvents.afterAssemblyReload`.
2. If the bridge already has reconnect logic, determine why editor windows (Map Designer) close on reload and whether they can persist/restore (EditorWindow state survives reload normally — check if they're being explicitly closed or failing to re-open).
3. Deliverable: write findings + the smallest viable fix to `STAGING/MCP_RELOAD_ROBUSTNESS_FINDINGS.md` (root cause + proposed patch + risk). If the fix is a few lines in a clearly-owned bridge file and obviously safe, apply it and note it; if it touches the installed package or is risky, STOP at the findings doc for Opus review (do NOT edit Packages/ without flagging).
4. Do NOT break the current working MCP connection while investigating.

---

## Done criteria
- Part A: transformOffset.y corrected + flush verified by screenshot; scene saved; before/after screenshot paths reported.
- Part B: `STAGING/MCP_RELOAD_ROBUSTNESS_FINDINGS.md` written; fix applied only if surgical+safe, else flagged.
- `read_console` 0 errors. Report files touched + line ranges. Write to CODEX_DONE.md.
- BLOCKED + question if anything ambiguous. NO commit (gated). Identity ydbilgin, no Claude trailer.
