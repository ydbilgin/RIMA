[NO TEXT BLOCK]
ABSOLUTELY NO TEXT ANYWHERE.
No class names. No labels. No captions. No letters. No words. No numbers. No UI text. No health bars. No banners. No tags. No watermarks. No frames. No grid lines.
The image must contain only the 16 characters on a fully transparent background, no typography of any kind.

[OUTPUT LAYOUT BLOCK]
Generate a clean 4x4 character contact sheet: 16 separate full-body pixel-art characters, evenly spaced, transparent background.
Render the contact sheet at minimum 2048x2048 so each cell holds high-resolution detail before any later downsample.
Each sprite should be designed with the readability of a 64-pixel-tall in-game ARPG sprite, but the sheet itself must not be a literal tiny 64x64 micro-image.
No boxes, no borders, no visible grid, no labels. Each character must be isolated and readable as its own sprite.
Add minimum 32-pixel transparent padding between every sprite. No silhouette touches another character, no silhouette touches the grid edge or frame.
Output PNG with alpha channel. Outside every character silhouette the background must be fully transparent RGBA (0,0,0,0).
No checkered transparency pattern, no off-white background, no painted scene, no floor tiles, no walls, no room, and no environmental props.
No shadows except a small soft oval ground shadow directly under each pair of feet.

[CAMERA BLOCK]
Hero Siege-like compact ARPG gameplay sprite camera: fake 2.5D top-down with a slight south-facing front bias.
High enough to minimize facial detail, but not pure 90-degree bird's-eye.
Every character uses a south-facing gameplay idle pose: the body faces the bottom of the screen, while the camera remains high above looking downward.
This is an in-game combat sprite for 8-direction action readability, NOT a front-view portrait, NOT a character-select pose, NOT a flat front paper-doll.

Camera geometry requirements for every character:
- The top of the skull / hair mass is the clearest part of the head.
- Top planes of shoulders are visible from above and read wider than the lower torso.
- Upper torso is compressed vertically by the top-down camera.
- Weapons, shields, staffs, bows, pistols, gauntlets, chains, lanterns, monster body shapes, and class props must read before facial features.
- Face details are minimal because of the high camera: eyes only tiny dark marks or 1-2 pixels, nose and mouth absent or nearly invisible, no readable facial expression.
- No centered portrait eyes, no detailed anime face, no large attractive portrait face.
- Torso height shortened by foreshortening, vertical body length visibly squashed compared to a front-view character.
- Legs visibly foreshortened and shorter than normal front-view legs.
- Feet small, low in the sprite, and partly de-emphasized by perspective.
- Soft oval ground shadow directly beneath each character's feet.

Character identity must come from silhouette, weapon, armor shape, robe shape, hair mass, posture, monster body shape, and color accents, not facial detail.

Avoid: southeast diagonal 3/4 view, diagonal eye-level body rotation, eye-level character-select pose, flat front paper-doll, centered portrait face, detailed face, full front body proportions, long straight legs, pure side view, pure 90-degree bird's-eye, chibi portrait sprite, southeast or southwest body twist, hips turned away from camera.

[DIRECTION LOCK BLOCK]
Exact SOUTH-facing direction for every humanoid character.
The body centerline must be vertical and face the bottom edge of the image, not the bottom-right or bottom-left.
Shoulders should read mostly horizontal across the sprite, not diagonally rotated.
Head, torso, hips, and feet must all point south/down-screen.
The character must look like the player/camera is directly below the character, not southeast of the character.
Weapons, staffs, bows, chains, and props may angle outward for silhouette readability, but the body direction must remain exact south.
Avoid any southeast-facing, southwest-facing, side-facing, diagonal 3/4 body turn, twisted hips, or over-the-shoulder pose.

[PROPORTION BLOCK]
Mature adult proportions under Hero Siege-like top-down foreshortening. Bodies should read as compact adult ARPG class sprites after perspective compression, not normal eye-level proportions.
NOT chibi, NOT super-deformed, NOT big-head-small-body, NOT 2-3 head-height proportions.
Keep heads readable for 64x64 pixel art, but do not enlarge them into cute chibi portrait heads.
Reference body mass: Hammerwatch / Diablo 2 / Last Epoch / Hero Siege in-game sprites - grounded, weighty, weathered adults.
Faces are tiny and compressed by the high camera, while bodies still read as full adults through shoulder width, torso mass, limb thickness, gear scale, and weapon silhouette.

[ANCHOR ISOLATION BLOCK]
Use all attached reference images ONLY for: Hero Siege-like 64x64 pixel density, compact ARPG sprite scale, outline weight, south-facing gameplay readability, minimal face detail, top-down-ish body compression, and class silhouette clarity.
These references are camera/style anchors, but NOT identity anchors.
Do NOT copy any exact character design, costume, weapon, shield, helmet, hood, face, color palette, class identity, pose, or prop from the references.
Do NOT generate UI, text, labels, a room, floor, walls, borders, grid lines, or background elements.
The target sprites must remain isolated transparent character sprites with 64x64 final sprite readability using the Hero Siege-like compact ARPG camera from the camera block and the exact SOUTH-facing direction from the direction lock block.
Generate completely original RIMA characters matching the identity descriptions below; the references are camera, scale, outline, and readability anchors only.

