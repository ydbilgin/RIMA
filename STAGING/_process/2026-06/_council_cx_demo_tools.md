ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Council danışmanlığı — FEASIBILITY / REUSE lensi: demo için bu gece eklenebilecek Director-tool/özellik önerilerinin "RIMA'da zaten ne kadarı hazır?" analizi. ANALYSIS ONLY, kod değişikliği YOK.

## Bağlam (OKU):
- STAGING/DEMO_SUNUM_PLANI_2026-06-13.md (run-of-show)
- CURRENT_STATUS.md (gece kuyruğu)
- Assets/Scripts/UI/DirectorMode.cs (yapıyı tara — hangi panel/sekme altyapısı var, yeni sekme/buton eklemek ne kadar ucuz?)
- STAGING/DEMO_POLISH_BACKLOG_2026-06-13.md

## Sorular:
1. DirectorMode'un mevcut UI/panel altyapısına yeni bir kontrol eklemenin GERÇEK maliyeti ne (kod yapısına bakarak: sekme sistemi var mı, buton/slider factory var mı)?
2. Şu adaylar için "zaten % kaç hazır" değerlendirmesi yap (reuse-vs-build): (a) düşman wave/preset spawn butonu (tek tıkla 5'li karışık grup), (b) slow-motion/timeScale slider (bullet-time gösterimi), (c) god-mode toggle (ölümsüz oyuncu — sunum güvenliği), (d) enemy HP bar toggle'ı, (e) kamera zoom slider'ı. Her biri: dokunulacak dosyalar + tahmini satır + kırılma riski.
3. Senin listende olmayan ama kod tabanında %80+ hazır olup 1 saatte bağlanabilecek başka bir "demo güçlendirici" var mı? (örn. mevcut ama bağlanmamış sistemler — prop validator, Loc, feel toggles...)
4. Bu adaylardan hangisi EditMode/smoke test güvencesine en kolay bağlanır?

## Çıktı:
Sonucu CODEX_DONE.md'ye yaz (profil dosyana). Madde madde, dosya+satır kanıtlı, Türkçe. Önceki audit'leri TEKRARLAMA.
