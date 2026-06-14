ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Role
DESIGN + REFERENCE-GAME consult. No code. Bring game-design judgment + how shipped games solve these problems (Hades, Slay the Spire, Dead Cells, Children of Morta, Diablo). Your answer is read by Opus, who synthesizes with a parallel Codex (technical) consult + the user. Focus on design soundness, player-experience, and the BEST-PRACTICE technique for each idea — complement, don't duplicate, a code-feasibility pass.

# CONTEXT (RIMA — iso roguelite, LOCKED)
- ISOMETRIC floating-island rooms (top-down rejected; connected walls abandoned). Room = iso diamond floor + cliff edges floating in void + free objects. Dark stone + cyan #00FFCC seal-energy, sparing.
- Run = StS2-style branching node graph; player picks ONE path per fork, room -> room.
- Reference target images exist: a room with 3 glowing portal-doors (player on a cyan ritual circle, island floating in void), a room-to-room transition (next room visible across a void bridge), and an overhead branch map. The user explicitly does NOT want the abstract overhead map.

# USER'S NEW DESIGN PROPOSALS
1. DOORS -> PORTALS: radially-symmetric rift = 1 sprite + center VFX, color-coded per destination type. (Removes need for 8-directional door art.)
2. PORTAL TRAVEL: character morphs into a glowing ORB in the portal's color, gets sucked in, teleported to the chosen next room, camera follows.
3. MAP-FRAGMENT mechanic (collect -> reveal more of the map ahead): remove it, or repurpose?
4. REPLACE overhead map with REAL ROOM PREVIEWS: instead of an abstract node map showing everything, show the genuine next room(s) as real room views SIDE BY SIDE; player sees only as far ahead as a "reveal" allows. Likely diegetic (next rooms float in the void beside the current island).

# QUESTIONS (answer each: VERDICT + 2-4 line design recommendation + 1 reference game that does it well + the main pitfall)
Q1. Portal-as-rift, color-coded by destination: is color-coding enough to communicate destination type (combat/treasure/elite/boss/rest)? What extra readable signal do shipped games add (icon, framing, ambient)? Best on-brand VFX style for a pixel/painterly cyan rift?
Q2. Morph-into-orb teleport: does this read well / feel good, or is it gimmicky over many repetitions? Pacing concern (how long should it take so it doesn't slow a 10-min loop)? Any game that morphs the avatar for travel as a positive example?
Q3. Map-fragment: from a roguelite-design view, should "information about what's ahead" be a COLLECTIBLE reward (fog-of-war reveal, à la StS map / scouting), or always-free? If kept, what's the most satisfying thing it should reveal? Recommend remove vs repurpose.
Q4. Real-room-preview-as-choice (diegetic next rooms in the void): is showing the GENUINE next rooms (vs abstract icons) a good idea — does it spoil tension or increase it? How to keep it readable when 2-3 previews sit side by side? How far ahead should be visible by default vs gated by reveal? Reference for diegetic/in-world map.

# OUTPUT
Per-question verdicts as above, then a final 1-paragraph "RECOMMENDED OVERALL DESIGN DIRECTION" + the single biggest open question for the user. Concise.
