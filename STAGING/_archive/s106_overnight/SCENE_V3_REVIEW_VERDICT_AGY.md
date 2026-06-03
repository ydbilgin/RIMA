# Scene v3 vs M3 — Antigravity Visual Review (Cycle 2 Verdict)

## 1. Re-Score Card

| Element | Prior | Current | Δ |
|---|---|---|---|
| 1. Arena shape | ★★★½☆ | ★★★★☆ | +½ |
| 2. Central rune circle focal | ★★☆☆☆ | ★★☆☆☆ | 0 |
| 3. Brazier corners | ★★☆☆☆ | ★★☆☆☆ | 0 |
| 4. Cliff face perimeter | ★★½☆☆ | ★★★☆☆ | +½ |
| 5. Lightning/storm BG | ★★☆☆☆ | ★★☆☆☆ | 0 |
| 6. Nebula/void atmosphere | ★★½☆☆ | ★★½☆☆ | 0 |
| 7. Columns/statues | ★★☆☆☆ | ★★½☆☆ | +½ |
| 8. Tonal contrast | ★★½☆☆ | ★½☆☆☆ | -1 |
| 9. Cyan glow intensity | ★★½☆☆ | ★★☆☆☆ | -½ |
| 10. "Looks alive" | ★★☆☆☆ | ★★☆☆☆ | 0 |

## 2. Overall Delta from Cycle 1 to Cycle 2
- **Structure & Layout (Improved):** Arena expansion to 59 cells (oval shape) and outward cliff offsets create a much better play space boundary. Corner pillars provide the correct structural framing.
- **Lighting & Contrast (Regressed):** Lowering global light to 0.15 without functioning local lights leaves the scene pitch-black and unplayable.
- **Visual Feedback (Broken):** Braziers, central portal decal, and lightning particles are placed in code/editor but completely unlit or invisible in the game view.

## 3. Final Decision + Top 3 Polish Items
The core architecture (3-Kit structure, wall-less style) is correct, but technical configuration issues render the scene pitch-black. We recommend **POLISH-CYCLE-3** to resolve these rendering bugs and bring the visual state to 100%.

### Top 3 Polish Items:
1. **Fix Light2D Target Sorting Layers & Flame Sprites**
   - *Impact:* High
   - *Target:* Map all local Light2D components (braziers, central portal) to include `Floor`, `Cliffs`, and `Props` sorting layers. Add animated flame sprites to the 4 braziers.
2. **Expose Background Nebula & Lightning Particles**
   - *Impact:* High
   - *Target:* Increase tint brightness of `L1_Nebula` (brighter purple/magenta). Ensure `LightningStreaks` particle system has a higher sorting order than the void background and uses an HDR-enabled emissive shader.
3. **Scale and Illuminate Framing Columns**
   - *Impact:* Medium
   - *Target:* Scale up the 4 broken granite pillars by 1.5x. Position them closer to the screen edges and add a faint warm spotlight or rim light to catch their edges.

VERDICT: POLISH-CYCLE-3
