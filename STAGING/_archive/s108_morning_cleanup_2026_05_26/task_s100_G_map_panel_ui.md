ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: MapPanel UI — Kırık Taş Tablet (Slay the Spire Tarzı Node Graph)

Kırık Taş Tablet map arayüzü — oda node'ları + bağlantı çizgileri + ilerleyiş. Karar #150 uyarınca. PixelLab gerektirmez — Unity UGUI + primitive sprites.

## NLM Context Çek

Başlamadan önce:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "MapPanel UI Kirik Tas Tablet tasarımı, node graph yapısı, Karar 150 detayları, mevcut UI script dosyaları"
```

## Mimari

### Data
- `MapNodeData` — node türü (combat, elite, rest, boss, event), position, connections[], isVisited, isCurrentRoom
- `MapGraphData` — ScriptableObject veya runtime class, MapNodeData[] nodes

### Runtime UI (Unity UGUI)
- `MapPanelUI.cs` — Canvas/Panel controller, Show(MapGraphData) / Hide()
- `MapNodeUI.cs` — tek node görselI (Image + Text); renk = node türüne göre (combat=red, rest=green, boss=purple vb.)
- `MapConnectionUI.cs` — iki node arası çizgi (LineRenderer veya UI Image rotated)

### File Paths (CREATE)
- `Assets/Scripts/UI/Map/MapNodeData.cs`
- `Assets/Scripts/UI/Map/MapGraphData.cs`
- `Assets/Scripts/UI/Map/MapPanelUI.cs`
- `Assets/Scripts/UI/Map/MapNodeUI.cs`
- `Assets/Scripts/UI/Map/MapConnectionUI.cs`
- `Assets/Prefabs/UI/MapPanel.prefab` — Canvas + sample 5-node graph (placeholder)

## Başarı Kriterleri
- [ ] MapPanelUI.Show(graph) çağrılabilir
- [ ] 5-node placeholder prefab Inspector'da görünür
- [ ] Compile errors yok
- [ ] Console 0 error
