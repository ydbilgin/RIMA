# RIMA Room Designer — Teknik Review

### Soru 1: UI Toolkit + IMGUI Canvas Hybrid
**Cevap:** En uygun secenek A: `IMGUIContainer + Handles.DrawCamera + HandleUtility.GUIPointToWorldRay`. UI Toolkit shell, toolbar ve inspector icin kullanilir; canvas alaninda ise IMGUI event modeli ve `Handles.DrawCamera` editor icinde daha dogrudan ve dusuk gecikmeli calisir. RenderTexture yolu da calisir ama mouse mapping, DPI, repaint ve lifetime yonetimi daha fazla hata yuzeyi ekler. 60fps+ icin repaint'i sadece drag/zoom/pan veya preview dirty iken tetikleyin.
**Pattern/Code:**
```csharp
sealed class RoomDesignerWindow : EditorWindow
{
    Camera previewCam;
    Grid grid;
    IMGUIContainer canvas;

    void CreateGUI()
    {
        canvas = new IMGUIContainer(DrawCanvas) { focusable = true };
        canvas.style.flexGrow = 1;
        rootVisualElement.Add(canvas);
    }

    void DrawCanvas()
    {
        var r = GUILayoutUtility.GetRect(0, 100000, 0, 100000, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        Event e = Event.current;
        Handles.DrawCamera(r, previewCam);

        if (r.Contains(e.mousePosition))
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var plane = new Plane(Vector3.forward, Vector3.zero);
            if (plane.Raycast(ray, out float hit))
            {
                Vector3 world = ray.GetPoint(hit);
                Vector3Int cell = grid.WorldToCell(world);
                // IsometricZAsY: tilemap GridLayout handles transform; use cell as source of truth.
            }
            if (e.type is EventType.MouseDown or EventType.MouseDrag) e.Use();
        }
    }
}
```
**Risk:** `Handles.DrawCamera` kamera `pixelRect` ayarinda HiDPI farklarini etkileyebilir; `EditorGUIUtility.pixelsPerPoint` ile test edilmeli. `HandleUtility.GUIPointToWorldRay` mevcut kameraya bagli oldugu icin draw ve input ayni IMGUI pass icinde olmali.
**Kaynak:** https://docs.unity3d.com/ScriptReference/Handles.DrawCamera.html, https://docs.unity3d.com/ScriptReference/HandleUtility.GUIPointToWorldRay.html, https://docs.unity.cn/Manual/UIE-uxml-element-IMGUIContainer.html

### Soru 2: Tile Palette Asset Preview
**Cevap:** `AssetPreview.GetAssetPreview` lazy/asenkron calisir; ilk frame `null` normaldir. Guvenilir pattern: preview iste, `AssetPreview.IsLoadingAssetPreviews()` true iken `EditorApplication.update` uzerinden repaint et, hazir olunca cache'e al. `Editor.CreatePreview` genel asset grid icin daha agir ve her asset tipi icin standart degil; ozel `Editor.RenderStaticPreview` implementasyonu asset sahibindeyse faydali olabilir.
**Pattern/Code:**
```csharp
readonly Dictionary<int, Texture2D> cache = new();

Texture2D GetPreview(Object asset)
{
    int id = asset.GetInstanceID();
    if (cache.TryGetValue(id, out var tex)) return tex;
    tex = AssetPreview.GetAssetPreview(asset) ?? AssetPreview.GetMiniThumbnail(asset);
    if (tex != null) cache[id] = tex;
    return tex;
}

void OnEnable() => EditorApplication.update += TickPreviewLoading;
void OnDisable() => EditorApplication.update -= TickPreviewLoading;
void TickPreviewLoading()
{
    if (AssetPreview.IsLoadingAssetPreviews())
        rootVisualElement?.MarkDirtyRepaint();
}
```
**Risk:** 100+ tile icin her frame tum assetlere preview sormayin; virtualized `ListView`/visible range ve instanceID cache kullanin. Asset import veya domain reload sonrasi cache temizlenmeli.
**Kaynak:** https://docs.unity3d.com/ScriptReference/AssetPreview.GetAssetPreview.html, https://docs.unity3d.com/ScriptReference/AssetPreview.IsLoadingAssetPreviews.html, https://docs.unity3d.com/ScriptReference/EditorApplication-update.html

