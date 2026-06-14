ACTIVE RULES: (1) think before answering (2) concrete, no speculation (3) flag disagreements explicitly (4) say UNSURE if you can't verify.

NLM ACCESS: If you need RIMA design context, query NLM via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

# GÖREV — REVIEW (kod + scope feasibility)
Opus bir stratejik sentez + temiz iş listesi yazdı: **`STAGING/STRATEGIC_SYNTHESIS_S6.md`**. Oku ve ELEŞTİREL review yap. Sen YAZMIYORSUN, sadece review + öneri (yeni routing: Opus yazar, Codex+agy review).

Değerlendir:
1. **Çelişki tablosu (Bölüm 2) doğru mu?** Özellikle KOD-seviyesi: C7 (SkillDraftSystem stub gerçekten ölü mü? `Assets/Scripts/Combat/Skills/SkillDraftSystem.cs` + `ActiveSkillData` kullanılıyor mu?), C9 (skill draft wiring gerçekten bozuk mu? `DraftManager`, `SkillOfferUI.cs:289`, `SkillDatabase.Add()` icon ataması). Grep ile DOĞRULA.
2. **İş listesi (Bölüm 6) feasibility:** T0.1 (skill draft fonksiyonel), T1.1 (tag synergy), T1.2 (Rift Shards ekonomi), T1.3 (corrupted elite) — her biri mevcut kod mimarisine OTURUYOR mu, yoksa büyük refactor mi gerektirir? Hangisi gizli-pahalı?
3. **Eksik çelişki / risk var mı?** Sentezin KAÇIRDIĞI teknik çelişki, dead-code, veya scope-trap.
4. **Öncelik doğru mu?** T0.1'i ilk yapmak mantıklı mı, yoksa başka bir bağımlılık önce mi gelmeli?

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED
- Her bölüm için AGREE / DISAGREE (+ gerekçe + file:line kanıt)
- Kaçırılan çelişki/risk listesi
- Gizli-pahalı item uyarıları
- Önceliklendirme önerisi (varsa değişiklik)
