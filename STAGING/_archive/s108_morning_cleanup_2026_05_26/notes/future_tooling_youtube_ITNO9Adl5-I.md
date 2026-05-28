# Future Tooling Note — YouTube Short ITNO9Adl5-I

Date: 2026-05-22
URL: https://www.youtube.com/shorts/ITNO9Adl5-I
Title: "Creating large pixel art images with PixelLab"
Assessed by: RIMA Research Agent (Gemini default model + WebFetch)

---

## What It Is

- Demonstrates PixelLab (pixelab.ai) for generating large-format pixel art assets.
- Key techniques shown: resolution upscaling (128px -> 256/512px), reference-based generation for style consistency, AI inpainting to edit specific regions via text prompt.
- Also highlights the Aseprite plugin, enabling PixelLab generation directly inside Aseprite without leaving the editor.

## RIMA Fit

- PixelLab is already RIMA's primary asset generation tool — this video is workflow reinforcement, not a new tool introduction.
- The Aseprite plugin is the notable detail: if RIMA ever does post-generation hand-editing in Aseprite, the plugin could remove the export/import round-trip between PixelLab web and Aseprite.
- Resolution upscaling workflow (start small, AI-upscale with added detail) could accelerate concept-to-final-asset for characters; currently we generate at target size directly.

## Details

- Software: PixelLab (web-based) + Aseprite plugin
- Task solved: Faster high-res pixel art production; style-consistent variation generation; targeted region editing
- Pricing: Subscription; ~$10-12/month commercial tier (RIMA already subscribed)
- Open source: No — commercial SaaS
- Learning curve: Low for PixelLab web (already in use); the Aseprite plugin requires a one-time install + API key setup, ~15-30 min

## Verdict

**LATER** — RIMA already uses PixelLab. The only actionable item here is the Aseprite plugin, which is worth testing once we enter a hand-editing/polish phase (post-vertical-slice). No immediate workflow change needed.

Action to take later: Install PixelLab Aseprite plugin when hand-polish of character or tile assets becomes a recurring task.
