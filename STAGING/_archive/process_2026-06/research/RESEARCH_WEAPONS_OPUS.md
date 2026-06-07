# RESEARCH: Weapon Variants & Skill Evolution — DESIGN-IDENTITY LENS (Opus)

Topic: each class equips weapon VARIANTS that change feel; skills EVOLVE per weapon.
Lens: does it SHARPEN class identity + the Sundered-Beat (BREAK -> EXECUTE), or DILUTE
into "many flavors of damage"? Phase-2 / future exploration, not demo work.

---

## 0. THE LOCKED-RULE COLLISION (must read first)

The topic as phrased collides head-on with TWO locked canon decisions:

1. **Decision #80 / #72 (LOCKED 2026-05-13): "1 class = 1 weapon = 1 silhouette. Variant YOK.
   Silah degisimi = sinif degisimi."** Weapon = 40-50% of silhouette; swapping the weapon
   silhouette is, by canon, swapping the class. So Ranger bow -> crossbow as a *new silhouette*
   is forbidden. The crossbow IS a different class slot.
2. **Build variety comes from the skill DRAFT, not from weapons** (NLM canon: RIMA explicitly
   abandons ARPG weapon trees / arsenals; Hades-style 3-choice draft + secondary-class is the
   variety engine). "Find a new bow" is the exact pattern RIMA rejected.

CONFLICTS WITH LOCKED RULES?: YES — the literal "equip weapon variants" framing violates #80.
I am NOT silently overriding it. The verdict below reframes the GOOD half of the idea
(weapons change HOW you break) into a form that lives INSIDE the locked rules. Anything that
requires a second weapon silhouette per class is flagged AVOID and routed to "that's a new class."

The salvageable, genuinely-RIMA idea is the user's own best instinct, quoted in the brief:
**"weapons change HOW you break, not just damage numbers."** That does not require a new weapon.
It requires the ONE locked weapon to have **FORMS / STANCES** that reshape the Sundered-Beat.

---

## 1. THE RECOMMENDED MODEL FOR RIMA: "Weapon Forms" (run-acquired, draft-gated), NOT an arsenal

### Verdict: Run-acquired, build-defining — like Hades Aspects in SPIRIT, but expressed as
### a single Legendary-tier DRAFT pick that re-shapes the class's signature verb, with ZERO
### change to the weapon silhouette.

Call it a **Weapon Form** (working name; "Stance" overloads Ronin/Summoner stance language).
A Form is one rare/legendary draft offer that says, in effect: *"your greatsword now BREAKS
the way a maul breaks"* — same greatsword on screen, different rhythm and break-shape.

Why this model and not the alternatives:

- **vs. literal weapon-variant arsenal (Ranger gets a crossbow item):** Rejected. Violates #80,
  doubles silhouette/art cost per class (10 classes x 2-3 weapons x 8 dir = art death-spiral the
  project already fears, see CoM postmortem in memory), and "many bows" is precisely the
  breadth-not-depth failure mode. A second bow that only changes draw speed and damage is noise.
- **vs. meta-unlock (buy the Form permanently with Echoes):** Rejected as the PRIMARY model.
  RIMA's tone is a somber, fragmented seal-keep where each run is a re-assembly of a broken self;
  the per-run draft IS the build. A permanently-equipped weapon form turns a run-shaping choice
  into a loadout screen — flattens the roguelite tension and competes with the skill draft for the
  same "what is my build this run" job. Meta's correct role is narrower (see Scope §4).
- **vs. Hades Aspects literally (pick weapon-aspect at the door before the run):** Close, but
  pre-run selection is meta-flavored and weakens the in-run discovery that RIMA's draft is built
  around. RIMA's twist on Aspects: the Form is an in-run DRAFT find, so it competes with skills for
  a draft slot — choosing the Form means giving up a skill, which is the depth-preserving cost.

### How it fits the tone
The seal-keep fantasy is "a shattered identity reassembling under pressure." A Weapon Form is not
a new toy you bought; it is the weapon *remembering a different way to break a seal*. Cyan =
seal energy; a Form re-colors/re-times the BREAK flash, not the blade. That reads as the same
character finding a deeper register of the same verb — exactly the somber, depth-over-novelty mood.

