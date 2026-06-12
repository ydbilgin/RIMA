ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ A — Jersey10 TMP font + Stat çekirdeği (damage taksonomisi dahil) + wiring. GATE: run_tests CombatContract yeşil → commit. Görsel iş YOK (saf kod/asset).

# OKU (zorunlu, sırayla)
1. `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md` — DamageType/ElementTag/renk/resist kararı (CANON, buna uy)
2. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` — 10 class KİLİTLİ stat değerleri (Phys/AP/HP/moveSpeed/atkSpeed)
3. `STAGING/DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md` §6 FAZ A + §8.4 (stat renk taksonomisi)
4. Handoff kod taslakları (ADAPTE et, namespace RIMA.Balance, Assets'e taşı):
   `STAGING/_process/2026-06/chatgpt_class_handoff/RIMA_class_stats_claude_handoff/code_snippets/` →
   DamageType.cs · DamageCalculator.cs · ClassStatProfile.cs · ClassStatRuntime.cs · ClassStatDatabase.cs · SkillRuntime_DamagePacket_IntegrationExample.cs · BasicAttackProfile_MigrationExample.cs
5. Mevcut kodu DOĞRULA (Grep/Read): `PlayerStats` (maxHP alanı), `Health.cs`, `PlayerClassManager.cs`, `PlayerAttack.cs`/`BasicAttackProfile`, `SkillRuntime.cs` DealDamage imzaları, `ClassType` enum nerede.

# İŞ

## A1 — Jersey 10 TMP font
- Jersey 10 TTF projede YOK. Google Fonts'tan (OFL lisans, yasal) indir → `Assets/Fonts/Jersey10/`.
- TMP SDF asset üret (TMP Font Asset Creator ayarları veya UnityMCP). Türkçe glyph (İ,ı,ş,ğ,ç,ö,ü) ATLAS'ta OLMALI — karakter setini doğrula.
- Lisans dosyası (OFL.txt) ekle.

## A2 — Stat çekirdeği + Damage taksonomisi (Assets/Scripts/Balance/)
- `enum DamageType { Physical, Ability, True }` (DEĞİŞMEZ).
- `enum ElementTag { None, Fire, Frost, Lightning, Void, Light, Poison }` (YENİ). Formüle GİRMEZ.
- `DamagePacket` struct: baseDamage, damageType, **elementTag (default None)**, crit bayrağı/çarpanı. Mevcut DealDamage çağıranları KIRMA (default elementTag).
- `DamageCalculator.Calculate`: mevcut physPower/abilityPower switch AYNEN. SONUNA savunma adımı ekle:
  - Physical → `armor` azaltır · Ability → `magicResist` azaltır · True → atla. Azaltma formülü `r/(r+K)` (K sabit, ör. 100). ElementTag formüle girmez.
- `ClassStatRuntime`: mevcut alanlar + `armor`, `magicResist`.
- `ClassStatProfile` + `ClassStatDatabase` + **10 class asset** (SANDBOX_DIRECTOR KİLİTLİ değerleri — Phys/AP/HP/moveSpeed/atkSpeed). Asset path: `Assets/.../Balance/Classes/`.
- `DealDamage(DamagePacket)` overload. Mevcut `DealDamage(int)` → nötr packet'e map (Physical, None, base=int). Mevcut çağıranlar bozulmaz.
- `DamageColors` statik harita: DamageType/ElementTag → Color (karar dokümanı renkleri — Lightning #FFE600, Crit #FFD24A). UI katmanı tüketir; math'e dokunmaz.
- **ERTELE (yapma):** per-element resist tablosu, ElementalMatrixSO, status-effect (burn/freeze) mekaniği. Sadece ElementTag kancası dursun.

## A3 — Wiring
- `PlayerClassManager`: seçili class → `ClassStatRuntime` üret → **HP `PlayerStats.maxHP`'ye** (Health.cs DEĞİL) · moveSpeed · atkSpeed uygula. `atkSpeedMult` SADECE cooldown'a etki eder (hareket/anim değil).
- 2 basic-attack (Warblade + ikinci class) damage girişini DamagePacket'e bağla.

# KRİTİK NOTLAR (CURRENT_STATUS'tan)
- HP → `PlayerStats.maxHP`, Health.cs değil. · atkSpeedMult sadece cooldown. · `DealDamage(int)` silme → nötr packet map. · `SpawnEnemy` imzasına dokunma. · timeScale konusu Faz B (burada yok).

# GATE + COMMIT
- `run_tests` (CombatContract) çalıştır → YEŞİL olmalı. Kırmızıysa DÜZELT, geçmiyorsa CODEX_DONE'a BLOCKED + sebep yaz, COMMIT ETME.
- Yeşilse: `read_console` 0 error doğrula → commit: `feat(balance): Faz A stat core + damage taxonomy (DamageType/ElementTag/armor/MR) + Jersey10 TMP + 10 class assets + wiring`.
- CODEX_DONE.md'ye yaz: ne yapıldı, hangi dosyalar, run_tests sonucu, kırılan/eklenen test, commit hash, BLOCKED varsa neden.

# YAPMA
- Director/uGUI/görsel iş YOK (Faz B). Per-element resist/status YOK. Spekülatif abstraction YOK. Sadece listelenen dosyalar.