---
10 Class Identity Blocks

01 - Warblade
Identity: adult male battle-worn melee warrior, mid-30s, mature proportions. Dark steel plate over warm brown weathered leather, light scuffs and battle wear. Greatsword carried on right shoulder, hilt up, blade angled across the back; both hands free or one hand on the hilt - clearly a two-handed greatsword silhouette. Palette: dark steel grey, warm desaturated brown leather, ember orange accent, dull silver edge highlights. Strictly NO blue, NO purple, NO green, NO magic glow, NO runes, NO gems, NO long cape. Grounded weathered ARPG warrior, NOT goth, NOT plague doctor, NOT hooded mage, NOT shining knight, NOT samurai.

02 - Elementalist
Identity: adult female elemental scholar, mid-20s, mature proportions, NOT chibi. Honey-blonde hair in a low bun, reading mainly as a top-down hair mass with small side strands. Combat-practical exposed midriff: cropped top + bare midriff + short skirt over dark fitted tights and high boots; arms bare or fingerless gloves. Long staff held in left hand, small floating rune disc hovering at right hand. Palette: dusty indigo robes/cloth, cream highlights, gold trim, soft warm rune glow. The floating rune disc emits a warm gold-cream glow matching her trim accent. Strictly NO bikini, NO full-cover robe, NO hooded face, NO pointy wizard hat, NO red/orange dominant. Grounded arcane scholar.

03 - Shadowblade
Identity: adult male phase assassin, lean wiry build. Lower-face cloth veil up to nose; upper face reads only as tiny dark eye marks under the high camera, never a detailed portrait face. Twin daggers, one in each hand, blades short and curved. Dark hood acceptable but the visible upper face must not become a fully black shadow. Palette: void black cloth, hot magenta accents on belt/sash/blade glow, worn brown leather harness. Strictly NO ninja headband cliche, NO full-face mask, NO glowing eyes, NO cape.

04 - Ranger
Identity: adult female rift stalker, athletic. Half-shaved hairstyle with a long braid on the unshaved side, hair crown and braid silhouette visible from above; war paint only reads as tiny dark marks, not a portrait face. NO face hood. Bone-recurve bow held in left hand, side-mounted quiver on right hip. Light leather armor, exposed forearms, fitted pants and laced boots. Palette: bone white, weathered brown, cold rift-purple accents on bow string and arrow fletching. Strictly NO green forest tones, NO Robin Hood cap, NO face hood, NOT cute archer girl.

05 - Ravager
Identity: adult male brutal berserker, broad shoulders, thick beard. TWO one-handed axes - ONE in each hand, both clearly visible, NEVER a single two-handed axe and NEVER fists. Heavy fur mantle on shoulders, bare or wrapped torso under, leather kilt and boots. Palette: dirty bronze, dark fur, crimson cloth wraps, dull iron axe heads. Strictly NO blue, NO purple, NO magic glow. NOT Brawler (no boxing guard, no gauntlets-only), NOT Warblade (no greatsword).

06 - Ronin
Identity: adult male iaido swordsman, lean disciplined posture. Katana sheathed at the LEFT hip; LEFT hand resting on the scabbard, RIGHT hand draw-ready near the hilt. Loose layered robe over fitted under-armor, sash at waist, dark hakama-like trousers. Palette: muted indigo and black, dull silver blade accent, off-white sash, optional subtle dull silver-blue draw-flash on the katana edge only. Strictly NO bright red, NO floral motifs, NO oni mask, NO topknot cliche, NOT a generic samurai shogun.

07 - Gunslinger
Identity: adult female ritual duelist, mid-20s, deep auburn red hair tied back. TWO pistols, BOTH visible - one in each hand or one drawn one holstered, but both readable. Dark leather longcoat or fitted vest, brass buckles, dusty red sash, fitted trousers and boots. Palette: dark leather brown, brass, dusty red, off-white shirt. Strictly NO cowboy hat, NO western frontier costume, NO magic glow, NO arcane runes - this is a mundane gunfighter aesthetic.

08 - Brawler
Identity: adult male footwork boxer, athletic mid-build, shaved or short hair. Boxing guard stance acceptable for primary frame. Bare torso or tight fighter undershirt, dark cloth pants tied at waist, dark steel gauntlets covering forearms and fists. Glowing arcane purple tattoo lines along the arms and chest; tattoo glow lines must stay 1-2 pixels wide maximum, thin readable lines, not large magenta blobs and not full-arm color washes. Palette: warm bronze skin, dark steel gauntlets, dark cloth pants, arcane purple tattoo glow accent. Strictly NO weapons, NO axes, NO swords, NO guns. NOT Ravager (no fur mantle, no axes).

