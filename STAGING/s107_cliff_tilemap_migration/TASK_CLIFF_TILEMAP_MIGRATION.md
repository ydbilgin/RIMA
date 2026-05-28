# TASK: Cliff Rendering — GameObject → Tilemap Migration (Agy Verdict)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Agy verdict'i (`STAGING/s107_cliff_optimization_research/`) uygulamak — CliffAutoPlacer artık her cliff için GameObject + SpriteRenderer instantiate ETMEYECEK; bunun yerine ikinci bir Tilemap'e `SetTile` çağrısıyla yerleştirecek. **Hedef perf kazancı:** 1.3 MB → 0.01 MB bellek, 536 → 1-2 draw call, Editor Hierarchy temiz.

## Bağlam (önceki refactor)
8→1 yön refactor zaten tamam: CliffPlacementRules artık `cliffBase` + `cliffVariants[]` tutuyor, GetVariant(int seed) deterministic random. Bu migration onunla uyumlu — variant seçimi Tilemap tile'a taşınacak.

## Yeni dosyalar

### 1. `Assets/Scripts/Environment/DeterministicVariantTile.cs` (yeni)
TileBase türevi, per-cell deterministic variant seçimi yapan custom tile:

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    [CreateAssetMenu(fileName = "DeterministicVariantTile", menuName = "RIMA/Environment/Deterministic Variant Tile")]
    public sealed class DeterministicVariantTile : TileBase
    {
        public Sprite baseSprite;
        public Sprite[] variants;
        public Vector3 transformOffset;  // worldOffset.y=0.15 buraya
        public Vector2 spriteScale = Vector2.one;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.colliderType = Tile.ColliderType.None;
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.color = Color.white;
            tileData.transform = Matrix4x4.TRS(
                transformOffset,
                Quaternion.identity,
                new Vector3(spriteScale.x, spriteScale.y, 1f));

            if (variants != null && variants.Length > 0)
            {
                int seed = DeterministicSeed(position);
                tileData.sprite = variants[(seed & 0x7fffffff) % variants.Length];
            }
            else
            {
                tileData.sprite = baseSprite;
            }
        }

        private static int DeterministicSeed(Vector3Int pos)
        {
            unchecked
            {
                int h = 17;
                h = h * 31 + pos.x;
                h = h * 31 + pos.y;
                h = h * 31 + pos.z;
                h ^= h << 13; h ^= h >> 17; h ^= h << 5;
                return h;
            }
        }
    }
}
```

## Değişen dosyalar

### 2. `Assets/Scripts/Environment/CliffAutoPlacer.cs` (refactor)
- `cliffParent` (Transform) yerine yeni alan: `public Tilemap cliffTilemap;` (target tilemap)
- `clearExistingOnRegenerate` aynı — ama artık `ClearChildren` yerine `cliffTilemap.ClearAllTiles()`
- `CreateCliff(Placement, int, Transform)` → `PlaceTile(Placement)` — child GameObject DEĞIL, tilemap.SetTile çağrısı
- `cliffTile: DeterministicVariantTile` alanı (ScriptableObject ref) — tek tile asset variant pool ile
- Sprite picking artık CliffAutoPlacer'da YOK — DeterministicVariantTile içinde
- `Regenerate()` algoritması aynı (CollectPlacements direction check)
- Her cell için cliff direction adetine göre o cell'de **tilemap'te 1 tile** olur (yan yana çok yön ise ilk gelen kazanır VEYA bir mantık ekle: direction'a göre cliff cell offseti?)

**ÖNEMLI mimari karar:** Tilemap tek hücrede 1 tile tutar. Eski sistem aynı floor cell'in 4 yönüne 4 ayrı cliff koyabiliyordu (cell etrafında halka). Tilemap'e geçince her cliff kendi cell'inde olmalı. Çözüm: cliff'i adjacent boş cell'e koy (komşu boş hücre cliff hücresi olur), her cliff kendi cell'inde. Bu hesabı CollectPlacements'ta yap:
```csharp
// for each floor cell:
//   if south empty -> place tile at (cell + SouthCell), not at cell
//   if north empty -> place tile at (cell + NorthCell)
//   ...
```
Bu sayede aynı boş hücreye birden fazla cliff denerse yine 1 tile kalır (son yazan kazanır) — corner case için `cliffTilemap.HasTile()` check ile skip.

### 3. `Assets/ScriptableObjects/Environment/CliffPlacementRules_Hades.asset` (minor edit)
- `worldOffset.y=0.15` ve `spriteScale=(1,1)` korunsun — ama bu değerler artık `DeterministicVariantTile` asset'ine transform olarak geçer (yeni tile asset'inde transformOffset=(0,0.15,0), spriteScale=(1,1))
- Rules artık placement geometry için kullanılır (komşu check, direction enum), sprite array değil
- `cliffBase` + `cliffVariants` korunsun — sonra DeterministicVariantTile asset'i yaratırken referans için kullanılacak
- VEYA daha temiz: DeterministicVariantTile asset'ini doğrudan rules'a ekle (`public DeterministicVariantTile cliffTile;`), Inspector'dan rule asset'e tek bir tile asset bağlanır
- **Tercih:** Rules SADECE placement metadata tutsun (direction enum, offset, sorting). Sprite veri DeterministicVariantTile'a taşınsın. Yeni asset: `Assets/ScriptableObjects/Environment/CliffTile_Hades.asset` (DeterministicVariantTile instance).

### 4. `Assets/Prefabs/Scene/CliffTilemap.prefab` (yeni) — OPTIONAL
Veya scene'de doğrudan CliffRing altında bir child GameObject:
- GameObject "CliffTilemap" — Grid'in child'ı (Floor ile aynı parent)
- Components: Tilemap, TilemapRenderer
- TilemapRenderer.sortingLayerName = "Ground"
- TilemapRenderer.sortingOrder = -50
- Material: standart Sprites-Default veya URP 2D Lit Sprite

Aslında daha basit: CliffAutoPlacer'ın AutoCreatePlacer logic'ine bu Tilemap yaratma ekle. Tek tıkla CliffRing + CliffTilemap + TilemapRenderer + DeterministicVariantTile asset bağlama hep beraber.

## Adım 5: CliffGenerateAction.cs auto-create logic güncelle
- AutoCreatePlacer artık CliffTilemap (child GameObject + Tilemap component + TilemapRenderer) da yaratmalı
- DeterministicVariantTile asset'ini auto-bind etmeli (`Assets/ScriptableObjects/Environment/CliffTile_Hades.asset`)
- Eğer asset yoksa LogWarning + selection'a alma

## Adım 6: Compile + verify
- `read_console` — 0 error
- Asset yaratıldı, Inspector'da DeterministicVariantTile görünür ve dolu
- Auto-create butonu çalışıyor — CliffRing + CliffTilemap + tile asset hepsi auto-bind
- Regenerate sonrası Hierarchy temiz (CliffRing içinde sadece CliffTilemap child, 500+ AutoCliff GameObject YOK)

## Hard constraints
- Backward-compat YASAK — eski `cliffParent` + ClearChildren mantığı silinir
- Surgical — sadece listelenen dosyalar
- BLOCKED: TileBase reflection compile issue, asmdef reference eksikse
- Commit YAPMA

## Bilinen riskler (Agy'den)
1. **Y-sort:** Player cliff arkasına geçtiğinde derinlik sorunu olabilir. İlk fazda tek Tilemap, gerekirse ileride Foreground/BackgroundCliffs ayrımı eklenir. Şimdilik URP Transparency Sort Mode = Custom Axis Y=1 zaten ayarlı (verify with `manage_graphics`).
2. **SpriteAtlas:** Agy önerdi ama bu task'a dahil DEĞIL. Tile asset variants[] zaten tek tile asset altında, atlas otomatik batching'i yapar (URP 2D Renderer). Eğer manuel atlas gerekirse ayrı task.
3. **Variant seçimi:** Tilemap cell'i recached oluyor — `DeterministicVariantTile.GetTileData` her draw'da çağrılır mı yoksa cache'li mi? Test ile doğrula. Cache'li ise sorun yok (deterministic seed her zaman aynı sprite döner).

## Inline rapor (<500 kelime, NOT file)
- Yeni dosya path + satır sayısı
- Değişen dosya path + edit özeti
- Compile error count (0 hedef)
- Hierarchy verify: CliffRing → CliffTilemap → tile asset, 500+ child GameObject YOK
- Auto-create test edilmiş mi (button click sim)
- Tilemap tile count = eski cliff GameObject count (her 1 boş komşu = 1 tile)
- BLOCKED varsa neden
