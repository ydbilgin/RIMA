# LIFE-01 batch — kill "spawn new GameObjects from OnDestroy" scene-close error

ACTIVE RULES: (1) think before coding (2) min code, surgical (3) only the listed files (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console; KENDİ hatanı çöz; raporla (0-error hedef).
E1: sonuç done dosyasına; dönüş ≤12 satır.
**PERSISTENCE (ZORUNLU — geçen sefer edit'ler diske yazılmadı):** edit'leri DİSKE yaz, sonra `git status --short` ve `git diff --stat` çalıştır, çıktısını done dosyasına YAPIŞTIR. "Yaptım" yetmez — değişen dosyalar git status'ta GÖRÜNMELİ.

## Amaç
RIMA scene-close hatası: `"Some objects were not cleaned up when closing the scene. (Did you spawn new GameObjects from OnDestroy?)"`. Kök neden = **lazy-singleton dirilmesi**: bir singleton'ın `Instance` getter'ı `_instance==null` iken `new GameObject(...).AddComponent<...>()` yapıyor; bir scene objesi teardown'da (OnDestroy/OnDisable) o `.Instance`'a erişince getter teardown sırasında GO spawn ediyor → Unity hatası.

## GOLD TEMPLATE (zaten doğru: Assets/Scripts/Combat/AttackTokenManager.cs)
```csharp
private static bool _shuttingDown;
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
private static void ResetStatics() { instance = null; _shuttingDown = false; }
public static T Instance { get {
    if (_shuttingDown) return null;                 // <-- teardown'da DİRİLTME
    if (instance == null && Application.isPlaying) { /* var go = new GameObject... */ }
    return instance; } }
private void OnApplicationQuit() { _shuttingDown = true; }
private void OnDestroy() { if (instance == this) { instance = null; _shuttingDown = true; } }
```
BuildPlacementController.cs (l.68-94) de aynı deseni `_isQuitting` + static `Application.quitting` aboneliğiyle taşıyor — referans.

## Yapılacak
1. Dirilen singleton'ları bul: `Instance` getter'ı `new GameObject` ile kendini spawn eden tüm sınıflar. (Aday liste — DOĞRULA: SkillOfferUI, DirectorMode, RoomRunDirector, RewardPickup, DraftManager, SkillCodexUI, PauseMenuUI, ChamberSelectBootstrap, MainMenuScreen, CharacterSelectScreen, ScreenShakeDriver, UIManager, ShopStand, DoorTrigger, AudioManager, RunStats, LiveRoomReloader, BossHealthBar, RoomLoader, MapFragment, ImpactFrameDriver, CheckpointManager.)
2. Bunlardan **guard'ı OLMAYANLARA** (AttackTokenManager + BuildPlacementController + BuildTileBrushController ZATEN guard'lı, ATLA) GOLD TEMPLATE guard'ını ekle: `_shuttingDown` static flag + ResetStatics + getter başında `if (_shuttingDown) return null;` + OnApplicationQuit/OnDestroy'da `_shuttingDown=true`. Eğer getter zaten `Application.isPlaying` kontrolü yapıyorsa SADECE `_shuttingDown` guard'ını ekle (mevcut mantığı bozma).
3. **Caller'lar zaten null-safe** (teardown'da `.Instance?.X` / `if(.Instance!=null)` kullanıyorlar) → getter null dönünce güvenle atlarlar; YENİ null-check EKLEME gerekmez, sadece doğrula.
4. DİKKAT: prime suspect `DraftManager` (ForgeUI.OnDestroy → DraftManager.Instance erişiyor). DraftManager guard'sızsa MUTLAKA guard'la.

## Kısıt
- 8-yön canon LOCKED (dokunma). Sadece singleton getter + teardown guard'ı; başka davranış DEĞİŞTİRME (no refactor).
- Compile 0-error.

## Rapor (done, ≤12 satır)
VERDICT + guard eklenen dosya listesi + `git status --short` çıktısı (persistence kanıtı) + console 0-error + DraftManager guard'landı mı.
