ACTIVE RULES: (1) think before answering (2) no speculation, ground claims (3) concise (4) say UNSURE if unclear.
NLM ACCESS: If you need RIMA design context, query NLM via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
RESPOND INLINE (not to a file). You are the PLAYER-EXPERIENCE / WISHLIST-CONVERSION reviewer.

# TASK: Review the demo "big work" priority from a PLAYER + STEAM-WISHLIST lens

RIMA = 2D top-down pixel roguelite (Hades / Children of Morta feel). Goal: turn a working
skeleton into a demo that CONVERTS Steam wishlists. 10-min loop, 5 rooms, Warblade class only.

## Candidate big-work items (8)
1. Combat-feel polish + F5 playtest (feel lock)
2. Weapon system live-test (mount/swing/VFX)
3. Boss fight (PenitentSovereign — sprite missing)
4. Mob variety + art (archive-restore vs PixelLab vs RTX-local)
5. Audio skeleton (music + SFX placeholder)
6. Live editor T3 integration (dev-tool)
7. Map/minimap + gate preview
8. Demo loop E2E (5 rooms) + Victory Wishlist CTA

## Already working (don't re-litigate): HUD, hit-confirm (slash/flash/spark) wired, SkillBar,
room-transition fade, hitstop, shake, damage numbers, rage, combo, dash i-frame.

## Canonical order to validate or CHALLENGE
(1) Combat-feel + Weapon → feel-freeze, (2) Audio (claimed game-feel ROI #1, "doubles hit feel"),
(3) Demo E2E + Wishlist CTA, (4) Mob/Boss art AFTER feel-freeze. DEFER: T3 editor, Map/minimap.

## ANSWER (inline, concise — no preamble)
1. **Do you AGREE with the order?** If not, what changes and WHY (player-impact reasoning).
2. **Which 3 items move first-60-seconds feel + wishlist conversion the MOST?** Rank them.
3. **Cheap-high-impact items MISSING from the list?** (e.g. screen-juice, run-stat screen, oda-monolog, elite recolor variant). Max 4, each with 1-line impact.
4. **Biggest risk** to wishlist conversion if we ship the demo as-is right now.
5. **Map/minimap**: agree it's DEFER for a 5-room linear demo, or is there a cheap version worth keeping? 1-2 sentences.
Keep total under ~400 words. Ground claims in roguelite/Steam-demo reality, not generic advice.
