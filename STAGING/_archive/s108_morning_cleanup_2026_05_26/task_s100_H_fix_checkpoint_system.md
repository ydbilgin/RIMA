ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Not needed for this fix.

# Amaç: Task H Fix — CheckpointSystem.cs eski sistem kaldır

rima-qc bulgusu: `Assets/Scripts/Core/CheckpointSystem.cs` (namespace RIMA) ile yeni `CheckpointManager.cs` (namespace RIMA.Save) iki paralel save sistemi oluşturuyor. Eski sistem superseded — kaldır.

## Silinecek

- `Assets/Scripts/Core/CheckpointSystem.cs`
- `Assets/Scripts/Core/CheckpointSystem.cs.meta`

## Adımlar

1. Grep ile `CheckpointSystem` referanslarını bul: `rg "CheckpointSystem" Assets/Scripts/`
2. Eğer başka script CheckpointSystem'i kullanıyorsa → o referansı CheckpointManager'a güncelle (ya da BLOCKED yaz)
3. Eğer kullanılmıyorsa → dosyayı ve .meta'yı sil
4. Grep ile `RIMA.CheckpointData` (eski, CheckpointSystem içindeki) referanslarını bul. Varsa `RIMA.Save.CheckpointData` ile replace et
5. AssetDatabase.Refresh() + compile
6. Console 0 error kontrol

## Başarı Kriterleri
- [ ] `Assets/Scripts/Core/CheckpointSystem.cs` YOK
- [ ] Compile errors yok
- [ ] Console 0 error
- [ ] Eğer referans varsa BLOCKED yaz, orchestrator yönlendirir
