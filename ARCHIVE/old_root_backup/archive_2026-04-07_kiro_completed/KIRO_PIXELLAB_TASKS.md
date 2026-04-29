# KIRO — PixelLab Üretim Görev Dosyası
*Bu dosyayı oku, sırayla uygula, başka dosya okuma.*

---

## PIXELLAB API YAPILANDIRMASI

**API Endpoint:** `https://api.pixellab.ai/mcp`  
**Transport:** HTTP  
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

```bash
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```

---

## AGENT TALİMATLARI — ÖNCE OKU

### Hangi endpoint nerede kullanılır

| Görev tipi | Endpoint | Neden |
|---|---|---|
| Oyuncu karakterleri | `create_character` | Humanoid, 8 yön rotation |
| Düşman karakterleri | `create_character` | Humanoid, 8 yön rotation |
| Skill ikonları | `create_map_object` | Standalone obje, transparent bg |
| Zemin / duvar tile | `create_tiles_pro` | tile_type="square_topdown" |
| **create_topdown_tileset KULLANMA** | ❌ | Wang terrain sistemi, yanlış tool |
| **create_tiles_pro tile_type belirtmeden KULLANMA** | ❌ | Default=isometric, diamond shape çıkar |
| **create_isometric_tile KULLANMA** | ❌ | Isometric, yanlış perspektif |

### Adımlar
1. Görevi başlat (`create_character` / `create_map_object` / `create_tiles_pro`)
2. Status kontrol: `get_character` veya `get_map_object` veya `get_tiles_pro` — tamamlandı mı?
3. ZIP veya PNG'yi `curl --fail` ile STAGING'e indir
4. Bir iş bitmeden diğerine geçme

### Nereye kaydet
**STAGING:** `F:/Antigravity Projeler/2d roguelite/STAGING/`

```
STAGING/
├── Characters/Players/Warblade/          → sprites.zip
├── Characters/Players/Elementalist/      → sprites.zip
├── Characters/Players/Shadowblade/       → sprites.zip
├── Characters/Players/Ranger/            → sprites.zip
├── Enemies/Act1/ShardWalker/             → sprites.zip
├── Enemies/Act1/SeamCrawler/             → sprites.zip
├── Enemies/Act1/VoidThrall/              → sprites.zip
├── Enemies/Act1/ChainWarden/             → sprites.zip
├── Enemies/Act1/Penitent/                → sprites.zip
├── Enemies/Act1/RelicCaster/             → sprites.zip
├── Enemies/Act1/FractureImp/             → sprites.zip
├── Enemies/Act1/IronWarden/              → sprites.zip
├── Enemies/Act1/TwiceBorn/Primary/       → sprites.zip
├── Enemies/Act1/TwiceBorn/Secondary/     → sprites.zip
├── Enemies/Act1/FractureKnight/          → sprites.zip
├── Enemies/Act1/Reliquary/               → sprites.zip
├── Enemies/Act2/MireStalker/             → sprites.zip
├── Enemies/Act2/RotPriest/               → sprites.zip
├── Icons/Skills/Warblade/                → 4x PNG
├── Icons/Skills/Elementalist/            → 4x PNG
├── Icons/Skills/Shadowblade/             → 4x PNG
├── Icons/Skills/Ranger/                  → 4x PNG
└── Tiles/Act1/                           → floor_stone.png + wall_stone.png
```

### Hata yönetimi
- 1 fail → aynı parametrelerle 1 kez daha dene
- 2. denemede de fail → `FAILED_{isim}.txt` yaz, devam et
- Animasyon ekleme — sadece karakter/obje üret

### Genel stil kuralı (tüm promptlar için)
- **Rift energy:** blue-purple glowing crack light, her karakterde bir yerde görünür
- **Palette:** koyu lacivert, derin mor, soğuk gri
- **Her prompt sonu:** `dark fantasy roguelite, RIMA universe`
- **Düşman aileleri:**
  - Fractured: parçalı insansı form, gap'lerden rift ışığı sızıyor
  - Rift-Born: zemine yapışık, organik, nemli görünüm
  - Emergent: ağır, köşeli, ezici

---

## KAPSAM NOTU — Bu batch'te ne yok