### One-line model statement
**Weapon Forms are rare/legendary draft picks that re-shape a class's Sundered-Beat (the BREAK
and the EXECUTE) on the SAME weapon silhouette; you acquire at most 1 per run, and taking it costs
a draft slot.** This is run-acquired and build-defining (Aspect-like), zero new art silhouettes,
and it spends the variety budget inside the system RIMA already locked.

---

## 2. THE SKILL-EVOLUTION PRINCIPLE: Forms change the SHAPE of BREAK/EXECUTE, never the numbers

A Form is legitimate ONLY if it changes the *geometry, timing, or condition* of the class's
break/execute gate. If it only changes damage/speed/range stats, it is noise and is rejected.

Three axes a Form may bend (pick ONE dominant axis per Form so the identity reads):

- **SHAPE of the BREAK** — single big break vs. multi-target break vs. break-over-time.
  (crossbow: one heavy bolt cracks one guard deeply; shotgun: one blast cracks several guards
  shallowly at close range.)
- **TIMING of the EXECUTE GATE** — does the gate open slower-but-bigger, or faster-but-briefer?
  (Form trades a wider draw window for a harder hit, or vice versa.)
- **CONDITION of the EXECUTE** — what state must exist to execute, and how you reach it.
  A Form can re-route which break-state the execute consumes — but ONLY within the class's
  OWNS tags. It may never grant a tag from the AVOIDS list (no Form gives Warblade `bleed`,
  no Form gives Ranger `armor-shred`). This keeps Forms from becoming cross-contamination.

### Worked examples (the "HOW you break" twist, by class verb)

1. **Warblade — Greatsword "Maul Form".**
   Base verb: 3-hit iron combo applies Sundered/Broken; Death Blow consumes it (400%, empties Rage).
   Maul Form changes the SHAPE: the 3rd combo hit becomes a single overhead that applies a deeper,
   ARMORED-AREA break — Broken now lands on every guard in a small frontal cone, not one target,
   but Death Blow's gate now needs TWO Broken targets stacked to fire (slower to set up, executes
   a cluster). Same greatsword. The beat changes from "pin one, break, execute" to "herd two,
   break the wall, execute the wall." Still 100% Warblade OWNS (armor-shred/broken/sundered/verdict).

2. **Ranger — Compound Bow "Heavy-Draw Form" (the canon-safe version of the crossbow idea).**
   Base verb: Mark + Trap -> Final Strike (gated on Marked AND Trapped). The user's crossbow is the
   right INTUITION (slower, one big break) but the wrong IMPLEMENTATION (new silhouette = #80
   violation). Heavy-Draw Form keeps the compound bow on screen and gives it the crossbow's FEEL:
   the tap-cadence is replaced by a single charged heavy bolt that applies BOTH Marked and a
   pin-in-place "staked" Trapped state on impact — one shot opens the whole execute gate, but draw
   time is long and Focus drains harder if rushed. The beat changes from "mark, then lay trap, then
   detonate" to "one disciplined heavy shot that marks-and-pins, then execute." A single bigger
   BREAK, exactly as the topic's best version asks — with no new weapon.

3. **Gunslinger — Revolver "Scattergun Vent Form" (the canon-safe shotgun idea).**
   Base verb: Heat rhythm; BREAK = Suppressed / Exposed-Line; EXECUTE = Deadshot (last bullet /
   perfect reload / down an exposed line). Scattergun Form changes the SHAPE of the break: a
   close-range vent-blast applies Suppressed to EVERY enemy in a short cone in one beat (breaks
   multiple guards shallowly, at knife range) and dumps Heat fast. Deadshot's gate re-routes: it now
   fires off "3+ Suppressed targets in cone" instead of a single exposed line — a close-range
   crowd-execute instead of a long-line single-execute. Same revolver silhouette; the "shotgun"
   exists entirely as a HEAT/break-shape behavior, honoring the topic's "breaks multiple guards at
   close range" insight without a second gun.

These three are the proof that the topic's most interesting answer is achievable with zero
silhouette breakage and full OWNS/AVOIDS compliance.

---

## 3. PER-CLASS VERDICT — which Forms SHARPEN, which "obvious variants" are NOISE

For each: the 1-2 Forms that genuinely sharpen the verb (+ the Sundered-Beat twist), and the
obvious-but-noise variants to AVOID. All Forms = same locked weapon silhouette.

