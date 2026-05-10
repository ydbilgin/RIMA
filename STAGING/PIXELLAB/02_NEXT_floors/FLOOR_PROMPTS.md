# Floor Tile Production Prompts — F1 / F2 / F3 + Transitions
*Companion to HOWTO.md. Copy-paste ready. Do not edit palette hex values.*

---

## Settings Reference (do not modify per call)

| Param | Value |
|---|---|
| Tool | Create Tiles Pro (Map section, Pro tab) |
| Shape | Isometric |
| View | High top-down |
| Background | #00FF00 (chromakey green) |
| Pixel Art | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | Previous approved tile (mandatory from F2 onward) |

Transitions only: use **Create Tileset Standard** (Wang mode). Not Create Tiles Pro.

---

## Shared Palette (all acts, all floor types — strictly these 5 colors)

```
Shadow / outline:  #1A1C20
Dark stone:        #2A2D34
Mid stone:         #3A3D48
Lit face:          #4E5260
Highlight:         #606575
```

---

## F1 — Cold Grey Granite
**16 variants | 64x64px | Act 1 entry rooms**

Produce 4 candidates first, QC, then expand to 16.
Each variant must have exactly one accent from the list in the prompt.

```
Isometric pixel art dungeon floor tile, cold grey granite stone slab.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fine mortar line (#1A1C20, 1-2px) at slab joints.
Accent (choose one per variant): hairline crack / faint moss (#263530) at grout / stone chip / water stain ring.
Hard pixel edges, NO anti-aliasing. Pixel cluster min 4px. Background #00FF00 chromakey.
```

Accent rotation for 16 variants (4 each):
- var01-04: hairline crack (different angle each)
- var05-08: faint moss (#263530) at grout joint corner
- var09-12: stone chip near edge
- var13-16: water stain ring (faint #2A2D34 ring, 6-8px diameter)

---

## F2 — Weathered Dark Stone
**16 variants | 64x64px | Act 1 mid rooms**

Load best F1 approved tile as style reference before generating.
Overall darker vs F1: reduce #4E5260 usage, lean on #2A2D34 and #3A3D48.

```
Isometric pixel art dungeon floor tile, weathered dark stone slab, more worn than F1.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines (#1A1C20, 2-3px).
Accent (choose one per variant): wider crack / lichen growth (#263530) spreading from joint / broken corner chip / old water damage pattern.
Darker overall vs F1 -- less lit face (#4E5260) usage. Hard pixel edges, NO anti-aliasing. Background #00FF00.
```

Accent rotation for 16 variants (4 each):
- var01-04: wider crack crossing tile diagonally
- var05-08: lichen (#263530) spreading 4-6px from mortar joint
- var09-12: broken corner chip (missing pixel cluster at one corner)
- var13-16: old water damage pattern (darker ring + slight discoloration band)

---

## F3 — Volcanic Black Stone
**16 variants | 64x64px | Act 1 boss / deep rooms**

Load best F2 approved tile as style reference. Darkest tier: danger signal.
Mortar lines nearly invisible (fused stone). Lava-crack vein is the accent.

```
Isometric pixel art dungeon floor tile, volcanic black stone slab with lava-crack veins.
Flat top-down diamond rhombus view.
Palette strictly: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Lava-crack accent: thin vein (#4A1A1A / #6A2A1A) crossing tile, max 8px total length. NO bright orange/red glow pixels (engine VFX overlay handles glow).
Mortar lines nearly invisible -- stone is fused/continuous.
Hard pixel edges, NO anti-aliasing. Background #00FF00.
```

Lava vein rotation for 16 variants:
- var01-04: single straight vein, different angles
- var05-08: Y-fork vein (short branches, max 8px total)
- var09-12: hairline vein near edge only
- var13-16: two parallel micro-veins (2px gap between)

---

## Transition Prompts — Create Tileset Standard (Wang mode)

Use **Create Tileset Standard** (NOT Create Tiles Pro) for all transitions.
Input: lower terrain prompt + upper terrain prompt. Tile size: 64x64. Background: #00FF00.

---

### Trans_F1F2 — Cold Granite to Weathered Stone

**Lower terrain prompt (F1 side):**
```
Isometric pixel art dungeon floor tile, cold grey granite stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fine mortar lines 1-2px. Background #00FF00.
```

**Upper terrain prompt (F2 side):**
```
Isometric pixel art dungeon floor tile, weathered dark stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines 2-3px. Darker overall. Background #00FF00.
```

**Transition description (paste into transition field):**
```
Isometric pixel art dungeon floor tile transition from cold grey granite (F1) to weathered dark stone (F2).
Wang-style edge blend at tile boundary. F1 side: #3A3D48 / #4E5260. F2 side: #2A2D34 / #3A3D48.
Mortar lines shift from fine (F1) to wide/cracked (F2) at blend zone.
Hard pixel edges. Background #00FF00.
```

---

### Trans_F2F3 — Weathered Stone to Volcanic

**Lower terrain prompt (F2 side):**
```
Isometric pixel art dungeon floor tile, weathered dark stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Wider mortar fracture lines 2-3px. Background #00FF00.
```

**Upper terrain prompt (F3 side):**
```
Isometric pixel art dungeon floor tile, volcanic black stone slab.
Palette: shadow #1A1C20, dark stone #2A2D34, mid stone #3A3D48, lit face #4E5260, highlight #606575.
Fused stone surface, faint lava-crack vein (#4A1A1A). Background #00FF00.
```

**Transition description (paste into transition field):**
```
Isometric pixel art dungeon floor tile transition from weathered dark stone (F2) to volcanic lava-crack stone (F3).
Wang-style edge blend. F2 side: dark stone, mortar cracks. F3 side: black volcanic stone, thin lava vein hint (#4A1A1A) near edge.
NO bright glow. Hard pixel edges. Background #00FF00.
```

---

## QC Checklist (per tile before proceeding to next batch)

- [ ] 2:1 isometric diamond rhombus shape (perfect top-down, not skewed)
- [ ] Background is exact #00FF00 (not near-green, not white)
- [ ] All pixels use only the 5 locked palette colors (+ accent color if specified)
- [ ] Pixel cluster min 4px (no single-pixel scatter)
- [ ] Hard pixel edges, zero anti-aliasing fringe
- [ ] Seamless edge: tile flush to diamond boundary (no gap or overlap)
- [ ] Transitions came from Create Tileset Standard, not Create Tiles Pro
- [ ] F1: one subtle accent (crack / moss / chip / water mark)
- [ ] F2: wider mortar (2-3px) + one worn accent; visibly darker than F1
- [ ] F3: lava vein max 8px, no bright orange/red, stone appears fused
- [ ] Hue drift check: place F1+F2+F3 side by side in Unity before finalizing
