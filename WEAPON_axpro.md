# WARBLADE WEAPON ANIMATION DECISION (AX_PRO)

## W1: (A) SİLAHLI-BAKE
**NEDEN:** RIMA'nın animasyonları frame-by-frame sprite-sheet olduğu ve skeletal (bone) altyapısı bulunmadığı için, her frame'de ele silah hizalamak (per-frame pivot) 5 gün kalan bir demo için çok riskli ve zahmetlidir. Akıcı bir vuruş hissi (LMB) için silahın gömülü olması (baked) en güvenli ve hızlı yoldur.

## W2: KORU
Mevcut silah-bake'li state'ler korunmalıdır. Animasyon üretiminde bu state'ler referans alınarak kılıçlı animasyonlar çıkartılmalıdır. State prompt'u da silahı destekleyecek şekilde ("weapon consistent, visibly holding sword") kullanılmalıdır.

## W3: DEMO-SONRASI ETKİSİ
Demo sonrasında loot-variety için ya silah sınıflarına (kılıç, mızrak vb.) özel ayrı sprite-sheet setleri üretilir ya da skeletal sisteme geçiş yapılır. Demo aşamasında her kılıç için ayrı görsel yerine tek bir kılıç tipiyle ilerlenmeli, over-engineering'den kaçınılmalıdır.

## W4: TEK CÜMLE TAVSİYE
Demo kısıtlamaları ve teknik altyapı göz önüne alındığında; silahı animasyonlara göm (bake), mevcut kılıçlı state'leri kullanarak üretimi hızlandır ve demo hedefine odaklan.
