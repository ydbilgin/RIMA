# Codex Task — RimaUnifiedPainter UI/UX Spec Review v1

## Role

You are reviewing a Unity IMGUI UI/UX redesign spec. **You will not write code.** Your job is technical implementability review only.

## Inputs

- **Spec to review:** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` (v1 draft by Opus)
- **Existing file:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (2812 lines, untouched — DO NOT modify)
- **Helper references:** Line citations in the draft point to specific functions.

## What the spec proposes (high-level)

1. **Panel 1 — Scene Organization foldout** (in-window hierarchy view with count/visibility/lock/delete per group: Walls / Statues / WallMountings / Patches / Mobs / FloorProps).
2. **Panel 2 — Collision Inspector** (always-visible; live preview of `ConfigureCollider` output via new dry-run helper `ResolveColliderPreview`; new `CollisionRulesSO` ScriptableObject for per-prefab-name-pattern default override; Scene gizmo toggle).
3. **Panel 3 — Palette tile redesign** (110×130 px, 96×96 thumb, 2-line wrapped label, collision-mode badge B/P/S/F/C colored letter bottom-right).
4. **Panel 4 — Status Banner** (always-visible 3-line banner above main columns, replacing the misleading "Assign a Target Parent" HelpBox).

## Review questions — answer EACH explicitly

### A. Per-panel implementability in IMGUI

For each Panel 1-4, answer:
- **A1.** Implementable in Unity IMGUI (Editor) with current Unity LTS APIs? Y/N + key API used.
- **A2.** Layout race risk? Specifically, does the spec mix `EditorGUILayout` and `GUILayoutUtility.GetRect` in a way that could cause `ArgumentException: Getting control X's position in a group with only N controls`?
- **A3.** Are `BeginFoldoutHeaderGroup` calls balanced (each has matching `EndFoldoutHeaderGroup`)?
- **A4.** Are `EditorGUI.indentLevel++/--` balanced (no leak across panels)?

### B. EditorWindow Repaint loop

- **B1.** Panel 1 count refresh — proposed approach is "OnHierarchyChange + every 0.5s via EditorApplication.update". Does this risk infinite repaint loops? Any specific pitfalls (e.g., `Repaint()` called from inside `OnGUI` itself)?
- **B2.** Panel 2 `ResolveColliderPreview` is called every OnGUI for the selected brush. Spec proposes caching by `(prefab.InstanceID, mode, scaleMult, rotation)`. Is the cache key sufficient? Will sprite-bounds calculation be costly per repaint without cache?
- **B3.** Panel 4 status banner — is the spec's claim "no extra dirty flag needed" correct given that `targetTilemap`/`targetParent` are SerializeField + accessed in OnGUI?

### C. Prefab field reference loss

- **C1.** `CollisionRulesSO` — spec proposes placing it at `Assets/Editor/` (Editor-only, build excludes). Will the painter window be able to reference this asset type from the same Editor assembly? Any asmdef issues?
- **C2.** The `CollisionMode` enum is currently nested inside `RimaUnifiedPainterWindow` (line 19). Can `CollisionRulesSO` (Editor folder, also Editor assembly) reference this nested enum cleanly? Or does it force the enum to be promoted to top-level / a separate file?
- **C3.** If enum stays nested and SO references it as `RimaUnifiedPainterWindow.CollisionMode`, does Unity's `[SerializeField]` properly serialize the enum + survive renames? Risk of asset-corruption on refactor?

### D. Naming / scope collision

- **D1.** Foldout numbering: spec adds `6. Scene Organization` and renames `4. Collider Boundaries` → `4. Collision Inspector`. Existing foldout state `[SerializeField] private bool showPrefabSettingsSection` (line 93) will keep its value across the rename — is that OK or does the rename require a state migration?
- **D2.** Spec proposes new `[SerializeField] bool showCollisionGizmo`, `bool showStatusBanner`, `Dictionary<string,bool> groupExpanded`. Any field name collision with existing window state? (Check lines 31-95 for current SerializeFields.)
- **D3.** New helper `ResolveColliderPreview` — does this name collide with any existing method or RIMA namespace symbol?

### E. Specific risk hotspots

