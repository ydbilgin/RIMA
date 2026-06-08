RIMA playtest screenshot bug pass yapmanı istiyorum. Bu görevde amaç tek tek patch tahmini değil, canlı repo akışına göre root-cause bulup düzeltmek. Kullanıcı 4 screenshot verdi ve şikayetler aşağıda.

ÖNEMLİ CANLI SİSTEM KURALI
- Eski `RoomLoader` / `RoomSequenceData` / `Gate.cs` / `GateBehavior` / `DoorTrigger` / `RuntimeRoomManager` hattına patch yazma.
- Güncel canlı yol: `MainMenu → CharacterSelect / ChamberSelectBootstrap → _Arena → RoomRunDirector → IsoRoomBuilder`.
- Oda/portal/movement fix'leri `_Arena`, `RoomRunDirector`, `IsoRoomBuilder`, `RoomTemplateSO`, `RoomRunExitDoorTrigger` tarafında aranmalı.

KULLANICI GÖZLEMLERİ

1) Play'e basınca yanlış ekran geliyor.
- Beklenen: Character Select / Chamber seçim ekranı.
- Gerçek: Kullanıcı görselinde doğrudan arena/gameplay benzeri sahne geliyor ve/veya Skill Codex paneli açılıyor.
- Karakter seçmeden oyuna girmiş gibi duruyor.
- Bu bir boot-flow/scene routing/state cleanup problemi olabilir.

2) Skill Codex / Yetenek Kodeksi ekranı yanlış zamanda açılıyor.
- ESC basınca büyük “YETENEK KODEKSİ” ekranı açılıyor.
- Bu doğrudan gameplay'de olmamalı.
- ESC default davranışı PauseMenu olmalı.
- PauseMenu içinde Resume / New Run / Settings / Skill Codex / Exit seçenekleri olmalı.
- Skill Codex debug panel gibi görünmemeli; PauseMenu içinden açılan ayrı polished panel olabilir.

3) Codex kapanınca/etkileşimden sonra kilitlenme var.
- Panel kapanınca Time.timeScale, input, cursor veya modal stack restore edilmiyor olabilir.
- Codex close routine bütün transient UI state'leri temizlemeli.

4) İki mouse cursor görünüyor.
- Custom cursor UI image + system cursor aynı anda aktif olabilir.
- CursorManager veya UI panel open/close state'i kontrol edilmeli.
- Aynı anda yalnızca tek cursor görünmeli.

5) Weapon/sword yanlış render oluyor.
- Kılıç yalnızca köşeye gidince clifflerin arasından görünüyor.
- Weapon floor üstünde ve karakterle birlikte görünmeli.
- Şu an weapon muhtemelen Ground/Cliff/Wall altında render oluyor veya Player SortingGroup dışında.
- WeaponSlot/pivot/localPosition da yanlış olabilir.
- Mevcut beyaz/uzun sword sprite kötü duruyor; Warblade için top-down ARPG açısına uygun yeniden çizilmiş/yenilenmiş sprite gerekir.

6) Screenshot 2'de mor çizginin sağına yürünemiyor.
- Görselde floor var ama player sağ tarafa geçemiyor.
- Bu invisible collider, RoomBounds clamp, walkable-mask mismatch veya IsoRoomBuilder collider üretim hatası olabilir.
- Debug overlay/log ekle: player hangi collider/layer tarafından bloklanıyor?
- Görsel floor = walkable olmalı.

7) Map/room çok küçük.
- Bu arena combat için dar hissettiriyor.
- Normal combat room minimum 24x18, tercihen 28x20 hissi vermeli.
- En az iki yönde 8+ unit dash lane olmalı.
- Merkez combat alanı açık kalmalı.

8) Screenshot 4'te hover sırasında mavi bozuk yazılar çıkıyor.
- Skill üstüne gelince mavi tooltip/text katmanı bozuluyor.
- Tooltip anchor/pivot/sorting/TMP overflow/word-wrap veya panel cleanup hatası olabilir.
- TooltipManager merkezi olmalı; panel kapanınca force-hide edilmeli.

9) Skill iconları her zaman yüklenmiyor.
- Bazı skill satırlarında icon yerine boş/fallback kutu görünüyor.
- Icon path / Resources.Load / Addressables key / case-sensitive dosya adı / ScriptableObject sprite ref kontrol edilmeli.
- Missing icon loglanmalı ve fallback kullanılmalı.

YAPILACAKLAR

A) Root-cause audit yap.
- Şu dosyaları özellikle kontrol et:
  - `Assets/Scripts/UI/MainMenuController.cs`
  - `Assets/Scripts/UI/CharacterSelectScreen.cs`
  - `Assets/Scripts/UI/ChamberSelectBootstrap.cs`
  - `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
  - `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`
  - `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
  - `_Arena.unity`, `CharacterSelect.unity`, `MainMenu.unity`

B) ESC/Pause/Codex input ayrımını düzelt.
- ESC gameplay'de PauseMenu açmalı.
- Skill Codex sadece PauseMenu içinden veya explicit debug key ile açılmalı.
- Codex close input, tooltip, cursor, modal stack, timeScale restore etmeli.

C) Boot flow'u doğrula.
- MainMenu Start -> CharacterSelect/Chamber olmalı.
- CharacterSelect/Chamber tamamlanmadan _Arena yüklenmemeli.
- Eski duplicate start/chamber/bootstrap sistemi tekrar hortlamış mı kontrol et.
- Sahne içinde stale root veya duplicated bootstrap varsa temizle.

D) Weapon sorting ve mount düzelt.
- Weapon PlayerRoot/SortingGroup altında olmalı.
- WeaponRenderer Entities layer'da, floor/cliff/wall üstünde render olmalı.
- WeaponSlot localPosition yönlere göre doğru olmalı.
- Warblade sword sprite placeholder kötü ise daha uygun sprite ile değiştir veya geçici olarak NoWeapon/hidden yap; cliff altından görünmesi kabul değil.

E) Movement/collider sorunu bul.
- Görsel floor alanı walkable değilse template/collider üretim mismatch'i düzelt.
- Debug log/gizmo ekle:
  - Player hangi collider'a çarpıyor?
  - Walkable polygon nerede?
  - Void/cliff collider nerede?
  - RoomBounds/clamp nerede?

F) Skill icon ve tooltip düzelt.
- Missing icon deterministik log üretmeli.
- Tooltip hover bozulmamalı; TMP text max width ve overflow ayarlı olmalı.
- Panel kapanınca tooltip force-hide.

ACCEPTANCE

- Play'e basınca CharacterSelect/Chamber açılır.
- Skill Codex başlangıçta açılmaz.
- ESC PauseMenu açar, Codex açmaz.
- PauseMenu seçenekleri var: Resume, New Run, Settings, Skill Codex, Exit.
- Codex kapanınca oyun kilitlenmez.
- Tek mouse cursor görünür.
- Weapon floor üstünde, cliff arkasından/altından görünmez.
- Sağ taraftaki floor alanına yürünebilir.
- Görsel floor ile walkable collider eşleşir.
- Normal combat arena en az 24x18/tercihen 28x20 hissi verir ve dash lane içerir.
- Hover'da mavi bozuk text çıkmaz.
- Missing skill iconlar loglanır ve fallback gösterir.

Lütfen sonucu şu formatta raporla:
1. Root causes
2. Değişen dosyalar
3. Testler / smoke sonucu
4. Kalan riskler
5. Kullanıcının tekrar playtest edeceği checklist
