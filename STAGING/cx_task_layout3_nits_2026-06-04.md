ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ax kod-review'un bulduğu 3 küçük nit'i düzelt (defansif null-guard + fade-flash polish). KOD-ONLY, sadece CharacterSelectScreen.cs.

# Dosya
Assets/Scripts/UI/CharacterSelectScreen.cs

# Fix'ler (surgical)
1. ~satır 1177 (MakeText truncation): `text` null olabilir → `int maxLen = Mathf.Min((text ?? "").Length, 14);` ve `(text ?? "").Substring(...)` kullan (null-safe).
2. ~satır 825 (BuildSkillRow skill adı): `skill.skillName` null olabilir → `skill.skillName?.ToUpperInvariant() ?? "UNKNOWN"` (ya da projede ToUpper() kullanılıyorsa `(skill.skillName ?? "UNKNOWN").ToUpper()`).
3. ~satır 753 (FadeIdentityPopup): alpha 0'a snap edip 1'e fade ediyor → hızlı sınıf-değişiminde flash. Fix: mevcut alpha'dan lerp et: `float startAlpha = identityPanelGroup.alpha;` cache'le, `startAlpha`→1f lerp. (0'a snap etme.)
- Satır numaraları yaklaşık; ilgili metotları bul. Başka şeye dokunma.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode kısa probe: SelectClass(Warblade)→identity dolu (flash-snap yok), skill satırları çiziliyor, NRE yok.
- Değişen satırları + compile sonucunu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
