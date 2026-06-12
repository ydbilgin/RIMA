# Council question — RIMA overnight review fix RİSK denetimi (DEEP / architecture lens)

Sen mimari/tasarım derinlik lensisin. RIMA = Unity 2D top-down ARPG roguelite. ChatGPT bir correctness review yaptı; Claude bulguları gerçek kodla DOĞRULADI. Senden bulguları tekrar doğrulamanı DEĞİL, önerilen fix'lerin mimari riskini denetlemeni istiyorum.

READ these files (repo kökü = bu dizin):
- STAGING/_inbox/chatgpt_overnight_review_2026-06-12/rima_overnight_review_claude_package/01_FINDINGS_DETAILED.md
- STAGING/_inbox/chatgpt_overnight_review_2026-06-12/rima_overnight_review_claude_package/02_PATCH_PLAN_BY_FILE.md
- Assets/Scripts/Balance/DamageCalculator.cs
- Assets/Scripts/Balance/DamagePacket.cs
- Assets/Scripts/Skills/SkillRuntime.cs
- Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs
- Assets/Scripts/Core/Health.cs
- Assets/Scripts/Player/PlayerStats.cs
- Assets/Scripts/Systems/PlayerClassManager.cs

## DOĞRULANMIŞ BULGULAR (tekrar audit etme)
- A1 finisher yanlışlıkla crit (1.5x). A2 defenderStats geçilmiyor (armor/MR ölü). A3 Ranger projectile packet bypass. B1 zero-damage 1 vuruyor. B2 postureOverflow tüketilmiyor. B3 PlayerStats vs Health çift HP otoritesi.

## SORULAR (madde madde, kısa)
1. **A2 mimari:** `ICombatStatsProvider` + `ResolveCombatStats(packet.attacker/target)` doğru soyutlama mı, yoksa over-engineering mi? Defender stat çözümünü Health'e mi yoksa ayrı provider'a mı bağlamalı? Demo kapsamında en sağlam ama minimal mimari ne?
2. **B1 risk:** Zero-damage'i no-op yapmanın combat event/telemetry/status-effect zincirine mimari etkisi? "blocked/immune hit" feedback'i ileride gerekirse bu fix onu engeller mi? Health.TakeDamage(0) no-op vs SkillRuntime guard — hangisi daha doğru katman?
3. **A1 denge:** Finisher crit'ten çıkınca combo kimliği zayıflar mı? comboDamage dizisi finisher gücünü zaten taşıyor mu, yoksa ayrı bir finisher-multiplier mı gerek? (mimari olarak crit ≠ finisher ayrımını nasıl temiz tutmalı — HitTag/DamageSourceType?)
4. **B3 HP authority:** PlayerStats vs Health çift-HP problemini demo için EN AZ riskle nasıl çözmeli? Tek-otorite (Health) mi, sync-bridge mi, yoksa şimdilik karar-notu+küçük sync mi? Production run sırasında maxHP değişimi refill etmeli mi?
5. **Uygulama sırası** + her fix için "kör commit edilebilir mi yoksa görsel/manuel doğrulama şart mı"?
6. ChatGPT'nin mimari olarak KAÇIRDIĞI bir şey?

Türkçe yaz (tam Türkçe karakter zorunlu: ı ğ ş ç ö ü). Kısa, madde madde, kanıtlı.
