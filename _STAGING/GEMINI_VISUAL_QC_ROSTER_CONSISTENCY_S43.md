# GEMINI VISUAL QC — Roster Style Consistency (S43)

**Date:** 2026-04-28  
**Task type:** Visual QC — image analysis only, no file changes  
**Requested by:** Claude / RIMA project

---

## Context

RIMA is a 2D pixel art roguelite with 10 playable character classes. All characters should read as if they came from the same artist and the same game. The user suspects that **Elementalist** and **Brawler** look stylistically inconsistent compared to the rest of the roster.

The characters have been produced as pixel art anchors (128×128px) via PixelLab → `_STAGING/anchors/[class]/[class]_anchor.png`

---

## Your Task

### Step 1 — Load and examine all PixelLab anchor images

Load these 10 files:
- `_STAGING/anchors/warblade/warblade_anchor.png`
- `_STAGING/anchors/elementalist/elementalist_anchor.png`
- `_STAGING/anchors/shadowblade/shadowblade_anchor.png`
- `_STAGING/anchors/ranger/ranger_anchor.png`
- `_STAGING/anchors/ravager/ravager_anchor.png`
- `_STAGING/anchors/ronin/ronin_anchor.png`
- `_STAGING/anchors/gunslinger/gunslinger_anchor.png`
- `_STAGING/anchors/brawler/brawler_anchor.png`
- `_STAGING/anchors/hexer/hexer_anchor.png`
- `_STAGING/anchors/summoner/summoner_anchor.png`

---

### Step 2 — Evaluate roster consistency

For the full 10-character roster, score these dimensions:

| Dimension | What to check |
|---|---|
| **Outline style** | Thickness, single-color vs multi-color, consistency across all 10 |
| **Pixel density** | How much detail per character area — does anyone look more or less detailed? |
| **Color saturation** | Are all characters equally muted / vibrant? Any outlier that looks washed out or oversaturated? |
| **Silhouette readability** | At 128px, can you instantly read the class from silhouette alone? |
| **Art style feel** | Does every character feel like the same artist, same production quality, same tone? |
| **Camera/perspective** | Does the camera angle feel consistent across all 10? |

---

### Step 3 — Focus: Elementalist vs Brawler vs the rest

After examining all 10, specifically answer:

1. **Does Elementalist look like she came from the same artist as the other 9?**
   - If not: what exactly is different? (outline, color, style, saturation, detail level, camera, other)
   - How severe is the divergence? (minor drift / noticeable / clearly from different production)

2. **Does Brawler look like he came from the same artist as the other 9?**
   - If not: what exactly is different?
   - How severe is the divergence?

3. **Which 2-3 characters from the roster look most consistent with each other?** (the "core style norm")

5. **Which characters besides Elementalist and Brawler, if any, also diverge?**

---

### Step 4 — Recommendation

For each divergent character:
- Is the issue fixable with a reprompt at the PixelLab stage? (style ref + stricter description)
- What specific change would bring it in line? (e.g. "outline needs to be thicker", "reduce saturation", "camera angle is lower than others")

---

## Output Format

```
## VISUAL QC REPORT — Roster Consistency S43

### Roster Norm (core style standard)
[Which 2-3 characters define the "correct" style]

### Elementalist — PASS / MINOR DRIFT / NOTABLE DRIFT / FAIL
Issue: [what specifically is different]
Stage: [concept / pixellab / both]
Fix: [what to change]

### Brawler — PASS / MINOR DRIFT / NOTABLE DRIFT / FAIL
Issue: [what specifically is different]
Stage: [concept / pixellab / both]
Fix: [what to change]

### Other divergent characters (if any)
[Class — severity — issue]

### Overall roster consistency score
[1-10, where 10 = all same artist]

### Priority fix list
1. [most urgent]
2. ...
```

---

## What NOT to do
- Do not change any files
- Do not suggest complete redesigns unless the divergence is severe
- Do not evaluate game design or skill systems — visual only
- Do not hallucinate image contents — only report what you actually see
