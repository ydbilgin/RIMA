# SKILL OFFER SYSTEM DECISION
Date: 2026-05-03
Status: LOCKED FOR PHASE 1 SYSTEM DESIGN
Source: `STAGING/SKILL_SYSTEM_ANALYSIS_PROMPT_NOTES.md` + existing S43/R4 locks

## Decision

RIMA should not use a visible skill tree screen for run progression.

The production model is:

1. Room reward offers 3 choices.
2. Choices can be a new active skill, an upgrade for an owned active skill, a passive/echo, or a tag synergy.
3. The core identity of each skill stays stable.
4. Common/Rare upgrades mostly add modifiers.
5. Epic upgrades can add a side mechanic.
6. Legendary upgrades can rarely change behavior.

Last Epoch influence is accepted only as skill-depth logic. It does not become a Last Epoch skill tree UI.

---

## Active Slot Rule

Every active skill must pass the 6-line test in `TASARIM/GLOBAL_REPEAT_RULES.md`.

Short version:

`If a skill only adds damage, only adds a buff, or only improves a future cast, it should become a passive/echo/upgrade offer unless it creates or consumes a readable combat state.`

Valid active skills must do at least one of these:

- apply a class-owned state
- consume a state
- create a positional/zone question
- move the player in a class-owned way
- answer a specific encounter problem
- create a resource-risk decision that is visible in combat

---

## Active To Passive / Upgrade Candidates

These are design-routing decisions. They do not delete the fantasy; they change where the fantasy lives in the Hades-style offer model.

| Class | Candidate | Decision | Reason |
|---|---|---|---|
| Warblade | Iron Crush | Convert to Rare/Epic upgrade unless redesigned to apply Broken/Sundered visibly | Current text is mostly self damage buff. |
| Warblade | Ironclad Momentum | Convert to defensive echo/passive | Strong as "after Earthsplitter, gain mitigation" offer; weak as active slot. |
| Warblade | Battle Surge | Convert to Rage-spend sustain boon | Healing-on-spend is a passive rule, not a button. |
| Elementalist | Radiant Pillar | Convert to Lightbreak echo upgrade | Aura buff can be an offer attached to Lightbreak / Fire-Frost rhythm. |
| Elementalist | Element Charge | Convert to Fire-line upgrade | "Fire spells instant, mana x2" is a modifier rule, not a distinct cast. |
| Shadowblade | Night Aperture | Convert to Legendary Phase/Scar upgrade | It modifies dash/Flicker behavior; no standalone encounter answer. |
| Shadowblade | Shadow Clone | Defer or convert to passive echo | Decoy AI and extra actor cost are high; risks Summoner overlap. |
| Ranger | Hunter's Step | Keep only if it remains the single build movement skill; otherwise convert to Tactical Roll upgrade | Ranger identity is trap/mark discipline, not mobility stacking. |
| Ravager | Death Wish | Convert to Legendary survival/frenzy offer or V-linked mode | Pure "cannot die" buff is mandatory if active and hard to balance. |
| Ronin | Iai Pressure | Convert to Iaido Stance upgrade | It only lets dash carry Quickdraw bonuses. |
| Gunslinger | Hot Lead | Convert to ammo passive/Heat boon | Bullet DoT rule is an offer, not a button. |
| Brawler | Unstoppable Force | Convert to Legendary Overdrive upgrade | It duplicates V fantasy and reads as aura mode. |
| Summoner | Dark Pact | Convert to sacrifice-economy passive | HP-for-summon bypass is an economy rule, not an active slot. |
| Summoner | Lich Form | Convert to V/Legendary mode, not normal active | Form buff competes with class burst and is visually expensive. |
| Hexer | Whisper Mark | Keep passive only | Already correct as passive auto-spread; must obey auto-spread exclusivity. |
| Hexer | Bleed Tax | Keep active only if HP cost creates visible Hex pressure and risk; otherwise passive/echo | HP-pay flat stack apply is borderline. |

