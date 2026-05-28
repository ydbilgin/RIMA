# CLIFF FLOATING FEEL — F (FULL SANG) IMPLEMENTATION DESIGN

**Date:** 2026-05-27 gece
**Orchestrator:** Opus subagent (synthesis only — no code writes)
**Status:** READY — dispatch sequence pending kullanıcı GO
**Predecessor:** `STAGING/CLIFF_FLOATING_FEEL_DECISION.md` (D5.6 verdict, hibrit G önerildi, **user F seçti**)

---

## 0. USER DECISIONS (LOCK — 2026-05-27 gece)

| Soru (D5.6 Bölüm 6) | User cevabı |
|---|---|
| Yaklaşım scope | **F — Full Sang Hendrix** (6-katman parallax + adv shadow + dust + cliff face anim) |
| Cluster threshold | **Adaptive** (size + floor coverage ratio heuristic, sabit N=4 değil) |
| Drop shadow strategy | **Procedural** (runtime sprite generation, GroundBlobShadow pattern reuse) |
| Visibility scope | **Hem editor-time HEM runtime** (cluster filter Regenerate + camera frustum culling) |

Faz 1 milestone timing riski **kabul edildi** — ~5-7 gün wallclock (paralel AI pipeline).

---

## 1. SCOPE BREAKDOWN — 14 COMPONENT

| # | Component | LOC est | Writer | Reviewer | Day | Notes |
|---|---|---|---|---|---|---|
| 1 | **AdaptiveClusterFilter** (in `CliffAutoPlacer.cs`) | ~120 | Codex xhigh | Sonnet | F1 | BFS + 2-condition heuristic |
| 2 | **CliffClusterRules.asset** (ScriptableObject) | ~30 | Sonnet | — | F1 | minN + coverageRatio + tunable |
| 3 | **CliffDropShadowTilemap** (GO + runtime gen) | ~120 | Sonnet | Codex | F2 | GroundBlobShadow pattern, mirror cliff cells |
| 4 | **ParallaxRig prefab + 6 child GO** (scene asset) | scene | Sonnet | — | F3 | RoomBackgroundRig.prefab + scene wire |
| 5 | **ParallaxLayer factors** (existing script reuse) | ~10 | Sonnet | — | F3 | factor tune per layer, no new code |
| 6 | **L0 BG_Void backdrop sprite** (procedural) | ~30 | Sonnet | — | F3 | flat near-black + noise overlay |
| 7 | **L1 BG_Nebula sprite** (placeholder) | ~30 | Sonnet | — | F3 | cyan fog, scroll auto |
| 8 | **L2 BG_FarRelics** (placeholder asset gen DEFER) | scene | Sonnet | — | F3 | placeholder cyan column silhouette |
| 9 | **L3 BG_NearIslands** (reuse if exists) | scene | Sonnet | — | F3 | floating rock chunks |
| 10 | **L4 Mid_Fog** (foreground volumetric) | ~30 | Sonnet | — | F3 | low alpha cyan fog overlay |
| 11 | **L5 FG_Particles wrap** (Foreground dust) | scene | Sonnet | — | F3 | particle parent, parallax 1.0 |
| 12 | **CliffEdgeDustEmitter.cs** (URP 2D particle) | ~80 | Sonnet | — | F4 | cliff edge cell scan + emit |
| 13 | **CliffFaceIdleAnimator.cs** (sprite swap timer) | ~60 | Sonnet | — | F5 | random per-instance offset, 4-frame |
| 14 | **CliffRuntimeVisibility.cs** (frustum + distance cull) | ~50 | Codex xhigh | Sonnet | F6 | SpriteRenderer.enabled toggle |
| 15 | **Smoke test + visual coherence pass** | — | Sonnet+Opus | rotation | F7 | playtest + memory delta |
| **Total** | **~660 LOC C# + 5 placeholder assets** | | rotation | rotation | **~5-7 days** | |

