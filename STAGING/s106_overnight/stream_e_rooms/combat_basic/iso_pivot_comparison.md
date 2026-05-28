# Stream J Iso Pivot Comparison - Combat Basic

## Before: scene_v3.png

The Stream G proof was still structurally orthogonal. The floor read as a rectangular, square-tile grid with straight horizontal and vertical rows, so the camera showed a top-down slab instead of a 2:1 diamond field. The wall pieces were anchored to rectangular perimeter coordinates; rear and side pieces sat on top of the old grid rather than following projected iso cell positions. Connector cadence was sparse, and several junctions still exposed large placeholder color blocks. Lighting existed in the scene, but the wall and floor renderers were not consistently assigned to the URP 2D lit sprite material path, so the warm/cyan light language of the reference did not read as integrated.

## After: scene_v5.png

The floor is now a Unity isometric Grid with cell size `(1, 0.5, 1)`, painted with 168 cells from the 16 imported `b340684f` PixelLab tile assets. The floor diamond now matches the intended 2:1 iso read much better than the previous rectangular tile grid. Wall placement now uses the iso projection `(x - y) * 0.5, (x + y) * 0.25`, so rear, side, and front chains follow the diamond floor coordinates instead of the orthogonal coordinate plane. Connector defaults were tightened to 2-3 cells, and extra connectors were added for short corner runs and door-adjacent seams. The scene now has a cool global Light2D, 2 warm torch Point Light2D objects, and 2 cyan ritual/portal Point Light2D objects. All 43 wall SpriteRenderers and the floor TilemapRenderer use `Universal Render Pipeline/2D/Sprite-Lit-Default`.

## Measured Changes

- Floor iso: yes. The room reads as a diamond field instead of a square rectangle.
- Floor assets: yes. All 16 `b340684f` tile assets exist and the combat floor uses deterministic random variants.
- Wall iso placement: yes. Wall anchors are iso-projected and saved in the scene.
- Pillar seams: partial. Connector count is 17, cadence is denser, and door/corner connector coverage is better. Some visual seams remain because placeholder wall/front pieces are still blocky.
- Lighting visible: partial. URP 2D lights and lit materials are active, but the current placeholder wall art and very dark tile values limit the perceived glow.
- chatgpt_ref likeness: 5.6/10. This is materially better than Stream G's 4.5/10 because the floor projection and palette now point in the right direction, but it does not reach 7/10. The main blockers are inherited placeholder wall/front sprites, non-reference wall silhouettes, lack of dense decorative rubble/props, and no proper portal/torch art to carry the lighting.

## Remaining Gaps

- Replace placeholder low-front/open-gap/connector visuals with real iso-matched pieces.
- Add real torch, portal, ritual, and rubble props instead of relying on light objects only.
- Tune wall sprite offsets per direction after real iso wall art lands.
- Add floor decals or ritual-circle overlay if Combat Basic is expected to match chatgpt_ref (2) specifically.
