ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA'nın mevcut skill mimarisini gerçek kodla okuyup, video'daki "modular ability (AIMING/DELIVERY/SHAPE/EFFECT) recipe" felsefesinin RIMA'ya FEASIBILITY + REUSE açısından ne kadar oturduğunu değerlendir. ANALYSIS ONLY — kod değişikliği YOK. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam: Video'nun anlattığı modular design
Bir tower-defense roguelite geliştiricisi (Feed the Overlord) 60+ ability için her spell'i ayrı kod yazmak yerine küçük yeniden-kullanılabilir modüllerden kuruyor. Spell 4 modüle ayrılıyor:
- **AIMING**: ne hedefleniyor (closest enemy / most-health / caster / crew member)
- **DELIVERY**: hedefe nasıl ulaşıyor (projectile / instant / drop-from-above / lingering field / chain)
- **SHAPE**: etki alanı (single / small circle / large circle / line / cone)
- **EFFECT**: ne yapıyor (damage / heal / burn / chill / poison / knockback / buff / debuff veya kombinasyon)

Spell = "recipe". Örn Frost Orb = closest + projectile + single + [damage, chill, bonus-vs-chill-stacks]. Hailstorm = closest + ticking-field + large-circle + [chill, damage-per-tick]. Nova = caster + instant + circle-around-caster + [damage-all, bonus-DoT-if-status].
Passive'ler de modüler: TRIGGER (on-attack) + REACTION (apply-poison) + TARGETING-LOGIC (the-attacked-enemy) + opsiyonel CONDITION (max every 5s).
Avantaj: yeni spell = sıfırdan değil, parça swap. Uyarı: her şeyi modülerleştirme; prototipte modülerleştirme.

## RIMA'nın MEVCUT durumu (orchestrator tespit etti — sen KODLA DOĞRULA)
- `Assets/Scripts/Skills/SkillData.cs`: ScriptableObject ama SADECE metadata (skillName, tier, icon, damage, cooldown, tags[], classType, isPassive, appliesEffect, skillType:System.Type). Kompozisyon YOK.
- `Assets/Scripts/Skills/Base/SkillBase.cs`: abstract MonoBehaviour, `abstract Execute()`. Her skill KENDİ bespoke subclass'ı (RoninQuickdraw, RoninIaidoStance, Elementalist_*, Shadowblade_*, Ranger_* ...). Echo origin/aim override mekanizması var.
- `Assets/Scripts/Balance/DamagePacket.cs`: struct — baseDamage, damageType, elementTag, sourceType, attacker, target, isCrit, sourceId. Damage taksonomisi + CombatContract mevcut.
- Yani RIMA ŞU AN video'nun uyardığı "bespoke blob per skill" pattern'inde.

## Soruların (her birine RIMA-spesifik, somut cevap)
1. **Kaç skill subclass var şu an?** `Assets/Scripts/Skills/**` ve class controller'ları tara, mevcut bespoke skill class sayısını ve ortak tekrar eden pattern'leri (projectile spawn, AOE circle, status-apply) listele. Hangi kod gerçekten tekrar ediyor (DRY ihlali) — yani modülerleştirmenin REUSE kazancı nerede somut?
2. **REUSE vs BUILD:** Halihazırda var olan yeniden-kullanılabilir parçalar neler (PlayerProjectile, DamagePacket, StatusEffect uygulama, targeting helper'ları)? Yani modüler framework'ün YARISI zaten yazılmış mı? Sıfırdan SO-composition mı gerekir yoksa mevcut parçaları bir "SkillRecipe" SO'su altında toplamak mı yeter?
3. **CombatContract / DamagePacket çakışması:** EFFECT modülü DamagePacket üretecek. Mevcut CombatContract gate'i (zero-damage, finisher, packet bypass kuralları) modüler EFFECT'lerle çakışır mı? Nerede risk var?
4. **Feasibility verdict:** RIMA'nın bu noktasında (skill-heavy, ~X class, prototip DEĞİL, demo aşaması) tam modüler refactor mı, yoksa SADECE yeni/tekrar eden skill'ler için opt-in recipe katmanı mı? Hangisi daha az kod + daha az risk? Mevcut bespoke skill'leri migrate etmenin maliyeti nedir?

Lens: feasibility / what-already-exists-in-RIMA / reuse-vs-build. Somut dosya adı + satır referansı ver. Prior audit'i tekrar ÜRETME. Sonucu CODEX_DONE.md'ye yaz.
