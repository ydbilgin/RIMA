# Lighting + Room-Fill — LOCK (S6, 2026-05-30) — 3-AI (NLM-input + agy + cx) → Opus

> User directive: don't blind-accept NLM; gas lamps look absurd on a floating slab → thematic effect + fill rooms; maybe enclose later.
> NLM lighting query timed out (so evaluation drove it, as asked). agy = visual/reference direction. cx = code-grounded implementable. Opus = lock.

## DECISION
**Floating-island identity KEPT, but shift to a "Ruined Keep" semi-enclosed hybrid, lit by seal-energy (cyan) — gas lamps REMOVED.**
This is the resolution of "maybe enclose rooms later": a hybrid (open slab + broken masonry framing), not full walls, not bare open slab.

### Layout (open vs enclosed → HYBRID)
- Open-floating slab kept; framed by **broken masonry** — sheared half-walls + cracked pillars + buttresses on TOP/SIDES (~1.5-2 player heights, block movement, catch light); BOTTOM/CORNERS stay open to the void (jagged crumbling edge). Ref: Hades Asphodel, Bastion.

### ⭐ LIGHT SOURCE LOGIC (refined — user's real concern: "floating, so where does light logically come from?")
agy verdict 2026-05-30: light comes from **BOTH** directions.
- **PRIMARY (from ABOVE):** a **Sky-Seal / overhead rift** key light — the dissolving seal in the void "sky" casts cyan key light DOWN. Gives top-down readability + downward-cast shadows + character contour (avoids the flat "floating asset" look). This is the world's "sun" equivalent.
- **SECONDARY (from BELOW / mid-air):** floor seal-rift cracks (emissive, up) · under-slab abyss rim-glow (defines silhouette) · **floating seal-shards** (mid-air hovering crystals = local point lights, moving shadows).
- **The discrete "Brazier" lamps are ILLOGICAL on a floating slab → CONVERT them into floating seal-shards:** reskin as hovering magical stones, bob at ~shoulder height (sine-wave script), KEEP their Light2D (now cyan) → shifting dynamic shadows. No lamp fixtures anywhere.
- Refs: Hades (Asphodel under-light + ambient), Bastion (overhead calamity-sky key + baked directional shadow), Hyper Light Drifter (cool ambient + neon emissive), Ori/GRIS/Children of Morta.
- One-line: **BOTH** — sky-seal key from above + floor-rift/abyss/shard fill from below.

### Light recipe (replaces the 4 warm Brazier lights)
| Role | Color | Intensity | Notes |
|---|---|---|---|
| Void ambient (global) | deep purple `#1F0C2B` | 0.2-0.3 | never pitch black |
| Slab rim-glow (under-edge, upward) | magenta/violet `#3D1E6D`→`#5A1E8C` | 0.7-0.9 | defines floating silhouette |
| Seal rift/glyph (KEY, emissive) | cyan `#00FFCC` | 1.2-1.5 | floor cracks/runes only, slow pulse, cyan <15% frame |
| Floating seal-shard (2nd key) | pale cyan `#7DFDFE` | 0.5-0.6 | on hovering crystal props → moving shadows |

### Room-fill (3 zones, combat stays clear)
- **Center 60%:** combat-clear; flat/low decor (cracked tiles, etched seal circle) + ONE Hero Prop (e.g. chained cracked pedestal).
- **Perimeter 25%:** vertical framing — shattered pillars (alpha-fade when player behind), hanging chains (sway on hit), crumbling half-walls.
- **BG/FG parallax:** distant severed towers + floating chunks + purple nebula; sparse FG archways/chains.

## IMPLEMENTATION (cx-mapped, scene-only first pass, no new runtime system)
1. **Lights:** retune the 4 `Brazier_*_WarmLight` Point Light2D in `PlayableArena_Test01` from orange `{1,0.5,0.2}` @4.5 → cyan `{0,1,0.8}` per the recipe (seal-rift key). Hide/swap the visible `Brazier_*` sprites for rift-crack/seal-glyph sources. Add the purple void-ambient + magenta under-rim if not present. (Scene lines cited in `CODEX_DONE.md` lighting analysis.)
2. **Room-fill:** set each phase room's `RoomSequenceData.focalElementPrefab` to an import-ready ShatteredKeep statue/shrine/rift-crystal (RoomLoader already instantiates it, `RoomLoader.cs:258-260`). For edge framing >1 prop, extend `RoomSequenceData` with a decor array (or use existing `PropSpec` with `emitsLight`/`lightSourceKind`/anchor/depth) in the same `BuildRoomContent` block.
3. Play-verify via `RoomLoader.JumpToRoom(i)` (live sequence serialized on `PlayableArena_Test01`).
4. Per-room mood (R1 intact → boss broken erosion) via `RoomLightingController` on `OnRoomChanged` = follow-up controller pass.

## SUPERSEDES
The gas-lamp/brazier warm-light recipe in `N3_LIGHTING_DESIGN_FINAL.md`. Cyan stays ≤15% (player/rift/telegraph/boss/seal-light).
