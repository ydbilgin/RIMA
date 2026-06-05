ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekmez (tooling işi, oyun tasarımı değil).

# Amaç
`cx_dispatch.py`'yi GLOBALE taşı: her projeden `cx dispatch --task-file <f> --effort <e>` çalışsın
(şu an RIMA klasörüne gömülü — başka projeler kullanamıyor). Pattern örneği ZATEN VAR: `cx limits`
komutu `CodexAuthManager\dispatch\cx_limits.py`'yi çağırıyor — aynısını dispatch için yap.

# Repo'lar
- Kaynak: `F:\Antigravity Projeler\2d roguelite\RIMA\cx_dispatch.py` (DEĞİŞTİRME/SİLME — yerinde kalır,
  geriye-uyumluluk; RIMA bunu kullanmaya devam edebilir).
- Hedef: `F:\Antigravity Projeler\CodexAuthManager\` (git repo; `codex_profile.ps1` = `cx` komutu;
  `dispatch\cx_limits.py` mevcut pattern).

# İş
1. **Path-bağımsızlık audit'i:** cx_dispatch.py'de script-dir/RIMA'ya bağlı path var mı?
   (CODEX_TASK_<profil>.md / CODEX_DONE_<profil>.md yazım yeri · `cx_profiles.local.json` okuma yeri ·
   başka sabit path). Hepsini **CWD-bazlı** yap: task/done dosyaları ÇAĞRILAN projenin köküne (CWD),
   `cx_profiles.local.json` CWD'de varsa onu, yoksa priority'siz quota-aware devam.
   Global ayar `~/.codex-profiles/.cx-settings.json` zaten global — dokunma.
2. Düzeltilmiş kopyayı `CodexAuthManager\dispatch\cx_dispatch.py` olarak ekle.
3. `codex_profile.ps1`'e **`cx dispatch <args...>`** subcommand'i ekle (cx limits pattern'i):
   tüm argümanları `python <repoRoot>\dispatch\cx_dispatch.py <args...>` 'e CWD'yi koruyarak geçir.
4. README'ye 3-5 satır `cx dispatch` dokümantasyonu.
5. **Round-trip TEST (kanıt şart):** RIMA-DIŞI bir klasörden (örn. `F:\Antigravity Projeler\CodexAuthManager`
   içinde geçici test klasörü) mini bir task dosyasıyla `cx dispatch --task-file ... --effort low` çağır →
   CODEX_TASK/DONE dosyalarının O KLASÖRDE oluştuğunu ve cevabın geldiğini doğrula (basit "echo OK" task'i yeter).
   ⚠️ Tek-instance kuralı: test, bu task'i çalıştıran instance ile AYNI PROFİLİ kullanamaz —
   farklı profil seç (`--profile` ile; `cx accounts`'tan boş olanı).
6. **ax kontrolü (sadece doğrula, kod yazma):** `ax` komutu global mi (Get-Command ax → kaynak path) +
   herhangi bir proje-bağımlı path varsayımı var mı (ax dispatch task-file'ı mutlak path alıyor, sorun yok
   beklentisi). 2-3 satır raporla.
7. CodexAuthManager'a commit (+push origin; identity=ydbilgin, Claude trailer YOK).

# Çıktı
CODEX_DONE.md: audit bulguları + değişen dosyalar + round-trip test kanıtı + ax doğrulaması + commit hash.
