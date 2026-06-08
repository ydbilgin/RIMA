# Council — DEEP / DESIGN / HADES-RESEARCH lens (Gemini 3.1 Pro High)

You are the deep design advisor. Research the Hades convention carefully and give a concrete, implementable interaction-prompt spec. Numeric where possible.

## Context
RIMA — high 3/4 top-down ARPG (refs Hades / Children of Morta / Diablo III). The "chamber" is a class-select room: 10 class figures stand on glowing cyan oval pedestals; the player walks among them; a cyan rift PORTAL exits; a training dummy lets you preview a class. Player approaches a figure and presses [G] to "attune"/become that class. Some classes are unlocked (Warblade, Elementalist), others are LOCKED and cost Echo (currency) to unlock.

## The user's directive
"It will be like Hades — the interaction prompt will be at the BOTTOM of the screen, at one fixed spot. Validate HOW it should be with a council, then we implement."

## Current state (problem)
There is already a fixed bottom-center screen prompt (anchor (0.5,0.13), shows e.g. "WARBLADE — Bürün" / "RANGER — [KİLİTLİ] {cost} Echo" / "Rift'e Gir"). BUT there are ALSO floating world-space NAME labels above each figure (the class name floats above the pedestal). So the class name shows twice when near a figure, and the floating one moves around as the player walks → the user perceives "interact text doesn't stay at a fixed spot."

## Questions — give a concrete spec
Q1 HADES CONVENTION (research): How does Hades ACTUALLY present interaction prompts and the identity of what you're interacting with? Is the use-prompt screen-anchored (fixed) or world-anchored above the object? Where exactly on screen? How does Hades tell the player WHAT they're about to interact with (name/label) vs the ACTION (button + verb)? Cite the real behavior.

Q2 FLOATING NAMES — keep or kill? For THIS 10-figure select gallery, should the floating world-space figure name labels be REMOVED entirely so ALL text lives in the fixed bottom prompt (pure Hades)? The tradeoff: without floating names, the player can't tell which figure is which until they walk up and read the bottom prompt. Is that acceptable for a class-select gallery (the player walks a line of distinct-looking figures), or do we need SOME at-a-glance identity (e.g. a small icon, a consistent label, or color-coding)? Give your recommended design and justify via genre/UX.

Q3 BOTTOM PROMPT CONTENT + FORMAT: Specify the exact one-line format for each case so identity + action + lock/cost read clearly at the fixed bottom spot:
- unlocked figure (e.g. Warblade)
- locked figure with a cost (e.g. Ronin, 150 Echo) — affordable vs not-affordable
- the exit portal
- the training dummy
Give the literal strings (Turkish UI; the game ships TR+EN) and where the [G] key-cap glyph sits.

Q4 PLACEMENT + BEHAVIOR + STYLING: exact screen anchor + offset from bottom (in a 640x360 / 1920x1080 reference), panel size, fade timing, one-prompt-at-a-time rule, what shows when the player is between two interactables, and key-cap styling. Validate or improve the current values (anchor 0.5,0.13; size 440x60; fade 0.08s; interaction radius 1.9).

Keep it tight and implementable — an orchestrator will turn this into a spec then code.
