# PixelLab — Production Reference
> Claude Code bu dosyayı Kiro görevi hazırlarken veya PixelLab kararı verirken okur.

---

## API

```
Endpoint : https://api.pixellab.ai/mcp
Auth     : Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0
```

**Tier 2 (Pixel Artisan):**
| Limit | Value |
|---|---|
| Monthly images | 5,000 |
| Max canvas | 400×400px |
| Concurrent jobs | 10 |
| Animation tools | Fully unlocked |
| Experimental tools | Available |

---

## Tool Decision Tree

```
New character (player / NPC / enemy)?
  → create_character  →  animate_character for each animation

Animation for existing character?
  Template exists?   →  animate_character (template)
  Custom action?     →  animate_character + action_description

Floor / wall tile?              →  create_topdown_tileset
Platform / sidescroller tile?   →  create_sidescroller_tileset
Decoration (chest, pillar)?     →  create_map_object (no_background: true)
```

---

## RIMA Character Size Guide

| Role | Size | Unity Scale (PPU=48) |
|---|---|---|
| Player | 96px | scale 0.5 (~1 unit) |
| Normal enemy | 64px | scale ~0.67 |
| Elite enemy | 80px | scale ~0.83 |
| Champion / boss | 96px | scale 0.5 |
| MiniBoss | 112px | scale ~0.58 |
| Tileset | 16×16 or 32×32 | Unity Rule Tile |

**Canvas rule:** Character should occupy ~60% of canvas height.
**Unity import:** PPU = canvas size in px. Filter Mode: Point. Compression: None.

---

## create_character — Key Parameters

```
size          : canvas px (96 for player/champ, 64/80 for enemies)
ai_freedom    : 400–800 (400–500 = style lock, 750 = default)
outline       : "selective"
shading       : "medium" or "high"
detail        : "medium" or "high"
```

**Style lock:** First character = reference. All others use `style_image` pointing to its south.png.

---

## animate_character — Key Parameters

```
character_id     : from list_characters or create_character result
animation_name   : English, hyphens only, lowercase  e.g. "walking-8-frames"
direction        : "north" | "south" | "east" | "west" |
                   "north-east" | "north-west" | "south-east" | "south-west"
n_frames         : walk=6-8, attack=4-8, death=7-8, idle=8
action_description: English — most important parameter
style_image      : path to existing frame — USE when regenerating or inconsistent
```

---

## Writing action_description

**Rule: Always English. Three things every description needs:**
1. Character look (class, outfit, weapon)
2. Exact action (motion, not vague)
3. Direction (use direction words below)

| direction param | Write in description |
|---|---|
| `north` | "facing upward / away from camera" |
| `south` | "facing downward / toward camera" |
| `east` | "facing right" |
| `west` | "facing left" |
| `north-east` | "upper-right diagonal" |
| `north-west` | "upper-left diagonal" |
| `south-east` | "lower-right diagonal" |
| `south-west` | "lower-left diagonal" |

**Good vs bad:**
```
BAD:  "attack south-west"
GOOD: "armored warrior swings greatsword toward the lower-left (south-west diagonal),
       attack motion clearly directed lower-left, weapon follows through, no loop"
```

---

## Prompt Templates

### Walk / Run
```
"[character] walking toward [direction-word], steady pace,
weight shifting naturally, feet clearly moving, loop animation"
```

### Melee Attack
```
"[character] attacks toward [direction-word], [weapon] swings from wind-up to follow-through,
attack motion clearly directed [direction-word], no loop"
```

### Death / Fall
```
"[character] staggers and falls backward, body collapsing to the ground,
death sequence, weight and momentum visible, no loop,
final frame: character lying flat on the ground"
```

### Idle / Fight Stance
```
"[character] in combat-ready idle stance facing [direction-word],
slight breathing motion, [weapon] drawn and ready, loop animation"
```

---

## Style Consistency — style_image

**When to use:**
- Regenerating a direction that came out wrong
- Starting a new animation for an established character
- Weapon/costume inconsistent across animations

```python
style_image = "path/to/CharacterName_S.png"  # south = best reference
ai_freedom  = 400
```

---

## animation_name — Cache & Naming Rules

- PixelLab caches by `animation_name` — same name on same character = "already complete" error
- **When re-generating:** add suffix — `death-fall-v2`, `quick-slash-v3`
- **Naming rules:** hyphens only, no spaces, all lowercase
  - Good: `death-fall`, `quick-horizontal-slash`, `cast-projectile`
  - Bad: `attack2`, `anim new`, `test`

---

## Tileset Chaining

Use `base_tile_id` to keep all tilesets in the same visual style:
```
Floor tileset → save its id
Wall tileset  → pass floor id as base_tile_id
Decoration    → pass floor id as base_tile_id
```

---

## QC Protocol — Kiro Applies After Every Direction

```
1. Read frame_000.png immediately after generation
2. Describe: "What is the character doing? Is direction correct?"
3. PASS = action and direction match the intent
4. FAIL = wrong action / wrong direction / black frames / placeholder
5. On FAIL: regenerate with -v2 suffix + more explicit action_description
6. Never save failing frames
7. Write result to Assets/STAGING/DONE.txt
```

---

## Known Issues & Fixes

| Problem | Fix |
|---|---|
| "Already complete" error | Change `animation_name` — add `-v2`, `-v3` |
| Black / empty frames (esp. diagonals) | Custom name + explicit direction in description |
| Wrong direction (SW looks like SE) | Add: `"facing lower-left diagonal, moving toward lower-left"` |
| Wrong action | More specific: `"falls backward, collapses, death, no loop, final frame: lying flat"` |
| Weapon inconsistent across anims | Add `style_image` + describe outfit explicitly |
| Too creative / unstable | Lower `ai_freedom` to 400–500 |

---

## Completion Log Format

Kiro always appends to `Assets/STAGING/DONE.txt`:
```
[DONE-BATCHNAME] Character/animation — N directions | YYYY-MM-DD
[QC-PASS] Character/anim/direction — "character falls backward" | YYYY-MM-DD
[QC-FAIL] Character/anim/direction — "wrong", regenerated as -v2 | YYYY-MM-DD
```
