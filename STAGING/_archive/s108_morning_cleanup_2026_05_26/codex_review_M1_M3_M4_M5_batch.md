# Codex Review Batch — M1 + M3 + M4 + M5 Coherence Check

**Date:** 2026-05-16 S86 PREP-3
**Workflow:** Opus implement → Codex review (16-18 May window). Bu görev = Codex review pass.
**Goal:** 4 dokümanın TUTARLILIK + DRIFT + MISSING + RISK + EXECUTABILITY denetimi. Implementation YOK — sadece review.

---

## 1. Görev

Aşağıdaki 4 dokümanı oku ve KAPSAMLI cross-review yap. Bu dokümanlar Brush V1 Sprint 9-10 + Wang tileset üretim pipeline'ının execute edilebilir spec'lerini içeriyor. Opus implement etmeye başlamadan önce Codex'in onayı gerekiyor.

### Okunacak dokümanlar (sıra önemli):
1. `STAGING/sprite_strategy_FINAL_LOCK.md` (M1 — TEK YETKİ kaynağı, çatışmada her zaman kazanır)
2. `STAGING/codex_brush_sprint9_atlas_importer.md` (M3 — Sprint 9 implementation spec)
3. `STAGING/codex_brush_sprint10_room_template_bank.md` (M4 — Sprint 10 implementation spec)
4. `STAGING/pixellab_l3_wang_chain.md` (M5 — PixelLab Wang tileset üretim spec)

### Referans dokümanlar (gerektiğinde oku):
- `STAGING/codex_meta_review.md` (önceki meta-review — drift detect için baseline)
- `STAGING/sprite_strategy_FINAL_PLAN.md` (önceki konsolide plan — M1 ile çelişki olmamalı)

---

## 2. Review Boyutları

### A. INTERNAL COHERENCE (her doc kendi içinde)

**M1 (FINAL_LOCK):**
- 17 bölüm tutarlı mı? Bir bölümün karar verdiği değer başka bölümde reddedilmiş mi?
- Bölüm 0 Core Locks tablosu Bölüm 4-13 ile birebir uyumlu mu?
- Sprint 9-13 sequence (Bölüm 10) FINAL_PLAN orijinal Sprint 9-13 ile karşılaştır — meta-review correction'ları (Sprint 10 RoomBank vertical slice BEFORE Natural Engine + Sprint 13 add + Markov/AI tag defer) doğru uygulanmış mı?
- LOCK seal'da listelenen "non-negotiable"lar diğer bölümlerde gerçekten enforce ediliyor mu?

**M3 (Sprint 9 spec):**
- Deliverables listesi M1 Bölüm 4 Data Model + Bölüm 7 Importer + Bölüm 9 P0 Retrofit ile birebir mi?
- File scope tablosu eksiksiz mi? Forbidden list Sprint 10+ scope'u doğru exclude ediyor mu?
- Exit criteria (10 madde) test edilebilir + verifiable mi?
- Open questions kritik mi yoksa Opus delegasyon noktası mı?
- Wang generator delegate edilmiş — BrushAtlasImporter ile arasındaki kontrat net mi?

**M4 (Sprint 10 spec):**
- RoomTemplateSO V1 full → Sprint 9 stub'la diff net mi? Sprint 9'da olmayan ne ekleniyor?
- RoomBankSO `Pick(RoomType, int seed)` deterministic implementation imkânı sağlanıyor mu? (seed → same room garantisi)
- Save/Load round-trip test scope'u GUID stability'i ispatlıyor mu?
- PlayMode test "exit valid" data validation only — Sprint 11 nav check'i excluded mı?
- RoomTemplate ≠ Encounter prensibi tüm field'larda korunuyor mu? (encounterTags var ama enemy identity yok)

**M5 (PixelLab Wang spec):**
- `mcp__pixellab__create_topdown_tileset` parametre listesi eksiksiz mi?
- Wang 16-case numerology (4-bit NESW) M1 Bölüm 2 ile birebir mi?
- QC checklist 16 case coverage'ı verify ediyor mu?
- Fallback (Strategy A explicit) tetikleme koşulu net mi?

### B. CROSS-DOCUMENT CONSISTENCY (4 doc birbiriyle)

- **M1 ↔ M3:** M3'deki Data Model field listesi M1 Bölüm 4 ile karşılaştır. Eksik/fazla field var mı? Field type'lar tutarlı mı?
- **M1 ↔ M4:** M4'deki RoomTemplateSO V1 M1 Bölüm 4'teki RoomTemplateV1 spec'iyle uyumlu mu? (M1 stub, M4 full — full = stub fields + impl alanları)
- **M3 ↔ M4:** Sprint 9 stub → Sprint 10 full geçişte breaking change var mı? RoomTemplateSO field rename/remove/type-change yok mu?
- **M1 ↔ M5:** Wang Full 16 corner set M1 Bölüm 2 LOCK ile M5 production spec birebir mi? Bitstring encoding (NESW vs SWNE vs WSEN) tek standart mı?
- **M3 ↔ M5:** M3 BrushAtlasImporter Wang generator → M5 PixelLab output PNG ile uyumlu mu? Master texture size + grid layout assumption'lar match ediyor mu?

### C. MISSING / GAPS

