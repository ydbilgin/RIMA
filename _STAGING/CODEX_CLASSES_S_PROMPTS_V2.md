# Codex Classes — S Direction Character Prompts V2
> RIMA | 2026-04-25 | v1 generic fallback düzeltildi — pozitif RIMA spec eklendi, class identity kilitleri güçlendirildi.
> v1 referans: `_STAGING/CODEX_CLASSES_S_PROMPTS.md` (değiştirme, korun)

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

## Original RIMA Identity Lock

Bu prompt pack herhangi bir oyunun, anime'nin, filmin veya mevcut karakter tasarımının kopyası gibi görünmemeli. Referans görseller sadece kamera, ölçek ve genel karanlık dünya tonu içindir; kostüm, silah, yüz, renk düzeni veya stil kopyalanmaz.

RIMA'nın özgün karakter dili:

- Fracturing sonrası hayatta kalmış normal insanlar: yıpranmış, işlevsel, sahada tamir edilmiş.
- Güç kaynağı dev aura değil; yara, ekipman, ritüel iz, asymmetry ve küçük rift kalıntıları.
- Her class tek net silhouette imzası taşır; süper kahraman kostümü, boss enerjisi veya generic NPC yok.
- Base sprite aktif skill kullanmıyor gibi durmalı; güçlü element/curse/rift renkleri VFX ve cast frame tarafına bırakılır.
- Tasarım pratik combat fantasy olmalı: özgün, karanlık, okunur, ama başka bir IP'yi çağrıştıracak kadar spesifik değil.

## Karar Notları — Hikaye + Skill Tutarlılığı

Bu dosya eski class promptlarını birebir kopyalamaz. RIMA'nın hikaye tonu, skill kimliği ve 128px silhouette ihtiyacına göre bazı kararları bilinçli değiştirdi.

| Class | Karar | Neden değişti |
|---|---|---|
| Warblade | Cold-blue tamamen yasak değil; çok sınırlı rift accent olarak geri alındı. | RIMA dünyasının ana fracture dili cold-blue. Tamamen mavi yasaklayınca Warblade generic brown/steel warrior'a kayıyor. Ancak skill/VFX tarafındaki mavi spam riskinden dolayı blue sadece sword fuller + 2-3 armor crack ile sınırlı. |
| Elementalist | Staff çıkarıldı; neutral crystal orb + field scholar yapıldı, noble mage dili çıkarıldı. | Skill kimliği fire/frost/radiance ritmi ama base sprite tek elemente boyanmamalı. Element renkleri cast/VFX layer'da yaşayacak; base orb renksiz veya çok hafif nötr parıltılı kalacak. |
| Shadowblade | Generic hooded rogue yerine veil mask + hot-magenta phase scar. | Skill kimliği phase/mark/execute; Ranger da mark kullandığı için Shadowblade mark'ı yakın infaz ve faz izi olarak ayrılmalı. |
| Ranger | Hood/cape/forest green çıkarıldı; bone bow + side quiver + rift-purple war paint kilitlendi. | Mark-based Rift Stalker identity için klasik orman okçusu dili yetersiz ve generic. Silhouette side-quiver/bone bow ile okunur. |
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
He should look cool through silhouette, weapon scale, battle-worn field repairs, and restrained rift details.
Visual tone: field-repaired survival gear, veteran heavy-sword weight, disciplined high-contrast readability — not a generic knight and not a copy of any existing game character.

CAMERA:
High top-down ARPG gameplay camera, suitable for an isometric dungeon game but do NOT render isometric tiles or environment.
NOT eye-level, NOT portrait, NOT heroic poster.
The camera is above the character looking down.
We must clearly see the top of the head, top planes of both shoulders, top of the right pauldron, tops of boots, and a mildly foreshortened face.
The character faces SOUTH/front; hips and shoulders square to camera plane. No 3/4 body turn.

CORE IDENTITY:
A tall disciplined veteran heavy swordsman, one of RIMA's surviving Vanguard after the Fracturing.
He is not a clean knight, not a paladin, not a barbarian, not a peasant mercenary, not a dwarf tank.
Tall human proportions, about 8 heads tall, broad and powerful but not squat.

RIMA NARRATIVE ANCHOR:
He survived the first Vanguard collapse during the Fracturing; his chest plate took a direct rift hit and was sealed mid-battle with iron wire stitching rather than proper repair — the crack is still visible, threaded shut with crude field work.
He wears two bone-and-iron oath tags from fallen squadmates, hung on a short chain around the neck — not decorative, not ceremonial, just worn and present.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the split battle-tabard: outer layer heavy weathered canvas with a half-faded Vanguard unit sigil, inner layer showing ember-orange cloth flash at the split center — this is the visual identifier that sets him apart from a generic plate knight.

FACE:
Short dark hair, short trimmed brown beard with a few grey strands.
Weathered tan skin, hard veteran expression.
Head upright, gaze slightly downward-forward into the battlefield, not direct portrait eye contact, not looking at the ground.
Right cheek: one rift-burn scar approximately 1.5cm long running diagonal, with faint cold-blue discoloration along the scar line — not glowing, just a mark left by rift energy.

