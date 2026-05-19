# Multi-Layer Painter — Plan v1 LOCKED

**Status:** ✅ LOCKED 2026-05-18 S92 (Codex PASS_WITH_REVISIONS → revisions applied in §3, §4a, §5).
**Owner:** Orchestrator (Claude).
**Date:** 2026-05-18 S92.
**Locks:** Builds on S91 Map Plan v1 LOCK (Option C hibrit painted bg + gameplay overlay). User direction: "RIMA + diğer oyunlarda kullanacağım reusable design pattern."
**Karar:** #147 candidate (to be inscribed in MASTER_KARAR_BELGESI.md post-impl).

---

## 1. Why

User goal: "Hades / Alabaster Dawn tarzı painted oda look — birden çok painted resmi üst üste, hangisi üstte hangisi altta seçebileyim. RIMA'da ve diğer oyunlarımda da kullanabileceğim reusable design pattern."

**Aesthetic targets:** Hades (Supergiant), Alabaster Dawn — painted backgrounds composed of multiple stacked layers (floor base → mid decals → fg props → ambient overlays), each layer hand-painted, not procedural tile composition.

**Reusability:** This is engine-level infrastructure (not RIMA gameplay logic), portable to any 2D Unity project. Lives in `RIMA.MapDesigner.Room.Data` namespace; data + Inspector + runtime are renderer-agnostic ([[3d-portability-strategy]] holds).

