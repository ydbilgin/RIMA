# Hero Siege & Hammerwatch Visual Stil Araştırması
*Tarih: 2026-05-12 | Model: Gemini 3.1 Pro Preview (default) | Amaç: RIMA referans analizi*

---

## Hero Siege Visual Stili

**Panic Art Studios | Steam | Aktif geliştirme**

- **Karakter sprite boyutu:** ~32x48 px chibi format (büyük kafa, küçük gövde). Boss karakterler 64x64 veya 128x128'e çıkar.
- **Kamera açısı:** 3/4 quarter-view, yaklaşık 45-60° tilt. Pure overhead değil; hem zemin hem karakter ön cephe görülür.
- **Pixel density + palette:** Orta yoğunluk. Ana ton: toprak renkleri, gri-kahve desaturated ambient. Loot, sağlık ve büyü efektleri için yüksek satürasyon neon aksanlar kullanılır. Dithering minimumdur; engine blending ağırlıklı.
- **Lighting:** Modern engine-side dinamik 2D lighting (Unity/GameMaker tarzı). Ağır vignetting, glow particle efektleri, flat-shaded pixel art üzerine additive ışık çemberleri ile atmosfer oluşturulur.
- **Karakter:tile oranı:** Karakter (~48px yüksek) 32x32 tile'ın üzerine taşar, belirgin dikey presence sağlar.
- **Genel ton:** Kanlı, baskılayıcı dark fantasy arcade. Neon-renk yetenek efektleri sayesinde okunabilirlik korunur.

---

## Hammerwatch 1 Visual Stili

**Crackshell | 2013 | Klasik dungeon crawler**

- **Karakter sprite boyutu:** Çok küçük — 16x16 ile 16x24 px arasında.
- **Kamera açısı:** 3/4 top-down, ~45° tilt. Duvarlar belirgin dikey yüksekliğe sahip (shadow mekaniği için).
- **Pixel density + palette:** Düşük çözünürlük, "hi-bit retro" tarzı. Sınırlı ama canlı, yüksek kontrastlı palette; ağır dithering yok. SNES/Amiga geliştirilmiş estetiği gibi görünür.
- **Lighting:** Engine-driven gerçek zamanlı dinamik gölgeler. Işık kaynakları duvar ve sütunlara karşı hard, pixel-perfect gölge döker — oyunun visual identity'sinin temeli budur.
- **Karakter:tile oranı:** Karakterler (16x24) 32x32 mantıksal tile blokları içinde ya tam sığar ya daha küçük kalır — zindan büyüklüğünü vurgulamak için kasıtlı küçük tutulmuş.
- **Genel ton:** Klasik retro fantasy. Canlı, temiz, grid-odaklı.

---

## Hammerwatch 2 Visual Stili (HW1'den Farkı)

**Crackshell | 2024 | Open-world sequel**

- **Temel fark:** HW1'in düz tile-based retro dungeon yapısından yüksek detaylı "2.5D" açık dünya estetiğine geçiş.
- **Pixel density:** Belirgin biçimde artmış. Ortamlar yoğun bitki örtüsü, çeşitli taş dokular ve çevre kalabalığıyla hand-painted hissi verir; HW1'in bloklu grid anlayışından uzaklaşılmış.
- **Kamera açısı:** 3/4 top-down korunmuş, ancak artan asset vertikality ile dünya daha katmanlı ve derinlikli hissettiriyor.
- **Palette/Lighting:** Lighting engine tamamen yenilenmiş — tam gün/gece döngüsü, hava efektleri, soft ambient light blending. Güneş batarken palette doğal değişiyor; fenerler ve dinamik büyü ışıkları zorunlu hale geliyor.
- **Yeni unsurlar:** Kapsamlı karakter özelleştirmesi, detaylı kasaba iç mekanları, karmaşık ambient animasyonlar (rüzgarda sallanan çimen, su yansımaları).

---

## Cinderia Karşılaştırması (Kısa)

- **Kamera açısı:** Hero Siege ve Hammerwatch'ın ortogonal (kare grid) yapısından farklı olarak **gerçek 2.5D izometrik** kamera kullanır.
- **Pixel density:** Karakterler için daha yüksek yoğunluk — modern detaylı chibi estetiğine yakın.
- **Palette:** "Hades yaklaşımını" mükemmel yansıtır: ortam ve zemin ağırlıklı desaturated "kül" tonları, combat yeteneği ve karakter aksanları için neon magenta, mor ve mavi vurgular.

