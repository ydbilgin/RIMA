# Task: How do real action games render the HELD WEAPON during a melee swing? (web research, sourced)

ACTIVE RULES: (1) think before answering (2) ground claims in real sources — cite GDC talks, dev breakdowns, frame analyses, articles, forum/dev posts with URLs (3) distinguish what you VERIFIED from what is general practice (4) BLOCKED if you cannot find sources, but give best-available.
NLM ACCESS: N/A — do not query NLM. Direct-read only this file.
RESPOND INLINE (captured to AGY_DONE_ydbilgin.md). Do NOT write to scratch/external files. This is the ONLY agy task running now (no clobber).
IMPORTANT: your internet research is the whole point of this task — be thorough and cite.

## The question to settle (the user pushed back on a claim)
A prior synthesis claimed: "during a melee swing, hide the held weapon sprite entirely and replace it with a slash-arc VFX." The user's instinct: **in Hades the weapon is NOT fully hidden** — it stays visible and is augmented by a trail/smear. The user proposes instead: keep the weapon **locked/baked to the hand**, and during the swing **REDUCE its visibility via VFX (alpha/blend/smear) rather than fully removing it** — feels more natural; fully removing the weapon "feels illogical."

We need a DEFINITIVE, sourced answer.

## Research these (with sources / frame breakdowns where possible)
1. **Hades (Supergiant)** specifically — during a melee attack (e.g. Stygian Blade), is the weapon sprite hidden, fully visible, or visible-with-a-trail/smear? Find dev talks, art breakdowns, frame-by-frame analyses. The user says it stays visible — confirm or refute.
2. Survey other action games and classify each as **(H) weapon hidden → effect only**, **(V) weapon stays visible + trail/smear**, or **(S) single smear pose**:
   - Dead Cells, Hyper Light Drifter, Children of Morta, CrossCode, Moonlighter, Enter the Gungeon (melee), 2D Zelda (ALttP/Minish Cap), Brawlhalla, and any pixel-art top-down ARPG you can source.
3. **The animation/game-feel principle**: smear frames, motion blur, weapon trails, and "sell the hit with VFX." Is the held weapon usually KEPT (and trailed) or REMOVED during the fast frames? What do practitioners (e.g. "Art of Screenshake", GMTK game-feel, animation principle write-ups) actually recommend?
4. For a **top-down 8-direction PIXEL game with a SEPARATE weapon sprite locked to the hand**: which is the norm — full hide vs keep-visible-with-trail vs reduce-opacity? Any examples of the "reduce opacity + trail" middle path specifically?

## Deliverable (sourced, decisive)
- A direct verdict on the Hades claim (hidden vs visible-with-trail) with the best source you found.
- A short classification table (game → H / V / S + 1-line + source).
- The industry norm for the held weapon during a swing, and whether the user's "keep weapon, reduce visibility + VFX" is a legitimate, common approach (yes/no + why).
- A 1-paragraph recommendation for RIMA's case (top-down 8-dir pixel, separate weapon locked to hand).
Lead with the verdict. Cite URLs.
