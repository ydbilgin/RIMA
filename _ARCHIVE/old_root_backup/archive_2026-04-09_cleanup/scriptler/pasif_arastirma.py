#!/usr/bin/env python3
"""
Pasif + Cross-Class Araştırması
=================================
Oyunumuz: 8 sınıflı flat top-down 2D roguelite, dual-class sistemi
Araştırılan: 28 dual-class kombinasyon için cross-class pasifler,
             neutral pasif genişletme, seçilebilir skill havuzu tasarımı
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

BASE = r"F:\Antigravity Projeler\2d roguelite\TASARIM"
CIKTI_DOSYASI    = os.path.join(BASE, "PASIF_ARASTIRMA_OLLAMA.md")
HAM_VERI_DOSYASI = os.path.join(BASE, "PASIF_HAM.md")

GPU_OPTIONS = {
    "temperature":    0.7,
    "top_p":          0.9,
    "num_predict":    8000,
    "num_ctx":        4096,
    "num_gpu":        99,
    "repeat_penalty": 1.05,
}

FORMAT_ONEKI = """Structure your response in EXACTLY TWO PARTS:

### PART 1 — RAW DATA
List ALL facts, names, numbers in TABLE or BULLET format. No analysis.

### PART 2 — ANALYSIS
Design lessons, adaptations, notes.

Now answer:
---
"""

# ── OYUN BAĞLAMI ──────────────────────────────────────────────────────────────

BAGLAM = """
IMPORTANT INSTRUCTION: Do NOT just describe what other games do. GENERATE ORIGINAL IDEAS
by harmonizing inspiration from multiple sources. Each passive/skill you design should feel
like it could have come from a mashup of 2-3 games, but is NEW and fits our game.

SOURCE GAMES TO DRAW FROM (harmanlama kaynakları):
  MMORPGs: WoW (Arms Warrior, Fire Mage, Subtlety Rogue, BM Hunter, Prot Paladin, Demonology Lock),
           BDO (Black Desert), KO (Knight Online), GW1 (Guild Wars 1 - dual profession system),
           ArcheAge (120 class system), FFXIV (Black Mage rotation, Paladin combo), Lost Ark (Identity),
           Elsword (2D action combos), Throne & Liberty
  Roguelites: Hades 1+2 (boon synergies, keepsakes), Slay the Spire 1+2 (card synergies),
              Enter the Gungeon (item synergies, "synergizes" tag), Dead Cells (proc chains),
              Risk of Rain 2 (item stacking), Balatro (joker synergies), Vampire Survivors
  Action RPGs: Diablo 2/3/4 (proc builds), Path of Exile (passive tree depth), Nioh (ki pulse)
  Other: Shadow of Mordor (nemesis), Inscryption (dark mechanics)

DESIGN PHILOSOPHY:
  - "Good" passive = creates a reason to play differently
  - Cross-class passive = makes BOTH classes feel more alive together
  - Complexity sweet spot: "I understood it in 1 read, but it surprised me the first time it proc'd"
  - Avoid: stat inflation (+X% damage to everything)
  - Prefer: conditional bonuses, new interaction types, proc chains

GAME: Flat top-down 2D roguelite. Player picks 2 classes at run start, mixes their skill pools.
8 CLASSES (resource system):
  - Warblade  → Rage (fills by taking/dealing dmg, 0-100)
  - Elementalist → Mana (regen, Fire/Frost state alternation)
  - Rogue     → Energy (fast regen) + Combo Points (0-5, builder→finisher)
  - Ranger    → Cooldown-only (no resource bar, skill timing)
  - Berserker → Fury (fills ONLY by TAKING damage)
  - Paladin   → Holy Power (rhythmic 0-100, spend at 100)
  - Summoner  → Charges (fills over time, max 4)
  - Hexer     → Hex Stacks (per-enemy 0-10 stacks → Hexblast)

