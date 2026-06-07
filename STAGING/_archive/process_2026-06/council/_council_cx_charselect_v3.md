ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Council advisor (FEASIBILITY/REUSE lens): CharacterSelect v3 konsept incelemesi + skill-Echo-unlock kararı için kod-tarafı gerçeklik raporu.

# Görev (ANALYSIS ONLY — kod değişikliği YOK)
READ FIRST: `STAGING/_council_brief_charselect_v3.md` (tüm bağlam, kullanıcı direktifleri, açık kararlar K1/K2/K3 orada).
Also read: `STAGING/mockups/charselect_chatgpt_brief_2026-06-05.md` (ChatGPT önerisi — benimseme, kendi fikrini sun).
Concept image (bakabiliyorsan): `STAGING/mockups/charselect_concept_ref_2026-06-05.png` — bakamıyorsan brief'teki metin tarifi yeterli.

Senin lensin = FEASIBILITY / WHAT-EXISTS / REUSE-vs-BUILD. Cevapla (file:line kanıtlı):

1. **K1 skill-Echo-unlock kod gerçekliği:** Mevcut unlock altyapısı ne durumda?
   - Karakter unlock: `CharacterSelectScreen` Echo harcama/unlock akışı (recent commit 567b8c75) — hangi sınıflar/alanlar? Persistence var mı (PlayerPrefs/save)?
   - Per-SKILL lock/unlock altyapısı VAR MI? (SkillDatabase, SkillDefinition/SO alanları, DraftManager pool filtreleme). "Mastery skill" kavramı kodda var mı?
   - Skill'ler Echo ile unlock'lanacaksa minimal yol ne? (örn. SkillDefinition'a `unlockCost` + unlocked-set persistence + DraftManager/SkillOfferUI pool filtresi). Kaç dosya/satır tahmini, riskler.
2. **Echo ekonomi kodu:** Echo nerede tutuluyor (meta-currency)? Run-sonu Echo kazanım akışı kodda var mı yok mu? `PlayerEconomy` (run-içi Gold) ile ilişkisi?
3. **K3 görsel feasibility:** Kullanıcı direktifleri (tek ekran no-scroll, tile-aligned karakterler, opak siyah kilitli silüet, ada-altı temiz) mevcut roster-room implementasyonunda ne durumda — hangileri ZATEN sağlanıyor, hangileri değişiklik ister? HTML-mockup→Unity translate edilirken en riskli kısım ne?
4. **ChatGPT önerisi AL/ATLA** (kod-feasibility açısından): hangi maddeler mevcut yapıyla bedava geliyor, hangileri pahalı?
5. Web araştırması yapabiliyorsan: 2-3 roguelite'ta meta-currency ile skill unlock precedent'i (Hades/Dead Cells/Rogue Legacy 2 vb.) — kısa; yapamıyorsan SKIP de (diğer advisorlar yapıyor).

Çıktı: CODEX_DONE.md'ye yaz. Do NOT reproduce any prior audit. Kısa, kanıtlı, madde madde.
