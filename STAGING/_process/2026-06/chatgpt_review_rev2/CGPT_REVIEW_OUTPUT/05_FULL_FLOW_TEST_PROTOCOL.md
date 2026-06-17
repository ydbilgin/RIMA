# 05 — Full-Flow Test Protocol

## Start condition

- Stop Play Mode.
- Clear Console.
- Start through the same entry route used in the presentation.
- Do not open `_Arena` directly as the only test.

## Runtime identity check

Log/inspect the canonical player:

- object name
- tag
- layer
- active state
- `PlayerController`
- `Health`
- `PlayerAttack`
- current HP

Pass only if the attackable collider/root used by enemy overlaps is tagged `Player`.

## Combat chain

1. Enter `_Arena`.
2. Spawn or start the real opening encounter.
3. Enemy at 9–12+ units must acquire the player.
4. Observe `Idle → Chase → Attack`.
5. Player must take damage.
6. Player must damage and kill at least one enemy.
7. Kill/progress counter must change.
8. Clear the wave.
9. Reward draft must open once.
10. Select a reward.
11. Continue to next room/run-map.

## Seam tests

After the first successful combat:

- Open and close F2 Build Mode.
- Open and close Director Mode.
- Trigger a room/portal transition.
- Spawn another enemy.
- Verify attack tokens still work.
- Verify `Time.timeScale == 1` after closing overlays.
- Verify no leftover scrim/modal.
- Verify no duplicated EventSystem/input.

## Boss chain

1. Reach or spawn boss in the playable arena.
2. Boss resolves player.
3. Boss HP bar/name appears.
4. Phase 1 attacks.
5. Cross 50% threshold and observe transition.
6. Cross 33% threshold and observe low-phase behavior.
7. Validate telegraph timing against damage.
8. Kill boss.
9. Verify expected reward/class/run continuation.

## Repetition

Run the complete cold path **three times**.

### Pass criteria

- 3/3 runs complete
- 0 Console errors
- no permanent timeScale pause
- no idle enemies caused by missing player reference
- no dead AttackTokenManager after transitions
- no stale reward/death UI
- no soft-lock from Build Mode props