09 - Summoner
Identity: adult female death commander, mid-20s, mature proportions. Human head readable, not masked or skull-faced; facial features are tiny and compressed by the Hammerwatch-like high camera. Long dark hair reads mainly as a dark hair mass from above, pale face only a small lower mark. Soul lantern carried in LEFT hand emitting cold cyan light, RIGHT hand raised in a commanding gesture. Long fitted dark robe with bone-white trim, asymmetric, fitted not bulky. Palette: void black robe, bone white trim and bone charms, cold cyan soul light only. Strictly NO purple, NO green, NO red, NO skull mask, NO necromancer hood-over-skull cliche.

10 - Hexer
Identity: adult female curse-binder, mid-30s. Human head readable inside a deep hood, but the face is only a small compressed light shape, not a portrait face and not a black void. Deep hood acceptable. Long deep violet robe, hanging bone charms on belt and sleeves. Curse staff in RIGHT hand; the staff TIP has a small skull motif emitting cursed green flame. Skull motif appears ONLY on the staff tip - NEVER on head, NEVER as mask, NEVER on chest. Palette: deep violet robe, weathered black trim, bone-white charms, cursed green flame accent on staff tip only. Strictly NO pointy witch hat, NO black-cat sidekick, NO skull mask, NOT a Halloween witch.

---
6 Mob/Boss Identity Blocks

11 - Fracture Imp
Identity: small hunched rift imp, knee-high to a human, predatory posture. Cracked dark grey skin, jagged irregular head shape, sharp claws on hands and bent digitigrade legs. A glowing rift crystal embedded in the chest or back, cold cyan-purple light leaking from cracks in the body. Palette: dark cracked grey skin, cold cyan-purple rift glow accent, no clothing. Strictly NO horns, NO devil tail cliche, NOT a goblin, NOT cute.

12 - Relic Caster
Identity: thin tall cursed relic-priest, mid-build. Torn ragged robe layered with bone charms and metal plaques. A floating relic tablet hovering near the right hand, faint cyan glyph glow on the tablet. Pale gaunt human head readable but face details are tiny and compressed by the Hammerwatch-like high camera; eyes are tiny shadow marks, not a portrait. Palette: weathered bone-white robe, dirty bronze metal plaques, cold cyan glyph glow on tablet only. Strictly NO mask, NO skull face, NO Christian-priest iconography.

13 - Seam Crawler
Identity: low wide armored centipede creature, body length roughly twice a human's height but only knee-high, multi-segmented. Stitched black armor plates fused along the back, glowing rift seams between plates leaking cold light. Many hooked clawed legs along both sides, raised armored head with mandibles. Quadruped exception to the south-facing direction lock: armored head points south toward the bottom of the cell, segmented body trails northward, body centerline stays vertical in the cell. Palette: matte black plating, glowing cold cyan-purple seam light, dull bone joints. Strictly NOT a giant insect-bug cliche, NOT a snake, NOT cute.

14 - Shard Walker
Identity: tall fractured stone humanoid, slow heavy posture. Body made of rough cracked stone with jagged crystal shards growing from shoulders, back, and head; ONE arm noticeably larger and ending in a large crystal shard fist. Cold rift cracks glow faintly between stone plates. Palette: weathered grey stone, cold rift-purple crystal glow, dark cracks. Strictly NOT a generic golem with eyes, NOT armored knight, no clothing.

15 - Chain Warden
Identity: heavy armored jailer humanoid, broad-shouldered intimidating silhouette. Rusted iron full plate, helmet with a narrow horizontal slit for eyes (no face visible). Heavy chains wrap around the torso and arms; one oversized chained gauntlet reads as the primary weapon-arm. The chains are the identity element - multiple, heavy, wrapping. Palette: rusted iron, dark leather straps, dull bronze rivets, no glow. Strictly NOT a generic knight, NOT a paladin, no cape, no shield.

16 - Penitent (mini-boss)
Identity: chained ritual penitent, tall gaunt humanoid mini-boss. Broken ritual armor, asymmetric and battle-damaged. Cracked ritual mask covering upper face - eyes glow faintly through cracks. Heavy restraint chains hanging from wrists and waist, dragging an iron weight from one chain. A glowing chest wound emits cold cyan-purple light through the cracked breastplate. Render Penitent at approximately 110-120% the height of the other 15 sprites in the sheet so the mini-boss reads taller and more imposing. Palette: weathered bone-white armor, rusted iron chains, cold cyan-purple wound glow. Strictly NOT a shirtless prisoner, NOT a generic knight, NOT a hooded executioner, the cracked-mask-plus-chest-wound is the identity.
