# ANIM_cx - Warblade demo animation council

S1 - Priority decision
P1 (must, visible in golden-path video):
- Idle / guarded breathing loop. Required because every shot starts or settles here.
- Run / directional locomotion. Required because edit-to-play needs movement proof, not a static character.
- LMB basic warblade strike. Required because this is the only stat-scaling combat beat; the attack animation must sell the damage math.

P2 (high value if P1 is clean early):
- One reusable generic skill-cast / combat flourish for Q/E/R/F button choreography. This is more useful than bespoke per-skill clips because the skills are stat-deaf in the demo.
- Warblade flinch / hit-react only if the recorded route actually shows the player taking damage. If the goal is to show stat-to-damage, target/enemy flinch is more valuable than a warblade flinch.

P3 (post-demo / reuse):
- Bespoke Q, E, R, F animations.
- Warblade death.
- Extra combo variants, charge variants, weapon flourish variants.

S2 - Q/E/R/F decision
Do not make bespoke Q/E/R/F animations for the demo. Use either LMB reuse, a single generic skill-cast, or animation-free VFX/UI choreography. The thesis is environment + vertical slice, the game side is only 20%, and per-direction cleanup is the real bottleneck. Bespoke Q/E/R/F clips would spend cleanup time on stat-deaf moments while the combat proof lives only in LMB.

S3 - State-first plan
P1 idle:
- State first: guarded breathing / ready stance, same sword silhouette, high-top-down, no pose exaggeration.
- Animation: 6-8 frame breathing idle loop at 10-12 fps, keep first frame.
- Interpolation: no.
- Directions: generate S, SE, E, NE, N; mirror SW, W, NW with flipX.

P1 run:
- State first: mid-run / forward lean with one leg planted and sword stable.
- Animation: 8 frame run loop at 10-12 fps, keep first frame.
- Interpolation: no; reroll bad directions instead of forcing interpolation.
- Directions: generate S, SE, E, NE, N; mirror SW, W, NW.

P1 LMB basic strike:
- State first: strike windup, sword clearly readable before release. Also create a strike/contact or follow-through state if PixelLab gives a clean pose.
- Animation: 6-8 frame basic warblade slash, fast anticipation -> contact -> short recovery. This should read at gameplay scale.
- Interpolation: yes for the riskiest directions if available: windup -> contact/follow-through. If recovery looks weak, accept a short snap back to idle in gameplay rather than spending another bespoke recover set.
- Directions: generate S, SE, E, NE, N; mirror SW, W, NW. If time collapses, polish only the camera-visible directions first, then fill the rest by mirror.

P2 generic skill-cast:
- State first: combat cast / weapon raised / energy-ready pose, not a unique Q/E/R/F identity.
- Animation: 6-8 frame reusable cast flourish, can be paired with VFX for all four skills.
- Interpolation: optional; use guarded idle -> cast pose only if the start jump is ugly.
- Directions: generate S, SE, E, NE, N; mirror SW, W, NW. If very short on time, E/SE/S first for video framing.

P2 flinch:
- State first: recoiling / guard-broken flinch pose.
- Animation: 4-6 frame hit reaction returning to guarded stance.
- Interpolation: useful as idle -> flinch or flinch -> idle, but only after P1 is already captured cleanly.
- Directions: generate S, SE, E, NE, N; mirror SW, W, NW. Skip entirely if player damage is not in the recording.

S4 - Budget and realism
Generation budget is not the constraint. P1 is roughly 45-90 generations including states, 15 required V3 direction jobs, and reroll buffer. P2 generic cast plus flinch is roughly another 35-75 generations. With 874 generations left, budget is safe.

Cleanup risk is the real cost. Expected cleanup: idle 1-2 hours, run 3-5 hours, LMB strike 4-8 hours, generic cast 3-5 hours, flinch 2-4 hours. P1 is realistic inside 5 days if it is produced golden-path-first and judged at gameplay scale. P1 + one P2 is realistic. P1 + generic cast + flinch + bespoke Q/E/R/F is not realistic.

Minimum video-beautifying set:
- Full P1: idle, run, LMB strike.
- Optional single P2: generic skill-cast if Q/E/R/F button presses visibly need body motion.
- No death, no bespoke Q/E/R/F, no combo expansion before OBS proof is captured.

TEK CUMLE TAVSIYE: Ship idle + run + one excellent LMB strike first, add one reusable generic cast only if P1 is already clean, and leave bespoke Q/E/R/F, warblade flinch, and death for post-demo unless the actual recording proves they are visible.
