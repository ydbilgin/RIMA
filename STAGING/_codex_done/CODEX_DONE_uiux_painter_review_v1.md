# Codex Review - UIUX Painter Redesign v1

## STATUS
LIVE_WITH_NOTES

## Section A - Per-panel IMGUI implementability
### Panel 1
- A1: Y. Implementable in Unity IMGUI with `EditorGUILayout.BeginFoldoutHeaderGroup`, `EditorGUILayout.HorizontalScope`, `GUILayout.Button`, `Transform.Find`, `Selection.objects`, `Undo`, `SceneView.Frame`, `EditorGUIUtility.PingObject`, and `SceneVisibilityManager` for editor-only visibility/picking. Note: the proposed visibility button uses `SetActive`, which is not editor-only.
- A2: Low layout race risk if implemented as specified with normal GUILayout rows. Do not call hierarchy refresh callbacks from `OnGUI`, and do not mix manual `GUILayoutUtility.GetRect` row drawing with extra `EditorGUILayout` controls inside the same reserved rect.
- A3: Y, provided the new `BeginFoldoutHeaderGroup("6. Scene Organization")` is always followed by exactly one `EndFoldoutHeaderGroup`, matching the existing foldout pattern.
- A4: Y. Panel 1 does not need indent changes. If any child rows use indentation, wrap with local save/restore or balanced increment/decrement.

### Panel 2
- A1: Y. Implementable with `EnumPopup`, labels/read-only fields, `EditorGUI.BeginDisabledGroup`, `ScriptableObject`/`AssetDatabase`, and `Handles.DrawWireCube` from the existing `SceneView.duringSceneGui` callback.
- A2: Low layout race risk if `Handles` drawing stays in `OnSceneGUI` and the inspector body uses GUILayout normally. The preview helper must not instantiate, add components, or mutate assets during `OnGUI`.
- A3: Y, if it replaces the existing foldout body and keeps the existing one-to-one `BeginFoldoutHeaderGroup` / `EndFoldoutHeaderGroup` structure.
- A4: Y, but use scope-safe indent handling around any nested Custom fields. The current code balances lines 579-582; keep that discipline.

### Panel 3
- A1: Y. The current palette item already reserves one rect with `GUILayoutUtility.GetRect` and manually draws with `GUI`, `EditorGUI.DrawRect`, and `GUI.Label`; resizing to 110x130 and adding a badge is straightforward.
- A2: Low if each tile continues to reserve exactly one rect and all thumbnail/label/badge drawing happens inside that rect. Do not add `EditorGUILayout` calls inside `DrawPaletteItemButton` after reserving the rect.
- A3: N/A. Panel 3 has no foldout header.
- A4: N/A. Panel 3 has no indent state.

### Panel 4
- A1: Y. Implementable with `EditorGUILayout.HorizontalScope`, `EditorStyles.toolbar` or `helpBox`, `EditorGUIUtility.IconContent`, `GUILayout.Label`, `EditorGUI.BeginDisabledGroup`, and `EditorGUIUtility.PingObject`.
- A2: Low. Use normal GUILayout rows for the 3-line banner, or reserve a single rect and draw it manually; avoid mixing both in one banner block.
- A3: N/A unless the collapse control is implemented as a foldout header. A mini toggle button does not require `EndFoldoutHeaderGroup`.
- A4: Y. No indent changes required.

## Section B - Repaint loop
- B1: The proposed `OnHierarchyChange` + 0.5s `EditorApplication.update` refresh does not inherently cause infinite repaint loops. The safe pattern is: hierarchy event marks counts dirty and calls `Repaint`; update checks elapsed time and recomputes only if dirty or interval elapsed. Pitfalls: do not call `Repaint()` unconditionally every update forever, do not mutate hierarchy from count refresh, and do not call `OnHierarchyChange()` manually from `OnGUI`.
- B2: The proposed cache key `(prefab.InstanceID, mode, scaleMult, rotation)` is not sufficient. It must also account for `PaletteCategory` for Auto resolution, custom size/offset for Custom, and a version/dirty stamp for `CollisionRulesSO` rules. If the helper reads sprite visible bounds, caching matters: `Sprite.bounds` is cheap, but `GetSpriteVisibleBounds` can be costly when it scans texture pixels before hitting `visibleBoundsCache`.
- B3: Mostly correct. The banner can derive status directly in `OnGUI` from `targetTilemap`, `targetParent`, and `activeBiome`; no separate dirty flag is required. However, the Ping path must not call `GetTargetParent()` if the spec promises no auto-create, because `GetTargetParent()` creates `Props_Root` as a side effect.

