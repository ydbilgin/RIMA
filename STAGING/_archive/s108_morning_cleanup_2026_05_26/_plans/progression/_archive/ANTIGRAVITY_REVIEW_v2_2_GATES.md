# Antigravity Third-Opinion Review — v2.2 Progression Gates

## Section 1 — 6 Gate Verdicts

### Gate 1 (Forge)
**Verdict:** **A1 (Karar #62 Reopen / Branch Node Replacement)**
**Canonical evidence:** 
- `TASARIM/MAP_ITEM_SYSTEM.md`: "Node ~4: Forge Room (guaranteed, sabit)" [Line 37]
- `TASARIM/MAP_ITEM_SYSTEM.md`: "Merchant: rare: Anvil etkilesimi ... safety valve ... pahalı tutulur" [Line 78, 257]
**Filozofi check:**
1. *Basit tut, scope dar:* Replacing the Mystery B02 branch node with a dedicated Forge Node preserves the 15-node limit (no topology expansion, no extra scene weight).
2. *Her run farklı hissi ZORUNLU:* Allowing early Forge crafting makes Act 1 builds highly dynamic, letting players combine components before the boss.
3. *Mekanik şişirme YASAK:* Avoids cluttering the Merchant UI/logic with a complex hybrid Forge sub-system.
4. *Demo önce:* Critical for the Demo (Act 1 vertical slice) to showcase RIMA's unique crafting loop.
**Risk:** Players might always branch to the Forge. This can be mitigated by making the Forge branch slightly more dangerous (e.g., adding an Elite combat guard before the Forge portal).

---

### Gate 2 (Echo Imprint Trigger)
**Verdict:** **B2 (First Elite of each Act Milestone Trigger)**
**Canonical evidence:**
- `TASARIM/ROOM_MECHANICS.md`: "Her 3 combat odada bir kez ... max 4/run (act basina 1 slot)" [Line 358-359]
- `PROGRESSION_PLAN_v2_2_LOCK.md`: "Act 1 locked topology makes this resolve at first Elite." [Line 308]
**Filozofi check:**
1. *Basit tut:* Eliminates cross-room runtime counters and complex "slot full" conditional card injections.
2. *Her run farklı hissi:* Variety is maintained through the combinations of 4 categories × 4 Acts, even if the node trigger is milestone-consistent.
3. *Mekanik şişirme YASAK:* Deletes the redundant combat-count tracking subsystem, replacing it with a static topology milestone.
4. *Demo önce:* Highly predictable and satisfying power spike for Demo playtesters at the first Elite.
**Risk:** Reduces the randomness of the trigger node. However, this is heavily outweighed by the stability and simplicity of the code.

---

### Gate 3 (Architect Break Ending NPC Removal)
**Verdict:** **C1 (Defer Mechanical Consequences for Phase 1)**
**Canonical evidence:**
- `TASARIM/GDD.md`: "Break = Gerçek risk, gerçek kayıp | Bazı NPC'ler yok olur" [Line 552]
- `TASARIM/GDD.md`: "BREAK ending seçilirse Hub'dan bazı NPC'ler KALICI silinir" [GDD Section 16]
**Filozofi check:**
1. *Basit tut, scope dar:* Save-file permanent NPC deletion and associated Hub UI state changes are high-risk and high-cost for early playtests.
2. *Demo önce:* Playtesters in the Demo are testing combat and progression, not end-game save-file persistence.
**Risk:** End-game players may find the narrative choice lacks bite in Phase 1, but this is a standard and acceptable deferral for a vertical slice.

---

### Gate 4 (Hub Class Unlock)
**Verdict:** **D2 (Canonical Override: 4 Starting Classes Unlocked)**
**Canonical evidence:**
- `TASARIM/GDD.md`: "Playtest priority = Warblade + Elementalist + Ranger + Shadowblade" [Line 628]
- `PROGRESSION_PLAN_v2_2_LOCK.md`: "Karar #150 LOCK: playtest priority = 4 class" [Line 119]
**Filozofi check:**
1. *Basit tut:* Pre-unlocking the 4 core classes is a trivial config flag change.
2. *Her run farklı hissi:* Quadruples run variety instantly from run 1. Playtesters are not forced to grind Warblade-only to unlock the core combat loops.
3. *Demo önce:* Crucial for showcasing the cross-class combos (e.g. Elementalist + Shadowblade) which is the game's core hook.
**Risk:** Pace issues for the remaining 6 classes. *Verdict:* Keep the remaining 6 classes locked behind adjusted progression costs (Ronin/Ravager @ 120 Echoes, Brawler/Summoner @ 180, Hexer @ 250) to preserve long-term goals.

---

### Gate 5 (Image 13 Stay/Break/Carry Bottom Band)
**Verdict:** **E1 (Manual Visual Crop + Under-Node Compact Label)**
**Canonical evidence:**
- `PROGRESSION_PLAN_v2_2_LOCK.md`: "Stay/Break/Carry = ending choice only ... NOT run-start" [Line 60]
**Filozofi check:**
1. *Basit tut, scope dar:* Deleting/cropping the bottom band manually is a 5-minute task. AI image regeneration risks introducing visual bugs or text corruptions.
2. *Mekanik şişirme YASAK:* Prevents cognitive overload by keeping ending choices confined strictly to the final Act 4 node area.
**Risk:** Manual edit must look seamless. If legibility is cramped near Act 4, use a clean 1-line text strip below the core chart.

---

### Gate 6 (Demo Scope - Class Sayısı)
**Verdict:** **F1 (Demo Restricted to 4 Core Classes)**
**Canonical evidence:**
- `STAGING/concepts/skill_sheets_v3/skill_enumeration_v3.json`: Ronin has only 4 skills, others have 8. Core 4 have 14-22 skills. [Lines 18-136]
**Filozofi check:**
1. *Basit tut, scope dar:* Restricting the playable pool to the 4 complete classes avoids severe draft weight dilution and balancing nightmares.
2. *Mekanik şişirme YASAK:* Avoids players draft-locking themselves into narrow 4-skill pools where variety is zero.
3. *Demo önce:* A polished, robust 4-class demo is infinitely superior to a buggy, bare 10-class showcase.
**Risk:** Players might request the other classes. *Mitigation:* Display the locked 6 classes in the UI as "Locked/Coming Soon" to outline project scale.

---

## Section 2 — 7 Codex Logic Conflict Validation

### Conflict 1 (Skill slot 4→6)
**Stance:** **AGREE**
**Reasoning:** Once the 4 active slots are full in Act 1, offering a "New Skill" that cannot be slotted is a wasted draft option. Suppressing the New Skill offer weight (to 0% or 10%) or opening a "Replace Skill" interface is mandatory for logical draft flow.

### Conflict 2 (Cross-class proc Act 1)
**Stance:** **AGREE**
**Reasoning:** Since secondary classes do not exist in Act 1, the `CrossClassProcManager` has no valid tags to evaluate. Disabling the manager during Act 1 prevents redundant update calls and potential null-pointer exceptions in the combat engine.

### Conflict 3 (Pity counter source)
**Stance:** **AGREE**
**Reasoning:** Elite Rewards and Boss Rewards are fixed high-tier reward pools. Including them in the standard 3-choice draft pity tracker will skew the math. Keeping pity limited strictly to standard drafts is clean and modular.

### Conflict 4 (Mystery branch fragment HUD)
**Stance:** **AGREE**
**Reasoning:** Showing "9/8" or "10/8" looks like an interface bug. Capping the boss gate progress meter at "8/8" and tracking optional branch fragments as separate "+X Bonus Reveal" badges is the correct UX solution.

### Conflict 5 (Curse Gate reward path)
**Stance:** **PARTIAL AGREE**
**Reasoning:** While limiting scope is good, the **Legendary Shortcut** (sacrifice 3 components + risk a curse for a random Legendary) is the thematic soul of the Curse Gate. We should keep **only this lane active** for Phase 1, discarding the other two lanes. Reject must yield +5% Max HP restore as a fallback.

### Conflict 6 (Forge/Anvil pricing semantic)
**Stance:** **AGREE**
**Reasoning:** Forge combines for free using a node action. The Merchant Anvil is a costly safety valve (150+ Gold). Since we recommended reopening the topology to add a Forge branch in Act 1, crafting remains accessible but strategically bounded, while the Anvil retains its correct emergency role.

### Conflict 7 (Boss Legendary 3-choice)
**Stance:** **AGREE**
**Reasoning:** Each of the 4 core classes has exactly 3 canonical Legendary items (`TASARIM/MAP_ITEM_SYSTEM.md` §3). The boss should present these 3 choices deterministically to support the class's 3 major build archetypes. No randomization is needed.

---

## Section 3 — Antigravity's Bulduğu Ek Mantıksızlık

### 1. The "Rest Node" Contradiction
- **Severity:** **CRITICAL**
- **Evidence:** 
  - `ROOM_MECHANICS.md` Line 433: *"Rest odası yok. HP dağıtık kaynaklardan gelir — bu kasıtlı."*
  - `MAP_ITEM_SYSTEM.md` Line 38: *"Node ~8: Rest Node (guaranteed, sabit)"*
  - `PROGRESSION_PLAN_v2_2_LOCK.md` Line 74: *"N04 | Rest | 1/2"* and *"N09 | Rest | 2/2"*
- **Issue:** Absolute logical desync in the core documentation. One file bans Rest rooms for survival tension, while the plan implements two of them in Act 1!
- **Fix:** Adopt a hybrid Rest Node model. Keep exactly 1 Rest Node per Act (N08/N09 transit node) but do NOT fully heal. Offer the player a choice: **Rest (Restore 25% Max HP)** OR **Reforge (Upgrade 1 Skill Tier)**. This preserves tension and aligns with modern roguelite standards.

### 2. The "Branching Soft-Lock" Boss Gate Risk
- **Severity:** **CRITICAL**
- **Evidence:**
  - `map_fragment_system.md` Line 51: *"Boss kapısı: 8 zorunlu fragment toplandığı zaman otomatik açılır."*
  - `map_fragment_system.md` Line 49: *"Toplam zorunlu: 8 fragment (combat 6 + elite 2)."*
- **Issue:** Under the fixed topology, if a player takes a branch node (B01 Curse Gate or B02 Mystery) they bypass some combat/elite nodes. Since the Boss Gate strictly checks for "8 fragments collected," a branching player will have only 7 fragments and will be **permanently soft-locked** at the Boss Door!
- **Fix:** The Boss Door must unlock automatically upon reaching Depth 12, regardless of fragment count. The "X/8" fragment counter should instead represent the "Tablet Reconstruction Level." Completing all 8 fragments should reward the player with a bonus (e.g. +1 free Legendary re-roll or +50 Gold), rather than gating entrance to the boss.

### 3. The "Sub-Room Map Fragment Drop" Bug
- **Severity:** **WARN**
- **Evidence:**
  - `ROOM_MECHANICS.md` Line 94: *"Macro Combat = 2+ sub-rooms. Map Fragment drops after MACRO clear, NOT per sub-room."*
  - `map_fragment_system.md` Line 22: *"Combat / Elite / Unknown odası temizlendiğinde -> reward sequence: ... Map fragment zemine düşer."*
- **Issue:** If a macro node transition fades between sub-rooms (e.g., entry_chamber to pillar_arena), a standard "room clear" hook might trigger the fragment spawn prematurely in sub-room 1, allowing players to pick it up and exit before completing the full node encounter.
- **Fix:** Script the `MapFragment` to instantiate exclusively via the `MacroRoomController` *after* the final sub-room is cleared. Transitional portals between sub-rooms must never spawn fragments.

---

## Section 4 — Final Recommendation

| Priority | Action | Rationale |
|---|---|---|
| **1** | Fix the **Branching Soft-Lock** Boss Gate logic. | Prevents severe runtime soft-locks where players are trapped before the Penitent Sovereign door. |
| **2** | Adopt the **A1 (B02 → Forge Branch)** Act 1 topology change. | Essential for teaching the core crafting loop in the Demo/Act 1 vertical slice. |
| **3** | Resolve the **Rest Node Contradiction** using the Hybrid Rest model. | Unifies core systems documentation and establishes healthy playtest survival tension. |
| **4** | Restrict the Demo/Phase 1 strictly to the **4 complete classes**. | Protects draft weights, gameplay polish, and overall playtest rating. |

---

## Section 5 — Open Questions for Orchestrator/User

1. **Gate 1 Forge Branch Danger:** Should the new B02 Forge branch be guarded by a high-threat encounter or carry an activation cost to balance its high-value craft rewards?
2. **Hybrid Rest Choice:** Does the Orchestrator/User approve of the "Restore 25% HP vs. Upgrade 1 Skill Tier" choice for the single Rest Node, or should we stick to pure HP restoration?
3. **Locked Class Previews:** In the Hub Class Selection UI, should the 6 locked classes be completely hidden, or visible as "Coming Soon" silhouettes to show future release depth?
