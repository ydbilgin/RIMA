# INVESTIGATE (read-only) — RIMA live demo paths, so consolidation is SAFE

ACTIVE RULES: (1) think before answering (2) DEFINITIVE answers with file:line + GUID evidence (3) no speculation — if you can't confirm, say UNCONFIRMED (4) do NOT edit any file.
Respond INLINE (captured to CODEX_DONE.md). This unblocks a careful "rewire-then-retire" consolidation by Opus.

## Why
We are consolidating competing systems into ONE spine (`Systems.Map.RoomLoader` confirmed live). Before deleting/
retiring duplicates, we must know EXACTLY which path the playable demo runs, or we'll break it. Answer definitively:

## Questions (each with file:line / prefab GUID evidence)
1. **Boss:** Two scripts (`Enemies/Boss/PenitentSovereign.cs` 684-line, `Enemies/BossAI_PenitentSovereign.cs` 297-line)
   AND two prefabs (`Prefabs/Enemies/Boss/PenitentSovereign.prefab`, `Prefabs/Enemies/Boss/BossAI_PenitentSovereign.prefab`).
   - Which prefab/script does the **5-room demo boss room actually spawn**? Trace it: RoomSequenceData / Phase1 boss
     room SO / EncounterWaveSO / RuntimeRoomManager spawn → which boss prefab GUID. Match GUID to the .prefab + its
     MonoBehaviour script GUID to the .cs.
   - Which boss script writes `Time.timeScale`/uses `VFX/ScreenShake`? (`PenitentSovereign.cs` references ScreenShake.)
   - The phase-transition threshold: where is 0.33f vs the canon 0.5f, in WHICH script.
2. **MapFragment:** `Systems/Map/RoomLoader.cs` — which MapFragment class does it actually reference/listen to
   (`RIMA.Environment.MapFragment` vs `RIMA.MapFragment` / `Core/MapFragment.cs`)? And which does `MapFragmentSpawner`
   instantiate? Give the field + line.
3. **RuntimeRoomManager:** Is `RuntimeRoomManager` a component in the LIVE demo scene (`PlayableArena_Test01.unity`
   and/or `Act1_ShatteredKeep.unity`)? Or is `Systems.Map.RoomLoader` the only active room driver? If RRM is NOT in
   the live scene, what still references it in code (so we know what breaks if retired)?
4. **Boss-death → DemoComplete chain:** the EXACT wired call path from boss death to `DemoCompleteOverlay.Show()` in
   the live setup (which boss script → which manager → `RoomLoader.RaiseDemoComplete`/`OnDemoComplete`). Is it the RRM
   route (`BossAI...:264 → RuntimeRoomManager.NotifyBossDefeated`) or a direct RoomLoader route?
5. **Camera:** which `CameraFollow` (`Camera/CameraFollow.cs` vs `Player/CameraFollow.cs`) is on the live scene camera?

Output a tight definitive table: System | LIVE one (keep) | dead/duplicate (retire) | evidence (file:line/GUID) |
what-references-the-dead-one. UNCONFIRMED where you truly can't tell. This is the safety map for the merge.
