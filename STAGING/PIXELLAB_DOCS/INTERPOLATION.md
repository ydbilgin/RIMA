# Interpolation Tool
*Kaynak: https://www.pixellab.ai/docs/tools/interpolation*

## Özet
İki keyframe arasındaki ara frame'leri üretir. Animasyon pipeline'ını hızlandırır.

## ⚠️ KRİTİK KISITLAMA
**Canvas: kesinlikle 64×64 px**

128×128 sprite → interpolation çalışmaz.

**RIMA Etkisi:**
Production Playbook Adım 21c (Walk Cycle) ve Adım 22b/c (Attack) interpolation pipeline kullanıyor.
128px karakter sprite'ları bu tool ile doğrudan çalışmaz.

**Çözüm Seçenekleri:**
1. Karakter üretimini 64×64'te yap → 16 frame (daha iyi animasyon) → Unity'de scale up
2. 128×128 için skeleton animation kullan (interpolation yerine)
3. 64×64 çalışma → Aseprite'ta 2x nearest-neighbor upscale → 128×128

## Parametreler

### Intermediate & Reference
- Intermediate frame guidance weight
- Reference keyframe guidance weight

### Character
- Character description
- Negative description
- Action description
- Camera view
- Directional orientation
- Character guidance weight

### Color
- Target palette

### General
- Seed

## Workflow Uygulaması
```
1. A Keyframe üret (extreme pose)
2. B Keyframe üret (opposite extreme)
3. Interpolate: A → B → smooth ara frame'ler
4. Sonuç: akıcı walk/attack cycle
```
