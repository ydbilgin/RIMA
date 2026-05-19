---
name: feedback-research-delegate-to-agents
description: "Orchestrator (Claude/Sonnet) araştırma + analiz + harici inceleme işlerini ASLA kendi yapmaz — rima-research / rima-sonnet / Codex'e delegasyon zorunlu. Tweet/video/site/doc inceleme = sub-agent."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: d18c4805-78ef-4c91-a0d0-14d6839db6a9
---

Orchestrator araştırma görevlerini kendi yapmaz. Daima sub-agent'a delegasyon.

**Why:** Orchestrator kendi WebFetch/yt-dlp/frame-extract/web-search yaptığında: (1) context dolar, (2) paralel hız kaybedilir, (3) Codex/Gemini quota kullanılmıyor. User 2026-05-18 net feedback: "orchestrator olarak değil sen bazı şeyleri çözdüğünden yavaş ilerliyoruz, agentlara versene araştırmaları için sen orchestratorsün".

**How to apply:**
- Tweet/video inceleme → `rima-research` (gemini -p) veya Codex (yt-dlp + frame extract)
- Site/SaaS karşılaştırma (autosprite.io vs PixelLab gibi) → `rima-research` (gemini)
- Repo dokümantasyon okuma (Karpathy CLAUDE.md vb.) → `rima-research` veya `rima-sonnet`
- Analiz + plan yazımı (Karpathy entegrasyon planı vb.) → `rima-sonnet` (no quota)
- Codex spec yazımı + cross-ref → `rima-sonnet`, sonra `cx_dispatch.py`

İhlal pattern (orchestrator'un kendi yapması YASAK):
- WebFetch + WebSearch'i araştırma için kullanmak (info pickup'tan fazla)
- yt-dlp + ffmpeg ile video frame extract
- Sayfa sayfa repo okuma
- Multi-step research synthesis

İstisnalar (orchestrator direkt OK):
- Tek WebFetch ile fact-check (< 30 saniye)
- DONE marker veya CURRENT_STATUS pickup (zaten kural)
- User'a sunulacak nihai özet/öneri (synthesis)

Bağlantılı: [[feedback-sonnet-first-routing]] [[feedback-codex-parallel-profile-workflow]] [[feedback-codex-vs-opus-split]]
