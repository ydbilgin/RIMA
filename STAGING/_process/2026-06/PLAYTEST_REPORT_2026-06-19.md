# RIMA Playtest Report — 2026-06-19

**Agent:** Single Unity-driving playtest agent (no code/git changes)
**Unity version:** Editor via MCP (UnityMCP)
**Scene start:** _Arena (editor) → Play Mode launches MainMenu per playModeStartScene

---

## P0 — Compile Gate

**VERDICT: PASS**

- `refresh_unity(scope=scripts, compile=request, wait_for_ready=true)` → success
- Console errors: 0 | Warnings: 0
- Editor ready for tools after compile

---

## P1 — T7: 2nd Reward Draft Pause Verification (CRITICAL)

**VERDICT: PASS**

### Run Path
| Step | Frame | Scene | State |
|------|-------|-------|-------|
| Play Mode start | — | MainMenu | — |
| Button_Basla.onClick.Invoke() [SHORTCUT] | 1178 | → CharacterSelect | — |
| Warblade Hit.onClick + StartButton.onClick [SHORTCUT] | 2465 | → _Arena | — |
| _Arena loaded | 2627 | _Arena | — |
| Opening draft | ~3100 | _Arena | timeScale=0, IsDraftActive=True, IsSkillOfferOpen=True |
| Player SetImmune(true) applied | — | — | Player protected |
| Iron Charge selected [SHORTCUT] | 3672 | — | IsDraftActive=False, timeScale=1 |
| Kill wave 1 (2× FractureImp) TakeDamage(9999) [SHORTCUT] | 4587 | — | — |
| Kill wave 2 (1× FractureImp) [SHORTCUT] | 5093 | — | — |
| Room 1 cleared | 5515 | — | LifecycleState=Cleared, timeScale=0.3 (slow-mo) |
| Slow-mo ended | 5892 | — | timeScale=1, RewardPickup spawned |
| RewardPickup 1 ForceCollect() [SHORTCUT] | 6338 | — | — |
| Reward 1 draft opened | 6338+ | — | timeScale=0, IsDraftActive=True, IsSkillOfferOpen=True, LifecycleState=Cleared |
| DEEP WOUND selected | — | — | Draft closed, timeScale=1 |
| LifecycleState=DoorOpen | — | — | TryEnterDoor(0) called |
| Room 2 entered (NodeId=2, Combat) | 8518 | — | 1× HalfThrall + 1× FractureImp |
| Player immunity re-applied | — | — | — |
| Kill wave 1 room 2 [SHORTCUT] | — | — | — |
| Kill wave 2 room 2 (1× HalfThrall) [SHORTCUT] | 8661 | — | — |
| Room 2 cleared | ~9210 | — | LifecycleState=Cleared, timeScale=0.3 |
| Slow-mo ended | 9595 | — | timeScale=1 |
| RewardPickup 2 spawned | 10258 | — | WasCollected=False |
| RewardPickup 2 ForceCollect() [SHORTCUT] | 10924 | — | — |
| **2nd reward draft opened — T7 CAPTURE** | **10924** | **_Arena** | **SEE BELOW** |

### T7 CRITICAL CAPTURE — 2nd Reward Draft (Frame 10924)

```
timeScale:              0          ← PAUSED (CORRECT, NOT FREEZE)
DraftManager.Instance:  OK
IsDraftActive:          True
IsDraftPending:         False
UIManager.IsSkillOfferOpen: True
UIManager.IsAnyModalOpen:   True
LifecycleState:         Cleared
SkillOfferUI.activeInHierarchy: True
SkillDatabase count:    111
```

**T7 answer: 2nd reward draft PAUSES (timeScale=0). NOT a freeze bug. PASS.**

### Screenshots
- `Assets/Screenshots/Playtest_2026-06-19/reward1_draft_open.png` — reward 1 draft (3 skill cards visible)
- `Assets/Screenshots/Playtest_2026-06-19/T7_reward2_draft_PASS.png` — reward 2 draft (3 skill cards visible)

### Shortcuts Used
- `Button_Basla.onClick.Invoke()` — MainMenu start
- `Warblade Hit.onClick.Invoke()` + `StartButton.onClick.Invoke()` — class select
- `Health.SetImmune(true)` on Player — prevent player death during test
- `Health.TakeDamage(9999)` on each enemy — lethal damage (no physical movement)
- `RewardPickup.ForceCollect()` (×2) — collect reward without player walking onto it
- `RoomRunDirector.TryEnterDoor(0)` — enter door without player walking through trigger

---

## P2 — Death / Reset Singleton Integrity

**VERDICT: FAIL — DraftManager._shuttingDown singleton reset bug CONFIRMED**

### Sequence
1. Player killed via `Health.SetImmune(false)` + `TakeDamage(9999)` at Frame 13180
2. Death screen appeared: buttons "TEKRAR DENE [R]" + "ANA MENÜ" visible, timeScale=0
3. Pre-restart state: `DraftManager.Instance=OK`, `_shuttingDown=False`, `IsSkillOfferOpen=False` (no leak) — CLEAN
4. `RestartButton.onClick.Invoke()` → scene reloaded (_Arena)