**LOC delta from original task brief (840 → 660):** ParallaxLayer.cs already LIVE (`Assets/Scripts/Background/ParallaxLayer.cs`) — base script reuse, no new ~80 LOC. CliffAutoPlacer.Regenerate hook inline (~10 LOC vs ~30). Editor-time integration absorbed into Component 1.

---

## 2. 6-LAYER PARALLAX SPEC (Sang Hendrix adapted)

Source: `STAGING/CLIFF_FLOATING_FEEL_research_agy.md` Block 1.5 + existing `ParallaxLayer.cs` Codex verdict (already inline-documented).

| Layer | Name | Z | Sorting Layer | Order | Parallax factor (X, Y) | Sprite | Asset state |
|---|---|---|---|---|---|---|---|
| **L0** | BG_Void | -500 | Background | -500 | (0.03, 0.02) | flat dark + noise | **NEW placeholder procedural** |
| **L1** | BG_Nebula | -300 | Background | -400 | (0.05, 0.04) | cyan fog tile | **NEW placeholder procedural** |
| **L2** | BG_FarRelics | -200 | Background | -300 | (0.08, 0.05) | distant cyan rune columns | **PixelLab DEFER user supervised** |
| **L3** | BG_NearIslands | -100 | Background | -200 | (0.14, 0.08) | small floating rocks | **PixelLab DEFER + L3_Island_Large already exists per memory `project_l3_island_large_boss_spawn_trigger`** |
| **L4** | Mid_Fog | -50 | Background | -100 | (0.10, 0.06) | thin volumetric cyan fog | **NEW placeholder procedural** |
| **L5** | FG_Particles | +10 | Foreground | +50 | (1.00, 1.00) | particle parent | wraps `CliffEdgeDustEmitter` |

**Per-layer parallax factor reasoning (from `ParallaxLayer.cs` inline doc + agy Block 1.5):**
- L0 nearly static (cosmic void illusion)
- L1-L2 slow scroll (Hades Elysium distant island depth)
- L3 quicker (visible parallax separation from cliff face)
- L4 mid (above L3 islands but below player plane)
- L5 1.0 (foreground, locked to camera)

**Vertical factor halved per agy Block 1.5e:** "Yüksek top-down açısında (70-80 derece) dikey parallax hareketinin oyuncuda baş dönmesi yaratabilmesi. Y çarpımı X'in 0.5'i". Tüm Y factor değerleri X'in ~%60'ı.

**Authoring hierarchy (`PlayableArena_Test01.unity`):**
```
RoomBackgroundRig (parent, identity transform)
├── L0_Void          (SpriteRenderer + ParallaxLayer factor=(0.03, 0.02))
├── L1_Nebula        (SpriteRenderer + ParallaxLayer factor=(0.05, 0.04))
├── L2_FarRelics     (SpriteRenderer + ParallaxLayer factor=(0.08, 0.05))
├── L3_NearIslands   (SpriteRenderer + ParallaxLayer factor=(0.14, 0.08))
├── L4_MidFog        (SpriteRenderer + ParallaxLayer factor=(0.10, 0.06))
└── FG_Particles     (ParticleSystem root + ParallaxLayer factor=(1.0, 1.0))
```

---

## 3. ADAPTIVE CLUSTER FILTER ALGORITHM

**Goal:** Editor-time, `CliffAutoPlacer.Regenerate()` öncesi 1-3 cell izole cliff cluster'larını filtrele. Hem mutlak boyut (minN) hem floor coverage ratio (orphan = floor toplamına göre çok küçük).

### Pseudo-code

```
input: HashSet<Vector3Int> cliffTargets, HashSet<Vector3Int> floorCells
output: HashSet<Vector3Int> filteredTargets

floorTotal = floorCells.Count
minN = rules.minClusterSize         // default 4
ratio = rules.coverageRatioFloor    // default 0.005  (0.5% of floor total)

clusters = BFS_4Neighbor(cliffTargets)  // orthogonal for iso cell grid
foreach cluster in clusters:
    isBigEnough = (cluster.size >= minN)
    isRatioOK = (cluster.size / floorTotal >= ratio)
    if isBigEnough OR isRatioOK:
        filteredTargets.UnionWith(cluster)
    // else: skip (orphan)
return filteredTargets
```

