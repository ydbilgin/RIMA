# Executive Summary

## Main conclusion

The highest demo risk is **state ownership**, not damage mathematics.

At least three systems can affect pause/resume behavior:

- `UIManager`
- `DirectorMode`
- Build Mode through `DirectorMode`

Draft UI, pause UI, death state, Build Mode, and Director Mode can overlap. Several paths directly assign `Time.timeScale`, so one system can resume the game while another still believes it owns a modal pause.

The second major risk is **draft concurrency**:

- room-clear draft delay is not tracked as pending,
- secondary-class draft uses an anonymous event subscription,
- reward timeout can open exits while a draft remains active,
- run restart may leave an old opening-draft coroutine alive.

## Risk counts

| Category | Count | Meaning |
|---|---:|---|
| Live-demo blockers | 7 | Can cause softlock, hidden modal state, duplicate draft, unavailable Build Mode, or incorrect resume |
| Cosmetic / workflow defects | 2 | Can corrupt authoring input or cause camera drift |
| Suspected balance edge | 1 | Needs contract confirmation before changing |
| Confirmed non-issues | 2 | Should not be “fixed” without new requirements |

## Highest-priority sequence

1. Establish one authoritative time-scale decision.
2. Make Director Mode available after normal menu flow.
3. Block or safely coordinate Build Mode with modal UI.
4. Serialize all draft scheduling.
5. Replace anonymous secondary-class listener with symmetric lifecycle subscription.
6. Define deterministic reward-draft timeout behavior.
7. Cancel stale opening-draft sequences on restart.
8. Add regression tests.
9. Handle Build Mode text-input focus and camera transition edge cases.

## Demo acceptance target

The following interaction matrix must work without softlock:

- Draft open → F2
- Pause open → F2
- Build Mode open → close Build Mode
- Build Mode open → rapid F2 toggle
- Secondary class selected → room clears during two-second delay
- Reward draft ignored for timeout duration
- Restart run while opening draft is pending
- MainMenu → CharacterSelect → `_Arena` → F2
- Direct `_Arena` launch → F2
- Death screen active → Director/Build exit
