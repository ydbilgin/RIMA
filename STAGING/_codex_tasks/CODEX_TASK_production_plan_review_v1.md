# Codex Task — Production Plan Detailed v1 Review

> **Reviewer:** Codex (gpt-5-codex high effort)
> **Plan dosyası:** `STAGING/PRODUCTION_PLAN_DETAILED_v1.md`
> **Master spec parent:** `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md` (v2 LIVE PASS_WITH_MINOR_REVISIONS, no Iter 3)
> **Wall+object plan parent:** `STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md`

## Görev

Opus production plan v1 dosyasını review et. Plan ham implementation dosyası değil — master spec'in EVIDENCE-BACKED somut prompt + faz sırası uygulaması.

Master spec v2 LIVE statüde, Codex Iter 2 onaylı. Bu plan onun **uygulama katmanı**. Sadece **uygulama drift'lerini** + **PixelLab API constraint uyum**'unu doğrula. Spec içeriği yeniden tartışma.

## Inputs (Sadece Bu Dosyaları Oku)

1. **`STAGING/PRODUCTION_PLAN_DETAILED_v1.md`** — Review target
2. **`STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`** — Parent spec (cross-ref)
3. **`STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md`** — Original wall+object plan (context)

> Başka dosya okuma. Memory entry okuma yetkisi sub-agent yok.

## Verdict Format

```
VERDICT: PASS / PASS_WITH_REVISIONS / NEEDS_REVISION / BLOCKED

SUMMARY: 1-3 cümle özet

CHECKS (her birine PASS / FAIL / FLAGGED):

1. PixelLab API constraint compliance
   - size enum doğru mu? (32 / 64 / 128 / 256)
   - n_frames enum [1, 4, 16, 64]?
   - view valid string?
   - object_view valid value veya None?

2. item_descriptions field caveat ACTIONABLE mi?
   - Pilot dispatch verify step plan'da var mı?
   - Plan B (REST direct) hazır mı?
   - Batch 1.1 pilot bu caveat'i çözüyor mu?

3. Prompt formülü Karar #7 uyum (her batch için)
   - Genre label YASAK uygulanmış mı?
   - 3rd-party name YASAK uygulanmış mı?
   - HEX color kullanımı consistent mi?
   - Canvas occupancy cue side wall'larda var mı?

4. Evidence layer kalitesi (KRİTİK)
   - Her batch için reference object verilmiş mi?
   - Drift gözlemler dürüst yansıtılmış mı (cherry-pick değil)?
   - "Pilot risk: kanıt yok" işaretleri batch'lerde var mı?
   - Var olan object_id'lere atıflar gerçek mi (var olduğunu varsay, Codex erişim yok)?

5. Bütçe master spec Karar #9 ile uyum
   - Range-based (lower-upper) verilmiş mi?
   - Cumulative + buffer hesabı tutarlı mı?
   - Total %24 (worst case 580 gen / 2433 bütçe) realistic mi?

6. State listesi Karar (Faz 2.1) HARD LOCK uyum
   - State listesi üretim öncesi user onay gate olarak işaretli mi?
   - Cross-class 6 slot defer alternatifi sunulmuş mu?

7. Pilot batch 1.1 önerisi mantıklı mı?
   - 4-test single batch coverage yeterli mi?
   - Mini pilot (n_frames=1 tek piece) alternatif önerilmiş mi (Açık Soru Q1)?
   - Pilot başarı/başarısızlık branch logic net mi?

8. Faz 2 V3 web UI USER manual delegation
   - feedback_pixellab_character_via_web_ui_v3 HARD LOCK uyumlu mu?
   - RIMA gen budget'tan ayrı olduğu işaretlenmiş mi?

9. Açık Sorular (Q1-Q6) actionable mi?
   - User Antigravity review için yeterli karar gate'i var mı?
   - Cevaplanabilir form'da mı (open-ended değil)?

10. Cross-batch overlap (Q4) doğru yönetilmiş mi?
    - Batch 1.4 floor clutter + Batch 4.1 item icons overlap noted?
    - Alternatif (rebase tek batch) açıkça sunulmuş mu?

REVISIONS NEEDED (eğer varsa):
- {revision 1}
- {revision 2}

OPEN QUESTIONS (her biri Q1-QN cevaplama):
- Q1 cevabı: pilot vs mini pilot — Codex önerisi?
- Q2 cevabı: state listesi 23 vs 17?
- Q3 cevabı: slash VFX state_of vs yeni object?
- Q4 cevabı: floor clutter + item icons overlap stratejisi?
- Q5 cevabı: damaged variant bütçesi OK (master spec'te zaten resolved)?
- Q6 cevabı: interior ruined view stratejisi?

PILOT DISPATCH READY?: YES / NO + sebep
```

