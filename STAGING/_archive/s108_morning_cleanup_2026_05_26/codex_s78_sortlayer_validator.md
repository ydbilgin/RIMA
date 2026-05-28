# Codex S78 Micro-fix — Sort Layer Validator

**Sebep:** rima-qc Codex Phase 1 (S78) review SOFT-PASS verdi. Critical olmayan ama fresh-checkout'ta görünmez overlay riski:
- `PatchOverlayPainter.cs:12` sortingLayerName = "Patch"
- `ScatterBrushPainter.cs:12` sortingLayerName = "Scatter"
- Bu iki sorting layer Project Settings > Tags and Layers'a kayıtlı DEĞIL → Unity silent fallback to "Default" → invisible/wrongly-ordered overlay

**Tahmini süre:** 15-20 dk

---

## Tek deliverable

`Assets/Editor/RimaSortingLayerValidator.cs` (yeni dosya):
- `[InitializeOnLoad]` static class
- Asset reload'da Project Settings > Tags and Layers > Sorting Layers'ı kontrol et
- "Patch" ve "Scatter" layer'ları YOKSA:
  - Default → Patch (order 1) → Scatter (order 2) sırasıyla ekle
  - `SerializedObject` ile `TagManager.asset` üzerinden ekleme (Unity Editor API)
- Layer'lar varsa hiçbir şey yapma (idempotent)
- İlk eklemede Debug.Log: "RIMA: Created sorting layers 'Patch' and 'Scatter' (Karar #135 organic render hierarchy)"

## Acceptance Criteria

1. `dotnet build .\RIMA.Editor.csproj` PASS
2. UnityMCP `read_console` ile compile error sıfır
3. Test: Project Settings > Tags and Layers > Sorting Layers'da "Patch" + "Scatter" görünür (Default'tan SONRA, sıralı)
4. Commit format: `[S78][D7] Sorting layer validator (Patch + Scatter for Karar #135)`

## File Scope

**ALLOWED:**
- `Assets/Editor/RimaSortingLayerValidator.cs` (yeni)

**FORBIDDEN:**
- Diğer tüm dosyalar — sadece bu micro-fix
- Patch/ScatterBrushPainter dokunma (zaten doğru çalışıyor)

## Implementation hint

```csharp
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class RimaSortingLayerValidator
{
    static RimaSortingLayerValidator()
    {
        EnsureLayer("Patch");
        EnsureLayer("Scatter");
    }

    static void EnsureLayer(string layerName)
    {
        var tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
        );
        var layers = tagManager.FindProperty("m_SortingLayers");
        for (int i = 0; i < layers.arraySize; i++)
        {
            if (layers.GetArrayElementAtIndex(i)
                .FindPropertyRelative("name").stringValue == layerName)
                return;
        }
        layers.InsertArrayElementAtIndex(layers.arraySize);
        var newLayer = layers.GetArrayElementAtIndex(layers.arraySize - 1);
        newLayer.FindPropertyRelative("name").stringValue = layerName;
        newLayer.FindPropertyRelative("uniqueID").intValue =
            Mathf.Abs(layerName.GetHashCode());
        tagManager.ApplyModifiedProperties();
        Debug.Log($"RIMA: Created sorting layer '{layerName}' (Karar #135 organic render hierarchy)");
    }
}
```

**Codex:** Yukarıdaki implementation hint'i baseline al, Unity API uygunluğunu doğrula (TagManager.asset path + uniqueID generation). Read every step and execute via shell commands. Do not describe — actually run them.

## Çıktı

- Tek commit: `[S78][D7] Sorting layer validator`
- CODEX_DONE_<profile>.md'ye kısa özet
