# Council CX Precommit Findings - Bugra - 2026-06-18

VERDICT: APPROVE-WITH-FIXES

Scope: read-only review of the current uncommitted working tree. Commands run: git status, git diff/stat/check, graphify query, rg/static source reads, prior auditor/build report reads. No code files changed. `ANTIGRAVITY.md` was requested by the local routing rules but is absent at repo root.

## P1 - F3 [Damage] telemetry is still partial for demo skills

Evidence:
- `Assets/Scripts/Skills/SkillRuntime.cs:146` and `Assets/Scripts/Skills/SkillRuntime.cs:197` emit `[Damage]` only when the passed attacker/packet attacker is tagged `Player`.
- Several live demo skills still call overloads that pass no attacker: `Assets/Scripts/Skills/Elementalist/SolarFlare.cs:30`, `Assets/Scripts/Skills/Elementalist/PrismBeam.cs:30`, `Assets/Scripts/Skills/Elementalist/PrismBeam.cs:35`, `Assets/Scripts/Skills/Elementalist/FrostWall.cs:33`, and Warblade `Assets/Scripts/Skills/Warblade/IronCharge.cs:68` from the rg sweep. These hits still deal damage, but the new SkillRuntime log gate cannot identify them as player-sourced.
- Fireball is another high-visibility case: `Assets/Scripts/Skills/Elementalist/Fireball.cs:73` calls `PlayerProjectile.Init(...)` without `attacker`, while `PlayerProjectile.cs:98` falls back to the projectile GameObject. That object is not tagged `Player`, so Fireball raw projectile damage will not log `[Damage]` even though ArcaneBlast now correctly passes `attacker: player.gameObject` at `Assets/Scripts/Skills/Elementalist/ArcaneBlast.cs:63`.

Impact: gameplay damage is not broken, but the new F3/demo verification instrumentation is not reliable as a general "player damage happened" signal. If the acceptance bar is full demo telemetry, fix before commit by routing these calls through an overload with `attacker: player.gameObject` plus an explicit element, or by adding intentional manual logs for the known 2-arg bypasses. If partial telemetry is intentional, document the scope clearly in the batch report.

## P1 - Working tree contains commit-hygiene blockers outside the reviewed code batch

Evidence from `git status --porcelain=v1`:
- Untracked generated recovery scene: `Assets/_Recovery/0 (2).unity` and `.meta`.
- Modified binary/font/report artifacts outside the six requested review groups: `Assets/Fonts/Jersey10/Jersey10-Regular SDF.asset`, `STAGING/report/RIMA_Senior_Design_Report.docx`, `STAGING/report/RIMA_Senior_Design_Report.md`.
- Many untracked staging/report outputs also exist. Some are expected process outputs, but the recovery scene is especially risky to sweep into a pre-demo commit.

Impact: not a code correctness regression, but this batch is not commit-ready as a blind working-tree commit. Before commit, stage only the intended code/assets or split/clean the generated recovery/report artifacts. No git add/restore/delete was run per dispatch rules.

## Passed checks / no blocking code regression found

1. Director bypass: PASS. `PlayerClassManager.SetPrimaryClass` gates on `!DirectorBypassClassUnlock && !(IsUnlocked && IsDemoPlayable)` at `Assets/Scripts/Systems/PlayerClassManager.cs:56-57`. `DirectorMode` sets `DirectorBypassClassUnlock = true`, calls `SetPrimaryClass(type)`, then resets it in `finally` at `Assets/Scripts/UI/DirectorMode.cs:2113-2120`. The bypass is real.

2. Class-select gate integrity: PASS for exposed class-select paths. `ClassUnlockPolicy.IsDemoPlayable` is the shared Warblade/Elementalist gate at `Assets/Scripts/Systems/ClassUnlockPolicy.cs:14-17`. Chamber world and popup paths call `IsDemoSelectable`, now delegated to policy at `Assets/Scripts/UI/ChamberSelectBootstrap.cs:1972-1974`; popup/start flow rejects non-demo classes at `Assets/Scripts/UI/ChamberSelectBootstrap.cs:1691` and `:1712`; the classic TAB start path rejects before live run at `Assets/Scripts/UI/CharacterSelectScreen.cs:1234-1238`; the alternate controller helper ANDs unlock with demo-playable at `Assets/Scripts/UI/CharacterSelectController.cs:160-161`.

3. ArcaneBlast fallback: PASS. `ArcaneBlast.FireProjectile` no longer returns on null prefab and creates a runtime projectile at `Assets/Scripts/Skills/Elementalist/ArcaneBlast.cs:52-69`; the fallback adds Rigidbody2D, trigger CircleCollider2D, SpriteRenderer, and PlayerProjectile at `:71-88`; `PlayerProjectile.Init` signature supports the named attacker/element args and `SetOnHit` exists at `Assets/Scripts/Skills/PlayerProjectile.cs:32-66`. Null guards are adequate.

4. Passive draft grant path: PASS. `DraftManager.HandlePassivePick` applies/levels the passive, logs `[Grant]`, and calls `HUDController.Instance?.ShowToast` at `Assets/Scripts/Skills/DraftManager.cs:499-522`. `HUDController.ShowToast` creates and destroys a short-lived HUD object via `ToastRoutine` at `Assets/Scripts/UI/HUDController.cs:518-561`; null/empty and missing-HUD cases are safe. Active skill grant path remains additive logging only at `Assets/Scripts/Skills/DraftManager.cs:752`.

5. DebugLogOverlay lifecycle: PASS. It self-installs after scene load, subscribes in `OnEnable`, unsubscribes in `OnDisable`, and unregisters from ScreenshotMode in `OnDestroy` at `Assets/Scripts/Debug/DebugLogOverlay.cs:31-59`. This matches the existing debug surface pattern in `DemoDebugPanel.cs` and does not leak log subscribers.

6. Elementalist 8-direction clips: PASS statically. All eight changed animation clips now point to real Elementalist sprite GUIDs with `fileID: 21300000`; rg confirmed each GUID resolves to an existing `.png.meta` under `Assets/Resources/Characters/Elementalist`. This matches the builder report.

7. Diff hygiene: `git diff --check` returned clean. Prior builder/auditor reports claim Unity compile/read_console clean after the relevant batches; I did not rerun Unity compile from this shell-only review.

## Final commit-before-fix list

- Fix or explicitly scope the partial `[Damage]` telemetry coverage for demo skills that still use attackerless damage paths.
- Do not commit the whole dirty tree blindly; remove/split/stage around `Assets/_Recovery/0 (2).unity`, the modified font SDF, and report artifacts unless the orchestrator explicitly wants them in this commit.
