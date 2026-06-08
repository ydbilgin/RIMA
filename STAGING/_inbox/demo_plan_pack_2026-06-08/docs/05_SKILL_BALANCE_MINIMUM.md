# 05 — Skill and Balance Minimum

## Amaç

Warblade ve Elementalist birbirinden farklı hissettirmeli. 192 skill’i demo’ya sokmak yok. Bu cümle özellikle önemli, çünkü fazla skill = fazla bug = fazla ağlama.

## Demo skill slotları

Öneri:
```text
LMB: primary
RMB: secondary
Q: class skill
Space: dash
```

6 slot UI kalsın ama ilk demo için 3 aktif skill yeterli.

## Warblade minimum

### LMB — Basic Slash
- Yakın cone/arc hitbox
- 1.0x damage
- Çok kısa cooldown veya combo window

### RMB — Cleave
- Daha geniş arc
- 1.5x damage
- 4-5s cooldown

### Q — Iron Charge
- Kısa ileri dash/saldırı
- Collision güvenli olmalı
- 6-8s cooldown

### Pasif
- Hit başına Rage + küçük damage buff opsiyonel

## Elementalist minimum

### LMB — Rune Bolt
- Orta menzil projectile
- 1.0x damage
- Projectile spawn rune disc civarında

### RMB — Arcane Burst
- Küçük AoE
- 1.3x damage
- 5-6s cooldown

### Q — Frost/Fire Field
- Kısa süreli alan
- slow veya burn
- 8-10s cooldown

### Pasif
- Consecutive cast veya mana regen şimdilik şart değil.

## Balance hedefi

Combat room clear süresi:
```text
Warblade: 25-45 saniye
Elementalist: 25-45 saniye
```

Boss kill süresi:
```text
Warblade: 90-150 saniye
Elementalist: 90-150 saniye
```

Ölüm:
- Oyuncu hata yaparsa ölür.
- İlk oda oyuncuyu öldürmez.
- Boss 2-4 denemede öğrenilebilir olmalı.

## Test ölçümleri

Her class için 5 run:
```text
Average room clear time
Boss time to kill
Damage taken
Death count
Most used skill
Least used skill
```

## Denge kaçış planı

Warblade çok güçsüzse:
- Cleave damage +15%
- Charge cooldown -1s
- Basic slash range +10%

Elementalist çok güçsüzse:
- Projectile speed +20%
- AoE radius +10%
- Cast lock azalt

Warblade çok güçlüyse:
- Cleave cooldown +1s
- Charge damage azalt

Elementalist çok güçlüyse:
- Projectile damage -10%
- AoE cooldown +1s

## Shop upgrade minimum

Shop itemları skill sistemini delmemeli.

Basit upgrade:
```text
+15% damage
+20 max HP
Heal 35%
-10% cooldown
```

Stack sistemi şimdilik yok veya max 1.