EXISTING CLASS PASSIVES (each class has 4, player picks 2):
  Warblade:  Bloodlust (kill→lifesteal), Iron Will (HP30%→dmg reduce), Juggernaut (Charge immune), Wrecking Ball (CC→+100% next hit)
  Elementalist: Fire Focus (fire hit→CD-2s), Burnout Recovery (take dmg→mana+8), Shatter (slow/frozen→+60% dmg), Temporal Flux (slowed enemy→+25% dmg from all)
  Rogue: Parry Riposte (perfect dodge→crit+energy), Shadow Focus (stealth speed+free cost), Venomous Wounds (bleed tick→energy), Murderous Intent (enemy HP35%→+30%dmg)
  Ranger: Expertise (all CD-15%), Dead Zone Mastery (enemy close→auto disengage), Marked Target (5 arrow hits→+30% dmg taken), Predator's Focus (stand still 2s→+20%dmg+crit)
  Berserker: Unyielding (HP70%+→dmg→Fury), Ruthless (HP30%→atk+lifesteal), War Machine (kill→Fury+40+invuln), Pain is Power (each 10% HP lost→+5% dmg, stacks)
  Paladin: Divine Focus (nearby enemy hit→CD-10%), Holy Endurance (take dmg→HP regen), Sanctified Wrath (HP80%+→+50% Holy Power gen), Light's Grace (spender→next builder free)
  Summoner: Army's Strength (per minion alive→+5% dmg), Blood for Power (minion dies→Charge+CD-30%), Undying Hunger (minion dmg→heals Summoner), Corpse Lord (minion kill→auto exploding corpse)
  Hexer: Debuff Threshold (5+stacks→+20% dmg), Punishing Hex (enemy attacks Hexer→2 stacks+ricochet), Lingering Curse (stack decay-50%), Soul Rend (10 stacks→HP regen blocked 8s)

WHAT WE NEED: For each dual-class combo, a CROSS-CLASS PASSIVE that:
  - Activates only when BOTH classes are active
  - Creates a synergy that neither class alone could achieve
  - Fits on 1-2 lines (roguelite tooltip length)
  - Has a thematic name reflecting the combo's identity
  - References the named archetypes: Runeguard, Shadow Saint, Phantom Hunter,
    Plague Caster, Fallen Saint, Plague Doctor, Blood Shepherd, Wild Hunter, etc.
"""

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — Tasarım Prensipleri: Cross-Class Pasif Nasıl Tasarlanır",
        "prompt": BAGLAM + """
Before designing the 28 cross-class passives, establish the design framework.

Research how multiclass synergy bonuses work in:
1. Guild Wars 1: secondary profession synergies — how do they create unique combos?
2. ArcheAge: 120 class combinations — how are synergies designed? What makes them feel distinct?
3. Pathfinder archetypes: how do archetype passives interact between classes?
4. Neverwinter Nights: feat synergies across multiclassing
5. Final Fantasy Tactics: job synergies and ability inheritance

Design rules to establish:
- How strong should a cross-class passive be? (vs a class-specific passive)
- Should it reference BOTH class mechanics or create a NEW mechanic?
- Should it be conditional (require specific skills to be equipped) or always active?
- Should there be multiple tiers (bonus at 2 skills from each class, stronger at 3)?
- How do you avoid cross-class passives making some combos feel mandatory?
"""
    },
    {
        "baslik": "BOLUM 02 — Warblade Cross-Class Pasifleri (7 combo)",
        "prompt": BAGLAM + """
Design cross-class passives for ALL 7 Warblade combinations:

WARBLADE + ELEMENTALIST (Runeguard archetype)
WARBLADE + ROGUE (Iron Shadow archetype)
WARBLADE + RANGER (Vanguard archetype)
WARBLADE + BERSERKER (Juggernaut archetype)
WARBLADE + PALADIN (Sentinel archetype)
WARBLADE + SUMMONER (Death Commander archetype)
WARBLADE + HEXER (Doombringer archetype)

For each, design:
1. PASSIVE NAME (evocative, 1-3 words)
2. TRIGGER CONDITION (what activates it)
3. EFFECT (concise, roguelite-length)
4. THEME (what fantasy does this combo enable?)
5. SYNERGY NOTE (which specific skills from each class combo best with this passive)

Warblade mechanics to leverage: Rage bar, stance swap, CC skills (Ground Stomp, War Stomp),
position-dependent skills, execute window, armor ignore.
"""
    },
    {
        "baslik": "BOLUM 03 — Elementalist Cross-Class Pasifleri (6 combo)",
        "prompt": BAGLAM + """
Design cross-class passives for ALL 6 remaining Elementalist combinations
(skip Warblade+Elementalist, already covered):

ELEMENTALIST + ROGUE (Arcane Shadow archetype — Mage+Rogue = 2 blinks, teleport kills)
ELEMENTALIST + RANGER (Storm Archer archetype)
ELEMENTALIST + BERSERKER (Chaos Incarnate archetype)
ELEMENTALIST + PALADIN (Divine Flamecaller archetype)
ELEMENTALIST + SUMMONER (Plague Caster archetype ⭐ DoT×stack→Hexblast)
ELEMENTALIST + HEXER (Plague Caster alternate — or separate archetype?)

