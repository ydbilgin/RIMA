# Council — Gemini 3.1 Pro (High): DEEP UX + art-direction lens

RIMA = pixel-art 2D ARPG (Unity URP, top-down 3/4 "Hades/Children of Morta" look). Dark "Shattered Keep" aesthetic: cyan #00FFCC (rift/seal energy), void-purple #3A1A4A, warm-orange #E89020 (secondary accent). We are polishing the run UI/UX and will generate UI art. Give deep, opinionated UX + art-direction guidance across 3 topics. Concrete > vague.

You MAY read these for grounding (file tools): F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\SkillOfferUI.cs, Assets\Scripts\UI\SkillBarUI.cs, Assets\Scripts\Camera\CameraZoom.cs, Assets\Scripts\Core\RewardPickup.cs.

## A — Camera scroll-zoom (pixel-perfect)
Setup: PixelPerfectCamera, refRes 640×360, PPU64, window 1280×720+. Recent attempt: while scrolling, disable PixelPerfectCamera + drive orthographicSize smoothly; ~0.12s after scroll stops, re-enable PPC snapping to the nearest crisp pixel ratio. User says it "still feels bad/wrong" and the framing looks too zoomed-out.
- What is the RIGHT zoom feel for a pixel-art ARPG? Rank: (1) fully smooth ortho (sacrifice strict pixel-perfect), (2) smooth-while-scroll then EASE onto the nearest crisp level (no hard pop), (3) discrete crisp steps with animated transitions. Justify the pick for THIS art style.
- The crisp pixel-perfect levels are chunky → snapping at rest jumps. How to avoid the ugly snap while keeping art crisp? Give a concrete recipe (lerp speed, settle timing, easing onto crisp, default/min/max zoom values for an ARPG — is default too far out?).

## B — Reward draft + HUD redesign (UX)
Current: 3 skill cards (180×260px) appear lower-center on a 1920×1080-ref width-match canvas → tiny, off-center, fonts 9–14px (unreadable). Cards JITTER on hover (HoverScale 1.08 on the whole card root + duplicate hover handlers → scale moves the click target → enter/exit flicker loop). "SEÇ"(Select) click reportedly does nothing (suspected: timeScale=0 during draft + scaled-time confirm coroutine stalling). Skill bar slots are 20/16px (tiny).
- Design the IDEAL reward-draft screen: layout (truly centered, card size relative to a 1920 ref, spacing, title/desc hierarchy, readable font sizes), and the premium card hover effect that does NOT jitter (lift/scale a child visual not the raycast target, bring-to-front, glow ring, dim others, subtle tilt?). Recommend 2-3 hover treatments fitting the cyan/void aesthetic + the selection-confirm effect.
- Skill bar (6 slots: LMB/RMB/Q/E/R/F) — recommended slot sizes/layout/placement for readability.

## C — UI art production plan (we will generate via image-gen)
We can generate UI art (on-brand, pixel-art-adjacent, NOT photoreal). Decide WHAT is worth generating vs doing in uGUI (9-slice/gradient/solid). Propose a concrete asset MANIFEST: card frame/backing, skill icons, HUD panel/backing, skill-bar slot frame (hex), tier/rarity chips — each with pixel dimensions and on-brand description. Pack vs individual. Keep it MINIMAL but premium (don't over-produce). Note which skill icons matter most first.

Output: tabular where possible. Feeds an Opus synthesis + then production + implementation.
