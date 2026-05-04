---
name: PixelLab generation budget
type: project
trigger: PixelLab budget, confirm_cost, batch generation
description: PixelLab generation budget and cost-check rule
---

## Gen Budget
~2414 remaining as of 2026-05-03 (no significant spend in 2026-05-03 session).
**CRITICAL: Credits EXPIRE 2026-05-18. No rollover. New 5000 arrives 2026-05-18.**

## 2026-05-02 Session Allocation (LOCKED)
- Run template (no walk in game, only run): 16 gen
- Warblade all 11 custom skills: ~1,760 gen (4 dirs x 40 each; south may be partial for some)
- Elementalist top 4 custom skills: ~640 gen
- Total target: ~2,416 gen (burns this cycle intentionally before expiry)

## May 18 Cycle (5000 new gen)
- Elementalist remaining 8 skills: ~1,280 gen
- Ranger all 10 skills: ~1,600 gen
- Shadowblade all 12 skills: ~1,920 gen
- Total: ~4,800 gen

## Rules
- confirm_cost=true for custom animations. Not needed for template animations.
- Template cost: 1 gen/direction. Custom: ~4-9 gen/direction at 128px (v0.4.92+).
- No walk animation in game -- only run template needed.
- API rate limit: 2 seconds between calls (staff confirmed).

## Animate with Text COST (CRITICAL UPDATE 2026-03-16)
As of v0.4.92, animate-with-text-v3 costs 1-9 gens (was 40 in v0.4.69).
Previous budget math was off by 5-40x. Recalculate all animation budgets.
Example: 4 directions x 9 gens = 36 gens max per animation (was 160).

## Pro Tool Batch Sizes
- 32x32 canvas: 64 images per 40-credit run
- 64x64 canvas: 16 images per 40-credit run
- 128x128 canvas: 4 images per 40-credit run
- Animate with Text (v3, v0.4.92+): 1-9 gens per animation

## Budget Stretch Tips (Discord-validated)
- For icons/loot: 32x32 = 4x more sprites per credit than 64x64
- Style reference auto-grids: upload individual sprites, no manual canvas tiling needed
