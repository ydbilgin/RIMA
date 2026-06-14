# F2: Procedural Cliff Drop Shadow Tilemap

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Cliff base'inden aşağı doğru procedural alpha gradient drop shadow. "Yükseklik" illusion strong. F design Bölüm 4 spec.

## İş kalemleri
1. **NEW `Assets/Scripts/Environment/CliffDropShadowGenerator.cs`** (~80 LOC)
   - GroundBlobShadow.cs pattern reuse (`Assets/Scripts/Environment/GroundBlobShadow.cs` mevcut)
   - Procedural Texture2D üretici: 32×16 px alpha gradient (top opaque 0.6 → bottom transparent 0)
   - Sprite oluşturucu Sprite.Create runtime
   - Singleton cache (DontSave)
2. **NEW `Assets/Scripts/Environment/CliffDropShadowPlacer.cs`** (~40 LOC)
   - CliffAutoPlacer.Regenerate sonrası hook → CliffTilemap her cell altına shadow tilemap'e SetTile
   - DecorCliffTilemap pattern reuse (D5.5 LIVE)
3. **Scene wire:** `PlayableArena_Test01.unity` Floor Grid altına yeni `CliffDropShadowTilemap` GameObject
   - TilemapRenderer sortingLayerName=`Decor_Cliff`, sortingOrder=`-20` (cliff base altında)
   - Material = Sprite-Lit-Default

## Dosyalar
- `CliffDropShadowGenerator.cs` NEW
- `CliffDropShadowPlacer.cs` NEW
- `PlayableArena_Test01.unity` (yeni Tilemap GameObject)
- ~120 LOC total

## Verify
- 0 err / 0 warn
- PlayMode aç → cliff cell'lerin altında soft shadow gradient görünür
- Shadow z-order: cliff base (-1) > shadow (-20) > floor (0)

## YASAK
- CliffAutoPlacer.cs core algorithm değişiklik (F1 LIVE)
- DecorCliffTilemap dokunma (D5.5 LIVE, paralel ayrı sistem)
- DirectionalCliffTile_Hades modify (D5 LIVE)
- PixelLab asset gen (gece halt, procedural ZORUNLU)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU

## Code rotation
Sen Sonnet yaz. Reviewer Codex xhigh F2 PASS sonrası.

Output: `STAGING/F2_DROP_SHADOW_DONE.md`
