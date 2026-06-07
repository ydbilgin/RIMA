# TASK: $imagegen — Portal asset production (one-by-one, QC'd)

ACTIVE RULES: (1) think before generating (2) one asset at a time, inspect each result before the next (3) surgical — only write under STAGING/imagegen/ (4) BLOCKED if unclear.

## Amaç
Produce the T3 portal visual assets via the established imagegen pipeline (generate → chroma-key cleanup → nearest-neighbor downscale → save PNG). NO Unity import in this task — output lands in `STAGING/imagegen/assets/portal_pack_2026-06-07/` for orchestrator QC; wiring happens in T3.

## Style references (use as image refs where the tool supports it; otherwise describe)
- PRIMARY: `STAGING/_incoming/portal_pack_2026-06-06/RIMA_PortalOnly_Updated_Pack/concept_sheets/01_rima_portal_sheet.png` (approved composition/silhouette reference — slate stone arch + cyan rift core + type rune + 2:1 dimetric)
- In-game pixel density refs: `Assets/Sprites/Environment/Doors/gate_north.png` (128x144, current gate) + `Assets/Sprites/Environment/Portal/portal_rift.png`
- Palette: slate gray stone + cyan (#3EE6E0 family) rift energy. PPU 64; character is 64px tall — portal must read at ~1.6× character height.

## Global production rules
- Background: SOLID MAGENTA (#FF00FF) for chroma-key. ⚠️ Because of this, NEVER use magenta/pink in the art itself — Elite accents = deep crimson/dark red, NOT magenta.
- Pixel-art look, crisp edges, no anti-alias soup, no text, no watermark, front-facing (south-facing) only — ONE facing direction.
- Per asset: generate → chroma cleanup (pixel-cleanup pipeline) → downscale nearest-neighbor to TARGET size → verify silhouette readable at target size → save `<name>.png` + note in `manifest.md` (prompt used, final size). If a result is muddy/unreadable at target size, retry ONCE with adjusted prompt before moving on; if still bad, log SKIP reason and continue.

## Asset list (produce IN THIS ORDER, one by one)
1. `portal_arch_combat.png` — TARGET 96x128. Ruined slate stone arch, cyan rift energy core filling the opening, small crossed-blades rune glyph on the keystone, subtle cyan glow. Clean readable silhouette.
2. `portal_arch_elite.png` — TARGET 96x128. SAME arch silhouette as combat (visual family!), but cracked/damaged frame, deep crimson accent cracks, skull-crown rune on keystone, slightly more aggressive core.
3. `portal_arch_reward.png` — TARGET 96x128. Same arch family, calm/dimmer cyan core, soft gold trim accents, chest/star rune on keystone.
4. `portal_arch_boss.png` — TARGET 128x176. Heavier oversized arch, fractured great-seal crest on top, dark red rune, dim slow rift core, imposing.
5. `rune_reward.png` — TARGET 32x32. Standalone chest/star rune icon, gold-on-dark, crisp pixel icon.
6. `rune_boss.png` — TARGET 32x32. Fractured-seal/skull rune icon, red-on-dark.
7. `decal_boss_ritual_circle.png` — TARGET 192x96 (iso-flattened ellipse). Cyan ritual circle ground decal, broken seal ring with runic marks, reads as lying FLAT on isometric floor (2:1 ellipse), transparent center areas.
8. `prop_seal_monolith.png` — TARGET 96x160. Tall slate seal monolith landmark, carved cyan glowing runes, cracked top, floating-island dark fantasy.

## Done criteria
- 8 PNGs (or documented SKIPs) in `STAGING/imagegen/assets/portal_pack_2026-06-07/` + `manifest.md`
- All chroma-cleaned (no magenta fringe), correct target sizes, transparent backgrounds
- Arch family (1-4) shares silhouette/palette identity — they must look like siblings
- Write summary to CODEX_DONE.md (per-asset status + any retries/skips). No code changes, no Unity import, no commit.
