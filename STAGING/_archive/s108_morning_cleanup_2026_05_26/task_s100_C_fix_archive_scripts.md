ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Not needed for this task.

# Amaç: Task C Fix — _Archive_painter_alt içindeki dead scripts'i sil

Cleanup task `Assets/Editor/_Archive_painter_alt/` klasörüne 5 script taşıdı ama Unity hâlâ compile ediyor. Bu klasör `Assets/Editor/` içinde olduğu için .asmdef dışlaması yok. Dosyaları sil.

## Silinecek dosyalar (tümü)

- `Assets/Editor/_Archive_painter_alt/Act1RoomPainter.cs`
- `Assets/Editor/_Archive_painter_alt/Act1RoomPainter.cs.meta`
- `Assets/Editor/_Archive_painter_alt/Act1RoomPainterEnhanced.cs`
- `Assets/Editor/_Archive_painter_alt/Act1RoomPainterEnhanced.cs.meta`
- `Assets/Editor/_Archive_painter_alt/DemoRoomPainter.cs`
- `Assets/Editor/_Archive_painter_alt/DemoRoomPainter.cs.meta`
- `Assets/Editor/_Archive_painter_alt/PilotRoomPainter.cs`
- `Assets/Editor/_Archive_painter_alt/PilotRoomPainter.cs.meta`
- `Assets/Editor/_Archive_painter_alt/SceneFloorPainter.cs`
- `Assets/Editor/_Archive_painter_alt/SceneFloorPainter.cs.meta`
- `Assets/Editor/_Archive_painter_alt.meta` (klasör boşalınca)
- Klasör de sil: `Assets/Editor/_Archive_painter_alt/`

## Adımlar

1. Yukarıdaki dosyaları sil (shell: `rm` veya Unity AssetDatabase.DeleteAsset)
2. `Assets/Editor/_Archive_painter_alt/` klasörü boşsa onu da sil
3. AssetDatabase.Refresh() çalıştır
4. Unity console: 0 error kontrol et

## Başarı Kriterleri
- [ ] `Assets/Editor/_Archive_painter_alt/` YOK
- [ ] Compile errors yok
- [ ] Console 0 error
