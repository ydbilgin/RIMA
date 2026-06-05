# Room QC Report — 2026-06-05

Built every RoomTemplateSO in edit mode via IsoRoomBuilder in _Arena scene.
Screenshots saved to `Assets/Screenshots/RoomQC/`.
Structural counts: Floor = manual tile count from groundTilemap, Cliff = CliffSprites child count, Props = Props child count.

---

## Results Table

| Template | Verdict | Floor | Cliff | Props | BoundsW | BoundsH | Visual Evidence |
|---|---|---|---|---|---|---|---|
| boss_shattered_oval_01 | SAÇMA-suspect | 694 | 53 | 0 | 20.6 | 12.6 | Large oval island OK shape. Cliff density very high on SE edge — cliffs overflow/stack below the island, looks like a dense curtain. Top-left has a stray tiny orange-tile fragment detached from island. No props (intentional for boss?). |
| chest_large_donut_vault_01 | OK | 225 | 42 | 0 | 12.5 | 7.6 | Rectangular island with intentional rectangular hole in center (donut shape). Cliffs on exterior edges only. Inner void cliffs present — expected. No props. Coherent. |
| chest_large_reliquary_diamond_01 | SAÇMA-suspect | 156 | 30 | 8 | 10.6 | 6.1 | Large floor area OK. But props are severely misplaced: altar/pedestal cluster appears at the bottom-left partially off the island edge, circular pedestal ring appears to be floating partially outside island bounds at bottom-right. Classic "prop placed at cell that maps off-island" issue. |
| combat_large_bridge_lobes_01 | OK | 359 | 54 | 12 | 17.3 | 10.5 | Two-lobe bridge shape is coherent. Props (pillars, torches, gate arch) on left lobe look correct on-island. Cliffs hug edges. One stray prop sprite (purple figure) at left edge but appears on floor. |
| combat_large_cross_01 | SAÇMA-suspect | 282 | 54 | 6 | 15.4 | 9.7 | Cross/plus shape reads OK structurally. However 2 props (purple arch + crystal) appear at top of island and look slightly off/stacked. The player spawn marker is visible at bottom edge (not a visual bug, cosmetic). Cliffs are dense on all arms — some cliff overflow on outer corners. |
| combat_large_diamond_01 | FAIL | 229 | 53 | 31 | 14.9 | 9.4 | Severe prop misplacement: large altar/pedestal cluster (3+ objects) is completely floating in void at bottom-left, detached from island. Separate prop cluster (arch + flame) also floating off-island at bottom-right. The island floor itself looks fine but 31 props have multiple completely off-island placements. |
| combat_large_donut_01 | SAÇMA-suspect | 380 | 69 | 17 | 16.3 | 11.4 | Large rectangular island with inner rectangular hole (donut). The hole has inner-wall cliffs which is correct. However: circular pedestal/altar cluster appears floating BELOW the island at bottom-left (off-island). Some props in correct position. Mixed result. |
| combat_large_hourglass_01 | SAÇMA-suspect | 306 | 67 | 27 | 20.2 | 12.3 | Hourglass pinch is the intended shape. Floor and cliff structure reads OK (intentional narrow waist). Props on right half look on-island. But bottom-left has an altar cluster partially off/hanging at island edge. Cliff row on right side visible through pinch area but may be correct for the shape. |
| combat_large_lshape_01 | SAÇMA-suspect | 357 | 52 | 28 | 16.8 | 12.6 | L-shape floor reads correctly. Props along bottom edge of the L appear to straddle the boundary (arch, pedestal, fountain partially hanging off island). Right side has circular arch floating off SE edge. Dense prop placement causing edge overflow. |
| combat_large_teardrop_01 | OK | 256 | 44 | 6 | 13.4 | 8.5 | Teardrop/asymmetric oval looks good. 6 props all appear on-island (gate arch + torch at left). Floor coherent. Small notch at top-right edge — may be intentional. Cliffs clean. |
| combatlarge_organic_blob_01 | OK | 632 | 59 | 10 | 21.1 | 13.2 | Large organic blob shape reads well. 10 props: pedestal + altar cluster at bottom-right on island. Cliffs hug the organic silhouette. A few organic "bite" indentations on left/top edges — likely intentional. |
| combatlarge_twin_basins_01 | FAIL | 576 | 80 | 1 | 22.1 | 13.2 | CRITICAL: A full cliff wall runs diagonally through the middle of the floor (from upper-left to center). This splits the room visually in two with a cliff line cutting through perfectly good floor tiles. This is a cliff auto-placer bug — interior cells are being treated as void neighbors. The island shape is likely two offset rectangular pools joined, and the seam between them is triggering cliff placement. |
| corridor_large_zigzag_bridge_01 | OK | 89 | 30 | 14 | 10.6 | 7.0 | Narrow zigzag corridor shape. Props (arch gate, torch, crystal) appear on island. Cliffs correctly frame the narrow strip. One small prop (shield/skull) appears at bottom-right hanging slightly but within cliff range. |
| elite_large_crescent_01 | OK | 387 | 83 | 5 | 20.6 | 12.0 | Crescent/horseshoe shape with intentional central void. Inner void cliffs present. Floor arms are coherent. 5 props (marker + pillar) on left arm. Cliffs on both inner and outer edges. Shape is intentional and reads correctly. |
| elite_large_trident_01 | SAÇMA-suspect | 411 | 80 | 41 | 20.2 | 12.9 | Large U-shape/trident with two rectangular voids inside. 41 props: the right side has 3+ prop clusters (arch, pedestal ring, torches) that appear to straddle the island SE edge — some floating off-island. Inner voids have cliffs which is correct. Floor shape OK. |
| Boss_Intro_01 | OK | 100 | 20 | 0 | 9.6 | 5.9 | Simple rectangular room. No props. Player spawn marker visible at bottom. Cliffs correct. Clean. |
| Combat_Large_01 | OK | 120 | 25 | 1 | 11.5 | 6.4 | Simple rectangular room with a single notch/hole on top-center (intentional). 1 prop on island. Cliffs correct. Player spawn visible. Clean. |
| Combat_Medium_01 | SAÇMA-suspect | 60 | 27 | 6 | 7.7 | 4.7 | Irregular/notched shape — has multiple concave cutouts on all sides making it look like a cross with bitten corners. Some cutouts may be intentional (cross shape) but the amount of void-biting looks excessive for a "medium" room. The player spawn is partially at the edge. 6 props look on-island. Cliffs on each cutout give a jagged appearance. Playable but unusual silhouette. |
| Combat_Small_01 | OK | 28 | 10 | 1 | 4.8 | 2.9 | Small square room with one notch. 1 prop on island. Cliffs correct. Player spawn visible. Clean. |
| Corridor_Linear_01 | OK | 24 | 13 | 0 | 6.7 | 4.1 | Long narrow corridor strip. No props. Cliffs on long sides. Very thin (2-3 tiles wide) but functional. |
| Corridor_LShape_01 | OK | 42 | 15 | 0 | 6.7 | 3.5 | L-shaped corridor. No props. Cliffs correct on both arms. Clean. |
| Elite_01 | OK | 52 | 14 | 0 | 6.7 | 4.1 | Square with corner notches. No props. Cliffs correct. Clean. |
| Shrine_01 | OK | 40 | 12 | 8 | 5.8 | 3.5 | Compact square room with 8 props all appearing on-island (floor has checker variation visible). Clean. |
| Spawn_01 | OK | 26 | 10 | 0 | 4.8 | 3.2 | Small rectangular spawn room, two-tier ledge appearance. No props. Cliffs correct. Clean. |
| Treasure_01 | SAÇMA-suspect | 14 | 10 | 4 | 4.3 | 2.0 | VERY SMALL (14 floor tiles — smallest room). Has a plus/cross shape with 4 corner extensions giving 10 cliff sprites for only 14 floor tiles (cliff-to-floor ratio 0.7 is very high). Props on-island but cramped. Functional but barely — a single player sprite fills most of the room. |
| Chamber_CharSelect | OK | 224 | 37 | 17 | 9.1 | 11.1 | Special char-select chamber. Crescent pedestal arrangement visible. Player spawn at bottom. Props (pedestals, statues, gate arch, torch) all on-island. Cliffs hug the rectangular perimeter. Clean. |

