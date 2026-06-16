# Reward Session — Root Cause ve Kalıcı Çözüm

## Sorun modeli

Mevcut davranış, reward objesinin yalnız bir prefab değil **stateful bir room transaction** olduğunu kodun yeterince ifade etmediğini gösteriyor. `Spawn -> Interact -> Apply -> Cleanup -> Unlock` tek owner altında değilse, önceki odanın objesi kalır ve yeni odanın input'unu çalar.

## Tek owner

`RuntimeRoom` veya `RoomFlowController`, yalnız bir `RewardSession` tutmalı.

```text
None
  -> Spawned
  -> AwaitingInteraction
  -> Inspecting
  -> Applying
  -> Completed
  -> Disposed
```

Her session şu kimlikleri taşır:
- `roomId`
- `rewardSessionId`
- `offerSetId`
- `spawnedObjectIds`
- `selectedRewardId`

## Atomik claim sırası

1. Oyuncu geçerli reward target'a girer.
2. `G` -> current room/session doğrulanır.
3. Inspect/selection UI açılır.
4. Oyuncu seçim yapar.
5. Reward bir kez uygulanır (`ApplyOnce`).
6. Sibling reward objeleri interaction'dan çıkarılır.
7. Objeler hatch/shatter/fade ile kapanır.
8. Session `Completed` olur.
9. Room reward gate açılır.
10. Kapılar açılır.

## Room transition savunması

- `OnRoomExit` içinde aktif session varsa:
  - seçim zorunluysa exit iptal edilir,
  - opsiyonel skip varsa explicit `Discard()` çağrılır.
- `Dispose()` her spawned object, event subscription, input handler ve modal referansını temizler.
- Global manager kullanılıyorsa session data room ID ile doğrulanır; reward objeleri `DontDestroyOnLoad` altında tutulmaz.

## Loglar

Development build:
```text
[Reward] Spawn room=R04 session=17 offers=3
[Reward] Focus target=egg_02 room=R04 session=17
[Reward] OpenInspect reward=RiftForgedEgg
[Reward] Apply reward=... result=Success
[Reward] Dispose session=17 objects=3 listeners=3
```

Geçersiz etkileşim sessizce yutulmasın:
```text
[Reward][ERROR] Interact rejected: target session=16 current session=17
```

## Kapı kuralı

Room mechanics ile uyumlu basit contract:
```text
CombatCleared && RewardResolved -> ExitDoorsOpen
```

Ödül içeriği ileride değişse bile bu contract değişmez.
