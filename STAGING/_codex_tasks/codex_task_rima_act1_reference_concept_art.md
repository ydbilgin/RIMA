# Codex Task — RIMA Act 1 In-Game Reference Concept Art (imagegen built-in)

ACTIVE RULES: (1) think before generating (2) honest verdict (3) BLOCKED if context unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## CRITICAL MODE INSTRUCTION

Use **built-in `image_gen` tool** (default mode per imagegen SKILL.md). Does NOT require OPENAI_API_KEY. DO NOT fall back to CLI `scripts/image_gen.py` — that requires API key not in env.

## Mission

Generate **ONE reference concept image** showing what RIMA should look like in-game during Act 1 Combat — playable feel, not a marketing shot. Used for art direction reference (Karar #143 6-layer pipeline visual target).

## Step 1 — Pull NLM context (REQUIRED before generation)

Run these NLM queries to gather canonical context:

```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Act 1 Shattered Keep / Sunken Keep visual theme — palette, materials, mood, environmental storytelling. What does the floor, walls, gates look like in canon?"

uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Warblade visual identity — outfit, weapon, silhouette. How does he stand in combat? Karar #100 angle 35°"

uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Elementalist visual identity — outfit, casting pose, element. Female mage. Cool palette match for Act 1"

uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Act 1 mob roster — Imp, Hulk, Reaper, Brute. Visual identity, first-encounter mob design."
```

## Step 2 — Inspect existing character sprites for style reference

Character sprites already exist in repo:
- Warblade south: `Assets/Art/Characters/Warblade/Rotations/warblade_south.png` (120x120, dark-haired, dark armor)
- Elementalist anchor PixelLab ID: `4c83c0be-e856-48f1-b8b5-9626e041a082` (you can fetch via `mcp__pixellab__get_character` if you want to inspect)
- Mob sprites: check `Assets/Resources/Mobs/` or `Assets/Sprites/Mobs/` if present — pick one to reference

Use these as **style anchors** for the concept art (palette, silhouette, scale).

## Step 3 — Generate ONE concept image

Use built-in `image_gen` with this prompt:

```
A wide top-down view in-game screenshot reference for a 2D action roguelike called RIMA, Act 1 "Shattered Keep" combat room.

CAMERA: 30-35 degree angled top-down (Hades-like), wide horizontal framing showing a single room with two stone arch gates left and right.

ENVIRONMENT:
- Cool weathered granite floor (muted gray #3A3D42 with blue-violet undertones), flat plain ground with minimal cracks, looks like worn temple foundation
- Stone pebble walls top and bottom (perimeter), ancient cool gray stone block construction with subtle moss at base
- Two stone arch gates: one left side, one right side, granite material matching walls, dark void inside arch hinting at corridor beyond
- L4 organic overlay patches (NOT on whole floor): moss cluster near each gate, debris near wall corners, center 6x4 region completely CLEAR for combat
- L5 scatter: small stones clustered near gates, occasional debris at wall bases
- Floor has DARK MUTED value — Yudou value isolation rule (floor low value, characters high contrast)

CHARACTERS (chunky 32px-scale pixel art, painterly hand-drawn):
- Warblade in center-left: dark-haired male warrior in dark cool blue armor, holding a sword in fighting stance, 35-degree facing
- Elementalist in center-right: female mage in cool blue robe with violet trim, hands extended casting a glowing cyan/violet ice or rift orb, 35-degree facing
- One Imp mob in foreground-right: small dark grotesque creature mid-charge toward the players, claws out, dark cool palette

ACTION READING:
- Warblade just landed beat 2 of melee combo (slight motion lines, orange Fracture spark VFX on Imp)
- Elementalist mid-cast (cyan particle aura forming in front of hands)
- Imp mid-leap (motion blur trail subtle)
- White core impact VFX (Yudou rule: VFX has near-white core, dark/transparent edges) at point Warblade hit Imp

STYLE:
- Painterly hand-drawn 32px-scale pixel art (Hades reference, NOT smooth modern pixel)
- Cool muted palette dominant (#3A3D42 floor, character pops with brighter saturation)
- High contrast between dark floor and characters (Yudou value isolation)
- NO border around image, NO frame, NO HUD, NO text, NO watermark
- Top-down 30-35 degree angle perfectly consistent across floor, walls, gates, characters
- Hades-style atmospheric lighting (dramatic but not too dark)

REFERENCE: think of Hades combat scene but in a cool stone keep instead of Underworld.

Output: 1536x1024 PNG (wide landscape) showing the full combat room scene.

Negative: smooth modern AA pixel art, bright fantasy colors, isometric pure 45 degree, side scroll, top-down pure 90 degree without angle, dungeon brick grid floor, repeating tile pattern, mosaic, cobblestone individual stones with gaps, UI HUD overlay, dialogue box, character name labels, watermark, text, logo, signature, frame, border around image
```

## Step 4 — Save output

1. Built-in image_gen saves to `$CODEX_HOME/generated_images/...`
2. Copy/move final PNG to: `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png`
3. Also copy a smaller preview to `STAGING/RIMA_Act1_concept_preview.png` (downscale to 800x533 for quick view)

## Step 5 — Honest visual inspection

Inspect output and report:
- Floor flat + plain + no visible grid? YES/NO
- Characters readable at chunky pixel scale? YES/NO
- 35° camera angle consistent across floor/walls/characters? YES/NO
- Cool muted palette respected? YES/NO
- L4 overlay placement semantic (gates + walls + clear center)? YES/NO
- Hades-feel atmospheric? YES/NO

If 4+ NOs, iterate ONCE with prompt fix. Max 2 built-in calls total.

## Required output

`STAGING/CODEX_DONE_rima_act1_concept.md`:

```
# STATUS
[LIVE / PARTIAL / BLOCKED]

# Generated file
[absolute path]

# NLM context used
[3-5 line summary of what NLM returned for Act 1 + Warblade + Elementalist + Imp]

# Visual inspection
[6 YES/NO from Step 5]

# Iteration count
[1 or 2 built-in image_gen calls]

# Critique (honest)
[What works, what doesn't, biggest gap from target]

# Next-step recommendation
[If passable: how to use as art bible reference. If iteration needed: what to fix next.]
```

Effort: medium. ~15-20 min. Single concept reference. Quality > speed.

## Hard rules

- USE built-in `image_gen` tool (no CLI fallback, no OPENAI_API_KEY usage)
- DO NOT generate game assets (this is REFERENCE art, not a tile/sprite)
- DO query NLM first for canonical context
- Single image output, not a batch
- 1536x1024 landscape recommended (wide combat scene)
- Honest verdict — if image is bad, say so
