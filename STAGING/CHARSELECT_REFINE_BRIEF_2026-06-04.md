# CHARACTER SELECT v2 — LAYOUT REFINEMENT — BRIEF
Date: 2026-06-04 · For council (cx feasibility / ax-3.1 design / ax-3.5 lean)

## CONTEXT
CharacterSelect v2 "diegetic roster room" just shipped + verified: generated iso ruined-keep backdrop (room_bg) + 10 real idle_south class sprites placed in a depth arc (front 4 unlocked / back 6 locked) + per-char click-select + pedestal_seal ring/glow under selected + dim others + bottom HUD strip (currently 3 FLAT zones: identity | skills | SEÇ/GERİ) + locked CTA "KİLİDİ AÇ — {cost} Echo". File: Assets/Scripts/UI/CharacterSelectScreen.cs (procedural).

## USER FEEDBACK (drives this refinement)
1. **"boxları olsun tıpkı oyun gibi"** — the bottom UI should be FRAMED BOXES like a real game / the reference (reference Image #9 bottom bar = 3 ornate framed panels: Classes-list | selected details | skills). Current flat zones → make them diegetic framed boxes.
2. **"arka arkaya yan yana değil daha geniş sıralanabilsin"** — characters are too cramped/back-to-back; spread them WIDER, more spacious, less overlap.
3. **OPEN QUESTION (user asked our opinion): "skiller sağ panelde mi daha iyi?"** — should skills go in a vertical RIGHT side-panel, or stay in the bottom bar (right zone)?

## CURRENT NUMBERS
- Characters arc (normalized X,Y): front Warblade(.35,.45) Elementalist(.45,.40) Ranger(.55,.40) Shadowblade(.65,.45); back Ronin(.20,.60) Ravager(.32,.55) Gunslinger(.44,.52) Brawler(.56,.52) Summoner(.68,.55) Hexer(.80,.60). → feels cramped, X only spans .20–.80.
- Bottom HUD strip anchored 0.0–0.25, 3 flat zones.
- Reuse assets: panel_frame_9slice (border 32), card_frame_9slice (border 28), button_9slice (border 16), bar_frame_9slice, pedestal_seal, bg_seal_keep. RimaUITheme.ClassIdentity/ClassAccent/SkillDatabase query.

## DECISION QUESTIONS
1. **Skills placement (resolve definitively):** bottom-bar RIGHT zone (keeps room clear, matches reference) vs vertical RIGHT side-panel (more skill room but COVERS the right-side spread characters) vs other (e.g., skill chips float near selected char, or a slide-up panel). Given the user wants characters spread WIDE across the screen, what's best? Recommend with reasoning.
2. **Framed boxes:** how to make the bottom HUD look like real game boxes using existing 9-slice Pack frames + cyan edge — which frame for which box, proportions, padding, so it reads premium/diegetic not flat.
3. **Character spread:** optimal arrangement for 10 chars so they're spacious + readable + click-targets don't overlap + still grouped (unlocked vs locked). New normalized coords (wider X span, e.g. .08–.92), depth/scale, spacing. Should it be 2 clean rows? a wider arc? Numbers.
4. **Cohesion:** does widening characters + adding bottom boxes risk crowding the bottom? How tall should the bottom bar be, and do characters need to sit higher to make room?

## CONSTRAINTS
- Reuse-first (existing Pack frames + sprites). Procedural Unity UI, 1920x1080 CanvasScaler, normalized anchors.
- Output: concrete spec (skills decision + box treatment + new character coords).
