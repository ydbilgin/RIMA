# Codex Classes — S Direction Character Prompts
> RIMA | 2026-04-25 | Yeni ChatGPT chat'inde S yönü karakter denemeleri için

## Kullanım

Yeni ChatGPT image chat'inde önce sadece `warbladeNEW7.png` referansını yükle.

```text
Use the attached Warblade sprite ONLY as a camera/perspective/game-sprite scale reference.
Do NOT copy its side-profile direction, pose, weapon, outfit, colors, or body rotation.
The new character must face SOUTH/front directly and follow the prompt below.
```

İlk denemede `rima_style_anchor.png` yükleme. Eğer sonuç oyun tonundan kopuk gelirse ikinci denemede ekle ve sadece şunu söyle:

```text
Use the second reference only for dark fractured dungeon mood, lighting contrast, and world tone.
Do NOT generate a room, floor, props, monsters, or environment.
```

## Ortak Kamera Kilidi

Her promptta bu kamera kuralları geçerli:

- High top-down ARPG gameplay camera.
- NOT eye-level, NOT portrait, NOT heroic poster.
- Single full-body character, black background only.
- Character faces SOUTH/front; hips and shoulders square to camera plane.
- We clearly see top of head, shoulder top planes, top of boots, and mild face foreshortening.
- Head upright; gaze slightly downward-forward into the battlefield, not direct portrait eye contact and not bowed.
- Large readable shapes for 128x128 pixel-art conversion.

## Karar Notları — Hikaye + Skill Tutarlılığı

Bu dosya eski class promptlarını birebir kopyalamaz. RIMA'nın hikaye tonu, skill kimliği ve 128px silhouette ihtiyacına göre bazı kararları bilinçli değiştirdi.

| Class | Karar | Neden değişti |
|---|---|---|
| Warblade | Cold-blue tamamen yasak değil; çok sınırlı rift accent olarak geri alındı. | RIMA dünyasının ana fracture dili cold-blue. Tamamen mavi yasaklayınca Warblade generic brown/steel warrior'a kayıyor. Ancak skill/VFX tarafındaki mavi spam riskinden dolayı blue sadece sword edge + 2-3 armor crack ile sınırlı. |
| Elementalist | Staff çıkarıldı; neutral crystal orb + field scholar yapıldı, noble mage dili çıkarıldı. | Skill kimliği fire/frost/radiance ritmi ama base sprite tek elemente boyanmamalı. Element renkleri cast/VFX layer'da yaşayacak; base orb renksiz veya çok hafif nötr parıltılı kalacak. |
| Shadowblade | Generic hooded rogue yerine veil mask + hot-magenta phase scar. | Skill kimliği phase/mark/execute; Ranger da mark kullandığı için Shadowblade mark'ı yakın infaz ve faz izi olarak ayrılmalı. |
| Ranger | Hood/cape/forest green çıkarıldı; bone bow + side quiver + rift-purple war paint kilitlendi. | Mark-based Rift Stalker identity için Tolkien archer dili yetersiz ve generic. Silhouette side-quiver/bone bow ile okunur. |
| Ravager | Barbarian cliché azaltıldı; pain-as-resource scarification eklendi. | Fury hasar alarak dolar. Görsel fracture mark beden travması olmalı, büyü glow'u değil. |
| Ronin | Minimal, sheathed iaido ve cold silver cut dili korundu. | Skill kimliği draw timing/Tension. Base pose'da drawn katana olmamalı; sakinlik sınıfın aksiyon öncesi kimliği. |
| Gunslinger | Clean western ve saloon dili çıkarıldı; ritual rift-iron duelist yapıldı. | Skill kimliği Heat, cursor shots, grenade/zone. Şapka/coat kalabilir ama western cosplay değil, kırık dünya silah ritüeli olmalı. |
| Brawler | Street boxer yerine rift-tattoo rhythm fighter. | Charge/Weave/Overdrive ritim kimliği beden üstünden okunmalı. Ravager'dan çıplak gövde overlap'i gauntlet + footwork + tattoo ile ayrılır. |
| Summoner | Staff yerine lantern + skull helm ana silhouette yapıldı. | Eski skill docs kendi saldırısında staff dese de S41 görsel ayrımda Summoner'ı Hexer'dan ayıran en güçlü okuma lantern/skull/cyan. Staff eklense Hexer/Elementalist ile çakışır. Summoner'ın saldırısı lantern command / soul gesture olarak yorumlanır. |
| Hexer | Lantern çıkarıldı; staff + visible face + minimal curse script kilitlendi. | Bazı eski notlarda Hexer lantern geçiyor, ama Summoner lantern kullanınca overlap doğuyor. Hexer'ın skill kimliği stack/curse-script; staff/glyph daha doğru. Base sprite'ta renk minimal kalır; güçlü green-violet VFX skill/cast frame'e bırakılır. |

