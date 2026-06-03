# CONSULT (code feasibility + build order) — RIMA demo roadmap

ACTIVE RULES: (1) think before answering (2) concrete, code-grounded (3) no speculation (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed.
Respond INLINE (captured to CODEX_DONE.md). This is INPUT — Opus makes the final call + writes the roadmap.

## The locked direction (NLM canon — the target)
~10-min polished vertical slice for Steam wishlists. Pure 2D top-down chibi pixel-art ARPG roguelite, Hades-Elysium
look. ONE class Warblade (8-dir, 3-hit melee combo, Rage). 5 rooms: 3 combat → 1 reward/draft → 1 boss (Penitent
Sovereign, demo = 50%-HP placeholder kill phase). Loop: clear → map-fragment drop → pickup (cyan beam, reveal next) →
3-card draft (Common/Rare) → gate unlock → walk in → 0.75s fade teleport. Must-haves: combat juice (hitstop 0.04-0.18,
dir shake, white flash, damage numbers, painterly VFX), cursor-aim, dash, draft UI, RoomLoader+Gate, map-fragment
ecosystem, Death/Victory + run-stats + Steam Wishlist CTA. Weaponless body + HandAnchor weapon; swing HIDES weapon +
shows painterly slash-arc VFX flipbook. CRITICAL pivot: graybox-combat-first — art production HALTS until combat
timing is frozen on graybox. Deferred: other classes, cross-class, Acts 2-3, extra room types, full boss, real synergies.

## What I need from you (code-feasibility + ordering advisor — NOT a state audit; a parallel Sonnet workflow is auditing state)
1. **Optimal BUILD SEQUENCE** to reach this demo, as ordered phases (6-9). Optimize for: lock combat FEEL first
   (graybox-first pivot), then loop coherence, then conversion/polish, then art/audio. For each phase: the 1-line goal +
   the key code work.
2. **Critical path + dependencies:** what MUST come before what (e.g. "rebinding before X", "boss-death→DemoComplete
   before Victory CTA"). Call out hard ordering constraints in the current codebase.
3. **Deceptively hard / high-risk code items** — where will time vanish or regressions hide? (You've seen the combat/
   juice/RoomLoader/skills code this session.)
4. **Quick wins** — high-value, low-risk code already 90% there (e.g. the 3 control-scheme bugs, boss-death race,
   DemoComplete wiring) that should go early.
5. **The single biggest code risk** to the demo + how to de-risk early.
Keep it tight + ordered. No code written. This feeds NLM-canon + ax-vision + Sonnet-state into Opus's final roadmap.
