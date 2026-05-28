ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: S99 Cleanup Batch A — SortingLayer Fix + Dead Scripts + Scene Archive

S99 birikmiş technical debt temizliği. Üç hedef: sortingLayer düzelt, dead scripts sil, placeholder sahneyi archive'a taşı.

## Detay Okuma

`STAGING/s99_sonnet_cleanup_analysis.md` dosyasını oku — silme listesi orada. Eğer dosya yoksa aşağıdaki genel kuralları uygula.

## Görevler

### 1. SortingLayer Fix
- `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/` altındaki TÜM prefab'ları tara
- SortingLayer="Default" olanları → SortingLayer="Walls" yap
- Özellikle: *_V2.prefab, *_placeholder.prefab olanları kontrol et

### 2. Dead Scripts Temizliği
- `Assets/Editor/` altındaki silinmiş/kullanılmayan painter scriptlerini kontrol et:
  - `Act1RoomPainter.cs` (git status'a göre deleted)
  - `Act1RoomPainterEnhanced.cs` (deleted)
  - `DevTools/DemoRoomPainter.cs` (deleted)
  - `DevTools/PilotRoomPainter.cs` (deleted)
  - `DevTools/SceneFloorPainter.cs` (deleted)
  - Bu .meta dosyaları da silinmiş; AssetDatabase.Refresh() yap
- `s99_sonnet_cleanup_analysis.md` listeliyorsa ek dead scripts de sil

### 3. Scene Archive
- `Assets/Scenes/Demo/PlaceholderRoomTest.unity` varsa → `Assets/Scenes/_Archive/PlaceholderRoomTest_s99.unity` olarak taşı
- .meta dosyasını da taşı
- Diğer artık kullanılmayan demo sahneler varsa (s99_sonnet_cleanup_analysis.md'den) de archive'a taşı

## Başarı Kriterleri

- [ ] Tüm wall prefab'larda SortingLayer="Walls"
- [ ] Dead script .meta artıkları temizlenmiş
- [ ] AssetDatabase.Refresh() çalıştırılmış
- [ ] PlaceholderRoomTest archived
- [ ] Console'da hata yok
