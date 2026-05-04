---
name: PixelLab community-validated pipeline workflows
description: End-to-end pipeline patterns validated by PixelLab community (rorriM, Sjalsol, Ultimatefrisbie1, snowli_on)
type: reference
---

## rorriM's End-to-End Automated Pipeline (HIGH confidence)
Stack: Claude + PixelLab MCP/REST API
- Layering + anchor frames/pixels for rigging animation logic
- Fully automated, no manual rigging, never opened Aseprite
- Key: `character_id` field is the backbone for multi-directional consistency
  (character_id is API-only -- NOT exposed in web UI)
- Generate one direction fully, then use rotation feature to derive other directions
  (never generate animations independently per direction -- causes drift)
- Video: https://youtu.be/PV-oXn8_EzM

## Sjalsol's Deterministic 3x3 Grid Template (MEDIUM confidence)
Instead of 8-direction feature (described as "hit/miss on consistency"), generate a 3x3 grid:
- Layout: back-left / back / back-right / left / (empty) / right / front-left / front / front-right
- Requires SIZE LOCK + FOOTPRINT LOCK + ANCHOR in prompt
- Use hot-swappable variable template (TYPE/STYLE/HEAD/BODY/etc fields)
- For 4-dir games: only need front/side/back -- stop there, don't request full 8

## Ultimatefrisbie1's Agentic Sprite Workflow
- Python script stitches individual PNGs into spritesheet (PixelLab has no native sheet export)
- Self-reflection: pipeline writes `lessons-learned.md` after each run
- Engine: Godot MCP integration (analogous to Unity MCP for RIMA)

## snowli_on's 2-Step Pose Batch (HIGH confidence, reactions from community)
Step 1: Generate ALL needed poses in ONE call via "Create image from style reference (pro)"
  -- request: walking, hurt, knockdown, attack, jump, run poses together
Step 2: Feed each pose into interpolation or "Animate with Text NEW"
"The AI thrives from a stable first frame reference."

## MCP vs Direct API V2 Decision
| Need | Use |
|---|---|
| Pro tools (animation, rotation, tiles) | API V2 directly |
| Latest features | API V2 directly |
| Map generation | API V2 only (not in MCP) |
| Quick prototyping | MCP OK |
| Batch/automation logic | API V2 |

## Python PNG Stitching Pattern
PixelLab returns individual PNGs, not native spritesheets.
Write a Python script to:
1. Receive individual frame PNGs from API
2. Stitch into single spritesheet (PIL/Pillow)
3. Import into Unity

## Agentic Safety Rules (community hard lessons)
- Never leave agents running unsupervised (documented: 200 files -> 261,231 C++ files overnight)
- Git commit after every agentic change (documented: 7 days of work rolled back via git reset --hard)
- Do NOT give agents open-ended "fix all bugs" instructions (causes hallucinated bug loops)
- AGENTS.md / Skills provide better guardrail alignment than free-form agents
- Write `lessons-learned.md` after each pipeline run
- Change prompt strings slightly between sessions to avoid cached LLM reasoning

## External Tools for RIMA Pipeline
| Tool | URL | Purpose |
|---|---|---|
| cleanEdge | https://torcado.com/cleanEdge/ | Clean pixel art rotations, removes jaggies |
| make_aseprite | https://github.com/Dzejrou/make_aseprite | Convert PixelLab exports to .aseprite files |
| Aseprite CLI | compile from source (free) | Batch sprite sheet ops, Lua scripting |
| Skeleton pose tool | https://lysle.net/satoshi/skeleton | Generate 2D skeleton poses as reference images |
| pixellab-forge-mcp | https://github.com/rabbitcannon/pixellab-forge-mcp | Community MCP targeting V2 endpoints |

## Discord Tileset Lesson (2026-05-04)

Source:
- User-shared Discord screenshots `tilesetdc.png` and `tilesetdc2.png`.

Takeaways:
- PixelLab tileset output may produce separate tiles, not a production-ready connected tileset.
- Community advice: choose a tile you like and feed it back as a style tile/reference to stabilize
  future outputs.
- For connected roads/paths/seams, draw or mask the desired connection line and use edit/inpaint
  instead of asking for a full connected tileset in one shot.
- No-outline mode can sometimes produce better tiles, but must be tested against RIMA readability.

RIMA application:
- Generate/select a small style reference pack first.
- Use style references for base floor variants, decals, and module cohesion.
- Assemble final connected rooms in Unity using masks, RuleTiles/RandomTiles, sockets, and visual
  shell rules.
- Do not rely on PixelLab to output a finished connected playable room or finished autotileset.
