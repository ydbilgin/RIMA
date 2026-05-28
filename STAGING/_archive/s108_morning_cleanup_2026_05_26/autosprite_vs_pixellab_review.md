# autosprite.io vs PixelLab — RIMA Değerlendirme

_Araştırma tarihi: Mayıs 2026 | Model: gemini-3.1-pro-preview (default)_

---

## TLDR (3 satır)

autosprite.io, tile üretiminde Codex imagegen'in yerini alabilecek güçlü bir seam-tiling editörü ve palette-lock özelliğiyle dikkat çekiyor. Karakter tarafında PixelLab'ın Create Image Pro kalitesine ve Custom Animation V3'ün state granülaritesine ulaşamıyor. **Öneri: Free plan üzerinden 5 adet 32×32 painterly floor tile üretip Codex imagegen çıktısıyla karşılaştır; sonuç iyiyse tile pipeline'ını shift et.**

---

## Ürün Özellikleri (autosprite.io)

| Özellik | Detay |
|---|---|
| Asset tipleri | Karakter, tile/tileset, animasyon, VFX (patlama, fireball), prop, UI elementi |
| Output formatları | Şeffaf PNG sprite sheet, JSON atlas dosyası, GIF |
| Çözünürlük seçenekleri | 32px – 512px arası ayarlanabilir frame boyutu |
| Kontrol parametreleri | Metin prompt, stil referans görseli yükleme, 20+ preset animasyon (idle/walk/run/attack), piksel boyutu, dithering kontrolü |
| Seamless tile desteği | EVET — dedicated Seamless Pixel Art Tile Editor; GEGL-based algoritma, X/Y seam offset slider, canlı 6×6 grid preview |
| Top-down 2D perspektif | EVET — Zelda-stili top-down perspective açıkça destekleniyor |
| Şeffaf PNG / alpha channel | EVET — "Corridor Key" teknolojisi ile soft edge transparency korunuyor |
| API / MCP entegrasyonu | REST API v1 (Pro/Enterprise planlarda); MCP (Model Context Protocol) desteği MEVCUT — Claude, Cursor, Gemini CLI entegrasyonu belgelenmiş; endpoint yüzey alanı PixelLab'dan küçük |
| Üretim hızı | Animasyon loop başına ~30-90 saniye; tam karakter sheet (çoklu anim) 2-5 dakika |

---

## Pricing Tablosu (2025–2026)

| Plan | Aylık Ücret | Gen Quota | Watermark | Ticari Kullanım | Notlar |
|---|---|---|---|---|---|
| Free | $0 | 15-20 kredi (aylık reset); 2 standard-res unlock/ay | **YOK** | **EVET** | Deneme için yeterli |
| Starter | $12/ay | 500 kredi | YOK | EVET | Unlimited high-res unlock, premium background remover |
| Pro | $29/ay | 1.500 kredi (2x rollover) | YOK | EVET | API erişimi, batch gen; PixelLab ile aynı fiyat bandı |
| Enterprise | $99/ay | Özel | YOK | EVET | Team management, custom credits |

_Not: One-time purchase seçeneği YOK, sadece subscription._

---

## Karşılaştırma Matrisi

| Özellik | PixelLab | autosprite.io | Kazanan |
|---|---|---|---|
| Karakter base generation kalitesi | Create Image Pro V3 — yüksek baseline kalite, yapısal doğruluk | Preset-based — iyi ama PixelLab kadar granüler değil | **PixelLab** |
| Animasyon frame tutarlılığı | Custom Animation V3, state-based workflow, manuel kontrol | Preset animasyonlar frame-to-frame tutarlılığında güçlü (preset sınırları dahilinde) | **Berabere** (PixelLab granüler, AutoSprite hızlı) |
| Pose / state granülaritesi | Skeleton-based; tam kontrol — sword swing yönü vs. gibi | Black-box preset — AI versiyonu, override zor | **PixelLab** |
| Seamless tile üretimi | Düz raw görsel; seam matematiksel garantisi YOK | Dedicated seam editor, GEGL algoritma, mathematical edge-blending | **autosprite.io** |
| Painterly stil desteği | Prompt-based, style image upload, güçlü | Prompt + style ref upload; painterly mode mevcut | Berabere |
| Palette lock / color snapping | Sınırlı; prompt-level kontrol | "Palette-Safe" — hex-level renk zorlama | **autosprite.io** |
| VFX / efekt sprite üretimi | Create Image Pro ile dolaylı üretim | Dedicated VFX presets (patlama, fireball) | **autosprite.io** |
| API / MCP derinliği | 32 endpoint; tam programatik Unity workflow | V1 REST API + temel MCP; bulk gen odaklı | **PixelLab** |
| 8-directional karakter control | Create Character — full 8-dir rotasyon | Desteklemiyor veya sınırlı | **PixelLab** |
| Playable preview (web içi test) | YOK | Built-in web engine — karakter download öncesi test edilebiliyor | **autosprite.io** |
| Free tier ticari kullanım | Evet (kredi bazlı sınır) | Evet (watermark yok, ticari izin var) | Berabere |

---

## RIMA için 3-5 Use Case

