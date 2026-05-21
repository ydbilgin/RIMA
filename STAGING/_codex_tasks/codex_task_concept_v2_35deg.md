# Codex Task — Spawn_01 Concept Redraw at 35° (image_gen)

ACTIVE RULES: (1) think before generating (2) min code, no speculation (3) surgical — listed output only (4) BLOCKED if unclear.

## Mission

Use the built-in `image_gen` tool (gpt-image-1 backend) to redraw the existing concept art at **35° angled top-down** (Karar #100 lock) instead of the current ~50-60° isometric. Single regen. No code, no analysis, no extra files beyond the image.

This is a yapısal alignment check — we need to see if the dungeon vision from `RIMA_Act1_Spawn01_concept_v1.png` survives at our locked camera angle of 35°. If it does, the existing wall sprite class is wrong (flat 2D, no depth); if it doesn't, we may need to revisit Karar #100.

## Reference

- Existing v1 concept (~50-60° isometric): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` — read it, study composition, do NOT just describe; carry the same scene/mood/elements

## Output

- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png` (write the generated image to this path)

Nothing else. No README, no comparison doc, no analysis.

## Prompt for image_gen

Pass the prompt below to `image_gen`. Use the same parameters that produced v1 (size, quality, style — check git log of the v1 generation if needed; default to size=1536×1024 or matching v1 dimensions if discoverable, quality=high).

```
2D pixel art, Hades-style high top-down view at exactly 35-degree camera angle (NOT 50-60 degree isometric, NOT pure 90 top-down). The angle should feel like Hades, Children of Morta, Death's Door — diagonal-ish but flatter than Diablo isometric.

Scene: a small dark dungeon spawn room, roughly 18x12 world tiles, with paved stone floor (gray-blue tint, mossy patches, scuff marks, scattered pebbles and rubble). The walls form a closed rectangle with two arched stone gateways on the LEFT and RIGHT sides (mid-height of each wall). Top and bottom walls are unbroken courses of stacked granite masonry.

Wall structure: at 35-degree angle the walls should read as 3-tier deep masonry — top edge with a 1-pixel saturated highlight (cool moonlight rim), middle body of mossy stacked brick, base with a soft shadow gradient bleeding onto the floor. Hanging metal chains, draped torn banners, cracks running down the wall surfaces, occasional small candle sconces (warm orange flame) on wall faces. Two or three cyan rift glows (#00FFCC, glowing crystalline cracks in the stone) at strategic wall positions — one inside each gateway, one or two on the walls themselves.

Foreground characters: a male warblade with dark hair and ragged armor in a slash stance with sparks flying, an elementalist female with blonde bun and blue robe casting a purple-cyan spell behind him, a small imp enemy (crow-skull-faced, cyan glow on its body) lunging at the warblade. All three at the center of the room.

Lighting: heavy vignette — center bright with combat sparks, edges fall into near-black. Cyan rift glows + a few warm candle accents. Atmosphere: oppressive, ancient, mossy, broken. NOT cheerful, NOT bright.

Palette: dark slate gray-blue stone, cyan #00FFCC accent (rift), warm orange candle accent (sparse), moss green patches, warbade brown leather, elementalist blue robe.

Style: pixel art with painterly polish (Hades / Octopath Traveler dungeon vibe at 35-degree angle). High detail per tile. Avoid flat tile-grid appearance — make the floor and walls feel hand-placed, not stamped from a 32px repeat.

CRITICAL: camera angle is 35 degrees from horizontal, NOT isometric (45+). The walls should appear MORE HORIZONTAL than the v1 reference — top wall reads as a thinner band across the top, side walls read as tall vertical-ish bands with foreshortening. Floor should dominate ~70% of the frame vertically.
```

## Acceptance

- Single PNG at `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png`
- Same scene elements as v1 (Warblade + Elementalist + Imp + 2 gates + cyan rifts + moss + chains + candles)
- DIFFERENT camera angle — visibly flatter/more horizontal than v1
- File written, no other outputs

## Done report

Append a single line to `STAGING/CODEX_DONE_concept_v2_35deg.md`:
- "Generated: Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png at <size>, image_gen cost: $0"

If image_gen fails or produces wrong angle, write BLOCKED with the gpt-image-1 error message. Do not retry beyond 2 attempts.

## Hard rules

- DO use `image_gen` (built-in gpt-image-1), NOT PixelLab MCP (different backend, different cost)
- DO read v1 first to match scene elements / characters / mood
- DO NOT modify any code, scene, or .meta files
- DO NOT generate analysis, comparison docs, or additional images
- DO NOT touch other reference images in `Assets/Art/Reference/`
