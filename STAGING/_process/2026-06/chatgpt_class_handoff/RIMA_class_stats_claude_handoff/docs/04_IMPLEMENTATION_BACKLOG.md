# Implementation Backlog — Claude için Görev Listesi

Bu backlog mevcut flat damage sistemini kırmadan class stat altyapısına geçirmek için yazıldı.

## Ana hedef

Mevcut oyunu bozmadan şu sistemi ekle:

```txt
ClassStatProfile → ClassStatRuntime → DamagePacket → DamageCalculator → Health.TakeDamage(netDamage)
```

## Phase 0 — Önce analiz

Claude önce şu dosyaları incelemeli:

- `Assets/Scripts/Skills/SkillData.cs`
- `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs`
- `Assets/Scripts/Core/Health.cs`
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Player/PlayerClassManager.cs`

Çıktı:

- Hangi dosyalar değişecek?
- Hangi public API kırılabilir?
- Eski `DealDamage(int)` çağrıları kaç yerde var?
- BasicAttackProfile asset'leri nasıl migrate edilecek?

## Phase 1 — Veri modeli

### 1.1 Enum ekle

- `DamageType.cs`
- `DamageSourceType.cs`

### 1.2 DamagePacket ekle

- `DamagePacket.cs`
- baseDamage
- damageType
- sourceType
- attacker
- target
- canCrit placeholder
- sourceId / skillId optional

### 1.3 ClassStatProfile ekle

- ScriptableObject
- `maxHP`
- `physPower`
- `abilityPower`
- `attackSpeedMult`
- `moveSpeed`
- 5-bar UI fields
- debugGlobalDamageMult

### 1.4 ClassStatRuntime ekle

- Runtime copy olmalı.
- Slider'lar SO asset'i doğrudan bozmasın.
- Scene reset'te profile yeniden kopyalanabilsin.

## Phase 2 — DamageCalculator

### 2.1 Stat multiplier

```txt
Physical → physPower / 100
Ability  → abilityPower / 100
True     → 1
```

### 2.2 Identity/build cap placeholder

Şimdilik şu alanlar runtime'da 1.0 default olabilir:

- identityBuildMultiplier, cap 3.0
- situationalMultiplier, cap 2.0
- overflowToPosture placeholder

### 2.3 Health sınırı

`Health` damage scaling hesaplamasın. Sadece gelen net damage uygulasın.

## Phase 3 — Mevcut sistemi kırmadan bağlama

### 3.1 SkillRuntime

Eski çağrı:

```csharp
DealDamage(target, int damage)
```

Yeni overload:

```csharp
DealDamage(target, DamagePacket packet)
```

Eski overload geçici olarak kalsın, ama TODO yazılsın.

### 3.2 BasicAttackProfile migration

Şunları ekle:

```csharp
public DamageType lmbDamageType = DamageType.Physical;
public DamageType rmbDamageType = DamageType.Physical;
```

Asset defaultları:

| Class | LMB | RMB |
|---|---|---|
| Warblade | Physical | Physical |
| Elementalist | Ability | Ability |
| Shadowblade | Physical | Ability or Physical, mevcut skill'e göre |
| Ranger | Physical | Physical |
| Ronin | Physical | Physical |

### 3.3 PlayerClassManager

- Selected class → ClassStatProfile bul.
- Runtime copy oluştur.
- Health maxHP uygula.
- PlayerController moveSpeed uygula.
- BasicAttack speed multiplier'a bağlanabilir.

## Phase 4 — Class stat asset'leri

10 asset oluştur:

- `Warblade_StatProfile.asset`
- `Elementalist_StatProfile.asset`
- `Shadowblade_StatProfile.asset`
- `Ranger_StatProfile.asset`
- `Ravager_StatProfile.asset`
- `Ronin_StatProfile.asset`
- `Gunslinger_StatProfile.asset`
- `Brawler_StatProfile.asset`
- `Summoner_StatProfile.asset`
- `Hexer_StatProfile.asset`

Veri kaynağı:

- `data/class_stats_v01.json`
- `data/class_stats_v01.csv`

## Phase 5 — Debug panel

İlk debug panel şunları göstermeli ve değiştirmeli:

- maxHP
- physPower
- abilityPower
- attackSpeedMult
- moveSpeed
- debugGlobalDamageMult

Butonlar:

- Reset profile
- Save preset JSON
- Load preset JSON
- Export build snapshot

## Phase 6 — Telemetry

İlk telemetry eventleri:

- DamageDealt
- DamageTaken
- HealDone
- ResourceGenerated
- ResourceSpent
- SkillCast
- EnemyKilled
- RoomStarted
- RoomCleared

Export format:

- JSON line per room
- CSV summary per room

## Kabul kriterleri

- Eski skill'ler çalışmaya devam eder.
- Warblade/Elementalist/Ranger/Shadowblade/Ronin basic attack damage type alır.
- Debug panelden stat değişince hasar ve HP gerçek zamanda etkilenir.
- Aynı enemy üzerinde Phys/AP farkı ölçülebilir.
- Build snapshot export alınabilir.
- ClassStatProfile asset'leri doğrudan runtime'da kirlenmez.

## Kaçınılacak hatalar

- `Health` içine class stat scaling koyma.
- ScriptableObject asset'ini runtime slider ile doğrudan değiştirme.
- Tek `damageMult`ı production stat yapma.
- `moveSpeed` farklarını çok açma.
- Summoner AP'yi fazla yüksek başlatma.
- Debug tool'u gameplay code'a gömme; mümkünse `#if UNITY_EDITOR || DEVELOPMENT_BUILD` kullan.
