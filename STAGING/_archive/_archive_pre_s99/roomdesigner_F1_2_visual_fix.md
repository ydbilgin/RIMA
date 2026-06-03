# Codex Task — Room Designer F1.2 Visual Render Fix

**Status:** READY FOR DISPATCH (next session)
**Branch:** master
**Estimated:** ~150-300 LOC + Unity layer setup automation
**Allowed paths:**
- `Assets/Editor/RoomDesigner/Canvas/RoomDesignerCanvas.cs`
- `Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss`
- `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` (sadece OnEnable'a layer setup)
- `Assets/Tests/EditMode/Editor/**` (regression test eklenecek)

**LOCKED — DOKUNMA:** `IRoomDesignerContext` API yüzeyi.

## Bug Tanımı (Sonnet 2026-05-10 MCP-driven QA loop'tan)

Room Designer açıldığında **canvas görsel olarak bozuk**:
1. **Büyük siyah dikdörtgen** canvas'ın ortasında — grid çizilmemiş alan
2. **2 kırmızı yatay bar** Tile Library altında (No tiles state'inde olmamalı)
3. **"MAP" cyan text** canvas üst sağda
4. Bu üçü Room Designer ENİNDE — başka window bleed-through DEĞİL (RD Monitor 4'te x=700, hiçbir window altında değil — Sonnet doğruladı)

## Root Cause Hipotezleri (Sonnet'in araştırdığı)

### Hipotez 1 — `Handles.DrawCamera` foreign content leak
- `previewCam.cullingMask = 0` (F1.1 fix sonrası, "RoomDesigner" layer yoksa)
- Buna rağmen DrawCamera bir şeyler render ediyor olabilir
- **Test:** DrawCamera satırını yorum dışı bırakıp DrawRect-only çalıştır → artefactlar kalıyor mu?
- Eğer kalıyorsa: artefact DrawCamera DEĞİL, IMGUI veya başka yerden geliyor

### Hipotez 2 — "RoomDesigner" Unity layer mevcut DEĞİL
- F1.1 YÜKSEK 6 fix layer yoksa `cullingMask = 0` set ediyor (ÇALIŞIYOR per kod)
- Ama `stageLayer = 0` (Default layer) — Floor/Walls/Decals tilemap'leri Default layer'a düşüyor
- Eğer scene'deki BAŞKA Default layer object'leri varsa, normalde cullingMask=0 onları engellemeli ama bug olabilir
- **Çözüm önerisi:** Layer'ı OnEnable'da OTOMATİK oluştur (TagManager.asset edit), fallback'e güvenme

### Hipotez 3 — `Handles.DrawCamera` editor scene cameras'ı da composite ediyor
- Unity dahili: bazı versiyonlarda DrawCamera scene gizmos veya GameView camera output'unu composite edebilir
- **Çözüm:** RenderTexture pattern'e geç:
  ```csharp
  var rt = RenderTexture.GetTemporary((int)rect.width, (int)rect.height, 16);
  previewCam.targetTexture = rt;
  previewCam.Render();
  previewCam.targetTexture = null;
  GUI.DrawTexture(rect, rt);
  RenderTexture.ReleaseTemporary(rt);
  ```
- Bu approach explicit kontrol verir, Handles internal'larına bağlı kalmaz

### Hipotez 4 — UI Toolkit IMGUIContainer composite bug
- IMGUI içeriği VisualElement'in üzerine compositing yapılırken transparency artefactları
- **Çözüm:** `imguiContainer.style.backgroundColor = new StyleColor(new Color(0.06f, 0.07f, 0.08f, 1f))` — IMGUIContainer'a explicit bg ver

### Hipotez 5 — Tile Library kırmızı barlar = test asset thumbnail
- AssetDatabase.FindAssets("t:TileBase", new[] { "Assets/Art/Tiles" }) iki tile bulmuş olabilir
- Thumbnail rendering bozuk → kırmızı çıkıyor
- **Verify:** `AssetDatabase.FindAssets` çağrı sonucunu log'la, kaç tile bulunduğunu gör

## Hedef Fix Sırası (priority)

### Fix 1 — Otomatik "RoomDesigner" Unity Layer Setup
**Sorun:** Layer manuel kurulmayı bekliyor, kullanıcı kurmamış.
**Fix:** `RimaRoomDesignerWindow.OnEnable`'da ilk açılışta `TagManager.asset`'i edit ederek "RoomDesigner" layer'ı boş bir slot'a (genelde layer 30 veya 31) otomatik ekle. Mevcutsa atla.
**Kod örnek:**
```csharp
private static void EnsureRoomDesignerLayer()
{
    const string LayerName = "RoomDesigner";
    if (LayerMask.NameToLayer(LayerName) >= 0) return;
    var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
    var layers = tagManager.FindProperty("layers");
    for (int i = 8; i < 32; i++) // user layers start at 8
    {
        var slot = layers.GetArrayElementAtIndex(i);
        if (string.IsNullOrEmpty(slot.stringValue))
        {
            slot.stringValue = LayerName;
            tagManager.ApplyModifiedProperties();
            return;
        }
    }
    Debug.LogWarning("RoomDesigner: no free user layer slot to claim");
}
```

### Fix 2 — RenderTexture Pattern Yerine Handles.DrawCamera
**Test:** Yorum testi geçtikten sonra (artefactlar DrawCamera yüzünden ise) RenderTexture pattern'e geç. Layer artık doğru olduğu için preview camera sadece kendi stage'ini render eder.

### Fix 3 — IMGUIContainer Explicit Background
**Insurance:** `imguiContainer` constructor'ında `style.backgroundColor = new StyleColor(new Color(0.06f, 0.07f, 0.08f, 1f))`. Transparency hiç olmasın.

### Fix 4 — Tile Library Diagnostic
**Test:** `TileLibraryPanel.ReloadTiles`'a `Debug.Log($"Found {guids.Length} TileBase assets")` ekle. Eğer 2 tile buluyorsa, asset path'lerini log'la. Kırmızı barlar gerçek tile thumbnail'leri mi yoksa AssetPreview render bug'u mu görmek için.

### Fix 5 — "MAP" Cyan Text Kaynağı
Sonnet'in ne olduğunu bulamadığı element. Diagnostic için:
- `Handles.BeginGUI()` ile birlikte zaten bazı debug element çiziliyor mu Canvas'ta?
- Editor pixelRect ile rect karışıklığı olabilir mi?
- Sebep bulununca raporla.

## Acceptance Criteria

- [ ] Window aç → canvas TEK TİP dark grey grid, başka renk/text artefakt YOK
- [ ] Kırmızı bar YOK (Tile Library "No tiles" gösteriyor veya gerçek tile thumbnail'leri)
- [ ] "MAP" cyan text YOK
- [ ] Siyah dikdörtgen YOK (canvas baştan sona grid)
- [ ] "RoomDesigner" Unity layer otomatik oluşturuldu (manuel setup gerekmez)
- [ ] Window küçültülünce/büyütülünce canvas konsisten render — overlap/leak yok
- [ ] Mevcut 17/17 EditMode test PASS (regression yok)
- [ ] Yeni test: `RoomDesignerCanvasRendersIsolatedTests.cs` — preview camera başka scene object'lerini render etmiyor (RoomDesigner layer'ında olmayanlar görünmüyor)

## Kaynak

- Sonnet MCP screenshot analizi: kullanıcı oturumu 2026-05-10
- F1.1 fix commit: `21753d8`
- BucketFillBrush radius bound + Canvas DrawRect patch'i uncommitted state'te kalabilir → bu commit'e dahil et

## Notlar

- Sonnet'in Canvas patch'i (`previewCam.pixelRect` + `Handles.SetCamera` kaldırıldı + DrawRect eklendi) zaten in-file. Tutmaya değer iyi base, bu üzerine inşa et.
- `BucketFillBrush.cs` MaxRadius=50 patch'i uncommitted state'te. Bu commit ile birlikte commit et.
- Diagnostic: Hipotez 1 testi için DrawCamera satırını yorum içi alıp 30 saniye test et. Sonra geri al, gerçek fix'e geç.

## CODEX_DISPATCH Global Kurallar

- Model: gpt-5.5
- Yorum yazma — WHY açık değilse istisna
- Güven Döngüsü zorunlu — Hipotez teyit testleri için Unity MCP kullanılabilir mi? Eğer Codex Unity MCP'ye erişebilirse, kendi screenshot'unu alabilir
- Test PASS olmadan commit etme

## Commit Message

```
fix(room-designer): F1.2 canvas render isolation + auto layer setup

- RoomDesigner Unity layer otomatik kurulum (TagManager.asset edit)
- IMGUIContainer explicit background (transparency artefact önleme)
- Optional: RenderTexture pattern yerine Handles.DrawCamera (eğer diagnostic gerektirirse)
- TileLibraryPanel diagnostic log (tile asset discovery)
- BucketFillBrush MaxRadius=50 bound (Sonnet uncommitted patch dahil)
- Canvas DrawRect bg fill (Sonnet uncommitted patch dahil)
- Regression test: canvas render isolation
```
