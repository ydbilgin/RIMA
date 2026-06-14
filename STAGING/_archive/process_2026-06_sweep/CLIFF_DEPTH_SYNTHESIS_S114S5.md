# Cliff + Depth Backdrop — Synthesis & Production Spec (S114 S5, 2026-05-29)

Triple-AI (Opus + agy + Codex) synthesis of the floating-island cliff edge + abyss depth backdrop.
Status: STRUCTURE done in PlayableArena_Test01; final ART = PixelLab next task.

## A. CLIFF EDGE — what was decided & done
- **Render BELOW floor** (Ground sorting, below Floor) → floor occludes cliff top, only the void-hang shows. No PPU shrink → depth kept. ✅ done
- **Single coherent variant** (cliff_S) for now; mixing thick+sivri styles = "2 types" complaint. Variant FAMILY (3 heights × 3 textures, SAME art language, TOP CONTACT LINE STABLE) = future. ✅ single done
- **Perimeter-only + cut overflow:** place only on S/SE/SW-empty front edges; CUT cells with floor within `southClearCells` (=5) screen-south steps (else hang over lower floor = "standing column" on notches/peninsulas). Cut > mask (mask breaks batching + looks pasted). South-only probe — diagonal probe OVER-CUTS on a diamond (killed 57/59). ✅ done, 55 cells
  - **Robust future rule (both AIs):** exterior-void flood-fill from map bounds, place only if drop opens to exterior void + south sight-cone clear. Makes it map-shape-agnostic. → TODO (spec'd, not yet impl)
- **Organic variation:** per-cell clustered height (Perlin + per-cell hash, `randomness`), top stable, x-jitter, no sprite stretch. ✅ done (DirectionalCliffTile: maxLift/randomness/clusterFrequency/gridJitter)
- **AO contact-shadow:** EdgeFX_Auto tilemap (Decals, above floor), soft dark gradient at floor front edge. ✅ done. Cyan rim per-cell = REMOVED (made sci-fi zigzag); a continuous island-boundary cyan rim or Light2D rim = future.
- **Floor blocks:** VoidBlocker perimeter ring (Grid collider), GAPS=0, floor fully walkable. ✅ done
- **Gaps for depth:** carve floor + keep void collision → inner cliff hangs in, void shows = depth. ✅ demo'd (gap N of spawn)

## B. DEPTH BACKDROP ("layer 3" abyss) — synthesis
Goal: below/behind the floating island, an animated abyss so depth reads. Floor=L1, cliff=L2, this=below.

### Layers (4-5, both AIs converge)
| Layer | Content | Parallax factor (X / Y=½X) | Sorting |
|---|---|---|---|
| Void gradient | dark base | 0.03 / 0.02 | back-most |
| Nebula / cyan rift | cosmic glow | 0.05 / 0.04 | |
| Distant ruins | floating debris | 0.08 / 0.05 | |
| Far islands (discrete, SPREAD not stacked) | small islands | 0.14 / 0.08 | |
| Fog / contact haze | covers cliff base | 0.10 / 0.06 | nearest, sorts just under cliff |

- **Y = ½ X** vertical parallax (Sang-Hendrix rule) — avoids top-down dizziness.
- **Light2D OFF the BG** (unlit material `Sprites-Default`) so abyss self-glows, not "lit cardboard". ✅ already unlit.
- **Sorting:** all BG below cliff (Ground negative orders / dedicated Backdrop layer). Floor > cliff > fog/contact > ruins > islands > nebula > void.
- **Island↔abyss blend:** cliff drop-shadow (CliffDropShadowPlacer) + contact fog at cliff base + bottom 30% of cliff fades to dark/cyan + tight cyan edge falloff. This fog is what stops the cliff looking "pasted over empty space".

### Animation (pixel-safe, both AIs)
- Transform drift (slow sine sway) for clouds/islands.
- Low-FPS sprite-sheet fog (NOT smooth shader scroll unless pixel-snapped).
- Low-count drifting dust/mote particles (point filter).

### USER PRODUCTION CONSTRAINTS (PixelLab — next task)
- **SEAMLESS / TILEABLE** sprites — ParallaxLayer.cs has NO wrap; for "continues as you walk" the BG must either cover the whole bounded room OR be tileable (drawMode=Tiled). Bounded arena → large seamless sprite covers it.
- **Sizes:** PixelLab 768×340 (proportional) OR Create-Image-Pro options: 688×384 (16:9), 512×288 (16:9), 512×512, 632×424 (3:2), 424×632 (2:3). Decide per layer aspect.
- **Real size, not squished, not all stacked** — full-screen layers (void/nebula/fog) centered+large; discrete elements (islands/ruins) spread out.
- **Per-map backdrop:** background varies by map/RoomType — config per room (future system: RoomBackgroundRig preset per RoomType SO).

### Current state (existing assets, structural preview)
RoomBackgroundRig enabled: L0_Void/L1_Nebula/L2_Ruins/L4_Fog at real scale (39×39 / 52×29), L3_Island_Small discrete, L3_Island_Large OFF (boss-only). Parallax factors + unlit OK. Depth shows in void/notch areas (subtle — island is large). Proper seamless PixelLab art = next task.

## C. What still needs the USER's art eye
- Final cliff "natural" judgment, AO strength, hang-variation amount, demo-gap placement.
- cliff_S.png pixel cleanup (user doing).
- Depth backdrop final look once PixelLab seamless assets land.
