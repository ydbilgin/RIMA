# KARAR #123 -- Natural Map Polish Additions (PROPOSED)

Date: 2026-05-13
Author: rima-design (Opus)
Builds on: Karar #116 (Tile Transition Quality), #118 (Hybrid Tile Composition),
#121 (Scatter Brush), #122 (Layered Room Generation)
Status: PROPOSED -- awaiting orchestrator lock
Research input: STAGING/natural_map_techniques_research.md NOT DELIVERED within wait
window; proposal authored from design judgment + palette + locked plan context.

---

## DESIGN PRINCIPLE

Mevcut plan zaten "structural natural" katmanlarini cozer:
- Karar #122 = oda silueti organik (CA cave)
- Karar #118 = transitions Wang, base layer plain
- Karar #121 = grid-bagimsiz scatter (stones/moss/rubble)
- Karar #116 = autotile raggedness + 3+ variant

Eksik katman: **micro-variation + atmosferik isik**. Tile sheet'ler ne kadar iyi
olursa olsun, grid hala "tekrar eden hucreler" hissi verir cunku:
1. Her tile ayni rotasyon/aynaya yerlesir -> goz pattern yakalar
2. Tile renkleri statik -> ortam aydinligi farkli alanlarda esit
3. Hucre sinirlari sert -> sub-tile feature yok
4. Goz "rasgele" gordukten sonra hala flat -> light/shadow dynamics yok

TOP 5, bu 4 acigi en dusuk gen + en yuksek "natural delta" ile kapatir.

---

## TOP 5 POLISH ADDITIONS (priority order)

### 1. Per-Tile Random Rotation + Flip -- Priority P0

**What**: Floor RuleTile'larina `TileFlags.LockTransform` kaldirilip
GameObject seviyesinde her hucrede deterministik (cell.GetHashCode())
0/90/180/270 rotasyon + horizontal flip uygula. Wall ve transition tile'larina
DEGIL -- sadece interior floor.

**Why for RIMA**:
- 3 variant tile + 8 transform = **24 effective tile variant** (free)
- Charcoal warm grey #2C2A2A pattern repetition'i goz seviyesinde ~%70 kirar
- Karar #116 raggedness ≥40% ile birlesince "el yapimi natural cave" hissi
- Zero gen cost, zero runtime cost (deterministik hash)
- En yuksek ROI noktasi -- bu yapilmazsa diger polish'lar bos kalir

**Cost**: gen=0, code=2h (RuleTile.Output OverrideTransform, or post-process
script that sets `tilemap.SetTransformMatrix` per cell after Generate)

**Implementation**:
- `RimaWangFloorTile.cs` (mevcut RuleTile) icinde `GetTileData` override:
  `tileData.transform = Matrix4x4.TRS(...)` -- hash from `position.GetHashCode()`
- Wall tile'larinda **kapali** (silhouette korunmali, Karar #116 wall edge)
- LayeredRoomGenerator.cs sonunda `ApplyRandomTransforms()` step
- Test: ayni seed iki kez Generate edilince ayni rotasyon pattern (deterministic)

**Conflicts**: NONE. Karar #116 raggedness'i tamamlayici.

---

### 2. Per-Instance HSV Tint Jitter -- Priority P0

**What**: Floor tile'lara ±%4 V (value) ve ±%3 H (hue) jitter, hucre bazli
deterministic. SpriteRenderer.color veya custom shader vertex color uzerinden.
Decals ve walls degil -- sadece floor tile'lar. Toplam %8 brightness range
yeterli (palette LOCKED kirilmasin).

**Why for RIMA**:
- Charcoal floor #2C2A2A tek deger -> kapasi ile ayni gri -> goz cansiz
- ±%4 V "ay isigi/sis lekesi" hissini ucretsiz uretir
- Karar #116g drop shadow oval ile birlesince derinlik artar
- Palette LOCKED'i kirmaz: ±%4 hala palette tolerance icinde
- Rift cyan #00FFCC accent decal'lar parlak kalirken floor'a "ortamsal"
  varyans katar
