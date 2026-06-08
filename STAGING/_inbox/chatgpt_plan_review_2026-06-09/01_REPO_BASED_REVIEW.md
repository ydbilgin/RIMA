# Repo Based Review

## Net verdict

Plan gerçekçi, ama bir şartla: önce "lineer demo modu" kodda ayrı bir yol olarak kilitlenmeli. Şu an plan metnindeki combat→combat→shop→combat→boss akışı tasarım olarak doğru, fakat mevcut DungeonGraph generator random/branching mantığında çalışıyor. Demo için bu akışı generator'a bırakmak hata olur.

## En kritik repo bulguları

1. Canlı yol `_Arena → RoomRunDirector → IsoRoomBuilder`.
2. DemoRoomBank içinde merchantRooms boş.
3. DungeonGraph.Generate şu an depth'e göre Combat/Boss/random Combat-Elite-Chest üretiyor. Merchant/Shop deterministik akışın içinde yok.
4. RoomRunDirector hâlâ FitCameraToRoom çağırıyor.
5. EncounterController temel olarak iyi: wave, ikinci dalga, active enemy reconcile ve OnRoomCleared var.

## Önerilen demo mimarisi

Yeni demo modu:

```csharp
[SerializeField] private bool forceDemoSequence = true;

private static readonly RoomType[] DemoSequence =
{
    RoomType.Combat,
    RoomType.Combat,
    RoomType.Merchant,
    RoomType.Combat,
    RoomType.Boss
};
```

Bu sequence, DungeonGraph.Generate yerine demo modunda doğrudan lineer graph üretmeli. Her node sadece bir child'a bağlanmalı.

## Yapılmaması gerekenler

- Random graph içinde seed kovalamak.
- Eski RoomLoader hattına shop/boss bağlamak.
- UI makyajı ile softlock'u örtmek.
- 10 class silah sistemini demo öncesi açmak.
- Shop'u inventory sistemine aşırı bağlamak.
