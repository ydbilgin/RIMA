# ANIM2_cx - Elementalist anim/VFX council

Kaynak notu: NotebookLM sorgusu auth expired ile dondu; karar mevcut kod (`Assets/Scripts/Skills/Elementalist/*`, `Assets/Scripts/VFX/SkillVfx.cs`), onceki warblade council karari ve arxiv VFX sahiplik notlariyla temellendi.

## E1 - Elementalist anim seti

**Karar:** Elementalist icin "attack" melee degil, **temel cast / bolt release** olmali. Warblade'in LMB strike'i neyse caster icin karsiligi budur: tek okunur beden hareketi + skill VFX.

**P1 post-demo minimum:**
- breathing idle
- run
- basic cast / bolt release

**P2 sadece P1 temizse:**
- flinch / hit-react
- element switch micro-cast pozu veya cast-hold/channel pozu

**P3:**
- bespoke Fire/Frost/Light skill castleri
- death
- long channel / ultimate / Lightbreak hero animasyonlari

State-first set:
- `breathing-idle`: sakin caster stance, eller enerjiye hazir.
- `mid-run`: robe/ayak hareketi okunur, orb/eller sabit.
- `cast-charge/windup`: iki el veya tek avuc ileri, enerji toplama.
- `flinch`: buyu bozulmus, agirlik geriye kacmis.

## E2 - Skill VFX yaklasimi

**Secim: C - hybrid.**

Gerekce:
- Engine-only guvenli ama Elementalist "guzel skill VFX" istegini tek basina tasimaz.
- Full PixelLab skill seti post-demo icin bile fazla; cleanup ve entegrasyon riskini buyutur.
- Hybrid, Dead Cells mantigini korur: tek cast anim + `SkillVfx` tint/additive/trail/scale-fade + az sayida hero PixelLab shape.

PixelLab cekirdek VFX assetleri:
- Fireball projectile: 8 direction, 64px, transparent, `create_8_direction_object`.
- Fire impact burst: 80px, transparent, `create_map_object`.
- Glacial spike line/cluster: 96x64 veya 96px canvas, transparent, `create_map_object`.
- Frozen orb core: 64px, transparent, `create_map_object`.
- Light beam core strip or impact flare: 96x32 strip veya 80px burst, transparent, `create_map_object`.

Uretme: Meteor, Blizzard, Frost Wall, Living Bomb mark, Lightbreak hero set simdilik yok. Bunlar engine telegraph + mevcut placeholder/Resources ile yurur.

## E3 - PixelLab prompt seti

### Character ids

- Warblade: `2656075d-d113-4f18-a6c1-94b5a6b8bf65`
- Elementalist: `4c83c0be-e856-48f1-b8b5-9626e041a082`

### Warblade state prompts - `create_character_state`

`mid-run`
```text
same warblade, high top-down 2D game sprite, mid-run pose, forward lean, one foot planted one foot trailing, sword kept readable at side, preserve armor and silhouette, no VFX, no redesign
```

`strike-windup`
```text
same warblade, high top-down 2D game sprite, sword strike windup pose, blade pulled back before a heavy slash, feet braced, torso twisted, preserve armor and weapon identity, no VFX, no redesign
```

`breathing-idle`
```text
same warblade, high top-down 2D game sprite, guarded breathing idle stance, sword ready but relaxed, weight balanced, subtle combat readiness, preserve armor and silhouette, no VFX, no redesign
```

`flinch`
```text
same warblade, high top-down 2D game sprite, hit reaction flinch pose, upper body recoiling, guard broken for a moment, sword still in hand, preserve armor and silhouette, no VFX, no redesign
```

### Elementalist state prompts - `create_character_state`

`mid-run`
```text
same elementalist, high top-down 2D game sprite, mid-run caster pose, robe and legs in motion, upper body controlled, casting hand and orb kept readable, preserve teal robe rune identity, no VFX, no redesign
```

