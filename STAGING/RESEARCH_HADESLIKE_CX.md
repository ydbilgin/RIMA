# Research: Hades-likes -> RIMA adaptation ideas - CX feasibility / map-structure lens

Scope: ranked by ROI for RIMA's actual Phase-1 spine: `RIMA.Systems.Map.RoomLoader`, `RoomSequenceData`, `DraftManager`, `Gate`, `MapFragment`, and `PenitentSovereign`. LOCKED direction preserved: cursor-aim, floating-island rooms, PixelLab sprites, cyan-sparing, painterly slash VFX.

## Ranked Ideas

1. `[Hades 1] -> Door reward previews -> RIMA mapping on RoomSequenceData + Gate tint/icon: add rewardKind/rewardPreview fields to each sequence entry, render a small icon/tint on Gate before unlock, and pass that room's reward context into DraftManager after fragment pickup -> Impl cost: S; add enum + serialized fields, Gate visual setter, one RoomLoader call when gate is built -> theme fit: strong; the gate becomes a readable "seal threshold" without changing the linear demo -> conflict flags: none; keep cyan sparse and use category colors/icons instead of permanent cyan glow.`

2. `[Hades 1] -> Room-exit boon choice -> RIMA mapping on existing MapFragment pickup -> DraftManager.ShowDraft() -> Gate.Unlock() chain in RoomLoader -> Impl cost: S; mostly content/tuning: curate offers by upcoming room and display "why this helps next" on draft cards -> theme fit: strong; the fragment becomes the act of reading the next broken map piece before choosing a technique -> conflict flags: none; do not add a separate pickup currency for demo.`

3. `[Dead Cells] -> Biome branching, compressed -> RIMA mapping on RoomLoader sequence index: offer one optional alternate next room at rooms 2/3 by swapping the next RoomSequenceData before LoadNext -> Impl cost: M; add alternateRoom field, choice UI after draft, and a guarded RoomLoader.SetNextRoomOverride(index, data) -> theme fit: strong if framed as choosing which floating island fragment to restore -> conflict flags: keep demo to one branch choice max; full node-map would overrun the 10-min flow.`

4. `[Hades 1/2] -> Mini-boss doors -> RIMA mapping on RoomSequenceData.mobSpawns.isElite + Gate reward preview: mark a room as Elite/MiniBoss, spawn one elite-heavy encounter, and guarantee rare/epic draft weighting after clear -> Impl cost: S-M; add isEliteRoom/rewardTierHint fields and feed room context into SkillOfferGenerator -> theme fit: strong; "guarded fragment" before the boss sells escalation -> conflict flags: none, but avoid adding a new boss prefab for demo.`

5. `[Curse of the Dead Gods] -> Room blessing/risk offer -> RIMA mapping on DraftManager reward offers: add one "Burdened" draft option that grants higher-tier skill/passive now and applies a simple next-room modifier through RoomLoader.CurrentRoomData -> Impl cost: M; add RewardOffer risk metadata, one RoomModifier component, and UI copy -> theme fit: strong; accepting a crack in the seal for power is RIMA-native -> conflict flags: keep as room-local modifier, not a permanent corruption system yet.`

6. `[Hades 1] -> Chaos gate -> RIMA mapping on MapFragment/Gate: rare fragment pickup can spawn a side "Rift Gate" choice before the normal gate unlocks; accept a penalty for the next room, then receive boosted draft weighting after that room clears -> Impl cost: M; add optional secondary Gate prefab/state, temporary RunModifier, and DraftManager reward boost -> theme fit: very strong; it is literally stepping through a cyan reality fracture -> conflict flags: cyan-sparing risk; use one short-lived rift effect only.`

7. `[Dead Cells] -> Cursed chest -> RIMA mapping on reward rooms: an interactable focalElementPrefab offers guaranteed epic draft but marks player "Oathbound"; one hit before next clear removes bonus and deals/inflicts penalty -> Impl cost: M; add CursedRelic interactable, player flag, Health damage listener, DraftManager boost -> theme fit: strong as a seal oath or forbidden fragment -> conflict flags: avoid one-hit death for demo; use "lose bonus + chip damage" instead.`

8. `[Risk of Rain 2] -> Time pressure scaling, localized -> RIMA mapping on RoomSequenceData.expectedDuration: if clear time exceeds expectedDuration, next room gets one extra elite affix or slightly reduced draft rarity; if under, add small gold/heal chance -> Impl cost: M; track room timer in RoomLoader, apply existing EliteAffix or reward weighting -> theme fit: moderate; the unstable islands punish hesitation -> conflict flags: do not implement global RoR2 clock; that conflicts with deliberate Sundered Beat pacing.`

