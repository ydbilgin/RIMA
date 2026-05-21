# Progression v2.1 Review

Input note: `memory/project_progression_canonical_lock.md` and `memory/project_rima_style_manifesto.md` were not present in the local `memory/` folder. Canonical validation used NotebookLM queries through the task-provided CLI. `13_all_acts_master_flow.png` exists locally; SHA256 `AF1BCEA0922DD6D45663780579F7584D7EDB5CB172AAF94E8CFCBE28A30AFADD`.

## Section 1 - Stay/Break/Carry Verdict

NLM query: "RIMA'da Stay/Break/Carry diye bir sistem var mi? Karar numarasi ne?"

Result: YES, but not as a numbered progression or run-start meta-track. NotebookLM identifies Stay / Break / Carry as the three post-Architect ending choices in GDD story section 16. It also states there is no dedicated `MASTER_KARAR_BELGESI.md` numbered decision for this system.

Verdict: do not formalize Stay / Break / Carry as a run-start meta-track in v2.1. The three icons on `13_all_acts_master_flow.png` may be valid ending iconography, but they are not evidence for a pre-run path selector, starting buff, or playstyle modifier.

Recommendation:
- Keep Stay / Break / Carry out of progression v2.1 mechanical systems.
- If image 13 intends those icons as run-start meta-track controls, regenerate or relabel it.
- If image 13 intends those icons as ending foreshadowing, move them visually to the Act 4 / Architect outcome area and label them as ending choices, not run modifiers.

Rejected alternatives for now:
- Path-selector: would create a new macro progression layer not supported by Karar #60/#61/#62/#63.
- Starting-buff: would drift into meta-progression power before the run begins and needs a separate balance lock.
- Playstyle-modifier: would overlap Skill Draft / Echo Imprint identity and risks becoming a hidden rune-like modifier lane.

## Section 2 - Death Imprint Mechanic Options

Canonical status: Death Imprint, formerly Echo Imprint Cascade, is a top mechanic candidate but not LOCKED. NLM confirms it requires a prototype/spec gate before implementation. Canonical record fields include `encounterId`, graph node, `currentSubRoomIndex`, `subRoomTag`, mob composition tags, and local context such as lighting. It must preserve macro encounter cadence and must not create sub-room map fragments, sub-room skill drafts, or progression-critical mid-encounter rewards.

### Option A - Pure Narrative Imprint

Mechanic: when the player dies, the game stores a high-resolution death signature and uses it only for death recap text, subtle room memory lines, and optional VFX when the player later reaches a similar place. The room may visually "remember" with a brief rift shimmer, darker crack, or one-line Ferryman/room observation, but no enemy stats, rewards, hazards, fragments, or Skill Draft odds change.

Manifesto fit: PASS. This is closest to Vivid Vulnerability: no blame, no pity, no mechanical exploitation of death. It supports the idea that the rift remembers without cloning Hades, Diablo, or StS loops.

Implementation cost: low. Pure VFX/UI/narrative hook plus persisted death signature fields. No new combat system.

Boss gate / fragment economy impact: none. Boss gate remains 8 mandatory fragments. No fragment loss, no fragment gain, no extra draft cadence.

### Option B - Persistent Room Scar

Mechanic: the exact sub-room where the player died receives a future-run scar if that macro encounter appears again or if a matching authored template is selected. Scar examples: +10% to +15% mob density, +1 authored hazard socket, lower lighting, or a minor route pressure cue. The scar is difficulty bias, not a reward source. It should apply at most once per run and expire after the player clears the affected macro encounter.

Manifesto fit: WARN. It fits "the room remembers" and Diablo-like dungeon weight, but can drift into punitive roguelite memory if the player reads death as permanent punishment. Keep it authored, visible, and capped.

Implementation cost: medium. Extend sub-room data and EncounterTemplate selection with death signature matching, scar state, visual overrides, and threat budget adjustment. Needs test coverage around macro reward cadence.

Boss gate / fragment economy impact: indirect difficulty only. No change to required 6 Combat + 2 Elite boss fragments. If scar increases hazard/mob pressure, it may make mandatory fragments harder to earn, so cap the effect and never apply it to Rest/Shop/transition safety rooms.

### Option C - Imprint Echo Drop

Mechanic: on the next run, entering the death sub-room or a close signature match spawns a ghost echo encounter shaped by the player's death context: class silhouette, weapon, one remembered attack rhythm, and the mob pressure that killed them. Killing the echo grants a Shattered Echoes bonus and clears the imprint. Recommended bonus for prototype: +25 to +40 Shattered Echoes, not a Map Fragment, Skill Draft, Relic, or boss gate resource.

Manifesto fit: PASS/WARN. Strong RIMA identity if the echo is expressive, vulnerable, and tactical. Risk: can become a Hades-style revenge shade or Diablo loot pinata if over-rewarded. Keep it sparse, readable, and emotionally framed.

