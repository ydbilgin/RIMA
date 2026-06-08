RIMA demo scope’u kilitliyoruz: Warblade + Elementalist, 3-4 combat oda, 1 shop, 1 boss, sonra Demo Clear.

Önce P0 bugları düzelt:
1) Play/New Run SkillCodex açmasın, CharacterSelect/Chamber açsın.
2) ESC SkillCodex değil PauseMenu açsın.
3) Overlay kapanınca Time.timeScale/input kilitlenmesin.
4) Tek cursor kalsın.
5) Warblade sword PlayerRoot/SortingGroup altında olsun, cliff/floor altında kalmasın.
6) Elementalist staff taşımasın, sağ avuç üstünde floating golden rune disc olsun.
7) Görsel floor = walkable olsun, görünmez sağ duvar bug’ını collider debug ile bul.
8) Canlı oda akışı `_Arena → RoomRunDirector → IsoRoomBuilder`; legacy `RuntimeRoomManager/RoomLoader/DoorTrigger/GateBehavior` tarafına patch yazma.

Demo sequence:
Combat_Small_Intro → Combat_Medium → Shop_01 → Combat_PreBoss → Boss_PenitentSovereign_Demo → DemoClear.

Test:
Warblade 3 full run, Elementalist 2 full run. 0 compile error, 0 exception, 0 softlock.
