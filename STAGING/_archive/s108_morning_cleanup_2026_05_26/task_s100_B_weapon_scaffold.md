ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: Warblade 8-Yön Silah Sistemi Code Scaffold

HandAnchor + OrientationSync + WeaponSorter ile 1 sprite'ın 8 yöne adapte edildiği weapon sistem iskeletini kur.

## Mimari

- 1 sprite/silah (Warblade sol elle). L+R asimetrik = max 2 sprite.
- 8 yön = HandAnchor localPos offset + weapon localRot offset (kod; sprite üretimi yok)
- WeaponSorter: N/NE/NW facing → sprite body'nin arkasında, diğerleri önünde

## Dosyalar (CREATE)

### 1. `Assets/Scripts/Combat/WeaponSorter.cs`
```
MonoBehaviour
- [SerializeField] SpriteRenderer weaponRenderer;
- [SerializeField] SpriteRenderer bodyRenderer;
- public void UpdateSort(FacingDirection dir)
    N/NE/NW → weaponRenderer.sortingOrder = bodyRenderer.sortingOrder - 1
    else     → weaponRenderer.sortingOrder = bodyRenderer.sortingOrder + 1
```

### 2. `Assets/Scripts/Combat/OrientationSync.cs`
```
MonoBehaviour
- [SerializeField] Transform handAnchor;
- [SerializeField] Transform weaponTransform;
- Vector2[] handOffsets = new Vector2[8] { ... } // S, SE, E, NE, N, NW, W, SW
- float[] weaponRotations = new float[8] { ... }  // degrees per direction
- public void Sync(FacingDirection dir) { apply offset + rotation }
```

### 3. `Assets/Scripts/Combat/FacingDirection.cs` (enum, eğer yoksa)
```
public enum FacingDirection { S, SE, E, NE, N, NW, W, SW }
```

### 4. `Assets/Prefabs/Combat/Weapons/Warblade.prefab`
- Root GO: "Warblade"
- Components: WeaponSorter, OrientationSync
- Child GO: "Sprite" — SpriteRenderer (placeholder white sprite veya mevcut warblade sprite varsa onu kullan)

### 5. `Assets/ScriptableObjects/Weapons/WeaponDatabase.asset` (CREATE veya UPDATE)
```
ScriptableObject: WeaponDatabase
- WeaponEntry[] entries
  - id: "warblade"
  - orientationOffsetDegrees: float[8] (varsayılan sıfırlar, sonra ayarlanır)
  - handOffsets: Vector2[8]
```
WeaponDatabase.cs script'i de oluştur (eğer yoksa):
`Assets/Scripts/Combat/WeaponDatabase.cs` — ScriptableObject, WeaponEntry[] entries

## Başarı Kriterleri

- [ ] Compile errors yok
- [ ] WeaponSorter + OrientationSync inspector'da görünür ve attach edilebilir
- [ ] Warblade.prefab Inspector'da WeaponSorter + OrientationSync gösteriyor
- [ ] WeaponDatabase.asset oluşturulmuş, Warblade entry var
- [ ] Console'da hata yok
