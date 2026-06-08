# 04 — Room, Shop, Boss Minimum Plan

## Demo room sequence

Önerilen forced sequence:

```text
1. Combat_Small_Intro
2. Combat_Medium
3. Shop_01
4. Combat_PreBoss
5. Boss_PenitentSovereign_Demo
6. DemoClear
```

## Oda boyutları

Minimum playable alan:
```text
Combat: 24×18 veya 28×20
Shop: 20×14
Boss: 32×24 minimum, ideal 36×26 veya 40×30
```

Şu anki screenshot’taki arena görsel olarak hoş ama combat için küçük. Kenar cliff iyi, ama merkez alan genişlemeli.

## Combat room kuralları

Her combat odasında:
- Oyuncu spawn clear alanın alt/orta tarafında.
- Mob spawn oyuncudan en az 5-7 unit uzak.
- En az iki yönde 8+ unit dash lane.
- Obstacle merkezin tamamını kapatmaz.
- Görsel floor ile collider walkable aynı olmalı.
- Exit portal oda clear olmadan aktifleşmez.

## Combat_Small_Intro

Amaç:
- Oyuncu hareket + saldırı öğrenir.

Enemy:
- 3-4 melee enemy.
- Ranged enemy yok veya en fazla 1.

Reward:
- Küçük Echo veya skill draft.

## Combat_Medium

Amaç:
- Class farkı hissedilir.

Enemy:
- 4-5 melee
- 1 ranged/caster
- Spawn wave olabilir ama şart değil.

Reward:
- Skill draft veya heal pickup.

## Shop_01

Minimum shop:
```text
3 item:
1. Heal +30%
2. Damage +15%
3. Max HP +20 veya cooldown -10%
```

UI:
- Item adı
- Fiyat
- G ile satın al veya tıklama
- Echo yetersizse kırmızı/disabled
- Alınınca apply effect + item sold out

Shop görsel:
- Güzel olması şart değil.
- Ama neyin satıldığı okunmalı.
- Portal/exit net olmalı.

## Combat_PreBoss

Amaç:
- Boss öncesi son ritim.

Enemy:
- 6-8 enemy
- 1 elite-lite olabilir.
- Çok karmaşık yapma. Demo boss’tan önce oyuncuyu sinir etmek parlak fikir değil, sadece insanlara özgü bir hata.

Reward:
- Heal pickup veya küçük upgrade
- Boss portal açılır.

## Boss room

Boss:
- Penitent Sovereign demo versiyonu kullanılabilir.
- 1 boss.
- 3 attack yeterli.

Attack set:
1. Slam
   - Yakın radius
   - 0.6-0.8 sn telegraph
   - Damage yüksek ama okunur

2. Rift Line / Chain Line
   - Düz çizgi telegraph
   - Oyuncuyu dash ile kaçmaya zorlar

3. Projectile Burst veya Charge
   - Elementalist ve Warblade için farklı dodge hissi sağlar

Faz:
- Basit HP %50 modifier yeterli:
  - attack interval biraz azalır
  - telegraph aynı kalır
  - ekstra visual pulse

## Boss clear

Boss ölünce:
```text
BossDeathFX
→ input kısa süre kapalı
→ DemoClearPanel
→ "Demo Complete"
→ Return to Main Menu
```

## Room transition

Demo için dallanma şart değil.
Forced linear sequence daha iyi:
```csharp
int demoRoomIndex;
RoomTemplateSO[] demoSequence;
Build(demoSequence[demoRoomIndex]);
```

Graph sonra. Demo’da “hatalı dallanma” göstermek yerine kısa ve biten bir deneyim göster. Çok romantik değil ama işe yarar, insanlık bazen işlevselliği kabul etmeli.
