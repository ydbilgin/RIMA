# Codex Task — Wall Chain Builder System + Iso Tilemap Pivot (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: ChatGPT analizini implement et — wall-chain construction system (asset placement DEĞIL). Diamond Iso Tilemap'e pivot (2026-05-21 Rectangular lock REVISE). WallChunkData ScriptableObject + WallChunk MonoBehaviour + WallChainBuilder algorithm + 17 mevcut prefab retrofit + 1 test scene.

**Bu büyük dispatch — ~4-6 saat. Adım adım ilerle, BLOCKED durumlarda dur.**

**Unity zorunlu açık. UnityMCP kullan.**

---

## Konteks

### Mevcut durum (bug1ol7lg dispatch sonrası)
- 17 wall prefab `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/HighTopDown_3_4/` altında
- Test scene `Assets/Scenes/Demo/TopDownTest_HighTopDown_3_4.unity`
- Floor diamond layout ortaya çıktı (Tilemap mode kontrol edilmemiş)
- 15 wall yan yana placement ama "asset placement" yaklaşımıyla, construction logic yok

### Project Lock Updates (bu dispatch ile)
- **Diamond Iso Tilemap pivot LIVE** — 2026-05-21 Rectangular Tilemap lock REVOKED. Karar #149 (yeni) yazılacak.
- HIGH TOP-DOWN 3/4 70-80° KORUNUR (Karar #148).
- Industry standard match: Hades, D3, Children of Morta — hepsi iso tilemap + HIGH TOP-DOWN 3/4 sprite.

### Memory references
- `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_high_top_down_3_4_lock_2026_05_24.md`
- `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_pillar_seam_cover_lock_2026_05_24.md`

---

## PHASE A — Iso Tilemap Pivot

**Hedef:** Yeni scene `Assets/Scenes/Demo/DiamondRoom_v1.unity` (mevcut TopDownTest scene'i DOKUNMA).

### A.1 — Scene oluştur
- New scene URP 2D template
- **Grid + Tilemap**:
  - Grid GameObject → `Cell Layout: Isometric Z As Y`
  - Cell Size: (1, 0.5, 1) — bu Unity world units; 128×64 px sprite ile uyumlu (PPU=64)
  - Cell Swizzle: default XYZ
- **Floor Tilemap** child:
  - TilemapRenderer mode: `Chunk` (or `Individual` for transparency layering)
  - Sorting Layer: "Floor"
- **Walls Empty Parent**:
  - Boş GameObject "Walls" — wall chunk'lar parent edilecek

### A.2 — Floor RuleTile veya Tile asset
**Karar Codex'e:** Mevcut 6 floor sprite'ı (128×64) iso tile olarak kullan:
- Her floor sprite → individual `Tile` ScriptableObject asset oluştur
- VEYA `RandomTile` ile 6 variant random distribute
- Cell-aligned placement (iso grid Y-stagger Unity tarafından otomatik)

### A.3 — Camera ayarı (HIGH TOP-DOWN 3/4 lock)
- Orthographic
- Size: 5
- `PixelPerfectCamera` PPU=64, Filter Point, Upscale RT ON, Pixel Snap ON
- Position: world origin
- Z rotation: 0 (sprite art zaten 3/4 perspective içeriyor — camera angle math'i değil)

### A.4 — Sorting layers
- Mevcut Sorting Layers'a ekle (yoksa):
  - Floor (en alt)
  - Walls (orta)
  - Characters (üst)
  - Props (en üst, ya da Y-sort'a dahil)
- TransparencySortMode: Custom Axis (0, 1, 0)

---

## PHASE B — WallChunkData ScriptableObject

**Path:** `Assets/Scripts/Runtime/Walls/WallChunkData.cs` (yeni klasör)

```csharp
using UnityEngine;
using System.Collections.Generic;

namespace RIMA.Walls
{
    public enum WallType
    {
        Connector_Straight,
        Connector_CornerOuter,
        Connector_CornerInner,
        Connector_End,
        Connector_DoorLeft,
        Connector_DoorRight,
        WallSpan_Short,    // 1 cell
        WallSpan_Medium,   // 2 cell
        WallSpan_Long,     // 3 cell
        Landmark,
        Pillar,
        SeamOverlay
    }

    public enum WallHeight
    {
        Normal,    // standard 6-cell tall
        Tall,      // 8-cell tall
        Base,      // 2-cell bottom
        Mid,       // 4-cell middle
        Cap        // 2-cell top
    }

    [System.Serializable]
    public struct SocketDef
    {
        public string socketName;          // "Torch", "Banner", "Chain", "Seam"
        public Vector2 localPosition;      // relative to footprint anchor
    }

    [CreateAssetMenu(fileName = "WallChunkData", menuName = "RIMA/Walls/Wall Chunk Data")]
    public class WallChunkData : ScriptableObject
    {
        [Header("Identity")]
        public string chunkId;
        public WallType wallType;
        public WallHeight heightVariant = WallHeight.Normal;
        public Sprite visual;

        [Header("Footprint (iso cell occupancy)")]
        [Tooltip("List of iso cells occupied. (0,0) = anchor cell. (1,0) = east of anchor.")]
        public List<Vector2Int> footprintCells = new List<Vector2Int> { Vector2Int.zero };

        [Header("Anchor")]
        [Tooltip("Local offset from cell center to sprite pivot (anchored at footprint base)")]
        public Vector2 anchorOffset = Vector2.zero;

        [Header("Sockets")]
        public List<SocketDef> sockets = new List<SocketDef>();

        [Header("Collision")]
        public Vector2 colliderSize = new Vector2(2f, 1f);  // Unity units, default 1 cell
        public Vector2 colliderOffset = Vector2.zero;
    }
}
```

---

## PHASE C — WallChunk MonoBehaviour

**Path:** `Assets/Scripts/Runtime/Walls/WallChunk.cs`

```csharp
using UnityEngine;

namespace RIMA.Walls
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WallChunk : MonoBehaviour
    {
        [SerializeField] private WallChunkData data;
        [SerializeField] private SpriteRenderer visualRenderer;
        [SerializeField] private BoxCollider2D footprintCollider;

        [Header("Socket Anchors (auto-populated)")]
        [SerializeField] private Transform footprintAnchor;
        [SerializeField] private Transform leftSocket;
        [SerializeField] private Transform rightSocket;
        [SerializeField] private Transform torchSocket;
        [SerializeField] private Transform bannerSocket;
        [SerializeField] private Transform seamSocket;

        public WallChunkData Data => data;
        public Transform FootprintAnchor => footprintAnchor;

        void OnValidate()
        {
            if (data == null) return;
            ApplyData();
        }

        public void ApplyData()
        {
            if (visualRenderer == null) visualRenderer = GetComponent<SpriteRenderer>();
            visualRenderer.sprite = data.visual;
            
            if (footprintCollider != null)
            {
                footprintCollider.size = data.colliderSize;
                footprintCollider.offset = data.colliderOffset;
            }
            // Sockets are pre-positioned child transforms; data.sockets list updates them
            ApplySockets();
        }

        void ApplySockets()
        {
            foreach (var socket in data.sockets)
            {
                Transform target = socket.socketName.ToLower() switch
                {
                    "torch" => torchSocket,
                    "banner" => bannerSocket,
                    "left" => leftSocket,
                    "right" => rightSocket,
                    "seam" => seamSocket,
                    _ => null
                };
                if (target != null)
                    target.localPosition = socket.localPosition;
            }
        }
    }
}
```

Prefab template `Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab`:
- Root GameObject + WallChunk + SpriteRenderer + BoxCollider2D
- Children: FootprintAnchor, LeftSocket, RightSocket, TorchSocket, BannerSocket, SeamSocket

---

## PHASE D — RoomFootprintPolygon ScriptableObject

**Path:** `Assets/Scripts/Runtime/Rooms/RoomFootprintPolygon.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

namespace RIMA.Rooms
{
    [CreateAssetMenu(fileName = "RoomFootprint", menuName = "RIMA/Rooms/Room Footprint Polygon")]
    public class RoomFootprintPolygon : ScriptableObject
    {
        [Header("Polygon vertices (iso grid coordinates)")]
        [Tooltip("Vertex order: clockwise from top. Edges between consecutive verts form wall chain.")]
        public List<Vector2Int> vertices = new List<Vector2Int>();

        [Header("Open edges (no walls, front-facing parapet)")]
        public List<int> openEdgeIndices = new List<int>();

        [Header("Entry points")]
        public List<Vector2Int> entryPoints = new List<Vector2Int>();
    }
}
```

Sample asset `Assets/Data/Rooms/SampleRoomFootprint.asset`:
- Diamond room: vertices (0,0), (4,1), (5,3), (4,5), (0,5), (-1,3) — 6-sided diamond-ish
- openEdgeIndices: [0] — bottom edge open

---

## PHASE E — WallChainBuilder Algorithm

**Path:** `Assets/Scripts/Runtime/Walls/WallChainBuilder.cs` (static class)

```csharp
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RIMA.Walls
{
    public static class WallChainBuilder
    {
        public static GameObject Build(RIMA.Rooms.RoomFootprintPolygon footprint, WallChunkLibrary library, Transform parent)
        {
            var root = new GameObject("WallChainRoot");
            root.transform.SetParent(parent);

            for (int i = 0; i < footprint.vertices.Count; i++)
            {
                bool isOpenEdge = footprint.openEdgeIndices.Contains(i);
                if (isOpenEdge) continue; // skip wall placement, will be open parapet

                Vector2Int v1 = footprint.vertices[i];
                Vector2Int v2 = footprint.vertices[(i + 1) % footprint.vertices.Count];

                EdgeType edgeType = ClassifyEdge(v1, v2);
                
                // Place connector column at v1
                PlaceConnectorColumn(library, v1, edgeType, root.transform);

                // Fill wall span between v1 and v2
                FillWallSpan(library, v1, v2, edgeType, root.transform);

                // Place seam overlay at junction
                PlaceSeamOverlay(library, v1, edgeType, GetNextEdgeType(footprint, i), root.transform);
            }

            return root;
        }

        public enum EdgeType { NE, NW, SE, SW, N, S, E, W }

        static EdgeType ClassifyEdge(Vector2Int v1, Vector2Int v2)
        {
            Vector2Int delta = v2 - v1;
            // Iso grid edge classification
            if (delta.x > 0 && delta.y == 0) return EdgeType.E;
            if (delta.x < 0 && delta.y == 0) return EdgeType.W;
            if (delta.y > 0 && delta.x == 0) return EdgeType.N;
            if (delta.y < 0 && delta.x == 0) return EdgeType.S;
            if (delta.x > 0 && delta.y > 0) return EdgeType.NE;
            if (delta.x > 0 && delta.y < 0) return EdgeType.SE;
            if (delta.x < 0 && delta.y > 0) return EdgeType.NW;
            return EdgeType.SW;
        }

        static EdgeType GetNextEdgeType(RIMA.Rooms.RoomFootprintPolygon footprint, int currentIdx)
        {
            int nextIdx = (currentIdx + 1) % footprint.vertices.Count;
            Vector2Int v1 = footprint.vertices[nextIdx];
            Vector2Int v2 = footprint.vertices[(nextIdx + 1) % footprint.vertices.Count];
            return ClassifyEdge(v1, v2);
        }

        static void PlaceConnectorColumn(WallChunkLibrary library, Vector2Int pos, EdgeType edge, Transform parent)
        {
            WallChunkData data = library.GetConnectorFor(edge);
            if (data == null) return;
            
            GameObject prefab = library.GetPrefab(data);
            Vector3 worldPos = IsoGridToWorld(pos);
            var instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, parent);
            instance.name = $"Connector_{edge}_{pos.x}_{pos.y}";
        }

        static void FillWallSpan(WallChunkLibrary library, Vector2Int v1, Vector2Int v2, EdgeType edge, Transform parent)
        {
            int distance = Mathf.Abs(v2.x - v1.x) + Mathf.Abs(v2.y - v1.y);
            Vector2Int step = new Vector2Int(System.Math.Sign(v2.x - v1.x), System.Math.Sign(v2.y - v1.y));

            Vector2Int current = v1 + step;
            while (current != v2)
            {
                WallChunkData data = library.GetSpanFor(edge, WallType.WallSpan_Short);
                if (data != null)
                {
                    GameObject prefab = library.GetPrefab(data);
                    Vector3 worldPos = IsoGridToWorld(current);
                    var instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, parent);
                    instance.name = $"Span_{edge}_{current.x}_{current.y}";
                }
                current += step;
            }
        }

        static void PlaceSeamOverlay(WallChunkLibrary library, Vector2Int pos, EdgeType prevEdge, EdgeType nextEdge, Transform parent)
        {
            // Seam at junction between prevEdge and nextEdge
            WallChunkData data = library.GetSeamFor(prevEdge, nextEdge);
            if (data == null) return;
            GameObject prefab = library.GetPrefab(data);
            Vector3 worldPos = IsoGridToWorld(pos);
            var instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, parent);
            instance.name = $"Seam_{prevEdge}_{nextEdge}_{pos.x}_{pos.y}";
        }

        static Vector3 IsoGridToWorld(Vector2Int isoCell)
        {
            // Unity IsometricZAsY: x = (col - row) * cellWidth/2, y = (col + row) * cellHeight/2
            // For cell size 128×64 at PPU=64 → world units (2, 1)
            float worldX = (isoCell.x - isoCell.y) * 1f;  // 2/2
            float worldY = (isoCell.x + isoCell.y) * 0.5f;  // 1/2
            return new Vector3(worldX, worldY, 0);
        }
    }
}
```

**Path:** `Assets/Scripts/Runtime/Walls/WallChunkLibrary.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

namespace RIMA.Walls
{
    [CreateAssetMenu(fileName = "WallChunkLibrary", menuName = "RIMA/Walls/Wall Chunk Library")]
    public class WallChunkLibrary : ScriptableObject
    {
        [System.Serializable]
        public struct LibEntry
        {
            public WallChunkData data;
            public GameObject prefab;
        }

        public List<LibEntry> entries = new List<LibEntry>();

        public WallChunkData GetConnectorFor(WallChainBuilder.EdgeType edge)
        {
            // For MVP, return universal pillar regardless of edge
            return entries.Find(e => e.data.wallType == WallType.Pillar).data;
        }

        public WallChunkData GetSpanFor(WallChainBuilder.EdgeType edge, WallType type)
        {
            // For MVP, return NW or NE based on edge
            var span = entries.Find(e =>
                (edge == WallChainBuilder.EdgeType.N || edge == WallChainBuilder.EdgeType.NE) &&
                e.data.chunkId.Contains("ne_mid_plain"));
            if (span.data != null) return span.data;
            
            return entries.Find(e => e.data.chunkId.Contains("nw_mid_plain")).data;
        }

        public WallChunkData GetSeamFor(WallChainBuilder.EdgeType prev, WallChainBuilder.EdgeType next)
        {
            // For MVP, no seam overlays — return null
            return null;
        }

        public GameObject GetPrefab(WallChunkData data)
        {
            return entries.Find(e => e.data == data).prefab;
        }
    }
}
```

---

## PHASE F — 17 Mevcut Prefab Retrofit

Her existing wall prefab'a WallChunk component + WallChunkData asset ekle:

| Prefab | WallChunkData chunkId | wallType | footprintCells | anchorOffset |
|---|---|---|---|---|
| wall_nw_mid_plain | nw_mid_plain | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_nw_mid_variant | nw_mid_variant | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_nw_mid_broken | nw_mid_broken | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_nw_doorway | nw_doorway | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_ne_mid_plain | ne_mid_plain | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_ne_mid_variant | ne_mid_variant | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_ne_mid_broken | ne_mid_broken | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_ne_doorway | ne_doorway | WallSpan_Short | [(0,0)] | (0, -0.5) |
| wall_n_corner | n_corner | Connector_CornerOuter | [(0,0)] | (0, -0.5) |
| wall_n_landmark | n_landmark | Landmark | [(0,0),(1,0)] | (0, -0.5) |
| wall_pillar_universal | pillar | Pillar | [(0,0)] | (0, -0.5) |

WallChunkData assets `Assets/Data/Walls/Act1_ShatteredKeep/HighTopDown_3_4/` altında.

WallChunkLibrary asset `Assets/Data/Walls/Act1_ShatteredKeep_Library.asset` — 17 entry içerir.

---

## PHASE G — Test Scene v2

`DiamondRoom_v1.unity` scene'inde:

1. **RoomFootprintPolygon** sample asset oluştur:
   - Vertices: (0,0), (4,1), (5,3), (4,5), (0,5), (-1,3) — 6-sided diamond
   - openEdgeIndices: [0] — bottom edge açık
   - entryPoints: (2,0)

2. **Build wall chain**:
   - WallChainBuilder.Build(footprint, library, scene.Walls)
   - Editor menu item `RIMA/Tools/Build Test Diamond Room`
   - OR runtime call from a test script `TestRoomBuilder.cs`

3. **Floor tilemap fill**:
   - Iso tilemap'e walkable cells'i floor RandomTile ile fill
   - Cells in polygon interior

4. **Warblade placement** room center

5. **Lighting**: aynı bug1ol7lg setup'ı

6. **Play mode test** + screenshot

---

## Phase H — Doc Update

`MEMORY/project_diamond_iso_tilemap_lock_2026_05_24.md` yeni dosya (user-level memory):

```markdown
---
name: diamond-iso-tilemap-lock-2026-05-24
description: "Diamond Iso Tilemap pivot — 2026-05-21 Rectangular Tilemap REVOKED. Karar #149."
metadata: 
  type: project
---

# Diamond Iso Tilemap LIVE — Karar #149

LIVE 2026-05-24: Unity Tilemap mode `IsometricZAsY` (cell size 1×0.5 world units, 128×64 px at PPU=64). ChatGPT analiz + industry standard (Hades/D3/CoM) match.

## Why
- HIGH TOP-DOWN 3/4 70-80° camera + iso tilemap = industry standard
- WallChunk footprint snap iso cell'e doğal
- Wall chain construction system iso grid'de daha temiz

## Supersedes
- [[project-topdown-pivot-lock]] (Rectangular tilemap claim REVOKED, camera angle zaten supersede edilmişti)
- 2026-05-21 S97 pivot Rectangular lock

## Reaffirms / Compatible
- [[project-high-top-down-3-4-lock-2026-05-24]] — camera angle korunur
- [[project-pillar-seam-cover-lock-2026-05-24]] — pillar strategy iso grid'e tam uyar
```

MEMORY.md update (index'e ekle).

---

## Verification (PASS criteria)

1. `DiamondRoom_v1.unity` scene exists
2. Iso tilemap mode `IsometricZAsY` set
3. WallChunkData / WallChunk / RoomFootprintPolygon / WallChainBuilder / WallChunkLibrary all py_compile + .cs compile clean
4. 17 prefab retrofit edilmiş (WallChunk component + WallChunkData asset)
5. WallChunkLibrary asset 17 entry içeriyor
6. Sample RoomFootprintPolygon asset mevcut
7. Editor menu `RIMA/Tools/Build Test Diamond Room` çalışıyor
8. Test scene Play mode başarılı, console clean
9. Screenshot `STAGING/screenshots/diamond_room_v1.png` kaydedildi

---

## Çıktı Raporu

`STAGING/codex_wall_chain_builder_DONE.md` yaz:
- Created files (paths)
- Compile status
- Test scene observations
- Screenshot path
- Issues / blockers
- ChatGPT analysis adherence checklist
- Next dispatch önerisi

git commit otomatik yapma — orchestrator review sonrası.

## Effort

4-6 saat Codex. Major phases A → H sequential. BLOCKED durumlarda orchestrator'a sor (komut satırından mesaj).
