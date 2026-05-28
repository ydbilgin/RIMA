# artofsully Voronoi / Cliff / Grass-Mask Analizi - Codex

**Source:** https://x.com/artofsully/status/2055082714559029683  
**Local evidence:** `STAGING/twitter_research/2055082714559029683/`  
**MP4:** 960x720, 30 fps, 28.42 sn  
**RIMA baglam:** Karar #143 Asama 2 water/elevation Wang entegrasyonu

---

## 0. Kisa Verdict

**Codex verdict:** Bu pipeline RIMA Asama 2 icin **mutlaka entegre edilmeli**, ama sadece water/elevation feature zone seviyesinde; Asama 1 duz floor pipeline'i kilitlenmeden ana map generator'a sokulmamali, cunku Voronoi river + bank mask sistemi #143-B'nin water/elevation ozel-edge rolunu cok iyi tamamliyor.

---

## 1. Gorsel desifrasyon

### 1.1 Timeline gozlemi

- `frame_001` ve `frame_002`: top-down generator preview. Turkuaz river network net olarak cell boundary gibi davranan buyuk polygon bolgeleri kesiyor.
- `frame_003`: river kenarinda cliff mass ile su arasinda yumusatilmis bank gorunuyor; keskin grid yerine organik erosion edge var.
- `frame_004` ve `frame_005`: su kenarinda beyaz/turkuaz foam band + sari zemin ustunde yesil mask. Mask hard edge degil, noisy alpha gibi dagiliyor.
- `frame_006`: character/camera controller testi. Ground seviyesinde grass mask vertex/terrain blend gibi dusuk frekansli, decal gibi tekil sprite degil.

### 1.2 Voronoi cell yapisi gorunuyor mu?

Evet, ilk iki frame'de river network cok guclu bicimde Voronoi dual graph izlenimi veriyor:

- River path'ler polygon cell boundary boyunca akiyor.
- Junction'lar 3-way / 4-way cell meet point gibi.
- Width sabit degil; bazi segmentlerde lake/pool genislemesi var.
- Cell iclerinde mesa/cliff props kalmis; boundary river, interior elevation olarak ayrilmis.

RIMA icin dogrudan kopya: terrain'i her tile icin Voronoi polygon olarak boyamak degil, **feature skeleton** olarak Voronoi edge graph uretmek.

```csharp
// RIMA icin hedef veri:
public sealed class NaturalFeatureGraph {
    public List<Vector2> sites;
    public List<FeatureEdge> edges;      // river / cliff / crack / circuit
    public List<FeatureJunction> nodes;  // confluence / split
}
```

### 1.3 Cliff smoothing nasil uygulanmis?

Gorsel kanit shader + geometry/mesh terrain karisimi gibi duruyor:

- Cliff faces 3D mesh/terrain yuzeyi; sadece tile edge degil.
- River kenarindaki dik cliff spiky peaks azaltilmis, suya yaklastikca slope daha soft.
- Su yuzeyi kenarinda foam/alpha band var; bu shader veya material blend katmani.
- Cliff rock detail hala textured mesh olarak duruyor; tamamen decal overlay degil.

RIMA 2D icin bu sonucu taklit etmenin dogru yolu mesh deform degil, **Wang edge + smooth overlay decal** kombinasyonu.

### 1.4 Vertex paint mask hangi LOD'da?

Mask high-density grass tile degil; terrain material blend gibi calisiyor:

- Yesil alanlar river bank proximity ile korele.
- Tekil tuft sprite tekrari okunmuyor.
- Ground seviyesinde noisy broad coverage var.
- Uzakta LOD gibi diffuse patch, yakinda soft texture blend.

RIMA analogu: `Texture2D alpha mask -> density multiplier` ve ustune L5 detail decal scatter.

