# C — Ek Dengeleme / Demo Tool Fikirleri

Planlı araçlar zaten iyi bir başlangıç:

- god-mode
- kill-all
- reset
- spawn
- stat slider'ları: HP / Phys / AP / attack speed / move speed
- debug HUD
- presenter mode
- slow-mo
- free-cam
- screenshot

Bunlara aşağıdakiler eklenmeli.

## Önerilen ek tool listesi

| Tool | Ne? | Unity'de nasıl? | Neden işe yarar? |
|---|---|---|---|
| DPS Meter | Son 5/10/30 sn hasar ortalaması | Hit event'lerini ring buffer'da tut | “Güçlü hissetti” yerine gerçek DPS görürsün |
| Damage Source Breakdown | Hasarı LMB/RMB/Skill/DoT/Minion/Item diye ayırır | `DamagePacket.sourceType` enum | Summoner/Hexer gibi sınıflarda hasar kaynağı anlaşılır |
| TTK Benchmark Room | Sabit dummy setleri: single target, 3 mob, 8 swarm | Test scene + preset spawn buttons | Class'ları aynı koşulda karşılaştırır |
| Hitbox/Hurtbox Overlay | Skill alanlarını ve enemy hurtbox'larını çiz | `OnDrawGizmos`, runtime debug material | “Vurdum ama değmedi” sorununu yakalar |
| Posture/Break Debug | Düşmanın posture değerini ve overflow hasarı gösterir | Enemy üstünde debug bar | ×3 cap sonrası overflow gerçekten çalışıyor mu görülür |
| Class A/B Compare | Aynı encounter'ı iki profile ile test eder | BalancePreset A/B hot swap | `physPower 110 mu 105 mi?` sorusunu ölçer |
| Encounter Difficulty Slider | Enemy HP/damage/count/affix density slider | `EncounterTuningProfile` | Zorluk class statından mı encounter'dan mı geliyor ayrılır |
| Seed Lock + Replay Seed | Aynı oda/düşman dizilimini tekrar üretir | Dungeon seed input + `Random.InitState(seed)` | Rastgelelik bahanesini kapatır |
| Stat Preset Save/Load | Slider ayarlarını JSON kaydeder | `JsonUtility.ToJson(runtimeProfile)` | İyi ayarları kaybetmezsin |
| Resource Economy Graph | Rage/Mana/Focus/Energy zaman grafiği | 30 sn history line graph | Kaynak çok mu hızlı doluyor, hiç mi dolmuyor görünür |
| Cooldown Uptime Tracker | Skill kaç kez kullanıldı, kaç sn CD'de kaldı | SkillBase activation/cooldown event log | Ölü skill'leri yakalar |
| Overkill/Wasted Damage Meter | Fazla hasar ne kadar boşa gidiyor? | `damage - remainingHP` | Execute/burst şişmesini ayıklar |
| Survivability Meter | Alınan hasar/sn, iyileşme/sn, ölümcül hit sayısı | Health event log | Tank/bruiser dayanıklılığı gerçek mi görülür |
| Enemy Pressure Timeline | Aynı anda kaç projectile/zone/melee threat aktif | Enemy cast event sayacı | Oda kaosu class statından ayrılır |
| Build Export Snapshot | Current class + stats + skills + items + room seed export | Clipboard JSON / Markdown export | Claude/Codex'e net test verisi atılır |

## Demo için öncelik sırası

### Priority 1 — Hemen

1. DPS Meter
2. Damage Source Breakdown
3. TTK Benchmark Room
4. Stat Preset Save/Load
5. Seed Lock
6. Build Export Snapshot

### Priority 2 — İlk denge turundan sonra

1. Resource Economy Graph
2. Cooldown Uptime Tracker
3. Overkill/Wasted Damage Meter
4. Survivability Meter
5. Hitbox/Hurtbox Overlay

### Priority 3 — Sunum cilası

1. Presenter Mode polish
2. Slow-mo hotkeys
3. Free-cam
4. Screenshot exporter
5. Side-by-side A/B report panel

## Debug HUD önerisi

Runtime HUD şunları göstermeli:

```txt
CLASS: Warblade
PROFILE: Warblade_v01
HP: 115 / 115
Phys: 110 | AP: 70 | AtkSpd: 0.90 | Move: 4.35
DPS 5s: 132.4
DPS 30s: 98.7
Damage Split:
  LMB: 54%
  Skill: 35%
  Item: 8%
  DoT: 3%
Resource:
  Rage generated: 230
  Rage wasted: 48
Room:
  Seed: ACT1_TEST_0042
  Clear Time: 36.2s
  Damage Taken: 81
```

## Gözden kaçan kritik şey

Slider yapmak yeterli değil. Denge verisi kaydedilmezse demo sadece oyuncak olur.

Minimum log şeması:

```txt
roomSeed
class
statProfile
skillLoadout
items
enemyPreset
clearTime
damageDealt
damageTaken
healingDone
deathCount
resourceGenerated
resourceWasted
topDamageSource
overkillDamage
postureDamage
```

Bunu JSON/CSV export etmek bitirme projesinde çok güçlü görünür:

> Aynı encounter seed'iyle Warblade, Ranger ve Elementalist test edildi. TTK, damage taken ve resource uptime şu şekilde değişti.

Bu “altyapı kuruldu” cümlesini gerçek yapar.
