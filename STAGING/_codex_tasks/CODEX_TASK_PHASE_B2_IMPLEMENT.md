# Codex Task — Phase B-2 Click-to-Place + Auto-Collider IMPLEMENTATION

**Profile:** wait until laurethgame fresh (post 03:13 reset) OR yasinderyabilgin/laurethayday available
**Effort:** xhigh
**Timeout:** 7200s (2 hours)
**Type:** Extend Phase B-1 window with placement + auto-collider

## Context

Phase B-1 DONE PASS (341/341, 124 sprites browse-able). Window: `Tools/RIMA/Map Designer/Asset Pack Browser`.

Phase B-2 extends Phase B-1 — adds Scene View click-to-place workflow + auto-collider attachment.

Reference docs (read before implementing):
- `STAGING/SPEC_PHASE_B1_ASSET_PACK_BROWSER.md` (existing spec)
- `STAGING/RIMA_DESIGN_PHASE_B_PRIORITY_VERDICT.md` (P0-b + P0-d defined)
- `F:/LaurethStudio/01_PIPELINE/auto_collider_from_sprite_pipeline.md` (full auto-collider spec)
- `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs` (Phase B-1 LIVE code)

## CRITICAL — Unity pre-check + workflow guardrails

Unity OPEN, instance `RIMA@ed023e0b`. Editor tool extension — NO scene composition changes (don't touch PlayableRoom hierarchy beyond what placement requires).

Active scene `RoomPipelineTest` Combat Room v14 LIVE — placement target = `PlayableRoom/Pro_Redesign_v14_CombatRoom` if available, else any GameObject the user has selected in Hierarchy.

## Implementation stages

### Stage 1 — PropDefinitionSO extension

Per `auto_collider_from_sprite_pipeline.md` §SO field additions, extend `Assets/Scripts/MapDesigner/Brush/Data/PropDefinitionSO.cs` with:

```csharp
public bool blocksMovement = false;
public ColliderShape colliderShape = ColliderShape.None;
[Range(0.3f, 1.0f)] public float colliderFootprintRatio = 0.7f;
public Vector2 colliderOffset = Vector2.zero;
public bool isTrigger = false;
public string colliderLayer = "Walls";  // STRING not LayerMask (renderer-agnostic LOCK)

public enum ColliderShape { None, Box, Circle, Capsule, PolygonAutoTrace }
```

Update existing prop assets with reasonable defaults per `auto_collider_from_sprite_pipeline.md` category table:
- BaseFloor / MacroPatch / Decals / Scatter / Accents: `blocksMovement = false`
- Walls: `blocksMovement = true, colliderShape = Box, footprintRatio = 1.0`
- VerticalProps (statue/column/brazier): `blocksMovement = true, colliderShape = Box (statue+column) or Circle (brazier base), footprintRatio = 0.6`
- AtmosphericAccent (portal/puddle): `blocksMovement = false`

### Stage 2 — Active target binding (rima-design P0-f)

In `AssetPackBrowserWindow`:
- Add window header field: "Active Room Root: [PlayableRoom/Pro_Redesign_v14_CombatRoom]" with dropdown or "select in Hierarchy" button
- Validate target on every placement — must be a Transform in active scene
- Visual highlight in Hierarchy (ping object)

### Stage 3 — Click-to-Place workflow

In `AssetPackBrowserWindow`:
- When sprite selected in browser → enter "placement mode"
- Cursor in Scene View → render ghost preview at world position (PPU-snapped using `Mathf.Round` to nearest 1/32 unit)
- Ghost uses semi-transparent SpriteRenderer at temp GameObject `_GhostPreview`
- Left-click in Scene View → instantiate GameObject under Active Room Root, set sprite, sortingOrder (per spec table per category), trigger Undo.RegisterCreatedObjectUndo
- Right-click → exit placement mode (destroy ghost)
- Esc key → exit placement mode

Use `SceneView.duringSceneGui` event for ghost rendering + click handling.

### Stage 4 — Auto-collider attach

On placement, if selected sprite's `PropDefinitionSO.blocksMovement == true`:
- Add appropriate `Collider2D` component (Box / Circle / Capsule / PolygonAutoTrace per `colliderShape`)
- Size = sprite bounds × `colliderFootprintRatio`
- Offset = sprite center + `colliderOffset`
- isTrigger = `isTrigger`
- Set GameObject layer via `LayerMask.NameToLayer(colliderLayer)` (string → int lookup at runtime, NOT at SO level)

For sprites from PatchAtlasSO (not PropDefinitionSO), use category defaults (walls = Box 1.0, all else = None).

### Stage 5 — Inspector edits (read-only → edit mode)

Phase B-1 right panel was read-only. Now make editable:
- Variant index ← → slider (cycles through atlas.variants)
- Scale slider 0.3-2.0 (applies to selected placed object's transform.localScale)
- Alpha slider 0.0-1.0 (SpriteRenderer.color.a)
- Flip X / Flip Y toggles
- SortingOrder override (int field)
- Collider config (if prop has blocksMovement): shape dropdown + footprint slider + offset XY + isTrigger

Changes apply to currently-selected placed object in scene. Use Undo.RecordObject for each change.

### Stage 6 — Tests

`Assets/Tests/EditMode/MapDesigner/AssetPackBrowserPlacementTests.cs`:

Minimum 8 tests:
- `PlacementMode_GhostFollowsCursor_PPUSnapped`
- `LeftClick_CreatesGameObject_UnderActiveTarget`
- `LeftClick_AppliesCorrectSortingOrder_PerCategory`
- `LeftClick_AppliesAutoCollider_WhenBlocksMovement`
- `RightClick_ExitsPlacementMode_DestroysGhost`
- `InspectorScaleSlider_AppliesToSelected`
- `InspectorVariantSlider_CyclesAtlasVariants`
- `Undo_RemovesLastPlacement`
- `PropWithoutColliderConfig_DoesNotAttachCollider`
- `ColliderFootprint_Respects_Ratio`

### Stage 7 — Verification

- 341 + 10 = **351/351 PASS** target
- Manual flow: open Asset Pack Browser → select wall → cursor in scene → ghost follows → click → wall appears with BoxCollider2D → undo → wall gone
- Screenshot Scene View with ghost preview visible + sample placement
- DONE marker

### Stage 8 — DONE marker

`STAGING/CODEX_TASK_PHASE_B2_IMPLEMENT_DONE.md`:
- Files modified (PropDefinitionSO + AssetPackBrowserWindow)
- Test count delta
- Sample screenshot path
- Iterations attempted
- Console errors
- Phase B-2 deliverable verdict

## Constraints

- DO NOT modify SO contract scripts EXCEPT PropDefinitionSO (per auto_collider spec)
- DO NOT extend PatchAtlasSO or PatchRole enum
- DO NOT modify scenes (placement creates GameObjects under Active Room Root but does NOT modify scene saved state during tests)
- DO NOT enter Play mode
- `colliderLayer` MUST stay `string` (renderer-agnostic LOCK per `[[3d-portability-strategy]]`) — string→int LayerMask map at runtime
- Use new Input System for any keyboard handling (Esc key)
- Save scene only if explicitly tested (Edit mode only)

## NEXT_SIGNAL

DONE → orchestrator review. If PASS → Phase B-3 dispatch (Room save/load + active target binding completion + Random variant + Layer toggle + Eyedropper). If FAIL → iterate.
