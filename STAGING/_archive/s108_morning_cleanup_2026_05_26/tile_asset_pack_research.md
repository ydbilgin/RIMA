# Hazır Tile Asset Pack Araştırması — 2026-05-14

> Araştırma modeli: Gemini CLI (default model) + WebSearch doğrulaması
> RIMA hedef: F1 Shattered Keep biome, top-down 2D, 32px tile, PPU=64, procedural dungeon, URP 2D Lights

---

## En İyi 5 Öneri

### 1. Epic RPG World Pack — Crypt (RafaelMatos)
- **URL:** https://rafaelmatos.itch.io/epic-rpg-world-asset-pack-crypt
- **Koleksiyon (tüm packler):** https://rafaelmatos.itch.io/epic-rpg-world-collection
- **Fiyat:** Free (CC0 benzeri — kişisel ve ticari kullanım serbest, yeniden satış yasak)
- **Tile size:** 32x32
- **Wang/RuleTile:** Tiled Map Editor autotile schema dahil; 47-tile blob standardına yakın düzenleme → Unity RuleTile'a 1:1 map edilebilir
- **License:** Ücretsiz ticari kullanım, kredi gerekmez; NFT ve AI training yasak; redistribution yasak
- **RIMA fit:** YÜKSEK — 32px, stone/crypt aesthetic, atmosferik dark palette, Tiled autotile → Wang pipeline'ımıza doğrudan girer. Crypt + Old Prison pack kombinasyonu F1 Shattered Keep için ideal. Alabaster Dawn karanlık ton ile iyi örtüşüyor.
- **Ek packler:** Old Prison (https://rafaelmatos.itch.io/epic-rpg-world-pack-old-prison-asset-tileset), Ancient Ruins Free Demo (https://rafaelmatos.itch.io/epic-rpg-world-pack-free-demo-ancient-ruins-asset-tileset)
- **Uyarı:** Gemini "autotile Tiled format" dedi, Wang uyumluluğu pack sayfasında doğrulanmalı.

---

### 2. 16x16 DungeonTileset II (0x72)
- **URL:** https://0x72.itch.io/dungeontileset-ii
- **Sewers genişlemesi:** https://0x72.itch.io/16x16-dungeontileset-ii-sewers
- **Fiyat:** Free
- **Tile size:** 16x16 (RIMA'da PPU=32 ile kullanılabilir)
- **Wang/RuleTile:** Autotile-ready wall versiyonları mevcut (tall/short); CC0 community'de Unity RuleTile setup örnekleri çok; aktif geliştirme
- **License:** CC0 — tam ticari özgürlük, attribution gerekmez
- **RIMA fit:** ORTA-YÜKSEK — 16px sınırlaması var (PPU=32 gerekir, RIMA şu an PPU=64@32px), koyu dungeon aesthetic mükemmel, community'de Unity setup rehberleri bol. Ancak boyut uyumsuzluğu ciddi: 16px için PPU=32 yaparsak mevcut asset pipeline'ı (128px chibi, 32px tile) karışır.
- **Neden listeye aldım:** Roguelite community'nin en yaygın referans noktası; community tarafından Unity RuleTile setup'ları mevcut; CC0 lisans = sıfır risk.

---

### 3. Rogue Fantasy Catacombs (Szadi art.)
- **URL:** https://szadiart.itch.io/rogue-fantasy-catacombs
- **Fiyat:** Free / Name Your Price
- **Tile size:** 16x16 (bazı versiyonlarda upscale sprite sheets mevcut)
- **Wang/RuleTile:** Modüler tile chunk'lar, Wang/blob logic için tasarlanmış; 47-tile bitmask için uygundur
- **License:** Public domain (kişisel + ticari serbest; yeniden satış yasak)
- **RIMA fit:** ORTA — 16px sorun ayni (yukarıda açıklandı). Catacomb aesthetic F1'e uygun, high contrast palette Alabaster Dawn'dan farklı ama URP shader ile tonlanabilir. itch.io roguelike tag en çok indirilen packler arasında.

---

### 4. Cainos — Pixel Art Top-Down Basic / Dungeon
- **URL (Asset Store):** https://assetstore.unity.com/packages/2d/environments/pixel-art-top-down-basic-cainos-187605
- **URL (itch.io):** https://cainos.itch.io/pixel-art-top-down-basic
- **Fiyat:** Ücretsiz (Basic pack); Platformer Dungeon pack ayrı (~$20 tahmin, doğrulama gerekli)
- **Tile size:** 32x32
- **Wang/RuleTile:** Unity Asset Store versiyonu pre-configured RuleTile + tile palette içeriyor (Gemini doğruladı, ancak Dungeon paketi sidescroller odaklı olabilir — top-down Basic kontrol edilmeli)
- **License:** Unity Asset Store Standard License — Steam release için geçerli ticari lisans
- **RIMA fit:** ORTA — Basic pack genel ortam (grass, stone), dungeon-specific content sınırlı olabilir. Unity native RuleTile konfigürasyonu en güçlü avantajı. Top-down dungeon özelleşmesi için yetersiz görünüyor; dark atmosphere için ek palette çalışması gerekir.
- **Uyarı:** "Pixel Art Platformer - Dungeon" (sidescroller) ile "Top Down Basic" birbirinden farklı pack; top-down dungeon kombinasyonu yok veya ayrı satın alım gerektirir.

---

### 5. 2D Top-Down Pixel Dungeon Pack (Mero Store Studios) — Unity Asset Store
- **URL:** https://assetstore.unity.com/packages/2d/environments/2d-top-down-pixel-dungeon-pack-336220
- **Fiyat:** Bilinmiyor (sayfada doğrulanmalı; Unity Asset Store 2024-2025 upload)
- **Tile size:** Belirtilmemiş (sayfada doğrulanmalı)
- **Wang/RuleTile:** Belirsiz — sayfada kontrol gerekli
- **License:** Unity Asset Store Standard License
- **RIMA fit:** BELIRSIZ — Yeni pack (ID 336220 → geç 2024/2025 yüklemesi), spesifik bilgiye ulaşılamadı. Top-down dungeon label doğru, ancak tile size ve autotile desteği sayfa ziyareti gerektiriyor.
- **Alternatif:** Dungeon Topdown Pixel Art (Pixel Hunter): https://assetstore.unity.com/packages/2d/environments/dungeon-topdown-pixel-art-313176

---

## RIMA için Tavsiye

**Birincil öneri: RafaelMatos — Epic RPG World Pack: Crypt + Old Prison**

Gerekçe:
1. 32x32 tile — RIMA spec ile tam uyumlu, PPU değişikliği gereksiz
2. Tiled autotile format mevcut, Wang RuleTile pipeline'ına (Karar #115/#119 AI ASCII Matrix Parser + Wang RuleTile) doğrudan entegre edilebilir
3. Ticari Steam lisansı: serbest kullanım, redistribution yasak ama kendi oyununda kullanmak serbest
4. Crypt + Old Prison kombinasyonu: charcoal floor + stone wall + ruined keep aesthetic = F1 Shattered Keep thematically mükemmel
5. Free pack → test edilip beğenilmezse sıfır maliyet

**İkincil öneri (eğer arama sonuçları daha iyi autotile desteği gösterirse): Cainos Top-Down Basic**
Unity-native RuleTile en büyük avantaj, ancak dungeon-specific atmosphere için extra palette/shader çalışması gerekir.

---

## Mevcut Sorunlar ve RIMA Spec Uyumu

| Sorun | Durum |
|---|---|
| 16px tile packs (0x72, Szadi) | PPU=32 gerektirir; RIMA pipeline'ı (PPU=64@32px) ile çakışır. Kullanılabilir ama PPU karışıklığı riski. |
| Wang autotile doğrulaması | RafaelMatos pack sayfası ziyaret edilerek autotile format (Tiled A4/B veya blob 47) kesin doğrulanmalı. |
| Dark atmosphere / Alabaster Dawn match | Hazır packler Alabaster Dawn paletini kullanmıyor. URP 2D Lights + custom colour grading ile tonlanabilir, ama manual palette override gerekir. |
| PixelLab gen yerini alabilir mi? | Kısmen. Floor tile ve stone wall için hazır pack kullanılabilir (placeholder / hızlı prototip). Scatter, prop, wall decal için PixelLab gen üstün kalır. Long-term: hybrid (hazır tile base + PixelLab atmosphere/scatter) önerilen strateji. |
| Karar #127 Stamp/Cluster Library | Hazır pack tile'ları stamp olarak kullanılabilir, ancak Szadi/0x72 16px boyutu sorun. RafaelMatos 32px stamplara uyumlu. |
| Karar #128 WangResolver | RafaelMatos Tiled autotile → RIMA AITilemapImporter'da blob bitmask doğrudan kullanılabilir. |

### Sonuç

PixelLab gen'in tamamen yerini almaz, ancak **F1 Shattered Keep prototip hızlandırması** için RafaelMatos Crypt pack + Cainos RuleTile config hibrit yaklaşımı makul. PixelLab scatter/prop/wall decal üretmeye devam; zemin ve duvar base tile için hazır pack placeholder olarak kullanılabilir. Albaster Dawn tonu için URP 2D Light + global colour adjustment shader gerekli.

---

## Kaynaklar

- https://rafaelmatos.itch.io/epic-rpg-world-asset-pack-crypt
- https://rafaelmatos.itch.io/epic-rpg-world-pack-old-prison-asset-tileset
- https://0x72.itch.io/dungeontileset-ii
- https://szadiart.itch.io/rogue-fantasy-catacombs
- https://assetstore.unity.com/packages/2d/environments/pixel-art-top-down-basic-cainos-187605
- https://assetstore.unity.com/packages/2d/environments/2d-top-down-pixel-dungeon-pack-336220
- https://assetstore.unity.com/packages/2d/environments/dungeon-topdown-pixel-art-313176
- https://itch.io/game-assets/tag-dungeon/tag-tileset
- https://itch.io/game-assets/tag-autotile/tag-tileset
