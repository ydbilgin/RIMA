# T1 Director-bleed fix — ADVERSARIAL COUNCIL REVIEW

ACTIVE RULES: (1) think before judging (2) min code, no speculation (3) surgical — only the listed file (4) BLOCKED if unclear.
You are an ADVERSARIAL reviewer. Do NOT reflexively approve. Try to find a bug, edge case, or regression. Be specific. Report <=10 lines to the done file: VERDICT (PASS / FAIL / RISK) + concrete findings + (if FAIL/RISK) the exact fix.

## Context (RIMA demo, 19 June)
Top-down ARPG, Unity 6000.3.6f1. CANON LOCKED: 8-direction sprites (do NOT propose 4-cardinal). Surgical only, NO refactor (demo scope).

## Problem (T1 Director-bleed)
Director Mode overlay canvas = `sortingOrder 950` (DirectorMode.cs:700). Reward draft (SkillOfferUI) canvas = `sortingOrder 1050` (SkillOfferUI.cs:172) — so the reward card renders ON TOP, but the Director overlay (full-screen dimmer alpha 0.35 + frames) stayed VISIBLE behind/around it -> visual "bleed"/clutter, reward looked dim/cluttered. The existing hide (`SetOverlayVisible(false)`) only fired on the dual-class draft path (DirectorMode.cs:2069-2073), NOT for reward drafts opened by other paths (RewardPickup -> SkillOfferUI -> UIManager.OpenSkillOffer).

Verified facts: `UIManager.IsAnyOverlayOpen` (UIManager.cs:41) = `tabOpen || settingsOpen || skillOfferOpen || skillCodexOpen || pauseOpen` — INCLUDES skillOfferOpen. `UIManager.Instance` is a static singleton (UIManager.cs:14). SkillOfferUI calls `UIManager.Instance.OpenSkillOffer()` on open (SkillOfferUI.cs:91/149). Director overlay is shown only in Director state (SetState: l.317 true on Director, l.324 false otherwise).

## Fix applied (DirectorMode.cs, 2 edits, compiles 0-error)
Edit 1 — in `Update()` right after `UpdateTelemetryDisplay(false);`:
```
UpdateOverlayBleedGuard();
```
Edit 2 — new method after `SetOverlayVisible`:
```csharp
private void UpdateOverlayBleedGuard()
{
    if (overlayCanvasGo == null) return;
    bool blockingOpen = UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen;
    bool shouldShow = State == DirectorModeState.Director && !blockingOpen;
    if (overlayCanvasGo.activeSelf != shouldShow)
    {
        overlayCanvasGo.SetActive(shouldShow);
    }
}
```

## Attack it — answer concretely
1. Does this FULLY kill the bleed (reward draft, skill codex, pause, settings, tab all open over Director)? Any overlay path that does NOT set a UIManager flag and would still bleed?
2. Regression: any scenario where the Director overlay is WRONGLY hidden (user in Director state, expects overlay, but it vanishes) or WRONGLY restored mid-draft?
3. Interaction with the dual-class path (l.2069-2073 sets State=Test then SetOverlayVisible(false)) — does the guard fight it or double-handle? When the dual-class ClassSelectionUI is open (does it set a UIManager flag? likely NOT), is there still a bleed for THAT specific UI?
4. `overlayCanvasGo.SetActive` toggled in Update every frame — any GC/perf/event-thrash concern? (it guards on activeSelf != shouldShow).
5. Is there a simpler/more-correct fix (e.g. CanvasGroup alpha vs SetActive, or lowering sortingOrder)? Only if clearly better.

Do NOT run Unity (code review only). The fix already compiles clean.