## Skill ve Ton Uyum Özeti

Genel tasarım sınırı: karakterler süper kahraman gibi parlamamalı, ama silik NPC gibi de durmamalı. Her biri normal insan temelli, yıpranmış, dungeon-survivor hissinde olmalı; farkı dev aura veya kostüm abartısı değil, silahı, zırhı, duruşu ve küçük rift izleri vermeli.

Kıyafet çeşitliliği: Bütün classlar aynı koyu robe/coat kalıbına düşmemeli. Kadın classlarda pratik açık katmanlar, sleeveless/rolled-sleeve parçalar, exposed forearms, asymmetrical wraps ve farklı silhouette kullanılabilir. Bu açıklık "combat practical" olmalı; bikini armor, lingerie armor veya superhero catsuit değil.

| Class | Skill fantezisi | Görsel karşılığı |
|---|---|---|
| Warblade | Ağır yakın dövüş, rift-charged greatsword, dayanıklılık. | Oversized greatsword, ağır ama kullanılmış plaka zırh, kontrollü cold-blue rift çatlakları; köylü değil veteran frontline knight. |
| Ranger | Tuzak/ok kontrolü, mesafe, avcı disiplini. | Hood/cape yok; bone-compound bow, side quiver, rift-purple fletching, scout harness; fantastik orman okçusu değil dungeon stalker. |
| Shadowblade | Shadow mark, phase dash, execute. | Asimetrik half-veil, hot-magenta phase edge, twin daggers; Ranger'dan daha dar, daha keskin, daha suikastçı silhouette. |
| Elementalist | Üç element döngüsü, cast kontrolü, alan büyüsü. | Tek neutral crystal orb/focus, field-scholar layers; element renkleri base orb'da değil, skill VFX/cast frame'de. |
| Ravager | Self-damage, rage, brutal bleed. | Scarified bare-muscle silhouette, cracked cleaver, iron straps; temiz şövalye değil, büyü glow'u da değil. |
| Ronin | Precision slash, parry, iaido timing. | Sheathed katana, minimal broken lamellar, silent duelist posture; Warblade'den daha hafif ve disiplinli. |
| Gunslinger | Rift-iron trick shots, marksman tempo. | Sawed-off pistol, ammo bandolier, rust-red coat; western cosplay değil cursed frontier duelist. |
| Brawler | Combo rhythm, close-range impact, body-as-weapon. | Wrapped fists, kinetic tattoo arcs, compact footwork; Ravager'dan daha teknik, daha kontrollü. |
| Summoner | Soul command, minion/lantern focus, death chorus. | Skull helm, ghost lantern, cyan soul wisps; staff yok, çünkü staff Hexer/Elementalist ile overlap yaratıyor. |
| Hexer | Curse stack, debuff script, ritual control. | Bone staff, visible face, exposed curse-script forearms, minimal dull curse ink; güçlü green-violet glyphs sadece VFX/cast tarafında. |

---

# 01 Warblade — Fractured Vanguard

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: WARBLADE — Fractured Vanguard.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through silhouette, weapon scale, battle wear, scars, and restrained rift details.

CAMERA:
High top-down ARPG gameplay camera, suitable for an isometric dungeon game but do NOT render isometric tiles or environment.
NOT eye-level, NOT portrait, NOT heroic poster.
The camera is above the character looking down.
We must clearly see the top of the head, top planes of both shoulders, top of the right pauldron, tops of boots, and a mildly foreshortened face.
The character faces SOUTH/front; hips and shoulders square to camera plane. No 3/4 body turn.

CORE IDENTITY:
A tall disciplined veteran heavy swordsman, one of RIMA's old martial faces after the Fracturing.
He is not a clean knight, not a paladin, not a barbarian, not a peasant mercenary, not a dwarf tank.
Tall human proportions, about 8 heads tall, broad and powerful but not squat.

FACE:
Short dark hair, short trimmed brown beard with a few grey strands.
Weathered tan skin, hard veteran expression.
Head upright, gaze slightly downward-forward into the battlefield, not direct portrait eye contact, not looking at the ground.
One old rift-burn scar on the right cheek, faint cold-blue/ember discoloration, not glowing.