Act 2'de tasarlanan 5 ek düşman bu batch'e dahil değil (gelecek sprint):
- Thorn Brute (96px, dikenli bruiser)
- Carrion Weaver (80px) + Rotling (24px, spawn enemy)
- Blood Lancer (72px, lifesteal skirmisher)
- Husk Thrower (64px, lobbing ranged)
- Decay Anchor (80px, stationary zone control)
- Devourer / Thornmother / Withered Apostle (Act 2 eliteleri)

Act 3 düşmanları da bu batch'te yok.

---

## BOYUT REFERANSI

| PixelLab size | Oyun rolü |
|---|---|
| 48px | True swarm (3-5+ aynı anda) |
| 64px | Kasıtlı kırılgan / support |
| 80px | Standart grunt / skirmisher |
| 96px | Oyuncu baseline / ciddi tehdit |
| 112px | Ağır / baskılayıcı |
| 128px | Elite / mini-boss |

---

## BÖLÜM 1 — OYUNCU KARAKTERLERİ

> `create_character` | mode: pro | view: low top-down | size: 96 | 8 yön
> Tüm oyuncu karakterleri size=96 — boy tutarlılığı için

---

### 1.1 Warblade
*Ağır savaşçı. Uzun-kaslı, bodur değil. İki el kılıcı, koyu demir zırh.*
```
mcp__pixellab__create_character(
  name="Warblade",
  description="tall imposing warrior with lean muscular build, long legs and broad shoulders but defined waist, NOT stocky NOT short NOT dwarf proportions, upright 6-head-height figure, dark iron armor with open face guard revealing scarred determined face with intense eyes, large two-handed greatsword with blue-purple rift energy crackling along blade edge, short tattered cape behind, dominant combat stance, dark iron and cold steel blue palette, rift energy blue-purple crack lines glowing at armor joints and sword edge, dark fantasy roguelite, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/sprites.zip`

---

### 1.2 Elementalist
*Büyücü. Amber göz, iki elden ateş+buz enerjisi ayrı ayrı çıkıyor, staff.*
```
mcp__pixellab__create_character(
  name="Elementalist",
  description="arcane mage with hood pulled back revealing intense face with glowing amber eyes, flowing dark robes, amber fire energy swirling from one hand and ice-blue frost energy from the other hand showing dual elemental mastery, staff with split amber-frost crystal at top, upright casting stance, rift energy blue-purple crack lines on robes and staff, deep purple dark robe base with amber and ice-blue energy accents, dark fantasy roguelite, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/sprites.zip`

---

### 1.3 Shadowblade
*Assassin. Alt yüz sarılı ama gözler parlıyor. Koyu indigo — tamamen siyah değil.*

> ⚠ Önceki üretimde tamamen siyah blob çıktı. Bu sefer renk aksanları zorunlu.
```
mcp__pixellab__create_character(
  name="Shadowblade",
  description="agile assassin with lower face wrapped in dark cloth, sharp glowing crimson-red eyes clearly visible as primary focal point, layered dark indigo-charcoal leather armor with subtle deep violet trim at edges, dual curved daggers with faint crimson rift energy glow on blade edges, crouched predatory stance weight on balls of feet, dark cloak with indigo-purple inner lining, deep indigo and charcoal palette NOT pure black, visible armor layer detail throughout, glowing eyes and dagger edges as light sources against dark body, dark fantasy roguelite, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/sprites.zip`

---

### 1.4 Ranger
*Okçu. Kapüşon gevşek, yüz görünür, rift enerjili ok nişanda, sadak sırtta.*
```
mcp__pixellab__create_character(
  name="Ranger",
  description="lean archer with hood loosely framing sharp focused face with alert eyes, worn dark leather armor with earthy tones, longbow held upright with rift-infused arrow nocked and glowing blue-purple at tip, quiver visible on back, upright ready stance showing precision and control, rift energy blue-purple on arrow tip and bow limbs, dark earthy palette deep forest greens and muted browns with rift blue-purple glow accents, dark fantasy roguelite, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/sprites.zip`

---

## BÖLÜM 2 — ACT 1 DÜŞMANLARI (Normal)

> `create_character` | mode: pro | view: low top-down | 8 yön
> ⚠ Palette çakışma tablosu:
> - Warblade (oyuncu) = koyu çelik mavi → FractureKnight FARKLI olmalı
> - VoidThrall = koyu mor → ChainWarden FARKLI olmalı
> - IronWarden = ağır mavi-mor → TwiceBorn Primary FARKLI olmalı

---

