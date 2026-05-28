ACCOUNT_SELECTED: ydbilgin
Swapping cred blob -> ydbilgin...
> ⚠️ **HATALI NOT (Hexer silahı):** Bu raporda Hexer silahı "Whip" olarak speküle edilmiş — bu AI hatası. **Canonical = Grimoire / Cursed Totem / Scepter** (`MEMORY/weapon_master_spec_10_class.md`, kullanıcı onayı 2026-05-28). Whip değil.

## 10 Cümle Özet

1. RIMA'nın chibi 120x120 karakter ve 64x64 mob standartları için endüstride piksel doğruluğu ve esneklik dengesi kritik bir öneme sahiptir.
2. Hades ve Children of Morta gibi başarılı roguelite oyunları, görsel derinlik ve üretim hızını optimize etmek amacıyla özelleştirilmiş sprite çözünürlükleri kullanmaktadır.
3. Chibi tarzındaki 120x120 piksel karakterler için tek elli silahlar 64x64 veya 64x128 canvas, çift elli büyük silahlar ise 128x256 veya 64x256 dikey canvas boyutlarında konumlandırılmalıdır.
4. PPU (Pixels Per Unit) normlarında Unity default değeri olan PPU=100 yerine PPU=64 seçilmesi, 32x32/64x64 tilemap'ler ve fizik motoru ile tam piksel uyumu sağlar.
5. HandAnchor mekaniğinde, silah sprite'ının tutma noktasının merkez pikselini pivot tanımlamak, montaj ve rotasyon işlemlerini matematiksel olarak en basite indirgeyen yöntemdir.
6. 8 yönlü hareket içeren top-down oyunlarda 5 yön çizilip diğer 3 yönün yatayda aynalanması üretim süresini neredeyse yarı yarıya azaltır.
7. Karakter aynalandığında silahın yönünün ters dönmemesi veya baş aşağı olmaması için HandAnchor offset ve rotasyon değerlerinin kod tarafından yön bazlı güncellenmesi zorunludur.
8. Animasyon yapısında Children of Morta'nın aşırı maliyetli gömülü yöntemi yerine, Hades'in "silahsız body + dinamik takılı silah + bağımsız VFX katmanı" modeli tercih edilmelidir.
9. Bu hibrit model, tek bir animasyon setiyle onlarca farklı silah varyasyonunu sıfır ek çizim maliyetiyle destekleme avantajı sunmaktadır.
10. Arayüz tarafında ise 64x64 piksel olarak tasarlanan skill icon'larının UI'da 128x128 import edilip downscale edilmesi, modern HD ekranlarda keskinliği korumanın en kararlı yoludur.

---

## Spesifik Weapon Canvas Tablosu

Aşağıdaki tablo, RIMA projesindeki 10 karakter sınıfı için belirlenen weapon canvas ve teknik özellik önerilerini içermektedir. [STAGING/PIXELLAB_INVENTORY_MASTER.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/PIXELLAB_INVENTORY_MASTER.md) dosyasındaki envanter yapılarına ve codebase'de yer alan [OrientationSync.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/OrientationSync.cs), [HandAnchorAttach.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Systems/Combat/HandAnchorAttach.cs) bileşenlerine tam uyumludur.

