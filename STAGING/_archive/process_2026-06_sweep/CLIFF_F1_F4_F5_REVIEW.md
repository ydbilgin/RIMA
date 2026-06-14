# Cliff F1 + F4 + F5 Opus Review

**Date:** 2026-05-27 LATE
**Reviewer:** Opus (audit only, no code mutation)
**Sonnet outputs reviewed:**
- `STAGING/F1_ADAPTIVE_CLUSTER_FILTER_DONE.md`
- `STAGING/F4_DUST_PARTICLE_DONE.md`
- `STAGING/F5_CLIFF_ANIM_DONE.md`

---

## F1: AdaptiveClusterFilter — VERDICT: PASS

**Audit checklist:**
- [✅] BFS flood fill doğru implementation (visited set, queue, neighbor iteration). `BFSFloodFill` (CliffAutoPlacer.cs:298-329) `visited.Add(start)` + `cluster.Add(start)` start node için, queue loop'unda `visited`+`cluster` ekleme tek noktada, double-enqueue yok.
- [✅] Iso 8-connectivity vectors HARD rule (`feedback_iso_grid_neighbor_vectors`). Static cell sabitleri (line 130-136) memory ile birebir: S=(-1,-1), N=(1,1), E=(1,-1), W=(-1,1), SE=(0,-1), SW=(-1,0). 8-conn ekleri NE=(1,0), NW=(0,1) DirectionalCliffTile.cs:62-63 ile birebir tutarlı.
- [✅] Adaptive heuristic doğru. `cluster.Count < minN && ratio < ratioFallback` (line 290) — sadece İKİSİ de düşükse orphan. Karar #1: spec ile aynı. floorTotal=0 guard (line 282) eklenmiş, div-by-zero yok.
- [✅] ManualPaintedCells whitelist intact. `ValidateManualPainted` (line 155-181) dokunulmadı, `Regenerate()` flow'u D5.5 ile aynı (line 197 hala ilk, line 201 `UnionWith(ManualPaintedCells)`). Orphan filter `CollectCliffCells` içinde sadece algorithmic cliff candidate'lere uygulanıyor — manual painted hala union'la geri ekleniyor.
- [✅] DirectionalCliffTile_Hades modify YASAK. `DirectionalCliffTile.cs` git unchanged (Read'de hash + spritesS layout S100 hali, F1 cycle'ında dokunulmamış).
- [⚠️] Performance: large floor map BFS perf. MINOR — `neighbors` array her BFSFloodFill çağrısında alloc ediliyor (line 309-314). 10k cell + 1k cluster start senaryosunda 1k array alloc; Editor-only çağrı için ihmal edilebilir ama micro-opt fırsatı var.
- [✅] Beklenen: 283 → ~128 cliff. Logic doğru (orphan floor cell'ler `CollectCliffCells` foreach içinde `continue` ile atlanıyor — line 258). Gerçek sayım Unity'de Regenerate sonrası doğrulanacak (verify pending).

**Bulgular:**
- [MINOR] `BFSFloodFill` her cluster için neighbors array yeniden alloc ediyor. CollectCliffCells genel ChunkSize karşısında küçük GC pressure. Düzeltme: `ComputeOrphanClusters` içinde array bir kez yarat, BFS helper'a parametre olarak geçir.
- [MINOR] `CountOrphanCells` (line 143-148) Inspector'da `OnInspectorGUI` her repaint'te BFS'yi tekrar koşuyor (CliffAutoPlacerEditor.cs:32). Floor değişmediği sürece cache edilebilir. Editor-only — perf etkisi düşük ama Inspector açıkken her tick BFS = istenmez. Cache invalidation `floorTilemap.tileChanged` event'ine bağlanabilir veya manual "Refresh Count" butonu konulabilir.
- [INFO] `clusterRules` `[SerializeField] private` — Inspector slot DrawDefaultInspector ile otomatik görünür, doğru.

**Düzeltme önerisi:** PASS — minor önerilen optimizasyonlar opsiyonel. F1 production-safe.

---

## F4: Cliff Edge Dust Particle — VERDICT: CONDITIONAL

**Audit checklist:**
- [✅] CliffAutoPlacer.cs DOKUNULMAMIŞ. CliffEdgeDustEmitter floorTilemap'i `cliffPlacer.floorTilemap` üzerinden okur, mutate etmez (line 57-58).
- [✅] Edge cell detection iso S/SE/SW void-neighbor logic doğru. Static vectors (CliffEdgeDustEmitter.cs:25-27) CliffAutoPlacer ile birebir, edge collect mantığı (line 62-70) `CollectCliffCells` ile semantik aynı.
- [⚠️] ParticleSystem per-cell instantiate. 283 cliff edge → 283 ParticleSystem GameObject. Memory + Update loop foreach maliyeti tahmin edilebilir ama büyük arena'da agresif. Single-system "burst at edge cells via shape.MeshRenderer" daha verimli ama scope artar; mevcut design spec uyumlu.
- [❌] LOD cull >20u distance Camera.main null safe — BUG. Update (line 139-141): `if (_cam == null || ...) return;` early-out ama Camera.main runtime'da yeniden assign EDİLMİYOR. `_cam` Start'ta atanıyor; PlayMode'da Camera.main daha sonra sahneye gelirse veya scene reload sonrası `_cam = null` kalır ve LOD pasif olur. **`Update`'in başında `if (_cam == null) _cam = Camera.main;` lazy re-fetch eklenmeli** (F5 pattern'i, `CliffFaceIdleAnimator.GetCameraWorldBounds` ile tutarlı — line 150).
- [❌] URP 2D Particle default material kullanım — KRİTİK BUG. `CreateEmitter` (line 79-137) ParticleSystem oluşturur ama `ParticleSystemRenderer.material` HİÇ ATANMAZ. Unity default'ta `ParticleSystemRenderer` material'i `Default-ParticleSystem` (built-in shader). URP'de bu **magenta görünür** (URP shader compatibility). Spec "URP 2D Particle default material" diyordu — bu açıkça atama gerektirir. Düzeltme: `rend.material = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");` veya URP `Default Particle` material'ini Inspector'da slot olarak expose et.
- [⚠️] 200 particle cap enforcement total scene-wide vs per-emitter. `main.maxParticles = settings.maxTotalParticles` (line 96) — HER emitter 200 cap demek 283 emitter × 200 = 56,600 teorik max. Sonra Update loop (line 144-146) `totalActive = sum(particleCount)` ile global ölçüp emission kesiyor (line 156). Doğru pattern AMA Spec "Max active particle count: 200 total scene-wide" diyor. Mevcut implementasyon SOFT cap (asymptotic) — emission kesilince mevcut particle'lar fade out ediyor, bu doğru. Yine de `main.maxParticles = settings.maxTotalParticles / Mathf.Max(1, _emitters.Count)` ile per-emitter hard cap koymak daha güvenli (transient overshoot riski azalır).
- [⚠️] `OnValidate` Update'te edit-mode'da BuildEmitters çağırıyor (line 169-174) — her Inspector slider hareketinde tüm 283 emitter destroy + recreate. Editor'da freeze potansiyeli. `OnValidate` event'i frekansı yüksek (drag slider = her frame). Spec'te "ContextMenu manual rebuild" var, OnValidate'i kaldırmak veya delay-coalesce (EditorApplication.delayCall) eklemek mantıklı.

**Bulgular:**
- [MAJOR] **URP material atanmamış — magenta render riski.** ParticleSystemRenderer.material null kalırsa URP'de görünmez veya magenta. Spec açıkça "URP 2D Particle default Material" istemiş; kodda atama yok. **Bu PlayMode'da görsel hatadır.**
- [MAJOR] **Camera.main re-fetch yok.** `_cam` Start'ta null ise sonsuza kadar null kalır; LOD pasif olur ve `tooFar` her zaman false → 200 cap'e direkt baskı. Lazy fetch zorunlu.
- [MINOR] Per-emitter `maxParticles` global cap'e eşit → transient overshoot. Bölme önerilir.
- [MINOR] `OnValidate` aggressive rebuild — Editor freeze potansiyeli.
- [INFO] `_emitterRoot` null check (line 50) `Destroy` ediyor — Editor'da `DestroyImmediate` gerekir, runtime'da `Destroy` doğru. Mevcut `Destroy` çağrısı Editor'da bir frame gecikmeli olur (kabul edilebilir, ama OnValidate flow'unda eski root + yeni root tek frame çakışır). `if (Application.isPlaying) Destroy(...) else DestroyImmediate(...)` daha temiz.

**Düzeltme önerisi (CONDITIONAL → PASS için zorunlu):**

```csharp
// CliffEdgeDustEmitter.cs CreateEmitter() içinde, "--- Renderer ---" bölümüne ekle:
var rend = go.GetComponent<ParticleSystemRenderer>();
rend.sortingLayerName = sortingLayerName;
rend.sortingOrder     = sortingOrder;
rend.renderMode = ParticleSystemRenderMode.Billboard;
// CRITICAL FIX: assign default sprite particle material for URP 2D compatibility
if (rend.sharedMaterial == null)
{
    // Use Unity's built-in particle default (Sprites-Default works in URP 2D Renderer)
    var defaultMat = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
    if (defaultMat != null) rend.sharedMaterial = defaultMat;
}
```

Veya Inspector'da `[SerializeField] private Material particleMaterial;` slot expose et, user manuel ata (daha temiz pattern).

```csharp
// Update() başında ekle:
private void Update()
{
    if (_cam == null) _cam = Camera.main; // lazy re-fetch (scene reload safe)
    if (_cam == null || settings == null || _emitters.Count == 0) return;
    // ...
}
```

```csharp
// CreateEmitter() main.maxParticles satırını şuna değiştir:
main.maxParticles = Mathf.Max(1, settings.maxTotalParticles / Mathf.Max(1, _emitters.Count + 1));
// (+1 because this emitter not yet in _emitters list when called from BuildEmitters loop)
```

---

## F5: Cliff Face Idle Animator — VERDICT: CONDITIONAL

**Audit checklist:**
- [✅] DirectionalCliffTile.cs DOKUNULMAMIŞ. Read confirmed — sadece `spritesS` field'ı okunuyor (line 76: `cliffTileSource.spritesS`).
- [✅] DirectionalCliffTile_Hades.asset DOKUNULMAMIŞ. Asset Glob'da değişmemiş (.meta same).
- [✅] Per-cell hash deterministic. `DeterministicHash` (line 159-169) DirectionalCliffTile.DeterministicSeed ile birebir aynı algoritma (17/31 prime + xor-shift). Sonnet "identical hash" iddiası doğrulandı.
- [⚠️] SetTile call performance — per-cell tile pool var (line 74-87, `_variantTiles[]` plain Tile pool), HER frame değil HER 3-5s'de bir 20 SetTile. GC kabul edilebilir AMA bkz. bulgular: pool Tile'ları DOĞRU TileBase referansı değil — sprite-only Tile (no transformOffset, no scale). Bu görsel uyumsuzluk yaratır (bkz. MAJOR bulgu).
- [✅] 3-5s coroutine interval randomized — `WaitForSeconds(Random.Range(min,max))` doğru pattern (line 104-105). Time.time poll yok, doğru.
- [⚠️] 20 visible cell limit — camera frustum culling doğru. Orthographic bounds (line 153-156) `c.x ± w` / `c.y ± h`. `frustum.Contains` Z-clamp `9999f` — 2D için OK. AMA `_tilemap.CellToWorld(cell)` iso layout'ta cell anchor'ı verir (sol-alt köşe), sprite merkez değil; merkez için `GetCellCenterWorld` daha doğru. Frustum tight check'te edge cell'ler false-negative kalabilir. MINOR — `maxAnimatedCells = 20` zaten generous.
- [❌] **Plain Tile pool — DirectionalCliffTile bypass strategy KRİTİK BUG.** `BuildVariantTilePool` (line 74-87) plain `Tile` asset'ler yaratır. Bu Tile'lar:
  - `transformOffset` YOK (DirectionalCliffTile.transformOffset = sprite'ı aşağı sarkıtma, line 20-32 DirectionalCliffTile.cs)
  - `spriteScale` YOK (DirectionalCliffTile.spriteScale)
  - `flags = TileFlags.LockColor` (line 84) — DirectionalCliffTile flags `LockTransform | LockColor` (DirectionalCliffTile.cs:26)
  - Bu Tile SetTile edildiğinde **sprite POZİSYONU değişir** (transformOffset kaybolur → cliff yukarı zıplar), **direction-aware sprite kayıp** (sadece spritesS'ten random — sprite cliff'in S/SE/SW yönüne uygun değilse görsel kırılır).
- [⚠️] **`CollectCliffCells` (line 89-97) tüm tilemap cell'lerini toplar AMA hangisinin DirectionalCliffTile_Hades olduğunu kontrol etmez.** ManualPaintedCells (D5.5) veya başka tile source'lar varsa onları da yerine yazar. Sadece spritesS variant'larına swap → ManualPaintedCells D5.5 special tile içeriyorsa override edilir.

**Bulgular:**
- [MAJOR] **DirectionalCliffTile_Hades'in `transformOffset` ve direction-aware sprite seçimi (spritesSE/spritesSW/spritesN/...) animation cycle'ında KAYBEDİLİR.** Sonnet pool plain Tile + `spritesS` only kullanıyor. Yani:
  - Cycle başlamadan: cliff E-facing cell'inde `spritesE[0]` görünüyor.
  - Animation cycle sonrası: aynı cell'de `spritesS[next]` görünür (yanlış yön sprite + transformOffset kayıp → cliff yukarı zıplar).
  - Bu spec'in "sprite array swap timer" amacını **bozar** — spec "DirectionalCliffTile.cs sprite array variant'larından random pick" diyordu, **aynı yönün variant'ları** içinde pick olmalıydı (yani cell yönünü tespit et, sonra o yönün spritesXX array'inden swap).
  - Doğru pattern: F5 her cell için yönü compute etmeli (DirectionalCliffTile.GetTileData mantığını mirror et: hasN/hasNE/hasNW/...) ve doğru `spritesXX[]` array'inden variant pick etmeli. Veya alternatif: DirectionalCliffTile'a built-in animation field eklemek (ama spec YASAK).
- [MAJOR] **`_cliffCells` ManualPaintedCells veya non-cliff Tile içerebilir.** CliffTilemap'e sadece `DirectionalCliffTile_Hades` set ediliyor varsayımı var ama D5.5 LIVE `DecorCliffTilemap` ayrı GameObject olabilir — `RequireComponent(typeof(Tilemap))` aynı GameObject'i hedef alır. Eğer CliffTilemap üzerinde başka tile çeşidi yoksa OK, ama F5 robust olmak için `_tilemap.GetTile(pos) == cliffTileSource` check'i eklemeli.
- [MINOR] Frustum check `CellToWorld` yerine `GetCellCenterWorld` daha doğru (edge cell tight bounds için).
- [MINOR] `BuildVariantTilePool` (line 80) `ScriptableObject.CreateInstance<Tile>()` runtime'da OK ama Editor edit-mode'da (CliffFaceIdleAnimator MonoBehaviour `[ExecuteAlways]` yok → sadece PlayMode çalışır, OK). Yine de tile pool `OnDestroy`'da `DestroyImmediate(_variantTiles[i])` ile cleanup edilmeli (memory leak).
- [INFO] `[RequireComponent(typeof(Tilemap))]` doğru — CliffTilemap GameObject'inde zaten Tilemap var.

**Düzeltme önerisi (CONDITIONAL → PASS için zorunlu):**

F5 design **fundamentally needs revision**. İki yol:

**Option A (minimal patch):** Pool'u 8 yönlü genişlet — her yön için Tile[] (spritesS/SE/SW/E/W/N/NE/NW). Her cell için yönü compute et (DirectionalCliffTile.cs:57-77 logic'inin kopyası) ve doğru pool'dan variant pick et. + `transformOffset` Tile.transform üzerinden taşı:

```csharp
private void BuildVariantTilePool()
{
    // 8-direction pool, each direction may have N variants
    _variantTilesByDir = new Dictionary<CliffDirection, Tile[]>();
    AddDir(CliffDirection.S, cliffTileSource.spritesS);
    AddDir(CliffDirection.SE, cliffTileSource.spritesSE);
    // ... all 8
}

private void AddDir(CliffDirection dir, Sprite[] sprites)
{
    if (sprites == null || sprites.Length == 0) return;
    var pool = new Tile[sprites.Length];
    for (int i = 0; i < sprites.Length; i++)
    {
        var t = ScriptableObject.CreateInstance<Tile>();
        t.sprite = sprites[i];
        t.colliderType = Tile.ColliderType.None;
        t.flags = TileFlags.LockTransform | TileFlags.LockColor;
        t.color = Color.white;
        t.transform = Matrix4x4.TRS(cliffTileSource.transformOffset, Quaternion.identity,
            new Vector3(cliffTileSource.spriteScale.x, cliffTileSource.spriteScale.y, 1f));
        pool[i] = t;
    }
    _variantTilesByDir[dir] = pool;
}
```

`AnimateVisibleBatch` içinde her cell için yön compute et + uygun pool'dan SetTile.

**Option B (cleaner):** F5'i komple revoke et — DirectionalCliffTile'a `AnimateSprites` opt-in field ekle, GetTileData içinde Random.Range veya time-based phase ile spritesXX'ten pick. Ama bu DirectionalCliffTile.cs modify YASAK. Yani **Option A zorunlu**.

Ek olarak:
- `_cliffCells` populate'da `_tilemap.GetTile(pos) == cliffTileSource` check eklemeli (ManualPaintedCells skip).
- `OnDestroy`'da pool cleanup: `foreach (var t in _variantTiles) DestroyImmediate(t);`

**Mevcut hali ile F5 PlayMode'da:** cycle başlar başlamaz cliff face'leri yön kaybeder ve `transformOffset` kaybolduğu için yukarı zıplar — visual regression CESUR.

---

## Cross-cutting concerns

**Audit checklist:**
- [✅] D2 + D5 + D5.5 LIVE özellikleri korunmuş. ValidateManualPainted (CliffAutoPlacer:155-181) intact, ManualPaintedCells/ManualOverrideCells flow Regenerate'de doğru sırada (line 197-201).
- [✅] ManualPaintedCells whitelist + DecorCliffTilemap dokunulmamış (F1 + F4 source). F5 risk: cliff tilemap üstünde manual painted tile varsa override eder (yukarı bulgular).
- [✅] Compile durumu: Sonnet 3 dosya da "0 err / 0 warn" rapor etti. Asset Glob'da .meta dosyaları yan yana → AssetDatabase happy.
- [✅] Yeni .cs asmdef konumu doğru. `Assets/Scripts/Environment/*.cs` (runtime) + `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` (editor) — mevcut asmdef yapısına uyumlu (existing CliffAutoPlacer.cs aynı klasörde).
- [✅] Memory rules respect. `feedback_iso_grid_neighbor_vectors` — F1 birebir, F4 birebir (S/SE/SW only — N/E/W edge irrelevant for falling dust). F5 `[RequireComponent(typeof(Tilemap))]` Input System ile çakışmaz (`feedback_input_system_active_keyboard_current` UI alanı, environment alanı değil).
- [✅] Asset paths: `Assets/ScriptableObjects/Environment/CliffClusterRules_Default.asset` + `CliffDustSettings_Default.asset` doğru yerde (Glob confirmed).

**Genel:**
- F1 LIVE-ready, F4 ve F5 PlayMode'da görsel regression yaratır.
- 3 dosya toplam ~370 LOC, scope spec ile uyumlu.

---

## Toplam Özet

| Modül | Verdict | Blocker var mı? | Notlar |
|---|---|---|---|
| F1 | **PASS** | Hayır | 2 minor perf önerisi opsiyonel. |
| F4 | **CONDITIONAL** | Evet (2 MAJOR) | URP material atanmamış + Camera.main re-fetch yok. |
| F5 | **CONDITIONAL** | Evet (2 MAJOR) | Plain Tile pool DirectionalCliffTile transformOffset + direction-aware sprite seçimini bozar. |

**Cleanup recommendation:**
1. F1 deploy edilebilir (LIVE).
2. F4 Sonnet dispatch → 2 MAJOR fix (yukarıdaki kod patches).
3. F5 Sonnet dispatch → Option A revision (8-direction pool + transformOffset + manual paint skip).

**Dispatch hint (Sonnet için):**
- F4 dispatch task: "CliffEdgeDustEmitter.cs CreateEmitter renderer.sharedMaterial atama + Update lazy Camera.main re-fetch + per-emitter maxParticles bölme" (3 surgical LOC change).
- F5 dispatch task: "CliffFaceIdleAnimator.cs BuildVariantTilePool 8-direction Dictionary refactor + AnimateVisibleBatch yön compute + _cliffCells GetTile check + OnDestroy pool cleanup" (~40 LOC).

Both fixes ≤30 dk Sonnet. Reviewer rotation: Opus review fix sonrası (mevcut Opus rolü devam).