```csharp
float riverFactor = DistanceCurve(distanceToRiver);
float paintFactor = manualMask.GetAlpha(cellUv);
float noiseFactor = Mathf.PerlinNoise(x * frequency, y * frequency);
float density = baseDensity * riverFactor * Mathf.Lerp(noiseFactor, paintFactor, manualMaskWeight);
```

---

## 2. RIMA Karar #143 Asama 2 entegrasyon

### 2.1 Voronoi -> water zone placement

Kullanilabilir, ama grid'in yerine gecmemeli. RIMA grid gameplay icin kalir; Voronoi sadece water/cliff feature mask uretir.

Onerilen akis:

```csharp
RoomData room = ProceduralRoomGenerator.Generate(recipe, seed);
VoronoiSites sites = JitteredSites(room.bounds, recipe.naturalFeatureSeed);
FeatureGraph graph = VoronoiFeatureBuilder.Build(sites, room.bounds);
WaterMask water = RiverMaskRasterizer.Rasterize(graph.SelectedEdges, room.gridSize, widthCurve);
ElevationMask cliffs = CliffMaskBuilder.FromWaterEdges(water, room.walkable);
```

Karar:

- Uniform site grid + jitter en iyi secim. Tam random sites bazi odalarda okunurlugu bozar.
- Biome'lar komple Voronoi cell olarak generate edilmemeli. Asama 2 sadece `water_zone`, `cliff_drop_zone`, `bank_zone` uretsin.
- Map Designer'da seed lock + manuel erase/paint desteklenmeli.

### 2.2 Cliff smoothing -> Pair E Wang painter

Pair E (`rubble<->cliff_drop`) icin ek **edge smoothing pass** gerekli.

Sebep:

- Sadece 16 Wang tile cliff kenarini readable yapar, ama organic river-adjacent cliff hissini tek basina vermez.
- artofsully klibinde cliff edge suya yakin yerde hem shape hem material olarak soft.
- RIMA pixel art'ta bunu `edge soften overlay` cozebilir.

Uygun RIMA pass:

```csharp
public sealed class FeatureEdgeSmoothingPass {
    public float cliffSoftRadiusCells = 1.5f;
    public PatchAtlasSO cliffSoftenDecals;
    public Texture2D bankMask;

    public void Paint(RoomData room, FeatureMask water, FeatureMask cliff) {
        // 1. Wang paints hard semantic transition.
        // 2. Overlay pass samples distance-to-feature.
        // 3. Decals only land on walkable floor cells.
    }
}
```

### 2.3 Vertex paint grass mask -> L5 DetailDecalPainter

Karar #143-E edge-biased density zaten wall proximity kullaniyor. Asama 2 icin ayni formulu **river bank proximity** ile genisletmek gerekir.

```csharp
float DensityForCell(Vector2Int cell, RoomData room) {
    if (!room.walkable[cell.x, cell.y]) return 0f;

    float wall = WallProximityCurve(cell);
    float river = RiverBankProximityCurve(cell);
    float manual = ManualMaskAlpha(cell);
    float noise = PerlinMask(cell);

    return baseDensity * Mathf.Max(wall, river) * Mathf.Lerp(noise, manual, manualMaskWeight);
}
```

L5 icin yeni parametreler:

- `FeatureMaskSO riverBankMask`
- `AnimationCurve bankDistanceDensityCurve`
- `float manualMaskWeight`
- `float perlinFrequency`
- `float maxCenterDensity`

### 2.4 3 spesifik Codex implementation task

#### Task A - NaturalFeatureGraph + Voronoi Water Mask

**Effort:** 4-8 saat  
**Scope:** new feature generator, no visual painter refactor.

Files:

```text
Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs
Assets/Scripts/MapDesigner/VoronoiWaterFeatureGenerator.cs
Assets/Scripts/Data/NaturalFeatureSettingsSO.cs
Assets/Tests/EditMode/VoronoiWaterFeatureGeneratorTests.cs
```

Acceptance:

