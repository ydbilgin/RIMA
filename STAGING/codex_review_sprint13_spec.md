# Sprint 13 Spec Review Request
**Date:** 2026-05-17 S87 night
**Reviewer:** Codex (GPT-5.5, profile auto-select)
**Spec to review:** `STAGING/codex_brush_sprint13_production_hardening.md`
**Owner of review:** Opus orchestrator (verdict drives next action: PASS → impl, FAIL → fix → re-review)

---

## 1. Scope of review

Read `STAGING/codex_brush_sprint13_production_hardening.md` end-to-end. Provide a structured verdict:

- **PASS** — spec implementable as-is, ready for Codex/Opus impl
- **PASS-WITH-CONDITIONS** — minor gaps acceptable to defer, list them
- **FAIL** — blockers exist, spec needs fix before impl

For each issue, classify severity: **P0 BLOCKER** (must fix), **P1 GAP** (high-priority defer), **P2 NIT** (polish).

---

## 2. Required verification matrix (you MUST check each)

### A. Sprint 9-12 LIVE additive-only

Confirm Sprint 13 doesn't break Sprint 9-12 LIVE method signatures. Specifically:
- `RoomTemplateSO` (Sprint 10 LIVE) — Sprint 13 only ADDS `walkableGrid` field + `IsWalkable(Vector2Int)` method. Existing fields unchanged. **VERIFY.**
- `PropDefinitionSO` (Sprint 12 LIVE) — Sprint 13 only ADDS `variantSprites` array + `PickVariant(int)` method. Existing fields unchanged. **VERIFY.**
- `PropPlacementData` (Sprint 12 LIVE) — Sprint 13 only adds `variantIndex` field, activates `rotationSteps` (declared in Sprint 12). **VERIFY.**
- `PropFootprintValidator` (Sprint 12 LIVE) — Sprint 13 changes `IsWalkableTile` body to use `template.IsWalkable(tilePos)` instead of `cameraBounds.tileRect` fallback. This IS a behavior change in body but keeps signature. **VERIFY this is acceptable per Sprint 12 PASS-WITH-CONDITIONS Condition 1.**
- `PropPlacer` (Sprint 12 LIVE) — Sprint 13 adds `R` hotkey + variant pick on click. Existing flow unchanged. **VERIFY.**
- `MapDesignerBrushWindow` (Sprint 12 LIVE) — NOT touched by Sprint 13. **VERIFY.**
- `WallOverlayPainter` (Sprint 11 LIVE) — NOT touched by Sprint 13. **VERIFY.**

### B. Karar #143-D/E/K compliance

- **#143-D:** L3 wall sprite scale stays 1.0 if `useNativeBucketVariantPath`. Sprint 13 doesn't touch L3 paint logic. **VERIFY.**
- **#143-E:** Sprite sortingLayer respects `Patch`/`Detail`/`Accent`/`Props`/`Entities` order. Sprint 13 PropSorterRuntime uses `propDef.sortingLayerOverride` — confirm this doesn't violate the layer hierarchy. **VERIFY.**
- **#143-K:** FeatureMask multiplier tolerance 0.01 for 8-bit Color alpha. Sprint 13 doesn't touch FeatureMask. **VERIFY.**

### C. Karar #144 (weaponless body) orthogonality

Sprint 13 is purely brush/props/room layer. Should not touch character animation, weapon attachment, body sprite. **VERIFY no inadvertent character-side code paths.**

### D. File path lock (§5 — 11 files in spec)

Spec §5 lists:
- 5 new source files
- 5 modified source files
- 9 test files
- 2 asset files

Verify no other paths needed. Check specifically:
- Does `PropRegistrySO` runtime path (non-editor build) need a `PropDefinitionPostprocessor.cs` (AssetPostprocessor) to auto-populate `propId` with AssetDatabase GUID at import time? If yes, this file is MISSING from §5.
- Does `PropPlacer` need a new `Editor` namespace import for `R` hotkey wiring?

### E. OQ resolutions (§6 — 5 OQ)

- OQ1 (Bridson radius tile basis) — verify consistent with Sprint 12 PropFootprintValidator distance check.
- OQ2 (rotation pivot bottom-left, footprint swap) — verify PropPlacer preview reflects rotated footprint correctly.
- OQ3 (variant pool deterministic seed = tilePosition.GetHashCode()) — verify Vector2Int.GetHashCode() is stable across Unity versions.
- OQ4 (propId IS GUID auto-populated via AssetPostprocessor) — but no AssetPostprocessor in §5 file list. **GAP?**
- OQ5 (Collider default layer) — verify no Sprint 14+ collision layer assumption.

### F. PropRegistrySO runtime semantics

Spec §2.6 has `RebuildIndex()` using `UnityEditor.AssetDatabase` inside `#if UNITY_EDITOR`. At runtime (player build), AssetDatabase doesn't exist. The spec says "Runtime uses `propId` for lookup" but the code doesn't show runtime population. **GAP — clarify how runtime populates the dictionary from `allProps` list:**
- Option A: Runtime build iterates `allProps`, uses `propDef.propId` field directly to populate dict.
- Option B: Lazy lookup — no dict, linear search through `allProps` each time.
- Option C: Pre-build dict at editor-time, serialize as inspector list of pairs.

Recommend resolution.

### G. Test coverage

Spec §3 lists 24 tests. Verify:
- All 7 forward-path items have test coverage
- Stream B (perf/undo/dep report/library) coverage adequate
- No regression tests for Sprint 9-12 (those still 282/282)

### H. Acceptance criteria

Spec §7 has 11 acceptance criteria. Verify:
- All measurable
- "Dependency report generated to STAGING/RIMA_BrushTool_Dependencies.md" — confirm `STAGING/` is the right location vs `Assets/Documentation/` or repo root
- "10-room library scaffolding ready" — confirm `.gitkeep` + `PropRegistry_v1.asset` + stub `RoomBankSO_Library_v1.asset` is enough scaffolding (user authors 10 rooms after)

### I. Memory anchors

Spec lists `[[combat-feel-research-combined]]` as anchor. Sprint 13 doesn't touch combat. **NIT — remove anchor or clarify rationale.**

---

## 3. Output format

Write verdict to: `STAGING/codex_review_sprint13_spec_DONE.md`

Top: `VERDICT: PASS | PASS-WITH-CONDITIONS | FAIL`

Then verification matrix A-I with each item: ✅ VERIFIED / ⚠️ GAP / ❌ BLOCKER + 1-line evidence.

Then if any P0/P1 issues, list them with:
- File: path + line ref (if applicable)
- Issue: what's wrong
- Fix: what spec should say

Then a 1-paragraph "Next action" summary for orchestrator.

---

## 4. Dispatch parameters

- Effort: high
- Background: yes
- Profile: laurethgame (auto-select via cx_dispatch.py LastRefresh)
- Estimated time: 10-20 min

---

End of Sprint 13 spec review task v1.0.
