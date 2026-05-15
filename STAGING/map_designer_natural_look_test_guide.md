# Map Designer "Doğal Görünüm" Test Walkthrough

**Hedef:** Phase1_ProceduralMap_Test sahnesinde 6-layer environment pipeline'ını adım adım doğrula. PixelLab asset gen öncesi/sonrası her iki durum kapsanır.

**Sahne:** `Assets/Scenes/Phase1_ProceduralMap_Test.unity`
**Editor menüsü:** `Window > RIMA > Map Designer`
**Mevcut Biome:** `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset`
**Mevcut RoomRecipe:** `Assets/Data/RoomRecipe_ShatteredKeep_Combat_01.asset` (16x12, 3 encounter slot)

**Layer haritası (referans):**
- L1: Base floor tilemap (biome tileset)
- L2: Floor variants (tilemap içi varyasyon)
- L3: Wall overlay (WallBrushSetSO — Hades-style organik brush)
- L4: Transition / biome patches (PatchAtlasSO — moss, large biome)
- L5: Detail decals (PatchAtlasSO — crack, moss cluster, starburst)
- L6: Accent (PatchAtlasSO — rift, sparse 0-3 per map)

---

## Aşama 0 — Hazırlık (1 dakika)

1. Unity Editor'ü aç. Proje açılışında csproj auto-regenerate ~30 sn sürer; Console temizlenmeden işlem yapma.
2. Project window: `Assets/Scenes/Phase1_ProceduralMap_Test.unity` zaten açık olmalı (saved). Değilse çift tıklayarak aç.
3. Menü: `Window > RIMA > Map Designer` aç. 6 layer toggle paneli ve `Room Recipe` slot'u görünmeli.
4. Console'da error olmamalı. Compile error varsa adım atma — önce çöz.

**Beklenen:** Map Designer penceresi temiz, RoomRecipe slot boş, 6 toggle (L1-L6) görünür.

**Sorun:** Menü görünmüyor → `Assets/Editor/RimaMapDesignerWindow.cs` compile hatası vardır. Console'a bak.

---

## Aşama 1 — Hızlı Başlangıç (Assetsiz Floor Test)

Yeni asset oluşturmadan mevcut tileset ile L1+L2 floor'u doğrula.

**Adımlar:**
1. Map Designer penceresinde `Room Recipe` field'ına `Assets/Data/RoomRecipe_ShatteredKeep_Combat_01.asset` sürükle.
2. `Seed`: 12345 yaz.
3. L1 ve L2 toggle ON, diğer layer'lar (L3-L6) OFF.
4. `Generate` butonuna bas.

**Beklenen:**
- Scene view'da 16x12 floor zone görünür (Shattered Keep tileset).
- Walkable cell'ler dolu, perimeter tile'ları wall placeholder.
- L3 wall overlay yok (WallBrushSet atanmadı), L4-L6 boş.
- Console: "Generated room with seed 12345" benzeri log.

**Asset yokluğunda fallback:** L3-L6 sessizce skip edilir, error fırlatmaz. RoomRecipe'da ilgili field null ise orchestrator o layer'ı atlar.

**Sorun:**
- Hiçbir şey görünmüyor → Floor tilemap reference Map Designer'da set edilmemiş olabilir. Pencere içinde `Floor Tilemap` slot'u boşsa scene'deki Tilemap GameObject'i sürükle.
- Tile'lar pembe → biome preset'teki tileset ref kopuk. `Shattered_Keep_F1_BiomePreset.asset` inspector'da tile array doğrula.

---

## Aşama 2 — L4 Transition Test (Mevcut Moss Atlas)

PixelLab gerektirmez. `PatchAtlas_Moss_ShatteredKeep.asset` ile edge-biased moss patch davranışını doğrula.

**Adımlar:**
1. `RoomRecipe_ShatteredKeep_Combat_01.asset` seç. Inspector'da `Transition Atlas` field → `Assets/Data/PatchAtlas_Moss_ShatteredKeep.asset` sürükle.
2. Map Designer'a dön. L4 toggle ON.
3. `Regenerate` (seed 12345 sabit kalsın).

**Beklenen:**
- Walkable cell'ler üzerinde 3 farklı moss sprite dağılır.
- Wall'a bitişik tile'larda yoğun (wallProximityFactor=1.5).
- Oda merkezindeki gameplay arena sparse (centerPathDensityReduction=0.05).
- Encounter slot'larından ≥2 tile uzakta.
- Aynı sprite örnekleri arasında ≥4 tile mesafe (minDistance=4).
- Bazı sprite'lar X-flip uygulanmış (allowFlipX=1).

