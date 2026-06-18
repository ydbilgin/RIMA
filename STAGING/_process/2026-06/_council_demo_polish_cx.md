# RIMA Demo Polish Council - cx Lens

Scope: read-only brainstorm for tomorrow's editor demo. Lens = cx / technical feasibility. Goal is maximum visual lift from engine/code polish and existing assets, with minimum PixelLab generation.

Context checked:
- `Assets/Scripts/VFX/SkillVfx.cs`: tint, additive burst/sweep, cast flash, projectile trail, melee arc, ground crack, chain bolt.
- `Assets/Scripts/Combat/Juice/*`: ScreenShakeDriver, HitPauseDriver, CameraPunchController, DamageNumberDriver.
- `Assets/Scripts/UI/UIManager.cs`: modal scrim stack already exists.
- `Assets/Scripts/UI/RimaUITheme.cs`: node icons, card/slot/bar frames, menu backdrops, rarity glow, low-hp vignette.
- `STAGING/_process/2026-06/demo_screenshots/*`: capture notes highlight overlays, build mode, combat, run map, low HP, director, boss.

## Ideas

1. Modal scrim unification pass for all blocking overlays.
   Short: Settings/Pause/Codex/Draft/Director already have a UIManager scrim stack; make sure every demo-facing blocking panel is registered and sorted through it, so the gameplay world and old buttons stop bleeding behind panels. Add a mild dark-violet tint instead of pure black if needed.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 0.75-1.5 saat
   Jury visibility: cok yuksek; Settings, Pause, Draft, Codex, Director are all screenshot/demo moments.

2. Build Mode centerpiece polish: quieter grid, stronger selected-tool state, placement feedback.
   Short: Build Mode is the live edit-to-play wow moment. Lower purple grid alpha/thickness, add a persistent "BUILD MODE" badge, tint ghost preview cyan/ember based on valid/invalid placement, and pulse selected prop/tile button. Reuse existing Director/Build UI colors and prop ghost.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 1.5-2.0 saat
   Jury visibility: cok yuksek; this is the editor-demo centerpiece.

3. Combat hit-readability juice pass using existing event bus.
   Short: Route the highest-frequency player actions through existing `SkillVfx` calls: CastFlash on skill start, MeleeArc/ImpactBurst on LMB hit, ProjectileTrail on missiles, GroundCrack/ChainBolt for two showcase skills. Keep ProcLimiter and 60 FPS cap. No new sprite required.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 1.5-2.5 saat
   Jury visibility: cok yuksek; combat is watched continuously.

4. Low-HP / Rage red-screen de-stack.
   Short: Current evidence shows red tint can dominate gameplay. Use one edge-only vignette path (`UI/RIMA/lowhp_vignette`) and cap sustained alpha around 0.12-0.18; disable or lower `RageVisualFeedback` full-screen alpha for demo. Keep hit flash short and edge-only.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 0.5-1.0 saat
   Jury visibility: yuksek; fixes combat readability and avoids "game is always damaged" impression.

5. Main menu backdrop reliability + tiny motion.
   Short: Ensure the painted backdrop path always wins over flat gradient (`RimaUITheme.CreateFullScreenBackdrop` / generated sprite cache), then add subtle ember/dust motes or slow parallax on existing `dust_mote` / `particle_ember`. This improves the first 5 seconds with no new art.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 0.75-1.25 saat
   Jury visibility: cok yuksek; first screen and reset path.

6. Run-map iconization from existing node sprites.
   Short: Replace rectangle+text dominance with `RimaUITheme.NodeIcon(RoomType)` sprites, stronger connection lines, and a bright current-node ring (`RIMA_UI_Node_Current`). Keep labels smaller/secondary.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: orta-yuksek
   Effort: 1.0-1.5 saat
   Jury visibility: yuksek; route map is a clear roguelite signal.

7. Skill bar polish: visible icons, key labels, cooldown clarity.
   Short: The skill bar code already supports icon, cooldown overlay, CD timer, key label, slot names, glow, and drag-drop. Audit golden path so draft-selected skills always populate icons and empty slots show intentional locked/empty visuals, not dead boxes.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: orta-yuksek
   Effort: 1.0-1.5 saat
   Jury visibility: yuksek; always visible during gameplay.

8. Damage number readability and color pass.
   Short: `DamageNumberDriver` is pooled and cheap. Add TMP outline/shadow, crit punch scale, source/type color via existing `DamageColors`, and slightly shorter lifetime so combat reads crisp instead of noisy.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: orta
   Effort: 0.75-1.25 saat
   Jury visibility: yuksek; every hit shows it.

9. Boss phase pop with existing toast, light, shake, and VFX.
   Short: On boss HP thresholds, fire `HUDController.ShowToast`, a medium ScreenShakeDriver pulse, HitPauseDriver beat, and a purple/cyan 2D light pulse. This sells phase changes without new boss art.
   Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
   Impact: yuksek
   Effort: 1.0-1.5 saat
   Jury visibility: yuksek if boss is shown; otherwise low.

10. Character select silhouette mitigation with rim/foot-ring reuse.
    Short: If locked/next-class figures appear as black blobs, tint them slate-gray, add cyan/purple rim and `ProceduralFootRing`, or reuse existing Warblade/Elementalist idle sprites as stand-ins. Avoid generating new class sprites before demo.
    Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
    Impact: orta-yuksek
    Effort: 1.0-1.5 saat
    Jury visibility: orta-yuksek; early flow but not the editor centerpiece.

11. Director Mode hierarchy trim for presentation.
    Short: Keep the dev tool impressive but reduce clutter: hide center monologue while Director is open, make active tab/status one clean panel, and rely on the existing bleed guard. Prioritize Spawn, Build, Telemetry tabs for demo script.
    Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
    Impact: orta
    Effort: 1.0-2.0 saat
    Jury visibility: yuksek if presenter uses Director.

12. URP 2D light/bloom preset pass.
    Short: Add or tune a global demo lighting profile: low bloom, slight vignette, portal/crystal 2D point lights, and muted ambient. Keep it conservative to preserve pixel readability.
    Tag: [engine/kod | asset-reuse | PixelLab-gen:0]
    Impact: orta-yuksek
    Effort: 1.5-3.0 saat
    Jury visibility: yuksek across every room, but riskier than UI/VFX tweaks.

## Skip / Defer

- New enemy/class sprite generation before demo: high risk, PixelLab cost, import/setup risk. Use tint/rim/reuse first.
- Heavy post-processing stack: can blur pixel art and fight Pixel Perfect Camera.
- Large Director UI redesign: too much surface area; trim and highlight instead.
- New biome/room art: too broad for tomorrow.

## ILK 5 Oneri

1. Modal scrim unification pass for all blocking overlays.
2. Build Mode centerpiece polish: quieter grid, selected-tool state, placement feedback.
3. Combat hit-readability juice pass using existing `SkillVfx` and event bus.
4. Low-HP / Rage red-screen de-stack.
5. Main menu backdrop reliability + tiny motion.

Total PixelLab-gen ihtiyaci: 0.
