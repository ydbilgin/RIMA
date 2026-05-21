# Codex Strategic Consult — OVAL Tile Feel + Unity Pipeline (full PixelLab test)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Scope
**NO CODE — strategic design verdict ≤ 500 words.** User asks: how to achieve OVAL/organic visual feel given that PixelLab tiles are SQUARE grid units. Also: should we try FULL PixelLab (drop Codex tiles entirely)?

## Background

Current state:
- v15d composition budget LIVE (negSpace 20% + floor 70/20/10 + cluster cap 3 + path 15% + L8 cap 10)
- v15d Codex gpt-image-1 tiles: 22 sprites + zone wiring + style check PASS (CombatBiome_v15d)
- v15e-A LIVE: mist 67→32, player visible, but purple crystal scatter remains
- PixelLab `create_tiles_pro` v1: 16 variations clean painterly cobblestone + path + dirt + transition (segmentation mode, no hard outlines, smooth) — PROVEN
- PixelLab `create_tiles_pro` v2: refined description, OVER-CORRECTED (some flat blob tiles)
- PixelLab dirt v1 single-description batch: 16 variations brown earth painterly — PROVEN
- Master image reference (688×384) produced via create_image_pro: shown clean composition target
- Codex earlier verdict on transitions: SPLIT (PixelLab organic + Codex hard Wang16) HIGH confidence

User's new strategic asks (3 questions):

### Q1 — OVAL FEEL architecture
Tiles are SQUARE grid units. How do we achieve OVAL/organic compositional feel?
- Zone shape oval (Blueprint Painter LIVE — designer paints organic blob)
- Tile content painterly (LIVE — segmentation mode)
- Tile EDGE between zones: hard pixel grid line OR smooth blend?
- Hard tile-grid visibility breaks ovallık

Options:
- **A**: Wang autotile (create_topdown_tileset corner-based) — smooth edge built-in
- **B**: Square tiles + organic decal overlay layer (decals masked at zone edges)
- **C**: Custom Unity shader (deferred S90+ per earlier verdict, revisit?)
- **D**: Larger tile size (64-128px) so grid is less visible
- **E**: Hybrid — A for biome chains + B for prop edges

Pick one with 3-sentence reasoning.

### Q2 — FULL PIXELLAB feasibility
User wants to test FULL PixelLab pipeline (drop Codex tiles entirely):
- `create_tiles_pro` for foundation
- `create_topdown_tileset` for Wang transitions
- `create_object` for hero props
- `create_character/state` for chars (LIVE)
- `animate_character/object` for anims
- `create_image_pro` for master references

vs current HYBRID (PixelLab chars/mobs/props + Codex gpt-image-1 tiles).

Earlier Codex verdict (transitions review): SPLIT C — PixelLab organic + Codex hard Wang16. User wants to revisit and test FULL PIXELLAB.

Reasons to test FULL:
- 5000 PixelLab budget unlimited room
- Unified workflow (less context switching)
- create_topdown_tileset purpose-built for Wang autotile
- Style consistency (single provider)

Reasons to keep HYBRID:
- Codex gpt-image-1 proven for hard Wang16 (per earlier verdict)
- v15d wiring already done with Codex tiles

Verdict: GO FULL or KEEP HYBRID + 3 sentence reasoning. If GO FULL, what's the migration path?

### Q3 — Unity render approach
Given Q1+Q2 verdicts, what's the Unity rendering approach?
- Standard Unity Tilemap (current via Blueprint Painter + AutoPopulator)
- Custom chunked sprite mesh (per Phase 1.5 architecture proposal)
- Sprite-based scatter (no Tilemap, all SpriteRenderers)
- Shader blending (S90+ deferred)

Phase 1.5 architecture eval (your earlier output `STAGING/CODEX_DONE_ROOM_COMPOSER_LIBRARY_EVAL.md`) proposed chunked visual renderer for L2b/L4/L5/L6. Confirm or revise.

Pick one for IMMEDIATE v15f (this week) + one for Phase 1.5 (future). Justify in 3 sentences.

## Output

Write `STAGING/CODEX_DONE_oval_feel_architecture.md`:
```
# Oval Feel Architecture — Codex Verdict

## Q1 — Oval feel approach
Verdict: A / B / C / D / E
Reasoning: <3 sentences>

## Q2 — Full PixelLab vs Hybrid
Verdict: FULL / HYBRID
Reasoning: <3 sentences>
Migration path if FULL: <if applicable, 3 bullet points>

## Q3 — Unity render approach
v15f immediate: <option>
Phase 1.5 future: <option>
Reasoning: <3 sentences>

## Confidence (per verdict)
Q1: HIGH/MED/LOW
Q2: HIGH/MED/LOW
Q3: HIGH/MED/LOW
```

≤ 500 words total. Opinionated, evidence-based. Opus will synthesize against user constraints.

## What NOT to do
- No code edits
- No new task files
- No image generation
- No long preamble
