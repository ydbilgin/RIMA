# PRODUCE (image gen) — RIMA menu background (VALIDATION image, then we scale)

ACTIVE RULES: (1) think before acting (2) min steps (3) save the file + report exact path/size (4) BLOCKED if your image tool is unavailable — say so explicitly, do not fake success.

## Goal
Use your image-generation skill (gpt-image-2 / image_gen / generate2dmap) to produce ONE real pixel-art asset that
replaces a placeholder. This is a **pipeline validation** — if it works + saves correctly, the orchestrator will send
the rest of the batch. Be honest about exactly how you generated and whether the file actually saved.

## The asset
- **File (OVERWRITE this path):** `Assets/Resources/UI/RIMA/menu_bg_island.png`
- **Final size:** 640×360 px (16:9). If your generator only outputs fixed sizes (e.g. 1536×1024 / 1024×1024),
  generate at the nearest 16:9 size then **downscale to exactly 640×360** (nearest/point, no smoothing) before saving.
- **There is a current placeholder at that path** (a procedural cyan-crack floating-island over purple void). If your
  tool supports an **init / reference image (image-to-image)**, use it to preserve composition + palette. If not,
  text-to-image is fine.
- **Prompt:** "A lone fractured floating stone keep-fragment suspended in a vast dark abyss, cyan rift cracks bleeding
  light along its torn cliff edges, one small warm ember brazier, slow drifting dust, cinematic low-angle hero shot,
  somber lonely mood, 2D pixel art, limited palette, deep-purple #3A1A4A to black void background, cyan #00FFCC rift
  energy glow as the only saturated accent, slate-grey stone, no text, no watermark, crisp pixels, PPU64 scale"

## Report back (inline, captured to CODEX_DONE.md)
1. Which tool/skill you used + the size you generated at + the size you saved.
2. Confirm the file exists at `Assets/Resources/UI/RIMA/menu_bg_island.png` (overwritten) — run a quick `ls -l` / dir
   and paste the bytes + dimensions.
3. If anything failed (tool unavailable, size mismatch, save failed) — say BLOCKED + exactly what failed. Do NOT claim
   success without the file on disk.

Do not touch any other file. One image only.
