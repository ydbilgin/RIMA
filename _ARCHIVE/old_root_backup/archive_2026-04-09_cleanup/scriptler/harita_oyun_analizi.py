"""
Harita Sistemi — Oyun Bazli Derin Analiz
Her referans oyunun harita sistemi tam olarak nasil calisiyor?
Neden o karari aldiler? Bizim icin ne anlam ifade ediyor?
"""

import requests
import json
import os
from datetime import datetime

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL = "deepseek-r1:14b"
OUTPUT_DIR = r"F:\Antigravity Projeler\2d roguelite\TASARIM"
OUTPUT_FILE = os.path.join(OUTPUT_DIR, "HARITA_OYUN_ANALIZI_OLLAMA.md")

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
  This means: seeing WHEN you'll fight elites again is strategically important.
- Room types: Normal (wave clear), Elite (Grudge encounter), Shop, Boss, Flux (rare reforge), Secret
- Flux rooms: max 1 per run, ~5-8% chance — rare and meaningful
- Boss Soul: after boss kill, one skill permanently mutates
- Meta progression: Hub between runs, permanent unlocks
- Run length target: 35-55 minutes
- Solo dev scope: must be implementable by 1 person
"""

bolumler = [
    {
        "id": "01",
        "baslik": "Hades 1 — Harita Sistemi Tam Analiz",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Deep analysis of Hades 1's map/navigation system.

Analyze in full detail:

**HOW IT WORKS:**
- Hades 1 has NO explicit map. Describe exactly how navigation works.
- What does the player see at any given moment? (Room exits, reward previews, etc.)
- How does the "reward preview" work — what info is shown before entering a room?
- How many rooms per "chamber" (biome)? What's the branching structure?
- How does the player choose between rooms? Binary choice? More?
- What room types exist and how are they distributed?
- How does the Chaos gate system work?

**WHY SUPERGIANT MADE THESE DECISIONS:**
- Why no explicit map? What does this serve narratively and mechanically?
- Why show reward previews? What player behavior does this encourage?
- Why binary (2 exit) choices instead of more?
- How does the linear-ish structure serve the game's narrative (escaping Hades)?

**WHAT PLAYERS LOVE AND HATE:**
- What do players love about this system? (specific community feedback)
- What are the criticisms? (what do players find frustrating or limiting?)
- How does not seeing the map affect replayability?

**WHAT WE CAN TAKE:**
Given our game's specifics (Grudge system, dual class, skill acquisition), what elements from Hades 1's map system would work for us, and what definitely wouldn't? Be specific.
"""
    },
    {
        "id": "02",
        "baslik": "Hades 2 — Harita Sistemi Tam Analiz (Hades 1'den ne degisti?)",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Deep analysis of Hades 2's map system, focusing on what changed from Hades 1 and WHY.

**HOW IT WORKS (Hades 2 specific):**
- Hades 2 introduced an actual visual node-based map. Describe it exactly.
- What does the map screen show? Room types? Enemy previews? Boon previews?
- How many nodes per region? How much branching?
- What's the "Surface" vs "Underworld" structure?
- How do Crossroads work as a hub?

**WHY THEY CHANGED FROM HADES 1:**
- What specific problems in Hades 1 did Hades 2's map solve?
- Was the change driven by player feedback, design philosophy, or gameplay needs?
- What did they sacrifice by adding a visible map? (what did they lose from Hades 1's blind system?)

**EARLY ACCESS RECEPTION:**
- How did players react to the new map system vs Hades 1?
- What do players prefer and why?
- Has the map system been iterated on during Early Access?

**WHAT WE CAN TAKE:**
Specifically: how does Hades 2's map system handle player planning vs mystery balance? What's the exact UI/UX? What would work for our Grudge system?
"""
    },
    {
        "id": "03",
        "baslik": "Slay the Spire 1+2 — Harita Sistemi Tam Analiz",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Deep analysis of Slay the Spire's map system (both 1 and 2).

**HOW IT WORKS (StS 1):**
- Describe the map system in full detail. What does the player see at act start?
- How many nodes per act? What's the exact branching structure?
- What are all the room types and their icons/symbols?
- How do path choices work — when must you commit to a path?
- What's the convergence structure (where do paths merge)?
- How do Elite rooms work — reward vs risk?
- How does the Boss node always being at the bottom work?
- What's the "? room" and how does mystery work within a visible map?

**WHY MEGA CRIT MADE THESE DECISIONS:**
- Why show the ENTIRE map from act start? What does full visibility enable?
- Why have multiple paths converge before boss? What experience does this create?
- Why are Elite rooms optional (you can route around them)? What player psychology does this serve?
- How does the visible map support the deck-building meta-game?

**StS 2 CHANGES:**
- What changed in StS 2's map system vs StS 1?
- Any new room types? Different branching? Different visibility?

**STRENGTHS AND WEAKNESSES:**
- Full visibility: what does it enable? What does it kill?
- How does "path optimization" feel — is it fun or does it become a solved puzzle?
- What do players who dislike StS's map criticize?

**WHAT WE CAN TAKE:**
Our game is action-focused ("build insane"), not a puzzle/planning game. What specific elements of StS's map would enhance our game vs which would shift the feel toward "planning game"?
"""
    },
    {
        "id": "04",
        "baslik": "Dead Cells — Harita Sistemi Tam Analiz",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Deep analysis of Dead Cells' map/biome system.

**HOW IT WORKS:**
- Dead Cells has biomes (zones) connected by transitions. Describe the full structure.
- How does the biome selection work? Can players choose which biome to go to?
- What branching exists between biomes? How many paths through the game?
- Within a biome: is it a single scrolling level or rooms? How big are biomes?
- What information does the player have about upcoming biomes?
- How do locked biomes (requiring specific runes/items to access) work?
- How does the "transition room" / exit selection work?

**THE BOSS CELLS SCALING SYSTEM:**
- How does difficulty selection (Boss Cells) interact with available paths?
- Does path variety change with difficulty?

**WHY MOTION TWIN MADE THESE DECISIONS:**
- Why a continuous scrolling level rather than discrete rooms?
- Why lock certain paths behind progression unlocks?
- What does biome variety serve over a single dungeon?

**WHAT PLAYERS LOVE/HATE:**
- Route memorization: feature or bug?
- How does the "multiple paths to the end" design affect replayability?
- What do players find frustrating about Dead Cells' navigation?

**WHAT WE CAN TAKE:**
Dead Cells uses discrete room-to-room navigation like us in some ways. What specific ideas transfer?
"""
    },
    {
        "id": "05",
        "baslik": "Enter the Gungeon + Risk of Rain 2 + Balatro — Harita Karsilastirmasi",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Analyze the map/navigation systems of Enter the Gungeon, Risk of Rain 2, and Balatro.
These three represent very different approaches. Extract what's useful for our game.

**ENTER THE GUNGEON:**
- Floor-based map: player can see all rooms on current floor and navigate freely. Describe exactly.
- What room types exist? How are they distributed per floor?
- How does the "secret room" discovery work?
- How does the shop/vendor system tie to navigation?
- What does free floor exploration (vs linear) enable?
- Player reception: what do they love/hate about the Gungeon's floor map?

**RISK OF RAIN 2:**
- Pure linear stage sequence — no map choice at all.
- How does stage progression work? (teleporter mechanic)
- What's the relationship between time spent in stage and difficulty?
- Why does RoR2's complete linearity WORK despite no choice? What makes it feel good?
- What player needs does it NOT serve? Why do some players dislike it?

**BALATRO:**
- Balatro has "blinds" (small/big/boss) within an "ante" (act-like structure).
- How does navigation work? Is there choice?
- What's the "skip blind" mechanic and what player psychology does it serve?
- How does Balatro create tension without a spatial map?

**SYNTHESIS FOR OUR GAME:**
- RoR2's linearity: what can we take (if anything) for our early prototype (FAZ 1)?
- Gungeon's free floor exploration: would this work for our room-based design?
- Balatro's ante/blind structure: any parallel to our act/room structure?
- What does "navigation freedom" do to combat pacing in action roguelites?
"""
    },
    {
        "id": "06",
        "baslik": "PoE Atlas + D2 Act Yapisi — ARPG Harita Dersleri",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Analyze Path of Exile's Atlas and Diablo 2's act structure for map/navigation lessons relevant to our game.

**PATH OF EXILE — ATLAS OF WORLDS:**
- The Atlas is PoE's endgame map system. Describe how it works.
- How does the node-based Atlas map work? What does the player see?
- How does "map tier" and "map adjacency" work?
- How do Watchstone/Atlas passive skills change available paths?
- What's the "target farming" concept — how does the Atlas allow players to pursue specific encounters?
- How does player agency in the Atlas create "build-enabling" moments?
- What lessons does PoE's Atlas teach about giving players control over encounter types?

**DIABLO 2 — ACT STRUCTURE:**
- D2 has 5 acts with linear story progression but open exploration within acts.
- How does within-act exploration work? (Randomized dungeons, fixed overworld)
- How does the waypoint system work and what player behavior does it enable?
- Why does D2's "known act structure + randomized interiors" still feel fresh after 20 years?
- What does D2 teach about balancing "known framework" with "random content"?

**NIOH — MISSION SELECT MAP:**
- Nioh uses a Japan map with selectable missions. How does this work?
- How does knowing mission contents in advance (via description) affect player preparation?
- What does this teach about pre-encounter information?

**LESSONS FOR OUR GAME:**
- PoE Atlas: target farming concept → can our map let players "target" specific Grudge elites?
- D2: known structure + random interior → how does this apply to our act/room design?
- Nioh: mission info upfront → how much info should our Elite room entry screen show?
- What does ARPG navigation teach that pure roguelite navigation doesn't?
"""
    },
    {
        "id": "07",
        "baslik": "Bizim Oyunumuza Ozel Sentez — Elimizdeki Kosullar + En Iyi Karar",
        "prompt": f"""
{OYUN_BAGLAMI}

TASK: Based on everything analyzed (Hades 1+2, StS, Dead Cells, Gungeon, RoR2, PoE, D2),
synthesize the optimal map system specifically for our game.

Our specific constraints and design pillars:
1. GRUDGE SYSTEM requires knowing when you'll fight elites again (planning ahead)
2. DUAL-CLASS BUILD CRAFTING is the core fantasy — map must support "build insane" moment
3. SKILL ACQUISITION (StS model) means each room is a build decision — pacing matters
4. SOLO DEV — complexity must be manageable
5. ACTION GAME — not a puzzle/planning game, map shouldn't feel like homework
6. Run length: 35-55 minutes
7. 8 classes, cross-class synergies — player wants to explore combinations

Go through each constraint and explain which map systems from the reference games best serve it:

**FOR GRUDGE PLANNING:**
What visibility level is needed? What specific information must be shown? Examples from which games?

**FOR BUILD CRAFTING PACING:**
How should room distribution support the "weak early → crystallizing mid → insane late" build arc?
Which game's pacing model is closest to this?

**FOR SOLO DEV SCOPE:**
What's the minimum viable map system that still feels good? What can be cut?

**FOR ACTION GAME FEEL:**
How do we prevent the map from becoming a "puzzle to solve before playing"?
Which systems keep the focus on combat, not navigation?

**FINAL RECOMMENDATION:**
- Exact act structure (how many acts, how many rooms, what types)
- What the map screen shows and when
- How path choices work
- What info is shown before entering each room type
- FAZ 1 minimum vs FAZ 3 full system
- One paragraph: what does our map system feel like to play?
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

def main():
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    with open(OUTPUT_FILE, "w", encoding="utf-8") as f:
        f.write(f"# Harita Sistemi — Oyun Bazli Derin Analiz (Ollama)\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | 7 bolum*\n\n---\n\n")

    for bolum in bolumler:
        print(f"[{bolum['id']}/07] {bolum['baslik']}...")
        yanit = ollama_sor(bolum["prompt"])

        with open(OUTPUT_FILE, "a", encoding="utf-8") as f:
            f.write(f"## BOLUM {bolum['id']} — {bolum['baslik']}\n\n")
            f.write(yanit)
            f.write("\n\n---\n\n")

        print(f"  [OK] Tamamlandi ({bolum['id']}/07)")

    print(f"\n[DONE] Tum bolumler tamamlandi.")
    print(f"   Cikti: {OUTPUT_FILE}")

if __name__ == "__main__":
    main()
