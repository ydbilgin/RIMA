# RIMA — Dungeon Oda Tasarım Felsefesi
> **Referans:** Infamous Keepers'tan mood + yapı, RIMA combat sistemine uyarlanmış.
> **Ne zaman yükle:** Yeni oda prefabı yaparken, tilemap tasarlarken, rima-codex'e oda gorevi verirken.

---

## Temel Felsefe (4 Kural)

1. **Her oda bir cümle.** "Düşman seni kapıdan girince köşeye sıkıştırır" → anlaşılır dövüş koşulu. "Vur, kaç, geri gel" akışı her odada çalışmalı.
2. **Isometric = derinlik illüzyonu, netlik zorunluluğu.** Duvarlar yüksek görünür ama düşmanlar, mermiler ve aktif zemini gizlemez. Readability > atmosfer.
3. **Dash lane her odada var.** Iron Charge 8m — bu mesafeyi engelleyen oda YASAK. Her odada en az 2 yönde 8+ unit temiz alan olacak.
4. **Authored feeling, procedural bağlantı.** Odaların içi elle tasarlanmış hissettirmeli. DungeonGraph N/E/W bağlantıları runtime'da rastgele — odaların kendi kimlikleri bunu taşır.

---

## Isometric Özel Kurallar

### Perspektif Gerçekliği
- Kamera: diamond zemin, köşegen bakış (Hades / Diablo 2)
- Kuzey = sol-üst köşe, Doğu = sağ-üst köşe, Güney = sağ-alt, Batı = sol-alt
- Kapılar: **N = sol-üst duvar, E = sağ-üst duvar, W = sol-alt duvar** (South yok)
- Duvarlar ön yüzlü: sadece flat-top tile değil, görünür yükseklik zorunlu

### Sorting Katmanları (RIMA standardı)
```
Ground → Walls → Entities → VFX
```
- Obstacle'lar Entities katmanında — oyuncu arkasına geçince gizlenmeli
- Boss arenalarında zemin desenleri Ground'da kalır, asla Walls'a çıkmaz

### Görünürlük Kısıtları
- Obstacle yüksekliği: maksimum 2 tile → 128px oyuncu arkasına tamamen kaybolmasın
- Dar geçitlerde mob silüeti görünür kalmak zorunda
- Merkezdeki tehlike (altar, ritual circle, chasm) asla obstacle ile çevrilmez

---

## Oda Boyut Kataloğu

> Tilemap birimi: 1 tile = 0.5×0.5 Unity unit (isometric grid, cellSize 1×0.5×1)
> Referans oda: 32×24 tiles

| Oda Tipi | Tilemap Boyut | Dövüş Alanı | Kapı Sayısı | Faz |
|----------|--------------|-------------|-------------|-----|
| Giriş Koridoru | 10×24 | dar geçit | 1→1 (N) | 1+ |
| Standart Dövüş | 24×18 | 18×12 temiz | 1-2 | 1+ |
| Geniş Dövüş | 28×20 | 22×14 temiz | 1-3 | 1+ |
| Yan Oda (reward) | 16×12 | — | 1 (geri) | 2+ |
| Elite Arenası | 28×22 | 22×16 temiz | 1-2 | 1+ |
| Miniboss Odası | 32×24 | 26×18 temiz | 1 | 2+ |
| Boss Arenası | 40×30 (özel) | 34×22 temiz | 1 (giriş only) | 1+ |
| Lanetli Oda | 20×16 | 14×10 temiz | 1-2 | 2+ |
| Dükkan/Nefes | 20×16 | — | 1-2 | 2+ |

**Faz 1 gerçeği:** Sadece Standart Dövüş, Elite Arenası, Boss Arenası kullanılır.

---

## Oda Kimlik Sözlüğü — Act 1 (Sunken Keep)

### Faz 1 Aktif Tipler

#### 1. Giriş Salonu (Entry Hall)
- **Boyut:** 24×18
- **Şekil:** Dikdörtgen, girişte dar → açılıyor
- **Obstacle:** Çökmüş 2 sütun (simetrik), arka duvarda banner kalıntısı
- **Mob:** 2-3 grunt, yan köşelerde başlar
- **Atmosfer:** Çatlak taş zemin, soluk meşale, duvarda zincir izi
- **Kural:** İlk oda — okuma kolay, baskı az, "oyun başladı" hissi