9. `[Returnal] -> Room modifiers/malignancy -> RIMA mapping on RoomSequenceData fields: add modifier tags like "rift-wind", "fractured-floor", "seal-dim" that RoomLoader applies via a small RoomModifierRunner on load -> Impl cost: M; data field + component that toggles hazards/decor/focal element behavior -> theme fit: strong; every island can have one legible instability rule -> conflict flags: must not hurt readability of cursor-aim combat or painterly slash VFX.`

10. `[Hades 2] -> Guardian / boss route telegraph -> RIMA mapping on final two RoomSequenceData entries: Gate and HUD preview "Sovereign approach" with red/boss category and an authored pre-boss reward room -> Impl cost: S; set room category/status text and one guaranteed draft/heal/gold offer before boss -> theme fit: strong; makes Penitent Sovereign feel approached, not merely loaded -> conflict flags: no rest-room fountain; reward should still be draft/economy based.`

11. `[Wizard of Legend] -> Arcana shop -> RIMA mapping on isRewardRoom: replace passive 2-second reward trigger with a compact shop/ferryman focal element offering 3 purchases: skill draft, heal, gold-to-reroll -> Impl cost: M; PlayerEconomy already exists in DraftManager reward handling, need shop UI/interactable and reward room branch -> theme fit: moderate-strong if the vendor is a quiet seal-keeper, not a flashy merchant -> conflict flags: no inventory grid; keep purchases draft-loop adjacent.`

12. `[Enter the Gungeon] -> Curse/reward doors -> RIMA mapping on Gate.State and Gate tint: some gates are "Oath Gates" with visible warning icon; entering applies one next-room drawback and increases fragment/draft reward -> Impl cost: M; Gate metadata + RunModifier + DraftManager reward boost -> theme fit: strong; the gate itself becomes the promise and the wound -> conflict flags: avoid too many gate variants in the demo; one Oath Gate is enough.`

13. `[Hades 1] -> Well of Charon / temporary buffs -> RIMA mapping on reward rooms and DraftManager fallback offers: add temporary one-room buffs such as faster dash cooldown, higher guard damage, or first EXECUTE heal -> Impl cost: S-M; add timed/run-scoped passive components and offer type -> theme fit: moderate; "seal echoes" can be transient blessings -> conflict flags: keep names/visuals RIMA-native, not shop-item clutter.`

14. `[Dead Cells] -> Scroll stat choice -> RIMA mapping on DraftManager: occasional non-skill draft card upgrades one axis for the next two rooms: Guard Break, Execute Damage, or Survival -> Impl cost: S; implement as passive RewardOffer variants and attach short-lived stat modifiers to player -> theme fit: moderate; simple readable build direction between rooms -> conflict flags: do not create permanent attribute spreadsheet or classless stat soup.`

15. `[Cinderia] -> Route reward identity / room blessing -> RIMA mapping on RoomSequenceData.cliffPatternKey + focalElementPrefab: pair room layout motif with reward identity, e.g. "broken altar" rooms bias passives, "chain bridge" rooms bias active skills -> Impl cost: S; data tagging and SkillOfferGenerator weighting -> theme fit: strong; floating-island silhouettes teach reward expectations visually -> conflict flags: verify against locked room art direction; no generic biome sprawl.`

16. `[Hades 1] -> Pact/Heat, deferred -> RIMA mapping after demo: run-start modifiers selecting extra elite rooms, harsher fragment pickup rules, or stronger PenitentSovereign phase thresholds -> Impl cost: L; needs run config UI, save data, balancing, and tests across RoomLoader/DraftManager/Boss -> theme fit: strong later as "seal strain" -> conflict flags: not for the 10-min demo; would distract from proving the base loop.`

## TOP 5 by ROI for the 10-min demo

1. Hades 1 door reward previews on `Gate` + `RoomSequenceData` (S): highest map-structure clarity for lowest code cost.
2. Hades 1 room-exit draft framing on the existing `MapFragment -> DraftManager -> Gate` loop (S): strengthens the core loop without new systems.
3. Hades/Dead Cells mini-boss room flag using `mobSpawns.isElite` and draft reward weighting (S-M): adds a memorable spike before `PenitentSovereign`.
4. Hades 2 boss approach telegraph through the final `RoomSequenceData`/Gate/HUD beat (S): improves run flow and boss anticipation cheaply.
5. Cinderia-style room reward identity tags using `cliffPatternKey`/focal elements and `SkillOfferGenerator` weighting (S): makes floating islands teach route choices visually.

Avoid for demo: full node-map branching, global RoR2 time scaling, permanent corruption meters, inventory/grid systems, and broad Heat/Pact UI. They are either larger than the current spine or conflict with RIMA's deliberate Sundered Beat pacing.
