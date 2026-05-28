# Infamous Keepers — RIMA Karşılaştırma Analizi
*Tarih: 2026-05-10*
*Kaynak: https://store.steampowered.com/app/4502750/Infamous_Keepers/*

## Screenshot Envanteri
- `01_combat_skeletons.jpg` — F1-style kombat, gri zemin, skeleton swarm + bandit squad
- `02_combat_traps.jpg` — F2-style kombat, kahverengi zemin, çoklu damage number stack
- `03_tavern_hub.jpg` — Hub/tavern bölgesi, sıcak ışık, NPC kümesi
- `04_skill_draft.jpg` — 3-kart draft UI, rarity tier sistemi

---

## 1. Kamera & Perspektif

### Infamous Keepers
- **High top-down isometric ~30-35°**
- 2:1 izometrik projeksiyon (diamond zemin pattern net görünüyor)
- Sabit kamera (rotasyon yok)

### RIMA LOCKED
- 35° High Top-Down ARPG açısı (Diablo 2/PoE tarzı)
- 2:1 izometrik (26.57° geometrik)
- 4 kardinal yön sprite (S/E/N/W)

### **Sonuç: TAM EŞLEŞME ✓**
Kamera kararımız doğrulandı. Aynı görsel dil. Onlar profesyonel ürün ve aynı açıyı seçmiş — bizim spec sağlam.

---

## 2. Wall Occlusion & Visual Shell

### Infamous Keepers (Image 1)
- Üst duvarlar çerçevenin dışında temiz kesiliyor
- Kuzey duvarlar arkasında oyuncu olduğunda fade var (image 1'de açıkça görünüyor — duvar üst köşeden yukarı)
- "Visual Shell" yaklaşımı net — kombat alanı duvarla çevrelenmiş, dışı yok

### RIMA LOCKED
- WallOcclusionFader (minAlpha 0.38, fadeRadius 2.2, fadeSpeed 10)
- Padding YASAK — Visual Shell ile sınırla
- Hades-tarzı duvar saydamlaştırma

### **Sonuç: AYNI YAKLAŞIM ✓ — ufak öğrenme var**
Onların duvar kesimi kamera çerçevesinin sınırında çok temiz. Bizim WallOcclusionFader implementation'ı edge case'leri (kamera kenarına yakın duvar parçaları) iyi yönetmeli. **Aksiyon:** WallOcclusionFader test scene'inde kamera kenarına yapışık duvar test edilsin.

---

## 3. Floor Tiles & Decoration

### Infamous Keepers
- **Image 1**: Soğuk gri taş zemin, diamond pattern, çok yoğun decal (kan lekesi, kemik tozu, çizik) — F1 style
- **Image 2**: Kahverengi/turuncu zemin (ahşap?), benzer decal yoğunluğu — F2 style
- **Image 3**: Ahşap taban, hub teması
- **Decal density yüksek** — neredeyse her tile'da bir iz var (kan, kemik, çizik, scorch)
- Combat alanı orta kısmı daha temiz (oynanabilirlik için)

### RIMA LOCKED
- F1/F2/F3 üç katman, Transition Tile'lar (Stone-to-Earth, Masonry-to-Rock)
- "Connected Generation" — scatter YASAK, her decal mantıklı kaynağa bağlı
- Decal F1 16 varyasyon spec'te
- Combat floor merkezi temiz (engel yok)

### **Sonuç: DOĞRU YOLDA — decal varyasyonu artırılabilir**
Onlar zemini çok zenginleştirmiş, RIMA spec'i 16 decal sınırlı. **Öneri:** Decal pool'unu 20-24'e çıkarmak. Özellikle "kuru kan lekesi" varyasyonları (büyük/orta/küçük + farklı şekil) için ekstra üretim. Bu görsel yoğunluk hissi farkını yaratıyor.

---

## 4. Skill Draft UI (Image 4 — Kritik Karşılaştırma)

### Infamous Keepers
- **3-kart draft** (Trait / Skill / Impact on towers)
- 3 rarity tier görünüyor: **Uncommon** (sade), **Epic** (mor parlak), **Rare** (mavi)
- Her kart: ikon + isim + tier label + 1-2 satır efekt
- Epic kartın arkasında **mor glow kolonu** — net görsel hiyerarşi
- Kafatası/kemik temalı çerçeve
- Üst bar: kaynak göstergeleri (gold/lives/red gem/blue gem)
- Alt bilgi: "Kamikaze - Level 2 reached!" — upgrade flow

### RIMA LOCKED
- 3-seçenek Hades-tarzı draft
- 4 tier: Common (Gri), Rare (Cyan/Blue), Epic (Mor), Legendary (Altın)
- Sadece ikon + isim + tip + tier + 1 satır + sinerji tag
- Slide in animasyonu, hover glow, seçim flash
- Statik metin baked PNG değil (RimaUITheme runtime)

### **Sonuç: TASARIM PARALEL — bizim 4-tier sistem daha derin**
Tier renk şeması neredeyse aynı (Epic=mor, Rare=mavi). Onlar 3 tier kullanıyor, biz 4 tier'a sahibiz (Legendary altın eklenmiş). **Öğrenilecek nokta:** Onların Epic kartının arkasındaki **mor glow kolonu** çok güçlü — görsel hiyerarşi anında okunur. Bizim implementation'da tier'a göre arka plan glow column eklenmeli.

**Aksiyon önerisi:** RIMA Skill Draft prefab'ında tier'a göre glow column intensity:
- Common: glow yok
- Rare: orta cyan glow
- Epic: güçlü mor glow (Infamous Keepers referans)
- Legendary: en güçlü altın glow

---

## 5. Floating Damage Numbers (Image 1 & 2)

### Infamous Keepers
- **Büyük kırmızı sayılar**, kalın font, koyu outline
- Image 2'de **9 sayı aynı anda görünüyor** (4, 1, 18, 2, 1, 1, 2, 2, 2)
- Cluster halinde, çakışmıyor (akıllı pozisyonlama)
- Kritik vuruş "20" image 1'de büyük ve merkez

### RIMA LOCKED
- Floating damage numbers default ON
- 3-Layer Feedback (Normal/Commit/Break)
- Hit Flash + Hit Particles + Screen Shake + **Hit Stop 2-5 frame**

### **Sonuç: BIZ DAHA İYİ HISSEDECEK (Hit Stop var)**
Onların hit feedback'i güzel ama **Hit Stop yok gibi** görünüyor (statik screenshot'tan kesin söylenemez ama). Bizim Hit Stop özellikle ağır vuruşlarda farkı yaratacak.

