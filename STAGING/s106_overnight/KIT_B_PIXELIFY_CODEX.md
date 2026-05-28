# Kit B Cliff Face Pixelify — Codex `$imagegen` (xhigh)

ACTIVE RULES: (1) think before generating (2) min output, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Kit B HD cliff face refs (1024×1536) → game-resolution PIXEL ART (128×192) regen via `$imagegen` BUILT-IN TOOL mode. User bu pixelified output'ları S-XL Pro Web UI'da init image olarak besleyip final prod sprites üretecek. Bizim çıktı bir intermediate STEP — pixelify reference quality.

## INPUT (9 HD cliff face refs)
- `STAGING/s106_overnight/ref_kit_b/cliff_N.png` (1024×1536)
- `STAGING/s106_overnight/ref_kit_b/cliff_S.png` ⭐ EN ÖNEMLİ (front face, camera-facing)
- `STAGING/s106_overnight/ref_kit_b/cliff_E.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_W.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_NE.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_NW.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_SE.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_SW.png`
- `STAGING/s106_overnight/ref_kit_b/cliff_cyan_glow.png` (energy seam variant)

## OUTPUT TARGET (9 pixel art versions)

**Folder:** `STAGING/s106_overnight/ref_kit_b_pixelified/`

**Per asset specs:**
- Size: **128×192 px RGBA** (2:3 portrait, matches Kit A iso PPU=64 scale)
- Transparent BG (RGBA alpha, NOT magenta — Remove BG ON anyway)
- Pixel art style — **strict pixel art aesthetic**, must integrate with existing 64×64 floor tiles
- View angle: **top-down 3/4 ~70-80°** (Hades / Children of Morta tier)
- Subject: vertical cliff face hanging below a floating stone arena, dark granite + warm orange highlights at top rim + cyan energy cracks dripping down
- Color palette: dark cool grays (#2a2e36 base) + warm orange rim light (#d4843a) + cyan crack accent (#00ffcc) — limit ~32-64 colors total

## METHOD

Use Codex `$imagegen` BUILT-IN TOOL mode (preferred — `feedback_codex_imagegen_skill_not_env_var.md`):
- **NOT** the CLI fallback `scripts/image_gen.py` (requires OPENAI_API_KEY)
- For each cliff direction, generate fresh pixel art at 128×192 with the HD ref as visual reference

### Per-direction prompt template
```
Pixel art floating stone island cliff face, viewed from top-down 3/4 perspective ~75°.
Direction: {N|S|E|W|NE|NW|SE|SW — describe which side the cliff face is showing}.
Dark granite stone hanging vertically below a floor surface, irregular layered rock formation, 
weathered worn texture. Top rim has warm orange brazier glow accent (subtle, 1-2 px highlight).
Cyan energy cracks dripping downward in places (#00ffcc, 1-2 px width, 2-3 vertical streaks max).
Bottom edge frays/disintegrates into the void below (transparent fade).
Pixel art, limited palette ~32 colors, strict integer pixels, transparent background.
Size: 128×192. No outline. No text. No logo.
```

### Cyan glow variant
```
Pixel art cliff face section, identical to cliff_S baseline but with a dominant CYAN ENERGY SEAM 
running vertically through the middle. Bright cyan core (#00ffcc), fading to deeper cyan (#0099aa) 
at edges. Cyan light bloom subtle glow on adjacent rock. Used as overlay highlight under rune zones. 
Pixel art, transparent background, 128×192.
```

## DELIVERABLES

1. **9 PNG files** in `STAGING/s106_overnight/ref_kit_b_pixelified/`:
   - cliff_S.png, cliff_N.png, cliff_E.png, cliff_W.png
   - cliff_NE.png, cliff_NW.png, cliff_SE.png, cliff_SW.png
   - cliff_cyan_glow.png
   - All 128×192 RGBA, transparent BG

2. **1 composite preview** `STAGING/s106_overnight/kit_b_pixelified_preview.png`:
   - Side-by-side: floor tile_0 (64×64) + cliff_S (128×192) + cliff_cyan_glow (128×192) + cliff_E (128×192)
   - 4x display upscale for visibility
   - Tonal consistency check — do they read as same art style?

3. **Brief verification report** `STAGING/s106_overnight/KIT_B_PIXELIFY_REPORT.md`:
   - Confirm 9 PNGs generated at exact 128×192 RGBA
   - Note any tonal mismatch vs Kit A floor (be honest)
   - File sizes per asset
   - Total `$imagegen` calls used

## CONSTRAINTS
- **NO PixelLab MCP calls** — user explicitly halted PixelLab dispatch (`feedback_pixellab_dispatch_halt_codex_imagegen_first.md`)
- **NO autonomous Web UI calls** — output is intermediate, user will manually feed to S-XL Pro
- **NO Unity edits** — pure asset generation
- 9 sprites + 1 preview + 1 report. That's it.
- If a direction fails to gen properly (e.g. wrong angle), regenerate once. Max 2 attempts per asset.

## TIME ESTIMATE
~45-60 dk for 9 sprites + preview + report at xhigh effort.

## DONE FILE
Write `CODEX_DONE_laurethayday.md` (or active profile) with `STATUS: DONE` and file paths.