`cast-charge-windup`
```text
same elementalist, high top-down 2D game sprite, spell cast windup pose, one hand forward gathering energy, other hand stabilizing near chest, feet planted, preserve teal robe rune identity, minimal hand glow only, no redesign
```

`breathing-idle`
```text
same elementalist, high top-down 2D game sprite, calm breathing idle caster stance, hands ready near body, robe settled, orb/rune details readable, preserve teal robe identity, no VFX, no redesign
```

`flinch`
```text
same elementalist, high top-down 2D game sprite, interrupted casting flinch pose, shoulders recoiling, one hand pulled back, robe motion small, preserve teal robe rune identity, no VFX, no redesign
```

### Animation prompts - `animate_character`, mode `v3`

Directions for production: generate `S, SE, E, NE, N`; mirror `SW, W, NW` with flipX.

Warblade:
- from `breathing-idle`: `guarded breathing idle loop, subtle chest and shoulder motion, sword steady, no foot sliding` - `frame_count: 8`
- from `mid-run`: `run loop, grounded top-down ARPG movement, stable sword silhouette, readable foot cycle` - `frame_count: 8`
- from `strike-windup`: `basic sword slash attack, fast anticipation into heavy slash and short recovery, no extra magic VFX` - `frame_count: 6`
- from `flinch`: `short hit reaction, recoil then recover toward guarded stance, no spin, no knockdown` - `frame_count: 4`

Elementalist:
- from `breathing-idle`: `calm caster breathing idle loop, subtle robe movement, hands steady, no large spell effects` - `frame_count: 8`
- from `mid-run`: `run loop for a robed caster, clean foot cycle, upper body controlled, hands and orb readable` - `frame_count: 8`
- from `cast-charge-windup`: `basic spell cast, gather energy then release forward, quick recovery to caster stance, small hand glow only` - `frame_count: 6`
- from `flinch`: `short interrupted cast hit reaction, recoil then regain balance, no fall, no large VFX` - `frame_count: 4`

### VFX asset prompts

Fireball projectile - `create_8_direction_object`, 64px, transparent:
```text
top-down pixel art fireball projectile, compact bright orange core with small ember tail, readable in 8 directions, 64x64, transparent background, no character, no ground shadow
```

Fire impact burst - `create_map_object`, 80px, transparent:
```text
top-down pixel art fire impact burst, circular ember explosion with bright core and short sparks, 80x80, transparent background, no character, no ground tile
```

Glacial spike cluster - `create_map_object`, 96px, transparent:
```text
top-down pixel art glacial spike line cluster, sharp cyan ice shards rising in a narrow forward line, readable gameplay silhouette, 96x64 canvas, transparent background, no character
```

Frozen orb core - `create_map_object`, 64px, transparent:
```text
top-down pixel art frozen orb, rotating cyan-blue ice sphere with small frost shards around it, 64x64, transparent background, no character, no ground tile
```

Light beam flare - `create_map_object`, 96px, transparent:
```text
top-down pixel art radiant light beam flare, narrow golden-white horizontal strip with bright center and crisp edges, 96x32, transparent background, no character, no rainbow spray
```

## E4 - Uretim sirasi + budget

1. Warblade P1 demo once: 4 state hazirla ama animasyonda P1 sadece idle/run/LMB. State call: 4. Anim call: 3 anim x 5 direction = 15. P2 flinch sadece P1 OBS proof temizse +5.
2. Elementalist post-demo: 4 state. P1 anim: idle/run/basic cast = 15. P2 flinch +5.
3. VFX post-demo lean core: 5 asset prompt. Fireball 8-dir = 8 gen; diger 4 map object = 4 gen. Reroll buffer dahil ~20-30 gen yeter.

Gercek bottleneck PixelLab kredi degil, per-direction cleanup. Warblade P1 demo gecikmeyecek; Elementalist sadece prompt/prep ve post-demo queue.
