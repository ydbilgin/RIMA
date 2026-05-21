# v2.2 Gate Resolution Review

Review basis:
- Read: PROGRESSION_PLAN_v2_2_LOCK.md, GATE_RESOLUTIONS_v2_2_PROPOSAL.md, PROGRESSION_PLAN_v2_1_REVIEW.md.
- Queried NotebookLM with the task notebook id for Forge, Echo Imprint, Stay/Break/Carry, hub spend, Karar #60-63, style manifesto, cross-class proc, and Curse Gate.
- Targeted local checks: TASARIM/MAP_ITEM_SYSTEM.md, TASARIM/ROOM_MECHANICS.md, TASARIM/GDD.md, TASARIM/map_fragment_system.md, TASARIM/CROSS_CLASS_PROC_SYSTEM.md, TASARIM/SUBROOM_TEMPLATES_ACT1.md.
- Local files missing: ANTIGRAVITY.md, memory/project_progression_canonical_lock.md, memory/project_rima_style_manifesto.md.

## Section 1 - Per-Gate Verdicts

### Gate 1 - Forge Node Conflict

Verdict: DISAGREE with pick (d) as written.

Reason: TASARIM/MAP_ITEM_SYSTEM.md is explicit that Forge is a guaranteed destiny node around node 4, and its room type table names Forge as a destiny node with 3 tabs and 1 main action per visit. The same file says Merchant Anvil is rare and expensive, a safety valve when the player cannot reach Forge, not the main Act 1 craft path. NLM returned the same: Node ~4 guaranteed Forge; Anvil prevents dead-end, not planned economy.

Canonical evidence:
- TASARIM/MAP_ITEM_SYSTEM.md: "Node ~4: Forge Room (guaranteed, sabit)."
- TASARIM/MAP_ITEM_SYSTEM.md: "Anvil etkilesimi (rare, pahali): Forge'a gitmeden 1 item combine yap."
- TASARIM/MAP_ITEM_SYSTEM.md: "Pahali tutulur ki 'her zaman Anvil git' olmasin."
- Karar #62 / v2.2 Act 1 topology removes Forge from Act 1, so this is a real canonical conflict.

Player experience check: Act 1 without a reliable Forge makes Combined Items almost invisible before the Act 1 boss. With 6 Combat at 20 percent component drop plus 2 Elite component choices, Act 1 can teach component collection, but not reliably teach combine decisions. A single Shop Anvil with rare/expensive availability cannot carry that teaching role.

Alternative:
- If Karar #62 topology is higher priority: lock "Act 1 no dedicated Forge, no guaranteed Combined craft; components are collection pressure only; rare Merchant Anvil remains safety valve, not main path." Move full Forge tutorial to Act 2 first guaranteed Forge node.
- If MAP_ITEM_SYSTEM is higher priority: reopen Karar #62 and replace one non-combat Act 1 node with Forge. This is cleaner mechanically, but violates the stated 15-node LOCK.

Recommended patch wording: "Act 1 Combined crafting is intentionally non-core. Merchant may rarely sell a Combined item or rare Anvil use. Full Forge destiny node starts Act 2 unless user reopens Act 1 topology."

### Gate 2 - Echo Imprint Trigger Math

Verdict: PARTIAL AGREE.

Reason: The orchestrator is right that the math needs one Imprint slot per act, but the canonical rule is not simply "after first Elite of each Act." NLM and TASARIM/ROOM_MECHANICS.md state: every 3 combat rooms, normal Skill Draft'a EK, max 4/run, act basina 1 slot. In Act 1, the third combat can align with the first Elite under the fixed topology, so "first Elite" is a good Act 1 shorthand. Replacing the canonical trigger globally risks breaking later acts where first Elite may not equal third combat.

Canonical evidence:
- TASARIM/ROOM_MECHANICS.md: "Her 3 combat odada bir kez, normal Skill Draft'a EK olarak sunulur."
- TASARIM/ROOM_MECHANICS.md: "Max 4 / run (act basina 1 slot acilir)."
- GDD repeats the same "Her 3 combat oda" and "Run basina max 4 Imprint" language.

Player experience check: A visible "first Elite always means Imprint" rule is readable, but too rigid if applied to every act. The canonical combat-count trigger is more systemic and still predictable enough.

Alternative:
- Keep canonical trigger: "After every third Combat/Elite clear, offer Echo Imprint in addition to normal reward, only if the current act's Imprint slot is unused."
- Add Act 1 implementation note: "In the locked Act 1 route this resolves at first Elite."

### Gate 3 - Architect Meta-Unlock

Verdict: PARTIAL AGREE.

