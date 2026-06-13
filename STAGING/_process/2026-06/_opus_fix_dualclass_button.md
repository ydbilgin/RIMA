ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: Gerekmez.
UNITY ERROR CHECK: İş bitince mcp__UnityMCP__read_console (Error) çağır; hata varsa ÇÖZ; raporda console durumu (0 error şartı).

Proje kökü: F:/Antigravity Projeler/2d roguelite/RIMA (Unity açık). CANLI DEMO bugün. Bağımsız review (cx/yekta) DUAL-CLASS DRAFT butonunu FAIL'ledi; stat preset'leri PASS (DOKUNMA). Fix tek dosyada: `Assets/Scripts/UI/DirectorMode.cs`. İlgili (oku, gerekiyorsa minimal dokun): `ClassSelectionUI.cs`, `PlayerClassManager.cs`.

# GÖREV: DUAL-CLASS DRAFT butonunun 3 review bulgusunu cerrahi düzelt

## BULGU 1 (ASIL DEMO-KIRAN) — Seçim UI'ı Director overlay'in ARKASINDA kalıyor
Kanıt: Director canvas sortingOrder=950 (`DirectorMode.cs:622-624`); ClassSelectionUI canvas sortingOrder=190 (`ClassSelectionUI.cs:164-169`); Director root Test state'te bile görünür + raycast bloke ediyor (`DirectorMode.cs:591-599`). Mevcut handler `SetState(Test)` çağırıyor ama Director overlay 950'de üstte kalıyor → kullanıcı sınıf kartlarını GÖREMEZ/TIKLAYAMAZ.
FIX: Buton dual-class draft'ı tetiklerken Director overlay'i TAMAMEN KAPAT/GİZLE (canvas root'unu inactive yap veya raycast+görünürlüğü kes), böylece ClassSelectionUI topmost aktif canvas olsun. `SetState(Test)`'e güvenme — root'u fiilen gizle. Seçim bitince Director KAPALI kalabilir (kullanıcı backquote ile tekrar açar) — basit ve güvenli yol bu.

## BULGU 2 — Ölüm-state guard'ı yok
Kanıt: `SelectDirectorClass()` ölüm guard'ı taşıyor (`DirectorMode.cs:1873-1879`) ama yeni `TriggerDualClassDraft()` (`1952-1975`) taşımıyor. Ölüm ekranı açıkken seçim yapılırsa `ClassSelectionUI.OnClassChosen` timeScale'i 1'e zorluyor (`ClassSelectionUI.cs:283-286`) → ölü UI açıkken oyun unpause olur.
FIX: `TriggerDualClassDraft()` başına ölüm/death-screen guard ekle (SelectDirectorClass'taki ile AYNI semantik); ölüm aktifse erken çık (istersen kısa toast "Önce Quick Reset").

## BULGU 3 — Buton seçim sonrası gizlenmiyor (bayat görünürlük)
Kanıt: Görünürlük yalnız `RefreshClassSkillPanel()` yolunda güncelleniyor (`1924-1929`) ve tetikten HEMEN sonra (`1973-1974`) — o an SecondaryClass hâlâ None, buton açık kalıyor. `PlayerClassManager` `OnSecondaryClassSelected` event'ini sunuyor (`PlayerClassManager.cs:21-22`) ama DirectorMode dinlemiyor.
FIX: `OnSecondaryClassSelected`'e abone ol (OnEnable/OnDisable simetrik veya panel kurulumunda tek abonelik + cleanup) → seçim olunca butonu gizle/refresh. Event aboneliği LEAK'siz olmalı.

## timeScale tutarlılığı (bütünleşik kontrol):
Yukarıdaki Bulgu 1 fix'iyle (Director KAPALI) akış: Director kapanır → ClassSelectionUI timeScale=0 sahibi → seçim → OnClassChosen timeScale=1 → oyun oynanabilir. Bu tutarlı; ekstra timeScale yazma EKLEME. Sadece ölüm-overlap'i Bulgu 2 guard'ı kesiyor — doğrula.

## KISIT: SADECE DirectorMode.cs (+ gerekiyorsa ClassSelectionUI/PlayerClassManager'a MİNİMAL, ama önce DirectorMode içinde çözmeyi dene). Stat preset kodu PASS — DOKUNMA. Blueprint/asset DOKUNMA. Git commit YAPMA.

## DOĞRULAMA (zorunlu):
1. Recompile → read_console 0 error.
2. EditMode BİR KEZ: assembly RIMA.Tests.EditMode; baseline 11 fail; ≤11 + yeni fail yok.
3. **Play Mode kabul testi (BULGU 1 kritik):** execute_code ile Director aç → dual-class butona bas → ASSERT: ClassSelectionUI aktif VE üstte (Director root artık gizli/raycast kapalı) → sınıf seç → controller 1→2, timeScale=1, buton gizlendi. Ölüm-guard: death-screen açıkken butona basınca seçim AÇILMAMALI. Compact assert string'leri (E5). Play Mode'dan ÇIK.

## RAPOR (E1): `STAGING/_process/2026-06/_opus_fix_dualclass_button_2026-06-13.md` (tam Türkçe karakter). DÖNÜŞ ≤10 satır: 3 bulgu durumu + Play Mode assert sonuçları + test/console + rapor yolu.
