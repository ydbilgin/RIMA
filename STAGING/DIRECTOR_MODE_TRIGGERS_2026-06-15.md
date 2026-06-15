# Director Mode — Trigger Senaryoları (NETLEŞTİRME, 2026-06-15)

> Yöntem: graphify query (graph.json: aktivasyon yüzeyi Bootstrap/Awake/Update/SetState) → hedefli read (DirectorMode.cs:145-211, 291-347). HARD RULE: cross-file soruda önce graphify (~71× ucuz).

## TETİKLENME SENARYOLARI (kesin)

| Giriş yolu | Director Mode | Kaynak |
|---|---|---|
| **Full-flow oyun** (Play → MainMenu → CharacterSelect → _Arena) | **ASLA spawn olmaz** — tüm session Director YOK | `Bootstrap()` `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` entry scene MainMenu/CharacterSelect ise return (`DirectorMode.cs:148,150-162`) |
| **Dev-direct** (Editor'da _Arena/test scene açık + Play, ya da F5 Play Arena) | **Spawn olur + `Awake`→`SetState(Test)` auto** | `Bootstrap` L164-166 + `Awake` L182-183 (`SetState(Test)` + `ShowTab(Spawn)`) |
| Backquote ` (runtime) | `ToggleState()` Test↔Director | `Update` L193-196 — **Build Mode aktifken inert** |
| Build Mode (F2) | `EnterBuildMode→SetState`, backquote inert | `BuildModeController.cs:211` → DirectorMode.SetState (coupling) |

## STATE DAVRANIŞI
- **Director state:** `Time.timeScale=0` (pause), player inaktif, `SetOverlayVisible(true)` overlay GÖRÜNÜR (`SetState` L309-316, `ResolveTimeScaleForState` L324-326). = editör.
- **Test state:** `Time.timeScale=1` (oyun çalışır), player aktif. **AMA overlay'i GİZLEMİYOR** (Test branch'te `SetOverlayVisible(false)` YOK) → overlay açık kalır. (`SetState` L296-320.)

## SONUÇ — game/director ayrımı
- ✅ **Gerçek oyun (full-flow) ZATEN ayrı:** Director hiç spawn olmaz, oyun tertemiz.
- ⚠️ **Dev-direct'te BLEED:** `Awake→SetState(Test)` overlay'i açık bırakır → gameplay sırasında Director paneli görünür (kullanıcının ekran görüntüsündeki durum; demo `_Arena`-direct'te koşacağı için önemli).

## ÖNERİLEN CERRAHİ FIX (option A)
`SetState`'te Test (Director-olmayan) branch'e `SetOverlayVisible(false)` ekle → Test/gameplay'de Director overlay GİZLİ, sadece backquote→Director'da görünür.
- **Demo etkisi: İYİLEŞTİRİR** — storyboard 0:55-1:25 "backquote → DirectorMode" beat'iyle birebir uyumlu (temiz gameplay → bilinçli tool çağırma → temiz gameplay).
- Kapsam: DirectorMode.cs `SetState` (+ Awake'in başlangıç state'i otomatik gizli başlar). ~2 satır, cerrahi.
- DİKKAT: full-flow'da Director hiç spawn olmadığından, demo Director'ı KULLANACAKSA dev-direct (_Arena) girişiyle koşulmalı — bu zaten böyle.
