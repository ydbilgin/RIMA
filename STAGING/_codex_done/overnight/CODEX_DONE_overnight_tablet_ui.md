# CODEX DONE - Overnight Tablet UI

## PNG outputs
- STAGING/concepts/overnight/05_tablet_mappanel_act1.png - 1280x800
- STAGING/concepts/overnight/06_tablet_minimap_128.png - 800x600
- STAGING/concepts/overnight/07_tablet_4act_evolution.png - 1280x320

## Alpha analysis
- 05_tablet_mappanel_act1.png: System.Drawing reports Format32bppArgb, alpha min 220, alpha max 255, transparent pixels 0, semi-transparent pixels 4156 / 1024000.
- 06_tablet_minimap_128.png: System.Drawing reports Format32bppArgb, alpha min 220, alpha max 255, transparent pixels 0, semi-transparent pixels 2796 / 480000.
- 07_tablet_4act_evolution.png: System.Drawing reports Format32bppArgb, alpha min 219, alpha max 255, transparent pixels 0, semi-transparent pixels 3196 / 409600.
- Practical import verdict: no fully transparent pixels. Treat these as opaque concept renders; if production import needs strict opacity, force alpha off in the Unity texture importer or flatten during atlas build.

## Tablet metaphor lore-fit verdict
PASS. The Kirik Tas Tablet metaphor reads as old-world, fractured, and run-state aware. Rusty gold framing keeps it ritual/ancient rather than sci-fi, while cyan rift seams clearly support route reveal, player position, branch state, and fragment progression. The 4-act evolution preserves the same map identity while letting each act shift material language: castle stone, flesh/bone, void fragments, and mirror rift.

## Implementation notes
- Recommended Unity path: UGUI Canvas for the playable prototype because the HUD minimap, tab map, counters, node icons, and line connectors are fast to wire with RectTransforms, Image, TextMeshPro, and simple pooled node prefabs.
- UI Toolkit is viable later for editor-like tooling, but UGUI is lower risk for animated rift lines, node hover states, controller focus, and in-combat HUD layering.
- Atlas plan: split into MapTablet_Base, MapNodes_Icons, MapRift_Lines, MapLegend_Icons, and ActEvolution_Showcase. Keep cyan rift glow sprites in a separate additive-friendly atlas/material if animated.
- Runtime composition: use a static tablet background, node prefab variants for revealed/open/hidden, a boss fragment indicator prefab with 8 slots, and route connectors rendered with tiled Image sprites or a lightweight LineRenderer-on-canvas solution.
- Import notes: concept PNGs are full-screen mockups, not final sliced UI. Production should recreate the frame, cracks, nodes, and legend as layered sprites for localization, accessibility scaling, and route-state animation.

## Notes
- ANTIGRAVITY.md was not present at repo root when checked.
- Generated source images remain in the Codex generated_images folder; final project copies are staged at the paths above.