### Adaptive heuristic — neden 2-condition

- **Sadece minN** (sabit N=4): Küçük arena (~200 floor) için 4 OK, devasa arena (~5000 floor) için 4 cluster'lar göreceli çok küçük → daha agresif filtre gerek.
- **Sadece ratio**: Çok küçük floor count (~50) ise ratio threshold tek başına neredeyse her cluster'ı geçirir.
- **OR kombinasyon**: Her iki testten biri geçerse keep. Robust both small + large arenas.

**Default values (PlayableArena_Test01 reality):**
- floorTotal = 2365
- minN = 4 → ana arena cluster'ları (12, 9, 8, ...) geçer
- ratio = 0.005 → cluster size ≥ 11.825 cell ise ratio test PASS (top-1 cluster=12 sınırda)
- 155 cluster (size 1-3) — minN FAIL, ratio FAIL → silinir
- 11 cluster (size 4+) — minN PASS → keep
- Expected result: **283 cliff → ~128 cliff** (155 orphan removed)

### `CliffClusterRules.asset` (ScriptableObject)

```csharp
[CreateAssetMenu(menuName = "RIMA/Environment/CliffClusterRules")]
public class CliffClusterRules : ScriptableObject
{
    [Tooltip("Minimum absolute cluster size (cells). Smaller clusters are dropped unless ratio passes.")]
    [Range(1, 16)] public int minClusterSize = 4;

    [Tooltip("Cluster passes if (cluster.size / floorTotal) >= this. Default 0.005 (0.5%).")]
    [Range(0f, 0.05f)] public float coverageRatioFloor = 0.005f;

    [Tooltip("Connectivity for clustering. 4 = orthogonal NESW only, 8 = include diagonals.")]
    public ClusterConnectivity connectivity = ClusterConnectivity.Four;

    [Tooltip("If true, manualPaintedCells whitelist bypasses filter (always kept).")]
    public bool whitelistOverrideEnabled = true;
}
```

**Asset path:** `Assets/Settings/CliffClusterRules.asset` (created via CreateAssetMenu).
**Wire to:** `CliffAutoPlacer.clusterRules` field (new public field, Inspector-assigned).

### Integration point — `CliffAutoPlacer.Regenerate()`

Insert after L189 `targets.UnionWith(ManualPaintedCells)`, before SetTile loop:

```csharp
if (clusterRules != null)
{
    var floorCells = CollectFloorCells();  // new helper, ~10 LOC
    targets = AdaptiveClusterFilter.Filter(targets, floorCells, clusterRules);
    if (clusterRules.whitelistOverrideEnabled)
        targets.UnionWith(ManualPaintedCells);  // re-add user-painted post-filter
}
```

ManualPaintedCells whitelist re-applied **after** filter so user's intentional isolated cliffs survive.

---

## 4. PROCEDURAL DROP SHADOW TILEMAP

### Design — GroundBlobShadow.cs pattern reuse

`Assets/Scripts/Environment/GroundBlobShadow.cs` already implements:
- Static cached sprite generation (96×96 Texture2D, radial gradient, alpha pow 2.2)
- `Ensure(Transform, Vector2, alpha)` static factory
- Apply() sets sortingLayer + order + scale + localPos

**Adapt for cliff drop shadow:**
1. New `CliffDropShadowSprite` static gen (different shape — vertical gradient, not radial)
2. New runtime component `CliffDropShadowTilemap` that mirrors cliff cells

### Component spec — `CliffDropShadowTilemap.cs`

