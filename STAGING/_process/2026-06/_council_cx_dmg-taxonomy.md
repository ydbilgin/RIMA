ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA damage type taksonomisi — FEASIBILITY/REUSE lens. Mevcut kod zemininden hangi enum/sistem genişlemesi en az kodla, en az refactor'la yapılır? ANALİZ ONLY, kod değiştirme.

# Bağlam (oku)
- Mevcut enum: `STAGING/_process/2026-06/chatgpt_class_handoff/RIMA_class_stats_claude_handoff/code_snippets/DamageType.cs` → `enum DamageType { Physical, Ability, True }`
- Mevcut calc: `.../code_snippets/DamageCalculator.cs` → physPower/abilityPower çarpanı, resist/armor UYGULANMIYOR (placeholder posture overflow var)
- `.../code_snippets/ClassStatProfile.cs`, `ClassStatRuntime.cs`, `ClassStatDatabase.cs` → 10 class stat modeli
- Canon (NLM): 2 ana stat = Physical Damage + Ability Power (AP). Karar: Phys/AP KİLİTLİ, üstüne ekleme.
- 10 class var, Elementalist element-switch yapıyor (fire/frost/lightning).

# Sorular (feasibility/reuse lens — kod açısından)
1. **AP elemental nasıl modellenmeli?** İki seçenek: (A) flat enum genişlet — `Physical, Ability, Fire, Frost, Lightning, Void, Light, True` (tek eksen) VS (B) iki eksen — `DamageType {Physical, Ability, True}` + ayrı `ElementTag {None, Fire, Frost, Lightning, Void, Light}` (Ability'nin alt-etiketi). Hangisi mevcut DamageCalculator.switch + ClassStatRuntime ile en az refactor? Elementalist switch'i hangisinde daha temiz?
2. **Resist/armor sistemi** kodda yok. Eklemek için: ClassStatRuntime'a hangi alanlar (armor flat? resistByType dict/array?), DamageCalculator.Calculate'e nereye giriş? En yalın imza ne? `DealDamage(int)` nötr-map ile çakışır mı?
3. **DamagePacket** struct'ı hangi alanlara genişlemeli (damageType + elementTag + ignoreResist bool)? Mevcut DamagePacket kullanıcılarını (SkillRuntime, PlayerAttack, mob attacks) kırar mı?
4. **Damage-number renk** — DamageType→renk eşlemesi runtime'da nerede (enum→Color tablosu)? Mevcut floating damage text sistemi var mı (Grep), reuse mu yeni mi?
5. Tahmini iş büyüklüğü: enum+calc+packet+resist+renk için kaç dosya, hangi mevcut testler kırılır (CombatContract)?

CODEX_DONE.md'ye yaz. Kod değiştirme. Mevcut audit'i tekrar üretme.
