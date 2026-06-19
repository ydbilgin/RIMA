# GÖREV: Bug-Fix Council — RIMA demo-killer triyajı (READ-ONLY ANALİZ)

Sen bağımsız bir teknik danışmansın. Bir Unity 2D ARPG'nin demo-öncesi playtest bulgularını ve fix yaklaşımını değerlendireceksin. **Kod yazma — sadece analiz + öneri.**

## 🔒 READ-ONLY — KESİN
❌ Hiçbir dosya düzenleme/oluşturma. ❌ `git` HİÇBİR komut (add/commit/push). ❌ Unity'ye yazma/Play. ✅ Sadece kod OKU + öneri ver. (Geçmişte bir advisor `git add .` yaptı — SAKIN.)
Çıktın Türkçe, kanıt-temelli.

## BAĞLAM — Playtest bulguları (2026-06-19, runtime-confirmed)
1. **T9 (CONFIRMED demo-killer):** In-game Restart sonrası `DraftManager.Instance == null` çünkü static `_shuttingDown == true`. Sahnede canlı DraftManager var ama `Instance` getter null dönüyor → restart'ta opening draft AÇILMIYOR.
2. **T7 (bug DEĞİL):** 2. reward draft iki temiz koşuda da düzgün pause etti (timeScale=0). Codex'in eski "freeze (timeScale=1)" gözlemi transient/sequence artefaktı görünüyor. (Latent race var: `RoomRunDirector.RoomClearSequence` `finally { RestoreGameplayTimeScale(); }` ~satır 1508, draft pause'u ile yarışabilir — ama temiz akışta tetiklenmiyor.)
3. T6 (door reward'dan önce açılıyor) = KASITLI (FIX-1 B1 Binding-of-Isaac decoupling, kodda dokümante) → aksiyon yok.
4. Ranger/Shadowblade kilitli ("geliştirme aşamasında") = kasıtlı, bug değil.

## KOD GERÇEKLERİ (Opus zaten doğruladı — teyit et/itiraz et)
- `Assets/Scripts/Skills/DraftManager.cs:23` → `get => _shuttingDown ? null : instance;`
- `DraftManager.cs:~130` → `OnDestroy()` içinde `instance=null; _shuttingDown=true;`
- `OnApplicationQuit()` → `_shuttingDown=true;`
- **Referans düzeltme (aynı bug-sınıfı zaten çözülmüş):** `Assets/Scripts/Combat/AttackTokenManager.cs:62-69` — OnDestroy SADECE instance'ı null'lar, `_shuttingDown`'a DOKUNMAZ; `_shuttingDown` sadece OnApplicationQuit'te. Yorum: ResetStatics (SubsystemRegistration) sadece play-mode girişinde çalışır, scene reload'da DEĞİL → OnDestroy'da _shuttingDown=true bırakmak tüm restart-run'ı bozar.
- **Aynı latent bug'a sahip kardeş singleton'lar:** `RunStats.cs:~92`, `Save/CheckpointManager.cs:~62`, `UI/BuildMode/BuildTileBrushController.cs:~152` (hepsi OnDestroy'da `_shuttingDown=true` yapıyor).

## KARAR VERİLECEKLER (her birine net öneri + gerekçe)
1. **T9 fix yaklaşımı:** (A) AttackTokenManager'ı aynala (OnDestroy `_shuttingDown` set etmesin; sadece OnApplicationQuit) — kanonik desen, VEYA (B) `Awake()`'te Instance-guard'dan ÖNCE `_shuttingDown=false` resetle. Hangisi daha güvenli/doğru? İkisi birden mi?
2. **Kapsam:** Kardeş singleton'lar (RunStats/CheckpointManager/BuildTileBrushController) da ŞİMDİ düzeltilsin mi? Düzeltilmezse demo'da restart sonrası bu sistemlerde (run-stats/checkpoint/build-mode) aynı kırılma riski var mı? Demo'da bu sistemler restart sonrası kullanılıyor mu?
3. **T7:** Ucuz savunma kalkanı (skill-offer overlay açıkken `RestoreGameplayTimeScale` timeScale'i 1 yapmasın) demo sigortası olarak eklensin mi, yoksa bırakılsın mı (temiz akışta tekrar üretilemedi)? Risk/maliyet?

## FORMAT
Her karar için: **ÖNERİ + gerekçe + risk**. Sonunda "fix sırası/önceliği" + "demo için minimum şart" özeti.
- **(cx / kod uzmanı):** kod-seviye doğruluk, kanonik desen, regresyon riski; kardeş singleton kullanımını kodda kontrol et.
- **(ax_pro / mimari):** mimari tutarlılık, kapsam kararı, demo-risk yargısı.
Done dosyana yaz (cx → CODEX_DONE.md; ax_pro → STAGING/_process/2026-06/_council_axpro_bugfix_2026-06-19.md). Dönüşün ≤10 satır.
