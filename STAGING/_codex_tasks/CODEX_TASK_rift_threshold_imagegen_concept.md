# CODEX TASK — Rift Threshold Concept Image Generation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

User RIMA "Rift Threshold" kapı tasarımının görsel concept'ini görmek istiyor. **gpt-image-1 backend ile 4 state concept image üret.**

Spec referans: `STAGING/DOOR_DESIGN_SPEC_S95.md` (Opus design, 9 section)

## Concept

**Rift Threshold:** Vertical rift seam through wall, gravity-aligned reality tear. NOT framed door.
- Wall: existing Pilot A `arch_opening` style (granite 35° iso arch)
- Rift overlay: vertical crystalline column through arch interior
- Direction-invariant (vertical pivot symmetric)

## Üret — 4 PNG (Act 1 cyan palette)

Her image **128×128 px, transparent background, iso 35° fake-top-down view, RIMA Act 1 Shattered Keep palette**.

### Image 1 — `rift_threshold_locked_act1.png`
```
Pixel art 128x128. Isometric 35-degree dungeon stone arch opening (granite #3A3D42 dark gray, baked left shadow). Inside the arch opening: a vertical crystalline rift seam, DIM state — dormant cyan crystal shard (#3A4D5C dim shell, #00FFCC dim core). Narrow 8-12px wide vertical line, soft outer glow radial gradient. Low intensity, ~30% alpha. Transparent background. 

Negative: horizontal accents, door ornaments, hinges, lock symbols, frames, padlocks, swirling vortex, perspective tilt, character figures.
```

### Image 2 — `rift_threshold_active_act1.png`
```
Pixel art 128x128. Same isometric granite arch opening. Inside: a BRIGHT pulsing vertical cyan rift seam (#00FFCC saturated core, soft cyan halo radiating outward). High glow intensity, alpha 90%, particles rising from the seam — cyan sparks lifting upward. Vertical line wider now ~16px. Aggressive radial bloom. Transparent background.

Negative: horizontal elements, framing, character figures, swirling motion, lens flare.
```

### Image 3 — `rift_threshold_portal_act1.png`
```
Pixel art 128x128. Same isometric granite arch opening. Inside: rift EXPANDING outward — vertical cyan column flaring into screen-fill white-cyan flash. Peak transition burst frame (frame 4 of 6-frame sequence). Bright white-cyan core, cyan outer halo radiating beyond arch edges. The rift is wider, ~32px, eating into the surrounding granite. Transparent background.

Negative: horizontal motion, frame ornament, character figures, complete obliteration of arch (arch should still be visible at edges).
```

### Image 4 — `rift_threshold_final_act1.png`
```
Pixel art 128x128. Same isometric granite arch opening but LARGER ~96×96 visible (sprite contains larger arch). Inside: extra-bright vertical cyan rift seam (active state PLUS more intensity), wider ~20px column, dense particle column rising, intense radial bloom. This signals "final sub-room exit — macro reward incoming." Brighter than `active` variant. Transparent background.

Negative: horizontal accents, ornamental framing, character figures, vortex animation, multiple rifts.
```

## Dispatch Detayı

- Backend: **gpt-image-1** (Codex imagegen pattern, memory `project_hybrid_asset_pipeline_lock`)
- Output: `STAGING/concepts/rift_threshold_*.png` (4 files)
- Style: pixel art, isometric 35°, RIMA Act 1 palette
- Transparent background mandatory

## Rapor

`STAGING/CODEX_DONE_rift_threshold_imagegen.md`:
- 4 PNG output path
- Generation parameters
- Visual notes per image
- User-visible inline preview önerisi (Markdown image embed)

## Constraint

- Sadece concept üretim, **gerçek game asset değil** (PixelLab batch sonra)
- 4 PNG'yi sahneye ekleme yok, sadece STAGING/concepts/
- Mevcut Pilot A wall'lara dokunma

## Effort

low — 4 imagegen call, ~5-10 dakika.
