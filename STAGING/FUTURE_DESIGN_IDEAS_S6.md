# RIMA — FUTURE DESIGN IDEAS (captured 2026-05-31 PM5, user) — NOT YET BUILT

Two strong ideas the user wants noted for later. Both fit the locked iso cliff-floating-island + void canon and the
containment-failure story.

## 1. DIEGETIC StS MAP — branch choices rendered IN-WORLD as visible rooms
- Instead of an abstract node screen, the StS2-style branching is shown IN the world: **door count = branch count**
  (procedural per run — 1 exit = 1 door, 3 exits = 3 doors).
- Past each door, the chosen NEXT room is rendered as a **preview floating in the void** (smaller / further / BELOW —
  look past the cliff edge and see the next room floating beneath = descending-dungeon feel). You see its TYPE
  (combat/elite/treasure/boss) from its contents/icon BEFORE choosing.
- Pick a door -> camera travels/descends to that room; it becomes current, other previews close.
- **iso is the RIGHT perspective for this** (void-depth lets rooms float "below"; top-down would flatten it into a map).
- Have: RoomLoader spine, DungeonGraph (StS node graph, currently orphan -> wire), Gate/door, MapFragment loop.
  Need: next-room preview render + door-count=branch-count wiring + camera transition.
- Scope: demo = simple (door -> node-select -> load next room). Full "all next rooms visible floating below" = signature polish.

## 2. BONES / DEATH-MARKER — meaningful decor + per-run epic narration
- **Lore:** this place is a containment/prison (Penitent Sovereign, the seal, the Brink). The skeletons/bones strewn
  around are NOT random decor — they are **those who tried to escape/contain before you and FAILED.** Every bone =
  a fallen predecessor. Makes the environment narrative, not filler.
- **Mechanic (future):** within a run, when the profile DIES, drop a **bone/skeleton marker at the death location**
  (like Dark Souls bloodstains / "the fallen"). Persists during the run. Cleanup logic options to decide later:
  - clear bones after you CLEAR that room (kill = absolution), OR
  - bones accumulate as a lore/difficulty signal, OR
  - bones become lootable / "avenge the fallen" buff, OR other.
- User words: "öldüğü yere kemik koysak epic bi anlatım olur" — the dungeon remembers your failures.
- Ties to canon: Sundered Echoes = forgotten faces; bones = the failed containers. Reuse `decor_bones` object.

Both = post-demo polish; lock the exact mechanic when we build the map/run loop.
