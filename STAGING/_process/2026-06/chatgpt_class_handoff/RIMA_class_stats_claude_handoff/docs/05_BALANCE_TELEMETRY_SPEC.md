# Balance Telemetry Spec v0.1

## Amaç

Dengeleme demosu sadece slider göstermekle kalmamalı. Aynı encounter seed'iyle class/preset karşılaştırması yapılabilmeli.

## Room summary JSON

```json
{
  "sessionId": "2026-06-12T00-15-00_RIMA_BALANCE",
  "roomSeed": "ACT1_TEST_0042",
  "classType": "Warblade",
  "statProfile": "Warblade_v01",
  "enemyPreset": "TripleThreat_8pt",
  "clearTimeSec": 36.2,
  "damageDealt": 4820,
  "damageTaken": 81,
  "healingDone": 24,
  "deathCount": 0,
  "overkillDamage": 310,
  "postureDamage": 126,
  "resourceGenerated": 230,
  "resourceSpent": 182,
  "resourceWasted": 48,
  "topDamageSource": "LMB",
  "damageBreakdown": {
    "LMB": 2600,
    "RMB": 350,
    "Skill": 1500,
    "DoT": 120,
    "Item": 250,
    "Minion": 0
  }
}
```

## Event log JSONL

Her event tek satır:

```json
{"t":1.24,"type":"DamageDealt","source":"LMB","damage":28,"damageType":"Physical","target":"ShardWalker"}
{"t":4.82,"type":"ResourceGenerated","resource":"Rage","amount":15,"reason":"LMBHit"}
{"t":8.12,"type":"SkillCast","skill":"IronCharge","resourceSpent":0}
{"t":36.2,"type":"RoomCleared","clearTimeSec":36.2}
```

## Minimum eventler

| Event | Alanlar |
|---|---|
| `DamageDealt` | time, attacker, target, damage, damageType, sourceType, sourceId, overkill, postureOverflow |
| `DamageTaken` | time, target, damage, sourceType |
| `HealDone` | time, target, amount, sourceType |
| `ResourceGenerated` | time, class, resource, amount, reason |
| `ResourceSpent` | time, class, resource, amount, skill |
| `ResourceWasted` | time, class, resource, amount, reason |
| `SkillCast` | time, skillId, class, cooldown, resourceCost |
| `EnemyKilled` | time, enemyId, enemyType, killerSource |
| `RoomStarted` | time, roomSeed, enemyPreset, class, statProfile |
| `RoomCleared` | time, clearTimeSec, roomSeed |

## CSV summary columns

```csv
sessionId,roomSeed,classType,statProfile,enemyPreset,clearTimeSec,damageDealt,damageTaken,healingDone,deathCount,overkillDamage,postureDamage,resourceGenerated,resourceSpent,resourceWasted,topDamageSource
```

## Unity implementation notu

- `BalanceTelemetry` singleton değilse daha temiz olur ama demo için singleton kabul.
- Eventleri önce memory'de tut, room clear'da summary üret.
- Export butonu:
  - `Application.persistentDataPath/RIMA_BalanceLogs/`
  - `room_summary.csv`
  - `events.jsonl`
- Clipboard snapshot için `GUIUtility.systemCopyBuffer` kullanılabilir.

## Kullanım senaryosu

1. Seed lock: `ACT1_TEST_0042`
2. Enemy preset: `TripleThreat_8pt`
3. Class: Warblade
4. Profile: Warblade_v01
5. Odayı temizle, log export et.
6. Aynı seed/preset ile Ranger_v01 test et.
7. `clearTime`, `damageTaken`, `resourceWasted`, `topDamageSource` karşılaştır.
