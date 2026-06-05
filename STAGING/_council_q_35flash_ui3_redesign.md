# RIMA — 3-Screen UI Redesign — LEAN / SHIP-FAST / OVER-ENGINEERING-CRITIC lens (Gemini 3.5 Flash)

READ this brief file first:
  F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\UI_REDESIGN_3SCREENS_BRIEF_2026-06-04b.md

You are the LEAN / PRAGMATIC advisor on a council (a deep-design advisor and a code-feasibility advisor answer separately). Your job is the OPPOSITE of gold-plating: the leanest path that still looks good, and a sharp critique of anything that risks over-engineering or scope-creep.

Answer concretely:

1. CHARACTER SELECT 3-column rebuild: what is the MINIMUM that delivers the wireframe and looks premium? Which of these are NICE-TO-HAVE that we should DEFER for v1: animated idle (vs static idle_south sprite), parallax/depth backdrop, per-skill icons, scroll on the right panel, locked-class teasers? Give a v1 (ship now) vs v2 (later) split.
2. PANEL TREATMENT: simplest recipe that satisfies "not a flat gray box" without an art pipeline — can we get there with the EXISTING 9-slice Pack frames (button_9slice, card_frame_9slice, pedestal_seal, bg_seal_keep) + tint + a cyan edge line, NO new generation? Say exactly how.
3. MAINMENU: smallest change set to make it look intentional instead of "floating text" — alignment, spacing, a divider/rule, button affordance — using only layout/typography, no new art. What's the 20% that gives 80%?
4. ASSET GENERATION: be the skeptic. What new assets are ACTUALLY needed vs what's vanity? If a CharSelect center backdrop is wanted, can we reuse bg_seal_keep / main_menu_bg dimmed instead of generating? Argue reuse-first hard. Give the SHORTEST possible generate-list (ideally zero or one).
5. SETTINGS: confirm the cheapest wiring (MainMenu AYARLAR opens the existing SettingsMenuUI) and flag any trap that would make it more than a 1-hook job.
6. RISK CALL: what is the single biggest time-sink / over-engineering trap in this whole redesign, and how do we avoid it?

Be blunt, terse, bullet-points. Favor reuse and deferral. Numbers where useful.
