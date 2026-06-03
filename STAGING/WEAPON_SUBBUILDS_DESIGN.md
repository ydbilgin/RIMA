# WEAPON SUB-BUILDS — Per-Class Design (Opus, APPROVED, overrides Decision #80)

Status: APPROVED by canon owner. The owner was warned this conflicts with locked Decision #80
("1 class = 1 weapon = 1 silhouette, variant YOK") and INSISTED. This design therefore
**supersedes #80** for the weapon-silhouette-immutability clause ONLY. Everything else in #80/#72
(class identity = its verb, no cross-class tag leakage) is PRESERVED and used as the guardrail below.

User intent (verbatim): "Give characters OPTIONAL weapon choices as a CHOICE; which ones should
they be — embed these into the characters, like a SUB-BUILD under the character." + "skills evolve
based on weapon." Examples: Ranger bow -> crossbow -> other; Gunslinger revolver -> shotgun -> faster.

---

## 0. WHAT THIS IS (the model in one paragraph)

Each class owns **2-3 WEAPON SUB-BUILDS**: a **base weapon** plus **1-2 alternates**. A sub-build is
chosen **at character-select / loadout, BEFORE the run starts, and is locked for the whole run** (it
is NOT a mid-run draft pick and NOT swappable on the fly). The chosen weapon is a **visibly different
held sprite** (the user wants real weapon choices, not invisible re-skins) AND it **re-shapes how the
class's 4 run-skills (Q/E/R/F) BREAK and EXECUTE** — never a flat stat boost. The skill draft (the
12-skill pool + secondary class) stays the in-run variety engine; the weapon sub-build is the
**pre-run identity branch** that re-tunes the verb your whole draft then builds on.

