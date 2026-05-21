# Codex Task — Phase B-1 Asset Pack Browser SPEC (write only, no implementation)

**Profile:** laurethayday (40%/10%, paralel laurethgame revoked)
**Effort:** xhigh
**Timeout:** 1800s (30 min)
**Type:** Full spec doc + EditorWindow class skeleton (no implementation, no test runs)

## Context

Orchestrator delegating Phase B (Map Designer UI/UX) — primary feature = **Asset Pack Browser** + **Click-to-Place** + **Auto-Collider**. User wants Sims-tarzı ease ("kullanıcı çok rahat asset packinden alıp yerleştirecek").

Phase A still in progress (`bi9s1bxna` autonomous redesign of map visual quality) — does not block Phase B-1 spec drafting.

Read these context files before drafting:
- `STAGING/PLAN_FAKE3D_AND_UIUX.md` — Phase B 15 features overview
- `F:/LaurethStudio/01_PIPELINE/auto_collider_from_sprite_pipeline.md` — auto-collider spec already drafted
- `memory/project_brush_v1_manual_composition_system.md` — current system
- Existing `Assets/Editor/MapDesigner/` files — current MapDesignerBrushWindow + supporting infra
- Existing `Assets/Scripts/MapDesigner/Brush/Data/` SO files — PatchAtlasSO + PropDefinitionSO + RoomVisualProfileSO

## Stage 1 — Architecture audit (read-only)

In `STAGING/CODEX_PHASE_B1_AUDIT.md`:

1. What does the EXISTING `MapDesignerBrushWindow` provide?
2. What's missing for "Asset Pack Browser + Click-to-Place + Auto-Collider"?
3. Should we EXTEND the existing window OR build a new `MapDesignerAssetBrowserWindow`?
4. SO dependencies — do PatchAtlasSO + PropDefinitionSO suffice for "asset pack" abstraction OR do we need a new `AssetPackManifestSO`?

## Stage 2 — Full Spec (write only)

In `STAGING/SPEC_PHASE_B1_ASSET_PACK_BROWSER.md`:

### Sections required:

#### 2.1 — User stories (5-8 stories)
"As a designer, I open the asset browser and see all my walls grouped by pack..."
"As a designer, I select a sprite from the browser and click in scene to place it..."
"As a designer, I want auto-collider when I place a wall so the player can't walk through..."

#### 2.2 — UI layout (text wireframe)
Left panel (browser) + center (scene view) + right panel (inspector) + bottom (palette grid). Detail dimensions, dockable behavior.

#### 2.3 — Class architecture
```
MapDesignerAssetBrowserWindow : EditorWindow
├── AssetPackBrowserPanel
│   ├── CategoryTreeView
│   ├── SpriteGridView
│   └── SearchBar
├── ScenePlacementController
│   ├── GhostPreviewRenderer
│   ├── PlacementValidator
│   └── ClickToPlaceHandler
├── SelectedSpriteInspector
│   ├── VariantSlider
│   ├── ScaleSlider
│   ├── ColliderConfigPanel
│   └── ...
└── UndoRedoManager (Unity.Undo backed)
```

#### 2.4 — SO data model
- `AssetPackManifestSO` — new, groups multiple PatchAtlasSO into a "pack"
- PropDefinitionSO field additions for auto-collider (per `auto_collider_from_sprite_pipeline.md`)
- Backward compat with existing PatchAtlasSO

#### 2.5 — Auto-collider behavior contract
- When placing prop with `blocksMovement=true` → add BoxCollider2D/CircleCollider2D per shape
- Footprint ratio + offset spec
- Layer assignment + collision matrix

#### 2.6 — Acceptance tests (EditMode)
- 10-15 test cases — open browser → select sprite → click in scene → verify GameObject + sprite + collider
- Each test name + Arrange/Act/Assert pseudocode

#### 2.7 — Edge cases + risks
- Empty pack
- Sprite missing (variants[i] = null)
- Collider overlap with existing scene geometry
- Undo/redo across pack switches
- Performance (1000+ placed sprites)

#### 2.8 — Phased delivery checkpoints
- B-1 deliverable: browser opens, displays packs/categories/sprites, search works (READ-ONLY)
- B-1 PASS criteria: orchestrator opens window, clicks through all categories, sees all 124 sprites (84 v2 + 40 v3)

### Stage 3 — EditorWindow skeleton class (no logic, just structure)

Write `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs.skeleton` (use `.skeleton` extension to avoid Unity compile — this is spec only):

```csharp
// Skeleton: structure only, NO method bodies, NO logic
using UnityEditor;
using UnityEngine;
namespace RIMA.MapDesigner.Editor
{
    public class AssetPackBrowserWindow : EditorWindow
    {
        [MenuItem("Tools/RIMA/Map Designer/Asset Pack Browser")]
        public static void Open() { /* TODO Phase B-1 implement */ }
        
        // ... fields/properties/method signatures only ...
    }
}
```

## Stage 4 — DONE marker

`STAGING/CODEX_TASK_PHASE_B1_ASSET_PACK_BROWSER_SPEC_DONE.md`:
- Audit findings summary
- Spec doc path
- Skeleton class path
- Time estimate for Phase B-1 implementation (after orchestrator approves spec)
- Risks + open questions

## Constraints

- **NO IMPLEMENTATION** — spec + skeleton only
- DO NOT touch Assets/Scripts or Assets/Scenes (read-only audit)
- DO NOT extend `PatchAtlasSO.PatchRole` enum
- DO NOT modify Phase 1.5 data-first executors
- Skeleton class file MUST have `.skeleton` extension (not `.cs`) so Unity doesn't try to compile

## NEXT_SIGNAL

Orchestrator reviews spec → approves OR requests revision → if approved, Phase B-1 implementation dispatch (separate task).
