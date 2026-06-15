# Suggested Commit / PR Text

## Commit title

`fix: harden demo pause, build mode, and draft lifecycle`

## Commit body

- centralize effective time-scale resolution across UI, Director, Build Mode, and death state
- bootstrap Director Mode after normal menu-to-game flow without exposing it in menu scenes
- block Build Mode while modal or pending draft state is active
- serialize delayed draft requests and prevent callback replacement
- replace anonymous secondary-class listener with symmetric subscription lifecycle
- resolve or close reward drafts before timeout opens exits
- cancel stale opening-draft sequences on run restart
- suppress Build Mode tool hotkeys while text input is focused
- guard camera rig restoration during rapid Build Mode toggles
- add PlayMode/EditMode regression coverage for live-demo state combinations

## PR verification notes

The review intentionally leaves the minimum-one-damage rule unchanged unless the combat contract explicitly requires zero output. Physical/ability power scaling and posture overflow behavior remain unchanged.