SILHOUETTE LOCK:
Massive two-handed greatsword resting on the right shoulder.
Large asymmetric right pauldron (dark forged steel, one etched rune).
Split battle-tabard with ember-orange inner flash at the center split.
Relaxed left fist at side, left forearm rolled sleeve showing battle tattoo and cold-blue rift-burn scar.
Right forearm armored in gauntlet.
Short bone-and-iron tag chain at neck.

ARMOR:
Dark cracked steel chest plate with visible rift crack sealed by crude iron wire stitching — the crack runs diagonal across the center, threaded shut rather than repaired.
Cold-blue rift residue visible only inside the crack line, 1-2mm wide, not spreading outward.
Right pauldron: large dark forged steel, one simple geometric etched rune, premium and iconic.
Left shoulder: heavy cloth strap layer plus a short bone-token chain — asymmetry with the right pauldron is intentional.
Under-armor: muted ember-brown cloth, visible in small gaps.
Boot tops: upper plate section with ad-hoc leather strap field repair — uneven but functional.

WEAPON:
Broad heavy forged steel greatsword, straight edges, visible simple heavy crossguard, tapered point, leather grip, resting over right shoulder.
Fuller (the groove running down the blade center) contains a thin line of cold-blue rift residue — the same color as his chest crack, connecting the visual language.
No stone slab blade, no club silhouette, no paddle shape, no big glow, no aura, no fire.

FRACTURE MARK:
Cold-blue accent appears in exactly three places: chest crack interior (sealed by iron wire), greatsword fuller line, and right cheek scar discoloration.
Keep blue accent extremely restrained — thin line, not spreading, not glowing.

POSE:
Strict SOUTH frontal baseline.
Right hand grips the greatsword near the shoulder, sword resting over the right shoulder.
Left arm relaxed at side, left fist closed, forearm visible with rolled sleeve.
Feet planted shoulder-width, both feet pointing forward.
Calm, heavy, disciplined, ready for combat.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large readable shapes, limited palette, strong silhouette, no micro-detail.

NEGATIVE:
No helmet, no cape, no noble gold armor, no holy paladin look, no blue aura spreading from body, no environment, no floor, no shadow, no eye-level portrait, no dwarf proportions, no full symmetrical pauldrons.
```

---

# 02 Elementalist — Field Arcanist

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: ELEMENTALIST — Field Arcanist.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through silhouette, field-worn asymmetric layering, visible study scars, and a single controlled arcane focus.
Visual tone: a practitioner who works with broken primal forces in dungeon conditions, not a university archmage.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, hand tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A mature female field scholar of broken primal forces, not a noble mage, not a glamorous sorceress, not a young fantasy girl.
She has spent years studying Fracturing residue in the field, converting raw rift energy into fire, frost, and radiance through a single focus crystal.

RIMA NARRATIVE ANCHOR:
She mapped fire, frost, and radiance rift signatures before most scholars admitted they were different phenomena — her forearms carry the proof: left forearm has small irregular fire-burn study marks from early ignition tests, right forearm has pale frost-bite discoloration patches from cold-resonance exposure, both self-inflicted through careful experimentation rather than combat.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the asymmetric layering: sleeveless left arm (fire-burn marks visible on skin) contrasted with rolled right sleeve exposing frost-bite marks, above a leather belt loaded with reagent pouches — the exposed arms mark her as a practitioner, not a robed caster.

FACE:
Mid-30s, weathered light skin, warm honey-blonde hair in a practical loose bun with a few strands framing the face.
Hair must read clearly blonde/golden at thumbnail scale — not black, not brown, not dark auburn.
No eyewear, no spectacles, no goggles — clear forehead, hair framing only.
Calm analytical expression, head upright, gaze slightly downward-forward into the battlefield.

CLOTHING:
Combat-sexy asymmetric field outfit: wrapped cream deep-V sleeveless combat tunic with dusty indigo-navy trim — left arm fully exposed, right sleeve rolled up to elbow.
Fitted cropped indigo open vest over the tunic, practical field cut, not decorative.
Exposed collarbone, shoulders, and forearms; high-waist belt harness with reagent pouches; fitted dark trousers and practical boots.
She should read as athletic, confident, elegant, and battle-ready through fit and posture, never lingerie armor.
Dark fitted trousers, worn leather boots.
Leather belt with 2-3 large reagent pouches and a small tool roll.
Fingerless gloves on right hand only.
No heavy gold, no aristocratic ornaments, no royal blue glamour, no full closed robe.

BODILY MARKS:
Left forearm: 4-5 small irregular reddish-brown fire-burn study marks scattered from wrist to mid-forearm, like old scald spots — not glowing, just healed tissue.
Right forearm: pale frost-bite discoloration in 2-3 coin-sized pale blue-white patches mid-forearm — flat against skin, not glowing.

SILHOUETTE LOCK:
One single crystal orb/focus floating above the LEFT hand at chest height.
RIGHT hand lowered near belt pouches, ready to draw reagent dust.
Asymmetric sleeve exposure is the silhouette differentiator — her arms are readable, not hidden.
No staff.

IMPLEMENT:
One single floating crystal orb/focus, no staff.
The orb is neutral clear crystal or smoky glass with only a very faint white-silver inner glint.
No fire color, no frost-blue color, no gold radiance color in the base sprite.
Element colors are reserved for separate cast/VFX frames, not the base character.
At 128px it should read as one neutral controlled focus, not a rainbow and not three separate symbols.

FRACTURE MARK:
Tiny neutral white-silver fracture veins inside the single orb only.
No separate fire/ice/light icons around the character.

POSE:
Strict SOUTH frontal baseline.
Left hand supports the single neutral orb, right hand near reagent belt.
Feet planted, one foot half-step forward, body balanced and grounded.
No direct portrait eye contact; she looks battlefield-forward like the accepted Warblade camera.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large clean shapes, limited palette, asymmetric sleeve exposure readable at thumbnail scale.

NEGATIVE:
No black hair, no brown hair, no staff, no colored element orb in base sprite, no fire orb, no ice orb, no gold light orb, no three separate floating symbols, no noble queen look, no gold jewelry, no huge spell blast, no floating full aura, no environment, no eye-level portrait, no anime girl, no full closed robe, no symmetrical sleeves.
```

