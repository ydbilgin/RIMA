# RIMA — 3-Screen UI Redesign — DEEP DESIGN / ARCHITECTURE lens (Gemini 3.1 Pro)

READ this brief file first (full context, canon, current state, target layout, the design tension):
  F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\UI_REDESIGN_3SCREENS_BRIEF_2026-06-04b.md

You are the DEEP DESIGN advisor on a council (a feasibility advisor and a lean/ship-fast advisor answer separately). Give your strongest, most thoughtful design direction. RIMA is a dark roguelite ("The Fracturing / Shattered Keep"), pixel-art, cyan #00FFCC Rift/seal energy on void-purple, "Vivid Vulnerability" mood, ink-on-paper UI philosophy.

Answer these, concretely and with rationale (this informs an implementation spec — give numbers, recipes, hierarchy):

1. CHARACTER SELECT (the centerpiece). Target = 3 columns in 16:9: LEFT vertical class-roster list (10 classes, avatars), CENTER selected-class idle showcase on a backdrop, RIGHT scrollable class-details + skills + CONFIRM. The user's reference is a 1024×1024 SQUARE that looks slightly cramped — you must ADAPT it to wide 16:9 so it breathes.
   a. Best column proportions (% of width) and vertical rhythm so it reads premium, NOT cramped.
   b. How to frame the CENTER showcase so the character is the hero: pedestal/platform? cyan ground-seal? vignette? depth layers? Where does the character stand, how big?
   c. Roster-list elegance: row height, avatar treatment, selected-state vs hover vs locked-state visual language. How to show 10 rows (4 unlocked, 6 locked) without clutter.
   d. Reading order / focal flow for a new player.

2. PANEL TREATMENT — resolve the canon tension: canon says "no opaque gray HUD boxes / ink-on-paper", but the loved reference uses framed stone-roster + parchment detail panels. Give a CONCRETE reconciliation recipe (material, edge treatment, opacity, cyan seal accents, how it stays diegetic and on-brand). Should panels be carved stone? aged parchment? edge-lit? translucent?

3. MAINMENU composition. Existing ruins-keep background stays. The current menu (title "RIMA" + "THE RIFT HUNTERS" + "Yine geldin." + bare cyan text buttons BAŞLA/AYARLAR/ÇIKIŞ + vignette + v1.0) feels plain/floating. Propose a stronger composition: title treatment, button hierarchy & placement (left-anchored column? lower-third? framed?), motion/feel, focal balance against the BG art — premium but minimal, ink-on-paper, Turkish text kept.

4. SKILL DISPLAY on the CharSelect right panel: best way to present a class's signature skills (icon + name + short desc) so it teaches identity without overwhelming. Card row? icon grid? Should it scroll? How many skills shown?

5. COLOR / TYPOGRAPHY system unifying all 3 screens (accent usage discipline so per-class color shines without chaos; heading vs body treatment; cyan reserved for what).

Keep it tight, structured, implementable. Numbers and recipes over adjectives. This is for a Unity runtime-built UI (procedural, CanvasScaler 1920×1080).