### Warblade (HEAVY/MELEE/RAGE; OWNS armor-shred/sundered/broken/iron/verdict)
- SHARPEN — **Maul Form** (§2.1): cone-break of multiple guards, execute consumes a cluster.
- SHARPEN — **Cleave/Wide Form**: combo arc widens; Broken spreads on hit, but Death Blow loses
  its single-target 400% and becomes a sweeping verdict. Changes break SHAPE (one->many) honestly.
- AVOIDS (noise): "faster lighter sword," "longer reach sword," "+crit greatsword." Pure stat dials.
  Also AVOID any Form that adds bleed/DoT — that is Ravager's lane (#80 cross-contamination).

### Elementalist (CASTER/RHYTHM/ELEMENTS; OWNS fire/frost/lightning/earth/brand/rotation; NO WEAPON)
- Special case: the "weapon" is a floating Rune Disc, no silhouette to vary. The Form axis here is
  the DISC'S ROTATION, not a weapon swap. This is the cleanest fit of all.
- SHARPEN — **Fixed-Element Form** (e.g. "Pyre Disc"): drops the rotate-through-3 cadence; the
  empowered-3rd always lands as Fire, but each element-lock changes which break the brand applies
  (Fire = ignite-break that spreads; Frost = freeze-break that holds the gate open longer).
  Changes the TIMING/CONDITION of the empowered beat, not damage.
- AVOIDS (noise): "staff," "wand," "orb-that-shoots-faster." Decision #146 NO STAFF/NO WAND is
  locked; any literal weapon for Elementalist is an instant reject.

### Shadowblade (AGILE/STEALTH/EXECUTE; OWNS veil/scar/phase/shadow/echo)
- SHARPEN — **Reverse-Grip "Reaper Form"**: phase-through leaves a SINGLE deep Scar instead of the
  normal scattered scars; Severance now needs only 1 deep Scar but the phase has longer recovery.
  Single-big-break twist on a fast class — high-risk single-target executioner.
- AVOIDS (noise): "longer daggers," "twin swords," "throwing knives." Throwing knives especially
  leaks toward Ranger distance/precision — AVOIDS list violation. Keep it scar/phase geometry.

### Ranger (RANGED/PRECISION/TRAPS; OWNS distance/trap/mark/precision/tripwire)
- SHARPEN — **Heavy-Draw Form** (§2.2): one charged bolt marks-and-pins; the canon-safe crossbow.
- SHARPEN — **Volley/Suppression Form**: rapid spread that applies shallow Mark to a cone, gate
  re-routes to "many marked + one tripwire" — area-denial sniper. Break SHAPE one->many.
- AVOIDS (noise): "crossbow item," "longbow vs shortbow," "+fire-rate bow." The crossbow as a new
  silhouette is the headline #80 violation; deliver its FEEL via Heavy-Draw Form instead.

### Ravager (HEAVY/BLEED/FRENZY; OWNS bleed/hook/aggression/carnage/rend)
- SHARPEN — **Hook Form**: the hook becomes the break — yank a guard out of formation, Wounded
  applies on the pull; Blood Verdict gate opens on "hooked + bleeding." Changes CONDITION + adds
  forced-movement geometry, deeply Ravager.
- SHARPEN — **Reckless Form**: removes recovery frames, bleed stacks build faster the lower your HP
  — pushes the take-damage-to-build identity harder. Changes the build-up RHYTHM of the break.
