# LIVE-TEST RECIPE — Beat: stat-damage (LMB lineer, Q/E/R/F stat-deaf)

> Tez: phys stat artinca SADECE LMB temel saldiri hasari LINEER artar; Q/E/R/F = `bypassStatScaling` → slider'a SAGIR (koreografi).
> Kod-anchor (Read ile dogrulandi): `DamageCalculator.cs:30-38` (`physPower/100f`, bypass→1f) · `BasicAttackBehaviorBase.cs:70-79` (LMB packet, bypass=false=SCALED) · `SkillRuntime.cs:120,141` (int-overload bypass=true) · `DirectorMode.cs:537 SetStatForValidation`, `:595 TelemetrySourceDamageForValidation`.

## LAUNCH (dev-direct)
- Sahne: `Assets/Scenes/_Arena.unity` ac (full-flow MainMenu DEGIL — RIMA-002: orada F2/backquote/Director OLU, stat tooling kurulmaz).
- Play'e bas. Warblade spawn (Player tag), PlayerController+PlayerAttack enabled, Console temiz olmali.
- maxHP slider'INA DOKUNMA (HP gorsel yalan — ayri bug).

## ADIMLAR (gercek input — bypass YOK)
1. Backquote(`) ya da F2 ile Director overlay ac → Stats sekmesi.
2. Bir dusman spawn et: Spawn sekmesi → enemy sec → arenada BOS bir noktaya tikla (overlay disi). (Director raycast riski: net bos alana tikla.)
3. Stats: **physPower = 50** ayarla. Test mode'a don (overlay'i kapat / timeScale=1 — pause'da hasar islemez).
4. Dusmana yaklas, **SADECE LMB** (sol tik) ile TAM 1 combo-adim vur. Dusen HP'yi not et (= D1).
5. Quick Reset / yeni dusman + ayni baslangic HP. physPower = 250 yap, tekrar Test mode.
6. Ayni LMB combo-adimi ile vur. Dusen HP'yi not et (= D2). BEKLE: D2 ≈ 5 × D1 (250/50).
7. KONTROL (negatif kol): physPower 250'de KAL, dusmana **Q (veya E/R/F)** ile vur → hasar DEGISMEZ (phys50 ile ayni). Bu beklenen "stat-deaf"tir.

## BEKLENEN (somut)
- LMB hasar phys50 → phys250 arasi LINEER 5× artar (statMultiplier = physPower/100: 0.5 → 2.5).
- Q/E/R/F hasari phys degerinden bagimsiz SABIT kalir (bypassStatScaling=true → statMultiplier=1f).

## DATA-PROOF (execute_code — overlay UI screenshot'a CIKMAZ, ZORUNLU)
Iki katman, ikisi de input-sonrasi okunur:
1. **Math-proof (saf, hizli):** her ayarda
   `PlayerClassManager.Instance.CurrentPrimaryStats.physPower` oku → 50 / 250 oldugunu dogrula.
   `DirectorMode.Instance.SetStatForValidation("physPower", 250f)` true donmeli (slider gercek baglandi).
2. **Real-hit-proof (gercek hasar):** dusman GameObject'in `Health` (`Assets/Scripts/Core/Health.cs`) → `CurrentHP` (`:15`) field'ini vurustan ONCE ve SONRA oku; delta = uygulanan hasar.
   - LMB telemetri kanit: `DirectorMode.Instance.TelemetrySourceDamageForValidation(<LMB DamageSourceType>)` phys250'de phys50'nin ~5×'i.
   - Q/E/R/F kanit: skill DamageSourceType total'i physPower degisince DEGISMEZ.
- HUD/Director/panel/popup elemanlari icin TEK gecerli kanit = bu field okumalari (activeInHierarchy + component state), screenshot DEGIL.

## SCREENSHOT
- DUNYA: `scene_view` capture (Warblade + dusman + arena) — 9.7.3 game-view ScreenCapture fix'i denenebilir; patlarsa scene_view kullan.
- Director overlay / HUD / hasar popup = OVERLAY → screenshot YOK, **DATA-PROOF kullan** (yukaridaki field okumalari).

## PASS / FAIL (olculebilir)
- PASS: D2/D1 oran ∈ [4.5, 5.5] (rounding+min-1 floor toleransi) VE Q/E/R/F deltasi phys50↔phys250 arasi degismez (±0, sadece Mathf.Max(1,...) floor toleransi).
- FAIL: LMB oran ~1× (slider islememis) VEYA Q/E/R/F oran ~5× (yanlis overload, bypass kirilmis).

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN nasil olusur)
- 🪤 **Beat'i Q/E/R/F ile test etmek:** 52/54 yetenek `bypassStatScaling:true` → slider'a sagir; hasar degismeyince "stat sistemi bozuk" yanlis-RED, ya da yanlis skill'le test edip "calismiyor" demek. KURAL: tez ispatini SADECE LMB ile yap.
- 🪤 **DealDamage(int) / DealDamageRaw'i dogrudan cagirip "hasar olctum" demek:** int-overload bypass=true paketler (`SkillRuntime.cs:120,141`) → DamageCalculator stat'i atlar; gercek LMB akisi DEGIL → yanlis-RED. GERCEK input = sahnede sol-tik combo.
- 🪤 **debugGlobalDamageMult ile kanitlamak:** bu carpan bypass'tan BAGIMSIZ HER pakete isler (`DamageCalculator.cs:45,52`) → physPower degismeden hasar oynar; "phys lineer" tezini ISPATLAMAZ, maskeleyebilir. Bu beat'te debugMult'a DOKUNMA (slider'i 1'de tut).
- 🪤 **timeScale=0 / pause'da olcmek:** Director pause'da vuruş islemez → 0 delta = yanlis-FAIL. Olcumu Test mode'da (timeScale=1) yap.
- 🪤 **maxHP slider oynatmak:** HP gorsel yalan + min-1 floor (`Health.cs`) → kucuk hasarlar 1'e clamp → oran bozulur. maxHP'ye dokunma; dusman tam can ile basla.
- 🪤 **SetStatForValidation donus degerini gormezden gelmek:** false donerse slider baglanmamis (overlay kurulmamis / yanlis key) → stat hic degismedi, sonraki olcum anlamsiz. Once true teyit et.
