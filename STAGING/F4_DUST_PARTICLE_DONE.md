# F4: Cliff Edge Dust Particle — DONE

**Date:** 2026-05-27  
**Status:** PASS — 0 compile errors, 0 warnings

## Delivered Files

| File | Status |
|---|---|
| `Assets/Scripts/Environment/CliffDustSettings.cs` | NEW — ~35 LOC ScriptableObject config |
| `Assets/Scripts/Environment/CliffEdgeDustEmitter.cs` | NEW — ~145 LOC emitter MonoBehaviour |
| `Assets/ScriptableObjects/Environment/CliffDustSettings_Default.asset` | NEW — default SO instance |

## Architecture

**CliffDustSettings (SO)** — all tweakable params in one asset:
- emissionRate (0.75/s default), lifetime (1.5–2s), fallSpeed, lateralSpread, gravityModifier
- colorTint: warm dust brown at alpha 0.3
- maxTotalParticles: 200 (scene-wide cap)
- lodCullDistance: 20 units

**CliffEdgeDustEmitter** — separate manager, reads from CliffAutoPlacer:
- CliffAutoPlacer NOT modified (F1 LIVE rule honoured)
- Reads floorTilemap directly, applies same S/SE/SW void-neighbor logic to collect edge cells
- Spawns one ParticleSystem child per edge cell under `_DustEmitters` root
- Particle config: downward velocity + tiny lateral spread, alpha fade-out gradient, Billboard renderMode
- Sorting: Ground layer, order -1 (matches cliff base)
- URP 2D default Particle material — no custom shader

**Performance guard (Update loop):**
- Counts totalActive particles across all emitters
- If `totalActive >= maxTotalParticles` → disables emission on all (stops new births, existing fade naturally)
- Camera sqrMagnitude check against `lodCullDistance` — distant emitters switch emission off

**Editor support:**
- `OnValidate` rebuilds emitters in edit mode when settings change
- `[ContextMenu("Rebuild Dust Emitters")]` for manual rebuild

## Wiring Instructions (PlayableArena_Test01)
1. Add `CliffEdgeDustEmitter` component to any GameObject (e.g. "DustManager")
2. Assign: `cliffPlacer` → CliffAutoPlacer in scene, `settings` → `CliffDustSettings_Default`
3. Enter PlayMode — dust particles fall from cliff edges

## Verify Checklist
- [x] 0 compile errors / 0 warnings
- [ ] PlayMode: subtle downward dust visible at cliff edges (alpha 0.3 warm brown)
- [ ] Performance: 200 particle cap respected, FPS stable
- [ ] LOD: emitters beyond 20u from camera stop emitting

## YASAK Compliance
- CliffAutoPlacer.cs: NOT modified
- No custom shader
- refresh_unity scope=all mode=force: DONE (Unity reconnected, 0 script errors)
