# Visual / Settings / Class / Lighting Feedback Pointer - 2026-05-03

## Why This Exists

This is the durable pointer for the latest user feedback after the Core 4 skill pass,
input/settings follow-up, and first large dungeon/map pass.

Load after:
- `CURRENT_STATUS.md`
- `STAGING/CORE4_SKILL_IMPLEMENTATION_POINTER_2026-05-03.md`
- `STAGING/INPUT_SETTINGS_LIGHTING_FOLLOWUP_2026-05-03.md`
- `STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ENV_MAP.md`

Reference art direction:
- `F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`

## Latest User Feedback

- Context must be preserved through repo pointer files, not assumed memory.
- ESC/test class switching was changing only skills/default slots, not the player class visuals.
- The current settings menu is not liked. Treat it as a temporary debug/settings overlay,
  not the production settings design.
- Current dungeon lighting is not liked. It reads too uniform; every area feels similar.
- The dungeon should move toward the mood/read of `rima_style_anchor`.
- Current floor and wall sprites are known to be unsuitable for final style. They are temporary
  system-test placeholders and should be regenerated around the style target.

## Code State From This Feedback

- `PlayerClassManager.SetPrimaryClass()` should be the central primary class switch path.
- Primary class switching must update both:
  - gameplay: class state, enabled skill controller, resource/default slots
  - presentation: player Animator controller / class appearance
- Settings class buttons should not own separate visual swap logic. They should call the same
  central class switch path as character select.

Implemented in this pass:
- `Assets/Scripts/Systems/PlayerClassManager.cs`
  - `ApplyPrimaryClassToPlayer()` now also applies the class Animator controller from
    `Resources/Characters/<Class>/<Class>`.
- `Assets/Scripts/UI/CharacterSelectScreen.cs`
  - Removed duplicated Animator swap so class select uses the central manager path.
- `Assets/Tests/EditMode/CharacterSelectTests.cs`
  - Added regression test that `SetPrimaryClass(Ranger)` changes the player's Animator
    controller, not only skill bindings.

Verification:
- Script validation clean for the three touched C# files.
- EditMode `CharacterSelectTests`: 7/7 PASS.

## How To Fix Settings

Do not polish the current full-screen generated settings page as final UI. Redesign it as a
proper pause/options hub.

Target direction:
- Open over gameplay with a dark translucent pause layer.
- Use a compact panel or two-column layout, not a sparse debug page.
- Separate production settings from test/debug tools.
- Production tabs should be small and clear: Controls, Gameplay, Audio/Video placeholders.
- Keep key rebinding, attack aim, dash mode, and HP bar visibility, but present them as grouped
  controls with stronger hierarchy.
- Test class switching should be visibly labeled as debug/playtest only, or moved to a dev panel.
- Class switch buttons should show the class identity clearly: class name, class color, and
  preferably the class portrait/idle sprite.
- The active class must be visually obvious after switching: player sprite/controller changes,
  skill bar refreshes, and any resource/class widget should reflect the new class.

Do not make class switching a skill-only shortcut again.

## How To Fix Dungeon Lighting

The current lighting issue is not just intensity. It is composition: the scene has readable fill
but lacks authored pools, contrast, and room identity.

Lighting direction:
- Keep gameplay readable. Do not return to darkness/fog as a visibility mechanic.
- Reduce the "same everywhere" read by giving each room family a light profile.
- Use a low cool ambient base, then local lights for identity.
- Place lights from room/landmark data, not only random generic positions.
- Use asymmetry: one bright landmark side, darker side pockets, wall-socket lights, and small
  prop lights.
- Keep black/negative space outside playable rooms where it supports the reference mood, but do
  not let the camera open on void.

Light archetypes to author:
- Warm torch/candle: amber, medium radius, attached to wall sockets or props.
- Cyan rift/magic: cool blue/cyan, larger softer radius, used near rift landmarks or crystals.
- Violet ritual/moon: purple/violet accent, sparse and mood-heavy, not everywhere.

Implementation direction:
- Add room lighting metadata to the room generation/template system.
- `RuntimeRoomManager` should instantiate lights from layout/template sockets.
- `LargeDungeonMapPainter` or future authored templates should expose landmark/light sockets.
- Do not tune final lighting only against the current placeholder tiles. Lighting and final
  floor/wall art need to be coordinated.

## How To Move Toward rima_style_anchor

Use `rima_style_anchor` as mood/reference, not as a baked room asset.

Keep:
- orthographic isometric gameplay camera
- modular Unity Tilemap room assembly
- black negative space outside rooms
- local light pools
- strong wall/floor readability
- props/landmarks as separate sprites

Regenerate environment art around this target:
- Floor tiles: `64x32` flat isometric top-surface diamonds.
- No raised slabs, no tall opaque footprint, no side faces on floor tiles.
- Floors should merge into one plane with subtle cracks/wear, not read as overlapping blocks.
- Wall modules: `64x96` transparent sprites with vertical face, top cap, corners, pillars,
  door/arch pieces, and sparse damage variants.
- Props: candles, chains, rubble, bones, crystals, plaques, broken slabs, moss/blood stains.
- Lights remain Unity-side 2D lights/VFX, not baked into every tile.

QC requirement before importing new assets:
- Contact sheet review.
- 4x4 repeated floor mockup.
- Simple room mockup with walls, one landmark, and planned light sockets.
- Reject any floor tile that creates height/overlap/checkerboard noise.
- Reject any wall that reads as a short parapet unless that is explicitly the intended piece.

## Recommended Next Work Order

1. Verify the class visual switch fix in Play Mode through ESC test class buttons.
2. Redesign the settings menu structure before adding more settings.
3. Add room lighting profiles/sockets so rooms stop sharing one uniform light mood.
4. Regenerate floor and wall modules toward `rima_style_anchor`.
5. Re-run visual QC with screenshots after lighting and new art are both present.
