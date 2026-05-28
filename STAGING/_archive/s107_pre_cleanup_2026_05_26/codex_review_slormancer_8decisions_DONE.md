# Codex Review: Slormancer-Inspired 8 Decision Candidates

## Executive verdict

Recommendation: accept the package, but do not accept it as one production chunk. The safe RIMA adaptation is a staged buildcraft stack: first readability and control support, then skill mastery, then weapon keystones, then endgame scaling. Slormancer is strongest as a buildcraft reference, not as a camera, boss, or full loot-economy reference. RIMA should keep its locked 64px/PPU hierarchy, 30-35 degree high top-down character view, weaponless body plus WeaponSR child pipeline, Character States workflow, and Karar #143-E readability discipline.

The only major scope correction is the skill expansion question. I recommend Path B+limited A: no broad class pool expansion, no 20-30 always-new skills. Weapon keystones should mostly modify existing skill mastery branches, but each class may get exactly one signature weapon-bound skill if the class identity needs a new input fantasy that cannot be expressed as a mastery mutation. Numeric target: 10 new weapon-bound skills total, one per class, plus weapon-aware mastery branches for the remaining keystones.

## Karar #147 - Per-Skill Mastery Tree

Verdict: PASS.

Gerekce: This is the highest-value Slormancer transfer because it deepens the existing 80-skill pool without requiring 80 new skills. The 3-tier, 2-3 choice model fits RIMA's roguelite run cadence better than a full ARPG passive web, as long as unlocks are run-readable and not permanent grind bloat.

RIMA uyum kontrolu: #74 is unaffected because mastery is data/UI, not sprite scale. #100 is unaffected because no camera or sprite style change is implied. #143-E benefits indirectly because mastery choices can push damage/VFX density, so each upgrade must carry a readability budget. #144 is compatible because weapon-aware mastery can query weapon identity without embedding weapons into body sprites. #145 is compatible because major mastery variants can request Character State variants only for top-tier visual deltas.

Sprint target: Sprint 16+ is correct, after combat integration and after basic UI clutter rules exist.

Dependency: implement after #153 UI clutter control and before #148 weapon keystones become fully systemic.

## Karar #148 - Class Weapon Keystone

Verdict: MODIFY.

Gerekce: 2-3 build-defining weapons per class is a good long-term target, but "each keystone radically changes skill behavior" risks multiplying design, VFX, balance, and tooltip load across 20-30 weapons. The fix is to define keystone weapons as "one unique modifier plus one weapon-aware mastery hook", with only one signature bound skill per class.

RIMA uyum kontrolu: #74 is safe if weapon sprites stay child SR assets with 64 PPU assumptions. #100 is safe if weapons are readable in 30-35 degree view and do not force Slormancer's more oblique camera. #143-E requires keystone VFX to preserve center readability, especially persistent zones. #144 is the core enabler. #145 can be used for boss-drop visual states, but only when a weapon changes posture or phase identity enough to justify asset work.

Specific fix: cap launch scope to 10 class signature keystones, one per class, then add the second and third keystone as modifier-only backlog entries. Each weapon gets a one-line "no new animation required / Character State required" flag before production.

Sprint target: move full systemic version from Sprint 18+ to Phase 2. Allow one pilot weapon in Sprint 18 only if #147 is already stable.

Dependency: #144 and #146 first, #147 before full behavior matrix, #151 before run-swap ergonomics.

## Karar #149 - Elite Affix Tooltip System

Verdict: PASS.

Gerekce: Elite affixes give room-level tactical variety at low content cost. The proposed six affixes are enough for a first pass and should be capped at one affix for early elites, two affixes only in higher curse tiers or late act rooms.

RIMA uyum kontrolu: #74 is safe because affixes are behavior/UI labels. #100 is safe. #143-E is relevant because affix VFX must not fill arena centers with uncontrolled noise. #144/#145 are not direct dependencies, but Character States can later create armored/enraged elite variants.

Sprint target: Sprint 15 is correct if implemented as a data label, tooltip, and one or two simple behavior hooks first.

Dependency: should follow baseline mob telegraph rules and precede #150 so curse tiers can reuse affix density.

## Karar #150 - 3-5 Step Heat/Curse Scaling

Verdict: PASS.

Gerekce: RIMA should not copy Slormancer's Wrath 10+100 infinite ARPG scaling. A 3-5 step curse ladder is enough for roguelite replay pressure, reward tuning, and build validation without burying the player under persistent loot math.

RIMA uyum kontrolu: #74/#100 are unaffected. #143-E matters because higher curse density must not raise visual clutter beyond readability. #144/#145 are indirect: weapon and state systems can be rewards, but should not be mandatory to enter Curse 1-2.

