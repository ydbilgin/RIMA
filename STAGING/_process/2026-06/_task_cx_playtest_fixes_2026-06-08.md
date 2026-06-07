ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself.

# Amaç
User did a live playtest and reported visual/feel bugs. INVESTIGATE root causes in code, FIX the 2 unambiguous value-tweaks, and REPORT root-cause + proposed fix (file:line) for the structural ones. Do NOT guess on structural fixes — report and stop.

## Screenshots (Codex can open these PNGs)
- `C:\Users\ydbil\.claude\image-cache\98e8b6e4-9607-4121-a6ab-94030b17e330\1.png` (scene view, room with pedestal ring + reward + door)
- `...\2.png` (GAME view of same room — HP/stamina bars top-left, 10-pedestal ring center, BIG cyan diamond reward right with "[G] Ödülü Al", stone-arch door, hotbar bottom)
- `...\3.png` (scene view, mostly EMPTY dark diamond room, camera + colored spawn gizmos center)
- `...\4.png` (GAME view, almost fully DARK — only character + HUD bars + hotbar visible, "nothing visible")

## Orchestrator's authoritative VISUAL read (trust this; your job is the CODE root cause)
1. **Reward too big.** The cyan diamond reward sprite dwarfs the player (~2x too large). FIX: scale it down to roughly player-height-ish (target ~0.5-0.6 of current). Find the reward pickup prefab/sprite scale.
2. **Camera too close / zoomed in.** Player + room feel cramped. FIX: zoom OUT (increase orthographic view / reference resolution / PPC). Find the live camera config used in the run/combat scene.
3. **Pedestal RING appears in a COMBAT room** (HP bars + reward + "[G] Ödülü Al" are present in image 2, yet a 10-disc attunement-pedestal ring sits in the center = nonsense for combat). AND user reports "3 Warblade, other classes missing, statues spawn in nonsense positions." This contradicts the earlier static finding (10/10 idle_south sprites on disk). → RUNTIME / scene-bleed bug. INVESTIGATE: what scene/template actually loads on Play? Is ChamberSelectBootstrap or its pedestal/statue spawn running inside a combat room? Why 3 identical Warblade statues instead of 10 distinct (or none) in a combat context? Report file:line root cause + proposed fix. DO NOT fix yet.
4. **Door issues:** (a) door (stone archway portal) is hard to LOCATE when it unlocks; (b) door should be **[G]-interact, NOT walk-through** (walking into it must not pass/teleport — like the rift). INVESTIGATE the door/gate interaction model in the run rooms (DoorTrigger / GateBehavior / IntraEncounterDoorTrigger / RoomRunDirector). Report current model (walk-trigger vs G-interact) + file:line + proposed change to G-interact + a locator idea. DO NOT fix yet.
5. **Image 3:** entry/first room on Play; user says the door/portal "should appear BLACK (silhouette) no matter what" but doesn't. INVESTIGATE the entry room's door render. Report only.
6. **Image 4 "nothing visible":** a run room is far too dark — no props/mobs/reward visible, just dark floor. INVESTIGATE: is this an empty room template, a lighting/global-light issue, or camera framing an empty area? Report likely cause + file:line. DO NOT fix yet.

## DO NOW (safe, unambiguous):
- Fix #1 (reward scale down) and #2 (camera zoom out). Keep changes surgical and data/config-level if possible. After each, note the exact file:line + old→new value.

## VERIFY
- `refresh_unity` + `read_console` → 0 compile errors. If you changed a prefab/scene/SO, note it.
- Do NOT commit yet — orchestrator will review the report + the 2 fixes, then route the structural fixes. Write everything to CODEX_DONE.md:
  - The 2 applied fixes (file:line, old→new).
  - Root-cause report for items 3,4,5,6 (file:line + proposed fix each), clearly marked "PROPOSED — NOT APPLIED".
  - Which scene actually loads on Play (the Build Settings scene 0 / the flow MainMenu→Chamber→run) and whether HUD + pedestals coexist by design or by bug.
