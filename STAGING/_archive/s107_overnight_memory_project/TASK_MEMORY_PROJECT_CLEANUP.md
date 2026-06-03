# TASK: Proje MEMORY/ + TASARIM/ Stale Cleanup (Conservative)

ACTIVE RULES: (1) think before deleting (2) min surgical action (3) MOVE not DELETE (4) BLOCKED if unsure.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: Proje root altındaki `MEMORY/` klasörü (varsa) ve `TASARIM/` klasörü içinde **REVOKED/stale** referansları temizle. Conservative — şüpheliyse BIRAK.

## Kapsam

### `MEMORY/` (proje root, varsa)
- Eğer klasör yoksa skip — `ls MEMORY/` ile kontrol
- Varsa: aynı kurallar (Active vs Archive)

### `TASARIM/` (TASARIM dosyaları)
- `MASTER_KARAR_BELGESI.md` — KAL (canonical, REVOKED kararlar override ile zaten işaretli)
- `FAZLAR/FAZ_MASTER.md` — KAL
- `GDD.md` — KAL
- Eski REVOKED karar dosyaları (eğer ayrı dosya olarak varsa) — _archive'a
- `ANIMATION_REDESIGN.md` referansı MASTER_KARAR'da var ama dosya YOK — orphan reference, MASTER_KARAR güncelleme YAPMA (sadece not düş)
- `_archive` zaten path'inde olan — ELLEME

### Yapılacaklar

#### Adım 1: Inventory
- `ls MEMORY/` → varsa içerik tara
- `ls TASARIM/` → ilk seviye dosya listesi
- `ls TASARIM/_archive/` → zaten arşiv (skip)

#### Adım 2: Stale tespit
- Dosya adında "REVOKED" / "OLD_" / "DEPRECATED"
- Frontmatter `status: DEPRECATED` veya benzeri
- Son güncelleme 2026-04 öncesi VE Karar #X REVOKED ifadesi var ve aktif değil
- ANIMATION_REDESIGN.md gibi orphan reference'lar — sadece raporla, dosya yoksa elleme

#### Adım 3: Move
- `MEMORY/_archive_overnight_2026_05_26/` (varsa)
- `TASARIM/_archive_overnight_2026_05_26/` (yoksa oluştur)
- Stale dosyalar oraya taşınır

#### Adım 4: Rapor
- MEMORY/ inventory + taşınan
- TASARIM/ inventory + taşınan
- BLOCKED veya orphan reference listesi (sadece bilgi, müdahale yok)

## Hard Constraints
- **MOVE not DELETE**
- Canonical TASARIM dosyalarına (MASTER_KARAR, FAZ_MASTER, GDD, SYSTEM_MAP) DOKUNMA
- `_archive` zaten path'inde olan ELLEME
- Şüphede kalırsa BIRAK
- Commit YAPMA

## Inline rapor (<400 kelime)
- MEMORY/ var mı, içerik
- TASARIM/ inventory + taşınan
- Orphan reference listesi (silinmedi, sadece info)
- BLOCKED varsa neden
