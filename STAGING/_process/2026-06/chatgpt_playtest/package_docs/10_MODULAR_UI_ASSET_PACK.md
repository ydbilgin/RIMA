# Modüler UI Asset Pack Üretim Rehberi

## Altın kural

Önce bitmiş oyun ekranı polish ile hedef görünüm onaylanır. Sonra o ekranı kuran parçalar çıkarılır. Önce rastgele çerçeve üretip sonra UI uydurmak yasak, çünkü bu yöntem yıllardır insanları gereksiz PNG mezarlıklarına götürüyor.

## Asset kategorileri

### A. Panel chrome
- `panel_large_base_9slice`
- `panel_medium_base_9slice`
- `panel_small_base_9slice`
- `tooltip_base_9slice`
- `modal_overlay_texture`

Base panel sade olmalı. Ağır köşe süsleri ayrı overlay.

### B. Corner/edge overlays
- normal stone corners
- broken variants A/B
- cyan active crack
- amber focus marker
- boss/danger red accent
- horizontal/vertical divider caps + repeatable middle

### C. Buttons
- base idle 9-slice
- hover overlay
- pressed overlay
- disabled overlay
- danger overlay
- focus marker

Metin asset içine gömülmez; TMP yazar.

### D. Tabs/categories
- idle base
- active fill/underline
- hover
- icon holder
- separator

### E. Skill slots
- primary slot base (LMB/RMB)
- secondary slot base
- ultimate slot base
- passive slot base
- rarity ring overlays
- cooldown radial mask
- locked/unlocked ulti overlay
- selected/hover
- keycap badge

### F. Vitality/resource
- frame 9-slice
- HP fill mask
- delayed-damage fill
- class resource fill mask
- perfect-condition pulse
- low-health crack/vignette
- class emblem socket

### G. Reward cards
- card base 9-slice
- header socket
- rarity strip overlays
- selected glow
- icon frame
- combo/synergy box 9-slice
- select button
- reroll/footer bar

### H. Minimap
- frame 9-slice
- node current/visited/unknown/combat/elite/boss/shop/reward
- connector horizontal/vertical/diagonal
- fog mask
- player arrow

### I. Settings controls
- toggle frame/fill/handle
- slider track/fill/handle
- dropdown frame/arrow/list
- checkbox states
- scrollbar track/thumb
- keybind box and listening state

### J. World reward UI
- interaction prompt panel
- rarity beam
- ground ring/decal
- pickup title plate
- proximity outline/glow
- confirm/cancel prompt

## Önerilen native boyutlar

- Büyük panel base: 256×256 veya 384×384, 32–48px border
- Küçük panel: 128×128, 20–28px border
- Button source: 256×64 veya 320×72
- Skill slot: 96×96 source; 64–92 runtime
- Reward card base: 384×576 source
- Minimap frame: 256×256
- Icons: 64×64 veya 96×96; aynı set içinde tek standart

Bu değerler başlangıçtır; köşe motifinin gerçek kalınlığına göre Sprite Editor border'ı ayarla.

## ChatGPT Image / PixelLab üretim akışı

1. Onaylı polish screen'i style reference yap.
2. Her seferinde tek kategori üret: yalnız buttons, yalnız corners vb.
3. Text ve ikon kullanma; temiz boş frames iste.
4. Düz kontrastlı arka plan veya transparency iste.
5. Aseprite/Photoshop'ta parçaları ayır, perspektif eğriliklerini düzelt.
6. Pixel cleanup: anti-aliasing kaldır, çizgi kalınlığını standardize et.
7. RGBA PNG export.
8. Unity: Sprite (2D and UI), Filter Point, Compression None, mipmap off.
9. Sprite Editor: border/slicing.
10. Prefab state testleri ve 1080p/1440p/ultrawide.

## Image prompt şablonu

```text
Create a modular pixel-art UI asset sheet for RIMA, matching the supplied polished gameplay screenshot. Produce only [CATEGORY]. Blackened fractured slate stone, aged dark iron, restrained cyan rift cracks, subtle amber focus accents. Straight repeatable middle edges for 9-slice use, separate corner decorations, no text, no icons, no perspective tilt, no mockup screen, clean spacing, transparent background, crisp nearest-neighbor pixel edges, consistent border thickness.
```

## Atlas stratejisi

```text
UI_RIMA/
  Chrome_Panels_1024.png
  Chrome_Corners_Dividers_1024.png
  Controls_Buttons_Tabs_1024.png
  Controls_Inputs_1024.png
  HUD_Bars_Slots_1024.png
  Reward_Cards_1024.png
  Minimap_Icons_512.png
  FX_Cyan_Amber_1024.png
```

Chrome ve FX aynı atlas olmamalı. FX shader/blend ister; panel Point filter ister.
