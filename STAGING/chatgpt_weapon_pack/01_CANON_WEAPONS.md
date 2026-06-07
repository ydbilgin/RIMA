# RIMA Kanonik Silah Tablosu — 10 Sınıf (NotebookLM tasarım kutsal kitabından, 2026-06-07 çekimi)

**TEMEL KURAL (LOCKED):** `1 sınıf = 1 silah = 1 silüet`. Varyant YOK. Silah değişimi = sınıf değişimi. Silah = silüetin %30-50'si. Silah HEP elde/hazır pozisyonda ("puf" yok); tek istisna Ronin kın/draw kimliği.

## Özet tablo (kanonik canvas spec'i — Karar #99 tablosu)

| # | Sınıf | Silah | PixelLab Canvas | Unity Final | Aksent | Durum |
|---|---|---|---|---|---|---|
| 1 | Warblade | İki elli Greatsword | 256×256→192 trim | 96×96 | `#C09455` pirinç | ✅ LIVE |
| 2 | Ronin | Katana (çekili) + kın (sol bel) | 128×128 | 64×64 | `#C8A87A` soluk altın | ✅ LIVE (drawn) |
| 3 | Gunslinger | Dual Rift-tech Pistol (L+R mirror) | 64×64 | 32×32 | `#FF4400` ateş turuncusu | ❌ kuyruk |
| 4 | Ranger | Compound Bow + Ok (SOL el) | 128×128 | 64×64 | `#7BA7BC` soğuk mavi | ❌ kuyruk |
| 5 | Elementalist | Floating Golden Rune Disc (ELDE DEĞİL — avuç üstünde süzülür; ASA/DEĞNEK YASAK, Karar #146) | 96×96 | 48×48 | `#FFF000` altın/sarı | ❌ kuyruk |
| 6 | Shadowblade | Twin Daggers reverse-grip (L+R mirror) | 64×64 | 32×32 | `#5A2A8A` void moru | ❌ kuyruk |
| 7 | Ravager | Dual Compact Axes / hatchet çifti | 128×128 | 64×64 | `#D43F3F` kan kırmızısı | ❌ kuyruk |
| 8 | Hexer | Grimoire / Cursed Totem / Scepter (Whip İPTAL — AI hatasıydı) | 96×96 | 48×48 | `#8B0000` koyu kırmızı | ❌ kuyruk |
| 9 | Brawler | YOK — yumruk/sargı (gövdeye baked veya opsiyonel ayrı sprite) | 96×96 | 48×48 çift | `#FF8C00` turuncu | ❌ kuyruk |
| 10 | Summoner | Soul Lantern (SOL el, hover; staff swing YOK) | 96×96 | 48×48 | `#00FF88` neon yeşil | ❌ kuyruk |

## Sınıf başına görsel kimlik notları

1. **Warblade** — koyu çelik, kahverengi deri kabza, süssüz işçi estetiği. Sprite'ın ~%45'i. Low-guard: uç yere yakın, yatay taşıma. Sırtta TAŞINMAZ.
2. **Ranger** — taktiksel/mekanik avcı yayı (orman okçusu DEĞİL, Karar #37). Sol elde, aşağı eğik at-rest. Asimetrik duruş.
3. **Shadowblade** — ince, temiz, keskin ikiz hançer; HEP reverse-grip, gövdeye yakın. **Gömülü glow YASAK.** Sprite'ın ~%30-35'i.
4. **Elementalist** — SİLAHSIZ sınıf. Altın rün diski sağ avucun ~3px üstünde döner/süzülür (~8px disk, 64px karakterde). HİÇBİR ELDE OBJE TUTULMAZ.
5. **Ravager** — kaba, vahşi görünümlü KISA balta çifti (uzun saplı tek balta DEĞİL). İki elde, kollar açık, agresif simetri.
6. **Ronin** — klasik katana, soluk altın/pirinç kabza; SAĞ elde çekili; SOL belde kın ZORUNLU (iaido kimliği).
7. **Gunslinger** — Rift-teknolojisi tabancalar; western/kovboy estetiği YASAK (Karar #38). İki elde simetrik (L+R mirror).
8. **Brawler** — silah YOK; koyu deri el sargıları/dövüş kemeri. Boxing guard duruşu.
9. **Summoner** — soğuk cyan ışıklı ruh feneri SOL elde süzülür; sağ el orkestra şefi yönlendirme jesti. Staff swing yakın-dövüş hamlesi YOK.
10. **Hexer** — macabre lanet estetiği (Elementalist=akademik, Summoner=yeşil-minyon'dan NET ayrı). Gövde önünde tutulan grimoire; baş öne eğik lanet okuyucu.

## Renk çakışma kuralları
Her sınıfın 1 dominant aksenti var, sınıflar arası çakışma yasak. Dikkat: Hexer koyu kırmızı (`#8B0000`) vs Ravager kan kırmızısı (`#D43F3F`) — ton ayrımı korunmalı.

## Ek kanonik bilgi (endüstri araştırması kaydından)
- Chibi 120×120 karakter için: tek elli silah 64×64 veya 64×128 canvas; çift elli büyük silah 128×256/64×256 dikey canvas.
- HandAnchor mekaniğinde silah sprite'ının TUTMA NOKTASI merkez pikseli pivot olmalı (rotasyon matematiğini basitleştirir).
- Mimari model: Hades tarzı "silahsız body + dinamik takılı silah + bağımsız VFX katmanı" (Children of Morta'nın gömülü yöntemi REDDEDİLDİ).
- Karakter aynalandığında (flipX) silahın ters/baş aşağı dönmemesi için yön bazlı offset+rotation kod tarafından güncellenir (kodda mevcut: OrientationSync).
