# Anchor QC Master S43
**Date:** 2026-04-29  
**Merged from:** `_QC_REPORT_S43.md`, `_GEMINI_REVIEW_S43.md`, `_CODEX_VERIFY_S43.md`  
**Scope:** Consolidated anchor QC only. Root-level anchor PNG convenience copies are intentional and excluded from cleanup.

## Cross-Roster Notes
- Technical cohesion is strong: 10/10 anchors are 128x128, height std dev is 1.04px, outline RGB std dev is 2.7-3.9.
- Gemini visual review gave overall roster PASS / same-artist feeling 9/10.
- Exact locked-hex accent scan is too strict for many PixelLab outputs; prefer visible 2-4px color-family accent clusters over exact hex equality.
- STYLE_BIBLE accent table is stale versus CURRENT_STATUS / MASTER_KARAR #163.

## Warblade
- Date: 2026-04-28 source reports
- Verdict: CONDITIONAL PASS
- Score: Technical 3/4; Gemini visual PASS
- Notes: Canvas/outline/identity pass. Strict #66AAFF scan failed, but Gemini saw cold-blue sword/chest crack clearly. Keep as style norm unless Claude wants exact accent touch-up.

## Elementalist
- Date: 2026-04-28 source reports + 2026-04-29 Codex Task 1
- Verdict: PASS / CANDIDATE PASS
- Score: Technical 4/4; `elementalist_anchor_mu.png` Task 1 score 7/7
- Notes: Existing anchor technically passes. New `elementalist_anchor_mu.png` was visually reviewed by Codex + Gemma and passed style cohesion; candidate disposition depends on Claude. Base orb should stay neutral enough for Unity runtime element overlay.

## Shadowblade
- Date: 2026-04-28 source reports
- Verdict: PASS WITH MINOR ACCENT WARN
- Score: Technical 2 PASS + 2 WARN /4; Gemini visual PASS
- Notes: Canvas/outline pass. Purple accent weak by strict scan, but Gemini saw style/identity pass. No style drift.

## Ranger
- Date: 2026-04-28 source reports
- Verdict: CONDITIONAL PASS
- Score: Technical 2 PASS + 1 WARN + 1 FAIL /4; Gemini visual PASS
- Notes: Canvas/outline pass. Strict gold accent failed; identity/quiver/bow mostly readable. Later edit should only add thin #FFCC00 bow crack, no silhouette changes.

## Ravager
- Date: 2026-04-28 source reports
- Verdict: CONDITIONAL PASS
- Score: Technical 3/4; Gemini visual PASS
- Notes: Canvas/outline/identity pass. Strict blood-red accent failed. Codex later classified Ravager scar as blood/fury class scar, not rift remnant.

## Ronin
- Date: 2026-04-28 source reports
- Verdict: PASS
- Score: Technical 4/4; Gemini visual PASS
- Notes: Canvas/outline/accent/identity pass. CodexVerify notes white accent may mix drawn blade/scabbard in scan; visually confirm sheathed identity before animation lock.

## Brawler
- Date: 2026-04-28 source reports
- Verdict: FAIL / NEEDS FIX
- Score: Technical 3/4, but identity conflict overrides
- Notes: Gemini said dark enough, but CodexVerify measured face/chest too light: face RGB(216,153,110), brightness 159.7; Gunslinger is darker at 126.7. Regen or targeted skin fix needed for DEEP EBONY identity.

## Gunslinger
- Date: 2026-04-28 source reports
- Verdict: CONDITIONAL PASS
- Score: Technical 3/4; Gemini visual PASS
- Notes: Canvas/outline/identity pass. Strict brass accent failed; later edit can add small #FFB800 pistol cracks only. Skin/hair identity acceptable in visual review.

## Hexer
- Date: 2026-04-28 source reports
- Verdict: WARN / NEEDS CURRENT RECHECK
- Score: Technical 2 PASS + 2 WARN /4; Gemini visual PASS with hand concern
- Notes: Lantern accent weak by strict scan. Gemini suggested flip because lantern looked right-hand; CodexVerify argues viewer-right likely character-left in south-facing anatomy, so flip is not automatically required. Current user-provided newer Hexer candidate should supersede this old anchor QC before final lock.

## Summoner
- Date: 2026-04-28 source reports
- Verdict: FAIL / NEEDS CURRENT RECHECK
- Score: Technical 3/4 but strict accent failed; Gemini visual PASS with staff-hand concern
- Notes: Strict #22FF88 scan failed in first QC; CodexVerify found green pixels at wider tolerance and says staff/hand anatomy needs manual visual check. Current user-provided newer Summoner candidate should supersede this old anchor QC before final lock.

## Reviewer Checklist
- PASS: All 10 classes have one section.
- PASS: Technical/Gemini/CodexVerify conflicts preserved.
- PASS: Root-level convenience anchor PNG copies excluded from cleanup.
- PASS: No Unity Assets touched.
