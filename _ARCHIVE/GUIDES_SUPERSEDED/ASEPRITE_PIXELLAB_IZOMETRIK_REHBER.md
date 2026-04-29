# ASEPRITE + PIXELLAB — İZOMETRİK (ISOMETRIC) İŞ AKIŞI REHBERİ

Bu rehber, RIMA projesinin Goblinz Studio kalitesinde **Karanlık Fantezi (Dark Fantasy) İzometrik** sanat stiline geçişi için PixelLab ve Aseprite ortak iş akışını (pipeline) tanımlar.

---

## 0. TEMEL KURALLAR VE GRID SİSTEMİ
- **Kamera Açısı:** Yarı-İzometrik (Oyunlardaki yaygın Dimetrik 2:1 oran).
- **Aseprite Grid Ayarları:** `View > Grid > Grid Settings` menüsünden **İzometrik Grid** yapısını aktif edin ve 2:1 oranında (örneğin 32x16 veya 64x32px boyutlarında) ayarlayın.
- **Yönler (8 Yönlü Sistem):** Üretim yükünü yarıya indirmek için sadece 5 yön üretilir. Geri kalan 3 yön Unity içinde `SpriteRenderer.flipX` komutuyla halledilir.
  - **Üretilecek Yönler:** South (S), South-East (SE), East (E), North-East (NE), North (N).
  - **Flip ile Elde Edilecek Yönler:** South-West (SW), West (W), North-West (NW).

---

## 1. PROMPT MÜHENDİSLİĞİ (PIXELLAB)
PixelLab Prompt'larında kamerayı gerçek izometrik açıya kilitlemek ve perspektif bozulmalarını önlemek için şu sihirli anahtar kelimeler **ŞARTTIR**:
- `"isometric pixel art"`
- `"diagonal view"`
- `"45 degree angle"`
- `"orthographic projection"`
- Yön belirteci: `"facing southeast"`, `"facing down"`, `"facing northeast"`

**Örnek Üretim Formatı:**
*(Stil + Konu + Yön + İzometrik Belirteçler + Kalite Belirteçleri)*
`"isometric pixel art, heavy armored dark fantasy warrior knight, carrying a greatsword, facing southeast, 45 degree angle, orthographic projection, dark medieval aesthetics, transparent background"`

---

## 2. PİXEL TEMİZLİĞİ VE RENK AZALTMA (CLEANUP PIPELINE)
Yapay zeka çıktısı doğrudan oyuna dahil edilmez. Standart kaliteyi korumak için:
1. **Reduce Colors (Quantization):** Aseprite içinde AI'ın bıraktığı gradyanları temizlemek ve piksel art dokusunu sertleştirmek için renkleri azaltın (Color Mode -> Indexed -> 16 veya 32 renk).
2. **Rough Frame Cleanup:** İzometrik grid dışına taşan, bulanık (anti-aliased) veya asimetrik hissettiren pikselleri elle (pencil tool ile) temizleyin.
3. **Hitbox & Anchor Point:** Karakterin ayak bastığı zemini/odak noktasını (pivot), Aseprite içerisindeki 2:1 elmas grid'in merkez noktasına oturtun.

---

## 3. TILE VE TILESET (ÇEVRE) ÜRETİMİ
Zemin ve duvarlar düz bir kare (top-down) grid yerine, elmas (diamond) şeklinde tasarlanır.
- **Zeminler (Floors):** Tamamen elmas bloklar olarak üretilir. Prompt'a `"isometric tile floor, diamond shape, seamless, 2:1 ratio"` eklenir. (Örn 64x32 boyutlarında bir canvas)
- **Duvarlar (Walls):** Zemin elmasının üst kenarlarından yükselen sütunlar şeklinde olmalıdır.

---

## 4. İLK AKSİYON ADIMI: WARBLADE TESTİ 
Test için PixelLab arayüzüne (Aseprite Extension) girin.
1. **Model:** Generate 
2. **Size:** 64x64 veya 128x128 
3. **Prompt:** `"isometric pixel art, heavy armored dark fantasy warrior knight, carrying a greatsword, facing southeast, 45 degree angle, orthographic projection, transparent background"`
4. Çıktı alındıktan sonra Aseprite İzometrik Grid'i açın ve sonucu grid üzerine oturtup temizleme işlemini yapın.
