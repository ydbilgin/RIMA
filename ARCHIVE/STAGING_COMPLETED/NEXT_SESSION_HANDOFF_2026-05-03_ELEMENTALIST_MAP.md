# Next Session Handoff - Elementalist / Map Follow-up - 2026-05-03

## Start Here

Current continuation point:
- Fix Elementalist implementation so it matches `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`.
- Keep the new natural dungeon/map work, but continue art/design pass later.
- User will reconnect in a new session and wants missing parts corrected.

Read first:
- `CURRENT_STATUS.md`
- `STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ELEMENTALIST_MAP.md`
- `STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ENV_MAP.md`
- `STAGING/RIMA_MAP_PRODUCTION_AND_TOOL_RESEARCH_2026-05-03.md`
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
- `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md`
- `Assets/Scripts/Player/PlayerAttack.cs`
- `Assets/Scripts/Skills/Elementalist/Elementalist_SkillController.cs`
- `Assets/Scripts/Skills/Base/SkillBase.cs`
- `Assets/Scripts/UI/SkillBarUI.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`

## What Was Done In This Session

### Natural dungeon / map pass

Main file:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`

Related scene/doc files:
- `Assets/Scenes/_IsoGame.unity`
- `STAGING/RIMA_MAP_PRODUCTION_AND_TOOL_RESEARCH_2026-05-03.md`

Changes:
- Large map painter moved away from plain rectangle-first generation.
- Added mask-first natural room generation inside `LargeDungeonMapPainterBase`.
- Added multiple authored/procedural room templates:
  - BrokenEntryHall
  - ChainGallery
  - ShrineCrossroad
  - CryptBasin
  - PillarArena
  - SplitVault
  - RitualHall
  - CollapsedLibrary
  - NarrowApproach
  - BossAntechamber
- Added local procedural room lights under `Procedural Room Lights`.
- Fixed Global Light 2D target sorting layers so the whole dungeon can be visible.
- Tilemap materials were set to Sprite-Lit-Default through Unity editor tooling.
- Final current lighting rule from user feedback:
  - Full playable dungeon should remain visible for now.
  - No darkness/fog/limited-vision gameplay until there is a real torch/light mechanic.
  - Local lights should be accents and composition markers, not the only visibility source.

Verification:
- Script validation: clean for touched map script.
- EditMode tests: 129/129 PASS.
- Useful screenshot:
  - `Assets/Screenshots/debug_game_view_natural_map_all_layers_lit_2026_05_03.png`

Remaining map work:
- Continue making maps look closer to `rima_style_anchor.png`.
- Need more natural composition: anchors, landmarks, side pockets, asymmetry, broken edges, thresholds.
- Need more templates, not one final map.
- Need final floor/wall art module pass; current art is still system/prototype quality.
- Tool research verdict was documented:
  - LDtk or Tiled are best for authored room masks.
  - Unity Tilemap remains final renderer.
  - PixelLab should generate modular art pieces, not whole playable maps.
  - Dungeon Architect / DunGen are heavier options.
  - WFC is useful for micro detail only, not main room composition.

### Elementalist prototype implementation

Files changed:
- `Assets/Scripts/Player/PlayerAttack.cs`
- `Assets/Scripts/Skills/Elementalist/Elementalist_SkillController.cs`
- `Assets/Scripts/Skills/Elementalist/Fireball.cs`
- `Assets/Scripts/Skills/Elementalist/ElementalistRuntimeVisuals.cs`
- `Assets/Scripts/Skills/Base/SkillBase.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`
- `Assets/Scripts/UI/SkillBarUI.cs`
- `Assets/Scenes/_IsoGame.unity`

Implemented:
- Elementalist now has a ranged LMB prototype through `PlayerAttack`.
- Runtime projectile visuals were added through `ElementalistRuntimeVisuals`.
- Fireball can spawn a runtime fallback projectile if no prefab is assigned.
- `Elementalist_SkillController` now creates a default loadout.
- `PlayerClassManager.SetPrimaryClass(Elementalist)` now adds/enables:
  - `ManaSystem`
  - `Elementalist_SkillController`
  - disables other primary class controllers
- `SkillBase` now re-resolves missing player/resource refs and prefers the correct class resource.
- SkillBar UI now supports Elementalist instead of being Warblade-only.
- HUD labels added:
  - class label
  - resource label
  - input hint
- Scene now has:
  - `PlayerClassManager` on `Systems`
  - `HUD_Canvas/SkillBar` with `SkillBarUI`
- Camera orthographic size was tuned to `5.2` for a slightly farther, more room-readable view.

Verification:
- Script validation: no compile errors in touched scripts.
- EditMode tests: 129/129 PASS.
- Play Mode smoke:
  - Elementalist selection works.
  - Mana starts at 100/100.
  - Warblade controller is disabled.
  - Elementalist slots exist.
  - LMB prototype and Fireball both create projectiles.
  - Fireball spends mana.
- Useful screenshot:
  - `Assets/Screenshots/debug_game_view_elementalist_skillbar_zoom_2026_05_03.png`

## Important Correction: Elementalist Is Not Fully Canonical Yet

User asked whether the implementation follows the class/skill design docs.

Answer:
- Only partially.
- The first implementation was a fast playable prototype based on existing repo files and current scene needs.
- It does not yet fully match `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`.

Canonical Elementalist design from the doc:
- Class verbs: `switch`, `shape`, `detonate`.
- Resource: Mana 0-100 plus Fire/Frost elemental state stacks.
- LMB:
  - `Rift Bolt`
  - fast projectile
  - Mana +3 on hit
  - every 3rd bolt is empowered
  - empowered bolt is bigger and adds +1 active Elemental State
  - can fire while moving
- RMB:
  - `Element Switch`
  - tap switches Fire/Frost
  - Fire/Frost casts build Resonance
  - when both reach 3, hold opens Lightbreak / Light State
- Core active skill list:
  - Fireball
  - Glacial Spike
  - Living Bomb
  - Blink
  - Frozen Orb
  - Prism Beam
  - Meteor
  - Frost Wall
  - Solar Flare
  - Radiant Pillar
  - Element Charge
  - Blizzard
- Offer-system decision doc says:
  - Normal Elementalist active pool should include Fireball, Glacial Spike, Living Bomb, Blink, Frozen Orb, Prism Beam, Meteor, Frost Wall, Solar Flare, Blizzard.
  - Radiant Pillar and Element Charge should become upgrade/passive style items, not normal standalone active offers.

Current wrong/placeholder parts:
- LMB is currently `Arcane Bolt` prototype, not canonical `Rift Bolt`.
- LMB does not yet restore Mana +3 on hit.
- LMB does not yet empower every 3rd bolt.
- LMB does not yet add Elemental State on empowered hit/cast.
- RMB currently triggers Fireball placeholder behavior.
- RMB should be `Element Switch`, not Fireball.
- Default loadout is currently prototype:
  - Q Fireball
  - E Blink
  - R Frozen Orb
  - F Meteor
- Canonical early loadout should probably start with:
  - Q Fireball
  - E Glacial Spike
  - R Living Bomb
  - F Blink
- HUD hint currently reflects the prototype behavior and must be updated after canonical correction.
- Elemental State HUD is missing.
- Lightbreak / Light State is not implemented.
- Trinity Storm / V burst is not implemented.

## Recommended Next Task

Make a bounded canonical Elementalist correction pass:

1. Replace LMB prototype naming/behavior with `Rift Bolt`.
2. Add Elementalist runtime state:
   - active element Fire/Frost
   - fire stacks
   - frost stacks
   - bolt counter
3. Add LMB effects:
   - Mana +3 on hit
   - every 3rd shot empowered
   - empowered shot bigger/stronger
   - empowered shot adds +1 active element stack
4. Change RMB to `Element Switch`.
5. Change default slots:
   - Q Fireball
   - E Glacial Spike
   - R Living Bomb
   - F Blink
6. Update SkillBar UI:
   - show Mana
   - show active element and Fire/Frost stacks
   - hint should say:
     `LMB RIFT BOLT | RMB SWITCH | Q FIREBALL | E GLACIAL SPIKE | R LIVING BOMB | F BLINK`
7. Verify:
   - script validation
   - EditMode tests
   - Play Mode smoke for LMB/RMB/Q/E/R/F

Do not implement all 12 Elementalist skills in the same pass unless explicitly requested. That would be a larger balance/design surface and should be split.

## Current User Intent At Session End

User said they will reconnect in a new session and wants the missing parts corrected.

Most likely next request:
- Correct Elementalist to the class/skill docs.
- Then continue map/lighting/template polish.

