# CONSULT (UX / genre research) — RIMA control scheme + key rebinding + HUD bars

ACTIVE RULES: (1) think before answering (2) minimal, no speculation (3) RIMA-fit is the #1 criterion (4) say BLOCKED if unclear.
Respond INLINE in your final message (NOT to a file — the dispatcher captures your stdout). Be concrete, ~1 page.

## Role
You are the UX / genre-conventions advisor. This will be synthesized with a Codex code-architecture consult and an
Opus final decision. Do NOT write code. Research best practice, then recommend what fits RIMA.

## What RIMA is
2D top-down ARPG roguelite, Hades / Children of Morta lineage, keyboard+mouse primary (gamepad secondary).
WASD move, dash on Space, **attack aims toward the mouse cursor** (already implemented + default ON), melee+ranged
hybrid per class, run-based with a draft/skill system (7-slot skill bar). Demo = 1 class (Warblade) + 5 rooms + boss.
Brand: cyan rift #00FFCC over dark-purple void; "Ashen Glyph" UI (translucent fractured stone, no hard rectangles,
things pulse/fade). The feel target is crunchy, readable, fast.

## Questions to answer (cite the games you draw from)
1. **Cursor-aim attack convention:** In top-down K+M ARPG roguelites that aim attacks at the cursor
   (e.g. Death Must Die, Halls of Torment, Brotato auto-aim variants, Hades, Children of Morta, Magicraft, Soulstone
   Survivors), what is the standard: do you face/strike toward the cursor on press (snap) or continuously rotate to
   cursor (hold)? How do melee-vs-ranged differ? What makes it feel good vs sloppy? One clear RIMA recommendation.
2. **Default keybinds:** Recommend a canonical default layout for: move, dash, primary attack, class-secondary,
   7 skills, ultimate/"rift-break", map, pause, interact. What do the reference games use? Which keys are
   ergonomic for a left-hand-on-WASD player (skills on 1-4 + Q/E/R/Shift? mouse buttons?).
3. **Key REBINDING UX:** What does a good rebinding settings screen look like in this genre? (press-to-rebind flow,
   conflict detection, reset-to-default, separate KB/gamepad tabs, which actions should be NON-rebindable). How
   much does the audience expect it for a demo/wishlist build vs full release — is it worth doing now?
4. **HUD bars logical layout:** For HP + a resource bar (RIMA "Rage") + cooldown skill bar + minimap, what is the
   canonical, readable arrangement for this genre? (player-anchored vs screen-corner bars, bottom-center skill bar
   like Hades/Diablo, HP placement, boss bar placement). Give a concrete RIMA layout that fits the "Ashen Glyph"
   translucent-stone aesthetic and the cyan brand.
5. **One paragraph: the single most important thing** to get right for RIMA's controls to feel pro, and the most
   common mistake indie ARPG roguelites make here.

Keep it tight and decision-useful. No code.
