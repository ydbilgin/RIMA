Videodaki temel izometrik pixel-art prensiplerini analiz edip, RIMA'nın **PixelLab AI + Unity 6 URP 2D** tabanlı üretim hattına (pipeline) entegre edebileceğimiz somut aksiyonlara dönüştürdüm.

İşte genel laflardan arındırılmış, doğrudan RIMA'nın üretim akışına yönelik analiz ve eylem planı:

### 1. Videodaki Temel İzometrik Prensipler ve Teknikler
Videoda manuel çizim için anlatılan ancak bizim AI pipeline'ımızda matematiğini anlamamız gereken temel kurallar şunlar:
*   **"Rule of Two" (2:1 Pixel Oranı):** Kusursuz izometrik yüzeyler için her 2 yatay piksele 1 dikey piksel düşmesi. Yüzeyler (diamond) çapraz çizgilerle, yükseklikler (cliff) ise tam dikey çizgilerle çizilir.
*   **Küpün 3 Yüzü ve Işıklandırma (Light Source):** Bir blok 3 elmastan oluşur (üst, sol, sağ). Üst yüzey en parlak, sol orta karanlık, sağ en karanlık olmalıdır. Bu derinlik hissini yaratır.
*   **Kontrast ile Okunabilirlik:** Zemin (floor) tile'ları net ve okunabilir, duvar (wall) tile'ları ise daha koyu çizilmelidir ki grid tabanlı oyunda oynanabilirlik artsın.
*   **Aşağıdan Yukarıya Katmanlama (Layering):** 2D uzayda 3D illüzyonu için objeler arkadan öne ve yukarıdan aşağıya (Z as Y) doğru sıralanarak çizilmelidir.

---

### 2. Bu Prensipleri RIMA Pipeline'ına Nasıl Uygularız?

AI (PixelLab) elle çizim gibi her pikseli 2:1 oranında mükemmel koyamayabilir. Bu yüzden üretim sonrası (post-process) müdahaleler şarttır.

**PixelLab Prompt Entegrasyonu:**
*   Videodaki "3 yüzlü ışıklandırma" kuralını AI'a dayatmalıyız.
*   *Prompt Ekleri:* `"perfect isometric projection, top-down diagonal light source, highly illuminated top surface, shadowed right face, dark slate base, sharp pixel art, clear distinct edges, cyan #00FFCC subtle highlights"`
*   Zemin ve duvarları ayırmak için duvar promptlarına `"darker contrast, deeply shadowed"` eklenmelidir.

**Unity Import ve Tile Ayarları:**
*   **Diamond Oranı Uyumsuzluğu:** RIMA'da elmas oranınız `0.96 x 0.585`. Bu standart 1:0.5 (2:1) değil. AI'ın ürettiği görsellerin bu gride tam oturması için Unity'de `Sprite Import Settings` içinden **Mesh Type: Full Rect** yapılmalıdır. (Tight mesh yaparsanız AI'ın fazlalık pikselleri yırtılmalara yol açar).
*   **PPU Ayarı:** Üretilen görsel 64x64 ise ve hücre boyutunuz 0.96x0.585 ise, PPU (Pixels Per Unit) değerini tam olarak 64'te tutup Tilemap Grid ölçeğini (Cell Size) `(0.96, 0.585, 1)` olarak sabitlemelisiniz.

**Post-Process Script (Üretim Sonrası):**
*   AI'dan çıkan sprite'ların kenarları (özellikle 2:1 rule of two pikselleri) genelde pürüzlü olur. Kusursuz *seamless tiling* için Unity içinde bir **"Diamond Masking" (Elmas Kırpma) Editor Script'i** yazılmalıdır. AI'dan gelen resmi alır, kusursuz 0.96x0.585 oranındaki bir elmas maskesinden geçirir ve kenardaki hatalı/taşıyan pikselleri şeffaf (alpha 0) yapar.

---

### 3. Manuel Teknikleri AI Yaklaşımıyla Taklit/Otomatize Etme

