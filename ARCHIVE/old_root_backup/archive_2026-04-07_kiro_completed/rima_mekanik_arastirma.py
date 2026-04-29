#!/usr/bin/env python3
"""
RIMA — Mekanik Araştırma
Roguelite referans oyunlarından mekanik, hikaye, mob, boss, ekonomi analizi.
Çalıştır: python rima_mekanik_arastirma.py
"""

import sys, requests
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

CIKTI_DOSYASI    = r"F:\Antigravity Projeler\2d roguelite\ARASTIRMA_MEKANIK.md"
HAM_VERI_DOSYASI = r"F:\Antigravity Projeler\2d roguelite\ARASTIRMA_MEKANIK_HAM.md"

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

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — Ekonomi ve Ödül Sistemleri (Sandık, Para, Dükkan)",
        "prompt": """
Analyze the economy and reward systems in these action roguelite games:
- Hades (2020, Supergiant)
- Hades II (2024, Supergiant)
- TMNT: Shredder's Revenge (2022)
- TMNT: Splintered Fate (2024, Apple Arcade)
- Dead Cells (2018, Motion Twin)
- Enter the Gungeon (2016, Dodge Roll)
- Returnal (2021, Housemarque)
- Risk of Rain 2 (2020, Hopoo Games)

For each game, list:
1. Currency types (names, how earned, what they buy)
2. Chest/loot chest types and drop rates
3. Shop types (in-run shop, meta-shop, reroll costs)
4. Reward pacing (how often player gets rewarded per run)
5. Special economy mechanics (curse systems, debt mechanics, gambling)
6. Meta-progression currencies (unlockables between runs)

Then analyze: What combination of these systems creates the most engaging economy loop for a 2D top-down dark fantasy roguelite like RIMA?
"""
    },
    {
        "baslik": "BOLUM 02 — Build Çeşitliliği ve Cross-Class Skill Sistemleri",
        "prompt": """
Analyze skill build diversity and cross-class mechanics in these roguelites:
- Hades: Boon system (Olympian gods grant passive upgrades), Duo boons, Legendary boons, Chaos boons
- Hades II: Additional aspects, arcana cards, incantations
- Dead Cells: Mutation system (3 mutation slots, synergies between weapon types)
- Risk of Rain 2: Item stacking system, character-specific abilities, printer/cauldron
- Vampire Survivors: Weapon evolution system, passive synergies
- Noita: Physics-based spell crafting, wand combinations
- Slay the Spire: Card synergy deck building

For each:
1. How many build paths exist per character
2. How cross-class or cross-character synergies work
3. How the player discovers builds (curation vs random)
4. Reroll mechanics and choice mitigation
5. Most memorable/broken synergies that made the game famous

Then analyze: RIMA has 4 classes (Warblade, Elementalist, Shadowblade, Ranger) with 12 skills each and a skill draft system. What build diversity mechanics should RIMA add? How should cross-class boons/items work?
"""
    },
    {
        "baslik": "BOLUM 03 — Mob Tasarımı: Normal, Elite, Sürü Mekaniği",
        "prompt": """
Analyze enemy/mob design in these action roguelites with focus on:
- Hades: Underworld enemies (Tartarus, Asphodel, Elysium, Styx mobs)
- Dead Cells: Enemy telegraphing, parry windows, elite modifiers
- Enter the Gungeon: Bullet pattern design, enemy combinations
- TMNT: Splintered Fate: Enemy variety, swarm mechanics
- Risk of Rain 2: Elite affixes (Blazing, Glacial, Overloading, etc.), champion elites
- Returnal: Enemy phases and pattern escalation

For each game, document:
1. Normal mob types and their UNIQUE mechanic (not just "melee" or "ranged")
2. Elite/Champion system — how elites differ from normals (affixes, health, rewards)
3. Swarm mechanics — how small enemies are made dangerous in groups
4. Enemy combination design — which enemy types are placed together and why
5. Visual telegraphing methods — how player reads incoming attacks
6. Enemy AI escalation across difficulty levels

Then analyze: RIMA has Act 1 enemies (ShardWalker, VoidThrall, SeamCrawler, ChainWarden, Penitent, RelicCaster, FractureImp) and elites (IronWarden, FractureKnight, TwiceBorn). What mob design principles should RIMA apply? What unique mechanics should each mob type have?
"""
    },
    {
        "baslik": "BOLUM 04 — Boss Tasarımı: Faz Geçişleri, AoE Tells, Pattern Tasarımı",
        "prompt": """
Analyze boss fight design in these games:
- Hades: All 4 boss types (Meg, Lernaean Bone Hydra, Theseus+Asterius, Hades) — phase transitions, arena mechanics
- Hades II: New bosses, Chronos fight
- Dead Cells: Boss telegraphing philosophy, parry-heavy design
- Enter the Gungeon: Bullet hell boss patterns, phase transitions
- TMNT: Splintered Fate: Boss mechanics specific to this game
- Hollow Knight: Boss design philosophy (tells, punish windows)
- Returnal: Phase-based sci-fi bosses

For each boss or boss system:
1. Phase transition triggers (HP threshold, time-based, mechanic-based)
2. AoE tell design (visual warning, timing window, player action required)
3. Unique mechanic that defines this boss (not shared with others)
4. Enrage mechanics
5. Reward structure (what drops from boss, does it affect meta)
6. Arena design (static vs dynamic arena)

Then analyze: Design a boss fight system for RIMA's TwiceBorn (Act 1 boss) that incorporates the best practices. Include: phase 1 and phase 2 mechanics, 3 specific attack patterns with tell timing, arena hazards, and reward structure.
"""
    },
    {
        "baslik": "BOLUM 05 — Hikaye Anlatımı ve Meta-Progression Narrative",
        "prompt": """
Analyze narrative and meta-progression storytelling in these roguelites:
- Hades: Dialogue system, relationship system, story through repeated runs, Greek mythology reinterpretation
- Hades II: Continued story, new characters, expanded mythology
- Dead Cells: Environmental storytelling, lore tablets, unreliable narrator
- Returnal: Loop-based psychological horror narrative, collectibles
- Disco Elysium: Skill-as-personality system (not roguelite but narrative reference)
- Risk of Rain 2: Codex lore system, item descriptions as narrative
- Hollow Knight: Silent protagonist, world-building through exploration

For each:
1. How story is delivered (dialogue, environmental, item descriptions, cutscenes)
2. How roguelite repetition is USED for narrative (not fought against)
3. Relationship/NPC systems and their mechanical impact
4. How world-building is layered without overwhelming new players
5. Player motivation to keep running (narrative hooks)

Then analyze: RIMA is set in a "rift world" (dark fantasy, rift energy, fractured reality theme). Design a narrative framework: Who is the player? Why do they keep running? What NPCs should exist at the hub? How should lore be delivered through runs?
"""
    },
    {
        "baslik": "BOLUM 06 — UI/HUD Tasarımı ve Oyuncu İletişimi",
        "prompt": """
Analyze HUD/UI design and player communication in these action roguelites:
- Hades: Minimal HUD, boon tooltips, death recap screen
- Dead Cells: Blueprint system UI, mutation selection screen
- Enter the Gungeon: Item synergy UI, table-flip mechanic
- Risk of Rain 2: Item count UI, DPS meter, Void Fields timer
- Returnal: Weapon trait system display, malignant items UI
- TMNT: Splintered Fate: Multiplayer HUD elements

For each:
1. What information is always visible vs contextual
2. How skill/ability cooldowns are communicated
3. How resource systems (health, special resource) are displayed
4. Death feedback — what does the player learn from dying
5. Run summary screens — what stats matter
6. Accessibility features for HUD

Then analyze: RIMA has 4 classes with different resource systems (Rage, Mana, Energy/ComboPoints, Focus). Design the ideal HUD layout for RIMA. What should always be visible? How should the skill draft screen look? What does the death screen show?
"""
    },
    {
        "baslik": "BOLUM 07 — Multiplayer ve Co-op Roguelite Mekaniği",
        "prompt": """
Analyze co-op and multiplayer mechanics in roguelites:
- Hades II: Added co-op elements
- TMNT: Shredder's Revenge: 6-player co-op, combo system between players
- TMNT: Splintered Fate: Co-op roguelite mechanics
- Risk of Rain 2: Full multiplayer scaling, item sharing vs individual
- Deep Rock Galactic: Class synergy in co-op, shared objectives
- It Takes Two: Forced co-op mechanics (not roguelite but co-op reference)
- Gunfire Reborn: Co-op roguelite with class synergies

For each:
1. Player count supported and how difficulty scales
2. Shared vs individual loot — what works better
3. Revive system design
4. Class synergy in co-op (do classes need each other?)
5. Communication tools (built-in ping, etc.)
6. How co-op changes build strategy vs solo

Then analyze: Should RIMA have co-op? If yes: 2-player or 4-player? How should cross-class synergies work in multiplayer? How does the skill draft change? What revive mechanic fits RIMA's dark fantasy tone?
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
    print(f"  RIMA Mekanik Araştırması")
    print(f"  Model: {MODEL} | Bölüm: {toplam}")
    print(f"  Tahmini süre: {toplam*5}–{toplam*12} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# RIMA — Mekanik Araştırma Çıktısı\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# RIMA — Ham Veri\n*Sadece PART 1 | {datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

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
