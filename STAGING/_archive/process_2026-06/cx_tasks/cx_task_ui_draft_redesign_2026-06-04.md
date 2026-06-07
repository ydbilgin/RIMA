ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Skill-draft kartlarının (1) hover JITTER'ını ve (2) "Seç" tıklamasının çalışmamasını TEK yapısal değişiklikle çöz, (3) kartları büyüt/ortala/okunaklı yap, (4) premium hover ekle, (5) draft'ın timeScale=0'da takılmasını engelle. Karar = `STAGING/UI_UX_REDESIGN_DECISION_2026-06-04.md` (Part B+C OKU). Council feasibility = `CODEX_DONE_laurethayday.md` (file:line'lar orada).

# Dosyalar (SADECE bunlar)
- Assets/Scripts/UI/SkillOfferUI.cs
- Assets/Scripts/Skills/DraftManager.cs

# Değişiklikler

## 1) Kart yapısı = sabit HITBOX + ölçeklenebilir VISUAL (jitter + "Seç" fix'i)
Mevcut: HoverScale kart ROOT'una uygulanıyor (`SkillOfferUI.cs:724,732`) → raycast-target kursör altından kayıyor → enter/exit flicker (jitter) VE tıklama iptal. Ayrıca çift handler (`:430` bg + `:496` btn).
YAP:
- `cardGo` (kart kökü) SABİT kalsın: tam `CardWidth×CardHeight`, üstüne transparent `Image` (alpha 0, `raycastTarget=true`) = HOVER HITBOX. Buna **TEK** `CardJuiceHandler` ekle. Diğer görsel Image'ların `raycastTarget=false` (Button kendi interactable Graphic'ini korur).
- Tüm görseller (glow, bg, icon, name, desc, chips, button) yeni child `VisualRoot` (RectTransform, stretch full) altına taşınsın.
- `TweenHover` artık `cardGo` yerine `VisualRoot.localScale`'i ölçeklesin. Çift handler'ı tek'e indir (bg+btn yerine sadece hitbox handler).
- Bring-to-front: hover başlarken `cardGo.transform.SetAsLastSibling()`, hover bitince orijinal sibling index'e geri koy. Per-card `Canvas`+`GraphicRaycaster`+`overrideSorting` churn'ünü KALDIR (`:382,:384,:708,:737`).

## 2) Premium hover
- VisualRoot: scale 1.05 + anchoredPosition.y +20 (hover). Hover edilmeyen iki kart: CanvasGroup alpha 0.4 + scale 0.95. Smooth tween (mevcut unscaled tween altyapısını kullan).
- Confirm efekti basit kalsın (seçilen flash + merkeze, diğerleri fade). 3D-tilt/particle EKLEME (over-engineering, defer).

## 3) Boyut/ortalama (constants — `SkillOfferUI.cs:35-46` ve font satırları)
- CardWidth 280, CardHeight 400, CardGap 36. icon 100 (`:441`), name 22 (`:464`), desc 14 (`:478`), tier chip 80×22 @10 (`:284`), chain chip @10 (`:356`), button 160×40 @15 (`:491,:504`), title 34 (`:183`), subtitle 15 (`:196`).
- Cards container TAM ORTADA: anchor (0.5,0.5), anchoredPosition (0,0) (`:204-209`). 3 kart yatay ortalı (`:373-377`).

## 4) "Seç" robustluk
- `DraftManager.ShowDraftDelayed` (`DraftManager.cs:99,101`) `WaitForSeconds`→`WaitForSecondsRealtime` (timeScale=0'da takılmasın). NOT: `ConfirmPickRoutine` zaten unscaled — DOKUNMA.

# Doğrulama / çıktı
- Compile-mantıklı yaz (UnityEngine/uGUI; dotnet build değil — Unity Test Runner/Editor ile orchestrator doğrulayacak). Hiyerarşi değişiminde mevcut referansları (onPick callback, DraftManager.OnOfferSelected) BOZMA.
- Sonucu CODEX_DONE.md'ye yaz: değişen dosyalar + yapı özeti + riskli noktalar. Sadece listelenen 2 dosyaya dokun.
