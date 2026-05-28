# N5 — RIMA Floating-Island Ambiance Bible (Opus 4.8 synthesis)

**Amaç:** N3 (ışık) + N4 (çatlak) + N1 (cliff canon) + walless-lock'u tek tutarlı "RIMA mantıksal güzelleştirme" referansına sentezle. Bu = ambiyans anayasası; üretim ve sahne-kurulum bunu izler. ÜRETİM YOK.

## 1. Tek cümle vizyon
Void üzerinde asılı, kenarları karanlığa dökülen, alttan cyan rift enerjisi sızan, sıcak brazier ışığıyla noktalanmış **yaşayan-ama-dengesiz taş ada** — duvarsız, yüksek-top-down 3/4, painterly-pixel.

## 2. Görsel katman yığını (arkadan öne, "mantıksal" derinlik)
| Z | Katman | İçerik | Işık | Kaynak karar |
|---|---|---|---|---|
| en arka | **Void BG** (parallax KitC) | deep-purple #3A1A4A→black gradient + seyrek cyan rift damarı | UNLIT (local light değmez → karton-dekor önler) | N3 |
| arka | **Cliff face/base** (L2-L3) | ada kalınlığı, aşağı karanlığa düşer | global ambient + cyan rim (kenar), alt fill YOK | N3, N1 |
| orta | **Floor** (L1) | slate iso-tile zemin | global ambient + brazier/rune focal pool | walless |
| orta+ | **Cracks/decor** (L4 overlay) | taş-çatlağı/cyan-rift/erozyon/yama (%15 max) | taş=unlit, cyan=emissive (Light2D yok) | N4 |
| ön | **Gameplay** (L6) | player/mob/VFX | tüm focal ışıklar | — |
| ışık | **Scene_Lighting** (bağımsız GO) | global + rim + brazier + rune | — | N3 (kritik fix) |

## 3. Palet (LOCKED)
slate #3A3D42 base · cyan rift #00FFCC accent (enerji/sınır) · warm #E89020 secondary (brazier/medeniyet) · deep-purple #3A1A4A bg-derinlik. **Kural:** cyan = enerji+sınır anlatır, warm = yaşam+ritüel anlatır, slate = nötr yapı. Renk = anlam, dekorasyon değil.

## 4. "Mantıksal güzelleştirme" ilkeleri (saçmalık-önleyici)
1. **Okunurluk > yoğunluk:** 640×360'ta gameplay (mob/mermi) her zaman seçilebilir. Dekor %15 yoğunluk limiti (N4). Çatlak min 2px.
2. **Tek PPU (64):** mixel YASAK. Boss bile büyük-tuval+PPU64 (N1). 
3. **Bake YOK, engine-side ışık:** sprite nötr çizilir, gölge/glow Light2D + emission-mask (N3). Baked+dynamic çakışması illüzyon kırar.
4. **Işık = mimari, dekor değil:** ışıklar Scene_Lighting altında, dekoratif obje child'ı DEĞİL (black-cliff kök nedeni, N3).
5. **Performans:** Shadowcaster2D ASLA (organik tilemap perf çöküşü); düşen gölge statik sprite drop-shadow. Light segment başına değil, room-level grouped.
6. **Anlam-tutarlı yerleşim:** cyan rift yakını cyan-crack, void-kenarı erozyon, yüksek-trafik aşınma, köprü/boğaz yama (N4). Random scatter DEĞİL, weighted-rule.
7. **Sınır = görsel, collider = veri:** erozyon/kenar görsel kopukluk walkable'ı değiştirmez (Room/Collider verisi otorite).

## 5. Cliff sistemi (N1 canon)
2-stage hibrit: CliffAutoPlacer kenar-otomatik + DecorCliffPainter manuel dokunuş. Demo odaları auto-cliff ile rebuild. Decor_Cliff sorting < Floor. (S110 "full-manuel" drift geçersiz.)

## 6. Üretilebilirlik roadmap (animasyon/art fazında — hazır spec)
Hepsi Python-cheap veya create_object, PixelLab-MCP minimal:
- **Işık asset'leri** (N3): emission-mask 32×32 (Python edge-extract) · void_gradient 1×256/512² (Python) · rune/brazier glow-mask 128×128 (additive).
- **Çatlak asset'leri** (N4): 4 tip, üretim tablosu+promptlar `N4_CRACKS_DESIGN_FINAL.md`'de hazır (create_object 32/48/64px).
- **Cliff sprite:** KitB_Cliff mevcut/üretilecek; placement sistemi hazır (auto-placer).
- **Sıra:** önce Scene_Lighting + Decor_Cliff light-target FIX (sistem, ND5) → sonra emission-mask + cracks (art) → en son polish VFX.

## 7. Sahne-kurulum öncelik (demo için, art-minimal)
1. **ND5:** Scene_Lighting GO + tüm Light2D'lere Decor_Cliff/Decor_Floor target (cliff siyah olmasın). [SİSTEM, gece-yapılabilir]
2. Oda rebuild auto-cliff (cliff'ler silinmişti) — mevcut KitA floor + cliff sprite. [SİSTEM]
3. Işık intensity tune (N3 reçete) — playtest. [A5 ile]
4. Crack/emission asset üretimi — art fazı (kullanıcı onayı). [ART, deferred]

**Sonuç:** Ambiyans tam tanımlı + üretim-hazır. Demo için gereken SİSTEM işi: ND5 (lighting fix) + oda rebuild. Geri kalan = art (animasyonla birlikte). "Sadece animasyon/art kalır" hedefiyle uyumlu.

**Index:** `reference_ambiance_bible_n5` → görsel yığın + 7 ilke + üretim roadmap.
