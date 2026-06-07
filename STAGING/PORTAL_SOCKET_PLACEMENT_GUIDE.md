# Portal Soket Yerleşim Kılavuzu

Canlı oda yolu `_Arena -> RoomRunDirector -> IsoRoomBuilder.BuildExitDoors` zinciridir. `RoomLoader`, `Gate.cs`, `DoorTrigger` ve `GateBehavior` bu T3 wiring işi için kullanılmaz.

## Soket Kararı

- Slot 0 / NW: Açılı kemer kullanır.
- Slot 1 / N: Frontal kemer kullanır.
- Slot 2 / NE: Aynı açılı kemer kullanılır, sadece kemer gövdesinde `flipX` çalışır.

Rün, badge veya label flip edilmez. Bunlar root altında ayrı child olarak kalır; sadece `Visual` child'ındaki kemer renderer'ı flip edilir.

## Çıkış Sayısı

- 1 çıkış: N slotu, frontal görsel.
- 2 çıkış: NW + NE slotları, iki açılı görsel. NE tarafında yalnız kemer gövdesi mirror edilir.
- 3 çıkış: NW + N + NE slotları.
- Boss çıkışı: center-only N slotu, boss frontal kemer.

## Tip Eşlemesi

- Combat: Combat portal.
- Elite: Elite portal v2.
- Chest/Reward: Chest portal.
- Boss: Boss frontal portal.
- Diğer tipler: Combat portalına düşer.

Kod tarafındaki kavramsal `EXIT_NW`, `EXIT_N`, `EXIT_NE` adları `RoomTemplateSO` içinde `door_NW_01`, `door_N_01`, `door_NE_01` köprüsüyle temsil edilir.