- 200x200 grid icin deterministic water mask uretir.
- Uniform+jitter site mode vardir.
- `RoomData.walkable` disina feature yazmaz.
- Map Designer debug overlay JSON/export destekler.

#### Task B - FeatureEdgeSmoothingPass for Pair E/F

**Effort:** 4-8 saat  
**Scope:** Wang output ustune overlay smoothing.

Files:

```text
Assets/Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs
Assets/Scripts/Data/FeatureEdgeSmoothingProfileSO.cs
Assets/Editor/RimaMapDesignerWindow.cs
```

Acceptance:

- Pair E cliff_drop ve Pair F water_pool icin ayri profile calisir.
- Wang tile semantic mask degismez; sadece overlay/decal cikar.
- `walkable == false` cell ustune L5/L6 decal dusmez.

#### Task C - MaskTextureSO + DetailDecalPainter Integration

**Effort:** 4-8 saat  
**Scope:** vertex paint analog.

Files:

```text
Assets/Scripts/Data/FeatureMaskSO.cs
Assets/Scripts/MapDesigner/DetailDecalPainter.cs
Assets/Scripts/MapDesigner/PatchOverlayPainter.cs
Assets/Editor/RimaMapDesignerWindow.cs
```

Acceptance:

- `Texture2D alpha` density multiplier olarak kullanilir.
- Perlin procedural mask + manual paint mask birlikte calisir.
- River bank distance curve L5 scatter'i artirir.
- Seed deterministic; ayni seed ayni decal layout verir.

---

## 3. LaurethStudio pipeline kazanci

### 3.1 STUDIO_KARAR_003'e eklenecek teknikler

3 teknik tek abstraction altinda toplanabilir: **Natural Feature Placement**.

- Voronoi rivers: structure-disiplinli organic path skeleton.
- Cliff smoothing: semantic edge'i gorsel edge'e yumusatan post-pass.
- Vertex paint mask: feature proximity'den material/detail density ureten mask.

Universal 6-layer icinde yeri:

```text
L1/L2 = structural floor grid
FeatureGraph = natural path/edge skeleton
L4 = broad feature transition brush
L5 = feature-proximity detail decals
L6 = sparse special accents
Wang = only hard semantic water/elevation boundary
```

### 3.2 Universal pattern

Ortak pattern:

```text
Sites -> Organic Graph -> Raster Mask -> Semantic Boundary -> Visual Smoothing -> Detail Density
```

Oyunlara transfer:

| Game | FeatureGraph | Semantic edge | Mask/detail |
|---|---|---|---|
| RIMA | river / cliff / rift crack | water, cliff_drop | moss, wet bank, rift dust |
| CircuitBreaker | circuit path / power trace | conductive / blocked | sparks, scorch, glow |
| Caterpillar | leaf vein / moisture line | edible / blocked / stem | dew, bite marks, pollen |

### 3.3 STUDIO_KARAR_003 revize 1 cumle

**STUDIO_KARAR_003 ek kural:** "Natural feature placement her oyunda `Sites -> Organic Graph -> Raster Mask -> Semantic Boundary -> Visual Smoothing -> Detail Density` hattini kullanir; Wang sadece hard semantic boundary icindir, organik zenginlik L4-L6 mask/overlay pass'lerinden gelir."

---

## 4. Voronoi production-ready library

### 4.1 Kisa oneriler

**RIMA icin onerilen yol:** once kendi lightweight jittered-grid nearest-site rasterizer; sonra gerekirse MIConvexHull/csDelaunay ekle.

Sebep:

- Asama 2 ihtiyaci full Voronoi polygon mesh degil, grid mask ve feature graph.
- 200x200 grid icin nearest-site rasterizer yeterli ve deterministic debug kolay.
- Production risk dusuk: dependency yok, Unity IL2CPP/asmdef sorunu yok.

### 4.2 Library adaylari

