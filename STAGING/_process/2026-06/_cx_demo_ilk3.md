ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only listed scope (4) BLOCKED if unclear.
NLM ACCESS: Gerekmez. Direct-read: aşağıdaki backlog + kod.

# Amaç
YARIN canlı demo. `STAGING/DEMO_POLISH_BACKLOG_2026-06-13.md` **Bölüm 3 "YARIN İÇİN İLK 3"**ü uygula (Fable-5 reviewer'ın kanıtlı bulguları — dosyada satır numaraları ve gerekçeler var, ÖNCE ONU OKU):

1. **DEMO RESET paketi:** DirectorMode'daki ÖLÜ Quick Reset butonunu (listener yok, ~DirectorMode.cs:2373) gerçek demo-reset'e bağla: oyuncuyu dirilt (Health'e minimal `Revive()` — ölüde Heal no-op olduğundan şart), Director-spawn düşmanları + propları temizle, DeathScreen'i kapat, timeScale=1 garanti (timeScale çift-sahip çatışması guard'ı — backlog'daki bulgu #4).
2. **PlayerProjectile telemetry yolu:** packet'siz dalı (PlayerProjectile.cs:88-100 else) SkillRuntime yolundan geçir — backlog'un önerdiği tek-dal değişiklik; BALANS DEĞİŞMEZ (hasar sayısı aynı kalmalı, sadece event/telemetry akışı düzelir). Fireball böylece CSV'ye düşer.
3. **Parlaklık paketi:** stat slider değişiminde kısa toast ("physPower 50→250"), pause'da DPS penceresi freeze (unscaled-pencere bulgusu), spawn'da minik dust-puff (SkillVfx.PlayBurst kısa ömür). ~50 satır, DirectorMode-içi.

## YASAK
Backlog'un diğer maddeleri (Shadowblade FindNearestEnemy, presetler vb.) — SADECE İLK 3. Refactor yok.

## GATE
1. Compile 0 error (`read_console`).
2. EditMode focused: DirectorModeValidationTests + VFXTests yeşil; full fail sayısı 13'ü AŞMAMALI.
3. Play-mode data-proof: `execute_code` ile (a) oyuncuyu öldür→Reset çağır→HP=Max+IsDead=false+timeScale=1 doğrula; (b) Fireball'u packet'li yoldan vur → `TelemetryEventCountForValidation` artıyor doğrula. Bitince `manage_editor stop` GARANTİ.
4. Console 0 error.

## RAPOR
CODEX_DONE_<profil>.md: değişen dosyalar + her madde 1 satır + GATE sonuçları (sayılarla). Fail/belirsizlik → BLOCKED.
