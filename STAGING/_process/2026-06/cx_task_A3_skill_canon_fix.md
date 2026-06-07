ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
A3 — Skill canon drift fix'i (STATICAUDIT_DECISION_2026-06-07 adım 2/4): Ranger'ın canonical skill'leri yanlışlıkla retire-listesinde, offer havuzuna giremiyor (demo'da Ranger oynanabilir sınıf = GERÇEK oynanış bug'ı). + revoked Shadowblade isimleri UI'da görünüyor.

# Kanıt (önceki audit'ten — doğrula, sonra fix)
- `SkillDatabase.IsRetiredOfferSkill` (Assets/Scripts/Skills/SkillDatabase.cs:595-607): Backstab, Shadow Step, Fan of Knives, Aimed Shot, Disengage, Multi Shot HEPSİ retire.
- NLM canon: Backstab/Shadow Step/Fan of Knives = REVOKED eski Shadowblade isimleri (retire DOĞRU) · **Aimed Shot/Disengage/Multi-Shot = Ranger'ın GÜNCEL canonical skill'leri (retire YANLIŞ)**.
- CharacterSelectScreen.cs:1559-1582 fallback'i revoked Shadowblade isimlerini gösteriyor.

# İşler
1. **NLM çapraz teyit:** "Ranger ve Shadowblade sınıflarının güncel canonical skill listeleri nedir?" sorgusu — Aimed Shot/Disengage/Multi Shot Ranger canon'unda mı KESİNLEŞTİR; Shadowblade'in güncel canonical skill adlarını al.
2. **Retire listesi fix'i:** Ranger'ın canonical'larını `IsRetiredOfferSkill`'den ÇIKAR (offer'a dönerler — isImplemented=true zaten). Shadowblade revoked'ları retire KALIR.
3. **Shadowblade eşleme kararı:** DB'de implemented Backstab/ShadowStep/FanOfKnives class'ları var ama isimleri revoked. NLM'deki güncel Shadowblade canonical adlarıyla 1:1 davranış eşleşmesi varsa → `skillName` string'lerini canonical ada RENAME et (class adı kalabilir; SaveData/PlayerPrefs'te skillName persist ediliyorsa migration notu düş). Eşleşme belirsizse → RENAME YAPMA, BLOCKED bölümünde tablo halinde raporla (kullanıcı karar verecek).
4. **CharacterSelectScreen fallback temizliği:** 1559-1582 civarı revoked isimler → güncel canonical isimler (3. işin sonucuna göre).
5. **ShadowStep yazım kayması:** ShadowStep.cs:26 skillName="Shadowstep" vs DB "Shadow Step" — tekille (DB ve class aynı string).
6. **Testler:** (a) `RangerCanonicalSkillsAppearInOffers` — Ranger offer havuzunda Aimed Shot/Disengage/Multi Shot bulunur; (b) `RetiredSkillsNeverOffered` — revoked Shadowblade adları hiçbir sınıf offer'ında çıkmaz; (c) mevcut skill testleri yeşil kalır. EditMode'da koştur.
7. Compile temiz + ilgili test grubu sonucunu raporla. COMMIT YAPMA (orchestrator commit'ler).

# Çıktı
CODEX_DONE'a: iş 1-7 DONE/BLOCKED + NLM canon tabloları + değişen dosya:satır listesi + test sonuçları. Shadowblade eşlemesi belirsizse karar tablosu.
