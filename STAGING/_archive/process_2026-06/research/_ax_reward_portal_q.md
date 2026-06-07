You are a senior game designer consulting on RIMA — a 2D high-top-down 3/4 isometric roguelite ARPG (think Hades / Children of Morta / Diablo III camera), Act 1 "Shattered Keep": shattered floating stone islands suspended in a purple void, cyan seal-energy (#00FFCC) rifts/cracks, 10 playable classes, run loop = clear a room of enemies -> take a portal -> next room, across several maps, then a boss. Art = pixel-art (PixelLab), PPU64, Pixel-Perfect URP-2D camera 640x360.

CANON (from the project's design notebook — authoritative, do not contradict):
- Long-term reward primitives = Map Fragment + Skill Draft. Skill Draft = a 3-choice skill offer (a SkillOfferGenerator already exists in code).
- Hub NPCs (between runs): Ferryman (lore/witness), Vrel (craft/augment using Boss Fragments), Sister Mourne (HP/healing), Cartographer (map upgrade/expansion using "Echo" currency).
- Door->Portal is already a locked decision (rooms connect via portals, not doors). Phased portal plan: P1 = a single portal at the room exit awards a Skill Draft 3-choice; P2 = "3-portal fan" laid along the island edge, each portal = a different next-room TYPE (combat / elite / treasure), a FanLayoutSolver exists; P3 = portals + "preview islands" (a real, mob-less low-detail preview of the next room shown across the void, traveled to via a cyan orb).
- Current live state: on room-clear a single reward pickup (a relic sprite) drops at room center. Portals/preview are coded but not yet wired into the live playable scenes.

Answer TWO questions. Be concrete and prescriptive (we will implement your recommendation). Prose + short bullets.

== Q1: REWARD SYSTEM (full design) ==
Give a shippable design for what the player EARNS and HOW it is presented, tied to the run loop + meta-progression. Cover explicitly:
- In-run rewards: what drops per normal room vs per elite room vs per treasure room vs per boss. (Skill Drafts? a run currency? HP/heal? consumables?)
- Reward CADENCE: every room, or only special rooms? auto-pickup vs a choice screen (and when each).
- How the 3 meta currencies/items connect: Echo (Cartographer map upgrades), Boss Fragment (Vrel craft/augment), Map Fragment + Skill Draft (canon primitives). What does the player carry OUT of a run into the hub?
- Skill Draft mechanics: when offered, 3 choices, rarity/weighting, can you bank/reroll?
- A clear MVP-FIRST -> FULL sequencing (what to build this week vs later), respecting the phased portal plan above.
Keep it RIMA-specific, not generic roguelite boilerplate.

== Q2: PORTAL ORIENTATION (decide) ==
For this high-top-down 3/4 iso camera, should a room-exit portal be:
  (A) VERTICAL — an upright standing rift / doorway whose plane roughly faces the camera (player walks INTO it), OR
  (B) HORIZONTAL — a flat glowing disc / pool lying on the ground (player steps ONTO it)?
Recommend ONE. Justify with: readability at this camera angle, the "cyan rift torn in the void" fantasy, the "3-portal fan along the island edge" layout, and the "portals rise/grow from the edge" room-clear reveal beat. Then give the practical pixel-art/sprite implications: canvas size guess, whether it needs 8-directional art or a single billboard, pivot, sorting vs the player, idle shimmer animation, and how an icon (room-type for the 3-fan) would sit on it. End with a one-line VERDICT: "Portal = VERTICAL" or "Portal = HORIZONTAL".
