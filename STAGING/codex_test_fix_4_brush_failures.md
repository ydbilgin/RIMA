# Codex Task — 4 Pre-existing Brush Test Failures Fix
**Tarih:** 2026-05-16 S86 SPRINT11_CLOSED
**Mode:** Codex impl + verify (cx_dispatch.py background, --effort high)
**Branch:** master (worktree current)

---

## Context

Sprint 11 closed PASS (193/197 EditMode PASS). 4 pre-existing test failures are scope-out of Sprint 11 review but block 100% green. Fix them now.

Each test has a stated symptom in `CURRENT_STATUS.md`:
- `FeatureEdgeSmoothingTests` — "2 vs 8" expected delta
- `FeatureMaskSOTests` — density issue
- `HitPauseDriverTests` — timing flake
- `NaturalFeatureGraphTests` — perf budget 57ms vs 20ms

**Test files (paths confirmed by orchestrator grep 2026-05-16):**
- `Assets/Tests/EditMode/Editor/FeatureEdgeSmoothingTests.cs`
- `Assets/Tests/EditMode/Editor/FeatureMaskSOTests.cs`
- `Assets/Tests/EditMode/Editor/HitPauseDriverTests.cs`
- `Assets/Tests/EditMode/Editor/NaturalFeatureGraphTests.cs`

---

## Required output

For each test:
1. **Diagnose root cause** — read the test + the SUT (system under test); report which side is wrong.
2. **Fix** — prefer fixing the test if the SUT behavior is correct & locked; fix the SUT if the SUT is provably wrong; if perf budget is unrealistic, document why and propose a new budget with the user-visible cost.
3. **Verify** — run the test, confirm PASS.
4. **Full suite** — `dotnet build` PASS + RIMA.Tests.EditMode 197/197 PASS.

Write findings to `STAGING/codex_test_fix_4_brush_failures_DONE.md` with this structure per test:
```
### {TestName}
- Symptom (from CURRENT_STATUS):
- Root cause:
- Side fixed (TEST or SUT or BOTH):
- Diff summary (1-3 bullets, file:line):
- Verified PASS: yes/no
```

End the report with:
```
## Suite verification
- dotnet build: PASS/FAIL
- EditMode 197/197 PASS: yes/no (state counts)
- Sprint 11 LIVE files touched: yes/no (list any)
- Karar #143-D/E/K compliance maintained: yes/no
```

---

## Constraints

- **DO NOT** touch Sprint 11 LIVE files: `Assets/Scripts/MapDesigner/Composition/*`, `WallOverlayPainter.cs`.
- **DO NOT** touch character pipeline files (S86 active production: PixelLab Character States rollout — separate concern).
- **DO NOT** modify Karar #143-D/E/K enforcement (walkableMask, wallProximityCurve, featureMaskMultiplier defaults).
- **DO NOT** add new packages or change asmdef references.
- If a test requires a Tilemap/TilemapRenderer pair, follow the S86 fix pattern (Tilemap **before** TilemapRenderer, see `STAGING/codex_brush_sprint11_natural_engine.md` for reference).
- If unsure, write the diagnosis to the report and HALT — do not change SUT logic without a clean root cause.

---

## Routing

- Effort: **high**
- Background dispatch via cx_dispatch.py (orchestrator launches with `run_in_background: true`)
- Profile: laurethgame (auto-select)
- Expected duration: 15-30 min
