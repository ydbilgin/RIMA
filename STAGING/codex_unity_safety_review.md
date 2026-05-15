# Codex Task — Unity Editor Stability Safety Review for Brush Tool Refactor

**Type:** Advisory review only — NO code changes
**Output:** Markdown analysis to `CODEX_DONE.md` with sections matching the template at the end of this file
**Effort:** high

---

## Context

The RIMA project is about to begin a 7-sprint refactor of the Map Designer into a unified Photoshop-style brush tool. Full design spec: `STAGING/map_designer_unified_brush_design.md` (read this first for full context).

**Stack:**
- Unity 6 (LTS)
- URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- Tilemap system (Unity stock)
- EditorWindow IMGUI (existing `RimaMapDesignerWindow.cs`)
- ScriptableObject-heavy data model (~15 new SO types planned)
- JSON round-trip serialization layer
- 2 asmdefs: `RIMA.Runtime` (runtime) and `RIMA.Editor` (editor only)

**Existing LIVE code that must not regress (Karar #143 Aşama 1+2):**
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs`
- `Assets/Scripts/MapDesigner/WallOverlayPainter.cs`
- `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs`
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs`
- `Assets/Scripts/MapDesigner/AccentPainter.cs`
- `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs`
- `Assets/Scripts/MapDesigner/VoronoiWaterFeatureGenerator.cs`
- `Assets/Scripts/MapDesigner/VoronoiElevationFeatureGenerator.cs`
- `Assets/Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs`
- `Assets/Scripts/Data/FeatureMaskSO.cs`
- `Assets/Scripts/Data/WallBrushSetSO.cs`
- `Assets/Scripts/Data/NaturalFeatureSettingsSO.cs`
- `Assets/Editor/RimaMapDesignerWindow.cs`

**History of Unity instability on this project:**
- Library/ folder has been corrupted multiple times in past sprints
- .meta file GUID collisions caused scene reference loss
- Asmdef circular dependency froze Unity for 30+ minutes
- AssetDatabase.Refresh() spammed during script gen caused asset import lockup

---

## Your Task — Review and Augment This Safety Plan

Below is the orchestrator's proposed safety plan. Review it line by line. For each section:

1. **Validate** — is each item correct and sufficient?
2. **Flag gaps** — what's missing, especially Unity 6 specific gotchas?
3. **Suggest concrete patterns** — provide exact API call sequences where ambiguous.
4. **Risk-score** — for each operation we'll do during the refactor, rate corruption risk (Low/Med/High) and prescribe mitigation.

### Plan Section A — Pre-flight per sprint
- Fresh branch `feature/brush-sprint-N`
- Disposable test scene `Assets/Scenes/_TestScenes/BrushTool_SprintN.unity`
- .meta files committed
- .gitignore covers Library/, Temp/, obj/, Logs/

### Plan Section B — Codex edit rules
- Max 8-10 files per dispatch
- New scripts assigned to correct asmdef (Runtime vs Editor)
- AssetDatabase.Refresh() batched at end, not per-file
- Asset creation: AssetDatabase.CreateAsset → EditorUtility.SetDirty → AssetDatabase.SaveAssets
- EditMode test required per new public API
- dotnet build RIMA.Runtime.csproj 0 errors BEFORE opening Unity

### Plan Section C — Sprint-end smoke gate
1. dotnet build pass
2. Unity Editor opens, csproj regenerate clean
3. EditMode test suite all PASS
4. PlayMode smoke: load Phase1_ProceduralMap_Test scene, 10s play, no null refs
5. git commit + tag sprint-N-pass

### Plan Section D — Forbidden ops
- Library/ delete while Unity open
- AssetDatabase.Refresh() per-file
- Script edit during domain reload
- git reset --hard / git clean -fd by Codex
- Direct master commits

### Plan Section E — Test strategy
- EditMode unit per public method (Codex)
- EditMode integration sprint end (Codex)
- PlayMode smoke sprint end (orchestrator)
- Visual regression sprint end (user)
- JSON round-trip Sprint 1 + 4
- Undo/Redo Sprint 3

---

## Specific Questions to Answer

**Q1 — AssetDatabase write order**
What is the exact correct sequence for creating 50+ ScriptableObject .asset files in a single Editor operation (e.g., importing a brush pack JSON)? Provide the canonical API call order including batch boundaries (AssetDatabase.StartAssetEditing / StopAssetEditing). Show a code example.

**Q2 — Domain reload safety**
We will add ~15 new ScriptableObject types and 6 IBrushExecutor classes. What is the safe way to ensure Unity doesn't trigger compile loops or reload thrash? Specifically:
- When is it safe to add `[InitializeOnLoad]`?
- How to avoid serialization version mismatch when SO field signatures change between sprints?
- Best practice for EditorWindow state persistence across domain reloads (EditorPrefs vs SessionState vs serialized field)?

**Q3 — Asmdef hygiene**
The new brush tool spans Runtime (data SOs) and Editor (window + executors). Recommend an asmdef structure:
- Should each major subsystem (Data, Executors, UI) get its own asmdef?
- How to prevent circular references between BrushExecutor (needs runtime painters) and EditorWindow (needs executors)?
- What goes in Editor folder vs separate Editor asmdef?

**Q4 — EditorWindow IMGUI stability**
We're refactoring an existing 3-panel EditorWindow. Known Unity 6 IMGUI gotchas?
- `OnGUI` exception handling — should we wrap in try/catch to prevent window crash?
- `SceneView.duringSceneGui` lifecycle (subscribe/unsubscribe rules)
- `Event.current` consumption order
- Undo.RecordObject vs Undo.RegisterCompleteObjectUndo for multi-object operations (composite brush stroke)

**Q5 — Tilemap + SpriteRenderer mixed rendering performance**
Brush tool will spawn 50-200 SpriteRenderer GameObjects per room on top of 2-3 Tilemap layers. URP 2D Renderer + Pixel Perfect Camera implications:
- Sprite atlas requirement for batching?
- Sorting layer + sortingOrder configuration to avoid Z-fighting between Tilemap and overlay sprites?
- 2D Light interaction with overlay sprites (do they receive light by default?)
- GameObject pool vs Mesh batching for overlay sprites?

**Q6 — JSON serialization safety**
Plan calls for JSON round-trip of SO data. Risks:
- `JsonUtility` vs `Newtonsoft Json.NET` — which is safer for Unity SO serialization?
- AssetDatabase.LoadAssetAtPath performance hit when resolving 100+ sprite refs from JSON
- Handling missing asset paths gracefully (fall back to placeholder vs throw)
- Versioning the JSON format for forward compatibility

**Q7 — Codex-specific risks**
You (Codex) will write much of this code via cx_dispatch.py. What patterns should you yourself follow to prevent project corruption?
- Should every Edit be preceded by a Read?
- How many files per dispatch is safe?
- When should you call `AssetDatabase.Refresh()` vs let Unity auto-detect?
- How to communicate "this change requires Unity restart" to the orchestrator?

**Q8 — Rollback hierarchy**
If a sprint introduces instability that's not caught until 2 sprints later, what's the rollback hierarchy?
- git revert single commit vs sprint tag rollback
- How to detect "is the corruption in my new code or in Library/Cache?"
- What's safe to nuke (Library/, obj/, Temp/, Logs/) without losing work?

---

## Output Format (write to CODEX_DONE.md)

```markdown
# Unity Safety Review — Brush Tool Refactor

## 1. Plan Section A — Pre-flight
[validation + gaps + additions]

## 2. Plan Section B — Codex edit rules
[validation + gaps + additions]

## 3. Plan Section C — Sprint-end smoke gate
[validation + gaps + additions]

## 4. Plan Section D — Forbidden ops
[validation + gaps + additions]

## 5. Plan Section E — Test strategy
[validation + gaps + additions]

## 6. Answers to Q1-Q8
[detailed answers with code examples where requested]

## 7. Risk Matrix
| Operation | Risk | Mitigation |
|---|---|---|
| [each major op in 7-sprint plan] | L/M/H | [concrete] |

## 8. Additional Recommendations
[anything not covered above that you think matters]

## 9. Verdict
[Approve plan as-is / Approve with additions / Reject and propose alternative]
```

Do not modify any code or assets. This is review-only. Write your output to CODEX_DONE.md.