**Sorun:**
- Hiç moss yok → `Transition Atlas` field gerçekten atandı mı? RoomRecipe inspector kontrol.
- Wall'ların üstünde moss → walkable filter çalışmıyor. Terrain map'te wall cell'lerde `walkable=false` olmalı.

---

## Aşama 3 — L6 Accent Test (Mevcut Rift Atlas)

**Adımlar:**
1. `RoomRecipe_ShatteredKeep_Combat_01.asset` inspector'da `Accent Atlas` field → `Assets/Data/PatchAtlas_Rift_Fracture.asset` sürükle.
2. Map Designer'da L6 toggle ON.
3. `Regenerate`.

**Beklenen:**
- 0-3 adet rift sprite görünür (density=0.03 çok seyrek).
- Encounter slot'larından ≥4 tile uzak (encounterAvoidRadius=4).
- Rift'ler arası ≥12 tile mesafe (minDistance=12).
- Bazı seed'lerde 0 rift normaldir — sparse accent layer.

**Doğrulama:** Seed'i 99999 yap → farklı rift dağılımı veya 0 rift görmelisin. 12345'e geri dön → aynı dağılım (deterministic).

**Sorun:**
- Çok fazla rift → density yüksek olabilir. PatchAtlas_Rift_Fracture inspector'da density=0.03 doğrula.
- Encounter üstüne düşüyor → encounterAvoidRadius=4 ayarlı mı kontrol et.

---

## Aşama 4 — L5 Detail Decal Test (Yeni Atlas Create)

Archive'dan 6 decal kullanarak yeni atlas oluştur.

**Adımlar:**

**4.1. Atlas oluştur:**
1. Project window: `Assets/Data/` klasöründe sağ tık → `Create > RIMA > Map > Patch Atlas`.
2. Adı: `PatchAtlas_Detail_ShatteredKeep`.

**4.2. Patches list doldur (Inspector):**
`Patches` array size = 6 yap, her entry için:

| Sprite | density | minDistance | allowFlipX | allowFlipY | rotationJitter |
|---|---|---|---|---|---|
| `Assets/Art/_archive_faz1/Decals/decal_0_crack.png` | 0.18 | 2 | true | true | 30 |
| `Assets/Art/_archive_faz1/Decals/decal_1_moss_cluster.png` | 0.15 | 3 | true | false | 15 |
| `Assets/Art/_archive_faz1/Decals/decal_2_moss_trail.png` | 0.12 | 3 | true | false | 15 |
| `Assets/Art/_archive_faz1/Decals/decal_4_cross_crack.png` | 0.15 | 2 | true | true | 90 |
| `Assets/Art/_archive_faz1/Decals/decal_6_moss_curve.png` | 0.15 | 3 | true | true | 30 |
| `Assets/Art/_archive_faz1/Decals/decal_3_starburst.png` | 0.08 | 4 | true | true | 45 |

Her entry için `size`: 1, `tintMin/Max`: beyaz (1,1,1,1) bırak, `sortingOrderRange`: (0, 2).

**4.3. Atlas üst seviye field'lar:**
- `edgeBiased`: true
- `wallProximityFactor`: 1.2
- `featureMask`: null (boş)
- `featureMaskWeight`: 0
- `encounterAvoidRadius`: 2
- `centerPathDensityReduction`: 0.08

**4.4. RoomRecipe bağla:**
1. `RoomRecipe_ShatteredKeep_Combat_01.asset` inspector → `Decal Atlas` field → yeni atlas sürükle.

**4.5. Regenerate:**
1. Map Designer L5 toggle ON.
2. `Regenerate`.

**Beklenen:**
- Floor üzerinde yoğun küçük detay (crack, moss cluster, starburst).
- Wall'a yakın bölgelerde dense, oda merkezinde seyrek.
- Encounter slot etrafında 2-tile temiz buffer.
- Aynı sprite kümeleri arası min mesafe korunmuş — clumping yok.
- Rotation ve flip varyasyonu ile mirror artifact gizli.

**Sorun:**
- Atlas oluşturma menüsü yok → `Create > RIMA > Map > Patch Atlas` görünmüyorsa `PatchAtlasSO.cs` `[CreateAssetMenu]` attribute'una bak.
- Decal sprite'lar pembe → import settings: Pixel Per Unit 32, Filter Mode Point, Compression None olmalı.
- Decal görünmüyor ama Console temiz → density ya da sprite null olabilir, list entry'lerini gez.

---

## Aşama 5 — L3 Wall Overlay (PixelLab Asset Gen Sonrası)

Bu adım PixelLab Web UI Pro ile wall sprite üretimini gerektirir.

**5.1. Asset üret:**
1. `STAGING/asset_gen_asama1_batch.md` aç → Template C (Wall Brush) prompt'larını kullan.
2. PixelLab Web UI Pro'da üret:
   - Horizontal wall: 384x216, 4 varyant
   - Vertical wall: 424x632, 4 varyant
   - Corner: 341x341, 4 varyant (her köşe)
   - Doorway gap: 192x144, 2 varyant

