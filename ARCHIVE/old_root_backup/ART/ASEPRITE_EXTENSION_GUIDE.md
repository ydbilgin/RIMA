# ASEPRITE_EXTENSION_GUIDE.md
> **Ne zaman yükle:** Aseprite kullanırken — tıklama bazlı rehber.
> ⚠️ Bazı adımlar Aseprite versiyon ve extension versiyonuna göre farklılık gösterebilir. Emin olmadığım yerlerde en güvenli yolu öneriyorum, not düştüm.

---

## REHBER 1 — EXTENSION KURULUMU

**Amaç:** Aseprite'a PixelLab extension'ını yükle.

```
STEP 1: pixellab.ai adresine git → "Download" veya "Get Extension" butonuna tıkla
STEP 2: İndirilen .aseprite-extension dosyasını bul (genelde Downloads klasörü)
STEP 3: Aseprite açık olmalı
STEP 4: İndirilen dosyayı Aseprite penceresine sürükle ve bırak
STEP 5: Çıkan "Install Extension?" diyaloğunda "Install" tıkla
STEP 6: Aseprite'ı kapat ve yeniden aç
STEP 7: Edit → Preferences → Extensions listesinde "PixelLab" görünüyor mu?
         EVET → Kurulum tamam
         HAYIR → Adımları tekrarla veya pixellab.ai support sayfasına bak

⚠️ NOT: Extension bazen Edit menüsünde değil, ayrı bir "Plugins" veya
"Scripts" menüsünde çıkabilir. Emin değilsem menülere tek tek bak.
```

---

## REHBER 2 — EXTENSION AÇMA

**Amaç:** PixelLab panelini Aseprite'ta açmak.

```
STEP 1: Aseprite'ı aç
STEP 2: Edit menüsüne tıkla
STEP 3: "PixelLab" seçeneğini bul ve tıkla
         BULAMAZSAN: Window menüsüne bak, veya toolbar'da ikon var mı kontrol et
STEP 4: Sağda veya ayrı pencerede PixelLab paneli açılır
STEP 5: API key alanı boşsa: pixellab.ai → hesabına gir → API key kopyala → yapıştır
         Beklenen görünüm: "Description" alanı, "Generate" butonu, stil seçenekleri
```

---

## REHBER 3 — BASE CHARACTER ÜRETİM (Aseprite içi extension ile)

> ⚠️ Bu workflow MCP'ye göre daha yavaş ve daha az otomatik. Küçük varyasyon veya
> referans üretmek için kullan. Büyük üretimi MCP ile yap.

```
STEP 1: Aseprite → File → New
STEP 2: Width: 96, Height: 96, Color Mode: RGB, Background: Transparent → OK
STEP 3: Katman adını "BASE" yap (Layers panelinde çift tıkla → yeniden adlandır)
STEP 4: Edit → PixelLab → Open (panel açılır)
STEP 5: "Description" kutusuna STYLE_BIBLE.md'deki karakter açıklamasını yapıştır
STEP 6: View: "Low top-down" seç
STEP 7: Outline: "Single color black outline" seç
STEP 8: Detail: "High detail" seç
STEP 9: "Generate" butonuna bas — 30-60 saniye bekle
STEP 10: Sonuç geldi:
         İYİ → File → Export As → [isim]_south_candidate_base_v1.png olarak kaydet
         KÖTÜ → Ctrl+Z ile geri al → Description'ı düzenle → tekrar Generate
```

---

## REHBER 4 — WORKING COPY OLUŞTURMA (Base'i Koruma)

**Amaç:** Approved base üzerine doğrudan yazma, her zaman kopya üzerinde çalış.

```
STEP 1: Approved base dosyasını bul (örn: warblade_south_approved_base_v1.png)
STEP 2: Bu dosyaya DOKUNMA — sadece kopyasını al
STEP 3: Windows'ta: dosyaya sağ tıkla → Kopyala → Aynı klasörde Yapıştır
         Dosya adını değiştir: warblade_south_approved_base_v1_WORKING.png
STEP 4: Aseprite → File → Open → WORKING kopyayı aç
STEP 5: Tüm editler sadece WORKING dosyada yapılır
STEP 6: Edit bittikten sonra: File → Export As → [isim]_EXPORT.png
         (WORKING dosyayı kaydet, ORIGINAL_BASE'e dokunma)
```

