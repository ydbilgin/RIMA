# Demo Batch-Fix SPEC — council+ChatGPT synthesis (2026-06-15)

> Kaynak: ChatGPT review (RIMA-001..012) + council 4-lens (bug-sweep + eval) + orchestrator resolution. Scope KARARI: (c) choreograph + cerrahi fix + F12 panic; **full refactor (GameTimeCoordinator/draft-serialization/BuildMode-FSM) POST-DEMO** (3 advisor + ChatGPT'nin kendi min-patch alternatifi). Sunum ~20 Haz.
> Builder: bu 6 fix'i CERRAHİ uygula. Listelenmeyen dosyaya DOKUNMA. Her fix sonrası mevcut davranışı bozma — sadece guard/cleanup ekle.

ACTIVE RULES: (1) think (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR; raporda console durumunu yaz.

## API'ler cx-doğrulandı (var, kullan)
- `UIManager.IsAnyOverlayOpen` — UIManager.cs:36-42
- `DraftManager.Instance.IsDraftActive` / `IsDraftPending` — DraftManager.cs:32-40
- `BuildPlacementController.searchField` (TMP_InputField) — BuildPlacementController.cs:153,856

## 6 CERRAHİ FIX

### FIX-1 — DraftManager lambda leak (multi-take softlock)
`Assets/Scripts/Skills/DraftManager.cs` ~L113-124 (Start içinde): `PlayerClassManager.Instance.OnSecondaryClassSelected += _ => {...}` anonim lambda.
→ Lambda'yı **named private method**'a çevir (ör. `OnSecondaryClassSelectedDraft(ClassType _)`). Start'ta `+= OnSecondaryClassSelectedDraft`. **OnDisable VE OnDestroy'da** `if (PlayerClassManager.Instance != null) PlayerClassManager.Instance.OnSecondaryClassSelected -= OnSecondaryClassSelectedDraft;` ile unsubscribe et. (Mevcut OnDisable RoomLoader event'lerini çözüyor — oraya ekle; OnDestroy yoksa minimal ekle.)

### FIX-2 — Build search-field hotkey guard (RIMA-008 canlı risk)
`Assets/Scripts/UI/BuildMode/BuildPlacementController.cs` `HandleKeyboard()` ~L295.
→ Metodun EN BAŞINA: `if (searchField != null && searchField.isFocused) return;` (TMP_InputField.isFocused). Arama yazarken F/E/bracket/Ctrl+Z tetiklenmesin.

### FIX-3 — ShowDraft re-entry guard (çift-draft)
`Assets/Scripts/Skills/DraftManager.cs` `ShowDraft()` ~L195.
→ Metodun EN BAŞINA (IsDraftPending=false satırından ÖNCE): `if (IsDraftActive) return;` (zaten aktif draft varken ikinci kez açma).

### FIX-4 — EnterBuildMode modal guard (centerpiece koruma)
`Assets/Scripts/UI/BuildModeController.cs` `EnterBuildMode()` ~L211.
→ Metodun EN BAŞINA: `if ((UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen) || (DraftManager.Instance != null && (DraftManager.Instance.IsDraftActive || DraftManager.Instance.IsDraftPending))) return;` (draft/overlay açıkken kazara F2 ile Build Mode'a girme).

### FIX-5 — DirectorMode camera target reset (2. oda void)
`Assets/Scripts/UI/DirectorMode.cs` `SetState()` Test branch (else bloğu, ~L317-322 — orchestrator'ın overlay-fix'iyle eklenen else). 
→ Test'e geçerken `hasCameraTarget = false;` ekle (Director'dan çıkınca kamera hedefini bırak ki yeni odada eski koordinata uçmasın). NOT: `hasCameraTarget` field'ını DirectorMode.cs:658 civarı doğrula; CacheCameraTarget onu true yapıyor.

### FIX-6 — Opening-draft coroutine retention (quick-reset safety)
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` `BeginRun()` ~L181-210 `StartCoroutine(OpeningKitDraftSequence())` referanssız.
→ `private Coroutine openingDraftSequence;` field ekle. BeginRun'da `openingDraftSequence = StartCoroutine(...)`. `StopClearSequences()` (~L1737) içinde `if (openingDraftSequence != null) { StopCoroutine(openingDraftSequence); openingDraftSequence = null; }`. Sequence tamamlanınca null'a çek.

## YAPMA (ertelendi)
- Timescale/GameTimeCoordinator (RIMA-001), draft-serialization sistemi (RIMA-004), BuildMode 4-state FSM, RewardPickup timeout (canonical zaten güvenli RoomRunDirector.cs:1309), Director full-flow bootstrap (002, dev-direct setup). Bunlara DOKUNMA.

## DOĞRULAMA (builder)
Derle (read_console 0 error). Her fix'in golden-path'i bozmadığını mantıken doğrula. Rapor: hangi dosya:line değişti, console durumu, ≤10 satır.