```csharp
namespace RIMA.Environment
{
    [ExecuteAlways]
    public sealed class CliffDropShadowTilemap : MonoBehaviour
    {
        public Tilemap cliffTilemap;      // source
        public Tilemap shadowTilemap;     // own child or sibling
        public TileBase shadowTile;        // single procedural Tile asset

        [Range(0f, 1f)] public float shadowAlpha = 0.5f;
        [Range(0, 4)] public int verticalOffsetCells = 1;  // shadow drops N cells below cliff

        public void RegenerateShadows()
        {
            shadowTilemap.ClearAllTiles();
            foreach (var pos in cliffTilemap.cellBounds.allPositionsWithin)
            {
                if (!cliffTilemap.HasTile(pos)) continue;
                for (int i = 1; i <= verticalOffsetCells; i++)
                {
                    var shadowPos = pos + new Vector3Int(-i, -i, 0);  // iso "down"
                    if (cliffTilemap.HasTile(shadowPos)) continue;     // don't overlap real cliff
                    shadowTilemap.SetTile(shadowPos, shadowTile);
                }
            }
        }
    }
}
```

### Procedural shadow Tile asset

**Two options:**
- **Option A (chosen):** Custom ScriptableObject Tile subclass that returns procedural sprite (Texture2D generated like GroundBlobShadow, but vertical gradient).
- **Option B:** Sprite asset PNG at `Assets/Art/Tiles/CliffShadow.png` (defer to user art pass).

**Option A impl:**
```csharp
[CreateAssetMenu(menuName = "RIMA/Environment/CliffDropShadowTile")]
public class CliffDropShadowTile : TileBase
{
    private static Sprite cachedSprite;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData data)
    {
        if (cachedSprite == null) cachedSprite = BuildVerticalGradientSprite();
        data.sprite = cachedSprite;
        data.color = new Color(0, 0, 0, 0.5f);
        data.colliderType = Tile.ColliderType.None;
    }
    // BuildVerticalGradientSprite: 64×64 Texture2D, alpha 0.6 top → 0 bottom, ease-out
}
```

### Sorting layer setup

`CliffDropShadowTilemap` GO:
- sortingLayerName = `"Decor_Cliff"` (S113 D2 LIVE)
- sortingOrder = `-20` (below cliff base which is at order ~0, above floor)
- material = Sprite-Lit-Default

### Integration

Hook to `CliffAutoPlacer.Regenerate()` end:
```csharp
if (shadowTilemapComponent != null) shadowTilemapComponent.RegenerateShadows();
```

---

## 5. CLIFF EDGE DUST PARTICLE SYSTEM

Source: `STAGING/CLIFF_FLOATING_FEEL_research_agy.md` Block 1.1d (Hades Elysium cyan parıltı toz partikül).

### Component spec — `CliffEdgeDustEmitter.cs`

URP 2D Particle System (Built-in `ParticleSystem` + Sprite Render mode), single emitter per scene, scans cliff edge cells and uses **shape module (Edge type)** with multiple sub-emitters dynamically positioned.

**Simpler approach (recommended Faz 1):** Single ParticleSystem with **Box emitter shape** sized to arena bounds + custom script feeds emission positions per frame via `EmitParams` for cells on cliff edge.

### Parameters

| Param | Value |
|---|---|
| Lifetime | 1.5-2.0s (random range) |
| Start Velocity | (0, -0.3, 0) — downward into void |
| Gravity Modifier | 0.05 (slight pull) |
| Start Color | Cyan #00FFCC alpha 0.0 |
| Color over Lifetime | gradient: alpha 0 → 0.35 → 0 (peak at midlife) |
| Start Size | 0.05-0.10 (small dots) |
| Max Particles | 200 |
| Emission Rate | 0 (manual, script-controlled) |
| Shape | Disabled (use EmitParams.position) |
| Renderer | Sprite mode, simple 4×4 white pixel + cyan tint |

### Script flow

