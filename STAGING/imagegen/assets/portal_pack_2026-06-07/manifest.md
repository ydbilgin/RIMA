# Portal Pack 2026-06-07 Manifest

Pipeline: built-in imagegen source on solid magenta chroma key, local chroma cleanup, nearest-neighbor target sizing, PNG with alpha.

Reference inputs:
- Primary composition reference: `STAGING/_incoming/portal_pack_2026-06-06/RIMA_PortalOnly_Updated_Pack/concept_sheets/01_rima_portal_sheet.png`
- Pixel density refs: `Assets/Sprites/Environment/Doors/gate_north.png`, `Assets/Sprites/Environment/Portal/portal_rift.png`

## Assets

### portal_arch_combat.png
- Final size: 96x128
- Status: PASS
- Retries: 0
- Prompt used: ruined slate stone arch, cyan rift energy core filling the opening, crossed-blades rune glyph on keystone, subtle cyan glow, crisp pixel-art sprite, front-facing south-facing, 2:1 dimetric feel, solid #FF00FF chroma-key background, no magenta or pink in art.

### portal_arch_elite.png
- Final size: 96x128
- Status: PASS
- Retries: 0
- Prompt used: same arch family and near-identical silhouette as combat, cracked/damaged slate frame, deep crimson accent cracks, skull-crown rune on keystone, more aggressive cyan rift core, crisp pixel-art sprite, solid #FF00FF chroma-key background, no magenta/pink/purple in art.

### portal_arch_reward.png
- Final size: 96x128
- Status: PASS
- Retries: 0
- Prompt used: same arch family and sibling silhouette as combat, calm dim cyan rift core, muted gold trim accents, chest/star rune on keystone, crisp pixel-art sprite, solid #FF00FF chroma-key background, no magenta or pink in art.

### portal_arch_boss.png
- Final size: 128x176
- Status: PASS
- Retries: 0
- Prompt used: heavier oversized slate stone portal arch in same family, taller and imposing, fractured great-seal crest on top, dark red rune, dim slow cyan rift core, crisp pixel-art sprite, solid #FF00FF chroma-key background, no magenta/pink/purple in art.

### rune_reward.png
- Final size: 32x32
- Status: PASS
- Retries: 0
- Prompt used: standalone chest/star reward rune icon, gold-on-dark, compact centered pixel-art icon, treasure chest silhouette with star-like rune mark, solid #FF00FF chroma-key background, no magenta or pink in icon.

### rune_boss.png
- Final size: 32x32
- Status: PASS
- Retries: 0
- Prompt used: standalone fractured-seal/skull boss rune icon, red-on-dark, compact centered pixel-art icon, cracked circular great-seal outline, solid #FF00FF chroma-key background, no magenta/pink/purple in icon.

### decal_boss_ritual_circle.png
- Final size: 192x96
- Status: PASS
- Retries: 0 imagegen retries; 1 cleanup threshold adjustment
- Prompt used: cyan ritual circle ground decal, broken seal ring with runic marks, transparent center areas, reads as lying flat as a 2:1 flattened ellipse, no floor tile or vertical objects, solid #FF00FF chroma-key background, no magenta or pink in art.

### prop_seal_monolith.png
- Final size: 96x160
- Status: PASS
- Retries: 0
- Prompt used: tall slate seal monolith landmark, carved cyan glowing runes, cracked top, floating-island dark fantasy, centered vertical pixel-art prop with slight dimetric depth, solid #FF00FF chroma-key background, no magenta or pink in art.

## Verification

Fresh verification confirmed:
- All 8 final PNGs exist.
- Final dimensions match targets.
- All finals are RGBA PNGs with transparent corners.
- All finals have non-empty alpha content.
- No visible magenta-key pixels remain in final opaque content.
