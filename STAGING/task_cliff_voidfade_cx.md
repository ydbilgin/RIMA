# CX TASK #3 — Cliff "spike forest" fix: void-fade bottom (research-backed)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

STANDING: tolerate domain-reload MCP "disposed" errors — after any recompile, poll `editor_state.isCompiling` (and CompilationPipeline) until false, then reconnect/retry the MCP call. Not a failure. Unity is OPEN, scene `_IsoGame.unity`.

## Amaç (purpose)
Cliff vertical ALIGNMENT is already fixed (transformOffset.y=-0.25, anchor 0.5,0, heightVariation off — DONE, do not touch). The remaining problem is the LOOK: the 3u-tall `DirectionalCliffTile` cliff sprites render as a tall, RAGGED "spike forest" curtain on the staircased iso edge. Make them read as a CLEAN, cohesive DARK cliff band that dissolves into the void — the Hades / Children of Morta technique.

## Research finding (Gemini 3.1 Pro, validated by Opus — implement the SHADER option)
The dominant problem is the bottom SILHOUETTE, not color. Fix = fade the LOWER portion of each cliff sprite to a solid dark void color (no alpha) so the ragged bottom edge melts into the dark background and the staircase silhouette disappears. Lowest-effort, no art rework = a per-sprite UV-Y gradient fade in the cliff TilemapRenderer material.

## Implementation
1. Create a URP 2D sprite shader (or Shader Graph) `Assets/Shaders/CliffVoidFade.shader` (namespace-free, URP 2D compatible — match the project's URP setup; the TilemapRenderer uses Sprite-Lit-Default normally, so make a Sprite-Lit variant OR an unlit variant if 2D lights aren't needed on cliffs — check what the floor/cliff currently use). The shader fades fragment color to a solid void color in the LOWER part of each sprite:
   - Use the sprite's own UV.y (per-tile 0..1 within the sprite rect) so EACH cliff fades its own bottom uniformly (this is why the staircase vanishes). `fade = smoothstep(0.55, 0.05, uv.y)` (top stays detailed, bottom ~half → void). 
   - `voidColor` = a serialized `_VoidColor` property, default `#1A1A24` (RGB 0.10,0.10,0.14). `c.rgb = lerp(c.rgb, _VoidColor, fade); c.a = max(c.a, fade*?)` — keep it OPAQUE at the bottom (no alpha fade — solid color melts into the void plane; do not fade to transparent or the void cracks behind will show through and re-create a silhouette).
   - Expose `_FadeStart`/`_FadeEnd` float props so the fade band is tunable.
2. Create material `Assets/Materials/CliffVoidFade.mat` using that shader, default `_VoidColor` matching the scene void. CHECK the actual void background color first: look at `Void_BG` / `bg_L0_void` sprite or the camera background in `_IsoGame`; set `_VoidColor` to match (so cliff bottoms blend into the existing void, not a mismatched band). If void is near-black, #1A1A24 is fine.
3. Assign the material to `IsoGrid/CliffRing/CliffTilemap` TilemapRenderer.material.
4. Keep the existing cliff sprites, pivot, transformOffset (-0.25), anchor (0.5,0), heightVariation off. Do NOT change placement/scale.

## Verify (REQUIRED — visual)
- Enter Play, frame the cliff/floor edge, capture `capture_source=game_view` (LIT). Confirm: cliff TOP stays detailed + flush to floor edge; cliff BOTTOM dissolves into the void (no ragged spike silhouette). It should read as a clean dark band, not a forest of spikes.
- If the fade band is wrong (too much detail cut, or silhouette still visible), tune `_FadeStart/_FadeEnd` / smoothstep range and re-shoot until clean. Capture before/after paths.
- `read_console` 0 errors. `dotnet build` or Unity recompile clean. Save `_IsoGame.unity` + assets.

## Done criteria
- `CliffVoidFade.shader` + `.mat` created, assigned to CliffTilemap, compiles clean, scene saved.
- Lit game_view screenshot shows clean dark cliff band (no spike-forest silhouette). Report screenshot paths.
- Report files touched + line ranges. Write to CODEX_DONE.md. BLOCKED + question if ambiguous. NO commit (gated). Identity ydbilgin, no Claude trailer.
