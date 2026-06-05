ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (KRİTİK BUG)
CharacterSelectScreen'in yeni 3-kolon runtime UI'ı GÖRÜNMÜYOR çünkü sahnedeki AUTHORED eski UI (CharacterSelectCanvas altındaki `CSS_Background` + `Root` = eski tek-panel "CLASS SELECT" + Warblade panel + GERİ/SEÇT) disable EDİLMİYOR ve runtime build'in üstünde/önünde render ediyor. Fix: authored UI'ı disable et + 3-kolon'u temiz bir runtime-root'a kur. KOD-ONLY (sahne .unity dosyasına dokunma — runtime'da disable et).

# Referans pattern (AYNISINI uygula)
`Assets/Scripts/UI/MainMenuController.cs` zaten doğru yapıyor:
- `DisableAuthoredRoot(canvasTransform)` → authored "Root"'u SetActive(false) yapıyor.
- `RemoveExistingRuntimeRoot` + `MakeRect("RuntimeRoot_MainMenu", canvas.transform, ...)` → runtime UI'ı ayrı bir full-stretch root'a kuruyor.
OKU ve aynı yaklaşımı CharacterSelectScreen'e taşı.

# Dosya (sadece bu)
Assets/Scripts/UI/CharacterSelectScreen.cs

# Yapılacaklar (BuildScreen)
1. targetCanvas resolve edildikten SONRA, yeni UI kurmadan ÖNCE: canvas altındaki TÜM mevcut authored çocukları disable et (özellikle `CSS_Background` ve `Root`). Güvenli yöntem: yeni runtime-root oluşturmadan önce `foreach (Transform ch in targetCanvas.transform) ch.gameObject.SetActive(false);` (authored'ları kapatır). 
2. 3-kolon UI'ı ayrı bir full-stretch **runtime root** altına kur: `MakeRect("RuntimeRoot_CharSelect", targetCanvas.transform, Vector2.zero, Vector2.one)` (offsetMin/Max=0). Tüm left/center/right panel'ler bu root'un altına parent'lansın (şu an doğrudan canvas.transform'a kuruluyorsa root'a çevir). Böylece authored disable + rebuild temiz/idempotent olur.
3. Idempotent: BuildScreen tekrar çağrılırsa önce eski "RuntimeRoot_CharSelect" varsa Destroy et (MainMenuController.RemoveExistingRuntimeRoot gibi).
4. CanvasScaler: targetCanvas scaler'ı zaten 1920×1080 (sahne) — ama garanti için MainMenuController.EnsureRuntimeScaler gibi koddan ScaleWithScreenSize/1920×1080/Match 0.5 zorla (varsa reuse et).
5. Mevcut tüm 3-kolon layout/logic (roster/showcase/skill panel/SEÇ/GERİ/EnsureSkillDatabase/RefreshSkillList) AYNEN korunsun — sadece authored-disable + runtime-root parent + scaler-ensure ekle.

# Doğrulama (ZORUNLU)
- Unity MCP: refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Mümkünse play-mode'da: GameObject.Find("Root") (authored) activeInHierarchy=false olmalı; "RuntimeRoot_CharSelect" var + altında 10 roster + 1 showcase + ScrollRect olmalı; authored "CLASS SELECT"/Warblade-panel görünür olmamalı.
- Değiştirilen metot/satırları + compile sonucunu profil-DONE dosyasına yaz. Sahne .unity DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
