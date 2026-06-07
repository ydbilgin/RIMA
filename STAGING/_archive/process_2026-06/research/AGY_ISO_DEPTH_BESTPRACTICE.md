# Task: Best-practice for depth-sort + movement + walkability in a TOP-DOWN 3/4 game with an iso-rendered floor

ACTIVE RULES: (1) think before answering (2) cite real engines/games/docs (3) give a concrete recommended architecture (4) BLOCKED only if stuck.
NLM ACCESS: do NOT query NLM. Direct-read only this file.
RESPOND INLINE -> AGY_DONE_ydbilgin.md. Only agy task now. Your web research is the value.

## The situation (RIMA, Unity URP 2D)
Top-down 3/4 ARPG (Hades / Children of Morta / Diablo look). The floor is built as a Unity **Isometric** Tilemap (diamond cells, cellSize 1 x 0.61). Player moves top-down cardinal (WASD = world X/Y). Three problems:
1. **Depth ("3D" feel):** the player currently renders ON TOP of tall objects (pillars/columns) even when standing BEHIND (north of) them. We want pillars to OCCLUDE the player when the player is behind them, and the player in front when below — i.e., proper Y-depth sorting so the scene reads 3D.
2. **Walkability matches the visible floor:** the player should be blocked exactly at the visible floor edge and be able to traverse narrow bridges, with FREE top-down movement kept (not iso-locked controls).
3. Tall objects should be obstacles you walk AROUND, not stand on.

## Answer these (with sources)
1. **Depth sorting — the canonical Unity 2D approach:** 
   - Manual `sortingOrder = -round(worldY * k)` per object (a "YSort" component) vs Camera **Transparency Sort Mode = Custom Axis (0,1,0)** (engine sorts by Y automatically within a sorting layer). Which is the industry-standard for top-down 2D depth? Pros/cons. Can they CONFLICT (manual order vs custom-axis)? If a project uses Custom Axis, should per-object sortingOrder be left equal (let the axis sort) instead of manually set?
   - The **pivot convention**: where should a character's and a prop's pivot/sort-point be so they sort correctly (feet/ground point)? How do tall objects (pillars) sort correctly when their sprite extends far above their base?
   - Tall-object occlusion (player partially hidden behind a pillar): is plain Y-sort enough, or do you need sprite "cutoff"/multi-part sorting? How do real 2.5D pixel games (CrossCode, Hyper Light Drifter, Moonlighter, Eastward, Sword & Sorcery) do it?
2. **Iso tilemap + top-down cardinal movement:** is it sound to render the floor as a Unity Isometric Tilemap while keeping free cardinal movement, or does that cause the "blocked on narrow diagonal bridge" problem? How do top-down games author bridges/walkable so free movement works? (e.g., avoid 1-tile diagonal bridges, or use a separate walkability grid decoupled from the iso render.)
3. **Walkability source of truth:** should walkability be a physics collider, or a logical grid checked in code (sample at the character's FEET point)? For an iso-rendered floor with free movement, what's the robust pattern so "walkable == the visible floor"?

## Deliver (inline)
- A clear recommended ARCHITECTURE for RIMA: (a) depth-sort method (manual YSort vs Camera Custom-Axis) + pivot rule, (b) walkability method, (c) how to keep free top-down movement while the floor is iso-rendered. 
- Lead with a 1-paragraph verdict, then specifics. Cite real games/Unity docs.
