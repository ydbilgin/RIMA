# L3 Parallax (Cliff-Altı Ambiyans) — Design Karar (Opus synthesis)

Tarih: 2026-05-28 · Karar veren: rima-design (Opus) · Süreç: Opus + mevcut Codex parallax review (`STAGING/PARALLAX_REVIEW_CODEX.md`) + dokümante game research (`STAGING/LAURETH_2D_ILLUSION_LIBRARY.md`) sentezi. Live agy/Codex dispatch bu çağrıda file-only interface nedeniyle atlandı; dokümante araştırma + kod kanıtı belirleyici.

> SINGLE LIVE. F3 6-katman canonical (`STAGING/F3_PARALLAX_6LAYER_DONE.md`) ile UYUMLU — onu ezmez, L3'ün altına dipteki ambiyans bandını netleştirir.

---

## KARAR: BOUNDED (single oversized fixed art) — LOOP DEĞİL

L3 ambiyans katmanı SONSUZ tile/wrap OLMAYACAK. Oda BOUNDED (kamera oda sınırına kadar pan eder, sonsuz scroll YOK), bu yüzden tek büyük sabit art + düşük camera-relative parallax yeterli ve doğru. Kamera kenara çarpınca daha fazlasını ortaya çıkardığı statik bir backdrop — top-down bounded-room normu budur.

**Neden loop değil:**
1. Kamera travel sınırlı (~12 birim). Düşük factor'da (0.10) katman sadece `12 × 0.10 = 1.2` birim kayar. Sprite'ın kendi çerçevesi içinde görünen pencere `12 − 1.2 = 10.8` birim kayar. Sprite bu kadar slack + viewport içerirse kenarı ASLA görünmez → wrap gereksiz.
2. `ParallaxLayer.cs` zaten origin-based (delta = cam − camStart, pos = layerStart + delta×factor, 64 PPU snap). TILING/wrap KODU YOK ve gerekmez. **Tek satır kod değişikliği gerekmiyor** — mevcut bileşen bounded parallax'ı doğru yapıyor.
3. Tiled SpriteRenderer / modulo-reposition = sonsuz-scroll oyunu (Sonic, sidescroller) için. Bounded arena'da OVERKILL: ekstra draw setup, seam riski, atlas karmaşası — sıfır kazanç.

## Oyunların yöntemi (kısa)
Top-down 3/4 (Hades Elysium, Children of Morta) düz bir zemin üzerinde GEZER — sidescroller'daki yatay hız-farkı parallax'ı burada zayıftır. Derinlik PARALLAX HIZIYLA değil, **atmospheric layering ile** verilir: ölçek + sis tint + opaklık kademesi + yavaş drift (Atmospheric Depth Tint, Layered Scroll Mist). Floating-arena void'da (platform kenarının altındaki uçurum) dip ambiyans = STATİK boyalı void + çok yavaş drift eden sis/bulut, kamera-relative parallax ÇOK DÜŞÜK. Loop etmez; oda-boyutu sabit art. Hades zemin altı boşluğu sahne başına authored, tile değil.

## Cliff-altı oturma reçetesi (sorting / factor / ölçek)
- **Sorting:** L3 ambiyans `sortingOrder = -350..-500`, sorting layer = en arka (Default veya BG). Decor_Cliff sorting layer order = 12 (ÇOK ÜSTTE). Parallax sadece X/Y oynatır, sorting'e DOKUNMAZ → L3 factor ne olursa olsun cliff'in GÖRSEL OLARAK arkasında/altında kalır. Z'yi `_layerStart.z` korur (kod zaten yapıyor). Gotcha YOK; sorting layer farkı mutlak.
- **Factor:** `(0.08, 0.04)` — X düşük (uzak hissi), Y = X'in yarısı (top-down dikey parallax hafif, F3 Sang pattern). F3'teki BG_Far (0.15) ile BG_Void (0.05) ARASINDA oturur; "cliff'in baya altı ambiyans" = Void'a yakın dip.
- **Ölçek:** sprite görsel olarak "uzak" → düşük kontrast + sis tint + hafif scale-down hissi (asset'te boyanır). Parallax düşük olduğu için kamera onu yavaş kaydırır = beyinde "çok uzakta" okur.

## RIMA somut öneri (değerlerle)
| Param | Değer | Gerekçe |
|---|---|---|
| Tipi | Single oversized fixed sprite (LOOP DEĞİL) | bounded oda |
| factor (X,Y) | **(0.08, 0.04)** | BG_Void↔BG_Far arası dip ambiyans |
| sortingOrder | **-400** | cliff (12) çok üstte; -420 BG_Far ile -350 BG_Mid arası |
| snapToPixel | true, PPU 64 | shimmer önle (mevcut default) |
| Boyut (min) | **≥ 1376 px geniş** (viewport 10 + travel×(1−factor) 11 ≈ 21 birim → 21×64≈1344, 1376 güvenli) | sizing formülü: `minWidth = viewportW + travelX×(1−factor)`. Mevcut 1024px placeholder (16 birim) 12-birim travel + 0.08 factor'da kenar AÇABİLİR → 1376'ya çıkar |
| Tile sayısı | **1** (oda başına) | bounded; varyant için sprite swap, tile değil |
| Drift (opsiyonel) | çok yavaş UV scroll / ayrı animator | "nefes alan" his — parallax değil, ambiyans hareketi |

Sizing aritmetiği: viewport = 640/64 = 10 birim. travelX ≈ 12. factor 0.08 → `minWidth = 10 + 12×0.92 = 10 + 11.04 = 21.04 birim ≈ 1347 px`. **1376 px** kullan (güvenli + BG_ARCH'taki "2 strip = 1376" ile tutarlı).

## F3 6-layer ile uyum/çakışma
ÇAKIŞMA YOK — TAMAMLAYICI. F3'te "L3" adı yok; F3 katmanları: BG_Void(0.05/-500), BG_Far(0.15/-420), BG_Mid(0.30/-350), BG_Near(0.50/-300), Mid_Ground(0.85/+10), Foreground_Front(1.10/+600). Kullanıcının "L3 cliff'in baya altı ambiyans" tanımı = F3'ün **BG_Void/BG_Far dip bandı**. Önerilen (0.08,0.04)/-400 bu ikisinin ARASINA temiz oturur; istenirse BG_Far'ı (0.15→0.08) hafif yumuşatıp -400'e çekerek tek "cliff-altı ambiyans" katmanına dönüştür. F3 boyutlarını 1024→1376 büyütmek tek prod aksiyonu (factor/sorting LOCK kalır).

## Authoring akışı
RimaRoomPainter `Parallax` layer + `ParallaxSection` Tier dropdown zaten LIVE. "Far 0.22" tier'ı `(0.08,0.04)` custom'a indir VEYA yeni "Ambient 0.08" preset'i ekle (TierPresetValues'a 0.08 + Y=0.5×). Preview Pan scrub (edit-mode, ExecuteAlways) ile Play'e girmeden cliff-altı derinlik doğrulanır. Tek tile, ReorderableList'te -400 sorting'e drag. Wrap/tiling toggle EKLEME — bounded oda için gereksiz scope.

## Locked-rule kontrolü
- F3 6-layer canonical (factor 0.05–1.10) → KORUNUR, bu karar dip bandı netleştirir, ezmez.
- Camera 640×360 / 64 PPU / pixelSnap → KORUNUR.
- Decor_Cliff sorting (12) → KORUNUR (L3 mutlak altında).
- **CONFLICTS WITH LOCKED RULES?: NONE.** Tek prod değişikliği: dip ambiyras sprite genişliği 1024→1376.