SILHOUETTE LOCK:
Massive two-handed greatsword resting on the right shoulder.
Larger asymmetric right pauldron.
Structured rugged leather skirt armor panels.
Relaxed left fist at side.

ARMOR:
Dark cracked steel armor over fitted black-brown leather and ember-brown cloth.
The armor is premium and iconic, not messy rags.
Chest plate has worn edges, scratches, repaired leather straps, and a few controlled cracks.
Right pauldron is large, forged dark steel, with one simple geometric etched rune.
Under-armor cloth is muted ember-brown, visible in small gaps under straps and plates.

WEAPON:
Broad heavy straight greatsword, thick spine, simple heavy crossguard, leather grip.
Subtle cold-blue rift edge-light only along parts of the blade.
No big glow, no aura.

FRACTURE MARK:
2-3 thin cold-blue fracture lines in the dark steel armor, like old damage from the Fracturing.
Keep blue accent extremely restrained.

POSE:
Strict SOUTH frontal baseline.
Right hand grips the greatsword near the shoulder, sword resting over the right shoulder.
Left arm relaxed at side, left fist closed.
Feet planted shoulder-width, both feet pointing forward.
Calm, heavy, disciplined, ready for combat.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large readable shapes, limited palette, strong silhouette, no micro-detail.

NEGATIVE:
No helmet, no cape, no noble gold armor, no holy paladin look, no blue aura, no environment, no floor, no shadow, no eye-level portrait, no dwarf proportions.
```

---

# 02 Elementalist — Field Arcanist

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: ELEMENTALIST — Field Arcanist.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through silhouette, tools, field-worn clothing, subtle arcane marks, and controlled elemental detail.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, hand tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A mature female field scholar of broken primal forces, not a noble mage, not a glamorous sorceress, not a young fantasy girl.
She studies Fracturing residue and converts it into fire, frost, and radiance.

FACE:
Mid-30s, weathered light skin, honey-blonde hair in a practical low bun.
Small round spectacles pushed up on forehead.
Calm analytical expression, head upright, gaze slightly downward-forward into the battlefield.

SILHOUETTE LOCK:
One single crystal orb/focus floating above the LEFT hand at chest height.
RIGHT hand lowered near belt pouches, ready to draw reagent dust.
Layered field-scholar outfit with open practical coat and exposed forearms.
No staff.

CLOTHING:
Dusty indigo-navy open field coat over cream sleeveless tunic and dark fitted trousers.
Rolled sleeves or exposed forearms with small burn/frost/radiance study marks.
Leather belt with 2-3 large reagent pouches, fingerless gloves, worn boots.
No heavy gold, no aristocratic ornaments, no royal blue glamour, no full closed robe.

IMPLEMENT:
One single floating crystal orb/focus, no staff.
The orb is neutral clear crystal / smoky glass with only a very faint white-silver inner glint.
No fire color, no frost-blue color, no gold radiance color in the base sprite.
Element colors are reserved for separate cast/VFX frames, not the base character.
At 128px it should read as one neutral controlled focus, not a rainbow and not three separate symbols.

FRACTURE MARK:
Tiny neutral white-silver fracture veins inside the single orb only.
No separate fire/ice/light icons around the character.

POSE:
Strict SOUTH frontal baseline.
Left hand supports the single orb, right hand near reagent belt.
Feet planted, one foot half-step forward, body balanced and grounded.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large clean shapes, limited palette, robe folds simplified.

NEGATIVE:
No staff, no colored element orb in base sprite, no fire orb, no ice orb, no gold light orb, no three separate floating symbols, no separate fire/ice/light orbs, no noble queen look, no gold jewelry, no huge spell blast, no floating full aura, no environment, no eye-level portrait, no anime girl.
```

---

# 03 Shadowblade — Veil Walker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: SHADOWBLADE — Veil Walker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through silhouette, restraint, assassin gear, scars, and small phase-rift details.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A phase assassin carrying one of the lost faces of the Fracturing.
Not a generic hooded rogue, not D&D thief, not purple smoke ninja.
Lean, lethal, still.

FACE:
Mid-20s, androgynous lean face, messy black hair over forehead.
Veil mask covers lower face from nose down; eyes and forehead visible.
Eyes carry faint hot-magenta pinprick glow.
Head upright, gaze slightly downward-forward, predator focus.

