# Sprite Forge vs imagegen Wall Comparison

## Inputs

| imagegen raw | Sprite Forge output |
|---|---|
| ![imagegen raw](concepts/fractured_chamber/iso_assets/raw/wall_nw_mid_plain_raw.png) | ![Sprite Forge output](concepts/fractured_chamber/iso_assets/wall_nw_mid_plain_sprite_forge.png) |

## Output Quality

Sprite Forge is the better fit for the current locked wall target. The new wall is a tall, mostly front-facing high top-down 3/4 wall band with a subtle top cap and slight side thickness. It avoids the true-isometric diagonal silhouette, has no banner or torch baked in, and is already final-sized at 128x384 with transparent alpha.

The older imagegen raw has stronger dramatic mass and high-detail masonry, but it reads as a true isometric diagonal wall piece. It also remains a large raw green-background image rather than a final transparent production tile, so it needs extra cleanup and projection correction before use.

## Cleanup Automation

Sprite Forge adds a useful deterministic cleanup stage after generation. For this sandbox wall, `generate2dsprite.py process` produced the raw copy, chroma-key-cleaned PNG, prompt log, and pipeline metadata before the final 128x384 alpha export. No frame split was needed for this single wall piece, but the same processor can split frames, align cells, and produce sheet metadata for animation or atlas outputs.

The existing imagegen path is stronger as a direct concept generator, but cleanup remains manual unless a separate processor is added. The green background and large raw dimensions make it less predictable for repeatable asset production.

## Pipeline Recommendation

Use a hybrid pipeline with Sprite Forge as the primary production/export path for RIMA environment pieces that must land as clean alpha PNGs or frame sheets. Keep imagegen as the primary visual ideation and style-reference source when broad composition, concept exploration, or high-fidelity mockups matter more than deterministic cleanup.

For fractured chamber wall assets: generate or select the concept style with imagegen, then route final wall pieces through Sprite Forge-style prompts and processor cleanup so projection, alpha, sizing, and repeatability are controlled.