**Öğrenilecek nokta:** Çoklu hit damage number cluster pozisyonlama. Birden fazla mob'a aynı anda vuran AOE skill'lerde sayıların üst üste binmemesi için pozisyon offset algoritması gerekiyor. **Aksiyon:** FloatingTextSpawner'a anti-overlap logic eklensin.

---

## 6. Hub Tasarımı (Image 3)

### Infamous Keepers
- Tavern teması: ahşap zemin, fıçılar, masalar, yemek
- 6+ NPC görünüyor (oturuyor, ayakta, çalışıyor)
- Sıcak ışık (şömine, mum) — kombat zindanlarına ZIT atmosfer
- Kalabalık ama okunaklı — her NPC ayrı bölgede

### RIMA LOCKED
- Hub canlı: NPC'ler döngüsel davranış (Ferryman gemide, Cartographer haritada)
- 4 ana NPC: Ferryman, Vrel, Sister Mourne, The Cartographer
- Hub Rest Pose (silah omuzda/belde)
- Dialogue'da camera zoom

### **Sonuç: ONLARIN HUB'I DAHA YOĞUN — referans olarak iyi**
Bizim spec 4 NPC. Onların tavern'inde 6+ var. **Öneri:** Background NPC'ler (silüet, isimsiz, döngüsel hareket) hub'ı doldursun. "Diğer ölmüş kahramanlar" gibi pasif karakterler — narrative açıdan da uygun.

**Aksiyon:** HUB_DESIGN_v1.md'ye "background NPC layer" eklensin (5-8 silüet, isimsiz, hub'da yaşam hissi için).

---

## 7. Atmospheric Lighting

### Infamous Keepers
- Çok karanlık ortam, lokalize ışık kaynakları
- Image 1: Kuzey köşede tek mum/torch, geri kalan loş
- Image 3: Şömine kaynaklı sıcak ışık, geri kalan ortam loş
- Light source visible (her ışığın kaynağı görünür)

### RIMA LOCKED
- Tüm Point Light 2D fiziksel kaynağa bağlı (havada asılı YASAK)
- F1: %70-90 amber torch, F3: %10-30 amber + cyan rift kristal
- Intensity 0.18-0.32 (kısık)

### **Sonuç: FELSEFEMIZ AYNI ✓ — biz daha sistematik**
Onlar da physical light source kullanıyor. Bizim katman bazlı (F1→F3 amber→cyan dönüşümü) yaklaşımımız daha planlı. Bu spec doğru, devam.

---

## 8. Toplam Değerlendirme

### Eşleşme Skoru
| Sistem | Eşleşme |
|---|---|
| Kamera & perspektif | %100 ✓ |
| Wall occlusion | %95 ✓ (edge case'leri test et) |
| Floor decoration | %85 (decal varyasyon arttırılabilir) |
| Skill draft UI | %90 (glow column ekle) |
| Damage numbers | %85 (anti-overlap logic ekle) |
| Hub design | %75 (background NPC eklenebilir) |
| Atmospheric lighting | %100 ✓ |

### RIMA Avantajı (onlarda yok / zayıf)
1. **Hit Stop sistemi** — combat juice anahtarı
2. **4-tier rarity** (Legendary) — derinlik fazlası
3. **Shadow Echo cross-class system** — özgünlük
4. **Connected Generation** — scatter yasağı, mantıklı dekorasyon
5. **Hero-focused gameplay** — onların squad chaos'u bizim power fantasy'mizle çelişir

### RIMA'nın Alabilecekleri (somut aksiyonlar)
1. **Decal pool 16 → 22 varyasyon** (kan lekesi büyük/orta/küçük + farklı şekiller)
2. **Skill Draft prefab'ında tier-based glow column** (Common no glow → Legendary güçlü altın)
3. **FloatingTextSpawner anti-overlap algoritması** (cluster pozisyonlama)
4. **Hub'a 5-8 background NPC silueti** (yaşam hissi)
5. **WallOcclusionFader edge case test scene** (kamera kenarına yapışık duvar)

### Almayacağımız (genre/tasarım çakışması)
1. Squad-based gameplay
2. Tower defense element
3. Aşırı yoğun duvar dekorasyonu (skull spam)
4. Karışık combat readability (bizim hero-focused yaklaşım korunmalı)

---

## Genel Yargı
**Infamous Keepers, RIMA'nın görsel dili ve sistem kararlarıyla %85+ paralel.** Bu validation. Sıfırdan icat etmiyoruz — denenmiş bir görsel reçeteyi kullanıyoruz. **Onlarda eksik olan ama bizde olan kritik şeyler (Hit Stop, 4-tier rarity, Shadow Echo, hero focus) RIMA'nın özgün alanları.**

Sıfır risk yok ama somut 5 küçük iyileştirme alındığında pipeline daha güçlü olur.
