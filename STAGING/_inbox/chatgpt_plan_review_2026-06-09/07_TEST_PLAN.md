# Test Plan

## EditMode
- Demo sequence tam 5 node üretir.
- Sequence sırası Combat, Combat, Merchant, Combat, Boss.
- Her node tek child'a sahiptir.
- Boss node child içermez.
- DemoRoomBank içinde Merchant room ref vardır.
- Combat/Boss/Shop template null değildir.
- Her template minimum 1 player spawn ve 1 exit slot içerir.

## PlayMode smoke
- BeginRun demo mode
- Combat clear simulate
- Reward taken simulate
- Door open
- Advance next
- 5 oda boyunca loop
- Boss death → Victory

## Manual playtest
Warblade full run.
Elementalist full run.
Boss death path.
Pause test her odada.

Fail koşulları:
- Console error
- NullReference
- softlock
- stuck door
- timeScale 0 kaçak
- oyuncunun floor dışına çıkması
