# 03 — Gerçekten Üretilmesi Gereken Asset Listesi

Bu liste "güzel asset olsun" diye değil, mevcut ekranların en büyük algı problemlerini kapatmak için yazıldı. Floor'ları tekrar üretme. Full wall sistemi kurma. Düşman sprite'ları ayrı iş; burada mob sanatı yok.

## P0 — En yüksek etki, en az scope

### 1. Portal rune seti — 32×32
**Adet:** 4  
**Dosyalar:**
- `rune_combat_32.png`
- `rune_elite_32.png`
- `rune_chest_32.png`
- `rune_boss_32.png`

**Neden:** 12_portals karesinde portallar güzel ama aynı görünüyor. Oyuncu rota kararını anlamıyor.

**Prompt yönü:** küçük, yüksek kontrast, tek ikon, transparent.

### 2. Portal label plaque / world label frame — 96×24 veya 128×32
**Adet:** 1 base frame  
**Neden:** Portal üstüne `COMBAT / ELITE / CHEST / BOSS` yazmak için tutarlı frame.

### 3. Hole rim / broken void edge decal — 64×32 ve 64×64
**Adet:** 4
- straight rim
- corner rim
- cracked rim
- glowing rift rim

**Neden:** İç delikler şu an düz siyah kare. Bu map hatası gibi duruyor.

### 4. Arrival ring VFX sheet/decal — 96×96
**Adet:** 1 sprite sheet veya 4 frame
**Neden:** 07 spawn anında oyuncu odaya "spawn edilmiş" gibi duruyor. Arrival ring bunu diegetic yapar.

### 5. Boss HP bar frame — UI 9-slice
**Boyut:** 512×48 veya scalable 9-slice  
**Neden:** 16_boss_room karesindeki sarı düz bar en büyük sunum kırıklarından biri.

## P1 — Oda makyajı

### 6. Edge filler prop set
**Adet:** 8 küçük prop  
**Boyut:** 32×32 / 64×64 / 96×64

Öneriler:
- broken stone chunk
- rift shard
- hanging chain stump
- cracked banner pole
- small rubble pile
- broken brazier
- void root / dark crystal nub
- tiny altar debris

**Neden:** Büyük odaların kenarları boş. Bunlar gameplay alanını daraltmadan görsel yoğunluk verir.

### 7. Landmark prop set
**Adet:** 5 büyük prop  
**Boyut:** 96×96 / 128×128 / 128×160

Öneriler:
- Seal Monolith
- Rift Crystal Cluster
- Broken Altar
- Toppled Statue Fragment
- Chained Obelisk

**Neden:** Her oda 1 landmark ister. Yoksa sadece floor + cliff.

### 8. Ground decal set
**Adet:** 10  
**Boyut:** 64×32 / 96×48 / 128×64

Öneriler:
- thin cyan crack
- circular rift scar
- broken ritual line
- old blood stain dark red
- scratched combat marks
- portal ground scorch
- faded rune tiles
- broken tile highlight
- boss red fracture
- chest gold dust mark

**Neden:** Floor tekrarını kırar. Asset değil "makyaj" ama etki büyük.

### 9. Back-edge low parapet / broken portal band
**Adet:** 4  
**Boyut:** 64×48 veya 96×64

**Neden:** Portalların arkasında tamamen boş void bazen "bitmemiş" okuyor. Full wall değil, düşük kırık arka çerçeve yeterli.

## P2 — Chamber özel

### 10. Smaller pedestal set
**Adet:** 2-3 varyant  
**Boyut:** 96×64 veya 128×80

**Neden:** Mevcut pedestal çok büyük ve karakter/label kapatıyor. Daha küçük pedestal + class glow daha iyi.

### 11. Class selection glow rings
**Adet:** 3
- locked/dim
- available
- selected

**Boyut:** pedestal footprint'e göre 96×48 / 128×64

### 12. Weapon silhouette icons
**Adet:** 10 küçük ikon  
**Boyut:** 32×32 veya 48×48

**Neden:** "1 class = 1 weapon = 1 silhouette" kararını character select ekranında direkt satarsın.

## P2 — Boss özel

### 13. Boss ritual circle
**Boyut:** 192×96 veya 256×128 isometric ellipse
**Neden:** Boss odası normal oda gibi duruyor. Bu tek decal bile "boss arena" der.

### 14. Boss seal fragments
**Adet:** 4-6 küçük shard  
**Boyut:** 32×32 / 48×48
**Neden:** Boss intro'da baş/arena çevresinde döner; sprite final olmasa bile boss moment yaratır.

### 15. Telegraph decals
**Adet:** 3
- line
- cone
- circle

**Boyut:** gameplay ölçüsüne göre
**Neden:** Boss ve mob readability için ortak kullanılır.

## Üretmeye gerek olmayanlar

- Yeni floor seti
- Full wall seti
- Cliff corner/endcap sistemi rewrite
- 8 yön portal
- Entry portal object
- Her oda için full painted background
- 10 farklı biome tema
- Yeni menu background
- Şimdilik shop/heal/lore portal

## PixelLab üretim batching önerisi

### Batch A — 32×32
- rune_combat
- rune_elite
- rune_chest
- rune_boss
- small rift shard
- small rubble
- chain stump
- broken rune icon

### Batch B — 64×64
- hole rim corner
- hole rim cracked
- broken brazier
- small altar debris
- void crystal nub
- banner stump

### Batch C — 96×48 / 96×64
- portal label plaque
- straight hole rim
- portal ground scorch
- rift crack decal
- low parapet segment

### Batch D — 128×128 / 128×160
- Seal Monolith
- Rift Crystal Cluster
- Broken Altar
- Toppled Statue
- Chained Obelisk

### Batch E — Boss
- boss ritual circle
- boss red fracture decal
- seal fragments
- boss HP bar frame
