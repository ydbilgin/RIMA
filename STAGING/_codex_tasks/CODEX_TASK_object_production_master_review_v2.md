# Codex Task — Object Üretim Master Spec Re-Validate v2

**Reviewer:** Codex (high effort)
**Spec under review:** `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md` (v2 revision yazılı)
**Previous review:** `STAGING/CODEX_DONE_object_production_master_review_v1.md` (PASS_WITH_REVISIONS)
**Output:** `STAGING/CODEX_DONE_object_production_master_review_v2.md`

## Görev

Opus v2 revision'ı Iter 1'deki 5 NEEDS_REVISION (Karar #1, #4, #5, #6, #9) için doğrula. Her birine **RESOLVED / STILL_NEEDS_WORK / NEW_ISSUE** verdict ver.

Ayrıca:
- v2'de yeni eklenen item (item_descriptions kullanımı, pilot gate'ler, range budget) implementable mi?
- v2'de yeni açık 7 soru kullanıcı için makul mi yoksa Codex'in cevaplaması gereken var mı?
- Spec **LIVE** olarak işaretlenebilir mi, yoksa Iter 3 gerek mi?

## Specific Check Points

### Karar 1: L2a tool — v2 revision
- v1: "create_object 128 + L2b ile aynı dispatch" (REJECTED, view tek değer)
- v2: Default `create_object` 128 single dispatch, fallback `create_isometric_tile` 64 thin tile. Karar gate = L2a runtime'da görünür mü.
- Soru: Bu pilot-gated yaklaşım RESOLVED mu? Eksik var mı?

### Karar 4: Grouping kuralı — v2 revision
- v1: "17-64). variants of items above" range shorthand
- v2: `item_descriptions` API field kullanılıyor, n_frames=64 için 64 explicit entry veya 16 base × 4 variant explicit.
- Soru: Template B v2'de 64 item_descriptions örneği API-valid mi? `item_descriptions` array uzunluğu n_frames ile eşit olmalı mı?

### Karar 5: state_of vs yeni object — v2 revision
- v1: "%30 pixel change" hard limit (REJECTED, API resmi değil)
- v2: Görsel heuristic table, silhouette değişim → yeni object, overlay → state_of
- Soru: Wall damaged için "silhouette değişim → yeni object batch" doğru karar mı? Bütçe +100-200 gen kabul edilebilir mi?

### Karar 6: View parametresi — v2 revision
- v1: `view="side"` + `object_view="top-down"` blend
- v2: Side wall'lar için `object_view=None` default, A/B test pilot ile sidescroller karşılaştırma
- Soru: Default null mu daha güvenli, yoksa explicit top-down/sidescroller seçimi mi? Pilot A/B test stratejisi yeterli mi?

### Karar 9: Bütçe plan — v2 revision
- v1: Exact "25 gen" claims (REJECTED, no API pricing)
- v2: Range-based ("25-40 gen"), `usage` log gate sonrası refine
- Soru: Range tahminleri Codex'in memory'den hatırladığı pro tool 40-credit run + S94 evidence'a uyumlu mu?

## v2'de Eklenen Yeni İçerik (Check)

### item_descriptions API integration
- Template A v2: 4 item_descriptions entry (face_NS, face_EW, corner, arch)
- Template B v2: 64 item_descriptions entry (16 base × 4 variant pass)
- Template C v2: 16 item_descriptions entry (wall-mounted decorations)
- Soru: Bu kullanımlar API contract'la uyumlu mu? `item_descriptions` zorunlu mu, opsiyonel mi multi-frame pack'lerde?

### Pilot gates
- Karar #1: L2a runtime visibility gate
- Karar #2: ilk batch sonrası stil tutarlılığı gate
- Karar #6: A/B test object_view default
- Karar #8: 4-piece batch visual review gate
- Soru: Bu gate'ler production workflow'a entegre edilebilir mi, yoksa user'a manuel review yükü çok mu?

### Range-based budget
- v2'de hem range hem ortalama subtotal verildi (210-420 gen Sıra 1-3)
- Soru: User için actionable mı, yoksa daha conservative bir tek değer mi daha iyi?

## Output Format

```markdown
# Codex Re-Validate — Object Production Master Spec v2

## Overall Verdict
{LIVE / PASS_WITH_MINOR_REVISIONS / NEEDS_ITER_3 / BLOCKED}

## Iter 1 Revisions Status

### Karar 1: L2a — RESOLVED / STILL_NEEDS_WORK
### Karar 4: Grouping — ...
### Karar 5: state_of — ...
### Karar 6: View — ...
### Karar 9: Budget — ...

## New Content Validation

### item_descriptions API integration: VALID / INVALID
### Pilot gates: ACTIONABLE / TOO_HEAVY
### Range-based budget: ACTIONABLE / TOO_VAGUE

## Final Recommendation
- LIVE: spec ready for first pilot dispatch
- OR: Iter 3 with these specific points: ...

## Critical Blockers (if any)
- ...
```

## Hard Constraints

- **Sadece review.**
- **PixelLab MCP dispatch ETME.**
- Iter 2 review, max ~20 dk.
- Live API check tekrar yapma gerek yok (Iter 1'de yapıldı), sadece v2 revision'ları değerlendir.
