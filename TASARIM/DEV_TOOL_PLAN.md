# Dev Tool Plan
Status: LOCKED 2026-05-06 — scheduled after UI/minimap/combat-camera acceptance

## Decision

Host: Unity runtime overlay (not Editor extension, not web tool).
Reason: camera, sorting, collision, prefabs, and player scale all live in Unity runtime.
An Editor extension or web tool cannot test in the same render/sort pipeline players see.

## First Milestone: RoomDescriptorEditorOverlay v0

Toggle: F9 hotkey. No separate scene.

Scope (exactly this, nothing more):
- Load one RoomDescriptor ScriptableObject
- Edit: gates, spawn anchors, prop slots, collision strips, tile paint rect
- Live-preview inside _IsoGame scene
- Save back to ScriptableObject + JSON sidecar

Out of scope for v0: multi-room, batch, procgen integration, undo/redo, final art authoring.

## Future Tool Ideas (Not Scheduled)

- Run Recorder + Timeline Scrubber: every room/skill/damage event in a ring buffer;
  editor scrubber replays as 2D top-down trace with skill markers
- Class Diff Sheet (F10): all 8 classes side by side on a chosen axis (DPS, mobility, CC frames)
  sourced from headless dummy-room batch runs into CSV
- Skill Tree Live-Edit Bridge: right-click any skill node in-game -> edit SO in Inspector
  with explicit Commit button (no auto-save on scene reload)
- Encounter Sandbox Room: dev-only room, spawn any enemy mix, toggle HP/AI flags,
  snapshot/restore state with hotkey
- Asset Coverage Heatmap: editor window listing every class x state x direction sprite slot,
  red-flagging missing or placeholder assets
