ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Modular-design council kararının (STAGING/MODULAR_ABILITY_DECISION_2026-06-12.md) "Şimdi Yap" adımının 1+2'si: DAVRANIŞ DEĞİŞTİRMEYEN saf DRY temizliği. Mevcut SkillBase mimarisi KIRILMAZ, yeni asset/mimari EKLENMEZ. İki iş: (1) tekrar eden AOE damage+status dizisini tek SkillRuntime helper'ına çek, (2) lokal targeting kopyalarını mevcut SkillRuntime helper'larına yönlendir.

## ⚠️ ALTIN KURAL: Davranış birebir korunur
Bu bir refactor, feature değil. Her değişiklikten ÖNCE eski ve yeni kod yolunun AYNI sonucu verdiğini kanıtla. En küçük şüphede (layer mask, self-exclusion, max-target sayısı, knockback, crit, status magnitude, ordering) DOKUNMA — o call-site'ı olduğu gibi bırak ve CODEX_DONE'da "SKIPPED: <neden>" diye raporla. Eksik bırakmak, yanlış değiştirmekten iyidir.

## İŞ 1 — AOE damage+status dizisi helper'ı
Tekrar eden pattern: `Physics2D.OverlapCircleAll(...) → her hedef için Health → SkillRuntime.DealDamage → opsiyonel ApplyEffect`.
Bilinen call-site'lar (önce OKU, doğrula):
- Assets/Scripts/Skills/Warblade/WarStomp.cs:35-55
- Assets/Scripts/Skills/Warblade/GravityCleave.cs:37-61
- Assets/Scripts/Skills/Shadowblade/FanOfKnives.cs:25-52
- Assets/Scripts/Skills/Elementalist/Blizzard.cs:67-75
- Assets/Scripts/Skills/Elementalist/Meteor.cs:49-68

Yapılacak:
1. `Assets/Scripts/Skills/SkillRuntime.cs` içine TEK yeni helper ekle — mevcut `EnemiesInCircle` + `DealDamage` + ApplyEffect'i kullanan ince bir sarmalayıcı. Önerilen imza (uyarlayabilirsin, ama mevcut DealDamage int-overload davranışını = `bypassStatScaling:true` KORU):
   `public static void DamageCircle(Vector3 origin, float radius, int damage, GameObject attacker, StatusEffectType effect = StatusEffectType.None, float effectDuration = 0f, float effectMagnitude = 0f, ... )`
   Helper'ın hedef toplama + hasar + status uygulama mantığı, çağıran skill'lerin ŞU AN yaptığıyla BİREBİR aynı olmalı.
2. Yukarıdaki 5 skill'i bu helper'a yönlendir — SADECE eğer skill'in döngüsü helper'la birebir eşdeğerse. Skill ekstra bir şey yapıyorsa (knockback, özel filtre, per-target farklı hasar, çoklu status) → o skill'i ATLA, raporla.
3. Helper SADECE basit "circle → damage → tek opsiyonel status" durumunu kapsar. Karmaşık olanı zorlama.

## İŞ 2 — Lokal targeting kopyalarını sil
`SkillRuntime` zaten sağlıyor: `FindNearestEnemy` (:26), `EnemiesInCircle` (:47), `EnemiesInLine` (:61), `EnemiesInCone` (:79). Şu skill'ler kendi lokal kopyalarını kullanıyor:
- Assets/Scripts/Skills/Shadowblade/Backstab.cs:59
- Assets/Scripts/Skills/Shadowblade/Hemorrhage.cs:52
- Assets/Scripts/Skills/Elementalist/ChainLightning.cs:85
- Assets/Scripts/Skills/Elementalist/LivingBomb.cs:83
- Assets/Scripts/Skills/Shadowblade/ShadowStep.cs:52

Yapılacak: her birinin lokal targeting mantığını OKU, ilgili SkillRuntime helper'ıyla eşdeğer mi DİFFLE. Eşdeğerse → SkillRuntime helper'ına yönlendir, lokal kopyayı sil. Eşdeğer DEĞİLSE (farklı layer mask, self-include/exclude, max-target, sıralama) → DOKUNMA, "SKIPPED: <fark>" raporla.

## YAPMA (kapsam dışı — bu task değil)
- DamagePacket bypassStatScaling / DOT'un Health.TakeDamage direkt çağrısını DEĞİŞTİRME (telemetri/scaling bozulur).
- Zaten paylaşılan StatusEffectSystem.ApplyEffect'i çağıran 45 call-site'ı bir wrapper'a sarma — bu churn, YAPMA.
- SkillRecipe SO / RecipeSkill / yeni ScriptableObject — bu POST-demo, bu task DEĞİL.
- Passive_StatThreshold — ayrı task (Step 2), bu task DEĞİL.
- Class-signature ultimate / boss / Echo skill'lerine dokunma.

## Doğrulama gate'i (commit ÖNCESİ zorunlu)
1. Proje derlenmeli (compile error YOK).
2. Edit-mode testleri çalıştır — özellikle HealthTests + status effect testleri. Hepsi yeşil olmalı.
3. Yeşil DEĞİLSE → commit ETME, CODEX_DONE'da BLOCKED + hata çıktısı yaz.
4. Yeşilse commit et: `refactor(skills): consolidate AOE damage helper + redirect local targeting to SkillRuntime [no behavior change]`.

## CODEX_DONE'a yaz
- İŞ 1: hangi 5 skill redirect edildi / hangileri SKIPPED (neden).
- İŞ 2: hangi skill redirect edildi / hangileri SKIPPED (neden).
- Eklenen helper imzası.
- Test sonucu (kaç test, pass/fail).
- Silinen satır sayısı (DRY kazancı).
