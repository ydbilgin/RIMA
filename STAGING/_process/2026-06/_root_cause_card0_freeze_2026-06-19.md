# Root Cause — Card_0 Opening-Kit Pick Freeze (timeScale stuck 0)

Date: 2026-06-19 · Read-only investigation · No Unity used · No code modified.

## TL;DR
The freeze is the classic **dual-owner stuck-0 race in `HitPauseDriver`**. During the draft pause
(`Time.timeScale == 0`), a basic-attack hit fires a hit-pause. `HitPauseDriver.PauseRoutine`
**captures `previousTimeScale = Time.timeScale` while it is still 0**, then "restores" to that
captured 0 one frame after the draft closes → permanent freeze until any other writer sets 1
(pause-menu open/close self-heals it). The skill (Iron Charge / Fireball) is **not** the writer;
the writer is `HitPauseDriver`, provoked by an attack input that leaks through during the paused draft.

## (a) Exact second writer of timeScale = 0
`Assets/Scripts/Combat/Juice/HitPauseDriver.cs:137` (`Time.timeScale = previousTimeScale;`)
restoring a value of **0** that was captured at line **124** (`previousTimeScale = Time.timeScale;`).
The pause is provoked by `Assets/Scripts/Combat/BasicAttack/MarkPulseBehavior.cs:129`
(`HitPauseDriver.Instance?.TriggerPause(0.03f)`) on a basic-attack melee hit.

## (b) Why Card_0 only (and both classes)
Card_0 is the first/auto-focused card and is confirmed via the SkillOfferUI confirm flow
(`SkillOfferUI.ConfirmPickRoutine`, runs on **unscaled** time while paused, ~0.36 s). The selecting
click / a held-or-buffered LMB on the **left/center** card screen position lands on (or carries into)
the player, and `PlayerAttack.Update()` (line 239, `attackAction.WasPressedThisFrame()`) has **no
overlay/timeScale gate** — input polling runs at timeScale 0 — so a basic-attack hit fires *during*
the still-paused draft. That hit calls `TriggerPause` while `timeScale==0`, so `previousTimeScale`
is captured as 0. Card_1/Card_2 sit off to the side, so their confirm click does not coincide with an
in-range melee hit landing inside the pause window. The mechanism is skill-agnostic — it reproduces
for Warblade (Card_0 = Iron Charge) and Elementalist (Card_0 = one of Fireball / Glacial Spike /
Chain Lightning; kit order is shuffled by `DraftManager.ShowOpeningKitDraft` `Random.Range` line 360,
so "Card_0" = the on-screen first card, not a fixed skill). Neither `IronCharge` nor the Elementalist
skills touch `Time.timeScale` in `Awake`/`Execute` — confirmed clean. So the shared trait is NOT the
skill; it is the Card_0 confirm-while-paused → attack-leak → hit-pause-captures-0 timing.

## (c) previousTimeScale = 0 capture site
`Assets/Scripts/Combat/Juice/HitPauseDriver.cs:124`
```
private IEnumerator PauseRoutine(float duration)
{
    previousTimeScale = Time.timeScale;   // <-- line 124: captures 0 if a pause-overlay is active
    Time.timeScale = Mathf.Clamp01(pauseTimeScale);
    ...
    Time.timeScale = previousTimeScale;   // <-- line 137: restores 0 → permanent freeze
}
```
The `OnDisable` (52) and `TriggerExecutePause` (105) restore paths share the same captured-0 hazard.

Corroborating prior evidence (already in repo):
- `STAGING/_process/2026-06/demo_fix_tasks/DONE_combatjuice_selout.md:34` — HitPauseDriver "captures
  previousTimeScale and restores it faithfully" (i.e. faithfully restores the bad 0).
- `.../DONE_combatjuice_selout.md:14` — damage numbers spawn while `timeScale=0` (draft pause), proving
  combat hit-resolution runs during the paused draft.
- `.../DONE_combat_scripted_verify.md:44` and `DONE_combat_p0.md:31` — documented "opening-draft
  timeScale=0 occasionally did not restore" symptom (previously misattributed to UI pause-stack).

## (d) Minimal surgical fix
Guard the capture so HitPauseDriver never adopts a paused (≤0) baseline, and never restores to 0.
This is the smallest correct change and addresses the root (capturing 0), not a band-aid.

File: `Assets/Scripts/Combat/Juice/HitPauseDriver.cs`

1) At the capture (line ~124), clamp the captured baseline to a sane running value:
```
// Never adopt a paused baseline (draft/pause overlay holds timeScale at 0).
// Capturing 0 here and restoring it later is the dual-owner stuck-0 freeze.
float current = Time.timeScale;
previousTimeScale = current > 0.0001f ? current : 1f;
```
(Replaces `previousTimeScale = Time.timeScale;` on line 124.)

2) Defense-in-depth — also skip starting a pause while an overlay already owns timeScale=0,
   so the juice doesn't fight UIManager. At the top of `TriggerPause` (line ~111):
```
if (Time.timeScale <= 0.0001f) return;   // an overlay (draft/pause) owns time — don't hit-pause
```

Either change alone fixes the freeze; (1) is the essential root fix, (2) prevents the wasted
coroutine and any sibling race. Lines 52, 105, 137 then can only ever restore a >0 value.

Regression risk: **Very low.** `pauseTimeScale` is already `Clamp01` (default 0), so a legitimate
hit-pause still freezes to 0 and restores to the >0 baseline — visuals unchanged. The only behavior
removed is "restore to 0" / "hit-pause while an overlay already paused the game," which are exactly
the bug. No public API change; no other caller reads `previousTimeScale`.

## (e) HitStop vs HitPauseDriver verdict
- **Live code: `HitPauseDriver` is the active single owner; `HitStop` is `[Obsolete]` and effectively
  dead on the player/skill path.** `HitStop.cs:11` carries `[System.Obsolete("Use HitPauseDriver —
  single timeScale owner")]`; its `DoFreeze` (63/65) still writes timeScale but is only referenced by
  Ronin skills (`RoninQuickdraw/FinalDraw/IaidoStance/SakuraVeil`), which are NOT in the Warblade/
  Elementalist demo kit and not on the Card_0 freeze path. The player basic-attack path routes through
  `HitPauseDriver` (`MarkPulseBehavior:129`), confirmed by the in-code comment there ("single timeScale
  owner — obsolete HitStop here caused a dual-owner stuck-0 race").
- **The report §11.9 is INVERTED vs the code.** `STAGING/report/RIMA_Senior_Design_Report.md:706` claims
  the fix delegated the single owner to **`HitStop`** and made **`HitPauseDriver` read-only**. The code
  is the exact opposite: `HitPauseDriver` writes (52/105/125/137); `HitStop` is the obsolete one. The
  report wording should be corrected to name `HitPauseDriver` as the single writer (report fix, not a
  code fix). The §11.x "defensive guard during draft" note (line 722) describes the residual race that
  the fix above actually closes.
- The 2026-06-18 "HitStop made obsolete in favor of HitPauseDriver" claim is **correct in the code**;
  the skill-equip path does not route through HitStop. The freeze is not a leftover HitStop write — it
  is HitPauseDriver capturing a 0 baseline.
