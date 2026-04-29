# RIMA — Gemini Referans Promptları
*Sadece Gemini üretimi için. PixelLab ayarları → `SPRITE_WORKFLOW.md`*

**Kullanım:** gemini.google.com → yeni sohbet → promptu yapıştır → üret → sağ tık kaydet

---

## ⚠️ HER GÖRSELİ KAYDEDERKEN KONTROL ET

**Top-down açı doğru mu?**
✅ Doğru: Omuzlar geniş, kafa tepeden küçük, yüz görünmüyor, ayaklar yok/çok küçük
❌ Yanlış: Yüz görünüyor, yan profil, 3/4 açı

Yanlışsa aynı sohbette yaz:
```
Regenerate from directly above, strict bird's eye top-down view. Camera is mounted on the ceiling looking straight down. Show wide shoulders, no face, tiny or hidden feet.
```

---

## FAZ 0 — LOGO

**Kaydet:** `ART/logo/rima_logo_kaynak.png`
```
A dark fantasy pixel art game logo. The large uppercase letters "RIMA" are written
boldly in dark steel color, spanning the upper portion of the image.
Below and to the right of the letter "I", the small lowercase letters "ft" hang
downward at an angle — as if they cracked off from the word RIFT and are drooping,
falling debris from a broken seal. They are smaller, slightly rotated, cracked in texture.
Below and to the right of the letter "A", the small lowercase letters "rch" hang
downward at the same angle — as if they cracked off from the word MARCH, same drooping effect.
At the exact break points where "ft" detached from "RI" and "rch" detached from "MA",
brilliant gold light bleeds through the crack, color #FFD700.
Letter color: dark steel #1E1E32. Background: void black #080808.
The hidden full words RIFT and MARCH become readable on closer inspection.
Pixel art logo style, high contrast, kintsugi dark aesthetic.
```
Kontrol: "ft" I'nın altından sarkıyor mu? "rch" A'nın altından sarkıyor mu? Kırık noktalarda altın parlaması var mı?

Yanlışsa:
```
Regenerate with heavier font weight, more angular chiseling, larger cold blue crack on the I
```

---

## KARAKTERLER

---

### WARBLADE (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/warblade/warblade_gemini_base.png`
```
A battle-hardened warrior viewed strictly from directly above, bird's eye aerial
top-down perspective, as if a camera is mounted on the ceiling looking straight down.
The warrior holds a single large greatsword in their RIGHT hand only, blade pointing
downward at their side. The LEFT hand is completely empty, relaxed and open at their
left side. NO weapon, NO shield, NO item in the left hand whatsoever.
Battered mismatched armor pieces — some iron plate, some leather straps, some rough cloth
patched over gaps. The armor is incomplete and worn, clearly repaired multiple times.
No helmet — weathered scarred face visible from above, unkempt dark hair, unshaven.
Short torn cape visible behind the shoulders. Asymmetric pauldrons visible from above,
one heavier than the other. Wide silhouette from the shoulders but ragged edges.
Feet barely visible. Retro pixel art style, limited color palette:
dark iron grey, worn leather brown, dull tarnished metal. Transparent background, no background elements.
Rough mercenary look, battle-hardened but ragged. Dangerous and powerful despite the worn gear.
IMPORTANT: Only ONE weapon total. ONE greatsword in right hand. Left hand empty.
```
Kontrol: omuzlar geniş ✓ | yüz görünüyor (sakal/yara izi) ✓ | SAĞ ELDE TEK KILIÇ ✓ | SOL EL BOŞ ✓ | KASK YOK ✓ | yamama zırh ✓ | Yüz görünüyor mu? ✓

Yanlışsa (iki elde silah):
```
Regenerate. The character holds ONE greatsword in the right hand only.
The left hand must be completely empty, hanging at their side with nothing in it.
No dual wield. No shield. Nothing in the left hand.
```

---

### ELEMENTALİST (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/elementalist/elementalist_gemini_base.png`
```
A young woman elementalist mage viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
She is beautiful but clearly worn by years of channeling raw elemental power — sharp features, high cheekbones, determined expression. Her face is visible from above: long dark hair partially loose and wild, some strands floating upward from heat energy around her. A diagonal scar runs across one cheek. Her eyes have a faint ember glow.
Her RIGHT hand grips a long cracked obsidian staff, tip crackling with flame. Her LEFT hand is extended outward, fingers splayed, channeling a swirling mix of fire and frost energy — small orbiting fire embers and ice shards spiral around her body.
She wears dark layered robes — torn at the hem, burned at the edges, patched with rough stitching. A worn leather sash cinches the waist. The robes have faded crimson rune stitching barely visible. Not elegant — practical and battle-worn.
Her hands and forearms are visibly scarred from years of raw elemental channeling: burn marks and frost cracks running up to the elbows.
Wide robes spreading outward. Retro pixel art style, color palette: deep crimson-orange fire, cold ice blue frost, void black robes, pale skin with ember highlights. Transparent background, no background elements.
```
Kontrol: kadın karakter ✓ | güzel ama yıpranmış yüz ✓ | saç serbest/dağınık ✓ | yanak yarası ✓ | staff sağ elde ✓ | sol el enerji kanalıyor ✓ | rünler solmuş ✓ | eller yanmış/yara izli ✓ | top-down ✓ | Yüz görünüyor mu? ✓

Yanlışsa (erkek görünümlü çıkarsa):
```
Regenerate. The elementalist must be a young woman with clearly feminine features — sharp beautiful face, long dark wild hair floating from heat, diagonal scar on cheek, ember-glow eyes. She is powerful and battle-worn but unmistakably female. Keep all other elements the same.
```

---