SILHOUETTE LOCK:
Low predator crouch.
LEFT hand forward with curved void-dagger in normal grip.
RIGHT hand near hip with reverse-grip dagger.
Fragmented cloth strips at waist, edges pixel-shattered.

CLOTHING:
Matte black fitted void-armor over dark leather.
Fragmented waist cloth, small dagger belt shapes, dark gloves.
No cape, no hood hiding the whole face.

WEAPONS:
Twin curved matte-black void-daggers, inward curve, hot-magenta crack along inner edge.
Not bright neon swords; just controlled lethal accent.

FRACTURE MARK:
Waist cloth edges break into 3-5 large pixel-like hot-magenta fragments, as if phasing out.
This is the class mark.

POSE:
Strict SOUTH frontal baseline, torso square.
Knees bent, body lowered, left dagger forward, right dagger low at hip.
Both feet still read as frontal baseline.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large silhouette, black + hot-magenta identity, minimal micro-detail.

NEGATIVE:
No generic hooded rogue, no cold purple, no blue, no cape, no smoke cloud covering silhouette, no environment, no eye-level portrait.
```

---

# 04 Ranger — Rift Stalker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RANGER — Rift Stalker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through silhouette, practical hunting gear, hard survival details, and restrained rift marks.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A wildlands rift-stalker who hunts across fractured ruins.
Not Tolkien green hood archer, not forest elf, not cape ranger.

FACE:
Female, mid-20s, lean athletic.
Half-shaved sides, short tight braid.
Vertical rift-purple war-paint stripes under eyes.
Head upright, gaze slightly downward-forward, calm hunter focus.

SILHOUETTE LOCK:
Bone-recurve bow held vertically in LEFT hand.
RIGHT hand selecting an arrow from side-hip quiver.
Animal-bone shoulder guard on RIGHT shoulder.
No hood, no cape.

CLOTHING:
Sleeveless worn brown leather vest over bone-white chest wrap, exposed forearms, leather trousers, soft boots.
Side-hip quiver on right hip, not back quiver.
Bone trinkets kept large and readable.
Practical hunter clothing, not a full robe and not bikini armor.

WEAPON:
Bone-recurve bow carved from large animal bone, sinew string.
Thin rift-purple crackle inside the bone curve, restrained.

FRACTURE MARK:
Rift-purple war paint under eyes and one hairline crack through the bone bow.
This is her fractured hunter mark.

POSE:
Strict SOUTH frontal baseline.
Bow vertical at left side, lower tip near ground but not touching.
Right hand at side quiver touching arrow fletching.
Feet planted, one half-step forward.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large bow silhouette, side quiver readable, limited palette.

NEGATIVE:
No forest green, no hood, no cape, no back quiver, no elf fantasy glamour, no environment, no eye-level portrait.
```

---

# 05 Ravager — Bloodbound Breaker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RAVAGER — Bloodbound Breaker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through raw physical presence, scars, weapon brutality, and restraint rather than flashy magic.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A brutal melee berserker whose body turned pain into power after the Fracturing.
Not a generic barbarian, not a shirtless Viking cliché, not Brawler.

FACE:
Late 30s male, massive but not caricature.
Wild dark hair tied roughly back, full unkempt beard, scarred skin.
Expression contained rage, not roaring.
Head upright, gaze slightly downward-forward.

SILHOUETTE LOCK:
Dual brutal hand axes, one in each hand.
Each axe is heavy and cleaver-like, but not oversized beyond human use.
Fur mantle on LEFT shoulder only.
Bare scarified chest, wide stance.

CLOTHING:
Leather/fur kilt, dark fur mantle, leather wraps, crude bone fastener.
No refined armor plates except small functional bracers.

WEAPON:
Two chipped steel cleaver-axes with short thick wooden hafts wrapped in leather and bone fragments.
Both axe heads must be clearly readable at 128x128.
No hammer, no mace, no single great axe.

FRACTURE MARK:
Raised scarification across chest has faint dried crimson lines, like old wounds that never fully closed.
No magic glow; the mark is bodily trauma.

POSE:
Strict SOUTH frontal baseline.
Right hand holds one axe low at right side, blade angled outward.
Left hand holds the second axe near left hip, blade angled outward.
Fur mantle hangs from left shoulder.
Feet wide, heavy forward readiness.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Massive silhouette, clear axe/fur/chest shapes, limited palette.

