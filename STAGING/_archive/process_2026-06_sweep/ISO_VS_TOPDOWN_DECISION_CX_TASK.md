# CX TASK — DECISION: Isometric vs Top-Down 3/4 for RIMA (pick one, justify)

ACTIVE RULES: (1) think before answering (2) be decisive — give ONE verdict (3) justify with the real tradeoffs (4) note assumptions.

NLM ACCESS (optional, may be expired — don't block): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"

## THE DECISION
RIMA (Unity URP 2D action-roguelite) must commit to ONE camera/floor projection. Pick: **ISOMETRIC** or **TOP-DOWN 3/4**. Give a clear verdict + justification + rough cost.

## FACTS (verified)
- **Locked direction (PROJECT_RULES + NLM canon):** "HIGH TOP-DOWN 3/4 (~70-80deg from horizon, Hades / Children of Morta / Diablo III ref), NO iso projection math, NO true 45deg diamond." Years of decisions assume this.
- **Character art:** 10 classes x 8 directions ALREADY produced (PixelLab) as TOP-DOWN 3/4 frontal sprites in `Assets/Resources/Characters/`. They are NOT isometric. Committing to true iso would likely require redrawing/regenerating all of them at the iso (shallow corner) angle — large art cost.
- **Combat:** cursor-aim (mouse-to-world). Top-down 3/4 maps screen X/Y ~ world X/Y intuitively; true iso rotates controls/aim onto a diagonal.
- **Code coupling (from prior audit):** the floor Grid is currently `cellLayout=Isometric` AND the cliff system (CliffAutoPlacer iso neighbor vectors, DirectionalCliffTile, CliffFaceIdleAnimator, CliffEdgeDustEmitter), WalkabilityMap, and several editor tools are iso-coupled. So STAYING iso = ~0 code migration; GOING top-down 3/4 = MEDIUM code migration (biggest risk = cliffs).
- **Look:** 3 concept images at `STAGING/floor_perspective_concepts/` — `01_isometric.png`, `02_topdown_3q.png`, `03_wallless_improved.png` (wall-less floating-island, char + cyan sword). View if you can; all three look good.

## ANSWER THESE
1. **Verdict:** ISO or TOP-DOWN 3/4? One sentence.
2. **Cost comparison:** Weigh "iso = no floor-code migration BUT redraw all character art at iso angle" vs "top-down 3/4 = keep all character art BUT medium floor/cliff code migration." Which total cost is smaller and why?
3. **Gameplay fit:** cursor-aim action-roguelite — which projection serves fast aimed combat + readability better, and why?
4. **Canon:** does flipping to iso contradict the locked direction enough to matter? Is there any reason the locked direction was wrong?
5. **Risk:** the single biggest risk of YOUR recommended choice, and how to mitigate.
Output a short decisive markdown. No code changes.
