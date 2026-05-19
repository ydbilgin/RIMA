# Codex Task — CB VISION_DOC Review (Doc 1/4)

**Tarih:** 2026-05-18
**Profile:** laurethayday
**Effort:** high
**Output:** `STAGING/CODEX_TASK_cb_vision_doc_review_DONE.md`

---

## ACTIVE RULES
(1) think before answering (2) min commentary, no fluff (3) sadece review yap — yeni karar üretme (4) PASS/REVISE/FAIL ver

## DİREKT REVIEW

Review hedef: `STAGING/CB_VISION_DOC_draft.md`

Bağlam: User pivot decision yaptı, RIMA'dan CB'ye geçiyor. Bu **3-document set'in 1. dosyası** (VISION_DOC — long-term identity, 12+ ay vade).

User explicit istekler:
1. Sub-genre LOCK = **"Real-Time Generative Action Roguelike"** (NOT Cascade ARPG — user override)
2. Item/Mastery tree açık karar (defer Faz 2)
3. RoR2/Megabonk loot/box feel notu eklenmeli
4. Future-self pickup notu (kullanıcı sonradan dönecek)
5. Klasör yapısı dokümante
6. Faz faz progression net

## REVIEW KRİTERLERİ

8 kritere göre PASS/FAIL/REVISE ver:

### 1. Sub-genre lock netliği
- "Real-Time Generative Action Roguelike" net mi?
- Section 2'de pivot edilemez şekilde belirtildi mi?
- Cascade ARPG önerini override eden user kararı saygıyla yansıtıldı mı?

### 2. Future-self pickup faydası
- Section 0'daki 9 madde — 6 ay sonra döndüğünde hızlı geri dönüş sağlar mı?
- Önemli pointer'lar (MVP_PLAN, README, sub-genre lock) net mi?

### 3. Item/Mastery decision tracking
- Section 20.4 NOT — 3 yaklaşım (loot yok / light loot / mastery tree) net mi?
- "Faz 2 playtest verisi ile karar" boundary sağlam mı?
- RoR2/Megabonk feel context'i var mı?

### 4. Anti-klon coverage
- Section 29 — 7 oyun klon riski + mitigasyon yeterli mi?
- Eksik klon riski var mı (örn: Last Epoch, Diablo 4)?

### 5. Phase progression coherence
- Section 30 — Phase 1-4 timeline tutarlı mı?
- Faz 2 (5 class + skill variant + 2 Act) gerçekçi mi 3-6 ay'da?
- Faz 4 atlas + 12+ class fantasy değil mi?

### 6. Class/Element/Tile taxonomy expansion
- Section 8 (5 → 11 element) coherent mi?
- Section 14 (3 → 12+ class) progression mantıklı mı?
- Section 10 (3 → 12 hibrit) MVP'den vizyona zincir doğru mu?

### 7. Decision log completeness
- Section 32 — 22 karar listesi MVP scope kararlarını tam yansıtıyor mu?
- Eksik karar var mı?
- "OPEN" vs "LOCKED" markers doğru mu?

### 8. Format + readability
- Markdown structure clean mi?
- Tablolar tutarlı mı?
- Türkçe/İngilizce karışık ifadeler problem mi?
- Section başlık numbering doğru mu (1-32)?

## OUTPUT FORMATI

Output: `STAGING/CODEX_TASK_cb_vision_doc_review_DONE.md`

```markdown
# CB VISION_DOC Review — Codex Verdict

## Overall: PASS / REVISE / FAIL

## Section-by-section
| Section | Status | Issue (if any) |
|---|---|---|
| 0. Future-self | PASS/REVISE | ... |
| 1. Vision | ... |
...

## Critical issues (MUST FIX before save)
1. ...
2. ...

## Minor suggestions (nice-to-have)
1. ...

## Anti-clone coverage gaps
- ...

## Eksik/Yanlış decisions (Decision Log)
- ...

## Final verdict
- [ ] APPROVE — User CB klasörüne taşıyabilir
- [ ] REVISE — Şu 3-5 critical fix gerekli
- [ ] FAIL — Major rework gerekli
```

**Beklenen cevap uzunluğu:** 300-600 satır.

**Bitiş:** `CB VISION_DOC REVIEW COMPLETE` satırı.