- M3 + M4'te Sprint 9-10 implementation tamamlamak için **YETERSIZ** kalan yer var mı? (Codex implement etse spec sorusu kalmayacak mı?)
- M1 LOCK doc'unda Bölüm 15 (Unresolved) listesindeki sorular Sprint 9-10 implementation'ı blocking mı? Eğer evet → Opus signoff için escalate et.
- 2 P0 retrofit (R1 scaleRange + R2 sorting layer) M3'te yeterince detay verilmiş mi? Mevcut codebase ile drift kontrolü?
- RoomTemplate ↔ EncounterBank API surface (Sprint 12+ scope) hiç düşünülmüş mü? Mevcut M4 spec Sprint 11/12 ile çakışmadan ilerleyebilir mi?

### D. EXECUTABILITY (Opus implement edebilir mi?)

- M3 + M4 file scope = ~25 yeni dosya + 4 mevcut dosya modify. Bu 1.5-2 day + 1-1.5 day estimate'larıyla **realistik** mi? Yoksa scope creep var mı?
- Hangi deliverable'ları kesilirse "minimum vertical slice loop" hâlâ kapanır? (M9 GO gate için minimum gereken alt küme nedir?)
- Wang generator implementation (M3 BrushAtlasImporter delegate'i) **belirsiz**: PixelLab output grid auto-detect vs template-driven hangisi? M3 + M5 birlikte hangisini implies ediyor?

### E. RISK + DRIFT

- `codex_meta_review.md` Section 6 Risks (8 risk) M3 + M4'te address ediliyor mu? Hangileri eksik?
- 16-18 May workflow override (Opus implement → Codex review) bu spec'lerin yapısını değiştiriyor mu? Normal Codex implement spec'i ile bu Opus implement spec arasında format/tone farkı zaten yansıtılmış mı?
- "Memory consolidation" (M2) tamamlandı — memory'deki yeni LOCK'lar (room-library-architecture, brush-tool-v1 Sprint 9-13) M1 ile çelişiyor mu?

---

## 3. Output Format

`STAGING/codex_review_M1_M3_M4_M5_DONE.md` dosyasını yaz. Şu yapıyı kullan:

```markdown
# Codex Review M1+M3+M4+M5 — DONE Report

## VERDICT
**Status:** PASS / PASS-WITH-CONDITIONS / FAIL
**Confidence:** %X
**Reason (1 cümle):** ...

## A. INTERNAL COHERENCE
- M1: [PASS/FAIL] — bulgular
- M3: [PASS/FAIL] — bulgular
- M4: [PASS/FAIL] — bulgular
- M5: [PASS/FAIL] — bulgular

## B. CROSS-DOCUMENT CONSISTENCY
- M1 ↔ M3: ...
- M1 ↔ M4: ...
- M3 ↔ M4: ...
- M1 ↔ M5: ...
- M3 ↔ M5: ...

## C. MISSING / GAPS
[Bulleted, evidence-backed]

## D. EXECUTABILITY
- M3 (Sprint 9) realistik mi? ...
- M4 (Sprint 10) realistik mi? ...
- Minimum vertical slice loop subset: ...
- Open question priority list (Opus signoff için): ...

## E. RISKS + DRIFT
[Bulleted, codex_meta_review.md baseline ile karşılaştır]

## F. BLOCKING CONCERNS (Opus implement öncesi çözülmeli)
[Numbered list, her madde için: konum + soru + öneri]

## G. RECOMMENDED ACTIONS
1. M1 / M3 / M4 / M5 edits (varsa)
2. Open questions for Opus signoff
3. Sprint 9 implementation green-light ediliyor mu?
4. Wang tileset dispatch (M6) green-light ediliyor mu?
```

---

## 4. KISITLAR

- Bu görev REVIEW. Implementation kodu yazma. C# yazma.
- Dosya değiştirme — sadece `STAGING/codex_review_M1_M3_M4_M5_DONE.md` yaz.
- Mevcut codebase'i incelemen gerekiyorsa: `Assets/Scripts/MapDesigner/Brush/` altındaki mevcut dosyalara bak (Sprint 1-8 LIVE), ama değiştirme.
- 4 dokümanı tam oku — özet okuma YASAK.
- BLOCKING concern'ler için somut evidence ver (dosya + satır + alıntı).

---

## 5. PASS Gate

Codex review SONUÇLARI:
- **PASS:** Opus 4 dokümanı kabul eder, M6 (Wang dispatch) + M7 (Sprint 9 implementation) yeşil ışık.
- **PASS-WITH-CONDITIONS:** Spesifik düzeltmeler yapılır, sonra ilerlenir.
- **FAIL:** Spec'ler revize edilir, ikinci review döngüsü.

Codex objektif kal. Spec'leri olumlu görmek için pas verme — gerçek gap varsa belirt.

---

## 6. References (oku öncelikle)

1. `STAGING/sprite_strategy_FINAL_LOCK.md`
2. `STAGING/codex_brush_sprint9_atlas_importer.md`
3. `STAGING/codex_brush_sprint10_room_template_bank.md`
4. `STAGING/pixellab_l3_wang_chain.md`
5. `STAGING/codex_meta_review.md` (drift baseline)

Opsiyonel mevcut codebase exploration:
- `Assets/Scripts/MapDesigner/Brush/Data/`
- `Assets/Scripts/MapDesigner/Brush/Execution/`
- `Assets/Scripts/MapDesigner/Brush/Validation/`
- `Assets/Tests/EditMode/Brush/`
