# ChatGPT'ye verilecek ODA TASARIM prompt'u (kopyala-yapıştır)

> Aşağıdaki bloğun TAMAMINI ChatGPT'ye yapıştır. Çıktı JSON gelir; biz `RoomTemplateSO`'ya çeviririz.

---

You are a level designer for **RIMA**, a 2D **isometric** roguelite action-RPG (Hades + Dead Cells feel), dark-fantasy biome "The Shattered Keep". You design individual ROOM layouts as data.

## World / aesthetic
- Rooms are **isometric floating stone islands** surrounded by **void** (purple-black abyss). Each room = one island of walkable stone floor. The floor edges drop into the void as cliffs — **WE auto-generate the cliffs**, so you only define the walkable floor SHAPE.
- Palette = meaning: slate stone floor · **cyan #00FFCC = rift energy / seals / doors** · void-purple background · warm orange only for reward/brazier.
- Shapes must be **ORGANIC and VARIED** — NOT plain rectangles. Use diamonds, crosses, L-shapes, bridges joining two lobes, hourglasses (pinched middle), donuts (center void hole), teardrops, blobs. **Shape variety is the #1 goal** — every Combat room must look different.

## Grid format (ASCII, origin top-left, row-major)
Each room = a `width` × `height` grid. Symbols:
- `.` = walkable stone floor
- ` ` (space) = void (no floor → becomes cliff/abyss edge)
- `P` = player entry spawn (EXACTLY 1, on floor, near a door)
- `e` = enemy spawn point (on floor)
- `C` = chest / reward spot (Chest rooms only)
- `B` = boss spawn (Boss rooms only, central)
Every row string MUST be exactly `width` characters long. The floor (`.`/`P`/`e`/`C`/`B`) must form ONE connected region (the only exception: donut rooms may have a void hole in the center).

## Door rules (CANON — important)
- Each room has **1–3 exits**. Doors face **NORTH, EAST, or WEST only — NEVER SOUTH** (the camera-facing front edge has no door).
- Each door sits on a walkable floor tile at the room's edge.

## Room types (design the layout to fit the purpose)
- **Spawn**: small (~10×8), safe, 0 enemies, 1 door. Run entry.
- **Combat**: medium (12×10 … 18×14), 3–6 enemies spread out, 1–3 doors. **Make 7 DISTINCT shapes.**
- **CombatLarge**: big arena (20×16+), 6–10 enemies, open center, 1–2 doors.
- **Elite**: medium-large, distinctive shape, 1–3 strong enemies, 1–2 doors.
- **Boss**: large symmetric arena (20×18+), exactly 1 central `B`, 1 entry door.
- **Chest**: small-medium, 0 enemies, 1–2 `C` spots, 1–2 doors.
- **Corridor**: narrow connector (e.g. 6×14), 0–2 enemies, exactly 2 doors (e.g. N + W).

## Output format (return ONLY a JSON array)
```json
[
  {
    "roomId": "combat_cross_01",
    "roomType": "Combat",
    "width": 16,
    "height": 12,
    "grid": [
      "      ....      ",
      "      .ee.      ",
      "  ....    ....  ",
      "  .e.      .e.  ",
      "....        ....",
      ".P..   ..   ..e.",
      "....   ..   ....",
      "  .e.      .e.  ",
      "  ....    ....  ",
      "      .ee.      ",
      "      ....      ",
      "      ....      "
    ],
    "doors": [{"dir":"N","x":8,"y":0},{"dir":"W","x":0,"y":5},{"dir":"E","x":15,"y":5}],
    "notes": "cross / plus shape, four arms"
  }
]
```

## Request
Design **15 rooms total**: 7 Combat (all different shapes) · 2 CombatLarge · 2 Elite · 1 Boss · 2 Chest · 1 Corridor.
Rules recap: every row == width chars; floor one connected region (donut exception); doors only N/E/W; exactly one `P` per room (skip `P` on Boss if you prefer it as a pure arena, but then mark the entry door). Maximize shape variety. Return only the JSON array.

---

### Biz ne yapacağız (ChatGPT çıktısı gelince)
JSON → `RoomTemplateSO` dönüştürücü: `grid` → `walkableGrid` bool[] (`.`/`P`/`e`/`C`/`B` = true), `doors` → `doorSockets`, `P` → `playerSpawn`, `e` → `enemySpawnSockets`, `B`/`C` → ilgili soket. Sonra IsoRoomBuilder ile iso oda + auto-cliff render → MapList havuzuna kat (mapsPerRun ~12).
