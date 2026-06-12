# RIMA RunMap/UI Asset — Düzeltilmiş Üretim Spec'i (v2)

> Bu dosya ChatGPT'ye (veya imagegen aracına) **doğru boyut + düzeltmelerle** yeniden üretim için.
> Önceki paket iyiydi ama 3 sorun vardı: (1) boyut oranları integer değildi → PPU 64 + Point filter'da bulanıklaşır, (2) player marker yanlış şekil ("A" harfi), (3) bazı node renkleri saf kırmızı (canon dışı). Aşağıda hepsi düzeltildi.

---

## 0. HERKES İÇİN GEÇERLİ KURALLAR (her prompt'a ekle)

```
STYLE: dark-fantasy pixel art game UI, top-down 2D roguelite
PALETTE (sadece bu renkler + tonları):
  - void purple   #3A1A4A   (panel arka plan, koyu zemin)
  - ember orange  #E89020   (vurgu, kenar, gem aksan)
  - slate         #3A3D42   (nötr taş, gri yüzeyler)
  - cyan accent   #00FFCC   (SADECE mevcut-konum vurgusu, ekranın ≤%15'i)
RENDER:
  - crisp pixel art, NO anti-aliasing, NO blur, NO soft gradients
  - transparent background (alpha PNG) — magenta KULLANMA
  - tek asset, ortalanmış, kenarda boşluk (padding) bırak
  - integer-friendly: aşağıdaki "author canvas" boyutunda üret
```

**Neden integer:** Unity'de PPU 64 + Point (nearest-neighbor) filter kullanıyoruz. Asset final boyutuna **tam katla** (2x/4x/8x) küçültülmeli. 265×385 gibi rastgele boyut → 120'ye küçültülünce bulanıklaşır. Aşağıdaki tablo her asset'i temiz integer orana sabitliyor.

---

## 1. BOYUT TABLOSU (KRİTİK)

| Asset grubu | Final (Unity display) | **Author canvas (bunu üret)** | Ölçek |
|---|---|---|---|
| RunMap node (her tip) | 128×96 px | **512×384 px** | 4× |
| Mevcut-node glow overlay | 128×96 px | **512×384 px** | 4× |
| Bağlantı dash segmenti | 32×32 px (tilelenebilir) | **128×128 px** | 4× |
| Rarity ribbon | 160×40 px | **640×160 px** | 4× |
| Minimap player marker | 24×24 px | **192×192 px** | 8× |
| Minimap room tile | 24×24 px | **192×192 px** | 8× |
| Minimap door marker | 16×16 px | **128×128 px** | 8× |
| Minimap frame | 280×220 px | **560×440 px** | 2× |

> Her grup **kendi içinde aynı canvas boyutunda** üretilmeli (tüm node'lar 512×384, tüm ribbon'lar 640×160). Tutarlı çerçeveleme = tutarlı görsel.

---

## 2. RUNMAP NODE SETİ (9 node)

**Canvas:** her biri 512×384 px, transparent, ortalanmış rounded-rectangle çerçeve + merkez ikon.
**Metin:** ÜRETME — etiketleri (SAVAŞ, ELİT...) Unity'de TextMeshPro ile basacağız. Sadece **ikon + çerçeve** çiz. (Önceki sette baked text vardı; bu sefer ikonsuz/yazısız temiz çerçeve istiyoruz.)

**Renk düzeltmeleri (saf kırmızı YASAK — ember ailesine çek):**

