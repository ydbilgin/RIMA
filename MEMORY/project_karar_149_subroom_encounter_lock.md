---
name: project-karar-149-subroom-encounter-lock
description: "S94 LOCK 2026-05-19: Karar #149 LIVE. Combat/Elite node = 1 EncounterTemplateSO (3-5 sub-room). Internal fade transitions, encounter-final reward only, 16x10 default. Codex APPROVE_WITH_REVISIONS, Opus tech spec in flight."
metadata:
  node_type: memory
  type: project
  originSessionId: b24b1f6b-b1d9-4d25-99ca-7dffbfd12c00
---

# Karar #149 — Combat Encounter Sub-Room Sequence LOCK (S94 2026-05-19)

**Supersedes:** [[project-subroom-encounter-system-proposal]] (proposal phase, now locked)

## What was decided

- Combat/Elite macro DungeonGraph node = one `EncounterTemplateSO` containing 3-5 ordered `RoomTemplateSO` sub-rooms.
- Sub-room transitions are encounter-internal via `RoomTransitionFX` fade-to-black. DungeonGraph does NOT advance between sub-rooms.
- No individual sub-room rewards (no skill draft, map fragment, Echo Imprint, or macro-route reward).
- Macro reward sequence fires only after final required sub-room is cleared.

## Key constraints

- **Default sub-room size:** 16x10. 12x8 allowed only for connectors, ambush pockets, or low-threat non-combat transitions.
- **Transition:** Fade-to-black (RoomTransitionFX). Door+camera-pan rejected (complexity). Seamless rejected (recreates large-room composition problem).
- **Enemy/threat:** Identity and budget stay in EncounterBank layer. RoomTemplateSO holds sockets/tags/masks/props/visual only. Spawn model = hybrid (pre-authored socket + enter-trigger spawn).
- **Save/load:** No mid-encounter save in MVP. Encounter restarts from sub-room 0 on reload. Mid-save is Phase 2 scope.

## Review

- **Codex verdict:** APPROVE_WITH_REVISIONS (`STAGING/CODEX_DONE_subroom_encounter_review.md`). Revisions addressed: 16x10 default confirmed, fade-to-black selected, hybrid spawn chosen, encounter-final reward confirmed.
- **Opus tech spec:** In flight as `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md`. Codex implementation gated on spec completion.

## Vertical slice target

2-sub-room Combat node, fade transition, sub-room clear gate unlock, final reward only.

## Conflict checks (all preserved)

- Karar #27 Echo/Death Imprint: counting at macro encounter granularity, not sub-room
- Karar #80 Class Silhouette: 16x10 preserves 64px chibi readability
- Karar #143 6-layer Painter: preserved + `encounterAvoidRadius` addition resolves spawn-pocket vs edge-density clutter
- Karar #147 Multi-Layer Painter: BackgroundLayerData gives each sub-room independent visual composition

## What still needs building

- `EncounterTemplateSO` (new SO)
- `SubRoomSequenceController` / EncounterController
- Internal gate trigger path (not DoorTrigger -> DungeonGraph)
- Reward/map fragment gating guard ("is final sub-room")
- CameraFollow public SetBounds/RefreshBounds
- 5+ usable sub-room templates

## Related

- [[project-subroom-encounter-system-proposal]] (superseded — design rationale preserved there)
- [[project-multilayer-painter-v1-lock]] (Karar #147)
- [[project-tile-angle-verdict-branch-d-e-lock]] (Karar #148, RIMA canonical view)
- `TASARIM/MASTER_KARAR_BELGESI.md` Karar #149
