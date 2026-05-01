# RIMA HUD — "Ashen Glyph" Design Spec
**Tarih:** 2026-04-13  
**Durum:** ONAY BEKLENIYOR -> onaylaninca Unity greybox (rima-codex)  
**Felsefe:** Kirli kâğıt üstüne mürekkep — HUD dünyadan çıkmaz, dünyayla nefes alır.

---

## Genel Felsefe

> "UI yoktur — sadece bilgi vardır."

Dead Cells + Hollow Knight yaklaşımı: her şey **context'e göre görünür**. Oynarken ekranın %90'ı boş, sadece dungeon ve karakter. Bilgi ihtiyacın olduğunda zaten orada. 

- Sert çerçeve yok  
- Dikdörtgen bar yok  
- Her şey ya kaybolur ya pulses — hiçbir şey statik durmaz  
- Renk = state, sayı = son çare

---

## Layout Haritası

```
┌─────────────────────────────────────────┐
│ [ODA ADI]                   [MİNİMAP]   │
│                                         │
│                                         │
│           [ OYUN ALANI ]                │
│                                         │
│                                         │
│ [HP]                     [CURRENCY]     │
│ [RAGE]   [LMB][RMB][1][2][3][4][5]      │
└─────────────────────────────────────────┘
```

Sol alt: vitality  
Alt orta-sol: skill glyphs  
Sağ alt: currency  
Sağ üst: minimap  
Sol üst: oda adı (geçici)

---

## Eleman Detayları

---

### 1. HP Şeridi — "Kan Çatlağı"

**Konum:** Sol alt köşe, ekranın 12px içinde  
**Boyut:** 72px genişlik × 4px yükseklik  
**Yapı:** Yatay ince şerit — çerçevesiz, gölgesiz

**State renkleri (fill sol→sağ):**
| HP % | Renk | Efekt |
|------|------|-------|
| >60% | `#4A9EBF` soğuk mavi | Statik |
| 30–60% | `#C8742A` amber | Çok hafif pulse (0.5sn) |
| <30% | `#C42B2B` kızıl | Hızlı pulse + 1px glow |
| 0 | Sönüyor | Fade out |

**Zemin (boş kısım):** `#1A1A1A` %60 opacity, 1px'den ince görünüm  
**Sayı:** HP şeridinin sol ucunda, 6px mesafede, tiny pixel font  
Format: `314` (sadece mevcut, max yok — sadelik)  
Opaklık: %70, hover'da %100

---

### 2. Rage Şeridi — "Boşluk Özü"

**Konum:** HP şeridinin 3px altında, aynı sol hizası  
**Boyut:** 48px × 3px — HP'den %33 kısa ve ince  

**State:**
| Rage | Renk | Efekt |
|------|------|-------|
| 0 | Görünmez | Yok |
| 1–99 | `#7B3FA0` void mor | Dolar sola→sağa |
| 100 | `#B86AFF` parlak mor | Pulse + subtle glow |

Rage 0'da **kaybolur** — boş bar göstermez. Birikiyor, görünür.

---

### 3. Skill Glyphs — "Rift Runeları"

