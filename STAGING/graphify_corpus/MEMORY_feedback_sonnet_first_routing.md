---
name: sonnet-first-routing
description: "User pref: prioritize Sonnet sub-agents over Opus orchestrator for non-critical work. Opus only for cross-system judgment. Codex for mechanical batch."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

# Sonnet-First Routing Preference

**Date:** 2026-05-16 S86 (Opus limit ~2-3 days, after that Sonnet weekly reset abundant).

**Rule:** Default routing favors Sonnet sub-agents. Opus orchestrator does NOT bulk-execute work — it dispatches. Codex handles mechanical batch.

**Why:** User has high Sonnet weekly limit + finite Opus daily allocation. Wasting Opus on QC/doc/analysis depletes critical-decision budget. User explicitly asked: "sonnete de biraz iş ver sonnet ve codex arasında zeka farkı var mı? sonnet limitim çok fazla". Confirmed Sonnet's intelligence is sufficient for analysis/review/cross-ref work; Opus reserved for cross-system design judgment.

**How to apply:**

### Sonnet-preferred tasks
- `rima-doc` — CURRENT_STATUS sync, memory writes, guide writing
- `rima-qc` — Codex output review, sprite/asset QC PASS/FAIL
- `rima-sonnet` — analysis, cross-ref, planning, Codex prompt writing
- `rima-codex` — dispatch to Codex (cx_dispatch fallback when CONDA bug active)
- `rima-research` — Gemini external research
- `rima-asset` — PixelLab prompt writing, asset production planning
- `Explore` — codebase search

### Codex-preferred tasks
- Mechanical code batch (effort=high for spec-heavy, medium for routine)
- Unity MCP batch operations
- Test impl
- Sprite-batch import
- Refactor / rename

### Opus-only tasks (limit-protected)
- 2+ system kesen design karar (rima-design sub-agent)
- "Sonnet/Codex iki farklı yol önerdi, hangisi?" karar tiebreaker
- Sprint architecture design (Sprint 11, 12, vb. spec writing)
- Karar #N proposal that touches 3+ existing locked decisions

### Decision tree

```
Görev geldi:
├─ Spec açık + mekanik mi? → Codex (cx_dispatch / rima-codex)
├─ Analiz / planlama / cross-ref? → rima-sonnet
├─ Doc / memory yazımı? → rima-doc
├─ QC PASS/FAIL? → rima-qc
├─ Web/external research? → rima-research
├─ Codebase search? → Explore
└─ 2+ system kesen design karar? → Opus orchestrator veya rima-design
```

### Window awareness
- 16-18 May (Opus 2 günü kaldı): Opus implement override aktif Sprint 10 için
- 19 May sonrası (Sonnet only): tüm routing Sonnet/Codex'e döner
- Opus sadece rima-design üzerinden son çare olarak

### Notes
- Sonnet 4.6 zeka olarak: code review + cross-file düşünme + Türkçe nuance + uzun task tracking → Codex'ten daha iyi
- Codex GPT-5.5: spec uygulama + Unity-spesifik kalıplar + CLI tool use + mekanik hız → Sonnet'ten daha iyi
- Karşılıklı complement, ikisini de kullan

## Cross-links
[[codex-vs-opus-split]] [[codex-as-reviewer-until-may18]] [[research-on-block]]
