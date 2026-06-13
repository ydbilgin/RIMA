ACTIVE RULES: (1) think before judging (2) evidence-based — diff'i kendin oku (3) surgical — SADECE git diff/dosya oku, HİÇBİR ŞEY değiştirme (4) emin değilsen UNCERTAIN.
NLM ACCESS: Gerekmez.

# Amaç — FIX TUR 2 REVIEW (writer=Claude Opus sub-agent; sen bağımsız reviewer'sın)
Önceki review'ında (CODEX_DONE_laurethayday.md son hali) iki fix'i FAIL'lemiştin. Tur 2 fix raporu: `STAGING/_process/2026-06/_fix_ilk3_tur2_2026-06-13.md` (önce oku). SADECE iki bulgunun kapanıp kapanmadığına odaklan.

## Doğrula (git diff, uncommitted):
1. **FIX 1 — `SkillRuntime.DealDamageRaw()` yeni raw yol + `PlayerProjectile.cs` packet'siz dal onu çağırıyor:**
   (a) Nihai hasar birebir gelen `damage` — DamageCalculator/ResolveCombatStats/status/identity/situational/debug/defense HİÇBİRİ raw yola dokunmuyor mu?
   (b) Tek-publish: OnDamageApplied + PublishSkillHit raw yolda birer kez; çifte publish yok.
   (c) Diğer DealDamage overload'larının davranışı değişmedi mi?
2. **FIX 2 — `DirectorMode.cs` TelemetryClock aktif-pause çıkarması:**
   (a) Pause AKTİFKEN clock tamamen donuyor mu (unscaledTime terimi iptal)?
   (b) Pause yokken davranış eski haliyle aynı mı; ClearTelemetry rebase'i (~2263-2277) ile çelişki yok mu?
   (c) Çağrı yerleri DEĞİŞMEMİŞ olmalı (sadece clock fonksiyonu).

## Scope notu:
- `Assets/Scripts/VFX/SkillVfx.cs`'te 1 satırlık fallback-path güncellemesi + `floor_riftcrack.png`'nin `Assets/Resources/Sprites/Environment/Decals/`'e taşınması ORCHESTRATOR işi (Unity-binding audit fix'i) — scope creep DEĞİL, yargılama.
- Önceki batch'in commit'lenmemiş değişiklikleri diff'te duruyor — onları da yargılama.
- Bunlar DIŞINDA yeni/açıklanmamış değişiklik varsa raporla.

## RAPOR → `CODEX_DONE_<kendi profilin>.md`
Fix başına PASS/FAIL + kanıt (satır no). Sonda: GENEL VERDİKT + commit'e uygun mu.