### 2.1 ShardWalker — 64px
*Fractured. Shard fırlatır, ölünce patlama. Parçalı insansı form.*
```
mcp__pixellab__create_character(
  name="ShardWalker",
  description="fractured humanoid creature made of broken stone shards held together by vivid blue-purple rift energy, visible glowing gaps between shards where rift light pours through, arm raised in throwing stance ready to launch shards, jagged silhouette with stone debris floating around body, cold stone grey with bright blue-purple rift glow at every fracture gap, dark fantasy roguelite enemy, RIMA universe",
  mode="pro",
  size=64,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/sprites.zip`

---

### 2.2 SeamCrawler — 48px
*Rift-Born. Zemin çatlaklarından çıkar, hızlı flanker. Yassı, pençe ve omurga belirgin.*
```
mcp__pixellab__create_character(
  name="SeamCrawler",
  description="flat low-profile spider-like creature clinging close to ground, sharp oversized claws and visible spine ridges, body pressed near surface in predator pose, rift energy cracks on underbelly glowing blue-purple, dark chitinous shell with rift glow, organic wet-looking appearance, rift-born creature, dark fantasy roguelite enemy, RIMA universe",
  mode="pro",
  size=48,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/SeamCrawler/sprites.zip`

---

### 2.3 VoidThrall — 96px
*Fractured. Ölünce ikiye bölünür. Uzun ince, void tendril'leri, translucent — tamamen siyah değil.*
```
mcp__pixellab__create_character(
  name="VoidThrall",
  description="tall thin humanoid wraith with elongated limbs and four void tendrils extending from its back and sides, body partially translucent with dark void energy visible within glowing faint indigo-violet, slow menacing upright stance, deep indigo-purple and translucent void palette with bright violet-white glow at tendril tips and visible inner core, NOT pure black body, internal glow makes silhouette readable against dark backgrounds, fractured dark fantasy enemy, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/VoidThrall/sprites.zip`

---

### 2.4 ChainWarden — 80px
*Fractured. Dash cezalandırır, 3 zincir fırlatır. Soğuk çelik-gri zırh — mor değil.*

> ⚠ VoidThrall ile palette çakışmasın: mor/violet kullanma.
```
mcp__pixellab__create_character(
  name="ChainWarden",
  description="heavy armored corrupted prison warden, three thick rusted iron chains extending from each arm coiled ready to throw, chains are the defining visual feature with glowing blue-purple rift energy at chain links, dark cold steel-grey armor with rust tones NOT purple NOT violet, cracked battleplate with hairline rift energy lines, imposing upright stance, cold steel-grey and rusted iron palette with rift glow on chain links only, dark fantasy emergent enemy, RIMA universe",
  mode="pro",
  size=80,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/ChainWarden/sprites.zip`

---

### 2.5 Penitent — 80px
*Emergent. Anti-heal aura yayar. Obsidian siyah + kırmızı çatlak — gri taş golem değil.*

> ⚠ Önceki üretimde generic gri golem çıktı, RIMA tonundan kopuktu.
```
mcp__pixellab__create_character(
  name="Penitent",
  description="massive hunched corrupted figure bound in dark obsidian-black stone, vivid deep crimson cracks glowing through the stone surface like fractured lava, enormous fists dragging on ground, shoulders drooping under immense guilt, visible anti-heal aura as dark ripples emanating from body, obsidian black body with bright crimson-red crack lines and faint blue-purple rift energy at edges, oppressive heavy silhouette, NOT plain grey stone NOT generic golem, emergent dark fantasy enemy, RIMA universe",
  mode="pro",
  size=80,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/Penitent/sprites.zip`

---

### 2.6 RelicCaster — 64px
*Fractured. Diğer düşmanlara kalkan verir, önce bu öldürülmeli. İnce, kırılgan, tek net siluet.*
```
mcp__pixellab__create_character(
  name="RelicCaster",
  description="tall thin corrupted sorcerer holding a broken ancient relic artifact that pulses blue-purple rift energy, robes tattered and floating at edges, one arm raised channeling shield energy from relic, lean fragile silhouette with clear distinct outline, pale grey and faded purple robe palette with strong rift energy glow emanating from relic artifact, fractured dark fantasy support enemy, RIMA universe",
  mode="pro",
  size=64,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/RelicCaster/sprites.zip`

---

