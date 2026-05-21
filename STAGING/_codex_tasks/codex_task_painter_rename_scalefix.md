# CODEX TASK — Painter Rename + Per-Category Scale Fix

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Offline mode — bağlam bu task'ın içinde.

---

## Hedef

`RimaUnifiedPainterWindow` editor window'unu **rename** + **per-category default scale** ekle.

## Görev A — Rename

**`RimaUnifiedPainterWindow` → `RimaWorldPainterWindow`**

### File rename
- `Assets/Editor/RimaUnifiedPainterWindow.cs` → `Assets/Editor/RimaWorldPainterWindow.cs`
- AssetDatabase.RenameAsset() veya File.Move + meta file taşı (GUID preserve)

### Class rename
- `public class RimaUnifiedPainterWindow : EditorWindow` → `public class RimaWorldPainterWindow : EditorWindow`
- Namespace `RIMA.Editor.MapDesigner` aynı kalır
- Constructor / nested class isimleri (ScanResult, TerrainGroup, WallVariantGroup) aynı kalır

### Reference update
- Tüm `RimaUnifiedPainterWindow` referansları → `RimaWorldPainterWindow`
- Grep: `grep -r "RimaUnifiedPainterWindow" Assets/` ile tara, hepsini değiştir
- MenuItem path varsa (örn. `MenuItem("RIMA/Unified Painter")` → `MenuItem("RIMA/World Painter")`)

## Görev B — Per-Category Scale Fix

### Problem
Mevcut `prefabScaleMultiplier = 1.0f` (line 44) tüm category'ler için aynı. Prop kategorisinde 64×64 sprite × 1.0 = world 1.0 (player ile aynı, çok büyük). CURRENT_STATUS LOCK: prop scale 0.4 olmalı.

### Çözüm — 4 ayrı default scale field

`prefabScaleMultiplier` mevcut field'ını **koru** (universal override gibi), ekstra olarak per-category default'lar ekle:

```csharp
[SerializeField] private float floorScale = 1.0f;   // Floor tile native
[SerializeField] private float wallScale = 0.5f;    // 128px wall sprite → 1 cell
[SerializeField] private float propScale = 0.4f;    // 64px prop → 0.4 world (CURRENT_STATUS LOCK)
[SerializeField] private float mobScale = 1.0f;     // 64px mob → 1 world
[SerializeField] private bool useCategoryScale = true; // toggle: true=per-category, false=universal multiplier
```

### Apply logic
Prefab instantiate eden YERLERDE (paint, drag-paint, etc.) scale assignment:

```csharp
float effectiveScale;
if (useCategoryScale) {
    effectiveScale = currentCategory switch {
        PaletteCategory.Floor => floorScale,
        PaletteCategory.Wall  => wallScale,
        PaletteCategory.Prop  => propScale,
        PaletteCategory.Mob   => mobScale,
        _ => 1.0f
    };
} else {
    effectiveScale = prefabScaleMultiplier;
}
instance.transform.localScale = new Vector3(effectiveScale, effectiveScale, 1f);
```

### UI ekleme (Inspector window)
Per-category scale toggle + 4 slider field'larını GUI'de göster. Mevcut "Scale Multiplier" slider'ının altına ekle:

```csharp
// Existing slider (universal)
EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
useCategoryScale = EditorGUILayout.Toggle("Per-Category Scale", useCategoryScale);
using (new EditorGUI.DisabledScope(!useCategoryScale)) {
    floorScale = EditorGUILayout.Slider("Floor", floorScale, 0.1f, 2f);
    wallScale  = EditorGUILayout.Slider("Wall",  wallScale,  0.1f, 2f);
    propScale  = EditorGUILayout.Slider("Prop",  propScale,  0.1f, 2f);
    mobScale   = EditorGUILayout.Slider("Mob",   mobScale,   0.1f, 2f);
}
using (new EditorGUI.DisabledScope(useCategoryScale)) {
    prefabScaleMultiplier = EditorGUILayout.Slider("Universal Override", prefabScaleMultiplier, 0.1f, 2f);
}
```

### Persistence
PlayerPrefs ile save/load (mevcut Painter PlayerPrefs pattern'ı varsa onu kullan, yoksa basit):
- `Painter.FloorScale`, `Painter.WallScale`, `Painter.PropScale`, `Painter.MobScale`, `Painter.UseCategoryScale`

## Görev C — Doğrulama

Codex aşağıdakileri doğrular:

1. **Compile**: `read_console` ile error/warning sıfır
2. **Window opens**: MenuItem'dan "RIMA/World Painter" açılır (rename sonrası path)
3. **Per-category scale UI**: 4 slider + toggle görünür
4. **Backward compat**: useCategoryScale=false → eski davranış (universal multiplier)

## Kısıtlar

- Sadece `Assets/Editor/RimaUnifiedPainterWindow.cs` + reference dosyaları değişir
- Diğer painter script'lerine (Act1RoomPainter, LayeredRoomPainter, etc.) DOKUNMA
- Git commit AT THE END: `[S96] Painter rename + per-category scale fix`
- BLOCKED if: file rename başarısız, reference grep eksik, compile error solve edilemiyor

## Final Report

`STAGING/CODEX_DONE_painter_rename_scalefix.md`:
- Rename verdict (file path before/after, GUID preserved?)
- Reference update list (kaç dosyada kaç occurrence değişti)
- Per-category scale verdict (4 slider çalışıyor mu, default değerler doğru mu)
- Compile status (read_console output snippet)
- Commit hash
- Next-step öneri (eski "Unified" referansı hala var mı?)

## Dispatch

```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_painter_rename_scalefix.md --effort high
```

Background. Notify on complete.