---

# 03 Shadowblade — Veil Walker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: SHADOWBLADE — Veil Walker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through silhouette restraint, asymmetric cloth damage, phase scars, and small lethal detail.
Visual tone: a practitioner who moves through rift-shadow, not a stage magician or a purple-smoke ninja.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A phase assassin who uses rift-shadow corridors to close distance and execute.
Not a generic hooded rogue, not a tabletop thief archetype, not purple smoke ninja.
Lean, lethal, still.

RIMA NARRATIVE ANCHOR:
He phased through a collapsing rift corridor during the Fracturing and emerged with permanent phase residue embedded in his body — the neck carries an old pale phase scar where a rift edge nearly intersected him, and the waist cloth shows permanent cloth-phase damage where reality partially overlapped during transit.
He wraps his left forearm deliberately to cover a phase-burn, asymmetric cloth armor as both protection and habit.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the waist cloth with large angular pixel-chunk hot-magenta phase fragments at the edges — the cloth appears to be mid-phase, partially dissolving into rift space, which is his class mark and no other class shares this visual language.

FACE:
Mid-20s, androgynous lean face, messy black hair over forehead.
Veil mask covers lower face from nose down; eyes and forehead visible.
Eyes carry faint hot-magenta pinprick glow — the only color on his face.
Head upright, gaze slightly downward-forward, predator focus.
Old phase scar visible at neck, just above the veil mask edge — a pale hairline discoloration running 2cm horizontal.

SILHOUETTE LOCK:
Low predator crouch, body weight forward.
LEFT hand forward with curved void-dagger in normal grip.
RIGHT hand near hip with reverse-grip dagger.
Fragmented waist cloth with large angular hot-magenta pixel chunks at edges.
LEFT forearm wrapped in dark cloth (asymmetry — right forearm bare dark leather).

CLOTHING:
Matte black fitted void-armor over dark leather.
Left forearm dark cloth wrap, right forearm bare leather — intentional asymmetry.
Fragmented waist cloth, small dagger belt shapes, dark fitted gloves.
Leather belt with one hot-magenta phase glyph embossed on the buckle face — a single readable shape, not decorative pattern.
No cape, no hood hiding the whole face.

WEAPONS:
Twin curved matte-black void-daggers, inward curve, hot-magenta crack along inner edge of each blade.
Not bright neon swords; controlled lethal accent — thin line inside the curve only.

FRACTURE MARK:
Hot-magenta appears in exactly three places: waist cloth edge fragments (large angular pixel chunks, 3-5 of them), belt buckle glyph (single embossed shape), dagger inner edge cracks (thin line).
No spreading purple smoke, no aura field.

CLASS COLOR LOCATION:
Hot-magenta accent — waist cloth edge fragments are the largest readable instance; belt buckle glyph is secondary; dagger edge crack is tertiary. Total hot-magenta coverage should read as restrained at 128px, not dominant.

POSE:
Strict SOUTH frontal baseline, torso square.
Knees bent, body lowered, left dagger forward, right dagger low at hip.
Both feet still read as frontal baseline.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Large silhouette, black + hot-magenta identity, minimal micro-detail, phase-fragment cloth readable.

