# UI Production Rules From Opus Review

Status: adopted notes after review
Date: 2026-05-04
Audience: AI agents first; keep compact.

## Adopt

- Combat HUD minimal; build/detail overlays rich.
- Reward UI uses fast 3-choice structure.
- UI frames must be thematic: dark stone, iron, tarnished gold, cyan rift light.
- Skill icons must be high-contrast silhouettes, not painterly noise.
- Skill details are on hover/hold/overlay; not always visible in combat.
- Tags/synergies must be visible in reward/build screens.
- Rarity colors: common gray, rare teal/blue, epic purple, legendary gold.
- Reward cards need micro-animation: slide in, hover glow, selection flash.
- PixelLab UI assets should have no baked gameplay text.
- Build UI from reusable components: 9-slice frames, runtime text, runtime icons.
- Gate production should use base threshold + swappable glow/icon/fog overlays.

## Conditional

- `Rift Tension`, `Soul`, and `Reroll` can have reserved UI slots only if backed by real runtime
  systems. Do not fake them as persistent HUD values.
- Minimum icon readability target: 32px native for small icons, 48px+ for important combat/reward
  icons.
- Hold-to-inspect is recommended for detailed text, but implement only when input flow is ready.

## Local Truth Overrides

Use local canonical docs over external research:
- `TASARIM/STYLE_BIBLE.md`: 35 degree ARPG, PixelLab `low top-down`, PPU=128, 128px native.
- `TASARIM/MASTER_KARAR_BELGESI.md`: camera and direction locks.

Known conflict:
- Some older CODEX/MEMORY notes still mention PPU=64 or 8-way/diagonal production. Treat those as
  legacy implementation context unless the task is explicitly about fixing that old pipeline.

## PixelLab UI Production Order

1. Master frame style: transparent dark stone/iron/gold frame, no text.
2. Core frames: skill slot, passive slot, reward arch, HP/resource bar, button, map node.
3. Icons: high-contrast silhouette, class accent color, transparent background.
4. Unity assembly: 9-slice frames, runtime text/icons, reusable templates, micro-animations.

## Gate Production Order

1. Neutral base gate silhouette.
2. Inpaint/style variants for room promise: combat, elite, chest, forge, merchant, event, curse,
   boss, unrevealed.
3. Separate glow/fog/seal/icon overlay where practical.
4. Animate reveal/open with first/end frames and interpolation.

## Avoid

- One flat generated UI image used as production UI.
- Equipment grid/backpack unless item systems become real.
- Full dungeon map reveal.
- Baked text in PixelLab frames.
- Icons below readable size.
- Constant full-room gameplay camera.
