ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
UI redesign (4 ekran) için FEASIBILITY/REUSE lens: mevcut Pack asset'leri + RimaUITheme yeter mi, imagegen nerede ŞART, kod-kapsamı ne.

READ this file: STAGING/UI_REDESIGN_BRIEF_2026-06-04.md (tüm bağlam + proposed redesign + açık sorular orada).
Ayrıca gerekirse oku: Assets/Scripts/UI/RimaUITheme.cs, Assets/Scripts/UI/SkillBarUI.cs, Assets/Scripts/Core/DeathScreenManager.cs, Assets/Scripts/UI/CharacterSelectScreen.cs, Assets/Scripts/UI/MainMenuScreen.cs.

ANALYSIS ONLY — kod değişikliği YOK. Cevapla (feasibility/reuse lens):
1. Her ekran için: mevcut `Resources/UI/RIMA/Pack/` asset'leri + RimaUITheme helper'ları redesign'a YETER Mİ, yoksa hangi spesifik asset imagegen ile üretilmeli? (MainMenu backdrop, Death backdrop — mevcut main_menu_bg/death_screen_bg IMPORT'lu mu, kaliteli mi?)
2. CharSelect yan panel: sınıf kimliklerini (NLM Ek-A brief'te) nereye koymalı — `RimaUITheme.ClassTagline` genişlet mi, yeni `ClassIdentity(cls)` mi? Skill adlarını controller'lardan çekmek FEASIBLE mı yoksa over-scope mu?
3. SkillBar: class-accent glow + key-label okunurluk + cooldown — SkillBarUI.cs'de surgical mı yoksa büyük mü?
4. Build seed kaldırma (DeathScreenManager COPY BUILD SEED): başka yerde seed referansı var mı (temizlik kapsamı)?
5. En düşük-risk uygulama sırası ve her ekranın tahmini kod-kapsamı (S/M/L).

Sonucu CODEX_DONE.md'ye yaz. Önceki audit'i tekrarlama. KISA, uygulanabilir.
