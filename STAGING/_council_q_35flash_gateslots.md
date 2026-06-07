# COUNCIL — LEAN LENS (Gemini 3.5 Flash): Authored gate slots

READ: `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T1-T9 queue, jury deadline) + `STAGING/PORTAL_PACK_DECISION_2026-06-06.md`.

USER wants: Map Designer'da her template için ENTRY belli + 3 EXIT slotu authored; runtime graph'a göre 1/2/3 kapıyı O SLOTLARDA render etsin. Mevcut: kuzey-anchor merkezli sıra (çalışıyor, 26/26 smoke), RoomSocketQCTool auto-fix, Rooms tab (liste+2D şematik önizleme+butonlar).

Ruthless lean questions (~600 words):
1. **Is full designer click-to-place slot editing worth it before the jury?** Or is the 80% solution: extend Fix Sockets to auto-compute NW/N/NE from back-edge walkable cells + show slots as colored dots on the EXISTING 2D schematic preview (read-only) + Inspector nudge for the rare bad case? Hour estimate for both paths.
2. **Cheapest correct selection logic:** write the actual statements (1 door→?, 2→?, 3→?, narrow room with 2 slots→?). Keep it dumb and deterministic.
3. **Migration trick:** 26 templates already have door_N + side door_W/E sockets. Cheapest migration (auto-convert W/E→NW/NE back-edge? just regenerate all via Fix Sockets?).
4. **Time bombs:** what blows up here (organic back edges, slot-on-cliff seam, portal sprite wider than slot spacing, graph child count > slot capacity)?
5. **Sequencing:** do slots as part of T3 (portal layer) in one cx task, or as a separate 1-2h pre-task? Which ships faster overall?
Disagree freely.
