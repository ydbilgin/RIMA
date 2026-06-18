# DONE — In-game log overlay + gameplay instrumentation + gate-fix (2026-06-18)

## A) DebugLogOverlay (NEW)
- File: `Assets/Scripts/Debug/DebugLogOverlay.cs` (~140 lines)
- Toggle key: **F3** (verified unbound; F1=DemoDebugPanel, F2=BuildMode, F10-F12=ScreenshotMode).
- Mirrors DemoDebugPanel: same `#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` guard,
  `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` bootstrap + DontDestroyOnLoad + ScreenshotMode.Register/Unregister.
- Subscribes `Application.logMessageReceived` in OnEnable, unsubscribes in OnDisable.
- Ring buffer (Queue) capped at 60. OnGUI top-right semi-transparent scrollable box.
- Colors: Error/Exception/Assert = red, Warning = yellow, Log = white. Error/Exception append first 4 stack lines.
- No deps beyond UnityEngine + InputSystem + ScreenshotMode.

## B) Instrumentation (consistent tags)
- **[Cast]**: `Assets/Scripts/Skills/Base/SkillBase.cs:85` (in `TryActivate`, after `Execute()`/`NotifySkillUsed`) — `skillName` + caster name.
- **[Damage]**: `Assets/Scripts/Skills/SkillRuntime.cs:195` (DamagePacket terminal `DealDamage`, after `health.TakeDamage`) AND `:148` (`DealDamageRaw`, projectile path). Both DISCRETE per-hit (not per-frame), and GATED to player-sourced only (`attacker.CompareTag("Player")`) — avoids enemy/DoT spam. Logs `finalDamage -> target (element)`.
- **[Grant]**: `Assets/Scripts/Skills/DraftManager.cs:748` (in `AssignActive`, on successful bind) — `skill -> slot N`; passive at `:519` (in `HandlePassivePick`) — `passive -> passive Lv N`.
- [Reward] pickup log left untouched.

## C) Gate-fix (auditor P1)
- File: `Assets/Scripts/UI/CharacterSelectController.cs:156-160`
- Routed the shared `IsUnlocked(ClassType)` helper through `ClassUnlockPolicy.IsDemoPlayable` (AND), mirroring `CharacterSelectScreen.OnStartRun`. All listed gate sites (grid 71/77, selected 89/94, confirm 121) flow through this helper, so one surgical/reversible change hardens them all. Now references `IsDemoPlayable`.

## VERIFY
- refresh_unity (force, compile) → editor_state `isCompiling=False`.
- read_console (errors): **0 errors**.
- Data-proof (reflection): DebugLogOverlay type exists, `HandleLog` (logMessageReceived subscriber) + `Bootstrap` present; SkillRuntime present; all 3 instrumented sites + gate-fix compiled clean.

## BLOCKED / deviations
- None. Damage-logging NOT skipped — it was made safe via the player-attacker gate (the only meaningful-application path; the two terminal damage overloads are per-hit, not per-frame). No git commit (per instruction).
