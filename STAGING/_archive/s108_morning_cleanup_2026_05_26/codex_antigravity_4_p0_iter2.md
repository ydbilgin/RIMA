# Antigravity 4 P0 iter 2 — execute every step, commit at end

## Context

User Alabaster Dawn doğallık seviyesi istiyor. Antigravity (önceki agent) 4 P0 kuralı LOCK etti (user-memory `feedback_antigravity_2_5d_locked_specs.md`). Bunlar RIMA Room Designer'ı 1 tile-block görünümünden Alabaster Dawn polish bar'ına çıkaracak en yüksek görsel ROI dispatch'i.

**RIMA mevcut durumu:** Antigravity AutoCliff brush + Demo Cliff Map yaptı, Unity error 0 (Sonnet temizledi). Şimdi 4 P0 iter 2 implementasyonu.

## 4 P0 Kuralı (iter 2 implementation)

### P0.1 — Y-Axis Sorting (Kusursuz Derinlik)

**Setup:**
```csharp
// Once at runtime/editor — bootstrap script
using UnityEngine.Rendering;
GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
GraphicsSettings.transparencySortAxis = new Vector3(0f, 1f, 0f);
```

Plus: `Edit > Project Settings > Graphics > Transparency Sort Mode = Custom Axis, Sort Axis (0, 1, 0)`. Code-side bootstrap GraphicsSettingsBootstrap.cs ekle (Awake / OnEnable / Editor sync).

**Sprite pivot:**
- Tüm SpriteRenderer pivot = **Bottom-Center**
- TextureImporter sprite mode Single → pivot Bottom; Multiple → SpriteEditor pivot Bottom-Center
- Mevcut Warblade/mob/tile sprite'larını Custom Pivot (0.5, 0) ile re-import et (Bottom-Center)

### P0.2 — Drop Shadow Layer (Layer 1.5)

**Asset:**
- Create `Assets/Art/VFX/DropShadow_Wall.asset` — TileBase, semi-transparent oval/rect sprite, multiply blend mode
- Create `Assets/Art/VFX/DropShadow_Prop_Oval.png` — 32×16 px oval, color #000000 alpha 0.4

**Implementation:**
- RoomDesignerWindow: Wall painted → cell-below check → if Floor → paint DropShadow on Decal layer (z-sort Layer 1.5 between Floor 1 ve Wall 2)
- Prop placement: child GameObject "Shadow" with oval multiply SpriteRenderer (-0.2 Y offset, parent prop transform)
- Karar #116(g) extend — wall + prop için zorunlu

**Unity tilemap layer order:**
```
- Floor (sortingOrder 0, Layer "Floor")
- DropShadow (sortingOrder 1, Layer "Shadow", blend Multiply or Color 0,0,0 alpha 0.4)
- Wall (sortingOrder 2, Layer "Wall", collision ON)
- Prop (sortingOrder 3, Layer "Prop")
```

### P0.3 — Elevation (Uçurum/Tepe Front + Top yüz)

**Tile asset structure:**
- Each Wall tile entry has TWO sub-sprites:
  - `wall_front` — vertical face (collision ON), darker shadow tone
  - `wall_top` — horizontal top surface (no collision, walkable), lighter tone matching floor
- RuleTile rules: top-most cell of wall stack → wall_top; body cells → wall_front

**Tilemap layer split:**
- `WallsTilemap_Front` (collision ON) — vertical faces
- `WallsTilemap_Top` (collision OFF, walkable) — top surfaces

**RoomDesigner brush extension:**
- Wall painted → place wall_front at cursor, wall_top one cell above (relative tilemap stack)
- Karar #116(f) compliance — sprite-baked Y-offset, no 3D geometry

### P0.4 — Border/Highlight on Wang Transitions (1px outline)

**Implementation choice (Codex karar versin):**
- **Option A — Sprite-baked:** Wang transition tiles already include 1px outline in PixelLab gen. Karar #116 PixelLab Pro prompt'a "1px lighter outline on upper seam, 1px darker outline on lower seam" eklenir. Tek-zaman gen, deterministic.
- **Option B — Shader-based:** URP 2D shader pass — terrain edge detect, draw 1px lighter upper / darker lower. Runtime, all tiles auto.
- **Option C — Post-process:** Aseprite manual outline per Wang sheet. Bir-zaman ama labor-intensive.

**Codex recommendation:** Option A (sprite-baked) — Karar #115 deterministic spec ile uyumlu, no runtime cost, PixelLab Pro prompt'a entegre. Codex implementation: pixellab_master_pipeline.md veya equivalent prompt template dosyasına "1px seam outline" mandatory clause ekle.

## Task — Execute all steps

### STEP 1 — Y-Sort bootstrap script

Create `Assets/Scripts/Core/GraphicsSettingsBootstrap.cs`:
- `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]` static method
- Set `GraphicsSettings.transparencySortMode = CustomAxis` and `transparencySortAxis = (0,1,0)`
- Plus Editor side `[InitializeOnLoad]` static class same logic (Editor + Build coverage)

