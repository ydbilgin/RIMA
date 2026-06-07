**RIMA - Room Design Council: Gameplay-Feel Synthesis**

Here is the senior combat/level design breakdown for RIMA. The overarching philosophy: **Rooms are weapons.** A flat arena is a missed opportunity; the geometry must actively interact with the player's dash, knockback mechanics (Warblade/Brawler), and ranged/melee dynamics. 

### 1. Shape Evaluation & Gameplay-Feel (The ChatGPT Pack)
The ChatGPT pack provides great perimeter variety but fails on internal structure (they are all flat floors). 

**S-Tier (Must Keep - Identity Defining):**
*   **`donut`:** The absolute best for RIMA. The center void hole turns the entire inner ring into a high-risk/high-reward knockback kill zone. Wraparound movement forces tactical kiting. 
*   **`hourglass`:** The central waist creates a brilliant chokepoint. Players must make risky dash-through decisions to reach ranged enemies on the other side.
*   **`bridge-lobes`:** Perfect for melee vs. ranged dynamics. Ranged enemies spawn in lobe B, firing across the void; player in lobe A must dash the bridge under fire. 
*   **`l-shape`:** Exceptional for corner pressure and ambush spawns. Allows kiting players to break line-of-sight (LoS).

**A-Tier (Solid Foundations - Keep & Tweak):**
*   **`cross`:** Great E-W combat lanes, but currently favors ranged enemies too heavily. **How to improve:** Add 4 central pillars or internal void pits to break LoS and provide cover for melee gap-closing.
*   **`elite_crescent` / `elite_trident`:** Excellent for directing player attention. Prongs dictate exactly where threats will emerge.

**C-Tier (Cut or Rework):**
*   **`organic-blob`:** Too amorphous. It’s just a big empty space. Cut it, or add massive central hazard zones/void holes to give it structural identity.
*   **`teardrop`:** The tail ends up as dead space once the player moves to the fat center. 

### 2. Size Variety & Distribution
**Having an all-large room pack is a pacing disaster.** If every room is large, positioning becomes too forgiving, and combat feels like a marathon. 
*   **Small rooms** are essential for claustrophobic, high-lethality spikes. They force the player to aggressively use knockbacks because there is no room to kite.
*   **Run Distribution (10-15 rooms):** ~20% Small (adrenaline spikes), ~50% Medium (tactical baseline), ~30% Large (climactic wave endurance). The ChatGPT pack should *only* be used for the 30% Large/Elite slots.

### 3. Room Type Scope
*   **Keep our internal library** for `Shrine`, `Spawn`, `Merchant`, and `Treasure`. These need to feel distinct, safe, and easily readable (usually smaller, symmetrical, and structurally stable compared to the "shattered" combat rooms).
*   **Missing Types to Add:** 
    *   **Hazard/Trap Rooms:** Light combat, heavy environmental threats (collapsing floors, laser sweeps).
    *   **Choice/Split Rooms:** Combat rooms that deliberately fork into multiple exits, acting as physical run-branching nodes.

### 4. Mechanics to Map Shapes
*   **Void-Edge Knockback:** Needs convex corners and central void holes. `donut`, `diamond`, and `hourglass` waist.
*   **Chokepoints (Melee vs Ranged):** Needs narrow connectors. `bridge-lobes`, `hourglass`.
*   **Multi-Lobe Flank:** Needs distinct zones. `twin-basins`. Waves should spawn in the lobe the player *isn't* in, forcing them to push into the flank.
*   **Cover/Pillar Zones:** Crucial missing element. `cross` and `diamond` must be updated with non-walkable cover blocks to allow summoners/casters to breathe and melee to approach ranged elites safely.

### 5. Run Pacing & Sequencing
A Hades-style 12-room run requires a "Tension-Breath" rhythm. 
*   **Beat 1 (Warmup):** Spawn -> Small Combat -> Medium Combat
*   **Beat 2 (Spike):** Large Combat (Bridge/Donut) -> Elite (Trident)
*   **Beat 3 (Breath):** Shrine / Merchant (Safe)
*   **Beat 4 (Climax):** Medium Combat -> Large Combat -> Boss (Shattered Oval)
Sizes must fluctuate. Never sequence two Large rooms back-to-back unless one is an Elite modifier.

### 6. FINAL RECOMMENDED SET (Type × Shape × Size)
Here is the actionable matrix to build/audit against:

**Combat Small (Need to Author - High Tension, High Void-Kill potential)**
*   `Small_Square_Pillar` (Single center pillar to loop around)
*   `Small_Diamond` (pure knockback arena)

**Combat Medium (Downscale ChatGPT / Use Library)**
*   `Med_L-Shape` (Adapt from ChatGPT)
*   `Med_Hourglass` (Adapt from ChatGPT - tight waist)
*   `Med_Cross_Cover` (Cross shape + 4 LoS pillars)

**Combat Large (Import from ChatGPT)**
*   `Large_Donut` (Keep as-is)
*   `Large_Bridge_Lobes` (Keep as-is)
*   `Large_Twin_Basins` (Keep as-is)
*   *Skip `organic-blob` and `teardrop`.*

**Elites & Bosses (Import from ChatGPT)**
*   `Elite_Trident` / `Elite_Crescent` (Keep as-is)
*   `Boss_Shattered_Oval` (Add internal hazard rings)

**Non-Combat (Use Existing Library)**
*   `Shrine_01`, `Spawn_01`, `Treasure_01`, `Merchant_01` (Keep these small, clean, and distinct from the chaotic combat islands).

