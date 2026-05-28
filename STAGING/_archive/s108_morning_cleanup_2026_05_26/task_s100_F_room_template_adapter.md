ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: RoomTemplateSO → RoomData 6-Katmanlı Prosedürel Boyama Adaptörü

Mevcut `RoomTemplateSO` ScriptableObject'inden `RoomData` runtime yapısına dönüşüm adaptörü. 6-katmanlı Multi-Layer Painter altyapısına bağlanır.

## NLM Context Çek

Başlamadan önce ZORUNLU sorgula:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "RoomTemplateSO RoomData yapısı, 6-katmanlı Multi-Layer Painter mimarisi, SubRoomSequenceController entegrasyonu, mevcut script dosya yolları"
```

## Beklenen Mimari (NLM'den doğrula)

- `RoomTemplateSO`: tasarım zamanı oda tanımı (layer painting data)
- `RoomData`: runtime oda state (enemies, props, triggers)
- Adaptör: `RoomTemplateAdapter.cs` — static veya MonoBehaviour, Convert(RoomTemplateSO) → RoomData

### File Paths (NLM'den mevcut yolları öğren, yoksa CREATE)
- `Assets/Scripts/Map/Runtime/RoomTemplateAdapter.cs`
- Mevcut `RoomTemplateSO.cs` yolunu NLM'den öğren — SADECE genişlet, yeni dosya yaratma
- Mevcut `RoomData.cs` yolunu NLM'den öğren — gerekirse field ekle

## Başarı Kriterleri
- [ ] `RoomTemplateAdapter.Convert(RoomTemplateSO)` → `RoomData` çalışıyor
- [ ] 6 layer her biri RoomData'ya map ediliyor
- [ ] Compile errors yok
- [ ] BLOCKED yaz eğer NLM'den mevcut RoomTemplateSO/RoomData şeması çekilemezse
