ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Not needed for this fix.

# Amaç: Task G Fix — MapPanel UI namespace RIMA → RIMA.UI.Map

5 dosyada namespace RIMA yerine RIMA.UI.Map olması gerekiyor.

## Değiştirilecek dosyalar

Her dosyada `namespace RIMA` → `namespace RIMA.UI.Map`:

1. `Assets/Scripts/UI/Map/MapNodeData.cs`
2. `Assets/Scripts/UI/Map/MapGraphData.cs`
3. `Assets/Scripts/UI/Map/MapPanelUI.cs`
4. `Assets/Scripts/UI/Map/MapNodeUI.cs`
5. `Assets/Scripts/UI/Map/MapConnectionUI.cs`

## Adımlar

Her dosyada:
- `namespace RIMA` → `namespace RIMA.UI.Map`
- Eğer başka dosyalar bu tipleri `using RIMA;` ile çekiyorsa, onları `using RIMA.UI.Map;` olarak güncelle (grep ile bul)

## Başarı Kriterleri
- [ ] 5 dosyada `namespace RIMA.UI.Map`
- [ ] `namespace RIMA` yok (Map dosyalarında)
- [ ] Compile errors yok
- [ ] Console 0 error