Do not mass-edit the canonical skill tables until the first Hades-offer prototype exists. The table above is the routing source for future contract rewrites.

---

## Keep As Active

These remain active because they create clear combat events, state gates, or encounter questions:

- Warblade: Iron Charge, Gravity Cleave, Sunder Mark, Earthsplitter, Iron Counter, Deep Wound, Death Blow
- Elementalist: Fireball, Glacial Spike, Living Bomb, Blink, Frozen Orb, Prism Beam, Meteor, Frost Wall, Solar Flare, Blizzard
- Shadowblade: Phase Step, Backstab Mark, Death Mark, Veil Burst, Scarbinding/Severance/Scar Collapse route, Smoke Veil if it has a clear smoke state
- Ranger: Pinning Shot, Marked Detonate, Bone Trap, Sweep Volley, Predator's Mark, Wireline Trap, Final Strike
- Ravager: Bloodlust Strike, Carnage Spin, Frenzied Leap, Wild Hack, Gnash, Bloodied Roar, Barbaric Charge, Choke Throw, Bone Crack
- Ronin: Quickdraw Slash, Soken-giri, Iaido Stance, Counter Draw, Sakura Veil, Crescent Arc, Flash Draw, Void Cleave
- Gunslinger: Rift Dash, Quickdraw, Cursor Storm, Deadshot, Smoke Grenade, Fan the Hammer, Suppression Fire, Rift Grenade, Ricochet, Reload Dance, Point Blank Execute
- Brawler: Bully, Crackjaw, Cheap Shot, Shove Off, Counter Blow, Aerial Rave, Cyclone Drive, Curbstomp, Kidney Hook
- Summoner: Raise Skeleton, Summon Golem, Command Beacon, Corpse Explosion, Death Nova, Commanding Strike, Blood for Power, Bone Shield, Soul Siphon Totem, Mass Sacrifice
- Hexer: Corruption, Agony, Pandemic, Hexblast, Spitback, Haunt, Unstable Affliction, Enervate, Foul Wave, Hex Overload, Blight Sigil

Borderline active skills must be contract-gated before PixelLab work.

---

## PixelLab Production Rule

`RIMA_skill_sheets/*.png` are concept references only.

Correct production split:

- caster animation sheet
- projectile or slash VFX sheet
- ground decal / zone / trap object
- impact VFX
- state pip / overlay
- Unity-side hit-stop, camera shake, flash, knockback, slide

Forbidden production requests:

- character + VFX + enemy + background baked into one animation sheet
- per-skill bespoke enemy reaction animation
- grapple, ragdoll, struggle, boss-specific reaction animation
- direct crop from concept sheet into Unity
- accepting sheet skill names as canonical without the skill doc

---

## PixelLab Feasibility Tiers

| Tier | Meaning | Examples |
|---|---|---|
| Green | Short caster pose + separate VFX/decal; production-friendly | Fireball, Pinning Shot, Sunder Mark, Hexblast, Quickdraw, Bully |
| Yellow | Needs separate state objects or careful Unity support | Scar placement/collapse, Wireline Trap, Frost Wall, Command Beacon, Reload Dance, Bone Shield |
| Red | Too cinematic or high engineering/art scope now | Choke Throw, Wall Eat, Shadow Clone, Lich Form, screen-wide multi-teleport chains, bespoke boss reactions |

Red does not mean "delete." It means defer, convert to upgrade, or rebuild as code-driven VFX + slide + existing hit-react.

---

## Too Cinematic / Simplify Before Contract

These need simplification before production prompts:

| Skill / Family | Problem | Production-safe version |
|---|---|---|
| Ravager Choke Throw | grab/hold/throw implies custom enemy animation | short pull-in VFX + enemy slide + generic stagger; no struggle frames |
| Brawler Aerial Rave / Wall Eat | juggle/wall slam can imply bespoke mob animation and collision risk | launch decal + vertical hit VFX; wall case uses nav-safe slide and no-wall fallback |
| Shadowblade Veil Burst / Flash Draw style chains | multi-target teleport can become cinematic panel sequence | 3-4 readable attack peaks + target pips; no full-screen cutscene |
| Summoner Lich Form / Bone Tide | form swap and swarm count are asset/AI heavy | V-linked visual overlay or passive summon burst; cap entity count |
| Elementalist Blizzard / Meteor | can become screen-covering illustration | zone decal + tick particles + short cast pose |
| Gunslinger Cursor Storm | can hide readability in bullet rain | cursor zone reticle + sparse muzzle frames + pooled impact pips |