NEGATIVE:
No generic hooded rogue, no cold purple, no blue, no cape, no smoke cloud covering silhouette, no environment, no eye-level portrait, no symmetrical forearm coverage.
```

---

# 04 Ranger — Rift Stalker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RANGER — Rift Stalker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through practical hunting gear, bone materials, hard survival details, and restrained rift marks.
Visual tone: a tracker who operates in fractured dungeon ruins, not a forest guardian or nature spirit.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A wildlands rift-stalker who hunts marked targets across fractured ruins using bone-weapons and rift-tracking.
A dungeon predator who reads rift trails and marks prey before closing in — disciplined, quiet, precise.

RIMA NARRATIVE ANCHOR:
She tracked rift-creature migration patterns before the Fracturing stabilized, learning to read rift-purple energy trails as hunting markers — the war paint under her eyes is a functional adaptation from this work, replicating the visual pattern of rift-purple trail signatures to confuse rift-sensitive creatures.
Her right shoulder bone guard was carved from the first large rift-creature she brought down solo; she keeps it as proof of method, not trophy.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the combination of bone-recurve bow vertical in left hand plus visible side-hip utility gear: quiver, trap canister, and tether spool. This reads as a tactical rift hunter, not a tribal barbarian archer.

FACE:
Female, mid-20s, lean athletic.
Half-shaved sides, short tight braid.
No tribal face paint. One thin cold-blue rift-tracker mark under the right eye only, restrained.
Head upright, gaze slightly downward-forward, calm hunter focus.

SILHOUETTE LOCK:
Bone-recurve bow held vertically in LEFT hand, lower tip near ground.
RIGHT hand selecting an arrow from side-hip quiver.
Small carved animal-bone shoulder guard on RIGHT shoulder only — no skull pauldron, no oversized barbarian shoulder.
Side-hip quiver on right hip, wide leather strap, 4-5 fletched arrows visible.
Trap canister belt and visible tether spool at left hip.
No hood, no cape.

CLOTHING:
Tactical dungeon-stalker clothing: sleeveless worn brown leather vest over bone-white chest wrap, exposed forearms, leather trousers, soft leather boots.
Midriff: narrow leather strap protector across the lower ribs — functional, combat-practical, not a gap exposing bare stomach.
Side-hip quiver leather strap is thick and prominent — a structural element, not a thin cord.
Bone trinket charms on 2-3 leather thongs at belt, each large enough to read at 128px.
Trap canister and tether spool must read as utility gear. Practical hunter clothing: no full robe, no bikini armor, no tribal barbarian outfit.

WEAPON:
Bone-recurve bow carved from large animal bone, sinew string, sinew-bound grip.
Thin cold-blue rift line inside the bone curve along the inner arc — restrained, one thin line.

FRACTURE MARK:
Cold-blue appears in exactly three places: one thin tracker mark under right eye, hairline crack through the bone bow inner arc, and one small arrow-tip glint in the side quiver.
No purple, no spreading cloud, no aura.

CLASS COLOR LOCATION:
Cold-blue accent — eye tracker mark is the face-level read; bow crack is the weapon-level read; arrow-tip glint is the quiver read. All thin, all restrained.

POSE:
Strict SOUTH frontal baseline.
Bow vertical at left side, lower tip near ground but not touching.
Right hand at side quiver, fingers touching arrow fletching.
Feet planted, one half-step forward.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Bone bow vertical line, side quiver, trap canister, and tether spool must all read clearly.

NEGATIVE:
No purple, no tribal face paint, no forest green, no hood, no cape, no back quiver, no elf fantasy glamour, no oversized skull shoulder, no barbarian outfit, no exposed bare midriff, no environment, no eye-level portrait, no left shoulder guard.
```

---

# 05 Ravager — Bloodbound Breaker

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RAVAGER — Bloodbound Breaker.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through raw physical presence, ritual scarification, brutal weapon silhouette, and zero magic glow.
Visual tone: pain converted to fuel, not wild animal rage — contained and deliberate.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A brutal melee berserker whose body turned pain into power after the Fracturing — fury charges as he takes damage, not through any external power source.
Not a generic barbarian, not a shirtless raider cliché, not a Brawler.
His body is the proof of his method: every scar is a record.

RIMA NARRATIVE ANCHOR:
He discovered during the Fracturing that rift-wound pain accelerated his body's recovery rate and temporarily amplified force output — he then ritually mapped this finding onto his chest as a single large geometric rune, carved and allowed to scar deliberately, as a reminder that suffering is the resource.
The belt trophies are fragments of enemy bone collected as waypoints: each one marks a fight where he took the first hit intentionally to activate his fury.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the bare scarified chest with one large geometric rune scar at center — not random scratches, one deliberate large shape — contrasted with the fur mantle on the left shoulder and two outward-facing cleaver axes.

FACE:
Late 30s male, massive but not caricature.
Wild dark hair tied roughly back with a leather strap, full unkempt beard, scarred skin.
Expression: contained rage, jaw set, not roaring open-mouthed.
Head upright, gaze slightly downward-forward.
One diagonal scar across the bridge of the nose from a blade catch — raised scar tissue, skin-tone, not bleeding.

SILHOUETTE LOCK:
Bare scarified chest with one large geometric rune scar at sternum.
Fur mantle on LEFT shoulder only — heavy, natural color, hanging over left arm.
Dual cleaver-axes, one in each hand, blades angled outward.
Belt with 4-5 large bone trophy fragments strung on leather cord — each fragment is a readable distinct shape, not a decorative band.
Wide stance.

CLOTHING:
Leather/fur kilt, dark fur mantle left shoulder only, leather straps across torso holding the kilt.
Crude bone clasps — large, readable.
No refined armor plates, except small functional bracers on forearms.

WEAPON:
Two chipped steel cleaver-axes with short thick wooden hafts wrapped in leather and bone fragment bindings.
Both axe heads must be clearly readable at 128px — matched brutal metal hand-axes with clear short handles and heavy wide single-bevel cleaver profile.
No giant stone slab axes, no fantasy paddles, no hammer, no mace, no single great axe.

