# B — 10 Class Sayısal Tablo v0.1

## Baz referans

```txt
Base HP = 100
Base physPower = 100
Base abilityPower = 100
Base attackSpeedMult = 1.00
Base moveSpeed = 4.50
```

## Mevcut kod sinyali

Kod analizine göre class farklılaştırması şu an neredeyse sadece `BasicAttackProfile` asset'lerinde:

| Implemented Class | Mevcut LMB raw |
|---|---:|
| Warblade | 25 + 30 + 40 = 95 |
| Elementalist | 18 + 18 + 28 = 64 |
| Shadowblade | 20 |
| Ranger | 18 |
| Ronin | 24 + 28 + 56 = 108 |

Bu değerler doğrudan DPS değildir. Range, recovery, cooldown, resource, state ve güvenlik penceresiyle birlikte okunmalı.

## Önerilen tablo

| Class | Arketip | Hasar tipi | maxHP | physPower | abilityPower | attackSpeed× | moveSpeed | Hasar | Dayan | Hız | Kontrol | Zorluk |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Warblade | Bruiser / Breaker | Phys | 115 | 110 | 70 | 0.90 | 4.35 | 4 | 4 | 2 | 4 | 2 |
| Elementalist | Mage / Controller | AP | 80 | 65 | 125 | 1.00 | 4.45 | 5 | 2 | 3 | 4 | 4 |
| Shadowblade | Assassin / Mobility | Phys | 80 | 95 | 80 | 1.35 | 4.75 | 4 | 2 | 5 | 3 | 5 |
| Ranger | Ranged / Trap-control | Phys | 85 | 105 | 80 | 1.05 | 4.65 | 4 | 2 | 4 | 5 | 4 |
| Ravager | Berserker / HP-trade | Phys | 125 | 115 | 65 | 0.85 | 4.35 | 5 | 4 | 2 | 3 | 4 |
| Ronin | Precision / Iaido burst | Phys | 85 | 115 | 75 | 1.00 | 4.60 | 4 | 2 | 4 | 3 | 5 |
| Gunslinger | Run-and-gun ranged | Phys | 85 | 100 | 80 | 1.25 | 4.75 | 4 | 2 | 5 | 3 | 4 |
| Brawler | Combo tank / Launcher | Phys | 130 | 95 | 65 | 1.20 | 4.45 | 4 | 5 | 3 | 5 | 4 |
| Summoner | Minion AP / Sacrifice | AP | 75 | 60 | 105 | 0.95 | 4.40 | 4 | 2 | 2 | 5 | 5 |
| Hexer | Curse AP / Stack burst | AP | 75 | 60 | 115 | 0.90 | 4.35 | 4 | 2 | 2 | 5 | 5 |

## Class gerekçeleri

### Warblade

- Mevcut 95 raw LMB combo zaten ağır vuruş sinyali veriyor.
- `physPower 110`, `attackSpeed 0.90` tutuldu.
- Amaç: güçlü tek combo, yavaş spam değil.
- `maxHP 115` yeterli; daha fazlası Brawler/Ravager alanına girer.

### Elementalist

- Mevcut 64 raw combo düşük ama AP skill burst ve kontrolle büyüyecek.
- `abilityPower 125`, `maxHP 80`.
- Çok kırılgan yapılmadı çünkü demo aşamasında animasyon cancel ve positioning kalitesi henüz final değil.

### Shadowblade

- 20 raw LMB düşük ama hızlı tekrar, phase ve Scar/collapse ile büyümeli.
- `attackSpeed 1.35`, `moveSpeed 4.75`.
- `physPower 95`: hasar stattan değil state penceresinden gelsin.
- Fazla phys verilirse assassin değil blender olur.

### Ranger

- 18 raw projectile düşük görünür ama range güvenliği ve trap/mark kontrolüyle dengelenir.
- `physPower 105`, `attackSpeed 1.05`, `moveSpeed 4.65`.
- Kontrol 5 çünkü class fantasy mark/trap/detonate üstüne kurulu.

### Ravager

- `maxHP 125`, `physPower 115`, `attackSpeed 0.85`.
- Warblade'den daha riskli ve daha vahşi.
- Fury/HP trade çalışınca patlamalı, baseline'da zaten patlamamalı.

### Ronin

- Mevcut 108 raw combo yüksek sinyal.
- `physPower 115`, `attackSpeed 1.00`, `maxHP 85`.
- Hasar timing ve Draw Window üzerinden çıkmalı.
- Zorluk 5 çünkü kaçırılan timing ağır cezalandırılmalı.

### Gunslinger

- `attackSpeed 1.25`, `moveSpeed 4.75`, `physPower 100`.
- Hasar çoklu atış, heat/reload yönetimi ve positioning'den gelmeli.
- Direkt `physPower 115` verilirse Ranger ile çakışır.

### Brawler

- `maxHP 130`, `control 5`.
- `physPower 95`: hasar raw stattan değil combo threshold, launch, Cracked/Shattered ve perfect hit pencerelerinden gelsin.
- En dayanıklı class olabilir ama en yüksek raw damage olmamalı.

### Summoner

- `abilityPower 105` kasıtlı düşük.
- Gerçek DPS minion cap, sacrifice ve corpse field ekonomisinden gelir.
- Summoner'a yüksek AP verilirse minion sayısı + sacrifice loop class'ı odayı otomatik oynar hale getirir.

### Hexer

- `abilityPower 115`, `attackSpeed 0.90`.
- Yüksek hasar Hex stack/blast penceresinde çıkmalı.
- Sürekli DPS orta kalmalı, yoksa 10-stack hedefi anlamsızlaşır.

## MoveSpeed farkları neden dar?

```txt
En düşük: 4.35
En yüksek: 4.75
Yaklaşık fark: %9
```

RIMA kararlarında tüm class'ların hızlı hissettirmesi gerekiyor. Bu yüzden ağır/hafif farkı finalde şu katmanlardan gelmeli:

| Hissiyat | Finalde nereden gelmeli? |
|---|---|
| Warblade ağır | startup, hit-stop, recovery, geniş impact |
| Shadowblade hızlı | kısa recovery, phase, cancel window |
| Ronin keskin | wait/draw/punish penceresi |
| Ravager vahşi | HP trade, damage taken tempo, berserk |
| Brawler baskıcı | combo rhythm, launch, body movement |

## Tuning notu

Bu tablo v0.1 başlangıç presetidir. İlk testte ölçülmesi gerekenler:

- TTK single target
- TTK 3 target
- TTK 8 swarm
- Damage taken / room
- Resource generated / room
- Resource wasted / room
- Clear time
- Top damage source
