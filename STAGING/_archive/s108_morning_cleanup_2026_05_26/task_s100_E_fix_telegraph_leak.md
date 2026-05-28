ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Not needed for this fix.

# Amaç: Task E Fix — TelegraphGroundMarker static Texture2D/Sprite memory leak

rima-qc bulgusu: `TelegraphGroundMarker.cs` içindeki `private static Sprite generatedCircleSprite` ve buna bağlı `Texture2D`, Editor domain reload'da native object leak yapıyor.

## Düzeltilecek dosya

`Assets/Scripts/Enemy/Telegraph/TelegraphGroundMarker.cs`

## Fix 1 — SubsystemRegistration cleanup

Static field'ı domain reload'da temizle:

```csharp
#if UNITY_EDITOR
[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
static void ClearStaticCache()
{
    if (generatedCircleSprite != null)
    {
        UnityEngine.Object.DestroyImmediate(generatedCircleSprite.texture);
        UnityEngine.Object.DestroyImmediate(generatedCircleSprite);
        generatedCircleSprite = null;
    }
}
#endif
```

Bu metodu class içine, static field'ın hemen altına ekle.

## Fix 2 — sortingLayerName magic string

`[SerializeField] private string sortingLayerName = "VFX"` satırını bul.

Üstüne tooltip ekle veya bir const string ile koruma sağla:
```csharp
private const string DefaultVfxSortingLayer = "VFX";
[SerializeField] private string sortingLayerName = DefaultVfxSortingLayer;
```
(İsteğe bağlı — minor improvement)

## Fix 3 — EnemyTelegraph ScreenShakeDriver cache

`Assets/Scripts/Enemy/Telegraph/EnemyTelegraph.cs` içinde `FindFirstObjectByType<ScreenShakeDriver>()` her `StartTelegraph` çağrısında çalışıyor.

`Awake()` metodunda bir kez cache'le:
```csharp
private ScreenShakeDriver _shakeDriver;

private void Awake()
{
    // ... mevcut kod ...
    _shakeDriver = FindFirstObjectByType<ScreenShakeDriver>();
}
```
Sonra `StartTelegraph` içindeki `FindSceneObject<ScreenShakeDriver>()` çağrısını `_shakeDriver` ile replace et.

## Başarı Kriterleri
- [ ] `ClearStaticCache` static method TelegraphGroundMarker'da var
- [ ] EnemyTelegraph'ta `_shakeDriver` Awake'de cache'leniyor
- [ ] `dotnet build RIMA.Runtime.csproj`: 0 errors
- [ ] Console 0 error
