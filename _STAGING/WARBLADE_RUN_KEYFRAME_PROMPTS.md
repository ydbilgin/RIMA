# WARBLADE RUN Keyframe Promptları (Create from Reference)

Bu workflow, koşu üretimi için **Animate with text NEW** yerine **Create from Reference** kullanır.
Her yön için **bir adet kilit kare** üretilir (tam animasyon döngüsü değil), ardından koşu döngüsü kareleri oluşturmak için **Aseprite Interpolate** kullanılır.

## Ayarlar

| Alan | Değer |
|---|---|
| Araç | Create from Reference |
| Referans 1 | Characters/Warblade/base/warblade_base_[YÖN].png (yöne özel base sprite) |
| Referans 2 | TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png |
| Boyut | 128px |
| Çıktı | Yön başına 1 kilit kare (tekil görsel, animasyon değil) |
| Outline | single color black |
| Shading | medium |
| Detail | medium |
| AI Freedom | 0 |
| Preset | male human |

## Yön Promptları

### SE
Referans 1: `Characters/Warblade/base/warblade_base_SE.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing southeast, mid-stride diagonal lean toward lower-right, right foot forward left foot pushing off, sword carried low trailing behind right side blade fully readable not merged with body, both hands on same long hilt right hand near crossguard left hand near pommel, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_SE.png`

### E
Referans 1: `Characters/Warblade/base/warblade_base_E.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing east, mid-stride strong side lean body angled east, right foot forward left foot pushing off, sword tucked behind right hip blade fully readable extending past body silhouette, both hands on same long hilt right hand near crossguard left hand near pommel, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_E.png`

### S
Referans 1: `Characters/Warblade/base/warblade_base_S.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing south, mid-stride slight forward lean toward viewer, one foot forward one foot behind, sword dragging low behind center mass blade fully readable not overlapping legs, both hands on same long hilt right hand near crossguard left hand near pommel, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_S.png`

### NE
Referans 1: `Characters/Warblade/base/warblade_base_NE.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing northeast, mid-stride torso angling away upper-right, right foot forward left foot pushing off, sword carried at side high transitioning downward blade clearly visible beside torso not behind it, both hands on same long hilt right hand near crossguard left hand near pommel two-hand spacing clearly visible, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_NE.png`

### N
Referans 1: `Characters/Warblade/base/warblade_base_N.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing north back-facing, mid-stride back visible shoulder pump, one foot forward one foot behind, sword offset to right side blade clearly visible beside spine not overlapping or merged with back, both hands on same long hilt right hand near crossguard left hand near pommel two-hand spacing clearly visible, heavy wide blade clear silhouette extending to right of body outline, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_N.png`

### SW
Referans 1: `Characters/Warblade/base/warblade_base_SW.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing southwest, mid-stride diagonal lean toward lower-left, left foot forward right foot pushing off, sword carried low trailing behind left side blade fully readable not merged with body, both hands on same long hilt right hand near crossguard left hand near pommel, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_SW.png`

### W
Referans 1: `Characters/Warblade/base/warblade_base_W.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing west, mid-stride strong side lean body angled west, left foot forward right foot pushing off, sword tucked behind left hip blade fully readable extending past body silhouette, both hands on same long hilt right hand near crossguard left hand near pommel, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_W.png`

### NW
Referans 1: `Characters/Warblade/base/warblade_base_NW.png`
Referans 2: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warrior_running_128.png`
Prompt: `warblade heavy knight running key frame, facing northwest, mid-stride torso angling away upper-left, left foot forward right foot pushing off, sword carried at side high transitioning downward blade clearly visible beside torso not behind it, both hands on same long hilt right hand near crossguard left hand near pommel two-hand spacing clearly visible, heavy wide blade clear silhouette, top-down ARPG gameplay camera slightly tilted overhead around 60-degree downward view, pixel art sprite 128px`
Çıktı dosyası: `warblade_run_KF_NW.png`

## QC Kontrol Listesi (Tekil Kilit Kare)

- [ ] Kılıç bıçak silüeti okunabilir, gövdeyle birleşmemiş
- [ ] İki el tutuşu görünür: sağ el crossguard yakınında, sol el pommel yakınında, aynı kabzada
- [ ] Kılıç ölçeği base sprite ile tutarlı
- [ ] Orta adım pozu net: ağırlık öne, bir ayak ilerde
- [ ] NE/N/NW: iki el aralığı açıkça görünür

## Sonraki Adım

Kilit kare QC'den geçtikten sonra Aseprite'ta aç, kilit kare ile boşta/geri dönüş pozu arasında 4-6 kare oluşturmak için Interpolate kullan ve Assets/Sprites/Characters/Warblade/run/ klasörüne dışa aktar.
