# Gate-Slot Implementation Review — Commit `f63ac34c`

**Reviewer:** ax-Opus-4.6 (static analysis, read-only)
**Commit:** `f63ac34c` — "feat(rooms): authored NW/N/NE exit slots with deterministic door selection"
**Spec:** `STAGING/GATESLOT_DECISION_2026-06-07.md`
**Date:** 2026-06-07T00:35Z
**Files:** 28 changed, +1265 / −146

---

## 1 · Selection Logic Exactness

**Spec rule:** `1→N` · `2→NW+NE (center EMPTY)` · `3→NW+N+NE`. Returned-door order = graph child index.

### Analysis

[TryResolveExitSlotsForDoorCount](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs#L116-L187) implements exactly:

| doorCount | Code path | Returned list | Matches spec? |
|-----------|-----------|---------------|---------------|
| 1 | L135-138: `slots[1] ?? ClosestToHorizontalCenter(slots)` | `[N]` | ✅ |
| 2 | L141-170: `left=slots[0], right=slots[2]` → `[NW, NE]` | `[NW, NE]` — center skipped | ✅ |
| 3 | L173-183: `[slots[0], slots[1], slots[2]]` | `[NW, N, NE]` | ✅ |

**Returned-door order = choice index:** [BuildExitDoors](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs#L735-L748) iterates `selectedSlots` with index `i`, which is used directly as `choiceIndex` in `CreateExitDoorObject(i, doorTypes[i], position)`. The `doorTypes` list is populated from `CurrentChoices` in-order at [RoomRunDirector:L206-210](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L206-L210). `child[0]→door[0]→leftmost slot` contract is preserved.

**Off-by-one risk:** None found. `CountSlots(slots) < doorCount` guard at L130 prevents selecting more slots than exist. The `slots` array is always exactly length 3 (L95).

**2-door fallback when NW or NE missing:** L152-168 handles degraded case: uses `N + surviving_wing`, ordering by x-position (left-to-right). This maintains the child-index = left-to-right spatial contract. ✅

### Finding

| Sev | Location | Note |
|-----|----------|------|
| — | — | No issues. Selection logic is correct and complete. |

---

## 2 · Fallback Correctness

**Spec rule:** Insufficient slots → legacy north-row fallback + warning. Never a half-state.

### Analysis

[IsoRoomBuilder.BuildExitDoors](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs#L735-L748):

```csharp
if (lastTemplate != null && lastTemplate.TryResolveExitSlotsForDoorCount(count, selectedSlots))
{
    // ALL doors placed on slots, early return
    return doors;
}
// OTHERWISE: entire fallback row
```

The slot path does an **early return** (L747). The fallback path (L750-795) places **all** doors on the legacy computed row. There is no code path where some doors are placed on slots and others on the row — the two paths are mutually exclusive.

**Warning dedup:** L751-756 uses a `HashSet<string>` keyed by `$"{templateName}:{count}"`, so each template+doorCount combo warns exactly once. This is an **improvement** over the old code which keyed only by template name (potentially missing different count scenarios). ✅

### Finding

| Sev | Location | Note |
|-----|----------|------|
| — | — | No half-state possible. Fallback is clean and complete. |

---

## 3 · RoomBankSO Pool Filter

**Spec rule:** `ValidExitSlotCount >= node.childCount` — template excluded from pool if insufficient.

### Analysis

[RoomBankSO.Pick(roomType, seed, requiredExitSlots)](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L32-L59):

```csharp
int required = Mathf.Clamp(requiredExitSlots, 0, 3);
// ...filter eligible where template.ValidExitSlotCount >= required
if (eligible.Count == 0) return null;
```

Called from [RoomRunDirector:L184](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L184):
```csharp
roomBank.Pick(CurrentRoomType, runSeed + CurrentNodeId, choices.Count)
```

**Null template path:** If `eligible.Count == 0` → returns `null` → RoomRunDirector L185-188 falls through to `fallbackTemplate`. If that is also null → L192 logs error and returns. **Safe.**

**Can it empty the pool at some depth?** Yes — theoretically if all templates of a given `RoomType` have `ValidExitSlotCount < 3` and graph produces a 3-child node, pool is empty → fallback template used. This is acceptable by design (spec says "runtime fallback" is the belt-and-suspenders layer). However:

### Finding

| Sev | Location | Finding | Fix |
|-----|----------|---------|-----|
| NOTE | [RoomBankSO.cs:51-53](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L51-L53) | When `eligible` is empty, returns `null` silently. No warning logged at the bank level — the warning only appears later at `IsoRoomBuilder` if the fallback template also can't resolve slots. For diagnostics, a `Debug.LogWarning` here noting that pool filtering eliminated all candidates for `(roomType, requiredExitSlots)` would aid debugging. | Add optional warning on empty eligible pool. Low priority. |
| NOTE | [RoomBankSO.cs:45](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L45) | `ValidExitSlotCount` is a computed property that calls `ResolveExitSlots()` (allocates `DoorSocket[3]` + iterates `doorSockets` list) on every `Pick()` call, for every template in the list. With 15+ templates × frequent room transitions this is negligible, but if the bank grows to 100+ templates, consider caching or a serialized field. | Future optimization note only. |

---

## 4 · Validator New Rules

**Spec rules reviewed:**
1. Slot walkable + north non-walkable (IsDoorEdge) ✅ — enforced by [IsValidExitSlot](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs#L189-L200) at L196-197
2. South corridor (2 cells) ✅ — [RoomTemplateValidator:L394-L400](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs#L123-L132) `ERR_EXIT_SLOT_NO_SOUTH_CORRIDOR`
3. Slot separation ≥3 ✅ — [ValidateSlotDistinctnessAndSeparation](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs#L270-L303) `ERR_EXIT_SLOTS_TOO_CLOSE`
4. 3 slots DISTINCT + South exit forbidden ✅ — `ERR_EXIT_SLOTS_NOT_DISTINCT` + `ERR_SOUTH_EXIT`
5. Spawn-to-slot ≥4 ✅ — [ValidateSlotSpawnDistance](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs#L305-L328) `ERR_PLAYER_TOO_CLOSE_TO_EXIT_SLOT`
6. NW/NE Y alignment ≤2 (WARN) ✅ — [ValidateSlotWarnings:L332-L338](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs#L330-L338)
7. Slot near prop <2 (WARN) ✅ — same method, L340-L362
8. Flood-fill reachability (WARN) ✅ — [ValidateSlotWarnings:L364-L380](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs#L364-L380)

### False-positive risk on crescent/zigzag

The separation check uses `Vector2Int.Distance` (Euclidean), so slots at different Y values that are ≥3 apart diagonally will pass. A crescent with NW at y=8 and NE at y=6 both on the north edge would:
- Pass separation (distance > 3) ✅
- Trigger WARN_EXIT_SLOT_WINGS_MISALIGNED (|Y| > 2) correctly ✅
- Each slot individually validated for IsDoorEdge + south corridor ✅

No false-positive risk identified for crescent/zigzag shapes.

### Performance of reachability check (flood-fill)

**FloodFill** is called from:
- `RoomTemplateValidator.ValidateSlotWarnings` — editor-only (menu item / save hook)
- `RoomSocketQCTool.CollectIssues` — editor-only (QC audit menu item)

**NOT called at runtime.** Confirmed by grep: `RoomTemplateValidator` is referenced only in [RoomBankSO.ValidateAll()](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L83), [RoomTemplateMenu](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateMenu.cs#L60), and [RoomTemplateSaver](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs#L118) — all editor code. ✅

### Finding

| Sev | Location | Finding | Fix |
|-----|----------|---------|-----|
| NOTE | [RoomSocketQCTool.cs:L177-L208](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomSocketQCTool.cs#L177-L208) | QCTool `CollectIssues` also runs flood-fill, duplicating the validator's logic. Both are editor-only, so no perf issue, but the code is duplicated. | Consider delegating QC audit to `RoomTemplateValidator.Validate()` to DRY up. Low priority. |

---

## 5 · Fix Sockets Migration — Legacy door_W/E Removal

**Spec rule:** Old `door_W_01` / `door_E_01` side socket IDs removed.

### Analysis

The old `TryFindEdgeCell` method that generated `door_E_01` and `door_W_01` sockets has been **deleted entirely** from [RoomSocketQCTool.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomSocketQCTool.cs) (diff lines 556-605 show removal).

**Grep results for `door_W_01`, `door_E_01`, `door_S_01`:** **Zero hits** across all `.cs` and `.asset` files. Clean removal. ✅

**Chamber skip intact:** [RoomSocketQCTool.cs:L69](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomSocketQCTool.cs#L69) still checks `if (path == CharSelectPath)` and skips. [RoomTemplateSocketTests.cs:L13](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/EditMode/Room/RoomTemplateSocketTests.cs#L13) still has the same `CharSelectPath` exclusion. ✅

### Finding

| Sev | Location | Note |
|-----|----------|------|
| — | — | Clean migration. No dangling references. |

---

## 6 · UnifiedMapDesigner Preview Changes

### Analysis

Changes in [UnifiedMapDesigner.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs):

1. **Door color differentiation** (L380-385 diff): Replaces single `Color.cyan` rect with `SlotPreviewColor(slotIndex)` returning distinct colors per NW/N/NE slot. Marker inflated from 4f to 7f for visibility.

2. **Label drawing** (L385): `DrawPreviewLabel` renders text above each marker. New `GUIStyle` allocation per call — but this is editor OnGUI, acceptable.

3. **ENTRY label** (L394): Green "ENTRY" label above spawn dot.

4. **Legend** (L397-400): "1:N  2:NW+NE  3:NW+N+NE" label below grid preview.

**GUI-layout breakage risk:** All additions use `EditorGUI.DrawRect` and `EditorGUI.LabelField` (immediate-mode GUI), called inside the existing draw loop at line ~376-392. These are purely additive rendering calls — no `GUILayout` or `EditorGUILayout` that could affect the layout stack. The legend label at `gridRect.yMax + 2f` adds 16px below the grid rect, which is safe as long as the caller allocates enough vertical space.

### Finding

| Sev | Location | Finding | Fix |
|-----|----------|---------|-----|
| NOTE | [UnifiedMapDesigner.cs:L397-L400](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L394-L400) | The 16px legend label is drawn at `gridRect.yMax + 2f` — if the Rooms tab's scroll area doesn't account for this extra height, the label may be clipped. No functional impact, cosmetic only. | Verify in editor that legend is visible; if clipped, increase reserved height by ~18px. |
| NOTE | [UnifiedMapDesigner.cs:L426-L434](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L421-L434) | `new GUIStyle(EditorStyles.boldLabel)` is allocated per `DrawPreviewLabel` call (up to 4 times per template preview — 3 doors + 1 entry). In an OnGUI repaint loop this creates mild GC pressure. | Cache the style as a static field. Very low priority — editor-only code. |

---

## 7 · Tests Added

### EditMode Tests

**[SampleRoomLibraryGeneratorTests](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/EditMode/Editor/SampleRoomLibraryGeneratorTests.cs):**
- L47: `Assert.GreaterOrEqual(room.ValidExitSlotCount, 1)` — validates all generated templates have ≥1 valid slot ✅
- L48: `Assert.IsNotNull(room.ResolveExitSlots()[1])` — validates N slot always present ✅
- L49: `Assert.IsFalse(room.doorSockets.Exists(door => ... door.direction == DoorDirection.South))` — no south exits ✅
- **Does NOT explicitly test center-empty for 2-door case in EditMode** — but this is covered by the PlayMode test below.

**[RoomTemplateSocketTests](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/EditMode/Room/RoomTemplateSocketTests.cs):**
- Renamed to `HaveAuthoredNorthExitSlots` ✅
- Validates all 25 on-disk templates: slot count ≥1, N slot present, separation ≥3, distinctness, no south exits ✅
- Chamber skip intact ✅

### PlayMode Tests

**[IsoRoomBuilderTests](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/PlayMode/Room/IsoRoomBuilderTests.cs):**

**`BuildExitDoors_ReturnsOneObjectPerDoorType` (L44-63):**
- 2-door case: asserts `doors[0].position.x = 0.5f` (NW) and `doors[1].position.x = 4.5f` (NE)
- **Center-empty assertion:** L62-63:
  ```csharp
  Assert.That(doors[0].transform.position.x, Is.Not.EqualTo(2.5f).Within(0.01f));
  Assert.That(doors[1].transform.position.x, Is.Not.EqualTo(2.5f).Within(0.01f));
  ```
  ✅ **This explicitly asserts that neither door in the 2-door case occupies the center (N) position.**

**`BuildExitDoors_UsesAuthoredSlotMappingForOneTwoAndThreeDoors` (L67-93):**
- 1-door: `oneDoor[0].position.x = 2.5f` (center = N slot) ✅
- 2-door: `twoDoors[0].position.x = 0.5f, twoDoors[1].position.x = 4.5f` (NW + NE) ✅
- 3-door: all three at 0.5, 2.5, 4.5 (NW, N, NE) with correct index order ✅
- **Child-index order:** Door names verify index (`ExitDoor_0_Combat`, `ExitDoor_1_Elite`, etc.) and positions are left-to-right ✅

### Test fixture template

The test fixture [CreateTemplate](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/PlayMode/Room/IsoRoomBuilderTests.cs#L133-L149) creates a 5×5 room with all 3 slots at positions (0,4), (2,4), (4,4) — exactly NW, N, NE. `walkableGrid = null` means all cells are walkable, so `IsValidExitSlot` will verify:
- `IsWalkable((x, 4))` = true ✅
- `!IsWalkable((x, 5))` = true (out of bounds = not walkable) ✅
- South corridor: `(x, 3)` and `(x, 2)` walkable = true ✅

**All 3 slots are valid** — the test fixture is correctly constructed for gate-slot testing.

### Finding

| Sev | Location | Finding | Fix |
|-----|----------|---------|-----|
| — | — | Tests cover all 7 acceptance criteria: 1/2/3 door selection, center-empty for 2-door, child-index order, NW/N/NE slot presence on all templates, separation rules. | — |

---

## Summary Findings Table

| # | Sev | File:Line | Finding | Suggested Fix |
|---|-----|-----------|---------|---------------|
| 1 | NOTE | [RoomBankSO.cs:51-53](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L51-L53) | Silent null return when pool filter eliminates all templates; diagnostic warning would help | Add `Debug.LogWarning` on empty eligible list |
| 2 | NOTE | [RoomBankSO.cs:45](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs#L45) | `ValidExitSlotCount` computed property allocates on each call in pool filter loop | Cache or serialize — only if bank grows large |
| 3 | NOTE | [RoomSocketQCTool.cs:177-208](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomSocketQCTool.cs#L177-L208) | Flood-fill + slot validation duplicated between QCTool and Validator | Delegate QCTool audit to Validator.Validate() |
| 4 | NOTE | [UnifiedMapDesigner.cs:397-400](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L394-L400) | Legend label at yMax+2 may clip if scroll area doesn't reserve 18px | Verify in editor; increase reserved height if needed |
| 5 | NOTE | [UnifiedMapDesigner.cs:426-434](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L421-L434) | GUIStyle allocated per DrawPreviewLabel call | Cache as static field — very low priority |

---

## Verdict

### ✅ PASS-WITH-NOTES

The implementation is **faithful to the spec** across all 7 review focus areas:

1. **Selection logic** — `1→N, 2→NW+NE, 3→NW+N+NE` is exact with correct left-to-right child-index ordering and graceful degradation. No off-by-one or reorder risks.
2. **Fallback** — mutually exclusive slot-path vs row-path; no half-state possible. Warning dedup improved.
3. **Pool filter** — correctly prevents undersized templates from being selected; null case chains to `fallbackTemplate` safely.
4. **Validator** — all 8 spec rules (5 MUST + 3 WARN) implemented correctly. Flood-fill is editor-only. No false-positive risk on odd shapes.
5. **Migration** — `door_W_01`/`door_E_01` cleanly removed; zero dangling references. Chamber skip intact.
6. **Designer preview** — additive rendering only; no layout-stack breakage risk.
7. **Tests** — all acceptance criteria covered including center-empty for 2-door and child-index order.

All 5 findings are severity NOTE (diagnostic/code-quality improvements, no bugs). No blocking issues.