Note: Elementalist has Fire State and Frost State alternation as core mechanic.
Mana management (high Fire damage = high mana cost, Frost = mana regen).
How do these interact with each partner class?

For each, design:
1. PASSIVE NAME
2. TRIGGER CONDITION
3. EFFECT
4. THEME
5. SYNERGY NOTE
"""
    },
    {
        "baslik": "BOLUM 04 — Rogue Cross-Class Pasifleri (5 combo)",
        "prompt": BAGLAM + """
Design cross-class passives for ALL 5 remaining Rogue combinations:

ROGUE + RANGER (Phantom Hunter archetype ⭐ CC from range + teleport finisher)
ROGUE + BERSERKER (Bloodfang archetype)
ROGUE + PALADIN (Shadow Saint archetype ⭐ Parry Riposte = both Energy + Holy Power)
ROGUE + SUMMONER (Deathmask archetype — corpse + stealth interaction?)
ROGUE + HEXER (Venomancer archetype — bleed + hex stacks both DoT based)

Rogue mechanics to leverage: Combo Points (0-5), stealth/Shadow Focus, Parry Riposte timing,
bleed DoT, Energy resource, positional attacks (Backstab from behind).

For each:
1. PASSIVE NAME
2. TRIGGER CONDITION
3. EFFECT
4. THEME
5. SYNERGY NOTE — which specific class skills combo with this passive
"""
    },
    {
        "baslik": "BOLUM 05 — Ranger Cross-Class Pasifleri (4 combo)",
        "prompt": BAGLAM + """
Design cross-class passives for ALL 4 remaining Ranger combinations:

RANGER + BERSERKER (Wild Hunter archetype ⭐ — Ranger setup, Berserker fury fills)
RANGER + PALADIN (Sacred Archer archetype)
RANGER + SUMMONER (Beast Commander archetype — ranged + minion control)
RANGER + HEXER (Plague Arrow archetype — ranged hex application)

Ranger mechanics to leverage: Cooldown-only resource (no bar), Expertise passive (CD reduction),
sweet spot range (medium distance = max damage), Dead Zone Mastery auto-disengage,
kiting behavior, Explosive Trap, Backward Dash.

Key tension: Ranger wants distance. Berserker/Paladin/Summoner might want close range.
How does the cross-class passive resolve or embrace this tension?

For each:
1. PASSIVE NAME
2. TRIGGER CONDITION
3. EFFECT
4. THEME
5. SYNERGY NOTE
"""
    },
    {
        "baslik": "BOLUM 06 — Berserker, Paladin, Summoner+Hexer Cross-Class Pasifleri (6 combo)",
        "prompt": BAGLAM + """
Design cross-class passives for the final 6 combinations:

BERSERKER + PALADIN (Fallen Saint archetype ⭐⭐ — INTENTIONALLY BROKEN: low HP = power + heal loop)
BERSERKER + SUMMONER (Blood Shepherd archetype ⭐ — minion deaths → Fury)
BERSERKER + HEXER (Plague Berserker archetype — hex stacks + low HP synergy)
PALADIN + SUMMONER (Holy Shepherd archetype)
PALADIN + HEXER (Inquisitor archetype — holy power + hex judgment)
SUMMONER + HEXER (Plague Doctor archetype ⭐⭐ — THEMATIC IDENTITY of the game)

Special note on FALLEN SAINT: This is intentionally the "broken" combo.
The cross-class passive should enable: staying permanently at low HP (Berserker power zone)
while Paladin heals just enough to not die. Design it to be powerful but skill-dependent.

Special note on PLAGUE DOCTOR: This is the game's thematic identity.
The cross-class passive should feel like "the endgame build everyone wants to try."

For each:
1. PASSIVE NAME
2. TRIGGER CONDITION
3. EFFECT
4. THEME
5. SYNERGY NOTE
6. [For Fallen Saint + Plague Doctor: add BROKEN BUILD NOTE — what specific skill combos make this broken]
"""
    },
    {
        "baslik": "BOLUM 07 — Neutral Pasif Genişletme: 8→14 Pasif",
        "prompt": BAGLAM + """