**5.2. Import:**
1. Sprite'ları `Assets/Art/Tiles/F1/Wall/` altına kopyala.
2. Tüm sprite'ları seç, Inspector'da:
   - Pixel Per Unit: 32
   - Filter Mode: Point (no filter)
   - Compression: None
   - Sprite Mode: Single
3. Apply.

**5.3. WallBrushSet oluştur:**
1. Project: `Assets/Data/` sağ tık → `Create > RIMA > Map > Wall Brush Set`.
2. Adı: `WallBrushSet_ShatteredKeep`.
3. Inspector:
   - `horizontal`: 4 sprite array
   - `vertical`: 4 sprite array
   - `corner`: 4 sprite array (TL, TR, BL, BR sırası)
   - `doorwayGap`: 1 sprite seç (2 varyant arasından)
   - `biomeKey`: "ShatteredKeep" (string match)

**5.4. RoomRecipe bağla:**
1. `RoomRecipe_ShatteredKeep_Combat_01.asset` inspector → `Wall Brush Set` field → yeni set sürükle.

**5.5. Regenerate:**
1. Map Designer L3 toggle ON.
2. `Regenerate`.

**Beklenen:**
- Oda perimeter'ı Hades-style büyük organik wall brush ile kaplı.
- Tile-grid hissi gitmiş, organic siluet.
- Doorway konumlarında gap sprite kullanılmış.
- Corner'lar doğru rotasyon/seçim.

**Sorun:**
- Wall sprite'lar yanlış scale → import PPU=32 doğrula. Brush 384x216 → 12x6.75 unit gözükmeli.
- Doorway kapalı → `doorwayGap` slot atanmadı veya RoomRecipe doorway pozisyonu yok.
- biomeKey mismatch → "ShatteredKeep" exact string olmalı (case-sensitive).

---

## Aşama 6 — L4 Large Biome Patch (512x512 Web UI Pro)

Floor repetition'ı kırmak için büyük biome patch'leri.

**6.1. Asset üret:**
1. `STAGING/asset_gen_asama1_batch.md` Template E (Large Biome Patch) prompt'larını kullan.
2. Web UI Pro 512x512, 2-3 varyant üret (ör. büyük moss zone, kırılmış taş cluster).

**6.2. Import:**
1. `Assets/Art/Tiles/F1/Patches/` altına kopyala. Import settings: PPU 32, Point, None.

**6.3. Atlas seçeneği — iki yol:**

**A) Mevcut moss atlas'a ekle:**
1. `PatchAtlas_Moss_ShatteredKeep.asset` aç.
2. `Patches` array'e yeni entry ekle:
   - sprite: large patch
   - size: 4-8 (tile coverage)
   - density: 0.02
   - minDistance: 20
   - allowFlipX: true
   - allowFlipY: true
   - rotationJitter: 0 (büyük patch'lerde rotation artifact yapar)

**B) Ayrı atlas:**
1. `Create > RIMA > Map > Patch Atlas` → `PatchAtlas_LargeBiome_ShatteredKeep`.
2. Aynı entry değerleri.
3. RoomRecipe'da `Transition Atlas` slotuna bunu ata (Moss yerine geçer veya ek slot kullan — recipe inspector'da hangi slot uygunsa).

**6.4. Regenerate.**

**Beklenen:**
- 16x12 oda içinde 0-2 büyük patch görünür.
- Floor tile repetition kırılmış.
- Patch'ler walkable bölgelere düşüyor, gameplay'i bloklamıyor (görsel, collision değil).

**Sorun:**
- Patch çok sık → density 0.02'den düşür (0.01).
- Patch overlap → minDistance artır (25-30).

---

## Aşama 7 — Aşama 2 Voronoi Natural Features (Opsiyonel)

Procedural natural feature gen — water/cliff zone clustering.

**Adımlar:**
1. Project: `Create > RIMA > Map > Natural Feature Settings`.
2. Adı: `NaturalFeatures_ShatteredKeep_Water`.
3. Inspector:
   - `siteCount`: 64
   - `featureType`: Water
   - Diğer parametreler default.
4. `RoomRecipe_ShatteredKeep_Combat_01.asset` inspector → `Feature Settings` field → bu asset sürükle.
5. Map Designer penceresinde "Natural Features" foldout aç → `Generate` tıkla.

