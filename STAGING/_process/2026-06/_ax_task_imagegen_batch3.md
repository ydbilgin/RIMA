# GÖREV: İmagegen BATCH-3 — T8 + oda-makyaj asset'leri (üretim + QC + paketleme)

Sen RIMA'nın asset üretim ajanısın. Görsel üretim araçlarınla (imagegen) aşağıdaki listeyi üret. Çıktı klasörü: `STAGING/imagegen/assets/batch3_2026-06-07/` + `manifest.md` (her asset: dosya adı, boyut, amaç, durum).

## PIPELINE KURALLARI (önceki batch'lerle aynı)
- Transparan gereken asset'ler: MAGENTA (#FF00FF) düz arka planla üret → chroma-key temizliği (pedestal pipeline'ı). Opak ortam görselleri (nebula, silüet) doğrudan üretilebilir.
- Pixel-art stili: dark gritty chibi dünyasıyla uyumlu, PPU 64 hedefi; üretim büyükse nearest-neighbor downscale + palet-snap (pixel_cleanup).
- Renk dili: cyan (#19C2C2 civarı) = Rift/Echo enerjisi; warm-orange (#E89020) = brazier/insan ateşi; void = çok koyu mor-lacivert. Kaynak referans: `STAGING/imagegen/assets/portal_pack_2026-06-07/` içindeki kemerler (stil eşleşmesi).

## ÜRETİM LİSTESİ
### A) VOID DERINLIK SETİ (T8 — spec: STAGING/VOID_BACKGROUND_DEPTH_SPEC_2026-06-07.md OKU)
1. `distant_island_silhouette_01/02/03.png` — uzak yüzen ada silüetleri (koyu, detaysız, parallax katmanı için; 512-768px geniş, opak-void üstü veya magenta)
2. `void_nebula_sheet.png` — void sisi/nebula dokusu (1024², opak, çok koyu mor-cyan gradyanlı, tile edilebilir)

### B) ODA MAKYAJI (UXFLOW kararı — ChatGPT 03 listesi P0/P1'den)
3. `hole_rim_straight/corner/cracked/glow.png` — iç-delik kenar decal'leri ×4 (64×32 / 64×64; donut deliklerinin "harita hatası" görünümünü kırar; hafif cyan glow varyantı)
4. `ground_decal_*.png` ×6: thin_cyan_crack · circular_rift_scar · ritual_line_broken · combat_scratches · portal_scorch · faded_rune_tiles (64×32 - 128×64)
5. `edge_filler_*.png` ×6: broken_stone_chunk · rift_shard · chain_stump · rubble_pile · void_crystal_nub · altar_debris (32×32 - 96×64)
6. `parapet_low_segment_01/02.png` — portal arkası alçak kırık korkuluk segmentleri ×2 (96×64; full duvar DEĞİL)
7. `portal_label_plaque.png` — portal üstü tür yazısı için taş plaket frame'i (128×32, ortası boş/düz — yazı runtime'da basılacak)
8. `arrival_ring.png` — spawn varış halkası (96×96, cyan, tek kare; VFX kodla döndürür/fade'ler)

## QC (her asset için)
Üretim sonrası kendi gözünle kontrol: stil tutarlı mı (kemerlerle yan yana koy), transparanlık temiz mi (magenta artığı yok), boyut doğru mu. FAIL olanı 1 kez yeniden üret; ikinci FAIL'de manifest'e NEEDS_REVIEW yaz.

## ÇIKTI
manifest.md + kontak-sheet HTML (`batch3_contact_sheet.html`) + stdout'a özet tablo (asset · durum · boyut). Unity'ye İMPORT ETME (wiring T8 task'inde).
