# Claude Task — RIMA Dungeon Visual Overhaul: Fake Isometric + Immersive Interior

Proje yolu: `F:\Antigravity Projeler\2d roguelite\RIMA\`

---

## Sorun

Mevcut dungeon odaları "yukarıdan arena izleme" hissi veriyor. Tüm oda tek ekranda görünüyor, 4-6 duvar her tarafı kapatıyor, oyuncu sanki bir diorama'nın içinde. Bu istemediğimiz his.

## İstenen His

**Dungeon'ın İÇİNDEYMİŞ gibi hissetmek.** Oyuncu karanlık bir dungeon'da yürüyor, etrafı keşfediyor, köşe dönüyor, duvarlar yanından geçiyor. Hades'teki gibi "arena seyircisi" değil, Diablo/Stoneshard'daki gibi "ben buradayım" hissi.

Aynı zamanda dungeon görsel olarak **fake isometric** estetikle zenginleşecek — duvarlar kalın, derinlikli, perspektifli çizilmiş. Ama altyapı (grid, hareket, collision) DEĞİŞMEYECEK.

---

## Temel Prensipler

### 1. Harita > Kamera
- Harita kamera viewport'undan çok daha büyük olacak
- Kamera tüm odayı göstermiyor, sadece oyuncunun etrafını gösteriyor
- Oyuncu hareket ettikçe yeni alanlar ortaya çıkıyor
- Dış çevre duvarları sadece oyuncu oraya yürüdüğünde görünür

### 2. Kamera Yakın, Oyuncuyu Takip Ediyor
- Kamera zoom seviyesi: haritanın yaklaşık %30-40'ı ekranda
- Oyuncu merkezde, kamera smooth follow
- Bu sayede duvarlar, sütunlar, geçitler kameranın dışına taşıyor → büyüklük hissi

### 3. İç Mimari Var
- Sadece dış çevre duvarı değil, odanın İÇİNDE de yapı elemanları:
  - Yarım duvarlar (sightline kıran ara bölmeler)
  - Sütunlar ve kırık kemerler
  - Niş'ler (duvarda girinti/çıkıntı)
  - Yıkık yapılar, enkaz
- Bunlar gameplay'e de etki ediyor: düşman arkasına saklanabilir, oyuncu cover kullanabilir

### 4. Koridorlar ve Geçişler
- Odalar arası sadece bir kapı teleport'u değil
- Dar koridorlar, geçitler, merdivenler var
- Koridor duvarları kameranın iki yanından ekran dışına uzanıyor
- "Dar geçit" → "geniş salon" geçişi dramatik his yaratıyor

### 5. Duvarlar Büyük ve İmposing
- Duvar sprite'ları yeterince büyük ki oyuncu yaklaştığında ekranın kenarını aşsın
- "Bu duvar benden çok daha büyük" hissi
- Uzaktan sadece üst kenarını görüyorsun, yaklaşınca tüm yüzey ortaya çıkıyor

### 6. Aydınlatma = Keşif
- Haritanın tamamı aydınlık değil
- Oyuncunun etrafı aydınlık, uzak alanlar karanlığa gömülü
- Meşaleler, sconce'lar lokal aydınlatma noktaları
- Duvarın arkasını göremiyorsun → merak ve keşif hissi

---

## Fake Isometric Nasıl?

Altyapı (Unity Grid, hareket, collision) DEĞİŞMİYOR. Sadece **art ve yerleşim** değişiyor:

### Korunan (dokunma)
- Unity Grid tipi: Rectangle
- Karakter controller/hareket: Aynı
- Collision sistemi: Aynı
- Y-sort rendering: Aynı
- Karakter sprite perspektifi: True South 35°
- Transform Squash: localScale.y = 0.819

### Fake Iso = Art Trick
- Duvar sprite'ları **isometric derinlik** gösterecek şekilde çizilmiş (kalınlık, yan yüzey, üst kenar, taş blok detayı)
- Zemin tile'ları hafif perspektif shading'i ile
- Oda şekli illa diamond olmak zorunda değil — oktagonal, düzensiz, organik olabilir
- Önemli olan **duvarların 3D derinlik hissi** vermesi, flat olmaması

### Duvar Tipleri
| Tip | Kullanım | Özellik |
|---|---|---|
| **Dış çevre duvarı** | Haritanın sınırı | En kalın, en yüksek, en detaylı |
| **İç ara duvar** | Oda içi bölme | Daha kısa, bazen yıkık/kırık |
| **Sütun** | Sightline kırıcı | Tek noktada dikey yapı |
| **Kemer/Kapı** | Geçiş noktası | Derinlik gösteren açıklık |
| **Koridor duvarı** | Dar geçitlerde | İki yanda paralel, uzun |

---

## Oda Tipolojisi

Dungeon tek tip arena değil. Farklı alanlar var:

| Alan Tipi | Boyut | Kamera İlişkisi | İç Yapı |
|---|---|---|---|
| **Geniş salon** | 40×30+ unit | Kamera odanın %30'unu gösterir | Sütunlar, niş'ler, merkez yapı |
| **Orta oda** | 25×20 unit | Kamera odanın %50'sini gösterir | Ara duvarlar, köşe yapıları |
| **Dar koridor** | 8×30+ unit | Kamera genişliği dolduruyor ama uzunluk büyük | İki yan duvar ekran dışına uzanır |
| **Kavşak** | 20×20 unit | Çoklu çıkış görünür | Merkez sütun veya yapı |
| **Boss odası** | 50×40+ unit | Çok büyük, kamera küçük kalır | Platformlar, engeller, faz değişim alanları |

---

## Referans Oyunlar

- **Stoneshard** — dungeon içi his, kamera yakın, harita büyük, ara duvarlar
- **Diablo 1-2** — karanlık dungeon, keşif, lokal aydınlatma
- **Moonlighter** (dungeon kısımları) — oda bazlı ama immersive
- **Darkest Dungeon** (koridor hissi) — dar geçitler, baskı hissi

**REFERANS DEĞİL:**
- Hades — arena seyircisi hissi, tüm oda görünür
- Enter the Gungeon — oda = kamera, flat

---

## Locked Kararlar (Dokunma)

- Karar #98: cyan #00FFCC tek ambient accent
- Karar #100: 35° angled top-down perspektif
- Karar #143: 6-layer Multi-Layer Painter
- Transform Squash: localScale.y = 0.819
- User-cannot-draw: manuel pixel paint YASAK

---

## Bu Prompt Ne İçin?

Bu prompt RIMA dungeon'larının yeni görsel ve mekansal yönünü tanımlıyor. İlk adım olarak:

1. Mevcut sahneyi (`RoomPipelineTest.unity`) incele
2. Mevcut sprite envanterini (`Assets/Art/AssetPacks/Act1_ShatteredKeep/`) kontrol et
3. Bu vizyona ulaşmak için nelerin değişmesi gerektiğini raporla:
   - Kamera zoom/follow ayarları
   - Harita boyut gereksinimleri
   - Eksik sprite tipleri (iç duvar, sütun, koridor duvarı vs.)
   - Room generation algoritma değişiklikleri
   - Lighting yaklaşımı
4. Küçük bir prototipi yap — mevcut sprite'larla bile olsa "büyük harita + yakın kamera + iç yapı" test et
5. Screenshot al, verdict yaz