**Beklenen:**
- 1-3 organik water zone (Voronoi siteCenter cluster'larından).
- L5 detail decal density bu zone'lara yakın artar (FeatureMaskSO çarpımı).
- Encounter slot'larından kaçınır.

**Sorun:**
- Hiç feature yok → siteCount düşük olabilir, 96-128 dene.
- Feature tüm odayı kaplıyor → siteCount çok yüksek, 32-48 dene.

---

## Aşama 8 — "Doğal Görünüm" Success Checklist

ChatGPT spec'inden 8 madde. Her birini gözle doğrula:

- [ ] **Floor tile repetition gizlenmiş** — L2 variation pool + L4 oval patch breakup ile aynı tile'ın grid hissi yok.
- [ ] **Encounter slot etrafı temiz** — radius 2-4 tile buffer, decal/patch encounter spawn'ı bloklamıyor.
- [ ] **Center 4x4 gameplay arena sparse** — oyuncunun döndüğü merkez bölge görsel olarak temiz, hareket okunaklı.
- [ ] **Wall yakını organic dense** — duvara bitişik tile'larda moss/decal yoğun (wallProximityFactor>1).
- [ ] **Aynı decal sprite arası min mesafe** — clumping yok, her sprite kendi minDistance'ına uyuyor.
- [ ] **FlipX/Y varyasyon ile mirror artifact gizli** — aynı sprite'ın yan yana iki örneği farklı flip ile.
- [ ] **Rift accent sparse** — per-map 0-3 rift, fazlası "spam" hissi verir.
- [ ] **Wall perimeter Hades-style siluet** — L3 brush ile grid kareleri yerine organik kenar (PixelLab asset sonrası).

---

## Aşama 9 — Sorun Giderme (Quick Reference)

| Belirti | Olası neden | Çözüm |
|---|---|---|
| L4/L5 boş kalıyor | Atlas sprite null veya density=0 | PatchEntry inspector'da sprite ref + density kontrol |
| Patch wall üstüne düşüyor | Walkable filter çalışmıyor | Terrain.walkable=false wall cell'lerde mi? |
| Aynı sprite kümeleniyor | minDistance düşük | minDistance 4-6'ya çıkar |
| Encounter etrafı kirli | encounterAvoidRadius düşük | 3-5 yap |
| Center fazla yoğun | centerPathDensityReduction yüksek | 0.05'e indir |
| Tile seam grid görünüyor | L2 variant az veya L4 yok | L2 pool genişlet ya da L4 large biome 512 ekle |
| Wall sprite pembe | Material/import bozuk | PPU 32, Point, None re-import |
| Doorway kapalı | WallBrushSet doorwayGap null | Slot ata |
| Rift çok fazla | density>0.03 | Atlas density düşür |
| Console "missing reference" | Field assignment kopuk | RoomRecipe ve atlas'larda null field ara |

---

## Aşama 10 — Before/After Screenshot Test

Aynı tileset, farklı seed → farklı natural variation görünmeli.

**Adımlar:**
1. Game window aç (`Window > General > Game`).
2. Phase1_ProceduralMap_Test sahnesinde Play tıkla.
3. Kamera oda merkezinde olacak şekilde ayarla. Aspect: Free veya 16:9.
4. F12 ile screenshot al → `screenshot_seed12345.png` adıyla `STAGING/screenshots/` altına kaydet.
5. Stop. Map Designer'da `Seed`: 54321 yap, `Regenerate`.
6. Play tekrar, F12 → `screenshot_seed54321.png`.
7. İki screenshot'ı yan yana karşılaştır:
   - Floor tile dağılımı farklı olmalı (L2 variation seed-driven).
   - Moss patch konumları farklı.
   - Rift varsa farklı yer veya yok.
   - Decal kompozisyonu farklı.
   - Wall layout aynı kalır (RoomRecipe sabit, sadece overlay seed-driven).

**Beklenen:** İki seed arası "doğal" his korunur, "random spam" hissi olmaz. Her ikisinde de Aşama 8 checklist'i geçer.

**Sorun:**
- İki seed aynı görünüyor → seed orchestrator'a geçirilmiyor olabilir. `MapLayerOrchestrator.Paint(..., seed)` argümanını izle.
- Seed değiştirince crash → null reference, atlas field'larından biri kopmuştur.

---

## Notlar

- **Asset yokluğunda fallback:** Tüm L3-L6 layer'lar null tolerant. Field boşsa orchestrator o layer'ı atlar, error fırlatmaz.
- **Test sırası önerisi:** Aşama 0-4 PixelLab gerektirmez, hemen yapılabilir. Aşama 5-6 asset gen sonrası. Aşama 7 opsiyonel (Aşama 2 LIVE).
- **Deterministic doğrulama:** Aynı seed + aynı RoomRecipe + aynı atlas referansları → her zaman aynı output. Eğer değişirse seed pipeline'ında bug var.
- **Performance:** 16x12 oda için 6 layer paint < 50ms beklenir. Daha uzun sürerse profile et.
