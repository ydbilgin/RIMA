# Skeleton Animation Tool
*Kaynak: https://www.pixellab.ai/docs/tools/animate-with-skeleton*

## Desteklenen Canvas Boyutları
256×256, 128×128, 64×64, 32×32, 16×16

## İki Kurulum Yöntemi

### 1. Template Skeleton
1. Karakter referansı seç → "Set reference" (skeleton otomatik tahmin eder)
2. Gerekirse Ctrl+Space+E ile skeleton'ı manuel düzelt
3. Template view ve direction ayarla
4. Animasyon template'i seç
5. Template skeleton'ı karakter oranlarına göre düzenle
6. Template insert et, her frame'i düzenle

**Pro tip:** Advanced options → "fixed head" → "always" — yüz tutarlılığını korur.

### 2. Animation-to-Animation
1. Referans karakter + kaynak animasyonu aynı projeye koy
2. Hedef karakterde "Set reference" tıkla
3. Animasyon frame'lerini seç → "Set animation"
4. "Edit skeleton" ile skeleton yerleşimini doğrula/düzelt
5. Scale/position ayarla
6. "Rescale" tıkla, tüm frame'leri gözden geçir

## Üretim Modları

| Mod | Açıklama |
|---|---|
| Freeze 1 → Generate 2 | 1 referans frame dondurulur, model kalanı üretir |
| Custom | Hangi frame'in dondurulacağı, hangisinde inpaint yapılacağı manuel seçilir |
| Freeze 2 → Generate 1 | 2 referans frame → mevcut animasyonu extend etmek için ideal |

## Önerilen İteratif Workflow
1. Skeleton kur (yöntemlerden biri)
2. Referans karakter ayarla
3. View ve direction konfigure et
4. Optimal frozen frame'leri konumlandır
5. İlk animasyonu üret
6. Kaba manuel düzeltmeler yap
7. Önceki frame'leri init image + inpaint mask ile yeniden üret
8. Init image strength'i kademeli artırarak iteratif refine et

## Parametreler
- **Character:** Kamera view, guidance weight
- **Color:** Target palette
- **Init image:** Strength, uygulama ayarları
- **General:** Seed

## Kısıtlama
Minimum Tier 1 abonelik.
