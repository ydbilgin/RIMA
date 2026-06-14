ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
graphify 2x2 deney sonuclarini FIZIBILITE/MALIYET/REUSE lensinden degerlendir; en uygun config'i oner.

## Oku (inline etme, dosyayi ac)
STAGING/_process/2026-06/graphify_config_council_brief.md  (tum metrikler, kalite analizi, 4 soru orada)
Istersen ham JSON ciktilar: STAGING/_process/2026-06/graphify_exp/{normal_sonnet,normal_opus,deep_sonnet,deep_opus}.json

## Lens: feasibility / cost / reproducibility / reuse
ANALYSIS ONLY, kod degisikligi YOK. Sonucu CODEX_DONE.md'ye yaz. Onceki bir audit'i tekrar URETME.

Cevapla:
1. Deney tasarimi saglam mi? AST katmanini deneyden cikarmak (4 config'de sabit oldugu icin) DOGRU bir izolasyon mu, yoksa AST'siz "reward->draft yakalandi" sonucu yaniltici mi?
2. graphify'in pratik degeri = AST'nin verdigi yapisal harita mi, yoksa LLM semantic kenarlar mi? Eger AST cogu yapisal kenari zaten bedava veriyorsa, mimari-harita amaci icin normal+sonnet GERCEKTEN yeterli mi?
3. Maliyet muhakemesi dogru mu: token ~esit (~50-65k) ama Opus ~5x fiyat -> normal+sonnet en ucuz reliable secim mi? deep+opus'un 5x'i sadece kucuk-korpus tasarim grafiginde mi haklı?
4. deep+sonnet'i elemek (dis-sembol-duplikasyonu gurultusu) teknik olarak hakli mi?
5. NET ONERI: tek config mi, amaca-gore ikili mi? Demo'ya 6 gun kala tum-codebase graphify run'i simdi mi post-demo mu?

Kisa, gerekce-li, madde madde.
