# TASK T2: Combat juice tuning + [RMB] Execute prompt + 8-SFX paketi + M1 dash buffer

ACTIVE RULES: (1) think before coding (2) min code — TUNING+BAĞLAMA işi, sıfırdan sistem YOK (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"

## Amaç
MASTER_PLAN T2 (`STAGING/MASTER_PLAN_FINAL_2026-06-06.md` §C/T2 + M1 bullet — READ). Vuruş hissi dozu + imza mekaniğin görünürlüğü + sessizliğin bitmesi. Unity OPEN.

## Mevcut sistemler (cx-audit kanıtlı — REUSE, yeniden yazma)
- `Assets/Scripts/Combat/Juice/`: FeelToggleSettings, HitPauseDriver (hit .04/crit .07/kill .12/finisher .18), ScreenShakeDriver, HitFlashDriver, BrokenStateVisual
- `Assets/Scripts/Combat/BasicAttack/`: BasicAttackProfile (startup/commitment/knockback dizileri), MeleeChainBehavior (slash arc emit)
- `Assets/Scripts/Skills/Warblade/DeathBlow.cs` (Broken/Sundered gate'li execute) + SkillStateTracker
- `Assets/Scripts/Audio/AudioManager.cs`: Sfx enum + muted-fallback + `Resources/Audio` override yükleme; hook'lar ZATEN ÇAĞRILI (PlayerController/Health/Gate/DraftManager/BasicAttackBehaviorBase)
- Chamber'daki [G]-interact dünya-prompt altyapısı (ChamberSelectBootstrap nearest-station prompt pattern'i)
- Dünkü walkable clamp (WalkabilityMap.ClampVelocityToWalkable) — dash/knockback ona uyumlu kalmalı

## Work items
1. **Juice değer pass'i:** light/heavy ayrımını netleştir — light: pause .03/flash beyaz/küçük knockback · heavy: pause .06/shake-S/Broken uygula · execute: freeze .08-.12 + büyük slash + shake-M · knockdown: shake-M. Serialized değerlerle oyna (FeelToggleSettings/driver alanları); kod değişikliği minimum. ⚠️ Düşman hit-flash = BEYAZ + kırmızı/magenta rim — CYAN YASAK (cyan=oyuncu/Rift).
2. **[RMB] Execute dünya-prompt'u:** Broken/Sundered hedef oyuncuya ≤2 birim iken hedefin üstünde "[RMB] İnfaz" world-space prompt (chamber [G]-prompt pattern'i REUSE; yeni UI sistemi YOK). DeathBlow tetiklenince prompt söner + freeze + SFX. Birden çok Broken hedefte: en yakın TEK hedefte göster.
3. **8-SFX paketi:** CC0 kaynak (Kenney.nl paketleri — Impact Sounds / Interface Sounds / RPG Audio; PowerShell ile indir, lisans dosyasını da koy `Assets/Audio/_licenses/`). 8 klip: swing(light/heavy pitch-vary) · hit_impact · dash · enemy_death · execute_payoff · room_clear/portal_open · draft hover+select · chamber_ambient loop. `Resources/Audio/` adlandırmasını AudioManager'ın beklediği isimlerle eşle (koda bak); eksik enum varsa MINIMAL genişlet. Volume dengesi kaba pass (kulak yakmasın).
4. **M1 dash input buffer (~80ms):** PlayerController — non-cancellable pencerede (attack windup/recovery, dash tail) basılan dash 0.08s kuyruklanır, ilk uygun frame'de tüketilir. Coyote-time YOK. Çift-dash YOK. Walkable clamp davranışı değişmez.
5. **Verify:** compile clean · play-probe: light vs heavy hissedilir fark (pause/shake değer dökümü logla) + Broken dummy'de prompt görünür→RMB→freeze+ses (chamber dummy'de test edilebilir!) + 8 event'in sesi çalıyor (AudioSource aktif kanıtı) + buffer probe (windup'ta dash bas→bitince çıkar) · smoke 26/26 + önceki yeşil testler bozulmadı (özellikle KnockbackTests + WalkableEnforcementTests).
6. **Commit** (ydbilgin, English, no Co-Authored-By): `feat(combat): juice tuning, execute prompt, sfx pack, dash input buffer`. Özet → `STAGING/_done_T2_juice_2026-06-07.md` (file:line + değer tablosu eski→yeni + klip listesi/lisans).

## Constraints
- HitInstance/payload REFACTOR YASAK (R5 sert red — mevcut zincire dokunma, sadece değer+bağlama).
- AudioMixer/bus stack YOK (post-demo); basit Resources yüklemesi yeter.
- ScreenshotMode/WalkabilityMap/exporter dosyalarına dokunma (bu gece commit'lendi); PlayerController'a SADECE buffer için dokun.
- timeScale freeze fizik bozarsa değeri düşür/geri al — FeelToggleSettings'ten kapatılabilir kalmalı.