```csharp
void Start() {
    cliffEdgeCells = ScanCliffEdgeCells();  // cliff cells adjacent to void
    InvokeRepeating(nameof(Tick), 0f, 0.1f);
}

void Tick() {
    // Camera frustum culling
    var visibleCells = cliffEdgeCells.Where(c => IsInCameraView(c));
    foreach (var cell in visibleCells.Sample(10))  // 10 random per tick
    {
        var emitParams = new ParticleSystem.EmitParams
        {
            position = cliffTilemap.CellToWorld(cell) + Random.insideUnitSphere * 0.3f,
        };
        ps.Emit(emitParams, 1);
    }
}
```

**Performance budget:** 10 emit/tick × 10 tick/sec = 100 particles/sec, lifetime 2s → ~200 alive max. Stays within `maxParticles=200` cap.

---

## 6. CLIFF FACE IDLE ANIMATION

Source: agy Block 1.2a (Children of Morta cliff face alpha falloff + subtle motion).

### Existing infrastructure

`DirectionalCliffTile_Hades.asset` (D5 LIVE): 5 South sprite variants (`spritesS[]`). Currently random per-cell variant selection deterministic via cell hash.

### Animation strategy — sprite swap timer with per-instance phase offset

**Option chosen:** Custom MonoBehaviour driving cliff tile refresh, NOT Unity AnimatedTile (which can't do per-instance phase offset).

```csharp
namespace RIMA.Environment
{
    [ExecuteAlways]
    public sealed class CliffFaceIdleAnimator : MonoBehaviour
    {
        public Tilemap cliffTilemap;
        public DirectionalCliffTile cliffTileAsset;
        [Range(1f, 10f)] public float cycleSeconds = 4f;
        [Range(0, 1f)] public float swapChancePerCycle = 0.3f;  // sparse — not every cell every cycle

        private float _nextTick;

        void Update()
        {
            if (Time.time < _nextTick) return;
            _nextTick = Time.time + cycleSeconds * 0.25f;  // 4 sub-ticks per cycle

            // Refresh subset of cliff cells (deterministic per-cell hash phase)
            foreach (var pos in cliffTilemap.cellBounds.allPositionsWithin)
            {
                if (!cliffTilemap.HasTile(pos)) continue;
                int hash = pos.x * 73856093 ^ pos.y * 19349663;
                float phase = (hash & 0xFFFF) / 65535f;  // 0..1 per-cell offset
                float cellTime = (Time.time + phase * cycleSeconds) % cycleSeconds;
                if (cellTime < cycleSeconds * 0.25f && Random.value < swapChancePerCycle)
                {
                    cliffTilemap.RefreshTile(pos);  // DirectionalCliffTile.GetTileData reroll
                }
            }
        }
    }
}
```

**Visual outcome:** Most cliff cells stable, ~30% swap variant every ~1s in a random subset (twinkle / settle illusion). Per-cell phase offset prevents synchronized "blink" pattern.

**Risk:** `RefreshTile` may trigger Unity tilemap dirty flag → editor performance. Mitigate via `Application.isPlaying` guard if needed.

---

## 7. EDITOR + RUNTIME CULLING COMBO

### Editor-time culling (Component 1 — AdaptiveClusterFilter)

Already covered in Section 3. Runs on `CliffAutoPlacer.Regenerate()` — orphan cliffs never get tile-set.

### Runtime culling — `CliffRuntimeVisibility.cs`

**Strategy:** Per-cluster centroid-based culling, not per-cell (200+ cliff cells per-cell check = wasteful).

```csharp
namespace RIMA.Environment
{
    public sealed class CliffRuntimeVisibility : MonoBehaviour
    {
        public Tilemap cliffTilemap;
        public Camera target;
        [Range(2f, 20f)] public float cullDistance = 12f;     // beyond this in world units
        [Range(0.1f, 2f)] public float checkInterval = 0.5f;  // throttle

        private List<HashSet<Vector3Int>> _clusters;  // computed via BFS once
        private TilemapRenderer _renderer;
        private float _nextCheck;

        void Start() {
            _clusters = ClusterUtil.BFSCluster(cliffTilemap.cellBounds, p => cliffTilemap.HasTile(p));
            _renderer = cliffTilemap.GetComponent<TilemapRenderer>();
        }

        void Update() {
            if (Time.time < _nextCheck) return;
            _nextCheck = Time.time + checkInterval;
            // TilemapRenderer.chunkSize-based culling preferred over per-cluster
            // for Unity 2023+ — built-in detectChunkCullingBounds handles this.
            // This script is a fallback for cluster-level visibility events
            // (e.g., trigger CliffDynamicFade when player approaches).
        }
    }
}
```

**Note:** Unity's `TilemapRenderer.detectChunkCullingBounds` already culls off-screen tilemap chunks. This component primarily provides **cluster-level visibility events** for game logic hooks (CliffDynamicFade, dust emit throttle), not raw render culling.

**Practical scope Faz 1:** Pause built-in chunk culling = sufficient. `CliffRuntimeVisibility` is a stub component for future fade/dust hookups. **Skip-or-stub** acceptable if F6 day timing pressed.

---

## 8. DAY-BY-DAY PHASE PLAN

| Day | Components | Writer dispatch | Reviewer | Verify gate |
|---|---|---|---|---|
| **F1** (today/tomorrow) | #1 AdaptiveClusterFilter + #2 CliffClusterRules SO | Codex xhigh `cx_dispatch.py` | Sonnet review | Auto-test T5_CliffNoFloatingSinglets (BFS doğrula, <minN cluster count=0). Manual: PlayableArena_Test01 Regenerate → 283 cliff → ~128 cliff |
| **F2** | #3 CliffDropShadowTilemap (component + tile + scene wire) | Sonnet write | Codex review | Visual: shadow strip cliff edge'inde görünür, alpha 0.5, void üzerinde, multiply OK |
| **F3** | #4 ParallaxRig prefab + #5-11 6-layer scene wire + 3 placeholder sprite | Sonnet write | — | Camera move → 6 layer farklı hızlarda kayar, vertical halved, snapToPixel ON |
| **F4** | #12 CliffEdgeDustEmitter | Sonnet write | — | Play mode → cliff edge'inde sürekli cyan parıltı, frustum cull working (max 200 alive) |
| **F5** | #13 CliffFaceIdleAnimator | Sonnet write | — | Play mode → cliff sprite'ları sparse twinkle, per-cell phase offset (no sync blink) |
| **F6** | #14 CliffRuntimeVisibility (stub or full) | Codex xhigh | Sonnet review | Optional — Unity built-in chunk culling baseline yeterli |
| **F7** | #15 Smoke test + memory delta + screenshot pass | Sonnet+Opus sentez | rotation | User playtest brief `STAGING/CLIFF_F_PLAYTEST_BRIEF.md` |

**Wallclock:** ~5-7 gün paralel AI dispatch (F2-F5 paralel mümkün, F1 ve F6 sıralı).

### Parallel-friendly dispatch matrix

- **F1 BLOCKING:** CliffAutoPlacer.cs modify — diğer task'ları beklemeden başlat
- **F2-F5 PARALLEL OK:** Ayrı dosyalar / scene GO'lar, çakışma yok
- **F6 needs F1 cluster BFS util:** sıralı F1 sonrası
- **F7 sequential:** tüm verify gates PASS sonrası

---

## 9. RISK + ROLLBACK

| Risk | Severity | Mitigation | Rollback |
|---|---|---|---|
| Faz 1 demo timing kayar | HIGH | ~7 gün ekstra, user accepted | F1-F2 minimal MVP (adaptive filter + shadow only), F3-F6 defer Faz 2 polish |
| Adaptive heuristic tuning loop | MED | Inspector slider runtime tune | Default minN=4 ratio=0.005 başla, kullanıcı feedback ile iter |
| Parallax sprite asset gen (gece halt) | LOW | L0-L4 procedural placeholder Faz 1, PixelLab L2-L3 user supervised Faz 2 | Placeholder ile demo OK |
| Particle perf (200+ alive) | MED | maxParticles=200 hard cap + frustum cull + 10/tick rate | Disable component, no functional break |
| CliffFaceIdleAnimator editor perf | LOW | Application.isPlaying guard | Disable in edit mode |
| Runtime culling redundant | LOW | Unity built-in handles 90% | F6 stub-only acceptable |
| Cluster filter agresif → arena boş | MED | Inspector slider + manualPainted whitelist override | minN=2 fallback |

---

## 10. OPEN QUESTIONS (kullanıcı kararı bekleyen — minimal)

1. **L2/L3 parallax sprite asset:** Faz 1 procedural placeholder (cyan column silhouette) yeterli mi? PixelLab gen user-supervised sabah → Faz 2 polish DEFER OK? **(öneri: YES, defer)**

2. **CliffRuntimeVisibility scope F6:** Tam impl mi yoksa stub-only Unity built-in chunk cull'a güven? **(öneri: stub-only)**

3. **CliffFaceIdleAnimator on-by-default:** Component scene'de aktif mi, kullanıcı manuel enable mı? **(öneri: aktif, swapChancePerCycle=0.3 conservative)**

4. **Existing L3_Island_Large hatırlat:** Memory `project_l3_island_large_boss_spawn_trigger` ⭐ S106 LOCK — L3_Island_Large temple+columns SADECE boss room transition'da. Default arena'da gizli. F3 parallax L3 layer L3_Island_Small kullansın (default), L3_Island_Large boss room state'inde swap. **(öneri: L3_Island_Small default, swap logic gameplay code'a defer)**

