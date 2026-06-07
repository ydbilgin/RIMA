ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
2-karakter gameplay demosu (Elementalist + Warblade) için Elementalist'in BÜYÜ FIRLATMA / cast VFX'ini nasıl üreteceğimizin FEASIBILITY lensi + in-game skill ikonları için PixelLab-vs-imagegen kuralı. ANALİZ ONLY — kod/asset değiştirme. Sonucu profil-DONE dosyasına yaz (cx_dispatch zaten CODEX_DONE_<profil>.md'ye yazdırıyor).

# Sorular (concrete, cite real paths/methods)
1. MEVCUT SİSTEM: Elementalist skill'leri (fireball, glacial spike, meteor — SkillDatabase/SkillIconRegistry'de var) ŞU AN nasıl ateşleniyor? Bir projectile prefab/spawner sistemi var mı? Elementalist skill controller hangi dosyada, cast'te ne spawn ediyor (görsel var mı yok mu)? Warblade'in melee VFX'i (SlashArcVFX) nasıl çalışıyor — projectile'a örnek olur mu? Mevcut: SlashArcVFX, JuiceManager, herhangi bir ProjectileBehavior/spawner. CITE paths.
2. ÜRETİM YÖNTEMİ seçenekleri (projectile/cast VFX için): (A) PixelLab animated sprite-sheet (premium, GATED, credit) (B) imagegen frame-sheet → dilimle → SpriteRenderer animation (C) Unity code/particle VFX (ParticleSystem + mevcut cyan/orange VFX sprite'ları + shader/trail) (D) hibrit. Her birinin RIMA'daki feasibility'si + mevcut altyapıya uyumu. Sheet-anim için Unity import/animation precedent (AnimationImportHelper, mevcut character anim .anim/controller pattern).
3. SHEET mi: projectile animation sheet üretilecekse — uniform grid (örn 4-8 frame yatay), PPU64, transparent. Mevcut karakter anim pipeline'ı (Resources/Characters/<C>/<c> RuntimeAnimatorController) projectile'a uyarlanabilir mi yoksa ayrı basit frame-flip mi?
4. IN-GAME İKONLAR (skill bar + char-select'te kullanılan): bunlar PixelLab mı imagegen mi? Kural (NLM canon + memory feedback_imagegen_onbrand_not_realistic_s6): abstract skill glyph = imagegen OK mı, yoksa gameplay'de göründüğü için PixelLab mı gerekir? SkillIconRegistry mevcut ikonları nasıl üretilmiş (varsa not)?
5. EN AZ-FRICTION önerisi: demo için Elementalist 2-3 büyüsünü "hissettiren" minimum üretim yolu (reuse-first; PixelLab gen GATED olduğunu unutma).

Terse, cite paths/methods. cx_dispatch profil-bazlı DONE dosyasına yazacak.
