# Natural-Looking 2D Top-Down Map Techniques Research
Tarih: 2026-05-13
Reference games: HLD / Hades / Loop Hero / Death's Door / Tunic / S&S / Hammerwatch / Alabaster Dawn

---

## A. Compositional Tricks (her tile aynı olmasın)

- **Density Gradient Zoning** — Zemin dokusunu tek tip yaymak yerine "yogun doku bölgesi + nefes alan duz bölge" karisimlari yarat. Slynyrd Pixelblog 20: "mix spaces of flat green with textured areas of differing vegetation density." Sahnede organik dolumluk hissi verir. Unity'de: Tilemap Paint modu ile dolu variant tile'lari dolu bölgelere, sade variant'lari geçis bölgelerine el ile uygula; RuleTile'a ek olarak bir "Density Decal" tilemap layer'i ayri tut.

- **3-5 Floor Variant + Random Tile** — Ayni arazi için en az 3 görsel variant çiz. Unity'de `RandomTile` (2D Tilemap Extras) kullanarak her paint isleminde rastgele variant seç. Seferde sadece renk/leke konumu farkli tile'lar yeterlidir; silüet degerini koruyan mikro-varyasyonlar (3-4 px offset patch) yeterlidir. HLD bu prensibi tüm zeminlerinde kullanir.

- **Cluster Discipline** — Doku klusterlarinin birbirine degmesinden kaçin; degdiğinde "gürültü blogu" olusur. Köse-temas kabul edilebilir, kenara-temas degil. Slynyrd'in terimi: "ramen noodle effect" (birbirine girift, okunamayan kümeler). Unity'de: Brush tool ile kontrollü paint, dogal bosgap birak.

