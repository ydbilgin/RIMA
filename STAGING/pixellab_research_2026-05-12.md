# PixelLab AI Araştırma Raporu — 2026-05-12
**Model:** gemini-3.1-pro-preview (default, 429 retry sonrası başarılı)
**Kapsam:** RIMA Pure 2D Top-Down, 64px chibi karakter, 32px tile, URP 2D Renderer

---

## PixelLab YouTube Son Tutorialları

**Kanal:** youtube.com/@PixelLab_AI

Son 6 ay içinde (Kasım 2025 – Mayıs 2026) tarih bazlı kesin liste elde edilemedi; Gemini grounding kaynakları aşağıdaki tutorial kategorilerini doğruladı:

| Konu | İçerik Özeti |
|------|-------------|
| **Character Creation – Concept to Sprite** | Referans görsel/sketch yükleyerek eşleşen pixel art karakter üretimi. 4 veya 8 yön rotasyonu otomatik oluşturma dahil. |
| **Animation to Animation Transfer (Motion Template)** | Bir sprite'tan hareket çıkarıp tamamen farklı bir karaktere uygulama — frame-frame çizim yapmadan animasyon aktarımı. |
| **Interior Map Building** | Ahşap zemin/iç mekan layout'u bloklama, ardından "Inpaint" ile eşleşen duvar ve kapı üretimi. Top-down RPG/roguelite için doğrudan geçerli. |

**NOT:** Belirli video URL'leri (doğrulanamayanlar) Gemini tarafından Vertex AI grounding kaynak etiketi olarak loglandı, tam permalink vermedi. Kanal sayfası (@PixelLab_AI) doğrudan kontrol edilmeli.

**CONFIDENCE:** MEDIUM — Kanal varlığı ve konu kategorileri doğrulandı; spesifik video URL'leri ve Nov-May tarih aralığı kesinleştirilemedi.

---

## Map Feature (create_map_object + create_topdown_tileset)

### create_topdown_tileset
- **Amaç:** Seamless Wang tileset üretimi. "Inner" terrain (örn. çimen) + "outer" terrain (örn. kir) tanımlanıyor, geçiş tile'ları otomatik oluşturuluyor.
- **Unity/Godot uyumu:** Üretilen tileset doğrudan Unity Tilemap'e aktarılabilir nitelikte (Wang tile compatible).
- **RIMA use case:** 32px floor tile varyantları için kullanılabilir — zemin-duvar geçiş tile seti üretimi.

### create_map_object
- **Amaç:** Şeffaf arkaplan ile tekil harita objeleri üretimi (ağaç, sandık, vb.).
- **Temel parametre:** `description` (obje tanımı) + `background_image` (varolan harita stili referansı).
- **Kritik özellik:** `background_image` parametresi, üretilen objenin mevcut haritanın renk paleti ve stiline kilitlenmesini sağlıyor — stil tutarsızlığı önleniyor.
- **RIMA use case:** Oda içi prop üretimi (sandık, varil, sütun vb.) için zemin tile'ı referans olarak verilmeli.

**Kaynak:** pixellab.ai (API docs)

---

## 8-Yön Üretim + Animasyon Pipeline (2026 Güncel)

### Mevcut Durum: animate_character MCP Yasağı

**MEVCUT RIMA KURALI KORUNUYOR.** 2026 itibariyle web üzerinde animate_character MCP'nin 4-frame limitinin veya VFX bug'ının düzeltildiğine dair resmi bir doğrulama bulunamadı.