---

## RIMA Aesthetic Match Tablosu

| Oyun | Puan (1-10) | Gerekçe |
|------|-------------|---------|
| **Hyper Light Drifter** | 9/10 | 35° tilt, desaturated zemin + neon combat VFX, Fractured Epic'in gold standard'ı |
| **Cinderia** | 9/10 | Chibi karakter oranı, dark fantasy tonu, yüksek kontrastlı VFX — izometrik olması tek eksi |
| **Hero Siege** | 8/10 | 64px chibi + 32px tile teknik uyumu mükemmel; ton biraz "mobile arcade" hissi veriyor |
| **Eitr** | 7/10 | Karanlık combat estetiği güçlü; ama izometrik açı ve agresif düşük satürasyon RIMA'nın canlı tonundan uzak |
| **Hammerwatch 2** | 6/10 | Dinamik lighting güçlü; ama art direction geleneksel fantasy RPG'ye yakın, stylized roguelite değil |
| **CrossCode** | 4/10 | Mükemmel pixel art ama parlak 16-bit SNES-anime tonu ve platforming perspektifi Fractured Epic'ten kopuk |
| **Hammerwatch 1** | 3/10 | Çok retro, çok küçük (16x16), palette/lighting modern URP 2D için yetersiz |
| **Tunic** | 2/10 | 3D low-poly + tilt-shift shader oyun — pixel art değil, PixelLab workflow'una uygulanamaz |

---

## PixelLab Üretilebilirlik

- **Hero Siege stili:** Yüksek üretilebilir. 64x64 chibi formatı AI pixel art için ideal canvas — bu çözünürlükte helmet, göz, zırh detayları net çıkar, noise'a dönüşmez.
- **Hammerwatch 1 stili:** Çok zor. AI, 16x16 ultra-düşük çözünürlükte son derece zorlanır. Bu ölçekte her piksel anatomiyi ima eder; AI çoğunlukla okunaksız gürültü üretir.
- **64px chibi sweet spot:** Hero Siege oranları (64px karakter / 32px tile) PixelLab Create Character için kesinlikle en uygun hedef. Tek geçişte yüksek detaylı, okunabilir karakterler üretmek bu çözünürlükte mümkün; environment tile'ları ise sade ve modüler kalır.

---

## TOP 3 Ranking + Rationale

**1. Hyper Light Drifter**
Birincil lighting, palette ve kamera açısı referansı. 35° tilt, kasvetli desaturated zindan ortamları ve agresif neon-renkli combat VFX kombinasyonu Fractured Epic'i mükemmel karşılıyor. URP 2D Renderer + Pixel Perfect Camera bu tarzı doğrudan replicate edebilir.

**2. Cinderia**
Birincil karakter oranı ve ton referansı. Stylized chibi karakterin karanlık bir aksiyon roguelite ortamında nasıl çalıştığını kanıtlıyor — PixelLab'in 64px create character workflow'una da doğrudan uyumlu.

**3. Hero Siege**
Birincil teknik scaling referansı. 64px+ karakter ile 32px tile grid'in top-down ARPG'de birlikte nasıl çalıştığını gösteriyor. PixelLab çıktılarının ekranda orantısız görünmeyeceğini kanıtlayan somut proof-of-concept.

---

## ONERILEN Pin (RIMA Reference Candidate)

**Hyper Light Drifter** — birincil stil referansı olarak pin edilmeli.
**Cinderia** — ikincil karakter oranı ve VFX ton referansı.
**Hero Siege** — teknik scaling kontrolü için (tile:karakter oranı doğrulama).

*Not: Hammerwatch 1 ve 2 RIMA için doğrudan stil referans adayı değil. HW2'nin dinamik lighting yaklaşımı (gün/gece, ambient blend) ilerleyen fazlarda ilham kaynağı olabilir.*

---

*Kaynak: Gemini 3.1 Pro Preview (default model, web search aktif) — 2026-05-12*
*Güven: MEDIUM-HIGH. Oyun boyutları için pixel-exact ölçümler mevcut değil; genel bilgi + yayınlanmış görseller baz alındı.*
