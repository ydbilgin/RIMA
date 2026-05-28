# F1: AdaptiveClusterFilter — DONE

**Date:** 2026-05-27  
**Agent:** Sonnet (mekanik impl)  
**Status:** PASS — 0 compile errors, 0 warnings

---

## Değişen Dosyalar

| Dosya | Değişim |
|---|---|
| `Assets/Scripts/Environment/CliffClusterRules.cs` | NEW ~25 LOC — ScriptableObject class |
| `Assets/ScriptableObjects/Environment/CliffClusterRules_Default.asset` | NEW — default asset (minN=4, ratio=0.005, 8conn=true) |
| `Assets/Scripts/Environment/CliffAutoPlacer.cs` | EXTEND ~90 LOC — clusterRules field + CollectFloorCells + ComputeOrphanClusters + BFSFloodFill + CountOrphanCells + ClusterRules property |
| `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` | EXTEND ~12 LOC — orphan count display + Adaptive Filter ENABLED/DISABLED badge |

---

## Cliff Count Before / After

| Durum | Count |
|---|---|
| Before (D5.6 reality) | 283 cliff tiles (166 cluster, 155 orphan cluster ≤3 cell) |
| After (expected after Regenerate + ClusterRules assigned) | ~128 cliff tiles (155 orphan floor cells excluded) |

> Actual count doğrulanması: Unity'de PlayableArena_Test01 açıp CliffAutoPlacer Inspector → `clusterRules` slot'a `CliffClusterRules_Default` ata → Regenerate (C) → `lastGeneratedCount` kontrol.

---

## Verify Checklist

- [x] `refresh_unity scope=all mode=force` → connection closed (domain reload) — normal
- [x] `refresh_unity mode=if_dirty` → success, idle
- [x] `read_console types=error` → 0 errors
- [x] `read_console types=warning` → 0 warnings
- [ ] PlayableArena_Test01 CliffAutoPlacer Inspector: `clusterRules` slot doludu (kullanıcı manuel ata)
- [ ] Regenerate (C) çağır → cliff count 283 → ~128
- [ ] Inspector: "Adaptive Filter ENABLED" badge görünür
- [ ] Inspector: "Orphan Cell Count" label sayı gösterir
- [ ] Manual paint (Shift+Click → D5.5 LIVE DecorCliffTilemap) hala çalışır
- [ ] ManualPaintedCells whitelist bozulmadı

---

## Teknik Notlar

### BFSFloodFill 8-connectivity neighbors
```
Iso HARD vectors: S=(-1,-1), N=(1,1), E=(1,-1), W=(-1,1), SE=(0,-1), SW=(-1,0)
8-connectivity ekler: NE=(1,0), NW=(0,1)
4-connectivity fallback: sadece S,N,E,W (clusterRules.use8Connectivity=false)
```

### Heuristic
```
cluster.Count < minClusterSize AND ratio < coverageRatioFallback → orphan
default: minN=4, ratio=0.005 (0.5%)
```

### Safe Defaults
- `clusterRules == null` → ComputeOrphanClusters returns empty → no filter, backward compatible
- ManualPaintedCells whitelist: D5.5 LIVE, dokunulmadı
- ValidateManualPainted: D5.5 LIVE, dokunulmadı
- DirectionalCliffTile_Hades: dokunulmadı
- YASAK: tüm maddeler sağlandı

---

## Next: Codex xhigh Review
F1 PASS sonrası Codex review (algoritma audit, edge case) — `feedback_code_writer_rotation` HARD kuralı gereği.

Asset GUID: `e628b5be416951049945901977cc0d87`