Sprint target: Phase 2 is correct. Do not ship this before the basic run loop, elite affixes, and loadout UX are stable.

Dependency: #149 first, #151 before high curse swapping, #153 before enemy density and damage number pressure increase.

## Karar #151 - Loadout System

Verdict: MODIFY.

Gerekce: Loadouts are strategically valuable once mastery and weapon keystones exist, but in-run hot swap can undermine roguelite commitment if it is free and instant. The system should start as hub/loadout presets plus limited room-gate swaps, not constant combat swapping.

RIMA uyum kontrolu: #74/#100 are unaffected. #143-E benefits because players can choose lower-clutter setups for dense rooms. #144 is relevant because loadouts may include weapon child SR definitions. #145 is relevant only if loadout swaps trigger visual state sets, which should be deferred.

Specific fix: make v1 "2 presets, swap at room entrance or shrine, not during active combat." Tab hotkey can open compare/swap UI only when safe. Add combat-swap later if playtests demand it.

Sprint target: HUD v2 is acceptable, but only as preset management. Full run-time hot swap should wait until #147/#148 are clearer.

Dependency: basic skill equip UI first, #147 data model next, #148 weapon slot integration before final.

## Karar #152 - Cursor-Based Active Camera

Verdict: MODIFY.

Gerekce: Cursor-weighted framing is useful for aim classes, but it must not break the locked 30-35 degree top-down read or create motion sickness in dense rooms. A fixed 80/20 blend should be a tunable profile, not a global hardcode.

RIMA uyum kontrolu: #74 is safe. #100 is safe only if this remains an XY framing offset and never changes camera angle, projection, or asset perspective. #143-E supports the change because wall-edge density and center sparseness work better when the player can see slightly toward aim direction. #144/#145 are unaffected.

Specific fix: implement as `CameraFollow` settings: enabled per class, weight 0.10-0.22, max offset in world units, smoothing time, and deadzone. Default off for melee-first classes until tested.

Sprint target: Sprint 14 is correct as a small technical feature and should be P0 because it informs Ranger/Gunslinger/Elementalist feel.

Dependency: camera baseline and input cursor world-position code first; no dependency on skill mastery.

## Karar #153 - Dynamic UI Clutter Control

Verdict: PASS.

Gerekce: This should ship before mastery and keystones, because every later system increases VFX, damage numbers, and tooltip density. Threat-point based damage number scaling and grouping is a practical early guardrail.

RIMA uyum kontrolu: #74/#100 are unaffected directly. #143-E is strongly aligned: both are readability rules that keep combat centers playable and decoration/noise under control. #144/#145 are indirect; weapon and state variants can add screen noise, so they need this system as a budget gate.

Sprint target: Sprint 15 is correct, but the data hooks should be designed during Sprint 14 if camera and UI are touched.

Dependency: should precede #147, #148, #149 high-affix rooms, and #150 high curse tiers.

## Karar #154 - AoE Telegraph Decal Pass

Verdict: MODIFY.

Gerekce: A telegraph readability pass is necessary, but pure color selection is not enough and should not be framed as "neon palette outside Vivid Vulnerability" without the existing accessibility standard. RIMA already needs outline pulse, ground shake, and color glow as separate channels.

RIMA uyum kontrolu: #74 is safe if telegraph sprite scale follows PPU and boss/mob scale hierarchy. #100 is safe if decals are authored for the locked top-down view. #143-E is central: AoE decals must be strongest at relevant danger zones but not hide floor/wall readability. #144 is unaffected. #145 can help only if a hazard belongs to a boss phase state.

Specific fix: keep the proposed Hexer blood red and Ranger cold-blue as decorative color identities, but require a non-color primary cue: outline pulse, shape language, and timing. Add a per-skill telegraph contract: radius, lead time, pulse count, color, outline thickness, and opacity ceiling.

Sprint target: Sprint 16+ is correct, after #153; individual boss-critical telegraphs can be pulled earlier if combat testing blocks.

Dependency: #153 first, enemy/hazard telegraph standard first, then class-specific palette pass.

## Skill expansion verdict

Preferred path: Path B plus limited Path A. Do not use Path C for the next production phase.

Reason: RIMA already has 10 classes, 8 active slots per class, about 80 active skills, cross-class echo pressure, four equip slots, and upcoming mastery choices. Expanding every class pool by 2-3 always-available skills would grow the pool to 100-110 before the current 80 are fully proven. That is a balance and asset multiplier, not a clarity win. Pure Path B is cheaper, but it risks making keystone weapons feel like passive affixes. The compromise is one weapon-bound signature skill per class, used only where the weapon fantasy cannot be represented by modifying an existing skill.