### 2.7 FractureImp — 48px
*Rift-Born. 3-4 aynı anda spawn. Küçük ama silueti net — glowing gözler ve pençe uçları.*

> Not: Tasarımda 32px yazıyor ama 32px üretimde okunaksız çıktı.
> 48px üretilip Unity'de küçük ölçeklenerek 32px görsel ağırlığı verilecek.
```
mcp__pixellab__create_character(
  name="FractureImp",
  description="small fast imp creature born from rift cracks, compact body with oversized claws disproportionately large for its size, pointed ears and jagged spiky outline creating a clear readable silhouette, bright blue-purple rift energy glowing on claw tips and along spine joints, two small glowing eyes as secondary focal point, dark charcoal body with vivid rift energy details, menacing small silhouette clearly distinct from larger enemies, rift-born swarm creature, dark fantasy enemy, RIMA universe",
  mode="pro",
  size=48,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/FractureImp/sprites.zip`

---

## BÖLÜM 3 — ACT 1 ELİTE DÜŞMANLAR

---

### 3.1 IronWarden — 128px (Elite / Miniboss)
*Fractured Elite. Oda baskısı + faz testi. Devasa zırh, boyundan büyük omuzlar.*

> Not: Boss olarak 128px, elite varyantı 96px. Bu görevde 128px üretiyoruz.
```
mcp__pixellab__create_character(
  name="IronWarden",
  description="massive heavily armored elite guardian with enormous pauldrons larger than its own head, thick dark iron plating cracked with vivid blue-purple rift energy seeping through every crack, wielding a massive two-handed maul, imposing wide powerful stance radiating overwhelming menace, dark iron black and deep steel blue palette with strong rift energy glow at armor cracks, elite fractured dark fantasy enemy, RIMA universe",
  mode="pro",
  size=128,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/IronWarden/sprites.zip`

---

### 3.2 TwiceBorn Primary — 80px (Elite Çift)
*Fractured Elite. Kemik-beyaz zırh — mavi zırhlı düşmanlardan belirgin ayrışım.*

> ⚠ Önceki üretimde standart mavi-çelik çıktı, IronWarden/FractureKnight ile çakıştı.
```
mcp__pixellab__create_character(
  name="TwiceBorn_Primary",
  description="elite reborn warrior with aged bone-white armor showing battle wear and cracks throughout, rift energy blue-purple glowing at every fracture point in pale armor, shield and mace held in aggressive combat stance, undead-reborn aesthetic with bone white as primary armor color distinctly different from other enemies, imposing slightly taller dominant posture, rift energy as only color accent against pale bone armor, fractured dark fantasy elite enemy, RIMA universe",
  mode="pro",
  size=80,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/TwiceBorn/Primary/sprites.zip`

---

### 3.3 TwiceBorn Secondary — 80px (Elite Çift)
*Fractured Elite. Aynı gövde ama koyu crimson zırh — Primary'den net ayrışım.*
```
mcp__pixellab__create_character(
  name="TwiceBorn_Secondary",
  description="elite reborn warrior with dark crimson-red armor showing battle damage and cracks, matching shield and blade in feral aggressive combat stance, same body proportions as twin counterpart but deep crimson-red armor instead of bone-white, slightly lower more feral aggressive posture, rift energy blue-purple at armor cracks as accent, dark crimson red as primary distinctive color, fractured dark fantasy elite enemy, RIMA universe",
  mode="pro",
  size=80,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/TwiceBorn/Secondary/sprites.zip`

---

### 3.4 FractureKnight — 80px (Elite Skirmisher)
*Fractured Elite. Oyuncunun dash'ini taklit eder. Obsidian siyah + cyan-teal — Warblade çelik mavisiyle karışmaz.*

> ⚠ Oyuncu Warblade ile palette çakışmaması için mavi-çelik kullanma.
```
mcp__pixellab__create_character(
  name="FractureKnight",
  description="agile elite fractured knight with obsidian-black cracked armor, vivid cyan-teal rift energy crack lines across the black armor as primary color accent, low crouched aggressive stance suggesting explosive fast movement, dual short swords held low at sides, athletic lean silhouette contrasting with heavier enemies, obsidian black armor with cyan-teal rift energy cracks NOT blue-steel NOT grey, fractured dark fantasy elite enemy, RIMA universe",
  mode="pro",
  size=80,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/FractureKnight/sprites.zip`

---