| Library | Durum | Arti | Risk |
|---|---|---|---|
| MIConvexHull | NuGet `1.1.19.1019`, .NET Standard 1.0, MIT | Delaunay/Voronoi mesh destekli, mature | Unity package import/asmdef test ister |
| csDelaunay | MIT, .NET Standard 2.0, Lloyd relaxation | Game map bolge uretimi icin uygun | Repo activity sinirli, no NuGet release |
| unity-delaunay | Unity odakli, MIT, author production-suitable diyor | Unity folder yapisi hazir | Daha eski, destruction demo bagaji var |
| RafaelKuebler/DelaunayVoronoi | MIT, 5000 point screenshot | Basit Bowyer-Watson referans | Library polish dusuk, direct vendor gerekir |

Kaynaklar:

- MIConvexHull GitHub: https://github.com/DesignEngrLab/MIConvexHull
- MIConvexHull NuGet: https://www.nuget.org/packages/MIConvexHull
- csDelaunay GitHub: https://github.com/jfg8/csDelaunay
- unity-delaunay GitHub: https://github.com/OskarSigvardsson/unity-delaunay
- DelaunayVoronoi GitHub: https://github.com/RafaelKuebler/DelaunayVoronoi

### 4.3 Uniform vs jittered Voronoi

RIMA icin:

- **Uniform grid sites + jitter:** secilmeli.
- **Pure random sites:** sadece non-combat overworld veya background mask icin.
- **Lloyd relaxed sites:** cok temiz/polished olur, dungeon kirikligi azalabilir.

```csharp
Vector2 SiteForCell(int ix, int iy, float spacing, float jitter, uint seed) {
    Vector2 center = new Vector2((ix + 0.5f) * spacing, (iy + 0.5f) * spacing);
    Vector2 offset = Hash2(seed, ix, iy) * spacing * jitter;
    return center + offset;
}
```

### 4.4 Performance tahmini

200x200 cell map = 40.000 samples.

Pragmatik tahmin:

- Jittered nearest-site rasterizer, ~64-256 sites, spatial bucket ile: **1-5 ms editor**, **<1 ms Burst/DOTS gerekmeden optimize C# olasi**.
- Naive all-sites nearest, 40.000 x 256 = 10.2M distance check: editor'da **5-20 ms** araligi makul.
- Full Delaunay/Voronoi polygon generation: site sayisina bagli, 256-1024 site icin genelde **single-digit ms to tens of ms** editor hedefi; RIMA bunu runtime her frame degil, generation-time calistirmali.

---

## 5. Cliff smoothing teknikleri

### 5.1 Tile-based

**Teknik:** Wang corner + extra edge soften tile variant.

Artisi:

- Pixel-perfect ve collision/semantic net.
- Pair E pipeline'a dogrudan uyar.
- Runtime ucuz.

Eksisi:

- 16 tile pattern tekrar edebilir.
- River-adjacent organic bank icin variant sayisi artar.
- Soft gradient pixel art'ta zor kontrol edilir.

### 5.2 Shader-based

**Teknik:** SDF distance to cliff line + height/material blend.

Artisi:

- Smooth edge en iyi burada.
- Distance curve ile wet/dust/foam kolay.
- Dynamic feature mask icin esnek.

Eksisi:

- RIMA 2D pixel art + Tilemap pipeline'da shader complexity artar.
- URP 2D sorting/mask/debug maliyeti var.
- Pixel-perfect readability bozulabilir.

### 5.3 Sprite-based

**Teknik:** Cliff base Wang tile + overlay smooth decal.

Artisi:

- Karar #143 L4/L5 overlay mantigina en uyumlu.
- Pixel art kontrolu yuksek.
- Asama 2 feature edge ile sinirli tutulabilir.
- Map Designer'da manuel duzeltme kolay.

Eksisi:

- Extra sorting/layer discipline ister.
- Fazla overlay combat floor'u kirletebilir.
- Decal atlas uretim/QA maliyeti var.

