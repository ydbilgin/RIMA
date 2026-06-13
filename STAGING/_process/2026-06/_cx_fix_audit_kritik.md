ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: Gerekmez (kod görevi).
UNITY ERROR CHECK: İş bitince UnityMCP read_console (Error) çağır; compile hatası varsa ÇÖZ, çözemiyorsan BLOCKED yaz; raporda console durumunu belirt (0 error şartı).

Proje kökü: F:/Antigravity Projeler/2d roguelite/RIMA (Unity açık). BUGÜN hocaya CANLI DEMO — minimal, cerrahi fix; refactor YOK.

# GÖREV: Bugünkü state-makinesi audit'inin 🔴 DEMO-KRİTİK bulgularını düzelt
Bulgu detayları + satır numaraları: `STAGING/_process/2026-06/_audit_state_2026-06-13.md` (ÖNCE OKU — sadece 🔴 bölümleri).

## FIX A — PlayerAttack hiçbir yerde disable edilmiyor (audit D-1 + DM-1, aynı kök)
Ölüm ekranında VE Director pause'da (timeScale=0) sol-tık hâlâ oyuncu saldırısı tetikliyor; Director'da tek tıklama hem spawn hem saldırı oluyor. `PlayerController.enabled` toggle'lanıyor ama `PlayerAttack` ayrı component, atlanıyor.
İSTENEN: Tek `SetPlayerActive(bool)` helper (audit'in önerdiği yer neresiyse — muhtemelen PlayerController veya merkezi bir yer) → PlayerController + PlayerAttack birlikte aç/kapa. Çağrı noktaları: ölüm (DeathScreenManager), revive/Quick Reset, Director pause giriş/çıkış (DirectorMode SetState). Simetri ŞART: her disable'ın karşılığı enable; Quick Reset sonrası oyuncu kesin saldırabilmeli.

## FIX B — Skill/mermi kill'leri sayılmıyor (audit S-1)
`CombatEventBus.PublishKill` SADECE `BasicAttackBehaviorBase.cs:93` ölüm dalında; `SkillRuntime.DealDamage` (ve YENİ `DealDamageRaw`) yolunda ölümde PublishKill YOK → skill ile öldürmede kill-juice (shake/hitpause) tetiklenmiyor + kill sayacı eksik.
İSTENEN: SkillRuntime ölüm tespit noktas(lar)ına PublishKill ekle — basic-attack dalıyla AYNI semantik (aynı parametreler/sıra). Çifte-publish tuzağına dikkat: bir ölüm yalnız BİR kez publish edilmeli (basic-attack yolu zaten publish ediyorsa skill yolu onunla çakışmamalı — guard'ı kontrol et).

## DOKUNULACAK DOSYALAR (audit'in işaret ettikleri; başkasına dokunma):
- Assets/Scripts/ altında ilgili dosyalar: PlayerAttack/PlayerController/DeathScreenManager/DirectorMode (Fix A) + SkillRuntime/BasicAttackBehaviorBase/CombatEventBus (Fix B) — audit raporundaki satır referanslarını izle.
- Blueprint .asset + CURRENT_STATUS.md DOKUNMA. Git commit YAPMA.

## DOĞRULAMA (zorunlu):
1. Recompile → read_console 0 error.
2. EditMode BİR KEZ: assembly `RIMA.Tests.EditMode`; baseline 11 fail; şart ≤11 + yeni fail yok.
3. Data-proof (execute_code, compact string): (a) Director pause'da PlayerAttack.enabled==false, çıkışta true; ölüm→revive sonrası true; (b) skill-kill simülasyonunda PublishKill TAM 1 kez.

## RAPOR (E1):
`STAGING/_process/2026-06/_cx_fix_audit_kritik_DONE_<profil>.md` + standart CODEX_DONE_<profil>.md. Tam Türkçe karakter ZORUNLU. Her fix: ne değişti (dosya+satır) + data-proof sonucu + test/console durumu. ≤10 satır özet yeter, uzun anlatma.
