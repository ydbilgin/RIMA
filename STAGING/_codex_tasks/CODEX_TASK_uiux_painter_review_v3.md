# Codex Task — RimaUnifiedPainter UI/UX Spec Review v3

## Role

Re-review v3 after Opus applied v2's NEEDS_REVISION feedback. **No code writing.** Final pass: confirm 4 hard fixes + 5 minor fixes landed correctly, no new regression.

## Inputs

- **Spec to review (v3):** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md`
- **Your v1 review:** `STAGING/CODEX_DONE_uiux_painter_review_v1.md`
- **Your v2 review:** `STAGING/CODEX_DONE_uiux_painter_review_v2.md`
- **Existing source:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (DO NOT modify)
- **Painter asmdef context:** Painter lives at `Assets/Editor/RimaUnifiedPainterWindow.cs` in the predefined Editor assembly. `Assets/Editor/MapDesigner/RIMA.MapDesigner.Editor.asmdef` has `autoReferenced: false` and references only `RIMA.Runtime` + `Unity.InputSystem` + `Unity.2D.Tilemap.Extras` — it does NOT see the painter type.

## What v3 claims to fix (4 hard + 5 minor from v2)

### Hard fixes
1. **V1 (SceneVisibilityManager):** All 3 callers (`Hide`, `Show`, `IsHidden`) + 2 more (`DisablePicking`, `EnablePicking`) now use named arg `includeDescendants: true`. No `includeChildren` API references in spec body.
2. **V5 (ZWSP literal):** All 3 occurrences (lines that were 20, 378, 533 in v2) now use ASCII escape text `​`, not literal invisible U+200B character. Byte-level verification: spec file has 0 occurrences of UTF-8 sequence `E2 80 8B`.
3. **V14 (asmdef placement):** `CollisionRulesSO.cs` script moved to `Assets/Editor/CollisionRulesSO.cs` (predefined Editor assembly, same as painter). Asset instance can live anywhere. Asmdef rationale documented in spec.
4. **S4 (banner state matrix):** Replaced single confusing 7-row table with 3 axis-separated tables (Tilemap / Parent / Biome), each axis has its own state.

### Minor fixes
5. **V3 (cache key):** `Dictionary<int, ResolvedCollider>` replaced with typed `Dictionary<PreviewCacheKey, ResolvedCollider>` where `PreviewCacheKey` is a readonly struct implementing `IEquatable` + `HashCode.Combine`.
6. **N3 (resolveReason GC):** Spec says resolveReason computed only on cache miss, const/static strings preferred, dynamic strings only for rule patterns.
7. **N4 (GetTargetParent drift):** Spec adds explicit refactor — `GetTargetParent` body becomes `Peek + create-if-null`, single resolve logic.
8. **N7 (SessionState namespace):** Key prefix changed from `painter.groupExpand.{name}` to `RIMA.UnifiedPainter.groupExpand.{name}` (window-unique).
9. **N8 (OpenPropertyEditor):** `Edit Rules SO` button uses `EditorUtility.OpenPropertyEditor`; fallback to Select+Ping with button label "Select Rules SO".

## Review questions

### W. Verification of v2 corrections (hard)

For each of the 4 hard items (V1, V5, V14, S4) and each of the 5 minor items (V3, N3, N4, N7, N8), confirm:

- **W1.** Fix applied in v3 spec body? Y/N + cite v3 line/section.
- **W2.** Fix is technically correct? Y/N + brief why.
- **W3.** Any new bug introduced by the fix?

### Z. Final spec-wide consistency

- **Z1.** Read the entire v3 spec end-to-end. Any internal contradictions between sections (e.g., Panel 1 uses includeDescendants but Panel 2 mockup or matrix still says includeChildren somewhere)?
- **Z2.** Open question 8 in v3 lists 4-6 caller sites where `CollisionResolver.Resolve` must be threaded. Confirm those line citations match the actual source file (lines 1453-1458, 1606-1617, 2613-2614, 2730-2731, 2460-2545). Are any callers missing?
- **Z3.** The `PreviewCacheKey` struct relies on `HashCode.Combine` (Unity uses System.HashCode since recent versions). Confirm this API is available in the Unity Editor target (Unity 6 / 2022 LTS / .NET Standard 2.1).
- **Z4.** `EditorUtility.OpenPropertyEditor` — confirm this API exists and is documented (or note Unity version dependency).

### G. Per-panel re-verdict

For each Panel 1-4 emit ONE of:
- `LIVE` — implementation-ready as-spec'd, ship
- `LIVE_WITH_MINOR_NOTES` — small polish items, OK for downstream task
- `NEEDS_REVISION` — material rework, do another iteration
- `REJECT` — concept not implementable

### H. Overall verdict

`LIVE` / `LIVE_WITH_MINOR_NOTES` / `NEEDS_REVISION` / `REJECT`.

If LIVE or LIVE_WITH_MINOR_NOTES, this spec is approved for downstream implementation task and the iter loop ends here.

## Output contract

Write to **`STAGING/CODEX_DONE_uiux_painter_review_v3.md`**:

```markdown
# Codex Review — UIUX Painter Redesign v3

## STATUS
{LIVE / LIVE_WITH_MINOR_NOTES / NEEDS_REVISION / REJECT}

## Section W — v2 fix verification
### Hard
1. V1 (SceneVisibilityManager): W1=... W2=... W3=...
2. V5 (ZWSP): ...
3. V14 (asmdef): ...
4. S4 (banner matrix): ...

### Minor
5. V3 (cache key): ...
6. N3 (GC): ...
7. N4 (GetTargetParent drift): ...
8. N7 (SessionState namespace): ...
9. N8 (OpenPropertyEditor): ...

## Section Z — Spec-wide consistency
- Z1: ...
- Z2: ...
- Z3: ...
- Z4: ...

## Section G — Per-panel re-verdict
- Panel 1: ...
- Panel 2: ...
- Panel 3: ...
- Panel 4: ...

## Section H — Overall verdict
{...}

## Quotable summary
{1-3 lines}
```

## Hard rules

- DO NOT edit any source file.
- DO NOT write code.
- DO answer every W/Z/G/H question.
- Effort: high.
- Output file: `STAGING/CODEX_DONE_uiux_painter_review_v3.md` (primary) + wrapper-mandated CODEX_DONE_*.md.

## Workflow

1. Read v3 spec full.
2. Cross-reference v1+v2 reviews.
3. Byte-level verify ZWSP cleanup (the spec file should have ZERO `E2 80 8B` sequences).
4. Skim relevant source ranges (line 19 enum, line 93 SerializeFields, line 394 header, line 422 panel, line 766 palette, line 1453/1606/2613/2730 caller sites, line 2250 GetTargetParent, line 2290 GetOrCreateGroupParent).
5. Confirm asmdef placement reasoning: `Assets/Editor/` predefined assembly vs `Assets/Editor/MapDesigner/RIMA.MapDesigner.Editor.asmdef` (autoReferenced: false).
6. Answer W/Z/G/H.
7. Write review to `STAGING/CODEX_DONE_uiux_painter_review_v3.md`.

## Verification helper (bash)

```bash
# ZWSP byte sequence count — must be 0:
python -c "print(open(r'STAGING/UIUX_PAINTER_DESIGN_DRAFT.md','rb').read().count(b'\xe2\x80\x8b'))"
```
