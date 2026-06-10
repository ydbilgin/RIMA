# 01 — P0 Fix: Warblade Greatsword Render / Attach Bug

## Sorun

Warblade'in greatsword'u:

1. Harita/zemin altında render olabiliyor.
2. Ele düzgün oturmuyor.
3. Yön değişimlerinde/swing sırasında pozisyon ve layer tutarlılığı bozulabiliyor.

## Kök Neden Adayları

### A) Sorting Layer Binding Start() içinde kalmış

`HandAnchorAttach.Start()` içinde:

```csharp
AttachWeapon("Base");
...
if (weaponRenderer != null && bodyRenderer != null)
{
    weaponRenderer.sortingLayerID = bodyRenderer.sortingLayerID;
}
```

Bu sadece Start sırasında güvenli.

Ama public `AttachWeapon(string formId)` sonradan çağrılırsa:

- Eski weapon destroy edilir.
- Yeni prefab instantiate edilir.
- Fakat yeni weapon için:
  - weaponRenderer yeniden garanti şekilde bulunmayabilir.
  - sortingLayerID yeniden kopyalanmayabilir.
  - OrientationSync yeni transform'a yeniden bağlanmayabilir.
  - `UpdateWeaponSortOrder()` sadece sortingOrder günceller, sortingLayer güncellemez.

Eğer weapon prefab Default sorting layer'da kalırsa ve layer sırası `Default / Floor / Entities` ise sword Floor'un altında kalabilir.

## Minimal Fix Prensibi

Layer/renderer/transform binding tek bir helper'a taşınmalı:

```text
AttachWeapon()
  → Instantiate
  → BindSpawnedWeapon()
```

Böylece Start'ta da, runtime weapon swap'te de aynı garanti verilir.

## Hedef Davranış

Her `AttachWeapon()` sonrası:

- `_weaponInstance` valid.
- `weaponRenderer` yeni instance'dan bulunmuş.
- `orientationSync.weaponTransform` yeni instance'a bağlanmış.
- `bodyRenderer` null ise fallback ile bulunmuş.
- `weaponRenderer.sortingLayerID` gövdeyle aynı ya da fallback olarak "Entities".
- `weaponRenderer.sortingOrder` gövde order'ına göre güncel.
- Sword hiçbir zaman Floor altında kalmıyor.

## Önerilen Kod Değişikliği

### HandAnchorAttach.cs

`AttachWeapon()` içine binding helper ekle.

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

    if (weaponRenderer != null)
    {
        if (bodyRenderer != null)
        {
            weaponRenderer.sortingLayerID = bodyRenderer.sortingLayerID;
        }
        else
        {
            weaponRenderer.sortingLayerName = "Entities";
            Debug.LogWarning("[HandAnchorAttach] bodyRenderer missing; weapon sorting layer forced to Entities.");
        }

        if (_playerController != null)
        {
            FacingDir8 dir = VectorToDir8(_playerController.FacingDirection);
            UpdateWeaponSortOrder(dir);
        }
    }
}
```

### Start() sadeleşmeli

Start içinde layer kopyalama tekrar yazılmamalı. Çünkü helper zaten yapmalı.

```csharp
private void Start()
{
    AttachWeapon("Base");
    _lastDir = (FacingDir8)(-1);
}
```

Eğer Start içinde ek güvenlik istenirse:

```csharp
BindSpawnedWeapon();
```

ama normalde AttachWeapon zaten çağırdığı için gerek yok.

## OrientationSync Fix

Şu an `SetWeaponTransform()` içinde weaponRenderer yalnızca null ise bulunuyorsa, eski renderer kalabilir.

Daha güvenli hali:

```csharp
public void SetWeaponTransform(Transform weapon)
{
    weaponTransform = weapon;
    weaponRenderer = weapon != null
        ? weapon.GetComponentInChildren<SpriteRenderer>(true)
        : null;
}
```

Bu özellikle weapon swap / destroy / reinstantiate durumlarında önemli.

## Ele Oturmama Problemi

### Muhtemel neden

`OrientationSync.handOffsets` 8 yönlü hardcoded:

```csharp
new Vector2(0.00f, -0.08f),
new Vector2(0.08f, -0.04f),
new Vector2(0.10f, 0.00f),
new Vector2(0.07f, 0.05f),
new Vector2(0.00f, 0.08f),
new Vector2(-0.07f, 0.05f),
new Vector2(-0.10f, 0.00f),
new Vector2(-0.08f, -0.04f)
```

Bu sistem eski 8-yön mantığına göre. RIMA'nın güncel üretim/animasyon kararı 4 cardinal yön: S/E/N/W. Runtime 8 movement olabilir ama sprite yönü 4 ana yöne indirgenmeli.

Bu yüzden 8 yönlü offset, feet-pivot kalibrasyonundan sonra bayat olabilir.

## Teslim İçin Minimal Çözüm

### Seçenek A — En hızlı

`handOffsets` değerlerini Inspector'da Warblade için yeniden kalibre et.

Önerilen test:
- S yönünde idle: sword grip eli kesiyor mu?
- E yönünde idle: sword elden kopuyor mu?
- N yönünde sword gövdenin arkasına geçiyor mu?
- W yönünde flip doğru mu?
- Attack sırasında swing center doğru mu?

### Seçenek B — Daha temiz

OrientationSync içinde 8 yön yerine 4 cardinal offset kullan:

```csharp
[SerializeField] private Vector2 southOffset = new Vector2(0f, -0.08f);
[SerializeField] private Vector2 eastOffset  = new Vector2(0.10f, 0f);
[SerializeField] private Vector2 northOffset = new Vector2(0f, 0.08f);
[SerializeField] private Vector2 westOffset  = new Vector2(-0.10f, 0f);

private Vector2 ResolveCardinalOffset(FacingDir8 dir)
{
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

Sonra `Sync()` içinde:

```csharp
if (handAnchor != null)
    handAnchor.localPosition = ResolveCardinalOffset(dir);
```

## Kabul Kriterleri

Bu fix başarılı sayılırsa:

- Warblade sword hiçbir odada zemin altında kalmaz.
- Sword her zaman Entities layer'da ya da bodyRenderer ile aynı layer'dadır.
- Oyuncu hareket ederken weapon sortingOrder gövdeyi takip eder.
- N/NE/NW yönlerinde sword gövdenin arkasına geçebilir ama Floor altına düşmez.
- S/E/W yönlerinde sword body üstünde okunur.
- Runtime `AttachWeapon("Base")` veya başka form çağrısı sonrası layer bozulmaz.
