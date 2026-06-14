# Codex Codebase Reality Check — Cliff Floating Feel

**Status:** Codex dispatch yarıda kaldı (yasinderyabilgin profile, 23:12 task yazıldı, 23:30 itibariyle CODEX_DONE boş, lock aktif, TaskStop yapıldı + lock manuel cleanup).
**Substitute:** Orchestrator Opus codebase analizini KENDİSİ yaptı (BFS cluster scan, scene parse, source audit). Aşağıdaki bulgular Opus orchestrator-substitute output'udur.

---

## Algorithm audit — CliffAutoPlacer.cs

- L212-244 `CollectCliffCells()`: cluster-aware DEĞİL. Basit kural: `if (sEmpty || seEmpty || swEmpty) cells.Add(cell)`.
- L186-190 Regenerate flow: BFS filter eklenebilir L189 `targets.UnionWith(ManualPaintedCells)` sonrasında (manuel whitelist filter'dan korunur).
- ManualOverrideCells (blacklist) + ManualPaintedCells (whitelist) S110 LIVE; A yaklaşımı bu set'leri intact bırakmalı.
- ValidateManualPainted() D5.5 cleanup, A önce bu çalışır.

## Sahne cluster sayım — PlayableArena_Test01.unity (BFS, iso 4-neighbor)

**Floor tilemap (2365 cell):**
- 18 cluster
- Top 5: 1166, 1114, 37, 8, 7
- size=1: 6, size=2: 2, size=3: 0, size=4-9: 7, size=10-99: 1, size=100+: 2

**Cliff tilemap (283 cell):**
- 166 cluster (!!)
- Top 5: 12, 9, 8, 8, 7
- size=1: 106 (!!)
- size=2: 38
- size=3: 11
- size=4+: 11

**Cliff parent floor cluster mapping:**
- parent_floor_cluster_size=1: 4 cliffs
- parent_floor_cluster_size=2: 1 cliff
- parent_floor_cluster_size=3: 0
- parent_floor_cluster_size=4: 5
- parent_floor_cluster_size=5: 3
- parent_floor_cluster_size=6-19: 3
- parent_floor_cluster_size=20+: 267

**KRİTİK BULGU:** İlk hipotez (izole floor cell üzerinde cliff) ÇÜRÜTÜLDÜ. Sadece 5 cliff "tiny floor" üstünde. Gerçek sorun: **cliff cluster'larının fragmente olması** — 155/166 cluster ≤3 cell. Her 1-cell cliff sprite top-pivot ile aşağı sarkıyor, çevresinde devamı yok → "asılı raf" hissi.

## Cliff sprite render geometry

- `DirectionalCliffTile.cs` L23-99: 8-yön sprite branch (hasN, hasS, hasE...), top-pivot 0.5,1.0
- `DirectionalCliffTile_Hades.asset`:
  - transformOffset: (0, 0, 0) — CURRENT 0
  - spriteScale: (1, 1)
  - spritesS: 5 varyant, diğer 7 yönde 1 sprite
- cliff_S.png.meta: spritePivot {x: 0.5, y: 1.0} (TOP-CENTER), spritePixelsToUnits: 64
- CliffTilemap GO m_LocalPosition.y = -0.3046875 (= cell height 0.609375 / 2, **yarım cell DOWN**, kasıtlı sarkma)

## Mevcut shadow/parallax infrastructure

- `GroundBlobShadow.cs` (99 satır): procedural runtime sprite generator (Texture2D + radial gradient + alpha falloff), player altı kullanılıyor.
  - **Pattern reusable** for cliff drop shadow: aynı tekniği CliffDropShadow için adapt et (vertical gradient, transformOffset -1 cell)
- `CliffDynamicFade.cs`: mevcut, içerik inspeksiyon edilmedi (Codex stuck, manuel atlandı)
- `RoomBackgroundRig` prefab YOK (Glob 0 hit)
- 3-Kit memory `project_3kit_bg_architecture_lock` LOCK ama scene wiring eksik

## Çözüm opsiyonu LOC tahminleri

| Opsiyon | Tahmin LOC | Risk | Test |
|---|---|---|---|
| A. Cluster size filter | ~50 | low | BFS unit test eklenebilir Phase1Demo |
| B. Dilate/erode morphology | ~80 | med | visual playtest |
| C. Drop shadow tilemap (GroundBlobShadow pattern reuse) | ~80 | med | visual playtest |
| D. BG_Far parallax 2-katman | ~80 + prefab + scene wire | low | visual playtest |
| E. transformOffset.y tune + sprite size reimport | ~5 + asset reimport | XS | visual playtest |
| F. Sang Hendrix 6-katman parallax full | ~120 + 4 asset | med | visual + perf |
| G. Hibrit A+C+D | ~210 | med | auto-test + visual |

## A implementation skeleton (önerilen)

```csharp
// CliffAutoPlacer.Regenerate() içinde, targets.UnionWith(ManualPaintedCells) sonrası
[Range(1,8)] public int minClusterSize = 4;
HashSet<Vector3Int> filtered = FilterTinyClusters(targets, minClusterSize, ManualPaintedCells);

// BFS connected component, iso 4-neighbor (NE, NW, SE, SW iso vectors)
private HashSet<Vector3Int> FilterTinyClusters(HashSet<Vector3Int> targets, int minN, HashSet<Vector3Int> whitelist)
{
    var result = new HashSet<Vector3Int>(targets);
    var visited = new HashSet<Vector3Int>();
    var IsoNeighbors = new[] { SouthCell, NorthCell, EastCell, WestCell };
    foreach (var cell in targets)
    {
        if (visited.Contains(cell)) continue;
        var component = new List<Vector3Int>();
        var q = new Queue<Vector3Int>();
        q.Enqueue(cell); visited.Add(cell);
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            component.Add(cur);
            foreach (var d in IsoNeighbors)
            {
                var n = cur + d;
                if (targets.Contains(n) && !visited.Contains(n)) { visited.Add(n); q.Enqueue(n); }
            }
        }
        if (component.Count < minN)
        {
            // remove from result, ama whitelist cell'leri koru
            foreach (var c in component) if (!whitelist.Contains(c)) result.Remove(c);
        }
    }
    return result;
}
```

## E implementation note

DirectionalCliffTile_Hades.asset:
```
transformOffset: {x: 0, y: -0.25, z: 0}  # CURRENT 0 → -0.25 (agy formülü)
```
Sprite reimport gerekli mi: cliff_S.png boyutu kontrol edilmedi (Codex stuck). Eğer 64x128 ise reimport 64x96. Eğer 64x96 zaten → sadece transformOffset değiş.

## Reality check soruları

1. RoomBackgroundRig prefab `Assets/Prefabs/Environment/` altında oluşturulmalı; 3-Kit memory'de path lock yok — confirm location.
2. URP 2D Lights setup ile drop shadow Multiply blend uyumu — material asset gerekli mi yoksa sprite-default OK?
3. CliffDynamicFade.cs cliff opacity manipulate ediyor mu — A+C ile çakışma riski var mı?
4. Phase1Demo auto-test asmdef'e T4_CliffNoFloatingSinglets eklemek için existing test pattern referans: T1 MapBoundaryTest.cs
5. Codex stuck root cause: lock pattern (`feedback_codex_stale_lock_after_taskstop` HARD rule yarın session start'ta investigate).

---
*Codex orchestrator-substitute report — 2026-05-27 gece*
