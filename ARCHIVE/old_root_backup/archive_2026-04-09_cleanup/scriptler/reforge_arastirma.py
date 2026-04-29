#!/usr/bin/env python3
"""
Reforge / Build Pivot Araştırması
===================================
Oyunumuz: Flat top-down 2D roguelite, 4 sütun (Role Break + Rotation Rogue + Grudge + Rage)
Araştırılan: Run ortası build yönü değiştirme sistemi — Neutral Skill/Pasif tasarımı,
             oyuncu psikolojisi, dengeleme yaklaşımları, en iyi implementasyonlar.
Çıktı: TASARIM/REFORGE_SISTEM_OLLAMA.md (ham) + TASARIM/REFORGE_HAM.md
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

BASE = r"F:\Antigravity Projeler\2d roguelite\TASARIM"
CIKTI_DOSYASI    = os.path.join(BASE, "REFORGE_SISTEM_OLLAMA.md")
HAM_VERI_DOSYASI = os.path.join(BASE, "REFORGE_HAM.md")

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

OYUN_BAGLAMI = """
GAME CONTEXT (read before answering):
- Game: Flat top-down 2D roguelite action
- 4 pillars: (1) Dual-class system — run start pick 2 classes from 8, mix skill pools; (2) MMORPG rotation — proc conditions, combo multipliers; (3) Grudge Waves — elite enemies remember how they died; (4) Rage bar — universal 0-100, V key burst
- 8 classes: Warblade, Elementalist, Rogue, Ranger, Brawler, Paladin, Summoner, Hexer
- Skill acquisition: Slay the Spire model — after each room, 3 options shown, pick 1, max 6 skill slots
- Room structure: Normal → Elite → Shop → [rare] Reforge → Boss
- Target: Solo dev, 6-month timeline, Steam, indie pixel art

WHAT WE WANT TO ADD — REFORGE/BUILD PIVOT SYSTEM:
After a room, rarely (5-8% chance) instead of 3 upgrade options, player gets a "Reforge" option:
  [SKILL CHANGE] Replace one skill → different skill from same class OR cross-class OR "Neutral Skill"
  [PASSIVE CHANGE] Replace one passive → same class / cross-class / "Neutral Passive"  (TFT Reforge logic)
  [ITEM CHANGE] Replace one item → select from options (Hades boon reroll style)

"Neutral Skills/Passives" = classless, universal skills that fit any build without breaking class identity.
"""

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — TFT Reforge Sistemi: Detaylı Mekanik Analiz",
        "prompt": OYUN_BAGLAMI + """
Analyze Teamfight Tactics (TFT) Reforge augment system in detail:
1. Exact mechanic: how does it work, when does it appear, what can be reforged?
2. Player psychology: what emotions does it trigger? (excitement, regret, relief?)
3. Frequency: how often does it appear, why that frequency?
4. Strategic depth: does it enable better builds or just randomize?
5. Failure cases: when does it feel bad? When does it feel amazing?
6. UI/UX: how is it presented? What information is shown before accepting?

Then: How should we adapt this for our dual-class roguelite?
- Should it be blind (no preview) or show what you'll get?
- Should there be a cost (resource) or be free?
- When in the run should it appear? (early/mid/late — different implications)
"""
    },
    {
        "baslik": "BOLUM 02 — Slay the Spire Kart Dönüşüm + Silme Sistemi",
        "prompt": OYUN_BAGLAMI + """
Analyze Slay the Spire's mid-run card modification systems:
1. Card Transform: what exactly happens, rarity rules, cost?
2. Card Remove: why is removing cards often stronger than adding?
3. Card Upgrade: how does upgrade differ from transform?
4. Shop card removal: cost, frequency, strategic importance?
5. Specific events that modify cards: list main ones and their mechanic.
6. Player psychology: "deck thinning" satisfaction — why does removing feel good?
7. Slay the Spire 2: what new card modification systems were added?

