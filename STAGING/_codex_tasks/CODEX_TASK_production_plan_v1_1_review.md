# Codex Task — Production Plan v1.1 Re-Review (Faz 2 Budget Empirical Check)

> **Profile:** any active cx profile
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_production_plan_v1_1_review.md`
> **Mode:** Review only. NO code/asset/scene/meta changes.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — review only (4) BLOCKED if unclear.

## Görev

`STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md` LIVE DRAFT. Faz 2 bütçe formülü revize edildi — empirik check + verdict.

## Inputs

- **Plan dosyası:** `STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md` (oku)
- **Parent v1:** `STAGING/PRODUCTION_PLAN_DETAILED_v1.md` (referans)
- **Master spec parent:** `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md` (Karar #9 bütçe)
- **Memory:** `memory/reference_pixellab_v3_budget_formula.md`
- **Memory:** `memory/feedback_pixellab_character_via_web_ui_v3.md`
- **Memory:** `memory/project_warblade_anim.md`
- **PixelLab:** `mcp__pixellab__list_characters` (limit 50) + `mcp__pixellab__get_character` selected character'lar için
- **PixelLab:** `mcp__pixellab__get_balance` baseline confirm

## 5 Soru — Verdict gerek

### Q1 — V3 Formula Empirical Check
17 mevcut character (list_characters) metadata'sından gen count görülebiliyor mu? Smooth full-state pipeline ile üretilmiş char varsa:
- Real gen / state hesapla
- Formula avg 32 gen/state ile karşılaştır
- **Verdict:** PASS / DRIFT / INCONCLUSIVE (formula sadece modeled, pilot real-cost log zorunlu)

### Q2 — Warblade 17-State Realistic mi?
544 avg / 650 worst hesabı:
- Frame counts per state (idle 4-6, walk 6-8, attack 8-10, signature 10-15) gerçekçi mi?
- Smooth frame sayıları avg/state'i 32'den 40+'a iter mi?
- **Verdict:** PASS_WITH_NOTES eğer modelled 32 sufficient, DRIFT eğer empirik 40+

### Q3 — Faz 2.2 Mob 200 gen Reasonable mı?
- 6-8 state × 30 avg = 180-240 → 200 holds
- Veya mob için **4-state minimum** (idle/walk/attack/die) yeter mi? ~120 gen
- **Verdict:** Accept 200 / Recommend 4-state cut to 120

### Q4 — Total Budget Margin Sufficient?
1264 avg / 2433 = %52 kullanım, %48 marj.
- Act 2 + Act 3 + iterasyon için yeter mi?
- Hesap: Act 2 muhtemelen Faz 1-3 benzeri = ~700 gen, Act 3 benzer = ~700 gen → 1400 gen → kalan 1169 marj YETMEZ
- Recommendation: Faz 2.2 mob defer (200 gen save) + 1 char DEFER strict
- **Verdict:** SUFFICIENT (current) / TIGHT (Act 2/3 risk) / NEEDS_DEFER

### Q5 — Pilot B Gate Discipline
3-state Warblade pilot Faz 2.1'e gating:
- Mantıklı mı, yoksa daha küçük scope mu (2-state) yeterli?
- Gate threshold (≤35/35-45/>45) realistic mi?
- **Verdict:** PASS / Recommend different scope

## Output Format

```markdown
# Production Plan v1.1 Re-Review — Codex Report

## Empirical Data
- list_characters: N total chars
- Sampled smooth-state chars: [list]
- Avg gen/state observed: XX
- Get_balance baseline: 2433 / 5000

## Q1 Verdict
- Formula avg 32: PASS / DRIFT / INCONCLUSIVE
- Notes: ...

## Q2 Verdict (Warblade 544 avg)
- ...

## Q3 Verdict (Mob 200)
- ...

## Q4 Verdict (Budget margin)
- Recommendation: ...

## Q5 Verdict (Pilot B gate)
- ...

## Overall Verdict
PASS / PASS_WITH_REVISIONS / NEEDS_REVISION / REJECT

## Suggested Revisions (varsa)
- {revision 1}

## Açık Sorular (varsa)
- {soru}
```

## Hard Constraints

- **No file writes outside output report.**
- **No new PixelLab dispatch** (list_objects + list_characters + get_balance read-only OK).
- **Auto-commit YOK.**
- **Max 1 iter** — review only, no back-and-forth.
- **BLOCKED if unclear:** list_characters API erişiminde sorun varsa durdur, modelled verdict ver, empirical defer et.