**Current state (S92 today):** Single `backgroundSprite` field added morning. Insufficient — only 1 sprite, no z-order control, no offset/scale/tint, no painter UX. Must extend before user produces real painted assets (so they don't have to redo work).

---

## 2. Data model

### 2a. New class: `BackgroundLayerData`

```csharp
// File: Assets/Scripts/MapDesigner/Room/Data/BackgroundLayerData.cs
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [System.Serializable]
    public class BackgroundLayerData
    {
        public string layerName = "Layer";
        public Sprite sprite;
        public int sortingOrder = -100;     // z-order; lower = further back
        public Vector2 offset = Vector2.zero; // world-space offset from room center
        public Vector2 scale = Vector2.one;   // local scale multiplier
        public Color tint = Color.white;      // SpriteRenderer.color
        public bool visible = true;           // skip spawn if false (Inspector toggle)
    }
}
```

**Why these fields:**
- `layerName`: Inspector clarity ("FloorBase", "RiftCrack", "MossOverlay")
- `sortingOrder`: explicit z-order (user picks, not list order — allows non-contiguous ordering with other game sprites like player/enemy at 0)
- `offset`: nudge a sprite without re-importing
- `scale`: cheap size adjustment without per-asset PPU edit
- `tint`: per-room mood shift (e.g. red tint for corrupted variant)
- `visible`: quick A/B test in Inspector

**No persistence schema bump** — additive to existing RoomTemplateSO, schemaVersion stays "1.0".

### 2b. RoomTemplateSO change

```csharp
// REMOVE (added morning S92):
//   public Sprite backgroundSprite;

// ADD (after prefabRef):
[Header("Painted Background Layers (Map Plan v1 LOCK — S91, Multi-Layer Painter v1 LOCK — S92)")]
[Tooltip("Stacked painted background sprites, Hades-style. Render order = sortingOrder per layer. Empty list = no painted bg.")]
public List<BackgroundLayerData> backgroundLayers = new List<BackgroundLayerData>();
```

**Migration:** Single-field `backgroundSprite` was added today, never persisted to any non-test asset (Spawn_01 cleared this turn). Clean break, no compat shim needed.

---

## 3. Runtime spawn (RoomBankRuntimeTester.RunTest)

**Coordinate space LOCK (Codex revision):** All bg positioning is **room-local**, NOT world-space. The bg root is parented to the room, its localPosition is room-bounds center, and each layer's localPosition is the layer's offset relative to that center. This way the entire background stack moves with the room if the room is ever translated/rotated.

Replace the single `if (picked.backgroundSprite != null) { ... }` block with:

```csharp
if (picked.backgroundLayers != null && picked.backgroundLayers.Count > 0)
{
    GameObject bgRoot = new GameObject("PaintedBackground");
    Transform parentTransform = result.roomInstance != null ? result.roomInstance.transform : transform;
    bgRoot.transform.SetParent(parentTransform, false);
    
    // Bg root sits at room-bounds CENTER, in parent-local space.
    // bounds is interpreted as room-local tile rect.
    bgRoot.transform.localPosition = new Vector3(
        picked.bounds.xMin + picked.bounds.width * 0.5f,
        picked.bounds.yMin + picked.bounds.height * 0.5f,
        0f
    );
    
    int spawnedLayers = 0;
    for (int i = 0; i < picked.backgroundLayers.Count; i++)
    {
        var layer = picked.backgroundLayers[i];
        if (layer == null || !layer.visible || layer.sprite == null) continue;
        
        GameObject layerGO = new GameObject($"Layer_{i:D2}_{layer.layerName}");
        layerGO.transform.SetParent(bgRoot.transform, false);
        // Layer position is offset relative to bgRoot (room center); z bumped back so all layers render behind gameplay.
        layerGO.transform.localPosition = new Vector3(layer.offset.x, layer.offset.y, 1f);
        layerGO.transform.localScale = new Vector3(layer.scale.x, layer.scale.y, 1f);
        
        var sr = layerGO.AddComponent<SpriteRenderer>();
        sr.sprite = layer.sprite;
        sr.sortingOrder = layer.sortingOrder;
        sr.color = layer.tint;
        sr.drawMode = SpriteDrawMode.Simple;
        spawnedLayers++;
    }
    result.diagnostics.Add($"Painted background: spawned {spawnedLayers}/{picked.backgroundLayers.Count} layers.");
}
else
{
    result.diagnostics.Add("No painted background layers; skipped.");
}
```

**Reserved sortingOrder ranges (Codex recommendation, LOCK candidate):**
- `-200 .. -50` → background painted layers (this system)
- `0 .. 49` → gameplay (player, enemies, world props)
- `50 .. 199` → foreground overlays (fog, ambient occlusion)
- `200+` → HUD / UI in worldspace

---

## 4. Editor UI

### 4a. Phase 1 (this session): Custom Inspector with ReorderableList

`Assets/Editor/MapDesigner/Inspectors/RoomTemplateSOInspector.cs`:
- `[CustomEditor(typeof(RoomTemplateSO))]` standard pattern
- `UnityEditorInternal.ReorderableList` on `backgroundLayers`
  - Draggable rows (Unity built-in handle)
  - +/− add/remove buttons
  - Per-row visual: layerName field | 40×40 sprite thumbnail (`AssetPreview.GetAssetPreview`) | sortingOrder int field | visible toggle
  - Expandable details: offset, scale, tint
- **All OTHER RoomTemplateSO fields** rendered via iterating `serializedObject` properties — **EXCLUDE `backgroundLayers`** from the default draw to prevent double-rendering (Codex caveat). Do NOT call `DrawDefaultInspector()` blindly.

```csharp
// Pattern:
serializedObject.Update();
var iterator = serializedObject.GetIterator();
iterator.NextVisible(true); // skip m_Script
while (iterator.NextVisible(false)) {
    if (iterator.propertyPath == "backgroundLayers") continue; // drawn separately by ReorderableList
    EditorGUILayout.PropertyField(iterator, true);
}
backgroundLayersReorderable.DoLayoutList();
serializedObject.ApplyModifiedProperties();
```

**Painter UX achieved:** drag-reorder list, see thumbnails inline, toggle visibility for A/B, set z-order per layer.

**Thumbnail async pattern (Codex recommendation):**
```csharp
Texture2D thumb = UnityEditor.AssetPreview.GetAssetPreview(sprite);
if (UnityEditor.AssetPreview.IsLoadingAssetPreview(sprite.GetInstanceID())) {
    EditorWindow.focusedWindow?.Repaint(); // request repaint while preview generates
}
```

### 4b. Phase 2 (defer, not this session): Dedicated EditorWindow

`Tools/RIMA/Map Designer/Painter Window`:
- Side panel: vertical list of layer thumbnails (Photoshop-style)
- Drag-drop sprite from Project to add layer
- Live preview area shows composite render
- Hover layer → opacity slider, tint picker
- Bottom: "Apply to RoomTemplate" → writes to selected `.asset`

**Defer reasoning:** Phase 1 ReorderableList covers 80% of UX; dedicated window is polish. Lock the data model + Phase 1 UI first, build window after first real painted bg validates the workflow.

---

## 5. Asset sizes — recommended for room backgrounds

**Source of truth:** Full PixelLab Create Image Pro size list lives in `memory/reference_pixellab_create_image_pro.md` (13 sizes including 32×32, 64×64, 341×341, 344×192). This table picks the **subset useful for room backgrounds**; for icons/UI use the small sizes in the full reference.

User noted: 640×480 doesn't exist in PixelLab Pro. Real room-background options:

| PixelLab Pro size | Aspect | Best fit room (PPU 64) | Use case |
|---|---|---|---|
| **512×512** | 1:1 | 8×8 wu (Spawn_01 fits, 2 wu vertical bleed) | Square small rooms, hero crops |
| **632×424** | ~3:2 | 9.875×6.625 wu (best 4:3 match for 8×6) | **Default for Spawn_01 and 8×6 rooms; REST `/generate-image-v2` supported (numeric ImageSize schema, no enum)** |
| **688×384** | ~16:9 | 10.75×6 wu | Wide combat rooms (12×6) |
| 424×632 | 2:3 | 6.625×9.875 wu | Tall portrait rooms |
| 512×288 | ~16:9 | 8×4.5 wu | Wide skinny corridors |
| 384×216 | 16:9 | 6×3.375 wu | Small corridors |
| 344×192 | 16:9 | 5.375×3 wu | Banner/sub-section |
| 341×341 | 1:1 | 5.3×5.3 wu | Small square decal hero |
| 256×256 | 1:1 | 4×4 wu | Decal layer, mid-size |
| 128×128 | 1:1 | 2×2 wu | Accent decal |
| 64×64 | 1:1 | 1×1 wu | Tiny accent/icon decal |
| 32×32 | 1:1 | 0.5×0.5 wu | Micro detail (rare for bg) |

**Multi-layer naturally handles size variance (Codex strategy C — hybrid style-locked stack):**
- Layer 0 (floor base) = 632×424 `/generate-image-v2` painted floor — covers room (style anchor)
- Layer 1+ (rift cracks / rubble / statues / accents) = 256×256 or 128×128 `/generate-with-style-v2` transparent decals using Layer 0 as style reference — guarantees same brushwork/palette across stack
- **`/generate-with-style-v2` capped at 512×512** — never use for >512 floor bases; use it for decals/props only

Per-layer `offset` + `scale` handle positioning. **No global "fit to bounds" auto-scale needed** because painter composes layers manually like Photoshop.

**Production rule (LOCK candidate):** Floor base layer SHOULD match room aspect ratio (use the table above). Decorative layers can be any size — `offset` places them.

**REST coverage note (S92 LOCK):** PixelLab REST `/generate-image-v2` uses numeric `image_size` (`width` 16-792, `height` 16-688; aspect-ratio implicit cap; no enum) — 632×424 supported, not Web UI-only. `/generate-with-style-v2` capped at 512×512 max. PixelLab MCP caps separately: `create_object` 256×256, `create_map_object` 400px. **REST API is automation-ready via Bearer token (Codex script can batch-generate).**

---

## 6. Migration impact

| Affected | Action |
|---|---|
| `RoomTemplateSO.backgroundSprite` (added morning) | DELETE field |
| Existing 10 RoomTemplate `.asset` files | None — backgroundSprite was never set on them (cleared Spawn_01 this turn) |
| `RoomBankRuntimeTester.RunTest()` | Replace single-sprite block with foreach-layer block |
| EditMode tests | Should remain 419/419 PASS — no test references `backgroundSprite` per grep this morning |
| Custom Inspector | NEW file `RoomTemplateSOInspector.cs` |

---

## 7. Memory LOCK entry (after Codex approval)

Create `memory/project_multilayer_painter_v1_lock.md`:
- Why: Hades/Alabaster Dawn aesthetic + reusable engine pattern
- Data: BackgroundLayerData class spec
- UX: Phase 1 ReorderableList Inspector, Phase 2 EditorWindow deferred
- Asset rule: floor base matches room aspect (PixelLab Pro sizes table), decorative layers any size + offset
- Cross-link: [[map-plan-v1-lock]], [[3d-portability-strategy]], [[karar-143-pipeline]]

Update `MEMORY.md` index.

Add Karar entry to `TASARIM/MASTER_KARAR_BELGESI.md`: "Karar #147: Multi-Layer Painter System LOCK (S92). Replaces single backgroundSprite. List<BackgroundLayerData> with sortingOrder/offset/scale/tint/visible. Inspector ReorderableList Phase 1, EditorWindow Phase 2."

---

## 8. Open questions for Codex review

1. **sortingOrder collision risk** — Player sprites typically at sortingOrder 0, world props at 10-50. Painted bg layers at -100..-50 should never collide. But should we expose a "sortingLayerName" field per layer (Unity SortingLayer asset) for clean separation? **Recommendation: NO for Phase 1**, simple int order is enough; revisit if z-fighting occurs in playtest.

2. **Mass spawning cost** — If a room has 8 layers and we have 14 MVP rooms, total ~112 SpriteRenderers across rooms. Acceptable (1 room loaded at a time). No batching needed for MVP.

3. **PixelLab character (Warblade) overlap?** — Warblade is a character sprite, not a background layer. Should NEVER live in `backgroundLayers`. Reaffirm scope: backgroundLayers = ONLY static painted decoration. Characters/enemies = separate spawn pipeline (PropPlacementData / runtime spawner).

4. **Phase 1.5 RoomData spec impact** — Spec line 100 says "BackgroundLayer sprite quad rendering BEFORE tile layers". Our List<BackgroundLayerData> aligns. When Phase 1.5 chunked renderer lands, it consumes the same list — no migration needed. ✓

5. **Inspector thumbnail performance** — `AssetPreview.GetAssetPreview` is async; first frame may show null. Use `AssetPreview.IsLoadingAssetPreview` + repaint pattern. Mention in implementation.

---

## 9. Success criteria

- ✓ `BackgroundLayerData` class compiles, no namespace conflicts
- ✓ `RoomTemplateSO.backgroundLayers` field replaces `backgroundSprite`, Inspector shows ReorderableList with drag-reorder + thumbnails
- ✓ `RoomBankRuntimeTester.RunTest()` spawns 1 SR per visible layer with correct sortingOrder/offset/scale/tint
- ✓ 419/419 EditMode tests PASS post-change
- ✓ 10/10 existing RoomTemplate assets deserialize (empty backgroundLayers list = no painted bg, fine)
- ✓ Demo: 3-layer stack assigned to Spawn_01 via UnityMCP, screenshot proves z-ordering works (floor → decal → prop)

---

## 10. Codex review request

Please verify:
- (a) Data model is sound; field set covers user's "painter style" intent (z-order + offset/scale/tint/visible)
- (b) Migration path is clean (no asset breakage)
- (c) Runtime spawn logic is correct (no obvious nullrefs, ordering)
- (d) Phase 1 vs Phase 2 split is reasonable (don't over-build UX before validating data)
- (e) Asset size table aligns with PixelLab Pro reality
- (f) Open questions §8 — your verdict on each

Return: PASS/FAIL with checklist + any FIX-FIRST items. If PASS, orchestrator dispatches Sonnet for Phase 1 implementation immediately.
