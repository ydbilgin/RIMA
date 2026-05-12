# RIMA Asset Production Plan — S59 (2026-05-12)

**LOCKED Spec (S59 + Cinderia Pin 2026-05-12):**
- Karakter sprite: **64x64 chibi**, silahlı 1-piece (sınıf-silah sabit)
- Mob sprite: 64x64 (küçük mob), 128x128 (büyük/elite mob)
- Mob boyut hiyerarşisi (2^n LOCKED): Küçük/orta mob 64x64, Elite mob 128x128, Miniboss 128x128, Act Boss 256x256, **Final Boss 256x256 PPU=64** (eski PPU=32 REVOKE)
- **Tüm sprite PPU=64 standardize** — boyut farkı sprite canvas ile gelir
- Renderer: URP 2D + Pixel Perfect Camera
- **Visual identity: Cinderia-inspired (TOP 1 pin)** — High Top-Down ~35° Hades simulation, chibi-but-detailed, dark fantasy environment + neon skill VFX. Birebir Cinderia kalitesi HEDEF DEGIL (64px sweet spot, manuel iş minimum). Stil tanımı: "Cinderia inspired" yeterli.
- **Manuel cleanup pass (basit, ~5-10 dk per sprite):** silhouette outline kontrol + transparent BG doğrulama + 1-2 piksel hata düzeltme. Overpaint/redraw YOK.
- Background: Transparent ON
- View: High top-down 30-35° (Hades match)

**Tool seçimi:**
- 4 sınıf south anchor + 6 mob test → **PixelLab `create_character` MCP veya Create Image Pro Web App**
- 8 yön sprite sheet → **PixelLab Web App "Create from Reference v3"** (south referans verilerek 8 yön otomatik)
- Animasyon → **PixelLab Web App "Custom Animation V3"** (8 frame max, 64x64 canvas budget 524.288)
- **YASAK:** `animate_character` MCP (4-frame limit + VFX bug + run cycle bozuk)

---

## Phase 1 — 4 Sınıf South Anchor (Silahlı 1-piece)

**Pipeline:** Create Image Pro (Web App) veya `create_character` MCP (single direction=south, transparent BG, 64x64 canvas).

