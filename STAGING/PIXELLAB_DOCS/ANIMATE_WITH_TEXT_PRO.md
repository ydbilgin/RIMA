# Animate with Text Pro
*Kaynak: https://www.pixellab.ai/docs/tools/animate-with-text-pro*

## Özet
Tek bir karakter sprite'ından text prompt ile animasyon frame'leri üretir.

## Workflow
1. Karakter referans image'ı yükle
2. Animasyon action'ını describe et (walk, jump, attack...)
3. Kamera view ve yönü belirt (opsiyonel)
4. Frame'leri otomatik üret

## Frame Çıktı Tablosu

| Input Boyutu | Çıktı | Maliyet |
|---|---|---|
| 32×32 veya 64×64 | **16 frame** (4×4 grid) | 20 gen |
| 65–128px | **4 frame** (2×2 grid) | 20 gen |
| 129–170px | **4 frame** (2×2 grid) | 25 gen |
| 171–256px | **4 frame** (2×2 grid) | 40 gen |

⚠️ **RIMA Notu:** 128px sprite → sadece 4 frame. 64px → 16 frame (daha akıcı animasyon).

## Parametreler
- **Reference Image:** Karakter sprite
- **Action:** Animasyon tipi (walk, attack, idle, death...)
- **View:** Kamera açısı (top-down varyantları / sidescroller)
- **Direction:** Karakter yönü (cardinal/diagonal)
- **No Background:** Transparent bg toggle
- **Seed:** Reproducibility

## Kısıtlamalar
- Max reference image: **256×256 px**
- Tier 1 abonelik zorunlu
- Aseprite, Pixelorama, Creator ile uyumlu