Adaptation for our game:
- We have skill slots (max 6), not a deck. How does "remove" translate?
- Should removing a skill have a benefit (slot freed = passive slot opens?)
- How does cross-class skill transform work? (Warrior skill → Mage variant)
"""
    },
    {
        "baslik": "BOLUM 03 — Hades Boon Reroll ve Pact of Punishment Esnekliği",
        "prompt": OYUN_BAGLAMI + """
Analyze Hades (1 and 2) mid-run flexibility systems:
1. Boon reroll: how does it work, cost (Obols), when available?
2. Boon upgrade (Duo/Legendary): how do upgrades interact with rerolls?
3. Pact of Punishment: how does difficulty affect build flexibility?
4. Infernal Gate (Hades 2): new mid-run modification system?
5. Chaos boons: risk/reward modification — what makes them special?
6. Player agency: at what point does Hades feel "forced" vs "freedom"?
7. Death-defiance as buffer: how safety nets affect build experimentation?

Key question: Hades gives you many rolls of the same slot (god boons).
We give you one choice per room. How should we balance the scarcity?
- Should we give more choices per room OR make reforge appear more often?
- Should reforge show 3 options (like normal) or just 1 fixed replacement?
"""
    },
    {
        "baslik": "BOLUM 04 — Risk of Rain 2 Printer ve Dead Cells Blueprint Sistemi",
        "prompt": OYUN_BAGLAMI + """
Analyze item exchange systems in Risk of Rain 2 and Dead Cells:

RISK OF RAIN 2:
1. Printer machines: exact mechanic, cost, what items can be printed?
2. Scrapper: converting items to scrap, strategic use?
3. 3D Printer item pool: how is it determined? What's the catch?
4. Player decisions: when is it worth printing vs keeping current item?
5. Lunar items: reroll/exchange mechanic at Bazaar Between Time?

DEAD CELLS:
1. Blueprint system: how does it work across runs?
2. Scroll alternate selection: how does build direction get locked?
3. Legendary weapons: modification/replacement rules?
4. Custom Mode: how does item reroll affect game feel?

Adaptation:
- Our game has 6 skill slots + 2 passive slots. Much simpler than RoR2's item stacking.
- Should we allow "spending" a skill (sacrifice it) to power up another skill?
- What's the equivalent of RoR2's "Bazaar" in our hub-based roguelite?
"""
    },
    {
        "baslik": "BOLUM 05 — Neutral Skill Tasarımı: Sınıfsız Evrensel Skill Prensipleri",
        "prompt": OYUN_BAGLAMI + """
This is the most important design question: how to design "Neutral Skills" — classless skills that:
- Fit any of our 8 classes (Warblade, Elementalist, Rogue, Ranger, Brawler, Paladin, Summoner, Hexer)
- Don't break class identity (a Rogue still feels like a Rogue with a Neutral skill)
- Are powerful enough to be worth taking (vs a class-specific skill)
- Aren't so powerful they become mandatory

Reference games:
- GW1: Universal skills that any class can use (e.g., Resurrection Signet, conditions)
- PoE: Keystones that any build can access but change playstyle
- Slay the Spire: Colorless cards — how balanced are they vs class cards?
- Hades: Room blessings that work for any weapon

Design questions to answer:
1. What categories of "neutral" effects work universally? (movement? defense? resource?)
2. How do you make a neutral skill feel different from a class skill thematically?
3. Neutral passives vs neutral actives: which is easier to design and balance?
4. How many neutral skills should exist in the pool? (too few = always same, too many = never same)
5. Should neutral skills scale with your class mechanics (e.g., a neutral skill that interacts with "any resource bar")?
6. Specific examples: design 5-8 neutral skills/passives for our game.
"""
    },
    {
        "baslik": "BOLUM 06 — Build Pivot Oyuncu Psikolojisi: Pişmanlık vs Esneklik",
        "prompt": OYUN_BAGLAMI + """
Research the psychology of "build pivot" moments in roguelikes — when players change direction mid-run.