FRACTURE MARK:
The large geometric rune scar on chest has faint dried deep crimson lines in the carved channels — like old wounds that were deliberately opened more than once and never fully closed.
No magic glow, no fire emanation, no aura. The mark is bodily trauma and ritual record.

CLASS COLOR LOCATION:
Deep crimson appears only inside the carved rune channels on chest — thin line of dried color, not spreading, not glowing.

POSE:
Strict SOUTH frontal baseline.
Right hand holds one axe low at right side, blade angled outward.
Left hand holds second axe near left hip, blade angled outward.
Fur mantle hangs from left shoulder.
Feet wide, heavy forward readiness.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Massive bare-chest silhouette, readable rune scar, cleaver axes, bone trophy belt all readable.

NEGATIVE:
No purple, no blue, no clean armor, no magic glow, no Brawler boxing pose, no random scratch pattern (rune must be one large geometric shape), no environment, no eye-level portrait.
```

---

# 06 Ronin — Severed Oath

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: RONIN — Severed Oath.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through calm discipline, sharp silhouette asymmetry, worn gear, and restrained rift-cut details.
Visual tone: the stillness before the draw — grounded, restrained, and not a stylized action poster.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, hand tops at hip, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A wandering draw-master who responded to the Fracturing by converting its chaos into a personal timing discipline.
Not exaggerated samurai fantasy, not noble warlord, not Shadowblade.
His armor is broken but his method is not.

RIMA NARRATIVE ANCHOR:
He severed his lord's oath after his lord's keep collapsed in the first rift wave — not from cowardice but because the oath structure itself cracked and there was nothing left to serve.
He carries the broken lamellar plate on his right shoulder because he cannot bring himself to fully discard it, and wraps the left shoulder in cloth because the paired plate is simply gone.
A silver hairline crack runs along the scabbard surface — a rift cut from when he drew through a collapsing rift boundary; the blade survived, the oath did not.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the Severed Oath silhouette: right shoulder broken lamellar plate remnant versus left shoulder bare cloth wrap, plus a pale oath-cord knot hanging from the sash and a cracked scabbard. He should read as a man carrying a broken oath, not a generic ronin.

FACE:
Male, mid-30s, slim wiry, clean-shaven.
Dark topknot, weathered light-tan skin.
Thin scar from left eye corner down left cheek — one clean old blade cut, pale on skin.
Calm severe expression; gaze slightly downward-forward, not direct portrait posing.

SILHOUETTE LOCK:
Katana sheathed at LEFT hip.
Both hands at left hip in iaido draw-ready position (left hand holding scabbard throat, right hand resting on grip).
RIGHT shoulder: broken lamellar plate remnant — segmented dark steel, noticeably damaged, gaps visible.
LEFT shoulder: dark cloth wrap — no plate, intentional absence.
Wide hakama legs.

CLOTHING:
Muted dark indigo split hakama pants with field straps, short sleeveless dark wrap top, dark sash belt.
Exposed forearms with wrist wraps; one pale severed-oath cord tied at waist as a simple readable identity mark.
Add one small broken clan seal cloth tag hanging from the sash, pale grey, cracked with a thin silver line.
Right shoulder broken lamellar plate — only surviving piece of what was once matched armor.
Left shoulder cloth wrap over the missing plate side.
Dark lacquered scabbard with a silver hairline crack running along the surface — one thin line, not decorative, a rift scar on the sheath itself.
Small wakizashi at belt.
Boots: leather wrap with iron-reinforced sole plate — not sandals, field-adapted footwear.

WEAPON:
Katana stays sheathed.
Wrapped grip and cracked scabbard read as one clean strong line at left hip.

FRACTURE MARK:
Silver hairline crack along the scabbard surface — thin single line, the same cold silver that appears on the broken right pauldron edge.
No aura, no glow.

CLASS COLOR LOCATION:
Cold silver appears in three places: hairline crack along scabbard surface (one thin line), broken edge of the right lamellar plate, and cracked line through the small oath tag. No other color accent.

POSE:
Strict SOUTH frontal baseline.
Left hand holds scabbard throat, right hand rests on grip.
Feet planted, left foot half-step forward.
Stillness before motion.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Minimal, sharp silhouette. Asymmetric shoulder read must be clear at thumbnail scale.

NEGATIVE:
No generic samurai cosplay, no clean ceremonial robe, no drawn katana, no exaggerated spiky hair, no red ceremonial armor, no fire, no purple, no matching pauldrons, no sandals, no environment, no eye-level portrait.
```

---

# 07 Gunslinger — Rift-Iron Duelist

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: GUNSLINGER — Rift-Iron Duelist.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through posture, asymmetric coat silhouette, weapon ritual, and worn frontier precision.
Visual tone: a cursed-world duelist who has reduced every action to economic habit — not theatrical, not romantic.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A ritualistic rift-world duelist who converted guncraft into a discipline of Heat economy and precise placement.
Not saloon gunslinger, not clean frontier hero, not swashbuckler, not gadget inventor.
Dangerous calm, precise, worn — every piece of kit has a specific function.