---

## REHBER 5 — ROUGH FRAME CLEANUP

**Amaç:** MCP'den gelen sprite'taki artifact piksellerini temizle.

```
STEP 1: Working copy'yi Aseprite'ta aç
STEP 2: View → Zoom In (Ctrl++) → en az 8x zoom — tek tek pikseller görünmeli
STEP 3: Eraser tool seç (E tuşu)
         Toolbar'da üstten boyut: "1px" seç (önemli — geniş silgi yanlışlıkla fazla siler)
STEP 4: Artifact olan piksel(leri) bul:
         - Karakterin dışına taşan yalnız piksel
         - Şeffaf olması gereken yerde renkli piksel
         - Outline dışına çıkmış renk kırıntısı
STEP 5: Artifact piksel üzerine tıkla → silindi
STEP 6: File → Export As → [isim]_EXPORT.png
STEP 7: Sonuç hâlâ kötüyse bana bildir — daha derin cleanup veya yeniden üretim kararı verelim

⚠️ NOT: Cleanup sırasında "asıl pikseli" silme riski var.
Her adımdan sonra: Ctrl+Z hazır tut. Büyük cleanup öncesi ekstra BACKUP al.
```

---

## REHBER 6 — RİSKLİ EDİT ÖNCESİ BACKUP

**Amaç:** Palette swap, merge, resize veya herhangi destructive işlem öncesi güvenlik.

```
STEP 1: Aseprite'ta dosyayı aç
STEP 2: File → Save As → [mevcut_dosya_adı]_BACKUP_[tarih].png
         Örnek: warblade_south_approved_base_v1_BACKUP_20260402.png
STEP 3: BACKUP kaydedildi → artık edit yapabilirsin
STEP 4: Edit işlemini yap
STEP 5: İyi sonuç → File → Export As → EXPORT kopyasını kaydet
         Kötü sonuç → BACKUP dosyasını aç, başa dön

⚠️ NOT: Aseprite'ın kendi "undo history" (Ctrl+Z) session kapatılınca sıfırlanır.
BACKUP almadan session kapatırsan geri dönemezsin.
```

---

## REHBER 7 — PALETİ SABİTLEME

**Amaç:** İki farklı karakterin renklerini tutarlı hale getir.

```
STEP 1: Onaylı ilk karakteri aç (warblade_south_approved_base_v1.png)
STEP 2: Window → Palettes → sol altta palette panel görünür
STEP 3: Palette panel sağ üstünde küçük ⋮ veya dropdown → "Save Palette"
STEP 4: İsim: "RIMA_Act1" → kaydet
STEP 5: Yeni karakteri aç (ikinci karakter)
STEP 6: Window → Palettes → aynı dropdown → "Load Palette" → RIMA_Act1
STEP 7: Edit → Replace Color ile yeni karakterin arka fon / outline renklerini
         palette'ten seç ve uygula

⚠️ NOT: "Replace Color" tüm katmanlarda çalışır — dikkatli ol.
Önce test için tek katmanda dene, sonra tümüne uygula.
```

---

## REHBER 8 — SEAMLESs TİLE KONTROLÜ

**Amaç:** Tile'ın kenarlarda birleşip birleşmediğini kontrol et.

```
STEP 1: Tile PNG'yi Aseprite'ta aç (örn: floor_stone_16x16.png)
STEP 2: Edit → Canvas Size → Width: 48, Height: 48, Anchor: üst-sol köşe → OK
         (Orijinal 16x16 → 48x48 canvas, sağ/alt alanda boş alan oluştu)
STEP 3: Layer → Duplicate Layer (3 kez → 4 layer olsun)
STEP 4: Layer 2 → Layer Properties → Offset X: 16, Y: 0
         Layer 3 → Offset X: 0, Y: 16
         Layer 4 → Offset X: 16, Y: 16
         (4 tile yan yana ve üst üste oluştu)
STEP 5: Kenarlara bak: görünür çizgi veya renk farkı var mı?
         YOK → Tile seamless ✓
         VAR → Pencil tool (B) ile kenarlara elle renk düzelt, tekrar kontrol et

⚠️ NOT: Layer offset'ini Layer Properties'ten değil yanlışlıkla Move tool ile
değiştirirsen diğer layerlar da kayabilir. Her layer'ı ayrı ayrı seç.
```

