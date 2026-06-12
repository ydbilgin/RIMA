ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ChatGPT'nin overnight-run correctness review'ündeki önerilen minimal fix'lerin RİSK/REUSE/FEASIBILITY denetimi (kod lensi). ANALYSIS ONLY — kod değiştirme.

## Bağlam
ChatGPT comparative review yaptı. Claude bulguları gerçek kodla DOĞRULADI (hepsi gerçek). Şimdi önerilen fix'lerin riskini ve RIMA'da zaten var olan altyapıyı denetliyoruz.

READ these source files (repo kökünden):
- STAGING/_inbox/chatgpt_overnight_review_2026-06-12/rima_overnight_review_claude_package/01_FINDINGS_DETAILED.md
- STAGING/_inbox/chatgpt_overnight_review_2026-06-12/rima_overnight_review_claude_package/02_PATCH_PLAN_BY_FILE.md
- Assets/Scripts/Balance/DamageCalculator.cs
- Assets/Scripts/Balance/DamagePacket.cs
- Assets/Scripts/Balance/ClassStatRuntime.cs
- Assets/Scripts/Skills/SkillRuntime.cs
- Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs
- Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs
- Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs
- Assets/Scripts/Core/Health.cs
- Assets/Scripts/Systems/PlayerClassManager.cs
- Assets/Tests/EditMode/ (CombatContract + damage ile ilgili suite'leri grep et)

## DOĞRULANMIŞ BULGULAR (tekrar audit ETME, risk denetle)
- A1: BasicAttackBehaviorBase.cs `isFinisher` → DamagePacket.Create 7. arg = isCrit → DamageCalculator critMult 1.5x. Fix: isCrit:false, sourceId "basic_lmb_finisher".
- A2: SkillRuntime.DealDamage `Calculate(packet, attackerStats)` defenderStats geçmiyor. Fix: ResolveCombatStats(packet.attacker/target) + ICombatStatsProvider interface.
- A3: ShotCadenceBehavior (Ranger) SpawnProjectile ham damage, SetDamagePacket yok. Fix: projectile.SetDamagePacket(...).
- B1: DamageCalculator finalDamage Mathf.Max(1,...). Fix: baseDamage<=0 ? 0 : Max(1,...).
- B2: postureOverflowDamage hesaplanıp dönüyor, tüketen yok.

## SORULAR (feasibility/reuse lensi, madde madde cevapla, CODEX_DONE.md'ye yaz)
1. **Test kırılma riski:** CombatContract ve damage edit-mode test'lerini grep et. B1 (zero-damage no-op) `Mathf.Max(1)` davranışına BAĞLI mevcut bir assert var mı? A1 (finisher crit kaldırma) finisher damage'ine bağlı bir test var mı? Hangi fix hangi test'i kırar — net listele.
2. **A2 hook reuse:** RIMA'da zaten bir defender-stat / ICombatStatsProvider benzeri arayüz, ya da enemy/Health üzerinde armor/MR taşıyan bir alan VAR MI? ICombatStatsProvider sıfırdan mı, yoksa mevcut bir şeye bağlanır mı? TryGetComponent<interface> bu Unity sürümünde güvenli mi? Kapsam şişmesi riski?
3. **A1 yan etki:** Finisher artık 1.5x almayınca, comboDamage[final] değeri zaten yüksek mi (yani finisher gücü orada mı tanımlı) yoksa finisher tamamen crit'e mi bağımlıydı? BasicAttackProfile comboDamage dizisine bak. Rebalance gerekir mi?
4. **A3 minimal yol:** Ranger fix'i ShotCadenceBehavior içinde SetDamagePacket mi, yoksa SkillRuntime.SpawnProjectile'a packet-alan overload mu daha az çağrıyı etkiler? Kaç çağıran var SpawnProjectile'ın?
5. **Güvenli uygulama sırası** öner (en düşük riskten en yükseğe).
6. ChatGPT'nin kaçırdığı, bu 5 dosyada gözüne çarpan başka bir correctness riski var mı?

Do NOT reproduce a full audit. Kısa, kanıtlı (dosya:satır), madde madde.