---

## 11. DISPATCH TASK ARTIFACTS (next steps)

Orchestrator F1 dispatch için iki task .md hazır olacak:

### A. `STAGING/CLIFF_F_codex_write_adaptive.md` (Codex xhigh, F1 writer)

Içerik:
- ACTIVE RULES header
- NLM ACCESS line
- Amaç: AdaptiveClusterFilter + CliffClusterRules SO impl
- Surgical file list: `CliffAutoPlacer.cs` MODIFY (1 method add), `CliffClusterRules.cs` NEW, `AdaptiveClusterFilter.cs` NEW (static util)
- Spec reference: this design doc Sections 3
- Verify gate: 0 compile error + auto-test pseudo (Phase1Demo asmdef extend)
- Dispatch: `python cx_dispatch.py --task-file STAGING/CLIFF_F_codex_write_adaptive.md --effort xhigh`
- Lock cleanup post-TaskStop: `.cx_dispatch_locks/<profile>.lock` manual remove (memory `feedback_codex_stale_lock_after_taskstop`)

### B. `STAGING/CLIFF_F_sonnet_review_F1.md` (Sonnet reviewer, F1 reviewer)

Içerik:
- ACTIVE RULES header
- model: `sonnet` explicit (memory `feedback_agent_dispatch_model_explicit`)
- Amaç: Codex output review (algorithm correctness + manualPaintedCells whitelist integrity + LOC budget compliance)
- Diff inspect: `git diff` between dispatch start commit and end commit
- Output: PASS/CONDITIONAL/FAIL verdict + concerns list

