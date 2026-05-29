ACTIVE RULES: (1) think before answering (2) concrete, min-fluff (3) stay in scope — cliff naturalness only (4) if a claim is uncertain, flag it.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
RESPOND INLINE in your transcript, NOT to a file.

# Amaç
RIMA'nın yüzen-ada (floating island) arena KENARININ "doğal" görünmesini sağlamak. Kullanıcı (sanat gözü keskin) mevcut cliff'i defalarca reddetti; en son "böyle de doğal durmadı" dedi. Concrete, Unity'de uygulanabilir teknikler istiyorum — impact/effort sıralı.

# Sahne / teknik kurulum (DEĞİŞMEZ kısıtlar)
- Unity URP 2D Renderer + Pixel Perfect Camera + 2D Lights. **Yüksek top-down 3/4 (~70-80°, Hades / Children of Morta / Diablo III bakışı).** 3D YOK, gerçek 45° iso math YOK.
- Tilemap: **Isometric (diamond)**, cellSize (1, 0.61, 1), **PPU 64**.
- Floor = opak diamond tile (tile_2 vb.), Floor sorting layer.
- Marka rengi cyan #00FFCC (rim light + rift). Brazier'lar sıcak turuncu. Global light 0.55 loş.
- **Shadowcaster2D YASAK** (geçmişte siyah-cliff felaketi). Anim 8-yön kod (bake yok). 2D pixel art.

# Cliff'in MEVCUT durumu (bu session düzelttiğim)
- Cliff = `DirectionalCliffTile` (TileBase SO). Yerleşim: floor cell'in S/SE/SW komşusu boşsa o floor cell'in KENDİSİNE konur (sadece dış-çevre, kuzey kenarlara konmaz).
- Cliff sprite `cliff_S`: 128x192 px, **pivot top-center (64,192)**, yani hücre merkezinden AŞAĞI sarkar (~3 birim). Görsel: kalın kaya sütunu, üstü kaya kapağı, altı molozlu/damlalı.
- **Sorting: cliff floor'un ALTINDA** (Ground#100 < Floor) → floor cliff'in üstünü ÖRTER, sadece ada kenarından void'e sarkan kısım görünür. transformOffset=(0,0,0).
- Şu an TEK varyant (cliff_S). 5 varyant vardı (2 kalın-sütun + 3 sivri-diş stili) ama "2 ayrı tür gibi" diye tek'e indirildi.
- Cliff Ground layer'da ve tüm Light2D'ler Ground'u aydınlatıyor (cyan rim + brazier + global) → cliff aydınlanıyor.
- Floor kenarı görünmez collider ile çevrili (oyuncu void'e düşmez).

# Çözülen ama YETMEYEN (kullanıcı hâlâ "doğal değil" diyor)
Önceki "floor üstünde dikilen kahverengi kuleler" sorunu sorting fix ile çözüldü. Şimdi cliff floor'un altından sarkıyor AMA:
- **Tek sprite tekrarı → "wallpaper" / tekrarlı perde gibi**, organik değil.
- **Düz 2D perde gibi düşey sarkıyor** — 3/4 iso'da doğal kaya yüzü perspektif/çekilme ister.
- Floor-üst ile cliff-yüz arasında **geçiş/gölge yok** → kenar aniden başlıyor.
- Sarkma **tek-tip uzunlukta**, düzensiz silüet yok.

# Kullanıcının EK isteği
Floor'da **kasıtlı boşluklar** (gaps) olsun, içinden **derinlik** görünsün (void'e bak). Boşluğun uzak/kuzey kenarına iç kaya yüzü sarkıyor — bu çalışıyor ama o da doğal görünmeli.

# SORU (concrete cevap istiyorum)
3/4 iso, PPU 64, 2D pixel, Shadowcaster YASAK kısıtlarıyla, bu cliff KENARINI doğal göstermek için:
1. **Sprite/asset** seviyesinde ne yapmalı? (cliff yüzü tasarımı, kapak vs yüz ayrımı, dual-grid/Wang edge-tile yaklaşımı buna değer mi, sarkma uzunluğu)
2. **Yerleşim algoritması** seviyesinde? (per-cell yükseklik varyasyonu, silüet kırma, köşe işleme, kaç varyant ve nasıl coherent tutulur)
3. **Render/gölge/ışık** seviyesinde? (Shadowcaster olmadan AO/gölge bandı nasıl yapılır — ayrı dark-strip tile? gradient sprite? cyan rim'i tepeye nasıl odaklarız? sorting/offset ince ayar)
4. Bu listede **en yüksek impact / en düşük effort** olan İLK 3 adım hangisi?

Kısa, madde-madde, RIMA'da bu hafta uygulanabilir somut adımlar. Teorik sanat dersi değil.