- **Irregular Pattern Rhythm** — Her 7-9 tile'da bir güçlü görsel aksan (çatlak, leke, çiçek, kir yigini). Matematiksel düzgün aralik (her 4 tile gibi) degil, algisal olarak "random ama dengeli." Unity'de: Scatter sprite sistemi (Karar #116) ile Perlin noise yogunluk haritasi üretip aksanlari dagit.

- **Negative Space** — Yogun doku bölgelerinin arasinda kasitli duz alan birak. Bu, gözün dinlenmesini saglar ve sonraki detayin agirligini arttirir. Loop Hero'da koy/zemin tile'lari arasindaki bos alanlar bu ilkeyi kullanir.

---

## B. Visual Layering (depth / lighting)

- **Multi-Layer Compositing** — Minimum 4 ayri Tilemap layer: (1) Base Floor, (2) Floor Decals, (3) Wall/Elevated, (4) Prop/Scatter. Her layer'in kendi Sort Order'i var. Unity'de her Tilemap'e ayri Sorting Layer ata; "Tiles" → "Decals" → "Walls" → "Props" siralamasi.

- **Soft Lights as Depth** — HLD imzali teknik: geleneksel renk-sinirli pixel art üstüne URP 2D soft point light'lar katla. Blending modu Additive, hafif opacity, zemin üzerinde sicak spot. Derin zemin karanligi + aydinlik geçis bölgesi organik derinlik verir. Unity'de: URP 2D Point Light (Freeform mümkün), Normal Map kullan + `Sprite-Lit-Default` shader.

- **Drop Shadow on Props** — Her prop/karakter altinda oval SpriteRenderer (beyaz sprite, scale ayarli, alpha ~0.3, Sorting Layer'i prop altinda). Zemine "basinç" hissi verir. Karar #116g hali hazirda onayladi; tüm scatter sprite'lara da ekle.

- **Wall Top Face Highlight** — Duvar üst yüzüne 1-2 piksel açik renk highlight ekleme, taban gölge ile birlikte fake isometric derinlik hissi yaratir. Slynyrd Pixelblog 43: "drop shadows limited to 1-2 wall faces, consistent shadow length." Unity'de sprite asset üzerinde bake edilir, kod gerektirmez.

- **Parallax Background Layer** — Camera hareketiyle farkli hizda kayan çok uzak zemin katmani (tas doku, kaya silueti). 2D roguelite'lerde genellikle sadece bir parallax layer yeterlidir. Unity'de Camera orthoSize * parallaxFactor ile lerp.

---

## C. Per-Tile Variation (free variety multiplier)

- **RuleTile Random Output** — Unity 2D Tilemap Extras `RuleTile` > Output: Random. Ayni kural setini birden fazla sprite ile eslestir; her placement'ta farkli sprite çeker. Kullanim: zemin, duvara-uzak bölge, köse tile'lari. Resmi dokümantasyon: docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@6.0/manual/RuleTile.html

- **Transform: Mirror/Rotate** — RuleTile'da her kural icin Transform ayarini "Mirror X" veya "Rotated" olarak set et. 1 sprite'tan 4 görsel varyasyon ücretsiz. Dikkat: pixel art'ta dikey-simetrik tile'larda mirror kullanilabilir ama karakter/siluet içerenlerde el kontrolü sart (Karar #99 referansi: weapon-hand flip hatasi).

- **Per-Instance Color Tint** — Scatter sprite'lara ve dekoratif prop'lara SpriteRenderer.color üzerinden hafif warm/cool shift (+/-10 HSV). Unity'de `GetComponent<SpriteRenderer>().color = new Color(r,g,b)`. Palette shader kullanarak (tante.hashnode.dev renk palette shader rehberi) 1 spritesheet'ten 3-4 renk varyasyonu üretilebilir. Çok güçlü: Moss için +yeşilimsi tint, Rubble için +soguk gri.

- **Scale Jitter** — Scatter sprite'lara küçük rastgele scale offset (0.85x - 1.15x). Hizada olmayan boyutlar grid hissini kirar. Unity'de `transform.localScale *= Random.Range(0.85f, 1.15f)` scatter spawn aninda.

- **Z-Rotation Microvariation** — Ufak açi kaymalari (+-5 derece) tag taslarinda, kirik kivrimciklarinda vs. Animasyonu olmayan statik dekorasyon için ideal. HLD çevre asset'lerinde görülür.

---

## D. Ambient Detail (particle / light / animated)

- **Dust Mote Particles** — Küçük, yavaş, yukarı-aşağı sürüklenen toz parçacikciklari. Pixel art için particle'lari 2x2 veya 4x4 sprite sprite sheet ile çal, Particle System "Billboard" modu, çok düsük emission rate (0.5-2/sn), genis alanda. Unity Particle System + Pixel Art Particles Pack (Asset Store #80249). HLD ve Hades'de koridorlarda kullanilmis.

- **God Rays (Isik Huzmesi)** — Hafif transparan isik dilimi SpriteRenderer'lar tilted 15-30 derece, çok hafif Additive blend, Animator'de yavaş alpha pulse. Tek kod gerekmez: animator + sprite. Hyper Light Drifter gibi atmosfer yaratiyor; HLD'nin shader tabanlı versiyon yerine sprite-based versiyonu daha kolay uygulanir.

- **Animated Tiles (Water/Lava/Rift)** — Unity 2D Tilemap Extras `AnimatedTile` ile 2-4 frame döngü. Kisa döngü (6-10 frame @ 8fps) yeterli canlilik saglar. Frame timing kritik: çok hizli = panik, çok yavas = statik görünüm. Su için 3-4 frame ripple, Lava icin 4-6 frame kabarcik.

- **Ambient Light Flicker** — Mum/alev yaninda point light'a çok küçük intensity titreme (sinusoidal + küçük perlin noise). Code: `light.intensity = baseIntensity + Mathf.Sin(Time.time * freq) * 0.1f`. HLD alev detaylarinda görülür.

- **Foreground Veil Particles** — Kameraya çok yakin, çok büyük, çok transparan toz/duman sprite'lar. Derinlik yanilsamasi yaratir. Parallax 0 ile sabitlenir, sadece scale büyüktür. Hades ve Death's Door'da yoğun kullanim.

---

## E. Edge / Transition Polish

- **Vegetation Tufts at Wall Base** — Duvar dibi scatter pozisyonuna ince çimen/yosun demetleri. Floor-Wall gecis noktasinda standart bir 4-6px yüksekliğinde ot/yosun sprite. Wall RuleTile'in "base edge" degiskenine elle boyali veya sprite-based eklenir. Slynyrd rehberi: "strategic edge styling (dry, cracked, or soft) influences blending quality."

- **Splatter Tile Technique** — Tile sinirinin disina tasarak organik kenar olusturma. Gamedeveloper.com makalesi (SPLATTERTILES): her arazi için bir base tile, kenarlar düzensiz (tas için köşeli, çamur için kıvrımlı, çimen için sivri). Komsu tile'lara taşan "organic spill" grid görünümünü kirpar. Unity'de: ek bir "splat" tilemap layer, Sort Order floor üstünde, tile'lar grid hücresinden büyük spritelar.

- **Hand-Painted Accent Decals at Intersections** — Floor-Wall-Corner kesisme noktalarinda tek seferlik hand-paint decal sprite'lar. 8-12px boyutunda, zemin üstünde Sorting Layer. Hades environment artist Joanne Tran'ın parlak çalismalarinda görülür: intersectionlarda küçük kir lekesi/çatir yigi.

- **Wang Raggedness Tuning** — Wang autotile'in transition kenarlarini dogallastiran gürültü miktari (Karar #116, %40+ raggedness). Slynyrd'in "irregular patterns break artificial-looking regularity" prensibiyle uyumlu. Daha yüksek raggedness = daha organik ama tutarsizlik riskli; %40-55 Alabaster Dawn referansi.

- **Gradient Shadow Belt** — Duvar dibi zemin üzerinde 2-3 tile genisliginde hafif kararmis overlay sprite (dark semi-transparent strip). Zemin-duvar geçisinde derinlik artirir, extra tile çizmeden. Unity'de: tek bir transparan gradient sprite, wall base'e hizalı.

---

## F. Procedural Tricks (algorithmic)

- **Cellular Automata Cave Shaping** — 4-5 kuralini (bir hücre 4+ komsu duvar hücresine sahipse duvar kalir/olur) birkaç iterasyon uygulayarak organik magara sekilleri üret. RogueBasin dökümantasyonu (roguebasin.com/Cellular_Automata) net implementasyon verir. Sebastian Lague Unity serisi Unity'de tam kaynak kodlu. Karar #116'daki CA generator ile zaten planlandi.

- **Voronoi-Based Crack Placement** — Worley/Voronoi noise ile zemindet crack pozisyonlari üret. Voronoi kenar degerleri ~0 olan pikseller crack merkezlerini isaretler. Unity'de: 2D dokuda Voronoi noise hesapla, belirli esik altindaki hücre kenarlarina crack decal sprite spawn et. Unity Discussions (thread #937392): Voronoi noise shader crack implementasyonu örnegi.

- **Perlin Noise Accent Density Gradient** — Scatter sprite yogunlugunu (Stones/Moss/Rubble/Dirt) Perlin noise haritasiyla modüle et. Yüksek noise degerinde yogun scatter, düsük degerde seyrek. Unity'de: `Mathf.PerlinNoise(x * scale, y * scale) > threshold` ile her hücre için scatter spawni karar al.

- **Priority-Based Terrain Layering** — Farkli arazi tiplerinin çakistigi noktada öncelik sirasi belirle (örn. lava > stone > dirt > grass). Yüksek öncelikli arazi alttekileri örter. dev.to/jhmciberman makalesi binary tileset organizasyon yöntemi ile bu mantigi formalize ediyor.

- **Oval Brush + CA Hybrid** — Elle oval brush ile ana oda sekilleri ciz, sonra CA smoothing uygulayarak kenarlari organik hale getir. Karar #116 bu yaklasimi destekliyor. Kodda: brush mask array + 2-3 CA iteration.

---

## G. Post-Processing (URP 2D)

- **Bloom (Intensity 1-2)** — Parlak alev/kristal/portal sprite'larin dogal parlama etkisi. URP Global Volume > Bloom > Intensity 1.0-1.5, Threshold ayarla. HDR'i Camera rendering'de aktif et. Pixel art icin çok yüksek Bloom bulanislik yaratir; 1.0-1.5 arasi korunmali. HLD'nin glowing portal/efektleri bu sekilde.

- **Vignette** — Ekran köselerini karartarak odagi merkeze çek. URP Global Volume > Vignette > Intensity 0.2-0.35, Smoothness 0.5. Koseler kararmasi roguelite atmosfer için standart. Unity Learn kursu (learn.unity.com/2d-lighting-for-pixel-art) tam setup adimlarini veriyor.

- **Color Grading / White Balance** — Biome'e göre sicak (warm dungeon, turuncu LUT) veya soguk (frost biome, mavi LUT) renk kaymaları. URP Volume > Color Grading > White Balance > Temperature slider. Halis Avakis LUT shader tutorial'i (halisavakis.com) özel LUT üretimi için rehber. Her biome'a farkli Volume Profile atamak biome geçisinde blend edililebilir.

- **LutLight2D (Pixel Art Specific)** — GitHub: NullTale/LutLight2D — pixel art aydınlatma için her rengin kendi shading gradyanini tanimlayan URP 2D renderer feature. Standart URP 2D light ile uyumsuz görünen yanlış renk shade'leri çözer. Pixel art palette'ini koruyarak isik hesaplar.

- **Chromatic Aberration (Çok Hafif)** — Ekran kenarlarinda mikro renk fringi. Intensity 0.05-0.1. Salt and Sanctuary gibi karanlık atmosferde incelikli ama etkileyici. Pixel art'ta fazla dikkat çekici olabilir; cok hafif tut.

---

## H. RIMA-Specific Önerileri (Karar #116 + #118 Üstüne)

Mevcut plan (Wang, 3+ floor variant, multi-layer, scatter, URP lights, drop shadow, CA, oval brush) saglamdır. Asagidaki 7 ekleme düsük maliyetle maksimum dogallik etkisi verir:

1. **Per-Instance Color Tint on Scatter Sprites** — Neden: Moss/Stone/Rubble tum scatter ayni renkte görünüyor; hafif warm/cool tint shift ile gruplanmasi kiriliyor. Maliyet: Scatter spawn scripte 2 satirlik SpriteRenderer.color kodu.

2. **Animated Tiles (Su + Rift)** — Neden: Statik animasyonsuz tile'lar 2026 standartlari altinda kalir; 3-4 frame ripple su tilelerine hayat katar. Maliyet: AnimatedTile asset + 3-4 frame sprite çizimi, kod sifir.

3. **Vegetation Tufts at Wall Base** — Neden: Wall-floor kesimleri sert; duvar dibi ufak moss/grass scatter sprite bu gecisi organiklestirir. Maliyet: 2-3 yeni 8px scatter sprite (PixelLab'dan üretilebilir), scatter spawn scriptine "wall-adjacent" kosu.

4. **Perlin Noise Accent Density Gradient (Scatter System'a Entegre)** — Neden: Mevcut scatter homojen dagiliyor; Perlin modülasyonu ile kümeler olusur, map daha organik hissettiriyor. Maliyet: ScatterBrushWindow'a 10-15 satirlik Perlin kosu (Karar #121 sistemi genisletmesi).

5. **Dust Mote Particle System** — Neden: Tüm referans oyunlarda var (HLD, Hades); atmosfer için en yüksek görsel ROI'ye sahip ambient detay. Maliyet: Tek Particle System prefab, 2x2 px sprite, sahneye dragdrop.

6. **Color Grading per-Biome Volume Profile** — Neden: F1 (dungeon) ile diger biomeler arasinda atmosfer farki saglayan ücretsiz görsel ayrım. Maliyet: Her biome için bir Volume Profile asset + Biome manager'de aktif profile gecisi.

7. **Wall-Base Gradient Shadow Belt** — Neden: Zemin-duvar geçisini derinlestirir, scatter sprite'larla birlikte çok etkileyici sonuç. Maliyet: Tek bir 32x16 transparan gradient sprite, wall base rule'una ek overlay tile.

---

## Kaynaklar

- [Pixelblog 20 - Top Down Tiles (Slynyrd)](https://www.slynyrd.com/blog/2019/8/27/pixelblog-20-top-down-tiles)
- [Pixelblog 43 - Top Down Tiles Part 2 (Slynyrd)](https://www.slynyrd.com/blog/2023/3/26/pixelblog-43-top-down-tiles-part-2)
- [Splattertiles - Gamedeveloper.com](https://www.gamedeveloper.com/art/splattertiles-how-to-tile-your-game-without-all-that-fuss-)
- [Procedural Pixel Art Tilemaps - DEV Community](https://dev.to/jhmciberman/procedural-pixel-art-tilemaps-57e2)
- [Unity Rule Tile Docs - 2D Tilemap Extras 6.0](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@6.0/manual/RuleTile.html)
- [Unity 2D Lighting Learn Course - Post Processing](https://learn.unity.com/course/2d-lighting-for-pixel-art/tutorial/enhance-effects-with-post-processing)
- [How to use 2D lights to set mood - Unity](https://unity.com/how-to/use-2d-lights-unity-set-mood)
- [LutLight2D - Pixel Art Lighting URP (GitHub)](https://github.com/NullTale/LutLight2D)
- [Hyper Light Drifter Art Direction Analysis](http://idrawwearinghats.blogspot.com/2014/04/art-direction-analysis-of-hyper-light.html)
- [Hades Environment Art - Supergiant Games](https://mcvuk.com/business-news/behind-the-art-of-hades-we-value-artistic-integrity-and-excellence-in-artistic-craft-at-supergiant-however-were-first-and-foremost-a-game-design-lead-team/)
- [Voronoi Noise Shader Crack - Unity Discussions](https://discussions.unity.com/t/voronoi-noise-shader-issue-to-make-procedural-cracks/937392)
- [Cellular Automata Cave Generation - RogueBasin](https://www.roguebasin.com/index.php/Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels)
- [Color Palette Shader for Pixel Art - Unity](https://tante.hashnode.dev/creating-a-lot-of-variations-of-your-pixelart-quickly)
- [LUT Color Grading Shader - Harry Alisavakis](https://halisavakis.com/my-take-on-shaders-color-grading-with-look-up-textures-lut/)
- [Pixel Art Tiles Guide 2026 - Sprite-AI](https://www.sprite-ai.art/blog/seamless-pixel-art-tiles)
- [Procedural Cave Generator Unity - GitHub](https://github.com/AK-Saigyouji/Procedural-Cave-Generator)
