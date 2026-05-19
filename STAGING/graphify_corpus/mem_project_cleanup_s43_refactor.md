---
name: S43 Cleanup and God Object Refactor
description: LargeDungeonMapPainterBase extracted to own file, legacy scripts deleted, TestSwitcher removed
type: project
originSessionId: 7fd24c1a-41d2-407d-8929-dc84b69eb65e
---
## 2026-05-05: God Object Split + Legacy Cleanup

**LargeDungeonMapPainterBase extraction:**
- Was embedded inside `RuntimeRoomManager.cs` (2604 lines total, 2 classes in 1 file)
- Extracted to `Assets/Scripts/Core/LargeDungeonMapPainterBase.cs` (1481 lines)
- `RuntimeRoomManager.cs` trimmed to 1132 lines (room lifecycle only)
- `LargeDungeonMapPainter.cs` still inherits from base — no consumer changes needed

**Why:** Graphify flagged as god object (74 edges). Single-file coupling made both classes hard to navigate.

**Deleted scripts (6 files):**
1. `Assets/Scripts/Core/_Legacy/RoomManager.cs` — replaced by RuntimeRoomManager
2. `Assets/Scripts/Core/_Legacy/HUDManager.cs` — replaced by HUDController
3. `Assets/Scripts/Editor/IsoDummyRenderer.cs` — one-time mannequin renderer, output already archived
4. `Assets/Scripts/Editor/FixTextureImport.cs` — one-time texture fix (hardcoded Act1 paths)
5. `Assets/Scripts/Editor/FixTilemapMaterials.cs` — one-time tilemap material fix
6. `Assets/Scripts/Utils/TestSwitcher.cs` — dev test overlay (class switch, mob isolate, K/F/R keys)

**Kept (still active in production code):**
- `PlaceholderSprite.cs` — referenced by BaseMobBehavior, ObstaclePrefabBuilder, BossChainProjectile
- `UIBarSetup.cs` — referenced by ChestBehavior, BaseMobBehavior

**How to apply:** If TestSwitcher functionality is needed again, rebuild as a ScriptableObject-driven debug panel. The old version had hardcoded mob names.
