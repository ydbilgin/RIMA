"""
Heat Sistemi + Performans Carpani + Puan/Leaderboard Arastirmasi
7 bolum — oyun bazli derin analiz + bizim oyunumuza sentez
"""

import requests
import json
import os
from datetime import datetime

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL = "deepseek-r1:14b"
OUTPUT_DIR = r"F:\Antigravity Projeler\2d roguelite\TASARIM"
OUTPUT_FILE = os.path.join(OUTPUT_DIR, "HEAT_PUAN_ARASTIRMA_OLLAMA.md")
LOG_FILE = r"C:\Users\ydbil\AppData\Local\Temp\heat_arastirma.log"

GPU_OPTIONS = {
    "temperature": 0.7,
    "num_predict": 7000,
    "num_ctx": 4096,
    "num_gpu": 99
}

OYUN_BAGLAMI = """
OUR GAME CONTEXT (always keep this in mind):
- 2D top-down flat roguelite action, solo dev, Unity 2D, Steam
- 8 classes, DUAL-CLASS system: pick 2 classes per run
- Skill acquisition: 3 choices after each room (Slay the Spire model), max 6 skill slots
- GRUDGE SYSTEM: Elite enemies remember how they died. Next encounter they resist that strategy.
  Player can INTENTIONALLY program them (kill with X so they're weak to Y later).
- Room types: Normal, Elite (Grudge encounter), Shop, Boss, Flux (rare reforge), Secret
- Boss Soul: after boss kill, one skill permanently mutates
- Meta progression: Hub between runs, permanent unlocks
- Run length target: 35-55 minutes
- 3 Acts, each with a boss
- Scoring system: base room points + performance multiplier + grudge bonus + heat multiplier
- Solo dev scope
- HEAT SYSTEM planned: progressive difficulty modifiers that stack
- GAUNTLET MODE: after boss kill, optional/mandatory survival wave (bullet hell style)
- Future SURVIVAL MODE: separate endless mode (B option)
"""

