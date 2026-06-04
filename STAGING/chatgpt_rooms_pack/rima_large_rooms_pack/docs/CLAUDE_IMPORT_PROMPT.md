You are helping build RIMA, a 2D isometric roguelite action-RPG with a Hades + Dead Cells feel.

Task:
Import and use the attached JSON room pack as the source of truth for The Shattered Keep biome.

Important:
- These are LARGE room layouts.
- The ASCII grid defines ONLY walkable floor.
- Void/cliffs must be generated automatically from the floor boundary.
- Do NOT convert these shapes into plain rectangles.
- Preserve shape identity: diamond, cross, L-shape, bridge lobes, hourglass, donut, teardrop, blob, twin basin, trident, crescent, vaults, zigzag corridor.
- Door directions are canon: NORTH, EAST, WEST only. Never create SOUTH doors.
- Doors are not drawn as ASCII symbols. They are metadata in `doors`.
- `P` is the player entry spawn and must remain exactly one per room.
- Use `e`, `C`, and `B` as spawn markers.
- All rows are already validated to match width.
- Walkable floor is connected. Donut rooms intentionally contain a center void hole.

Recommended implementation:
1. Parse `rooms/rima_shattered_keep_rooms_large.json`.
2. For each grid cell:
   - `.` / `P` / `e` / `C` / `B` => place floor tile.
   - space => void.
3. Generate cliffs around floor boundary cells.
4. Place door/seal visuals at door coordinates using cyan #00FFCC.
5. Replace spawn markers with gameplay entities after floor generation.
6. Keep the original ASCII shape data separate from decorative population.

Please build the importer/tooling around this exact schema:

{
  "roomId": "string",
  "roomType": "Spawn | Combat | CombatLarge | Elite | Boss | Chest | Corridor",
  "width": "number",
  "height": "number",
  "grid": ["array of row strings"],
  "doors": [{"dir":"N|E|W","x":0,"y":0}],
  "notes": "string"
}

Quality checklist:
- Never trim row spaces.
- Never use `.strip()` on grid rows.
- Preserve every row string exactly.
- Door coordinates must point to an edge floor cell.
- South doors are invalid.
- Any importer should treat spaces as real cells, not missing data.