### 3.5 Reliquary — 96px (Elite Sabit Yapı)
*Fractured Elite. Hareket etmez. 4 shard etrafında döner, shard'lar tek tek kırılabilir.*
```
mcp__pixellab__create_character(
  name="Reliquary",
  description="stationary elite construct, round symmetrical corrupted relic guardian with no legs, four glowing crystal shards visibly orbiting its central spherical body radiating blue-purple rift energy, heavily armored spherical core of ancient stone and metal construction cracked with blue-purple rift energy throughout, imposing symmetric silhouette with orbiting shards clearly visible as separate distinct elements, fractured dark fantasy elite construct, RIMA universe",
  mode="pro",
  size=96,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act1/Reliquary/sprites.zip`

---

## BÖLÜM 4 — ACT 2 DÜŞMANLARI (Başlangıç)

> `create_character` | mode: pro | view: low top-down | 8 yön
> Palet: derin mor, çürüyen toprak, sickly yeşil — Act 1'den bilinçli ayrışım
> Not: Thorn Brute, Carrion Weaver, Blood Lancer, Husk Thrower, Decay Anchor sonraki batch

---

### 4.1 MireStalker — 64px
*Rift-Born. Oyuncunun durduğu yere bataklık bırakır. Uzun bacaklar, bataklık damlıyor.*
```
mcp__pixellab__create_character(
  name="MireStalker",
  description="fast rift-born creature with elongated dripping legs leaving swamp residue trails behind, lean hunting body held low and forward in predatory stance, dark purple-tinged swamp mud visibly dripping from limbs, patches of sickly green bioluminescence on body surface, rift energy blue-purple cracks at leg joints, deep purple and dark earthy swamp green palette, dark fantasy enemy Act 2, RIMA universe",
  mode="pro",
  size=64,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act2/MireStalker/sprites.zip`

---

### 4.2 RotPriest — 64px
*Fractured. Anti-heal curse uygular. İnce, eğik, çürük cüppe, necrotic yeşil asa.*
```
mcp__pixellab__create_character(
  name="RotPriest",
  description="thin corrupted priest figure in decaying dark robes covered in rot and dead wilted flowers, hands raised in corrupted blessing gesture emitting sickly anti-heal aura as dark necrotic green wisps, hollow face visible under tattered hood with vacant eyes, staff of dead twisted wood topped with necrotic green glow, deep purple and necrotic green palette with faint blue-purple rift energy traces, fractured dark fantasy support enemy Act 2, RIMA universe",
  mode="pro",
  size=64,
  view="low top-down"
)
```
Kaydet: `STAGING/Enemies/Act2/RotPriest/sprites.zip`

---

## BÖLÜM 5 — SKILL İKONLARI

> **Endpoint: `create_map_object`** — standalone obje, transparent background
> **`create_tiles_pro` KULLANMA** — tile formatı üretir, ikon olmaz
> **`create_character` KULLANMA** — humanoid karakter formatı, ikon olmaz
>
> Parametreler her ikon için:
> - width=64, height=64
> - view="side" (yan görünüm ikonu daha net gösterir)
> - shading="detailed shading"
> - outline="single color outline"
> - detail="high detail"

---

### 5.1 Warblade Skill İkonları

**IronCharge** (Dash + stun):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, armored steel gauntlet fist punching forward with explosive blue-purple rift energy impact burst and motion speed lines, dark iron and steel colors, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Warblade/IronCharge.png`

**GravityCleave** (AoE kılıç slam):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, massive broadsword slamming downward with blue-purple rift energy shockwave rings radiating outward from impact point, dark iron blade, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Warblade/GravityCleave.png`

**WarStomp** (Knockup AoE):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, armored boot stomping ground with concentric shockwave rings expanding outward and blue-purple rift energy burst at impact point, dark iron color, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Warblade/WarStomp.png`

**DeathBlow** (Execute finisher):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, large sword poised for downward execute strike with concentrated blood-red and blue-purple rift energy at blade tip, dark iron blade with rift energy glow, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Warblade/DeathBlow.png`

---

### 5.2 Elementalist Skill İkonları

**Fireball**:
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, flaming orb with amber orange fire and rift energy blue-purple core glowing within, ember particles trailing around it, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Elementalist/Fireball.png`

**GlacialSpike**:
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, sharp crystalline ice spike with cold blue-white frost glow and rift energy shimmer, frost crystals forming around base, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Elementalist/GlacialSpike.png`

