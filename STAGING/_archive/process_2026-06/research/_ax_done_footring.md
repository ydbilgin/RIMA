# CharSelect v3.3 Mini: Procedural Foot Ring Implementation

## Değişen Dosyalar ve Satır Aralıkları
### 1. `Assets/Scripts/UI/RimaUITheme.cs`
- **Satır 271-276**: `ProceduralFootRing` static property eklendi. `MakeEllipseRing(160, 100, 64f, 6.0f, 8.0f)` ile 160x100 ebatlarında AA ellipse üretimi cache'lendi.
- **Satır 427-500**: `MakeEllipseRing` helper metodu eklendi. Matematiksel olarak $height = width \times 0.61$ iso oranını koruyacak şekilde elips hesaplaması yapar, iç halka kalınlığını 6px ve dış yumuşak glow falloff'u 8px olarak ayarlar.

### 2. `Assets/Scripts/UI/CharacterSelectScreen.cs`
- **Satır 31**: Eski `PackPedestal` (sprite tabanlı halka görseli) referansı kaldırıldı.
- **Satır 79**: Yeni `[SerializeField] private float footRingWidth = 160f;` alanı eklendi.
- **Satır 482-500**: Hover halkası (`glow`) yeni boyutu (`footRingWidth`, `footRingWidth * 0.61f`) ve procedural halka sprite'ı (`RimaUITheme.ProceduralFootRing`) ile güncellendi. Eski sprite tabanlı `PedestalSeal` kaldırıldı (kodda `seal = null` yapıldı). `BuildSelectionVfx` çağrısına `footRingWidth` geçirildi.
- **Satır 614-647**: `BuildSelectionVfx` metodu dynamic characterSize yerine `ringWidth` parametresi alacak şekilde güncellendi. `SelectionFootRing` sprite'ı procedural sprite ile değiştirildi ve boyutu sabitlendi.

## Build Çıktısı Özeti
- `dotnet build RIMA.Runtime.csproj` başarıyla tamamlandı.
- **Hatalar (Errors):** 0
- **Uyarılar (Warnings):** 95 (önceden de var olan diğer script uyarıları, yeni hata veya uyarı eklenmedi).

## Bilinen Kısıtlar / Notlar
- `PedestalSeal` ve `SelectionGlowDisk` görselleri tamamen devre dışı bırakıldı/kaldırıldı, çünkü procedural halka tek başına temiz bir seçim görseli sunuyor.
- Point-filter kullanılmadı, Bilinear olarak bırakılarak elipsin yumuşak kenarlı (AA) ve glow'lu çıkması sağlandı.
- Halka boyutu artık tüm karakterlerde sabit (kutularının FIT ölçeklerinden bağımsız) ve ayak noktasına (`footY`) tam anchor'lanmış durumdadır.