NEGATIVE:
No purple, no blue, no clean armor, no dual axes, no Brawler boxing pose, no environment, no eye-level portrait.
```

---

# 06 Ronin — Severed Oath

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RONIN — Severed Oath.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through calm discipline, clean silhouette, worn gear, and restrained rift-cut details.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, hand tops at hip, boot/sandal tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A wandering draw-master who answered the Fracturing with discipline and ritual.
Not anime samurai, not noble daimyo, not Shadowblade.

FACE:
Male, mid-30s, slim wiry, clean-shaven.
Dark topknot, weathered light-tan skin, thin scar from left eye down cheek.
Calm severe expression; gaze slightly downward-forward, not direct portrait posing.

SILHOUETTE LOCK:
Katana sheathed at LEFT hip.
Both hands at left hip in iaido draw-ready position.
Single armor pauldron on RIGHT shoulder.
Wide hakama legs.

CLOTHING:
Muted dark indigo hakama, short kimono top, dark sash belt.
Simple dark lacquered scabbard, small wakizashi at belt.
Right shoulder segmented dark steel pauldron.

WEAPON:
Katana stays sheathed.
Wrapped grip and scabbard read as one clean strong line at left hip.

FRACTURE MARK:
One thin cold silver-blue line along the scabbard mouth and right pauldron edge, like a restrained cut in reality.
No aura.

POSE:
Strict SOUTH frontal baseline.
Left hand holds scabbard throat, right hand rests on grip.
Feet planted, left foot half-step forward.
Stillness before motion.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Minimal, sharp, calm silhouette, limited palette.

NEGATIVE:
No drawn katana, no anime hair, no red samurai armor, no fire, no purple, no environment, no eye-level portrait.
```

---

# 07 Gunslinger — Rift-Iron Duelist

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: GUNSLINGER — Rift-Iron Duelist.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through posture, practical weapon gear, worn coat shapes, and restrained rift-metal details.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A ritualistic rift-world duelist, not saloon cowgirl, not clean western hero, not fantasy pirate.
Dangerous calm, precise, worn.

FACE:
Female, mid-20s, deep auburn braid, weathered light skin with freckles.
No hat. Eyes focused slightly downward-forward, visible and sharp.
Expression calm and lethal.

SILHOUETTE LOCK:
No hat, no hood, no helmet.
Short asymmetrical leather half-coat, open front, high collar.
RIGHT pistol held barrel-down at right side.
LEFT hand touching partially-holstered pistol at left hip.
Auburn braid and high leather collar replace the hat silhouette.

CLOTHING:
Dark short leather half-coat over fitted charcoal shirt, dark trousers, leather gun belt with dual thigh holsters.
Rolled sleeves or exposed forearms with small powder burns and tool scars.
Dusty red-brown neck scarf, brass buckle, small bone charm tied to the gun belt.
No puffy sleeves, no long full coat.

WEAPONS:
Twin revolvers, dark steel with brass cylinders and trigger guards.
No magical hand glow.

FRACTURE MARK:
Tiny cold-silver etching inside revolver barrels and one subtle rift scratch on the brass buckle.
It reads as weapon ritual, not magic aura.

POSE:
Strict SOUTH frontal baseline.
Right hand holds pistol down, left hand rests on left holster grip.
Feet planted in duelist stance, right foot slightly back but toes forward.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Short half-coat, pistols, auburn braid, high collar, and dual holsters must be readable.

NEGATIVE:
No hat, no cowboy hat, no wide-brim hat, no saloon outfit, no puffy sleeves, no long full coat, no cape, no purple glow, no blue aura, no environment, no eye-level portrait.
```

---

# 08 Brawler — Rift-Fist Contender

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: BRAWLER — Rift-Fist Contender.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through compact fighting stance, wraps, bruises, tattoos, and controlled kinetic marks.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, fist/gauntlet tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A disciplined close-combat fighter who carved Fracturing force into his body rhythm.
Not Ravager, not monk, not street thug, not boxer stereotype.

FACE:
Male, mid-20s, tall lean-muscular, short black hair.
Bronze skin, clean-shaven, focused controlled aggression.
Gaze slightly downward-forward over raised guard.

SILHOUETTE LOCK:
Bare chest with bold arcane tattoos.
Bone-metal gauntlets on both hands.
LEFT fist high at chin guard, RIGHT fist chambered at sternum.
Boxing footwork stance.

CLOTHING:
Loose dark charcoal trousers, leather wrist/ankle wraps, minimal footwear.
No torso armor.

WEAPONS:
No weapons except gauntlets.
Gauntlets are back-of-hand and knuckle armor, bone-white + dark steel, not full gloves.

FRACTURE MARK:
Arcane purple tattoo lines across chest, shoulders, and forearms.
Glow is subtle, like ink under skin, not neon aura.

POSE:
Strict SOUTH frontal baseline.
No visible torso rotation; asymmetry comes from guard arms only.
Left foot forward, right foot back, both feet readable.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large fists/gauntlets, bare chest, tattoos readable.

NEGATIVE:
No weapon, no plate armor, no barbarian fur, no massive axe, no bright purple aura, no environment, no eye-level portrait.
```

