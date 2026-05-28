# TASK: Painter'a "Generate Cliffs" Butonu Ekle

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Floor çiziminden Cliff regenerate'e UX köprüsü kur. Kullanıcı painter window'undan ayrılmadan tek tıkla CliffAutoPlacer.Regenerate() trigger edebilsin. Şu an Hierarchy → CliffRing → Inspector → CliffAutoPlacer → Regenerate adımı zorunlu, bu çok adım. Painter'a "Generate Cliffs" butonu eklenince akış: paint → click → done.

## Hedef dosyalar
1. `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` — top toolbar'a buton (her tab'tan erişilebilir)
2. `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` — tile painter v4 toolbar'ına da buton (foldout themes'in üstüne veya altına, en görünür yere)

## Paylaşılan helper
Tek bir helper class veya method oluştur — kod tekrarı yok:

**Önerilen yer:** `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs` (yeni dosya)

```csharp
namespace RIMA.Editor.MapDesigner
{
    internal static class CliffGenerateAction
    {
        public static void DrawButton(float height = 32f)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var placer = UnityEngine.Object.FindObjectOfType<RIMA.Environment.CliffAutoPlacer>();
                bool enabled = placer != null && placer.IsReady;

                using (new EditorGUI.DisabledScope(!enabled))
                {
                    var content = new GUIContent(
                        "🪨 Generate Cliffs",
                        enabled ? $"Auto-place {placer.CountPreviewPlacements()} cliffs based on current floor shape"
                                : "Add a CliffAutoPlacer with floorTilemap + rules to enable");
                    if (GUILayout.Button(content, GUILayout.Height(height)))
                    {
                        placer.Regenerate();
                        EditorUtility.SetDirty(placer.gameObject);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(placer.gameObject.scene);
                        Debug.Log($"[CliffGenerate] {placer.LastGeneratedCount} cliffs generated.");
                    }
                }

                if (!enabled)
                {
                    EditorGUILayout.LabelField("No CliffAutoPlacer in scene", EditorStyles.miniLabel, GUILayout.Width(180));
                }
            }
        }
    }
}
```

(asmdef uyumluluğunu kontrol et — Editor asmdef'inde RIMA.Runtime'a reference olmalı CliffAutoPlacer'ı görebilmek için. Zaten varsa sorun yok.)

## Adım 1: Helper dosyayı oluştur
`Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs` — yukarıdaki kod (using'ler tamamla)

## Adım 2: UnifiedMapDesigner.cs'e ekle
- Top toolbar (en üstte, tab seçicinin üstünde veya yanında) `CliffGenerateAction.DrawButton(28f)` çağır
- Surgical: sadece OnGUI veya equivalent çizim metoduna 2-3 satır ekle, başka kısma dokunma

## Adım 3: MinimalTilePainter.cs'e ekle
- Side panel'in en üstüne (Active Selection Card'ın hemen ÜSTÜNE) `CliffGenerateAction.DrawButton(32f)` çağır
- Veya en altına — kullanıcı paint → en alttaki butona basar mantığı da iyi
- Tercih: en üstte daha görünür, başlangıçta orayı dene
- Surgical: ilgili OnGUI metoduna 1-2 satır ekle

## Adım 4: Compile + verify
- `read_console` — 0 error olmalı (asmdef issue olursa BLOCKED)
- Hata yoksa Unity'de pencereyi aç (manual test gerekmez, sadece compile başarılı + buton görünür mü doğrula)

## Hard constraints
- Helper dosyayı küçük tut (~30 satır)
- Edit'ler surgical (toplam ~10 satır mevcut dosyalara eklenecek)
- Visual feedback: buton tooltip preview count gösterir (`CountPreviewPlacements()` zaten var, kullan)
- Disabled state: CliffAutoPlacer yoksa veya IsReady false ise buton sönük
- BLOCKED: asmdef reference yok, namespace ulaşılamaz, compile hatası fix edilemez
- Commit YAPMA

## Inline rapor (<400 kelime)
- Eklenen dosya path + satır sayısı
- Mevcut dosyalara yapılan değişiklik (satır numarası + ne yapıldı)
- Console error count (0 hedef)
- Buton position (yukarı/aşağı, tab toolbar/side panel)
- BLOCKED varsa neden
