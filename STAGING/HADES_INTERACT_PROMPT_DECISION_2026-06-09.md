# HADES-STYLE INTERACT PROMPT — DECISION (2026-06-09)

Council: cx (feasibility) + ax-3.1-Pro (deep/Hades-research) + ax-3.5-Flash (lean) → Opus synthesis.
Process: `STAGING/_process/2026-06/_council_{cx,q_31pro,q_35flash}_hades-interact-prompt.md` + CODEX_DONE.md.
User directive: "Hades gibi — interact ekranın altında, hep aynı yerde. Council ile doğrula, sonra yap."

## Core agreement (all 3 advisors)
The position-varying text the user sees = the floating WORLD-space class NAME labels (EchoLabel_{cls}) above each figure. The fixed bottom prompt ALREADY exists and already shows identity+action. Showing the name twice (floating + bottom) is a double-text anti-pattern. **FIX = delete the floating figure name labels; identity lives only in the fixed bottom prompt.** (Note: real Hades is world-anchored, but we honor the user's explicit fixed-bottom request.)

## DECISION
1. **DELETE the floating figure name labels** (EchoLabel). cx file:lines — creation `~1010-1014`, field assign `~1023-1024`, refresh/SetActive `~1725-1730`, EchoStation fields `~2060-2061`, stale cleanup prefix `~841-842`. Low risk: label is display-only; selection/unlock/attune/highlight all use classType/statue/pedestal/light, not the label.
2. **Keep the single fixed bottom prompt** (CreatePromptLabel/ChamberPromptPanel/ShowPrompt) + proximity loop + pedestal/light highlight (those don't depend on the label).
3. **Unify prompt string format** — `[G] {IDENTITY} — {ACTION} ({cost})` (ShowPrompt strips `[G]` and renders it as the key-cap box):
   - Unlocked figure: `[G] WARBLADE — Bürün`
   - Locked & affordable: `[G] RONIN — Kilidi Aç ({cost} Echo)`
   - Locked & NOT affordable: `RONIN — Kilitli ({cost} Echo)` — no `[G]` keycap, text tinted red/grey
   - Portal: `[G] RİFT — Gir` (⚠️ ensure the Loc string includes `[G]` or the keycap won't show — cx flag on `chamber_select.prompt.enter_rift`)
   - Dummy: `[G] KUKLA — Sınıf Seç`
4. **Widen the prompt panel** 440→~560 wide (cx: fontSize 24 + no-wrap risks clipping long TR strings). Keep height 60.
5. **Fade** 0.08→~0.14s (3.1-Pro: 0.08 feels like a snap; 0.14 smoother). NO scale animation (lean).
6. **Keep** anchor (0.5,0.13), interaction radius 1.9, the [G] keycap system.
7. **Keep the dummy HP bar + KUKLA label** — that is combat feedback near the dummy (you hit it → see HP drop), NOT an interaction prompt. Only the FIGURE class-name labels are removed.

## REJECTED as over-engineering for a demo (ax-3.5-Flash, agreed)
- Hysteresis / anti-flicker filter (add only if playtest shows flicker between adjacent figures).
- Dynamic-width auto-sizing of the panel (fixed ~560 is fine).
- Scale / elastic appear animation.
- Pedestal hover shaders, attune screen-shake, proximity color/volume filters.
- At-a-glance identity icons/spotlights — figures have distinct silhouettes; walk-up→bottom-prompt names them. Good enough for demo.

## Net
A small deletion pass (kill EchoLabel) + string-format unify + panel widen + slightly slower fade. No new UI system. Identity + action now always at one fixed bottom spot = the user's Hades request.

## Side note (Automata Games tweet ref the user shared)
The shared X post = a dev prototyping their game's road/traffic system in python/pygame, visualizing ISO vs FLAT grids side-by-side to debug an iso-coordinate bug faster. Relevance to RIMA: (a) validates our "smart-test" approach (prototype tricky spatial logic outside the engine to understand it), (b) directly applicable to RIMA's iso-layout pain (straight grid columns render diagonal on screen). FUTURE OPTION: a tiny python/pygame iso-layout visualizer to nail chamber figure spacing/positions before doing it in Unity. Not actioned now — logged as an idea.
