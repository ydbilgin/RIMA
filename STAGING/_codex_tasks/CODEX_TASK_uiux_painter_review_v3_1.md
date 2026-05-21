# Codex Task — RimaUnifiedPainter UI/UX Spec Polish Review v3.1

## Role

Final polish pass. Your v3 review identified 2 contradictions in the open questions. v3.1 fixes those only — no panel body changes. Verify clean.

## Inputs

- **Spec to review (v3.1):** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md`
- **Your v3 review:** `STAGING/CODEX_DONE_uiux_painter_review_v3.md`
- **Source:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (DO NOT modify)

## What v3.1 fixed

1. **Open Question 1** — was contradicting V14 fix by recommending `Assets/Editor/MapDesigner/Rules/CollisionRulesSO.cs`. Now reads: `Assets/Editor/CollisionRulesSO.cs` (predefined Editor asm, matching body). Asset instance location remains free.
2. **Open Question 8 caller list** — was missing `ConfigureAssetPackColliders` (source line 1799-1801, button at line 704-709). Now lists 6 callers including this bulk-bootstrap caller, with explicit note that SO rules apply there too to prevent drift.

## Review questions

### P. Polish verification

- **P1.** Open Question 1: does v3.1 now consistently say `Assets/Editor/CollisionRulesSO.cs` everywhere (body + question)? Cite lines.
- **P2.** Open Question 8: does v3.1 caller list include all 6 collision-resolve callsites (Paint, DrawOutline, ConfigureAssetPackColliders, PaintWallWithConnections, UpdateWallConnectionsAt, Save/Load)? Any still missing?
- **P3.** Any NEW contradiction introduced by the v3.1 polish (e.g., the Iter Log claim doesn't match the doc body)?

### G. Per-panel final verdict

- Panel 1, 2, 3, 4: LIVE / LIVE_WITH_MINOR_NOTES / NEEDS_REVISION / REJECT.

### H. Overall final verdict

LIVE / LIVE_WITH_MINOR_NOTES / NEEDS_REVISION / REJECT.

If LIVE or LIVE_WITH_MINOR_NOTES, the spec is approved for downstream implementation task.

## Output contract

Write to **`STAGING/CODEX_DONE_uiux_painter_review_v3_1.md`**:

```markdown
# Codex Review — UIUX Painter Redesign v3.1 (polish)

## STATUS
{LIVE / LIVE_WITH_MINOR_NOTES / NEEDS_REVISION / REJECT}

## Section P — Polish verification
- P1: ...
- P2: ...
- P3: ...

## Section G — Per-panel final verdict
- Panel 1: ...
- Panel 2: ...
- Panel 3: ...
- Panel 4: ...

## Section H — Overall final verdict
{...}

## Quotable summary
{1-2 lines}
```

## Hard rules

- DO NOT edit any source file.
- DO NOT write code.
- Effort: medium (compact polish review, not full re-audit).
- Output: `STAGING/CODEX_DONE_uiux_painter_review_v3_1.md` + wrapper-mandated CODEX_DONE_*.md.

## Workflow

1. Read v3.1 spec full (Iter Log + Open Questions 1 + 8 + body line 292-298 + caller spec section).
2. Confirm no contradictions remain.
3. Verify ZWSP count = 0 (byte-level): `python -c "print(open(r'STAGING/UIUX_PAINTER_DESIGN_DRAFT.md','rb').read().count(b'\xe2\x80\x8b'))"`.
4. Answer P/G/H.
5. Write review.
