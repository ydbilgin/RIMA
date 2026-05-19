# Codex Task — Review of MAP_PLAN_v1.md

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Goal

Review `STAGING/MAP_PLAN_v1.md` (Opus draft, RIMA map design roadmap). Bu plan oyun haritasının vizyonunu, MVP scope'unu, oda tiplerini, biome'ları, generation philosophy'i, room sizes'ı, library yaklaşımını, v15h LIVE state bağlantısını, Phase 1.5 spec entegrasyonunu ve asset production roadmap'ını içeriyor. **Hedef:** "Map plan netleşsin ki ne üreteceğimizi bilelim" — user direktifi 2026-05-18.

## What to review (focus areas)

1. **Scope realism (MVP gerçek 10 dk loop'a uygun mu?):**
   - 8-9 oda + Boss F1 + 1 biome + Warblade only — bu gerçek 10 dk mı yoksa 20 dk mı?
   - 20 RoomTemplate target çok mu / az mı?
   - Mevcut 10 template'in MVP-readiness'i (gerçek dosya kontrolü: `Assets/Data/Rooms/Library/`)

2. **Asset production budget fit (5000 PixelLab + Codex hybrid):**
   - W1 800 gen tahmini map+character için doğru mu?
   - 5000 PixelLab allocation LOCK'ına çakışma var mı? (`memory/project_5000_pixellab_allocation_lock.md`)
   - Mob roster 4-6 MVP için yeterli mi (8 mob roster planlanmıştı)?

3. **Phase 1.5 RoomData spec uyumu:**
   - `STAGING/PHASE_1_5_ROOMDATA_SPEC_DRAFT.md` ile bu plan uyumlu mu?
   - 5 open question'ı sen oku ve listele — Map Plan'da bunlar açıklı görünüyor (?'lı). Opus'un karara bağlaması için somut sorular hazırla.

4. **Gameplay loop coverage (NLM kanonik vs plan):**
   - Oda tipleri 9 (Combat/Elite/Boss/Shop/Forge/Curse/Event/Unknown/Spirit). MVP 5 tip. Plan eksik tip mi düşürmüş?
   - 15-node Act 1 topology vs MVP 8-9 oda — sıkıştırma mantıklı mı, yoksa "tam Act 1" mi olmalı?
   - Map Fragment / 3-fork visibility mekaniği MVP'de var mı yok mu? Plan'da explicit değil.

5. **Missing edge cases / red flags:**
   - DungeonMapUI implementation status (19 günlük memory) — gerçekten var mı yok mu? Grep: `find Assets/Scripts -name "*DungeonMap*"`
   - v15h composition pipeline ↔ RoomTemplate köprüsü — Plan §11 #2'de soruyor. Concrete önerin?
   - Threat Points scaling system implement edilmiş mi? Grep: `find Assets/Scripts -name "*ThreatBudget*" -o -name "*ThreatPoints*"`
   - Mob spawn waves / encounter system MVP'de var mı? Grep: `find Assets/Scripts -name "*Encounter*" -o -name "*WaveSpawner*"`

6. **v15h LIVE state — fix priority:**
   - Wang RuleTile 6/16 yarım — fix priority MVP'de critical mi yoksa ertelenebilir mi?
   - L5-L8 atmospheric layer boş — MVP'de gerekli mi?
   - 2 Warblade overlap — definite bug mı yoksa intentional test stamp mı? Composer kodunu kontrol et.

## What to output

`STAGING/CODEX_REVIEW_MAP_PLAN_DONE.md` formatında:

```
# Codex Review — MAP_PLAN_v1.md

## Verdict
[ACCEPT / ACCEPT_WITH_MODS / REJECT]

## Strengths
- ...

## Weaknesses / Risks
- ...

## Concrete recommendations
1. (priority HIGH/MED/LOW) — change X to Y because Z
2. ...

## Open question resolutions (for Opus)
- §11 #1: ... (suggest with evidence)
- §11 #2: ... 
- §11 #3 (Phase 1.5 5 questions): list them verbatim + suggest answers
- ...

## Asset gap evidence
- Library: [actual file list]
- DungeonMapUI: [exists/missing — file paths]
- ThreatBudget: [exists/missing]
- Encounter system: [exists/missing]

## Red flags
- ...
```

## Constraints

- **NO code changes.** Sadece review + recommendation.
- **NO PixelLab/Codex imagegen.**
- Mevcut dosyaları **oku** (Read), gerekirse grep. Kod değiştirme.
- 30 dk içinde tamamla — uzun research değil, surgical analiz.
- Yargılarını **evidence-backed** yap (file:line yoksa "I think" değil).

## Honest constraints

- Bu plan opus'un draft'ı — eksik veya yanlış olabilir, dürüst eleştir.
- "Plan iyi" demek yetmez — somut HIGH/MED/LOW önerileri olmalı.
- 5 open question (Phase 1.5) ve 8 open decision (Map Plan §11) somut karara hazır hâle getir.