Reason: Story-only first kill fits Phase 1 scope, but "Stay/Break/Carry are only narrative" is not fully supported by GDD/NLM. NLM reports GDD section 16 frames Stay/Break/Carry as post-Architect endings and says Break has a mechanical save-file consequence: certain Hub NPCs are permanently removed. So Phase 1 can defer mechanical consequences, but the LOCK should not state that the endings are purely narrative unless it explicitly overrides/de-scopes GDD section 16.

Canonical evidence:
- GDD/NLM: Architect defeat returns to Hub, then presents three endings.
- NLM: Stay = melancholic peace, Carry = roguelite loop return, Break = real risk/loss with permanent Hub NPC removal.
- v2.2 already has Architect win reward as Shattered Echoes 50-75 plus ending.

Motivation check: Phase 1 can survive with first-kill ending access plus 50-75 Shattered Echoes. Adding a new class/keepsake now is scope expansion. Adding a repeat +X Echoes bonus is also not currently canonical beyond the existing 50-75 final boss band.

Alternative:
- Phase 1: first Architect kill unlocks ending choice presentation; no new permanent power system.
- Record explicitly: "GDD ending consequences, especially Break removing Hub NPCs, are deferred/blocked for user signoff."
- Do not add a new repeat-clear Echo bonus unless economy is rebalanced.

### Gate 4 - Hub Spend Catalog

Verdict: DISAGREE with pick (e) as written.

Reason: NLM found a canonical hub spend catalog already exists, and it does not match the proposal. Canonical spend includes class unlocks, Vrel Augment Craft at 50 Echoes, and Cartographer map QoL at 100 Echoes. Canonical starting class is Warblade only, not 4 starting classes. Canonical class unlock costs are mostly 80-250 Echoes with milestone alternatives, not 100/150/200/250/300/400 sequential.

Canonical evidence:
- NLM: Shattered Echoes are hub meta currency for class unlocks, small permanent upgrades, and map upgrades.
- NLM: Warblade starts unlocked.
- NLM: Elementalist 80 Echoes, Ranger 80, Shadowblade 150 or Act 1 three clears, Ravager/Ronin 150 with milestone routes, Gunslinger/Brawler/Summoner around 200 with milestone routes, Hexer 250 plus Elementalist run condition. NLM had one internal source conflict on Brawler/Hexer exact costs, so this needs final canonical tie-break, but it is still not the orchestrator's 4-start/100-400 plan.

Pace check: With v2.2 earn estimate around 30-40 Echoes for a strong run, 80 Echoes is about 2-3 good runs; 150 is 4-5; 250 is 7-9. That is acceptable. The proposed 400 final class is likely too slow for Phase 1 unless run-end kill Echoes are high and clearly shown.

Alternative:
- Use canonical Phase 1 minimal hub: Warblade start; class unlock table from NLM/canonical source; postpone Vrel/Cartographer if scope is too high, but do not delete them silently.
- If the user wants 4 starting classes, mark it as an explicit override of current canon and rebalance unlock costs downward/upward after that decision.

### Gate 5 - Image 13 Handling

Verdict: PARTIAL AGREE.

Reason: Relabel/no-regen is the right low-cost first move, but the exact placement needs care. Local image check confirmed STAGING/concepts/overnight/13_all_acts_master_flow.png is 1024x1536, SHA256 AF1BCEA0922DD6D45663780579F7584D7EDB5CB172AAF94E8CFCBE28A30AFADD. The bottom Stay/Break/Carry band is visually strong enough to read as a separate meta-track. The Act 4 row is already crowded on the right by the Act 4 info card and Tier IV card, so moving three full ending tiles into the row will likely reduce legibility.

Evidence:
- CurrentStatus names image 13 publish-ready.
- Local concept folders checked: compact_sheets has 4 pngs, threshold_gallery has one showcase png, overnight has 13_all_acts_master_flow.png plus related concepts. Text search found S/B/C references in task/docs, not other known render specs. Pixel text inside images was not OCR-reviewed.
- Visual inspection: Act 4 row has room for a small "Ending Choice" label near/below the Architect outcome, not for the full bottom-band tile treatment.

Alternative:
- First edit: delete bottom band or reduce it to one compact "Ending Choice: Stay / Break / Carry" strip directly under the Architect boss card/arrow.
- If manual edit quality is low, regenerate image 13 with the corrected semantics.

## Section 2 - Additional Logic Conflicts

### 1. Skill slot 4 to 6 progression

Severity: WARN.

Issue: Act 1 starts with 4 active slots and primary-only draft. The two future slots should be visible-locked only, not valid offer targets. If the draft offers a new skill after 4 slots are full, the player could receive an offer they cannot place.

