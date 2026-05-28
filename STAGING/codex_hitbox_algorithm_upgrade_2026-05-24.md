# Codex Task — Char Hitbox + WallChainBuilder Algorithm Upgrade (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç

Iki paralel iş:
1. **Char hitbox + Rigidbody** ekle (Warblade prefab + scene instance)
2. **WallChainBuilder algorithm full implementation** — Codex Q5 verdict'inden: door arches, low edges, alcove/protrusion, span packing (3x/2x/1x greedy), seam selection (lookup table), edge classification

User ChatGPT v2 spec onayladı: 4 sheet 40 piece. Char 64 px ACTUAL (not 120 — canvas size). MVP 11 piece.

## TASK A — Char Hitbox

**Warblade prefab path (eğer yoksa scene'deki Warblade GO'ya ekle):**
- Find: `GameObject.Find("Warblade")` veya sahne'de varsa
- Add components:
  - `CircleCollider2D`
    - radius: **0.2** Unity unit (~13 px at PPU 64, char footprint base)
    - offset: (0, **-0.3**) — ayak izi (sprite center'dan aşağı)
    - isTrigger: false (gameplay collision)
  - `Rigidbody2D`
    - bodyType: **Kinematic** (movement script-driven, no physics fall)
    - gravityScale: 0
    - constraints: FreezeRotation
- Verify: Play mode'da char wall'a doğru yürür → wall collider'da durur (col detected)
- console clean check

**Char prefab kayıt yoksa, sahne'deki Warblade'i prefab'a çevir:**
- Path: `Assets/Prefabs/Characters/Warblade.prefab`
- Mevcut warblade_south.png sprite reference

## TASK B — WallChainBuilder Algorithm Upgrade

**Files:**
- `Assets/Scripts/Runtime/Walls/WallChainBuilder.cs`
- `Assets/Scripts/Runtime/Walls/WallChunkData.cs` (enum extension)
- `Assets/Scripts/Runtime/Walls/WallChunkLibrary.cs` (lookup tables)
- `Assets/Scripts/Runtime/Walls/WallChunk.cs` (socket extensions)

### B1 — WallType enum genişlet

```csharp
public enum WallType
{
    // Connectors (existing + new)
    Connector_Straight,
    Connector_OuterCorner,
    Connector_InnerCorner,
    Connector_End,
    Connector_DoorLeft,
    Connector_DoorRight,
    Connector_Alcove,
    Connector_Protrusion,
    
    // Wall spans
    WallSpan_PlainMidA,  // 128x96 plain variant A
    WallSpan_PlainMidB,
    WallSpan_PlainMidC,
    WallSpan_Short,      // 1x 128x64
    WallSpan_Medium,     // 2x 192x64
    WallSpan_Long,       // 3x 256x64
    
    // Low walls (front edge parapet)
    LowWall_1x,          // 128x40
    LowWall_2x,          // 256x40
    
    // Corners
    OuterCorner_L,       // 128x96
    OuterCorner_R,
    InnerCorner_L,
    InnerCorner_R,
    
    // Doors
    DoorArch_2w,         // 192x128
    DoorArch_3w,         // 256x128
    
    // Shape
    OpenGap,
    ShortStop,
    
    // Seams (14)
    Seam_Straight,
    Seam_OuterCorner,
    Seam_InnerCorner,
    Seam_DoorJambL,
    Seam_DoorJambR,
    Seam_BaseTrim,
    Seam_FrontEdgeL,
    Seam_FrontEdgeR,
    Seam_FrontCornerL,
    Seam_FrontCornerR,
    Seam_PillarWall,
    Seam_ShadowPatch,
    Seam_CleanupCap,
    Seam_GapFiller,
    Seam_PlainFiller,
    Seam_MicroOccluder,
    
    Landmark,
    Pillar,
    SeamOverlay  // legacy fallback
}
```

### B2 — WallChunk socket extensions (Codex Q4)

`WallChunk.cs` add fields:
```csharp
[SerializeField] private Transform seamSocketLeft;
[SerializeField] private Transform seamSocketRight;
[SerializeField] private Transform optionalPropSocket;
```

`ApplySockets()` switch eklemeleri:
```csharp
"seam_left" => seamSocketLeft,
"seam_right" => seamSocketRight,
"prop" => optionalPropSocket,
```

Legacy `seamSocket` korunur fallback.

### B3 — WallChainBuilder algorithm IMPLEMENTATION

Mevcut stub'ları doldur:

**`ClassifyConnector(edgePrev, edgeNext, isDoor)` (yeni method):**
```csharp
public static WallType ClassifyConnector(EdgeType prev, EdgeType next, bool isDoorStart)
{
    if (isDoorStart) return WallType.Connector_DoorLeft;  // (or DoorRight based on context)
    
    // Convex (outer) — edges turn outward
    // Concave (inner) — edges turn inward
    int angleClass = CalcAngle(prev, next);
    
    if (angleClass == 0) return WallType.Connector_Straight;  // straight line
    if (angleClass == 90) return WallType.Connector_OuterCorner;
    if (angleClass == -90) return WallType.Connector_InnerCorner;
    if (angleClass == 180) return WallType.Connector_End;  // U-turn / termination
    return WallType.Connector_Straight;
}
```

**`PackSpans(int length)` (yeni method):**
```csharp
public static List<WallType> PackSpans(int length)
{
    var result = new List<WallType>();
    while (length > 0)
    {
        if (length >= 3) { result.Add(WallType.WallSpan_Long); length -= 3; }
        else if (length >= 2) { result.Add(WallType.WallSpan_Medium); length -= 2; }
        else { result.Add(WallType.WallSpan_Short); length -= 1; }
    }
    return result;
}
```

**`GetSeamFor(prev, next)` (Library lookup tablosu):**
- Same direction → `Seam_Straight`
- Outer turn (convex 90°) → `Seam_OuterCorner`
- Inner turn (concave) → `Seam_InnerCorner`
- Door start/end → `Seam_DoorJambL` / `Seam_DoorJambR`
- Front edge corner → `Seam_FrontCornerL` / `Seam_FrontCornerR`
- Otherwise: `null` (no seam needed)

**Front edge support (open edges):**
- For `openEdgeIndices`, instead of skipping completely, place **LowWall_1x/2x** along the edge (parapet) + `Seam_FrontCornerL/R` at edge endpoints

**Door arch placement:**
- If RoomFootprintPolygon has `doorEdgeIndices: List<int>` (new field):
  - For each door edge: place `Connector_DoorLeft` + `Connector_DoorRight` + `DoorArch_2w` between
  - Skip span filling on door edges

**`FillWallSpan` rewrite:**
- Calculate edge length in cells
- Call PackSpans → list of WallType
- Place each at calculated positions along edge

### B4 — WallChunkLibrary lookup tables

`WallChunkLibrary.cs` upgrade:
- `GetConnectorFor(WallType.Connector_OuterCorner)` → returns OuterCorner WallChunkData
- `GetSeamFor(prev, next)` → uses lookup logic, returns appropriate seam data
- `GetSpanFor(WallType type)` → returns Plain/Short/Medium/Long span
- `GetLowWallFor(int length)` → returns LowWall_1x or LowWall_2x

Library entries için placeholder data (sprite null OK — Sheet 1+2 sonrası fill):
- 8 connector + 8 span + 4 corner + 2 door arch + 16 seam = 38 entry placeholder

### B5 — RoomFootprintPolygon extension

Add field:
```csharp
public List<int> doorEdgeIndices = new List<int>();  // edges that should have door arch
public List<int> lowEdgeIndices = new List<int>();    // edges that should have low wall (open front edge)
```

Update Build algorithm to handle these.

## Test Scene

`Assets/Scenes/Demo/DiamondRoom_v1.unity` (or new `RoomV1.unity`):
- 6-vertex hexagonal room footprint:
  - vertices: (0,0), (4,1), (5,3), (4,5), (0,5), (-1,3)
  - openEdgeIndices: [0] (front bottom open)
  - lowEdgeIndices: [0] (parapet)
  - doorEdgeIndices: [3] (top-left edge has door)
- Rebuild via `RIMA/Tools/Build Test Diamond Room` menu (use menu OR rebuild via execute_code)
- Play mode test: char walks toward wall → collision detected
- Console clean
- Screenshot: `STAGING/screenshots/room_v6_algorithm_complete.png`

## Verification

1. WallType enum compile ok
2. WallChunk sockets compile ok
3. WallChainBuilder algorithm compile ok
4. WallChunkLibrary lookup compile ok
5. Char prefab has CircleCollider2D + Rigidbody2D
6. Play mode: char collides with walls
7. Test scene: walls placed with correct connector + span packing + (optional) seams
8. Console clean

## Çıktı raporu

`STAGING/codex_hitbox_algorithm_DONE.md`:
- Files modified
- Compile status
- Play mode collision test result
- Screenshot path
- Algorithm coverage (which features work, which still stub)
- Issues/blockers

git commit YASAK (orchestrator review).

## Effort

~1-1.5 saat Codex. Multi-phase implementation. xhigh effort.
