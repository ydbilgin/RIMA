ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory.

RESPOND INLINE — dosya yazma. Tasarımı reply olarak döndür.

# Amaç (DÜŞÜN, ÜRETME)
RIMA'nın duvarsız floating-ada zemininde "mantıksal çatlaklar / patch'ler" için ÜRETİM SPEC + YERLEŞİM MANTIĞI tasarla. Asset ÜRETME — sadece boyut + nasıl üretilir + nereye/nasıl doğal eklenir.

# Bağlam (LOCKED)
- Görsel: wall-less floating-island Hades Elysium, void üstünde asılı ada, cliff kenarları karanlığa düşer.
- Palet: slate #3A3D42 / cyan rift #00FFCC / warm #E89020 / deep purple #3A1A4A.
- Kamera High Top-Down 3/4 ~70-80°, PixelPerfect 640×360, PPU 64, tile 32px grid.
- 6-layer: L1 Floor / L2 Cliff base / L3 Cliff face decor / L4 Walkable decor / L5 Wall blocker / L6 Gameplay.
- Cyan "yarık" (rift) 3-scale görsel dil mevcut (mikro/orta/büyük). Çatlaklar bununla tutarlı olmalı.
- Üretim aracı: create_object / create_map_object (PixelLab). 32px tile-aligned veya 48/64 decor.

# Sorular (gerekçeli)
1. **Çatlak tipleri:** Floating ada zeminine hangi mantıksal çatlak/patch türleri uyar? (a) zemin taş çatlağı (yapısal yorgunluk), (b) cyan rift-crack (enerji sızıntısı, parlar), (c) kenar-erozyon (void'e doğru parçalanma), (d) yama/onarım izi (farklı taş). Her tip ambiyansta NE anlatır?
2. **Boyut (create_object):** Her tip hangi px? Tile-aligned 32px mı, serbest decor 48/64px mı? Tek-parça mı tileable-set mi? Gerekçe (PPU 64, grid 32). Hangi tip kaç varyant?
3. **Yerleşim MANTIĞI:** Doğal görünmesi için nereye, hangi kuralla? (rift yakını cyan-crack yoğun / kenar yakını erozyon / merkez seyrek / yüksek-trafik path'te aşınma). Decor L4 mü L1-overlay mı? Random scatter mı weighted-rule mı? Room Painter brush ile mi otomatik mi?
4. **Saçmalık tespiti:** Bu çatlak planında mantıksız/uygulanamaz/over-engineered bir şey var mı? (örn. her tile'a çatlak = noise; cyan-crack lighting ile çakışma; 32px çatlak 64-PPU'da görünürlük)
5. **Üretilebilirlik:** Her çatlak tipi için net create_object spec (boyut, prompt yönü, kaç gen, tileable mi) — kullanıcı "üret" dediğinde hazır olsun. ÜRETME.

# Çıktı
Numaralı yanıt + sonda "ÇATLAK ÜRETİM TABLOSU (tip | px | varyant | tool | yerleşim-kuralı | layer)".
