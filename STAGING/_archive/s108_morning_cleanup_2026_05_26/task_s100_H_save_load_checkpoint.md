ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: Mid-Encounter Save/Load — Oda Geçişi Checkpoint Sistemi

SubRoomSequenceController oda geçişlerinde oyunu kaydetmek için checkpoint save. JSON tabanlı, PlayerPrefs değil. PixelLab gerektirmez.

## NLM Context Çek

Başlamadan önce:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "mid-encounter save load sistemi, SubRoomSequenceController dosya yolu, checkpoint tasarımı, mevcut player state verileri"
```

## Mimari

### Save Data Structs
- `CheckpointData` — serialize edilebilir class:
  - playerHealth, playerMaxHealth
  - currentRoomId, currentActId
  - inventory / equipped (NLM'den mevcut yapıyı öğren)
  - timestamp

### Core Scripts
- `CheckpointManager.cs` — singleton MonoBehaviour
  - `Save(CheckpointData data)` → JSON → `Application.persistentDataPath/checkpoint.json`
  - `Load()` → `CheckpointData` veya null
  - `HasCheckpoint()` → bool
  - `Clear()` — oda temizlenince checkpoint sil

### Integration Point
- SubRoomSequenceController'a `CheckpointManager.Save(...)` çağrısını hook olarak ekle (OnRoomTransitionStart event veya mevcut transition method)
- Mevcut dosya yolunu NLM'den öğren; minimal surgical change

### File Paths (CREATE)
- `Assets/Scripts/Save/CheckpointData.cs`
- `Assets/Scripts/Save/CheckpointManager.cs`

## Başarı Kriterleri
- [ ] `CheckpointManager.Instance.Save(data)` çalışıyor
- [ ] `CheckpointManager.Instance.Load()` null veya valid data dönüyor
- [ ] JSON `Application.persistentDataPath/checkpoint.json`'a yazılıyor
- [ ] Compile errors yok
