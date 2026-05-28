# LaurethStudio 2D Illusion Tricks & Knowledge Library

**Source:** agy dispatch `b8xc1t05i` (2026-05-26 S109 close)
**Task file:** `STAGING/s109_lauretthstudio_2d_illusion_knowhow_agy.md`
**Scope:** Studio-level, RIMA-bağımsız. Tüm LaurethStudio 2D ve hibrit 2.5D oyun projelerinde reusable.
**Engine target:** Unity 2022.3+, URP / Built-in karışık. PixelLab pipeline (S-XL Pro / Map Object / Tiles Pro / Edit Image).

---

## Catalog — 32 Teknik

Kısaltmalar:
- **Türler:** RL (Roguelite), CZ (Cozy Farm/Sim), RPG (Role-Playing Game), PL (Platformer), AR (Retro Arcade), IC (Casual Incremental)
- **Uyum:** Y (tam) / P (kısmi) / N (uyumsuz)
- **Efor:** XS (1 sprite, 5 dk) / S (sprite + setup 30 dk) / M (multi-sprite + shader 2-4 saat) / L (full system 1 gün+)

| Teknik | Kategori | Klasik (yıl) | Modern Indie | Tür Uyumu | Efor | Mekanizma |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **Parallax Scrolling** | Depth & Perspective | *Shadow of the Beast* (1989) | *Hollow Knight* (2017) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:P | S | Kamera hızına göre farklı katmanların ters oranda kaydırılması |
| **Forced Perspective** | Depth & Perspective | *Super Mario RPG* (1996) | *Eastward* (2021) | RL:Y/CZ:Y/RPG:Y/PL:N/AR:P/IC:N | M | 2D düzlemde 3D derinlik vermek için sprite'ların dimetrik/izometrik açıyla üst üste çizilmesi |
| **Atmospheric Depth Tint** | Depth & Perspective | *Super Metroid* (1994) | *Ori and the Blind Forest* (2015) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | S | Arka plan katmanlarının kamera uzaklığına göre sis/atmosfer rengine kademeli boyanması |
| **Vanishing Point Scale Rails** | Depth & Perspective | *Space Harrier* (1985) | *Death Road to Canada* (2016) | RL:Y/CZ:N/RPG:P/PL:P/AR:Y/IC:N | M | Sprite Y pozisyonu azaldıkça ölçeğinin logaritmik küçültülerek ufuk çizgisine yaklaşması |
| **Palette Light Ramps** | Lighting & Shading | *Doom* (1993) | *Caves of Qud* (2015) | RL:Y/CZ:P/RPG:Y/PL:P/AR:Y/IC:Y | S | Piksel renklerinin ışık şiddetine göre önceden tanımlı renk paleti rampaları üzerinden kaydırılması |
| **Dithered Shadow Decals** | Lighting & Shading | *Chrono Trigger* (1995) | *Sea of Stars* (2023) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:P | XS | Karakter altına dama tahtası (checkerboard) piksel gölge sprite'ı |
| **Dynamic Bevel Fake Normal** | Lighting & Shading | *Street Fighter III* (1997) | *The Last Spell* (2021) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | M | Sprite kenarlarına ışık yönü vektörüne göre shader ile specular ve gölge eklenmesi |
| **Poor Man's Bloom & Godrays** | Lighting & Shading | *Castlevania: SOTN* (1997) | *Blasphemous* (2019) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | S | Additive blending konik ışık sprite'larının statik veya hafif sallantılı yerleşimi |
| **Mode 7 Floor Projection** | Motion | *F-Zero* (1990) | *Octopath Traveler* (2018) | RL:P/CZ:P/RPG:Y/PL:N/AR:Y/IC:N | L | Düz 2D harita dokusunun 3D perspektif matrisiyle trapezoidal yamultularak zemin olarak çizilmesi |
| **Sub-Pixel Scroll Locking** | Motion | *Mega Man 2* (1988) | *Celeste* (2018) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | S | Sprite pozisyonlarının PPU gridine yuvarlanarak jitter önlenmesi |
| **Sprite Scaling Pseudo-Zoom** | Motion | *OutRun* (1986) | *Enter the Gungeon* (2016) | RL:Y/CZ:P/RPG:Y/PL:Y/AR:Y/IC:P | S | Kamera zoom yerine sprite transform ölçeklerinin pivot noktasına orantılı büyütülmesi |
| **POV Sprite Flicker** | Motion | *Zelda II* (1987) | *Shovel Knight* (2014) | RL:Y/CZ:N/RPG:P/PL:Y/AR:Y/IC:N | XS | Hasar anında SR'ın 1 frame açık 1 frame kapalı transparanlık illüzyonu |
| **Parallax Velocity Mismatch** | Motion | *Sonic the Hedgehog* (1991) | *Katana Zero* (2019) | RL:Y/CZ:P/RPG:P/PL:Y/AR:Y/IC:N | S | Aynı katmandaki sprite parçalarının dikey eksende farklı hızlarda kayarak derinlik vermesi |
| **Skewed Z-Billboarding** | Volume & 3D | *Doom* (1993) | *Don't Starve* (2013) | RL:Y/CZ:Y/RPG:Y/PL:N/AR:P/IC:N | M | 3D uzaydaki 2D sprite'ların Y ekseninde kameraya skew ederek derinlik kazanması |
| **HD-2D Hybrid Depth** | Volume & 3D | *Ragnarok Online* (2002) | *Octopath Traveler* (2018) | RL:Y/CZ:Y/RPG:Y/PL:N/AR:N/IC:N | L | 3D tilemap zemin üzerine 2D billboard sprite'ların shadow map ile oturtulması |
| **Fake Screen-Space Reflection** | Volume & 3D | *Street Fighter II* (1991) | *Kingdom: Two Crowns* (2018) | RL:P/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | M | Zemin altı sprite'ların dikey simetri + opaklık azaltma + wave shader ile çizilmesi |
| **Water Plane Refraction Fake** | Volume & 3D | *Super Mario World* (1990) | *Stardew Valley* (2016) | RL:P/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | M | Su sprite arkasındaki UV koordinatlarının sine wave ile saptırılarak kırılma hissi |
| **Dimetric Squash** | Volume & 3D | *Landstalker* (1992) | *CrossCode* (2018) | RL:Y/CZ:Y/RPG:Y/PL:N/AR:P/IC:N | S | 3/4 üstten görünümde sprite'ların dikey eksende %86 basık çizilerek 3D izometrik hissi |
| **Sine-Wave Dust Motes** | Particle & Atmosphere | *Super Metroid* (1994) | *Dead Cells* (2018) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | S | Havada uçuşan toz zerrelerinin sin tabanlı rüzgar ve salınım formülüyle hareketi |
| **Layered Scroll Mist** | Particle & Atmosphere | *Castlevania: SOTN* (1997) | *Dead Cells* (2018) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | S | Farklı opaklıkta noise dokusunun farklı hızlarla UV kaydırılmasıyla derin sis |
| **Ember Wind & Drag** | Particle & Atmosphere | *Chronos* (1995) | *Hades* (2020) | RL:Y/CZ:P/RPG:Y/PL:Y/AR:Y/IC:N | S | Kıvılcımların yukarı çıkarken rastgele X eksende sarsılması ve zamanla sönümlenmesi |
| **Sparkle Pulse & Fake Flare** | Particle & Atmosphere | *Final Fantasy VI* (1994) | *Owlboy* (2016) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | XS | Değerli eşya köşelerinde 4 kollu yıldız sprite'ı rotate + scale animasyonu |
| **Palette Cycling** | Time & Rhythm | *Zelda: Link's Awakening* (1993) | *Shovel Knight* (2014) | RL:P/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | S | Doku pikselleri sabit, palet indeksleri döngüsel değiştirilerek su/ateş akışı |
| **Sprite Ghost Trails** | Time & Rhythm | *Mega Man X* (1993) | *Katana Zero* (2019) | RL:Y/CZ:N/RPG:P/PL:Y/AR:Y/IC:N | S | Hızlı hareket eden karakterin arkasında belirli aralıklarla fade-out sprite kopyaları |
| **Squash & Stretch Easing** | Time & Rhythm | *Super Mario World* (1990) | *Celeste* (2018) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:P | S | Zıplama/düşmede sprite'ın hacmini koruyarak dikeyde uzatılıp basılması |
| **ASMR Tile Crackle Sync** | Time & Rhythm | *Dig Dug* (1982) | *SteamWorld Dig* (2013) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | S | Kırılan karo/taş parça frame'leri ses dalga boyu zirveleriyle senkron |
| **Dynamic Letterboxing** | Composition & Framing | *Out of This World* (1991) | *Hades* (2020) | RL:Y/CZ:N/RPG:Y/PL:Y/AR:N/IC:N | XS | Kritik vuruş/boss/diyalog anında alt-üst siyah barların yumuşakça daralması |
| **Foreground DoF Blur** | Composition & Framing | *Donkey Kong Country* (1994) | *Ori and the Will of the Wisps* (2020) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:N/IC:N | M | Kameraya çok yakın ön plan nesnelerinin Gaussian blur shader ile bulanıklaştırılması |
| **Edge Vignette Fade** | Composition & Framing | *Resident Evil 2D* (1996) | *Darkest Dungeon* (2016) | RL:Y/CZ:N/RPG:Y/PL:P/AR:P/IC:N | XS | Ekran kenarlarını karartan radyal gradyan ile oyuncu odağının merkezde toplanması |
| **Fog of War Viewport Reveal** | Composition & Framing | *Diablo* (1996) | *Teardown / Indie RPGs* | RL:Y/CZ:P/RPG:Y/PL:N/AR:P/IC:N | M | Oyuncu etrafındaki görünür alanın Render Texture mask + yumuşak fırça ile çizilmesi |
| **Pixel-Perfect Snap** | Engine & Render | *NES Zelda* (1986) | *Shovel Knight* (2014) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:Y/IC:Y | S | Kamera koordinatlarının RoundToInt ile piksel sınırlarına kilitlenmesi |
| **Shader Graph UV-Offset Wave** | Engine & Render | *Chrono Trigger* (1995) | *Eastward* (2021) | RL:Y/CZ:Y/RPG:Y/PL:Y/AR:P/IC:N | M | Sprite UV haritasının Time parametreli sin dalgaları ile kaydırılması (bayrak/ot) |