RIMA NARRATIVE ANCHOR:
She kept a kill-count tattoo system on her right forearm started after the Fracturing — each mark is a small geometric hash specific to the weapon load she used, a personal accounting system for what works.
The diagonal chest bandolier is functional, not decorative: she pre-loaded specific powder charges into each slot for different shot types before every run, because reloading under pressure is the mistake she refuses to repeat.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the dark burgundy short asymmetric half-coat with fitted waist, open front showing the diagonal bandolier and dual holsters beneath — the coat asymmetry plus auburn braid replace any hat silhouette.

FACE:
Female, mid-20s, deep auburn hair in a long tight braid over one shoulder.
Weathered light skin with freckles, small powder-burn mark at left jaw line — one small dark spot from a near-miss, not decorative.
No hat. Eyes focused slightly downward-forward, visible and sharp.
Expression: calm, lethal, economical.

SILHOUETTE LOCK:
Dark burgundy short asymmetric half-coat, open front, high collar, fitted waist.
Diagonal bandolier chest-strap from left shoulder to right hip — wide leather strap with 6-8 readable charge loops.
RIGHT pistol held barrel-down at right side.
LEFT pistol held low at left side.
Auburn braid over one shoulder, no hat.
High collar on the coat.

CLOTHING:
Combat-sexy duelist outfit: dark burgundy short asymmetric half-coat over fitted black sleeveless leather vest, dark trousers, tight gun belt with dual thigh holsters.
RIGHT forearm: exposed, rolled sleeve or half-sleeve — forearm shows 4-6 small geometric hash-mark tattoos in dark ink (kill accounting marks, not tribal swirls).
LEFT forearm: partial sleeve, small powder-burn scorch marks in 2-3 places on skin — flat dark marks, not glowing.
Exposed forearms, visible waist/holster line, practical heeled combat boots.
Stylish and feminine through fitted waist, braid, holsters, and confident duelist posture; practical and dangerous, not lingerie armor.
No puffy sleeves, no long brown duster, no long symmetrical full coat, no asymmetric coat with identical hem lengths.

WEAPONS:
Twin revolvers, dark iron-black metal with minimal brass cylinder visible.
No magical hand glow, no neon rift energy on weapons.
One faint cold-silver etching along the barrel of the right pistol — one thin geometric line, weapon ritual mark.

FRACTURE MARK:
Cold-silver etch on right pistol barrel only — one thin line, ritual mark.
Powder-burn marks on left forearm skin — flat, dark, non-glowing.

CLASS COLOR LOCATION:
Rust-red appears in the neck scarf — one accent piece. Cold-silver appears only on the right pistol barrel etch — one thin line.

POSE:
Strict SOUTH frontal baseline.
Right hand holds pistol barrel-down, left hand holds second pistol low at left side.
Feet planted in duelist stance, right foot slightly back but toes forward.
Coat open, bandolier visible at chest.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Short burgundy half-coat, diagonal bandolier, auburn braid, two pistols, and dual holsters must all be readable at thumbnail scale.

NEGATIVE:
No hat, no cowboy hat, no wide-brim hat, no saloon outfit, no puffy sleeves, no long brown duster, no full symmetrical coat, no cape, no purple glow, no blue aura, no neon weapons, no environment, no eye-level portrait.
```

---

# 08 Brawler — Rift-Fist Contender

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: BRAWLER — Rift-Fist Contender.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
He should look cool through compact fighting stance, precise tattoo geometry, gauntlet weight, and kinetic readiness.
Visual tone: a fighter who has internalized Fracturing rhythm into his body discipline — not rage, not mysticism, pure timing.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of head, shoulder tops, fist/gauntlet tops, boot tops, and mild face foreshortening must be visible.

CORE IDENTITY:
A disciplined close-combat rhythm fighter who carved Fracturing force into his body as geometric kinetic script.
Not Ravager (no rage, no random scars), not monk (no spiritual posture), not street thug, not boxer stereotype.
His body is a calibrated instrument, not a scarred survivor.

RIMA NARRATIVE ANCHOR:
He discovered that the Fracturing produced rhythmic force pulses in the rift field that matched human combat timing — he spent months mapping these pulse patterns onto his body as geometric flowing rune lines, not as magic, but as a memory system for the rhythm.
The broken nose ridge is from the one fight where he got the timing wrong and took the hit flush — he kept it unset as a reminder that the rhythm is not automatic.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the bare chest with geometric flowing rune tattoos running from forearms up through chest and shoulders — not random tribal shapes, but continuous connected line-work that reads as one flowing geometric script — paired with the bone-and-steel knuckle gauntlets raised in active guard.

FACE:
Male, mid-20s, tall lean-muscular, short black hair, close-cropped.
Bronze skin, clean-shaven.
Broken nose: nose bridge has a slight leftward deviation and a healed break ridge — not dramatic, just a subtle read that says it was not set after breaking.
Focused controlled aggression, gaze slightly downward-forward over raised guard.

SILHOUETTE LOCK:
Bare chest with geometric flowing rune tattoo system — continuous connected line-work, not scattered tribal dots.
Bone-and-steel knuckle gauntlets on both hands — back-of-hand plate and knuckle strip only, not full boxing gloves.
Both fists held in a compact guard close to the torso; LEFT fist slightly higher, RIGHT fist near sternum.
Strapped fighting sandals — leather strap-wrapped ankle and foot with flat reinforced sole, not barefoot, not boots.
Square SOUTH baseline fighting stance, not a lunging attack pose.

CLOTHING:
Loose dark charcoal trousers, leather ankle wraps, strapped fighting sandals.
Minimal leather wrist wrap at both wrists beneath the gauntlets.
No torso armor, no shirt.

WEAPONS:
No weapons except gauntlets.
Gauntlets are back-of-hand plate and knuckle armor only — bone-white outer shell with dark steel knuckle face, not full gloves, not boxing wraps covering the whole hand.

FRACTURE MARK:
Geometric flowing rune tattoo lines across chest, shoulders, and forearms — one connected system, like flowing angular script, NOT random tribal patterns, NOT scattered dots.
Tattoo ink is deep charcoal-purple, flat against skin — like ink under skin, not neon aura, not glowing, not raised.

CLASS COLOR LOCATION:
Deep charcoal-purple appears only in the tattoo line-work — flat ink, zero glow. The gauntlets are bone-white with dark steel face — no color accent on weapons.

POSE:
Strict SOUTH frontal baseline.
No visible torso rotation; asymmetry comes from guard arm positions only.
Feet planted shoulder-width, both toes generally forward; one half-step offset is allowed but no lunge.
This is an idle S base, not an attack frame.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Tattoo geometry, gauntlet mass, and sandal construction must read at thumbnail scale.

NEGATIVE:
No lunging attack pose, no raised punch toward camera, no weapon, no plate armor, no barbarian fur, no massive axe, no bright purple aura, no random tribal scatter tattoos, no barefoot, no full boots, no environment, no eye-level portrait.
```