### Soru 3: Atomic Save — Prefab + ScriptableObject Pair
**Cevap:** `StartAssetEditing/StopAssetEditing` gercek transaction degil, sadece import batching'dir; cross-reference atomikligi manuel iki fazli save ile kurulmalidir. En temiz sira: once gecici/unique path'lerde SO ve prefab olustur, ikisini import ettikten sonra referanslari set et, dirty/save yap, sonra final path'e move et veya basarisizlikta olusan assetleri sil. `SaveAsPrefabAsset` batch icinde `null` dondurebilir, bu yuzden referans kurulumu icin import sonrasi load daha guvenlidir.
**Pattern/Code:**
```csharp
string soPath = "Assets/Rooms/tmp_room.asset";
string prefabPath = "Assets/Rooms/tmp_room.prefab";
var made = new List<string>();
try
{
    var bp = ScriptableObject.CreateInstance<RoomBlueprint>();
    AssetDatabase.CreateAsset(bp, soPath); made.Add(soPath);
    PrefabUtility.SaveAsPrefabAsset(rootGo, prefabPath, out bool ok);
    if (!ok) throw new Exception("Prefab save failed");
    made.Add(prefabPath);
    AssetDatabase.ImportAsset(soPath);
    AssetDatabase.ImportAsset(prefabPath);
    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    bp.prefab = prefab;
    prefab.GetComponent<RoomPrefabLink>().blueprint = bp;
    EditorUtility.SetDirty(bp);
    EditorUtility.SetDirty(prefab);
    AssetDatabase.SaveAssets();
}
catch { foreach (var p in made) AssetDatabase.DeleteAsset(p); throw; }
```
**Risk:** Tam rollback yoktur; disk yazimi ve import yarida kesilirse orphan asset kalabilir. Bunu manifest, temp path ve startup cleanup ile telafi edin.
**Kaynak:** https://docs.unity3d.com/ScriptReference/AssetDatabase.StartAssetEditing.html, https://docs.unity3d.com/ScriptReference/AssetDatabase.CreateAsset.html, https://docs.unity3d.com/ScriptReference/PrefabUtility.SaveAsPrefabAsset.html, https://docs.unity3d.com/ScriptReference/AssetDatabase.DeleteAsset.html

### Soru 4: MCP File Watcher Pattern
**Cevap:** Editor icinde en guvenli pattern polling'dir: `EditorApplication.update` ile `mcp_responses` klasorunu periyodik tara, main-thread disi event ve platform farklarini azalt. `FileSystemWatcher` hizli bildirim icin eklenebilir ama duplicate/missing event ve buffer overflow davranislari nedeniyle tek kaynak olmamali. `.tmp -> rename` dogru pattern; reader sadece `.json` gorsun ve yazim ayni klasorde bitsin.
**Pattern/Code:**
```csharp
double nextPoll;
void OnEnable() => EditorApplication.update += PollMcp;
void OnDisable() => EditorApplication.update -= PollMcp;
void PollMcp()
{
    if (EditorApplication.timeSinceStartup < nextPoll) return;
    nextPoll = EditorApplication.timeSinceStartup + 0.5;
    foreach (var path in Directory.EnumerateFiles(respDir, "*.json"))
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var json = new StreamReader(fs).ReadToEnd();
        // parse id/status, then archive or mark consumed
    }
}
```
**Risk:** Windows/macOS rename ayni volume ve ayni klasorde pratikte atomik kabul edilir; network drive, antivirus ve cloud sync bunu bozabilir. 500ms debounce yeterli baslangic degeri, fakat tamamlanma sinyali dosya adinin `.json` olarak rename edilmesi olmali; "Changed oldu, oku" yeterli degil.
**Kaynak:** https://docs.unity3d.com/ScriptReference/EditorApplication-update.html, https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-10.0