---

## REHBER 9 — ANİMASYON İÇİN SPRITE HAZIRLAMA

**Amaç:** MCP animasyon öncesi veya sonrası sprite'ı doğru formata getir.

```
STEP 1: Approved base PNG'yi aç (warblade_south_approved_base_v1.png)
STEP 2: BACKUP al (Rehber 6)
STEP 3: Canvas boyutunu kontrol et:
         PixelLab karakterde canvas sprite'tan ~%40 büyük gelir (96px karakter → ~134px canvas)
         Bu normal — Unity'de sprite bounds ile trim edilecek
STEP 4: Sprite tek katmanda mı? (Layers panelini kontrol et)
         Birden fazla katman varsa: Layer → Flatten Image (tüm layerları birleştir)
STEP 5: File → Export As → [isim]_anim_ready_v1.png
         Bu dosya MCP animate_character'a referans olarak kullanılabilir
         Veya Aseprite'ta animasyon yapacaksan bu dosyayı base frame olarak kullan
```

---

## REHBER 10 — SPRITE SHEET EXPORT (Unity için)

**Amaç:** Animasyon frame'lerini Unity'nin anlayacağı sprite sheet olarak çıkart.

```
STEP 1: Animasyon dosyasını Aseprite'ta aç (tüm frame'ler Timeline'da görünmeli)
STEP 2: File → Export Sprite Sheet
STEP 3: Açılan pencerede:
         Layout → "Horizontal Strip" (frame'ler yan yana)
         ☑ Trim Cels (boşlukları kes)
         ☑ Merge Duplicates (tekrar frame'leri birleştir)
         Columns: 0 (otomatik hesaplasın)
STEP 4: Output bölümü:
         ☑ PNG → dosya adı: [isim]_[animasyon]_sheet_v1.png
         ☐ JSON (Unity Animator için gerek yok, manuel slice yapacağız)
STEP 5: Export butonuna bas
STEP 6: Unity'de: Sprite Mode → Multiple → Slice → By Cell Size → frame boyutunu gir

⚠️ NOT: "Trim Cels" bazen frame'lerin boyutunu değiştirir.
Unity'de tutarsız boyut sorunları yaşarsan Trim'i kapat, sabit canvas boyutunda export et.
```

---

## REHBER 11 — APPROVED BASE'DEN DERIVED VARIANT

**Amaç:** Onaylı base'den renk varyantı veya enemy tier varyantı üret (Elite, Champion).

```
STEP 1: approved_base dosyasını kopyala (BACKUP değil, derived_variant klasörüne kopyala)
         warblade_south_approved_base_v1.png → derived_variant/warblade_elite_v1.png
STEP 2: Kopyayı Aseprite'ta aç
STEP 3: Edit → Replace Color
         Source color: orijinal armor rengi (pipette ile seç)
         Target color: Elite rengi (örn: #FF6600 turuncu)
         Tolerance: 20-30 arası başla
STEP 4: Önizleme → iyi görünüyor → OK
         Kötü görünüyor → Cancel → Tolerance'ı ayarla
STEP 5: File → Export As → derived_variant/warblade_elite_v1.png
STEP 6: ASSET_LOG'a yaz: "derived from: approved_base_v1, method: palette swap, target: Elite tier"

⚠️ NOT: Replace Color outline rengini de etkileyebilir. Outline'ı korumak istiyorsan
önce outline layer'ını kilitle (Layers panelinde kilit ikonu) — bu Aseprite'ta varsa.
Yoksa outline renklerini elle geri düzelt.
```