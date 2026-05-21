# Codex Task — RimaUnifiedPainter UI/UX Spec Review v2

## Role

You are re-reviewing the v2 revision of the spec. **You will not write code.** Verify Opus applied your v1 feedback correctly and check for any NEW risk introduced by v2 changes.

## Inputs

- **Spec to review (v2):** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md`
- **Your previous review (v1):** `STAGING/CODEX_DONE_uiux_painter_review_v1.md`
- **Existing source file:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (DO NOT modify)

## What changed v1 → v2 (Opus claims)

1. **Panel 1 visibility:** `SetActive` REPLACED with `SceneVisibilityManager.Hide/Show`. Lock durability claim REMOVED (tooltip explicitly says "editor state, not durable").
2. **Panel 2 cache key:** Expanded to `(prefab.InstanceID, category, chosenMode, scaleMult, rotationSteps, customSize/Offset hash, rulesSO GetDirtyCount)`.
3. **Panel 4 Ping:** New `PeekTargetParent()` helper introduced; never auto-creates `Props_Root`. Banner and Panel 1 Ping use this helper exclusively.
4. **Panel 3 ZWSP:** Display-only `name.Replace("_", "_​")` at render time; no invisible literals in source.
5. **Gizmo colorblind hardening:** Added shape hints (WallBlock cross diagonals, white/dark outlines) per E2.
6. **NEW (Codex F2 fix):** Two new helpers introduced — `GroupClassifier` (canonical 6-group source) and `CollisionResolver` (single dry-run + placement resolver). `ConfigureCollider` and `GetOrCreateGroupParent` delegate to these (1 line each).

The v2 doc has a final table mapping every v1 finding to its v2 fix. Verify them.

## Review questions — answer EACH explicitly

### V. Verification of v1 corrections

For each v1 finding listed in v2's "Düzeltme matrix" table, answer:
- **V1.** Did Opus actually apply the fix as described in the v2 spec body? Y/N + brief evidence (cite v2 spec section/line).
- **V2.** Is the applied fix technically sound (no new bug introduced)? Y/N + why.

Cover at least these 14 items from the matrix:
- Panel 1 SetActive → SceneVisibilityManager
- Panel 1 lock durability claim removal
- Panel 2 cache key expansion
- Panel 4 Ping side-effect fix (PeekTargetParent)
- Panel 3 ZWSP source-literal cleanup
- Gizmo colorblind hardening
- CollisionResolver single source
- GroupClassifier single source
- Foldout state preservation (showPrefabSettingsSection rename)
- Dictionary<string,bool> → SessionState
- Editor visibility durability tooltip
- OnHierarchyChange loop fix (time-polling removed)
- Banner OnGUI direct derivation (no extra dirty flag)
- BeginDisabledGroup pattern + asmdef placement

### N. New risk introduced by v2

The two new helpers (`GroupClassifier`, `CollisionResolver`) and `PeekTargetParent` are NEW in v2. Verify each:

- **N1.** `GroupClassifier`: spec says `GetOrCreateGroupParent` (line 2290) will delegate to it. Does this require modifying the existing function? Spec claims "non-destructive — current file untouched until implementation task." Confirm v2 itself does not force any source edit.
- **N2.** `CollisionResolver`: spec says `ConfigureCollider` (line 1901) will delegate. Same non-destructive claim. Confirm.
- **N3.** `CollisionResolver.Resolve` returns a `ResolvedCollider` struct including `resolveReason: string`. Does building this string every call create GC pressure visible in IMGUI repaints (the cache should help, but verify intent)?
- **N4.** `PeekTargetParent` duplicates the resolve logic from `GetTargetParent` minus auto-create. Drift risk: future change to one may not propagate. Acceptable, or should Opus refactor `GetTargetParent` to use `PeekTargetParent + create-if-null`?
- **N5.** `CollisionRulesSO` glob match (`name.StartsWith(pattern[:-1])` if endswith `*`): is this a sufficient pattern matcher, or should v2 spec mandate a more robust matcher (e.g., regex / Unity-style wildcards)?
- **N6.** Spec proposes `EditorUtility.GetDirtyCount(rulesSO)` as cache version key. Verify this API exists and returns a value useful for cache invalidation (vs. e.g., `AssetDatabase.GetAssetDependencyHash`).
- **N7.** `SessionState.GetBool($"painter.groupExpand.{name}", true)` — does this key namespace risk collision with other Unity packages? Should it include a window-unique prefix?
- **N8.** Spec proposes `Selection.activeObject = rulesAsset; EditorGUIUtility.PingObject(rulesAsset);` to "edit" the SO. Will this open the SO in the Inspector reliably, or does it need `Selection.activeContext` or `EditorUtility.OpenPropertyEditor`?

### S. Spec internal consistency

- **S1.** v2 says "GroupClassifier.CanonicalGroups = 6 names" but Panel 1 mockup also shows the labels matching exactly. Confirm `GetOrCreateGroupParent` (line 2290) currently produces the same 6 names: Walls / Statues / WallMountings / Patches / Mobs / FloorProps. (Read line 2290-2330 to verify.)
- **S2.** Open question 6 in v2 says "implementation Codex will write the helpers." This implies the spec, even when implemented, requires a small refactor inside `ConfigureCollider` and `GetOrCreateGroupParent`. Is this consistent with "Karpathy #3 surgical"? Or is it scope creep?
- **S3.** Panel 2 mockup shows "Override Mode: [Auto ▼]" — this is the existing `customCollisionMode` field. Spec does not introduce a new field for override; reuses existing. Confirm no shadowing.
- **S4.** Panel 4 banner state matrix has 7 conditions. Walk through each — are any unreachable / contradictory (e.g., "Parent: explicit + non-null" vs "Parent: explicit + null + Props_Root exists" — is "explicit + null" possible)?

### G. Per-panel re-verdict

For each Panel 1-4 emit ONE:
- `LIVE` — implementable, ship
- `LIVE_WITH_MINOR_NOTES` — small polish items
- `NEEDS_REVISION` — material rework needed
- `REJECT`

### H. Overall verdict

`LIVE` / `LIVE_WITH_MINOR_NOTES` / `NEEDS_REVISION` / `REJECT`.

If verdict is LIVE or LIVE_WITH_MINOR_NOTES, this spec is approved for downstream implementation task.

## Output contract

Write to **`STAGING/CODEX_DONE_uiux_painter_review_v2.md`**:

```markdown
# Codex Review — UIUX Painter Redesign v2

## STATUS
{LIVE / LIVE_WITH_MINOR_NOTES / NEEDS_REVISION / REJECT}

## Section V — Verification of v1 corrections
{14 items, V1+V2 each}

## Section N — New risk (v2-introduced)
- N1: ...
- N2: ...
- N3: ...
- N4: ...
- N5: ...
- N6: ...
- N7: ...
- N8: ...

## Section S — Spec internal consistency
- S1: ...
- S2: ...
- S3: ...
- S4: ...

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
- DO answer every V/N/S/G/H question.
- Effort: high.
- Output file: `STAGING/CODEX_DONE_uiux_painter_review_v2.md` (primary) + wrapper-mandated CODEX_DONE_*.md.

## Workflow

1. Read `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` (v2).
2. Cross-reference with your own v1 review `STAGING/CODEX_DONE_uiux_painter_review_v1.md`.
3. Targeted reads from `Assets/Editor/RimaUnifiedPainterWindow.cs` around lines 19, 31-95, 394, 422, 689-694, 766-927, 1849-1978, 2250-2330.
4. Answer V/N/S/G/H.
5. Write review to `STAGING/CODEX_DONE_uiux_painter_review_v2.md`.
