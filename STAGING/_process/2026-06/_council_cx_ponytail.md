ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ponytail (AI-agent kod-minimalizm plugin'i) RIMA'da kullanilmali mi? FIZIBILITE/ENTEGRASYON/REUSE lensinden degerlendir.

## Oku (inline etme)
STAGING/_process/2026-06/ponytail_opus_review.md  (Opus subagent'in tam analizi — ne oldugu, ayak izi, overlap, C# transferi, verdict orada)
Gerekirse repo: https://github.com/DietrichGebert/ponytail

## Lens: feasibility / integration / reuse. ANALYSIS ONLY. Sonucu CODEX_DONE.md'ye yaz.
1. ponytail'in hook mekanizmasi (hooks.json: SessionStart + her-prompt UserPromptSubmit + 3 skill) RIMA'nin mevcut Claude Code + cx-dispatch kurulumuna TEMIZ entegre olur mu? Cakisma/teardown riski?
2. "cross-agent Codex consistency" vaadi RIMA'nin cx wrapper'iyla BEDAVA gelir mi, yoksa ekstra entegrasyon isi mi (cx ayri profil/dispatch sistemi)?
3. PROJECT_RULES Karpathy-4 ile ~%80 overlap. Tam plugin reuse degeri var mi, yoksa sadece /ponytail-review pattern'ini elle checklist olarak almak yeterli mi?
4. C#/Unity'de pratik fayda OLCULEBILIR mi (benchmark'lar JS/Python; "mevcut C# sisteminde cerrahi edit" senaryosuna transfer var mi)?
5. NET: adopt-now / adopt-post-demo / skip + somut reuse onerisi (neyi nasil alalim).

Kisa, gerekce-li, madde madde.
