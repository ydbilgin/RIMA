# FAZ A KOD REVIEW — bağımsız correctness lens (Gemini 3.1 Pro)

cx (laurethayday) DEMO TOOLS Faz A'yı yazdı + commit'ledi (`169e198e`). Sen BAĞIMSIZ reviewer'sın (writer≠reviewer). Kodu OKU, correctness/regression hatası ara. Türkçe yanıt, tam Türkçe karakter.

## CANON (buna uymalı)
- `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md` — DamageType/ElementTag/renk/resist kararı
- `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` — 10 class KİLİTLİ stat değerleri

## OKU (yeni/değişen dosyalar)
- `Assets/Scripts/Balance/` → DamageType.cs, ElementTag (DamagePacket içinde mi?), DamagePacket, DamageCalculator.cs, ClassStatProfile.cs, ClassStatRuntime.cs, ClassStatDatabase.cs, DamageColors
- `Assets/Scripts/Systems/PlayerClassManager.cs` (ApplyClassStats wiring)
- `Assets/Scripts/Player/PlayerStats.cs` (SetMaxHP var mı / doğru mu)
- `Assets/Scripts/Player/PlayerController.cs` (SetMoveSpeed)
- `Assets/Scripts/Player/PlayerAttack.cs` + `Assets/Scripts/Combat/BasicAttack/*` (DamagePacket entegrasyonu, DealDamage(int) korunmuş mu)
- `Assets/Scripts/Skills/SkillRuntime.cs`, `PlayerProjectile.cs` (DealDamage overload bozulmamış mı)
- `Assets/Resources/Balance/Classes/*.asset` (10 profil — değerler SANDBOX_DIRECTOR ile eşleşiyor mu?)

## KONTROL ET (PASS/FAIL + kanıt, dosya:satır)
1. **DamageCalculator:** physPower/abilityPower switch + armor/magicResist azaltma `r/(r+100)` + True savunmayı atlıyor mu? ElementTag formüle GİRMEMELİ — girmiş mi?
2. **Renkler:** DamageColors'da Lightning `#FFE600` (crit `#FFD24A` ile çakışmıyor), True beyaz, Physical ember, Ability cyan doğru mu?
3. **Wiring:** HP `PlayerStats.maxHP`'ye gidiyor (Health.cs DEĞİL)? atkSpeedMult SADECE cooldown'a (hareket/anim değil)? moveSpeed doğru?
4. **Geriye uyum:** `DealDamage(int)` çağıranlar kırılmış mı? DamagePacket default ElementTag.None ile eski çağrılar çalışıyor mu?
5. **10 class değeri:** asset'lerdeki HP/physPower/abilityPower/atkSpeedMult/moveSpeed SANDBOX_DIRECTOR tablosuyla birebir mi? Yanlış/eksik değer var mı?
6. **Regression riski:** namespace/using, null-ref (GetComponent null), Resources.Load path doğru mu?

Sonuç: **PASS** veya **FAIL** + bulgular listesi (her biri dosya:satır + neden). Belirsizse "doğrula" de.
