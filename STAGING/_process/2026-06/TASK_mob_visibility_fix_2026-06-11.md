# TASK: Mob Visibility Fix (2026-06-11)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun
Mob'lar oyun içinde görünmüyor. Prefab sorting layer "Entities"/order=0 doğru set ama runtime'da kayboluyor.

## Bilinen Bulgular

### Bulgu 1 — EnsureVisibleSprite sorting layer set etmiyor
`Assets/Scripts/Enemies/BaseMobBehavior.cs` satır 113-134:
```csharp
private void EnsureVisibleSprite()
{
    if (SR == null) return;
    if (SR.sprite != null && SR.sprite.texture != null) return;
    // ... sprite oluşturuyor ...
    SR.sprite = Sprite.Create(...);
    SR.color = Color.white;
    SR.sharedMaterial = new Material(shader);
    // ← BURAYA sortingLayerName = "Entities" EKSİK
}
```

### Bulgu 2 — PlaceholderSprite.OnEnable sırası
`Assets/Scripts/Utils/PlaceholderSprite.cs` satır 24: `if (sr.sprite != null) return;`
`BaseMobBehavior.Awake()` → `EnsureVisibleSprite()` → sprite set eder → PlaceholderSprite.OnEnable → `sr.sprite != null` → return early.
Bu OK ama EnsureVisibleSprite sorting layer set etmediği için sorun devam eder.

### Bulgu 3 — Spawn durumu bilinmiyor
`EncounterController.cs` mob spawn akışını doğrula: mob'lar gerçekten Instantiate ediliyor mu?

## Yapılacak

### Adım 1: EnsureVisibleSprite fix (BaseMobBehavior.cs)
Satır 128 sonrasına ekle:
```csharp
SR.sortingLayerName = "Entities";
// sortingOrder dokunma — IsoSorter manage ediyor
```

### Adım 2: Spawn doğrulama
`Assets/Scripts/Encounter/EncounterController.cs` oku. Mob Instantiate ve Enable akışını kontrol et. Eğer mob'lar `SetActive(false)` başlıyorsa ve OnEnable-dependent init varsa flag at.

### Adım 3: Mob prefab sorting layer kontrolü
`Assets/Prefabs/Enemies/FractureImp.prefab` açıp SpriteRenderer `m_SortingLayerID` ve `m_SortingLayer` değerlerini doğrula. `m_SortingLayer: 5` = "Entities" olmalı. Yanlışsa düzelt.

### Adım 4: Console'da runtime hata kontrolü
Unity play mode başlat (en az 10 saniye), `read_console` ile ERROR ve WARNING'leri oku. Mob spawn/visibility ile ilgili error varsa kaydet.

## Etkilenen Dosyalar (SADECE BUNLAR)
- `Assets/Scripts/Enemies/BaseMobBehavior.cs` (EnsureVisibleSprite sorting layer fix)
- `Assets/Prefabs/Enemies/FractureImp.prefab` (gerekirse sorting layer fix)
- `Assets/Scripts/Encounter/EncounterController.cs` (sadece oku, BLOCKED değilse fix et)

## BLOCKED Durumları
- Spawn sistemi bambaşka bir yerde çalışıyorsa ve hangi dosya olduğu belirsizse → BLOCKED yaz, bulguyu listele
- EncounterController yoksa → hangi dosya spawn yapıyor bul, bildir

## Başarı Kriteri
1. Play mode'da mob'lar görünür (kırmızı placeholder veya gerçek sprite)
2. Unity console: mob spawn/visibility için error yok
3. `git status` — sadece değiştirilen dosyalar

## Commit
```
fix(enemies): EnsureVisibleSprite sorting layer + mob visibility restore
```
