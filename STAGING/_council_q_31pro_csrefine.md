# RIMA — CharacterSelect v2 Layout Refinement — DESIGN lens (Gemini 3.1 Pro)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_REFINE_BRIEF_2026-06-04.md
(Roster-room CharSelect shipped; user wants: framed game-like BOXES in bottom bar, characters spread WIDER/less-cramped, and asks whether skills should be a right panel.)

RIMA = dark pixel-art roguelite, cyan #00FFCC, void-purple, iso 3/4, premium/diegetic. Concrete, numbers:
1. **Skills placement — decide:** bottom-bar RIGHT zone (keeps the room clear for wide-spread chars; matches the loved reference) vs vertical RIGHT side-panel (covers right-side characters) vs other. Recommend ONE with reasoning. If bottom-right, how many skill chips, layout. If you think a hybrid is better (e.g., 3 skill chips bottom + "more" expand), say so.
2. **Framed boxes (the user's main ask):** design the bottom bar as 3 diegetic framed boxes (Classes-recap | selected identity+resource | skills) using 9-slice stone/obsidian frames + cyan edge-light. Proportions (widths), height of the bar, padding, how the frames read as "real game UI" not flat. Should the Classes-recap box even exist now that the roster IS the room (maybe replace with a portrait+name of the selected)? Recommend the 3-box content.
3. **Character spread — new coordinates:** give exact new normalized (X,Y) for all 10 so they're spacious, readable, click-targets clearly separated, grouped (4 unlocked front / 6 locked back), spanning wider (≈.08–.92 X). 2 clean rows or a graceful arc? Per-row scale. Ensure they sit high enough that the taller bottom box-bar doesn't crowd them.
4. **Balance:** with a taller framed bottom bar + wider chars, what bottom-bar height (normalized) keeps it premium without eating the room? Where's the selected-char focal emphasis now (bigger scale + seal)?

Tight, concrete, numbers + hex. Drives a procedural Unity build.
