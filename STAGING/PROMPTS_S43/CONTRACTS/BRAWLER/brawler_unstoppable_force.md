# SKILL VISUAL CONTRACT -- Brawler: Unstoppable Force

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_UNSTOPPABLE_FORCE` |
| display_name | Unstoppable Force |
| slot | master |
| role | pressure / reposition |
| state_owner | yes (applies no-decay Charge state to caster for 4s; each dash auto-triggers Crackjaw) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: the Brawler enters a 4-second berserker state. Charge does not decay. Speed increases 50%. Every dash auto-fires a full Crackjaw combo. The body reads as unstoppable forward momentum -- arms pumping, hunched forward, low center of gravity. The state activates with a burst of bruise-purple body aura, then simmers for the duration. With Cyclone Drive equipped, Charge stays at maximum and Cyclone Drive duration extends +2s (no loop).

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | power surge: body hunches forward, shoulders pull back, Charge aura ignites -- activation tell | aura burst reads as the activation moment |
| active_burst | yes | 3 | activation burst pose (F1 aura peak), settle into run-stance (F2), body locks into forward-lean mode (F3) | bruise-purple aura burst at F1; audio anchor F1; after F3 caster enters the 4s duration state |
| loop | yes | 3 | sustained forward-lean run -- aggressive, low, pumping; Charge no-decay aura flickers on body | 3-frame run loop; aura on knuckles and shoulders simmers purple-brown; state continues for 4s |
| dash_trigger | yes | 2 | each dash fires Crackjaw: short forward lunge pose (F1 dash-in, F2 punch extension) | reuses Crackjaw active silhouette; must be recognizable as the same combo at 64px |
| recovery | yes | 2 | state-end: body straightens up from forward lean, aura fades | Charge decay resumes; aura dims |

Frame total: 12. Master skill -- quota flag: 12 frames is above standard. Design confirmation required.

Note: dash_trigger (2 frames) is a sub-animation that fires per dash during the 4s window. It does not add to the spritesheet row count -- it should reuse the Crackjaw active row or be generated as a shared frame reference.

Cyclone Drive synergy note: if Cyclone Drive is in loadout, Charge stays pinned at 5 (max) for the duration and Cyclone Drive duration extends +2s (no loop). No additional animation frames needed -- same state aura, slightly longer duration tick.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | no-Charge-decay state on caster for 4s | Charge counter stops flashing/decaying; aura stays lit at current tier | Charge UI counter freezes; body aura stays at activation level (bruise-purple) |
| applies | speed +50% (movement buff) | run loop is visually faster; frame timing change not a new frame | same loop frames, faster playback |
| applies | each dash auto-fires Crackjaw | dash_trigger sub-animation plays per dash | see dash_trigger state above |
| reads | Cyclone Drive in loadout: Charge stays max, Cyclone Drive +2s | Charge aura during loop shows at full-purple (max tier) for entire duration | full-purple knuckle and shoulder aura throughout loop; no partial-charge brown |
| consumes | state ends at 4s or on manual cancel | recovery state plays; aura dims and fades over 2 frames | aura fade is a linear dim, not a burst collapse |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | activation burst at F1 of active_burst: bruise-purple body-aura flare (radial from caster body, not a projectile); lasts 1 frame at full intensity, persists at low glow for duration |
| impact_particle | yes | each auto-Crackjaw (dash_trigger F2): standard Crackjaw impact particles (dust-puff chain + uppercut ring on final hit) -- inherit from Crackjaw VFX spec |
| trail | yes | movement trail during loop: 1-frame ghost silhouette behind caster at 30% opacity (speed read); brown-dirt tone |
| screen_overlay | no | master skill -- screen overlay is strong but reserved for V Burst (OVERDRIVE); this is a passive-state skill, not a V Burst; no overlay |
| hit_reaction_on_enemy | yes | inherit from Crackjaw: `cracked` on jab/cross/hook; Charged State last hit uses `shattered` if Cracked present (since Charge stays pinned at 5 during Unstoppable Force, every auto-Crackjaw fires at Charged State) |
| audio_anchor_frame | active_burst F1 | activation burst is the audible beat; loop has a sustained low rumble (ambient SFX, not a beat); each auto-Crackjaw inherits Crackjaw audio anchors |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Charge no-decay state (Brawler resource), auto-Crackjaw (Brawler OWNS combo), Cracked/Shattered inheritance; off-axis hunched body |
| Avoids every AVOIDS item | PASS | no weapon-armor break; no pre-draw stillness; forward momentum is pure body |
| Counter-archetype distinct (Rule #57) | n/a | Unstoppable Force is pressure/reposition, not counter |
| No HP-execute visual cue (Rule #56) | PASS | state duration is time-gated (4s); aura reads Charge resource tier; no HP check |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | auto-Crackjaw uses Brawler Cracked (organic body fissures); no metallic plate shards; Sundered language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | hunched forward-lean power surge (activation) + aggressive run loop is a unique silhouette; no other Brawler skill has a sustained state run loop |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active_burst, loop, recovery) |
| frames_per_row | 2, 3, 3, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 10 frames (dash_trigger reuses Crackjaw active row) |
| priority | P2 -- master skill, unlocks late; not on critical path |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