| Yöntem | Durum | RIMA Kararı |
|--------|-------|-------------|
| `animate_character` MCP tool | Limit/bug durumu belirsiz (2026'da fix raporlanmadı) | **YASAK — korunuyor** |
| PixelLab Web App animasyonu | Tam kontrol, motion template, Edit Image Pro | **ZORUNLU** |

### Önerilen Pipeline (Web App)
1. `create_character` MCP → 8 yön sprite sheet üret
2. Web App → Motion Template ile animasyon aktar (farklı karakterden hareket al)
3. Web App → Edit Image Pro → silah attachment workflow
4. `select_object_frames` MCP → frame seçimi/ayıklama (bu tool YASAK DEĞİL)

**CONFIDENCE:** MEDIUM — Yasak kararı korunuyor; güncel fix bilgisi Gemini tarafından bulunamadı (veri yok = fix yok varsayımı güvenli).

---

## Reference-Based Variant Generation

### Doğru Tool: vary_object

**Endpoint:** `/v2/vary-object`

**Parametreler:**
- `image`: Referans sprite (örn. base villager)
- `description`: Yeni design tanımı (örn. "orc warrior, same size")
- `seed`: Sabit seed — proporsiyon ve perspektif kilitlemek için kritik

**Workflow: Orc from Villager Template**
1. Base 64px villager sprite'ı `vary_object`'e `image` olarak ver
2. `description`: tam stil/boyut tanımı + "same proportions, same pixel density, same top-down perspective"
3. `seed` değerini sabitle — varyant tutarlılığı için
4. `style_options` ile `outline`, `shading`, `color_palette` referans sprite'tan kilitle

### Stil Tutarlılığı Prompt Paterni
```
"[NEW_CHARACTER_TYPE], same canvas size [64x64], same top-down view angle,
same chibi proportions, same outline weight, same shading style,
color palette: [HEX LIST FROM REFERENCE]"
```

### Generate with Style (Pro) Kullanımı
- `style_reference_images`: Mevcut karakter asset'lerini referans ver (4-16 adet)
- `color_palette` string: Projenin sabit renk paletini yaz
- Üretilen asset'i `vary_object` pipeline'a besle → drift sıfırlanır

**CONFIDENCE:** HIGH — vary_object endpoint ve parametreler API docs'tan doğrulandı.

---

## 128px Mob Workflow (64px Karakter ile Consistency)

### Temel Sorun
- 64x64 canvas: maksimum 16 stil referans → kompleks stil öğrenme mükemmel
- 128x128 canvas: maksimum 4 stil referans → odak detay, drift riski yüksek

### Önerilen 3-Adım Pipeline

**Adım 1 — Stil Baseline Oluştur (64px)**
- 10-16 çeşitli 64px referans ile baseline üret
- En başarılı 64px asset'i "anchor" olarak seç

**Adım 2 — 128px'e Geç (Upscale veya Inpaint)**
- Seçenek A: PixelLab AI Upscaling → 64px anchor'ı 128x128'e çıkar
- Seçenek B: 128px canvas'ta Inpainting → 64px silüeti koruyarak detay ekle

**Adım 3 — Yeni 128px Mob Üretimi**
- `style_reference_images` olarak en iyi 64px asset'leri (veya upscale edilmişleri) ver — max 4 referans
- `style_guidance_weight`'i yüksek tut → model referansın shading mantığına bağlı kalır, "daha gerçekçi" stile kaymaz
- Prompt'ta pixel density ve outline weight açıkça belirt

**ÖNEMLİ:** 128px canvas'ta 4 referans limiti nedeniyle referans seçimi kritik. Stil anchor olarak en "saf" 64px asset'i (en az drift olan) seç.

**CONFIDENCE:** MEDIUM-HIGH — Çözüm mantıklı ve API parametrelerine dayalı; ancak 128px workflow tam tutarlılık garantisi resmi olarak doğrulanamadı.

---

## RIMA İçin Aksiyon Listesi

1. **YouTube kanalını doğrudan tara** — youtube.com/@PixelLab_AI → son 6 ay videolarını listele, "Motion Template" ve "Interior Map Building" tutorial'larını izle.

2. **create_map_object workflow'u test et** — Prop üretiminde `background_image` parametresine mevcut 32px floor tile referansını ver; stil tutarlılığını kontrol et.

3. **animate_character MCP yasağını koru** — 2026'da fix raporlanmadı. Web App animasyonu zorunlu kuralı değişmiyor.

4. **vary_object ile seed-locked variant pipeline kur** — Base chibi karakter sprite'ı referans olarak, seed sabit, `style_options` ile outline+shading kilitle. İlk mob varyantını bu pipeline ile üret.

5. **64px → 128px upscale testini yap** — En başarılı 64px chibi asset'i PixelLab AI Upscale ile 128px'e çıkar; sonucu mob referansı olarak kullan.

6. **128px boss üretiminde 4-referans limitini göz önünde tut** — Referans seçimini dikkatli yap: en iyi 4 64px asset, `style_guidance_weight` yüksek.

7. **create_topdown_tileset ile floor geçiş tile'larını üret** — Inner: dungeon stone, outer: void/wall, Wang tileset formatı; Unity Tilemap Rule Tile ile kullan.

8. **Motion Template workflow'u pilot et** — Bir karakterin animasyonunu ("idle walk attack") başka bir karakter üzerine Motion Template ile aktar; Web App üzerinden.

9. **MCP tool kullanım diyagramını güncelle** — `create_character`, `create_topdown_tileset`, `create_map_object`, `vary_object`, `select_object_frames` → MCP OK. `animate_character` → YASAK. Bu ayrımı RIMA pipeline doc'una yaz.

10. **PixelLab Discord'u kontrol et** — animate_character bug fix veya 128px workflow resmi guide için Discord'da arama yap (Gemini bu kaynağa ulaşamadı).

---

## Araştırma Metadata

| Parametre | Değer |
|-----------|-------|
| Model | gemini-3.1-pro-preview (default) |
| Tarih | 2026-05-12 |
| HTTP 429 | Evet (retry sonrası başarılı) |
| Güven Skoru Genel | MEDIUM (YouTube video URL'leri kesinleşmedi, API parametreleri HIGH) |
| Bulunamayan Veri | animate_character 2026 fix durumu, spesifik video permalink'leri |
| Önerilen Ek Araştırma | PixelLab Discord, pixellab.ai changelog/release notes |
