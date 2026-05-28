# TASK: Auto-Memory Cleanup (Conservative Move-to-Archive)

ACTIVE RULES: (1) think before deleting (2) min surgical action (3) MOVE not DELETE (4) BLOCKED if unsure.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/` klasöründeki 197 auto-memory dosyasını conservative cleanup. REVOKED/stale entries `_archive_overnight_2026_05_26/` alt klasörüne taşı. MEMORY.md index yeniden yaz — sadece Active + Review Later kalır. MOVE not DELETE.

## Kategorize Kuralları

### Kesin _archive'a taşı:
- Adında "REVOKED" geçen
- Adında "old_" / "_v1" / "_v2" prefix var ve karşılığında daha yeni dosya var
- 2026-04 öncesi `date` field'lı project memory'ler EĞER LIVE status'ta değilse
- `_archive` zaten path'inde olan (zaten arşiv altında — ELLEME)
- `description` field'ında "SUPERSEDED" veya "DEPRECATED" geçenler
- Memory'de **Iso Archive** section'da listelenmiş ama ana dosya konumunda hala bulunan duplicates

### Kesin tut (Active):
- `MEMORY.md` index'te **Active (S106 NIGHT LATEST)** veya **Active (S105 carry)** section altında listelenmiş
- type=feedback HARD rule (her zaman tutulur)
- type=reference (her zaman tutulur, lookup için)
- `description` field'ında "LIVE" veya "LOCKED" geçen project memory'ler
- 2026-05-13 sonrası date stamp

### Şüpheli durumda BIRAK:
- Açık bir REVOKED işareti yoksa, sil değil — bırak. Conservative.

## Yapılacaklar

### Adım 1: Inventory
- Tüm 197 dosyayı listele
- Her birini Read et (frontmatter sadece, ilk 20 satır)
- Kategorize: ACTIVE / ARCHIVE / SUSPICIOUS

### Adım 2: Move to _archive
- Hedef klasör: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/_archive_overnight_2026_05_26/`
- ARCHIVE kategorisindekileri taşı (Bash mv)
- SUSPICIOUS olanlara dokunma

### Adım 3: MEMORY.md Index Yeniden Yaz
- Mevcut MEMORY.md format'ını koru
- Active section: kalan Active dosyaları listele (her biri 1 satır: `- **dosyaadi.md** - özet`)
- Review Later section: eski Review Later entries (varsa)
- Iso Archive section: KALDIR (zaten _archive/ klasöründe)
- _archive_overnight_2026_05_26 section ekle: taşınan dosyalar listesi (geri alınabilir)
- 200 satır altında kalsın (Claude index limit)

### Adım 4: Verify
- 197 → kalan Active sayısı (~30-50 hedef)
- Taşınan sayısı (~150 hedef)
- Index satır sayısı
- MEMORY.md format doğru mu

## Hard Constraints
- **MOVE not DELETE** — `mv` Bash, `rm` ASLA
- Şüphede kalırsa BIRAK
- HARD rule feedback dosyalarına DOKUNMA (her zaman Active)
- Reference type dosyalara DOKUNMA
- type=user dosyalara DOKUNMA (kullanıcı profili kalıcı)
- Commit YAPMA
- Git operasyonu YAPMA

## Inline rapor (<500 kelime)
- Inventory: 197 dosya kaç Active / Archive / Suspicious
- Taşınan dosya sayısı
- _archive klasör path
- MEMORY.md yeni satır sayısı + format doğru mu
- BLOCKED varsa neden
- Beklenmedik durumlar