Numeric target: 10 new weapon-bound skills total for the first full keystone pass. Not 20-30. Each class receives 2-3 keystone weapons, but only one of them unlocks a new skill. The remaining 1-2 keystones per class alter existing mastery branches, cooldown patterns, resource conversion, projectile shape, summon rules, or zone behavior.

Production order: skill spec first, mastery tree second, keystone weapon third. The reason is simple: the base skill pool defines the combat language. Mastery defines legal mutation space. Weapons then choose which mutation space to bend. If weapons are designed first, they will force one-off exceptions and asset demands before the system has constraints.

Detailed mapping:

Warblade:
- Bloodseeker Axe: no new skill. Modifies Iron Charge / cleave mastery with HP trade, bleed burst, and self-wound payoff.
- Oathbreaker Greatsword: new bound skill, "Grave Oath." A heavy commit STRIKE/STATE hybrid that marks the next weapon skill to echo once if the player takes damage during windup. This needs new skill treatment because it changes Warblade's risk posture, not only damage math.
- Banner Maul: no new skill. Modifies zone/control mastery, adding slam aftershocks and elite stagger windows.

Hexer:
- Blight Lantern: no new skill. Modifies existing Blight Sigil and curse zone mastery with delayed detonation and anti-heal.
- Bone Covenant Scepter: new bound skill, "Debt Bloom." A REACTIVE/ZONE skill that stores curse ticks and blooms them when enemies die inside the mark. This needs a bound skill because it creates a new death-economy loop.
- Ash Needle: no new skill. Modifies projectile/hex mastery into piercing marks and lower-clutter single-target execution.

Recommended 10-class one-bound-skill allocation:

| Class | New bound skill? | Keystone needing it | Other keystones use |
|---|---:|---|---|
| Warblade | 1 | Oathbreaker Greatsword | HP trade / stagger mastery |
| Elementalist | 1 | Prism Core Staff | element braid mastery |
| Shadowblade | 1 | Eclipse Twinblade | stealth echo mastery |
| Ranger | 1 | Stormstring Bow | trap / arrow mastery |
| Ravager | 1 | Hunger Cleaver | rage overflow mastery |
| Ronin | 1 | Moon-Sheath Katana | draw / counter mastery |
| Gunslinger | 1 | Blackpowder Relic | reload / ricochet mastery |
| Brawler | 1 | Iron Vow Gauntlets | guard / throw mastery |
| Summoner | 1 | Choir Bell | minion command mastery |
| Hexer | 1 | Bone Covenant Scepter | blight / curse mastery |

Weapon-bound skills should occupy a special temporary slot while the weapon is equipped, not permanently expand the class draft pool. They should not count as a fifth equip slot unless the loadout system explicitly supports it. For v1, they can replace the weapon's active command or attach to the Ghost Attack input family, depending on controller ergonomics.

## Final table

| Karar | Verdict | Sprint | Dependency |
|---|---|---|---|
| #147 Per-Skill Mastery Tree | PASS | P1, Sprint 16+ | #153 before, #148 after |
| #148 Class Weapon Keystone | MODIFY | P2, Phase 2 / Sprint 18 pilot | #144/#146/#147 |
| #149 Elite Affix Tooltip | PASS | P1, Sprint 15 | mob telegraph baseline |
| #150 Heat/Curse Scaling | PASS | P2, Phase 2 | #149/#151/#153 |
| #151 Loadout System | MODIFY | P1/P2, HUD v2 | skill equip UI, #147 |
| #152 Cursor-Based Active Camera | MODIFY | P0, Sprint 14 | camera/input baseline |
| #153 Dynamic UI Clutter Control | PASS | P0/P1, Sprint 15 | UIManager/theme hooks |
| #154 AoE Telegraph Decal Pass | MODIFY | P1, Sprint 16+ | #153, telegraph standard |

Priority ordering:

P0: #152 camera pilot, #153 clutter foundations.

P1: #149 elite affix v1, #151 loadout preset v1, #147 mastery data/UI, #154 telegraph contract.

P2: #148 full weapon keystone system, #150 curse scaling, second/third class keystone waves.

Skill expansion verdict: Path B plus limited Path A, 10 new weapon-bound skills total, not 20-30. No Path C before the current 80-skill pool is playable and audited.

Total scope estimate: 5-7 focused sprints for the whole package if kept staged. Sprint 14 handles camera/control proof. Sprint 15 handles clutter and elite affix UI. Sprint 16 handles mastery and telegraph contracts. Sprint 17 hardens mastery plus loadout v1. Sprint 18 pilots one or two weapon keystones. Phase 2 expands to all 10 bound skills and adds curse scaling. If Path A is expanded to 20-30 new skills, scope jumps to 9-12 sprints and should be rejected for now.