#### 2. Bekleme Salonu / Devriye Odası (Guard Hall)
- **Boyut:** 24×18
- **Şekil:** Dikdörtgen, 2 köşede kısa duvar çıkıntısı (L-cover)
- **Obstacle:** 2×2 taş blok ortada (bölücü), kırık tahta mobilya
- **Mob:** 3-4 grunt, ortada veya köşelerde
- **Amaç:** Standart dövüş — block etrafında kovalamaca

#### 3. Dar Geçit (Transition Corridor)
- **Boyut:** 10×24
- **Şekil:** Uzun koridor, 2 küçük alcove (nişler) kenarda
- **Obstacle:** Çökmüş sütun parçası nişlerde (geçişi engellemez)
- **Mob:** 1-2 mob ambush (koridor ortasında spawn)
- **Kural:** Baskı anı — açık alandan dar geçişe ritim kırıcı

#### 4. Ritüel Dairesi (Ritual Chamber)
- **Boyut:** 26×20
- **Şekil:** Geniş dikdörtgen, ortada büyük circular yer deseni (tehlikeli alan)
- **Obstacle:** 4 sütun köşelerde (oda kenarlarına yakın, merkez açık), ortada kırık altar
- **Mob:** 3 grunt daire etrafında yerleşik, opsiyonel 1 void-touched
- **Tehlike:** Ritual circle aktif → içinde durma veya hazard zone
- **Atmosfer:** Rün işaretleri zeminde, soğuk mavi ışık patikası, duvarda kan izi

#### 5. Hücre Bloğu / Tutukevi (Prison Block)
- **Boyut:** 28×20
- **Şekil:** Merkez koridor + 2 yan hücre sırası (kısa duvarlarla bölünmüş)
- **Obstacle:** Hücre duvarları = kısa engel (1×3 tile), 2-3 kırık kafes
- **Mob:** Hücre içinden çıkma hissi — mob'lar kenarlarda, ortaya saldırı
- **Atmosfer:** Zincir, kafes, çürük tahta, paslanmış demir kapı

#### 6. Elite Arenası (Champion Hall)
- **Boyut:** 28×22
- **Şekil:** Geniş dikdörtgen, köşelerde 2×2 platform çıkıntısı (elevation hissi)
- **Obstacle:** 2 sütun simetrik (giriş tarafına yakın), merkez BOŞ
- **Mob:** 1 Elite (160-192px) merkez → 2 grunt yan
- **Kural:** Elite göründüğünde alan açık — oyuncu kaçma ve yaklaşma seçeneğini görmeli
- **Ödül:** Elite ölünce odanın ortasında item/skill drop

#### 7. Boss Arenası — Penitent Sovereign (Throne Hall)
- **Boyut:** 40×30 (büyük, özel tilemap)
- **Şekil:** Geniş dikdörtgen, arka köşede platform yükseltisi (boss durur), girişte 2 sütun
- **Obstacle:** Giriş sütunları (boss öncesi cover, boss sonrası engel), zincir kalıntıları yerde
- **Mob:** Boss tek başına (Faz 1 test: sadece 1 faz)
- **Kural:** Girişte sütun = ilk hareket için siper. Boss platformu = yükseklik hissi. Alan açık — Gravity Cleave, Iron Charge tam çalışmalı.
- **Atmosfer:** Devasa zincirler tavana kadar, çatlak taht, zemin kan lekesi, soğuk mavi rift ışığı sütunlardan sızıyor

---

## Obstacle Tasarım Kuralları

### Onaylı Obstacle Tipleri (Act 1)

| Obstacle | Boyut (tile) | Kullanım | Yasak |
|----------|-------------|----------|-------|
| Taş Sütun | 1×1 | Taktiksel siper, çift → koridor dar geçit | Oda ortasına tek |
| Kırık Duvar | 2×1 veya 3×1 | L-cover, partial wall | Tam çizgi → kaçış yolunu kesiyor |
| Küçük Altar | 2×2 | Merkez tehlike, ritüel atmosfer | Elite odalarında merkez |
| Mezar Taşı | 2×1 | Crypt odalarda, yüksekliği az | Yoğun küme |
| Kafes (Cage) | 1×2 | Dekorasyon + ince siper | Mob görünürlüğünü engelleme |
| Chasm (Uçurum) | 3×2+ | Tehlike zone, geçilmez | Çıkışı kesiyor |
| Meşale Direği | 1×1 | Işık noktası, sadece dekorasyon | Obstacle olarak kullanma |

