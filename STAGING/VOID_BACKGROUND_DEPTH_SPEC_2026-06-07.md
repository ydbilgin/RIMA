# VOID BACKGROUND DERİNLİK SPEC'İ (T8 tasarım temeli — 2026-06-07)
**Amaç:** "Void üstünde yüzen ada" hissi — düz mor arka plan ölü duruyor; katmanlı derinlik gerekli. Mevcut sistemler REUSE: `ParallaxLayer` (void/nebula/ruins/islands/fog modları kodda VAR), `bg_L4_fog.png` (1672×941), BgKit_RefC katmanları, `CliffEdgeDustEmitter`, `RoomMoodLightPool`.

## Katman yığını (arkadan öne, parallax çarpanı = kamera hareketine oran)
| # | Katman | Parallax | İçerik | Kaynak |
|---|---|---|---|---|
| L0 | Void taban | 0 (sabit) | Düz renk DEĞİL: dikey gradyan — altta zifir mor-siyah, üstte hafif mor pus. Ekranın altı daha karanlık = "aşağısı dipsiz" hissi; üst kısım hafif aydınlık = göz portallara akar | tek gradient sprite/quad |
| L1 | Nebula/aurora | 0.02-0.05 | Çok yavaş kayan büyük yumuşak şekiller, cyan-mor, düşük alpha (~0.15) — sonsuz derinlik sinyali | BgKit_RefC / 1 imagegen wisp sheet |
| L2 | **Uzak ada silüetleri** | 0.08-0.12 | 2-3 boyda koyu silüet, seyrek; rift'lerinden ÇOK soluk cyan sızıntı. "Bu dünyada başka adalar da var" — lore + derinlik çapası | ❗ imagegen batch-3 (2-3 sprite, S) |
| L3 | Orta sis bandı | 0.15-0.20 | bg_L4_fog reuse, alpha 0.3-0.4; kameradan BAĞIMSIZ yavaş yatay drift (rüzgâr) — statik sahnede bile hayat | mevcut |
| L4 | Yakın toz/parçacık | ~0.3 | Ada kenarlarında yükselen seyrek toz motları; güney cliff önünde yoğunlaşır | CliffEdgeDustEmitter + atmos_dust (mevcut) |
| L5 | OYUN ADASI | 1.0 | — | — |
| L6 | Ön-plan sis (POST-DEMO park) | >1 | Ada altından geçen nadir yakın sis — "derinlik sandviçi" ama okunabilirlik riski | park |

## Kurallar (kilit)
1. **Parallax çarpanları KÜÇÜK kalır** (≤0.2 ana katmanlar): yüksek 3/4 kamerada agresif parallax "yana bakıyormuşum" hissi verir, tepeden-bakış illüzyonunu kırar. Az = inandırıcı.
2. **Pixel Perfect uyumu:** katman pozisyonları PPU-grid'e kuantize edilir (frame başına round) — jitter/shimmer bombası (Flash R2 uyarısı) böyle sönümlenir. Test: kamera yavaş pan'de titreme yok.
3. **Renk disiplini:** arka plan saturasyonu DÜŞÜK; arka plandaki hiçbir cyan, oyun-içi cyan sinyallerinden (portal çekirdeği, rün, Echo) daha parlak OLAMAZ. Mor egemen; cyan sadece uzak ada rim'lerinde eser miktarda.
4. **Oda tipine mood tint'i:** background sprite'ları değişmez; `RoomMoodLightPool` global tint ile elite=hafif kızıl pus, boss=daha derin karanlık + eser kızıl. Süreklilik korunur ("aynı void"), tema hissi bedavaya gelir.
5. **Performans:** katman = tek büyük sprite/quad (kamera zoom-out'unu kapsayacak boy) + 1 particle system; per-frame alloc yok.
6. **Geçiş sürekliliği:** background oda geçişinde YENİDEN kurulmaz — _Arena'da kalıcı, oda rebuild'inden bağımsız (RoomRunDirector'a değil sahneye bağlı root).

## Uygulama eşlemesi (T8 task'ine girecek)
- ParallaxLayer'ları _Arena'da kalıcı `VoidBackdrop` root'una bağla (L0-L4); Chamber'a da aynı root (daha koyu varyant).
- Eksik asset: **batch-3 imagegen (S):** 2-3 uzak ada silüeti + 1 nebula wisp sheet → Lane B kuyruğuna (batch-2 sonrası).
- Doğrulama: ScreenshotMode "room overview" preset'inde önce/sonra karşılaştırma + pan-jitter testi + FPS.
- R5 (video analizi) derinlik tekniği çıkarırsa bu spec'in üstüne revize gelir.
