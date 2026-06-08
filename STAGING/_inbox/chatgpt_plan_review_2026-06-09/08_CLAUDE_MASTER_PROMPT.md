# Claude Master Prompt

RIMA repo için DEMO_MASTER_PLAN_2026-06-09.md planını uygula ama önce aşağıdaki review kararlarını dikkate al.

CANLI MİMARİ:
- Patch hedefi: `_Arena → RoomRunDirector → IsoRoomBuilder`.
- Eski RoomLoader / RuntimeRoomManager / DoorTrigger / GateBehavior hattına yeni iş bağlama.

DEMO HEDEF:
- Warblade + Elementalist oynanabilir.
- Diğer sınıflar chamber'da kilitli görünür.
- Akış: MainMenu → Chamber → Combat → Combat → Shop → Combat → Boss → Victory/Death.
- Branching yok. Zorunlu lineer.
- 0 stuck, 0 console error.

ÖNEMLİ REPO BULGULARI:
- DemoRoomBank içinde merchantRooms şu an boş. Shop eklemek için Merchant template ve bank ref şart.
- DungeonGraph.Generate random/branching çalışıyor ve Merchant üretmiyor. Demo için ayrı deterministic linear generator veya forced sequence ekle.
- RoomRunDirector hâlâ FitCameraToRoom yapıyor. Demo fixed camera için useFixedDemoCamera + fixedOrthographicSize ekle.
- EncounterController wave/clear temeli var. Softlock muhtemelen RoomRunDirector clear/reward/door lifecycle tarafında.

IMPLEMENTATION PLAN:
1. Softlock lifecycle fix.
2. Demo forced sequence.
3. Fixed camera.
4. Two class lock.
5. PauseMenu.
6. Shop.
7. Boss placeholder → real telegraphs.
8. Tests.

ACCEPTANCE:
- MainMenu'den Victory'ye kadar 2 sınıfla gidiliyor.
- Console 0 error.
- Softlock yok.
- Shop satın alma çalışıyor.
- Boss ölünce Victory geliyor.
- ESC pause açıyor, codex açmıyor.
