# Codex Task — WallChainBuilder Continuation (Phase F + G + H) (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Önceki dispatch (bnc0am4rw) **timeout sonrası yarıda kaldı**. 4 script + 1 scene oluşturuldu. Şimdi kalan 3 phase'i tamamla.

## Mevcut Durum (önceki dispatch'ten kalan)

✅ Oluşturulmuş:
- `Assets/Scripts/Runtime/Walls/WallChunkData.cs`
- `Assets/Scripts/Runtime/Walls/WallChunk.cs`
- `Assets/Scripts/Runtime/Walls/WallChainBuilder.cs`
- `Assets/Scripts/Runtime/Walls/WallChunkLibrary.cs`
- `Assets/Scenes/Demo/DiamondRoom_v1.unity`

❓ Bilinmeyen / yapılmamış:
- `Assets/Scripts/Runtime/Rooms/RoomFootprintPolygon.cs` — kontrol et, yoksa oluştur
- 17 prefab retrofit (WallChunk + WallChunkData assignment)
- WallChunkData asset'ları (`Assets/Data/Walls/Act1_ShatteredKeep/HighTopDown_3_4/`)
- WallChunkLibrary asset (`Assets/Data/Walls/Act1_ShatteredKeep_Library.asset`)
- Sample RoomFootprintPolygon asset (`Assets/Data/Rooms/SampleRoomFootprint.asset`)
- DiamondRoom_v1.unity scene compose (WallChainBuilder.Build çağrısı)
- Memory file (Karar #149 Diamond Iso Tilemap)

## Sıra (Kısıtlı timeout — hızlı ilerle)

### Step 1 — Status check (max 3 dk)
1. `Assets/Scripts/Runtime/Walls/` ve `Assets/Scripts/Runtime/Rooms/` listeleyip mevcut dosyaları kontrol et
2. `dotnet build` veya Unity compile check — script'ler hata vermiyor mu
3. `read_console` — clean mi

Eğer 4 script compile-clean ise Step 2'ye geç. Hatalı ise düzelt önce.

### Step 2 — RoomFootprintPolygon.cs (max 5 dk)
`Assets/Scripts/Runtime/Rooms/RoomFootprintPolygon.cs` yoksa oluştur:

```csharp
using UnityEngine;
using System.Collections.Generic;

namespace RIMA.Rooms
{
    [CreateAssetMenu(fileName = "RoomFootprint", menuName = "RIMA/Rooms/Room Footprint Polygon")]
    public class RoomFootprintPolygon : ScriptableObject
    {
        public List<Vector2Int> vertices = new List<Vector2Int>();
        public List<int> openEdgeIndices = new List<int>();
        public List<Vector2Int> entryPoints = new List<Vector2Int>();
    }
}
```

### Step 3 — 17 WallChunkData asset oluştur (max 15 dk)

`Assets/Data/Walls/Act1_ShatteredKeep/HighTopDown_3_4/` klasörü oluştur. 17 .asset:

| Asset adı | chunkId | wallType | footprintCells | sprite ref |
|---|---|---|---|---|
| wall_nw_mid_plain.asset | nw_mid_plain | WallSpan_Short | [(0,0)] | wall_nw_mid_plain sprite |
| wall_nw_mid_variant.asset | nw_mid_variant | WallSpan_Short | [(0,0)] | wall_nw_mid_variant sprite |
| wall_nw_mid_broken.asset | nw_mid_broken | WallSpan_Short | [(0,0)] | wall_nw_mid_broken sprite |
| wall_nw_doorway.asset | nw_doorway | WallSpan_Short | [(0,0)] | wall_nw_doorway sprite |
| wall_ne_mid_plain.asset | ne_mid_plain | WallSpan_Short | [(0,0)] | wall_ne_mid_plain sprite |
| wall_ne_mid_variant.asset | ne_mid_variant | WallSpan_Short | [(0,0)] | wall_ne_mid_variant sprite |
| wall_ne_mid_broken.asset | ne_mid_broken | WallSpan_Short | [(0,0)] | wall_ne_mid_broken sprite |
| wall_ne_doorway.asset | ne_doorway | WallSpan_Short | [(0,0)] | wall_ne_doorway sprite |
| wall_n_corner.asset | n_corner | Connector_CornerOuter | [(0,0)] | wall_n_corner sprite |
| wall_n_landmark.asset | n_landmark | Landmark | [(0,0),(1,0)] | wall_n_landmark sprite |
| wall_pillar_universal.asset | pillar | Pillar | [(0,0)] | wall_pillar_universal sprite |
| iso_floor_clean.asset | floor_clean | (not wall — atla) |
| iso_floor_cracked.asset | floor_cracked | (atla) |
| iso_floor_rift_glow.asset | floor_rift_glow | (atla) |
| iso_floor_broken.asset | floor_broken | (atla) |
| iso_floor_edge_light.asset | floor_edge_light | (atla) |
| iso_floor_debris.asset | floor_debris | (atla) |

Yani: **11 wall asset için WallChunkData oluştur**. Floor için ayrı sistem (Tilemap RuleTile).

### Step 4 — 11 prefab retrofit (max 10 dk)

`Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/HighTopDown_3_4/` altında 11 wall prefab var. Her birine:
- WallChunk component ekle
- Karşılık gelen WallChunkData asset assign et
- FootprintAnchor, LeftSocket, RightSocket, TorchSocket, BannerSocket, SeamSocket child Transform'lar ekle (boş GameObject)

Editor script ile bulk processing yap (foreach prefab → modify → save).

### Step 5 — WallChunkLibrary asset (max 5 dk)

`Assets/Data/Walls/Act1_ShatteredKeep_Library.asset` oluştur. 11 wall entry içersin (data + prefab pairs).

### Step 6 — Sample RoomFootprintPolygon (max 3 dk)

`Assets/Data/Rooms/SampleRoomFootprint.asset` oluştur:
- vertices: (0,0), (4,1), (5,3), (4,5), (0,5), (-1,3) — 6-sided diamond
- openEdgeIndices: [0]
- entryPoints: (2,0)

### Step 7 — Test scene compose (max 15 dk)

`DiamondRoom_v1.unity` scene'inde:

1. **Iso Tilemap setup** — eğer scene bos veya partial ise:
   - Grid GameObject `Cell Layout: Isometric Z As Y`, Cell Size (1, 0.5, 1)
   - Floor Tilemap child (iso tilemap)
   - Walls empty parent

2. **Build wall chain çağrısı**:
   - Editor menu `[MenuItem("RIMA/Tools/Build Test Diamond Room")]` static metod:
     - Load SampleRoomFootprint asset
     - Load WallChunkLibrary asset
     - Call WallChainBuilder.Build(footprint, library, walls_parent)
   - Menu'den çağır, wall chain instantiate olsun

3. **Floor tilemap fill** — interior cells'e floor RandomTile paint et (en az 20 cell)

4. **Warblade** ortada (mevcut warblade_south.png ile placeholder)

5. **Camera** Orthographic, PixelPerfect PPU=64, position (2, 2, -10)

6. **Lighting**: Global Light cool, 2 warm point light (torch), 1 cyan rift point

7. Save scene

8. **Play mode test** — 3 saniye dene, console clean kontrol et

9. Screenshot al → `STAGING/screenshots/diamond_room_v1.png`

### Step 8 — Memory file (max 5 dk)

`C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_diamond_iso_tilemap_lock_2026_05_24.md` oluştur:

```markdown
---
name: diamond-iso-tilemap-lock-2026-05-24
description: "Diamond Iso Tilemap pivot LIVE. 2026-05-21 Rectangular Tilemap REVOKED. Karar #149."
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
- [[project-topdown-pivot-lock]] (Rectangular tilemap claim REVOKED)
- 2026-05-21 S97 pivot Rectangular lock

## Compatible
- [[project-high-top-down-3-4-lock-2026-05-24]] — camera angle korunur
- [[project-pillar-seam-cover-lock-2026-05-24]] — pillar strategy iso grid'e tam uyar
```

`MEMORY.md` index'e bir satır ekle:
```
- project_diamond_iso_tilemap_lock_2026_05_24.md - ⭐ LIVE 2026-05-24: Diamond Iso Tilemap pivot, Karar #149.
```

## Çıktı raporu

`STAGING/codex_wall_chain_continuation_DONE.md` yaz, kısa:
- Created files (paths)
- Compile status
- Test scene play mode result
- Screenshot path
- Issues / blockers

git commit otomatik yapma.

## Effort

~50-60 dk total. Acil ilerle, BLOCKED durumlarda dur.