**Blink** (Teleport):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, arcane teleport afterimage with blue-purple rift energy swirl and ghostly dual-image trail showing instant movement blink, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Elementalist/Blink.png`

**Meteor**:
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, large flaming meteor with amber fire trail and rift energy crackling around it mid-fall, impact shockwave rings below, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Elementalist/Meteor.png`

---

### 5.3 Shadowblade Skill İkonları

**Backstab**:
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, curved dagger striking from behind with critical hit spark and shadow afterimage, rift energy crimson glow on blade edge, dark purple and crimson colors, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Shadowblade/Backstab.png`

**Shadowstep**:
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, dark shadow dash leaving split afterimage trail in deep violet and indigo with rift energy wisps showing instant movement, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Shadowblade/Shadowstep.png`

**FanOfKnives** (360° AoE):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, multiple curved daggers radiating outward in circular 360 degree burst pattern from center point, crimson rift energy glow on each blade tip, dark silver and deep purple colors, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Shadowblade/FanOfKnives.png`

**Vanish** (Stealth):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, figure dissolving into dark shadow wisps with violet smoke and rift energy fade effect, disappearing into darkness leaving only outline trace, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Shadowblade/Vanish.png`

---

### 5.4 Ranger Skill İkonları

**AimedShot** (Şarj ok):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, arrow nocked on bow fully drawn back with rift energy charging bright blue-purple on arrow tip, focused precision aim, dark earthy bow with rift glow on tip, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Ranger/AimedShot.png`

**RainOfArrows** (Burst):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, multiple rift-energy arrows falling in dense downward rain pattern, blue-purple glowing arrowheads, falling spread pattern showing area coverage, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Ranger/RainOfArrows.png`

**ConcussiveArrow** (Stun):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, single heavy arrow with blue-purple rift energy impact burst explosion at tip with stun rings radiating outward, dark earthy arrow with rift energy impact, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Ranger/ConcussiveArrow.png`

**EvasiveRoll** (Dash):
```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, rolling dodge motion with rift energy streak trail behind showing agile evasive roll movement, three motion blur frames visible, dark leather and blue-purple rift energy, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
İndir → kaydet: `STAGING/Icons/Skills/Ranger/EvasiveRoll.png`

---

## BÖLÜM 6 — TILE'LAR

> **Endpoint: `create_tiles_pro`** — square_topdown parametresiyle flat görünüm
> **tile_type="square_topdown"** geçmezsen DEFAULT=isometric, diamond shape çıkar
> **tile_view="top-down"** geçmezsen default depth eklenir
>
> Parametreler:
> - tile_type="square_topdown"  ← ZORUNlu, bu olmadan diamond shape çıkar
> - tile_view="top-down"        ← flat, depth yok
> - tile_size=32
> - outline_mode="segmentation" ← daha temiz tile, outline artifact yok
> - n_tiles=1

### 6.1 Act 1 Zemin Tile
```
mcp__pixellab__create_tiles_pro(
  description="1). cold dark dungeon stone floor tile, cracked worn cobblestone, dark charcoal-grey stone surface with subtle blue-purple rift energy hairline cracks between some stones, muted and dark must recede into background",
  tile_type="square_topdown",
  tile_view="top-down",
  tile_size=32,
  outline_mode="segmentation",
  n_tiles=1
)
```
İndir tile PNG → kaydet: `STAGING/Tiles/Act1/floor_stone.png`

### 6.2 Act 1 Duvar Tile
```
mcp__pixellab__create_tiles_pro(
  description="1). dark dungeon fortress wall tile, rough dark grey stone with mortar lines and some missing bricks, darker than floor tile for clear visual separation, cold muted blue-grey palette with subtle rift energy at mortar cracks",
  tile_type="square_topdown",
  tile_view="top-down",
  tile_size=32,
  outline_mode="segmentation",
  n_tiles=1
)
```
İndir tile PNG → kaydet: `STAGING/Tiles/Act1/wall_stone.png`

---

## TAMAMLAMA

Her iş bitince `STAGING/DONE.txt` dosyasına kaydet:
```
[DONE] CharacterName | character_id | tarih
[DONE] icon_IronCharge | object_id | tarih
[DONE] floor_stone | tile_id | tarih
[FAILED] CharacterName | sebep
```

Claude Code review edecek. Animasyon görevleri ayrı batch.