Evidence: GDD says Act 1 has 4 active slots, only primary class skills; Act 1 boss adds +2 slots for secondary. ROOM_MECHANICS says all slots full shifts weights to 10 percent New Skill and 70 percent Tier Upgrade.

Fix: In Act 1, once 4 active slots are full, "New Skill" offers require replace/skip rules or are suppressed unless they can replace an owned skill. Locked slots display "opens after Act 1 boss" and are not draft destinations.

### 2. Cross-class proc Act 1

Severity: INFO.

Issue: CROSS_CLASS_PROC_SYSTEM.md is LIVE/LOCKED, but Act 1 has no secondary class. Proc cannot run in Act 1 unless a future Legendary carrier creates a cross-family tag, which is not Act 1 baseline.

Evidence: NLM says Act 1 is single-class; Act 1 boss secondary pick activates cross-class passives and Act 2 draft pools.

Fix: Add implementation guard: CrossClassProcManager disabled until secondary class selected. Act 1 tooltips can show "locked until second face recovered."

### 3. Pity system Act 1 math

Severity: WARN.

Issue: v2.2 says Act 1 has 8 fragment-bearing nodes, implying 8 Skill Drafts. ROOM_MECHANICS says Combat or Elite opens draft, but also says Elite opens a separate Elite Reward "normal Draft yerine." If Elite does not count as normal draft, Act 1 has 6 normal drafts, and "absent 5 consecutive drafts" can trigger only at the end. If Elite counts, pity cadence is different.

Fix: Define pity counter source. Recommended: normal 3-choice Skill Draft only; Elite Reward does not count. Act 1 pity can trigger on the 6th normal draft at most.

### 4. Mystery branch fragment economy

Severity: WARN.

Issue: Bonus Mystery fragment can push total pickups beyond the 8 mandatory boss quota. A HUD that only says "X / 8" becomes unclear at 9/8 or 10/8.

Evidence: Karar #63 and map_fragment_system.md define 8 mandatory fragments and optional bonus fragments from branch nodes.

Fix: Use "Mandatory 8/8" plus a small "+1 bonus reveal" badge, or cap the main counter at 8/8 and track bonus fragments separately. Do not show 9/8 as if boss gate required overflow.

### 5. Curse Gate reward path

Severity: WARN.

Issue: v2.2 says Curse Gate has no fragment and reward is risk/reward, while map_fragment_system.md says Curse Gate gives optional fragment if Burden is accepted. ROOM_MECHANICS says reject gives Max HP 5 percent restore; MAP_ITEM_SYSTEM says Curse can be Legendary shortcut using 3 bare components with 60/40 risk.

Fix: Choose one Phase 1 rule. Recommended narrow rule: Curse Gate has no mandatory boss quota fragment; accepting Burden may grant either Gift/Legendary shortcut or optional bonus reveal, but not both by default. Reject gives +5 percent HP only.

### 6. Combined Item Forge cost and Anvil pricing

Severity: WARN.

Issue: Forge crafting costs no Gold; the cost is one main action per Forge visit. Anvil is rare and expensive. If Act 1 has no Forge, Anvil cannot become the main combine path without contradicting its safety-valve role.

Fix: Write this explicitly: "Forge combine = no gold, 1 main action. Merchant Anvil = gold cost + rare availability + same 1 combine action." If Act 1 no Forge, Combined crafting is intentionally rare in Act 1.

### 7. Boss Legendary 3-choice pool

Severity: INFO.

Issue: v2.2 says "class-specific Legendary 3-choice." MAP_ITEM_SYSTEM says each class has 3 Legendary options. That implies the boss presents the class's three anchors, not a random 3 from a larger pool.

Fix: Patch wording to: "Boss offers the current primary class's 3 canonical Legendary anchors." If future classes gain more than 3, then add randomization then.

## Section 3 - Filozofi Compliance Audit

Gate 1 Forge:
- Current pick: FAIL/WARN. It keeps scope narrow but turns a rare safety valve into main Act 1 economy. That is hidden complexity and weak teaching.
- Best fit: Act 1 no Forge/no core combine, Act 2 full Forge. Clear, narrow, honest.

Gate 2 Echo:
- Current pick: PASS if written as Act 1 shorthand; WARN if replacing canonical combat-count system globally.
- Best fit: every 3 combat/elite clears with per-act slot cap. Simple counter, no extra subsystem.

Gate 3 Architect:
- Current pick: PASS for scope, WARN for canonical accuracy. Story-first is narrow; denying GDD mechanical ending consequences creates future contradiction.
- Best fit: Phase 1 ending presentation only, with GDD consequences deferred.

