---
name: PixelLab sprite production workflow
type: project
trigger: CFR v3, sprite production, char_id, rotation, skin tone, hades, direction
description: CFR v3 rotation workflow, char_id tracking, 128px readability warning, Hades 4-dir system
---

## CFR v3 Workflow (all 10 classes)
1. PixelLab UI -> Create from Reference v3
2. Upload <class>_anchor.png (Characters/anchors/ -- south-facing idle)
3. Camera: Low Top-Down. Type: Humanoid > Pro
4. Paste description from STAGING/PROMPTS_S43/PIXELLAB_CFR_V3_PROMPTS.md
5. Generate v3 Rotation (4 dirs: S/E/N/W)
6. Save char_id -> record in CURRENT_STATUS.md Anchor Status table
7. MCP animation uses char_id -- NOT the anchor PNG directly

char_id is the persistent character identity in PixelLab.
All future animation frames (idle, walk, attack) must use the same char_id for consistency.

## Dark Skin Warning (128px)
Do not specify near-black / deep ebony skin in PixelLab prompts at 128px scale.
Near-black skin + dark clothing = unreadable silhouette. Confirmed: 3 Brawler REGEN attempts failed.
Trigger "Black African" also caused mask/monster face output.
Fix: let PixelLab choose readable tone. If dark look needed, pair with high-contrast costume element.

## 4-Direction System (Hades-Style, UPDATED 2026-05-02)

Anchor files in Characters/anchors/<class>/rotations/ are NOW CORRECTLY named by true visual content (renamed 2026-05-02). Old offset is gone.

### Anchor -> Game Direction mapping (for generating new sprites)
| Anchor to use as reference | Game direction slot | Sprite file to save as | Visual content |
|---|---|---|---|
| south.png (full front) | EAST (press right) | <class>_idle_east.png | SE equivalent |
| east.png (right profile) | NORTH (press up) | <class>_idle_north.png | NE equivalent |
| north.png (full back) | WEST (press left) | <class>_idle_west.png | NW equivalent |
| west.png (left profile) | SOUTH (press down) | <class>_idle_south.png | SW equivalent |

### Unity .anim -> sprite mapping (current wave-1 classes)
- idle_east.anim -> _idle_south.png sprite (front view)
- idle_north.anim -> _idle_west.png sprite (right profile)
- idle_west.anim -> _idle_north.png sprite (back view)
- idle_south.anim -> _idle_east.png sprite (left profile)

### Rules
- Generate 4 directions using the correct anchor as reference per above table
- Sprite slot names (_east/_north/_west/_south) are animation slot names, NOT visual content names
- West direction: generate separately (back view has no mirror relationship to east)
- See STAGING/PRODUCTION_GUIDE_S43.md for full spec

## PERMANENT RULE (2026-05-02)

**animate_character MCP is FORBIDDEN for all character animations.**
**char_ids intentionally removed. Do NOT look them up or use them.**

Reason: MCP output quality unacceptable -- 4-frame limit, embedded VFX in sprite frames, running animation poor.
All character animations are produced manually by the user in the PixelLab UI.
Claude prepares prompt documents only. Never calls animate_character.

## Claude role for character animations
- Prepare action description prompts as documents (dispatched via Codex)
- User executes in PixelLab UI "Animate with Text NEW"
- Target: 8-16 frames, NO embedded VFX in sprite frames, clean silhouette

## Character Description Prompt Structure (sjalsol template — FULL)

Use for ANY PixelLab character description (create_character, CFR v3, reference-based gen).
Fill only relevant fields; skip fields that don't apply.

```
TYPE: [humanoid / creature / etc]
STYLE: [art style, size, feel]
HEAD: [head shape, eyes, distinctive features]
BODY: [body type, stance]
LIMBS: [arm/leg description]
EXTRA: [accessories, gear]
CLOTHING: [outfit details]
HANDS: [what hands hold / look like]
SILHOUETTE: [key silhouette identifiers]
COLOR: [palette, skin, clothing colors, shade steps]
POSE: [for walk/attack frames -- specific pose description]
```

For EXISTING char_id animations: do NOT re-describe the character.
Just provide: action_description + directions + frame_count. That's it.

RULES block (reference-based generation only):
- Use provided sprite as scale/grid/pixel density reference ONLY
- Do not redesign or enlarge
- Clean pixel clusters, no dithering, no noise
