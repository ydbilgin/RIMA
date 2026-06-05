# TASK: Map Designer "Rooms" sekmesi — RoomTemplateSO front-door (Faz-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
Council kararı `STAGING/MAPDESIGNER_DECISION_2026-06-06.md` (OKU) Faz-1'ini implement et. Kullanıcı yarın sunumda `RIMA/Map Designer` ile canlı oda gösterimi yapacak. Unity Editor AÇIK (UnityMCP). Senin envanterin `CODEX_DONE_yasinderyabilgin.md` sonunda (ROI tablosu + risk notları) — kendi bulgularını uygula.

## Yapılacaklar (karar dokümanındaki FAZ-1 maddeleri birebir)
1. `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`'e yeni **"Rooms" sekmesi** — İLK ve DEFAULT sekme:
   - `AssetDatabase.FindAssets("t:RoomTemplateSO", Assets/Data/Rooms)` listesi; arama kutusu + klasör grupları; tıklayınca seçili-template state.
   - Seçili template için **2D şematik önizleme**: EditorGUI.DrawRect grid — walkable=koyu, hole/void=siyah, kapı soketleri=cyan, playerSpawn=yeşil nokta. Basit, hızlı, her OnGUI'de yeniden çizilebilir (cache şart değil).
   - Renkli butonlar (GUI.backgroundColor): **Build in Arena** (yeşil) · **Auto Props** (mavi) · **Save Assets** (turuncu) + üstte dirty-yıldızı (`* Unsaved Changes`, EditorUtility.IsDirty kontrolü).
2. **Build in Arena:** `Assets/Editor/Rooms/RoomTemplateBrowserWindow.cs`'deki BuildInArena mantığını ortak static helper'a çıkar (örn. `RoomTemplateBuildUtility`), iki pencere de onu kullansın. Play-mode guard + sahne-değiştirme onayı korunur.
3. **Auto Props:** seçili template için `PopulateGeneratedPropsMenu.cs` mantığını reuse (gerekirse servis metoda çıkar): seed int alanı + 🎲 Randomize butonu + onay dialogu ("X prop silinip yeniden üretilecek") + `Undo.RecordObject(template, ...)` + `EditorUtility.SetDirty` + sonrasında otomatik rebuild (sahnede kuruluysa). Ada-dışı prop üretmediğini placer'ın mevcut walkable doğrulaması garanti ediyor — değiştirme.
4. **Smoke test:** editör testi VEYA menü aksiyonu (`RIMA/Rooms/QC/Smoke Test All Templates` benzeri — zaten RoomQCFixMenu var, oraya ekleyebilirsin): 26 template döngüsü → IsoRoomBuilder.Build + prop-walkable doğrulaması → 0 exception raporu. Çalıştır, çıktıyı kanıt olarak ver.

## YASAKLAR (kırılma riskleri — envanterindeki notlar)
- UnifiedDesignerCore / RoomData yolu / F2 overlay davranışına DOKUNMA (diğer sekmeler aynen çalışmaya devam etmeli).
- UnifiedDesignerTests kırılmamalı (EditMode'da koştur, kanıtla).
- Legacy RoomTemplateLoader KULLANMA (prefab-ref ister).
- walkableGrid'e YAZMA yok bu fazda (sadece okuma — şematik önizleme).
- Sahne SAVE yok. Canlı SO→auto-rebuild bağı YOK (manuel buton).

## Doğrulama (MANDATORY, UnityMCP)
1. Compile 0 error + UnifiedDesignerTests yeşil (+ smoke test çıktısı).
2. Pencereyi aç (execute_menu_item "RIMA/Map Designer") → exception yok; Rooms sekmesi default geliyor; liste 26+ template.
3. Bir template'i Build in Arena ile kur (children dolu), Auto Props'u farklı 2 seed'le çalıştır (prop listesi değişiyor + propOutsideFloor=0), Undo'nun çalıştığını doğrula (Undo.PerformUndo sonrası prop sayısı eski haline döner), Save Assets sonrası dirty temiz.
4. Editörü MainMenu sahnesinde bırak, konsol temiz.

## Commit
Değişen dosyalar. `feat(editor): Map Designer Rooms tab - RoomTemplateSO browser + build + auto-props (phase 1)`. Identity ydbilgin, NO Co-Authored-By.

## Output
CODEX_DONE: dosya listesi + doğrulama kanıtları (test sonuçları, seed önce/sonra prop sayıları, undo kanıtı) + commit hash + Faz-2'ye bıraktıkların.
