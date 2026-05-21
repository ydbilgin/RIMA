# CODEX TASK — Painter Wang Adjacency Graceful Degrade (3-Piece Mode)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun (Orchestrator Diagnose)

`Assets/Editor/RimaUnifiedPainterWindow.cs:3576-3697` `UpdateWallConnectionsAt` + `ApplyWallConnectionFamily` **4 wall piece** bekliyor (face_NS + face_EW + corner + crack). Mevcut Pilot A pack'inde **sadece 3 var** (face_NS 2nd-pass audit'te archived; face_EW + corner_outer + arch_opening kalan).

Sonuç:
1. `face_NS` yok → `pNW_SE` ve `pNE_SW` her ikisi de `face_EW` kullanıyor → yan yana iki cell paint **aynı sprite** çıkıyor → "duvarlar yan yana seri seri gitmiyor" (user feedback).
2. `arch_opening` "arch" keyword → `damaged/crack` rolüne map (line 3692) → tek-neighbor case'inde %15 olasılıkla rastgele arch çıkıyor (lines 3601, 3606) → user istemediği yerlerde rift arch.

## Fix — Katman A: Graceful Degrade

### Fix 1 — `ApplyWallConnectionFamily` (line 3678-3698)
`arch` keyword'u **damaged/crack rolünden ÇIKAR**. Şu an:
```csharp
GameObject damaged = FindWallPrefabByKeyword(family, "crack") ?? FindWallPrefabByKeyword(family, "arch") ?? face;
```
Yeni:
```csharp
GameObject damaged = FindWallPrefabByKeyword(family, "crack") ?? face;
```
Yani: explicit "crack" keyword içeren prefab yoksa `face` kullan (random arch yerine).

### Fix 2 — `UpdateWallConnectionsAt` tek-face-piece graceful degrade (line 3592-3597)
Şu an:
```csharp
GameObject pNW_SE = wallPrefabs[0].prefab;
GameObject pNE_SW = wallPrefabs.Count > 1 ? wallPrefabs[1].prefab : pNW_SE;
```

Eğer `pNW_SE == pNE_SW` (yani face_NS yok, ikisi de aynı face_EW) durumunda **Y rotation 90°** ekle NE-SW yönü için. `rotationSteps` mantığını güncelle:

- (hasNE || hasSW) && !hasNW && !hasSE → use face_EW with `rotationSteps = 0`
- (hasNW || hasSE) && !hasNE && !hasSW → use face_EW with `rotationSteps = 1` (yani 90° döndür) — **YENİ**

Bu graceful degrade `face_NS prefab yokluğunda` da 4 direction wang variation sağlar (rotation ile).

**Detay logic:**
```csharp
// Detect single-face fallback case
bool isSingleFaceFallback = (pNW_SE == pNE_SW);

if ((hasNE || hasSW) && !hasNW && !hasSE)
{
    newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f && pCrack != face) ? pCrack : pNE_SW;
    rotationSteps = 0;
}
else if ((hasNW || hasSE) && !hasNE && !hasSW)
{
    newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f && pCrack != face) ? pCrack : pNW_SE;
    rotationSteps = isSingleFaceFallback ? 1 : 0;  // 90° rotation when no face_NS available
}
```

`pCrack != face` check'i Fix 1 sonrası `pCrack`'in `face`'a fallback olduğu durumda crack injection'ı önler.

### Fix 3 — `arch_opening` filterleme (line 3593-3596, wall prefab array build)

`arch` keyword içeren prefab'ları Wang adjacency wallPrefabs array'inden ÇIKAR. Decoration/manual placement için palette'te kalsın ama auto-connect formülünü etkilemesin.

Önerilen approach: `wallPrefabs` build edilirken (muhtemelen `ScanWallPrefabsMultiFolder` veya benzeri) filtre ekle:
- "arch" keyword'lü prefab'ları `wallPrefabs` Wang-list'inden çıkar
- Ama palette `wallPalette` listesinde kalsın (user'ın manual seçimi için)

Eğer iki list aynıysa ayrılması gerek. Veya minimal değişim: `UpdateWallConnectionsAt` içinde `pCrack = pCrack.name contains "arch" ? face : pCrack` filter.

## Test (UnityMCP)

1. Sahneyi aç `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`
2. Painter aç, Wall sekmesi
3. `pilot_a_wall_face_EW` seç, auto-connect ON
4. 3 yan yana cell'e paint et: (5,5), (5,6), (5,7)
5. Sonuç bekle:
   - 3 wall görünür, parent `Walls_Root/Walls/...`
   - Adjacency variation: orta cell (5,6) iki neighbor'lu → connection variant, kenarlar tek neighbor
   - Hiç random `arch_opening` çıkmamalı (Fix 1 + 3)
   - Wall sprite'ları görsel olarak yan yana, gap/overlap minimum
6. Test objelerini Undo ile temizle, sahne save etme

## Allowed File Writes

- **MODIFY:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (3 fix bölgesi: 3592-3597, 3601-3608, 3692)

## Forbidden

- Diğer painter logic'ine dokunma (Fix 1+2+3 sınırlı)
- Sahne dosyalarına dokunma
- prefab dosyalarına dokunma

## Rapor

`STAGING/CODEX_DONE_painter_wang_degrade.md`:
- Fix uygulanan line'lar
- Compile 0 error verify (`read_console`)
- UnityMCP test sonucu (3 yan yana wall paint, sprite'lar variation, hiç random arch çıkmadı)
- Cleanup verify

## Effort

medium — 3 yer fix + UnityMCP test, ~30 dakika.
