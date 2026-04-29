#!/usr/bin/env python3
"""
SINIF SKILL HAVUZU — Derin Araştırma + Sentez
==============================================
Hedef:
  - 8 sınıfı 12 skille genişlet (4 yeni per class)
  - Tag/Etiket sistemini netleştir
  - 12→4 seçim mekanizmasını oturtur
  - Her sınıf için kesin havuz tablosunu yaz

Modeller: deepseek-r1:14b (reasoning) + qwen2.5:14b (synthesis)
"""

import sys, requests, os, time
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://127.0.0.1:11434/api/generate"
MODEL_REASON  = "deepseek-r1:14b"   # derin analiz + tasarım
MODEL_SYNTH   = "qwen2.5:14b"       # sentez + tablo üretimi

CIKTI = r"F:\Antigravity Projeler\2d roguelite\TASARIM\arastirma\SKILL_HAVUZU_FINAL.md"

GPU_OPTIONS_REASON = {
    "temperature": 0.6, "top_p": 0.9,
    "num_predict": 6000, "num_ctx": 8192,
    "num_gpu": 99, "repeat_penalty": 1.05,
}
GPU_OPTIONS_SYNTH = {
    "temperature": 0.4, "top_p": 0.85,
    "num_predict": 8000, "num_ctx": 8192,
    "num_gpu": 99, "repeat_penalty": 1.02,
}

# ─────────────────────────────────────────────────────────────────
# OYUN BAĞLAMI — Her promptta kullanılacak
# ─────────────────────────────────────────────────────────────────
BAGLAM = """
=== GAME CONTEXT (read carefully before answering) ===

GAME: "2D Roguelite" — Flat top-down 2D action roguelite
PLATFORM: PC/Steam | ENGINE: Unity 6.3, 2D URP | DEVELOPER: Solo dev

CORE CONCEPT:
  - MMORPG dual-class build crafting (Guild Wars 1 philosophy)
  - Slay the Spire skill acquisition (room-by-room, 3 offers → pick 1)
  - Hades room structure (hub → dungeon → boss)
  - Grudge nemesis (enemies remember how you killed them)
  - "This build is insane" moment = MMORPG rotation click + roguelite format

CURRENT 8 CLASSES:
  1. Warblade   — Rage (0-100, fills by dealing+taking dmg, decays idle)
  2. Elementalist — Mana (0-100, slow regen) + Fire/Frost State alternation
  3. Rogue      — Energy (fast regen) + Combo Points (0-5, builder→finisher)
  4. Ranger     — NEW: Focus (0-100, fills when 4m+ from enemy, decays close)
  5. Brawler    — Fury (fills ONLY by TAKING damage, HP-inverse bonus)
  6. Paladin    — Holy Power (0-100, rhythmic builder→spender)
  7. Summoner   — Charges (0-4, auto +1/8s, sacrifice minions = instant charge)
  8. Hexer      — Hex Stacks per enemy (0-10), with PHASE SYSTEM:
                   0-3: Debuff | 4-6: Pressure | 7-9: Overload | 10: Hexblast

SKILL ACQUISITION (roguelite model):
  - Run start: 0 skills
  - Each room: 3 offers shown → player picks 1
  - Max slots: 6 active + 2 passive + 1 ultimate
  - Pool per class: EXPANDING from 8 → 12 skills
  - Player NEVER sees all 12 at once — they see 3 per room

CURRENT SKILL STRUCTURE PER CLASS:
  8 active | 4 passive | 2 ultimate = 14 per class × 8 classes = 112 base skills
  + 28 cross-class ultimates
  EXPANDING TO: 12 active | 4 passive | 2 ultimate = 16 per class

TAG SYSTEM (proposed — being finalized):
  ⚓ ANCHOR   — standalone power, no setup needed
  → OPENER    — best as first in a combo sequence
  ⚡ CHAIN    — gets bonus when used after specific skill
  ↑ BUILDER   — generates resource / creates setup
  ↓ SPENDER   — consumes resource / cashes out setup
  💥 FINISHER — conditional massive damage
  ⬡ CONTROL  — CC (stun, root, slow, knockback, silence)
  ✦ AMPLIFIER — makes other skills more powerful
  [Each skill gets 1-2 tags max]

REFERENCE GAMES (for DNA/inspiration):
  MMORPGs: WoW (Warrior/Mage/Rogue/Hunter/Paladin/Warlock/DK),
           FFXIV (Black Mage/Paladin combos), GW1 (dual profession),
           BDO, Lost Ark (Identity gauge), ArcheAge, Throne & Liberty
  Roguelites: Hades 1+2, Slay the Spire, Dead Cells, Risk of Rain 2,
              Enter the Gungeon, Balatro, Curse of the Dead Gods
  ARPGs: Diablo 2/4, Path of Exile 1/2, Nioh 2

DESIGN RULES:
  1. "Good skill" = creates a reason to play differently
  2. Complexity sweet spot: "understood in 1 read, surprised first time it procs"
  3. Each 4-skill selection naturally has: 1-2 ANCHOR + 1 OPENER + 1-2 CHAIN
  4. No flat stat inflation — prefer conditional bonuses and proc chains
  5. Solo dev scope: implementations must be feasible in Unity 2D
=== END CONTEXT ===
"""

