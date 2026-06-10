# 05 — Claude Code / Codex Tek Prompt

Aşağıdaki metni Claude Code, Codex veya Antigravity içine tek parça at.

```text
RIMA Unity 6 LTS 2D roguelite repo’sunda teslim öncesi iki P0 blocker fix’i gerçek kod üzerinden uygula veya uygulanabilir patch çıkar. Varsayım yapma; dosyaları oku, file:line kanıtla, sonra minimal fix yap. Büyük refactor istemiyorum. Hedef full game değil, bitirme projesi için çalışan vertical slice.

Repo: ydbilgin/RIMA
Branch: master

GENEL TESLİM HEDEFİ
Şu an hedef:
Combat → Combat → Merchant → Combat → Boss
Buna mümkünse:
→ Secondary Class Selection → Unlock Draft → PostBossCombat → DemoComplete
eklenecek.

PostBossCombat yetişmezse minimum:
Boss → Secondary Class Selection → Unlock Draft → DemoComplete
olmalı.

P0-1 — Warblade greatsword render/attach bug

Belirti:
- Warblade sword bazen floor/map altında render oluyor.
- Sword ele düzgün oturmuyor.

İncele:
- Assets/Scripts/Systems/Combat/HandAnchorAttach.cs
- Assets/Scripts/Combat/OrientationSync.cs
- Assets/Scripts/Core/IsoSorter.cs
- Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs
- ilgili prefab/weapon database asset referansları

Doğrula:
1. HandAnchorAttach.Start() içinde AttachWeapon("Base") sonrası renderer/layer binding var mı?
2. Public AttachWeapon(string formId) sonradan çağrıldığında yeni weapon instance için:
   - weaponRenderer yeniden bulunuyor mu?
   - OrientationSync.SetWeaponTransform yeniden çağrılıyor mu?
   - bodyRenderer null ise fallback var mı?
   - weaponRenderer.sortingLayerID bodyRenderer’dan yeniden kopyalanıyor mu?
   - UpdateWeaponSortOrder sadece sortingOrder mı set ediyor?
3. Runtime’da weapon prefab Default layer’da kalıyorsa Floor layer altında kalma kök nedeni bu mu?
4. OrientationSync.handOffsets 8 yönlü hardcoded ve güncel 4-cardinal pipeline’a göre bayat mı?

Uygula:
- HandAnchorAttach içinde BindSpawnedWeapon() helper ekle.
- AttachWeapon() sonunda BindSpawnedWeapon() çağır.
- Start() içindeki ayrı layer binding’i kaldır veya helper’a yönlendir.
- BindSpawnedWeapon:
  - weaponRenderer = _weaponInstance.GetComponentInChildren<SpriteRenderer>(true)
  - orientationSync.SetWeaponTransform(_weaponInstance.transform)
  - bodyRenderer null ise GetComponentInChildren<SpriteRenderer>(true)
  - weaponRenderer.sortingLayerID = bodyRenderer.sortingLayerID
  - bodyRenderer yoksa weaponRenderer.sortingLayerName = "Entities"
  - current facing dir ile UpdateWeaponSortOrder çağır
- OrientationSync.SetWeaponTransform yeni weapon geldiğinde weaponRenderer’ı her zaman güncellesin, sadece null ise değil.
- Hand offset için teslim öncesi minimal çözüm: 4 cardinal offset mode ekle veya mevcut 8 offsetleri S/E/N/W kalibrasyonuna indir.

Kabul:
- Sword hiçbir odada floor altında kalmayacak.
- AttachWeapon runtime’da tekrar çağrılsa da layer bozulmayacak.
- Sword yeni spawned renderer’a bağlı olacak.
- Sword S/E/N/W yönlerinde ele yakın duracak.

P0-2 — Dual-class demo flow

Belirti:
- PlayerClassManager, ClassSelectionTrigger, SkillBarUI, DraftManager tarafında secondary class sistemi var.
- Ama boss ölümü demo complete/victory akışına gidiyorsa dual-class gerçek demo akışında erişilebilir değil.
- PenitentSovereign içinde suppressClassSelectOnDeath default true olabilir.

İncele:
- Assets/Scripts/Systems/PlayerClassManager.cs
- Assets/Scripts/Systems/ClassSelectionTrigger.cs
- Assets/Scripts/Enemies/Boss/PenitentSovereign.cs
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
- Assets/Scripts/UI/SkillBarUI.cs
- Assets/Scripts/Skills/DraftManager.cs
- SkillOfferGenerator ve ClassSelectionUI varsa onları da kontrol et.

Doğrula:
1. Boss death path gerçek runtime’da PlayerClassManager.TriggerClassSelection() çağırıyor mu?
2. suppressClassSelectOnDeath true ise class selection bastırılıyor mu?
3. RoomRunDirector boss sonrası doğrudan MarkVictory/DemoComplete yapıyor mu?
4. Secondary seçilse bile oynanacak post-boss oda var mı?
5. DraftManager OnSecondaryClassSelected sonrası unlock draft açıyor mu?
6. SkillBarUI OnSecondaryClassSelected sonrası controller re-resolve ediyor mu?
7. Secondary selection sadece T test tuşuyla mı erişilebilir?

Uygula:
- Boss death akışında class selection gerçek flow’a bağlansın.
- suppressClassSelectOnDeath teslim build için false olmalı veya field kaldırılmalı.
- Boss ölünce PlayerClassManager.Instance.TriggerClassSelection() çağrılmalı.
- DemoComplete class selection’dan önce çıkmamalı.
- RoomRunDirector boss clear sequence:
  1. boss clear
  2. secondary class selection bekle
  3. DraftManager unlock draft varsa bitmesini bekle
  4. post-boss oda varsa geç
  5. yoksa DemoComplete göster
- Mümkünse DungeonGraph.BuildDemoSequence:
  Combat → Combat → Merchant → Combat → Boss → Combat
  olacak şekilde kısa post-boss combat node ekle.
- Post-boss oda düşük zorlukta olsun; amaç challenge değil secondary skill’i göstermek.

Kabul:
- T tuşuna basmadan boss ölünce secondary class selection açılır.
- DemoComplete selection’ı ezmez.
- Secondary seçildikten sonra slotlar/draft/UI güncellenir.
- Post-boss oda varsa secondary skill kullanılabilir.
- Console error yok.

ÇIKTI FORMATIN:
1. Bulduğun kök nedenleri file:line ile yaz.
2. Uyguladığın değişiklikleri özetle.
3. Değişen dosyaları listele.
4. Manual test checklist ver.
5. Eğer bir fix’i uygulayamadıysan nedenini ve minimal manuel adımı yaz.
6. Büyük refactor yapma; teslimi riske sokma.
```
