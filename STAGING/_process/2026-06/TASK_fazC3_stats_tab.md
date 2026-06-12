ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ C3 — Director "Stats" sekmesini doldur (flagship: stat çekirdeğini canlı sergiler). Boş Stats paneline slider'lar → ClassStatRuntime CANLI düzenleme + Reset/Save/Export. GATE: derleme 0 error + slider→runtime canlı uygulanıyor. Görsel doğruluk SABAHA.

# OKU (zorunlu)
1. `Assets/Scripts/UI/DirectorMode.cs` — Faz B iskelet. Panel yapısı: ContentArea > Stats paneli (CanvasGroup). `DirectorMode.Instance`, `OnTabChanged`, panels dict, jersey10Font + chrome sprite alanları. Stats panelini BURADAN bul/genişlet.
2. `Assets/Scripts/Balance/ClassStatRuntime.cs` — düzenlenecek alanlar (maxHP, physPower, abilityPower, attackSpeedMult, moveSpeed, debugGlobalDamageMult).
3. `Assets/Scripts/Systems/PlayerClassManager.cs` — `CurrentPrimaryStats` (ClassStatRuntime kopyası, Faz A'da wire edildi). Slider bunu düzenleyecek + canlı uygulayacak (SetMaxHP/SetMoveSpeed/attackSpeedMult yolları).
4. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` §4 Stats satırı + §3 chrome eşleme.
5. `STAGING/DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md` §8.4 stat renk taksonomisi (slider etiket renkleri: HP #C82026, Damage #E89020 ember, Speed #00FFCC cyan, Crit #FFD24A, Cooldown #A65BFF mor, Armor #8A9098).
6. `Loc.cs` — `Loc.T()` (TR+EN); etiketler key'le.

# İŞ — Stats sekmesi içeriği
- Stats paneline (mevcut boş CanvasGroup) slider satırları ekle:
  - maxHP · physPower · abilityPower · attackSpeedMult · moveSpeed · debugGlobalDamageMult
  - Her satır: etiket (renk taksonomisi) + slider + canlı sayı değeri (TMP, Jersey10).
- **Canlı uygula:** slider değişince `PlayerClassManager.CurrentPrimaryStats` güncelle + oyuncuya anında yansıt (maxHP→PlayerStats, moveSpeed→PlayerController, attackSpeedMult→PlayerAttack). Faz A wiring yollarını KULLAN, yeni wiring yazma.
- **Reset:** aktif class'ın ClassStatProfile asset değerlerine döndür (CreateRuntimeCopy).
- **Save:** mevcut runtime'ı geçici tut (preset slot, PlayerPrefs veya in-memory — min-code, JSON yeter).
- **Export:** runtime stat'ları JSON string olarak panoya/Debug.Log (cx/Claude test verisi). Dosya yazma opsiyonel.
- Chrome: panel arka plan `minimap_frame`, butonlar `menu_button`, büyük aksiyon (Export) `ribbon_base`. CanvasScaler/font B'den miras.

# GATE + COMMIT
- `read_console` 0 error (domain reload sonrası).
- UnityMCP doğrula: Director aç → Stats sekmesi → slider oynat → `CurrentPrimaryStats` değeri değişiyor + oyuncuya yansıyor (execute_code ile assert). Reset profile değerine dönüyor.
- VARSA `run_tests` yeşil (CombatContract bozulmamalı).
- Geçerse commit: `feat(director): Faz C3 Stats tab — live ClassStatRuntime sliders + Reset/Save/Export [visual unverified]`. Geçmezse BLOCKED, COMMIT ETME.
- CODEX_DONE.md: ne eklendi, canlı-uygula doğrulaması, derleme/test, görsel-bekleyen, commit hash.

# YAPMA
- Diğer 5 sekme (Spawn/Class&Skill/Build/Map/Telemetry) YOK — sadece Stats.
- Yeni stat alanı/abstraction YOK. Faz A wiring'ini tekrar yazma, kullan.
- Spawn/PaintCell/DungeonGraph hook'larına dokunma.