# ─────────────────────────────────────────────────────────────────
# BÖLÜMLER
# ─────────────────────────────────────────────────────────────────
BOLUMLER = [

    # ── BÖLÜM 1 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 1 — 12→4 SEÇİM MİMARİSİ: Hangi Sistem Bu Oyuna Uyar?",
        "model": MODEL_REASON,
        "opts":  GPU_OPTIONS_REASON,
        "prompt": BAGLAM + """
TASK: Design the definitive skill selection architecture for this game.
The pool expands to 12 active skills per class. Player picks 4-6 over a run.

Analyze these systems and adapt — do NOT copy, SYNTHESIZE:

A) SLAY THE SPIRE model: see 3 cards per room, pick 1, never see full pool
   Pros: no overwhelm, natural discovery
   Cons: RNG can give bad offers, no agency over what you see

B) HADES model: each "vendor" (god) has a specific offer set, reroll available
   Pros: themed offers, can reroll once, some predictability
   Cons: less surprise, boon god rotation feels mechanical

C) GUILD WARS 1 model: pick ALL 8 skills before the mission in a skill panel
   Pros: full agency, build planning is itself a game
   Cons: analysis paralysis with 140 skills; doesn't fit roguelite run structure

D) DEAD CELLS model: unlock blueprints permanently, they enter the drop pool
   Pros: progression feels meaningful, meta-layer
   Cons: early runs feel empty

E) LOST ARK model: 20-25 skills, equip 8 on hotbar, can swap between runs
   Pros: huge customization, Tripod system per skill
   Cons: too complex for roguelite pacing

For THIS GAME specifically (dual-class, 12 skills per class, room-by-room):

Design a HYBRID system that answers:
1. Do players choose any starting skills? How many? How shown?
2. How are 3 room offers weighted? (how many from each class? neutral?)
3. Early vs late room biasing? (simpler skills early, complex late?)
4. When slot 6 is full: auto-upgrade only, or drop+replace option?
5. How does the tag system (ANCHOR/CHAIN/etc) integrate into the UI?
6. What PREVENTS the player from getting a pure-random bad run with no synergy?

Output format:
### SYSTEM NAME: [give it a name]
### RULE SET: [numbered rules, 1-10]
### ROOM OFFER WEIGHTS: [table]
### UI FLOW: [step by step what player sees]
### WHY THIS WORKS: [3 bullet design arguments]
""",
    },

    # ── BÖLÜM 2 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 2 — TAG SİSTEMİ: Nasıl Çalışır, Nasıl Görünür?",
        "model": MODEL_REASON,
        "opts":  GPU_OPTIONS_REASON,
        "prompt": BAGLAM + """
TASK: Finalize and validate the 8-tag system for this game's skills.

Proposed tags:
  ⚓ ANCHOR   — standalone power, no setup needed
  → OPENER    — best as first in a combo sequence
  ⚡ CHAIN    — gets bonus when used after specific skill
  ↑ BUILDER   — generates resource / creates setup
  ↓ SPENDER   — consumes resource / cashes out setup
  💥 FINISHER — conditional massive damage (HP threshold, stack threshold, etc)
  ⬡ CONTROL  — CC: stun, root, slow, knockback, silence, fear
  ✦ AMPLIFIER — makes other skills/damage types stronger

Analyze this tag system against real games:
1. Slay the Spire: Attack/Skill/Power/Curse + keywords (Exhaust, Retain, Innate)
   → What can we learn about communicating synergies without text walls?
2. Dead Cells: Brutality (red) / Tactics (purple) / Survival (green) color coding
   → How does color = build identity work?
3. Enter the Gungeon: "Synergizes with..." hidden discovery
   → Should some tag interactions be hidden until discovered?
4. Monster Hunter: "Can combo from A, can combo into B" directional arrows
   → How to show directionality of CHAIN skills?

Answer these questions:
Q1: Are 8 tags too many? Too few? What's the right number for immediate comprehension?
Q2: Should CHAIN tag show WHICH skill it chains with, or just "has chain bonus"?
Q3: Should some skills have HIDDEN tags that are revealed mid-run? (discovery mechanic)
Q4: How do tags interact with the dual-class system? (cross-class chains?)
Q5: In a 4-skill loadout, what's the IDEAL tag distribution for each class archetype?

Then: for each of the 8 classes, give the IDEAL 4-skill loadout's tag distribution:
Example:
  Warblade ideal 4: [OPENER][CONTROL] + [AMPLIFIER] + [FINISHER] + [ANCHOR]
  Elementalist ideal 4: ...

Output format:
### FINAL TAG COUNT AND DEFINITIONS: [revised if needed]
### TAG UI DESIGN: [how each tag looks and is communicated]
### IDEAL 4-SKILL TAG DISTRIBUTIONS: [table per class]
### DISCOVERY TAGS: [which tags/interactions should be hidden and when revealed]
""",
    },

    # ── BÖLÜM 3 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 3 — WARBLADE + ELEMENTALİST: 12'ye Genişletme",
        "model": MODEL_SYNTH,
        "opts":  GPU_OPTIONS_SYNTH,
        "prompt": BAGLAM + """
TASK: Expand Warblade and Elementalist from 8 to 12 active skills each.
Design 4 NEW skills per class. Then tag ALL 12 with the tag system.

=== WARBLADE (current 8) ===
1. Charge         [OPENER][CONTROL]  — 8m dash+stun, Rage+20
2. Mortal Strike  [CHAIN][FINISHER]  — big dmg+heal reduction, chain: Charge→100% reduction
3. Colossus Smash [AMPLIFIER]        — 6s window: all dmg sources +30%
4. Whirlwind      [ANCHOR][SPENDER]  — 2s spin AoE, consumes Rage
5. Shield Slam    [CONTROL]          — 100% knockback, wall=stun, Rage-20
6. Execute        [FINISHER]         — HP<30%: 400% dmg, chain: Mortal Strike→600%
7. Hamstring      [CONTROL][OPENER]  — 50% slow+bleed, chain: Charge→3s stun
8. War Stomp      [CONTROL][BUILDER] — 3m knockup 2s, Rage+25

Design 4 NEW Warblade skills that fill these gaps:
GAP A: No defensive repositioning (Charge is offensive, no repositioning tool)
GAP B: No persistent Rage-generating rotation skill (Whirlwind spends, nothing generates sustainably)
GAP C: No armor/defense debuff (Colossus Smash is dmg amplifier, not armor strip)
GAP D: No anti-magic / magic resistance mechanic (pure physical focus)

For each new skill, provide:
- NAME
- TAGS (1-2 from the tag system)
- EFFECT (2-3 lines, specific numbers)
- CHAIN BONUS (if CHAIN tagged)
- DNA (2-3 MMORPG sources it draws from)
- WHY IT FILLS THE GAP

=== ELEMENTALIST (current 8) ===
1. Fireball       [BUILDER][OPENER]  — fire DoT, Fire State builder
2. Frostbolt      [CONTROL][BUILDER] — slow+Mana regen, Brain Freeze proc
3. Living Bomb    [CHAIN][SPENDER]   — 5s delayed explosion, spreads on kill
4. Blink          [OPENER][ANCHOR]   — 6m instant teleport, next spell+20% dmg
5. Frozen Orb     [CONTROL][ANCHOR]  — slow-moving orb, chills path
6. Arcane Blast   [BUILDER][SPENDER] — escalating stack, 4th cast→Barrage
7. Meteor         [FINISHER][CONTROL]— 1.5s channel, big AoE, knockdown
8. Mirror Image   [ANCHOR]           — 2 copies, absorbs 1 hit each, death explosion

Design 4 NEW Elementalist skills that fill these gaps:
GAP A: No lightning/storm element (only Fire and Frost exist)
GAP B: No active Mana protection / shield
GAP C: No instant-cast burst that doesn't require setup (everything needs wind-up)
GAP D: No sustained ground AoE (Meteor is one-shot, Blizzard concept missing)

Output format for EACH class:
### [CLASS] — NEW SKILLS (4)
| # | Name | Tags | Effect | Chain Bonus | DNA |
|---|------|------|--------|-------------|-----|
... (fill table)

### [CLASS] — FULL 12 SKILL TABLE WITH TAGS
| # | Name | Tags | One-line summary |
|---|------|------|-----------------|
... (all 12)

### [CLASS] — BUILD AXES (3 viable 4-skill combos)
Build A "name": skills X+Y+Z+W — playstyle description
Build B "name": ...
Build C "name": ...
""",
    },

    # ── BÖLÜM 4 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 4 — ROGUE + RANGER: 12'ye Genişletme",
        "model": MODEL_SYNTH,
        "opts":  GPU_OPTIONS_SYNTH,
        "prompt": BAGLAM + """
TASK: Expand Rogue and Ranger from 8 to 12 active skills each.

=== ROGUE (current 8) ===
1. Backstab        [OPENER][BUILDER]  — positional: back = 200% dmg+3CP
2. Hemorrhage      [BUILDER]          — bleed DoT+2CP, spreads on kill
3. Rupture         [FINISHER][SPENDER]— 5CP finisher: big bleed, detonates existing bleed
4. Shadowstep      [OPENER][CONTROL]  — instant teleport to target, 0.5s stun
5. Kidney Shot     [CONTROL][SPENDER] — 5CP finisher: 4s stun
6. Ambush          [FINISHER][OPENER] — stealth-only: 300% dmg+4CP, longer stealth=more dmg
7. Fan of Knives   [ANCHOR][CONTROL]  — 360° instant AoE, spreads all active bleeds/debuffs
8. Evasion         [ANCHOR][BUILDER]  — 4s 100% dodge, each dodge=+1CP, kill=CD reset

Design 4 NEW Rogue skills:
GAP A: No poison (distinct from bleed — different DoT type, different interactions)
GAP B: No directional mobility (Shadowstep=targeted, need directional burst movement)
GAP C: No cooldown reset utility (WoW Preparation is iconic — where's the Rogue's "reset all"?)
GAP D: No hard counter to being surrounded/cornered (Evasion helps but no "get out now" panic)

=== RANGER (current 8 + new Focus resource) ===
Focus resource: fills when 4m+ from enemy (+10/s), decays when close (-20/s)
Focus 75+: arrows +25% dmg | Focus 100: next skill free cast

1. Aimed Shot      [FINISHER][CHAIN]  — 1.5s charge → big dmg, instant if target immobile
2. Concussive Arrow[CONTROL][OPENER]  — instant knockback+root 2s
3. Serpent Sting   [BUILDER][OPENER]  — poison DoT 10s + armor debuff, refreshes
4. Explosive Trap  [CONTROL][BUILDER] — ground trap, chain-triggers multiple traps
5. Multi-Shot      [ANCHOR][BUILDER]  — piercing AoE, Serpent Sting on all, 5hits→CD-3s
6. Disengage       [OPENER][CONTROL]  — 6m backflip, slow zone left behind
7. Black Arrow     [BUILDER][CHAIN]   — DoT→killed enemy spawns spirit (Summoner dual=minion)
8. Volley          [CONTROL][ANCHOR]  — 3s rain AoE on marked zone, slow+tick

Design 4 NEW Ranger skills:
GAP A: No sustained rapid-fire channeled attack (Aimed Shot=charged, Volley=AoE — no direct rapid fire)
GAP B: No companion/pet (Black Arrow is DoT→spirit, no persistent pet)
GAP C: No close-range desperation skill (being trapped at <2m is dangerous with no answer)
GAP D: No vision/utility (no Flare, no detection, no tactical info skill)

Output format: same as previous section (tables + build axes)
""",
    },

    # ── BÖLÜM 5 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 5 — BRAWLER + PALADİN: 12'ye Genişletme",
        "model": MODEL_SYNTH,
        "opts":  GPU_OPTIONS_SYNTH,
        "prompt": BAGLAM + """
TASK: Expand Brawler and Paladin from 8 to 12 active skills each.

=== BRAWLER (current 8) ===
Fury fills ONLY from taking damage (+15/hit). HP-inverse: lower HP = faster Fury gain.
1. Bloodlust Strike [CHAIN][FINISHER] — HP-scaled dmg (30HP=+120%), chain: Fury80%→Slaughter unlocks
2. Whirlwind        [ANCHOR][SPENDER] — 2s spin, each hit reduces own defense (max -30%)
3. Frenzied Leap    [OPENER][BUILDER] — jump to target, CD resets on hit, 3x consecutive→Frenzy5s
4. Reckless Swing   [FINISHER][SPENDER]— huge single hit, 2s fully vulnerable window
5. Bloodthirst      [ANCHOR][BUILDER] — 5 fast hits, each heals, HP-low=more heal
6. Intimidating Shout[CONTROL]        — 3m panic 3s, chain: panicked enemy hit→back stab counts
7. Barbaric Charge  [OPENER][CONTROL] — CC-immune dash, everything in path pushed, wall=stun
8. Last Rites       [FINISHER]        — HP<15% only: 600% dmg, 4s vulnerable after

Design 4 NEW Brawler skills:
GAP A: No grab/throw mechanic (Brawler should be able to grab and reposition enemies)
GAP B: No armor break (Shatter Armor as concept — Brawler punches through defenses)
GAP C: No Fury speed control (currently: Fury fills slow unless you get hit; need a way to accelerate)
GAP D: No "temporary immunity then big counter" mechanic (Evasion-like but Brawler-themed)

=== PALADIN (current 8) ===
Holy Power (0-100): builder skills fill it, spender skills use it at 100.
1. Crusader Strike  [BUILDER][OPENER]  — basic melee+HP+25, chain: 3-strike pattern→+60% dmg
2. Divine Storm     [BUILDER][AMPLIFIER]— 360° melee AoE, HP+15/target (3+targets=HP+50)
3. Judgment         [BUILDER][CHAIN]   — ranged holy blast, debuffed enemy=+50% dmg
4. Consecration     [BUILDER][ANCHOR]  — sacred ground 5s, tick dmg+HP+5/s while standing
5. Hammer of Wrath  [FINISHER][CHAIN]  — HP<20% targets only, HP+30, chain: Execute(Warblade)
6. Avenger's Shield [CONTROL][BUILDER] — bouncing shield, 3 targets, each=silence+HP+15
7. Holy Shock       [ANCHOR][BUILDER]  — dual use: enemy=dmg+HP+15 / self=heal(×3 if HP<30%)
8. Shield of Retribution[CHAIN][SPENDER]— 3s block, blocked dmg released as AoE (Consecration=2×)

Design 4 NEW Paladin skills:
GAP A: No sustained blessing/buff that persists (everything is active combat — no lasting passive buff from skills)
GAP B: No emergency self-heal large enough to matter in a pinch (Holy Shock heals small)
GAP C: No crowd management vs many enemies simultaneously rushing (Avenger's Shield hits 3, Consecration is ground — no mass push/repel)
GAP D: No "anti-minion / anti-undead" utility (thematic: Paladin vs Summoner — where's Turn Undead?)

Output format: same (tables + build axes)
""",
    },

    # ── BÖLÜM 6 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 6 — SUMMONER + HEXER: 12'ye Genişletme",
        "model": MODEL_SYNTH,
        "opts":  GPU_OPTIONS_SYNTH,
        "prompt": BAGLAM + """
TASK: Expand Summoner and Hexer from 8 to 12 active skills each.
Both classes have tempo issues in short roguelite fights — new skills must help.

=== SUMMONER (current 8 + sacrifice philosophy) ===
Charges (0-4, +1/8s auto, minion death=+1 instant). NEW PHILOSOPHY: sacrifice=power.
1. Raise Skeleton   [BUILDER][OPENER]  — 1Charge→melee skeleton (max 3), 3 together→Rally+40%
2. Summon Golem     [ANCHOR][CONTROL]  — 2Charges→1 big Golem, blocks path, HP<20%=self-explodes
3. Rally Cry        [AMPLIFIER]        — all minions +20% dmg+speed 10s (mixed types=+40%)
4. Corpse Explosion [FINISHER][CHAIN]  — detonate corpse→AoE, 3+ corpses=chain reaction
5. Death Nova       [CHAIN][CONTROL]   — sacrifice 1 minion→8s poison cloud (Hexer dual=spread debuffs)
6. Commanding Strike[AMPLIFIER][CONTROL]— order minion: 4× dmg attack+invuln (no minion=Summoner self-hits 2×)
7. Blood for Power  [BUILDER][SPENDER] — sacrifice minion→Charge+1+CD-30% all skills
8. Bone Shield      [ANCHOR][BUILDER]  — 3s absorption using minion as shield, absorbed dmg=Charge+1

Design 4 NEW Summoner skills:
GAP A: No ranged skeleton (all summons are melee; need ranged minion type for tactical diversity)
GAP B: No emergency self-sustain when minions are all dead (completely helpless without Charges)
GAP C: No "transformation" of summoner into combat mode temporarily (Lich Form concept — Summoner enters melee)
GAP D: No HP-to-Charge conversion (Dark Pact concept — pay HP to get Charges when resource dry)

=== HEXER (current 8 + phase system) ===
Hex Stacks per enemy (0-10). Phase thresholds: 0-3 Debuff | 4-6 Pressure | 7-9 Overload | 10 Hexblast
Each phase transition gives a small bonus feedback.
1. Corruption       [OPENER][BUILDER]  — instant 3 stacks, 4s moderate DoT
2. Agony            [BUILDER]          — slow-applying DoT, 2 stacks/tick (continuous)
3. Pandemic         [AMPLIFIER][CHAIN] — copy ALL stacks from one target to all nearby
4. Hexblast         [FINISHER][SPENDER]— 10-stack detonation, CD resets on kill, chain-explodes
5. Empathy          [CHAIN][CONTROL]   — curse: enemy attacks return 30% dmg to self
6. Haunt            [BUILDER][CONTROL] — attach ghost: follows+ticks+3stacks, ends at 10 stacks auto-Hexblast
7. Unstable Affliction[FINISHER][CHAIN]— if dispelled/healed→instant explode (stun). Anti-heal skill
8. Enervate         [CONTROL][AMPLIFIER]— -50% move/-40% atk speed 10s, 5+stacks=duration×2

Design 4 NEW Hexer skills:
GAP A: No mass apply option (applying stacks one enemy at a time is slow in groups)
GAP B: No interrupt/silence hex (Hexer should be able to stop enemy special abilities)
GAP C: No self-protection hex (Hexer is glass cannon — no defensive option)
GAP D: No high-risk instant stack dump (Soul Bargain: sacrifice HP for instant stacks)

Output format: same (tables + build axes + note on tempo fix with new skills)
""",
    },

    # ── BÖLÜM 7 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 7 — CROSS-CLASS SINERJILER: 12 Havuzunda Yeni Etkileşimler",
        "model": MODEL_REASON,
        "opts":  GPU_OPTIONS_REASON,
        "prompt": BAGLAM + """
TASK: With 12 skills per class (4 new skills added), identify the most exciting
NEW cross-class synergies that didn't exist before.

Focus on these new skill interactions (the new skills from previous sections):

WARBLADE new: Heroic Leap, Rallying Cry, Rupture Strike, Last Man Standing
ELEMENTALIST new: Chain Lightning, Mana Shield, Combustion, Blizzard
ROGUE new: Deadly Poison, Sprint, Preparation, Vanish
RANGER new: Rapid Fire, Spirit Wolf, Flare, Point Blank
BRAWLER new: Iron Grab, War Cry, Shatter Armor, Death Wish
PALADIN new: Blessed Weapon, Lay on Hands, Devotion Aura, Divine Sacrifice
SUMMONER new: Raise Archer, Sacrificial Pact, Dark Pact, Lich Form
HEXER new: Mass Hex, Silence Hex, Cursed Mirror, Soul Bargain

Find the TOP 10 most interesting cross-class interactions involving at least 1 new skill.

For each interaction:
1. COMBO NAME: (evocative title)
2. CLASSES: which two classes
3. SKILLS INVOLVED: specific skill names
4. WHAT HAPPENS: the actual mechanic interaction
5. WHY IT'S "INSANE": the emotional peak moment
6. BUILD RISK: what makes this hard to execute
7. RATING: S/A/B tier

Also identify:
- 3 interactions that seem broken and need BALANCING NOTES
- 2 interactions that create a "hidden" surprise combo (not obvious until discovered)
- The best new combo for Plague Doctor (Summoner+Hexer) with the new skills

Output as a well-formatted table + detailed descriptions for top 3 interactions.
""",
    },

    # ── BÖLÜM 8 ─────────────────────────────────────────────────
    {
        "baslik": "BÖLÜM 8 — FİNAL SENTEZ: Kesin Sistem Dokümanı",
        "model": MODEL_SYNTH,
        "opts":  GPU_OPTIONS_SYNTH,
        "prompt": BAGLAM + """
TASK: Write the DEFINITIVE system document for the 12-skill pool expansion.
This is the final design document that will replace the old 8-skill system.

Structure it as:

### 1. SYSTEM OVERVIEW
- Why 12 skills per class (vs 8)
- The selection architecture (hybrid model)
- Tag system summary

### 2. SELECTION FLOW — STEP BY STEP
How a player goes from "run start" to "4 active skills equipped" over 4-6 rooms.
Include: what they see at run start, how offers are weighted, when complex skills appear.

### 3. TAG SYSTEM — FINAL DEFINITIONS
All tags with: symbol, color, 1-line definition, example skill per class.

### 4. POOL STRUCTURE PER CLASS (summary table)
For each of the 8 classes:
| # | Name | Tags | Type | Key Interaction |
Where Type = Core/Advanced/Mastery based on complexity

### 5. BUILD DIVERSITY MATH
- How many viable 4-skill combos exist per class (estimate)?
- How does dual-class multiply this?
- Why 12>8 for replayability

### 6. IMPLEMENTATION NOTES (solo dev, Unity 2D)
- What's the minimum viable implementation for FAZ 2?
- Which features can be added later without redesign?
- Tag system in Unity: ScriptableObject design suggestion

Write this as a clean, final design reference document.
Use tables where helpful. Be concrete and specific.
This document will be used directly to build the game.
""",
    },
]


