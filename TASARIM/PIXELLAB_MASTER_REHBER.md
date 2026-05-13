# PixelLab AI Üretim Rehberi (Video Analizi)

## ⚠️ RIMA-S60+S66 OVERRIDE (Bu bölüm canonical, video notları alt referans)

Bu rehber genel PixelLab eğitimini kapsar. **RIMA proje kararları aşağıdaki override'lara tabidir** — çelişki durumunda Master Karar Belgesi canonical:

### Pipeline (S60 LOCKED + S66 Update)
- **Karakter üretim:** Create Image Pixen NEW (S-XL) → reference image → Create Character → 8 yön (Karar #114)
- **Animasyon:** Custom Animation V3 (Karar #108): 4-16 frame, 3 gen/dir; Create State 20-40 gen
- **Tile üretim:** Maps > Tileset Pro (Gemini-backed, S66 onaylı `create_topdown_tileset` Wang autotile). 16 tile + transition. Eski Maps Tab/Inpaint Pro tileset workflow Karar #75 REVISION ile kısmen onaylı — sadece izole varyant için `create_tiles_pro`.
- **MCP:** Üretim için YASAK (Karar #106). Web UI kullan.

### Görsel Standartlar
- **Karakter:** 64x64 chibi (Karar #100 + #73)
- **Tile:** 32x32 (Karar #100)
- **View angle:** ~35° high top-down (Karar #113, Hades reference)
- **Yön sayısı:** 8 yön — 5 gen + 3 mirror (Karar #114, eski 4 yön kuralları #53 + #88 REVOKED)
- **PPU:** 64 sabit
- **Filter:** Point, no compression, no mipmap

### Tema (Karar #77 Vivid Vulnerability)
- Salt and Sanctuary chibi-but-serious + Hades theatrical mythic
- Cyan/violet rift accent, blood/horror YASAK
- F1 palette: #2C2A2A (floor), #4A3F3F (wall), #7BA7BC (cold blue), torch #C4682A
- Ritual Catastrophe framing (void cracks, broken sigils), grimdark DEĞİL

### Naming (Karar #112 Glossary)
- **Shard** (Capital-S) = Fracturing reality fragment
- **shard** (lowercase) = currency/material
- **Trace** = run-içi cryptic identity sinyali
- **Awakening** = class intro micro-segment
- **Echoes** = currency (Karar #27)
- **Echo Twin** = Act 2 boss

### REVOKED Pipeline Sections (Bu rehberin alt bölümlerinde olabilir, görmezden gel)
- 2.5D / KayKit / 3D pre-render (Karar #72 REVOKED)
- Mature ARPG 60° tile (Karar #100 REVOKED)
- 4 yön rotation kuralı (Karar #114 REVOKED, şimdi 8 yön)
- Karakter+çevre hibrit perspektif (Karar #113 REJECTED, ~35° tek konverjans)
- Map Workshop multi-tile connected room output (Karar #75 LOCKED, S66 revision: `create_topdown_tileset` Wang Pro mode istisna)

Bu belge, PixelLab'ın resmi eğitim videolarından çıkarılmış kritik ipuçlarını, araçların (tools) nasıl kullanılacağını ve üretim iş akışlarını (workflow) frame-frame inceleyerek derlenmiş **kesin (locked)** bir başvuru kaynağıdır. AI araçlarının oyun motoruna tam uyumlu (engine-ready) ve estetik olarak kaliteli varlıklar (assets) üretmesi için bu kurallara uyulması zorunludur.

## 1. Karakter Animasyonları (Özellikle Silahlı Karakterler)

Silah kullanan (kılıç sallayan, ok atan) karakterler için standart animasyon şablonları genellikle bozuk sonuçlar verir çünkü bu şablonlar genelde boş elli karakterlerle eğitilmiştir.
- **Kullanılacak Araç:** `Custom animation V3`
- **Adım Adım Süreç:**
  1. Başlangıç karesini seçin.
  2. Prompt girin (Örn: `quick sword attack, Zelda style` veya `Knight holding a sword walking loop`).
  3. **"Enhance with AI"** butonuna basın (prompt'un AI tarafından detaylandırılmasını sağlar).
  4. Frame sayısını **6** olarak ayarlayın. Sistem başlangıç karesini de tuttuğu için toplamda **7 frame** elde edersiniz.

> [!TIP]
> **Yürüme (Walk) Animasyonları İçin Altın Kural:** Yürüme döngüsü oluştururken başlangıç karesi (start frame) olarak karakterin düz ve hareketsiz durduğu (standing) kareyi **KULLANMAYIN**. Bunun yerine, karakterin bacaklarının zaten açık olduğu, adım atmış olduğu bir orta kareyi (mid-stride) başlangıç karesi olarak belirleyin. Bu yöntem animasyonun çok daha doğal ve akıcı olmasını sağlar.

> [!IMPORTANT]
> **Manuel Müdahale Şarttır:** İlk AI üretimi nadiren mükemmel olur. En iyi jenerasyonu seçip `Edit in Pixelorama` diyerek manuel temizlik yapın.
> - Kötü hissettiren (clunky) frameleri silin.
> - Kılıç vuruşlarında kılıcın pozisyonunu manuel kopyala-yapıştır ile hareket ettirerek **vuruş hissini (impact)** artırın.
> - Kuzeye yürüme gibi arkadan görünümlerde kafa/kask bozuluyorsa, düzgün bir kask karesini kopyalayıp bozuk framelere yapıştırın.

## 2. Karakter Skalası (Ölçek) ve Style Reference Pro

Yeni bir düşman veya NPC üretirken oyunun ana stiline sadık kalmak için "Style Reference Pro" aracı kullanılır.
- **Kullanılacak Araç:** `Create from style reference pro`
- **Adım Adım Süreç:** Ana karakterinizin güneye bakan (south-facing) temel sprite'ını referans olarak araca sürükleyin.
- Prompt girin: `RPG knights and princesses`, `RPG characters, archer, farmer` vb.

> [!WARNING]
> **Canvas Boyutu Hatasına Dikkat:** Ana karakterinizin sprite'ını araca sürüklerken canvas boyutu çok büyükse (örneğin 56x56), AI yeni üreteceği karakterleri bu devasa canvasa sığdırmaya çalışacak ve ana karakterinizden orantısız büyüklükte (giant) NPC'ler/düşmanlar yaratacaktır.
> **Çözüm:** Referans olarak vereceğiniz karakterin canvas boyutunu mutlaka **32x32**'ye küçültüp (veya ana gövde ne kadar yer kaplıyorsa o boyuta croplayıp) sisteme atın. Boyut küçük olduğunda AI batch (grup) başına daha fazla varyasyon sunar ve oyun içi ölçek (scale) mükemmel eşleşir.

## 3. Yön (Rotation) Aracı ve Hızlı Karakter Aktarımı

Tek bir yöne (örn: güneye) bakan karakter üretildikten sonra 8 yöne çevirmek için:
- **Kullanılacak Araç:** `Generate Eight Rotations`
- Çıkan sonuç Pixelorama'da hızlıca temizlenir.
- **YENİ ÖZELLİK (Zaman Tasarrufu):** Rotation işlemi bittikten sonra her kareyi tek tek "Character Creator"a atmak yerine, doğrudan rotation sayfasındaki **"Create Character"** butonuna basın. Bu buton tüm yönleri otomatik içe aktarır, iskeleti (skeleton) tahmin eder ve anında animasyona geçmenizi sağlar.

## 4. Harita, Zemin ve Tileset Üretimi (Adım Adım Nasıl Yapılır?)

Oyun türünüze (Top-Down veya Side-Scroller) göre harita ve zemin üretim stratejileri değişir. İşte PixelLab'daki en etkili harita/tileset iş akışları:

### A. Top-Down İç Mekan (Interior) Haritası Üretimi
1. **Temel Zemin Planı:** `Maps Tab` sekmesinde yeni bir harita açın. Standart bir zemin tileset'i (örneğin *wooden floor*) kullanarak odanın taslağını çizin.
2. **Karakter ile Ölçeklendirme (Kritik!):** Sol menüden `Place Character` diyerek oyundaki ana karakterinizi haritaya bırakın. Mobilya ve duvar üretirken karakterin referans boyutu (scale) olarak sahnede durması, eşyaların devasa veya karınca gibi küçük olmasını engeller.
3. **Duvarlar ve Kapılar (Inpaint Pro):** Odanın duvarlarını örmek için `Inpaint Pro`'yu kullanın. Duvarın olmasını istediğiniz alanı boyayın ve prompt girin: `brick wall with nice windows`. Duvarın devamına kapı eklerken, bir önceki duvarın küçük bir parçasını da seçime dahil edip `brick wall with a door` yazın. Böylece AI stili koruyarak kesintisiz bir yapı oluşturur.
4. **Mobilya ve Objeler (Create Object):** Sol menüden `Create Object` seçeneğiyle eşya eklenecek alanı seçin. 
   - *Prompt Örnekleri:* `top-down small green sofa`, `small fridge, front-facing, top-down`.
   - **İpucu:** Çıkan objeler genelde büyük olmaya meyillidir. İstediğiniz boyutu yakalamak için prompta **"small"** kelimesini eklemek çok etkilidir.

### B. Side-Scroller (Yatay İlerlemeli) Tileset ve Harita Üretimi
1. **İlk Üretim:** Canvas boyutunu **128x128** (veya 32x32 tiles için daha büyük) ayarlayın. `Create Tile Set Sidescroller` aracını açın.
2. **Tile Tanımları:** Üst (Top) tile için `grass`, orta (Center) tile için `dirt` veya `rocks` yazın ve üretin.
3. **Dışa Aktarma (Export):** Çıkan sonucu `Export as Tile Set` butonuna tıklayarak oyun motorunuz için indirin (Unity'de Tilemap / Rule Tile için **15-tile** veya **3x3 tile set** formatları idealdir).
4. **Geliştirme / Cilalama (Edit Image Pro):** İlk üretilen tileset'ler genellikle düz olur. Tileset'i `Edit Image Pro` aracıyla açıp şu sihirli promptu girin: 
   - *Prompt:* `make the tile set prettier with real grass accents and stone walls` veya `make it a more realistic grass and dirt tile set`. AI mevcut şablonu koruyarak harika dokular ekleyecektir.
5. **Özel Çevre Detayları (Inpaint):** Haritayı dizerken arada boşluk bırakın. O boşluğu `Inpaint` ile seçip *Prompt:* `a sidescroller game waterfall between the grass tiles` yazın. Mükemmel entegre edilmiş şelaleler elde edersiniz.

### C. Parallax Arka Planlar (Backgrounds)
1. **Temel Zemin:** Geniş bir canvas açın (örneğin 512x288 veya 320x176). `Inpaint V3` kullanarak *Prompt:* `a seamless horizontal snowy landscape background for a side-scroller game` yazın. Oluşan resim yatayda kusursuz olarak tekrar edebilir (tileable) olacaktır.
2. **Parallax Katmanlarını Ayırma:** Arka plandan sadece ağaçları almak istiyorsanız, görüntüyü `Edit Image Pro`'ya atın.
   - *Prompt:* `leave just the trees for parallax, remove everything else, keep the same composition`.
   - Seçeneklerden `Remove Background` ve `New Layer`'ı seçip üretin. 
   - AI, arkadaki dağı ve gökyüzünü silip size sadece ağaçların olduğu şeffaf bir katman verecektir. Aynı işlemi "sadece dağlar" veya "sadece bulutlar" diyerek tekrarlayın ve Unity'deki Parallax efektiniz için hazır katmanlar oluşturun.

## 5. Çevre Objeleri (Decorations)

- **Ağaçlar ve Heykeller:** Haritada `Create Object Tool` kullanın. İlgili alanı seçin ve prompt girin (Örn: `top-down tree`). Daha yüksek kaliteli, detaylı sonuçlar (örneğin heykeller veya heybetli ağaçlar) için **`Pro Inpainting`** modunu açın.
- **Binalar / Evler:** `Creator Tab` içindeki **`Create image small to XL`** aracını kullanın. Canvas boyutunu **128x128** yapın. Prompt: `top-down wooden house with a sword symbol`. Üretilen evi doğrudan haritaya sürükleyebilirsiniz.

## 6. Arayüz (UI) ve Mermiler (Projectiles)

- **UI Objeleri (Can, İkon vb.):** `Small 2XL Tool` kullanın. Canvas: **32x32**. Prompt: `small heart icon`. Hızlıca temiz assetler elde edilir.
- **Mermiler (Ok, Büyü Küresi vb.):** `Create Image Pro` kullanın. Canvas: **32x32**.
  - **Prompt:** `a single arrow in eight directions` (Pro aracı sprite sheet halinde çoklu üretim yapar).
  - Bu çıktıyı Aseprite/Pixelorama içine aktarın. En temiz görünen tek bir oku/mermiyi seçin, ardından onu kopyalayıp manuel olarak döndürerek (rotate & flip) 8 yönlü temiz bir sprite sheet oluşturun. (Bunu AI'a bırakmak yerine manuel yapmak 100% mükemmel sonucu garanti eder).

---
**ÖZET:** PixelLab tek tıklamayla bitmiş ürün veren sihirli bir düğme değil, iş akışınızı hızlandıran **kolaboratif** bir araçtır. **"Generate (Üret) -> Clean (Temizle/Düzelt) -> Animate (Hareketlendir) -> Refine (Cilala)"** döngüsünü kavrayan kişi bu araçla her şeyi yapabilir. İlk üretimleri her zaman Pixelorama'da insan eliyle rötuşlayın.
