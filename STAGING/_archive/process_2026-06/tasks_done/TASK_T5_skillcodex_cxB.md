ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ESC SkillCodexUI MVP: oyunu duraklatıp seçili sınıfın (ve istenirse tüm 10 sınıfın) skill'lerini codex tarzı tam-ekran listeleyen ekran. (SODAMAN backlog item 3.)

# Spec
READ: `STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md` §6 item 3 + inventory `CODEX_DONE_yasinderyabilgin.md` §5 (file:line) — ama şu GÜNCELLEMEYLE: SkillDatabase artık 10 sınıfın TAMAMINI içeriyor (bu session commit `f31469b4`): placeholder skill'lerde `isImplemented=false` flag'i var; codex listelemesinde placeholder'lar görünür ama "(yakında)" gibi muted işaretle ayrılmalı.

- Yeni `Assets/Scripts/UI/SkillCodexUI.cs`, runtime-built fullscreen Canvas overlay (sahne düzenleme YOK).
- ESC ile aç/kapa; `UIManager` pause-layer'ına ENTEGRE: timeScale'i KENDİN YAZMA — UIManager'ın mevcut pause sahipliğini kullan (UIManager.cs:223-232 timeScale owner; :119-132 ESC/offer state blokları). SettingsMenu ESC'iyle ÇAKIŞMA: mevcut ESC handler'ın öncelik zincirine ekle (settings açıksa codex açılmasın vb.).
- İçerik: üstte sınıf seçici (10 sınıf, default=aktif sınıf `PlayerClassManager.SelectedClass`), altında skill listesi — `SkillDatabase.GetAll()` filtreli. Satır görseli: CharacterSelectScreen skill-row pattern'ini reuse et (:1096-1113). isImplemented=false → karanlık/muted + isim yalnız.
- Stil: opak kutu YOK (UI canon ink-on-paper) — koyu translucent panel + cyan hairline. Blur/desat YOK (v1, council kararı).
- TooltipSystem entegrasyonu OPSİYONEL (varsa ucuz; yoksa atla).

# Kısıtlar
- Sadece: yeni SkillCodexUI.cs + UIManager.cs (dar ekleme) + gerekirse HUDController'a bootstrap. Başka dosya yok.
- Verify (UnityMCP): play → ESC aç → liste doğru (aktif sınıf + sınıf değiştirme + placeholder muted) → ESC kapa → oyun devam (timeScale restore) → draft/settings ESC'leriyle çakışma yok. Console temiz.
- Commit when verified (English, ydbilgin identity, no Claude trailer).

Write result to CODEX_DONE.md.
