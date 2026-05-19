---
name: codex-as-reviewer-until-may18
description: "16-18 Mayıs 2026 arası Codex reviewer rolünde — Claude Code (Opus/Sonnet) implement, Codex review. Geçici routing tersine çevrildi."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# Codex-as-Reviewer Routing — May 16-18, 2026 (geçici)

**Rule:** Bu 3 günlük pencerede (16-18 Mayıs 2026) **Codex reviewer rolünde**. Claude Code (Opus/Sonnet) implementation yazar, Codex sadece **bağlam + kod review** alır, "iyi mi / değiştir / red" cevabı verir.

**Why:** User'ın Claude Code limit'i bu günlerde **aşırı yüksek seviyede**, Codex'in idle capacity'si yüksek. Eskisi (Codex implement / Opus review) tersine: now Opus/Sonnet implement / Codex review. Limit normalize olunca eski routing'e dön.

**How to apply:**
- Implementation iş (kod yaz, refactor, dosya oluştur) → Opus veya Sonnet (Claude Code) yapar
- Review iş (yapılan kod doğru mu, spec'e uyuyor mu, hata var mı, atlanan şey var mı) → rima-codex agent dispatch
- Codex'e dispatch ederken: bağlam dosyaları + yazılan kod parçaları + review check listesi ver
- Codex'in cevabı: PASS / FAIL with specific evidence (eski rima-qc behavior'a benzer)
- Codex'e implementation dispatch ETME bu pencere içinde (idle capacity boşa harca demek değil, doğru iş için kullan)

**Exception:**
- User explicit "Codex implement et" derse → onay anına özel bypass
- Mechanical task'lar (file rename, dependency bump) Codex implement edebilir, ama spec'e uyan iş Opus

**End date:** 2026-05-18 (limit normalize olunca [[codex-vs-opus-split]] eski routing'e dön)

# See Also
- [[codex-vs-opus-split]] — Eski routing kuralı (limit normal döndüğünde geçerli)
- [[research-on-block]] — Block durumunda Codex/Gemini/NLM/Web kullan kuralı
