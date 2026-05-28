# RIMA Modular Wall Production Strategy

**Date:** 2026-05-23 (S102)
**Purpose:** Hedge LoRA-based template approach with **modular wall pieces** that can connect. Explore PixelLab production for connectable walls. Inpaint-based stitching as fallback.
**Pairs with:** `architecture_decision.md` (Hybrid Template+Decor LOCK) · `STAGING/iqg_dual_grid_2d_research.md`

---

## 0. Why this strategy exists

User's instinct: **"LoRA training istediğimiz gibi olmayabilir."** Smart hedge:
- LoRA training devam etsin → primary template path
- **Paralel modular path** kurulsun → fallback + extension layer

S101 LOCK zaten "wall extension/repair layer" olarak modular'a yer açtı. Bu rapor onu detaylandırıyor.

---

## 1. MEVCUT envanter (zaten elimizde olan)

### A) Wall pilot_a (S95) — 7 parça MODÜLER SET ✅
Path: `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/`

| Frame | Piece | Function | Quality |
|---|---|---|---|
| 1 | face_EW | Düz duvar yatay (E-W run) | ✅ Style/palette ✓ |
| 2 | corner_outer | Dış 90° köşe | ✅ Style/palette ✓ |
| 3 | arch_opening | Kapı arch (taş, içi taş döküntü) | ✅ Style/palette ✓ |
| 4 | face_NS | Düz duvar dikey (N-S run) | ✅ Style/palette ✓ |
| 5 | corner_inner | İç köşe (girinti için) | ✅ Style/palette ✓ |
| 6 | T_junction | T-bağlantı (iki duvar buluşur) | ✅ Style/palette ✓ |
| 7 | end_cap | Duvar bitiş (açık geçit) | ✅ Style/palette ✓ |

**Bu 7 parça top-down modüler için MİNİMUM VIABLE KIT.** RIMA palette (dark stone + cyan rift cracks) zaten içinde. Pixel art, ~128×128 each.

### B) Wall modular v1 (5 parça) ✅
Path: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_modular_v1/`
- tile_archway_full
- tile_banner_a, tile_banner_b (decorated walls)
- tile_big_column
- tile_big_corner
- tile_burst_h/l/s/v (decals)

### C) Wall_pack_v3 + contact_sheet
Path: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/`

### D) Existing PixelLab Wang tilesets (account inventory) — 25 adet
PixelLab MCP'de mevcut, **0 credit ek maliyet**. Many already RIMA-palette match:
- Dark slate gray stone floor
- Weathered granite floor (cool tones)
- Dark rubble stone floor
- Cool grey granite stone surface
- Dark broken stone wall terrain (2 variants!)

**Bunlar Wang tile (4-way otomatik bağlantı). Modular wall tile için DIREKT ALINABİLİR.**

---

## 2. Minimum Viable Modular Wall Kit (full coverage)

Top-down 85-90° iso projection için **gerekli parçalar:**

### Kritik (must-have, geometric coverage)
| # | Parça | Mevcut mu? |
|---|---|---|
| 1 | Düz duvar E-W | ✅ pilot_a_face_EW |
| 2 | Düz duvar N-S | ✅ pilot_a_face_NS |
| 3 | Outer corner NE | ⚠️ Sadece "corner_outer" var — 4 rotasyon türetilmeli |
| 4 | Outer corner NW | ⚠️ Aynı (rotate ile) |
| 5 | Outer corner SE | ⚠️ Aynı |
| 6 | Outer corner SW | ⚠️ Aynı |
| 7 | Inner corner (girinti) | ✅ pilot_a_corner_inner |
| 8 | T-junction | ✅ pilot_a_T_junction |
| 9 | End cap | ✅ pilot_a_end_cap |
| 10 | Arch opening (door socket) | ✅ pilot_a_arch_opening |

