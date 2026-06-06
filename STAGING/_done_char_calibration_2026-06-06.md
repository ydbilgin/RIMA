# Character Pivot Calibration + Sorting Fix — 2026-06-06

## Part 1 — Pivot Audit

### Method
Alpha-scan of lowest opaque pixel row (alpha > 10/255) from canvas bottom.
Pivot Y = feetRow / canvasHeight (normalised). "OFF" = abs(currentPx - autoPx) > 2px.

### Resources/Characters (idle sprites) — pre-existing state
| Class | Canvas H | Feet Row | Auto Pivot | Cur Pivot | Diff px | Status |
|-------|----------|----------|-----------|-----------|---------|--------|
| Brawler | 120 | 30 | 0.250 | 0.250 | 0.0 | OK |
| Elementalist | 120 | 30 | 0.250 | 0.250 | 0.0 | OK |
| Gunslinger | 124 | 31 | 0.250 | 0.250 | 0.0 | OK |
| Hexer | 124 | 31 | 0.250 | 0.250 | 0.0 | OK |
| Ranger | 128 | 33 | 0.258 | 0.258 | 0.0 | OK |
| Ravager | 124 | 32 | 0.258 | 0.258 | 0.0 | OK |
| Ronin | 128 | 33 | 0.258 | 0.258 | 0.0 | OK |
| Shadowblade | 124 | 31 | 0.250 | 0.250 | 0.0 | OK |
| Summoner | 124 | 32 | 0.258 | 0.258 | 0.0 | OK |
| Warblade | 120 | 30 | 0.250 | 0.250 | 0.0 | OK |

All OK — previous SpritePivotBatchFix run had already corrected these.

### Art/Characters (rotation sprites) — FIXED THIS SESSION
Were all at pivot 0.5 (canvas center), ~30px off. Applied measured feet pivot to 82 sprites.

| Class | Canvas H | Feet Row | Applied Pivot |
|-------|----------|----------|--------------|
| Brawler | 120 | 30 | 0.250 |
| Elementalist | 120 | 30 | 0.250 |
| Gunslinger | 124 | 31 | 0.250 |
| Hexer | 124 | 31 | 0.250 |
| Ranger | 128 | 33 | 0.258 |
| Ravager | 124 | 32 | 0.258 |
| Ronin | 128 | 33 | 0.258 |
| Shadowblade | 124 | 31 | 0.250 |
| Summoner | 124 | 32 | 0.258 |
| Warblade | 120 | 30 | 0.250 |

## Part 2 — Sorting Layers

### Changes made
- **Player Body SpriteRenderer**: Default (0) → Entities (1293760285)
- **Player WeaponSprite SpriteRenderer**: Default (0) → Entities (1293760285)
- **IsoSorter added** to Player Body (baseOrder=0, pivotOffsetY=0)
- **All 16 mob prefabs** (ShatteredKeep_PixelLab): Default → Entities + IsoSorter added
- **Dead "Player" sorting layer** (uniqueID=0, duplicate of Default) removed from TagManager.asset
- Weapon sort order is managed by HandAnchorAttach.UpdateWeaponSortOrder() (body.sortingOrder ±1 per direction) — still works correctly since weapon is now on same Entities layer as body

### Sorting layer stack (ground→top)
Ground → Floor → Decals → Walls → Entities (player+mobs) → Props → VFX → UI

## Part 3 — Tool Added
`Assets/Editor/Tools/CharacterPivotCalibrationWindow.cs`
- Menu: RIMA/Characters/Calibration
- Per-class rows: auto feet offset, current pivot, diff, OFF/OK badge
- [Apply Auto] per class or all
- Manual nudge field (±pixels) persisted via EditorPrefs

## Part 4 — Removed
`Assets/Resources/Prefabs/Player.prefab` + .meta — dead duplicate, zero references (GUID 93e5ce1b968ccdb49b3f9d59a3d64542 unreferenced).

## Verification
- Compile clean (0 errors after adding CalibrationWindow)
- Player prefab loads: Body=Entities, WeaponSprite=Entities, IsoSorter on Body (confirmed via execute_code)
- Mob prefab enemy_00: Entities layer + IsoSorter PRESENT (confirmed)
- All sorting layer IDs resolve correctly (no "Player" dead layer)
