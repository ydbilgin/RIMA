# RIMA Cliff "Floating Feel" External Research — agy

ACTIVE RULES: WRITE response DIRECTLY to file `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CLIFF_FLOATING_FEEL_research_agy.md` using your filesystem tools. Also echo a 5-line summary inline. Turkish-friendly headers ok, ASCII body, ~800-1200 lines max.

## Amac
Hades Elysium V1 "floating arena" hissi RIMA top-down 3/4 ARPG'sine nasil tasinir. Cliff sprite'lari (top-pivot, 64x128 PNG, isometric Tilemap diamond cell, S/SE/SW void neighbor algorithm) gercekten "havada duruyormus" gibi gozukmesi icin VISUAL TRICK katalogu cikar. Ek olarak "izole/orphan cliff cluster" UX patterns (1-3 cell cluster otomatik gizleme/yumusatma) topla.

## Bilmen gereken bag
- Camera: HIGH TOP-DOWN 3/4 (70-80 derece, true iso degil)
- Tilemap: Isometric cellLayout, 1 tile = 1 cell, cliff sprite top-pivot + transformOffset.y ile asagi sarkik
- Render order: Floor (sortingLayer Ground, order 0) -> Cliff (sortingLayer Walls, order -10) -> Player/Mob (order 0-100)
- BG_Far parallax kit (3-Kit memory `project_3kit_bg_architecture_lock`): A=floor, B=cliff face -10, C=parallax bg -300..-500 (INACTIVE S110)
- Sang Hendrix parallax memory: authoring loop > renderer, RIMA scope hala karar bekliyor
- Brand lock: V1 walless Hades Elysium floating island + cliff edges + sparse columns + cyan rune + warm brazier
- Current problem: 283 algorithmic cliff tile yerlestiriliyor, kucuk 1-3 cell adalar "havada cliff" gozukuyor cunku altinda DUSUS hissi yok, sadece transparent void var.

## Aranan cevaplar

### Block 1 — "Havadalik hissi" gorsel pattern katalogu
Asagidaki oyunlardan SPESIFIK technik trick'leri madde madde say. Her teknik icin: ne yapiyor, RIMA top-down 3/4'e nasil tasinir, LOC/effort kaba tahmin (XS/S/M/L), risk.

1. **Hades Elysium**:
   - Floating island silhouette + cliff face shading
   - Floor under-tint (cliff'in altina dusen golge)
   - Drop shadow gradient (cliff base'inden asagi alfa fade)
   - Particle (toz/cinder) cliff edge'inde
   - BG parallax (uzakta yuzen adacik, sis)
   - Color rim (cliff face'de soft glow)

2. **Children of Morta**:
   - Cliff edge top-pivot sprite + alpha falloff
   - Cliff face decor stratification (rock band, root, moss)
   - Multi-layer sandwich (face + base shadow + far parallax)

3. **Hyper Light Drifter**:
   - Flat color band cliff (saturated palette, no gradient)
   - Parallax cloud underlay (cliff "havada" kanitla)
   - Negative space treatment (void = solid dark color, "dipsiz")

4. **Octopath Traveler 2 (HD-2D)**:
   - Multi-layer sprite sandwich (foreground/midground/BG plates)
   - Fog gradient (alttan koyu, ust acik volumetric)
   - Atmospheric tilt-shift blur

5. **Sang Hendrix Realtime Parallax** (Twitter/YouTube serisi):
   - "Ucurum" hissi parallax recipe (kac katman, hangi hizlar, neye paralle hareket)
   - Authoring loop nasil sahnedeki tek "uzaklik" parametresine bagli

### Block 2 — "Izole cliff cluster" gizleme/yumusatma UX
Algoritma seviyesinde "1-3 cell connected component" cliff cluster'larini nasil handle etmeli. Yine SPESIFIK pattern referansi:

- Minimum cluster size filter (es. 4+ cell connected component only)
- Dilate/erode morphology (1-cell adalari erode + 1-cell pocket'lari dilate)
- Boundary smoothing (corner round, A* path fairing)
- Visibility filter (camera frustum + min screen-space size)
- Soft alpha falloff (cluster edge'inde feathering)
- "Outcrop" vs "wall edge" semantik ayrim (kucuk cluster gosterilebilir ama farkli sprite ile, decorative outcrop)

Hangi oyunlar bu pattern'i kullaniyor? Procgen literature reference?

### Block 3 — Drop shadow + parallax depth ucurum trick'i
RIMA'da MEVCUT: GroundBlobShadow.cs (player altinda shadow). Cliff icin benzer "alttan gradient asagi solan" shadow nasil yapilir:
- Sprite layer (asagi acilim gradient PNG)
- URP 2D Light cookie alternative
- Shader gradient (URP Lit 2D vs Unlit 2D)
- Tilemap-driven (cliff cell altina ekstra tilemap)

BG_Far parallax aktivasyon onerisi: 3-Kit memory'de "INACTIVE S110" yaziyor, hangi 1-2 katman bg eklenirse "floating island" hissi 80% gelir?

### Block 4 — Camera angle ve sprite height etkilesimi
HIGH TOP-DOWN 3/4 (70-80 derece) camera angle ile cliff sprite asagi sarkik durusu nasil etkilenmeli:
- Sprite height/aspect (64x128 mu, 64x96 mi, 64x192 mi optimal?)
- transformOffset.y ne kadar olmali? Mevcut bilinmiyor; senin onerin.
- Floor cell genisligi (Grid cellSize) ile cliff sprite ratio
- "Sarkma" gorsel marjini (cliff sprite asagi tasarken yaninda kalan floor cell ne kadar gozukuyor)

## Format
- Block 1, 2, 3, 4 ayri basliklar
- Her teknik icin: tanim, RIMA aplike notu, LOC/effort, risk, oneri (kullan/skip/test)
- En sonda 3-5 satir TOP-3 oneri ozeti
- Inline yanit (dosyaya yazma)