| Sınıf (Class) | Silah Türü (Weapon Type) | Önerilen Canvas Boyutu | PPU Normu | Pivot Noktası (Custom) | Endüstriyel Eşdeğer / Referans |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Warblade** | Greatsword (2-Hand Large) | 128 × 256 px | 64 / 100 | Kabza Ortası (Alt %25) | Hades - Stygius (Arthur Aspect) |
| **Ronin** | Katana (1-Hand Long) | 64 × 192 px | 64 / 100 | Kabza Ortası (Alt %20) | Sekiro / Katana 8-dir (`692f43ce`) |
| **Gunslinger** | Pistol (1-Hand Small) | 96 × 64 px | 64 / 100 | Tetik/Kabza Ortası (x:%30, y:%50) | Enter the Gungeon - Flintlock |
| **Ranger** | Bow (2-Hand Large) | 128 × 192 px | 64 / 100 | Kabza Merkezi (x:%50, y:%50) | Children of Morta - Linda's Bow |
| **Elementalist** | Staff (2-Hand Long) | 64 × 256 px | 64 / 100 | Şaft Merkezi (Alt %45) | Children of Morta - Lucy's Staff |
| **Shadowblade** | Daggers (Dual 1-Hand) | 48 × 96 px | 64 / 100 | Kabza Ortası (Alt %20) | Children of Morta - Kevin's Daggers |
| **Ravager** | Greataxe (2-Hand Chunky) | 128 × 192 px | 64 / 100 | Sap Ortası (Alt %30) | Children of Morta - Joey's Greataxe |
| **Hexer** | Whip (1-Hand Long) | 96 × 192 px | 64 / 100 | Kabza Altı (Alt %15) | Castlevania - Vampire Killer Whip |
| **Brawler** | Gauntlets (Dual Fist) | 64 × 64 px | 64 / 100 | Bilek Giriş Çizgisi (x:%50, y:%50) | Children of Morta - Mark's Fists |
| **Summoner** | Tome + Orb (1-Hand Each) | 64 × 64 px (Her biri) | 64 / 100 | Kitap: Merkez / Orb: Float Merkez | Diablo III - Wizard Offhands |

### Pivot ve PPU Normları Açıklaması:
- **PPU = 64 (Tercih Edilen):** 32x32 veya 64x64 piksel zemin karoları (tile) kullanan 2D ARPG'ler için standarttır. `1 Unit = 64 Pixels` eşitliği sayesinde fizik hesaplamaları, grid snapping ve pixel-perfect kameralar tam tamsayı (integer) ölçekle çalışır, "mixel" (farklı piksel yoğunlukları) oluşmasını engeller.
- **PPU = 100 (Varsayılan):** Unity'nin varsayılanıdır. Eğer karakter sprite'ları PPU=100 olarak import edildiyse, silahların da PPU=100 yapılması zorunludur.
- **1-Hand vs 2-Hand Farkı:** Tek elli silahlar genellikle kabza tabanlı tek bir pivot noktası kullanırken; yay, asa ve çift elli büyük kılıçlar iki elin ortalama pozisyonuna ([SpriteHandData.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Data/SpriteHandData.cs)) veya gövde merkezine göre dengelenmiş geniş pivotlara ihtiyaç duyarlar.

---

## HandAnchor Yönlendirme (Mirroring) Mekaniği

8 yönlü (8-dir) sprite yansıtma mimarisinde endüstri standardı **5+3 (yansıtılmış)** yapısıdır:
- Çizilen yönler: **S, SE, E, NE, N**
- Kodla yansıtılan yönler (flipX): **SW, W, NW** (East tabanlı yönlerin yansıtılması)

[OrientationSync.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/OrientationSync.cs)'te bulunan baseline offset değerleri, yansıtma (mirror) anında aşağıdaki matematiksel formülle güncellenmelidir:

$$\text{TargetOffset.x} = \text{BaseOffset.x} \times (\text{flipX} ? -1 : 1)$$

Silah rotasyonu ($Z$ ekseni Euler açısı) ise yansıtılmış yönlerde ters açısal ivme almalıdır. Örneğin:
- **East (E):** $0^\circ \rightarrow$ **West (W):** $180^\circ$ (veya flipX ile $0^\circ$)
- **South-East (SE):** $-45^\circ \rightarrow$ **South-West (SW):** $-135^\circ$
- **North-East (NE):** $45^\circ \rightarrow$ **North-West (NW):** $135^\circ$

[WeaponSorter.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/WeaponSorter.cs) katman sıralaması kuralları:
- **N, NE, NW:** Silah SpriteRenderer sortingOrder değeri Karakterin sortingOrder değerinden **-1** (arkada) olmalıdır.
- **S, SE, SW, E, W:** Silah SpriteRenderer sortingOrder değeri Karakterin sortingOrder değerinden **+1** (önde) olmalıdır.

---

## Animasyon Flow ve VFX Önerisi

