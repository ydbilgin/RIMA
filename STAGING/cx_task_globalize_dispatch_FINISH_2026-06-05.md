ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekmez (tooling işi, oyun tasarımı değil).

# Amaç
ÖNCEKİ TASK YARIDA KESİLDİ (shell kill). Kod işi BİTMİŞ durumda — sadece KALAN adımları tamamla:
round-trip test + ax kontrolü + commit/push + DONE raporu. KOD YENİDEN YAZMA.

# Mevcut durum (doğrulanmış)
Repo: `F:\Antigravity Projeler\CodexAuthManager\` (git; HEAD=aded360, working tree DIRTY — bunlar önceki
task'in TAMAMLANMIŞ kod çıktısı, DOKUNMA/REVERT ETME):
- `dispatch/cx_dispatch.py` — CWD-bazlı globalize edilmiş kopya (yeni)
- `codex_profile.ps1` — `cx dispatch <args...>` subcommand eklenmiş
- `README.md` — `cx dispatch` dokümantasyonu eklenmiş
- `dispatch_smoke_20260605_181427/` — yarım kalmış smoke klasörü, içinde sadece `mini_task.md` var
  (test hiç koşmadı). Bu klasörü kullan veya sil + yenisini aç, sana kalmış.

# Kalan iş
1. **Hızlı sanity:** `dispatch/cx_dispatch.py` diff'ini oku — CWD-bazlılık tam mı?
   (CODEX_TASK/DONE dosyaları CWD'ye yazılıyor mu · `cx_profiles.local.json` CWD'den okunuyor mu,
   yoksa quota-aware devam mı · `~/.codex-profiles/.cx-settings.json` global kalmış mı).
   Eksik varsa MINIMAL fix.
2. **Round-trip TEST (kanıt şart):** RIMA-DIŞI klasörden (smoke klasörü uygun) mini task ile
   `cx dispatch --task-file ... --effort low` çağır → CODEX_TASK/DONE dosyalarının O KLASÖRDE
   oluştuğunu ve cevabın geldiğini doğrula ("echo OK" task'i yeter).
   ⚠️ Tek-instance kuralı: testteki profil, BU task'i çalıştıran instance'ın profiliyle AYNI OLAMAZ —
   `--profile` ile farklı boş profil seç (`cx accounts`).
3. **ax kontrolü (sadece doğrula, kod yazma):** `Get-Command ax` → kaynak path global mi +
   proje-bağımlı path varsayımı var mı. 2-3 satır raporla.
4. Smoke/test artıklarını temizle (smoke klasörü commit'e GİRMESİN).
5. CodexAuthManager'a commit (+push origin; identity=ydbilgin, Claude trailer YOK).

# Çıktı
CODEX_DONE: sanity bulgusu + round-trip test kanıtı (dosya path'leri + içerik) + ax doğrulaması + commit hash.
