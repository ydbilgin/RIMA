# CODEX TASK — Progression Plan v0 Review → v1

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Offline mode (auth expired) — bağlam local memory ve plan dokümanından alınacak.

---

## Hedef

`STAGING/PROGRESSION_PLAN_v0_DRAFT.md` dosyasını incele. Bu RIMA için map fragment progression + reward + room type planının ilk taslağı.

7 açık sorusu var. Plan'ı review et, v1 üret.

## Görev

### Adım 1: PLAN OKU
Read: `STAGING/PROGRESSION_PLAN_v0_DRAFT.md` (full file).

### Adım 2: CONTEXT OKU (paralel)
- `CURRENT_STATUS.md`
- `MEMORY/project_karar_149_subroom_encounter_lock.md`
- `MEMORY/project_karar_150_act1_envanter_live.md`
- `MEMORY/project_path_c_hybrid_lock.md`
- `MEMORY/PIXELLAB_TOOL_GUIDE.md` (optional, eğer threshold için ek bilgi varsa)
- Skill code spotcheck: `Assets/Scripts/Skills/Warblade/*.cs` (1-2 dosya)

### Adım 3: REVIEW
Her bölümü (2a-2f) PASS / NEEDS REVISION / FAIL olarak değerlendir:
- 2a Threshold karar (A2 Imprint Scar vs C1 Scar Compass)
- 2b Room Type Mapping (9 tip × variant)
- 2c Reward Drop Canonical (8 drop)
- 2d Map Fragment Progression
- 2e Rune Sistemi
- 2f Echo Imprint Cascade Integration

### Adım 4: 7 AÇIK SORUYA CEVAP
Plan'ın madde 3'teki 7 açık soruya direkt cevap ver. Sebep + öneri.

### Adım 5: v1 ÜRET
`STAGING/PROGRESSION_PLAN_v1_CODEX.md` yaz:
- v0'ı baz al
- 7 açık soruyu RESOLVED yap (LOCK decisions)
- Tutarsızlık varsa düzelt
- Sub-room (Karar #149) ile entegrasyon detay ekle
- Skill code integration (rune sistemi) detay ekle
- Production cost güncelle
- "Codex review verdict" section ekle (NEEDS REVISION listesi)
- v0'a göre değişen maddeler highlight et (CHANGED: ...)

### Adım 6: NEXT-STEP ÖNERİ
v1 sonunda orchestrator için 3-5 next-step öneri:
- Hangi dispatch (asset gen, code impl, shader work)
- Sırası ve dependency
- Hangileri paralel hangileri serial

## Kısıtlar

- Plan dökümanına YAZMA, sadece v1'i yeni dosya olarak yaz
- Kod implementasyonu YAPMA, sadece plan refinement
- Belirsizlik varsa açıkça yaz, tahmin YASAK
- BLOCKED if: plan dosyası okunamaz, memory dosyası eksik

## Çıktı

| Path | İçerik |
|---|---|
| `STAGING/PROGRESSION_PLAN_v1_CODEX.md` | v1 revised plan |
| `STAGING/CODEX_DONE_progression_plan_review.md` | Review verdict (PASS/REVISION counts, 7 soruya cevap özeti, next-step) |

## Stil

Terse, decision-oriented. "X olmalı çünkü Y" — kararsızlık ve uzun açıklama yok.

## Dispatch

```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_progression_plan_review.md --effort high
```

Background. Notify on complete.
