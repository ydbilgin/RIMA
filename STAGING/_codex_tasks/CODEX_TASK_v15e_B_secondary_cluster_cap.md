# Codex Task — v15e-B Secondary Prop Cluster Cap (small chunk)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Background
v15e-A LIVE: L8 atmospheric cap 10 enforced → mist 67→32 (good). But v15e-A screenshot still shows 7-8 purple crystal scatter — secondary props (L5) are NOT cluster-aware. Hero cluster cap=3 (L4-L7) only counts FIRST-tier props; purple crystals slip into L5 secondary uniformly.

## Goal
Extend cluster-cap logic to L5 (secondary props). New field `secondaryClusterCap` per zone, AutoPopulator enforces it.

## Files to modify

### 1. `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
Add 1 field next to existing composition budget:
```csharp
[Range(0, 8)]
[Tooltip("Max L5 secondary prop clusters per zone. Default 4. Prevents purple crystal scatter from breaking hero cluster cap.")]
public int secondaryClusterCap = 4;
```

### 2. `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`
In L5 placement path, track cluster anchors:
- Compute cluster anchors from L5 candidate placements
- Cap to `secondaryClusterCap`
- Anchor radius 2 cells (smaller than hero cluster's 2-cell buffer)
- Secondary cluster size = 1-3 props per anchor

### 3. `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs`
Update metrics:
```
Secondary clusters: 3 / 4 cap -- OK
```

### 4. Update zone .asset files
- `zone_stone.asset` → secondaryClusterCap = 4
- `zone_grass.asset` → secondaryClusterCap = 3
- Others default 4

### 5. 2 new EditMode tests
- Secondary cluster cap respected
- Backward compat (cap=0 = unlimited legacy)

## Acceptance
- All existing 405 PASS → 407 PASS
- Re-render via `RimaV15cSceneComposer.Build()` → `Assets/Screenshots/PlayableRoom_combat_v15e_B_secondary_cap_LIVE.png`
- Purple crystal scatter should drop from 7-8 visible to ~3-4
- Metrics output captures secondary cluster count
- DONE marker: `STAGING/CODEX_TASK_v15e_B_secondary_cluster_cap_DONE.md`

## What NOT to do
- No touch L4/L6/L7 hero cluster cap (v15d locked)
- No touch L8 atmospheric cap (v15e-A locked)
- No new ScriptableObject
- No Brush V1 file changes
- No commit