- AVOIDS (noise): "bigger axe," "armor-shred axe" (that's Warblade — AVOIDS), "cleaner controlled
  swing" (that's the Warblade contrast point; do not blur).

### Ronin (FAST/PARRY/IAIJUTSU; OWNS iaido/stillness/opened/sheathe/precision)
- SHARPEN — **Long-Sheathe "Drawn-Tension Form"** (canon-aware; Ronin sheath/draw is an explicit
  #71 exception so the weapon already has two states): widens the parry deflect window but the Flash
  Draw execute now needs a FULL Tension bar AND a held sheathe — a slower, all-or-nothing iaijutsu.
  Changes the TIMING of the gate, deepening the "wait, draw, punish" verb.
- AVOIDS (noise): "dual katana," "no-sheathe fast katana." No-sheathe destroys the iaido identity;
  dual-wield leaks toward Shadowblade fluidity. Both reject.

### Gunslinger (RANGED/HEAT/CHAMBERS; OWNS heat/reload/burst/vent/bullet)
- SHARPEN — **Scattergun Vent Form** (§2.3): close cone-break of many guards, crowd Deadshot.
- SHARPEN — **Fan-Hammer Form**: dumps the whole chamber in one beat for max Heat instantly,
  Deadshot gate opens on "0 bullets + last shot landed" — a feast-or-famine empty-the-gun rhythm.
  Changes the HEAT timing of the break, not damage.
- AVOIDS (noise): "rifle," "SMG," "faster pistol." A rifle is a new silhouette (#80). The shotgun
  and rifle FEELS belong as Heat/break-shape Forms on the one revolver, never as new guns.

### Brawler (MELEE/ARMOR/MOMENTUM; OWNS cracked/shattered/brawl/counter/crack)
- SHARPEN — **Counter-Stance Form**: jab combo trades reach for a parry-window on every 4th beat;
  Cracked converts to Shattered only off a successful counter — turns the verb toward defensive
  reads. Changes the CONDITION of the shatter.
- AVOIDS (noise): "brass knuckles," "gauntlets," "weapon in hand." Brawler's whole identity is
  weaponless body-combat; any held weapon is an identity kill. Forms here are STANCES only.

### Summoner (SOUL/RIFT; OWNS soul/summon/sacrifice/minion/bond; weapon = Soul Lantern, no swing)
- SHARPEN — **Lantern-Beacon Form**: the lantern stops being passive and becomes the break — its
  light marks a "Sic" zone; the Sic commit-beat sacrifices a marked minion to BREAK all enemies in
  the zone (sacrifice-as-break). Changes the SHAPE of the command cadence. Pure Summoner OWNS.
- AVOIDS (noise): "staff," "scythe," "a melee Summoner weapon." NO swing is canon; the lantern is
  not a melee weapon. Forms re-shape the command/sacrifice rhythm, never give the Summoner a sword.

### Hexer (CURSE/STACKS; OWNS hexed/curse/stack/accumulation/spread; weapon = Grimoire/Cursed Scepter)
- SHARPEN — **Plague Form**: Curseweave's commit-beat trades single-target stack depth for SPREAD —
  Hexblast at 10 stacks jumps to neighbors at reduced stacks. Changes the SHAPE of the curse break
  (deep-single -> wide-shallow), leaning into the OWNS `spread` tag.
- AVOIDS (noise): "attack scepter," "wand that shoots." Hexer breaks via stacks/accumulation, not
  projectile DPS; a damage-stick scepter dilutes the patience-curse identity.

---

## 4. SCOPE GUARDRAILS — keep this depth-not-breadth

1. **TOP GUARDRAIL — Zero new weapon silhouettes, ever. Forms re-shape the ONE locked weapon's
   verb. The day a Form needs a second silhouette, it is not a Form — it is a new class.** This is
   the single rule that keeps #80 intact and kills the art death-spiral. Non-negotiable.
2. **One Form per run, max.** A Form is a rare/legendary DRAFT pick that costs a draft slot. Taking
   the Form means giving up a skill that run — that opportunity cost is what preserves depth.
   No loadout screen, no "equip 3 forms."
3. **A Form must bend exactly ONE axis (break-shape OR gate-timing OR execute-condition) and stay
   inside the class OWNS list.** A Form that only moves stat numbers is rejected at design review.
   A Form that grants an AVOIDS-list tag is rejected as cross-contamination.
4. **Cap at 2 Forms per class** for the full game (not Phase-1 demo). 10 classes x 2 Forms = 20
   total content units that reuse existing art and plug into the existing draft system — that is a
   depth multiplier on the 12-skill kits, not a breadth explosion. Phase-1 demo ships ZERO Forms
   (Warblade demo stays on its base verb; Forms are a Phase-2+ depth layer).
5. **Forms are run-discovered, not meta-bought.** Meta (Echoes) keeps its locked job: class
   unlocks + hub upgrades. The only acceptable meta touch is a hub option to make a discovered Form
   *appear in your draft pool* more often — influencing odds, never granting the Form directly.
6. **Naming discipline:** call them "Weapon Forms" / "Stances," never "weapons" or "variants," in
   all docs — the word "variant" is canon-poisoned by #80 and will read as a rule violation.
