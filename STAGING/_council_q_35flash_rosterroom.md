# RIMA — CharacterSelect v2 "Roster Room" — LEAN / SHIP-FAST lens (Gemini 3.5 Flash)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_ROSTERROOM_BRIEF_2026-06-04.md

You are the LEAN / over-engineering-critic advisor. The current 3-column CharSelect already WORKS and ships. The user wants to evolve it into a diegetic roster-room (one generated bg + characters standing in it + click-to-select + bottom bar + in-room locked classes). Keep it shippable, avoid traps. Blunt bullets:

1. **Minimum viable roster-room:** what's the smallest build that delivers the vision? Static character sprites placed on a generated bg + click-to-select + cyan seal under selected + bottom bar. What can be V1 vs deferred (idle animation = later, per user; parallax/particles; per-char hover anims; fancy locked reveals)?
2. **Reuse hard:** what survives from the current CharacterSelectScreen (SkillDatabase query, ClassIdentity, IsUnlocked/UnlockCost, SEÇ/GERİ, RuntimeRoot, CreateFullScreenBackdrop, pedestal_seal as selection ring)? Argue keep-don't-rebuild where possible.
3. **Biggest traps:** (a) placing 10 sprites cleanly over a generated bg at 16:9/4K without misalignment, (b) generating a bg whose floor anchor zones actually match where you place sprites, (c) click hit-areas overlapping for back/front characters, (d) over-designing locked-class UX. How to dodge each.
4. **Locked classes — cheapest acceptable:** simplest on-brand way to show 6 locked chars + Echo cost without a big system (e.g., dim sprite + small lock + cost text on select). Don't gold-plate.
5. **Background generation:** is one square 1024 image framed in the top ~70% (bottom bar over lower part) the pragmatic choice vs a wide custom bg? Cheapest path that looks good. Should the central selection seal be PART of the generated bg or a separate reused sprite (so it can glow/animate)? Argue separate.
6. **Ship order:** generate bg → wire room + placed sprites + click-select + seal → bottom bar → locked states. What unblocks visual feel-test fastest?

Favor reuse + defer. Numbers where useful.