### Variation (nice-to-have, art richness)
| # | Parça | Mevcut mu? |
|---|---|---|
| 11 | Wall with banner (red/blue/torn) | ✅ tile_banner_a/b |
| 12 | Wall with torch sconce | ⚠️ Decor overlay yerine wall variant olabilir |
| 13 | Cracked wall variant | ✅ wall_burst decals |
| 14 | Wall with crack burst | ✅ tile_burst_h/v |
| 15 | Tall pillar | ✅ tile_big_column |

**Sonuç:** Mevcut kit **temel kapsam dolu** (1-10 cover via existing + rotations). Decor variants 11-15 da var. **Yeni üretime KESİN gerek YOK** — önce mevcut kit'i Unity'de test et.

---

## 3. Üç üretim yolu

### Yol A — Mevcut envanteri kullan (SIFIR yeni gen) ⭐ önerilen ilk adım
**Adımlar:**
1. wall_pilot_a 7 parçayı **Unity'de Tilemap'e yerleştir**
2. Edge match testi: 4-5 farklı duvar layout dene
3. Edge gap/clip varsa not düş → spesifik fix gereken parça(lar)
4. PASS → modular kit lock, yeni gen YOK

**Süre:** ~2-3 saat Unity test
**Risk:** Eski parçalar style drift olabilir (S95'ten beri palette evrildi)
**Maliyet:** 0 PixelLab credit

### Yol B — PixelLab `create_topdown_tileset` (Wang tile generator) ⚙️ orta
**Mantık:** PixelLab'ın native Wang tile üretici tool'u var. 16-tile veya 4-tile setler üretir, edge'ler **garanti uyumlu** (Wang kuralları).

**Adımlar:**
1. Reference image: master_room veya pilot_a_face_EW
2. PixelLab web UI'da `create_topdown_tileset` çağır:
   - Description: "dark fantasy dungeon wall, weathered stone, cyan rift cracks, RIMA palette"
   - Tile size: 32px veya 64px
   - Type: square_topdown 16var
3. Output: 16 tile Wang set
4. Unity'de Tilemap + Rule Tile asset ile import
5. Auto-tiling otomatik çalışır

**Süre:** ~1-2 saat (gen + import)
**Maliyet:** ~5-10 PixelLab credit (1 Wang tileset)
**Avantaj:** Edge match **algoritmik garanti**
**Dezavantaj:** Style fully PixelLab'ın kararı, RIMA palette'a tam oturmayabilir

### Yol C — Manuel + inpaint stitching 🛠️ yavaş ama kontrollü
**Mantık:** Her parçayı **tek tek** üret, edge'leri inpaint ile pürüzsüzleştir.

**Adımlar:**
1. **Anchor üret** — bir master piece (face_EW gibi). Bunun edge profili herkes için referans.
2. **Sıradaki parça** — anchor'ı PixelLab `create_image_pro` reference olarak ver, prompt: "create N-S facing wall, match left edge of reference EXACTLY"
3. **Inpaint pass** — çıkan parçayı Aseprite/Photoshop'a aktar, edge'leri pürüzlü ise inpaint ile düzelt
4. **Verify** — 2 parçayı yan yana koy, edge match doğrula
5. **Repeat** — 7-10 parça için tekrarla

**Süre:** ~3-5 saat (10 parça için)
**Maliyet:** ~10-20 PixelLab credit
**Avantaj:** Stil tam kontrol
**Dezavantaj:** Yavaş, edge drift riski yüksek, manuel inpaint emek

---

## 4. Karşılaştırma matrisi

| Boyut | Yol A (mevcut) | Yol B (Wang) | Yol C (manual+inpaint) |
|---|---|---|---|
| **Süre** | 2-3 saat | 1-2 saat | 3-5 saat |
| **Credit maliyet** | 0 | 5-10 | 10-20 |
| **Edge match güveni** | Test edilmeli | ✅ Algoritmik | Manuel doğrulama |
| **Style RIMA-fit** | ✅ %95 (eski ama uygun) | ⚠️ %70 (PixelLab default) | ✅ %95 (kontrol) |
| **Iterate hızı** | Hızlı | Yeniden gen gerek | Yavaş |
| **Risk** | Style drift | PixelLab style mismatch | Edge inpaint zorluğu |

---

## 5. Önerilen plan — 3 fazlı hedge

### Faz 1 (NOW, 2-3 saat) — Yol A test
- Mevcut 7 parçayı Unity Tilemap'e koy
- 3-4 farklı oda layout dene (rectangle, L-shape, T-shape, diamond)
- Edge match kontrol
- PASS → kit LIVE, modular hedge tamamlandı, sadece banner/decor variant prodüksiyon kaldı
- PARTIAL → spesifik problemli parça(lar) belirle → Faz 2'ye o parça(lar) için git
- FAIL → Faz 2 full

### Faz 2 (LoRA paralel, ~1 saat) — Yol B (sadece eksiklere)
- PixelLab `create_topdown_tileset` ile Wang wall set üret
- Mevcut Shattered Keep palette tilesetlerini reference ver
- Output: 16-tile Wang wall set
- Unity'de Rule Tile ile entegre
- Faz 1 PARTIAL ise sadece eksik parçaları üret

### Faz 3 (gerekirse, 3-5 saat) — Yol C (son çare)
- Sadece Faz 1+2'nin yapamadığı specific edge case'ler için
- Manuel + inpaint pürüzsüzleştirme

---

## 6. Şu an LoRA training ile ilişki

**Paralel iş, çakışmıyor:**
- LoRA training (~5-7 saat kaldı) → **template painting** üretimi için
- Modular wall kit → **template extension/repair layer** + **corridor connectors**

Iki sistem birbirini tamamlar:
- Template (LoRA) = full oda görüntüsü
- Modular walls (kit) = template kenarına bağlanır, ekstra duvar/koridor üretir
- Decor overlay = ikisinin üzerine spawn

---

## 7. Önerim — Şu an yapılabilecek

1. **Şimdi (15 dk):** Codex'e dispatch et:
   > "Open `Assets/Scenes/` test scene, instantiate the 7 wall_pilot_a sprites side-by-side at 1.59 unit spacing (S100 lock), screenshot the result. Show edge match status."

2. **Test PASS olursa (~1 saat):** Mevcut kit'i Unity'de Tilemap asset olarak commit et, modular_wall_v2 olarak rename et.

3. **Test FAIL olursa (~2 saat):** PixelLab web UI'da `create_topdown_tileset` ile yeni Wang wall set üret (Yol B). Reference: mevcut Act1 dark slate stone tilesetlerinden biri.

4. **Bu sırada LoRA training devam eder** — 1500 step'te durup test ederiz, template kalitesini görürüz.

---

## 8. Critical reminder — sınırlar

- ❌ PixelLab credit harcamayı yormalısın (kuralın gereği), Yol A önce zorunlu
- ❌ Tile-based modular kit RIMA için historically zor (S100 ve önceki revoke kararları var) — ama bu sefer "extension layer" rolünde, primary değil
- ✅ S101 LOCK zaten modular wall'ı "extension layer" olarak kabul ediyor — bu doc o LOCK'la uyumlu, çatışmıyor
- ✅ Yol A test = 0 risk, 0 maliyet → ne olursa olsun yapılmalı

---

## 9. Output beklenen

Bu strategy belgesini onaylarsan:

| Aksiyon | Tool | Süre |
|---|---|---|
| Faz 1 Unity test setup | Codex dispatch | 1 saat |
| Faz 1 sonucu review | Orchestrator + Opus | 30 dk |
| Faz 2 (gerekirse) Wang üretim | User PixelLab web UI | 1 saat |
| Faz 2 Unity entegrasyon | Codex dispatch | 1 saat |
| Modular kit LIVE lock | Doc + memory | 30 dk |

Faz 1'i hemen başlatabilirim — onayını bekliyorum.
