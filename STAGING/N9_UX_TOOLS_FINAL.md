# N9 — Dev UX/Tool Ideation + Build Plan FINAL (Opus 4.8, agy ideated)

**Amaç:** demo/dev iş akışını hızlandıran tool'lar. agy ideation (ydbilgin) + Opus önceliklendirme. Build = Codex (writer) + Opus (review) → multi-AI tam. Task: `STAGING/N9_UX_TOOLS.task.md`.

## agy 7 fikir (özet)
1. Play-From-Here Room Jump (XS) · 2. Debug Cheats F1 panel (S) · 3. One-Click Sandbox Launcher F5 (XS) · 4. Live-Reload Diagnostics HUD (S) · 5. Mob-Spawn Visualizer gizmos (S) · 6. Brush Preset Manager (S) · 7. Room Snapshot/Diff (M — agy İPTAL, düşük ROI git zaten var).

## Opus önceliklendirme (değer/efor, animasyon-bağımsız, demo-test'i hızlandırır)
**GECE BUILD (Codex writer → Opus review):**
1. **Play-From-Here / LoadRoomByIndex** — `RoomLoader.LoadRoomByIndex(int)` (mevcut SwapRoomWhileBlack/Teardown reuse) + küçük editor/runtime GUI. Demo'yu baştan oynamadan oda-test. **ND2'deki menü-boot derdini de bypass eder.** Değer XXL.
2. **Debug Cheats F1 panel** — yeni MonoBehaviour, OnGUI, `#if DEVELOPMENT_BUILD||UNITY_EDITOR`: Kill All Mobs / God Mode / Speed 2x / Force Room Clear / Restart Room. Combat+transition test hızı. Bağımsız, sıfır-risk.
3. **One-Click Sandbox Launcher** — editor helper, F5 shortcut: save + open PlayableArena_Test01 + EnterPlaymode.

**DEFER (değerli ama sonra):** 4 Live-Reload HUD (live-editor olgunlaşınca), 5 Mob-Spawn gizmo (encounter tasarımında), 6 Brush preset (painter QoL).

**İPTAL (agy+Opus):** 7 Snapshot/Diff (git zaten yapıyor), aşırı node-editor encounter tool (over-engineered).

## Build spec (Codex'e — N10 task)
- **Dosya 1:** `RoomLoader.cs` += `public void LoadRoomByIndex(int index)` — TeardownCurrentRoom + TeleportPlayer + BuildRoomContent + index/CurrentRoomData set + OnRoomChanged (LoadFirstRoom/SwapRoomWhileBlack pattern, transition FX opsiyonel). Range guard.
- **Dosya 2 (yeni):** `Assets/Scripts/Debug/DemoDebugPanel.cs` — `#if DEVELOPMENT_BUILD||UNITY_EDITOR`, OnGUI F1-toggle, butonlar: KillAll (FindObjectsByType<Health> tag=Enemy → TakeDamage 99999), GodMode (player Health invuln flag/sürekli heal), Speed (Time.timeScale toggle 1/2/0.25), ForceRoomClear (RoomLoader.OnRoomCleared fire — reflection veya public helper), RestartRoom (LoadRoomByIndex current).
- **Dosya 3 (yeni):** `Assets/Editor/RimaDevShortcuts.cs` — `[MenuItem]` + `[Shortcut("RIMA/Play Arena", F5)]`: save open scene + EditorSceneManager.OpenScene(PlayableArena_Test01) + EditorApplication.EnterPlaymode.

**Index:** `reference_dev_tools_n9` → top-3 build (Play-From-Here / Debug-Cheats-F1 / Sandbox-Launcher).
