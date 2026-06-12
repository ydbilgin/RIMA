# Council question — RIMA overnight review fix (LEAN / ship-fast / over-engineering critique lens)

Sen "en yalın yol + over-engineering eleştirisi" lensisin. RIMA = Unity 2D top-down ARPG roguelite, DEMO aşaması. ChatGPT bir correctness review yaptı, Claude gerçek kodla doğruladı. Senden bulguları doğrulamanı DEĞİL, önerilen fix'lerin AŞIRI MÜHENDİSLİK içerip içermediğini ve en yalın güvenli yolu söylemeni istiyorum.

READ:
- STAGING/_inbox/chatgpt_overnight_review_2026-06-12/rima_overnight_review_claude_package/02_PATCH_PLAN_BY_FILE.md
- Assets/Scripts/Skills/SkillRuntime.cs
- Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs
- Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs
- Assets/Scripts/Balance/DamageCalculator.cs

## DOĞRULANMIŞ BULGULAR
A1 finisher yanlış crit (1.5x). A2 defenderStats geçilmiyor. A3 Ranger projectile packet bypass. B1 zero-damage 1 vuruyor. B2 postureOverflow ölü.

## SORULAR (madde madde, kısa, acımasız-yalın)
1. **A2 over-engineering mi?** ChatGPT `ICombatStatsProvider` interface + ResolveCombatStats öneriyor. DEMO için bu fazla mı? En yalın hali ne — sadece `Calculate(packet, attacker, defender)` çağrısına target'tan stat geçmek yeterli mi, yoksa interface şart mı? Enemy'lerde stat yoksa Neutral yeterli mi?
2. **A1 en küçük diff:** tek satır `isCrit:false` yeterli mi, sourceId değişikliği şart mı?
3. **B1 en yalın:** Health.TakeDamage(0) no-op tek satır mı, yoksa DamageCalculator + SkillRuntime iki yerde mi? Hangisi minimum?
4. **Hangi fix'leri ŞİMDİ yapma:** Bu 5 bulgudan hangisi demo için ertelenebilir / gereksiz? (örn. B2 postureOverflow sadece TODO mu?)
5. **En yalın güvenli uygulama sırası** + toplam kaç satır diff tahmini.
6. Bu fix paketinde "demo'yu geç, sonra yap" diyeceğin bir şey var mı?

Türkçe yaz (tam Türkçe karakter zorunlu: ı ğ ş ç ö ü). Kısa, yalın, madde madde.
