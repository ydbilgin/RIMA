---
name: nlm-first-context-strict
description: "S91 LOCK 2026-05-18: NLM-first for ALL context queries (lint, conflict check, summary). Sub-agents da NLM kullansın — dispatch prompt'una NLM CLI çağrısını ekle. Direct file read sadece allowed exceptions için."
metadata:
  node_type: memory
  type: feedback
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# NLM-First Context Strict (S91 LOCK)

**Kural:** Hem orchestrator HEM sub-agent'lar context query için **önce NLM**. Direct file read sadece allowed exceptions.

**Why:** User feedback 2026-05-18 — "hala dosya okuyorsun ama nlm ile tam bağlam isteyebilirsin ya da nlm ile olayı sorabilirsin o sana özet geçebilir. subagentlar da kullanabilirse onlar da kullanabilir." `/lint` çağrısında MASTER_KARAR + ANIMATION_BIBLE + FAZ_MASTER + GDD'yi direct okudum oysa tek NLM query ile özet alabilirdim.

PROJECT_RULES.md "NotebookLM HARD RULE" zaten yazılı ama uygulamada sıklıkla atlanıyor. Bu memory pekiştirme.

## Allowed direct file reads (NLM SKIP)

| Dosya | Sebep |
|---|---|
| `CURRENT_STATUS.md` | Session başı, NLM-cache gecikmeli |
| `.claude/PROJECT_RULES.md` | Session başı |
| `CODEX_DISPATCH.md`, `CODEX_TASK_*.md`, `CODEX_DONE*.md` | Operational state, NLM'de yok |
| `Assets/Scripts/**` | Code files, NLM'de yok |
| `STAGING/*` | Operational/draft, NLM'de yok |
| `~/.claude/projects/.../memory/*` | Auto-memory, NLM'den ayrı sistem |

## NLM-first MANDATORY for

| İhtiyaç | NLM query |
|---|---|
| Karar lookup | "Karar #X ne diyor, hangi karar bunu override etti?" |
| Drift/conflict check | "FAZ_MASTER, MASTER_KARAR, GDD'de X konusunda çelişki var mı?" |
| Spec özet | "RIMA dungeon map system tam spec özetle (oda tipleri, biome, sayılar)" |
| Lint/audit | "TASARIM/* dosyalarında STALE veya conflict var mı?" |
| Cross-ref | "X feature'ı Y dosyada nasıl tanımlanmış, Z dosyada nasıl çelişiyor?" |

## NLM CLI (orchestrator + sub-agent)

```bash
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "your question"
```

Notebook ID: `30ddffa5-292f-4248-8e77-68074af901be` (LIVE RIMA design knowledge base).
**YASAK ID:** `ed3c8952-417c-4988-84a7-425d25ba3b08` (eski, deprecated)
**Eski referans (PROJECT_RULES'da geçer):** `06a27df3-79e6-43da-a550-2937149af0a4` — kullanmadan önce status check.

## Sub-agent dispatch template — MANDATORY (S91 LOCK)

**Her sub-agent dispatch'inde ZORUNLU 2 satır enjeksiyon (eksik = orchestrator hatası):**

```
ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.
```

**User direktifi 2026-05-18 S91:** "agentlar da skilleri kullanabiliyor değil mi bu nlm olayını kesinlikle kendilerine iletmelisin agentlar oradan sorgu yapıp bağlamları almalı bunu hard memory'e kaydet"

→ Bu kural HARD RULE. PROJECT_RULES.md'de "NotebookLM HARD RULE > Sub-Agent NLM Access MANDATORY" section'ında da yazılı. Orchestrator dispatch öncesi prompt'u kontrol eder — yoksa dispatch'i durdurup ekle.

### Agent skill access durumu
- **Skills (e.g. /nlm, /lint, /graphify):** Sub-agents Skill tool'u DOĞRUDAN kullanamaz (skill = orchestrator-only)
- **NLM CLI (uvx):** Sub-agents Bash tool ile DOĞRUDAN çağırabilir (no skill needed)
- **Memory files:** Sub-agents Read tool ile direkt okuyabilir (allowed exception)
- **NLM via CLI = canonical sub-agent context path**

## When NLM cannot answer

Ardışık deneme:
1. NLM query → cevap yok / hatalı
2. `~/.claude/projects/.../memory/MEMORY.md` index → ilgili memory file → direkt oku
3. STAGING/ veya TASARIM/ → grep + Read (sadece ilgili satır aralığı)

## Related
- [[orchestrator-delegation-strict]]
- [[research-delegate-to-agents]]
