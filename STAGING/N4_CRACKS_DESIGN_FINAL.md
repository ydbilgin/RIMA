# N4 — Mantıksal Çatlak/Patch Tasarımı FINAL (Opus 4.8, triple-AI CONVERGE)

**Amaç (DÜŞÜN):** floating-ada zemini çatlak/patch üretim spec + yerleşim mantığı. ÜRETİM YOK — hazır olsun. Opus + Codex (yasinderyabilgin) + agy (ydbilgin). Task: `STAGING/N4_CRACKS_DESIGN.task.md`.

## 4 çatlak tipi (ambiyans anlatısı)
| Tip | Anlatı | Görsel dil |
|---|---|---|
| **A. Zemin taş çatlağı** | yapısal yorgunluk, "eski/yaşayan ada", void'de yük taşıma stresi | slate #3A3D42 + deep-purple #3A1A4A gölgeli ince yarık, ışıksız |
| **B. Cyan rift-crack** | alttaki void/çekirdek enerjisi sızıyor, "yaşayan+dengesiz güç" | neon cyan #00FFCC sızan, kenar ısınmış deep-purple. **emissive ama Light2D SPAWN ETMEZ** |
| **C. Kenar-erozyon** | void adayı yutuyor, sınır güvensiz — floating-island'ı EN İYİ destekler | pürüzlü dişli kırık hat, void'e bakan kopma |
| **D. Yama/onarım izi** | geçmişte medeniyet, ritüel kültür, stabilizasyon onarımı | warm #E89020 tuğla/metal kelepçe, harç dolgu |

## Üretim tablosu (LOCK — üretilmedi)
| Tip | px | Varyant | Tool | Yerleşim kuralı | Layer |
|---|---|---|---|---|---|
| Zemin taş çatlağı | 32×32 tile-aligned (+48 opsiyonel) | 6 | create_object | merkez seyrek, path'te yoğun, rift/kenar dışı nötr | L4 overlay (MVP) |
| Cyan rift-crack mikro | 32×32 tileable 3'lü (uç/gövde/uç) | 4 | create_object | rift objesine 1-3 tile, düşük yoğunluk | L4 overlay (emissive) |
| Cyan rift-crack orta | 48×48 decor (grid-dışı taşar) | 3 | create_object | rift çevresi vurgu, oda başına 1-3 | L4 overlay (emissive) |
| Kenar-erozyon edge | 32×32 yönlü (düz×2/köşe×2 min) | 8 | create_map_object | SADECE cliff/floor sınırı, edge-direction'a göre, iç bölgede ASLA | L4 overlay (L2 sınırında) |
| Kenar-erozyon corner/lip | 64×64 decor | 4 | create_object | ada köşe/çıkıntı, void'e bakan kırık uç | L4 overlay |
| Yama/onarım küçük | 32×32 tile | 4 | create_object | köprü ayağı, dar boğaz, kapı önü, brazier çevresi | L4 overlay |
| Yama/onarım büyük | 64×64 irregular | 2 | create_object | oda başına 0-2, tekrarsız+rotasyonlu | L4 overlay |

**Layer kararı (Opus):** MVP'de hepsi **L4 walkable-decor overlay** (painter esnekliği + emissive cyan gölgeden etkilenmesin). Stabil tipler (taş çatlağı/yama) sonra L1 floor-varyantına bake edilebilir. Cyan ASLA L1'e bake edilmez (emissive).

## Yerleşim = weighted-rule (random DEĞİL) + painter brush
- **Merkez:** çok seyrek (her 12-18 tile'da 1 / max %10).
- **Rift yakını:** cyan-crack yoğunlaşır (1-3 tile), uzakta biter → normal çatlağa döner.
- **Kenar yakını:** erozyon 0-2 tile, köşede 64px parçalanma.
- **Yüksek-trafik path:** köprü bağlantısı, kapı önü, combat-merkez rotası → aşınma + yama.
- **MVP araç:** Room Painter **4 brush** ("Rift Crack" / "Edge Erosion" / "Floor Fatigue" / "Repair Patch"). Tam-otomatik weighted-scatter = 2. aşama.

## 🔴 SAÇMALIK / RİSK (triple-AI)
1. **Noise tehdidi:** her tile'a çatlak = 640×360'ta zemin okunmaz, gameplay elementi (mob/mermi) seçilemez → **%15 yoğunluk limiti** (oda zemininin max %15'i overlay).
2. **Cyan-light çakışması:** cyan-crack standart normal-map'le gölgelenirse sönük; Light2D spawn ederse N3 rift-lighting ile çakışır → **unlit/emissive material, Light2D YOK** (ortam ışığını N3'teki rune/rim verir).
3. **32px@PPU64 görünürlük:** 1px detay aliasing'de kaybolur → çatlak gövdesi **min 2px**, uç 1px.
4. **Collider:** kenar-erozyon görsel kopuk gösterse de walkable sınır Room/Collider verisinden gelir — erozyon collider DEĞİŞTİRMEZ.
5. **Carpet-pattern:** 64px yama tekrarı "halı deseni" → nadir + rotasyonlu.

## Üretim promptları (hazır, ÜRETİLMEDİ)
- Taş çatlağı: `pixel art floor crack decal, top-down 3/4, dark slate grey stone, deep purple cracks, weathered, tile-aligned 32x32, transparent bg, 2px min crack width, no AA`
- Cyan rift: `pixel art glowing energy crack, neon cyan rift from dark purple fissure, emissive, top-down 3/4, 48x48, transparent bg`
- Kenar-erozyon: `pixel art cliff edge erosion decal, crumbling slate, void transition, top-down, 32x32 tileable directional edge, transparent bg`
- Yama: `pixel art stone repair patch, warm orange-brown bricks in slate floor, iron clamps, top-down 3/4, 32x32`
Hepsi 64 PPU, point filter, no compression, transparent bg, no chars/UI, no full-tile floor.

**Index:** `reference_floating_island_cracks_n4` → 4 tip + üretim tablosu + %15 limit + 5 saçmalık + promptlar.
