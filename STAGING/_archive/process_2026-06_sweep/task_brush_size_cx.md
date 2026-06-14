# TASK: Map Designer'a brush BOYUTU (1/3/5/10) ekle (cx)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece 2 dosya, listelenen değişiklik (4) BLOCKED yaz belirsizse.

NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read: Assets/Scripts kod.

Amaç: Kullanıcı RIMA Map Designer'da floor'u tek-tek hücre yerine **1 / 3 / 5 / 10 tile'lık fırça** ile boyamak istiyor. Şu an brush boyutu YOK (tek hücre). Brush-size ekle. Sen yazarsın (writer); Opus + Unity compile review eder (writer≠reviewer).

## FIX NOKTALARI (Explore haritaladı — kesin)

### 1. `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs`
- Yeni property ekle (diğer property'lerin yanına, ~satır 25): `public int BrushSize { get; set; } = 1;`
- `public void Paint(Vector3Int cell, Vector3 worldPos)` (~:45-62) — tek hücre yerine NxN loop:
  ```csharp
  int min = -(BrushSize - 1) / 2;
  int max = BrushSize / 2;
  for (int dx = min; dx <= max; dx++)
    for (int dy = min; dy <= max; dy++) {
        Vector3Int t = cell + new Vector3Int(dx, dy, 0);
        // mevcut tek-hücre Resolve + PutCategory mantığını t için uygula
        // (UnifiedPaintVariantResolver.Resolve her hücrede çağrılsın → deterministik spatial-hash varyasyon korunur)
    }
  ```
  Mevcut BeforeMutate/AfterMutate event'leri loop DIŞINDA bir kez çağrılsın (tek undo step).
- `public void Erase(Vector3Int cell)` (~:64-71) — aynı NxN loop ile her hücreyi sil.

### 2. `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`
- `DrawCategory(...)` (~:200-214), Paint/Erase toggle'ın ALTINA bir brush-size BUTON satırı (kullanıcı net "1li 3lü 5li 10lu" istedi → discrete butonlar, slider DEĞİL):
  ```csharp
  EditorGUILayout.BeginHorizontal();
  EditorGUILayout.LabelField("Brush:", GUILayout.Width(45));
  foreach (int sz in new[]{1,3,5,10}) {
      bool on = _core.BrushSize == sz;
      if (GUILayout.Toggle(on, sz.ToString(), EditorStyles.miniButton) && !on) _core.BrushSize = sz;
  }
  EditorGUILayout.EndHorizontal();
  ```
- `DrawStatusBar()` (~:460) brush gösterimine " size:" + _core.BrushSize ekle.

## SUCCESS
- Unity compile 0 hata (read_console veya dotnet build ile doğrula).
- Paint/Erase BrushSize'a göre NxN alanı boyar/siler; BrushSize=1 eski davranış.
- Variant resolver her hücrede çağrılır (granit grubu 4 varyantı NxN'de de karışık görünür).
- Undo tek step (BeforeMutate/AfterMutate loop dışında).

ASCII, Türkçe notlar. Sadece bu 2 dosya.
