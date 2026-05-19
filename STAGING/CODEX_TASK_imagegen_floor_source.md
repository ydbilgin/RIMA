# Codex Task — Imagegen L2b Macro Floor Source

You have access to the `imagegen` skill backed by `gpt-image-1`.

## Goal

Generate ONE large seamless painterly dark slate stone floor texture for RIMA roguelite, sized 1024×1024. This image will be sliced by a Python script into 64×64 / 128×128 / 256×256 macro patch candidates for the L2b layer (NOT base tiles).

This is also a **test dispatch** — confirm that the `imagegen` skill still works in your current profile (last successful dispatch was `b44kom7jl` for the Painterly Pack v1). If it errors, report the error verbatim so we can debug.

## Output path

Save to: `STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png`

If multiple variants generated, name them `_v2`, `_v3`, etc.

## Prompt for gpt-image-1

```
Large seamless dark slate stone floor area for a dark fantasy roguelite ground surface, viewed from a low top-down 30 to 35 degree perspective angle, painterly pixel-art-compatible style with fractured-epic mood. The surface is covered in irregular natural cobblestone shapes of varied sizes ranging from small fist-sized stones to large flagstones, organically packed together without any straight grid lines or repeating patterns. Each stone has subtle worn weathered grain, soft painterly highlights catching cool ambient light from the upper-left direction, and gentle shadowed edges where stones meet. The dominant palette is muted slate blue-gray hex around 3A4250 with subtle cooler shadow pockets hex around 2A323C and faint warm amber undertones hex around 6B5840 hinting in deep crack lines and dust accumulations. Stones bleed into neighbors with soft painterly transitions, no hard borders between stones, no decorative frame around the image. The entire 1024 by 1024 area is a continuous floor surface with edges extending past the canvas naturally as if the floor continues beyond. Plain undecorated stones only — no props, no characters, no decals, no symbols, no rune carvings, no glowing elements, no blood, no rift, no ritual marks, no painted decoration, no border frame. Pure low-contrast atmospheric stone floor texture suitable for a dark fantasy ARPG dungeon arena ground.
```

## Parameters

- size: 1024×1024 (use the largest gpt-image-1 supports)
- quality: high
- style: natural / painterly (NOT vivid)
- seed: pick consistent seed so we can re-generate variants

## Reject criteria

- Visible grid lines or tile borders → reject
- Bright orange / red / neon colors → reject
- Glowing elements, runes, symbols → reject
- Photorealistic high-detail noise → reject (we want painterly low-contrast)
- Top-down 90 degree (no depth) → reject (need 30-35° angle visible)
- Cartoon style → reject

If output drifts, retry with seed adjustment or prompt refinement. Document what was changed.

## Report back

Write `STAGING/CODEX_TASK_imagegen_floor_source_DONE.md` with:
- File path of generated PNG
- gpt-image-1 model status (worked? credit cost?)
- Any drift issues observed
- Recommendation: GO (use this for slicing) or NO-GO (need different prompt)

## Out of scope

- Do NOT modify any Assets/ folder files
- Do NOT touch existing Painterly_Pack_v1
- Do NOT generate multiple sources unless first one fails QC — start with 1, then escalate
- Do NOT use PixelLab MCP (credits depleted)