| Stil / Pattern | Karakter Detayı | Silah Entegrasyonu | VFX Yapısı | RIMA Üretim Maliyeti |
| :--- | :--- | :--- | :--- | :--- |
| **Hades Pattern** | Basit / Pre-rendered | Ayrı Katman (Attached) | Dinamik Parçacık + Shader | Düşük (Hızlı Prototip) |
| **Children of Morta (CoM)** | Çok Detaylı Piksel | Sprite İçine Gömülü (Baked) | Ekran Sarsıntısı + Hit Flash | Çok Yüksek (Her silaha yeni sheet) |
| **Hyper Light Drifter (HLD)**| Düz Geometrik | Soyut / Minimal | Paralel Slash Sprite Overlay | Orta (Efekt Ağırlıklı) |

### RIMA İçin Öneri: Hades + HLD Hibrit Modeli
RIMA projesinin hızlı ve esnek ilerlemesi için en ideal yapı **Hades tarzı silahsız gövde (weaponless body) + dinamik takılı silah** altyapısı ile **HLD tarzı ekranı domine eden büyük slash/darbe VFX sprite katmanlarının** birleşimidir.

#### Neden Bu Model?
1. **Sıfır Ek Çizim Maliyeti:** Karakter animasyonları (Idle, Walk, Attack vb.) silahsız olarak bir kez üretildiğinde, tüm yeni silahlar (Greatsword, Katana, Pistol) kod tabanındaki [HandAnchorAttach.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Systems/Combat/HandAnchorAttach.cs) aracılığıyla sprite'ın eline dinamik olarak bindirilir.
2. **Vuruş Hissiyatı (Juice):** Silahın karakter sprite'ı ile bükülmemesinin yarattığı statik his, saldırı anlarında tetiklenen büyük, yarı saydam ve parlayan piksel slash efektleriyle (HLD tarzı) ve hafif vuruş duraksamalarıyla (hitstop/freeze frame) tamamen kapatılır.

#### Teknik VFX Kütüphane Tavsiyeleri:
- **Unity URP 2D Shader Graph:** Elysium V1 cyan neon parlamalarını silahlara ve trail (iz) efektlerine dinamik olarak vermek için kullanılmalıdır. Materyal üzerinden yanıp sönme (hit flash) ve çözünme (dissolve) efektleri Shader ile çözülmelidir.
- **Unity Particle System (2D):** Karakterin hareket izleri (dash dust), silah savurma esnasında çıkan kıvılcımlar ve portal efektleri için idealdir.
- **Sprite Sheet Flipbook VFX:** Klasik piksel patlamaları ve savurma yayları (slash sweeps) için bağımsız animasyonlu sprite'lar saldırı apex frame'inde (`attack_apex_state`) spawn edilmelidir.

---

## Skill Icon UI Standartları

[STAGING/PIXELLAB_INVENTORY_MASTER.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/PIXELLAB_INVENTORY_MASTER.md) dosyasındaki 23 adet cloud-only `skill_icons` veri setine yönelik UI standartları:

- **Standart Boyut:** Piksel sanatı tutarlılığı için **64×64 piksel** çizim standardı en idealidir. Ancak, modern HD/4K ekranlarda ikonların bulanıklaşmasını önlemek için Unity'de **128×128 piksel** çözünürlüğünde import edilip (Filter Mode: **Point / No Filter** ve Compression: **None** seçilerek) UI Canvas üzerinde downscale edilmesi (örneğin 48x48 veya 64x64 UI alanına sığdırılması) gerekir.
- **Sanat Tarzı ve Tutarlılık Tavsiyeleri:**
  - **Ortak Çerçeve (Border):** Tüm yetenek ikonlarında koyu metalik veya Elysium V1 temalı parlayan cyan çizgilerden oluşan sabit 2 piksellik bir dış çerçeve kullanılmalıdır.
  - **Renk Kodlaması (Color Coding):** Sınıflara veya hasar türlerine göre arka plan degrade (gradient) tonları belirlenmelidir (örneğin Hexer yetenekleri için koyu mor/yeşil, Warblade için altın/bronz, Elementalist için cyan/mavi).
  - **Kontrast:** Ön plandaki yetenek sembolü (kılıç, portal, patlama) parlak ve yüksek kontrastlı olmalı; arka plan ise sembolün okunabilirliğini engellemeyecek şekilde koyu ve az detaylı tutulmalıdır.