### SHADOWBLADE (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/shadowblade/shadowblade_gemini_base.png`
```
A young woman assassin rogue viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
She holds a SHORT DAGGER in each hand — right hand and left hand both, daggers pointing forward in a low ready stance. Dual daggers are the character's identity.
Dark form-fitting worn leather armor — patched and re-stitched many times, worn through at stress points. Almost fully black but faded from wear. A partial shadow trail follows behind the figure, as if they just moved. Small wisps of dark smoke rise from the daggers.
Very low crouching silhouette, hard to see edges blending into darkness. A dark hood and partial cloth mask cover most of the face, but her eyes are clearly visible — angular sharp features, cold and empty gaze, dark circles under the eyes from chronic sleeplessness. Old cut scars visible around the eyes and on what skin shows. She deliberately looks unremarkable — plain worn clothing over the assassin gear, easy to overlook in a crowd. The only tell is her eyes: too still, too aware. A beauty that's there if you look, but she has no interest in being seen. No special assassin flair.
Retro pixel art, color palette: shadow black, dark grey, faint void purple edge glow. Transparent background.
IMPORTANT: Both hands hold daggers. This is correct for this character.
```
Kontrol: iki elde hançer ✓ | alçak gizli duruş ✓ | gölge iz ✓ | top-down ✓ | Yüz görünüyor mu? (gözler açık, maske/hood altında kesik izleri, gözaltı koyu) ✓ | kasıtlı sıradan görünüm ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (erkek görünümlü çıkarsa):
```
Regenerate. The assassin must be a young woman with clearly feminine features — angular sharp face, cold empty eyes with dark circles underneath, short messy hair or loose bun visible under the hood. She is deliberately plain and unremarkable but unmistakably female. Keep all other elements the same.
```

---

### RANGER (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/ranger/ranger_gemini_base.png`
```
A middle-aged man ranger hunter viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
He holds a longbow horizontally in the LEFT hand, arm extended. The RIGHT hand is drawn back as if just released an arrow, fingers still curled. A quiver of arrows is visible on his back from above.
Patched leather armor — mismatched pieces, animal hide stitched over tears, practical and rough. No hood — long unwashed hair, matted and tangled with small leaves and twigs caught in it, falls loosely to the sides visible from above. Full unkempt beard, thick and disheveled. The face is deeply sun-darkened with cracked weathered skin, squinting eyes narrowed from years of tracking distant targets. Hands visibly calloused and scarred. The face is dirty, mud-streaked from living in the wilds. Not a noble hunter — not noble at all, genuinely feral. Someone who has survived alone in hostile terrain for a long time. He looks like he hasn't spoken to another person in months. Comfortable with that.
Lean athletic silhouette, much lighter than a warrior. Feet visible — the ranger is in a wide stance for stability. Shoulders lean and narrow compared to the warrior class.
Retro pixel art, color palette: dark forest green, earthy brown, cold steel arrow tips. Transparent background.
```
Kontrol: yay sol elde ✓ | sağ el çekme pozisyonunda ✓ | sadak sırtta görünüyor ✓ | top-down ✓ | Yüz görünüyor mu? (güneşten yanmış deri, tam sakal, uzun dağınık saç, saçta yaprak/dal, kısık gözler, çamurlu) ✓ | yamalanmış deri zırh ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (kadın görünümlü çıkarsa):
```
Regenerate. The ranger must be a middle-aged man with clearly masculine features — full thick unkempt beard, long matted hair with debris caught in it, deeply weathered sun-darkened face with cracked skin, squinting eyes. He is not noble or heroic — genuinely feral and wild. Keep all other elements the same.
```

---

### RAVAGER (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/ravager/ravager_gemini_base.png`
```
A massive man berserker ravager viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
He holds a massive two-handed great axe, gripping it with BOTH HANDS in the center of the body. The axe head is enormous, wider than his shoulders. This is correct — both hands grip the same weapon.
Enormous build — nearly inhuman scale. Huge wide shoulders, thick neck, barrel chest, tree-trunk arms. Bare upper torso or only minimal rough fur/leather straps across the chest. Sun-darkened skin covered in old scars and ritualistic war paint in tribal patterns across his face and body. No helmet — wild unkempt tangled hair and a dense beard, both massive, visible from above. The face is broad and scarred, war paint across brow and cheeks. Patches of rough hide and fur tied around the waist and legs, barely functional. Looks like civilization is a distant memory. Each scar tells a story of a fight he won. Red rage energy aura faintly visible at the edges of the figure — fury building.
Wide brutal overwhelming silhouette. His sheer mass dominates the frame. The scale makes normal-sized weapons look small.
Retro pixel art, color palette: dark iron axe, sun-darkened scarred skin, war paint deep red and black, blood red fury glow at edges. Transparent background.
```
Kontrol: iki elde büyük balta ✓ | çıplak üst gövde ✓ | DEV silüet ✓ | top-down ✓ | Yüz görünüyor mu? (kaskı yok, vahşi saç+sakal, tribal savaş boyası) ✓ | yara izleri ✓ | neredeyse insanüstü büyüklük ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (küçük veya kadın görünümlü çıkarsa):
```
Regenerate. The ravager must be an enormous man — nearly inhuman scale, massive shoulders and arms wider than a doorframe, dense beard, wild tangled hair, tribal war paint. He should look like he could flip a cart over with one hand. Overwhelming presence and size. Keep all other elements the same.
```

---

