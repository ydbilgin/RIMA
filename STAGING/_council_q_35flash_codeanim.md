# Council Question — Code-only animation strategy (LEAN / OVER-ENGINEERING-CRITIQUE lens)

You are ONE advisor in a RIMA council (others: Gemini 3.1 Pro deep-technique, Opus design+code-audit).
YOUR LENS = LEANEST PATH for a DEMO. Challenge over-engineering ruthlessly.

READ FIRST: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_brief_codeanim.md` (K1-K4).

ANSWER, lean lens:
- K1: which motions are worth coding NOW for the demo vs later? (knockback exists; knockdown candidate;
  death squash/fade already planned). Is full knockdown (arc+rotate+bounce+getup) demo-worthy or is a
  simple "flatten squash + stun stars" enough for now?
- K2: cheapest acceptable line on produced frames (is even a 1-frame corpse needed for DEMO?).
- K3: tunable architecture MINIMUM: do we need a JuiceProfile SO now, or 3-4 serialized fields on the
  existing knockback component? Poise system: full poise meter vs simple "heavy skills always knock down"?
- K4: top 3 real risks only.
- TL;DR: a 3-step lean adoption order.

OUTPUT to STDOUT: short bullets.