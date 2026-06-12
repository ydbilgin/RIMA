# CLAUDE'A PROMPT — RIMA Class Stat Sistemi Handoff

RIMA Unity 2D roguelite projesinde class stat sistemi için bu paketi incele ve uygulanabilir plan çıkar.

## Bağlam

RIMA'da 10 class var:

Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer.

Mevcut durumda kod sistemi flat:

- `Health.maxHP = 100` tüm class'larda aynı.
- `PlayerController.moveSpeed = 4.5` tüm class'larda aynı.
- Dash şu an sabit: speed 18, duration 0.15s, cd 0.8s.
- Per-class stat scaling yok.
- Farklılaştırma şimdilik çoğunlukla `BasicAttackProfile` asset'lerinde.
- `SkillRuntime` damage formülü yaklaşık olarak `base × statusMult × incomingDamageMult`.
- Attack/MagicAttack statı, crit, armor, merkezi class stat sistemi yok.

## Ana karar

Tek `damageMult` production modeline geçme.

RIMA canon açısından **Phys/AP split korunacak**:

- `physPower`
- `abilityPower`

Gemini'nin önerdiği tek `damageMult` sadece debug/presenter override olarak kullanılabilir:

```csharp
public float debugGlobalDamageMult = 1f;
```

Production stat omurgası:

```txt
maxHP
physPower
abilityPower
attackSpeedMult
moveSpeed
```

UI anlatımı:

```txt
Hasar / Dayanıklılık / Hız / Kontrol / Zorluk
```

## Senden istediğim

Önce bu paketteki dosyaları oku:

- `README.md`
- `docs/01_A_DECISION_PHYS_AP_VS_DAMAGE_MULT.md`
- `docs/02_B_CLASS_NUMERIC_TABLE.md`
- `docs/03_C_BALANCE_DEBUG_TOOLS.md`
- `docs/04_IMPLEMENTATION_BACKLOG.md`
- `docs/05_BALANCE_TELEMETRY_SPEC.md`
- `data/class_stats_v01.json`
- `data/class_stats_v01.csv`
- `code_snippets/*.cs`

Sonra repo içinde mevcut Unity dosyalarını incele:

- `Assets/Scripts/Skills/SkillData.cs`
- `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs`
- `Assets/Scripts/Core/Health.cs`
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Player/PlayerClassManager.cs`

## İlk çıktı formatın

Kod yazmadan önce şunu üret:

```md
# RIMA Class Stat Implementation Plan

## 1. Mevcut dosya analizi
- Hangi dosyalar değişecek?
- Hangi API kırılabilir?
- Eski DealDamage(int) çağrıları nerelerde var?

## 2. Yeni dosyalar
- DamageType.cs
- DamageSourceType.cs
- DamagePacket.cs
- ClassStatProfile.cs
- ClassStatRuntime.cs
- ClassStatDatabase.cs
- DamageCalculator.cs
- BalanceTelemetry.cs

## 3. Migration stratejisi
- Eski int damage çağrılarını nasıl kırmadan taşıyacağız?
- BasicAttackProfile asset'leri nasıl migrate edilecek?
- PlayerClassManager class seçince statleri nasıl uygulayacak?

## 4. Riskler
- Health içine scaling koyma riski
- ScriptableObject runtime mutation riski
- Summoner AP abuse riski
- MoveSpeed farklarını abartma riski

## 5. Uygulama sırası
Aşama aşama commit planı.
```

## Sonra implementation yap

Uygulama sırası:

1. `DamageType`, `DamageSourceType`, `DamagePacket` ekle.
2. `ClassStatProfile`, `ClassStatRuntime`, `ClassStatDatabase` ekle.
3. `DamageCalculator` ekle.
4. `BasicAttackProfile` içine damage type alanlarını ekle.
5. `SkillRuntime` içine `DamagePacket` kullanan yeni damage path'i ekle.
6. Eski `DealDamage(int)` çağrılarını kırmadan compatibility overload bırak.
7. `PlayerClassManager` class seçildiğinde runtime stat profile oluştursun.
8. `Health` sadece net damage uygulasın, class scaling bilmesin.
9. Debug panelde stat slider'ları ekle.
10. Telemetry starter ekle: DPS, damage source breakdown, room summary export.

## 10 class stat değerleri

Veri kaynağı olarak `data/class_stats_v01.json` kullan.

Kısa tablo:

| Class | Type | HP | Phys | AP | AtkSpd | Move |
|---|---|---:|---:|---:|---:|---:|
| Warblade | Phys | 115 | 110 | 70 | 0.90 | 4.35 |
| Elementalist | AP | 80 | 65 | 125 | 1.00 | 4.45 |
| Shadowblade | Phys | 80 | 95 | 80 | 1.35 | 4.75 |
| Ranger | Phys | 85 | 105 | 80 | 1.05 | 4.65 |
| Ravager | Phys | 125 | 115 | 65 | 0.85 | 4.35 |
| Ronin | Phys | 85 | 115 | 75 | 1.00 | 4.60 |
| Gunslinger | Phys | 85 | 100 | 80 | 1.25 | 4.75 |
| Brawler | Phys | 130 | 95 | 65 | 1.20 | 4.45 |
| Summoner | AP | 75 | 60 | 105 | 0.95 | 4.40 |
| Hexer | AP | 75 | 60 | 115 | 0.90 | 4.35 |

## Design guardrails

- Phys/AP split korunacak.
- Tek `damageMult` production stat olmayacak.
- `debugGlobalDamageMult` sadece debug/presenter override.
- `Health` class stat bilmeyecek.
- ScriptableObject asset'leri runtime slider ile doğrudan kirletilmeyecek.
- MoveSpeed farkları dar kalacak; class hissi finalde animasyon/recovery/cancel ile verilecek.
- Summoner AP yüksek başlatılmayacak; minion economy abuse riskli.
- Hexer sustained DPS değil, stack/blast penceresi class'ı olacak.
- Brawler tanky/control olacak ama raw phys damage aşırı yükseltilmeyecek.

## Kabul kriteri

- Oyun compile olmalı.
- Eski skill'ler bozulmadan çalışmalı.
- Stat slider'dan `physPower` değişince physical damage değişmeli.
- Stat slider'dan `abilityPower` değişince ability damage değişmeli.
- Warblade ve Elementalist aynı enemy üstünde farklı scaling göstermeli.
- Debug HUD damage source breakdown gösterebilmeli.
- Room summary JSON veya clipboard export alınabilmeli.
