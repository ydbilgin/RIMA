# TASK: Cliff Auto-Create + 8→1 Yön Refactor (Atomic)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: İki birbiriyle bağlantılı değişikliği atomik tek dispatch'te yap:
1. **Auto-create:** Painter'daki "Generate Cliffs" butonu sahnede CliffAutoPlacer YOKSA otomatik oluştursun + bind etsin + Regenerate çağırsın. Şu an disabled kalıyor ("No CliffAutoPlacer in scene") — user sildikten sonra UI üzerinden tek tıkla geri alabilsin.
2. **8→1 yön refactor:** CliffPlacementRules 9 sprite slot (4 cardinal + 4 corner + 1 accent) yerine **1 base sprite + List<Sprite> variants** tutsun. CliffAutoPlacer direction-agnostic seçim yapsın (placement için direction enum kalsın — boşluk dedect için lazım, ama sprite seçimi tek listeden DeterministicChance ile).

İkisi atomic çünkü auto-create yeni rules asset format'ı ile uyumlu olmalı.

## Hedef dosyalar
1. `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs` — auto-create logic ekle
2. `Assets/Scripts/Environment/CliffPlacementRules.cs` — 9 sprite alanı → 1 base + variants
3. `Assets/Scripts/Environment/CliffAutoPlacer.cs` — sprite seçimini variants'tan deterministic yap, GetSprite(direction) çağrısı kaldır
4. `Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset` — yeni format, mevcut cliffS sprite'ı base yap, diğer 8'i variants listesine taşı (mevcut Kit B 9 sprite, hepsini variants'a koyabiliriz tek base seçip — base = mevcut cliffS)

## Adım 1: CliffPlacementRules.cs refactor
**Sil:** `cliffS, cliffN, cliffE, cliffW, cliffNE, cliffNW, cliffSE, cliffSW, cliffAccent, accentChance` field'ları
**Sil:** `GetSprite(CliffDirection)` method
**Ekle:**
```csharp
[Header("Cliff sprite")]
public Sprite cliffBase;                           // ana sprite (variants içine de eklenmiş gibi davranır)
public Sprite[] cliffVariants = Array.Empty<Sprite>();  // varyant pool — base dahil tüm seçenekler

public Sprite GetVariant(int seed)
{
    if (cliffVariants == null || cliffVariants.Length == 0) return cliffBase;
    int idx = (seed & 0x7fffffff) % cliffVariants.Length;
    return cliffVariants[idx];
}
```
Direction enum + DirectionOffset struct KALSIN (offset placement için hala lazım). worldOffset, directionOffsets[], spriteScale, spritePivot, sortingOrder, sortingLayer, pixelsPerUnit korunsun.

## Adım 2: CliffAutoPlacer.cs refactor
- `AddPlacement` ve `Placement.Sprite` field'ı yön-agnostik olmalı
- `SelectSprite(placement)` method'unu kaldır, doğrudan `rules.GetVariant(deterministicSeed)` çağır
- Deterministic seed = `DeterministicChance(cell, direction)` çıkarımının int hash'i
- `rules.cliffAccent` ve `rules.accentChance` referansları kaldır (sprite alanları silindi)

## Adım 3: CliffPlacementRules_Hades.asset güncelle
YAML'i yeni format'a çevir:
- Mevcut cliffS (`2a3d49363e3628c4292a7d2c6f575c9e`) → `cliffBase`
- Mevcut 9 sprite (cliffS, cliffN, cliffE, cliffW, cliffNE, cliffNW, cliffSE, cliffSW, cliffAccent) → `cliffVariants` array
- Eski alanları sil (`cliffS`, `cliffN`, ..., `accentChance`)
- Yeni alan formatı:
  ```yaml
  cliffBase: {fileID: 21300000, guid: 2a3d49363e3628c4292a7d2c6f575c9e, type: 3}
  cliffVariants:
  - {fileID: 21300000, guid: 2a3d49363e3628c4292a7d2c6f575c9e, type: 3}
  - {fileID: 21300000, guid: eff10014d1767a84c99113624ea94080, type: 3}
  - {fileID: 21300000, guid: c08164f7fa618734897d163dd95333ff, type: 3}
  - {fileID: 21300000, guid: b8ce4a81b9a8c404a8aaa0c4e92b7211, type: 3}
  - {fileID: 21300000, guid: 3f3ab8c77a30738418aa094d2ad3efa4, type: 3}
  - {fileID: 21300000, guid: 62da8f70f5c21fe44999dff49dffcb15, type: 3}
  - {fileID: 21300000, guid: 8e059e7d4307669438753289a79c7059, type: 3}
  - {fileID: 21300000, guid: cced08e37b130ad43a4bf86632242964, type: 3}
  - {fileID: 21300000, guid: 366bfe9e05bd10041b1194c2942a971f, type: 3}
  ```
- `worldOffset.y: 0.15`, `spriteScale: (1, 1)` — mevcut tweak değerleri korunsun
- `directionOffsets: []` boş kalsın (şimdilik)

## Adım 4: CliffGenerateAction.cs auto-create logic
Mevcut `DrawButton(float height)` yerine 3 durum handle eden mantık ekle:

```csharp
public static void DrawButton(float height = 32f)
{
    var placer = UnityEngine.Object.FindObjectOfType<RIMA.Environment.CliffAutoPlacer>();

    if (placer == null)
    {
        // Auto-create state
        var content = new GUIContent("🪨 Create CliffAutoPlacer + Generate",
            "Sahnede CliffAutoPlacer yok — tıkla, otomatik oluştur ve cliff'leri üret.");
        if (GUILayout.Button(content, GUILayout.Height(height)))
        {
            placer = AutoCreatePlacer();
            if (placer != null && placer.IsReady)
            {
                placer.Regenerate();
                EditorUtility.SetDirty(placer.gameObject);
                EditorSceneManager.MarkSceneDirty(placer.gameObject.scene);
                Debug.Log($"[CliffGenerate] Created + generated {placer.LastGeneratedCount} cliffs.");
            }
            else if (placer != null)
            {
                Debug.LogWarning("[CliffGenerate] Placer created but not ready — check floor Tilemap + rules asset.");
            }
        }
        return;
    }

    // ... existing IsReady check + Regenerate button (mevcut kod) ...
}

private static RIMA.Environment.CliffAutoPlacer AutoCreatePlacer()
{
    // Find or create CliffRing GameObject
    var ringGO = GameObject.Find("CliffRing");
    if (ringGO == null)
    {
        ringGO = new GameObject("CliffRing");
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(ringGO.scene);
    }

    var placer = ringGO.GetComponent<RIMA.Environment.CliffAutoPlacer>();
    if (placer == null) placer = ringGO.AddComponent<RIMA.Environment.CliffAutoPlacer>();

    // Auto-bind floor Tilemap (find first non-VoidBlocker Tilemap in scene)
    if (placer.floorTilemap == null)
    {
        var tilemaps = UnityEngine.Object.FindObjectsOfType<UnityEngine.Tilemaps.Tilemap>();
        foreach (var tm in tilemaps)
        {
            if (tm.gameObject.name.ToLower().Contains("void")) continue;
            placer.floorTilemap = tm;
            break;
        }
    }

    // Auto-bind rules asset
    if (placer.rules == null)
    {
        placer.rules = UnityEditor.AssetDatabase.LoadAssetAtPath<RIMA.Environment.CliffPlacementRules>(
            "Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset");
    }

    // cliffParent = self
    if (placer.cliffParent == null) placer.cliffParent = ringGO.transform;

    return placer;
}
```

## Adım 5: Compile + verify
- `read_console` — 0 error, 0 warning (mevcut "Multiple embedded packages" hariç)
- Asset import sırasında break olmasın (CliffPlacementRules_Hades sprite ref'leri kalmalı)
- Unity'de painter aç, "🪨 Create CliffAutoPlacer + Generate" butonunun göründüğünü ve disabled olmadığını doğrula
- Manual test gerekmiyor, sadece compile + asset import OK

## Hard constraints
- Backward-compat YASAK — eski 9-slot sprite alanları silinecek (CLAUDE.md "Don't add features... refactor or introduce abstractions beyond what the task requires.")
- Surgical — sadece listelenen 4 dosya
- BLOCKED: asset YAML format'ı Unity tarafından invalid sayılırsa, asmdef reference issue
- Commit YAPMA

## Inline rapor (<400 kelime)
- Her 4 dosyada yapılan değişiklik (kısa)
- Compile error count (0 hedef)
- Asset import OK mı (rules asset Inspector'da görünüyor mu)
- BLOCKED varsa neden
