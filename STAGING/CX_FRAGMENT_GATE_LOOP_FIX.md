# Task: Fix the broken demo loop — map fragment does NOT drop on room-clear + gates do NOT open per run

ACTIVE RULES: (1) think before coding (2) minimal surgical fix (3) cite file:line; touch only the loop path (4) BLOCKED + findings if a ref is scene-only (Unity-side) — say exactly which scene refs are missing.
NLM ACCESS: optional; if it errors proceed from code. Direct-read: the loop scripts + `PlayableArena_Test01.unity` + CURRENT_STATUS.md + this file.
RESPOND INLINE -> CODEX_DONE.md. cx task (profile yekta).

## The live bug (user playtest, 2026-05-30)
In `PlayableArena_Test01`, the demo loop is broken at runtime:
1. **Killing all mobs in a combat room does NOT drop the Map Fragment.** (Canon loop: clear wave → fragment drops at center → pickup → reveals next room → 3-card draft → gate unlocks.)
2. **Gates/doors do NOT open per run** (should unlock after the room's clear/fragment-pickup condition each run).
The fix code for these supposedly landed earlier (BLOCK A3 "fragment drop on combat-room clear → pickup→draft→Gate.Unlock"; "Gate.Unlock idempotence"). But it does NOT fire live. Find the REAL break.

## Investigate the live path (cite file:line)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` — how does it detect "room cleared"? Where does it spawn/drop the MapFragment? Is the clear event actually subscribed (mob-death → count → clear)? Is `MapFragmentSpawner.fragmentPrefab` / drop anchor null?
- `MapFragment` (Environment.MapFragment — the LIVE one, NOT Core.MapFragment) + `MapFragmentSpawner` — is the prefab reference wired? Does it instantiate?
- `Gate` / gate-unlock — what condition calls `Gate.Unlock()`? Is it gated on fragment pickup or clear? Is it firing? Per-run reset?
- The enemy/mob death → room clear counter: is the combat room actually counting kills and raising a "cleared" event? (FractureImp etc.)
- Check whether the live `RoomSequenceData` for the phase rooms even has a fragment/gate configured, or if `useFragmentGateFlow` / `clear-to-unlock` flags are set wrong.

## Deliver
1. **Root cause** of BOTH (drop + gate) — code bug vs missing SCENE ref. If a scene ref is null (e.g. `fragmentPrefab`, gate target, spawner anchor), name the exact component+field so the orchestrator can wire it in Unity.
2. **The code fix** (surgical) for whatever is a code bug — write it, then state `dotnet build RIMA.Runtime.csproj` result.
3. **Placeholder asset sizes for PixelLab** — the user will regenerate the map-fragment + gate/door art in PixelLab later, so any placeholder we add must be a PixelLab-generatable canvas size: **map-fragment pickup = 64x64 px object, gate/door = 64x96 (or 64x64) object, PPU 64**. Note where the placeholder sprite is assigned so it can be swapped.
4. A 3-line manual repro/verify (e.g. JumpToRoom(0) → kill mobs → expect fragment at center → pickup → gate opens).

Keep it surgical. Report code-vs-scene split clearly so the orchestrator can do the Unity wiring.
