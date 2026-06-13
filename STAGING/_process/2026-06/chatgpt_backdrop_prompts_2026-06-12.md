# ChatGPT Backdrop Prompts — Shattered Keep FAR Layer (10 varyasyon)

**Amaç:** Arena tile'ının ARKASINDA duracak opaque FAR backdrop. ChatGPT'de 10'unu ayrı ayrı üret, en iyisini seç.
**Sonraki adım:** Seçileni 680×384'e indir → `/pixelify` (init image, AI Freedom 0) → PPU 32 import.

## ⛔ HEPSİNDE ortak kural (her promptun sonunda zaten var)
- Ortası ve alt yarısı **DÜZ BOŞ KARANLIK** — zemin yok, taban yok, platform yok, çukur/delik yok.
- Silüetler **SADECE üst ve yan kenarlarda**, küçük ve düşük kontrast.
- **16:9 landscape, pixel-art style.**
- Palet: slate `#3A3D42` · void mor `#3A1A4A` · cyan accent `#2BD9D9` · (ember `#E89020` sadece #7'de).
- Boyut: 16:9 üret (ideal **1360×768** = 680×384'ün 2 katı, temiz downscale).

## ✅ Seçerken bak
Ortası gerçekten boş mu? · Platform/zemin SIZMAMIŞ mı? · Silüetler sadece kenarda mı? · Cyan band okunuyor mu? · Yeterince karanlık mı (gri değil)?

---

## 1 — Classic Void
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void — deep purple-black darkness fills most of the frame. Along the far top and the left and right edges only, small distant silhouettes of broken ruined castle towers and bare dead trees, very dark and low-contrast. A single thin glowing cold-cyan crack of light runs horizontally across the upper portion, like a rift tearing the sky. The entire center and lower half is flat empty darkness — no ground, no floor, no platform, no pit, no holes — just void. Matte, low detail, atmospheric, somber, very low contrast. Muted slate grey #3A3D42 and deep void purple #3A1A4A palette with a faint cyan #2BD9D9 glow accent.
Do NOT include: central platform, stone floor, arena, any holes or pits in the ground, characters, UI, text.
```

## 2 — Jagged Rift
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void of deep purple-black darkness. Across the upper third, a jagged branching crack of glowing cold-cyan light tears through the dark sky like forked lightning frozen in place, with faint cyan bloom around it. Along the top and side edges only, small distant silhouettes of shattered castle ruins, very dark and low-contrast. The center and lower half is completely flat empty darkness — no ground, no floor, no platform, no pit. Matte, low detail, dramatic but somber, very low contrast. Muted slate grey #3A3D42 and deep void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```

## 3 — Misty Depths
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. Faint translucent purple-grey mist drifts low across the bottom edge, with distant broken castle towers and dead trees half-dissolving into the haze along the top and side edges only. A thin horizontal cold-cyan glow near the top, soft and diffuse. The center is flat empty darkness — no ground, no floor, no platform, no pit, just void fading into mist at the very bottom. Matte, low detail, eerie and atmospheric, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```

## 4 — Starfield Void
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast cosmic emptiness of deep purple-black darkness, scattered with tiny faint cold stars and dust specks like a dim night sky. Along the top and side edges only, small distant silhouettes of broken castle spires reaching up into the void. A thin horizontal cold-cyan rift of light near the top. The center and lower half is flat empty starlit darkness — no ground, no floor, no platform, no pit. Matte, low detail, vast and lonely, very low contrast. Void purple #3A1A4A and slate grey #3A3D42 palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```

## 5 — Distant Mountains
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. Along the top and side edges only, jagged broken mountain ridges and crumbling spires form a low dark silhouette band, very dark and low-contrast. A thin glowing cold-cyan crack of light runs horizontally across the upper portion. The center and lower half is flat empty purple-black darkness — no ground, no floor, no platform, no pit. Matte, low detail, bleak and atmospheric, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```

## 6 — Floating Ruins
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. Small fragments of floating broken rock islands and shattered stone bridge pieces drift in the darkness near the top and side edges only, very dark and low-contrast, suspended in emptiness. A thin horizontal cold-cyan rift glow near the top. The center and lower half is flat empty void — no ground, no floor, no continuous platform, no pit. Matte, low detail, surreal and somber, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, solid stone floor, arena, holes or pits, characters, UI, text.
```

## 7 — Ember Dusk (dual accent)
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. A faint warm ember-orange #E89020 glow bleeds along the very bottom edges like distant dying fires, while a thin cold-cyan #2BD9D9 rift of light cuts across the top. Along the top and side edges only, small distant broken castle silhouettes, very dark. The center is flat empty darkness — no ground, no floor, no platform, no pit. Matte, low detail, moody dusk atmosphere, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette with both cyan and faint ember accents.
Do NOT include: central platform, stone floor, arena, holes or pits, bright fire, characters, UI, text.
```

## 8 — Storm Seam
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. Across the upper portion, a wide glowing cold-cyan seam of light tears the sky with a soft bloom, surrounded by faint wispy dark storm clouds. Along the top and side edges only, small distant broken castle towers in silhouette, very dark. The center and lower half is flat empty purple-black darkness — no ground, no floor, no platform, no pit. Matte, low detail, ominous and atmospheric, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```

## 9 — Minimal Abyss (en subtle)
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. An almost pure dark gradient — deep purple-black void with barely any detail. A few extremely faint, tiny distant silhouettes of broken towers near the top corners only, nearly invisible. A single quiet thin cold-cyan line of light near the top. The entire center and lower two-thirds is flat empty darkness — no ground, no floor, no platform, no pit, nothing. Extremely minimal, matte, very dark, very low contrast, calm and empty. Void purple #3A1A4A and slate grey #3A3D42 palette, faint cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, busy detail, characters, UI, text.
```

## 10 — Shattered Sky
```
A dark fantasy game background backdrop in pixel-art style, 16:9 landscape. A vast empty near-black void. Across the top third, several thin glowing cold-cyan cracks fracture the dark sky like broken glass, with small floating debris motes drifting around them. Along the side edges only, small distant broken castle silhouettes, very dark. The center and lower half is flat empty purple-black darkness — no ground, no floor, no platform, no pit. Matte, low detail, fractured and eerie, very low contrast. Slate grey #3A3D42 and void purple #3A1A4A palette, cyan #2BD9D9 accent.
Do NOT include: central platform, stone floor, arena, holes or pits, characters, UI, text.
```
