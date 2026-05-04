# Core 4 Skill Implementation Pointer - 2026-05-03

## Scope

This pass makes the first playable code version of the four launch/test classes:
- Warblade
- Elementalist
- Ranger
- Shadowblade

Goal is runtime behavior first. Animation/VFX assets are still future production work.
Runtime visuals are simple generated circles/projectiles/zones where no prefab exists.

## Shared Runtime Support

New support scripts:
- `Assets/Scripts/Skills/SkillStateTracker.cs`
  - Temporary class-specific combat states without expanding `StatusEffectType`.
  - Current states: `Sundered`, `Broken`, `RangerMarked`, `Trapped`, `RiftScar`, `BackstabMarked`, `DeathMarked`, `SmokeVeiled`.
- `Assets/Scripts/Skills/SkillRuntime.cs`
  - Shared helpers for enemy queries, line/cone/circle hits, runtime projectiles, runtime zone visuals.
- `Assets/Scripts/Skills/PlayerProjectile.cs`
  - Added an `onHit` callback hook so class basics can restore resource or apply class states on projectile hit.

## Basic Inputs

`Assets/Scripts/Player/PlayerAttack.cs` now owns class basic actions:

| Class | LMB | RMB |
|---|---|---|
| Warblade | Iron Combo melee chain | Rage Outlet: spends 30 Rage, short AoE damage + stagger |
| Elementalist | Rift Bolt projectile; every 3rd is empowered; hit restores Mana +3 | Tap Element Switch; hold 0.2s tries Lightbreak |
| Ranger | Rift Arrow projectile; hold 1s charges and marks | Tactical Roll + instant arrow |
| Shadowblade | Veil Strike short slash; applies scar/mark and Sever | Veil Flicker phase step; passed enemies gain Rift Scar |

## Warblade Skills

Default slots:
- Q `Iron Charge`: dash line hit, stun, Rage gain.
- E `Gravity Cleave`: AoE pull + damage + slow; after Iron Charge, stun bonus.
- R `Sunder Mark`: marks nearest enemy as `Sundered` and applies Weakened.
- F `Earthsplitter`: three ground-crack waves, stun and `Broken` stacks.

Offer/extra implemented scripts:
- `Iron Counter`: parry window; on damage taken, counter nearest enemy and stun.
- `Deep Wound`: close hit + Bleed DoT, Rage +35.
- `Death Blow`: execute on low HP, `Sundered`, or `Broken`; spends all Rage.
- Existing `Crippling Blow`, `Iron Crush`, `Ironclad Momentum`, `Battle Surge` remain usable legacy/advanced support scripts.

## Elementalist Skills

Default slots:
- Q `Fireball`: projectile, Burning, Fire State +1, 3 casts can trigger Living Bomb.
- E `Glacial Spike`: line damage + Chill/Freeze interaction, Frost State +2, consumes Fire State 1.
- R `Living Bomb`: delayed explosion on nearest enemy; kill can chain to nearby enemies.
- F `Blink`: 6m reposition, damages enemies passed through, empowers next spell placeholder.

Offer/extra implemented scripts:
- `Frozen Orb`: now has runtime fallback orb if prefab is missing.
- `Prism Beam`: piercing line beam, spends Light State for stronger impact.
- `Meteor`: delayed AoE, chilled/frozen targets take stronger knockdown.
- `Frost Wall`: line wall effect, Chill + damage.
- `Solar Flare`: cone radiant burst, stronger during Light State.
- `Blizzard`: delayed slow/tick zone.

State:
- Active element: Fire/Frost/Light.
- Fire/Frost/Light stacks.
- Fire/Frost resonance.
- Lightbreak: requires Fire Resonance 3, Frost Resonance 3, Fire State 3, Frost State 3; consumes 3+3, opens 6s Light State, 8s cooldown.

## Ranger Skills

Default slots:
- Q `Pinning Shot`: projectile root/stun, applies `Trapped` and `RangerMarked`.
- E `Bone Trap`: forward trap zone, roots/marks enemies in the zone.
- R `Marked Detonate`: detonates all marked enemies near player, damage scales by mark stacks.
- F `Sweep Volley`: cone volley, damage + marks.

