ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Faz A regression fix. `DealDamage(int)` legacy çağrıları NÖTR olmalı = stat-scaling UYGULANMAMALI. Şu an int-overload `DamageType.Physical` map'liyor → physPower/100 ölçeği uygulanıyor → Elementalist büyüleri (physPower 65) eski flat hasarın ~%65'ine düşüyor. Karar dokümanı "nötr packet" diyor → davranış DEĞİŞMEMELİ.

# Kanıt (rima-qc bulgusu)
- `Assets/Scripts/Skills/SkillRuntime.cs` int overload'ları → `DamagePacket` (Physical, ElementTag.None) → `DamageCalculator.Calculate` → `statMultiplier = physPower/100`.
- Sonuç: eski flat int hasar artık aktif class physPower'ı ile çarpılıyor. Migrate edilmemiş tüm skill'ler (Elementalist/Ranger/Shadowblade/Ronin int çağrıları) etkileniyor.

# Çözüm (surgical, min-code)
1. `DamagePacket`'e `bool bypassStatScaling` alanı ekle (default false).
2. `SkillRuntime.cs` int-tabanlı `DealDamage(...int...)` overload'ları → oluşturduğu nötr packet'te `bypassStatScaling = true`.
3. `DamageCalculator.Calculate`: `bypassStatScaling == true` ise `statMultiplier = 1f` (physPower/abilityPower atla). Savunma (armor/MR) adımı AYNEN kalsın (nötr=eski davranış; armor şu an 0 zaten). True bypass mantığına dokunma.
4. Faz A'da AÇIKÇA migrate edilen basic-attack packet çağrıları (Warblade/Elementalist BasicAttackProfile) `bypassStatScaling = false` KALIR — onlar ölçeklenmeye devam eder (doğru DamageType ile).

# DOĞRULAMA
- `run_tests` CombatContract → PASS (3/3) kalmalı.
- Grep: int overload çağıranların hiçbiri kırılmamalı (imza değişmiyor, sadece iç packet flag).
- `read_console` 0 error.
- Mantık testi (yorum/manuel): Elementalist int-skill hasarı fix öncesi×(100/65) = fix sonrası (eski flat'e döndü) olduğunu açıkla.

# COMMIT
- Geçerse: `fix(balance): int DealDamage neutral — bypass stat scaling for unmigrated callers`
- CODEX_DONE.md: değişen dosyalar, run_tests sonucu, hangi çağrılar etkilendi, commit hash. BLOCKED varsa COMMIT ETME.

# YAPMA
- Sadece bu 3 dosya (DamagePacket / SkillRuntime / DamageCalculator). Basic-attack packet path'ine dokunma. Yeni abstraction YOK. armor/MR/True mantığını değiştirme.
