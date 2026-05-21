# Overnight Wall Types Per Room Result

PNG path: `STAGING/concepts/overnight/03_wall_types_per_room.png`

Source copy retained for traceability: `STAGING/concepts/overnight/03_wall_types_per_room.raw.png`

## Shader vs New-Gen Split

| Room | Verdict | Reason |
|---|---|---|
| Entry | Shader-driven OK | Same clean granite base; faint cyan glow/tint can be material parameters. |
| Combat | Shader-driven OK with crack mask | Battle wear and cyan cracks can be overlay/mask if brick geometry stays locked. |
| Elite | New PNG / decal set needed | Reinforcement silhouette and sigil carving change readable structure. |
| Rest | Shader-driven OK | Warm tint, dormant rift, and transit symbol can be controlled overlays. |
| Shop | Shader-driven OK with prop decals | Gold trade mark and lantern hooks are additive decals; base wall can remain shared. |
| Curse | New PNG needed | Black staining, red bleed, and thorn accents change the material identity too much for tint-only shader work. |
| Mystery | Shader-driven OK plus mist overlay | Pale stone and question sigil fit shader/decal treatment; mist should be a separate overlay VFX/sprite. |
| Boss | New PNG needed | Massive sealed wall, runes, and 8-fragment slot pattern require unique authored pixels. |

## Re-Gen List

| Priority | Asset | Need |
|---|---|---|
| P0 | `face_EW` | Re-gen required; current S95 canvas fill/drift issue blocks reliable horizontal wall read. |
| P0 | `face_NS` | Re-gen required; current S95 narrow vertical bar drift blocks tile-mate confidence. |
| P1 | Elite wall overlay/pieces | New authored reinforcement + sigil pixels. Minimum: `face_EW`, `corner_outer`; full set if Elite rooms use all 7 pieces. |
| P1 | Curse wall overlay/pieces | New authored corruption + thorns. Minimum: `face_EW`, `corner_outer`; full set if Curse rooms use all 7 pieces. |
| P1 | Boss wall overlay/pieces | New authored massive sealed/rune form. Minimum: `face_EW`, `corner_outer`; full set if Boss rooms use all 7 pieces. |
| P2 | Combat crack mask | Can be generated as mask/decal instead of full wall PNG. |
| P2 | Shop mark/hooks decal | Decal sheet sufficient. |
| P2 | Mystery mist + question sigil | Overlay/VFX/decal sufficient. |

## Production Cost Estimate

| Scope | Extra generations |
|---|---:|
| Mandatory Pilot A repair: `face_EW`, `face_NS` | 2 |
| Minimum mood-specific structural pieces: Elite/Curse/Boss x (`face_EW`, `corner_outer`) | 6 |
| Optional decal/mask sheet for Combat/Rest/Shop/Mystery | 1-2 |
| Full 7-piece authored sets for Elite/Curse/Boss instead of minimum pieces | 21 |

Recommended production path: 9-10 extra generations first (2 repair + 6 structural mood pieces + 1-2 decal sheets). Escalate to 23-25 only if rooms visibly use all 7 wall pieces in camera.

## Tile-Mate Edge Verdict

| Room | Verdict | Notes |
|---|---|---|
| Entry | continues | Shared granite brick pattern reads reusable. |
| Combat | continues | OK if cracks are masks that do not move outer brick seams. |
| Elite | breaks unless authored on shared template | Reinforcement and sigils must preserve edge pixels explicitly. |
| Rest | continues | Tint/symbol overlay does not threaten mating. |
| Shop | continues | Decals/hooks should avoid seam pixels. |
| Curse | breaks unless authored on shared template | Thorn and red bleed accents can destroy edge continuity. |
| Mystery | continues | Mist overlay should be non-tile collision/visual layer. |
| Boss | intentionally breaks | Boss walls can be unique; do not force universal tile-mate unless used in normal room walls. |

Final call: Universal Shader is enough for Entry, Rest, Shop, and most Mystery/Combat treatment. Elite, Curse, and Boss need new authored PNGs or strict template-based overlay generation. The base brick edge map should remain locked across all shader variants.
