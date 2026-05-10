# Transition Floors (F1→F2, F2→F3)

**Boyut:** 64x64px → **8 varyasyon her biri** (sheet: 2 cols × 4 rows)
**Tool:** Create Tiles Pro (Isometric, High top-down)
**Style Reference:** İlgili iki floor'un approved tile'ı yükle

---

## Trans F1→F2

```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition stone: left half of tile is clean F1 grey granite (#2E3038, #424555), right half transitions into cracked F2 style (#2A2C35, #3C3F4E, #263530 lichen). The split is diagonal not vertical, creating a natural rock fault line. Flat baked lighting only. Hard pixel edges. 8 variations, each with slightly different fault line angle and lichen spread.
```

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/Trans_F1F2 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f1f2_
```

---

## Trans F2→F3

```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition: F2 cracked stone (#3C3F4E) bleeding into F3 volcanic basalt (#222230, #4A1A1A energy). Diagonal fault line with glowing crack appears at transition. 8 variations, varied crack thickness and glow intensity.
```

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/Trans_F2F3 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f2f3_
```