---

## 1. Studio Cookbook — TOP 10 Cross-Game Patterns

LaurethStudio'da yapılacak her oyunda doğrudan uygulanabilecek, görsel kaliteyi en üst seviyeye taşıyan 10 temel şablon:

1. **Dual-Axis Parallax with Ground Anchor** — Hem yatay hem dikey parallax, zemin seviyesi katmanı oyuncuya kilitlenir. Derinlik kopmaz.
2. **Juice-Up Squash & Stretch on Jump/Land** — Zıplarken %15 uzat, inerken %15 bas. Premium fiziksel his.
3. **Low-Opacity Vignette Overlay** — %10-15 kenar kararması. Karanlık roguelite veya RPG'lerde olmazsa olmaz.
4. **Foreground Depth-of-Field (DoF) Layering** — Ön plan branch/sütun blur'ı sahne hacmini iki katına çıkarır.
5. **Dithered Drop Shadows for Air-Ground Relation** — Düz transparan daire yerine dithered → piksel sanatına oturur.
6. **Sub-pixel Movement with Integer Rendering** — Physics float, render `Mathf.Round`. Akıcı hareket + tutarlı piksel.
7. **Dynamic Screen Shake with Positional Decay** — Sin + noise, decay ile piksel sınırında durur. Impact juice.
8. **Poor Man's Bloom (Sprite Stacking)** — Gerçek Bloom piksel sanatını bulandırır. Additive Blend kopya yerleştir.
9. **Color Temperature Palette Shimmer** — Gece mavi/mor multiply, meşale etrafı sıcak turuncu. Dinamik ambient illüzyonu.
10. **Cinematic Viewport Reveal (Letterbox)** — 21:9 daraltma, sinematik mod uyarısı bilinçaltı.

