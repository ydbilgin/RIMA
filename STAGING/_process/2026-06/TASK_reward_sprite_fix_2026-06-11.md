# TASK: Reward Pickup Sprite Fix (2026-06-11)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun
Ödül pickup sprite'ı oyunda çok küçük görünüyor — zemin decal boyutunda.

## Kök Neden
1. `DefaultRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest"` — bu bir multi-sprite sheet.
   `Resources.Load<Sprite>("UI/RIMA/RIMA_UI_Node_Chest")` multi-sprite texture'da **null döner**.
   Kod null'ı `edge_filler_rift_shard` fallback'ine düşürüyor (küçük zemin decal = ~0.5 world unit).

2. `RIMA_UI_Node_Chest.png.meta`:
   - `spritePixelsToUnits: 100` — proje standardı 64
   - `filterMode: 1` (Bilinear) — pixel art = 0 (Point) olmalı

3. Sprite sheet'te iki sub-sprite var:
   - `RIMA_UI_Node_Chest_0`: 10×36 px (çok küçük, yanlış)
   - `RIMA_UI_Node_Chest_1`: 62×72 px (doğru — chest ikonu)

## Yapılacak

### Fix 1: DefaultRewardSpritePath güncelle (RoomRunDirector.cs)
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` satır 127:
```csharp
// ÖNCE:
private const string DefaultRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest";
// SONRA:
private const string DefaultRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest_1";
```

### Fix 2: Texture import ayarları (.meta dosyası)
`Assets/Resources/UI/RIMA/RIMA_UI_Node_Chest.png.meta`:
- `spritePixelsToUnits: 100` → `spritePixelsToUnits: 64`
- `filterMode: 1` → `filterMode: 0`
- Her iki sub-sprite'ın pivot'u `{x: 0.5, y: 0.5}` olmalı (şu an `{x: 0, y: 0}` — merkez pivot yok)

### Fix 3: RewardPickup.cs fallback de güncelle (satır 38)
```csharp
// ÖNCE:
Sprite riftShard = Resources.Load<Sprite>("Props/edge_filler_rift_shard");
// SONRA:
Sprite riftShard = Resources.Load<Sprite>("UI/RIMA/RIMA_UI_Node_Chest_1");
```
Eğer bu da null dönerse sadece log at, başka fallback ekleme.

## Etkilenen Dosyalar (SADECE BUNLAR)
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` (satır 127)
- `Assets/Scripts/Core/RewardPickup.cs` (satır 38)
- `Assets/Resources/UI/RIMA/RIMA_UI_Node_Chest.png.meta` (PPU + filterMode + pivot fix)

## Kapsam Dışı
- Kırmızı kutu = Unity editor gizmo (collider bounds), build'de görünmez, dokunma
- Sprite sanat değişikliği yok
- Reward sistemi mekanik değişikliği yok

## Başarı Kriteri
1. Play mode: ödül ikonu görünür boyutta (karakter sprite'ının ~yarısı kadar)
2. Unity console: "RIMA_UI_Node_Chest_1" yüklendi logu (veya RewardPickup spawn logu hata içermiyor)
3. Compile error yok

## Commit
```
fix(reward): chest sprite load fix — multi-sprite path + PPU 64 + point filter
```
