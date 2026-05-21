# Codex Task — Progression v2 FINAL Plan Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS (mandatory for any RIMA design context):
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## Görev

Orchestrator + user `STAGING/_plans/progression/PROGRESSION_PLAN_v2_FINAL.md` üzerinde 4 boşluk/anlamsızlık tespit etti. Sen bunları NLM canonical ile doğrulayıp **revize edilmiş v2.1 plan** üreteceksin.

## Girdi Dosyalar (oku)

1. `STAGING/_plans/progression/PROGRESSION_PLAN_v2_FINAL.md` — mevcut v2 plan
2. `STAGING/concepts/overnight/13_all_acts_master_flow.png` — Codex render, alt bantta Stay/Break/Carry ikonları var
3. `memory/project_progression_canonical_lock.md` — Karar #60/61/62/63 canonical
4. `memory/project_rima_style_manifesto.md` — Sentez stil HARD LOCK
5. NLM canonical kararlar (yukarıdaki CLI ile sorgula)

## Tespit Edilen 4 Problem

### Problem 1 — Stay/Break/Carry meta-track şüphesi
Image 13 alt bantta 3 ikon var (STAY-yeşil, BREAK-kırmızı, CARRY-cyan). Orchestrator bunlara "run-başı meta-track" anlamı verdi ama:
- v2 plan'da geçmiyor
- NLM canonical Karar #60/61/62/63'te yok
- Sadece Codex render dekoru olabilir

**Yapılacak:**
- NLM'ye sor: "RIMA'da Stay/Break/Carry diye bir sistem var mı? Karar numarası ne?"
- Cevap **YES + canonical referans varsa** → v2.1'e formal sistem olarak ekle, mekanik tarifi yap
- Cevap **YES ama belirsiz** → 3 alternatif tasarım öner (path-selector / starting-buff / playstyle-modifier), tradeoff analizi
- Cevap **NO** → "render dekoru, sistem değil" notu düş, v2.1'den temizle, image 13'e regenerate uyarısı bırak

### Problem 2 — Death Imprint mekanik spec gap
v2 plan'da "proposal, not locked, spec gate pending" diyor ama somut alternative yok. User "anlamadım" dedi. Visual lock var (image 12), mekanik tanımsız.

**Yapılacak — 3 alternative mekanik öner:**
- **A) Pure narrative imprint** — visual only, gameplay etkisi yok. Sadece "the room remembers" hissi.
- **B) Persistent room scar** — öldüğün sub-room sonraki run'da +%X mob density / +1 hazard / lighting darker. Hard difficulty bias.
- **C) Imprint Echo Drop** — öldüğün sub-room next-run girişinde "ghost echo" mob spawnlar (sen+silahın), kill edersen Shattered Echoes bonus +X. Risk/reward.

Her alternatif için:
- Mekanik tarif (1 paragraf)
- RIMA Style Manifesto uyumu (Hades/AD/Diablo sentez mi, clone mu, drift mi)
- Implementation cost (yeni system / mevcut sub-room data extend / pure VFX)
- Boss gate / fragment ekonomisine etki

Orchestrator + user'a karar gate bırak (recommendation belirt ama LOCK etme).

### Problem 3 — Act 4 son boss reward boşluğu
Image 13 Act 4 satırında "All Max" yazıyor — user "run bitiyor zaten ne düşecek ki sonraki runda kullanabilecem?" dedi. v2 plan Section 2'de sadece Act 1 boss reward var (Relic + HP %50). Act 2/3/4 boss reward'ları yazılmamış.

**Yapılacak:**
- "All Max" ne demek netleştir (Act 4 drop pool tier max → render decoration olabilir, drop değil)
- Act 4 son boss (The Architect) **win/lose flow** explicit yaz:
  - WIN → Shattered Echoes bonus +X (büyük), "Architect defeated" meta-unlock (yeni class / keepsake / hub feature), ending sequence
  - LOSE → standard Shattered Echoes (Act 4'e gelmenin bonus'u), unlock yok
- Act 2 Echo Twin + Act 3 Fracture Sovereign reward'larını da spesifik yaz (Relic tier eskalasyonu Epic→Legendary)

### Problem 4 — RIMA Style Manifesto Drift Audit
v2 plan stilistik olarak Manifesto'ya uyumlu mu kontrol et:
- Sentez (Hades + Alabaster Dawn + Diablo) korunuyor mu, yoksa tek referansa kayıyor mu
- Anti-pattern catalog'da v2 plan ihlal eden bölüm var mı (örn. Hades-clone room types, StS-clone reveal, Diablo-clone loot)
- 9 Combined Item placeholder isim (Iron Veil / Cursebound Coil / vs.) — Manifesto'ya uygun mu, yoksa generic fantasy mı

PASS / WARN / FAIL per nokta, her FAIL için fix önerisi.

---

## Çıktı

`STAGING/_plans/progression/PROGRESSION_PLAN_v2_1_REVIEW.md` aşağıdaki yapıda:

```
# Progression v2.1 Review

## Section 1 — Stay/Break/Carry Verdict
[NLM query result + recommendation]

## Section 2 — Death Imprint Mechanic Options
[3 alternative full spec + recommendation + LOCK gate]

## Section 3 — Act 2/3/4 Boss Reward Spec
[Full reward flow per act, win/lose branches]

## Section 4 — Style Drift Audit
[Per-section PASS/WARN/FAIL with evidence quotes]

## Section 5 — v2.1 Patch Diff
[v2 FINAL'den değişen satırlar — explicit before/after]

## Section 6 — Open Questions (orchestrator/user gate)
[Karar bekleyenler liste]
```

## Kısıt

- v2 FINAL'i sıfırdan yazma — sadece 4 problem alanına dokun.
- NLM canonical kararla çelişen revizyon ÖNERME — sadece raporla.
- Style Manifesto'yu bozacak fix önerme.
- Pure mechanical (kod) implementation YOK — bu salt design review.

## Effort
high