## Section C - Prefab/SO reference loss
- C1: Yes, if `CollisionRulesSO` script is placed directly under `Assets/Editor/` with `RimaUnifiedPainterWindow.cs`, it can live in the predefined editor assembly and reference the window type. Build exclusion is fine. Caution: `Assets/Editor/MapDesigner/` has an asmdef, and asmdef placement changes the answer; keep the SO script out of that asmdef unless references are deliberately managed.
- C2: The nested enum can be referenced cleanly as `RIMA.Editor.MapDesigner.RimaUnifiedPainterWindow.CollisionMode` if the SO is in the same editor assembly or otherwise has a valid reference. It does not force a top-level enum for this spec. A top-level enum would be cleaner long-term, but it is a refactor.
- C3: Unity will serialize the enum field as an integer value in the SO asset, so it can survive member renames if numeric values and field names stay stable. Risks are semantic, not immediate corruption: reordering enum values, moving the enum type during a later refactor, or changing the SO script class/field name can remap or lose meaning.

## Section D - Naming/scope
- D1: Keeping `showPrefabSettingsSection` across the rename is technically OK and needs no migration. Minor mismatch: the spec's "default expanded" claim is not guaranteed for existing users whose old Collider Boundaries foldout was serialized closed. Use a new field only if v2 requires a fresh default.
- D2: No name collision found with current serialized fields lines 31-95. `showCollisionGizmo` and `showStatusBanner` are new. `groupExpanded` does not collide, but a `Dictionary<string,bool>` is not Unity-serialized; use it as runtime state only, or use `SessionState`, `EditorPrefs`, or a serializable list if persistence matters.
- D3: No existing `ResolveColliderPreview` symbol was found in the window or broader project search. Name is safe.

## Section E - Risk hotspots
- E1: `GUIStyle.wordWrap` should not be relied on to break consistently at underscores; it primarily breaks at normal wrap opportunities. ZWSP insertion is safe for IMGUI rendering if applied to a display-only string. Avoid embedding an invisible literal in source; use `"\u200B"` so the behavior is explicit and ASCII-safe.
- E2: The proposed gizmo colors are serviceable but not fully robust. Yellow/orange can be weak on light Scene backgrounds, and red/green alone is colorblind-hostile. Add stable shape/line treatment, higher alpha, or a thin dark/bright backing line if gizmo readability is a shipping requirement.
- E3: `SceneVisibilityManager.DisablePicking` is editor-only and does not affect runtime. Its state is editor tooling state, not a scene-authored lock. It may persist via Unity's scene visibility state in `Library`, but it should not be presented as a durable project/scene lock that survives all save/load/reopen workflows. Also, `SetActive` for visibility is destructive relative to the spec: it changes active state, affects runtime behavior, and is serialized with the scene.
- E4: Confirmed. `EditorGUI.BeginDisabledGroup(parent == null)` / `EndDisabledGroup` is the correct scoped IMGUI pattern. Avoid raw `GUI.enabled` mutation unless saved/restored immediately.

## Section F - Cross-cutting
- F1: No proposal forces an immediate edit while this remains a spec-only task. For future implementation, `CollisionRulesSO` does not force enum hoisting if it stays in the same editor assembly. Putting it in runtime `Assets/ScriptableObjects/Map/` or an asmdef would force a larger enum/API refactor.
- F2: Hidden refactor implications: `ResolveColliderPreview`, palette badges, scene gizmo, and placement must share one collision resolver or they will drift; CollisionRulesSO priority must be threaded through normal placement, wall replacement/autoconnect, map load, and preview; group names should come from one classifier instead of duplicating `GetOrCreateGroupParent` logic; Panel 4 must split "peek existing parent" from `GetTargetParent()` to avoid auto-create side effects; serialized dictionary persistence needs a deliberate approach.
- F3: A few UX/technical claims need correction. "Toggle scene visibility (does not affect save)" is false if implemented with `groupGo.SetActive`; use `SceneVisibilityManager.Hide/Show` style editor visibility instead. "OnHierarchyChange invalidates cache" is fine for create/destroy/reparent count changes, but do not rely on it as the only source for every derived UI state. "Ping GetTargetParent without creating Props_Root" is false if it calls the current helper.

## Section G - Per-panel verdict
- Panel 1: LIVE_WITH_NOTES - Implementable, but replace `SetActive` visibility with editor scene visibility and do not promise persistent lock semantics.
- Panel 2: LIVE_WITH_NOTES - Implementable, but expand the cache key and centralize collider resolution to avoid preview/placement drift.
- Panel 3: LIVE_WITH_NOTES - Implementable, but use display-only `\u200B` for underscore wrapping and keep the tile as one manual IMGUI rect.
- Panel 4: LIVE_WITH_NOTES - Implementable, but do not call `GetTargetParent()` for read-only ping/status because it can create `Props_Root`.

## Section H - Overall verdict
LIVE_WITH_NOTES

## Quotable summary (1-3 lines for spec excerpts)
The redesign is implementable in Unity IMGUI, but v2 should correct three technical claims before implementation: `SetActive` is not non-destructive visibility, the collision preview cache key is too narrow, and status/ping code must not call the auto-creating `GetTargetParent()` path.
