"""
Harita Sistemi Araştırması — Ollama deepseek-r1:14b
6 bölüm: roguelite map sistemleri, oyuncu psikolojisi, Grudge entegrasyonu
"""

import requests
import json
import os
from datetime import datetime

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL = "deepseek-r1:14b"
OUTPUT_DIR = r"F:\Antigravity Projeler\2d roguelite\TASARIM"
OUTPUT_FILE = os.path.join(OUTPUT_DIR, "HARITA_ARASTIRMA_OLLAMA.md")
HAM_FILE = os.path.join(OUTPUT_DIR, "HARITA_HAM.md")

GPU_OPTIONS = {
    "temperature": 0.7,
    "num_predict": 6000,
    "num_ctx": 4096,
    "num_gpu": 99
}

OYUN_BAGLAMLARI = """
GAME CONTEXT (read before answering):
- 2D top-down flat roguelite action game
- 8 classes, dual-class system (pick 2 per run)
- Skill acquisition: Slay the Spire model — 3 choices after each room, max 6 skill slots
- Room types: Normal (wave clear), Elite (Grudge enemy), Shop, Boss, Flux (rare reforge), Secret
- Grudge system: Elites remember how they died → gain resistance to that strategy next time.
  Player can INTENTIONALLY program them (kill with X so they resist X, making Y easier)
- Flux rooms: max 1 per run, ~5-8% chance, swap a skill/passive
- Boss Soul: after boss kill, one skill mutates (player chooses mutation type)
- Meta progression: Hub unlocks between runs
- Solo dev, Unity 2D, Steam target — scope must stay manageable
- Reference games: Hades 1+2, Slay the Spire 1+2, Dead Cells, Enter the Gungeon, Risk of Rain 2,
  Balatro, Vampire Survivors, PoE, Diablo 2, Nioh, WoW, FFXIV, GW1, BDO, Lost Ark

KEY DESIGN TENSION: The Grudge system rewards PLANNING (knowing when you'll fight elites again).
A map that shows upcoming rooms supports planning. A mystery map creates tension but reduces agency.
"""