### Post-restart capture (Frame 15665)

```
ActiveScene:               _Arena
timeScale:                 1
DraftManager.Instance:     NULL          ← BUG
_shuttingDown (static):    True          ← ROOT CAUSE
DraftManager in scene:     1             ← instance EXISTS but Instance prop returns null
SkillDatabase.Instance:    OK, count=111
IsSkillOfferOpen:          False         ← no draft UI leak
Stale RewardPickup:        0             ← clean
```

### Bug Analysis
- `DraftManager.OnDestroy()` sets static `_shuttingDown = true` when scene unloads on restart
- On scene reload, a new DraftManager MonoBehaviour IS instantiated (1 instance found), BUT
- The `Instance` property guard-checks `_shuttingDown` and returns null while it is True
- `_shuttingDown` is never reset to False in the new scene load — it persists as a static field across scene reloads within the same Play Mode session
- **Effect:** Opening draft never fires on restart; player gets stuck in room with no skills
- **Scope:** Only triggered by in-game "Restart" (scene reload within Play Mode). Fresh Play Mode always starts with `_shuttingDown=False`.

---

## P3 — Class Smoke Test

**VERDICT: Warblade=PASS | Elementalist=PASS | Ranger=BLOCKED (class locked) | Shadowblade=BLOCKED (class locked)**

### Warblade (tested in P1)
- Opening draft: timeScale=0, IsDraftActive=True, IsSkillOfferOpen=True — PASS
- Iron Charge selectable and picked — PASS
- DraftManager.Instance non-null — PASS
- _shuttingDown=False on fresh run — PASS

### Elementalist (Frame ~1731)
```
Scene:                  _Arena
Class:                  Elementalist
timeScale:              0
DraftManager.Instance:  OK
IsDraftActive:          True
IsDraftPending:         False
_shuttingDown:          False
IsSkillOfferOpen:       True
Offered skills:         GLACIAL SPIKE, CHAIN LIGHTNING, FIREBALL
```
- Opening draft present with Elementalist-specific skills — PASS
- GLACIAL SPIKE selected — PASS
- Required 2 pick rounds (opening kit multi-slot) — draft fully dismisses after 2nd pick
- DraftManager.Instance null? NO — PASS

### Ranger
- CharacterSelect StartButton shows "GELİŞTİRME AŞAMASINDA" (interactable=False) — class locked in demo build
- **BLOCKED: cannot test**

### Shadowblade
- Same: "GELİŞTİRME AŞAMASINDA" (interactable=False) — class locked
- **BLOCKED: cannot test**

Note: Only Warblade and Elementalist are currently unlocked in CharacterSelect.

---

## P4 — Golden-Path Bug Sweep

**VERDICT: 2 bugs found (P2 already covers #1), 1 observation**

### Bug #1 — DraftManager._shuttingDown restart (P2 above)
FAIL — confirmed in P2.

### Bug #2 — Player dies during room combat (original run 1)
In the first attempt at P1, the player (without immunity) died during room 2 combat (a live HalfThrall 2nd-wave hit the player). The death triggered:
- timeScale=0 (correct — death slow-mo, same as reward draft = could be confused)
- LifecycleState stayed at Combat (not Cleared)
- Draft did NOT open — correct (player dead, not reward collect)
This is NOT a new bug but confirms the SetImmune workaround was necessary for reliable test execution.

### Observation — TryEnterDoor picks node type by index
- `TryEnterDoor(0)` → landed on Merchant node (no combat). TryEnterDoor is a choice index, not a door direction.
- Had to call `TryEnterDoor(2)` to get a Combat node in the first run.
- In the successful run, `TryEnterDoor(0)` landed on a Combat node — node selection is graph-dependent per run seed.

### Console final state
- Total errors: 0
- Total warnings: 0
- No MCP tooling artifacts observed

---

## Summary

| Phase | Verdict | Key Evidence |
|-------|---------|-------------|
| P0 Compile | PASS | 0 errors/warnings |
| P1 T7 (2nd reward draft) | **PASS** | timeScale=0, IsDraftActive=True, IsSkillOfferOpen=True at Frame 10924 |
| P2 Death/reset | **FAIL** | DraftManager._shuttingDown=True after restart, Instance=NULL, opening draft never fires |
| P3 Warblade | PASS | Draft + skill select OK |
| P3 Elementalist | PASS | Draft + skill select OK (2-slot opening kit) |
| P3 Ranger | BLOCKED | Class locked (GELİŞTİRME AŞAMASINDA) |
| P3 Shadowblade | BLOCKED | Class locked (GELİŞTİRME AŞAMASINDA) |
| P4 Bug sweep | 0 new bugs | 2 already above |

---

## Fix Recommendation (for next session, not done here)

**DraftManager._shuttingDown reset bug (P2):**
In `DraftManager.Awake()` or the singleton initialization, reset `_shuttingDown = false` before the Instance assignment check. The flag should only block access during the brief window of `OnDestroy`, not persist across scene reloads.

---

No git, no asset/scene/prefab saved, working tree unchanged except runtime state.
