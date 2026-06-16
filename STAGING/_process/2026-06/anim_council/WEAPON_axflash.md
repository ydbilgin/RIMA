# WEAPON MOUNT DECISION — ax_flash Analysis

## W1: Seçim & Neden
* **Seçim:** **(A) SİLAHLI-BAKE**
* **Neden:** RIMA skeletal/bone sistemi barındırmayan geleneksel frame-by-frame sprite-sheet animasyonu kullanmaktadır. 5 günlük demo süresi ve kısıtlı kota göz önüne alındığında, her frame için el pozisyonu ve pivot hizalaması (SpriteHandData SO veya magenta piksel takibi) yapmak aşırı zaman alıcı ve risklidir. A seçeneği görsel akıcılığı anında sağlar ve takvimi korur.

## W2: Mevcut State Durumu & Prompt Revizyonu
* **Eylem:** Mevcut kılıçlı state'leri **KORU**.
* **Prompt Güncellemesi (A için):**
  `Outfit & Gear: identical to anchor reference (preserve weapon identity - dark steel greatsword with brass crossguard, held firmly in hand). Negative: no weaponless hands, no floating or detached weapon parts.`

## W3: Demo Sonrası Loot-Variety Etkisi
* **Analiz:** Demo için A seçilmesi, projenin sonsuza dek bu yolda kalmasını gerektirmez. Demo sonrasında (post-demo) asıl karakter gövdeleri silahsızlaştırılarak ve Aseprite/Unity Animator üzerinden el pivot verisi çıkarılarak **B (Silahsız + Mount)** altyapısına temiz bir geçiş yapılabilir. Demo için her silah varyasyonu üretilmeyecek olup, demo-sonrası için altyapı ertelenmelidir.

## W4: Tek Cümle Tavsiye
* 5 günlük teslimat baskısı altında görsel akıcılığı ve çalışma zamanı kararlılığını riske atmamak adına demoda **(A) Silahlı-Bake** ile ilerleyin, esnek mount altyapısını demo sonrasına erteleyin.