---

# 09 Summoner — Grave Chorus

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: SUMMONER — Grave Chorus.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
They should look cool through ritual silhouette, lantern focus, death-worn gear, and restrained soul light.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of skull helm, shoulder tops, lantern top, boot/robe hem perspective, and mild face foreshortening must be visible.

CORE IDENTITY:
A death commander who gathers lost echoes after the Fracturing.
Not generic necromancer, not Hexer, not green curse mage.

FACE:
Tall slim figure, age ambiguous.
Skeleton-mask helm covers face, bleached bone-white, cyan eye sockets.
Hood pushed partly back so skull helm is visible.
Head upright, gaze slightly downward-forward through cyan sockets.

SILHOUETTE LOCK:
Soul lantern in LEFT hand at shoulder height.
RIGHT hand lowered in palm-down command gesture.
Skeleton-mask helm.
Tall dark robe mass with bone belt fetishes.

CLOTHING:
Void-black tattered robe, hood pushed back.
Leather belt with 3-4 readable bone fetishes.
Dark boots barely visible below hem.

IMPLEMENT:
Iron cage soul lantern with cyan-blue-white flame.
Lantern is the authority symbol.

FRACTURE MARK:
Three tiny cyan echo wisps near lantern and skull eye sockets.
No green, no violet.

POSE:
Strict SOUTH frontal baseline.
Left lantern shoulder-high, right command hand low.
Feet/robe hem frontal, body square.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Skull helm, cyan lantern, robe silhouette readable.

NEGATIVE:
No green, no purple, no curse staff, no hood hiding skull helm, no orange flame, no environment, no eye-level portrait.
```

---

# 10 Hexer — Curse-Script Oracle

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: HEXER — Curse-Script Oracle.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through posture, curse tools, worn robes, script details, and restrained curse ink.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of hood/head, shoulder tops, staff orb top, boot/robe hem perspective, and mild face foreshortening must be visible.

CORE IDENTITY:
A living curse-binder who reads the Fracturing as a written disease.
Not Summoner, not skull mask, not generic witch.

FACE:
Female, late-20s, slim, pale sickly skin.
No skull mask, no face mask.
Hood up but face fully visible; dark hair visible under hood.
Sharp eyes with dual green-violet glint.
Head upright, gaze slightly downward-forward, predatory focus.

SILHOUETTE LOCK:
Curse staff in RIGHT hand, vertical.
LEFT hand raised at chest height in curse-weave gesture.
Short hooded mantle over asymmetrical ritual tunic, not a full heavy robe.
Face visible, no mask, exposed curse-script forearms.

CLOTHING:
Deep void-black hooded mantle with split asymmetrical ritual tunic over dark trousers.
Exposed forearms covered in readable dull charcoal curse script tattoos with only a tiny muted green-violet hint.
Large readable geometric sigil embroidery on hood border, cuffs, and hem.
Wide dark sash belt.
No skull mask, no skeleton face, no full closed robe.

IMPLEMENT:
Twisted dark wooden staff, right hand.
Small dull bone/black-glass focus at top, mostly unlit.
Strong green-violet energy is reserved for separate cast/VFX frames, not the base character.

FRACTURE MARK:
2-3 tiny dull curse glyph marks between left fingers, barely glowing.
A few matching glyphs embroidered on robe edges.
No cyan, no strong green-violet aura in the base sprite.

POSE:
Strict SOUTH frontal baseline.
Right staff vertical near right foot, left curse hand at chest height.
Right foot half-step forward, body square.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Staff, visible face, hood silhouette, exposed curse-script forearms, and muted curse marks readable.

NEGATIVE:
No skull mask, no face mask, no skeleton helm, no lantern, no cyan, no strong green-violet aura in base sprite, no green-only palette, no full closed robe, no environment, no eye-level portrait.
```