**1. 32×32 Seamless Floor Tile Replacement (Codex imagegen → autosprite.io)**
- Şu an: Codex imagegen (gpt-image-1 backend) ile painterly floor/wall tile üretimi — seam matematiksel garanti YOK, post-process gerekiyor
- autosprite.io free plan'da 5 tile gen yap, Codex çıktısıyla seam kalitesini karşılaştır
- Beklenen tasarruf: Tile başına manuel seam-fix zamanı (~10-20 dk/tile) sıfıra iniyor; Karar #157 hybrid pipeline'da tile rolü autosprite'a kayabilir

**2. VFX Sprite Sheet Üretimi (yeni pipeline)**
- RIMA'da 64-128px VFX (slash, spark, burst, cold blue/void purple) şu an ayrı üretim gerektiriyor
- autosprite.io dedicated VFX presets + şeffaf PNG sprite sheet çıktısı — doğrudan Unity'e import edilebilir
- Beklenen tasarruf: VFX per-effect üretim süreci %60-70 kısalabilir

**3. Statik PixelLab Karakterden Hızlı Animasyon Sheet Üretimi**
- PixelLab ile üretilen base karakter statik PNG'sini autosprite'a ver
- Bir tıkla idle/walk/run/attack preset set üret; JSON atlas ile Unity Sprite Animator direkt kurulumu
- Beklenen tasarruf: PixelLab'ın Custom Animation V3 slot maliyetini ve süresini azaltır; Class skin variant animasyonları için ekonomik

**4. Palette-Safe Tile Varyantları (biome tutarlılığı)**
- RIMA biome-aware tile sistemi (Karar #143 6-katmanlı pipeline) için renk tutarlılığı kritik
- autosprite.io'nun hex-level palette lock özelliği ile biome renk paleti enforced olarak tile varyantı üretilebilir
- Beklenen tasarruf: Biome renk kayması QC roundtrip'leri azalır

**5. Düşman/Mob Animasyon Hızlandırma**
- 8 mob (Imp to Hulk) sprite set için animasyon üretimi — custom state workflow yerine preset ile başlangıç
- autosprite preset animasyonları mob için yeterli; player character'dan farklı olarak pose granülaritesi daha az kritik

---

## Free Plan Deneme Önerisi

**EVET — öncelik: tile kalite karşılaştırması.**

Ne denenecek:
1. 5 adet 32×32 painterly stone floor tile üret (prompt: "top-down stone floor tile, painterly brushwork, warm amber tones, seamless, Hades reference")
2. Aynı prompt'u Codex imagegen (gpt-image-1) ile çalıştır
3. Her iki çıktıyı Unity'de 4×4 tile grid'de yay, seam görünürlüğü ve painterly kaliteyi karşılaştır
4. Sonuç iyiyse Karar #157 pipeline review için orchestrator'a raporla

Maliyet: $0 (free plan 15-20 kredi; 5 tile ~5-10 kredi kullanır)

---

## Risk + Sınır

- **Stil tutarsızlığı riski:** autosprite'ın kendi karakter base generation kalitesi PixelLab seviyesinde değil; karakter üretiminde ana tool olarak kullanmak RIMA karakter tutarlılığını bozabilir. Tile/VFX/animasyon-assist rolleri daha güvenli.
- **8-dir karakter desteği belirsizliği:** RIMA'nın 8-directional karakter üretim ihtiyacı (5 produce + 3 mirror) için autosprite'ın desteği confirm edilmemiş — Gemini "sınırlı" flagledi (MEDIUM CONFIDENCE).
- **API derinliği sınırlı:** PixelLab'ın 32 MCP endpoint'ine kıyasla autosprite V1 API bulk-gen odaklı; Unity MCP workflow entegrasyonu daha kısıtlı. Codex dispatch pipeline'a entegrasyon PixelLab kadar kolay olmayabilir.
- **Frame flickering:** Reddit raporlarına göre animasyon loop'larında 1-2 frame hallucination (örn. silah kayboluyor) görülebiliyor; Aseprite'da manuel touch-up gerektirebilir.
- **Royalty-free netlik:** Free plan commercial use "EVET" belirtilmiş ancak Terms of Service detay incelemesi önerilir — AI-generated asset ownership ToS değişkenlik gösterebilir.
- **Subscription-only:** One-time purchase yok; aylık devam gerektiriyor.

---

## Sources

Gemini araştırması kaynak URL verme kapasitesi değişkendir; aşağıdakiler Gemini'nin referans verdiği veya araştırma sırasında tespit edilen kaynaklar:

- https://autosprite.io (resmi site)
- https://autosprite.io/docs/api (V1 REST API dökümantasyonu)
- Reddit r/aigamedev — autosprite V2 community thread (Mayıs 2025-2026)
- https://itch.io forums — indie dev AI art tool comparison threads
- PixelLab ToS karşılaştırma için: https://pixellab.ai/termsofservice
- GEGL seamless algorithm documentation (referans: GIMP/GEGL docs)

_CONFIDENCE notu: Pricing rakamları ve feature listesi HIGH CONFIDENCE (community + docs cross-check). 8-dir desteği ve MCP endpoint derinliği MEDIUM CONFIDENCE (Gemini "sınırlı" flagledi, doğrudan API doc incelemesi önerilir). Painterly kalite değerlendirmesi MEDIUM CONFIDENCE (subjektif, free plan deneme ile doğrula)._