---

# 09 Summoner — Grave Chorus

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: SUMMONER — Grave Chorus.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through ritual silhouette, death-authority posture, echo-made bone crown command, and restrained soul light.
Visual tone: a death commander who gathers echoes, not a green-rot necromancer or a ghost mage.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of bone crown / half-skull visor, shoulder tops, lantern top, boot/robe hem perspective, and mild face foreshortening must be visible.

CORE IDENTITY:
A female death commander who gathers lost echoes that accumulated after the Fracturing — soul fragments that could not resolve because the rift tore their departure paths.
Not generic necromancer, not Hexer, not green curse mage.
Authority through presence, not through aggression.

RIMA NARRATIVE ANCHOR:
After the Fracturing, soul echoes began accumulating in rift-adjacent zones because the Fracturing severed normal departure paths — this character became a gatherer and commander of those trapped echoes, offering them structure in exchange for service.
The bone crown was not looted — it was constructed by echoes who chose to mark their commander, each carved section contributed by a different bound soul; the crown is therefore a record of allegiances, not a death trophy.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the asymmetrical echo-made bone crown / half-skull visor above the iron cage lantern held at shoulder height — the vertical axis from crown to lantern is the command line, the defining visual of a soul-authority who issues orders from above.

FACE:
Tall slim female figure.
Asymmetrical echo-made bone crown / half-skull visor frames the upper face, bleached bone-white, with dim cyan eye sockets — not blinding, just present.
Lower face partly visible so she does not read as a generic skeleton monster.
Hood pushed partly back so bone crown top is visible and clear.
Head upright, gaze slightly downward-forward through cyan sockets.

SILHOUETTE LOCK:
Soul lantern in LEFT hand at shoulder height — iron cage frame, cyan-white flame visible inside.
RIGHT hand lowered in palm-down command gesture — fingers spread slightly, commanding downward.
Asymmetrical echo-made bone crown / half-skull visor with cyan eye sockets.
Layered black funeral-mantle with narrow waist and 3 large readable bone fetish shapes hanging from belt — each a distinct large object (knot of bones, carved fragment, tied claw), not a decorative band of small beads.
One vertical cyan echo-tether runs from lantern toward chest, thin and restrained.

CLOTHING:
Void-black layered funeral-mantle, hood pushed back so bone crown is primary read.
Leather belt with exactly 3 large bone fetish objects — 3 readable distinct shapes, not 5+ small ones.
Dark boots barely visible below hem.
Below the robe hem at the base: a faint narrow cyan inner rim light along the inside of the hem edge — like soul light contained inside the robe, just visible at the bottom.

IMPLEMENT:
Iron cage soul lantern, left hand, shoulder height.
Cyan-white flame inside the cage — restrained, not a flare.
No staff.

FRACTURE MARK:
Cyan appears in four restrained places: bone crown eye sockets (dim, present), lantern flame (contained inside cage), one thin echo-tether from lantern to chest, robe inner hem rim (narrow thin light at hem base).
No green, no violet, no orange flame.

CLASS COLOR LOCATION:
Cyan accent — eye sockets are the face-level read; lantern flame is the implement-level read; hem rim is the body-level subtle read. Total cyan coverage is restrained, not dominant.

