ACTIVE RULES: (1) think before judging (2) evidence-based — diff'i kendin oku (3) surgical — SADECE git diff/dosya oku + EditMode test koş, başka HİÇBİR ŞEY değiştirme (4) emin değilsen UNCERTAIN.
NLM ACCESS: Gerekmez.

# Amaç — CODE REVIEW (writer≠reviewer: yazan profil yasinderyabilgin, SEN farklı profilsin)
Demo İLK 3 batch'ini bağımsız review et. Task spec: `STAGING/_process/2026-06/_cx_demo_ilk3.md` (önce oku). Writer raporu: `CODEX_DONE_yasinderyabilgin.md`.

## İncele (git diff ile, uncommitted):
1. `Assets/Scripts/Core/Health.cs` — yeni `Revive()`: minimal mi, TakeDamage/Heal/IsDead semantiğini bozuyor mu, event'leri (OnDamageTaken vb.) yanlış tetikliyor mu?
2. `Assets/Scripts/Core/DeathScreenManager.cs` — death-state iptali temiz mi, timeScale çift-sahip çatışması gerçekten guard'landı mı?
3. `Assets/Scripts/Skills/PlayerProjectile.cs` — packet'siz dal SkillRuntime'a yönlendi; **BALANS DEĞİŞMEMELİ** (hasar miktarı aynı yol/aynı sayı mı? double-publish kalktı mı, yoksa event kaybı mı oldu?)
4. `Assets/Scripts/UI/DirectorMode.cs` — SADECE bu batch'in ekleri (Quick Reset listener, toast, DPS freeze, dust puff); önceki commit'li kısımları YARGILAMA.

## Kontroller:
- Scope creep var mı (İLK 3 dışı değişiklik)?
- Toast/dust-puff'ta leak riski (destroy edilmeyen GameObject/coroutine)?
- DPS freeze: pause'dan dönünce pencere doğru devam ediyor mu?
- EditMode'u BİR KEZ kendin koş (assembly RIMA_EditMode_Tests): fail sayısı ≤13 doğrula.

## RAPOR → `CODEX_DONE_laurethayday.md`
Her dosya: PASS/FAIL + 1-2 satır kanıt (satır no). Sonda: GENEL VERDİKT + commit'e uygun mu + (varsa) sorun önem sırası.