Core tension:
- Too much flexibility → no build identity, no commitment satisfaction
- Too little flexibility → "this run is over" feeling when bad luck strikes early

Research questions:
1. Regret mechanics: what makes a player feel "I wish I hadn't taken that skill"?
2. Sunk cost in roguelikes: how do players behave when a build isn't working?
3. "Pivoting" as a skill: in competitive roguelite play, when do top players pivot?
4. The "commit or adapt" decision: which games handle it best?
5. Forced builds vs emergent builds: player satisfaction differences
6. How does game tempo affect pivot opportunities? (faster = fewer good moments to pivot)
7. Post-run analysis: do players remember their pivots fondly or as failures?

Games to reference: StS, Hades, Balatro, Dead Cells, Dicey Dungeons, Across the Obelisk

For our game specifically:
- We have proc chains (Rotation Rogue pillar) — pivoting could break carefully built rotations
- Grudge system means enemies adapt to our strategy — forced pivots are valuable
- How should the Reforge system feel "earned" not "random"?
"""
    },
    {
        "baslik": "BOLUM 07 — Cross-Class Skill Alırken Kısıt ve Denge Mekanizmaları",
        "prompt": OYUN_BAGLAMI + """
Our game: dual-class system. Player picks 2 classes, builds from their combined pool.
Reforge system can offer: same-class skill, cross-class skill (from a 3rd class), or neutral skill.

The problem: if cross-class skills are available, why pick 2 classes at start at all?

Research how other games handle multi-class skill access:
1. GW1: How is cross-profession skill access limited? (attribute investment, rune cost)
2. ArcheAge: 120 class system — how does triple class work, what are the limits?
3. PoE: Multi-class via Ascendancy — what costs restrict cross-class skills?
4. D&D 5e multiclassing: mechanical costs of dipping into other classes?
5. Pathfinder: Archetype system — how are cross-class abilities gated?

Then design our system:
1. What should the cost/restriction be for taking a cross-class skill via Reforge?
   Options: (a) no restriction — rare enough that it's fine; (b) costs Soul Dust; (c) takes an extra skill slot; (d) reduces effectiveness (60% of normal power)
2. Should cross-class Reforge skills be visually distinct? (special frame, color)
3. How does a cross-class skill interact with our proc system? (does Warrior skill proc Mage's condition?)
4. Hard limit: should players be capped at "max 1 cross-class skill per run"?
"""
    },
    {
        "baslik": "BOLUM 08 — Balatro Joker Sistemi ve Dicey Dungeons Reroll — Build Pivot İmkanları",
        "prompt": OYUN_BAGLAMI + """
Analyze Balatro's joker management and Dicey Dungeons' reroll/modification systems:

BALATRO:
1. Joker selling: when, why, what's the sell value logic?
2. Joker ordering: how does position affect activation?
3. Spectral cards: card transformation — what can they do?
4. Tarot cards: how do they modify cards/jokers?
5. The Planet cards: how do hand upgrades interact with build pivots?
6. "The Wheel of Fortune" and similar random modifiers: good or bad design?
7. Why does Balatro feel so good when pivoting? What's the psychological hook?

DICEY DUNGEONS:
1. Reroll mechanic: exactly how does it work in each episode?
2. Witch class: full reroll focus — how does it change playstyle?
3. Inventor class: equipment modification — what can be changed?
4. Episode modifiers: how do they force different build paths?

Key insight to extract:
- Balatro makes selling/replacing feel exciting, not like a loss. How?
- Dicey Dungeons makes reroll a core skill, not an escape hatch. How?
- How can we make Reforge feel like an exciting opportunity, not a "fix my mistake" button?
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
    print(f"  Konu: Build Pivot / Reforge Sistemi")
    print(f"  Tahmini süre: {toplam*5}–{toplam*12} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Reforge Sistemi — Ollama Araştırması\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Reforge Ham Veri\n*Sadece PART 1 | {datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

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
