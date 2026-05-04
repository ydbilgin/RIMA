# RIMA S43 Animation Template — animate_character

## Parameters (fixed for all skill animations)

| Param | Value |
|---|---|
| endpoint | MCP `animate_character` |
| mode | v3 |
| frame_count | 8 |
| directions | ["south", "east", "north", "west"] |
| ai_freedom | LOW (200) |
| confirm_cost | true |

Direction mapping (Low Top-Down, matches anchor labels):
- south = front-facing toward camera
- east  = right side profile
- north = back-facing away from camera
- west  = left side profile

## action_description Template

```
FOOTPRINT LOCK: identical pixel extents (top, bottom, left, right) across all frames.
ANCHOR: feet aligned to same pixel row, head height constant throughout animation.
CONTINUITY: same character design, weapon, armor, palette -- same character rotated, not redesigned.
ACTION: {action_phrase}
```

## Warblade Skill Queue

**NOTE: char_id removed. User animates manually in PixelLab UI. Claude prepares prompts only.**

| # | Skill | action_phrase | Status |
|---|---|---|---|
| 1 | iron_charge | heavy armored shoulder charge with ground tremor impact and weapon collision | pending |
| 2 | sunder_mark | precise forward thrust into enemy armor creating visible fissure crack | pending |
| 3 | crippling_blow | heavy overhead weapon slam into enemy's flank with deliberate impact | pending |
| 4 | deep_wound | controlled raking slash leaving precise wound trail across target | pending |
| 5 | earthsplitter | weapon drives into ground erupting upward in massive knockup | pending |
| 6 | gravity_cleave | both hands grip overhead slam into ground creating inward-dragging shockwave | pending |
| 7 | iron_counter | planted armor brace pivots to explosive counter-slam erupting force outward | pending |
| 8 | iron_crush | character shifts into low threatening stance anchoring weight with weapon forward | pending |
| 9 | ironclad_momentum | character drops into guard stance with weapon held horizontally across body | pending |
| 10 | death_blow | overwhelming downward strike with all Rage channeled into catastrophic weapon detonation | pending |
| 11 | battle_surge | character drives weapon pommel into ground igniting armor seams in surge stance | pending |