## Önemli Kontroller

### A. API Constraint (Hard Validation)

PixelLab MCP tool schemas:
- `create_object`: `size` enum, `view` string, `directions` int, `n_frames` enum, `object_view` optional
- `item_descriptions`: optional `list[str]`, length should ideally == n_frames
- `create_object_state`: parent `object_id` required, state edit description

Master spec v2 Iter 2 ACTIONABLE caveat:
- `item_descriptions` field MCP wrapper'da forward edip etmediği **kanıtlanmamış**. Plan'ın pilot batch (Batch 1.1) bu verify step'ini içermeli.

**Kontrol:** Bu caveat plan'da Batch 1.1 risk listesinde **explicit** mi? Plan B (REST direct) plan'da mı?

### B. Evidence Layer (KRİTİK — bu plan'ın ana value-add'i)

Her batch için Opus geçmiş object_id'leri referansladı + visual gözlem yazdı. Codex erişimi yok, ama:
- Object_id format UUID v4 doğru mu?
- Gözlemler (drift / billboard / flat) dürüst yansıtılmış mı? Plan'ın açık fail örnekleri (65c99904 flat drift) var mı?
- Cherry-pick: sadece başarı örnekleri verilmiş mi (FAIL), yoksa fail ve başarı dürüstçe verilmiş mi (PASS)?

### C. Bütçe Hesap Self-Consistency

Plan'da:
- Faz 1: 240 reserve / 340 worst
- Faz 2: 0 (V3 ayrı)
- Faz 3: 150 worst
- Faz 4: 90
- Total: 480 reserve / 580 worst
- Bütçe: 2,433 gen (subscription kalan)
- % max: 24%

**Kontrol:** Bu rakamlar arasında self-consistency var mı? Master spec Karar #9'un range-based + reserve upper bound önerisi uygulanmış mı?

### D. Memory HARD LOCK Uyumu

Plan birden fazla HARD LOCK'a atıfta bulunuyor:
- `feedback_pixellab_character_via_web_ui_v3` (Faz 2)
- `feedback_character_state_planning_before_production` (Faz 2.1 state listesi)
- `feedback_wall_decoration_pure_attachment_only` (Batch 1.3 + 1.5)
- `feedback_user_cannot_draw_full_autonomy_required` (Aseprite fallback YASAK)
- `feedback_pixellab_no_dark_fantasy` (genre label YASAK)
- `feedback_negation_to_positive_prompts` (positive RIMA specs)

**Kontrol:** Plan'ın her batch prompt'unda bu lock'lar implicitly uyumlu mu? Genre label sızdırma yok mu?

## Önemli OLMAYAN Kontroller (skip)

- Spec içeriği tartışma (master spec v2 LIVE — Iter 2 PASS_WITH_MINOR_REVISIONS, kapalı)
- Yeni karar önerisi (orchestrator gate, Opus karar)
- Object_id'lerin gerçek var olduğu doğrulama (Codex'in PixelLab erişimi yok)
- Visual quality değerlendirmesi (Opus visual gözlem yaptı)

## Constraints

- Output: `STAGING/CODEX_DONE_production_plan_review_v1.md`
- Effort: high
- Max 800 satır output (verdict + checks + revisions + Q&A)
- Background DEĞİL — orchestrator bekler.

## Reference (master spec v2 LIVE özet)

Master spec'in Iter 2 sonrası 9 LIVE karar:
1. L2a wall base: pilot-gated, default create_object 128 view="low top-down" single
2. L2b wall face: create_object 128 view="side" n_frames=4 + item_descriptions PILOT GATE
3. Size × n_frames matrix (32→64, 64→16, 128→4 natural threshold)
4. Grouping rule: main description = style anchor, item_descriptions = per-frame identity, no duplicate numbering
5. state_of vs new object: görsel heuristic (% piksel API resmi YASAK)
6. View mapping: side wall object_view=None default (top-down blend YASAK)
7. Prompt formula: [GEOMETRY, MATERIAL+HEX, DETAIL+HEX, PERSPECTIVE+CANVAS, STYLE, BG]
8. 4-piece batch: create_object n_frames=4 + item_descriptions PILOT GATE
9. Budget range-based, Faz 1-3 reserve 420 upper, expected 210-420

Implementation caveat: `item_descriptions` MCP wrapper forward verify Batch 1.1'de.