Implementation cost: high. Requires ghost actor/VFX, spawn rules, death signature matching, reward UI, persistence, and balance controls. It also needs anti-farming rules.

Boss gate / fragment economy impact: Shattered Echoes only. Must not count toward boss gate, fragment reveal, Skill Draft cadence, or Relic economy. If approved, it should be a meta-currency risk/reward side interaction.

Recommendation: prototype Option A first as the safe narrative layer, then decide whether Option C earns production scope. Do not lock Option B or C until orchestrator/user approve the mechanic and economy impact.

LOCK gate: Death Imprint remains UNLOCKED. Orchestrator/user must choose A, B, C, or reject the system before any implementation spec.

## Section 3 - Act 2/3/4 Boss Reward Spec

Clarification: "All Max" on image 13 should not be treated as an Act 4 drop. The canonical final boss reward is not HP/Gold/item payout; The Architect win routes into the three ending choices. If "All Max" means drop pool tier max, it is render shorthand and should be removed or relabeled because the run is ending.

### Act 1 - Penitent Sovereign

Status from v2 FINAL: already partially specified.

Reward flow:
- WIN: Max HP +50%, 75 Gold, guaranteed Boss reward surface: Relic plus class-specific Legendary choice. Opens Secondary Class selection, +2 skill slots, and Cross-class passive layer.
- LOSE: standard death flow and Shattered Echoes earned so far. No secondary class unlock from this run.

### Act 2 - Echo Twin

Reward flow:
- WIN: Max HP +50%, 75 Gold, guaranteed Boss reward surface: Relic plus class-specific Legendary choice. Unlock Cross-Class Ultimate access for the current run. This is the point where the run becomes full dual-class build expression.
- LOSE: standard death flow and Shattered Echoes earned so far, including Act 2 reach/room/boss-attempt earnings if scoring grants them. No Cross-Class Ultimate unlock from this run.

Tier escalation: by Act 2, reward presentation may offer stronger Legendary anchors, but Relics do not need a tier label. Avoid "Epic Relic" unless a separate Relic tier decision is locked.

### Act 3 - Fracture Sovereign

Reward flow:
- WIN: Max HP +50%, 75 Gold, guaranteed Boss reward surface: Relic plus class-specific Legendary choice. Unlock Legendary tier Skill Draft upgrades after the boss.
- LOSE: standard death flow and Shattered Echoes earned so far. No Legendary tier unlock from this run.

Tier escalation: Skill Draft tiers escalate to Legendary after Act 3 boss. This is a Skill tier unlock, not a Relic tier unlock.

### Act 4 - The Architect

Reward flow:
- WIN: no HP restore and no Gold payout unless a later final-score table says otherwise. Award the locked boss-kill Shattered Echoes band (50-75; recommend upper bound for final boss if a single value is required), then route to Architect defeated meta-unlock and ending sequence. Candidate meta-unlocks: a new class, keepsake, or hub feature, but the exact unlock must be selected by orchestrator/user. Present Stay / Break / Carry as the three ending choices.
- LOSE: standard death flow and Shattered Echoes earned so far for reaching Act 4 and clearing prior content. No Architect defeated unlock and no ending choice.

Open value: do not hard-lock a new "large +X" Architect bonus beyond the canonical 50-75 boss-kill Echoes without a separate economy decision.

## Section 4 - Style Drift Audit

### Synthesis: Hades + Alabaster Dawn + Diablo

Verdict: PASS.

Evidence from v2 FINAL: "Threshold language exists to support Karar #61: Hades-style discrete room flow plus StS macro graph hybrid." This preserves Hades room readability and StS macro map planning without accepting Diablo open-world scatter. v2 also keeps Kirik Tas Tablet and act visual evolution, which supports Alabaster Dawn polish and RIMA-specific visual identity.

Fix needed: none for the four reviewed areas.

### Hades-clone room type risk

Verdict: PASS.

Evidence from v2 FINAL: "Corridor deleted. Act 1 uses 8 node types from Karar #62." It also deletes Chest and Forge as Act 1 node types. This avoids blindly cloning Hades door/reward rooms.

Fix needed: keep reward doors as readability/reference only, not a new reward-door economy.

### StS-clone reveal risk

Verdict: WARN.

Evidence from v2 FINAL: reveal odds are "1 node 65%, 2 nodes 30%, 3 nodes 5%, open node +1 hop." NLM canonical also describes Kirik Tas Tablet as the RIMA metaphor. The structure is StS-style, but the RIMA-specific tablet frame, cyan rift cracks, minimap, and act evolution keep it from being a plain StS clone.

Fix: every map/UI spec should lead with Kirik Tas Tablet art and interaction language, not "StS map" as player-facing identity.

### Diablo-clone loot risk

Verdict: WARN.