### PALADİN (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/paladin/paladin_gemini_base.png`
```
An aging man fallen paladin warrior viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
He holds a large battered tower shield in the LEFT hand — the shield is deeply dented and chipped, old holy symbols scratched and worn, shield facing forward-left. The RIGHT hand holds a worn holy war hammer, head pointing downward ready to strike, the handle wrapped in fraying cloth.
Old, heavily worn plate armor — no longer immaculate. Pieces are dented, scratched, and repaired with crude iron patches. The golden insignia has mostly flaked off. No helmet — a tired, gaunt face of a man in his fifties, visible from above. Grey-white hair, perhaps once golden, now dull. Deep lines carved into his face. Hollow eyes that have seen too much and now believe in very little, yet cannot stop. Short grey stubble on jaw and cheeks. Shoulders slightly stooped — he used to stand straighter. He won every battle and lost everything that mattered. The holy light still comes, but he no longer knows why. Holy light still faintly bleeds through the cracks in the armor — not from divinity, but from something that refuses to go out.
Wide silhouette due to the tower shield on one side. The shield makes the left side of the figure much wider.
Retro pixel art, color palette: dark tarnished iron plate, faded worn gold, faint holy white-gold glow through cracks. Transparent background.
```
Kontrol: kalkan sol elde ✓ | çekiç sağ elde ✓ | kutsal ışık çatlaklardan (sönük) ✓ | kalkan sola çıkıntı yapıyor ✓ | top-down ✓ | Yüz görünüyor mu? (kask yok, 50'li yaşlar, gri-beyaz saç, kısa gri sakal, içi boş gözler, düşük omuzlar) ✓ | eskimiş/çentikli ekipman ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (genç veya kadın görünümlü çıkarsa):
```
Regenerate. The paladin must be a clearly aging man — visibly in his fifties, grey-white hair, short grey stubble on face, deep wrinkles, hollow weary eyes. Stooped posture. Not young, not vigorous. The weight of decades of loss shows on his face. Keep all other elements the same.
```

---

### SUMMONER (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/summoner/summoner_gemini_base.png`
```
An elderly woman summoner, small and hunched but radiating quiet authority, viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
She holds a bone staff topped with a skull in the RIGHT hand, staff pointing upward. The LEFT hand is outstretched, palm up, with dark energy rising from it.
Two very small skeleton minion silhouettes orbit slowly around her figure — they are small and faint but visible from above. She holds them close like family, not like tools.
Old, torn and frayed dark robes — holes patched with mismatched cloth she stitched herself, hems ragged and dirty. No hood or mask — an elderly woman's face visible from above. White-grey hair pulled into a long loose bun, wild and imperfect. Deep lines carved into her face from decades of forbidden work, but her eyes are sharp and intelligent — nothing dull about them. Small and slightly hunched forward, but her presence fills the space. Bony, thin hands with prominent knuckles, but every movement is precise and deliberate. Grandmother energy mixed with quiet menace. She's seen empires fall. She's the reason some of them fell. The figure is frail in frame — not sinister skull-faced but the quiet dangerous kind of old.
Retro pixel art, color palette: void black robes, bone white accents, sickly green-purple necromantic energy. Transparent background.
```
Kontrol: staff sağ elde ✓ | sol el enerji salıyor ✓ | 2 küçük iskelet çevrede ✓ | top-down ✓ | Yüz görünüyor mu? (yaşlı kadın yüzü, beyaz-gri saç gevşek topuzda, derin çizgiler, keskin gözler, kask/hood yok) ✓ | eski yırtık cübbe (kendi dikmiş) ✓ | küçük öne eğik ama otoriter duruş ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (erkek veya sakallı çıkarsa):
```
Regenerate. The summoner must be an elderly woman — no beard, clearly female face with deep wrinkles, white-grey hair in a loose bun, small and slightly hunched. She looks like a grandmother but radiates quiet dangerous authority. The skeleton minions orbit her like family members. Keep all other elements the same.
```

---

### HEXER (Ana Karakter | 64×64)

**Kaydet:** `ART/karakterler/hexer/hexer_gemini_base.png`
```
A young woman hexer warlock viewed strictly from directly above, bird's eye aerial top-down perspective, as if a camera is mounted on the ceiling looking straight down.
She holds a cursed tome in the LEFT hand, open and floating slightly. The RIGHT hand is raised with fingers spread, dripping with void purple corruption energy that falls downward like liquid.
Dark worn coat or robe — dirty, frayed, with void purple corruption spreading across the fabric like growing stains. Multiple hex marks and curse symbols visible on the clothing, some burned into the fabric. She carries forbidden books tucked into the coat.
No hat or full hood — a young woman's face, pale and nearly sickly looking, visible from above. Long messy dark hair, tangled and uncared for. Thin and slight frame. Wide eyes, slightly unfocused — the pact is affecting her mind. The skin shows signs of void corruption spreading from around the eyes and temples: purple-black patterns like ink seeping under the skin, half beautiful half horrifying. Not monstrous — there is something hauntingly striking about her face, beautiful and wrong at the same time. The face of someone who thought she was just borrowing power. She didn't read the fine print.
Retro pixel art, color palette: deep void purple, corruption black, sickly green-yellow curse marks, pale skin with purple corruption veins. Transparent background.
```
Kontrol: kitap sol elde ✓ | sağ el lanet enerjisi ✓ | mor korozyon yayılıyor ✓ | top-down ✓ | Yüz görünüyor mu? (genç kadın, soluk hasta ten, uzun dağınık koyu saç, geniş odaksız gözler, void korozyonu gözler çevresinde yarı güzel yarı korkunç) ✓ | ince zayıf çerçeve ✓ | Cinsiyet okunuyor mu? ✓

Yanlışsa (erkek görünümlü çıkarsa):
```
Regenerate. The hexer must be a young woman with clearly feminine features — lean thin build, long messy dark tangled hair, pale sickly complexion, wide slightly unfocused female face. She is hauntingly striking — beautiful and wrong at the same time, void corruption spreading around her eyes like dark ink. She looks like a student who got in over her head. Keep all other elements the same.
```

---

## FAZ 1 — GRUNT TİER (32×32)

---

### SHARD WALKER (Grunt | Act 1-2-3)

**Kaydet:** `ART/dusmanlar/grunt_shard/grunt_shard_gemini_base.png`
```
A humanoid figure assembled from floating broken stone shards and bone fragments,
strictly top-down bird's eye view from directly above.
The pieces are loosely arranged in a vague warrior shape with gaps between them.
Cold blue light bleeds through the gaps between the shards.
No solid body, just hovering fragments in a warrior silhouette.
The head is the largest shard at the top, two thin arm-like shard strips extend sideways.
Retro pixel art style, dark stone and cold blue palette. Transparent background.
```
Kontrol: parçalar arası boşluk ✓ | mavi ışık sızıyor ✓ | top-down ✓

Yanlışsa:
```
Regenerate strictly from directly above. Show floating disconnected stone shards
arranged loosely in a humanoid shape, viewed from a ceiling camera looking straight down.
Gaps between shards must be visible with blue glow showing through.
```

---

### VOID THRALL (Grunt | Act 1-2)
*Sadece TAM FORM üret. Sol/sağ yarılar PixelLab'dan sonra Aseprite'ta kesilecek.*

**Kaydet:** `ART/dusmanlar/grunt_thrall/grunt_thrall_gemini_base.png`
```
A humanoid warrior in dark armor, strictly top-down bird's eye view from directly above.
Running vertically through the center of the chest is a deep glowing crack,
void purple energy seeping from the fissure. The overall silhouette is sinister and medieval.
The face is completely hidden by a helmet or hood. Wide shoulders, small head at top.
Retro pixel art, dark iron and void purple palette. Transparent background.
```
Kontrol: dikey çatlak göğüs ortasında ✓ | yüz gizli ✓ | top-down ✓

---

### SEAM CRAWLER (Grunt | Act 1-2)

**Kaydet:** `ART/dusmanlar/grunt_seam/grunt_seam_gemini_base.png`
```
Top-down bird's eye view of a creature living inside a floor crack.
Only its two long dark claws and a spine ridge are visible above the crack surface.
The body is hidden below, pressed flat against the fissure.
The claws grip the crack edges tightly. Shadowy dark form.
Horror fantasy enemy, dark stone texture. Transparent background.
```
Kontrol: gövde gizli ✓ | sadece pençeler + omurga görünüyor ✓ | zemin çatlağına yapışık ✓

Yanlışsa:
```
Regenerate. Show ONLY the two claws and spine ridge above the crack surface.
The rest of the body must be hidden below the floor. Top-down view.
```

---

### ECHO HOUND (Grunt | Act 2-3)

**Kaydet:** `ART/dusmanlar/grunt_echo/grunt_echo_gemini_base.png`
```
A ghostly wolf-like creature, top-down bird's eye view from directly above.
Semi-transparent indigo form with white glowing eyes.
A motion afterimage trails behind it in lower opacity.
No solid body, just energy lines and silhouette forming a predatory crouching shape.
The body is 70% visible, edges fading. Indigo and deep navy color palette.
Dark fantasy enemy sprite, transparent background.
```
Kontrol: yarı transparan ✓ | afterimage iz ✓ | beyaz gözler ✓ | yırtıcı duruş ✓

Yanlışsa:
```
Regenerate. The creature must be semi-transparent like a ghost, indigo color.
Show a motion trail afterimage behind it at lower opacity. Top-down view.
```

---

### HOLLOW MITE (Grunt | Act 1 sürü, Act 2)

**Kaydet:** `ART/dusmanlar/grunt_mite/grunt_mite_gemini_base.png`
```
A small hollow insect-like creature, top-down bird's eye view from directly above.
Six legs, dark exoskeleton. The body interior is completely transparent and hollow,
like an empty shell with a faint glowing core visible inside.
Very small creature, roughly the size of a large spider.
Dark brown-black exoskeleton with a tiny pale blue glowing core inside the hollow shell.
Dark fantasy swarm enemy, transparent background.
```
Kontrol: içi boş görünüyor ✓ | çok küçük ✓ | içten ışık görünüyor ✓

---

### CHAIN BOUND (Grunt | Act 2-3)
*Tek figür üret. Aseprite'ta 3 varyant yapılacak.*

**Kaydet:** `ART/dusmanlar/grunt_chain/grunt_chain_gemini_base.png`
```
A small damaged armored humanoid soldier, top-down bird's eye view from directly above.
Dark iron plate armor with a glowing void purple anchor point on the chest.
The anchor point is a small circular mark where a chain would attach, glowing faintly purple.
The armor is damaged but the figure looks aggressive and combat-ready.
Facing downward (south). Retro pixel art, dark iron and void purple palette, transparent background.
```
Kontrol: göğüste zincir bağlantı noktası görünüyor ✓ | top-down ✓ | küçük figür ✓

---

## FAZ 1 — ELİTE TİER (64×64)

---

### THE TWICE-BORN (Elite | Act 1 nadir, Act 2)

**Kaydet:** `ART/dusmanlar/elite_twiceborn/twiceborn_gemini_base.png`
```
Two identical dark-robed warriors connected by a thin glowing golden thread at their chests,
viewed from directly above in strict bird's eye top-down perspective.
The two figures stand side by side in a 64x64 frame.
One figure holds a shield in a defensive stance, one arm raised.
The other figure has a sword raised in attack stance, sword in RIGHT hand only, left hand empty.
Both wear matching but differently damaged dark armor.
The golden thread runs from heart-to-heart between them.
Retro pixel art, dark iron and worn gold palette, transparent background.
```
Kontrol: 2 figür ✓ | altın ip kalp-kalp ✓ | kılıç sadece sağ elde ✓ | kalkan tarafında silah yok ✓

---

### FRACTURE-BORN (Elite | Act 2-3)
*İki Gemini üretimi: atmosfer referansı + PixelLab'a girecek tam form.*

**1. SPAWN REFERANSI** (sadece görsel referans, PixelLab'a girmeyecek)
**Kaydet:** `ART/dusmanlar/elite_fracture/fracture_gemini_spawn_ref.png`
```
Two long dark clawed hands reaching up from a glowing floor crack,
top-down bird's eye view from directly above.
The fingers grip the crack edges, pulling upward.
Void dark energy rises from below the crack like smoke.
The crack glows cold blue-white. Horror ambush scene.
Dark fantasy, transparent background.
```

**2. TAM FORM** (PixelLab'a Init Image olarak girecek)
**Kaydet:** `ART/dusmanlar/elite_fracture/fracture_gemini_fullform.png`
```
A tall thin creature that just crawled out from a dimensional rift in the floor,
top-down bird's eye view from directly above.
Disproportionately long arms, evolved for climbing through narrow cracks.
Near-black body with glowing seams at joints where void energy leaks.
A residual crack scar runs along the spine.
Predatory crouching posture, very long reach. Dark fantasy elite enemy.
Transparent background.
```
Kontrol: kollar çok uzun ✓ | eklem yerlerinde glow ✓ | omurgada çatlak izi ✓

---

### SPORE HOLLOW (Elite | Act 2)

**Kaydet:** `ART/dusmanlar/elite_spore/spore_gemini_base.png`
```
A hollow human shell figure, top-down bird's eye view from directly above.
The figure has a blank featureless face with empty eye sockets.
Orange-brown mushroom-like growths and spores burst from cracks across the body,
especially from the shoulders and back.
The body appears grey and lifeless but the fungal growths are vibrant orange-brown.
Shambling posture, heavy slow movement implied. Dark fantasy enemy, transparent background.
```
Kontrol: mantar büyümeleri omuz ve sırtta ✓ | yüz boş ✓ | soluk gri beden ✓

---

### SHARD BROOD (Elite | Act 2-3)

**Kaydet:** `ART/dusmanlar/elite_shardbrood/shardbrood_gemini_base.png`
```
A large humanoid figure assembled from dozens of floating broken stone shards,
top-down bird's eye view from directly above.
Much larger than a normal shard warrior, taking up most of a 64x64 frame.
The main body has three distinct clusters of smaller shards visibly attached to the torso,
each cluster glowing cold blue as if barely contained and ready to break free.
The overall form is unstable, shards constantly threatening to scatter.
Cold blue glow through all gaps. Dark fantasy elite enemy, transparent background.
```
Kontrol: ana gövde büyük ✓ | 3 adet shard kümesi belirgin ✓ | mavi glow ✓

---

### HOLLOW CRADLE (Elite | Act 2)

**Kaydet:** `ART/dusmanlar/elite_cradle/cradle_gemini_base.png`
```
A human-shaped shell figure, top-down bird's eye view from directly above.
The body is covered in deep cracks running across the torso, shoulders, and limbs.
Through the cracks, small insect-like creatures (hollow mites) are visible inside the body,
their tiny pale eyes glowing faintly through the gaps.
A few small insect forms are already crawling out of the largest cracks.
The shell body is pale grey, hollow inside. The mites inside are dark with pale glow eyes.
Dark fantasy elite enemy, transparent background.
```
Kontrol: vücutta çatlaklar ✓ | çatlaklardan mite gözleri görünüyor ✓ | bazı mite dışarı çıkıyor ✓

---

### ECHO ANCHOR (Elite/Sabit | Act 3)

**Kaydet:** `ART/dusmanlar/elite_anchor/anchor_gemini_base.png`
```
A ghostly humanoid figure chained to the ground by multiple void chains, unable to move,
top-down bird's eye view from directly above.
The figure is translucent indigo, the chains are dark void with purple glow.
Three or four faint Echo Hound silhouettes orbit slowly around the chained figure,
as if echoes spawned from it. The orbiting silhouettes are very faint, almost ghost-like.
The chains anchor into the ground at four points around the figure.
Dark fantasy stationary elite enemy, transparent background.
```
Kontrol: zincirlere bağlı ✓ | çevresinde soluk Echo Hound siluetleri ✓

---

## FAZ 1 — STATİK HAZARD

---

### RIFT MAW (Statik | Act 3)

**Kaydet:** `ART/dusmanlar/static_riftmaw/riftmaw_gemini_base.png`
```
A large circular rift opening embedded in the floor, top-down bird's eye view from above.
The rift has jagged torn edges like teeth surrounding a pure void black interior.
Faint golden glow deep inside the void. Particles and debris visibly being pulled inward
from the surrounding area. The edges pulse as if the rift is breathing and alive.
No movement, stationary floor hazard. Dark fantasy, transparent background.
```
Kontrol: yuvarlak ✓ | dişli kenarlar ✓ | içi tam siyah ✓ | partiküller içe çekiliyor ✓

---

### THE WOUND (Statik | Act 1-2-3 özel oda)

**Kaydet:** `ART/dusmanlar/static_wound/wound_gemini_base.png`
```
A floating oval wound in reality hovering in mid-air, top-down bird's eye view from above.
The wound has organic ragged crimson-purple edges that pulse with life.
The interior is void black. Red and crimson glow radiates from the edges.
Red particle tendrils extend outward from the edges, reaching toward nearby areas.
The wound floats slightly above the ground, casting a faint shadow.
It is a living injury made manifest. Dark fantasy unique entity, transparent background.
```
Kontrol: havada asılı ✓ | organik kenarlar ✓ | kırmızı partiküller dışa uzanıyor ✓

---

## FAZ 2-3 — ÖZEL MOB'LAR

---

### CLASS MIMIC (Özel | Act 2-3)
*Her class için ayrı sohbet. Aşağıdaki Warblade örneği — diğerleri için silah/siluet değişir.*

**Kaydet:** `ART/dusmanlar/special_mimic/mimic_warblade_gemini_base.png`
```
A distorted translucent mirror copy of a heavily armored warrior,
top-down bird's eye view from directly above.
The silhouette matches a dark armored warrior but is desaturated and void-tinted purple.
Where the face should be: only void black, no eyes, no features.
Movements appear slightly glitched, pixels dissolving at the edges.
The copy holds a greatsword in right hand only, left hand empty, same as the original.
Semi-transparent, ghostly, corrupted. Dark spirit entity, transparent background.
```
Kontrol: original'dan belirgin biçimde kopuk/bozuk ✓ | yüz yok ✓ | void tint ✓

---

### REMNANT HOST (Özel | Act 3)
*3 ayrı sohbet — her form farklı renk ve silüet.*

**FORM 1 — Savaşçı (kırmızı tint)**
**Kaydet:** `ART/dusmanlar/special_remnant/remnant_gemini_form1.png`
```
A humanoid figure glitching between forms, top-down bird's eye view from directly above.
Currently showing a red-tinted heavy fighter form.
The edges of the body are dissolving into pixels and fragments, as if unstable.
The body is solid at the center but breaks apart at the edges.
Red color tint throughout. Heavy fighter silhouette with fragments flying off.
Three overlapping ghost forms barely visible at the same position, slightly offset.
Dark fantasy elite enemy, transparent background.
```

**FORM 2 — Büyücü (mavi tint)**
**Kaydet:** `ART/dusmanlar/special_remnant/remnant_gemini_form2.png`
```
A humanoid figure glitching between forms, top-down bird's eye view from directly above.
Currently showing a blue-tinted mage form. Rune fragments flying off the edges.
The body is unstable, pixels dissolving at the edges, three ghost overlapping forms visible.
Blue color tint throughout. Mage silhouette holding a staff in right hand only.
Dark fantasy elite enemy, transparent background.
```

**FORM 3 — Avcı (yeşil tint)**
**Kaydet:** `ART/dusmanlar/special_remnant/remnant_gemini_form3.png`
```
A humanoid figure glitching between forms, top-down bird's eye view from directly above.
Currently showing a green-tinted archer form. Arrow fragments flying off the edges.
The body is unstable, pixels dissolving at the edges, three ghost overlapping forms visible.
Green color tint throughout. Archer silhouette with bow in right hand only, left hand empty.
Dark fantasy elite enemy, transparent background.
```

---

## FAZ 1 — BOSS

---

### IRON WARDEN (Act 1 Boss | 128×128)

**Kaydet:** `ART/dusmanlar/boss/iron_warden/boss_iron_warden_gemini_base.png`
```
A massive iron golem guardian, top-down bird's eye view from directly above,
as if a camera is mounted far above on the ceiling looking straight down.
Full dark plate armor heavily damaged and covered in deep cracks.
Cold blue energy seeps through the cracks. Several broken sword shards are embedded
in its back and shoulders like trophies.
The figure is enormous. Head very small at the top, extremely wide shoulders.
Radiates an overwhelming slow unstoppable presence.
Retro pixel art, dark iron and cold blue palette. Transparent background.
IMPORTANT: The figure must look MASSIVE. Head takes up less than 10% of height.
Shoulders extremely wide. The scale should feel like a building, not a person.
```
Kontrol: baş çok küçük ✓ | omuzlar çok geniş ✓ | kılıç kırıkları sırtta ✓ | devasa his ✓

Yanlışsa:
```
Regenerate. The iron warden must be MASSIVE. Make the head much smaller,
shoulders much wider. It should look like an iron fortress, not a person.
Top-down view, ceiling camera looking straight down.
```

---

## FAZ 1 — TİLE REFERANSlARI *(opsiyonel)*

### FLOOR TILE
**Kaydet:** `ART/tiles/act1/act1_floor_gemini_ref.png`
```
Pixel art tile, 16x16 pixels, seamless tileable, top-down 2D dark fantasy
dungeon floor, dark grey cracked cobblestone #1E1E32 with thin cold blue
glowing fissures #7BA7BC, dungeon atmosphere, transparent background,
3 slight variations of the same tile design
```

### WALL TILE
**Kaydet:** `ART/tiles/act1/act1_wall_gemini_ref.png`
```
Pixel art tile, 16x32 pixels, seamless tileable, top-down dark fantasy game
wall, crumbling fortress stone, cold blue barely visible in cracks,
dark stone texture #080808 #1E1E32, game wall tile, transparent background
```

### CRACK OVERLAY
**Kaydet:** `ART/tiles/act1/act1_crack_gemini_ref.png`
```
Pixel art overlay sprite, 16x16 pixels, transparent background,
thin crack line patterns only, no fill, 1-2 pixel wide,
cold blue glow color #7BA7BC, dark fantasy floor decoration overlay,
4 different crack angle variations, game decor sprite
```

---

## ACT MEKANLARI — Konsept Referansları

*Her act için mekan konsepti Gemini'den alınır, PixelLab tileset üretiminde referans olarak kullanılır.*

---

### ACT 1 — PARÇALANMIŞ KALELER (The Shattered Halls)

**Atmosfer:** Boyutlar arası kırılmayla paramparça olmuş antik bir kale. Taş duvarlar iki boyut arasında sıkışmış — bir yarısı burada, diğer yarısı void'de. Soğuk mavi çatlaklar her yüzeyi kesiyor.
**Mob tonu:** Asker kalıntıları, parçalanmış varlıklar, antik yönetimin gölgeleri.
**Renk paleti:** Koyu demir grisi, soğuk mavi, yıpranmış altın.

**GENEL KONSEPT REFERANS**
**Kaydet:** `ART/mekanlar/act1/act1_konsept_gemini.png`
```
A top-down view of a shattered ancient fortress interior, bird's eye perspective from directly above.
The stone floor is cracked with cold blue glowing fissures running across it. Some floor tiles are missing, revealing void darkness below.
Ancient stone walls visible at the edges, partially collapsed. Cold blue dimensional tears run vertically through the walls.
Scattered broken armor and weapons on the floor. Dim cold blue ambient light, no warm tones.
Dark fantasy dungeon, retro pixel art style, color palette: dark iron grey #1E1E32, cold blue #4A7FA5, void black #080808, worn gold #8B7355.
Transparent background or void black background.
```

**FLOOR TILE — Kale Zemini**
**Kaydet:** `ART/tiles/act1/act1_floor_gemini_ref.png`
```
Pixel art seamless tileable floor tile, 16x16 pixels, top-down 2D view.
Dark grey cracked stone floor, cold blue glowing fissures running through the cracks.
Ancient fortress stone texture. Colors: dark grey #1E1E32, cold blue crack glow #4A7FA5.
Seamless on all four edges. Transparent background.
```

**WALL TILE — Kale Duvarı**
**Kaydet:** `ART/tiles/act1/act1_wall_gemini_ref.png`
```
Pixel art seamless tileable wall tile, 16x32 pixels, top-down 2D dark fantasy dungeon.
Crumbling fortress stone wall. Cold blue dimensional tears visible in the stone.
Dark stone texture. Colors: dark stone #1E1E32, cold blue #4A7FA5, mortar grey #2A2A40.
Seamless on left and right edges.
```

---

### ACT 2 — BOŞ YÜRÜYÜŞ (The Void Marches)

**Atmosfer:** Boyutlar arasındaki uçsuz bucaksız void uzayı. Yerçekimi kararsız. Yüzen platform adaları var ama arasında sonsuz karanlık. Void enerjisi mor kristal formlar oluşturmuş.
**Mob tonu:** Void'den doğan varlıklar, yankılar, mantar kolonileri.
**Renk paleti:** Derin mor, void siyahı, elektrik viyole, sızıntı camgöbeği.

**GENEL KONSEPT REFERANS**
**Kaydet:** `ART/mekanlar/act2/act2_konsept_gemini.png`
```
A top-down view of a void dimension environment, bird's eye perspective from directly above.
Floating stone platform islands surrounded by pure void darkness. Between platforms: nothing, just black.
Void purple crystal formations growing from the platform edges, glowing faintly.
Purple dimensional energy wisps drifting slowly across the platforms.
Unstable, beautiful and terrifying atmosphere. Colors: void purple #6A0DAD, electric violet #8B00FF, void black #040408, sickly cyan wisps #00FFAA.
Retro pixel art style, transparent or void black background.
```

**FLOOR TILE — Void Platform**
**Kaydet:** `ART/tiles/act2/act2_floor_gemini_ref.png`
```
Pixel art seamless tileable floor tile, 16x16 pixels, top-down 2D view.
Dark floating platform surface with void purple energy seeping through cracks.
The tile edges fade to void darkness suggesting the platform edge is near.
Colors: deep dark purple-grey #1A0A2E, void purple crack glow #6A0DAD.
Seamless on all four edges.
```

**WALL TILE — Void Kristal**
**Kaydet:** `ART/tiles/act2/act2_wall_gemini_ref.png`
```
Pixel art seamless tileable wall tile, 16x32 pixels, top-down 2D dark fantasy.
Void crystal formation wall. Sharp angular crystal shapes with deep purple glow inside.
Colors: crystal black #0A0015, void purple glow #6A0DAD, electric violet edge #8B00FF.
Seamless on left and right edges.
```

---

### ACT 3 — YAKINSAMA ÇEKİRDEĞİ (The Convergence Core)

**Atmosfer:** The Fracturing'in merkezi. Onlarca farklı dünya buraya çökmüş. Gotik kale parçaları, antik tapınak sütunları, sanayi metal yapıları — hepsi altın çatlaklarla birbirine kaynaşmış. Her şey hem yıkılmış hem kutsal görünüyor.
**Mob tonu:** Gerçekliği bükücüler, taklit edenler, kalan her şeyin kalıntıları.
**Renk paleti:** Koyu altın, kan kırmızısı, void siyahı, bozuk beyaz.

**GENEL KONSEPT REFERANS**
**Kaydet:** `ART/mekanlar/act3/act3_konsept_gemini.png`
```
A top-down view of a convergence zone where multiple worlds have been smashed together, bird's eye perspective from directly above.
The floor shows chaotic patchwork of different architectural styles fused together: gothic stone, ancient marble, rusted metal — all cracked with golden light bleeding through every seam.
Everywhere: golden glowing fractures binding the mismatched pieces together, like kintsugi on a cosmic scale.
The overall atmosphere is overwhelming — sacred and profane at once.
Colors: deep gold #C8960C, blood crimson #8B0000, void black #040408, corroded white #C8BEB2.
Retro pixel art style, transparent or dark background.
```

**FLOOR TILE — Kırık Dünya Zemini**
**Kaydet:** `ART/tiles/act3/act3_floor_gemini_ref.png`
```
Pixel art seamless tileable floor tile, 16x16 pixels, top-down 2D view.
Chaotic patchwork floor: two or three different stone/material types fused together with golden glowing cracks at the seams, like kintsugi repair.
Colors: mixed dark stones, golden crack glow #C8960C, void black gaps #040408.
Seamless on all four edges.
```

**WALL TILE — Yakınsama Duvarı**
**Kaydet:** `ART/tiles/act3/act3_wall_gemini_ref.png`
```
Pixel art seamless tileable wall tile, 16x32 pixels, top-down 2D dark fantasy.
Merged wall: gothic stone on the left half fused with rusted metal on the right half, a golden glowing seam running vertically where they meet.
Colors: dark stone #1E1E32, corroded metal #3A2E1E, gold seam glow #C8960C.
Seamless on left and right edges.
```

---

## ACT BAZLI YENİ MOB'LAR

---

### ACT 1 YENİ MOB — HALL SENTINEL (Fractured | Act 1, Elite 64×64)

**Konsept:** Artık var olmayan bir kapıyı hâlâ koruyan taş bekçi. The Fracturing'den önce buradaydı ve hâlâ görevde. Yaklaşılana kadar hareketsiz — sonra ani, ezici saldırı.

**Kaydet:** `ART/dusmanlar/elite_sentinel/sentinel_gemini_base.png`
```
A massive stone guardian statue, top-down bird's eye view from directly above.
The figure is made entirely of dark stone — no flesh, no cloth. Heavy stone plate armor carved directly into the form. A giant stone shield in the LEFT arm, tower shield flat face visible from above. A stone war hammer in the RIGHT arm, head resting on the ground.
Cold blue glowing cracks run through the stone everywhere — signs of dimensional damage.
The figure is perfectly still, like a statue, but clearly ready to move. No face visible, only a carved helmet from above.
Colors: dark stone grey, cold blue crack glow. Transparent background. Dark fantasy elite enemy.
```
Kontrol: tamamen taştan ✓ | kalkan sol ✓ | çekiç sağ ✓ | büyük ve ağır hissettiriyor ✓

---

### ACT 2 YENİ MOB — VOID PHANTOM (Rift-Born | Act 2, Grunt 32×32)

**Konsept:** Void'e tamamen çözülmüş insansı enerji. Duvarlardan kısa süreli geçebiliyor. Geçerken dokunulamaz ama saldıramaz da. Oyuncuyu sürpriz pozisyondan vurmak için kullanıyor.

**Kaydet:** `ART/dusmanlar/grunt_phantom/phantom_gemini_base.png`
```
A humanoid figure completely dissolved into void purple energy, top-down bird's eye view from directly above.
No solid body — only a vague human silhouette made of concentrated void energy, slightly blurry at all edges as if phasing in and out of reality.
The form is entirely deep void purple, with slightly brighter core at center fading to near-invisible edges.
No features, no armor, no weapons visible — only the energy shape of a person.
Colors: void purple #6A0DAD, near-black edges #0A0015. Transparent background. Dark fantasy grunt enemy.
```
Kontrol: tamamen enerji ✓ | belirsiz kenarlar ✓ | insansı ama solid değil ✓ | top-down ✓

---

### ACT 3 YENİ MOB — CONVERGENCE WRAITH (Emergent | Act 3, Elite 64×64)

**Konsept:** Birden fazla yok olmuş dünyanın parçalarından oluşmuş. Bedeni gotik taş, antik metal ve organik doku karışımı — altın çatlaklarla tutturulmuş. Birden fazla medeniyetin saldırı stilini aynı anda kullanıyor.

**Kaydet:** `ART/dusmanlar/elite_wraith/wraith_gemini_base.png`
```
A chaotic humanoid figure assembled from fragments of multiple destroyed worlds, top-down bird's eye view from directly above.
The body is a patchwork: one arm is gothic dark stone, the other is corroded metal, the torso is a mix of both with organic void tissue in the gaps. Golden glowing cracks run through every seam holding the pieces together, like kintsugi on a body.
The overall silhouette is humanoid but asymmetrical and unsettling.
Colors: dark stone grey, corroded metal brown, golden seam glow #C8960C, void black gaps. Transparent background. Dark fantasy Act 3 elite enemy.
```
Kontrol: asimetrik karışık malzeme ✓ | altın çatlak dikişler ✓ | insansı ama bozuk ✓ | top-down ✓

---

## KAYIT YERLERİ ÖZET

| Görsel | Kayıt Yolu |
|--------|-----------|
| Logo | `ART/logo/rima_logo_kaynak.png` |
| Warblade | `ART/karakterler/warblade/warblade_gemini_base.png` |
| Shard Walker | `ART/dusmanlar/grunt_shard/grunt_shard_gemini_base.png` |
| Void Thrall | `ART/dusmanlar/grunt_thrall/grunt_thrall_gemini_base.png` |
| Seam Crawler | `ART/dusmanlar/grunt_seam/grunt_seam_gemini_base.png` |
| Echo Hound | `ART/dusmanlar/grunt_echo/grunt_echo_gemini_base.png` |
| Hollow Mite | `ART/dusmanlar/grunt_mite/grunt_mite_gemini_base.png` |
| Chain Bound | `ART/dusmanlar/grunt_chain/grunt_chain_gemini_base.png` |
| The Twice-Born | `ART/dusmanlar/elite_twiceborn/twiceborn_gemini_base.png` |
| Fracture-Born spawn ref | `ART/dusmanlar/elite_fracture/fracture_gemini_spawn_ref.png` |
| Fracture-Born tam form | `ART/dusmanlar/elite_fracture/fracture_gemini_fullform.png` |
| Spore Hollow | `ART/dusmanlar/elite_spore/spore_gemini_base.png` |
| Shard Brood | `ART/dusmanlar/elite_shardbrood/shardbrood_gemini_base.png` |
| Hollow Cradle | `ART/dusmanlar/elite_cradle/cradle_gemini_base.png` |
| Echo Anchor | `ART/dusmanlar/elite_anchor/anchor_gemini_base.png` |
| Rift Maw | `ART/dusmanlar/static_riftmaw/riftmaw_gemini_base.png` |
| The Wound | `ART/dusmanlar/static_wound/wound_gemini_base.png` |
| Class Mimic (Warblade) | `ART/dusmanlar/special_mimic/mimic_warblade_gemini_base.png` |
| Remnant Host Form 1 | `ART/dusmanlar/special_remnant/remnant_gemini_form1.png` |
| Remnant Host Form 2 | `ART/dusmanlar/special_remnant/remnant_gemini_form2.png` |
| Remnant Host Form 3 | `ART/dusmanlar/special_remnant/remnant_gemini_form3.png` |
| Iron Warden | `ART/dusmanlar/boss/iron_warden/boss_iron_warden_gemini_base.png` |
| Floor tile ref | `ART/tiles/act1/act1_floor_gemini_ref.png` |
| Wall tile ref | `ART/tiles/act1/act1_wall_gemini_ref.png` |
| Crack overlay ref | `ART/tiles/act1/act1_crack_gemini_ref.png` |
| Elementalist | `ART/karakterler/elementalist/elementalist_gemini_base.png` |
| Shadowblade | `ART/karakterler/shadowblade/shadowblade_gemini_base.png` |
| Ranger | `ART/karakterler/ranger/ranger_gemini_base.png` |
| Ravager | `ART/karakterler/ravager/ravager_gemini_base.png` |
| Paladin | `ART/karakterler/paladin/paladin_gemini_base.png` |
| Summoner | `ART/karakterler/summoner/summoner_gemini_base.png` |
| Hexer | `ART/karakterler/hexer/hexer_gemini_base.png` |
| Act 1 konsept | `ART/mekanlar/act1/act1_konsept_gemini.png` |
| Act 1 floor | `ART/tiles/act1/act1_floor_gemini_ref.png` |
| Act 1 wall | `ART/tiles/act1/act1_wall_gemini_ref.png` |
| Act 2 konsept | `ART/mekanlar/act2/act2_konsept_gemini.png` |
| Act 2 floor | `ART/tiles/act2/act2_floor_gemini_ref.png` |
| Act 2 wall | `ART/tiles/act2/act2_wall_gemini_ref.png` |
| Act 3 konsept | `ART/mekanlar/act3/act3_konsept_gemini.png` |
| Act 3 floor | `ART/tiles/act3/act3_floor_gemini_ref.png` |
| Act 3 wall | `ART/tiles/act3/act3_wall_gemini_ref.png` |
| Hall Sentinel | `ART/dusmanlar/elite_sentinel/sentinel_gemini_base.png` |
| Void Phantom | `ART/dusmanlar/grunt_phantom/phantom_gemini_base.png` |
| Convergence Wraith | `ART/dusmanlar/elite_wraith/wraith_gemini_base.png` |