- Karar #121 scatter (stones/moss) jitter aralarinda gec; aksini orgun yapar

**Cost**: gen=0, code=3h (custom URP shader veya per-tile SpriteRenderer
color set; LayeredRoomGenerator post-pass)

**Implementation**:
- Floor RuleTile -> `tileData.color = HSVJitter(position.GetHashCode())`
  formuluyle hesapla; range hard-coded ±0.04V/±0.03H
- Decal/wall tilemap'larina UYGULAMA (LayerMask check)
- Rift accent tile'larinda **kapali** (cyan parlamasini koruma)

**Conflicts**: PALETTE LOCKED'a yakin -- jitter range hard-coded ±%4'te
clamp edilirse ihlal yok. Doc'ta clamp belirtilmeli.

---

### 3. URP 2D Ambient Light + Torch Flicker Point Lights -- Priority P0

**What**: Room generation sonunda 2 layer light spawn:
- (a) Global 2D Light, color = warm dim (#3a3030), intensity 0.45 -- ambient
- (b) Torch point lights, color #C4682A (palette torch), intensity 1.0,
  radius 4-6 tile, **flicker animator** ±%15 intensity 0.1-0.3s noise.
  Konum: Karar #122'de prop placement (wall sconce anchor)

**Why for RIMA**:
- Karar #116 zaten URP 2D Light istiyor -> bu sadece **kullanim spec**
- Ambient torch flicker dungeon hissinin %50'si (sabit isik =
  cartoony, flickering = roguelite)
- Aydinlik dengesizligi: torch yakini sicak, koseler soguk #7BA7BC tarafa
  donuk -> palette aksent dengesi aktif
- Karar #121 scatter rubble/moss flicker icinde gorunup kaybolarak natural
- Faz 1.5 prop pipeline'inda sconce slot var (PropPlacer)

**Cost**: gen=0 (torch sprite zaten var), code=4h
- `RimaTorchFlickerController.cs` (Perlin noise * intensity)
- `LayeredRoomGenerator.SpawnLighting()` -- sconce prop placement noktasinda
  Light2D child instantiate
- 1 prefab `TorchSconce.prefab` (sprite + Light2D + flicker script)

**Implementation**:
- Light2D component (URP 2D), Falloff 0.5, Inner Radius 0
- Flicker: `intensity = baseIntensity * (1 + Mathf.PerlinNoise(Time.time*3, 0)*0.15f)`
- Ambient global Light2D scene-level prefab
- Toggle: RoomConfig.lightingEnabled (test mode bypass)

**Conflicts**: NONE. Karar #116 ile *bire bir uyumlu*, sadece operationalize.

---

### 4. Sub-Tile Sprite Fragments (Cross-Cell Overlays) -- Priority P1

**What**: 8-12 sprite "fragment" asset (cracked stone 2x3 cell, root vine
3x1 cell, scattered bone pile 2x2 cell, leaf cluster 1x3 cell). PropPlacer
gibi ama scale + transparency + sorting order ile floor uzerine, hucre
grid'ini aciktan **kesen** sprite'lar. 5-12 adet per oda.

**Why for RIMA**:
- Karar #121 scatter "tek hucre item" -> hala grid hissi var
- Cross-cell fragment grid'i **gorsel olarak yok eder** (most powerful
  "natural" trick)
- 8-12 sprite = ~80 gen credit (mevcut 300 budget'in %27'si) -- karsiligi
  yuksek
- Dungeon icin: ayrismis tas, kok, kemik, yaprak -- her oda farkli mix
- Karar #122 per-room template'e ekle: KeepCombat = bone+stone,
  GardenCombat = vine+leaf
