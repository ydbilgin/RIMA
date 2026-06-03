# Research: per-class weapon variants + weapon swap + weapon-driven skill evolution - CX feasibility

## Ground truth from current code

- Weapon attachment already exists as runtime prefab attachment, not as a gameplay system. `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` calls `AttachWeapon("Base")` in `Start()`, looks up `WeaponDatabaseSO.GetWeapon(classId, formId)`, destroys the previous instance, instantiates the weapon prefab under `handAnchor`, then wires it to `OrientationSync.SetWeaponTransform`.
- `WeaponDatabaseSO.WeaponEntry` already has `classId`, `formId`, `weaponPrefab`, anchor/grip offsets, two-handed flags, orientation offset, and 8 hand offsets. This is already close to a variant database, but the runtime only uses `"Base"` and does not expose selected weapon state.
- `OrientationSync` owns 8-direction hand anchor offsets, 8 base rotations, `flipY`, and procedural swing rotation. Swapping weapon prefab at runtime is low-risk visually if the replacement prefab has a `SpriteRenderer` and sane pivot/offset data. The missing piece is re-wiring `weaponRenderer` and resyncing sort/rotation after a swap.
- Basic attacks are data-driven through `BasicAttackProfile` plus `IBasicAttackBehavior` implementations. This is better suited to weapon modifiers than the active skill system is.
- Active skills are component-driven MonoBehaviours. `SkillData` is created at runtime in `SkillDatabase`, and each entry points to a concrete `System.Type skillType`. Controllers assign `SkillBase` components into slots. That means full skill replacement is possible, but every new evolved skill currently implies a new component class or branching inside an existing component.
- Draft is currently skill/passive reward flow. `DraftManager` stores `List<SkillData> currentActiveSkills`, passive levels, and assigns active skills through `Warblade_SkillController`. It does not have a generic reward category for weapons and is still Warblade-controller centric in assignment, even though the offer pool knows class types.
- Implemented skill surface is uneven across the 10 class canon. Warblade, Elementalist, Shadowblade, Ranger, and Ronin have concrete controller/skills. Gunslinger, Brawler, Ravager, Summoner, and Hexer exist in `ClassType` and basic attack profile behavior enum/logic, but their full active skill controllers are not present in the inspected skill tree.

LOCKED direction check:
- Compatible with cursor-aim if variants modify existing attacks/skills rather than replacing input model.
- Compatible with floating-island and painterly VFX if variants are visual/gameplay data, not world-structure changes.
- Compatible with PixelLab sprites because the current weapon path already wants separate weapon prefabs/sprites.
- Risk against "12 skills/class depth-not-breadth" if weapon variants multiply every skill into full unique sets. The sane model is one base skill canon plus small weapon modifiers.

## Ranked ROI recommendation

### Rank 1 - Option B: weapon = stat/behavior modifier on existing skills

Best ROI. Build this first.

What changes:
- Add a `WeaponVariantDefinition` ScriptableObject or extend current `WeaponDatabaseSO.WeaponEntry` into a richer definition.
- Store selected weapon on a runtime `PlayerWeaponState` component.
- Let `HandAnchorAttach` expose `EquipWeapon(string formId)` that calls `AttachWeapon(formId)`, rewires `OrientationSync.SetWeaponTransform`, refreshes `weaponRenderer`, and syncs current facing direction.
- Add a read-only API such as `PlayerWeaponState.Current` and small query helpers: `HasTag`, `GetFloat`, `GetInt`, or typed properties.
- For the prototype, add variant checks inside 1-2 skill components only, not every skill. Example: Ranger `PinningShot` and `MarkedDetonate`, or Warblade `IronCharge` and `GravityCleave`.
- For basic attacks, either swap `BasicAttackProfile` or apply a lightweight modifier inside the relevant `IBasicAttackBehavior`.

