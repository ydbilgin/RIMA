# EVRENSEL AI SANAT ÜRETİM SİSTEMİ (Gemini + PixelLab + ComfyUI)

Bu klasör, herhangi bir yeni (veya mevcut) projede oyun sanat dili öğretisini ve üretim hattını (pipeline) sıfırdan kurmak için **Tek Kaynak (Single Source of Truth)** işlevi görür.

> ⚠️ **YAPAY ZEKA ASİSTANLARI İÇİN TAVİZ VERİLMEZ KURAL:**
> Çalıştığınız herhangi bir projede kullanıcı size *"F:\Antigravity Projeler\ART_REDIRECT readmeyi oku, ona göre art yaptır bana"* dediyse:
> 1. Hemen bu belgeyi okuyun.
> 2. Kullanıcının çalışmakta olduğu yeni projenin içine, bu standartlara %100 uyan, kullanıcının tıklayacağı butonlara kadar tarif eden detaylı bir `ART/SPRITE_WORKFLOW.md` (Adım Adım Üretim Rehberi) oluşturun.
> 3. Herhangi bir "Placeholder" (Geçici) aset ürettirmeyin. Her zaman buradaki en yüksek kalite sıfır-hata üretim zincirine (*Gemini Konsepti -> PixelLab Base Çizim -> ComfyUI Animasyon*) bağlı kalın.

---

## ⚙️ 1. SİSTEM MANTIĞI VE ARAÇLARIN ROLÜ

Büyük projelerde kararlı (stable) ve aynı sanat diline sahip retro pikseller elde etmek için bu 3 zincir KESİN KURALDIR:

1. **Gemini:** Konsepti, ince detayı ve mükemmel oyun açısını (örneğin *Top-Down*) oluşturarak yüksek çözünürlüklü devasa bir kaba taslak / referans verir.
2. **PixelLab (Aseprite):** Gemini'nin devasa resmini alır, oyunun gerçek çözünürlük limitlerine (örn. 32x32, 64x64) ve kısıtlı renk paletine sıkıştırır. Oyunda kullanılacak asıl temiz pikselli "BASE" (Ana Sabit Tek Kare) sprite'ı üretir.
3. **ComfyUI (Python Script):** PixelLab'ın ürettiği pikselli BASE sprite'ı girdi (input) olarak alır. Karakterin Idle, Walk, Attack hareket setlerini (Sprite Sheet) otomatik oluşturur. **ComfyUI, asla doğrudan Gemini görselini girdi olarak kullanmaz, aksi halde pikseller uyuşmaz.**

---

## 🛠 2. KULLANICI İÇİN PRATİK İŞ AKIŞI (Uygulama Adımları)

Yeni projelere yapay zeka tarafından eklenecek olan üretim rehberleri, kullanıcı için "şuna tıkla, buraya bunu yaz" şeklinde şu standart adımları barındırmalıdır:

### ADIM 1 — Gemini Referansı (Konsept)
- Kullanıcıya her düşman/karakter için yapıştıracağı tam prompt metni verilir.
- **Top-Down Kamera Kuralı:** Eğer oyun yukarıdan bakışlıysa prompt içerisinde kesinlikle şu kalıp bulunmalıdır:
  > `Viewed strictly from directly above, bird's eye aerial top-down perspective — as if a camera is mounted on the ceiling looking straight down. The art style is retro pixel art. Transparent background.`
- Gemini'den alınan başarılı resim, projeye klasörüne `[asset_adi]_gemini_base.png` adıyla kaydedilir.

### ADIM 2 — PixelLab ile BASE Sprite Üretimi (Tıklama Rehberi)
1. Aseprite içinde `File → New` ile doğru çözünürlükte boş tuval açılır (Arka plan Transparent).
2. İndirilen `[asset_adi]_gemini_base.png` dosyası Aseprite tuvaline sürüklenip bırakılır. Katman adı `REFERANS` yapılır.
3. `Edit → PixelLab → Open plugin` açılır.
4. **ÇÖZÜNÜRLÜK BUTONU SEÇİMİ (Kritik):**
   - ⚠️ **KÜÇÜK (≤ 80px) Tuval:** Kesinlikle **`Create S-M image`** (Medium) kullanılmalıdır! (Aksi halde resim çamura / gürültüye döner).
   - ⚠️ **BÜYÜK (≥ 96px) Tuval:** Kesinlikle **`Create M-XL image`** (SDXL Detaylı) kullanılmalıdır!
5. **Parametre Ayarları:**
   - **Description:** Gemini promptunun 1-2 cümlelik basit özeti.
   - **Negative Prompt:** `side view, front facing, 3/4 view, isometric, 3d, realistic, background, scenery`
   - **Camera View:** Genellikle `Low top-down` (Zelda tarzı) veya tam `Top-down`. Yön `South`.
   - **Init Image → Set:** Butonuna basıp tuvaldeki o ilk `REFERANS` katmanındaki Gemini resmi seçilir.
   - **Init Image Strength:** Aslına sadakat oranıdır. `500` iyi bir varsayılandır. Daha çok orijinaline yapışması isteniyorsa `650-700` ayarlanır.
   - **Background Removal:** `ON` (Açık).
   - **Output Method:** Önceki katmanları ezmesini engellemek için `Add new layer` seçilir.
6. En alttan **Generate** tuşuna basılır. Sonuç berbat ise silgiyle boğuşmak yerine `CTRL+Z` ile katman geri alınıp; PixelLab içinden ufak ayar yapılarak tekrar Generate edilir.
7. Mükemmel sonuç elde edilince `REFERANS` adlı katman silinir. Kalan pikselli asıl tuval `[asset_adi]_S_BASE.png` ("S", Still/Static) olarak kaydedilir.

### ADIM 3 — ComfyUI ile Animasyon Üretimi
Kullanıcılar PC'yi kilitlememek için animasyonları Aseprite PixelLab içinden arayüzle üretmezler.
- Bunun yerine doğrudan projenin yerel python komutu çalıştırılır:
  > `python tools/generate_anim.py --only [asset_adi] --anim-only` *(Örnek komut)*
- Bu otomasyon scripti; 2. Adımda çizilen o tek `_S_BASE.png` karesine bakarak projenin gereksinimi olan (Idle, Walk, Attack vb.) tüm animasyon sprite-sheet'lerini çıkartır.

---

## 📚 3. PIXELLAB (ASEPRITE EXTENSION) DERİN BİLGİ VE SORUN GİDERME

Proje Asistanı (AI) için önemli arka plan kuralları:

* **ÇÖZÜNÜRLÜK HATASI (Noise/Gürültü):** 32x32 boyutunda minik bir tuval açıp yanlışlıkla "M-XL" butonuna basarsanız 1024x1024'lük devasa bir detay matematiğini zorla 32 piksele sıkıştırmaya (downscale) çalışır. Sonuç asla keskin olmaz. Bulanık, çamurlu ve noiselu bir doku çıkar. Pixel Art ruhu yok olur. Bunu engellemek için ≤ 80px'de kesinlikle S-M modeli kullanılmalıdır.
* **ANİMASYON TİTREMESİ (Flickering) KURALI:** Eğer animasyon komutu yerine ufak bir VFX/Sprite **PixelLab içindeki** "Animate with Text (New)" özelliğiyle Aseprite'ta manuel yapılacaksa: Arayüzdeki "Init Image" ayarında referans olarak Gemini/İlk Kare resmi **seçilmez**! Kesinlikle **"Current Layer (Anim)"** seçeneği işaretlenmelidir ki resimler (frameler) birbirini takip etsin ve titreme (flickering) olmasın.
