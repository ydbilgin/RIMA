# Regression Test Matrix

Use existing test conventions in the repository. Prefer focused tests over a giant end-to-end test that fails with the emotional clarity of a smoke alarm.

## A. Time-scale tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| TS-01 | PlayMode | No overlay, Director Test | Enter Director | `timeScale == 0` |
| TS-02 | PlayMode | Director active | Exit Director | `timeScale == 1` |
| TS-03 | PlayMode | Skill offer active | Enter/exit Director | `timeScale == 0`, offer remains active |
| TS-04 | PlayMode | Pause active | Enter/exit Director | `timeScale == 0` |
| TS-05 | PlayMode | TAB open | Enter/exit Director | returns to intended TAB scale |
| TS-06 | PlayMode | Death active | Exit Director | `timeScale == 0` |
| TS-07 | PlayMode | Scene reload | Reset state | no stale pause reason |
| TS-08 | PlayMode | F12 panic | Trigger panic | recoverable reasons cleared, scale correct |

## B. Director bootstrap tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| DB-01 | PlayMode | Initial MainMenu | Load CharacterSelect then Arena | one Director instance |
| DB-02 | PlayMode | Initial Arena | Wait one frame | one Director instance |
| DB-03 | PlayMode | Domain reload disabled simulation | Re-enter | no stale duplicate |
| DB-04 | PlayMode | Menu scene | Inspect overlay | not shown |

## C. Build Mode tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| BM-01 | PlayMode | Draft active | Press F2 | Build Mode inactive |
| BM-02 | PlayMode | Draft pending | Press F2 | Build Mode inactive |
| BM-03 | PlayMode | Pause active | Press F2 | Build Mode inactive |
| BM-04 | PlayMode | No modal | Press F2 | Build Mode active |
| BM-05 | PlayMode | Active Build Mode | Exit | camera rig restored |
| BM-06 | PlayMode | Exit transition | Spam F2 | no camera drift |
| BM-07 | PlayMode | Ten enter/exit cycles | Complete cycles | original ortho size restored |
| BM-08 | PlayMode | Search input focused | Type F/E/[ ] | no tool action |
| BM-09 | PlayMode | Search input focused | Ctrl+Z | text behavior only |
| BM-10 | PlayMode | Search input unfocused | Hotkeys | tools work |

## D. Draft scheduling tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| DR-01 | PlayMode | Room clear | Fire event twice | one pending request |
| DR-02 | PlayMode | Room-clear delay active | Open reward draft | delayed request cancelled/superseded |
| DR-03 | PlayMode | Room-clear delay active | Portal trigger | one active draft |
| DR-04 | PlayMode | Secondary unlock | Observe delay | pending flag true |
| DR-05 | PlayMode | Disable DraftManager during delay | Wait | no draft |
| DR-06 | PlayMode | Re-enable manager repeatedly | Select secondary class | one callback |
| DR-07 | PlayMode | Manager recreated | Select secondary class | one callback |
| DR-08 | PlayMode | Active draft | Try another unsafe ShowDraft | rejected or queued by policy |
| DR-09 | PlayMode | Hide draft | Inspect state | active false, UI closed |
| DR-10 | PlayMode | Cancel pending | Wait beyond delay | no draft |

## E. Reward timeout tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| RW-01 | PlayMode | Reward collected | Resolve manually | doors open afterward |
| RW-02 | PlayMode | Reward collected | Reach timeout | draft resolved/closed |
| RW-03 | PlayMode | Timeout | Inspect UIManager | skill offer false |
| RW-04 | PlayMode | Timeout | Inspect DraftManager | active false |
| RW-05 | PlayMode | Timeout with death/modal | Complete | time scale matches remaining state |
| RW-06 | PlayMode | ForceCollect twice | Invoke | one reward, one draft |

## F. Run restart tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| RR-01 | PlayMode | BeginRun | BeginRun same frame | one opening draft |
| RR-02 | PlayMode | Opening wait active | Restart | old routine aborted |
| RR-03 | PlayMode | Opening draft active | Restart | no old auto-pick |
| RR-04 | PlayMode | Director disabled | Wait | no stale draft |

## G. Damage contract tests

| ID | Type | Setup | Action | Expected |
|---|---|---|---|---|
| DM-01 | EditMode | Physical power 100 | Calculate | multiplier 1 |
| DM-02 | EditMode | Ability power 150 | Calculate | multiplier 1.5 |
| DM-03 | EditMode | Armor 100 | Calculate | defense multiplier 0.5 |
| DM-04 | EditMode | True damage | Calculate | ignores defense |
| DM-05 | EditMode | Positive base, zero debug mult | Calculate | expected value documented |
| DM-06 | EditMode | Identity above cap | Calculate | health damage capped, overflow separate |

## Manual live-demo script

Run this exact sequence in a build or Editor Play Mode:

1. MainMenu → CharacterSelect → Arena.
2. Press F2, enter and exit Build Mode.
3. Open pause, press F2, confirm Build Mode is rejected.
4. Open draft, press F2, confirm Build Mode is rejected.
5. Resolve draft.
6. Trigger secondary-class selection and clear room during its delay.
7. Confirm only one draft.
8. Collect reward and deliberately wait for configured timeout using a temporarily reduced test value.
9. Confirm UI closes/resolves and exits open.
10. Restart run while opening draft is pending.
11. Confirm one opening draft.
12. Enter Build Mode and rapidly press F2 repeatedly.
13. Confirm camera and controls restore.
14. Trigger death screen, enter/exit Director path if allowed, confirm game remains paused.
15. Press F12 and confirm recovery path.

Record logs for every state transition.