Data model:
- Prefer new `WeaponVariantDefinition : ScriptableObject`:
  - `ClassType classType`
  - `string variantId`
  - `string displayName`
  - `string weaponFormId`
  - `BasicAttackProfile basicAttackOverride` optional
  - `WeaponModifier[] modifiers`
  - optional icon/description
- Keep `WeaponDatabaseSO` as visual mount database. Do not overload it with balance until the prototype proves out.

Cost: M.
- Runtime visual swap is S because the mount system already exists.
- Gameplay modifier plumbing is M because skills are component-specific and need opt-in hooks.
- Content authoring cost is controllable because each variant modifies a few verbs, not all 12 skills.

Why this fits RIMA:
- RIMA already has class identity mechanics in controllers: Ranger mark/trap/focus, Elementalist active element, Shadowblade scar/sever, Warblade rage, Ronin tension. Weapon variants can bias those existing mechanics without inventing parallel systems.
- Preserves 12 skills/class depth-not-breadth: the skill remains the same named skill, but has a weapon-flavored rider.

### Rank 2 - Option A: cosmetic-only sprite swap

Lowest risk, but limited design value.

What changes:
- Add public `EquipWeapon(formId)` to `HandAnchorAttach`.
- Fix `AttachWeapon` to rewire `OrientationSync`, `weaponRenderer`, alpha, sort order, and current orientation every time, not only in `Start()`.
- Add selected `formId` storage somewhere persistent for the run, probably `PlayerWeaponState`.
- Add weapon variant entries to `WeaponDatabaseSO`.

Data model:
- Current `WeaponDatabaseSO.WeaponEntry` is enough for pure cosmetic variants.

Cost: S.
- The core is already present. The main risk is prefab pivot/offset quality and testing 8 directions.

Why not stop here:
- Cosmetic swap can validate PixelLab weapon production and runtime mounting, but it will not justify a Phase-2 system by itself unless weapon identity is intended as visual progression only.

### Rank 3 - Option C: weapon = swaps whole skill set

Highest content cost and biggest conflict with locked direction. Avoid as default.

What changes:
- Every weapon variant needs its own loadout table or skill database entries.
- Controllers need to rebuild slots when weapon changes, preserve cooldown/resource state, and update UI.
- `SkillDatabase` must understand variant-specific skill pools or separate `skillType` mappings.
- `DraftManager` must filter offers by class + weapon variant and prevent stale skills from another weapon from remaining equipped.

Data model:
- `WeaponVariantDefinition` with `SkillData[] skillPoolOverride` or `Dictionary<baseSkillId, evolvedSkillType>`.
- Likely also needs `SkillId` fields because current `SkillData` is runtime-created and name/type based.

Cost: L.
- It multiplies code and content. It also fights the 12 skills/class canon by turning "12 skills" into "12 times N weapons" unless the variants are very sparse.

When it becomes acceptable:
- Only for rare signature transformations, e.g. a Phase-2 mastery weapon that replaces 1 slot or 1 basic attack behavior, not a full class kit.

## Least-churn weapon-driven evolution model

Recommended model: base skill component + weapon modifier query + optional small strategy object.

Implementation shape:
- Add `PlayerWeaponState` on the player:
  - current `WeaponVariantDefinition`
  - `Equip(WeaponVariantDefinition def)`
  - invokes `OnWeaponChanged`
  - calls `HandAnchorAttach.EquipWeapon(def.weaponFormId)`
- Add a small helper in `SkillBase`:
  - cache `PlayerWeaponState weaponState`
  - expose `protected WeaponVariantDefinition CurrentWeapon`
- Add targeted hooks in evolved skills:
  - direct conditional for prototype
  - later, move to `WeaponSkillModifier` data if patterns stabilize

Prototype first: Ranger.

