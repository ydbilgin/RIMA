# CODEX TASK — STAGING Directory Cleanup + Organization

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

---

## Hedef

`STAGING/` klasörü overnight çalışmadan sonra dağınık. Mantıksal subfolder'lara organize et, paths korunsun, history kaybolmasın.

## Mevcut Karmaşa

`STAGING/` root altında karışık dosyalar:
- `codex_task_overnight_*.md` (8+ task file)
- `codex_task_*.md` (eski task'lar)
- `CODEX_DONE_overnight_*.md` (8+ done report)
- `CODEX_DONE_*.md` (eski done report)
- `ANTIGRAVITY_DONE_*.md`
- `PROMPT_antigravity_*.md`
- `NLM_CANONICAL_BATCH_*.md`
- `PROGRESSION_PLAN_v0/v1/v2_*.md`
- `MORNING_INVENTORY_*.md`
- `concepts/` (subfolder)
- Diğer dağınık .md'ler

## Önerilen Yeni Yapı

```
STAGING/
├── concepts/                          (KORU — visual outputs)
│   ├── overnight/                     (overnight Codex imagegen)
│   ├── compact_sheets/                (S95 compact comparison)
│   ├── threshold_gallery/             (C1 isolated)
│   ├── skill_sheets/                  (v1 deprecated)
│   ├── skill_sheets_v2/               (v2 character + ALL skills)
│   ├── door_brainstorm/               (Codex brainstorm renders if any)
│   └── *.png                          (root level: rift_threshold_*)
│
├── _codex_tasks/                       (YENİ — task dispatch files)
│   ├── overnight/
│   │   └── codex_task_overnight_*.md
│   └── (eski codex_task_*.md)
│
├── _codex_done/                        (YENİ — done reports)
│   ├── overnight/
│   │   └── CODEX_DONE_overnight_*.md
│   └── (eski CODEX_DONE_*.md)
│
├── _antigravity/                       (YENİ — antigravity workflow)
│   ├── prompts/
│   │   └── PROMPT_antigravity_*.md
│   └── done/
│       └── ANTIGRAVITY_DONE_*.md
│
├── _research/                          (YENİ — research outputs)
│   ├── nlm/
│   │   └── NLM_CANONICAL_BATCH_*.md
│   └── pixellab/
│       └── (PIXELLAB_KB_INVENTORY.md if exists)
│
├── _plans/                             (YENİ — design plans)
│   ├── progression/
│   │   └── PROGRESSION_PLAN_v*_*.md
│   ├── door/
│   │   └── DOOR_DESIGN_SPEC_*.md
│   └── (other plan files)
│
├── _inventories/                       (YENİ — inventory + reports)
│   ├── MORNING_INVENTORY_*.md
│   ├── ASSET_*_AUDIT_*.md
│   ├── ISO_SHOWCASE_ROOM_*.md
│   └── PRODUCTION_BATCHES_*.md
│
├── _archive/                           (KORU — already exists for old)
│
└── CURRENT_STATUS.md ↑ kalır root'ta proje root'unda zaten
```

## Görev

### Adım 1: Mevcut yapıyı listele
`STAGING/` root altındaki tüm .md ve klasörleri tara, kategorize et (ne nereye gidecek).

### Adım 2: Subfolder oluştur
Eksik klasörler:
- `STAGING/_codex_tasks/overnight/`
- `STAGING/_codex_done/overnight/`
- `STAGING/_antigravity/prompts/`
- `STAGING/_antigravity/done/`
- `STAGING/_research/nlm/`
- `STAGING/_research/pixellab/`
- `STAGING/_plans/progression/`
- `STAGING/_plans/door/`
- `STAGING/_inventories/`

### Adım 3: Move (git mv ile, history korunur)
Her dosyayı yeni hedefine `git mv` ile taşı:
- `codex_task_overnight_*.md` → `_codex_tasks/overnight/`
- Diğer `codex_task_*.md` → `_codex_tasks/`
- `CODEX_DONE_overnight_*.md` → `_codex_done/overnight/`
- Diğer `CODEX_DONE_*.md` → `_codex_done/`
- `PROMPT_antigravity_*.md` → `_antigravity/prompts/`
- `ANTIGRAVITY_DONE_*.md` → `_antigravity/done/`
- `NLM_CANONICAL_BATCH_*.md` → `_research/nlm/`
- `PROGRESSION_PLAN_*.md` → `_plans/progression/`
- `DOOR_DESIGN_SPEC_*.md` → `_plans/door/`
- `MORNING_INVENTORY_*.md` → `_inventories/`
- `ASSET_*_AUDIT_*.md` → `_inventories/`
- `ISO_SHOWCASE_ROOM_*.md` → `_inventories/`
- `PRODUCTION_BATCHES_*.md` → `_inventories/`

### Adım 4: README index
`STAGING/README.md` yaz — yeni yapı açıklaması + nereden ne bulunur listesi.

### Adım 5: CURRENT_STATUS path update
`CURRENT_STATUS.md`'de path referansları varsa güncelle (concepts/overnight değişmedi ama task/done path'leri değişti — eğer referans yoksa atla).

### Adım 6: Git commit
```bash
git add -A
git commit -m "[S96] STAGING cleanup — organize overnight outputs into subfolders"
```

## Kısıtlar

- `STAGING/concepts/` DOKUNMA — visual outputs as-is kalır
- `STAGING/_archive/` DOKUNMA — already organized
- Hiçbir dosya SİLİNMEZ, sadece move
- Git mv kullan (history preserve), copy + delete YASAK
- BLOCKED if: write izni yok, git mv fail

## Final Report

`STAGING/CODEX_DONE_staging_cleanup.md`:
- Move log (kaç dosya hangi subfolder'a)
- Yeni structure tree (terse)
- Commit hash
- README.md path

## Dispatch

`--effort medium` (basit mechanical), `--profile laurethayday`, background.
