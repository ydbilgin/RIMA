# D2: Cliff Fix 0 + 6-Layer Architecture LOCK — DONE
Date: 2026-05-27

## Compile Status
Unity console: **0 errors / 0 warnings** (post-refresh)
Only entries: MCP-FOR-UNITY reconnect messages (expected from domain reload, not errors)

## Verify Checklist — ALL PASS (15/15)
```
[PASS] CliffAutoPlacer.cliffTile = Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset
[PASS] AssetCategory.CliffFaceDecor defined
[PASS] AssetCategory.WallBlocker defined
[PASS] AssetCategory.GameplayObject defined
[PASS] RoomPainterPhysicsRules compiled
[PASS] PhysicsConfig.layer field exists
[PASS] 'mounting_cliff' → Cliff/noBlock (got layer=Cliff isBlock=False)
[PASS] 'statue' → Wall/block (got layer=Wall isBlock=True)
[PASS] SortingLayer Decor_Cliff registered
[PASS] SortingLayer Decor_Floor registered
[PASS] mounting_00: sortingLayer=Decor_Cliff order=50
[PASS] mounting_00 sprite pivot = TopCenter (val=2)
[PASS] statue_00: BoxCollider2D solid (trigger=False) order=100
[PASS] Chest.prefab: BoxCollider2D trigger=True order=150
[PASS] TargetLayer.L6 defined (all L1-L6 present)
```

## Changed File Inventory

### 1. Cliff Fix 0 — ALREADY DONE (pre-existing)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` — CliffAutoPlacer.cliffTile already pointed to DirectionalCliffTile_Hades.asset (no change needed)

### 2. Enums.cs — +3 AssetCategory entries
- **File**: `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs`
- Added: `CliffFaceDecor` (L3), `WallBlocker` (L5), `GameplayObject` (L6)
- `TargetLayer { L1..L6 }` confirmed LIVE (pre-existing line 17)

### 3. RoomPainterPhysicsRules.cs — +8 keywords + struct field
- **File**: `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs`
- `PhysicsConfig` struct: added `public RoomLayer layer;` field + constructor optional param (default=Floor)
- New keywords (ordered specific-before-generic):
  - `mounting` → L3 Cliff, no collider
  - `vine` → L3 Cliff, no collider
  - `chain` → L3 Cliff, no collider
  - `rune_circle` → L4 Decals, no collider
  - `bone_cluster` → L4 Decals, no collider
  - `statue` → L5 Wall, BoxCollider2D solid
  - `pedestal` → L5 Wall, BoxCollider2D solid
  - `plinth` → L5 Wall, BoxCollider2D solid
- All 8 inserted BEFORE `wall`/`cliff` to prevent premature keyword match

### 4. TagManager.asset — +2 sorting layers
- **File**: `ProjectSettings/TagManager.asset`
- Added: `Decor_Cliff` (uniqueID: 1200000005)
- Added: `Decor_Floor` (uniqueID: 1200000006)
- Both confirmed live via `SortingLayer.layers` runtime check

### 5. Prefab metadata backfill — 33 prefabs
**mounting_*.prefab × 15** (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/`)
- sortingLayerName = `Decor_Cliff`, sortingOrder = 50
- Sprite importer pivot = TopCenter (SpriteAlignment.TopCenter, val=2) — 15/15 reimported

**statue_*.prefab × 14** (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/`)
- sortingLayerName = `Default`, sortingOrder = 100
- BoxCollider2D isTrigger = false (solid)

**Obstacle prefabs × 3** (`Assets/Prefabs/Obstacles/`)
- Chasm.prefab, NarrowPassage.prefab, StoneColumn.prefab
- sortingOrder = 100, BoxCollider2D solid

**Chest.prefab** (`Assets/Prefabs/`)
- sortingOrder = 150, BoxCollider2D isTrigger = true

**MapFragment.prefab** (`Assets/Prefabs/Environment/`)
- sortingOrder = 150

**RewardPickup.prefab** (`Assets/Prefabs/`)
- sortingOrder = 150

## Notes / Scope Boundaries
- AssetCategory ScriptableObject reference per-prefab (SO slot assignment) is D3+ scope per task spec — not attempted in D2
- FractureImp_Playtest.prefab (AssetCategory=Enemy) untouched per task spec
- No new .cs files created
- No Editor window UI changes
- DirectionalCliffTile.cs / CliffAutoPlacer.cs code untouched (slot already correct)
- Mounting pivot: top-center ONLY (SpriteAlignment.TopCenter=2), bottom-center not used

## LOC Delta
- Enums.cs: +4 lines
- RoomPainterPhysicsRules.cs: +17 lines (struct field + constructor param + 8 keyword rules)
- TagManager.asset: +6 lines
- Total: ~27 LOC (task est 250-300 was for full impl including D3+ scope items)
