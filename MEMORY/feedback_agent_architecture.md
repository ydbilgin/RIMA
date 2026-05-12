---
name: Agent Architecture — Router vs Reasoning
description: Sub-agent mimarisi 2-tier; rima-codex/rima-research router agents (sadece tool çağırır, düşünmez), rima-design/qc/doc/asset reasoning agents (Sonnet instance)
type: feedback
---

# Agent Architecture — Router vs Reasoning (S58'de finalize edildi)

İki tip sub-agent var, karıştırma:

## Router Agents (düşünmez, devreder)
- **rima-codex**: cx CLI'ı çağırır, çıktıyı döndürür. Sadece Bash tool. ~2-3k token baseline. Kendi analiz yapmaz.
- **rima-research**: gemini -p ile soruyu çalıştırır, ham çıktıyı döndürür. Kendi yorum yapmaz.

## Reasoning Agents (Sonnet instance)
- **rima-design**: Game design kararları, balance, system architecture
- **rima-doc**: Docs/memory güncelleme, CURRENT_STATUS sync
- **rima-qc**: Codex/asset/QC review, PASS/FAIL raporu
- **rima-asset**: PixelLab prompt yazımı, asset üretim planı

## Why
Router agents kendi düşünme yapmadığı için token-ucuz ve hızlı. Reasoning'in agent içine gömülmesi karışıklık yaratıyordu. Ayrım sonrası: Claude orchestrator akıl yürütür, gerekirse reasoning agent'a danışır, router'ları sadece tool wrapper olarak kullanır.

## How to apply
- Kullanıcı "sonnetle araştır" derse → Claude direkt araştırır, rima-research spawn etme
- Küçük UnityMCP işi (<5 tool call) → Claude direkt yapar, rima-codex spawn etme
- Büyük batch UnityMCP işi → CODEX_TASK.md yaz → rima-codex spawn et
- Non-UnityMCP iş (kod / araştırma / dosya işlemi) → /codex skill kullan (cx bash direkt)
