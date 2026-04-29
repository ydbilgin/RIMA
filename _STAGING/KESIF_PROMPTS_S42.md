# Keşif Modu Prompts (S42 · 64px Create-from-Style-Reference Pro)

**Tarih:** 2026-04-26 · **Mode:** PixelLab "Create from Style Reference (Pro)" · **Kullanım:** 16 AYRI run (4×4 sheet YASAK — text leak + identity drift, bkz. `memory/project_64px_pivot_s42.md`).

## UI Settings (her 16 run için aynı)

| Alan | Değer |
|---|---|
| Mode | Create from Style Reference (Pro) |
| Size | 64 px |
| Body type | bipedal-realistic (mob istisnası altta) |
| View | high top-down |
| n_directions | 8 |
| Variations | 16 |
| Style image PRIMARY | `_STAGING/style_refs_s42/anchor_camera_60.png` |
| Style image 2nd | boş — Warblade PASS sonrası onun output'u 2. slot |

**Style image checkboxes:** Outline ON · Shading ON · Color OFF · Detail OFF
**Fallback set 1** (Hero Siege'e benziyorsa): Outline ON · Shading OFF · Detail OFF · Color OFF
**Fallback set 2** (kamera tutmuyor renk iyi): Outline ON · Shading ON · Detail ON · Color OFF

**Mob body type istisnası:** Fracture Imp / Seam Crawler / Shard Walker → `humanoid` veya `creature` (UI'da hangisi varsa). Penitent / Relic Caster / Chain Warden → `bipedal-realistic`.

## Pilot Sıra (kolay→zor identity)

Warblade → Ranger → Ronin → Ravager → Brawler → Gunslinger → Elementalist → Shadowblade → Summoner → Hexer → Fracture Imp → Relic Caster → Seam Crawler → Shard Walker → Chain Warden → Penitent

## Shared Blocks (her description'a aynen)

### [NO TEXT BLOCK]
```
ABSOLUTELY NO TEXT ANYWHERE.
No class names. No labels. No captions. No letters. No words. No numbers. No UI text. No health bars. No banners. No tags. No watermarks. No frames. No grid lines.
The sprite must contain only the character on a fully transparent background, no typography of any kind.
```

### [CAMERA BLOCK]
```
Camera angle: ~60 degrees from horizontal (~30 degrees from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera, locked.
South-facing front pose for the primary frame; character faces the player/camera.
Camera is above the character. Top of head and hair clearly visible. Forehead and hair-top read more than the lower face.
Eyes small and high on the face, NOT centered like a portrait. Nose and mouth tiny and simplified. Chin compressed.
Upper shoulder planes visible. Torso slightly compressed by foreshortening. Feet lower in the frame and slightly smaller than head/shoulders.
Avoid: southeast diagonal 3/4 view, eye-level character-select pose, flat front paper-doll, pure side view, pure 90-degree bird's-eye.
```

### [ANCHOR ISOLATION BLOCK]
```
Use the attached style reference ONLY for: camera angle (~60 degrees from horizontal high top-down ARPG), outline weight, shading style, pixel density, and color saturation level.
Do NOT copy from the reference: any character design, any mask, any hood shape, any costume, any weapon, any shield, any lantern, any prop, any specific color palette, any identity element. The reference figure is plague-doctor-like and is NOT in this game.
Generate a completely original character matching the identity description below; the reference is a STYLE anchor only.
```

## 10 Class Identity Blocks

### 01 Warblade
```
Identity: adult male battle-worn melee warrior, mid-30s, mature proportions. Dark steel plate over warm brown weathered leather, light scuffs and battle wear. Greatsword carried on right shoulder, hilt up, blade angled across the back; both hands free or one hand on the hilt — clearly a two-handed greatsword silhouette. Palette: dark steel grey, warm desaturated brown leather, ember orange accent, dull silver edge highlights. Strictly NO blue, NO purple, NO green, NO magic glow, NO runes, NO gems, NO long cape. Grounded fantasy, weathered, NOT goth, NOT plague doctor, NOT hooded mage, NOT shining knight, NOT samurai.
```

### 02 Elementalist
```
Identity: adult female elemental scholar, mid-20s, mature proportions, NOT chibi. Honey-blonde hair in a low bun, side strands. Combat-practical exposed midriff: cropped top + bare midriff + short skirt over dark fitted tights and high boots; arms bare or fingerless gloves. Long staff held in left hand, small floating rune disc hovering at right hand. Palette: dusty indigo robes/cloth, cream highlights, gold trim, soft warm rune glow. Strictly NO bikini, NO full-cover robe, NO hooded face, NO pointy wizard hat, NO red/orange dominant. Grounded arcane scholar.
```

### 03 Shadowblade
```
Identity: adult male phase assassin, lean wiry build. Lower-face cloth veil up to nose; eyes visible above veil. Twin daggers, one in each hand, blades short and curved. Dark hood acceptable but eyes always visible, NEVER fully shadowed face. Palette: void black cloth, hot magenta accents on belt/sash/blade glow, worn brown leather harness. Strictly NO ninja headband cliché, NO full-face mask, NO glowing eyes, NO cape.
```

### 04 Ranger
```
Identity: adult female rift stalker, athletic. Half-shaved hairstyle with a long braid on the unshaved side, war paint stripes across the eyes. NO face hood, head fully visible. Bone-recurve bow held in left hand, side-mounted quiver on right hip. Light leather armor, exposed forearms, fitted pants and laced boots. Palette: bone white, weathered brown, cold rift-purple accents on bow string and arrow fletching. Strictly NO green forest tones, NO Robin Hood cap, NO face hood, NOT cute archer girl.
```

### 05 Ravager
```
Identity: adult male brutal berserker, broad shoulders, thick beard. TWO one-handed axes — ONE in each hand, both clearly visible, NEVER a single two-handed axe and NEVER fists. Heavy fur mantle on shoulders, bare or wrapped torso under, leather kilt and boots. Palette: dirty bronze, dark fur, crimson cloth wraps, dull iron axe heads. Strictly NO blue, NO purple, NO magic glow. NOT Brawler (no boxing guard, no gauntlets-only), NOT Warblade (no greatsword).
```

### 06 Ronin
```
Identity: adult male iaido swordsman, lean disciplined posture. Katana sheathed at the LEFT hip; LEFT hand resting on the scabbard, RIGHT hand draw-ready near the hilt. Loose layered robe over fitted under-armor, sash at waist, dark hakama-like trousers. Palette: muted indigo and black, dull silver blade accent, off-white sash. Strictly NO bright red, NO floral motifs, NO oni mask, NO topknot cliche, NOT a generic samurai shogun.
```

### 07 Gunslinger
```
Identity: adult female ritual duelist, mid-20s, deep auburn red hair tied back. TWO pistols, BOTH visible — one in each hand or one drawn one holstered, but both readable. Dark leather longcoat or fitted vest, brass buckles, dusty red sash, fitted trousers and boots. Palette: dark leather brown, brass, dusty red, off-white shirt. Strictly NO cowboy hat, NO western frontier costume, NO magic glow, NO arcane runes — this is a mundane gunfighter aesthetic.
```

### 08 Brawler
```
Identity: adult male footwork boxer, athletic mid-build, shaved or short hair. Boxing guard stance acceptable for primary frame. Bare torso or tight fighter undershirt, dark cloth pants tied at waist, dark steel gauntlets covering forearms and fists. Glowing arcane purple tattoo lines along the arms and chest. Palette: warm bronze skin, dark steel gauntlets, dark cloth pants, arcane purple tattoo glow accent. Strictly NO weapons, NO axes, NO swords, NO guns. NOT Ravager (no fur mantle, no axes).
```

### 09 Summoner
```
Identity: adult female death commander, mid-20s, mature proportions. HUMAN FACE FULLY VISIBLE — NO mask, NO skull face, NO horns, NO glowing-only eye sockets. Long dark hair, pale skin. Soul lantern carried in LEFT hand emitting cold cyan light, RIGHT hand raised in a commanding gesture. Long fitted dark robe with bone-white trim, asymmetric, fitted not bulky. Palette: void black robe, bone white trim and bone charms, cold cyan soul light only. Strictly NO purple, NO green, NO red, NO skull mask, NO necromancer hood-over-skull cliche.
```

### 10 Hexer
```
Identity: adult female curse-binder, mid-30s. FACE VISIBLE — NO mask. Deep hood acceptable but face inside the hood always visible. Long deep violet robe, hanging bone charms on belt and sleeves. Curse staff in RIGHT hand; the staff TIP has a small skull motif emitting cursed green flame. Skull motif appears ONLY on the staff tip — NEVER on head, NEVER as mask, NEVER on chest. Palette: deep violet robe, weathered black trim, bone-white charms, cursed green flame accent on staff tip only. Strictly NO pointy witch hat, NO black-cat sidekick, NO skull mask, NOT a Halloween witch.
```

## 6 Mob/Boss Identity Blocks

### 11 Fracture Imp
```
Identity: small hunched rift imp, knee-high to a human, predatory posture. Cracked dark grey skin, jagged irregular head shape, sharp claws on hands and bent digitigrade legs. A glowing rift crystal embedded in the chest or back, cold cyan-purple light leaking from cracks in the body. Palette: dark cracked grey skin, cold cyan-purple rift glow accent, no clothing. Strictly NO horns, NO devil tail cliche, NOT a goblin, NOT cute.
```

### 12 Relic Caster
```
Identity: thin tall cursed relic-priest, mid-build. Torn ragged robe layered with bone charms and metal plaques. A floating relic tablet hovering near the right hand, faint cyan glyph glow on the tablet. Pale gaunt face fully visible, hollow-cheeked, eyes shadowed but visible. Palette: weathered bone-white robe, dirty bronze metal plaques, cold cyan glyph glow on tablet only. Strictly NO mask, NO skull face, NO Christian-priest iconography.
```

### 13 Seam Crawler
```
Identity: low wide armored centipede creature, body length roughly twice a human's height but only knee-high, multi-segmented. Stitched black armor plates fused along the back, glowing rift seams between plates leaking cold light. Many hooked clawed legs along both sides, raised armored head with mandibles. Palette: matte black plating, glowing cold cyan-purple seam light, dull bone joints. Strictly NOT a giant insect-bug cliche, NOT a snake, NOT cute.
```

### 14 Shard Walker
```
Identity: tall fractured stone humanoid, slow heavy posture. Body made of rough cracked stone with jagged crystal shards growing from shoulders, back, and head; ONE arm noticeably larger and ending in a large crystal shard fist. Cold rift cracks glow faintly between stone plates. Palette: weathered grey stone, cold rift-purple crystal glow, dark cracks. Strictly NOT a generic golem with eyes, NOT armored knight, no clothing.
```

### 15 Chain Warden
```
Identity: heavy armored jailer humanoid, broad-shouldered intimidating silhouette. Rusted iron full plate, helmet with a narrow horizontal slit for eyes (no face visible). Heavy chains wrap around the torso and arms; one oversized chained gauntlet reads as the primary weapon-arm. The chains are the identity element — multiple, heavy, wrapping. Palette: rusted iron, dark leather straps, dull bronze rivets, no glow. Strictly NOT a generic knight, NOT a paladin, no cape, no shield.
```

### 16 Penitent (mini-boss)
```
Identity: chained ritual penitent, tall gaunt humanoid mini-boss. Broken ritual armor, asymmetric and battle-damaged. Cracked ritual mask covering upper face — eyes glow faintly through cracks. Heavy restraint chains hanging from wrists and waist, dragging an iron weight from one chain. A glowing chest wound emits cold cyan-purple light through the cracked breastplate. Palette: weathered bone-white armor, rusted iron chains, cold cyan-purple wound glow. Strictly NOT a shirtless prisoner, NOT a generic knight, NOT a hooded executioner, the cracked-mask-plus-chest-wound is the identity.
```

## QC her variation için (5 kapı, biri düşerse RED)
1. Kamera 60° (head-top, eyes high&small, foreshortened torso)
2. Anchor isolation (hooded/lantern/shield/plague mask leak yok)
3. Identity match (silah/silüet/palette per-class spec)
4. Palette disiplin (yasaklı renkler yok)
5. NO TEXT (tek harf bile = RED)

PASS eşiği: 6+/16 havuz hazır · 3-5 sınır (prompt tweak) · 0-2 checkbox fallback set'e geç.

## Workflow
1. Kullanıcı PixelLab UI'da run → 16 variation döner
2. Sheet Claude'a → QC PASS/RED + en iyi 1-2 seçim
3. PASS karakter → kullanıcı `character_id` verir
4. Tüm class PASS sonrası Claude MCP `animate_character` ile id üzerinden anim üretir (Run/Attack/Idle/Hit/Death)
