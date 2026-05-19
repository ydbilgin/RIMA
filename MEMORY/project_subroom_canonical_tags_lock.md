---
name: project-subroom-canonical-tags-lock
description: "2026-05-19 S94 LATE LOCK — EncounterTemplateSO.sequence[i].subRoomTag 5 canonical string values. enum eklemek YASAK MVP. Mirror validator spec dahil."
metadata:
  node_type: memory
  type: project
  originSessionId: 9aecb83a-6b4d-4534-98af-97da4c678d26
---

# Sub-Room Canonical Tags + Mirror Validator LOCK (S94 LATE NIGHT 2026-05-19)

**Karar #150 LIVE → Karar #149 sub-room slot grammar** Codex APPROVE_WITH_REVISIONS verdict #4 ile lock.

## Canonical `subRoomTag` strings (5 değer)

`EncounterTemplateSO.sequence[i].subRoomTag` field **string** (mevcut, değişmez). Authoring time'da bu 5 değerden biri kullanılır:

| Tag | Slot purpose | Layout cue |
|---|---|---|
| `entry_chamber` | Player spawn, low threat or 1st wave | Wide opening, single archway exit |
| `pillar_arena` | Combat zone, 2-3 internal pillars for cover | Open center, perimeter pillars |
| `collapse_corridor` | Connector, ambush pocket, narrow chokepoint | Partial wall stubs, debris piles |
| `ritual_hall` | Boss sub-room, hero rift accent | Symmetric layout, hero L6 rift in center |
| `crypt_cell` | Optional reward / mystery, small 12×8 | Single chest socket |

**HARD RULES:**
- Enum eklemek YASAK MVP scope'unda — string lock yeterli (Codex revision #4)
- `RoomTemplateSO.roomType` macro taxonomy (Combat/Elite/Boss/etc.) DEĞİŞMEZ — slot grammar ONLY EncounterTemplateSO scope'unda
- Production authoring friction çıkarsa Phase 2'de `SubRoomSlotType` enum eklenebilir

## Archway mirror validator spec (Codex revision #3)

`Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs` mevcut. Eksik check'ler (one code edit):

| Rule | Severity | Logic |
|---|---|---|
| Inverse direction | Error | `from.DoorDirection` inverse `to.DoorDirection` olmalı (N↔S, E↔W) |
| Compatible edge | Error | Iki socket compatible edge'lerde (top↔bottom, left↔right) |
| Width match | Warning | `widthInTiles` ±1 tolerance |
| Mirrored placement | Warning | Sub-room N exit X-pos ≈ sub-room N+1 entry X-pos, ±2 cell tolerance |

User direktifi: "sağ alttaki kapıdan geçince mapın 2. bölümü açılacak, tutarlı bağlantılı" → bu kural set authoring time'da bunu enforce eder.

## Combat encounter pattern örnekleri (data-driven)

Hard-code YASAK. `EncounterTemplateSO.sequence` array'i data-driven, authoring level designer tasarlar:

```
Combat encounter (4 sub-room):
  [0] entry_chamber   (no enemies, breath)
  [1] pillar_arena    (wave 1: 3 imps)
  [2] collapse_corridor (wave 2: elite + 2 imps ambush)
  [3] pillar_arena    (wave 3: boss-imp + 2 swarm, REWARD here)

Elite encounter (single intense):
  [0] entry_chamber
  [1] ritual_hall    (boss + summon adds, REWARD)

Boss encounter (3-step ritual):
  [0] entry_chamber
  [1] collapse_corridor (boss key fight)
  [2] ritual_hall    (boss phase 2, REWARD)
```

Per-Act variation:
- Act 1: Same pattern with cool granite walls
- Act 2: Same pattern with bone-wrapped pillars
- Act 3: Same pattern with void-stone + gold sigils

## Source

- Codex review: `STAGING/CODEX_DONE_karar_150_review.md` (revision #3, #4)
- Full spec: `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md` §1.2.5 + §1.2.6

## Related

- [[project-karar-150-fake-isometric-lock]] LIVE
- [[project-karar-149-subroom-encounter-lock]] LIVE
- [[project-roadmap-dungeon-buildup-lock]] 6-faz