- Drop shadow oval (Karar #116g) zaten her sprite'a uygulanir -> derinlik

**Cost**: gen=80 credit, code=2h
- 8 fragment sprite PixelLab Tileset Pro mode (1 prompt per fragment,
  64-96px varying)
- `FragmentScatter.cs` (PropPlacer'a benzer, ama large sprite + rotation/flip,
  AnchorZone "interior floor only")
- `RoomConfig.fragmentSpriteList` + `fragmentCountPerRoom` field

**Implementation**:
- PixelLab prompt: "weathered stone fragment, 64x96 px, top-down view,
  charcoal grey #2C2A2A + light grey crack lines #4A3F3F, drop shadow built-in,
  fits seamlessly on cave floor"
- Scatter logic: poisson disk sampling, min distance 3 tile, max 8 per oda
- Sort order: floor + 1, decal + 2 (decals ustte)

**Conflicts**: gen 80 credit big chunk; ~220 kalir. Karar #121 ile
**tamamlayici** (scatter = single cell, fragment = multi cell). Code path
ayri olmali.

---

### 5. Ambient Dust Mote Particle (Volumetric Suggestion) -- Priority P1

**What**: Tek ParticleSystem prefab `AmbientDustMotes.prefab`. Sayi 30-50,
size 0.05-0.15, color warm dim #C4682A * alpha 0.15, slow upward
velocity (0.1-0.3 u/s) + sin-wave horizontal drift, lifetime 8s. Oda
center'da spawn, oda bounds icine kisitla.

**Why for RIMA**:
- Tek prefab, 1 instantiate per oda -- runtime cost minimal
- "Hava var" hissi -> roguelite atmosferinin ikinci anahtari (ilk = torch
  flicker)
- Torch noktalarinda dust **kontrast** -> isik shaft suggestion (free volumetric)
- Rift oda turunde dust color = #00FFCC cyan (1 satir variant) -> tema farki
- Karar #122 oda turune gore RoomConfig.dustColor override

**Cost**: gen=0, code=2h
- `AmbientDust.prefab` (ParticleSystem, additive blend)
- LayeredRoomGenerator.SpawnAmbience() step
- RoomConfig.dustEnabled + dustColor + dustDensity fields

**Implementation**:
- Shape: Box, oda bounds matched (LayeredRoomGenerator runtime resize)
- Emission rate = dustDensity * area; max 50 (perf cap)
- Texture: tek 8x8 px soft circle additive (built-in Unity Default-Particle ok)
- Toggle Quality Settings'te (low: dustEnabled=false)

**Conflicts**: NONE. URP 2D pipeline particle uyumlu.

---

## REJECTED additions (yer acan trade-off)

- **Animated rift crack tiles (8-frame loop)**: Cok pahali (gen ~50, code 4h,
  8-frame tilemap animation Unity Tilemap Animated Tile API + her crack tile
  icin frame seti). Karar #121 scatter "rift crack sprite" zaten ortusur.
  Faz 2.0'a ertelenir.
- **Post-process Bloom on accent decals**: URP 2D bloom global -> tum sahne
  parlar, palette LOCKED kirilir (charcoal #2C2A2A bloom ile sutle gri'ye
  kayar). Rejected; rift accent zaten parlak renk #00FFCC kontrast verir.
- **Camera breath idle drift**: Roguelite combat'ta confusing, motion sickness
  risk. Yargi: combat tempo'ya zarar verir.
- **God rays from ceiling cracks**: 2D top-down'da yon yanlis (ust kamera ->
  vertical light shaft cisim olarak yanlis projecte). Side-view olsa P1.
- **Vignette + color grading**: URP 2D Pixel Perfect ile vignette pixel grid'i
  kirar; pixel perfect Karar #116 onceligi. Reject.
- **Vegetation tuft prefab at wall-floor edge**: Karar #122 transition layer'a
  girer, Karar #118 hibrit kompozisyona dahil edilmeli ama #123'ten cikar
  cunku #118'in zaten kapsadigi alan.