### Soru 5: Multi-Tilemap Undo/Redo
**Cevap:** Ayni event/current group icinde birden fazla `Undo.RegisterCompleteObjectUndo` tek undo step olarak gorunebilir; garanti icin group'u kendiniz yonetin. Brush stroke basinda `IncrementCurrentGroup`, `SetCurrentGroupName`, etkilenen tilemap'leri tek `RegisterCompleteObjectUndo(Object[])` veya pes pese kaydet, stroke sonunda `CollapseUndoOperations(group)` kullanin. Tilemap hucre yazimlari genellikle array benzeri toplu state degistirdigi icin `RegisterCompleteObjectUndo` daha guvenli; scalar serialized field icin `RecordObject` daha ucuzdur.
**Pattern/Code:**
```csharp
void ApplyStroke(Tilemap floor, Tilemap walls, Tilemap decals, IEnumerable<CellEdit> edits)
{
    Undo.IncrementCurrentGroup();
    int group = Undo.GetCurrentGroup();
    Undo.SetCurrentGroupName("Paint Room Tiles");
    Undo.RegisterCompleteObjectUndo(new Object[] { floor, walls, decals }, "Paint Room Tiles");
    foreach (var edit in edits)
        edit.Target.SetTile(edit.Cell, edit.Tile);
    floor.RefreshAllTiles(); walls.RefreshAllTiles(); decals.RefreshAllTiles();
    Undo.CollapseUndoOperations(group);
}
```
**Risk:** Undo kaydindan sonra stroke sirasinda yeni tilemap etkilenirse onu ayni group'a sonradan eklemek gerekir. Uzun drag'lerde her mouse move ayri group acmamali; mouse down/up stroke lifecycle kullanilmali.
**Kaynak:** https://docs.unity3d.com/ScriptReference/Undo.html

### Soru 6: AssetPostprocessor for AI Tile Output
**Cevap:** Import ayarlari `OnPreprocessTexture` icinde, piksel chromakey degisimi `OnPostprocessTexture(Texture2D)` icinde yapilmali. Preprocess'te `TextureImporter.isReadable = true`, alpha/sprite ayarlari ve compression kapatma; postprocess'te `(g > r+b) && (g > 100)` filtresi ile alpha'yi binary snap edin. Path filter `assetPath` uzerinden sadece `Assets/Art/Tiles/AI_Generated/` altina uygulanabilir.
**Pattern/Code:**
```csharp
class AiTilePostprocessor : AssetPostprocessor
{
    static bool IsAiTile(string p) => p.Replace('\\','/').StartsWith("Assets/Art/Tiles/AI_Generated/");
    void OnPreprocessTexture()
    {
        if (!IsAiTile(assetPath)) return;
        var ti = (TextureImporter)assetImporter;
        ti.isReadable = true;
        ti.alphaSource = TextureImporterAlphaSource.FromInput;
        ti.mipmapEnabled = false;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
    }
    void OnPostprocessTexture(Texture2D tex)
    {
        if (!IsAiTile(assetPath)) return;
        var px = tex.GetPixels32();
        for (int i = 0; i < px.Length; i++)
        {
            var c = px[i];
            c.a = (c.g > c.r + c.b && c.g > 100) ? (byte)0 : (byte)255;
            px[i] = c;
        }
        tex.SetPixels32(px);
        tex.Apply(false, false);
    }
}
```
**Risk:** Baska postprocessor ayni asset path'e dokunuyorsa execution order belirlenmeli veya mevcut postprocessor'a tek path-guard'li branch olarak entegre edilmeli. Postprocess'te importer setting degistirmek mevcut import'a etki etmeyebilir; bu yuzden ayarlar preprocess'te kalmali.
**Kaynak:** https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPostprocessTexture.html, https://docs.unity3d.com/ja/460/ScriptReference/AssetPostprocessor.OnPreprocessTexture.html, https://docs.unity.cn/ScriptReference/AssetPostprocessor-assetPath.html