Gate 4 Hub:
- Current pick: FAIL. It invents a new 4-start class economy and overrides canonical hub spend without saying so.
- Best fit: Warblade start, canonical class unlocks, optional defer of Vrel/Cartographer to reduce scope.

Gate 5 Image:
- Current pick: PASS/WARN. Relabel first is narrow and cheap. Placement must avoid crowding Act 4 row.

Overall: v2.2 is closest to the philosophy when it removes systems, but some removals silently override canon. The lock should distinguish "deferred for Phase 1" from "deleted from canon."

## Section 4 - Recommended v2.3 Patch List

Patch 1 - Forge / Anvil wording:
- BEFORE: "Forge feature Shop icinde Act 1, dedicated Forge node Act 2+ only."
- AFTER: "Karar #62 Act 1 has no dedicated Forge. This explicitly overrides/de-scopes MAP_ITEM_SYSTEM's Act 1 guaranteed Forge for Phase 1. Act 1 core is component collection; Merchant Anvil remains rare/expensive safety valve, not main craft path. First guaranteed full Forge node starts Act 2."

Patch 2 - Echo Imprint trigger:
- BEFORE: "Trigger = after first Elite of each Act."
- AFTER: "Trigger = after every 3rd Combat/Elite clear, normal reward'a ek, if that act's 1 Imprint slot is unused. Act 1 locked topology makes this resolve at first Elite."

Patch 3 - Architect ending:
- BEFORE: "Story-only first kill, Stay/Break/Carry 3 ending = replayability."
- AFTER: "Phase 1: Architect kill grants 50-75 Shattered Echoes and opens Stay/Break/Carry ending choice. Mechanical ending consequences from GDD, especially Break hub impact, are deferred until user signoff."

Patch 4 - Hub economy:
- BEFORE: "Phase 1 = class unlock only; 4 starting classes; 100/150/200/250/300/400 Echoes."
- AFTER: "Canonical baseline: Warblade starts unlocked. Class unlocks use canonical Echo costs/milestones; exact Brawler/Hexer cost conflict requires tie-break. Vrel/Cartographer spends are canonical but may be Phase 2+ deferred."

Patch 5 - Skill slot draft guard:
- BEFORE: "Act 1 4 active, 2 visible-locked" with no offer routing.
- AFTER: "Visible-locked slots are UI only. Draft cannot place into them. When 4 Act 1 slots are full, New Skill either opens replace flow or is suppressed by weight table."

Patch 6 - Pity counter:
- BEFORE: "Skill absent 5 consecutive drafts -> 80% next draft."
- AFTER: "Pity counts normal 3-choice Skill Drafts only. Elite Reward and boss reward do not increment/reset unless explicitly selected as Skill Draft."

Patch 7 - Branch fragment HUD:
- BEFORE: "HUD counter top-center X/8."
- AFTER: "Mandatory counter caps at 8/8. Optional branch fragments display as +bonus reveal, not 9/8 boss progress."

Patch 8 - Curse Gate:
- BEFORE: "Curse Gate no fragment, risk/reward."
- AFTER: "Curse Gate does not count toward mandatory 8. Reject = +5 percent HP. Accept = one locked reward lane selected for Phase 1: Burden/Gift OR Legendary shortcut OR optional bonus reveal. Do not stack all three by default."

Patch 9 - Boss Legendary:
- BEFORE: "class-specific Legendary 3-choice."
- AFTER: "Current primary class's 3 canonical Legendary anchors are offered. No randomization unless class has >3 anchors later."

Patch 10 - Image 13:
- BEFORE: bottom-band Stay/Break/Carry reads as meta-track.
- AFTER: remove bottom band; add compact "Ending Choice: Stay / Break / Carry" below Architect outcome. If manual relabel is cramped, regenerate image 13.

## Section 5 - Open Questions

1. Which canon wins for Act 1 Forge: MAP_ITEM_SYSTEM guaranteed Forge or Karar #62 no-Forge topology?
2. Does user approve replacing "every 3 combat rooms" with first-Elite globally, or should first-Elite remain Act 1 shorthand only?
3. Are GDD Stay/Break/Carry mechanical consequences deferred, or should Break's Hub NPC removal stay canonical for Phase 1?
4. Which class unlock table is final? NLM sources conflict on later-class exact costs, but all conflict with the proposed 4-start/100-400 plan.
5. For Curse Gate Phase 1, choose exactly one accept reward lane: Burden/Gift, Legendary shortcut, or bonus fragment/reveal.