---

## 2. Pixel Art-Specific Tricks & PixelLab Prompt Formulas (TOP 10)

1. **Dithered Shadow Texture:** `pixel art dithered shadow pattern, black and transparent checkerboard texture, retro game asset, clean lines, 16-bit --no shading, no blur`
2. **Isometric Flat Sprite:** `isometric 2.5d style [object], pixel art, 3/4 view, orthographic projection, detailed pixel texture, asset for cozy rpg, sharp outline --no perspective distortion`
3. **Tileable Parallax Sky:** `seamless tileable pixel art horizontal background, distant mountains and sky, atmospheric perspective, cold purple tone, 8-bit style, clean repetition --no vertical seams`
4. **2D Normal Map Fake (heightmap):** `grayscale depth map of [object], pixel art heightmap, white highlights for high points, dark grays for crevices, clean bumps --no colors`
5. **Water Plane Refraction Alpha:** `pixel art water caustic pattern, blue and cyan wave lines, tileable water surface texture, top-down perspective --no objects, no shores`
6. **Foreshortened Character:** `pixel art action pose [character], strong foreshortening, fist close to camera, body receding in distance, dynamic perspective, arcade style --no flat pose`
7. **Atmospheric Dust Mote Sheet:** `pixel art particle sprite sheet, glowing dust motes, simple 3x3 and 5x5 pixel sparkles, white and pale yellow, transparent background`
8. **Low-Frame Fire Animation:** `pixel art campfire animation sheet, 4 frames loop, stylized fire flame, retro arcade palette, clean pixel clusters --no interpolation`
9. **Vignette Edge Corner:** `pixel art vignette corners, dithered black border pattern, dark corners fading to transparent center, retro screen filter asset`
10. **9-Slice Border Frame:** `pixel art UI panel border, 9-slice compatible frame, stone or wood texture, corner decors, central area hollow and transparent --no interior fill`