| Node | İkon | Çerçeve rengi | Not |
|---|---|---|---|
| `node_combat` | çapraz kılıçlar | slate-mavi `#4A5260` | demo-aktif |
| `node_elite` | boynuzlu kafa | ember-kırmızı `#8A3A20` | saf kırmızı DEĞİL |
| `node_boss` | kafatası | derin ember-crimson `#A8401C` | saf kırmızı DEĞİL |
| `node_merchant` | kese/$ | teal `#2A6B5A` | demo-aktif |
| `node_chest` | sandık | altın-ember `#8A6A20` | demo-aktif |
| `node_forge` | örs | void-mor `#5A3A6B` | demo-aktif |
| `node_event` | soru işareti | soğuk mavi `#3A5A6B` | OPSİYONEL (demo'da mekanik yok) |
| `node_hidden` | yok / silüet | koyu `#1A1A1E`, %40 opacity | reveal olmamış state |
| `node_current_glow` | (boş overlay) | cyan `#00FFCC` parlak halka | mevcut node üstüne biner |

> `node_event`'i atlayabilirsin (demo'da Event odası yok). `node_hidden` ve `node_current_glow` **state**'tir, oda tipi değil — ikisi de gerekli.

---

## 3. BAĞLANTI KİTİ (connection)

**Canvas:** 128×128 px, transparent.
- Tek bir **kesik çizgi (dash) segmenti** üret — dikey, gri `#505560`, yumuşak değil sert pixel.
- Ayrıca tek bir küçük **cyan diamond ◆** (yol işareti) üret, 64×64 canvas.
- Bunları Unity'de tekrarlayıp/uzatarak node'lar arası yol çizeceğiz (LineRenderer/dash tile).

---

## 4. RARITY RIBBON (3 adet) — bunlar zaten iyiydi, sadece boyut sabitle

**Canvas:** her biri 640×160 px, transparent.
- Yatay şerit, sol ve sağ uçta küçük **cyan diamond ◆** süs.
- Ortada baked yazı (lokalizasyon yok, baked OK): `COMMON` / `RARE` / `EPİK`
- Yazı: beyaz `#F4F0E6`, pixel font, ortalanmış.

| Ribbon | Şerit rengi |
|---|---|
| `ribbon_common` | gri `#8A9098` |
| `ribbon_rare` | mavi-cyan `#1B7BA8` |
| `ribbon_epic` | mor `#7B3FA8` (void-mor ailesi) |

---

## 5. MINIMAP MARKERLAR — player marker DÜZELT

**Canvas:** player & tile 192×192, door 128×128, transparent.

| Marker | Şekil | Renk | DÜZELTME |
|---|---|---|---|
| `player_marker` | **yönlü üçgen/ok** (yukarı bakan, facing yönü) | parlak cyan `#00FFCC` | ⚠️ "A" harfi DEĞİL — top-down oyunda yön bildiren ok olmalı |
| `room_tile` | dolu yuvarlak-köşeli kare | slate `#3A3D42` | OK, aynı kalsın |
| `door_marker` | küçük dikey çentik/çizgi | ember `#E89020` | OK, aynı kalsın |

---

## 6. MINIMAP FRAME — 9-slice uyumlu yeniden çiz

**Canvas:** 560×440 px, transparent, iç alan **tamamen boş** (harita oraya çizilecek).
- Final 280×220, **9-slice border = 18px** (author'da 36px).
- **KRİTİK:** Köşe süsleri (cyan diamond + ember gem) **sadece 4 köşede**, köşe-bölgesinin içinde kalmalı.
- **Kenarlar DÜZ olmalı** — üst/alt/yan kenar ortasında diamond/gem KOYMA (9-slice'ta stretch edilince tekrarlanıp bozulur).
- Border malzemesi: void-mor `#3A1A4A` zemin + slate `#22272D` taş çerçeve.

---

## 7. ÜRETİM SIRASI
1. Rarity ribbon (en temiz, palet ankoru)
2. Minimap marker (player = yönlü ok!)
3. RunMap node seti (renk düzeltmeleriyle)
4. Connection kit
5. Minimap frame (9-slice düz kenar)

---

## 8. ÇIKTI FORMATI
- Her asset **ayrı transparent PNG** (sheet değil — ya da hem sheet hem ayrı dosya).
- Dosya adları yukarıdaki `node_*`, `ribbon_*`, `player_marker` vb. ile birebir.
- Magenta arka plan **kullanma**, doğrudan alpha transparency ver.
```