# ─────────────────────────────────────────────────────────────────
def ollama_sor(prompt: str, model: str, opts: dict) -> str:
    payload = {"model": model, "prompt": prompt, "stream": False, "options": opts}
    try:
        r = requests.post(OLLAMA_URL, json=payload, timeout=1800)
        r.raise_for_status()
        raw = r.json().get("response", "").strip()
        # deepseek'in <think> bloklarını temizle
        if "<think>" in raw and "</think>" in raw:
            raw = raw[raw.rfind("</think>") + len("</think>"):].strip()
        return raw
    except Exception as e:
        return f"[HATA: {e}]"


def main():
    toplam = len(BOLUMLER)
    print("\n" + "=" * 65)
    print(f"  SKILL HAVUZU ARAŞTIRMASI — {toplam} Bölüm")
    print(f"  Modeller: {MODEL_REASON} + {MODEL_SYNTH}")
    print(f"  Çıktı: {CIKTI}")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print(f"  Tahmini süre: {toplam * 8}–{toplam * 15} dakika")
    print("=" * 65)

    with open(CIKTI, "w", encoding="utf-8") as f:
        f.write(f"# SINIF SKILL HAVUZU — FİNAL TASARIM\n")
        f.write(f"*{datetime.now().strftime('%Y-%m-%d %H:%M')} | 8 Sınıf × 12 Skill + Tag Sistemi*\n\n")
        f.write(f"---\n\n")

    basari = 0
    for i, b in enumerate(BOLUMLER, 1):
        model_kisa = b["model"].split(":")[0]
        print(f"\n[{i:02d}/{toplam}] {datetime.now().strftime('%H:%M')} [{model_kisa}]")
        print(f"         {b['baslik']}")
        print(f"         Düşünüyor...", end="", flush=True)
        t0 = time.time()

        cikti = ollama_sor(b["prompt"], b["model"], b["opts"])
        sure = time.time() - t0

        if cikti.startswith("[HATA"):
            print(f" ✗ ({sure:.0f}s)\n         {cikti}")
        else:
            print(f" ✓  ({sure:.0f}s, {len(cikti):,} karakter)")
            basari += 1

        with open(CIKTI, "a", encoding="utf-8") as f:
            f.write(f"## {b['baslik']}\n\n")
            f.write(cikti)
            f.write("\n\n---\n\n")

    print(f"\n{'=' * 65}")
    print(f"  TAMAMLANDI: {basari}/{toplam} bölüm başarılı")
    print(f"  Süre: {datetime.now().strftime('%H:%M')}")
    print(f"  Dosya: {CIKTI}")
    print("=" * 65)


if __name__ == "__main__":
    main()
