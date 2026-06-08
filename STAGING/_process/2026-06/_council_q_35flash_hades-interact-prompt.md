# Council — LEAN / SHIP-FAST / OVER-ENGINEERING CRITIQUE lens (Gemini 3.5 Flash High)

You are the pragmatic advisor. Give the LEANEST path and call out over-engineering. Terse, numeric.

## Context
RIMA chamber = class-select room. 10 class figures on cyan pedestals; player walks up and presses [G] to attune; some classes locked (cost Echo). There is ALREADY a fixed bottom-center screen prompt (anchor (0.5,0.13), 440x60, dark+cyan-border, [G] keycap, fontSize 24, fade 0.08s, shows "WARBLADE — Bürün" etc, triggers within radius 1.9). There are ALSO floating world-space class-NAME labels above each figure (only nearest shown). The class name thus shows twice when near a figure, and the floating one moves around → user wants EVERYTHING at the fixed bottom spot (Hades-style).

## Questions — leanest fix
Q1 The leanest way to get "all interaction text at one fixed bottom spot": is it simply "delete the floating CreateWorldText name labels and keep the existing bottom prompt (which already shows name+action)"? Is that 90% of the work for 10% of the effort? What's the minimum diff?
Q2 Without floating names, the player can't tell which figure is which until walking up. Is adding any at-a-glance identity (icons, color-coding, spotlight) OVER-ENGINEERING for a demo class-select room, or is "walk up → bottom prompt names it" good enough? Be blunt.
Q3 The existing bottom prompt already branches per case (unlocked/locked+cost/portal/dummy). Is re-formatting the strings enough, or is anything else needed? Flag any over-build (hysteresis, dynamic-width auto-sizing, scale animation) — is that worth it for a demo or skip?
Q4 Validate or simplify the current values (anchor 0.5,0.13, 440x60, fade 0.08s, radius 1.9). What's the single smallest set of changes to ship a clean Hades-feel bottom prompt? What should we NOT build?

Be blunt about what to skip.
