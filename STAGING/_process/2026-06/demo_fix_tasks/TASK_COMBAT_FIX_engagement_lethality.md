# TASK — Fix room-1 combat: engagement (idle) + lethality (cluster instant-death)

ACTIVE RULES: (1) KÖK NEDENİ önce KANITLA (data), sonra fix (2) min/cerrahi kod (3) sadece combat engagement+token-gating; balance'ı gut etme (4) BLOCKED if intended-difficulty belirsizse orchestrator'a sor.

NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR, raporda console.

E1 OUTPUT: Sonucu DOSYAYA yaz; dönüşün ≤10 satır + yol.

## AMAÇ (demo #1 öncelik)
Combat BROKEN (`DONE_combat_health.md`): (1) düşmanlar IDLE kalıyor (engage etmiyor), (2) engage olunca küme 3-4sn'de oyuncuyu öldürüyor. Oynanabilir combat olmadan demo yok. **Opening room-1 wave WINNABLE olmalı.**

## KÖK NEDEN — önce KANITLA (execute_code, runtime)
**A) Engagement/Idle:**
- `BaseMobBehavior.cs:105` `Start()` → `FindGameObjectWithTag("Player")` TEK SEFER; null kalırsa `Update()`:154 + dist-calc:166 kalıcı early-return → sonsuz Idle, no velocity. **Re-acquire YOK.**
- Runtime'da DOĞRULA: spawn edilen mob'larda `Player` null mu? (idle mob'ları örnekle). Ayrıca per-prefab `detectionRange` override'ı spawn-mesafesinden küçük mü (HalfThrall 6.05'te idle kaldı, base detection=8 → ya Player null ya prefab override <6.05). Hangisi KANITLA.

**B) Lethality:**
- `AttackTokenType` (BaseMobBehavior.cs:60/73) eşzamanlı-saldıran sınırlama sistemi VAR. Runtime'da DOĞRULA: küme halindeyken aynı anda kaç mob `Attack` state + gerçekten hasar veriyor? Token-gating çalışıyor mu yoksa hepsi mi vuruyor? (kümede 100→0/3-4sn = gating bozuk şüphesi).
- `MobAttack_PenitentCombo.cs:19-21` damage 20/25/40.

## FIX (cerrahi, WINNABLE hedefli)
**A)** Düşmanlar Player'ı GÜVENİLİR edinsin: `Player` null ise `Update`'te yeniden dene (kalıcı-idle yerine). detectionRange spawn-mesafesi için yeterli olsun (gerekirse base'i ~10'a çek VEYA spawner mob'ları range içine koysun — hangisi minse). Düşman spawn sonrası ~2-3sn içinde Chase→Attack'a geçmeli.
**B)** Eşzamanlı-saldıran gating çalışsın: kümede aynı anda en fazla 1-2 mob vursun (token sistemi onarılırsa damage'a dokunmadan çözülür — TERCİH bu). Damage'ı düşürmek SON çare.
**KISIT:** `BossAI_PenitentSovereign` (boss) DOKUNMA — sadece wave'in type-3 Penitent-MOB'u. spawnProps=false gate (F1 fix) DOKUNMA. Karpathy cerrahi.

## VERIFY (data-driven playtest — bu DÖNGÜ kullanıcının istediği)
Fix sonrası opening wave OYNA + ölç (execute_code, screenshot değil):
- Düşmanlar spawn sonrası ~2-3sn içinde Chase (velocity>0) → Attack'a geçiyor mu?
- Oyuncu opening wave'i (başlangıç skill'leriyle) **survive edip CLEAR ediyor mu** — HP sonu >0, RunStats.Kills = wave-count?
- Süre ~20-40s (idle DEĞİL, instant-death DEĞİL)?
- Kümede eşzamanlı saldıran ≤2?
- 1 gerçek mid-combat screenshot (düşman Chase/Attack + player alive + HP bar) — semantic doğrula.
- read_console 0.

Eğer fix sonrası HÂLÂ instant-death/idle varsa → BLOCKED yaz + data, sessizce partial bırakma.

## RAPOR `STAGING/_process/2026-06/demo_fix_tasks/DONE_combat_fix.md`
- Kök neden(ler) kanıtla (A null-Player mı detection mı; B token-gating durumu)
- Fix: dosya+satır listesi (cerrahi)
- Before/after data tablosu (t|HP|aliveEnemies|kills|chasing?|concurrent-attackers)
- VERDICT: COMBAT PLAYABLE Y/N + opening-wave clear süresi
