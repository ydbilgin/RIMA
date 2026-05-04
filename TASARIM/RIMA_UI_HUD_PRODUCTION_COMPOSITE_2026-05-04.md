# RIMA UI / HUD Production Composite - 2026-05-04

Status: production design draft
Audience: AI agents first; compact implementation-facing spec

Primary visual mockup:
- `TASARIM/UI_CONCEPTS/HUD/rima_hud_composite_mockup_2026-05-04.svg`

Source rules:
- `TASARIM/RIMA_UI_STATE_BLUEPRINT_2026-05-04.md`
- `TASARIM/UI_PRODUCTION_RULES_FROM_OPUS_REVIEW_2026-05-04.md`
- `TASARIM/GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md`

## Core Direction

RIMA combat UI should feel like a dark stone instrument panel, not a full RPG character sheet.

Combat HUD:
- quiet, stable, low height
- no long descriptions
- icons and readiness first
- danger/readability above decoration

Rich UI belongs in:
- reward draft overlay
- build/character overlay
- gate clear/map strip
- special rooms

## Composite Layout

### Combat State

Top-left:
- class crest / portrait socket
- HP bar
- class resource bar
- low-HP danger pulse layer

Top-center:
- room objective/status micro-banner
- room type icon
- elite/boss/curse warning only when relevant

Top-right:
- partial route strip
- current node, direct exits, one step fog preview if revealed
- no full route

Bottom-center:
- six skill slots:
  - Q/E/R/F primary
  - Z/X secondary, dim until unlocked
- cooldown radial inside slot
- key label in lower corner
- icon silhouette is the read, text is not

Bottom-left:
- transient passive/proc chips
- only active/recent passives; no full passive list

Near interactable:
- small `[G] Action` prompt
- object beam/highlight
- no modal until player interacts

### Room Clear State

Sequence:
1. `ROOM CLEARED` micro-banner.
2. Reward object highlights.
3. Gates remain dim/locked.
4. After reward pickup, gates open and gate promises become readable.
5. Optional route strip updates to next 1-2 graph nodes.

### Reward Draft State

Center:
- 3 reward cards
- card = icon, skill name, type, tier, 1-line effect, tags

Right side:
- compact build summary
- active slots, class resource, current synergies

Bottom:
- replace mode row if active slots are full

Motion:
- cards slide in
- hover glow
- selection flash
- rejected cards fade

### Gate Choice State

In-world:
- gate sockets are actual thresholds, not floating arrows
- forms: arch, breach, stair, chained doorway, rift threshold, bridge mouth, shrine passage

Overlay:
- small room-type icon over gate
- lock/fog/reveal layer
- optional short route label from runtime data

Map:
- visited/current/direct exits
- one-step or two-step revealed nodes only
- hidden route stays fogged

### Build Overlay State

Access:
- pause/character/build screen only

Show:
- class identity
- active kit
- passives/echoes
- tags and synergies
- recent rewards
- current route context

Avoid:
- backpack/equipment grid
- fake run resources
- full classic RPG stat sheet

## Production Components

PixelLab / asset targets:

| Component | Native Size | Count | Notes |
|---|---:|---:|---|
| HUD frame 9-slice | 256x64 | 1-2 | dark stone/iron, no baked text |
| Skill slot frame | 64x64 | 6 variants | ready/cooldown/locked/secondary/ultimate-ready/danger |
| Reward card frame | 256x384 | 4 rarity variants | common/rare/epic/legendary |
| Passive chip frame | 96x32 | 3 variants | active/proc/expired |
| Map node icons | 48x48 | 8-10 | combat/elite/chest/forge/merchant/event/curse/boss/unknown/current |
| Gate overlay icons | 64x64 | 8-10 | match map node vocabulary |
| HP/resource bar caps | 192x32 | 2-4 | red HP, class resource fill mask |
| Button prompt frame | 160x48 | 1 | runtime key/text |

Skill icon target:
- important icons: 48px+ readability
- native source can be 64x64
- transparent background
- high-contrast silhouette
- class accent color
- no painterly noise

## UI Asset Pack Prompt Shape

Use this later for PixelLab/UI asset generation:

```text
dark fantasy pixel art UI component sheet, RIMA game interface, reusable transparent UI frames,
dark cracked stone, black iron trim, tarnished gold accents, pale cyan rift glow, sharp rectangular
8px radius or less, no text, no numbers, no icons except empty sockets, clean 9-slice friendly
edges, high contrast, readable at small size
```

## Implementation Notes

- Build UI from prefabs/templates, not one baked image.
- Keep runtime text and runtime icons separate from generated frames.
- Combat HUD must not resize when cooldowns/locks/procs change.
- Use color plus shape/icon, never color-only meaning.
- Gate and map vocabulary should share icon shapes.

