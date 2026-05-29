ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
  (NOTE: NLM auth may be EXPIRED this session — if it errors, DO NOT block. Use local files instead.)
Direct-read allowed: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# AMAÇ
RIMA roguelite "vertical slice" döngüsünü bağla: 5 oda E2E + her oda TAMAMLANINCA (combat clear VEYA fragment pickup) çalışan **skill seçim ekranı** (gerçek ikonlarla) açılır, oyuncu skill seçince kapı açılır, sonraki odaya geçer. Warblade demo sınıfı.
**YENİ SKILL TASARLAMA.** Var olan 17 Warblade skill + 19 ikon zaten mevcut — sadece OYUNA BAĞLA.

# KISITLAR
- SADECE .cs + gerekli yeni ScriptableObject/Editor script + Resources baking. **SAHNE (.unity) DÜZENLEME YAPMA** — orchestrator (Claude) verify + scene-wire yapacak. İstisna: Part B'de ShowDraft'ı koda-dayanıklı yap ki sahne ref'i gerekmesin.
- Unity AÇIK olmalı (UnityMCP stdio config'de). Erişilemezse: code yaz, compile/play-verify'ı "DEFERRED" işaretle (BLOCKED DEĞİL).
- Her .cs değişiminden sonra `read_console` → 0 error görmeden devam etme.
- Karpathy: en az kod, spekülatif feature yok, sadece listelenen dosyalar.

# KAYNAK HARITASI (Explore ile doğrulandı — file:line)
- Skill SO: `Assets/Scripts/Skills/SkillData.cs` (alanlar: skillName, icon, tier, classType, isPassive...). `icon` alanı runtime'da BOŞ kalıyor.
- Runtime registry: `Assets/Scripts/Skills/SkillDatabase.cs` — `Build()` içinde `ScriptableObject.CreateInstance<SkillData>()` ile skilleri kodla yaratıyor; `Add()`/helper'lar (WB/EL/SH/RG/Passive) **`icon` ATAMIYOR**. 17 Warblade skill burada.
- Offer UI: `Assets/Scripts/UI/SkillOfferUI.cs:289-298` — `icon == null` → placeholder kutu (renk). Icon dolu gelince düzgün çizer (UI tarafı DOĞRU, sorun veri).
- Draft orchestrator: `Assets/Scripts/Skills/DraftManager.cs` — `ShowDraft()`, `HandleRoomCleared()` (OnRoomCleared'a 2s auto-timer), `EnsureDependencies()` (offerGenerator/offerUI FindAnyObjectByType + yoksa new GameObject). Sahnede ikisi de null.
- Offer generator: `Assets/Scripts/Skills/SkillOfferGenerator.cs` — 3 offer üretir, `GetPool` retired-skill filtreler.
- Oda yükleyici: `Assets/Scripts/Systems/Map/RoomLoader.cs` — `BuildRoomContent()` (~200), combat: OnRoomCleared→`gate.Unlock()` DİREKT (~282-288); reward: `RewardRoomAutoTrigger` coroutine (~292) fragment pickup→`gate.Unlock()` DİREKT; boss: `WireBossDeathListener`→DemoComplete. `JumpToRoom(int)` test için var.
- İkonlar: `Assets/Sprites/UI/Icons/` 19 PNG. Örn: `Icon_Warblade_IronCharge.png`, `Icon_Warblade_GravityCleave.png`, `Icon_Warblade_WarStomp.png`, `Icon_Warblade_DeathBlow.png`, `Icon_WhirlwindSlash.png`, `Icon_RiftStrike.png`, `Icon_CrushingBlow.png` (+ Elementalist/Shadowblade/Ranger). Skill adları boşluklu ("Iron Charge").

---

# PART A — Skill ikonları runtime'da yüklensin (placeholder bug fix)
**Asset TAŞIMA YOK.** Registry pattern:
1. `Assets/Scripts/Skills/SkillIconRegistry.cs` — yeni `SkillIconRegistry : ScriptableObject`:
   - `[System.Serializable] class Entry { public string key; public Sprite sprite; }`
   - `public List<Entry> entries;`
   - `public Sprite Get(string skillName)` — normalize (boşlukları sil + lower) anahtar eşleşmesi; bulamazsa null.
2. `Assets/Editor/Skills/SkillIconRegistryBuilder.cs` — `[MenuItem("RIMA/Skills/Rebuild Icon Registry")]`:
   - `Assets/Sprites/UI/Icons/*.png` tara (AssetDatabase).
   - Her dosya için key = dosya adından `Icon_` prefiksi + (varsa) sınıf prefiksi (`Warblade_`/`Elementalist_`/`Shadowblade_`/`Ranger_`) soyulup normalize (ör. `Icon_Warblade_IronCharge` → `ironcharge`). Sınıf prefiksi yoksa direkt (`Icon_WhirlwindSlash` → `whirlwindslash`).
   - `Assets/Resources/SkillIconRegistry.asset` oluştur/güncelle, entries doldur. (Resources klasörü yoksa oluştur.)
3. `SkillDatabase.cs` `Build()` başında: `var iconReg = Resources.Load<SkillIconRegistry>("SkillIconRegistry");` — `Add()` (ve passive helper) içinde `d.icon = iconReg != null ? iconReg.Get(d.skillName) : null;` (icon zaten setliyse ezme — yalnız null ise ata).
4. Menüyü ÇALIŞTIR (UnityMCP `execute_menu_item` "RIMA/Skills/Rebuild Icon Registry") → registry bake. Eşleşme oranını raporla: kaç/17 Warblade skill ikon buldu. Eşleşmeyenleri warn-log'la (non-fatal).

# PART B — DraftManager gerçekten açılsın (sahne ref'i olmadan)
`DraftManager.EnsureDependencies()` offerGenerator+offerUI'yi auto-create ediyor mu doğrula:
- EDİYORSA: `ShowDraft()`'in `EnsureDependencies()`'i **her zaman, erken-çıkıştan ÖNCE** çağırdığından emin ol. Gerekirse çağrı sırasını düzelt. Sahne edit YOK.
- ETMİYORSA: `ShowDraft()` içine lazy-ensure ekle.
Hedef: sahnede offerGenerator/offerUI atanmamış olsa bile `ShowDraft()` 3 kartı (gerçek ikonlu) çizer. Min code.

# PART C — Oda tamamlanınca draft → seçim → kapı açılır
File: `Assets/Scripts/Systems/Map/RoomLoader.cs`.
Tek helper ekle: `IEnumerator UnlockGateAfterDraft(Gate gate)`:
- `if (DraftManager.Instance != null) { DraftManager.Instance.ShowDraft(); yield return null; while (DraftManager.IsDraftActive) yield return null; }`
- sonra `gate.Unlock();`
- DraftManager yoksa/null → direkt `gate.Unlock()` (soft-lock'tan kaçın).
Yönlendir:
- Combat oda: OnRoomCleared→gate.Unlock() yerine → `StartCoroutine(UnlockGateAfterDraft(gate))`.
- Reward oda: `RewardRoomAutoTrigger` fragment pickup→gate.Unlock() yerine → `StartCoroutine(UnlockGateAfterDraft(gate))`.
- Boss oda (gate yok) → değişmez.
**ÇİFT-DRAFT ÖNLE (tek kaynak):** Draft artık SADECE RoomLoader'dan tetiklenir. DraftManager'ın `OnRoomCleared` auto-timer'ı (`HandleRoomCleared` → `ShowDraftDelayed`) bu akışta çakışmasın. Min yol: `RoomLoader`'a `public static bool DraftDrivenByRoomLoader = true;` ekle; `DraftManager.HandleRoomCleared()` başında `if (RoomLoader.DraftDrivenByRoomLoader) return;` ile auto-timer'ı atla. (Var olan usePortalGatedDraft mantığını BOZMA.)

# DOĞRULAMA (Unity açıksa — UnityMCP)
1. Her .cs sonrası `read_console` → 0 error (compile temiz).
2. Play `Assets/Scenes/Test/PlayableArena_Test01.unity`. `JumpToRoom`/F1 ile:
   - Room1: mobları öldür → **draft 3 kart GERÇEK ikonla** açılır → seç → kapı açılır.
   - Room4 (reward/Vestibule): fragment al → draft → seç → kapı.
   - Room5 boss erişilebilir (boss spawn olur).
3. Screenshot al (UnityMCP) ve done dosyasına yol ekle.
Unity erişilemezse: play-verify = DEFERRED (BLOCKED değil), code+ (mümkünse) compile yap.

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED | PARTIAL | BLOCKED
- Değişen/eklenen dosyalar (path + 1 satır özet)
- Icon eşleşme: N/17 Warblade (+ eşleşmeyenler)
- read_console: temiz mi?
- Play-verify: PASS / DEFERRED + bulgular
- Orchestrator'a kalan (ör. sahne wire gerekiyorsa NE, screenshot yolu)
- Riskler / belirsizlikte BLOCKED gerekçesi
