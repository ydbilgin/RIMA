RIMA class stat sistemi için bu paketi oku ve repo içinde uygulama planı çıkar.

Kritik karar: tek `damageMult` production stat değil. Phys/AP split korunacak. `damageMult` sadece debug override olabilir.

Yeni omurga:
- `ClassStatProfile` ScriptableObject
- `ClassStatRuntime` runtime copy
- `DamagePacket`
- `DamageCalculator`
- `DamageType`: Physical / Ability / True
- `DamageSourceType`: LMB / RMB / Skill / DoT / Minion / Item

Stats:
- maxHP
- physPower
- abilityPower
- attackSpeedMult
- moveSpeed
- UI 5-bar: damage/durability/speed/control/difficulty

Önce şu dosyaları oku:
- README.md
- docs/01_A_DECISION_PHYS_AP_VS_DAMAGE_MULT.md
- docs/02_B_CLASS_NUMERIC_TABLE.md
- docs/03_C_BALANCE_DEBUG_TOOLS.md
- docs/04_IMPLEMENTATION_BACKLOG.md
- data/class_stats_v01.json
- code_snippets/*.cs

Sonra Unity repo tarafında şu dosyaları incele:
- SkillData.cs
- BasicAttackProfile.cs
- Health.cs
- PlayerController.cs
- SkillRuntime.cs
- PlayerClassManager.cs

Önce kod yazma. İlk cevap olarak implementation plan çıkar:
1. hangi dosyalar değişecek
2. hangi yeni dosyalar eklenecek
3. eski damage çağrıları nasıl kırılmadan migrate edilecek
4. BasicAttackProfile asset'leri nasıl damage type alacak
5. PlayerClassManager seçilen class statlerini nasıl uygulayacak
6. debug slider + telemetry nasıl bağlanacak

Sonra aşama aşama implement et.