Offer/extra implemented scripts:
- `Hunter's Step`: reposition dash, Focus gain, next-crit state placeholder.
- `Predator's Mark`: AoE mark; Focus 75+ allows more marked targets.
- `Final Strike`: only works on targets that are both `RangerMarked` and `Trapped`; large execute damage.
- `Wireline Trap`: line trap, snare/stun + mark + trapped state.

## Shadowblade Skills

Default slots:
- Q `Phase Step`: phase dash through enemies, damage + Rift Scar, brief stealth.
- E `Backstab Mark`: close strike; stealth version hits harder, applies backstab mark + scar.
- R `Death Mark`: delayed explosion mark.
- F `Shadow Pin`: dagger projectile, stun/root + Rift Scar.

Offer/extra implemented scripts:
- `Shadow Clone`: decoy pulse object, applies damage + Rift Scar.
- `Veil Burst`: 4 short teleport-strikes, applies scars.
- `Severance`: collapses Rift Scar stacks for damage and Sever gain.
- `Smoke Veil`: stealth/smoke state, slows nearby enemies.
- `Chain Cull`: jumps between marked/scarred targets up to 3 hops.
- `Night Aperture`: 6s aperture state placeholder for mirrored scar animation/system pass.

State:
- `Sever` 0-100 lives on `Shadowblade_SkillController`.
- LMB and phase movement generate Sever.

## Animation Integration Notes

Do not wire final animations directly inside each skill script yet. Add a small future bridge:
- `SkillAnimationBridge` listens to `SkillBase.TryActivate()` success or a future `OnSkillActivated` event.
- Basic actions from `PlayerAttack` emit stable action names.
- Bridge maps action names to Animator triggers.

Suggested trigger names:
- Warblade basics: `Basic_0`, `Basic_1`, `Basic_2`, `RageOutlet`
- Elementalist basics: `RiftBolt`, `RiftBolt_Empowered`, `ElementSwitch`, `Lightbreak`
- Ranger basics: `RiftArrow`, `RiftArrow_Charged`, `TacticalRoll`
- Shadowblade basics: `VeilStrike`, `VeilFlicker`
- Skill triggers: `Skill_<SkillNameNoSpaces>`, for example `Skill_IronCharge`, `Skill_GlacialSpike`, `Skill_PinningShot`, `Skill_PhaseStep`

Production animation workflow:
- Keep the locked 3-segment rule from `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`: PEAK first, then START_TO_PEAK, then PEAK_TO_END, then combine.
- Generate/run clips per current player facing convention. Current production movement direction decision is still a known conflict in status docs; resolve final 4-diagonal vs 8-way before mass skill animation generation.
- For each skill, first animate the body pose and timing. Projectiles, trap zones, beam/wall decals, and hit flashes should stay Unity-side VFX objects.
- Do not bake gameplay hit timing into sprites. Add animation events later only as presentation sync points; code remains authoritative for hit detection.

## Next Recommended Work

## Verification

- Unity script refresh/compile: no game compile errors after full asset refresh.
- Script validation: checked representative touched scripts, no validation errors.
- EditMode tests: 129/129 PASS.
- Play Mode smoke: primary class switching verified for all four classes.
  - Warblade: `Iron Charge, Gravity Cleave, Sunder Mark, Earthsplitter`
  - Elementalist: `Fireball, Glacial Spike, Living Bomb, Blink`
  - Ranger: `Pinning Shot, Bone Trap, Marked Detonate, Sweep Volley`
  - Shadowblade: `Phase Step, Backstab Mark, Death Mark, Shadow Pin`
- Console caveat: console still shows MCP client handler exit exceptions from the tooling package; no game compile/runtime skill error was reported.

## Next Recommended Work

1. Add `SkillAnimationBridge` once final clip naming and 4-dir/8-dir direction policy is locked.
2. Make PlayMode tests for class selection and Q/E/R/F activation for all four classes.
3. Replace runtime circle visuals with final PixelLab/Unity VFX prefabs class by class.
4. Continue map/lighting/template polish after this skill pass is reviewed.
