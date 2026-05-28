ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: Task B Fix — FacingDirection rename + WeaponDatabase duplicate kaldır

Task B (weapon scaffold) iki hata yaptı — bunları düzelt.

## Fix 1: FacingDirection enum → FacingDir8

### Neden:
`PlayerController.cs` içinde `Vector2 FacingDirection` property'si zaten var.
Yeni bir `FacingDirection` enum yaratmak namespace ambiguity yaratır.

### Adımlar:
1. Eğer `Assets/Scripts/Combat/FacingDirection.cs` yaratıldıysa:
   - İçeriği `FacingDir8` olarak rename et
   - Dosyayı `Assets/Scripts/Core/FacingDir8.cs` olarak taşı (Assets/Scripts/Combat/FacingDirection.cs sil)
2. Eğer `FacingDirection` enum kullanılan script varsa (WeaponSorter.cs, OrientationSync.cs) → `FacingDir8` olarak güncelle
3. Dosya yaratılmamışsa: `Assets/Scripts/Core/FacingDir8.cs` dosyasını oluştur:
   ```csharp
   namespace RIMA.Core
   {
       public enum FacingDir8 { S, SE, E, NE, N, NW, W, SW }
   }
   ```

## Fix 2: WeaponDatabase duplicate → WeaponDatabaseSO kullan

### Neden:
`Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs` (namespace `RIMA`) zaten mevcut.
Task B `Assets/Scripts/Combat/WeaponDatabase.cs` adında duplicate yaratmış olabilir.

### Adımlar:
1. `Assets/Scripts/Combat/WeaponDatabase.cs` varsa → SİL (ve .meta)
2. `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs` oku — mevcut `WeaponEntry` struct'a `handOffsets: Vector2[8]` field'ı yoksa ekle
3. Warblade.prefab veya diğer scriptlerde `WeaponDatabase` referansı varsa → `WeaponDatabaseSO` olarak güncelle
4. `Assets/ScriptableObjects/Weapons/WeaponDatabase.asset` varsa ve `WeaponDatabase` tipindeyse → sil, yerine `WeaponDatabaseSO.asset` oluştur (WeaponDatabaseSO tipinde)

## Başarı Kriterleri
- [ ] `Assets/Scripts/Combat/FacingDirection.cs` YOK
- [ ] `Assets/Scripts/Core/FacingDir8.cs` VAR, namespace RIMA.Core
- [ ] `Assets/Scripts/Combat/WeaponDatabase.cs` YOK
- [ ] `WeaponDatabaseSO.cs` WeaponEntry içinde `handOffsets Vector2[8]` var
- [ ] Compile errors yok