We currently have 8 neutral passives. We need to expand to 14 total.
EXISTING 8 neutral passives (do NOT redesign these, just reference them):
  P1 Evasion: 10% dodge, dodge→Rage+5
  P2 Bolstered Resolve: no damage 3s → incoming dmg -20%
  P3 Predator's Mark: hit CC'd enemy → next skill CD-2s
  P4 Bloodstained Path: kill → Rage+5
  P5 Momentum: after dash, 2s window → next skill +25% dmg
  P6 Glass Cannon: dmg+12%, max HP-10
  P7 Iron Skin: HP below 50% → incoming dmg -15%
  P8 Hunter's Focus: 3 hits same enemy → 4th hit guaranteed crit

Design 6 NEW neutral passives that:
- Don't overlap with existing 8
- Work for any of the 8 classes without referencing class-specific resources
- Fill gaps: we're missing something for SUMMONER-heavy or HEX-heavy builds that
  might want neutral passives... or maybe neutral passives just can't serve those builds.
- Consider categories we're missing: economy/Soul Dust, Grudge interaction,
  room-clearing speed, boss-specific bonuses

Also design 6 NEW neutral ACTIVE skills (we have 8, aiming for 14):
EXISTING 8: Surge Step, Fracture Point, Vital Strike, Chain Reaction, Recoil, Adrenaline Rush, Last Stand, Earthshatter
Add 6 more that cover different niches (sustained damage, debuff removal, positioning tool, etc.)
"""
    },
    {
        "baslik": "BOLUM 08 — Seçilebilir Skill Sistemi: Oda Ödülü Havuz Tasarımı",
        "prompt": BAGLAM + """
Design the SKILL ACQUISITION SYSTEM — how skills are offered and selected during a run.

Context:
- Player picks 2 classes at run start (e.g., Warblade + Elementalist)
- Each class has 8 active skills, 4 passives, 2 ultimates
- Player starts with: 0 skills (picks first 2 in first 2 rooms? Or starts with 2?)
- After each room: offered 3 choices, picks 1
- Max 6 active skill slots, max 2 passive slots, 1 ultimate slot