- **E1.** Panel 3 label `wordWrap = true` with underscored prefab names: does Unity's `GUIStyle.wordWrap` break on `_`? Spec proposes ZWSP injection (`name.Replace("_", "_​")`) as fallback. Is this safe in IMGUI label rendering (any glyph fallback weirdness)?
- **E2.** Panel 2 Scene gizmo — spec routes `Handles.DrawWireCube` through `OnSceneGUI` callback. Is the proposed colour mapping (Passable=green, ..., WallBlock=red) gizmo-readable in both Light/Dark scene backgrounds?
- **E3.** Panel 1 group lock via `SceneVisibilityManager.DisablePicking` — does this survive scene save/load? Will the lock state be preserved when the user closes Unity and reopens?
- **E4.** Panel 4 ping button calls `EditorGUIUtility.PingObject` on a potentially-null parent. Spec says "disabled if null". Confirm `EditorGUI.BeginDisabledGroup` is the correct pattern (not `GUI.enabled` global mutation).

### F. Cross-cutting

- **F1.** Spec aims to be **non-destructive** — i.e., the existing `RimaUnifiedPainterWindow.cs` should compile + run unchanged until a future implementation task picks this up. Are there any spec proposals that would FORCE an immediate edit to the existing file (e.g., `CollisionMode` enum hoist that breaks current SerializeFields)?
- **F2.** Spec uses Karpathy #3 (surgical, no architecture refactor). Are there hidden refactor implications you spotted? List them.
- **F3.** Any UX claim in the spec that is technically false (e.g., "OnHierarchyChange invalidates cache" — does this event actually fire when group counts change?).

### G. Final verdict per panel

For each of Panel 1, 2, 3, 4 emit ONE of:
- `LIVE` — implementable as-spec'd, ship it
- `LIVE_WITH_NOTES` — minor changes Opus should make, list them
- `NEEDS_REVISION` — major rework required, describe what
- `REJECT` — concept not implementable as IMGUI

### H. Overall verdict

`LIVE / LIVE_WITH_NOTES / NEEDS_REVISION / REJECT` for the whole spec.

## Output contract

Write your full review to **`STAGING/CODEX_DONE_uiux_painter_review_v1.md`** with this structure:

```markdown
# Codex Review — UIUX Painter Redesign v1

## STATUS
{LIVE / LIVE_WITH_NOTES / NEEDS_REVISION / REJECT}

## Section A — Per-panel IMGUI implementability
### Panel 1
- A1: ...
- A2: ...
- A3: ...
- A4: ...
### Panel 2
...
### Panel 3
...
### Panel 4
...

## Section B — Repaint loop
- B1: ...
- B2: ...
- B3: ...

## Section C — Prefab/SO reference loss
- C1: ...
- C2: ...
- C3: ...

## Section D — Naming/scope
- D1: ...
- D2: ...
- D3: ...

## Section E — Risk hotspots
- E1: ...
- E2: ...
- E3: ...
- E4: ...

## Section F — Cross-cutting
- F1: ...
- F2: ...
- F3: ...

## Section G — Per-panel verdict
- Panel 1: VERDICT — note
- Panel 2: VERDICT — note
- Panel 3: VERDICT — note
- Panel 4: VERDICT — note

## Section H — Overall verdict
{...}

## Quotable summary (1-3 lines for spec excerpts)
{...}
```

## Hard rules

- **DO NOT edit** `Assets/Editor/RimaUnifiedPainterWindow.cs` or any other source file. Review only.
- **DO NOT write code.** Pseudo-code is fine if needed to illustrate a counter-proposal.
- **DO answer all A-H questions explicitly.** Skipping = NEEDS_REVISION trigger downstream.
- **Effort:** high (this is a complex spec, take time).
- **Output file:** `STAGING/CODEX_DONE_uiux_painter_review_v1.md` AND also the default wrapper-required CODEX_DONE file (dispatcher contract).

## Workflow

1. Read `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` fully.
2. Skim `Assets/Editor/RimaUnifiedPainterWindow.cs` (use targeted ranges; the spec cites lines 19, 93, 394, 422, 569-595, 825-927, 1849-1899, 1901-1978, 2290-2330).
3. Answer A-H questions.
4. Write review to `STAGING/CODEX_DONE_uiux_painter_review_v1.md`.
5. Echo a short summary to the wrapper-mandated CODEX_DONE_*.md (the dispatcher reads that one for status).
