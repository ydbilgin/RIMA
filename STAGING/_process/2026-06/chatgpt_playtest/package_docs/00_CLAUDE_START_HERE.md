# CLAUDE — BURADAN BAŞLA

Bu ZIP'i RIMA Unity projesinin kökünde açıp önce raporu oku, sonra gerçek kodu incele. Bu paket yalnız görsel öneri değildir; playtestte progression'ı bozan buglar ve bunların teknik çözüm yolu da dahildir.

## Kullanıcının senden istediği çıktı

1. Mevcut kodda her bugın gerçek root cause'unu bul.
2. P0 bugları düzeltmeden UI yeniden tasarımına geçme.
3. Görsel polish ekranlarını layout ve art-direction referansı olarak kullan.
4. Modüler asset pack'i 9-slice/overlay mantığıyla planla.
5. Rift-Forged Egg'i yeni bir gereksiz ekonomi katmanı yapmadan world-reward sunum sistemi olarak çöz.
6. Değişikliklerden sonra test raporu üret.

## P0 sırası

1. `REWARD-01` Eski üçlü ödüller sonraki odalarda kalıyor.
2. `REWARD-02` Güncel ödülde `G` hiçbir seçim ekranı açmıyor.
3. `REWARD-03` Ödül çözülmeden odadan çıkılabiliyor veya session sessizce terk edilebiliyor.
4. `AIM-01` WeaponSlot yön değiştirirken body aynı frame dönmüyor.
5. `AIM-02` Skill hedefi mouse/cursor yerine karakterin facing veya last-move yönünü kullanıyor.

## P1 sırası

6. `LIFE-01` Scene kapanırken `BuildPlacementController` temizlenmiyor veya yeniden spawn oluyor.
7. `UI-01` Reward kartında metin/footer genişliği çöküyor ve yazı dikey sarılıyor.
8. `DATA-01` “X ile eşleşir” belirsiz; bazı eşleşmeler canonical skill verisiyle uyuşmuyor.
9. HUD, Pause, Settings ve Codex okunabilirlik/polish pass'i.

## Okuma sırası

1. `00_READ_FIRST/01_SOURCE_PRIORITY_AND_CONFLICTS.md`
2. `01_PLAYTEST_EVIDENCE/03_EVIDENCE_INDEX.md`
3. `02_BUGS_AND_FIXES/04_MASTER_BUG_REGISTER.md`
4. `02_BUGS_AND_FIXES/05_REWARD_SESSION_ROOT_CAUSE_AND_FIX.md`
5. `02_BUGS_AND_FIXES/06_AIM_FACING_ROOT_CAUSE_AND_FIX.md`
6. `02_BUGS_AND_FIXES/07_BUILD_PLACEMENT_LIFECYCLE_FIX.md`
7. `03_UI_UX_POLISH/08_UI_UX_POLISH_SPEC.md`
8. `04_ASSET_PACK_GUIDE/10_MODULAR_UI_ASSET_PACK.md`
9. `05_RIFT_FORGED_EGG/12_RIFT_FORGED_EGG_SYSTEM.md`
10. `07_IMPLEMENTATION_AND_TESTS/15_IMPLEMENTATION_ROADMAP.md`
11. `07_IMPLEMENTATION_AND_TESTS/16_REGRESSION_TEST_PLAN.md`

## Çalışma şekli

- Raporlardaki örnek sınıf isimlerini körlemesine kopyalama; projedeki gerçek dosya/sınıf adlarını bul.
- Her düzeltme için önce bir failing test/reproduction yaz.
- Reward, input ve lifecycle sorunlarını lokal `Destroy()` yamalarıyla değil, state ownership ile çöz.
- Görsel polish dosyalarındaki metinler ve skill sayıları konsept olabilir. Canonical veri her zaman karar belgelerinden ve gerçek ScriptableObject'lerden gelmelidir.

## Claude'un sonunda vereceği rapor

- Root cause
- Değişen dosyalar
- Uygulanan çözüm
- Test edilen senaryolar
- Sonuç
- Kalan riskler
- Yeni asset/scene/prefab listesi