POSE:
Strict SOUTH frontal baseline.
Left lantern raised to shoulder height, right command hand low with palm facing down.
Feet/robe hem frontal, body square.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Bone crown / half-skull visor, cyan lantern, thin echo-tether, 3 bone fetishes on belt, and robe hem rim all readable.

NEGATIVE:
No green, no purple, no curse staff, no full skeleton face mask, no standard grim reaper skull, no hood completely hiding bone crown, no orange flame, no more than 3 belt fetishes, no environment, no eye-level portrait.
```

---

# 10 Hexer — Curse-Script Oracle

```text
Generate a premium stylized fantasy action-RPG character sprite concept.
Single full-body character on black background only.

CLASS: HEXER — Curse-Script Oracle.

TONE:
Grounded human adventurer, not a superhero, not a glowing champion, not a generic villager/NPC.
She should look cool through posture, exposed curse-script forearms, asymmetric ritual layering, and visible face.
Visual tone: a curse practitioner who operates through written script, not through robes and aura — her tools are ink and bone, not fire or spectacle.

CAMERA:
High top-down ARPG gameplay camera. NOT eye-level, NOT portrait.
Character faces SOUTH/front; hips and shoulders square to camera plane.
Top of hood/head, shoulder tops, staff orb top, boot/robe hem perspective, and mild face foreshortening must be visible.

CORE IDENTITY:
A living curse-binder who reads the Fracturing as a written disease that can be reproduced, layered, and detonated on a target.
Not Summoner, not skull mask, not generic witch, not robed oracle who hides inside cloth.
Her face is visible. Her forearms are exposed. Her work is readable on her body.

RIMA NARRATIVE ANCHOR:
She was a rift-cartographer before the Fracturing who mapped Fracturing propagation patterns — when she realized the patterns were not random but followed script-like rules, she began transcribing them onto her forearms as a memory and research system.
The bone trophy ring at her neck holds 7 small bone fragments, each from a target she successfully diagnosed and cursed to death — she keeps them because each represents a completed proof of her system, not for intimidation.

SILHOUETTE SIGNATURE:
The single most readable element at 128px is the exposed forearms covered in readable charcoal curse-script tattoos contrasted against a cropped hood and asymmetric ritual tunic — the forearms are the visual delivery of her class identity; they are not hidden, not covered, and not decorative background.

FACE:
Clearly female, late-20s, slim, pale sickly skin, sharp cheekbones, readable feminine face.
No skull mask, no face mask.
Cropped hood framing the face — hood top visible from above, but face fully readable from the front.
Dark hair visible at sides under hood.
Sharp eyes with dual green-violet glint in the iris — the only strong color on her face.
Head upright, gaze slightly downward-forward, predatory focus.

SILHOUETTE LOCK:
Curse staff in RIGHT hand, vertical, near right foot.
LEFT hand raised at chest height in curse-weave gesture, fingers spread with small barely-glowing glyph marks between fingers.
Cropped hood, face fully visible.
Exposed forearms with 3-4 large readable charcoal curse-script marks, not dense micro-detail.
Fitted asymmetric ritual tunic with exposed collarbone/forearms — not a full heavy robe.
Bone trophy ring at neck: 7 small bone fragments on a leather cord, readable as distinct shapes.

CLOTHING:
Cropped hood (not a full robe hood — the hem stops at shoulder level, no voluminous cloth mass below).
Asymmetric ritual tunic over dark trousers — tunic is open at the sides or cut shorter on one side, NOT a full closed robe.
Both forearms fully exposed — 3-4 large curse-script marks in charcoal ink with only a faint muted green hint, flat against skin, not glowing in the base sprite.
Wide dark sash belt.
No gold thread, no metallic embroidery, no decorative shimmer.
Geometric sigil embroidery on hood border and tunic hem in dark charcoal thread only — readable shapes, no shimmer.

IMPLEMENT:
Twisted dark wooden staff, right hand, vertical.
Small dull bone or black-glass focus at top, unlit.
Strong green-violet energy is reserved for separate cast/VFX frames, not the base character.

FRACTURE MARK:
2-3 tiny dull curse glyph marks between left hand fingers, barely glowing — the minimum needed to identify the class at 128px.
Matching charcoal glyphs at hood border and tunic hem — flat thread, no glow.

CLASS COLOR LOCATION:
Muted green appears only inside the curse-script tattoo lines as a faint undertone — not dominant, not glowing. The iris green-violet glint is the strongest color accent on the face. Zero gold anywhere.

POSE:
Strict SOUTH frontal baseline.
Right staff vertical near right foot, left curse hand at chest height, fingers spread.
Right foot half-step forward, body square.

STYLE:
Stylized painterly concept optimized for 128x128 pixel-art conversion.
Staff, clearly female visible face, cropped hood, 3-4 large curse-script forearm marks, and bone trophy ring must all be readable. Avoid micro-detail that will collapse in pixel art.

NEGATIVE:
No skull mask, no face mask, no masculine wizard face, no skeleton helm, no lantern, no cyan, no strong green-violet aura in base sprite, no full closed robe, no gold thread, no metallic embroidery, no environment, no eye-level portrait, no covered forearms.
```
