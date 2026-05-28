# TASK: Assets/Sprites Cleanup with Unity Dependency Check (HASSAS)

ACTIVE RULES: (1) think before deleting (2) min surgical action (3) MOVE not DELETE (4) BLOCKED if unsure.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: `Assets/Sprites/` altında **kullanılmayan** asset klasörlerini conservative cleanup. Unity dependency check zorunlu — scene/prefab referansı varsa DOKUNMA. MOVE not DELETE.

## User Kararları (kesin)
- **TUTULACAK:**
  - `Assets/Sprites/Tiles/` (floor + dimetric tiles)
  - `Assets/Sprites/AssetPackV3/walls/` (duvarlar — "belki kullanılır")
  - `Assets/Sprites/Environment/KitB_Cliff/` (mevcut cliff sprite'lar — şimdilik)
  - `Assets/Sprites/Environment/KitC_BG/` (parallax BG, ileride aktif)
  - `Assets/Sprites/UI/` (UI elemanları)
- **SİLİNECEK (eğer Unity dependency YOK ise):**
  - `Assets/Sprites/Characters/Anchors/` (eski karakter anchor data — user net dedi temizle)
  - `Assets/Sprites/AssetPackV3/` İÇİNDE walls + tiles dışındaki içerik (eski procgen)
- **ŞÜPHELI (önce dependency check sonra karar):**
  - `Assets/Sprites/Mobs/` — yeni mob sprite'ları var, eski olanları sil
  - `Assets/Sprites/Enemies/` — Enemies aktif mi?
  - `Assets/Resources/Characters/Anchors/` (Resources altında ayrı path) — eski

## Yapılacaklar

### Adım 1: Inventory + dependency taxonomy
```csharp
// execute_code ile her klasör için dependency tarama:
foreach (string folderPath in suspiciousPaths) {
    string[] assets = AssetDatabase.FindAssets("", new[] { folderPath });
    foreach (string guid in assets) {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        string[] dependents = FindAssetUsages(path);
        // Eğer prefab/scene/scriptableObject dependent varsa: TUT
        // Yoksa: archive candidate
    }
}
```

UnityMCP `manage_asset` action=search ile de bulunabilir.

### Adım 2: Categorize per file
- `unused` → arşivlenebilir
- `referenced` → DOKUNMA, raporla
- `unknown` → şüphede BIRAK

### Adım 3: Move to _archive
- Hedef: `Assets/_archive_sprites_overnight_2026_05_26/` (proje root altında, Assets/ altında değil — Unity import etmesin)
- VEYA: `_archive_sprites_overnight_2026_05_26/` (proje root, Assets dışı)
- Tilde suffix ile Unity ignore: `_archive_sprites_overnight_2026_05_26~/`
- Asset + .meta dosyaları BERABER taşı (Unity GUID kayboldu olmasın)
- Asset Database refresh sonra

### Adım 4: Verify
- Taşınan dosya sayısı
- Console error count (0 hedef — referans kırılmamış)
- Scene assets hala görünür mü (PlayableArena_Test01 + PlayableArena)

## Hard Constraints
- **MOVE not DELETE**
- Dependency varsa DOKUNMA
- Şüphede BIRAK
- .meta dosyalarını BERABER taşı (Unity GUID consistency)
- Asset Database refresh after move
- Console error 0 olmalı
- Commit YAPMA, git YAPMA

## Inline rapor (<500 kelime)
- Inventory: hangi klasörler tarandı
- Each suspicious folder için: dependency var mı, taşındı mı, kaldı mı
- Taşınan toplam asset sayısı
- Console error count
- BLOCKED veya beklenmedik durumlar
