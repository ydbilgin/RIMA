# CODEX DONE - Overnight Floor Types Per Room

## PNG path
- STAGING/concepts/overnight/04_floor_types_per_room.png
- Validation: 1280x960 PNG, RGB, SHA256 F7A177ED0FF492120743361BB7AEDE7BBB59DBF104B3E76F8E94B8D378B5CD9E

## Shader vs new-gen split

| Room | Baseline use | New PNG need | Verdict |
|---|---|---|---|
| Entry | b340684f granite/cyan bundle with low rift emission | None | Shader-driven enough |
| Combat | b340684f rift variants plus density control | None for base; reuse L5 crack decals | Shader-driven enough |
| Elite | b340684f base tile | Heavy rift vein decals + sigil circle decals | New transparent decals, not new base |
| Rest | b340684f granite warmed by shader/palette | None; healing glow can be VFX sprite/shader | Shader-driven enough |
| Shop | b340684f polished/warm shader pass | Trade carpet decal + gold glow/accent decals | New transparent decals |
| Curse Gate | b340684f can support layout only | Blood/corruption patch + dark rift; optional corrupted base | New PNG recommended |
| Mystery | b340684f mist/cool palette base | Pale rune/event sigil decals | New transparent decals |
| Boss | b340684f insufficient for sealed arena identity | Obsidian base + massive rift + arena line decals | New PNG base/decal pack required |

Short answer: the b340684f 16 PNG bundle is enough for Entry, Combat, and Rest, and can remain the base for Elite, Shop, and Mystery. Curse Gate and Boss should not be shader-only because their semantic read is a warning/finale read, not just a palette shift.

## Production cost

Recommended PixelLab budget for a production floor-room pack:

| Pack | Purpose | Suggested route | Count |
|---|---|---|---:|
| Elite rift veins | Heavy challenge mark | create_object, 1-dir, n_frames=16 | 16 candidates |
| Elite sigil circles | Arena challenge decal | create_object, 1-dir, n_frames=16 | 16 candidates |
| Shop carpet | NPC/trade zone anchor | create_object, 1-dir, n_frames=16 | 16 candidates |
| Shop gold glow/accent | Trade highlight decals | create_object, 1-dir, n_frames=16 | 16 candidates |
| Curse blood/corruption | Risk patch language | create_object or create_map_object | 16 candidates |
| Curse dark rift | Non-cyan corrupted rift | create_object, 1-dir, n_frames=16 | 16 candidates |
| Mystery runes/sigil | Event identity | create_object, 1-dir, n_frames=16 | 16 candidates |
| Boss obsidian base | Distinct final-room base | create_tiles_pro or object pack | 16 candidates |
| Boss massive rift/arena lines | Final arena floor decal | create_object, 1-dir, n_frames=16 | 16 candidates |

Recommended cost: 9 PixelLab jobs / about 144 candidate frames. Minimal cost: 7 jobs / about 112 candidates if Boss and Curse base recolors are shader-driven first. Conservative cost: 10 jobs / about 160 candidates if Curse Gate also receives a unique base tile pack.

## 6-layer pipeline mapping per variant

| Room | L1 base | L2a patch / variation | L4 decal | L5 VFX / accent |
|---|---|---|---|---|
| Entry | Clean b340684f granite | Very low-worn granite variation | Tiny dormant rift seam, sparse | Low cyan pulse, almost off |
| Combat | Standard b340684f granite/rift | Scattered cracked floor variants | Thin cyan crack decals near edges, avoid encounter center | Moderate cyan shimmer on cracks |
| Elite | Darkened b340684f base | Purple/cyan stress patches | Heavy vein overlays + sigil circles | Brighter cyan rim pulse and challenge marker glow |
| Rest | Warm shader-tinted granite | Soft warm organic patch | Healing glow patch, smooth oval, no hazard read | Green/gold low-frequency pulse |
| Shop | Polished/warm granite | Clean center with lower dirt noise | Trade carpet, gold trim, small coin/symbol decals | Static gold glow, low particle count |
| Curse Gate | Dark corrupted granite or unique base | Blood/corruption organic patch | Blood stains + dark rift fracture decals | Red/dark pulse, corrupted edge wisps |
| Mystery | Cool mist-tinted granite | Desaturated pale stone variation | Pale runes + event sigil, partial opacity | Mist layer and intermittent rune flicker |
| Boss | Unique sealed obsidian base | Large radial arena wear/pressure patch | Massive central rift + arena line circles | Strong cyan rift core, controlled so combat telegraphs stay readable |

L3 wall sprites are intentionally not part of the floor sheet. They remain perimeter cap sprites in Karar #143 and should be layered above these floor compositions at room assembly time.

## Notes
- Output is a deterministic concept sheet generated locally with Pillow so the requested file path and 1280x960 dimensions are exact.
- No Unity assets, scenes, prefabs, or importer settings were modified.
