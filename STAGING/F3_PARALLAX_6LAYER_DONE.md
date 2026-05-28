# F3: 6-Katman Parallax Setup — DONE

Tarih: 2026-05-27

## Özet
ParallaxRig rig `PlayableArena_Test01.unity` sahnesine eklendi. 6 child GameObject, her birinde `SpriteRenderer` + `ParallaxLayer.cs` bileşenleri. Placeholder PNG'ler procedural olarak bake edildi.

## Oluşturulan Dosyalar

### Placeholder Sprites (6 adet, 1024×256, Sprite, 64 PPU)
- `Assets/Sprites/Environment/Parallax/Placeholder/BG_Void.png`
- `Assets/Sprites/Environment/Parallax/Placeholder/BG_Far.png`
- `Assets/Sprites/Environment/Parallax/Placeholder/BG_Mid.png`
- `Assets/Sprites/Environment/Parallax/Placeholder/BG_Near.png`
- `Assets/Sprites/Environment/Parallax/Placeholder/Mid_Ground.png`
- `Assets/Sprites/Environment/Parallax/Placeholder/Foreground_Front.png`

Renk şeması: Hades Elysium V1 dark-blue → cyan-glow gradient (arched vertical shimmer).

### Sahne Değişiklikleri
`Assets/Scenes/Test/PlayableArena_Test01.unity` → yeni `ParallaxRig` root GameObject eklendi.

## Layer Konfigürasyonu

| GameObject       | parallaxFactor (X, Y) | sortingOrder | Sprite assigned |
|------------------|-----------------------|-------------|-----------------|
| BG_Void          | (0.05, 0.025)         | -500        | BG_Void.png     |
| BG_Far           | (0.15, 0.075)         | -420        | BG_Far.png      |
| BG_Mid           | (0.30, 0.150)         | -350        | BG_Mid.png      |
| BG_Near          | (0.50, 0.250)         | -300        | BG_Near.png     |
| Mid_Ground       | (0.85, 0.425)         |   10        | Mid_Ground.png  |
| Foreground_Front | (1.10, 0.550)         |  600        | Foreground_Front.png |

Not: Y factoru X'in yarısı (TopDown camera için dikey parallax hafifletildi). Task spec tek factor vermiş, `ParallaxLayer.cs` zaten `Vector2 factor` alıyor — horizontal full, vertical 0.5x uygulama, Sang Hendrix pattern.

## RoomBackgroundRig Durumu
Mevcut `RoomBackgroundRig` (5 eski katman, L0-L4) `activeInHierarchy=false` olarak **korundu** — dokunulmadı. Yeni `ParallaxRig` ayrı root olarak eklendi; çakışma yok.

## Verify Sonuçları
- 0 compile error / 0 warning (console temiz)
- `refresh_unity scope=all mode=force` tamamlandı
- Sahne kaydedildi
- PlayMode: camera hareket ettikçe 6 katman ParallaxLayer.LateUpdate() ile farklı hızda kayar (factor: 0.05 → 1.10 artan sıra)

## Sonraki Adım (F3 Faz 2 Polish)
- Placeholder PNG'leri gerçek Hades Elysium V1 art ile değiştir
- Y factor'ı camera angle'a göre fine-tune et
- Foreground_Front layer için alpha mask / edge fade shader düşün
