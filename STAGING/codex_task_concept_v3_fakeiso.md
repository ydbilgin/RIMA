# Codex Task — Spawn_01 Concept v3 Fake Isometric (image_gen)

ACTIVE RULES: (1) think before generating (2) min code, no speculation (3) surgical — listed output only (4) BLOCKED if unclear.

## Mission

Use built-in `image_gen` (gpt-image-1 backend) to draw a third concept variant: **fake isometric** dungeon view. This is NOT real isometric camera rotation — it's the well-known technique used by Stardew Valley, Octopath Traveler, classic SNES RPGs where the grid stays rectangular but wall art shows isometric depth, giving the illusion of diagonal/3D room without breaking 4-direction movement.

Single regen. No code, no analysis, no extra files beyond the image.

## References

- v1 (50-60° isometric, REJECTED): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` — too tilted, breaks 4-dir
- v2 (35° tilt, 0° Y rotation): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png` — facing room, walls flat
- v3 hedef: **35° tilt PRESERVED, but walls drawn with isometric depth + room shape diamond-filled**

Inspirations: Stardew Valley combat caves, Octopath Traveler dungeon rooms, classic top-down RPGs (Secret of Mana / Chrono Trigger / Lufia caves) — the room LOOKS isometric/3D but movement is grid-aligned 4-direction.

## Output

- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png`

Nothing else.

## Prompt for image_gen

```
2D pixel art, fake-isometric dungeon room in the style of Stardew Valley caves, Octopath Traveler dungeons, and classic SNES RPG dungeons (Secret of Mana, Lufia 2, Chrono Trigger). The camera angle is high top-down at 35 degrees BUT the walls are drawn with strong isometric depth showing TWO faces — a top cap AND a front face — to create a 3D illusion. Same level of perspective as Stardew Valley mine levels or the early Octopath caves.

CRITICAL distinctions:
- NOT true isometric (no 45-degree camera rotation around Y axis)
- NOT pure top-down (walls have visible depth, not flat)
- The room shape is DIAMOND-FILLED — the playable floor area has cut corners (the four corner tiles are filled with diagonal wall pieces, not floor). This gives the room an octagonal / lozenge silhouette while the underlying grid remains rectangular. Think of Pokemon Mystery Dungeon room shapes, or Stardew Valley dungeon room layouts.
- Characters face N/S/E/W only (4-direction sprite system — no diagonal poses).

Room layout: rectangular floor area roughly 18 wide x 12 tall in world tiles, but with the 4 corners cut off by 45-degree diagonal wall segments, creating an octagonal silhouette. Top wall (horizontal), bottom wall (horizontal), left wall (vertical), right wall (vertical), and four 45-degree corner walls connecting them.

Walls: drawn with isometric depth showing the wall's top cap (lighter stone with 1px cool highlight at the very top edge) and the wall's front face (darker mossy stacked granite brick). The bottom of every wall casts a soft shadow gradient onto the floor. Hanging chains, draped torn banners, small candle sconces (warm orange flames) on wall faces. Cyan rift cracks (#00FFCC glowing crystalline veins) on 2-3 wall positions.

Two arched stone gateways: one on the left wall, one on the right wall. Each gateway shows depth (you can see INTO the dark passage beyond, with a cyan rift glow inside).

Floor: paved stone tiles (gray-blue), moss patches breaking the grid pattern, scattered pebbles and rubble at wall bases, occasional bones or small debris near corners. The floor should never look like a 32px grid stamp — it should feel hand-placed despite being tileable.

Foreground characters (centered): male warblade with dark hair, ragged armor, in a slash stance with orange sparks flying — facing diagonally (sprite is south-facing but slash direction is east). A female elementalist with blonde bun, blue robe, casting a purple-cyan spell behind him. A small imp enemy (crow-skull-faced, cyan glow) lunging toward the warblade from the east side.

Lighting: heavy vignette — center bright with combat sparks, edges falling into near-black corners. Cyan rift glows + warm candle accents. Atmosphere: oppressive, ancient, mossy, broken.

Palette: dark slate gray-blue stone (#3a4050 base), cyan #00FFCC accent (rift), warm orange candle accent (sparse, ~#FF9966), moss green patches (#4a6b3a), warblade brown leather, elementalist blue robe.

Style: pixel art with painterly polish. The decisive visual cue is that walls show isometric depth (top cap + front face) while the room footprint is diamond-cut at corners — Stardew Valley / Octopath / classic 16-bit RPG dungeon aesthetic, NOT modern angled-Hades aesthetic.

Camera: positioned high and looking down at 35 degrees from horizontal. NO Y-axis rotation — the top and bottom walls remain perfectly horizontal in screen space, the left and right walls remain perfectly vertical. The "isometric feel" comes ENTIRELY from the wall sprite depth and the diamond room shape, NOT from camera rotation.
```

## Acceptance

- Single PNG at `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png`
- Same scene elements as v1/v2 (Warblade + Elementalist + Imp + 2 gates + cyan rifts + moss + chains + candles)
- DIFFERENT room shape than v2 — diamond corners cut at 45° (octagonal silhouette)
- Wall sprites show top cap + front face (isometric depth illusion)
- Top + bottom walls still HORIZONTAL in screen space (no Y rotation)
- Left + right walls still VERTICAL in screen space (no Y rotation)

## Done report

Append one line to `STAGING/CODEX_DONE_concept_v3_fakeiso.md`:
- "Generated: Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png at <size>, image_gen cost: $0"

If image_gen produces wrong shape (full rectangle without diamond corners, OR true isometric rotation), retry up to 2 times with prompt emphasis adjustments. Beyond 2, write BLOCKED.

## Hard rules

- DO use `image_gen` (built-in gpt-image-1)
- DO read v1 + v2 first to match scene elements
- DO NOT modify any code, scene, or .meta files
- DO NOT generate analysis/comparison docs
