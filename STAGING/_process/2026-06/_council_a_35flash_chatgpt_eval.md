### Q1: Yeni Bulguların Doğrulanması (Evidence)

*   **RIMA-003 (Build Mode aktif modal'ı gizler):** **GERÇEK.** 
    *   **Kanıt:** [BuildModeController.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs#L211) içindeki `EnterBuildMode()` ve `Toggle()` metotları herhangi bir modal veya draft kontrolü yapmadan çalışır. Girdiğinde [BuildModeController.cs:L287](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs#L287) satırında `HideOtherUiCanvases()` çağrılarak aktif draft/pause pencereleri görünmez yapılır ancak mantıksal durumları ve zaman akışları temizlenmez.
*   **RIMA-006 (Reward timeout HideDraft çağırmaz):** **GERÇEK.**
    *   **Kanıt:** [RewardPickup.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/RewardPickup.cs#L181-L186) içindeki `DraftThenOpenExit()` coroutine'i 90 saniye sonra döngüden çıkar ve kapıları açar. Ancak `HideDraft()` çağrısı yapmadığı için draft ekranı ve `UIManager` üzerindeki `skillOfferOpen` kilitli kalır. (Scripted demoda 90 saniye beklenmeyeceği için tetiklenmesi beklenmez).
*   **RIMA-007 (Opening-draft coroutine restart'ta kalır):** **GERÇEK.**
    *   **Kanıt:** [RoomRunDirector.cs:L209](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L209) satırında `OpeningKitDraftSequence()` referans kaydedilmeden başlatılır. [RoomRunDirector.cs:L1737](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L1737) içindeki `StopClearSequences()` ise yalnızca `clearSequence` ve `slowMoSequence` coroutine'lerini durdurur. Aynı sahnede restart atıldığında eski coroutine hayatta kalır.
*   **RIMA-008 (Build hotkey text-input'ta tetiklenir):** **GERÇEK.**
    *   **Kanıt:** [BuildPlacementController.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildMode/BuildPlacementController.cs#L295-L344) içindeki `HandleKeyboard()` metodu, [BuildPlacementController.cs:L153](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildMode/BuildPlacementController.cs#L153) adresindeki `searchField` odağına sahip olup olmadığını kontrol etmeden `F`, `E` ve köşeli parantez kısayollarını tetikler.

---

### Q2: Scope Değerlendirmesi & Over-Engineering Eleştirisi

Sunuma **5 gün kala**, centerpiece ve golden-path çalışır durumdayken ChatGPT'nin önerdiği **GameTimeCoordinator, Draft-Serialization ve BuildMode FSM full-refactor'ı NET BİR OVER-ENGINEERING'dir ve son derece tehlikelidir.**

*   **Optimal Tercih:** **(c) Choreograph-around + sadece ucuz-kesin fix'ler + F12 panic.**
*   **Gerekçe:** Scripted akışta (10x prova edilmiş) tehlikeli modal kombinasyonlarına basılmayacaktır. Büyük sistemlerin baştan yazılması Unity tarafında öngörülemeyen serialization ve state bug'ları doğurur. Sadece çoklu take/restart sırasında çökme yaratabilecek sızıntılar ve sunum sırasında hata payını sıfırlayan basit guard'lar kodlanmalıdır.

---

### Q3: Uygulanacak En Küçük Batch-Fix Listesi

> [!WARNING]
> **UYARI:** Büyük refactor süreçlerine kesinlikle girilmemeli, aşağıdaki cerrahi düzeltmelerle sınırlı kalınmalıdır.

1.  **YAP (Sızıntı Önleme):** [DraftManager.cs:L114-L124](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/DraftManager.cs#L114-L124)
    *   *Açıklama:* `OnSecondaryClassSelected` anonim lambda aboneliğini named metota çevir ve `OnDisable`/`OnDestroy` içinde unsubscribe et. Tekrarlı run'larda `MissingReferenceException` vermesini önler (Demo-Blocker).
2.  **YAP (Arama Alanı Kilidi):** [BuildPlacementController.cs:L295](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildMode/BuildPlacementController.cs#L295)
    *   *Açıklama:* `HandleKeyboard()` başına `if (searchField != null && searchField.isFocused) return;` guard'ını ekle. Arama yaparken harf kısayollarının tetiklenmesini önler.
3.  **YAP (Kamera Kayma Düzeltmesi):** [DirectorMode.cs:L320-L325](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs#L320-L325)
    *   *Açıklama:* Director modundan çıkıldığında (`else` bloğu) `hasCameraTarget = false;` yap. Bir sonraki odaya geçip Director'a girildiğinde kameranın ilk odanın koordinatlarına uçmasını (void'e düşüş) engeller.
4.  **YAP (Basit Modal Guard):** [BuildModeController.cs:L211](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs#L211)
    *   *Açıklama:* `EnterBuildMode()` başına `if ((UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen) || (DraftManager.Instance != null && (DraftManager.Instance.IsDraftActive || DraftManager.Instance.IsDraftPending))) return;` ekle. Sunucu yanlışlıkla F2'ye basarsa kazara build modun açılmasını engeller.
5.  **YAP (Çift Draft Guard):** [DraftManager.cs:L195](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/DraftManager.cs#L195)
    *   *Açıklama:* `ShowDraft()` metodunun en başına `if (IsDraftActive) return;` guard'ı ekle.

#### Erteleme / Choreograph Olarak İşaretlenenler:
*   **RIMA-001 (Time scale koordinatörü):** ERTELE / POST-DEMO. (Provaya sadık kalınacak, F12 panic kullanılacak).
*   **RIMA-002 (Director bootstrap):** ERTELE / SETUP. (Demo doğrudan `_Arena` sahnesinden başlatılacak).
*   **RIMA-006 (Reward timeout kapı açılışı):** ERTELE / CHOREOGRAPH. (Presenter ödülü 90 sn bekletmeden seçecek).
*   **RIMA-007 (Opening draft coroutine restart):** ERTELE / CHOREOGRAPH. (Run başladıktan sonraki ilk 2 saniye içinde restart atılmayacak, atılırsa sahne reload edilecek).
*   **RIMA-009 (F2 spam kamera zoom bozulması):** ERTELE / CHOREOGRAPH. (Hızlı ardışık F2 basışlarından kaçınılacak).
*   **RIMA-010 (Hasar clamp):** DISMISSED / POST-DEMO.

---
### Summary of Work
I evaluated the ChatGPT code review findings (003, 006, 007, 008) and verified them as confirmed bugs using target file paths and line numbers. I critiqued the suggested refactoring architecture as over-engineering for a rehearsal-ready demo scheduled in 5 days. I proposed a minimal batch-fix strategy containing only 5 precise, surgical fixes while delegating the rest to choreography/setup or post-demo milestones.

