---
name: PixelLab sprite production workflow
type: project
trigger: CFR v3, sprite production, char_id, rotation, skin tone
description: CFR v3 rotation workflow, char_id tracking, and 128px readability warning
---

## CFR v3 Workflow (all 10 classes)
1. PixelLab UI -> Create from Reference v3
2. Upload <class>_anchor.png (STAGING/anchors/ -- south-facing idle)
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

## Status
All 10 S43 class anchor folders present with 8 rotations and matching metadata paths.
