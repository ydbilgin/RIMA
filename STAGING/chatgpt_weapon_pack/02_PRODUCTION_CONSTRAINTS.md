# Üretim Altyapısı Gerçekleri + Çözülecek Çelişki

## A) CANLI ÇALIŞAN ÖRNEK (yegâne ground truth)
`04_ASSETS/Warblade_Greatsword.png` — oyunda uçtan uca çalışan TEK silah:
- **64×16 px** PNG (yatay çizim, uç SAĞA bakar, sap solda)
- Unity import: Point filter, **PPU 64**, alignment=Custom, **spritePivot (0.18, 0.5)** = sap noktası
- `HandAnchorAttach.AttachWeapon()` ile karakterin handAnchor'ına takılır; `OrientationSync` yön başına `weaponRotations[8]` + `handOffsets[8]` uygular; swing animasyonu prosedürel (alpha-0.4 fade'li yay)
- Karakter: 120×120 canvas, çizim ~64px efektif, ayak-pivot, PPU 64

## B) ⚠️ ÇÖZÜLECEK BOYUT ÇELİŞKİSİ (ana sorulardan biri)
İki farklı yaklaşım kayıtlarda yan yana duruyor:
1. **Kanon tablo (01_CANON_WEAPONS.md):** büyük canvas üret (örn. Ranger yayı 128×128) → Unity'de final ölçeğe küçült (64×64). Gerekçe: detay payı.
2. **Canlı pratik + lean batch planı:** HEDEF boyutta direkt üret (greatsword 64×16 — 96×96 bile değil!). Gerekçe: pixel-perfect üretim, downscale artefaktı yok, PixelLab zaten native piksel üretiyor.
Senden net karar + gerekçe istiyoruz. Not: pixel-art'ta downscale (128→64) genellikle piksel grid'ini bozar; PixelLab native ürettiği için "büyük üret küçült" mantığı şüpheli — ama kanon tabloda yazıyor.

## C) PixelLab API gerçekleri (2026-06-07 doğrulanmış, openapi.json + llms.txt)
- Araç: **`POST /v2/create-1-direction-object`** (tek açı obje). Karakter aracı ve map-object aracı silah için uygun değil. Ayrı "asset pack" endpoint'i YOK; batch = `item_descriptions[]` parametresi.
- **`size` ve `style_images` BİRLİKTE VERİLEMEZ.** style_images verilince çıktı boyutunu EN BÜYÜK referans görseli belirler.
- Aday limitleri (style_images İLE): ≤85px → **8 aday/istek**, ≤170px → 4, üstü → 1. (style_images OLMADAN: ≤42px→64, ≤85px→16, ≤170px→4.)
- style_images: her biri base64 PNG, max 256×256; ideal 1-3 yüksek kaliteli ref (mevcut greatsword + sınıf karakteri downscale'i = hibrit ref stratejimiz).
- `palette_lock`/`ai_freedom` parametreleri bu endpoint'te YOK.
- Çıktı: sadece transparan PNG. **Pivot/grip metadata GELMEZ** — pivot Unity Sprite Editor'da elle set edilir.
- İmagegen alternatifi (Gemini/GPT görüntü modelleri + downscale/quantize) DEĞERLENDİRİLDİ ve REDDEDİLDİ: 32-64px'te piksel grid tutarsızlığı + anti-alias lekesi; temizlik maliyeti sıfırdan üretimden pahalı. PixelLab native piksel üretimi net kazanan.

## D) Çizim açısı — ikinci açık karar
- Canlı greatsword: YATAY, uç sağa (0° = doğu) — OrientationSync rotasyonları buna göre ayarlı.
- Dış araştırma önerisi: DİKEY (sap altta, uç yukarıda) veya 45° diyagonal — rotasyon matematiği için.
- Senden karar: yeni silahlar hangi açıyla çizilmeli? (Mevcut canlı konvansiyon = yatay; değiştirmek OrientationSync rotations dizisinin yeniden ayarını gerektirir — anchor tuning tool'u var, maliyet düşük ama sıfır değil.)

## E) Attach mimarisi özeti (kod 03_CODE'da)
- Karakter prefab'ında `handAnchor` Transform (el hizasında child)
- `WeaponDatabaseSO` → sınıf→silah prefab eşlemesi; silah prefab'ı = SpriteRenderer'lı basit GO
- `HandAnchorAttach` spawn'da silahı anchor'a child'lar → `OrientationSync.SetWeapon()` 
- `OrientationSync`: 8 yön için `handOffsets[8]` (Vector2) + `weaponRotations[8]` (derece) + flipY + sorting order senkronu; prosedürel swing base rotasyonun ÜSTÜNE biner
- `OrientationSyncAnchorEditor`: SceneView'da yön seçip marker sürükleyerek offset tuning — yeni silah eklemenin son adımı bu tool'la kalibrasyon
- İlk üretim partisi: Ranger yayı → Shadowblade hançer (tek sprite, off-hand runtime flipX) → Elementalist rün diski (attach DEĞİL — avuç üstü hover, muhtemelen ayrı küçük script: bob + spin)

## Beklenen çıktı formatı (senden)
1. **Karar bloğu:** boyut stratejisi (B) + çizim açısı (D) — net seçim + 2-3 cümle gerekçe
2. **Silah spec tablosu (10 satır):** sınıf · sprite boyutu (px, kesin) · form/silüet tarifi · grip noktası (sprite üzerinde nerede) · özel not (mirror/hover/baked)
3. **PixelLab batch planı:** batch listesi (≤8 item/batch, boyut karıştırma yok) + her item için İngilizce `description` metni (style: dark gritty chibi pixel art, transparan bg)
4. **Ele yerleştirme rehberi:** yön-bazlı davranış özeti + özel durumlar (disk hover parametreleri, bow sol-el, dual-weapon flipX, Ronin kın çözümü: gövdeye mi baked ayrı sprite mı — ÖNERİ ver)
5. **Riskler:** gözden kaçan tutarsızlık/çelişki görürsen açıkça yaz