Why Ranger:
- Ranger already has clear weapon fantasy in the task: bow -> crossbow -> other bow.
- Ranger has existing concrete controller and skills under `Assets/Scripts/Skills/Ranger`.
- Ranger skills already share state (`RangerMarked`, `Trapped`) through `SkillStateTracker`, making weapon effects easy to express without rebuilding the class.
- `Ranger_SkillController` is simpler than Warblade secondary slots and less entangled with `DraftManager` assumptions.

Concrete prototype:
- Add two Ranger variants:
  - `Ranger_Bow_Base`: default, no gameplay modifier.
  - `Ranger_Crossbow`: slower basic cadence, higher mark/trap payoff.
- Visual:
  - add `WeaponDatabaseSO` entries for `classId = "Ranger"`, `formId = "Base"` and `formId = "Crossbow"`.
- Gameplay:
  - `PinningShot`: if crossbow, increase projectile damage or root duration, reduce projectile speed slightly.
  - `MarkedDetonate`: if crossbow, consume mark for stronger single-target detonation but smaller/no area effect.
  - Basic attack `ShotCadenceBehavior`: either use a `BasicAttackProfile` override or read a modifier for projectile cooldown/damage.
- UI/draft:
  - add one debug or draft offer path to equip crossbow after a room clear.

Avoid prototyping first on:
- Warblade: viable, but `DraftManager` and secondary slot logic are more entangled with Warblade today, so it can hide architecture problems.
- Ronin: lives under `Assets/Scripts/Combat/Classes/Ronin`, separate from the legacy `Assets/Scripts/Skills` tree. Good later test for cross-system support, not first.
- Gunslinger/Brawler/Ravager/Summoner/Hexer: canon classes exist in enums/identity, but full active skill controller surface was not present in inspected code. Do not base architecture on missing systems.

## Acquisition flow recommendation

Recommendation: reuse DraftManager UI flow at first, but add weapon offers as a new reward type rather than pretending a weapon is a `SkillData`.

What changes:
- Extend `RewardType` with `Weapon`.
- Extend `RewardOffer` with `WeaponVariantDefinition weapon`.
- Add `WeaponOfferGenerator` or let `SkillOfferGenerator` take a small list of eligible weapon offers after room depth gates.
- `DraftManager.OnOfferSelected` handles `RewardType.Weapon` by calling `PlayerWeaponState.Equip(chosen.weapon)`, then finishes the pick.
- Keep weapons out of `currentActiveSkills` and passive levels.

Cost: M.
- UI already displays reward cards, but tooltip/icon/copy path may need a weapon branch.
- Draft cadence, room gating, and selection lock are already solved.

Why not a new acquisition system now:
- A new forge/vendor/loadout layer is larger and not needed to validate the mechanic.
- Existing rooms already call `DraftManager.ShowDraft()`, including fragment-gated draft flow and forge milestones.

Recommended Phase-2 acquisition rules:
- Weapon variants appear at fixed milestone rooms or rare draft slots, not in every normal skill draft.
- First variant choice should be deterministic or heavily weighted to show the system.
- Later swaps should be explicit "replace current weapon" choices, because changing weapon identity mid-run changes build expectations.

## Scope and power-creep control

Sane scope:
- Start with 2 variants per class: Base + one alternate.
- Only 1-2 skills per class get explicit weapon riders in Phase-2.
- One basic attack modifier per weapon is allowed.
- No full 12-skill remap.
- No variant-specific passive tree until the base feature is proven.

Power budget:
- Weapon variants should trade shape, not add raw power.
- Each weapon gets one upside and one cost:
  - slower but heavier
  - wider but weaker
  - safer but lower burst
  - better setup but worse immediate damage
- Avoid stacking multiplicative bonuses with existing passives unless deliberately capped.

Shared base + modifier rule:
- Every class keeps the same canonical 12 skills.
- Weapon modifiers affect:
  - range/radius/count
  - resource cost/refund
  - state application duration/stacks
  - cooldown
  - basic attack profile
- Weapon modifiers should not introduce new resource bars.