**Konum:** Alt merkez, sol kenardan 88px boşluk bırakarak (HP/Rage'den sonra başlar)  
**Slot sayısı:** LMB + RMB + 1 + 2 + 3 + 4 + 5 = 7 slot  
**Boyut:** LMB/RMB: 20×20px | 1-5: 16×16px  
**Aralık:** 5px  

**Görünüm:**
- İkon: beyaz pixel art sembol  
- Arkaplan: `#0D0D12` %55 opacity, **hexagon mask** — kare değil, altıgen  
- Sınır: yok — arkaplan şekli yeterli  

**State'ler:**
| Durum | Görünüm |
|-------|---------|
| Ready | Normal parlaklık |
| Cooldown | %30 opacity + clockwise dark sweep (pie timer, ince, 1px) |
| Empty slot | `—` sembolü, %20 opacity |
| Active (basılı) | 1px cold blue outer glow, 1 frame |

**LMB/RMB farkı:** Diğerlerinden 4px daha büyük, hafif parlak — öncelik hissettiriyor.

---

### 4. Currency — "Ruh Kıvılcımı"

**Konum:** Sağ alt köşe, 12px margin  
**Format:** `◆ 1,240` — küçük rune sembolü + sayı  
**Renk:** `#FFD166` altın sarısı  
**Font:** Tiny pixel, 6px yükseklik  

**Davranış:**
- Başlangıç: %35 opacity (fark edilmez)  
- Artış olduğunda: %100'e çıkar, hafif +N spawn animasyonu (0.8sn)  
- 3sn sonra: %35'e döner  
- Hover: %100  

---

### 5. Oda Adı — "Taş Yazıt"

**Konum:** Sol üst, 10px margin  
**Format:** `CRIMSON CRYPTS — ODA 4` küçük caps, pixel serif  
**Renk:** `#E8E4D9` kemik beyazı, %75 opacity  
**Boyut:** 8px font yüksekliği  

**Davranış:**
- Odaya girince: fade in 0.4sn  
- 3sn görünür  
- Fade out 1.2sn — yavaş, dramatik  
- Sonra: tamamen yok, ekranda hiçbir şey kalmaz

---

### 6. Minimap — "Kemik Tablet"

**Bu tasarımın en önemli kararı.**

#### Neden düz (yamuk değil):
Oyun ~60° perspektif görünümlü ama minimap **soyut bilgi** verir — geometri vermez. Soyut bilgiyi düz grid verir, yamuk grid vermez. Hades, Dead Cells, Hollow Knight hepsi düz — çünkü yamuk = confusing.

#### Tasarım: "Kırık Taş Levha"

**Konum:** Sağ üst, 10px margin  
**Boyut:** 72×72px  
**Dış çerçeve:** 4px kalınlık, kırık/aşınmış taş dokulu — köşeler mükemmel değil, hafif irregular pixel. Renk: `#3A3028` koyu taş.

**İçerik — Node Map:**
- Her oda: küçük dikdörtgen (8×6px)  
- Odalar arası bağlantı: 2px çizgi  
- Layout: basit grid (her katmanda max 4-5 oda)

**Oda renkleri:**
| Durum | Renk | Detay |
|-------|------|-------|
| Aktif (şu an) | `#6EC6FF` cold blue | 1px glow |
| Ziyaret edildi | `#4A4035` koyu taş | Dolu |
| Boss odası | `#8B2020` koyu kırmızı | Skull sembolü içinde |
| Hazine odası | `#B8860B` altın | Küçük ◆ içinde |
| Ziyaret edilmedi | `#1A1510` neredeyse siyah | Sadece gölge |
| Bilinmeyen | Yok | Görünmez — keşfedince belirir |

**Çerçeve üst kısmı:** Çok küçük bir başlık alanı yok — çerçeve yeterli. Gereksiz "MAP" yazısı yok.

**TAB basıldığında:** Minimap büyür → full screen overlay, oda açıklamaları görünür (ayrı spec).

---

## Renk Paleti Özeti

```
Cold Blue (HP full, aktif oda, glow):  #6EC6FF
Void Purple (Rage):                    #9B5DE5 → #B86AFF
Crimson (HP low):                      #C42B2B
Amber (HP mid):                        #C8742A
Soul Gold (currency, hazine):          #FFD166
Stone Dark (minimap bg, borders):      #3A3028
Near-Black (skill bg):                 #0D0D12
Bone White (text):                     #E8E4D9
```

---

## Animasyon Kuralları

| Eleman | Animasyon | Süre |
|--------|-----------|------|
| HP pulse (low) | Scale 1.0→1.03→1.0, loop | 0.6sn |
| Rage pulse (full) | Opacity 100→70→100, loop | 0.8sn |
| Skill cooldown | Pie sweep, clockwise | Gerçek cooldown süresi |
| Currency artış | Float-up +N text, fade | 0.8sn |
| Oda adı | Fade in → wait → fade out | 0.4 + 3 + 1.2sn |
| HP değişimi | Bar instant, glow flash 1 frame | — |

---

## Neyin OLMADAĞI (Yasak Listesi)

- ❌ Dikdörtgen çerçeveli panel / window background
- ❌ Siyah opak panel (beyni kesen blok)
- ❌ "HP: 314/720" format — sayı max'sız, minimal
- ❌ Sabit opacity — her şey context'e göre soluklaşır
- ❌ Yamuk minimap (isometric diamond)
- ❌ "MAP" / "SKILLS" / "RAGE" label yazıları — ikon ve renk anlatır
- ❌ Skill bar'ın altında/üstünde yatay ayırıcı çizgi
- ❌ Renk gradyanı yerine solid fill (eski bar hissi verir)

---

## Uygulama Sırası (Sonraki Session)

1. **Greybox (rima-codex)** — Unity'de placeholder renkli objelerle tam layout  
   - HP: 72×4px kırmızı Image  
   - Rage: 48×3px mor Image  
   - Skill slots: 7 adet hexagon-masked Image  
   - Currency: TextMeshPro sağ alt  
   - Minimap: 72×72px RenderTexture panel  
   - Oda adı: TextMeshPro sol üst, fade animasyonu  

2. **In-game test** — oynarken boyutlar doğru mu, okunuyor mu  

3. **Pixel art asset üretimi** — greybox onaylanınca Aseprite'ta  

---

*Bu doküman onaylanmadan Unity'e dokunulmaz.*