- **Hand-curated "spot" tiles at Perlin maxima**: Per-tile rotation (#1) +
  HSV jitter (#2) + fragment (#4) toplaminda ayni effect, daha az ozel-case.
  Rejected as redundant.

---

## INTEGRATION ORDER

Mevcut Codex iter 2 task'i (oval brush + canvas fix + multi-layer + library)
**code-only Editor tooling fix**. Iter 2'ye RUNTIME polish eklemek scope
genisletir + iter 2 PASS riskini artirir.

**Iter 2'ye EKLEME** (zero-risk, gen-free, hizli):
- (1) Per-Tile Random Rotation + Flip -- 2h, mevcut RuleTile dosyalarinda
  GetTileData override, scope minik
- (2) Per-Instance HSV Tint Jitter -- 3h, ayni RuleTile path, palette clamp

Toplam +5h, iter 2 onceden ~14 partial QC plan'a 2 yeni partial ekler:
- Partial 15: Per-Tile Transform Determinism (ayni seed -> ayni rotasyon)
- Partial 16: HSV Jitter Range Clamp (±%4 V, ±%3 H, no overflow)

**Iter 3 olarak AYRI dispatch** (runtime, prefab, particle, lighting):
- (3) URP 2D Lighting + Torch Flicker -- 4h, prefab + script + LayeredRoomGenerator hook
- (4) Sub-Tile Sprite Fragments -- 2h code + 80 gen credit + PixelLab dispatch
- (5) Ambient Dust Mote Particle -- 2h prefab + script

Toplam iter 3 = ~8h code + 80 gen credit. QC plan ayri (5 partial:
lighting spawn, flicker amplitude, fragment placement, dust emission, perf).

---

## DECISION SUMMARY

DECISION: 5 polish addition onerilir. **2 tane (rotation+flip, HSV jitter)
Codex iter 2'ye eklenir** (gen=0, low-risk, RuleTile path). **3 tane
(lighting+flicker, fragments, dust) Codex iter 3 ayri dispatch** (runtime
+ gen 80 credit).

RATIONALE:
- Iter 2 mevcut scope (Editor tool QC) gen-free polish'lari ucretsiz alir
- Iter 3 runtime polish ayri risk profili (lighting/particle/prefab) =
  ayri QC plan + ayri PASS gate
- Gen 80 credit = mevcut 300 budget'in %27'si; 220 kalir (animation +
  enemy + boss icin yeterli)
- 5 gunluk timeline'da: Iter 2 (gun 1-2), Iter 3 (gun 3-4), QC + polish
  (gun 5)

TRADE-OFF:
- Rejected: bloom, vignette, god rays, camera drift, animated rift -- Faz 2.0
- Gen 80 credit fragment'lara ayrildi -> ileride animation gen kisilirsa
  ilk fragment'tan kesilir (per-tile transform/HSV korunur, zero gen)

SYSTEMS AFFECTED:
- RuleTile classes (RimaWangFloorTile.cs ve variantlar)
- LayeredRoomGenerator.cs (SpawnLighting + SpawnAmbience + FragmentScatter steps)
- RoomConfig ScriptableObject (5 yeni field: lightingEnabled, dustColor,
  dustDensity, fragmentSpriteList, fragmentCountPerRoom)
- 3 yeni prefab: TorchSconce, AmbientDust, FragmentScatter root
- 8-12 yeni PixelLab fragment sprite

CONFLICTS WITH LOCKED RULES?: **PARTIAL**
- (2) HSV Jitter palette LOCKED'a yakin. Hard clamp ±%4V/±%3H rule yazilirsa
  ihlal yok. Doc'ta clamp zorunlu belirtilmeli.
- Diger 4 addition NONE.

ORCHESTRATOR NEXT STEP:
1. Bu dosyayi review et, Karar #123 PROPOSED -> LOCKED'a cek (MASTER_KARAR_BELGESI guncelle, rima-doc spawn)
2. Codex iter 2 task'ina partial 15 + 16 EKLE (per-tile transform + HSV jitter)
3. Codex iter 3 yeni dispatch hazirla (lighting + fragments + dust); rima-asset
   spawn fragment sprite PixelLab promptlari icin (8-12 prompt, Tileset Pro mode)
4. QC plan'a yeni partial'lar ekle (rima-doc spawn)
5. Research dosyasi (`STAGING/natural_map_techniques_research.md`) gelirse
   bu dosya cross-check yapilip ek addition varsa Karar #123b olarak ayri
   onerilebilir
