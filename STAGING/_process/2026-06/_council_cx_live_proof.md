ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Golden-path "film-proof" item'larını VIDEO KAYDI OLMADAN runtime'da kanıtlamak için kesin codebase reçetesi (cx feasibility lens). ANALYSIS ONLY, kod değiştirme. Kullanıcı video kaydı alamıyor → her şey execute_code data-proof ile kanıtlanmalı.

## OKU
1. Assets/Scripts/UI/DirectorMode.cs (stat slider + spawn + telemetry *ForValidation hook'ları, ~L575-596, L1687, L2078-2464)
2. Assets/Scripts/Balance/DamageCalculator.cs (physPower/100 scaling)
3. Assets/Scripts/Core/RewardPickup.cs (G-collect: Update L62-68, OnTriggerEnter2D L70-76, playerInRange)
4. Assets/Scripts/Skills/SkillRuntime.cs (DealDamage / telemetry kaydı, ~L193)
5. Bildiğim hook'lar: DirectorMode.SetStatForValidation(string,float)·SelectFirstSpawnEnemyForValidation()·SpawnSelectedEnemyAtForValidation(Vector2)·DirectorSpawnedEnemyCountForValidation()·AssignBasicAttackButtonsForValidation()·ExportTelemetryCsvForValidation()·TelemetrySourceDamageForValidation(DamageSourceType)·TelemetryEventCountForValidation()

## SORULAR (execute_code ile çağrılabilir KESİN reçete, file:line)
**A) stat→damage CANLI:** `_Arena`-direct play'de adım-adım: physPower'ı gerçek slider yolundan set et (SetStatForValidation?), düşman spawn et, **bir LIVE basic attack (LMB) tetikle** ve enemy'nin aldığı GERÇEK hasarı oku. EN KRİTİK: basic attack'i execute_code'dan nasıl tetiklerim (player controller / SkillRuntime / hangi metod)? Hasarı nereden okurum (enemy Health delta / telemetry TelemetrySourceDamage)? physPower=50 vs 250'de hasar oranı 5x mı?
**B) Telemetry CSV:** `ExportTelemetryCsvForValidation()` doğru hook mu? Anlamlı CSV için önce hangi event'ler üretilmeli (combat hit)? Çıktı formatı ne?
**C) G-collect trigger:** Gerçek G-collect path'ini (ForceCollect bypass DEĞİL) runtime'da nasıl kanıtlarım — playerInRange'i gerçekten tetikleyip (player'ı reward trigger'ına sok) collect'i doğrula? G keypress simülasyonu mümkün mü, yoksa OnTriggerEnter2D + Update path'ini mi doğrulamalı?

Çıktı: CODEX_DONE.md, her item için execute_code-ready adımlar + file:line. ≤ kısa.