Evidence from v2 FINAL: reward catalog includes Components, Combined Items, Relics, Gold, and Shards. NLM style guidance accepts ARPG build depth but rejects equipment-slot stat bloat and open-world loot scatter. v2 protects this by removing Rune, Boss Key, Health Orb drop, and Echo Essence naming.

Fix: avoid tiered random loot framing for Relics. Use "Relic = rule-bender from Boss/rare Event" and "Combined Item = crafted/synergy item" instead of Diablo-style rarity spray.

### Death Imprint style fit

Verdict: WARN until locked.

Evidence from v2 FINAL: "Death Imprint is a proposal, not a locked system." This is correct, but the plan lacks a concrete choice for user review.

Fix: use Section 2 options as the decision gate. Option A is safest for Manifesto; Option C is strongest if production scope is accepted.

### Stay/Break/Carry placement

Verdict: FAIL if interpreted as a run-start meta-track; PASS if interpreted as final ending iconography.

Evidence: NLM says Stay / Break / Carry are post-Architect ending choices with no numbered progression decision. v2 FINAL does not mention them.

Fix: do not add them to progression mechanics. Relabel/regenerate image 13 if the bottom band implies pre-run modifiers.

### Combined Item placeholder names

Verdict: WARN/FAIL depending on source.

Evidence from v2 FINAL: "Combined Items C01-C09... Names/recipes are not enumerated in this task block; reserve 9 canonical slots without inventing names." That is a PASS because it avoids inventing names. NLM style/economy source lists canonical combined item names: Vampiric Blade, Phantom Weave, Frenzy Core, Warlord's Plate, Rift Piercer, Soul Tap, Fracture Amp, Ghost Step, Iron Will.

If placeholders like Iron Veil / Cursebound Coil are being considered, verdict is FAIL for current lock because they are generic fantasy and not the NLM-listed canonical combined item set. Replace them with the canonical 9 names above or keep neutral C01-C09 placeholders until orchestrator confirms an overwrite.

## Section 5 - v2.1 Patch Diff

Patch 1 - Stay/Break/Carry:
- BEFORE: v2 FINAL has no Stay / Break / Carry section; image 13 bottom-band icons could be misread as progression mechanics.
- AFTER: add note: "Stay / Break / Carry are Architect ending choices, not run-start meta-track mechanics. Do not implement as path selector, starting buff, or playstyle modifier. If image 13 implies that, regenerate or relabel."

Patch 2 - Death Imprint:
- BEFORE: "Death Imprint is a proposal, not a locked system... Spec gate pending."
- AFTER: keep UNLOCKED status and add three options: A pure narrative imprint, B persistent room scar, C imprint echo drop. Recommendation: prototype A first; consider C after scope approval; do not ship B/C without economy and difficulty approval.

Patch 3 - Act boss rewards:
- BEFORE: Act 1 node N12 says "Boss reward: Relic plus Boss HP chance 50%." Act 2/3/4 reward flows are absent.
- AFTER: replace "Boss HP chance 50%" with "Max HP +50% on Act 1/2/3 boss win." Add Act 2 Echo Twin: HP +50%, 75 Gold, Relic + class Legendary choice, Cross-Class Ultimate unlock. Add Act 3 Fracture Sovereign: HP +50%, 75 Gold, Relic + class Legendary choice, Legendary Skill Draft tier unlock. Add Act 4 Architect: no HP/Gold drop; boss-kill Shattered Echoes band 50-75; Architect defeated meta-unlock gate; Stay/Break/Carry ending choice; lose = standard death and earned Echoes only.

Patch 4 - "All Max":
- BEFORE: image 13 Act 4 row says "All Max", creating ambiguity about a final drop that cannot be used in the same run.
- AFTER: treat "All Max" as render shorthand only. Remove it from mechanical plan unless it is redefined as "all run systems have reached maximum availability before final boss."

Patch 5 - Style drift:
- BEFORE: v2 FINAL is mostly aligned but lacks explicit style drift audit for the four new problems.
- AFTER: add PASS/WARN/FAIL audit: synthesis PASS, room type PASS, map reveal WARN, loot framing WARN, Death Imprint WARN, Stay/Break/Carry FAIL if run-start mechanic, Combined Item placeholder names FAIL if non-canonical names replace C01-C09.

## Section 6 - Open Questions (orchestrator/user gate)

1. Stay/Break/Carry image handling: should image 13 be regenerated, or should those icons be moved/relabelled as Architect ending choices?
2. Death Imprint decision: choose A pure narrative, B persistent room scar, C imprint echo drop, or reject/defer.
3. Architect win meta-unlock: pick one concrete unlock category: new class, keepsake, hub feature, or story-only ending unlock.
4. Architect Shattered Echoes: use canonical boss-kill band 50-75 only, or approve an extra final-clear bonus value.
5. Combined Items: keep v2 neutral C01-C09 placeholders, or replace them now with NLM-listed canonical names.