Questions to answer:
1. Starting loadout: does the player start with any skills? Which?
   Option A: Start with 1 basic skill from each class (2 total)
   Option B: First room = choose your first skill from 3 options (each class's #1)
   Option C: Lobby screen lets you pick 2 starting skills
   Which feels most roguelite? Which is most skill-expressive?

2. Offer pool composition per room:
   - How many from Class 1 vs Class 2 vs Neutral in each offer of 3?
   - Should the game bias toward filling "gaps" (e.g., if you have 4 Warblade skills, offer more Elementalist)?
   - Should early offers be simpler skills, later offers be complex/high-synergy skills?

3. Skill UPGRADE offers:
   - When does "upgrade an existing skill" appear as an option?
   - Is it a separate offer slot, or replaces one of the 3 offer slots?
   - How often should upgrades appear vs new skills?

4. ULTIMATE acquisition:
   - Does the ultimate start as available, or must be unlocked mid-run?
   - Both ultimates shown at start? Player picks 1? Or one unlocked mid-run?

5. Max slot management:
   - When the player hits 6 skill slots and 2 passive slots — what happens at room end?
   - Auto-upgrade mode? Or a choice to DROP one skill to take the new one?

6. Reference systems for best practices:
   - StS card rewards (fixed 3 per fight, character pool)
   - Hades boon system (each god offers 1 specific boon, reroll available)
   - Dead Cells blueprint (unlock then always available)
   - Dicey Dungeons episode modifiers
"""
    },
    {
        "baslik": "BOLUM 09 — Sınıf Pasiflerinin Upgrade Versiyonları",
        "prompt": BAGLAM + """
Each class has 4 passives. When a player "upgrades" a passive (via room reward or shop),
what does the upgrade do?

Design the UPGRADE VERSION for all 32 class passives (4 per class × 8 classes):

WARBLADE passives + upgrades:
  Bloodlust → upgrade = ?
  Iron Will → upgrade = ?
  Juggernaut → upgrade = ?
  Wrecking Ball → upgrade = ?

ELEMENTALIST passives + upgrades:
  Fire Focus → upgrade = ?
  Burnout Recovery → upgrade = ?
  Shatter → upgrade = ?
  Temporal Flux → upgrade = ?

ROGUE passives + upgrades:
  Parry Riposte → upgrade = ?
  Shadow Focus → upgrade = ?
  Venomous Wounds → upgrade = ?
  Murderous Intent → upgrade = ?

RANGER passives + upgrades:
  Expertise → upgrade = ?
  Dead Zone Mastery → upgrade = ?
  Marked Target → upgrade = ?
  Predator's Focus → upgrade = ?

BERSERKER passives + upgrades:
  Unyielding → upgrade = ?
  Ruthless → upgrade = ?
  War Machine → upgrade = ?
  Pain is Power → upgrade = ?

PALADIN passives + upgrades:
  Divine Focus → upgrade = ?
  Holy Endurance → upgrade = ?
  Sanctified Wrath → upgrade = ?
  Light's Grace → upgrade = ?

SUMMONER passives + upgrades:
  Army's Strength → upgrade = ?
  Blood for Power → upgrade = ?
  Undying Hunger → upgrade = ?
  Corpse Lord → upgrade = ?

HEXER passives + upgrades:
  Debuff Threshold → upgrade = ?
  Punishing Hex → upgrade = ?
  Lingering Curse → upgrade = ?
  Soul Rend → upgrade = ?

Rule: Upgrade should do ONE of:
  (a) Strengthen the existing effect (numbers up)
  (b) Add a SECONDARY trigger condition
  (c) Remove a limitation
  (d) Add a new but thematically consistent effect
"""
    },
    {
        "baslik": "BOLUM 10 — Ultimate Skill Tasarımı: 8 Sınıf Identity Transform",
        "prompt": BAGLAM + """
Each class has 2 ultimates in the Rage system (charged by [V] key).
Beyond that, there's an "Identity Transform" (meta-progression unlock):
when Rage is full, hold [V] → 10s full transformation with completely different 4-skill set.

Design the IDENTITY TRANSFORM kit for all 8 classes:
(When the class is in Identity Transform mode for 10s, what are the 4 skills?)

These should feel like a powered-up version of the class, or a secret "true form":
- Warblade Identity: AVATAR OF WAR (raging titan, unstoppable)
- Elementalist Identity: ARCHMAGE / PRIMORDIAL (elements unleashed)
- Rogue Identity: DEATH'S SHADOW (absolute stealth killer)
- Ranger Identity: APEX PREDATOR (every shot is devastating)
- Berserker Identity: CHAOS INCARNATE (the rage takes over)
- Paladin Identity: DIVINE CHAMPION (fully empowered holy warrior)
- Summoner Identity: DEATH LORD (undead army at full power)
- Hexer Identity: CURSE EMBODIED (becomes the curse itself)

For each class, design:
1. IDENTITY TRANSFORM NAME
2. 4 SKILLS available during transform (brief description each)
3. PASSIVE BONUS during transform
4. EXIT CONDITION (ends at 10s, or can end early?)
5. HOW IT SYNERGIZES with cross-class pairing
   (e.g., if Warblade+Paladin, Identity Transform = which class transforms?)
"""
    },
]

def ollama_sor(prompt: str) -> str:
    payload = {
        "model": MODEL,
        "prompt": FORMAT_ONEKI + prompt,
        "stream": False,
        "options": GPU_OPTIONS,
    }
    try:
        r = requests.post(OLLAMA_URL, json=payload, timeout=1200)
        r.raise_for_status()
        return r.json().get("response", "").strip()
    except Exception as e:
        return f"HATA: {e}"


def ham_veri_cikar(metin: str) -> str:
    if "### PART 2" in metin:
        return metin.split("### PART 2")[0].strip()
    return metin


def main():
    toplam = len(BOLUMLER)
    print("\n" + "="*65)
    print(f"  Model: {MODEL} | Bölüm: {toplam}")
    print(f"  Konu: Pasifler + Cross-Class + Skill Sistemi")
    print(f"  Tahmini süre: {toplam*5}–{toplam*12} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Pasif + Cross-Class Araştırması — Ollama\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Pasif Ham Veri\n*Sadece PART 1 | {datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

    basari = 0
    for i, bolum in enumerate(BOLUMLER, 1):
        print(f"\n[{i:02d}/{toplam}] {datetime.now().strftime('%H:%M')} — {bolum['baslik'][:60]}")
        print(f"         Düşünüyor...", end="", flush=True)

        cikti = ollama_sor(bolum["prompt"])

        if cikti.startswith("HATA"):
            print(f" ✗\n         {cikti}")
        else:
            print(f" ✓  ({len(cikti):,} karakter)")
            basari += 1

        with open(CIKTI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{cikti}\n\n---\n\n")

        with open(HAM_VERI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{ham_veri_cikar(cikti)}\n\n---\n\n")

    print(f"\n{'='*65}")
    print(f"  TAMAMLANDI: {basari}/{toplam} | {datetime.now().strftime('%H:%M')}")
    print(f"  Çıktı: {CIKTI_DOSYASI}")
    print("="*65)


if __name__ == "__main__":
    main()