Videodaki manuel teknikleri (Wang tiling, cliff çizimi) AI ile tek tek üretmek yerine prosedürel olarak çözmeliyiz:

*   **Yükseklik ve Derinlik (Cliff Yönleri):** Videodaki gibi sol ve sağ duvarları AI'a çizdirmek tutarsızlığa yol açar.
    *   *AI Çözümü:* PixelLab'dan **sadece** düz zemin (üst elmas) tile'ları üretin.
    *   *Unity Çözümü:* Uçurum (cliff) kısımları için AI zeminini Unity'de bir script veya shader ile aşağı doğru "extrude" edin (uzatın). Uzatılan kısmı koda dayalı olarak karartın (dark iron/slate rengine doğru gradient). Böylece AI'ın dikey piksel hatalarından kurtulursunuz.
*   **Seamless Wang Tiling:** Organik büyük odalar için tile'ların birleşme noktalarındaki izleri (seam) yok etmek gerekir.
    *   *AI Çözümü:* AI'a 16 varyant ürettirirken kenarları belirginleştirmemesini söyleyin (`"borderless, seamless internal texture"`).
    *   *Unity Çözümü:* Tilemap üzerinde "Rule Tile" (veya Advanced Rule Tile) kullanın. 16 varyantı noise (perlin) bazlı rastgele dağıtan özel bir kural yazın. Aralara Cyan (#00FFCC) parlayan ufak damar/çatlak tile varyantları ekleyerek geçişleri kamufle edin.

---

### 4. RIMA İçin Eklenecek SOMUT Aksiyonlar (Fikir & Görevler)

**Aksiyon 1: "AI-to-Iso" Kırpma Aracı (Editor Script)**
*   **Ne:** PixelLab'dan inen kare veya hatalı kenarlı görselleri, tam `0.96 x 0.585` gridine uygun kesen bir Unity Editor aracı.
*   **Nasıl:** Script, texture'ın ortasını merkez alır, matematiksel olarak "Rule of 2" sınırları çizer ve bu sınırların dışında kalan piksellerin Alpha değerini sıfırlar. Bu sayede AI görselleri kusursuz yan yana dizilir.

**Aksiyon 2: Procedural Cliff Shader (URP 2D)**
*   **Ne:** Zemin tile'larının uçurum/kenar kısımlarını manuel AI promptuyla uğraşmadan aşağı doğru çizen shader.
*   **Nasıl:** Shader Graph'ta Z ekseninde (Y aşağısına doğru) texture'ı sündüren (extrude) ve RIMA'nın dark slate/iron palet renklerine (örn: `#1A1E24`) doğru karartan bir efekt yazılacak. Bu, videodaki "dikey çizgiler yüksekliği belirtir" kuralını otomatize eder.

**Aksiyon 3: Standartlaştırılmış PixelLab Prompt Şablonları**
*   **Zemin (Floor) için:** `[Subject] texture, true isometric view, diamond shape only, flat surface, top-down left light, clear slate base, 00FFCC energy cracks, seamless interior, pixel art, high contrast --no vertical walls`
*   **Duvar/Engel (Wall) için:** `[Subject] tall block, true isometric perspective, illuminated top diamond, deep shadowed right face, dark iron material, 2:1 pixel ratio edges, clean pixel art`

**Aksiyon 4: Unity Sorting Axis Ayarının Kesinleştirilmesi**
*   Videodaki "arkadan öne katmanlama" sorununu Unity'de kalıcı çözmek için: Edit > Project Settings > Graphics > **Transparency Sort Axis** değerini RIMA'nın gridine özel olarak **(X: 0, Y: 1, Z: -0.26)** civarı bir değere (0.585 yüksekliğine göre test edilmeli) ayarlamak veya Tilemap Renderer içindeki "Custom Axis" ile Y'ye göre sıralamayı (Z as Y) zorlamak. (Genellikle Y=1, Z=-0.26 veya duruma göre Y=0.5, Z=-1 izometrik oyunlarda clipping'i çözer).

