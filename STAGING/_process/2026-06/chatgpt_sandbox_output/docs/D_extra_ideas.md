# D) Ek Fikirler

## 1. Snapshot Stack
Sadece tek Quick Reset değil, 5 slotlu snapshot stack:
- F5: snapshot al
- F9: son snapshot’a dön
- Shift+F9: snapshot listesi

Sunumda çok iyi görünür: “aynı encounter’ı tekrar tekrar aynı koşulla test ediyorum.”

## 2. Dummy AI Mode
Spawn edilen düşmana AI davranışı atanmalı:
- Passive
- Attack Player
- Move Only
- Cast Only
- No Cooldown
- Boss Pattern Loop

Bu yoksa skill testleri combat karmaşasında kaybolur.

## 3. Hitbox / Hurtbox Overlay
Tek tuş: H
- Oyuncu hitbox: cyan
- Düşman hurtbox: amber
- Damage zone: kırmızı
- Projectile path: ince çizgi

Özellikle isometric/top-down okumada “vurdu mu vurmadı mı” tartışmasını bitirir.

## 4. Damage Source Waterfall
Telemetry sekmesinde hasar kaynağı kırılımı:
- LMB
- RMB
- Skill
- DoT
- Minion
- Item proc
- Environment

Summoner/Hexer gibi sınıflarda bu zorunlu. Yoksa DPS var ama neden var bilmiyorsun. Muhteşem bir bilinmezlik, ama tasarım için değil.

## 5. Encounter Recipe Export
Spawn + room + stats + class + seed tek JSON’a çıkar:
```json
{
  "roomSeed": "A13F",
  "class": "Warblade",
  "stats": { "hp": 115, "phys": 110 },
  "enemies": [{ "id": "ShardWalker", "pos": [4, 7] }],
  "tilesChanged": [],
  "props": []
}
```
Bu Claude/Codex’e bug veya balance görevi verirken altın değerinde.

## 6. Presentation Mode
Bitirme sunumu için ayrı mini mod:
- Sol rail küçülür
- Sadece aktif stat değişimi, DPS, TTK ve seed görünür
- “Before / After” stat preset hotkey’i

Hoca debug panel değil, sistem görür. Nadiren de olsa insan algısını düşünmek işe yarıyor.

## 7. Safety Layer
Director Mode build’de çalışacaksa:
- Sadece dev build / hidden flag ile açılır
- Save dosyasına yazmaz
- Analytics/achievements kapalı
- Runtime asset kirletmez

Yanlışlıkla oyuncuya kalırsa eğlenceli olur, ama senin için değil.