### Yerleşim Kuralları
- Obstacle **asla** kapı önünü blocklamaz (2 tile clearance her kapı önünde)
- Merkez alan: minimal obstacle — boss ve elite odalarında MERKEZİ BOŞ BIRAK
- Sütun çifti = koridor oluşturur → en az 4 tile aralıklı koy (dash geçebilmeli)
- Chasm kenarında 1 tile düz zemin → oyuncu kontrolsüz düşmez

---

## DungeonGraph Entegrasyonu

### Mevcut Sistem
- Her oda 1-3 çıkışa sahip: %25 sadece N, %40 N+E, %25 N+W, %10 N+E+W
- South çıkış yok — oda tasarımlarında güney duvarı her zaman kapalı
- Exit olmayan kapılar: Hidden state (görünmez)

### Oda Prefab Tasarım Kuralı
Her oda prefabında 3 potansiyel kapı slot'u var (N/E/W).  
Kapı slotları **her zaman temizlenmiş** — önünde 3 tile boş alan olmalı.  
DungeonGraph hangi kapının açık olduğuna runtime'da karar verir.

```
Oda tilemap tasarlarken:
  - Kuzey duvar ortasında: 4 tile boşluk (N kapısı için)
  - Doğu duvar yukarısında: 4 tile boşluk (E kapısı için)
  - Batı duvar yukarısında: 4 tile boşluk (W kapısı için)
  - Bu boslukları doldurmak YASAK (rima-codex kurali)
```

---

## Act 1 Oda Dizilim Şablonu (Faz 1)

```
[Giriş Salonu] → [Guard Hall] → [Dar Geçit] → [Ritüel Dairesi] → [Elite Arenası] → [Throne Hall BOSS]
                              ↗
              [Hücre Bloğu]
```

**Rhythm:** baskı → nefes (dar geçit) → baskı → escalation (elite) → peak (boss)

**Kapı tipleri (görsel okuma):**
- Normal kapı: taş kemer
- Elite kapı: kan izi + mühür işareti (kırmızı)
- Boss kapı: zincirli + kapalı + kırık seal (mavi rift ışığı)

---

## Görsel Dil (Act 1 — Sunken Keep)

### Materyal Seti
- Zemin: `#2C2A2A` koyu enkaz taşı, hairline mavi rift çatlaklar
- Duvar: `#4A3F3F` ön yüzlü taş, gölgeli taban
- Vurgu: meşale turuncusu (`#C4682A`), soğuk mavi (`#7BA7BC`), kan kırmızı (`#8B1A1A`)

### Atmosfer Prop Seti (PixelLab ile üretilecek)
- Zincir sarkan tavan bağlantısı
- Duvarda çatlak + rift ışığı sızıntısı
- Kırık rün taşı
- Çömlek kemik parçası / kafatası
- Yanmış mum + mum mumluğu
- Kan lekesi zemin deseni
- Paslanmış demir kafes
- Rulo banner (lacivert + soluk amblem)

### YASAK Görseller
- Açık renkli veya parlak zemin (okunabilirlik bozar)
- Boyutlu dekorasyon (mob ile karışır)
- Tekdüze düz duvar (yükseklik hissi ölür)
- Ortada büyük obstacle (boss/elite oda)

---

## rima-codex Is Notlari

Her oda prefabı yapılırken şu kontrol listesi:
```
[ ] N/E/W kapı slotları temizlenmiş (3 tile boşluk)
[ ] Merkez alan açık (elite/boss oda = kesinlikle)
[ ] En az 2 yönde 8+ tile düz alan (dash lane)
[ ] Obstacle tile yüksekliği ≤ 2 tile görsel yükseklik
[ ] Düşman spawn noktaları obstacle içine gömülmüyor
[ ] Isometric sorting: Ground → Walls → Entities
```

**SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.**
