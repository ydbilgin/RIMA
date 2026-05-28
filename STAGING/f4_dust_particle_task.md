# F4: Cliff Edge Dust Particle System

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Cliff edge boyunca havada fall dust particle — "düşüş" hissi. Unity Particle System + URP 2D Lights.

## İş kalemleri
1. **NEW `Assets/Scripts/Environment/CliffEdgeDustEmitter.cs`** (~80 LOC)
   - CliffAutoPlacer cells → cliff edge cell'leri tespit (S/SE/SW void neighbor olan)
   - Her edge cell'e ParticleSystem instantiate (singleton GameObject veya pooled)
   - Emission: 0.5-1 particle/s, random offset
   - Velocity: -Y direction (downward), gravity 0.1
   - Lifetime: 1.5-2s, alpha fade out
   - Color: cliff base sprite dominant renk dimmer alpha 0.3
   - Sorting order: cliff base (-1) ile aynı
2. **Performance guard:**
   - Max active particle count: 200 total scene-wide
   - Camera distance LOD: uzak cliff'lerde emission off
3. **Configurable:** ScriptableObject `CliffDustSettings.asset` (emission rate, lifetime, color tint)

## Dosyalar
- `CliffEdgeDustEmitter.cs` NEW
- `Assets/ScriptableObjects/Environment/CliffDustSettings_Default.asset` NEW
- `Assets/Scripts/Environment/CliffDustSettings.cs` NEW (~20 LOC SO class)
- `PlayableArena_Test01.unity` (CliffEdgeDustEmitter GameObject attach)
- Toplam ~100 LOC

## Verify
- 0 err / 0 warn
- PlayMode aç → cliff edge'lerden subtle dust düşer (alpha fade)
- Performance: 60 FPS korunur 200 particle limit ile

## YASAK
- CliffAutoPlacer dokunma (F1 LIVE)
- Custom shader (URP 2D Particle default Material yeterli)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU

## Code rotation
Sen Sonnet yaz. Reviewer Codex xhigh F4 PASS sonrası.

Output: `STAGING/F4_DUST_PARTICLE_DONE.md`
