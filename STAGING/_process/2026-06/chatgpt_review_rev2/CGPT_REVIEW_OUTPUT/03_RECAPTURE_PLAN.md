# 03 — Recapture Plan

## Critical recaptures

Capture these after the full-flow gate passes.

### A. Combat truth

1. **Live combat wide**
   - No pause/death/draft overlay.
   - At least 3 enemies actively moving.
   - Player HP visible.
   - Timer greater than 00:00.
   - Kill counter or room progress visible.

2. **Enemy attack**
   - One readable projectile or telegraph.
   - Damage has not already occurred.
   - Player and attacker both visible.

3. **Player LMB impact**
   - Slash/hit arc visible.
   - Enemy clearly in hit range.
   - Prefer damage number, hit flash, or CombatJuice evidence.

4. **Mid HP**
   - Actual HP bar between approximately 40–70%.
   - No other modal.

5. **Low HP**
   - HP under approximately 25%.
   - Vignette visibly stronger than mid-HP frame.
   - No reward/death modal.

### B. Progression/UI

6. **Character sheet**
   - Real TAB sheet open.
   - Stats/build visible.

7. **Run-map**
   - Actual branching nodes.
   - Current node and possible paths readable.
   - Use OBS/Game View if ScreenSpaceOverlay is not captured by the existing screenshot path.

8. **Reward selection**
   - Existing frame 04 is usable.
   - Add one post-selection frame showing the selected skill applied.

### C. Tooling thesis

9. **Director spawn action**
   - Spawn tab open.
   - Spawned enemy visible in the world.
   - Prefer selection/inspector with ID/HP/AI.

10. **Director stat mutation**
   - Existing frame 19 is usable.
   - Add gameplay result if practical.

11. **Telemetry**
   - Perform LMB/RMB/skill actions first.
   - Capture non-zero totals/percentages.

12. **Free-cam before/after pair**
   - Same scene, clearly different camera position.
   - The current frame 23 is only a Spawn-tab screenshot.

13. **Edit-to-Play pair**
   - Frame A: place one highly visible prop in F2.
   - Exit F2.
   - Frame B: same camera/room, same prop visible during play.
   - This pair is the strongest proof of the thesis.

14. **Tile paint/erase**
   - Use a large 3×3 contrasting patch.
   - Capture before, painted, then erased/hole states.
   - Current 26–28 do not show the world delta.

### D. Room types and boss

15. **Merchant**
   - Merchant/stands/prices clearly visible.
   - No empty-room substitute.

16. **Elite**
   - Elite enemy plus affix/badge/special treatment visible.

17. **Boss full HP**
   - Boss inside the walkable arena.
   - Boss HP bar and name visible.
   - Player also visible.

18. **Boss phase transition**
   - HP below threshold.
   - Transition VFX/text/color state visible.

19. **Boss telegraph**
   - ChainExplosion, Wrath safe zone, or Charge line visible before damage.

20. **Boss low phase**
   - Low boss HP bar visible.
   - No draft overlay.
   - Phase-specific visual state visible.

## Acceptance rules for every new shot

- Filename must match the visible state.
- No unrelated modal overlay.
- Do not count exact SHA uniqueness as state verification.
- Compare new screenshot visually with the previous one.
- For changed states, require a visible world/UI delta.
- Combat shots require timer/progress greater than zero.
- Boss shots require the boss to be inside the playable arena.
- Capture a short backup video of the complete demo route.
