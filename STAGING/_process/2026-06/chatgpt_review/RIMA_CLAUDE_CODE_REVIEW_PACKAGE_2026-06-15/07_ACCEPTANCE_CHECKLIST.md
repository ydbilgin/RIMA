# Acceptance Checklist

Claude should not mark the task complete until every applicable item is checked.

## Time and modal state

- [ ] Exactly one authority determines effective `Time.timeScale`.
- [ ] Director exit cannot resume behind a draft.
- [ ] Director exit cannot resume behind pause/settings/codex.
- [ ] Director exit cannot resume during death.
- [ ] TAB slow motion remains correct.
- [ ] Scene load clears stale transient state.
- [ ] F12 recovery uses the same state APIs.

## Director and Build Mode

- [ ] Normal menu-to-game flow creates one usable Director instance.
- [ ] Direct Arena entry creates one usable Director instance.
- [ ] Build Mode cannot enter during active/pending draft.
- [ ] Build Mode cannot enter during blocking modal UI.
- [ ] Rejected entry changes no camera/UI/player state.
- [ ] Camera rig always restores after exit.
- [ ] Rapid toggling causes no ortho drift.
- [ ] Working template is destroyed exactly once.
- [ ] Hidden canvases are restored exactly once.

## Draft lifecycle

- [ ] One active draft maximum.
- [ ] One pending draft maximum.
- [ ] Pending state covers all delayed draft sources.
- [ ] Room-clear duplicate events collapse.
- [ ] Immediate reward/portal request cannot be overwritten by old delay.
- [ ] Pending draft is cancelled on disable/reset.
- [ ] Secondary-class subscription is named and symmetric.
- [ ] Recreating DraftManager does not accumulate callbacks.
- [ ] Timeout leaves no active draft.
- [ ] Timeout closes SkillOffer UI.
- [ ] Timeout restores the correct effective time scale.
- [ ] Doors open only after modal cleanup.

## Run lifecycle

- [ ] Opening draft coroutine is retained or generation-guarded.
- [ ] Same-scene restart cannot produce stale opening draft.
- [ ] Disabling the director cancels stale run coroutines.

## Build authoring input

- [ ] Search field focus suppresses tool hotkeys.
- [ ] Ctrl+Z in text input does not undo room content.
- [ ] Tool hotkeys work immediately after focus leaves input.

## Damage

- [ ] Power baseline of 100 remains multiplier 1.
- [ ] Defense curve remains unchanged.
- [ ] Posture overflow remains separate from health damage.
- [ ] Minimum-one-damage behavior is documented by test or intentionally changed.

## Final evidence

- [ ] Current file:line references reported.
- [ ] All confirmed findings have tests.
- [ ] All rejected findings include source evidence.
- [ ] Unity compiles without new warnings attributable to the patch.
- [ ] EditMode tests pass.
- [ ] PlayMode tests pass.
- [ ] Manual live-demo sequence completed.
- [ ] Remaining risks are explicitly listed.
