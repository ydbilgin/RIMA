ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Council-onaylı combat-correctness fix'leri uygula (karar: `STAGING/OVERNIGHT_REVIEW_FIX_DECISION_2026-06-12.md`). 5 bulgu CONFIRMED. **Uygula sırası: B2(doc) → A1 → A3 → A2 → B1(son).** A4 (Director raycast) BU TASK DEĞİL (Play-mode-only doğrulama gerek).

## Fix'ler (file:line — karar dosyasından)

### A1 — finisher ≠ crit
`Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs:69-77`: finisher → `DamagePacket.Create` 7. arg `isCrit` veriliyor → `DamageCalculator.cs:46` critMult 1.5x → ~%50 finisher şişmesi.
FIX: finisher packet'inde `isCrit:false` + `sourceId:"basic_lmb_finisher"`. (comboDamage zaten finisher gücünü taşıyor → rebalance YOK. `DifficultyBalanceTests.cs:39-44` Warblade combo=95 no-crit varsayıyor → bu fix runtime'ı teste HİZALAR.)

### A3 — Ranger + HeatGauge packet bypass
Raw `SpawnProjectile` (no SetDamagePacket) → pipeline bypass. Bu task'ta DÜZELT (basic-attack archetype'ları):
- `Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs:53` (Ranger)
- `Assets/Scripts/Combat/BasicAttack/HeatGaugeBehavior.cs:89` + `:130` (Gunslinger dual + hip)
FIX: her call-site'tan SONRA `projectile.SetDamagePacket(...)` ekle (Elementalist `CastRhythmBehavior.cs:81` referans pattern). **Lokal SetDamagePacket per call-site** — SpawnProjectile overload'u YAPMA (shared API, 5 caller). ShadowPin/PinningShot (skill) BU TASK DEĞİL.

### A2 — defender stats (lean, interface-free)
`Assets/Scripts/Skills/SkillRuntime.cs:138-141`: `Calculate(packet, attackerStats)` — defenderStats atlanıyor → armor/MR ölü.
FIX: `SkillRuntime`'a static `ResolveCombatStats(GameObject)` ekle (Player→`PlayerClassManager.CurrentPrimaryStats`, değilse `Neutral`). `Calculate`'a defenderStats geç. Yorum: `// TODO: ICombatStatsProvider when enemies carry stats`. INTERFACE YAZMA (over-engineering, demo).

### B1 — zero-damage no-op (+ TEST FLIP zorunlu)
`Assets/Scripts/Core/Health.cs`: `if (amount <= 0) return;` (DamageCalculator.cs:58 `Mathf.Max(1,...)` 0→1 veriyordu).
**ZORUNLU:** `Assets/Tests/EditMode/HealthTests.cs:62-66` `TakeDamage_ZeroDamage_StillDeals1` assert'ini ÇEVİR → 0 hasarda `CurrentHP` DEĞİŞMEZ (no-op). Test adını da uygunsa güncelle.

### B2 — posture overflow (TODO only)
`Assets/Scripts/Balance/DamageCalculator.cs:60-64` postureOverflowDamage hesaplanıyor, tüketici yok. SADECE `// TODO:` yorumu ekle, sistem yazma.

### E1/E2 — flag (TODO only, fix YOK)
- E1 `Health.cs` `effective = Mathf.Max(1, amount * incomingDamageMultiplier)` → 100% DR hit yine 1 verir. `// TODO E1:` yorumu (no class has 100% DR yet).
- E2 `SkillRuntime.cs` bypassStatScaling crit+cap uygular → `// TODO E2:` yorumu.

## YAPMA
- A4 Director raycast (`DirectorMode.cs`) — BU TASK DEĞİL (Play-mode doğrulama).
- ICombatStatsProvider interface YAZMA.
- SpawnProjectile overload EKLEME (per-call-site SetDamagePacket).
- B3 HP authority — sadece karar notu varsa dokunma, bu task değil.
- İlgisiz refactor YOK.

## Doğrulama (commit ÖNCESİ — ZORUNLU GATE)
1. `read_console` → 0 compile error.
2. **UnityMCP `run_tests` EditMode** (Unity AÇIK → in-editor test ÇALIŞIR, batchmode'un aksine):
   - `CombatContractTests` → PASS (crit/finisher/armor bağı yok, değişmemeli)
   - `HealthTests` → PASS (flipped assert dahil)
   - `DifficultyBalanceTests` → PASS (A1 buna hizalanıyor — Warblade combo=95)
   - Hepsi YEŞİL değilse → commit ETME, BLOCKED + hata çıktısı yaz.
3. Yeşilse commit: `fix(combat): finisher≠crit + ranger/heatgauge packet + defender stats + zero-damage no-op [council-decided]`.

## CODEX_DONE'a yaz
- Her fix file:line + ne değişti.
- run_tests sonucu (kaç test, pass/fail, hangi suite'ler).
- HealthTests flip teyidi.
- SKIPPED/BLOCKED varsa neden.
