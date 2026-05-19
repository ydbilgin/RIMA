---
name: orchestrator-delegation-strict
description: "S91 LOCK 2026-05-18: Orchestrator BULK iş yapmaz — büyük doc/memory write + analiz + batch sync HEPSİ sub-agent dispatch. User orchestrator ile sohbet etmek istiyor, beklerken arka planda paralel iş yürüsün."
metadata:
  node_type: memory
  type: feedback
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# Orchestrator Strict Delegation (S91 LOCK)

**Kural:** Orchestrator (Opus/Sonnet) **bulk iş YAPMAZ**. Sub-agent dispatch zorunlu. User orchestrator ile sohbet eder, sub-agent'lar paralel iş yürütür.

**Why:** User feedback 2026-05-18 S91 — "agentlara görev versene sen yapıyorsun. biz seninle konuşmaya devam edelim orchestrator olmayı unutma". Önceki turlar'da MAP_PLAN_v1.md draft + FINAL.md + memory yazımı tüm bulk orchestrator yaptı; user bunu istemiyor.

**How to apply:**

| İş tipi | Delegasyon |
|---|---|
| Büyük doc/spec write (>200 satır) | `rima-doc` agent (Sonnet) |
| Memory file write (dolu, structured) | `rima-doc` agent |
| Multi-file analiz / cross-ref | `rima-sonnet` agent |
| NLM query | `/nlm` skill (CLI direkt) veya `rima-research` |
| Codex prompt writing | `rima-sonnet` agent (sonra cx_dispatch.py) |
| Asset prompt writing | `rima-asset` agent |
| Codex review dispatch | `cx_dispatch.py` (Codex direkt) |
| QC review | `rima-qc` agent |
| Tek dosya read/edit (<5 satır) | Orchestrator direkt OK |
| Tek-line memory index update | Orchestrator direkt OK |
| Skill invoke (Skill tool) | Orchestrator direkt OK |
| AskUserQuestion | Orchestrator direkt OK |
| Quick UnityMCP probe (1-2 call) | Orchestrator direkt OK |

**Trigger:** Eğer yapacağın iş > 5 dakika sürer veya > 200 satır yazı doğurur → sub-agent dispatch.

**Sohbet akışı:**
- User soru sorar → orchestrator cevaplar (sohbet)
- İş gerekirse → orchestrator agent spawn (background or foreground)
- Agent çalışırken → orchestrator user'la konuşmaya devam
- Agent biter → orchestrator user'a özet + next step

**Related:** [[research-delegate-to-agents]] [[sonnet-first-routing]] [[codex-vs-opus-split]] [[codex-parallel-profile-workflow]]