**Prompt Template (TYPE/HEAD/BODY/LIMBS format, Image #2'deki Ranger örneği):**

### 1. WARBLADE (south)

```
TYPE: humanoid 64x64 chibi pixel art warrior
STYLE: very small 16-bit RPG sprite, RIMA dark fantasy, clean pixel clusters
HEAD: medium hair, fierce eyes, stoic jaw
BODY: heavy build, broad shoulders, upright stance
LIMBS: muscular arms gripping greatsword with both hands, planted legs
WEAPON: greatsword pointing down at center, blade visible, hilt at chest level
CLOTHING: heavy plate armor, dark steel pauldrons, deep red surcoat, iron boots
HANDS: both hands gripping greatsword hilt firmly, knuckles forward
SILHOUETTE: tall vertical with sword down, broad shoulder plates, recognizable knight profile
COLOR: dark steel grey, deep red accent, brown leather, 2-3 shade steps
POSE: neutral idle, weight evenly on both feet, weapon held vertical center

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, no anti-alias, no noise, no embedded glow, no gradients, no painterly
VIEW: 35° high top-down 3/4 perspective (Hades-style, Into Samomor kamera referansi)
STYLE: RIMA kendi tarzi — 16-bit chunky pixel art, hard pixel edges, clean clusters, asymmetric distinct silhouette, Fractured Epic ton (dark fantasy + canli renkler)
PALETTE: desaturated env cool tones (Into Samomor: stone grey #4A4A4A, dark navy #2A2E35, faded forest #1A2B1A) + saturated neon accents on weapons/skills (RIMA Rift Cyan #00FFCC imza, amber #FFB000, neon yellow #FFF000)
VFX RULE: NO embedded glow/gradient/anti-aliasing on sprite — engine-side only (Unity URP 2D Bloom + Particle + 2D Lights + Vignette)
```

### 2. RANGER (south)

```
TYPE: humanoid 64x64 chibi pixel art ranger
STYLE: very small 16-bit RPG sprite, RIMA dark fantasy
HEAD: hooded cloak hides face, sharp eyes barely visible, defined jaw
BODY: lean athletic, upright relaxed stance
LIMBS: bow held diagonally in left hand, right hand near quiver string
WEAPON: short composite bow, dark wood with silver tip, quiver behind right shoulder
CLOTHING: leather armor, hooded cloak (deep forest green), bracers, belt with pouches, tall brown boots
EXTRA: shoulder mantle, arrow quiver
HANDS: left hand grips bow grip, right hand at draw-ready
SILHOUETTE: hood peak, shoulder mantle, bow diagonal, tall boots
COLOR: earthy greens, dark browns, leather tan, 2-3 shade steps
POSE: neutral idle, weight on left leg, slight forward lean

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, no anti-alias, no noise, no embedded glow, no gradients, no painterly
VIEW: 35° high top-down 3/4 perspective (Hades-style, Into Samomor kamera referansi)
STYLE: RIMA kendi tarzi — 16-bit chunky pixel art, hard pixel edges, clean clusters, asymmetric distinct silhouette, Fractured Epic ton (dark fantasy + canli renkler)
PALETTE: desaturated env cool tones (Into Samomor: stone grey #4A4A4A, dark navy #2A2E35, faded forest #1A2B1A) + saturated neon accents on weapons/skills (RIMA Rift Cyan #00FFCC imza, amber #FFB000, neon yellow #FFF000)
VFX RULE: NO embedded glow/gradient/anti-aliasing on sprite — engine-side only (Unity URP 2D Bloom + Particle + 2D Lights + Vignette)
```

### 3. SHADOWBLADE (south)

```
TYPE: humanoid 64x64 chibi pixel art assassin
STYLE: very small 16-bit RPG sprite, RIMA dark fantasy, stealth aesthetic
HEAD: dark bandana covering lower face, sharp narrow eyes, hood hiding hair
BODY: lean compact, slightly crouched posture, ready stance
LIMBS: both arms angled with daggers in reverse grip (blade pointing down behind wrist)
WEAPON: dual daggers reverse-grip, dark steel blades, leather-wrapped hilts
CLOTHING: dark leather armor, bandana, tight pants, soft-soled boots, belt with throwing knives
HANDS: reverse grip on both daggers, knuckles outward
SILHOUETTE: bandana, hood, two dagger blades pointing down, crouched profile
COLOR: deep black-purple, dark grey, cold blue accent, 2-3 shade steps
POSE: low ready stance, weight slightly forward, daggers reverse-held at hip

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, no anti-alias, no noise, no embedded glow, no gradients, no painterly
VIEW: 35° high top-down 3/4 perspective (Hades-style, Into Samomor kamera referansi)
STYLE: RIMA kendi tarzi — 16-bit chunky pixel art, hard pixel edges, clean clusters, asymmetric distinct silhouette, Fractured Epic ton (dark fantasy + canli renkler)
PALETTE: desaturated env cool tones (Into Samomor: stone grey #4A4A4A, dark navy #2A2E35, faded forest #1A2B1A) + saturated neon accents on weapons/skills (RIMA Rift Cyan #00FFCC imza, amber #FFB000, neon yellow #FFF000)
VFX RULE: NO embedded glow/gradient/anti-aliasing on sprite — engine-side only (Unity URP 2D Bloom + Particle + 2D Lights + Vignette)
```

### 4. ELEMENTALIST (south)

```
TYPE: humanoid 64x64 chibi pixel art mage
STYLE: very small 16-bit RPG sprite, RIMA dark fantasy, mystical
HEAD: hooded robe partially shadows face, calm focused eyes, beardless or thin beard
BODY: slim, upright graceful posture
LIMBS: long staff held diagonal in right hand, left hand at side or chest
WEAPON: tall wooden staff with crystal/gem at top, runes carved along shaft
CLOTHING: long flowing robe (deep blue or purple), wide sleeves, hood, sash belt, simple sandals/cloth wraps
HANDS: right hand grips staff firmly, left hand relaxed (no orb — REMOVED per S57 decision)
SILHOUETTE: tall staff, flowing robe, hood peak, slim profile
COLOR: deep blue/purple robe, cream sash, dark wood staff, glowing crystal (cyan/violet), 2-3 shade steps
POSE: serene neutral idle, weight on both feet, staff at slight diagonal

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, no anti-alias, no noise, no embedded glow, no gradients, no painterly
VIEW: 35° high top-down 3/4 perspective (Hades-style, Into Samomor kamera referansi)
STYLE: RIMA kendi tarzi — 16-bit chunky pixel art, hard pixel edges, clean clusters, asymmetric distinct silhouette, Fractured Epic ton (dark fantasy + canli renkler)
PALETTE: desaturated env cool tones (Into Samomor: stone grey #4A4A4A, dark navy #2A2E35, faded forest #1A2B1A) + saturated neon accents on weapons/skills (RIMA Rift Cyan #00FFCC imza, amber #FFB000, neon yellow #FFF000)
VFX RULE: NO embedded glow/gradient/anti-aliasing on sprite — engine-side only (Unity URP 2D Bloom + Particle + 2D Lights + Vignette)
```

---

## Phase 2 — 6 Mob 64px (Boyut Testi)

**Amaç:** 64px boyutunda mob silüeti okunabilir mi? Karakter (64px) ile boyut farkı yeterli mi? Eğer bazıları için 128px daha uygunsa, mob hiyerarşi düzenlenir.

### Mob 1 — Goblin Warrior

```
TYPE: tiny humanoid 64x64 chibi pixel art
STYLE: very small 16-bit RPG monster sprite, RIMA dark fantasy
HEAD: oversized green head, heavy brow, beady dark eyes, small lower tusks, dark messy hair tufts
BODY: short compact body, slight hunch
LIMBS: tiny arms holding rusty short sword, short bandy legs
WEAPON: rusty short sword in right hand
CLOTHING: ragged brown tunic, dark belt, short ragged pants, tiny dark boots
HANDS: right grips sword, left empty
SILHOUETTE: oversized green head, heavy brow, small tusks, messy hair, tiny body, sword right
COLOR: green skin, dark brown clothing, rusty steel, 2-3 shade steps
POSE: aggressive ready, weight forward, sword raised slightly

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, no anti-alias, view: high top-down 30°
```

### Mob 2 — Skeleton Warrior

```
TYPE: humanoid 64x64 chibi pixel art skeleton
STYLE: very small 16-bit RPG undead sprite, RIMA dark fantasy
HEAD: bone skull, empty eye sockets glowing faint blue, exposed jaw
BODY: bone frame, exposed ribs, hunched upright
LIMBS: skeletal arms holding broken sword and shield
WEAPON: chipped longsword right hand, broken round shield left hand
CLOTHING: tattered rusted chainmail rags, scraps over bones, ragged loincloth
HANDS: bone fingers gripping weapons
SILHOUETTE: skull, ribs visible, sword + shield silhouette
COLOR: bone white, rusty steel, faded grey rags, faint blue eye glow, 2-3 shade steps
POSE: rigid undead stance, weight even, weapons at ready

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, view: high top-down 30°
```

### Mob 3 — Slime

```
TYPE: amorphous creature 64x64 chibi pixel art
STYLE: very small 16-bit RPG slime sprite, RIMA dark fantasy
HEAD: no separate head, blob top
BODY: gelatinous green-blue blob with translucent appearance, small bubbles inside
LIMBS: none, just blob base
EXTRA: two small black dot eyes, no mouth
WEAPON: none
SILHOUETTE: blob teardrop, two eye dots
COLOR: translucent green-blue, darker green core, white highlight, 2-3 shade steps
POSE: settled blob, slight wiggle

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, view: high top-down 30°
```

### Mob 4 — Wraith

```
TYPE: ethereal humanoid 64x64 chibi pixel art ghost
STYLE: very small 16-bit RPG wraith sprite, RIMA dark fantasy
HEAD: hooded skull face, glowing white eye sockets, no jaw (mist trails)
BODY: tattered cloak floating, no legs (mist tail)
LIMBS: skeletal claw hands extended, no legs
EXTRA: floating, mist trail at base, transparent edges
WEAPON: ethereal claws
HANDS: bony claws outstretched
SILHOUETTE: hood peak, glowing eyes, mist tail, claw hands
COLOR: dark grey-blue cloak, ghostly white-cyan glow, transparent edges, 2-3 shade steps
POSE: floating menacing, claws forward

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, view: high top-down 30°
```

### Mob 5 — Mushroom Monster

```
TYPE: fungal creature 64x64 chibi pixel art
STYLE: very small 16-bit RPG creature sprite, RIMA dark fantasy
HEAD: large red mushroom cap with white spots, eyes underneath cap
BODY: short white stalk body, no neck
LIMBS: two short stubby arms, two stumpy legs
EXTRA: glowing spore particles around base
WEAPON: none (will headbutt)
HANDS: tiny stub hands
SILHOUETTE: oversized red cap, short white stalk body, four stubs
COLOR: red cap with white spots, off-white stalk, dark mossy green base, spore yellow glow, 2-3 shade steps
POSE: bouncing ready, slight cap tilt

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, view: high top-down 30°
```

### Mob 6 — Giant Rat

```
TYPE: small quadruped 64x64 chibi pixel art
STYLE: very small 16-bit RPG vermin sprite, RIMA dark fantasy
HEAD: pointed snout, beady red eyes, large notched ears, prominent yellow fangs
BODY: long crouched body, scruffy fur, hunched back
LIMBS: four short clawed legs, long pink tail trailing
EXTRA: matted fur, slight slobber
WEAPON: teeth and claws (no weapon)
HANDS: paws with claws
SILHOUETTE: pointed snout, ears, hunched back, long tail, four legs
COLOR: dirty brown fur, dark grey shading, pink tail, yellow fangs, red eyes, 2-3 shade steps
POSE: low predator ready, weight forward, tail trailing

LAYOUT: single south-facing frame, 64x64 canvas, transparent BG
RULES: pixel art, no dithering, view: high top-down 30°
```

---

## Phase 3 — Pilot Karakter (1 sınıf) → 8 Yön + Animasyon

**Pilot seçimi:** 4 south anchor üretildikten sonra, en başarılı olan veya kullanıcı tercihi.

**Pipeline:**
1. **Create from Reference v3 (Web App)** — south anchor referans, 8 yön otomatik üretim. Settings: View=High top-down, Directions=8 (N/NE/E/SE/S/SW/W/NW), Canvas=64x64
2. **Custom Animation V3 (Web App)** — per direction animasyon
   - Idle: 6 frame, subtle breathing, looping
   - Run: 6 frame, mid-stride (Karar #42: Walk YOK, sadece Run; idle = interpolate first+last frame)
   - Attack LMB: 8 frame (4+4 segment)
   - Hurt: 4 frame
   - Death: 6 frame
   - Dash: 4 frame
3. **Frame budget:** 64x64 → max 128 frame teorik; pratikte 8 frame per anim güvenli
4. **Skill VFX:** Ayrı 64-128px sprite (karakter spriteinden bağımsız), `create_object` MCP veya manuel

---

## Karar Notları

- **Map tools KULLANILMAYACAK** (NLM LOCKED 2026-05-11, S59 KEEP) — PixelLab map güvenilir değil, Unity Tilemap + Wang autotile zorunlu
- **`animate_character` MCP YASAK** (LOCKED 2026-05-02, S59 KEEP) — Web App Custom Animation V3 zorunlu
- **64px chibi LOCKED** (S59 2026-05-12) — eski "64px native YASAK" S58 kararı REVOKED
- **Silahlı 1-piece LOCKED** (S59 2026-05-12) — body-only + WeaponAnchorMap sistemi REVOKED

## Çıktı Lokasyonları

- `PIXELLAB_OUTPUTS/warblade/south_64.png`
- `PIXELLAB_OUTPUTS/ranger/south_64.png`
- `PIXELLAB_OUTPUTS/shadowblade/south_64.png`
- `PIXELLAB_OUTPUTS/elementalist/south_64.png`
- `PIXELLAB_OUTPUTS/mobs/goblin_64.png`
- `PIXELLAB_OUTPUTS/mobs/skeleton_64.png`
- `PIXELLAB_OUTPUTS/mobs/slime_64.png`
- `PIXELLAB_OUTPUTS/mobs/wraith_64.png`
- `PIXELLAB_OUTPUTS/mobs/mushroom_64.png`
- `PIXELLAB_OUTPUTS/mobs/rat_64.png`

## Sonraki Adımlar

1. Kullanıcı PixelLab Web App'te (veya MCP ile) bu 10 sprite'ı üretsin
2. Boyut testi: 64px mob okunabilirliği → bazıları 128px'e taşınır mı?
3. Pilot sınıf seç → 8 yön + animasyon
4. Unity `_Sandbox.unity`'de test
