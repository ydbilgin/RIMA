---
name: CLAUDE.md Stub + PROJECT_RULES Pattern
description: CLAUDE.md 3 satır stub olarak tut, asıl kurallar .claude/PROJECT_RULES.md'de; sub-agent'lar stub alır (~30 token), orchestrator session başında PROJECT_RULES.md okur
type: feedback
---

# CLAUDE.md Stub Pattern (S58'de finalize)

## Yapı

**`CLAUDE.md` (3 satır, ~30 token):**
```
# RIMA
Session start: `.claude/PROJECT_RULES.md` + `CURRENT_STATUS.md` oku. Başka okuma.
Sub-agents: bu dosyayı yoksay, orchestrator bağlamı doğrudan verir.
```

**`.claude/PROJECT_RULES.md` (asıl kurallar):**
- HARD RULES (codex routing, asset üretim, hard yasaklar)
- Role: Orchestra Conductor tanımı
- Sub-agent listesi (router vs reasoning)
- NotebookLM rules
- NLM Sync policy
- Skills listesi
- Token saving, /clear policy, vs.

## Why

Önceki yapı: CLAUDE.md harness tarafından her sub-agent invocation'da otomatik yüklenirdi (~3-5k token per agent). 6 sub-agent × 5k = 30k token ziyan.

Yeni yapı: CLAUDE.md stub = ~30 token. Sub-agent'lar orchestrator'ın verdiği bağlamla çalışır. PROJECT_RULES.md sadece orchestrator (Claude) session başında okur (1 kez).

**Token tasarrufu: ~25k+ per session, paralel agent kullanımında daha fazla.**

## How to apply

- CLAUDE.md → her zaman stub form
- Asıl içerik `.claude/PROJECT_RULES.md`
- Sub-agent prompt'larına stub'tan bilgi çıkma — orchestrator açıkça verir
- Yeni proje kurarken aynı pattern uygula
