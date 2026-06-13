ACTIVE RULES: (1) think before acting (2) min steps, sadece listele/oku/özetle (3) surgical — SADECE aşağıdaki yolları tara, başka yere dokunma, HİÇBİR DOSYA DEĞİŞTİRME (4) BLOCKED if unclear.

NLM ACCESS: Gerekmez (saf envanter). 

# Amaç
RIMA'nın TÜM organizasyon yapısını indexle (orchestrator'a düzenleme + LaurethStudio uyarlama kararı için ham envanter). Bu SALT-OKUMA görevi — hiçbir şey değiştirme/silme.

## Taranacak yerler (hepsi):
1. `F:\Antigravity Projeler\2d roguelite\RIMA\.claude\` — tüm dosyalar (PROJECT_RULES.md, settings*, varsa skills/, hooks). Her dosya: ad + boyut + 1-2 satır ne işe yaradığı.
2. `C:\Users\ydbil\.claude\skills\` — GLOBAL skill klasörleri. Her biri: ad + SKILL.md description satırı + RIMA'ya-özgü mü yoksa proje-bağımsız mı (içinde hardcoded RIMA yolu/adı var mı GREP'le bak).
3. `C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\` — TÜM .md dosyaları. Her biri: name + metadata type (user/feedback/project/reference) + description satırı + son anlamlı tarih + ŞÜPHE BAYRAKLARI: (a) MEMORY.md index'inde kayıtlı DEĞİL (orphan), (b) içeriği başka dosyayla çakışıyor/tekrar, (c) REVOKED/eski karara referans, (d) tarihi 14+ gün eski VE point-in-time gözlem.
4. `F:\Antigravity Projeler\2d roguelite\RIMA\MEMORY\` — repo-içi paylaşımlı memory (INDEX.md + dosyalar). Aynı format. Ayrıca: `~/.claude/projects/...` memory'siyle ÇAKIŞAN konular var mı?
5. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\` — SADECE üst-seviye .md dosyaları (alt klasörlere girme). Her biri: ad + tarih + LIVE-mi-bayat-mı şüphesi (karar dosyası mı, süresi geçmiş task mı).
6. `F:\Antigravity Projeler\2d roguelite\RIMA\CLAUDE.md` + `RULES.md` + `AGENTS.md` (varsa) — 1'er satır özet.

## ÇIKTI — şu dosyaya yaz: `STAGING/_process/2026-06/_index_rima_structure_RESULT.md`
Bölümler:
- **A. .claude/ envanteri** (tablo)
- **B. Global skills** (tablo: ad | tek satır | proje-bağımsız mı E/H | RIMA-hardcode notu)
- **C. Memory (user-level)** (tablo: dosya | type | 1 satır | bayraklar)
- **D. Memory (repo MEMORY/)** (tablo + user-level ile çakışma listesi)
- **E. STAGING üst-seviye** (tablo: dosya | tarih | LIVE/bayat şüphesi)
- **F. ŞÜPHELİ/BAYAT TOP-10** — düzenleme adayları, gerekçeyle
- **G. İstatistik** — toplam dosya sayıları

Türkçe karakter tam doğru kullan (ı/ş/ğ/ü/ö/ç). Yorum/öneri YOK — sadece envanter + bayrak. Karar orchestrator'ın.