## 10-class mapping, constrained by current code reality

Warblade - greatsword:
- Base: balanced greatsword.
- Variant: heavy cleaver.
- Modifier lane: bigger arcs, slower commitment, more rage on multi-hit, lower dash cancel.
- Prototype later on `GravityCleave` or `Earthsplitter`, not first.

Ranger - bow:
- Base: bow.
- Variant: crossbow.
- Modifier lane: slower shots, higher single-target mark/trap payoff.
- Best first prototype: `PinningShot`, `MarkedDetonate`, `ShotCadenceBehavior`.

Gunslinger - revolver:
- Base: revolver.
- Variant: shotgun or fast repeater.
- Modifier lane: heat gain/cooling, cone spread vs precision.
- Code caution: `HeatGaugeBehavior` exists, but inspected active skill controller/files were not present.

Ronin - katana:
- Base: katana.
- Variant: nodachi or iaito.
- Modifier lane: tension timing, quickdraw distance, counter window.
- Code caution: Ronin controller/skills exist in `Assets/Scripts/Combat/Classes/Ronin`, separate from legacy skills, so use as second architecture validation after Ranger.

Shadowblade - daggers:
- Base: paired daggers.
- Variant: sickles or needle blades.
- Modifier lane: scar application, sever generation, stealth exit effects.
- Good candidate after Ranger because `ShadowbladeSkillBase` already centralizes controller access.

Brawler - fists:
- Base: wraps/fists.
- Variant: gauntlets.
- Modifier lane: stagger/knockback vs faster combo/resource return.
- Code caution: class exists in enums/default cancel windows, but full skill controller not observed.

Elementalist - caster:
- Base: catalyst focus.
- Variant: staff, orb, or wand.
- Modifier lane: active element bias, state stack generation, beam/orb behavior.
- Good later because `Elementalist_SkillController.ActiveElement` is already a strong modifier point.

Ravager - heavy:
- Base: axe/maul.
- Variant: chain axe or hammer.
- Modifier lane: mark stacks, sunder duration, heavier knockback.
- Code caution: basic `MarkPulseBehavior` exists, full active skill set not observed.

Summoner - RIFT:
- Base: rift focus.
- Variant: idol/book/totem.
- Modifier lane: summon count, duration, command pulse, rift scar synergy.
- Code caution: enum/canon only in inspected code; do not implement until class system exists.

Hexer - CURSE:
- Base: curse focus.
- Variant: hex doll/talisman.
- Modifier lane: curse spread, delayed detonation, debuff duration.
- Code caution: enum/canon only in inspected code; do not implement until class system exists.

## Build order if Phase-2 starts

1. Add `WeaponVariantDefinition` and `PlayerWeaponState`.
2. Add `HandAnchorAttach.EquipWeapon(formId)` and make runtime rewire robust after every swap.
3. Create Ranger Base/Crossbow weapon entries and a minimal debug equip path.
4. Add Ranger skill modifiers to `PinningShot` and `MarkedDetonate`.
5. Add basic attack modifier support for `ShotCadenceBehavior` through profile override or simple numeric modifiers.
6. Add draft weapon offer support as `RewardType.Weapon`.
7. Validate 8-direction weapon visuals, sort order, swing alpha, and current-facing resync.
8. Repeat with one melee class, preferably Shadowblade or Ronin, to prove the model handles non-projectile skills.
9. Only then map the same architecture to Warblade and Elementalist.
10. Defer Gunslinger/Brawler/Ravager/Summoner/Hexer weapon variants until their active skill controllers are real in code.

## TOP build-order if we did this in Phase-2

Top 3:
1. Ranger crossbow prototype: visual swap + two skill riders + basic attack cadence modifier.
2. Draft weapon reward branch: new `RewardType.Weapon`, no contamination of `SkillData`.
3. Second-class validation with Shadowblade or Ronin to prove the system is not Ranger-only.