---

## Class Consistency Verdict

The 10 class verb system is still strong:

- Warblade: engage / break / execute
- Ranger: mark / trap / detonate
- Shadowblade: phase / scar / collapse
- Hexer: stack / spread / blast
- Elementalist: switch / shape / detonate
- Brawler: weave / combo / launch
- Ravager: suffer / trade / frenzy
- Ronin: wait / draw / punish
- Gunslinger: slide / shoot / reload
- Summoner: command / sacrifice / raise

Overlap risks to keep watching:

- Warblade / Brawler / Ravager: all physical impact; separate by armor break vs body crack vs blood/frenzy.
- Shadowblade / Hexer / Summoner: all dark VFX; separate by scar geometry vs stack pips vs minion/corpse bodies.
- Ranger / Gunslinger: both ranged; separate by trap/mark planning vs heat/reload movement.
- Ronin / Shadowblade: both burst reposition; Ronin must be draw timing, Shadowblade must be phase-through scar geometry.

---

## Hades-Style Offer Pool

Offer categories:

| Category | Purpose | Example |
|---|---|---|
| New Active | Gives a new button if player has empty or replaceable slot | Iron Charge, Fireball, Bone Trap |
| Skill Upgrade | Modifies an owned active skill | Fireball burns longer; Bone Trap applies Marked |
| Passive/Echo | Adds a rule to future actions | Rage spend heals after internal CD |
| Tag Synergy | Bridges primary and secondary class tags | Marked target gains Hex stack on detonation |
| Resource Mod | Alters class resource pacing | Rage gain on Broken target, Heat vent on reload |
| Risk Offer | Adds power with cost | HP cost for Fury/Charge, boss-room curse |

Offer weighting for Phase 1 prototype:

- 45% owned active skill upgrade
- 25% new active or replace active
- 15% passive/echo
- 10% tag synergy
- 5% risk offer

Rules:

- Always offer at least one choice that upgrades an owned skill after room 2.
- Never offer more than one new active skill in the same reward unless the player has fewer than 2 actives.
- Legendary behavior changes require the player to already own the base skill.
- Auto-spread effects obey existing Hexer exclusivity.
- Build cap still applies: max 1 skill movement per build.
- A run should not require reading a tree; the offer text must state the exact behavior change.

---

## Minimum Tag Set

Start with a small ASCII tag set. Do not expose all tags to players at once.

### Shape Tags

- `melee`
- `projectile`
- `line`
- `cone`
- `zone`
- `dash`
- `counter`
- `summon`

### Combat Role Tags

- `opener`
- `control`
- `detonate`
- `execute`
- `sustain`
- `resource`
- `defense`

### State Tags

Use the public states from `CLASS_STATE_CONTRACT.md` first:

- `broken`
- `sundered`
- `burning`
- `frozen`
- `scar`
- `marked`
- `trapped`
- `wounded`
- `opened`
- `suppressed`
- `cracked`
- `corpse`
- `hexed`

Phase 1 minimum for implementation can start smaller:

- `broken`
- `sundered`
- `marked`
- `trapped`
- `burning`
- `frozen`
- `scar`
- `cracked`

---

## Implementation Order

1. Phase 1: Warblade + core offer framework.
2. Add Elementalist/Ranger/Shadowblade upgrades because they are Core 4.
3. Only then expand to Brawler/Ravager/Ronin/Gunslinger/Summoner/Hexer.
4. Red-tier cinematic skills remain out of sprite production until their visual contract passes.

This keeps the current production priority intact while giving future class passes a single routing rule.
