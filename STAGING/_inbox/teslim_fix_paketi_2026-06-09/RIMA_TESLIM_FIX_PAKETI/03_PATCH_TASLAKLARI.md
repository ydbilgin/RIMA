# 03 — Patch Taslakları

Bu dosyadaki kodlar körlemesine uygulanacak final patch değildir. Ama Claude Code / Codex için net yön verir.

Gerçek dosyadaki mevcut method yapısına göre uyarlanmalı.

---

# Patch A — HandAnchorAttach: Spawned Weapon Binding

## Amaç

`AttachWeapon()` her çağrıldığında yeni weapon instance:

- Renderer bağlasın.
- OrientationSync'e transform versin.
- Sorting layer'ı body ile eşitlesin.
- Sorting order'ı hemen güncellesin.

## Taslak

```csharp
public void AttachWeapon(string formId)
{
    if (_weaponInstance != null)
        Destroy(_weaponInstance);

    var entry = weaponDatabase?.GetWeapon(classId, formId);
    if (entry?.weaponPrefab == null)
        return;

    _currentEntry = entry;
    _weaponInstance = Instantiate(entry.weaponPrefab, handAnchor);
    _weaponInstance.transform.localPosition = entry.anchorOffset;
    _weaponInstance.transform.localRotation = Quaternion.identity;

    BindSpawnedWeapon();
}

private void BindSpawnedWeapon()
{
    if (_weaponInstance == null)
        return;

    weaponRenderer = _weaponInstance.GetComponentInChildren<SpriteRenderer>(true);

    if (orientationSync != null)
        orientationSync.SetWeaponTransform(_weaponInstance.transform);

    if (bodyRenderer == null)
        bodyRenderer = GetComponentInChildren<SpriteRenderer>(true);

    if (weaponRenderer == null)
    {
        Debug.LogWarning("[HandAnchorAttach] Spawned weapon has no SpriteRenderer.");
        return;
    }

    if (bodyRenderer != null)
    {
        weaponRenderer.sortingLayerID = bodyRenderer.sortingLayerID;
    }
    else
    {
        weaponRenderer.sortingLayerName = "Entities";
        Debug.LogWarning("[HandAnchorAttach] bodyRenderer missing; forced weapon layer to Entities.");
    }

    if (_playerController != null)
    {
        FacingDir8 dir = VectorToDir8(_playerController.FacingDirection);
        UpdateWeaponSortOrder(dir);
    }
}
```

## Start() Temizliği

Eski Start içindeki manuel binding sadeleşmeli:

```csharp
private void Start()
{
    AttachWeapon("Base");
    _lastDir = (FacingDir8)(-1);
}
```

---

# Patch B — OrientationSync: Renderer Always Refresh

## Amaç

Yeni weapon transform geldiğinde renderer da her zaman güncellensin.

## Taslak

```csharp
public void SetWeaponTransform(Transform weapon)
{
    weaponTransform = weapon;
    weaponRenderer = weapon != null
        ? weapon.GetComponentInChildren<SpriteRenderer>(true)
        : null;
}
```

---

# Patch C — OrientationSync: 4 Cardinal Offset Mode

## Amaç

8 yönlü hardcoded offset yerine teslim için 4 ana yönü güvenli kullanmak.

## Taslak

```csharp
[Header("Cardinal Hand Offsets")]
[SerializeField] private bool useCardinalOffsets = true;
[SerializeField] private Vector2 southOffset = new Vector2(0f, -0.08f);
[SerializeField] private Vector2 eastOffset  = new Vector2(0.10f, 0f);
[SerializeField] private Vector2 northOffset = new Vector2(0f, 0.08f);
[SerializeField] private Vector2 westOffset  = new Vector2(-0.10f, 0f);

private Vector2 ResolveHandOffset(FacingDir8 dir)
{
    if (!useCardinalOffsets && handOffsets != null)
    {
        int index = (int)dir;
        if (index >= 0 && index < handOffsets.Length)
            return handOffsets[index];
    }

    switch (dir)
    {
        case FacingDir8.N:
        case FacingDir8.NE:
        case FacingDir8.NW:
            return northOffset;

        case FacingDir8.E:
        case FacingDir8.SE:
            return eastOffset;

        case FacingDir8.W:
        case FacingDir8.SW:
            return westOffset;

        case FacingDir8.S:
        default:
            return southOffset;
    }
}
```

