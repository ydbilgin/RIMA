# PixelLab Tile Prompt — Genel Notlar
# Act 1: Shattered Keep | Isometric 2D Tilemap

---

## Isometric Aci Tutarliligi

Isometrik tile uretiminde en sik karsilasilan sorun: PixelLab'in farkli varyasyonlarda
farkli kamera acisi kullanmasidir. Bunu onlemek icin:

1. Her promptta "isometric top-down angle" veya "isometric front-facing" ifadesini acik yaz.
2. Floor tile icin: "2:1 diamond rhombus, left/right corners at horizontal midpoints" ifadesi
   perspektifi kitler.
3. Wall tile icin: "vertical front surface" + "top face zone 10px" gibi piksel-duzeyinde
   bolge tanimlamalari acinin kaymasini onler.
4. Tum tile seti uretildikten sonra yan yana koydugunda perspektif tutarsizligi
   goz ile hemen anlasilir — birbirini takip etmeyen tile'lari ayikla.

---

## Seamless Tiling (Dikis Gozukmemesi)

Floor tile icin:
- Diamond sinirinin tam kenarindaki piksel tas rengi olmali, arka plan degil.
  PixelLab bazen siniri 1-2px iceriden bitirir — bu komsu tile'larla bosluk olusturur.
- Prompt'a "tile edges are seamless, pattern continues flush to diamond boundary" ekle.
- Unity'de Rule Tile veya 4-way connected tile kullaniyorsan, kenarlardaki ton
  farkinin minimum olmasi gerekir.

Wall tile icin:
- Tugla paterinin sol ve sag kenarlarinin ortada birleşmesi gerekir.
  Staggered (zigzag) tugla paterinde sol kenar yarım tugla ile bitmeli,
  sag kenar da o yarim tuglayi tamamlamali.
- Prompt'a "brick pattern wraps seamlessly at left and right edges" eklenebilir.

---

## Chromakey: Sik Yapilan Hatalar

| Hata                          | Sonuc                              | Duzeltme                         |
|-------------------------------|------------------------------------|----------------------------------|
| Arka plan #00FF00 degil       | process_tiles.py silmiyor          | Background Color alanini kontrol et |
| Arka plan transparan          | Pipeline chromakey bulamiyor       | Solid green sec, transparan degil |
| Koyu tile rengi G>200         | Tile pikselleri de siliniyor       | Paleti kontrol et, G<150 olmali  |
| Magenta #FF00FF kullanimi     | Eski pipeline, deprecated          | Her zaman #00FF00 kullan         |
| JPEG kaydetme                 | Renk sıkıstırma chromakey bozar    | PNG olarak kaydet                |

---

## Varyasyon Stratejisi (16 variations)

16 varyasyon uretirken PixelLab'e verilen prompt tum varyasyonlara esit uygulanir.
Cok spesifik "varyasyon 3'te X olmali" gibi kontrol yoktur. Bu nedenle:

- Prompt'a "each variation has a DIFFERENT subtle accent" yaz — bu varyasyonlari
  birbirinden ayiran tek bir kucuk unsur olmali demektir.
- "Never more than one accent per tile" yazarak prompts cok kirlilikten korur.
- Varyasyonlar arasinda renk tonu farkı minimum olmali; desen farki maksimum.
  Renk tutarsizligi Unity'de tile'lar yan yana geldiginde goz alici olur.

---

## Yaygin PixelLab Hatalari ve Cozumleri

| Hata                               | Neden                          | Prompt Duzeltmesi                              |
|------------------------------------|--------------------------------|------------------------------------------------|
| Anti-aliasing / bulanik kenarlar  | Varsayilan smooth rendering    | "hard pixel edges, no anti-aliasing"           |
| Gradient shading                   | AI guzel goruntuyu tercih eder | "NO gradients, flat baked lighting"            |
| Cok kucuk piksel detayi (<2px)     | AI kucuk doku olusturur        | "pixel clusters minimum 4px wide"              |
| Yanlis perspektif / kamera kayması | Prompt acik degilse            | Diamond/zone olcularini piksel seviyesinde ver |
| Varyasyonlar cok benzer           | Accent belirtilmemis           | "subtly different ... one accent feature"      |
| Arka plan rengi kaçiyor            | Settings gozden kacinildi      | Her uretimden once Settings'i kontrol et       |

---

## Canvas Boyut Referansi

| Tile Turu    | Canvas    | Notlar                                  |
|--------------|-----------|-----------------------------------------|
| Floor (F1)   | 64 x 64   | Diamond shape, 2:1 isometric            |
| Wall (W1)    | 64 x 96   | 3 zone: top face / front face / AO base |
| Large Object | 128 x 128 | NPC/boss reference (ayri pipeline)      |

---

## process_tiles.py Referans

Filtre mantigi:
  pixel kaldirılır eger: G > 200 AND R < 60 AND B < 60

Guvenli palet alanlari (filtreden gecmez, yani tile uzerinde kalir):
  #2A2A35 — R=42, G=42, B=53  → R=42 < 60 ama G=42 < 200, GUVENLI
  #1A1A25 — R=26, G=26, B=37  → G=26 < 200, GUVENLI
  #404050 — R=64, G=64, B=80  → G=64 < 200, GUVENLI

Tehlikeli renk alanlari (yanilsiyla chromakey silebilir):
  Hicbir tile rengi G > 200 olmamali — yesile calan renk kullanma.