---

## 3. Common Mistakes (Yeni Başlayanların Yaptığı 5 Hata)

1. **Uniform Parallax Velocity** — Tüm katmanlar yakın hızlarda → flat. Çözüm: logaritmik dağılım (en uzak %5, en yakın %150).
2. **Baked Light & Dynamic Light Overlap** — Sprite'taki baked yön + ters yönden dinamik light = confusion. Çözüm: sprite'ları nötr çiz, yönü shader/PainterSuite ile uygula.
3. **Jittery Camera (no Pixel-Perfect Snap)** — Float pozisyon → dikey çizgi titremesi. Çözüm: kamera position render öncesi RoundToInt'e PPU-aligned.
4. **Mixels** — 16+32+64 PPU yan yana = illüzyon kırılır. Çözüm: tek PPU lock + tüm asset compliance zorunlu.
5. **Linear Alpha Fading for Fog** — Düz transparent katman = "kirli cam". Çözüm: exponential opaklık + dithered geçiş maskeleri.

---

## 4. PainterSuite v1.1+ Tooling & Automation Suggestions

LaurethStudio.PainterSuite paketine eklenebilecek otomasyon araçları:

### Auto-DoF Foreground Layer Generator
**İşlev:** Foreground etiketli sprite'ları otomatik algılar, kamera fokus mesafesine göre dinamik Gaussian Blur shader uygular.
**Fayda:** Her sahne için ayrı blur texture üretme zahmeti yok.

### Parallax Profile Editor (Visual Curve)
**İşlev:** Sürükle-bırak grafik arayüzü ile katmanların Z-depth + kamera kayma hız eğrileri (ease-in/out) ayarlama.
**Fayda:** Matematiksel hesaplama yapmadan mükemmel derinlik hissi saniyelerde.

### Dithered Shadow Baker
**İşlev:** Seçilen sprite'ın alt bound verilerini okuyarak otomatik dithered shadow decal üretir + yerleştirir.
**Fayda:** Karakter/nesne yerleştirirken shadow setup süresi → 0.

---

## 5. Actionable Seeds — LaurethStudio Platform

Pazarda fark yaratacak 3 kritik kütüphane/araç:

### Seed 1: LaurethProc — Visual Depth & Parallax Controller (Lib)
- **Kategori:** Depth & Perspective Illusions
- **Efor:** M (1 dev, 3 gün)
- **Diferansiyatör:** Rakipler parallax katmanlarını manuel Transform script ile yönetir. Bu lib **procgen dünyada** katman derinliklerini runtime'da otomatik hesaplar — tasarımcı sadece "sis yoğunluğu" + "mesafe katsayısı" parametrelerini değiştirir; kütüphane tüm arka planları otomatik ölçekler, boyar, hareket hızlarını belirler.

### Seed 2: PainterSuite — Fake-3D Billboarding & Normal Generator (Tool)
- **Kategori:** Volume & Lighting Illusions
- **Efor:** L (1 dev, 1 hafta)
- **Diferansiyatör:** Square Enix HD-2D özel 3D motor araçları kullanır. Bu araç flat 2D sprite'lardan **depth + normal map** çıkarır → Unity 3D URP ışıklandırma sistemiyle eşleştirir. Edge-detection + bevel ile sprite kenarlarına otomatik "pah kırma" → dinamik ışıkta parlar. Piksel sanatçısı normal çizmek zorunda kalmaz.

### Seed 3: LaurethTime — Rhythm & ASMR Feedback Sync (Lib)
- **Kategori:** Time & Rhythm Illusions
- **Efor:** M (1 dev, 4 gün)
- **Diferansiyatör:** Incremental/Cozy oyunlarda vuruş/kırma hissi kritik. Bu kütüphane oyun içi yıkım/tıklama/toplama animasyon karelerini **çalınan ses efektinin tepe frekanslarıyla dinamik hizalar**. Taş kırılırken çıtırtının en tiz anında debris fırlar. Mikro senkronizasyon = premium "game feel".

---

## Related

- [[painter-suite-v1-1-roadmap-seeds]]
- [[painter-suite-plan-v2-locked]]
- [[project-laureth-studio-master-plan]]
- `STAGING/x_posts_research_agy_2026_05_26.md` (precursor — splat shader + iso editor seeds)
