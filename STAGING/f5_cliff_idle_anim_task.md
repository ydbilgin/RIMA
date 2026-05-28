# F5: Cliff Face Idle Animation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Cliff face sprite subtle idle anim — DirectionalCliffTile_Hades sprite array swap timer ile randomized. Default-on (user kararı).

## İş kalemleri
1. **NEW `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs`** (~60 LOC)
   - MonoBehaviour, CliffTilemap GameObject'e attach
   - Periodic Coroutine: ~3-5s interval
   - Random cliff cell seç (cluster içinden, ManualPaintedCells exclude OK)
   - DirectionalCliffTile.cs sprite array variant'larından random pick (D5 sprite arrays LIVE)
   - SetTile cell, refresh
   - Per-cell hash offset (deterministic per-instance)
2. **Per-instance animation offset:**
   - cell.x + cell.y hash → unique phase shift
   - 4-6 frame cycle, slow (3-5s per cycle)
3. **Performance:**
   - Camera frustum check: visible cell'ler only
   - Max animated cell: 20 simultaneously

## Dosyalar
- `CliffFaceIdleAnimator.cs` NEW
- `PlayableArena_Test01.unity` (CliffTilemap GameObject'e attach component)
- ~80 LOC total

## Verify
- 0 err / 0 warn
- PlayMode aç → cliff sprite'ları periodically subtle variant değişir
- Performance: 60 FPS korunur

## YASAK
- DirectionalCliffTile.cs modify (LIVE D5)
- DirectionalCliffTile_Hades.asset modify (LIVE D5)
- AnimatedTile yeni asset oluşturma (script-based yeterli)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU

## Code rotation
Sen Sonnet yaz. Reviewer Opus visual coherence F5 PASS sonrası.

Output: `STAGING/F5_CLIFF_ANIM_DONE.md`