The load-bearing principle carried over from all prior research:
**"a weapon changes HOW you break and execute, not the damage numbers."** A sub-build is legitimate
ONLY if it bends ONE axis of the Sundered-Beat:
- **BREAK SHAPE** — one deep break vs. many shallow breaks (bow's single heavy bolt vs. shotgun cone).
- **EXECUTE TIMING** — the gate opens slower-but-bigger vs. faster-but-briefer.
- **EXECUTE CONDITION** — which break-state the execute consumes (always inside the class OWNS tags;
  a sub-build may NEVER grant an AVOIDS-list tag — no weapon gives Warblade bleed).

A sub-build that only moves damage/speed/range is rejected at design review as noise.

### Why this is feasible (the reframe that dissolves the old art death-spiral)
RIMA weapons are NOT baked into the body. Per the locked HandAnchor system (`HandAnchorAttach.cs`,
`WeaponDatabaseSO.cs`), the weapon is a **separate sprite** parented to a hand anchor and oriented
8-dir **by code** (`OrientationSync`). The mount API is already form-aware:
`HandAnchorAttach.AttachWeapon(string formId)` and `WeaponDatabaseSO.GetWeapon(classId, formId)`.
So a new weapon sub-build = **ONE new ~64px weapon sprite + a data row + skill conditionals**, NOT a
10x character resprite. The body/silhouette stays the same; only the held weapon and the verb change.
Decision #80 predates this HandAnchor split (#123/#144), which is exactly why the old art objection
no longer holds.

### Scope discipline (depth, not breadth)
- **Cap: 3 sub-builds per class MAX** (base + 2). 10 classes x avg ~2.4 = the budget below.
- Each alternate must be a **real identity branch** (a different way to play the class), never a
  "+5% faster" dial. If two alternates feel the same, cut one.
- **Casters/weaponless classes** (Elementalist, Summoner, Hexer, Brawler) get sub-builds expressed as
  their **focus object / stance**, not a swung weapon — see their rows. These cost the least art.

---

## 1. PER-CLASS WEAPON-OPTIONS TABLE (the "hangileri olmalı" answer)

Format per row: **Name** — feel — how Q/E/R/F evolve (axis bent). Base is always first.
OWNS tags are the guardrail; alternates stay inside them.

### 1. Warblade — greatsword (OWNS: armor-shred / sundered / broken / iron / verdict)
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Greatsword (base)** | Pin one, break its guard, execute. | Q/E build Broken on a single target; R Death Blow consumes Broken (single-target verdict); F single-line surge. |
| **Warhammer / Maul** | Slow, seismic, crowd-cracker. | BREAK SHAPE one→many: combo's 3rd hit becomes an overhead that applies Broken to a small frontal CONE; R Death Blow now needs TWO Broken targets stacked to fire — herd two, break the wall, execute the wall. |
| **Sword-and-Shield** | Stand the line, punish on the read. | EXECUTE CONDITION: a guard-stance replaces the dash; Broken applies only after you absorb/deflect a hit, then R executes the staggered attacker. Defensive Warblade. |

### 2. Elementalist — floating Rune Disc (OWNS: fire/frost/lightning/earth/brand/rotation; NO STAFF/WAND, #146)
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Rune Disc (base)** | Rotate through elements, empowered 3rd beat. | Q/E rotate the active element; R empowered cast brands per current element; F is the rotation reset. |
| **Twin Orbs (split disc)** | Two elements held at once, dual-brand. | BREAK SHAPE: holds TWO elements simultaneously; R applies a dual-brand (e.g. ignite + freeze) that breaks on the SECOND element's tick — wider, combo-dependent break. (Off-hand orb pair sprite, still no staff.) |
| **Sigil Stone (anchored)** | Plant your element, zone control. | EXECUTE TIMING: drops a stationary elemental sigil; the empowered beat fires from the sigil, not you — slower setup, holds the brand window open longer. Stationary-caster branch. |

### 3. Shadowblade — twin daggers (OWNS: veil/scar/phase/shadow/echo)
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Twin Daggers (base)** | Many scattered scars, fast execute. | Q/E phase leaves scattered Scars; R Severance consumes multiple shallow Scars. |
| **Reverse-grip Single Dagger** | One deep cut, single-target assassin. | BREAK SHAPE many→one: phase leaves ONE DEEP Scar; R Severance needs only 1 deep Scar but phase recovery is longer. High-risk single-target executioner. |

(Shadowblade capped at 2 — no second alternate that doesn't leak Ranger distance or Ravager bleed.)

### 4. Ranger — compound bow (OWNS: distance/trap/mark/precision/tripwire)
| Sub-build | Feel | Skill evolution (axis) — THE user's bow→crossbow example |
|---|---|---|
| **Compound Bow (base)** | Tap cadence: mark, lay trap, detonate. | Q mark, E trap, R Final Strike (gated on Marked AND Trapped). |
| **Crossbow / Heavy Arbalest** | One disciplined heavy bolt. | BREAK SHAPE: tap-cadence → ONE charged heavy bolt that applies BOTH Marked AND a pin-in-place Trapped on impact; one shot opens the whole gate, but draw is long and Focus drains if rushed. The crossbow FEEL, delivered as a real different weapon. |
| **Recurve / Volley Bow** | Area-denial, many shallow marks. | BREAK SHAPE one→many: rapid spread applies shallow Mark across a cone; R re-routes to "many Marked + one tripwire" → suppression sniper. |

### 5. Ravager — dual compact axes (OWNS: bleed/hook/aggression/carnage/rend)
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Dual Axes (base)** | Frenzied bleed, low-HP danger. | Q/E stack Wounded/bleed; R Blood Verdict on bleeding targets. |
| **Hook / Harpoon** | Yank them out of formation. | EXECUTE CONDITION + movement geometry: the hook becomes the break — yank a guard, Wounded applies on the pull; R Blood Verdict opens on "hooked + bleeding." |
| **Two-Handed Cleaver** | Reckless, build-by-taking-damage. | EXECUTE TIMING: removes recovery frames; bleed stacks build faster the lower your HP — pushes the take-damage-to-build rhythm harder. |

### 6. Ronin — katana + sheath (OWNS: iaido/stillness/opened/sheathe/precision) (#71 sheath/draw is canon)
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Katana (base)** | Draw, cut, sheathe — one breath. | Tension builds on iaido cycle; R Final Draw on full Tension. |
| **Nodachi (long-sheathe)** | Slower, all-or-nothing iaijutsu. | EXECUTE TIMING: widens the parry/deflect window, but Final Draw now needs FULL Tension AND a held sheathe — a heavier, single decisive cut. Deepens the wait-draw-punish verb. |

(Ronin capped at 2 — dual-wield leaks Shadowblade; no-sheathe destroys iaido identity. Both rejected.)

### 7. Gunslinger — revolver (OWNS: heat/reload/burst/vent/bullet) — THE user's revolver→shotgun→faster example
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Revolver (base)** | Heat rhythm, single exposed-line execute. | Q/E build Heat + Suppressed; R Deadshot down a single exposed line. |
| **Sawed-Off Shotgun** | Close-range crowd shredder. | BREAK SHAPE: a close-range vent-blast applies Suppressed to EVERY enemy in a short CONE in one beat and dumps Heat fast; Deadshot re-routes to "3+ Suppressed in cone" — close-range crowd-execute. The "shotgun" the user named, as a real weapon. |
| **Fan-Hammer / Quickdraw Revolver** | Empty the gun, feast-or-famine. | EXECUTE TIMING ("faster gun"): dumps the whole chamber in one beat for instant max Heat; Deadshot opens on "0 bullets + last shot landed." The "faster" feel = Heat timing, expressed as a visibly lighter quickdraw revolver. |

### 8. Brawler — fists/gauntlets (OWNS: cracked/shattered/brawl/counter/crack) — weaponless; sub-build = GAUNTLET
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Bare Fists (base)** | Momentum combo, crack→shatter. | Q/E jab combo builds Cracked; R shatter on a Cracked target. |
| **Heavy Gauntlets** | Slower, harder, single big crack. | BREAK SHAPE: combo trades speed for a single overhand that Cracks deeply; shatter window is bigger but rarer. (Gauntlet IS a visible held/worn sprite — the only "weapon" Brawler can wear without killing the bare-knuckle identity.) |
| **Counter-Stance (no new sprite)** | Defensive read-and-punish. | EXECUTE CONDITION: every 4th beat is a parry window; Cracked→Shattered only off a successful counter. Pure stance — reuses bare-fist art. |

### 9. Summoner — Soul Lantern (OWNS: soul/summon/sacrifice/minion/bond; NO swing) — Charges 0-4, Mark & Sic
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Soul Lantern (base)** | Mark with Bone Spike, Sic on commit-beat. | LMB marks, commit-beat Sics minions to the marked target; R sacrifices for burst. |
| **Bone Censer (beacon)** | The light itself is the break. | BREAK SHAPE of the command cadence: the censer projects a "Sic zone"; the commit-beat sacrifices a marked minion to BREAK every enemy in the zone — sacrifice-as-area-break. Pure Summoner OWNS. |

(Summoner capped at 2 — no melee weapon; "staff/scythe" rejected. The off-hand object varies, not a sword.)

### 10. Hexer — Grimoire / Cursed Scepter (OWNS: hexed/curse/stack/accumulation/spread) — Hex Stacks 0-10
| Sub-build | Feel | Skill evolution (axis) |
|---|---|---|
| **Grimoire (base)** | Stack curse deep on one target, detonate at 10. | Q Corruption stacks, R Hexblast at 10. |
| **Cursed Totem / Scepter** | Spread the plague wide-and-shallow. | BREAK SHAPE deep-single→wide-shallow: the commit-beat trades single-target stack depth for SPREAD; Hexblast at 10 jumps to neighbors at reduced stacks. Leans into the OWNS `spread` tag. |

(Hexer capped at 2 — "attack wand" that shoots dilutes the patience-curse identity; rejected.)

---

## 2. SELECTION MODEL — "sub-build under the character"

**Where:** the **character-select / loadout screen**, as a SECOND tier under the chosen class.
Flow: pick class → the class card expands to show its 2-3 weapon sub-builds as a row of cards →
pick one → it is **locked for the entire run**. Default = base weapon (one-click play for new players).

**Persistence:** chosen pre-run, immutable mid-run. This is deliberate and matches the user's
"embed into the character, like a sub-build" framing:
- It does NOT compete with the in-run skill draft (the draft stays the moment-to-moment variety
  engine). The sub-build sets the verb your draft then specializes.
- No mid-run weapon-swap → no build-identity whiplash, no "what was my draft even for" problem, no
  on-the-fly arsenal churn. The cost of choosing wrong is "play a different run," which is the
  correct roguelite stake.

**UI presentation (honor existing CharacterSelectController.cs):**
- Under the class portrait, a horizontal strip of 2-3 weapon cards: weapon icon + name + a ONE-LINE
  "feel" tag (e.g. Crossbow: "One heavy bolt — pins and marks in a single shot").
- Selected card highlighted in cyan (seal-energy), unselected dimmed. Locked-in confirmation on Play.
- Tooltip/hover shows the Q/E/R/F evolution deltas vs. base so the choice is legible.

**Secondary class interaction:** when the secondary class unlocks (Act 1 boss), it brings ITS base
weapon only for the demo/Phase-2 scope (sub-build selection for the secondary is a later layer). The
primary weapon sub-build is what defines the run's identity.

---

## 3. ART COST (total new weapon sprites)

Each sub-build weapon = **ONE ~64px PixelLab pixel weapon sprite**, 8-dir BY CODE via the existing
HandAnchor/OrientationSync mount. **No body resprite. No per-direction baking.** Casters' focus
objects and Ronin's sheath follow the same 1-sprite rule.

| Class | Base (exists?) | New sprites needed | Notes |
|---|---|---|---|
| Warblade | ✅ greatsword `441bccf0` | **2** (warhammer, shield+sword) | shield = small off-hand sprite |
| Elementalist | ❌ rune disc queued | **2** (twin-orb pair, sigil stone) | no staff/wand |
| Shadowblade | ❌ twin daggers queued | **1** (reverse-grip single) | could mirror-reuse dagger art |
| Ranger | ❌ bow queued (+3 bow V2 in cloud) | **2** (crossbow, recurve) | recurve may reuse a V2 bow variant → maybe **1 truly new** |
| Ravager | ❌ dual axes queued | **2** (hook/harpoon, cleaver) | |
| Ronin | ✅ katana `692f43ce` | **1** (nodachi) | reuses sheath logic |
| Gunslinger | ❌ dual pistols queued | **2** (sawed-off, fan-hammer revolver) | fan-hammer may recolor base revolver → maybe **1 truly new** |
| Brawler | ❌ fists/gauntlet queued | **1** (heavy gauntlet) | counter-stance = NO new sprite (reuses fists) |
| Summoner | ❌ soul lantern queued | **1** (bone censer) | off-hand object only, no swing |
| Hexer | ❌ grimoire queued | **1** (cursed totem/scepter) | |

**TOTAL NEW WEAPON SPRITES: 15** (with ~2-3 reusable/recolor from existing V2 cloud backups → as
few as **~12 truly-new gens**). Each is a single 1-direction PixelLab object the existing pipeline
already produces (`create_1_direction_object`, PPU64). This is the entire art bill — roughly the cost
of one class's animation set, spread across all 10 classes' depth. NO silhouette death-spiral.

Reuse flags: Ranger recurve (cloud has 3 bow V2 variants), Gunslinger fan-hammer (recolor of base
revolver), Shadowblade reverse-grip (flipX of a dagger). Brawler counter-stance and Warblade S&S
stance reuse base body; only the off-hand shield is new for S&S.

---

## 4. IMPL MODEL (data + binding + reuse) — cost S/M/L per piece

| Piece | What | Cost |
|---|---|---|
| **`WeaponSubBuild` SO** | New ScriptableObject: `classType`, `subBuildId` (== `formId`), display name, one-line feel, weapon icon, `formId` string that maps to the existing `WeaponDatabaseSO` row, and a list of **skill-variant overrides** (skillName → which Q/E/R/F variant to bind). | **S** |
| **WeaponDatabase rows** | Add a `WeaponEntry` per sub-build (`classId` + `formId` + `weaponPrefab` + offsets). The SO + `GetWeapon(classId, formId)` **already support this** — pure data entry per new sprite. | **S** |
| **Mount swap** | `HandAnchorAttach.AttachWeapon(formId)` **already exists and takes a formId**; called once at run start with the selected sub-build's id instead of hardcoded `"Base"`. Add a public re-sync so OrientationSync/weaponRenderer rebind after the swap (mostly there). | **S** |
| **Skill reads the weapon** | A `PlayerWeaponState` component holds the run's `subBuildId`. Each affected skill queries it (`player.CurrentSubBuild`) and branches its break-shape / gate-timing / execute-condition. Only the **1-2 skills per class** that the sub-build re-shapes need a conditional — NOT all 12. Pattern: the skill's `Execute()` switches on the sub-build id. | **M** |
| **Loadout selection** | Extend `CharacterSelectController.cs` with the sub-build strip; persist the choice into the run-start payload that wires the player prefab (alongside classId). | **M** |
| **Skill-variant binding** | Bind which Q/E/R/F behavior is active per sub-build. Cleanest: each affected `SkillBase` reads sub-build state at `Execute()` (decorator-by-query), NOT separate skill assets per weapon — keeps the canonical 12-skill pool intact and avoids the "12 x N weapons" content blowup. | **M** |

**Total impl: MEDIUM.** No new mount system, no draft-system surgery, no per-weapon skill-asset
explosion. The expensive half (visual mount, 8-dir-by-code, formId plumbing) **already exists**.
The real work is: 1 SO type, N data rows, 1 player-state component, 1-2 skill conditionals per class,
1 loadout UI strip.

**Reuse summary:** HandAnchorAttach (form-aware), WeaponDatabaseSO (classId/formId keyed),
OrientationSync (8-dir by code), CharacterSelectController (extend), the existing 12-skill kits
(query-decorate, do not duplicate). Honors: cursor-aim (unchanged), PixelLab pixel weapons,
Enemy-layer + Custom-Axis Y-sort (weapon sprite stays a child of the player, inherits sort).

---

## 5. WAVE-5 BUILD ORDER (Phase-2, after demo combat-depth + cross-class)

Phase-1 demo ships ZERO sub-builds (Warblade base only). This is a Phase-2 depth layer. Order:

1. **W5.0 — `WeaponSubBuild` SO + `PlayerWeaponState` + AttachWeapon(formId) wired at run start.**
   Plumbing only; no UI yet; default to base. (S+S) — proves the mount swap end-to-end.
2. **W5.1 — RANGER FIRST PROTOTYPE: crossbow sub-build.** Cleanest controller, mark/trap state
   already shared via SkillStateTracker; the user's headline example. One new sprite + 2 skill
   conditionals. Validate the full loop on ONE class before scaling. (M)
3. **W5.2 — Loadout UI strip in CharacterSelect** (Ranger base + crossbow selectable). (M)
4. **W5.3 — GUNSLINGER (shotgun + fan-hammer)** — the user's second named example; 2 sprites,
   proves multi-alternate selection. (M)
5. **W5.4 — Roll out remaining melee** (Warblade, Ravager, Ronin, Shadowblade) — 1-2 sprites each,
   reuse the proven skill-query pattern. (M each)
6. **W5.5 — Weaponless/caster sub-builds** (Elementalist twin-orb/sigil, Summoner censer, Hexer
   totem, Brawler gauntlet/stance) — lowest art cost, focus-object only. (S-M each)
7. **W5.6 — Tooltip evolution-deltas + balance pass** across all sub-builds; confirm each alternate
   is a real identity branch (cut any that test as a stat-dial). (M)

Gate to start W5: demo combat-depth (ChainWindowTracker / Sundered Beat) + cross-class must be live,
so sub-builds re-shape a verb that actually has depth to re-shape.

---

## 6. GUARDRAILS (carried from #80, still binding even though silhouette-lock is lifted)

1. **Cap 3 sub-builds/class. Each MUST be a real identity branch (different way to play), never a
   stat dial.** Two-feel-alike → cut one.
2. **A sub-build bends exactly ONE axis** (break-shape OR gate-timing OR execute-condition) and stays
   inside the class OWNS list. **No AVOIDS-tag leakage** (no Warblade bleed, no Ranger-distance daggers,
   no melee Summoner). This is the surviving heart of #80 — class identity is still sacred.
3. **One sprite per sub-build, 8-dir by code. The day a sub-build needs a body resprite, it's a new
   class, not a sub-build.** Keeps the art bill at ~15 sprites total.
4. **Chosen pre-run, locked for the run.** Not a draft pick, not mid-run swappable.
5. **Casters/Brawler/Summoner/Hexer vary the FOCUS OBJECT or STANCE, not a swung weapon.**
6. **Skills are query-decorated, not duplicated.** The canonical 12-skill pool stays one pool;
   sub-builds re-shape 1-2 of them via a runtime query — never "12 skills x N weapons" assets.
