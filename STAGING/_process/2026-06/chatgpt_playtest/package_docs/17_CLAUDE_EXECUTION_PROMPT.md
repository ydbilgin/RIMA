# Claude'a Verilecek Ana Görev

Bu ZIP, RIMA Unity projesinin 16 Haziran playtest UI/runtime incelemesidir. Önce `00_READ_FIRST/00_CLAUDE_START_HERE.md` ve kaynak önceliğini oku. Sonra gerçek projeyi inceleyerek rapordaki hipotezleri doğrula.

Öncelikle şu P0 sorunlarını düzelt:
1. Önceki odadan kalan reward objeleri.
2. G interaction'ın reward UI açmaması.
3. Reward çözülmeden oda geçişi.
4. WeaponSlot/body facing ayrışması.
5. Skill'lerin cursor yerine eski facing yönünü kullanması.

Ardından `BuildPlacementController` scene cleanup uyarısını çöz, reward-card TMP/layout sorununu düzelt ve ambiguous combo etiketlerini canonical trigger/outcome metnine geçir.

UI yeniden tasarımında `03_UI_UX_POLISH/GENERATED_POLISH_SCREENS/` görsellerini final kalite ve hierarchy referansı olarak kullan; bitmapleri doğrudan oyuna koyma. `04_ASSET_PACK_GUIDE/` içindeki modüler 9-slice/overlay yapısıyla prefab kur. Text ve gameplay değerleri görsellerden değil canonical ScriptableObject/karar belgelerinden gelmeli.

Rift-Forged Egg'i ayrı bir yeni inventory/economy katmanı yapma. Önce mevcut reward definition'larını sunan world mystery-container olarak uygula. Bir seçim yapıldığında reward atomik uygulanmalı, kardeşler temizlenmeli, session tamamlanmalı ve kapılar açılmalı.

Kod değişikliğinden önce her bugın gerçek root cause'unu yaz. İş sonunda şunları raporla:
- Root cause
- Değişen dosyalar
- Yeni/updated prefabs ve assets
- Uygulanan çözüm
- Test sonuçları
- Before/after kanıtları
- Kalan riskler ve sonraki sprint