### 5.4 RIMA icin secim

**En uygun:** Tile-based semantic boundary + sprite-based smoothing overlay.

```text
1. CornerWangPainter paints cliff/water hard boundary.
2. FeatureEdgeSmoothingPass computes distance-to-boundary.
3. L4/L5 overlay sprites soften bank/cliff visuals.
4. Decoration filters keep gameplay cells readable.
```

Shader-based SDF sadece ileri faz debug/large water polish icin defer edilmeli.

---

## 6. Vertex paint mask analog (RIMA 2D)

### 6.1 3D vertex paint -> 2D analog

artofsully 3D vertex paint muhtemelen terrain material blend icin kullaniliyor. RIMA 2D analog:

- `Texture2D alpha` mask asset
- per-cell/per-pixel density multiplier
- manual Map Designer paint layer
- Perlin noise procedural breakup
- river/cliff/rift proximity curves

### 6.2 Data model

```csharp
[CreateAssetMenu(menuName = "RIMA/Map/Feature Mask")]
public sealed class FeatureMaskSO : ScriptableObject {
    public Texture2D alphaMask;
    public Vector2Int gridSize;
    public float worldUnitsPerPixel = 1f;
    public bool invert;
    public AnimationCurve remap = AnimationCurve.Linear(0, 0, 1, 1);
}
```

```csharp
[Serializable]
public sealed class DetailDensityProfile {
    public float baseDensity = 0.08f;
    public AnimationCurve riverBankDistanceCurve;
    public AnimationCurve wallDistanceCurve;
    public FeatureMaskSO manualMask;
    public float manualMaskWeight = 0.65f;
    public float perlinFrequency = 0.12f;
}
```

### 6.3 Map Designer UI

Gerekli kontroller:

- Mask tab: `Water`, `Cliff`, `Grass/Bank`, `Rift`.
- Brush mode: paint/erase/smooth.
- Preview: alpha mask overlay + density heatmap.
- Seed lock: procedural mask regenerate, manual paint korunur.
- Export: `FeatureMaskSO` asset + JSON debug.

### 6.4 Codex implementation outline

```text
Step 1: FeatureMaskSO ekle.
Step 2: Map Designer mask texture create/load/save UI ekle.
Step 3: DetailDecalPainter density sampling API ekle.
Step 4: PatchOverlayPainter icin optional mask multiplier ekle.
Step 5: Test scene: river bank mask -> moss/wet detail density.
```

Risk kontrol:

- Mask gameplay collision uretmez; sadece visual density.
- Walkable filter her zaman son gate olur.
- Center arena max density clamp zorunlu.

---

## 7. Karar #143 ek maddesi onerisi

**Karar #143-J (aday):** Asama 2 water/elevation feature zone'lari icin `NaturalFeatureGraph -> RasterMask -> Wang semantic boundary -> L4/L5 smoothing/detail mask` pipeline'i kullanilir; Voronoi/jittered graph sadece feature skeleton uretir, temel combat grid ve walkable semantic Karar #143 L1-L2'de kalir.

**Karar #143-K (aday):** L5 detail density sadece wall proximity degil, `feature proximity` ile de carpilanir; river bank, cliff rim, rift crack gibi feature mask'ler Map Designer'da manual paint + procedural Perlin olarak duzenlenebilir.

---

## 8. Net sonuc

- artofsully klibi RIMA icin gorsel referans degil, **pipeline referansi**.
- Voronoi rivers RIMA'da feature graph olarak alinmali.
- Cliff smoothing Pair E/F icin semantic Wang + overlay pass olarak uygulanmali.
- Vertex paint mask 2D'de `FeatureMaskSO + DetailDecalPainter density` olarak uygulanmali.
- Studio tarafinda bu pattern RIMA, CircuitBreaker ve Caterpillar icin ortak natural feature abstraction'a donusebilir.

