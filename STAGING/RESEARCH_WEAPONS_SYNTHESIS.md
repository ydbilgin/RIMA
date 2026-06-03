# RESEARCH SYNTHESIS — Weapon Variants & Skill Evolution (Opus reconcile, 3-AI)

Topic: the user wants each class to equip weapon VARIANTS (Ranger bow -> crossbow,
Gunslinger revolver -> shotgun -> faster) where skills EVOLVE per weapon. User is enthusiastic.

This re-litigates LOCKED canon. The user gets the final call. Below is the verified status,
the conflict in plain terms, the two real options, a recommendation, and per-class examples.

---

## 1. CANON VERIFICATION (via NLM — not taken on faith)

I queried the design notebook directly. The canon claim Opus flagged is CONFIRMED, with exact
quotes. Two separate locks are involved:

### Lock 1 — "1 class = 1 weapon = 1 silhouette, no variants"
> "KRITIK KURAL: 1 sinif = 1 silah = 1 silhouette. Variant YOK. Silah degisimi = sinif degisimi.
> (Karar #72 + #80)"  — CLASS_SILHOUETTE_BIBLE.md, LOCKED 2026-05-13

> "Silah = siluet'in %40-50'si — silah degisimi = sinif degisimi, variant uretilmez"
> — Cross-Class Kurallar (LOCKED)

Meaning: the weapon is 40-50% of how you recognize a class on a 64px chibi screen. Swapping the
weapon silhouette IS swapping the class. A "Ranger crossbow" as a new on-screen weapon is, by
canon definition, a different class slot — not a Ranger variant. NOTE: later decisions (#123/#144)
decoupled the weapon into a separate Unity sprite for animation reasons, but the IDENTITY rule
(one class bound to one weapon type) is explicitly stated to remain intact.

### Lock 2 — build variety comes from the DRAFT, not weapons
> "RIMA should not use a visible skill tree screen for run progression. The production model is:
> Room reward offers 3 choices [new skill / upgrade / passive-echo / tag synergy]."
> — SKILL OFFER SYSTEM DECISION, LOCKED 2026-05-03

> "Her class'in 12 skill'i var ama bir run'da hepsini alamazsin — kasitli."
> "Act 1'de 4 slotla 12 skill havuzundan secim yapiyorsun ... Secondary class gelince 2 yeni slot."

Meaning: RIMA deliberately rejects ARPG weapon-loot trees. Variety = (a) which of the 12 skills
you draft + upgrade, and (b) the secondary class. The "find a better bow" loop is the exact
pattern RIMA chose NOT to build.

VERDICT: Opus's canon read is ACCURATE on both points. The literal idea does collide with locked
decisions #72 + #80. This is a real conflict, not a misreading.

---

## 2. THE CONFLICT, IN PLAIN TERMS

The user's idea: "Let a class own several weapons (bow, crossbow, faster gun) and the skills change
depending on which one you're holding." This is exciting because weapon choice is a beloved
roguelite hook (Hades, Dead Cells).

The canon's position: "A class IS its weapon. The bow is half of what makes a Ranger a Ranger. If
you put a crossbow on screen instead, that's not a Ranger anymore — it's a different class. And the
thing that's supposed to make runs feel different is the skill draft, not a weapon shelf."

So the tension is specifically about the WEAPON SILHOUETTE (the thing you see on screen) and about
where "build variety" is allowed to come from. The user's underlying instinct — quoted in the brief
as "weapons change HOW you break, not just damage numbers" — is 100% aligned with RIMA's depth
philosophy. The collision is only with the literal "equip a different-looking weapon" part.

---

## 3. THE OPTIONS (clean decision for the user)

### OPTION A — "Weapon Forms" reframe  (canon-safe)

**What it is:** Keep the one locked weapon silhouette on screen forever. But let a class's signature
weapon adopt a FORM (working name; "Stance" is overloaded by Ronin/Summoner) that re-shapes its
Sundered-Beat. Same bow on screen — but it now BREAKS like a crossbow breaks. You acquire it as a
rare/legendary DRAFT pick during a run; taking it costs a draft slot (so you give up a skill).

**The skill-evolution principle (the load-bearing rule):** A Form is legitimate ONLY if it changes
the SHAPE, TIMING, or CONDITION of the class's break/execute gate — never the raw numbers. Pick ONE
axis per Form so the identity reads:
- SHAPE of the BREAK — one deep break vs. many shallow breaks (the bow's "one heavy bolt" vs the
  shotgun's "crack several guards at close range").
- TIMING of the EXECUTE gate — slower-but-bigger window vs. faster-but-briefer.
- CONDITION of the EXECUTE — which break-state the execute consumes, ALWAYS staying inside the
  class's OWNS tags (a Form may never grant an AVOIDS-list tag — no Form gives Warblade bleed).

A Form that only tweaks damage/speed/range is rejected at design review as noise.

**Concrete per-class Forms** (the three the user cares about, enriched by cx feasibility + agy breadth):

1. Ranger — "Heavy-Draw Form" (THE canon-safe crossbow). Same compound bow on screen. Replaces the
   tap-cadence with ONE charged heavy bolt that applies BOTH Marked and a pin-in-place Trapped on
   impact — one shot opens the whole execute gate, but draw time is long and Focus drains if rushed.
   Beat changes from "mark, lay trap, detonate" -> "one disciplined heavy shot marks-and-pins, then
   execute." Delivers the crossbow FEEL with zero new silhouette. (cx confirms: PinningShot +
   MarkedDetonate riders + ShotCadence modifier = the cheapest real prototype in the codebase.)

2. Gunslinger — "Scattergun Vent Form" (the canon-safe shotgun). Same revolver on screen. A
   close-range vent-blast applies Suppressed to EVERY enemy in a short cone in one beat and dumps
   Heat fast. Deadshot's gate re-routes from "single exposed line" -> "3+ Suppressed targets in
   cone" = a close-range crowd-execute. The "shotgun" lives entirely as a Heat/break-shape behavior.

3. Gunslinger — "Fan-Hammer Form" (the canon-safe "faster gun"). Dumps the whole chamber in one
   beat for instant max Heat; Deadshot opens on "0 bullets + last shot landed" — a feast-or-famine
   empty-the-gun rhythm. The "faster" feel = Heat-timing change, not a fire-rate stat.

4. Warblade — "Maul Form." Same greatsword. The 3rd combo hit becomes a single overhead that
   applies Broken to every guard in a small frontal cone, but Death Blow now needs TWO Broken
   targets stacked to fire. Beat changes from "pin one, break, execute" -> "herd two, break the
   wall, execute the wall." Stays fully inside Warblade OWNS (armor-shred / broken / sundered).

(Full 10-class Form map with AVOIDS lists is in RESEARCH_WEAPONS_OPUS.md §3. Elementalist/Summoner/
Hexer have NO swung weapon, so their "Form" axis is disc-rotation / lantern-command / curse-spread —
not a weapon swap at all. Brawler is weaponless: Forms = stances only.)

**Where it slots:** Phase-2 depth layer, likely WAVE 5+. Phase-1 demo ships ZERO Forms (Warblade
stays on its base verb). Scope guardrail: max 2 Forms per class for the full game = 20 content units
that reuse existing art and plug into the existing draft. One Form per run, max.

**Impl cost (from cx):** MEDIUM. The visual mount already exists — HandAnchorAttach already does
runtime prefab attach via WeaponDatabaseSO(classId, formId); it just needs a public EquipForm(formId)
that re-wires OrientationSync + weaponRenderer + resyncs facing after a swap. Gameplay = a
PlayerWeaponState component + a CurrentForm query in SkillBase + targeted conditionals in 1-2 skills
per class (NOT all 12). Acquisition = extend RewardType with one new value and route it in
DraftManager — do NOT smuggle a Form in as a SkillData. Recommended first prototype: Ranger (cleanest
existing controller, mark/trap state already shared via SkillStateTracker).

**Art cost:** Reuses the existing one-silhouette pipeline. A Form may recolor/re-time the BREAK flash
(cyan seal-energy), not the blade. No new 8-direction weapon sprites.

---

### OPTION B — Literal weapon-swap / arsenal  (the user's exact words)

**What it is:** Ranger genuinely equips a crossbow that looks different on screen; Gunslinger swaps
to a visibly different shotgun; skills swap with the weapon. This is cx's "Rank 3 / Option C" and
agy's "Option C on-the-fly swap."

**What it would take / re-litigate:**
- Re-opens and overturns LOCKED Decision #72 + #80 ("Variant YOK. Silah degisimi = sinif degisimi").
  This is not a tuning change; it rewrites the class-identity foundation that 10 classes were
  designed against.
- ART DEATH-SPIRAL (the canon's own stated fear, echoed by agy + the CoM postmortem in memory):
  a new visible weapon = a new silhouette = new 8-direction sprites + offsets + swing tuning per
  weapon per class. 10 classes x 2-3 weapons x 8 directions is exactly the bake-explosion RIMA
  abandoned. agy explicitly flags this and routes the literal version to "mitigated only by NOT
  giving each variant its own animation" — which lands you back at Option A.
- SKILL CONTENT BLOWUP: cx rates the full skill-set-swap version LARGE cost — every weapon needs its
  own loadout table; DraftManager must filter by weapon and purge stale skills; "12 skills" becomes
  "12 x N weapons." Directly fights the depth-not-breadth lock.
- BUILD-IDENTITY whiplash: swapping weapon mid-run changes what your whole draft was building toward
  (cx + agy both flag this as a balance/UX hazard).

**Honest risk:** This is the highest-cost, highest-conflict path and the one most likely to recreate
the failure modes RIMA documented and chose to escape. It is buildable, but it spends the project's
scarcest resource (art) on the thing canon says dilutes class identity.

---

### HYBRID middle ground (worth naming, not recommending as the headline)

agy proposed an "Aspect + Evolution Card" hybrid: unlock a weapon Form in the HUB with Shattered
Echoes (pre-run, Hades-Aspect style), then mid-run draft cards evolve it. This is Option A wearing a
meta-progression coat. The risk Opus flags: a permanently-equipped, hub-bought Form turns a run-shaping
choice into a loadout screen and competes with the skill draft for the same "what's my build" job —
flattening roguelite tension. The clean compromise: keep the Form itself a RUN-DISCOVERED draft pick
(Option A), and let meta's ONLY touch be "raise the odds a discovered Form appears in your pool" —
influencing odds, never granting the Form directly. That preserves both the in-run discovery and the
locked role of Echoes (class unlocks + hub upgrades).

---

## 4. RECOMMENDATION (the user's call — this re-litigates locked canon)

Recommend OPTION A ("Weapon Forms"), with the run-discovered acquisition and the odds-only meta touch.

Reasoning:
- It gives the user the thing they actually want — "weapons change HOW you break" — including the
  crossbow FEEL, the shotgun FEEL, and the faster-gun FEEL, with concrete designs already drafted.
- It does NOT re-litigate #72/#80, does NOT trigger the art death-spiral, and reuses the existing
  mount system (cx: MEDIUM cost, Ranger-first prototype is small).
- It spends the variety budget INSIDE the locked draft system rather than fighting it.
- It is the rare proposal where the canon-safe version is also the genuinely better design: a Form
  competing for a draft slot creates an opportunity cost (give up a skill to reshape your verb) that
  a free weapon-swap never has.

If the user specifically wants visibly-different weapons on screen (Option B), that is legitimate but
it is a deliberate decision to REOPEN locked canon #72/#80 and accept the art cost. It should not be
done silently or as a side effect of "skill evolution" — it is a foundation-level rewrite and deserves
its own explicit go/no-go.

---

## 5. RECONCILING WHERE cx / agy DIVERGE FROM OPUS

Opus predicted cx and agy would "treat the arsenal literally." Verdict on who's right where:

- On the CANON CALL: Opus is right. Neither cx nor agy front-loaded the #72/#80 collision as a
  hard stop; both presented the literal weapon-swap (cx Rank 3 / Option C, agy Option C) as a real
  menu item and only flagged the 12-skill/sprite-budget conflicts as mitigations. The NLM quotes
  confirm the silhouette lock is a HARD identity rule, so the literal swap is a canon REOPEN, not a
  scoping choice. Opus correctly elevated this to the top of the decision.

- On the DESIGN MODEL: all three CONVERGE, which is the strong signal. Opus's "Forms re-shape
  break SHAPE/TIMING/CONDITION, never numbers," cx's "base skill + weapon modifier decorator, keep
  the 12 canonical skills," and agy's "runtime decorator that shifts Shape/State tags, reuse 3 rigs"
  are the SAME architecture described in three vocabularies. This convergence is the main reason to
  trust Option A.

- On FEASIBILITY: cx is the authority and adds what Opus could not see — the mount system already
  exists (HandAnchorAttach + WeaponDatabaseSO with classId/formId), so the visual half is nearly
  free; the real work is the modifier query + 1-2 skill hooks per class + a RewardType.Weapon draft
  branch; Ranger is the correct first prototype (not Warblade — its DraftManager/secondary-slot
  entanglement would hide architecture problems). Trust cx here.

- On BREADTH: agy is the authority on the acquisition-model menu (Hub-Aspect vs Daedalus-Hammer vs
  Dead-Cells drop) and supplies the richest per-class variant table. Use agy's table as an IDEA bank
  for Form FEELS, but FILTER it through Opus's two guardrails: (1) no second silhouette, (2) no
  AVOIDS-tag leakage. Several agy entries violate these as written — e.g. Ronin "Dual Wakizashi auto-
  applies Bleed" leaks Ravager's bleed lane; Shadowblade "Shattered Darts" (throwing knives) leaks
  Ranger distance; "Veil Scythe / Nodachi / Sawed-Off" are literally new silhouettes. Keep the FEEL,
  drop the silhouette and the cross-contamination, and they become valid Forms.

- WHERE agy OVER-REACHES: the "on-the-fly Dead-Cells weapon drop" (Option C) clashes with RIMA's
  character-focused, build-deliberate loop and agy itself lists the cons. Do not adopt it.

Net: Opus owns the canon + identity guardrails, cx owns feasibility + build order, agy owns the
breadth bank. Filtered through each other, they produce ONE coherent recommendation = Option A.