bolumler = [
    {
        "id": "01",
        "baslik": "Heat/Ascension Sistemleri — 5 Oyun Derin Analiz",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Deep analysis of progressive difficulty/challenge systems in 5 games.

**HADES HEAT SYSTEM:**
- Exactly how does the Pact of Punishment work? What are all the modifiers?
- How many Heat levels? What's the progression curve?
- Which modifiers are most impactful vs least? Why?
- How does Heat interact with rewards/meta-progression?
- What do players love/hate about Hades Heat system?
- Any modifiers that actually change HOW you play (not just HP buffs)?

**DEAD CELLS BOSS CELLS:**
- How do Boss Cells work? What changes at each cell level?
- How does biome/path availability change with difficulty?
- What "curse" mechanics exist?
- How does it affect replayability?

**SLAY THE SPIRE ASCENSION:**
- Ascension 1-20: what exactly changes at each level?
- Which Ascensions are most interesting (change strategy) vs just stat buffs?
- Why does STS Ascension feel satisfying to climb?

**PATH OF EXILE MAP MODS:**
- How do map modifiers work (corrupted maps, beyond, etc.)?
- What's the "juice" system? How does stacking mods increase reward?
- What lessons does PoE teach about modifier stacking?

**RISK OF RAIN 2 DIFFICULTY:**
- Eclipse mode: how does it work?
- Time-pressure difficulty scaling: how does it create urgency?

**SYNTHESIS FOR OUR GAME:**
- Which Heat modifiers from these games would work for our Grudge system?
- What makes a Heat modifier "interesting" vs "just a stat buff"?
- Suggest 15 specific Heat modifiers for our game, categorized by:
  (a) Simple stat changes (acceptable but boring)
  (b) Rule changes (change how you play — best ones)
  (c) Grudge-specific modifiers (unique to our game)
"""
    },
    {
        "id": "02",
        "baslik": "Performans Carpani Sistemleri — Dogru Metrikleri Olcmek",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: How should performance multipliers work in our specific game?

**WHAT GAMES MEASURE:**
- Binding of Isaac: how does the grade/rank system work? What does it measure?
- Hades: does Hades have a performance scoring system? Why/why not?
- Devil May Cry: SSS ranking system — what exactly is measured? (hasar, stil, cesaret)
- Sifu: aging/score system — how does it measure mastery?
- Crypt of the Necrodancer: what does it measure? (rhythm + speed)

**THE CORE QUESTION:**
What separates "measuring skill" from "measuring luck" in a roguelite?
- Room clear time: is this skill or luck (depends on what spawned)?
- No-hit: is this skill or luck (depends on enemy behavior)?
- Damage dealt: skill or build power?
- Efficiency (damage per second): better measure?

**FOR OUR GAME SPECIFICALLY:**
Given our Grudge system, dual class, and skill acquisition:

1. Which performance metrics are VALID (measure actual player skill)?
2. Which metrics are INVALID (measure luck/build power, not skill)?
3. How should we handle: player uses OP cross-class combo vs weak combo?
4. Should performance multiplier apply per-room or only at run end?
5. No-hit in our game: is it achievable and meaningful at Heat 5+?

**GRUDGE PERFORMANCE METRIC (unique to us):**
How should we score "grudge mastery"?
- Option A: Did you kill elite with their weakness? Binary yes/no bonus
- Option B: Track how many encounters before you discovered weakness
- Option C: Track intentional programming (kill with X specifically to set up Y)
Which is most skill-expressive? Which is fairest for leaderboard?

**FINAL RECOMMENDATION:**
Design the exact performance multiplier formula for our game.
Include: which metrics, how they stack, what range (0.5x - 3.0x?), edge cases.
"""
    },
    {
        "id": "03",
        "baslik": "Puan Sistemi Psikolojisi + Leaderboard Tasarimi",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: How should scoring and leaderboards be designed for maximum engagement?

**SCORING PSYCHOLOGY:**
- What makes players care about scores? (Intrinsic vs extrinsic motivation)
- Why do some games (Hades) have no score system and still feel great?
- Why do some games (Isaac) have score and it feels meaningful?
- What's the difference between "high score" and "achievement" motivation?
- When does a score system feel cheap/gameable vs deep?

**LEADERBOARD BEST PRACTICES:**
- How do games like Dead Cells, Risk of Rain 2, Hades handle competitive elements?
- Why do "global leaderboards" often fail for roguelites specifically?
- What filters make leaderboards meaningful? (class, heat, combo, speedrun?)
- "Friends leaderboard" vs "global": which drives more engagement?
- How does Spelunky handle its scoring? (classic roguelite score design)
- How does Vampire Survivors handle scoring? (simple but effective)

**ANTI-CHEAT / SCORE INTEGRITY:**
- How do roguelites prevent score manipulation?
- Should we store run replay data for verification?
- What data should be in leaderboard "detail" (visible to others)?

**FOR OUR GAME:**
Given Heat system + Grudge + dual class:
1. Should we have ONE score or separate dimensions (speed score, grudge score, etc.)?
2. How do we prevent "optimal path" gaming (players picking only high-value rooms)?
3. If player ignores Shop and rushes = high time score but missed power = harder boss: how do we reward both playstyles?
4. Suggest final leaderboard structure: what categories, what's shown, what's filterable.
"""
    },
    {
        "id": "04",
        "baslik": "Endless/Survival Mod Tasarimi — Gauntlet + Ilerideki B Modu",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: How should endless/survival modes be designed in our game?

**GAUNTLET MODE (after boss kill, in-run):**
- This is 60-90 seconds of increasing waves after a boss dies.
- Player keeps their current build.
- Drops fall on the ground (Soul Dust + temporary buffs).
- How should wave escalation work? (Vampire Survivors model vs other?)
- What drop system works best for a 60-90 second window?
- Should Gauntlet have its own separate scoring or feed into run score?
- How does Gauntlet interact with Grudge system (do Grudge enemies appear)?
- What makes Gauntlet feel different from normal rooms?

**VAMPIRE SURVIVORS / BROTATO MODEL:**
- How does VS escalate difficulty over time? (enemy count, speed, HP)
- What's VS's drop/pick-up system? Why does it feel good?
- How does Brotato differ from VS in terms of survival loop?
- What makes survival feel endless but not tedious?

**TRANSITION FROM ROGUELITE TO SURVIVAL:**
- The core fantasy is different: roguelite = "build insane", survival = "dodge everything"
- How do games successfully blend these? (Enter the Gungeon Dodge Roll game mode?)
- What design principles keep the two modes feeling cohesive?

**FOR OUR GAME — FUTURE SURVIVAL MODE (B option):**
1. Should survival use the same skill acquisition system (pick 3 after X kills)?
2. Should Grudge system exist in survival? (enemies remember across waves?)
3. Drop design: what drops in survival mode? How frequent? What types?
4. Wave design: 5 waves per "act"? Or truly endless with scaling?
5. What's the core fantasy difference and how do we communicate it?
"""
    },
    {
        "id": "05",
        "baslik": "Skill/Pasif Sistemi Genisletme — Heat ile Skill Eklemeleri",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: How can the skill/passive system be extended with Heat and new mechanics?

**HEAT-MODIFIED SKILL ACQUISITION:**
- At higher Heat levels, how should skill acquisition change?
  Option A: Fewer choices (2 instead of 3)
  Option B: Worse quality cards offered
  Option C: Cards have "curses" attached (power + downside)
  Option D: Some cards "locked" behind completing Grudge encounters

- Should Heat affect the skill pool itself (removing certain overpowered skills)?
- How does Slay the Spire's "Cursed" card system work and what can we learn?

**CURSE CARDS / CURSED SKILLS:**
- What are "curse" mechanics in roguelites? (StS, Hades Chaos, Binding of Isaac curses)
- Design principle: a curse should create interesting decisions, not just punish
- Suggest 8 "cursed skill" concepts for our game:
  A skill that is powerful but has a meaningful downside
  Examples: "Charge does +100% damage but costs 30 HP to use"
  Make them specific to our skill system (Grudge, Rage, dual class)

**NEW PASSIVE TYPES — What's missing?**
Currently we have: class passives (14/class), neutral passives (14), cross-class passives (28).
What OTHER passive categories could exist?
- Heat-unlocked passives (only available at Heat 3+)?
- Boss Soul passives (from defeating specific bosses)?
- Grudge reward passives (for perfect Grudge mastery)?
- Corrupted passives (powerful but with cost)?

**SKILL MUTATION EXTENSIONS:**
Currently Boss Soul mutates one skill. Could this go further?
- Chain mutations (mutate 3 times across 3 boss kills)?
- Mutation choices (2 options for each mutation)?
- Corrupted mutation (very powerful but risky)?
- How does this affect build identity across a run?

**FINAL SUGGESTIONS:**
Give 10 concrete new skill/passive ideas that would enhance our game's "build insane" fantasy,
specifically leveraging the Grudge system and dual-class in ways not yet covered.
"""
    },
    {
        "id": "06",
        "baslik": "Run Modifier / Curse-Blessing Sistemleri",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Should our game have run modifiers (blessings/curses) beyond Heat?

**REFERENCE SYSTEMS:**
- Hades Chaos Boon: powerful effect with a temporary downside (miss X attacks). How does this create engaging decisions?
- Slay the Spire: Events that offer powerful rewards with permanent run downsides. What makes them interesting?
- Enter the Gungeon: Junk/Curse system — how does curse accumulate and what does it do?
- Path of Exile: Map modifiers — how do "juicing" maps with mods create risk/reward?
- Binding of Isaac: Devil Deal trade (HP for powerful items) — why does this feel iconic?

**THE CORE DESIGN QUESTION:**
A good run modifier:
  ✓ Creates a meaningful decision
  ✓ Changes HOW you play, not just difficulty
  ✓ Has interesting interactions with existing systems
  ✗ Doesn't just subtract fun (flat damage reduction)
  ✗ Doesn't create NPE (negative play experience without recourse)

**FOR OUR GAME:**
We have: Flux room (reforge), Shop (buy items), Elite (Grudge encounter)
We could add: Events/shrines that offer curse-blessing trades

Design 12 specific curse-blessing events for our game:
- Each should: [offer powerful reward] in exchange for [interesting downside]
- 4 should specifically interact with the Grudge system
- 4 should interact with the dual-class/skill system
- 4 should interact with resources (Soul Dust, HP, Rage/Fury etc.)

Also: Should these events be in Heat 0 (base game) or only unlock at Heat 3+?
What's the design philosophy argument for each approach?
"""
    },
    {
        "id": "07",
        "baslik": "Bizim Oyunumuza Ozel Tam Sentez — Heat + Puan + Endless + Ekstralar",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Full synthesis — design the complete Heat + Scoring + Endless system for our specific game.

Based on everything analyzed (Hades Heat, Dead Cells BC, STS Ascension, scoring psychology, survival modes, skill extensions):

**HEAT SYSTEM — FINAL DESIGN:**
Design exactly 20 Heat modifiers for our game, organized into tiers:
  Tier 1 (Heat 1-4):  Accessible difficulty increases for early players
  Tier 2 (Heat 5-9):  Rule changes that affect strategy
  Tier 3 (Heat 10-14): Serious challenge — changes fundamental gameplay
  Tier 4 (Heat 15-20): Masochist tier — near-impossible

For each modifier, specify:
  - Exact mechanical effect
  - Which existing system it interacts with (Grudge/skill/heat/economy)
  - Why it's interesting (not just a stat buff)

**SCORING SYSTEM — FINAL DESIGN:**
Given all research, design the final scoring formula:
  - Exact base room values
  - Exact performance multiplier ranges and conditions
  - Grudge scoring specifics
  - Heat multiplier curve
  - Run-end bonuses
  - What data gets stored for leaderboard

**GAUNTLET MODE — FINAL DESIGN:**
  - Wave escalation curve (exact numbers)
  - Drop types and rates
  - How it feeds into run score
  - Heat interaction (does Gauntlet change with Heat?)
  - When is it optional vs mandatory?

**WHAT ELSE SHOULD WE ADD:**
Given our game's specific systems, what 5 features/mechanics are most valuable to add
that AREN'T currently in the design? Be specific, scope-aware (solo dev), and explain
why each would enhance the "build insane" fantasy specifically.

**ONE PARAGRAPH SUMMARY:**
What does the complete experience feel like at Heat 10, with full scoring and Gauntlet?
Describe the player's session from run start to Gauntlet end.
"""
    }
]


def ollama_sor(prompt, max_tokens=7000):
    data = {
        "model": MODEL,
        "prompt": prompt,
        "stream": False,
        "options": {**GPU_OPTIONS, "num_predict": max_tokens}
    }
    try:
        resp = requests.post(OLLAMA_URL, json=data, timeout=360)
        resp.raise_for_status()
        return resp.json().get("response", "")
    except Exception as e:
        return f"HATA: {e}"


def log(msg):
    print(msg)
    with open(LOG_FILE, "a", encoding="utf-8") as f:
        f.write(msg + "\n")


def main():
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    with open(LOG_FILE, "w", encoding="utf-8") as f:
        f.write("")

    with open(OUTPUT_FILE, "w", encoding="utf-8") as f:
        f.write(f"# Heat + Puan + Endless Sistem Arastirmasi (Ollama)\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | 7 bolum*\n\n---\n\n")

    for bolum in bolumler:
        log(f"[{bolum['id']}/07] {bolum['baslik']}...")
        yanit = ollama_sor(bolum["prompt"])

        with open(OUTPUT_FILE, "a", encoding="utf-8") as f:
            f.write(f"## BOLUM {bolum['id']} — {bolum['baslik']}\n\n")
            f.write(yanit)
            f.write("\n\n---\n\n")

        log(f"  [OK] Tamamlandi ({bolum['id']}/07)")

    log(f"\n[DONE] Tum bolumler tamamlandi.")
    log(f"   Cikti: {OUTPUT_FILE}")


if __name__ == "__main__":
    main()