Also update `ProjectSettings/GraphicsSettings.asset` directly if possible (Edit programmatic):
- `m_TransparencySortMode: 3` (CustomAxis)
- `m_TransparencySortAxis: {x: 0, y: 1, z: 0}`

### STEP 2 — Sprite pivot batch fix

For all sprite assets in `Assets/Art/Characters/**`, `Assets/Art/Tiles/F1/**`, `Assets/Art/Mobs/**`:
- TextureImporter pivot = Custom (0.5, 0) Bottom-Center
- Script: `Assets/Editor/Tools/SpritePivotBatchFix.cs` — menu `RIMA > Tools > Fix All Sprite Pivots`

### STEP 3 — Drop Shadow asset + tilemap layer

- Create `Assets/Art/VFX/DropShadow_Oval.png` (32×16, oval black alpha 0.4) — generate procedurally or use texture
- Create `Assets/Art/VFX/DropShadow_Wall.asset` TileBase variant
- Update RoomDesignerWindow brush logic: wall paint → DropShadow place on cell-below
- Update prop placement code: instantiate "Shadow" child GameObject with oval multiply sprite

### STEP 4 — Wall Front + Top tile structure

- Update RuleTile asset for F1 wall to expose `wall_front_sprite` and `wall_top_sprite` slots
- Split existing WallsTilemap into `WallsTilemap_Front` (collision) + `WallsTilemap_Top` (no collision)
- RoomDesignerWindow paint logic: wall click → wall_front at (x,y), wall_top at (x, y+1) if cell empty

### STEP 5 — 1px Wang outline (Option A sprite-baked)

- Update `STAGING/PIXELLAB_PRODUCTION_STEPS.md` (or pixellab_master_pipeline.md if exists) — Wang transition prompt mandatory clause: "1px lighter outline on upper terrain seam (HSV +10%), 1px darker outline on lower terrain seam (HSV -10%), crisp edges no muddy pixel blend"
- Add commit note: existing F1 Wang tile sheets will be re-generated in next PixelLab batch (no automatic re-gen, manual user task)

### STEP 6 — Unity test

- Open Demo Cliff Map scene
- Play mode test:
  - Karakter cliff arkasına geçince doğru sort (Y-Axis Sort)
  - Wall + Prop'lar altında shadow görünür (Drop Shadow)
  - Wall top face walkable görünür (Elevation)
- read_console error/warning clean

### STEP 7 — Commit

```bash
git add Assets/Scripts/Core/GraphicsSettingsBootstrap.cs Assets/Editor/Tools/SpritePivotBatchFix.cs Assets/Art/VFX/DropShadow_*.png Assets/Art/VFX/DropShadow_*.asset Assets/Editor/RoomDesigner/**/*.cs ProjectSettings/GraphicsSettings.asset STAGING/PIXELLAB_PRODUCTION_STEPS.md
git commit -m "[antigravity-iter2] 4 P0 — Y-Sort + Drop Shadow + Wall Front/Top + Wang outline

- Y-Axis Sort CustomAxis (0,1,0) bootstrap + ProjectSettings
- Sprite pivot batch fix Bottom-Center (Characters/Mobs/Tiles)
- DropShadow tile + auto-place on wall paint (Layer 1.5)
- Wall Front (collision) + Top (walkable) tilemap split
- 1px Wang outline mandatory clause in PixelLab prompts"
```

### STEP 8 — Report

Write `STAGING/antigravity_4_p0_iter2_report.md`:
```markdown
# Antigravity 4 P0 iter 2 Implementation Report

## Y-Sort
[implemented Y/N, test result]

## Drop Shadow
[asset + layer + logic — implemented Y/N]

## Elevation Front + Top
[tilemap split + RuleTile — implemented Y/N]

## 1px Wang outline
[prompt template updated, existing tile re-gen pending Y/N]

## Test (Demo Cliff Map)
[Y-Sort visual: pass/fail]
[Drop Shadow visual: pass/fail]
[Walkable top face: pass/fail]
[Console: 0 errors?]

## Pending (next iter)
[1px Wang outline existing tile re-gen via PixelLab Web UI — user task]
```

Append CODEX_DONE.md.

## Constraints

- Execute every step
- DO NOT change Karar # locks (sadece implementation, spec değişmez)
- Test in Unity before commit (read_console clean)
- If Sprite Pivot batch fix breaks existing scene refs, ROLLBACK that step and report
- Drop Shadow assets: procedural generation via Codex inline texture creation if possible (32x16 oval pixmap)

## Source References (read these)

1. `C:\Users\ydbil\.ccs\instances\yasinderyabilgin\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\feedback_antigravity_2_5d_locked_specs.md` — 4 P0 spec (Codex MCP olmadan oku, file system path)
2. `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #115/#116/#117/#118 references
3. `Assets/Editor/RoomDesigner/**/*.cs` — Antigravity'nin mevcut brush kodu
4. `Assets/Scripts/Core/LargeDungeonMapPainterBase.cs` — wall paint logic
5. `Packages/manifest.json` — `com.unity.2d.tilemap.extras` zaten yüklü
