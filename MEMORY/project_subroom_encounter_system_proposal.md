---
name: project-subroom-encounter-system-proposal
description: "S94 PROPOSAL Sub-room encounter system. Combat Encounter = 4-5 connected small sub-room sequence (12x8 each), fade-to-black transition. Dead Cells biome model + RIMA combat. Karar"
metadata: 
  node_type: memory
  type: project
  status: superseded
  superseded_by: project-karar-149-subroom-encounter-lock
  originSessionId: b24b1f6b-b1d9-4d25-99ca-7dffbfd12c00
---

> SUPERSEDED 2026-05-19 — see [[project-karar-149-subroom-encounter-lock]]

# Sub-Room Encounter System Proposal (S94 2026-05-19)

**Karar status:** PROPOSAL — Codex review bekliyor (dispatch S94 sabah). User confirmation sonrası Karar #149.

## Idea (user önerdi)

"Combat Room" şu anki spec'te tek büyük arena. **User önerisi:** Bir Combat Encounter = **4-5 connected sub-room sequence**. Sub-room arası fade-to-black + camera swap. Reward sub-room'da değil, encounter sonunda.

## Industry karşılığı

- **Dead Cells biome model** — multi-room sequence, seamless door transitions
- **Hollow Knight area transitions** — door + camera pan
- **Hades waves in single arena** — sadece tek room ama wave-paced (RIMA hibrit edebilir)

## Why this works for RIMA

### 1. Mevcut mimari tam destekliyor (sıfır rewrite)
- `RoomTemplateSO` LIVE
- `Multi-Layer Painter` (Karar #147) — her template ayrı L4/L5 set
- `RoomBank` runtime LIVE
- `DungeonGraph + RuntimeRoomManager` LIVE
- `DoorSocket + GateSocket + DoorTrigger` LIVE

**Gereken:** `EncounterTemplateSO` (yeni) + camera fade shader + sub-room sequence logic.

### 2. Production sorununu çözüyor

Tek büyük room compose etmek zor — hep mantıksız görünüyor. 12x8 küçük sub-room ile:
- Her sub-room **tek tema** (moss-heavy / debris-heavy / rift-heavy / corridor / open)
- Compose etmek kolay (<50 sprite/sub-room)
- 5 sub-room kombinasyonu **infinite varyasyon**

### 3. Combat pacing daha rich

Yeni rhythm:
- Sub-room 1: 2-3 mob (giriş)
- Sub-room 2: 4 mob + crack ambient
- Sub-room 3: corridor (no mob, narrative breath)
- Sub-room 4: 5 mob (combat peak)
- Sub-room 5: elite + reward

Hades'in "tek arena boring after wave 3" sorununu çözer.

## RIMA-specific integration

- **Death Imprint Cascade** (top epic mech): hangi sub-room'da öldün → sonraki run o sub-room cursed
- **Family Tag** spawn: sub-room 2 = Bleed-heavy, sub-room 4 = Veil-heavy
- **Cross-class Echo** trigger: sub-room geçişinde tetiklenir

## Implementation cost estimate

| İş | Süre |
|---|---|
| Encounter system design spec (1-page) | 30 dk |
| Camera fade shader | 2-3 saat |
| EncounterTemplateSO + sequence logic | 1 gün |
| Sub-room template prototype (5 thematic preset) | 1 gün manual |
| Mob distribution + reward placement | 1 gün |
| Playtest validation | 1 gün |
| **Toplam MVP** | **~1 hafta** |

## Open questions (user + Codex review needed)

1. Sub-room boyutu? 12x8 (intimate) vs 16x10 (mevcut Spawn boyu)
2. Geçiş tipi? Fade-to-black (Dead Cells, hızlı) vs Door anim + camera pan (Hollow Knight) vs No transition seamless (Hades, kompleks)
3. Mob spawn: önceden distribute vs sub-room enter trigger vs hibrit
4. Reward: encounter sonunda sadece mi, yoksa elite sub-room'da extra var mı

## Conflicts check (Karar reference)

- Karar #143 6-layer pipeline — uyumlu, her sub-room L1-L6 use eder
- Karar #147 Multi-Layer Painter — uyumlu, her sub-room template ayrı paint
- Karar #80 character silhouette — uyumlu
- Karar #25 meta-progression — uyumlu, encounter sonunda reward gel
- Karar #27 Echo Imprint — **GÜÇLENDİRİR** (sub-room granularity = death-imprint placement precision)

## Decision queue

- **User:** Plan'ı kabul ettiğini söyledi, Codex'e review için yolladı
- **Codex:** Dispatch S94 sabah, verdict bekliyor
- **Lock candidate:** Karar #149 — "Combat Encounter = 4-5 sub-room sequence"

## Related

- [[project-combat-architecture]] — current combat v4
- [[project-multilayer-painter-v1-lock]] — Karar #147 visual composition
- [[project-echo-imprint-cascade-signature-candidate]] — Death Imprint integration
- [[reference-room-mechanics]] — Draft/Echo/Tag/Economy room rules
- `STAGING/RIMA_VISUAL_PRODUCTION_PLAN.md` — 5-phase production plan (revision pending)
- `STAGING/codex_task_subroom_encounter_review.md` (dispatch coming)