`Sync()` içinde:

```csharp
if (handAnchor != null)
{
    handAnchor.localPosition = ResolveHandOffset(dir);
}
```

---

# Patch D — PenitentSovereign: Boss Death Class Selection

## Amaç

Boss ölünce class selection gerçek demo akışında açılsın.

## Taslak

```csharp
[Header("Demo Flow")]
[SerializeField] private bool suppressClassSelectOnDeath = false;
```

Death handler içinde:

```csharp
private void HandleDeath()
{
    if (dead) return;
    dead = true;

    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    if (!suppressClassSelectOnDeath)
    {
        PlayerClassManager.Instance?.TriggerClassSelection();
    }

    // Burada doğrudan DemoComplete çağırma.
    // RoomRunDirector bu akışı yönetsin.
}
```

Eğer şu an death handler RoomRunDirector’a haber veriyorsa, o haber kalsın ama DemoComplete class seçimi bitmeden gelmesin.

---

# Patch E — RoomRunDirector: Boss Clear Sequence

## Amaç

Boss odası clear olunca önce dual-class, sonra demo complete veya post-boss oda.

## Taslak

```csharp
private IEnumerator HandleBossClearedSequence()
{
    bool needsSecondary =
        PlayerClassManager.Instance != null &&
        PlayerClassManager.Instance.SecondaryClass == ClassType.None;

    if (needsSecondary)
    {
        bool picked = false;

        void OnPicked(ClassType _) => picked = true;

        PlayerClassManager.Instance.OnSecondaryClassSelected += OnPicked;
        PlayerClassManager.Instance.TriggerClassSelection();

        yield return new WaitUntil(() =>
            picked ||
            PlayerClassManager.Instance == null ||
            PlayerClassManager.Instance.SecondaryClass != ClassType.None);

        if (PlayerClassManager.Instance != null)
            PlayerClassManager.Instance.OnSecondaryClassSelected -= OnPicked;
    }

    // DraftManager secondary seçimi sonrası unlock draft açıyorsa bekle.
    if (DraftManager.Instance != null)
    {
        yield return new WaitWhile(() => DraftManager.Instance.IsDraftActive);
    }

    if (CanAdvanceToPostBossNode())
    {
        AdvanceToPostBossNode();
        yield break;
    }

    lifecycle.MarkVictory();
    ShowDemoComplete();
}
```

Gerekli helper'lar dosyanın mevcut graph/door sistemine göre uyarlanmalı.

---

# Patch F — DungeonGraph.BuildDemoSequence: Post-Boss Node

## Amaç

Dual-class seçildikten sonra oyuncuya kısa bir oda daha oynatmak.

## Taslak

```csharp
// Eski:
Combat → Combat → Merchant → Combat → Boss

// Yeni:
Combat → Combat → Merchant → Combat → Boss → Combat
```

Post-boss combat düşük zorlukta olmalı:

- 2 Fracture Imp
- 1 Shard Walker
- kısa oda
- amaç combat challenge değil, secondary sistem gösterimi

---

# Patch G — Guard: DemoComplete Class Selection'ı Ezmesin

## Amaç

Boss ölümü aynı frame içinde hem selection hem complete açmasın.

## Taslak

```csharp
private bool waitingForDualClassReward;

private void CompleteRun()
{
    if (waitingForDualClassReward)
        return;

    // normal demo complete
}
```

veya daha temiz:

```csharp
if (CurrentRoomType == RIMA.RoomType.Boss &&
    PlayerClassManager.Instance != null &&
    PlayerClassManager.Instance.SecondaryClass == ClassType.None)
{
    StartCoroutine(HandleBossClearedSequence());
    return;
}
```
