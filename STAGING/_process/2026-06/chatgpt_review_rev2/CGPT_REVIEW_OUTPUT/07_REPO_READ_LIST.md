# 07 — Repository Read List

Read these first:

## Primary review context

- `STAGING/_process/2026-06/chatgpt_review_rev2/CHATGPT_FULL_REVIEW_PACK_2026-06-17.md`
- `STAGING/DEMO_BITIRME_DECISION_2026-06-17.md`
- `STAGING/ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`

## Combat target/detection

- `Assets/Scripts/Enemies/BaseMobBehavior.cs`
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Core/Health.cs`
- `ProjectSettings/TagManager.asset`
- `Assets/Scenes/_Arena.unity`
- Player base prefab and runtime character prefab chain
- Any CharacterSelect/RuntimeRoomManager code that instantiates or swaps the player

## Attack/token/lethality

- `Assets/Scripts/Combat/AttackTokenManager.cs`
- `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs`
- `Assets/Resources/Encounters/Act1_Wave_Pilot.asset`
- encounter spawner/selection code that interprets threat/opening budget

## Boss

- `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`
- `Assets/Scripts/Enemies/EnemyTelegraph.cs`
- Boss prefab and BossHealthBar wiring
- Boss intro/room-spawn code

## Flow and seams

- room transition/portal code
- reward draft trigger and teardown
- run-map overlay
- death screen teardown
- Build Mode F2 enter/exit and persistence
- Director Mode open/close and timeScale behavior
- CombatJuice prefab and `_Arena` scene reference

## Screenshot sources

- `STAGING/_process/2026-06/demo_screenshots/capture_v3/`
- Included corrected audit in this ZIP
