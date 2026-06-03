# Review: UI Pack wired into CharacterSelectScreen (B4) — CX

ACTIVE RULES: (1) think before judging (2) report only real issues as file:line + concrete fix (3) reviewer, not writer — do NOT rewrite (4) BLOCKED if you can't read the file.

NLM ACCESS: not needed.

## Context
On-brand UI Pack sprites (Resources `UI/RIMA/Pack/*`) were wired into the procedural class-select screen. Build is green. Verify the wiring is correct and won't cause UI/interaction bugs.

## File
`Assets/Scripts/UI/CharacterSelectScreen.cs`

## Review focus — PASS/FAIL + file:line
1. **Backdrop:** CSS_Background Image gets bg_seal_keep (Simple, white, SetAsFirstSibling). Confirm it stays BEHIND title/panels and doesn't eat raycasts meant for cards/button.
2. **Card frame raycast:** the new CardFrame overlay Image is `raycastTarget = false` so the card's Button still gets clicks. Confirm — this is the #1 risk (a raycast-blocking overlay would make cards unclickable).
3. **Sibling ordering:** pedestal behind portrait (SetAsFirstSibling on center panel), card frame on top (SetAsLastSibling). Confirm the portrait/text remain visible through the transparent frame center.
4. **Null-guards:** every Resources.Load<Sprite> guarded; missing sprite -> falls back to prior visual, never throws, never white box. Confirm.
5. **Selection/lock visuals preserved:** SelectClass / ApplyCardLockVisual still drive the card bg tint (frame is a separate overlay). Confirm no regression to selected/locked card appearance.
6. **Button:** start button bg swapped to button_9slice (Sliced) while keeping the per-class accent color tint. Confirm Sliced + border works and the accent tint still applies.

## Output
Top line `STATUS: PASS` or `STATUS: FAIL`, then findings. Tight.
