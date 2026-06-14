# AX (Gemini) TASK — Floor iso vs top-down: ART-DIRECTION / REFERENCE verdict

You are an art-direction + 2D-rendering consultant for a Unity URP 2D action-roguelite (RIMA).

## SITUATION
The game's locked art direction is **HIGH TOP-DOWN 3/4 (~70-80 degrees from horizon)**, references Hades / Children of Morta / Diablo III. Camera is a flat 2D orthographic camera (no 3D tilt), Y-sorted via Custom Axis. Character sprites are drawn top-down 3/4. Cliffs/walls are tall sprites (128x192) with visible vertical faces, Hades-style.

PROBLEM: the FLOOR is currently built with a Unity **Isometric tilemap** (cellLayout=Isometric, ~2:1) using **35-degree isometric diamond floor slabs**. Result: the floor reads as a diagonal iso diamond grid, which fights the top-down characters/cliffs — "the floor doesn't sit right." A square TOP-DOWN stone-floor tileset already exists unused in the project.

## ANSWER THESE (be concrete, cite how the reference games actually do it)
1. How do Hades, Children of Morta, Bastion, Diablo-likes actually author their FLOOR for a top-down 3/4 look? Do they use Unity-style isometric diamond tilemaps, or rectangular/free-painted top-down floors with the 3/4 illusion coming from ART + tall props + Y-sort? Be specific about grid vs hand-painted.
2. For a flat 2D ortho camera + top-down 3/4 sprites, WHY does a 35deg iso diamond floor look wrong next to them? Explain the perspective mismatch in plain terms.
3. What is the RIGHT floor approach to make it "sit right" under top-down 3/4 sprites? (rectangular grid + top-down tile art? subtle vertical foreshortening in the tile art? no visible grid lattice at all?) Give the recommended tile art characteristics (square vs diamond, how much top-edge thickness, seam/lattice visibility, value contrast to avoid diagonal noise).
4. If we switch from iso diamonds to top-down square tiles, what art pitfalls cause a top-down floor to look "flat/boring" and how do top games avoid it (value variation, decals, large-scale tonal patches, debris, light pooling)?
5. One-paragraph recommendation + a short bullet list of concrete dos/don'ts for the RIMA floor.

Keep it focused and practical. This is consultation only.