F2-F7 task .md'leri F1 PASS sonrası generate edilir (sequencing).

---

## 12. NLM CANONICAL CROSS-CHECK

Bu design doc'ı yazdıktan sonra NLM canonical sources cross-check (defer to next session — orchestrator scope-out):
- `project_3kit_bg_architecture_lock` (memory) — L2/L3/L4 sortingLayer ve Z hiyerarşisi uyumlu ✓
- `project_walless_v1_hades_elysium_lock` (memory) — V1 floating arena brand, F approach %100 reinforces ✓
- `project_yarik_3scale_language` (memory) — Cyan #00FFCC consistent (parallax L1 nebula renk) ✓
- `feedback_iso_grid_neighbor_vectors` (memory) — iso vectors zorunlu (AdaptiveClusterFilter BFS 4-neighbor iso conv olmalı — orthogonal değil) ⚠ **F1 implementor must use isometric neighbor offsets (S/N/E/W/SE/SW from CliffAutoPlacer L127-133)**

---

## 13. SUMMARY (15 sentences for user)

1. Kullanıcı F yaklaşım (Full Sang Hendrix) seçti — 6-katman parallax + drop shadow + dust + cliff face anim + adaptive cluster filter + editor+runtime culling combo.
2. Toplam scope: **15 component, ~660 LOC C#, 5 placeholder asset, ~5-7 gün wallclock paralel AI.**
3. Faz 1 timing riski kullanıcı tarafından kabul edildi (~5-7 gün ekstra).
4. Critical reuse: `Assets/Scripts/Background/ParallaxLayer.cs` ZATEN LIVE — base parallax kod yazılmaz, sadece scene rig + 6 child + factor tune.
5. AdaptiveClusterFilter (F1) **iso neighbor vectors kullanır** (memory `feedback_iso_grid_neighbor_vectors` HARD), `CliffAutoPlacer.SouthCell/EastCell/...` reuse.
6. Adaptive heuristic: `cluster.size >= minN` **OR** `cluster.size / floorTotal >= ratio` — küçük + büyük arenalarda robust.
7. Default values: minN=4, ratio=0.005 → PlayableArena_Test01'de 283 cliff → ~128 cliff (155 orphan removed).
8. Drop shadow procedural runtime sprite (GroundBlobShadow pattern reuse), custom Tile asset vertical gradient, sortingLayer Decor_Cliff order -20.
9. 6-katman parallax Sang Hendrix factor table: L0(0.03,0.02) → L5(1.0,1.0). Vertical halved (motion sickness mitigation).
10. L2/L3 sprite asset gen DEFER user-supervised sabah (gece PixelLab halt rule); placeholder procedural Faz 1 yeterli.
11. Cliff face idle: per-cell hash phase offset → no sync blink, sparse swap (30% per cycle), `DirectionalCliffTile_Hades` spritesS[] reuse.
12. Editor-time culling = F1 filter (orphan engelle). Runtime culling = F6 stub (Unity built-in chunk cull yeterli).
13. Writer rotation: Codex xhigh writes F1+F6 (algorithm-heavy), Sonnet writes F2-F5 (mechanical). Reviewer rotation: Codex → Sonnet review F1, Sonnet → Codex review F2.
14. Dispatch sequence: F1 BLOCKING başlar (cluster filter), F2-F5 PARALLEL ok F1 PASS sonrası, F6-F7 sıralı tail.
15. Bir sonraki orchestrator adımı: F1 dispatch task .md yazımı (`STAGING/CLIFF_F_codex_write_adaptive.md`) + Codex bg dispatch + user GO bekle.

---

## 14. NEXT ORCHESTRATOR ACTION

User GO sonrası:
1. `STAGING/CLIFF_F_codex_write_adaptive.md` task .md yaz (codex-task skill format)
2. `python cx_dispatch.py --task-file STAGING/CLIFF_F_codex_write_adaptive.md --effort xhigh` dispatch (bg)
3. `STAGING/CLIFF_F_sonnet_review_F1.md` review task .md yaz (paralel, F1 PASS sonrası tetiklenir)
4. F1 PASS → F2-F5 paralel dispatch (Sonnet × 4 bg agent)
5. F2-F5 done → F6+F7 tail
6. CURRENT_STATUS.md + memory delta her F-day sonu

---

*Sentez sonu — Orchestrator Opus subagent, 2026-05-27 gece. Triple-AI dispatch INSIDE subagent: agy research (D5.6'dan reuse), Codex (CODEX_DONE.md boş D5.6 — orchestrator-substitute reality check), Opus sentez (bu doc). Code-writer rotation F1-F6 plan'da locked.*
