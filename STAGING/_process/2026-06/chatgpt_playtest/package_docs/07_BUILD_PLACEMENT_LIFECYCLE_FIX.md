# BuildPlacementController Cleanup Uyarısı

## Uyarı
```text
Some objects were not cleaned up when closing the scene.
(Did you spawn new GameObjects from OnDestroy?)
BuildPlacementController
```

## En olası zincir

1. Scene unload başlar.
2. Mevcut controller destroy edilir.
3. `_instance` null olur.
4. Başka bir `OnDisable/OnDestroy` içinde `BuildPlacementController.Instance` çağrılır.
5. Lazy getter `new GameObject("BuildPlacementController")` üretir.
6. Unity kapanış sonunda yeni objeyi bulur ve warning verir.

## Projede ara

```text
BuildPlacementController.Instance
new GameObject("BuildPlacementController")
AddComponent<BuildPlacementController>
DontDestroyOnLoad
OnDestroy
OnDisable
OnApplicationQuit
```

## Kalıcı çözüm

- Singleton getter GameObject üretmemeli.
- Controller bootstrap sahnesinde veya açık prefab üzerinden kurulmalı.
- Teardown kodu `.Instance` yerine `TryGetInstance(out var controller)` kullanmalı.
- `OnDestroy` yalnız unsubscribe ve instance clear yapmalı.
- `isQuitting/isShuttingDown` guard olmalı.
- Duplicate instance `Awake` içinde log + destroy edilmeli.

## Domain Reload testleri

Hem Domain Reload açık hem kapalı test et. Static `_instance` reset davranışı farklı olabilir. `RuntimeInitializeOnLoadMethod(SubsystemRegistration)` ile static alan resetlenebilir.
