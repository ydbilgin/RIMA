# DUAL-CLASS DRAFT butonu — cerrahi fix raporu (2026-06-13)

Dosya: `Assets/Scripts/UI/DirectorMode.cs` (tek dosya; ClassSelectionUI/PlayerClassManager'a DOKUNULMADI).

## BULGU 1 — Seçim UI'ı Director overlay arkasında kalıyor → DÜZELTİLDİ
- BuildOverlay'de oluşturulan canvas referansı `overlayCanvasGo` alanında saklandı.
- `TriggerDualClassDraft()` artık `SetState(Test)` sonrası `SetOverlayVisible(false)` ile Director canvas root'unu fiilen kapatıyor (görünürlük + raycast birlikte kesiliyor). Böylece ClassSelectionUI (sortingOrder 190) topmost aktif overlay oluyor.
- Director'a geri dönüşte (`SetState(Director)`) `SetOverlayVisible(true)` ile overlay geri geliyor (backquote ile re-open çalışır).
- RebuildOverlayRuntime cleanup'ına `overlayCanvasGo = null` eklendi (stale referans yok).

## BULGU 2 — Ölüm-state guard'ı yok → DÜZELTİLDİ
- `TriggerDualClassDraft()` başına SelectDirectorClass ile AYNI semantik ölüm guard'ı eklendi: `playerHealth.IsDead` VEYA `DeathScreenManager.IsDeathActiveForDemo` ise erken çıkış + "player_dead" toast.

## BULGU 3 — Buton seçim sonrası gizlenmiyor → DÜZELTİLDİ
- `PlayerClassManager.OnSecondaryClassSelected` event'ine LEAK'siz abonelik: `TryHookSecondaryClassListener()` (idempotent, `secondaryClassListenerHooked` bayrağıyla) OnEnable + tetik anında çağrılır; `UnhookSecondaryClassListener()` OnDisable + OnDestroy'da simetrik temizlik yapar. Event geldiğinde `RefreshDualClassDraftButton()` butonu anında gizliyor.

## timeScale tutarlılığı
- Ekstra timeScale yazımı EKLENMEDİ. Akış doğrulandı: Director kapanır → ClassSelectionUI timeScale=0 sahibi → OnClassChosen timeScale=1.

## DOĞRULAMA
- Recompile: `read_console` (Error) = **0 error** (compile sonrası ve Play Mode sonrası).
- EditMode (RIMA.Tests.EditMode): 541 koşu, **11 fail** = baseline (pixellab/Wang PNG, MCP scene method, perf anti-pattern, prefab health, DontDestroyOnLoad). Yeni fail YOK; dual-class ile ilgili fail YOK.
- Play Mode kabul testi (`_Arena` sahnesi, execute_code):
  - BULGU 1: tetik sonrası `dirOverlay_active=False | selUI_active=True | selUI_so=190 | timeScale=0` (Director gizli, seçim UI topmost).
  - Akış: sınıf seçimi (Elementalist) → `ctrl 1->2 | sec=Elementalist | timeScale=1 | btnActive=False` (controller arttı, oyun unpause, buton gizlendi).
  - BULGU 2 guard: death aktifken tetik → `selOpened=False | sec=None | guard_PASS=True` (seçim AÇILMADI).
- Play Mode'dan ÇIKILDI.

Git commit YAPILMADI (kısıt gereği).