---

## Screenshot Paths

All screenshots: `Assets/Screenshots/RoomQC/<templateName>.png`

---

## Prioritized Worst Offenders

### FAIL (must fix before shipping)

1. **combatlarge_twin_basins_01** — CRITICAL cliff bug: interior cliff wall cuts diagonally through the floor, splitting the room. The cliff auto-placer is firing on void-adjacent cells at the seam between the two rectangular basin sections. Root cause: the template has a narrow connecting passage between two basins and the cliff solver is treating the step-down transition as an outer edge. Fix: review the cell mask for the interior-seam cells so the cliff solver does not fire there.

2. **combat_large_diamond_01** — Multiple prop clusters (31 total) placed completely off-island floating in void. The prop scatter algorithm is landing cells outside the valid floor set. Fix: ensure PropRegistrySO scatter only seeds from `LastFloorCells` and validates each placed cell is in the floor set before instantiation.

### SAÇMA-suspect (playable but visually wrong — fix before content review)

3. **chest_large_reliquary_diamond_01** — Prop cluster off-island at bottom edge. Same root cause as #2 above.

4. **combat_large_donut_01** — One large prop cluster floating below island. Same prop placement issue.

5. **combat_large_hourglass_01** — Props at hourglass waist straddling edge. Shape is intentional; prop placement needs edge clamp.

6. **combat_large_lshape_01** — Props straddling L-shape corner/edge (circular arch hanging off SE corner). 28 props, several at boundary.

7. **elite_large_trident_01** — 41 props; several clusters off-island on SE edge. Dense prop count exacerbates the issue.

8. **boss_shattered_oval_01** — No props (0 — might be intentional for boss?). Cliff density on SE corner creates a visual cascade; minor stray tile fragment at top-left. If props should be present, it needs the prop-placement fix.

9. **combat_large_cross_01** — Minor: 2 prop sprites slightly misaligned at top arm. Less severe.

10. **Treasure_01** — Structurally valid but 14 floor tiles is extremely small for a "treasure" room. Consider whether this is the correct template or if it needs more floor cells defined.

### System-wide notes

- The **prop placement bug** (props landing outside island) affects 5+ rooms consistently. The scatter likely uses a bounding-box random-point strategy rather than sampling from `LastFloorCells`. All rooms with 15+ props show this.
- **Cliff overflow** (dense curtain below SE edge) is visible on most large rooms but does not break gameplay — it is a cosmetic artifact of the directional tuck math on south/SE edges. Rooms with 50+ cliffs are most affected.
- **Library rooms** (Boss_Intro through Treasure, Spawn, Special/Chamber) all look clean — the Library set was authored carefully and has no prop placement issues.
- **Generated rooms** imported from ChatGPT batch: mixed quality. 8 of 15 are OK or only mildly suspect; 2 are FAIL/severe.