bolumler = [
    {
        "id": "01",
        "baslik": "Roguelite Map Sistemleri — Karşılaştırmalı Analiz",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Analyze the major roguelite map systems and their design tradeoffs.

Analyze these map systems in depth:
1. **Hades 1**: No explicit map — linear corridors, occasional binary choice between 2 paths.
   Player sees only current room and immediate next.
2. **Hades 2**: Actual node-based map per act — player sees branching paths, room types labeled.
3. **Slay the Spire / StS2**: Full branching map visible from act start — all room types shown,
   multiple path choices, convergence at elite/boss nodes.
4. **Dead Cells**: Partial — you see exits but not the full biome map. Some paths locked until
   unlocked via blueprints.
5. **Enter the Gungeon**: Floor map — you can see all rooms on current floor, move freely within floor.
6. **Risk of Rain 2**: Linear stage sequence — no map choice, teleporter to next stage.

For each system analyze:
- Player agency level (1-5)
- Planning depth (1-5)
- Tension/mystery (1-5)
- Implementation complexity for solo dev (1-5 where 5=hardest)
- Best suited for what kind of game

Then: which systems are most beloved by players and why? What are the biggest criticisms of each?

GENERATE ORIGINAL ANALYSIS — harmonize from all reference games.
"""
    },
    {
        "id": "02",
        "baslik": "Grudge Sistemi ile Harita Entegrasyonu",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Design the optimal map system specifically for a game with the Grudge mechanic.

The Grudge mechanic is CENTRAL to this game. Here's how it works in detail:
- Elite enemies carry a "Grudge Badge" showing what strategy killed them last time
- If you killed them with fire last run, they now have fire resistance
- INTENTIONAL PROGRAMMING: You can kill an elite with strategy X intentionally,
  knowing you'll face them again later in the same run and they'll be weak to Y
- Boss at end of act watched your entire run and adapted their patterns

DESIGN QUESTION: Does the map need to show upcoming room types for Grudge programming to be meaningful?

Analyze:
1. If player CANNOT see future rooms: How does Grudge programming work? Is it still meaningful?
   Or does it become random/frustrating?
2. If player CAN see future rooms (StS style): How does Grudge become a strategic tool?
   Example: "I see there are 2 more elites before boss — I'll program them with ice weakness now"
3. Hybrid approaches: Show only current floor? Show next 1-2 rooms only?
4. What information must be shown vs. what creates good mystery?

Specifically for our game: Should the player see:
- Room type (Normal/Elite/Shop/Boss/Flux)?
- Which specific elite is in an Elite room?
- Grudge Badge of elites on the map (their current memory)?
- Multiple path choices?

Generate 3 concrete map system proposals with their Grudge integration mechanics.
"""
    },
    {
        "id": "03",
        "baslik": "3 Act Yapısı ve Oda Dağılımı",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Design the act structure and room distribution for a complete run.

A full run should feel like: early game (build setup) → mid game (build crystallizing) → late game (build executing).

Design:
1. How many acts? (Hades has 6 biomes + Elysium; StS has 3 acts; Dead Cells has 4 zones)
   Recommend for our game with reasoning.

2. Room count per act — balance between:
   - Not too long (roguelite should be 30-60 min per run)
   - Enough rooms for build to develop
   - Grudge cycling (enough elites to interact with)

3. Room type distribution per act:
   - Normal rooms: how many?
   - Elite rooms: how many? (remember: Grudge needs enough encounters)
   - Shop rooms: how many? (Soul Dust economy)
   - Boss: 1 per act (confirmed design)
   - Flux rooms: max 1 per run (~5-8%)
   - Secret rooms: if any

4. Difficulty scaling across acts:
   - Enemy HP/damage scaling
   - New enemy types per act
   - Grudge intensity (do elites gain more memory later?)

5. Pacing rhythm within an act:
   - Where should elite rooms appear? Early/late/spread?
   - Where should shops appear?
   - Should there be "rest" rooms (no combat)?

Reference: How do StS (3 acts, ~15-17 rooms per act), Hades (6 chambers, ~3-5 rooms each),
and Dead Cells (4 biomes, variable) handle this?
"""
    },
    {
        "id": "04",
        "baslik": "Path Choice ve Oyuncu Ajansı",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Design the path choice system — how does the player navigate between rooms?

Core question: How much branching/choice should the map have?

Analyze and recommend:

1. **Binary choice (Hades 1 style)**: After most rooms, pick left or right path.
   - Simple to implement
   - Limited but present agency
   - No full map visible

2. **Full branching map (StS style)**:
   - See entire act's node structure from start
   - Multiple convergence/divergence points
   - Heavy planning, less surprise

3. **Guaranteed room types + random order**:
   - Each act guarantees: X normals, Y elites, 1 shop, 1 boss
   - Order is randomized but player sees next 2 rooms
   - Middle ground

4. **Curated paths**:
   - 3 distinct paths per act (each with different room composition)
   - "Aggressive path" (more elites, more rewards), "Safe path" (more shops), "Mystery path"
   - Player chooses path at act start, then plays linearly

For our specific game:
- Dual-class build crafting benefits from which system?
- Grudge programming benefits from which system?
- Solo dev feasibility?

Generate: recommended system with specific UI/UX details (what does the player see, when do they choose, how does it feel?)
"""
    },
    {
        "id": "05",
        "baslik": "Özel Oda Tipleri Tasarımı",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Design each special room type in detail — what happens in it, how it fits the map.

Design these room types:

1. **NORMAL ODA** (most common):
   - Wave structure (how many waves? how many enemies?)
   - Does difficulty scale within the act?
   - After clear: always gives skill/passive choice?

2. **ELİTE ODA** (Grudge encounter):
   - Entry experience: see the Grudge Badge before entering?
   - Combat: is it 1 elite, or elite + minions?
   - Reward: what's different from normal room? (better loot? guaranteed upgrade?)
   - Grudge feedback: after killing, what info is shown? ("Molten Grunt now remembers fire")

3. **SHOP ODA** (Soul Dust economy):
   - Layout: what's for sale? how many slots?
   - Refresh mechanic: can you reroll items?
   - Any special service? (Skill removal, forced upgrade)
   - How often should it appear?

4. **FLUX ODA** (Reforge/build pivot):
   - Entry: max 1 per run, how is it announced?
   - Experience: preview what you get before deciding what to give up
   - Corrupted variant: same room but darker, stronger option with downside

5. **GİZLİ ODA** (Secret room):
   - How does player discover it? (hidden passage, condition, item?)
   - What's inside? (puzzle, loot, lore?)
   - Should it even be in the game? (scope consideration)

6. **DİNLENME / HUB ODASI** (optional):
   - Does the game need rest rooms?
   - HP recovery mechanic (only in rest rooms? or per-room passive regen?)

For each: implementation complexity, player experience, and how it interacts with Grudge/build systems.
"""
    },
    {
        "id": "06",
        "baslik": "Harita Tasarım Sentezi + UI/UX + Solo Dev Önceliği",
        "prompt": f"""
{OYUN_BAGLAMLARI}

TASK: Synthesize everything into a final map system recommendation for this specific game.

Given everything analyzed:
- Grudge system needs some planning visibility
- Dual-class build crafting benefits from route agency
- Solo dev: keep it implementable
- Game feel goal: "MMORPG build insane moment" — not puzzle/planning game

SYNTHESIZE:

1. **Final map structure recommendation**:
   What EXACTLY does the map look like? Draw it in text/ASCII.
   How many acts? How many rooms per act? What does the player see?

2. **Path choice**: What exactly are the choices and when?

3. **Room type visibility**: What room types are revealed on the map, and when?

4. **Grudge integration on map**: What Grudge information appears on the map UI?

5. **Comparison to Hades vs StS**: Is this closer to one or the other? What did we borrow from each?

6. **Solo dev priority order**:
   - What's the minimum viable map for FAZ 1?
   - What's the full system for FAZ 3-4?
   - What can be added in FAZ 5 as polish?

7. **What makes this map system feel GOOD**:
   One paragraph on the player emotional experience — from dungeon start to boss kill.

Generate a concrete, implementable design. Not theoretical — actual design decisions.
"""
    }
]

def ollama_sor(prompt, max_tokens=6000):
    data = {
        "model": MODEL,
        "prompt": prompt,
        "stream": False,
        "options": {**GPU_OPTIONS, "num_predict": max_tokens}
    }
    try:
        resp = requests.post(OLLAMA_URL, json=data, timeout=300)
        resp.raise_for_status()
        return resp.json().get("response", "")
    except Exception as e:
        return f"HATA: {e}"

def main():
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    # Bolum 01 zaten tamamlandi — append modunda devam et (OUTPUT_FILE dokunma)
    with open(HAM_FILE, "w", encoding="utf-8") as f:
        f.write(f"# Harita Sistemi — Ham Ollama Ciktisi\n*{datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n")

    for bolum in bolumler:
        if bolum["id"] == "01":
            print(f"[01/06] Atlaniyor (zaten tamamlandi)")
            continue
        print(f"[{bolum['id']}/06] {bolum['baslik']}...")
        yanit = ollama_sor(bolum["prompt"])

        with open(OUTPUT_FILE, "a", encoding="utf-8") as f:
            f.write(f"## BOLUM {bolum['id']} — {bolum['baslik']}\n\n")
            f.write(yanit)
            f.write("\n\n---\n\n")

        with open(HAM_FILE, "a", encoding="utf-8") as f:
            f.write(f"### [{bolum['id']}] {bolum['baslik']}\n\n{yanit}\n\n---\n\n")

        print(f"  [OK] Tamamlandi")

    print(f"\n[DONE] Tum bolumler tamamlandi.")
    print(f"   Cikti: {OUTPUT_FILE}")

if __name__ == "__main__":
    main()
