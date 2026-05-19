ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.

# Codex Task — Spawn_01 Concept v4 "Inside the Dungeon" (image_gen)

ACTIVE RULES: (1) think before generating (2) min code, no speculation (3) surgical — listed output only (4) BLOCKED if unclear.

## Mission

Use built-in `image_gen` (gpt-image-1) to draw concept v4. Same fake-isometric technique from v3 (35° tilt, walls with depth, 4-dir compatible) BUT user feedback on v3:

> "şu an boş arena yukarıdan gibi hissediyorum, ben dungeon İÇİNDE gibi hissetmek istiyorum. Maps biraz daha büyük olacak. Ara duvarları (internal walls) göreceğim, arena perimeter duvarlarını sadece map limitine gidince göreceğim. Baklava dilimi (diamond shape) zorunlu değil."

Single regen. No code, no analysis.

## References

- v3 (CLOSEST candidate but feels arena-like): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png` — diamond walkable shape, ALL walls visible because room is small + arena-bounded
- v2 (rejected, 0° Y rotation flat walls): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png`
- v1 (rejected, 50° tilted): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png`

v4 hedef: **fake iso BUT zoom-out + internal architecture + edge-only perimeter walls.**

## Output

`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`

## Prompt for image_gen

```
2D pixel art, fake-isometric dungeon view in the style of classic Diablo dungeon levels, Dead Cells biomes, and Octopath Traveler underground sections. Camera tilted at 35 degrees high top-down, NO Y-axis rotation (top/bottom walls horizontal in screen space, left/right walls vertical). Walls drawn with isometric depth showing top cap + front face.

CRITICAL framing — this is the most important part of the brief:
- This is NOT a closed arena room. The player is INSIDE a larger dungeon and the camera frames a SECTION of it, not the whole footprint.
- The visible map area should be roughly TWICE the size of v3 (think 32 wide x 22 tall world tiles instead of 18 wide x 12 tall).
- PERIMETER walls (outer boundary of the level) are mostly OFF-SCREEN or visible only at one corner/edge of the frame. The player feels they could walk in multiple directions and find more dungeon.
- INTERNAL walls are the primary visual element. Show 2-4 internal wall structures (interior pillars, dividing walls, sub-room partitions, broken collapsed sections, archway connectors between sub-rooms). These create gameplay obstacles and sight-line variety.
- The walkable area should NOT be a diamond/lozenge silhouette. Irregular layout is preferred — some open spaces, some tight corridors, some pillared interior sections.

Structural elements to include:
- 2-3 INTERIOR free-standing pillars or wall stubs (rectangular block sections, isometric depth visible)
- 1 archway connecting current section to a hint of another section visible beyond (dark passage with a cyan rift glow at the far end suggesting another room)
- 1 broken/collapsed wall section (rubble pile spilling onto the floor, showing the dungeon has decayed)
- 1 corner of the perimeter outer wall visible at one edge of the frame (the player can tell they are near the map edge in that direction but plenty of dungeon extends the other ways)
- Banners hanging from interior walls (purple, torn)
- Hanging chains (vertical, going up into off-screen ceiling darkness)
- Wall-mounted warm candle sconces (orange flame) scattered along interior walls
- Cyan rift cracks (#00FFCC) on 2-3 wall surfaces — interior walls

Floor:
- Paved stone tiles (gray-blue, mossy)
- Larger and more varied — different floor sections separated by elevation hints (a single step down between sub-sections, or a different tile pattern marking "this is a different sub-room")
- Scattered details: pebbles, rubble, bones, dropped weapons, broken pottery, moss patches
- Some sections darker than others (lighting variation suggesting different rooms)

Foreground (centered): male warblade with dark hair, ragged armor, slashing east at an imp enemy (crow-skull-faced cyan-glowing creature). A female elementalist with blonde bun + blue robe casting a purple-cyan spell. The combat scene is the focal point but does NOT dominate the entire frame — there is plenty of dungeon visible around them.

Lighting: heavy vignette with combat sparks bright at center. Interior darker than v3 (this is deep dungeon, not entry hall). Cyan rift glows + warm candle accents are the only sources of color. Atmosphere: oppressive, ancient, MUCH BIGGER than v3, claustrophobic but not arena-bounded.

Palette: dark slate gray-blue stone (#3a4050), cyan #00FFCC accent, warm orange candle accent, moss green patches, blood-red banner remnants, warblade brown leather, elementalist blue robe.

Style: pixel art with painterly polish. Diablo 2 / Dead Cells / Octopath dungeon vibe — but at 35-degree fake iso angle with grid-aligned 4-dir character compatibility.

CRITICAL anti-patterns to avoid:
- Do NOT draw the whole room with all 4 perimeter walls visible (that's arena feel — v3 problem)
- Do NOT make the diamond/lozenge silhouette dominant (allow irregular layouts)
- Do NOT center the combat scene with empty floor padding (combat is central but environment is rich around it)
- Do NOT make all walls the same height/style (vary internal walls from perimeter walls — interior ones can be lower stubs or half-height pillars)

Camera: high top-down at 35 degrees from horizontal. Frame zooms out to show ~32x22 world tiles. Combat scene occupies center ~10x7 tiles of that. Surrounding ~75% of the frame shows dungeon architecture, internal walls, sub-sections.
```

## Acceptance

- Single PNG at `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`
- Scene shows MORE dungeon than v3 (zoomed out, less arena-bounded)
- Internal walls / pillars / sub-sections as primary architecture
- Perimeter outer walls visible only at one edge OR one corner OR off-screen entirely
- NOT a diamond/lozenge shape — irregular layout
- Same 35° fake-iso angle as v3
- Same characters + same combat moment

## Done report

Append one line to `STAGING/CODEX_DONE_concept_v4_inside_dungeon.md`:
- "Generated: Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png at <size>, image_gen cost: $0"

If image_gen produces wrong framing (whole arena with all 4 perimeter walls, OR diamond shape, OR no internal walls), retry up to 2 times with emphasis on the off-screen perimeter + internal architecture.

## Hard rules

- DO use `image_gen` (built-in gpt-image-1)
- DO read v1+v2+v3 first
- DO NOT modify any code/scene/.meta
- DO NOT generate analysis docs


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.