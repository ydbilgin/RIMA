# GÖREV: T9 singleton + kardeşler + T7 guard FIX (cx implement)

ACTIVE RULES: (1) think before coding (2) min/surgical — SADECE aşağıdaki 5 dosya, SADECE belirtilen satırlar (3) ilgisiz refactor YOK (4) BLOCKED if unclear.
**GİT YOK:** commit/push/add HİÇBİRİ. (Commit'i orchestrator wrap'te yapacak.)
UNITY ERROR CHECK: iş bitince `read_console` (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR. 0-sürpriz raporla.

## BAĞLAM
Playtest T9'u runtime-confirm etti: in-game Restart sonrası `DraftManager.Instance==null` çünkü static `_shuttingDown` scene reload'da true kalıyor → opening draft açılmıyor. Council (cx+ax_pro) oybirliği: AttackTokenManager desenini aynala + kardeşleri de düzelt + T7 ucuz guard. Bu görev o kararın implementasyonu.

**Referans desen (DOĞRU çözüm zaten burada):** `Assets/Scripts/Combat/AttackTokenManager.cs:60-72` — OnDestroy SADECE `if (instance==this) instance=null;` yapar, `_shuttingDown`'a DOKUNMAZ; `_shuttingDown=true` SADECE OnApplicationQuit'te. Yorum bloğu nedenini açıklıyor (oku).

## FIX 1 — DraftManager (T9 ana)
`Assets/Scripts/Skills/DraftManager.cs` OnDestroy (~123-132): `_shuttingDown = true;` satırını **SİL** (sadece `instance = null;` kalsın, `if (instance==this)` guard'ı koru). `OnApplicationQuit` (~134-137) `_shuttingDown=true;` KALSIN. Reset (`ResetStatics`/SubsystemRegistration ~29-32) `_shuttingDown=false` KALSIN.

## FIX 2 — Kardeş singletonlar (aynı desen)
Aynı düzeltme (OnDestroy'dan `_shuttingDown=true` sil, OnApplicationQuit'te bırak, instance=null koru):
- `Assets/Scripts/Core/RunStats.cs` (OnDestroy ~90-94)
- `Assets/Scripts/Save/CheckpointManager.cs` (OnDestroy ~57-64)
- `Assets/Scripts/UI/BuildMode/BuildTileBrushController.cs` (OnDestroy ~147-159 — burada TeardownAll() gibi ek mantık varsa DOKUNMA, sadece `_shuttingDown=true` satırını çıkar)
Her birinde: OnApplicationQuit'teki `_shuttingDown=true` KALIR; reset KALIR; sadece OnDestroy'daki `_shuttingDown=true` gider.

## FIX 3 — T7 ucuz guard
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` → `RestoreGameplayTimeScale()` (~1940 civarı) metodunun EN BAŞINA guard ekle: skill-offer overlay açık VEYA draft aktif/pending iken erken `return` (timeScale'i 1 yapmadan). Doğru koşulu API'ye göre seç: `UIManager.Instance != null && UIManager.Instance.IsSkillOfferOpen` (ve varsa `DraftManager.Instance?.IsDraftActive`). 
**ÖNEMLİ:** Bu guard normal akışı bozmamalı — draft KAPANIRKEN timeScale'i kim restore ediyor kontrol et (UIManager draft-close path'i kendi restore'unu yapıyorsa guard güvenli; cx analizi: UIManager scene-load reset'i :56-103 stuck-at-0'ı temizliyor). Eğer guard draft-close restore'unu kıracaksa BLOCKED yaz, ekleme.

## BİTİRİNCE
1. `refresh_unity(scope=scripts, compile=request, wait_for_ready=true)` → `read_console`. 0 error/warning (MCP ExecuteCode "objects not cleaned up" artefaktı hariç).
2. **Runtime re-verify (Unity Play):** (a) _Arena fresh → in-game Restart → `DraftManager.Instance != null` ve opening draft AÇILIYOR mu? (b) 1 reward döngüsü → reward draft hâlâ timeScale=0 pause ediyor mu (T7 guard regresyon yapmadı mı)? Kısa state-probe kanıtı ver.
3. GİT YOK.

## ÇIKTI
CODEX_DONE'a: her dosyada ne değişti (önce/sonra satır) + compile console durumu + runtime re-verify sonucu (DraftManager.Instance restart sonrası, opening draft, reward pause). Dönüşün ≤10 satır.