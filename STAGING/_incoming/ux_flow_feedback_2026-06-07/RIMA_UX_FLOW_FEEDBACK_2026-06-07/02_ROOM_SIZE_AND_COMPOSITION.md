# 02 — Oda Boyutu, Kamera ve Kompozisyon Feedback

## Kısa hüküm

Mevcut run odaları teknik olarak çalışıyor ama **çok büyük, çok boş ve çok düz** duruyor. Bu, düşman placeholder'ından bağımsız bir problem. Moblar final olsa bile oyuncu hâlâ koca taş halının üzerinde yürüyormuş gibi hisseder.

Bu oyunun odaları "dev savaş alanı" değil, "void üstünde sıkı, okunur combat adaları" olmalı.

## Mevcut problem

### 1. Oyuncu/oda oranı yanlış
Oyuncu çok küçük kalıyor. 1920×1080 karede odanın büyük kısmı sadece tekrar eden floor. Bu da oyunu mobil/prototip gibi değil, boş editör sahnesi gibi gösteriyor.

### 2. Oda silüeti organik ama gameplay kompozisyonu yok
Adanın şekli dıştan ilginç; içi yine düz açık alan. Prop ve landmark'lar combat akışını yönlendirmiyor.

### 3. Cliffler güçlü ama fazla tekrar ediyor
Cliffler havada ada hissi veriyor. Ama uzun kenarlarda aynı doku tekrarlandığı için screenshot'ta "pattern wall" gibi okuyor.

### 4. İç boşluk/hole düz siyah kare
Donut/delik güzel fikir ama kenar rim'i yoksa debug hole gibi duruyor.

## Önerilen oda boyutları

Bu öneriler grid mantığı içindir; birebir zorunlu değil ama hedef kompozisyonu netleştirir.

| Oda tipi | Önerilen walkable alan | Amaç |
|---|---:|---|
| Normal Combat | 18×12 ile 22×14 | Daha sıkı savaş, daha az boşluk |
| Elite | 20×14 ile 24×15 | Daha geniş ama hâlâ kontrol edilebilir |
| Chest/Reward | 12×8 ile 16×10 | Küçük, güvenli, hızlı okunur |
| Branch/Portal room | 18×10 ile 22×12 | Üst kenarda 1-3 portal net görünür |
| Boss | 26×16 ile 30×18 | Büyük ama merkez boş, boss üst-orta |
| Chamber | 24×16 civarı | Pedestal tören alanı, ama disk kalabalığı yok |

## Kamera önerisi

Şu an run odalarında kamera biraz fazla geniş hissettiriyor.

### Demo screenshot için
- Combat kamerayı **%10-15 yakınlaştır**.
- Oyuncu sahnenin sağ-altında sıkışmasın; merkez-sağ yerine orta-alt/orta olsun.
- Portal karelerinde üst/back edge görünecek kadar geniş, ama floor boşluğu azaltılmış olmalı.

### Canlı gameplay için
- Kamera zoom'u oda tipine göre sabitlenebilir:
  - Combat: yakın
  - Portal seçim: biraz geniş
  - Boss: geniş
  - Chamber: geniş ama karakterler okunacak kadar yakın

## Oda kompozisyon reçetesi

Her combat oda şu beş bölgeyle tasarlanmalı:

```text
[Exit Band]     portal slotları + 1-2 landmark prop
[Upper Combat]  düşman spawn / ranged zone
[Combat Core]   temiz hareket alanı, minimum prop
[Lower Combat]  oyuncu giriş / ilk güvenli alan
[Entry Zone]    arrival VFX + kısa yönlendirme
```

## Normal combat oda makyajı

Mevcut büyük odaları çöpe atma. Şunlarla kurtar:

1. **Combat Core'u görsel olarak daralt**  
   Kenarlara prop/decal koy, ortayı temiz bırak. Böylece oda fiziksel olarak büyük kalsa bile göz "oynanacak alanı" anlar.

2. **Edge filler kullan**  
   Cliff kenarlarına küçük kırık taşlar, chain stumps, rune shards, rubble clusters.

3. **Deliğe rim ekle**  
   Siyah kare deliğin çevresine 1 tile genişliğinde broken rim/decal koy. Yoksa "map hatası" gibi.

4. **Back edge'i portal band yap**  
   Portalların arkasına full wall değil, 2-3 düşük kırık sütun/parapet segmenti koy.

5. **Spawn alanına arrival ring**  
   Oyuncu odaya "bırakılmış" gibi değil, Rift'ten düşmüş gibi gelsin.

## Chamber makyajı

Mevcut Chamber'ın sorunu boyuttan çok **pedestal scale ve label düzeni**.

Yapılacak:
- Pedestal çapını %25-35 küçült.
- 10 pedestal iki yay şeklinde:
  - ön yay: 5 class
  - arka yay: 5 class
- Merkezde büyük rift crack / class binding altar.
- Seçilebilir class pedestalında:
  - class name
  - small glow
  - küçük weapon silhouette
- Seçili class:
  - pedestal ring pulse
  - karakter ghost yükselir
  - `Bürün: Warblade [E]`

Mevcut düz büyük diskler karakter seçimini "sınıf vitrini" yerine "beton kapak koleksiyonu" gibi gösteriyor. Evet, acımasız ama görünen bu.

## Boss odası makyajı

Boss odası normal combat odası gibi görünmemeli.

Minimum:
- Merkezde veya üst-orta boss arena seal.
- Boss spawn noktasında ritual circle.
- Boss HP bar koyu çerçeveli, düz sarı blok değil.
- Arena kenarlarında 2-4 boss-specific prop:
  - kırık kraliyet sancakları
  - chained monolith
  - red fracture crystals
  - floating seal fragments
- Boss intro 1.5s:
  - arena hafif kararır
  - boss adı çıkar
  - ritual circle pulse
  - HP bar slide-in

## Portal odası makyajı

Portallar iyi başlangıç ama tür bilgisi yok.

Her portal:
- frame
- core glow
- rune icon
- small label
- ground crack/decal
- proximity highlight

Portal türleri:
- Combat: cyan/white crossed blades
- Elite: magenta/red crack + skull/diamond rune
- Chest/Reward: gold chest/rune
- Boss: red seal/crownless fracture rune

2 portal durumunda:
- NW + NE açık
- center boş kalmalı
- ortadaki boşluk bilinçli görünmeli; center'da hafif inactive rift scar olabilir ama portal gibi görünmesin.

## Hızlı kazanım sırası

1. Kamera %10 yakın + player kompozisyonu düzelt.
2. Portal label/rune ekle.
3. Chamber pedestal küçült.
4. Boss HP bar ve yeşil debug kareyi temizle.
5. Hole rim / void edge decal üret.
6. Ground decal ve edge filler ekle.
7. Void background depth ekle.
