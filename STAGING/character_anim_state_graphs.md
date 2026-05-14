# Character Animation State Graphs + Prompts

Tarih: 2026-05-13
Karar: rima-design (Opus) önerisi, S68

**Locked rules applied**: Karar #71 (weapon never leaves grip), #99 (weapon in left hand), #108 (no frame count in prompt), #109 (per-class personality idle), #114 (8 directions direct gen), #120 (attack split at apex if >=12f).

**Prompt style**: positive-only, no pixel counts, no frame counts, magnitude words (subtle/gentle/minimal), anatomical weapon grounding, locked anchors stated.

---

## Class 1: Warblade

### Identity
- Weapon: Greatsword (two-handed, primary grip left hand on hilt)
- Personality: Tanky melee, slow heavy slashes
- Idle personality (#109): shoulder roll / pauldron settle — a weary veteran easing his shoulder

### States
- **Base**: low ready stance, greatsword held diagonal across body, left hand on hilt at hip, right hand on blade midway, blade tip resting near left foot
- **AttackApex**: greatsword raised overhead, both hands gripping hilt, blade pointing backward, weight shifted onto back foot, chest open
- **HurtMax**: shoulder recoiled, head tilted slightly down, blade dipping toward ground, left foot bracing back
- **DeathFinal**: kneeling on right knee, greatsword planted blade-down in front, left hand still wrapped around hilt, head bowed

### Animations

#### 1. Idle (loop, Karar #109 shoulder settle)
**Chain**: Base → Base (loop)
**Prompt**: The warrior holds a steady low ready stance with weight even between both feet. His left shoulder rolls back in a slow weary settle, then eases forward again as breath cycles through the chest. The greatsword stays angled across his body with the left hand wrapped on the hilt and the right resting midway on the flat of the blade, tip hovering near his left boot. Head, hips and feet remain anchored; only the shoulders, chest and forearms drift. The mood is patient and tired, like a veteran resting between killings.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop with foot cycle)
**Prompt**: The warrior advances with a heavy plodding step, alternating left and right feet in a grounded cycle. His torso rocks subtly with each footfall and the greatsword sways across his hips, kept firmly in his left hand with the right steadying the blade. Shoulders rise and dip with the weight transfer while the head stays level and forward-facing. Boots strike fully flat without bounce. The loop reads as deliberate, armored advance with no wasted motion.

#### 3. Attack Part 1 — Wind-up (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split, single apex
**Prompt**: The warrior coils into a powerful wind-up, shifting weight onto his back foot and rotating his torso to draw the greatsword up and behind his head. Both hands remain firmly clamped on the hilt with the left leading; the blade arcs from his hip up past his shoulder. Head stays focused forward on the target as the shoulders pull back and the chest opens. Feet stay planted with only the rear heel lifting slightly. The motion reads as deliberate anticipation, gathering force without lunging yet.

#### 4. Attack Part 2 — Downswing (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The warrior unleashes a heavy downward chop, driving the greatsword from overhead through a diagonal arc back down across his body. Both hands stay locked on the hilt with the left leading the swing; the blade sweeps past his right shoulder and finishes resting near his left foot. Weight transfers forward onto the front leg as the shoulders square and the chest closes. Head tracks the blade then lifts to forward gaze. The recovery settles into the low ready, breath visibly heavy.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The warrior takes a blunt impact to the upper chest, his left shoulder snapping backward and head ducking briefly. The greatsword dips with him but stays firmly gripped in his left hand. Feet stay planted with a small brace into the back foot, hips barely shifting. He recovers quickly, shoulders rolling forward and head lifting back to forward gaze. The recoil is sharp but contained, befitting heavy armor.

#### 6. Death (Karar #71 weapon stays gripped)
**Chain**: Base → DeathFinal
**Prompt**: The warrior staggers a single step, then sinks heavily onto his right knee. The greatsword stays clenched in his left hand and is driven point-first into the ground in front of him for support, never leaving his grip. His right hand slides down to also brace the hilt as his torso slumps forward and his head bows toward the pommel. Shoulders settle, chest stills, and the blade remains planted as a final marker. The pose holds without recovery.

---

## Class 2: Hexer

### Identity
- Weapon: Grimoire (left hand, open-faced, glowing pages)
- Personality: Magic caster, page-flip cadence
- Idle personality (#109): grimoire page-turn — fingertip flicks a page, eyes scan

### States
- **Base**: upright stance, grimoire cradled open in left forearm at chest height, right hand hovering above pages, head tilted down reading
- **AttackApex**: right arm extended fully forward palm out, grimoire raised high in left hand above shoulder, head lifted with chin up
- **HurtMax**: torso curled inward, grimoire clutched tight against chest with left hand, right arm shielding face
- **DeathFinal**: collapsed to knees, grimoire cradled tightly in left arm against chest, right hand draped over its cover, head fallen forward

### Animations

#### 1. Idle (loop, Karar #109 page-flip)
**Chain**: Base → Base (loop)
**Prompt**: The hexer stands quietly with the grimoire balanced open across her left forearm at chest height. Her right index finger flicks across a page in a slow scholarly turn, then drifts back to hover above the next line as her eyes track the text. Robe hem and sleeves stir with a gentle drift. Head, hips and feet remain anchored; only the right hand, fingers and brow shift. The mood is contemplative, half-distracted, like a scholar caught mid-thought.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The hexer walks at a measured glide, robe sweeping with each step in soft folds. The grimoire stays cradled open in her left arm against her chest, her right hand resting protectively on its edge so it never closes. Shoulders rise and fall subtly with each footfall and the head stays level, gaze fixed forward. Feet alternate in a smooth heel-toe pattern. The loop reads as cautious, deliberate travel through dangerous places.

#### 3. Attack Part 1 — Channel (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split
**Prompt**: The hexer draws breath and lifts the grimoire upward in her left hand until it rises past her shoulder, pages flaring open. Her right arm sweeps inward toward her chest gathering an unseen current, fingers curling. Her head tilts up and her chest opens as the channel builds. Feet stay planted with weight grounding into the back leg. The pose accumulates pressure without release, robes lifting at the hem.

#### 4. Attack Part 2 — Release (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The hexer thrusts her right palm forward releasing the channeled hex, fingers splayed and arm fully extended. The grimoire descends back to chest level in her left arm, pages still open. Her head snaps level to track the projectile and her shoulders square forward as the chest closes. Weight rolls onto the front foot then settles. Robes flutter back into place as she returns to a poised reading stance.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The hexer flinches inward, curling her torso forward and pulling the grimoire tight against her chest with her left arm. Her right forearm rises to shield her face and her head ducks. Feet stay planted with knees softening. She unfolds quickly, lowering the shielding arm and lifting her head, the grimoire returning to its open cradle. The recoil reads as fragile but defiant.

#### 6. Death (Karar #71 grimoire stays held)
**Chain**: Base → DeathFinal
**Prompt**: The hexer's legs buckle as she sinks to her knees, the grimoire pressed firmly against her chest in her left arm and never falling. Her right hand drapes over its cover as if shielding it. Her torso slumps forward and her head falls until her brow rests on the spine of the book. Shoulders settle, robes pool around her, and the grimoire remains cradled in her grip as the final image.

---

## Class 3: Rivenguard

### Identity
- Weapon: Tower shield (left hand) + short mace (right hand strapped at hip — primary action is shield)
- Personality: Tanky defender, immovable bulwark
- Idle personality (#109): shield brace — re-grips shield strap, plants foot

### States
- **Base**: shield raised in left hand covering torso, mace held low in right hand at hip, feet shoulder-width
- **AttackApex**: shield rammed forward at full extension, left shoulder driving behind it, right hand drawn back with mace cocked
- **HurtMax**: shield pressed close against chest, body braced backward, head ducked behind rim
- **DeathFinal**: fallen onto back propped on left elbow, shield still strapped to left forearm covering chest, mace gripped in right hand resting on ground

### Animations

#### 1. Idle (loop, Karar #109 shield brace)
**Chain**: Base → Base (loop)
**Prompt**: The guardian holds his tower shield steady in his left hand covering most of his torso. He re-grips the leather strap with subtle tension, knuckles flexing, then plants his right boot firmly as if expecting an impact. The short mace hangs ready in his right hand. Head, hips and feet remain anchored except for the planting step; only the left forearm and right wrist shift. The mood is patient, immovable, eyes scanning above the shield rim.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The guardian advances at a steady armored pace, shield held high in his left hand and angled outward. His right hand keeps the mace at hip ready. Each step lands flat and grounded with the torso rocking minimally. Shoulders stay square behind the shield and head remains level and forward. The cycle reads as a wall on the move, controlled and unhurried.

#### 3. Attack Part 1 — Shield wind (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: shield bash 10f total; single apex; split optional, here used
**Prompt**: The guardian coils backward, pulling the tower shield close to his left shoulder and rotating his torso to load the bash. His right hand draws the mace back at hip in counterbalance. Weight gathers onto his back leg, knee bending, head locked forward over the shield rim. Chest tightens as the bash builds. Feet stay planted with only the rear heel rising.

#### 4. Attack Part 2 — Shield slam (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The guardian drives the tower shield forward in an explosive shove, his left shoulder behind it and his full weight transferring onto the front foot. His right hand holds the mace cocked back ready to follow up. Head stays level tracking the target. After the slam he pulls the shield back to cover his torso and settles into the braced stance, breath heavy.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The guardian absorbs a heavy blow, the shield pressing close against his chest as his torso braces backward. His head ducks behind the rim and his right hand grips the mace tighter. Feet stay rooted with knees softening to take the force. He pushes the shield back outward and lifts his head to a forward gaze, recovering with controlled exhale.

#### 6. Death (Karar #71 shield and mace stay gripped)
**Chain**: Base → DeathFinal
**Prompt**: The guardian staggers backward then collapses, falling onto his back propped on his left elbow. The tower shield remains strapped to his left forearm, dragged across his chest as final cover. His right hand still grips the mace, resting heavy on the ground beside him. His head tips back and his chest stills behind the shield. The pose holds with both weapons firmly held.

---

## Class 4: Shrike

### Identity
- Weapon: Twin daggers (left hand primary, right hand secondary — both gripped throughout)
- Personality: Agile melee, twitchy predator
- Idle personality (#109): dagger flip — left wrist spins blade reverse-grip then back

### States
- **Base**: low crouched stance, daggers held in reverse grip down at sides, head forward predatory
- **AttackApex**: mid-pirouette, left dagger extended forward in stab, right dagger raised by ear ready for follow-up
- **HurtMax**: torso twisted sideways, daggers crossed defensively in front of face
- **DeathFinal**: curled on side on the ground, daggers crossed against chest, hands still wrapped around hilts

### Animations

#### 1. Idle (loop, Karar #109 dagger flip)
**Chain**: Base → Base (loop)
**Prompt**: The shrike crouches low and ready, weight forward on the balls of her feet. Her left wrist spins the dagger from reverse grip to forward grip and back in a slow restless flourish, while her right hand holds its blade still at her hip. Her shoulders rise and drop in shallow breath. Head, hips and feet remain anchored; only the left wrist and forearm twirl. The mood is twitchy and predatory, a coiled spring.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The shrike pads forward in a low silent gait, knees bent and steps rolling onto the balls of her feet. Her shoulders lead each stride slightly with a subtle prowl. Daggers stay locked in reverse grip at her sides, blades trailing back along her forearms. Head stays low and forward, eyes scanning. The cycle reads as a stalking hunter, swift and quiet.

#### 3. Attack — single shot (no split, under 12f)
**Chain**: Base → Base
**Prompt**: The shrike spins forward in a sharp half-pirouette, her left dagger stabbing out at full extension as the right rises by her ear for a follow-up. Her torso rotates with the strike and weight pushes onto her front foot. Head whips to track the target then returns forward. She recoils back into a low crouch with both daggers retracted to her sides, ready to strike again.

#### 4. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The shrike twists sideways from the impact, crossing both daggers in front of her face as a reflexive guard. Her shoulders hunch and her head tilts away from the blow. Feet stay planted with knees deepening the crouch. She snaps back upright, uncrossing the blades and resuming the predatory stance, breath fast.

#### 5. Death (Karar #71 daggers stay gripped)
**Chain**: Base → DeathFinal
**Prompt**: The shrike drops to one knee then curls onto her side on the ground. Both daggers stay clenched in her hands and cross against her chest as she folds inward. Her head settles against the floor with the blades framing her collarbone. Shoulders go slack but fingers remain wrapped tight around the hilts. The final image is a curled silhouette holding her weapons close.

---

## Class 5: Lonebow

### Identity
- Weapon: Longbow (left hand holds bow always; right hand draws string)
- Personality: Ranged bow, patient marksman
- Idle personality (#109): arrow nock test — fingers tap quiver, check string tension

### States
- **Base**: upright stance, bow held vertical in left hand at hip, right hand resting at quiver behind right shoulder
- **AttackApex**: full draw, bow extended forward in left hand, right hand pulling string back past cheek, arrow nocked
- **HurtMax**: torso recoiled, bow lowered across chest in left hand, right arm raised in flinch
- **DeathFinal**: slumped sitting against unseen wall, knees bent up, bow laid across lap with left hand still wrapped on the grip

### Animations

#### 1. Idle (loop, Karar #109 quiver tap)
**Chain**: Base → Base (loop)
**Prompt**: The archer stands relaxed with the longbow gripped vertically in his left hand at his hip. His right hand reaches back over his shoulder and his fingers tap the fletching of an arrow in the quiver, then drift to test the bowstring's tension with a light pluck. Shoulders rise and fall in slow breath. Head, hips and feet remain anchored; only the right arm and fingertips drift. The mood is patient and watchful, like a hunter waiting for prey.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The archer moves forward at a careful, balanced pace, bow held vertical in his left hand at his side and right hand swinging gently near the quiver. His torso stays upright with subtle shoulder roll for each step. Head stays level and scanning, feet alternate in a quiet rolling step. The cycle reads as a forest scout: alert, soft-footed, ready to draw.

#### 3. Attack Part 1 — Draw (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split (full draw-fire is 12f, apex at full draw)
**Prompt**: The archer raises the longbow forward in his left arm to a fully extended horizontal hold. His right hand pulls an arrow from the quiver, nocks it, and draws the string back past his cheek with elbow rising. His torso rotates slightly to a shooting angle and his head turns to sight along the shaft. Feet stay planted with weight settling onto the back leg.

#### 4. Attack Part 2 — Release (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The archer releases the string, his right hand snapping back past his ear in clean follow-through. The bow stays gripped in his left hand and quivers forward with the recoil. His head tracks the arrow's flight then resets forward. He lowers the bow back to his hip and returns his right hand toward the quiver, settling into ready stance.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The archer recoils backward, the longbow swinging down across his chest in his left hand. His right arm rises in a flinching guard near his face and his head ducks. Feet stay planted with knees softening. He uncurls quickly, lowering the right arm and bringing the bow back upright at his hip, gaze restored forward.

#### 6. Death (Karar #71 bow stays gripped)
**Chain**: Base → DeathFinal
**Prompt**: The archer staggers backward and slides down to a seated slump as if catching against a wall. His knees bend up and the longbow lays across his lap, his left hand never releasing the grip. His right hand falls limp at his side, fingers grazing the bowstring. His head tips forward onto his chest. Shoulders settle and the bow remains held across him as the final image.

---

## Class 6: Pyrelance

### Identity
- Weapon: Polearm with flame-tipped head (left hand forward on shaft, right hand back on butt)
- Personality: Polearm zoner, controlled aggression
- Idle personality (#109): butt-tap rhythm — taps polearm butt on ground in slow tempo

### States
- **Base**: upright stance, polearm held diagonal across body, left hand forward gripping mid-shaft, right hand on the butt resting near right hip, flame-tip pointing upper-left
- **AttackApex**: polearm fully thrust forward, left hand extended at front grip, right hand cocked back at chest, body twisted into the line
- **HurtMax**: polearm pulled back across chest defensively in left hand, right arm bracing high
- **DeathFinal**: kneeling forward, polearm planted butt-down on the ground in front, left hand high on the shaft, right hand sliding down it for support

### Animations

#### 1. Idle (loop, Karar #109 butt-tap rhythm)
**Chain**: Base → Base (loop)
**Prompt**: The lancer stands tall with the polearm angled diagonally across her body, left hand mid-shaft, right hand on the butt near her hip. She lifts the butt of the polearm and taps it lightly against the ground in a slow steady rhythm, the shaft rocking subtly in her grip. Shoulders rise and fall with breath. Head, hips and feet remain anchored; only the right hand and forearm drive the tap. The mood is measured and martial, marking time like a sentinel.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The lancer strides forward, the polearm carried diagonally across her body with the flame-tip leading high in her left hand and the butt trailing low in her right. Her torso rocks lightly with each step and the shaft sways with controlled rhythm. Head stays level and forward, feet alternate in a confident march. The loop reads as disciplined formation pace.

#### 3. Attack Part 1 — Pull-back (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split (full thrust 12f)
**Prompt**: The lancer pulls the polearm back along her right side, sliding her left hand toward her right hand on the butt to load the thrust. Her torso rotates sideways presenting a narrow profile and her shoulders coil. The flame-tip aims forward like a held breath. Weight shifts onto the back foot, knee bending. The pose gathers spear energy without release.

#### 4. Attack Part 2 — Thrust (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The lancer drives the polearm forward in a full extension thrust, her left hand sliding back along the shaft to lead the strike and her right hand pushing from the butt at her chest. Her torso rotates square to the target and her weight transfers onto the front foot. Head tracks the tip then settles forward. She retracts the shaft back across her body to the ready stance.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The lancer recoils and pulls the polearm back across her chest defensively, left hand sliding up the shaft and right hand bracing high near her shoulder. Her head ducks and her torso curls slightly. Feet stay planted with knees softening. She unfolds quickly, lowering the shaft back to its diagonal carry, head lifting to forward gaze.

#### 6. Death (Karar #71 polearm stays gripped)
**Chain**: Base → DeathFinal
**Prompt**: The lancer falls forward onto one knee and plants the polearm butt-down into the ground in front of her for support. Her left hand stays high on the shaft and her right hand slides down to join it, both clinging to the weapon. Her torso slumps against the shaft and her head bows toward her hands. The polearm stands as a final marker, never falling from her grip.

---

## Class 7: Rotwidow

### Identity
- Weapon: Curved scythe (left hand on lower haft, right hand higher on the snath, blade hooks upper-left)
- Personality: Scythe reaper, slow swooping arcs
- Idle personality (#109): scythe lean — leans weight against the snath like a walking staff, sways

### States
- **Base**: standing slightly hunched, scythe held vertical with butt on the ground in left hand at lower haft, right hand resting higher on the snath, blade hooking outward to the upper-left
- **AttackApex**: scythe swept across the body at full horizontal sweep, blade trailing on the far side, left hand at the butt and right hand mid-haft, torso fully rotated
- **HurtMax**: scythe pulled in close vertically, both hands high on the snath, torso curled around it
- **DeathFinal**: collapsed forward draped over the scythe haft on the ground, left hand still wrapped around the lower haft, blade hook beside her head

### Animations

#### 1. Idle (loop, Karar #109 scythe lean)
**Chain**: Base → Base (loop)
**Prompt**: The reaper stands slightly hunched, the scythe planted vertically beside her like a walking staff. Her left hand grips the lower haft and her right hand drapes over the snath higher up. She rocks her weight gently against the shaft, shoulders swaying side to side as if listening to a distant whisper. Head, hips and feet remain anchored; only the torso and shoulders drift. The mood is uncanny and patient, a graveyard wind.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The reaper shuffles forward with a slow gliding gait, the scythe trailing slightly behind her shoulder in both hands, butt lifted off the ground. Her left hand holds low on the haft and her right hand mid-snath, the blade hooking back over her shoulder. Her torso rocks subtly with each step and her head tilts forward. The cycle reads as a creeping reaper, slow and inevitable.

#### 3. Attack Part 1 — Wind (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split (full sweep 12f)
**Prompt**: The reaper coils to her right, drawing the scythe far back behind her right shoulder. Her left hand slides up the haft and her right hand drops low, loading the wide arc. Her torso twists fully and her weight settles onto the back foot. The blade hangs in the air like a held breath. Head tracks the target with cold focus.

#### 4. Attack Part 2 — Sweep (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The reaper unleashes a wide horizontal scythe sweep across the front of her body, blade arcing through space at chest height. Her left hand drives at the butt and her right hand pivots mid-haft as the torso rotates fully through the swing. Weight rolls onto the front foot. The follow-through carries the blade past her left side, then she plants the scythe back upright into the resting stance.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The reaper pulls the scythe in tight against her body, both hands gripping high on the snath with the blade vertical above her head. Her torso curls around the haft and her head ducks. Feet stay planted with knees folding inward. She unfurls slowly, lowering the scythe back to its planted resting position with the same haunted calm.

#### 6. Death (Karar #71 scythe stays gripped)
**Chain**: Base → DeathFinal
**Prompt**: The reaper sinks forward as if exhaling her last breath, draping her torso across the scythe haft as it tilts to the ground. Her left hand stays wrapped around the lower haft and her right hand slides along the snath above her head. The blade hook lies on the ground beside her face like a closing parenthesis. Shoulders settle, the scythe remains in her grip as the final image.

---

## Class 8: Hollowcaller

### Identity
- Weapon: Bone horn / summoning relic (left hand holds horn; right hand commands)
- Personality: Summoner, ritual conductor
- Idle personality (#109): horn polish — wipes horn rim with thumb, breathes on it

### States
- **Base**: upright stance, bone horn held in left hand at chest height with mouthpiece near collarbone, right hand spread open at hip palm out
- **AttackApex**: horn raised to lips with left hand, right hand thrust forward palm-out commanding, torso leaned slightly back as if echo expanding
- **HurtMax**: torso curled forward, horn clutched close to chest in left hand, right arm crossed defensively over horn
- **DeathFinal**: kneeling, horn held to chest in left hand, right hand draped over horn rim as if cradling, head bowed forward

### Animations

#### 1. Idle (loop, Karar #109 horn polish)
**Chain**: Base → Base (loop)
**Prompt**: The caller stands quietly with the bone horn cradled in his left hand at chest height. His right thumb traces the rim of the horn in a slow polishing circle, then his lips part as he breathes warm air across the mouthpiece. Shoulders rise and fall gently with breath. Head, hips and feet remain anchored; only the right hand and brow drift. The mood is ceremonial and intimate, like preparing an instrument before a rite.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The caller walks forward with a measured ritual pace, the bone horn cradled close to his chest in his left hand and his right hand swinging gently at his side. His torso stays upright with minimal sway and the horn rocks lightly against his collarbone. Head stays level and forward, feet alternate in a quiet rolling step. The loop reads as a priest walking among ruins.

#### 3. Attack Part 1 — Inhale (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split (full call 12f)
**Prompt**: The caller raises the bone horn to his lips with his left hand, his chest expanding deeply as he draws breath. His right hand sweeps inward gathering presence, fingers curling near his sternum. Head tilts back slightly and torso leans backward, gathering the call. Feet stay planted with weight grounding low. The pose builds resonance like a held inhalation.

#### 4. Attack Part 2 — Call (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The caller blows the bone horn with full breath, his torso pushing forward and his right hand thrusting out palm-up to direct the summoned echo. The horn stays gripped firmly in his left hand at his lips. His head levels and his shoulders square forward. Weight transfers onto the front foot. As the call fades he lowers the horn back to his chest and his right hand returns to his hip.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The caller curls forward over the bone horn, pulling it tight against his chest with his left hand and crossing his right arm over it protectively. His head ducks and his shoulders hunch around the relic. Feet stay planted with knees softening. He uncurls slowly, lowering the right arm and lifting the horn back to chest-cradle, head returning to forward gaze.

#### 6. Death (Karar #71 horn stays held)
**Chain**: Base → DeathFinal
**Prompt**: The caller sinks to his knees, the bone horn pressed firmly against his chest in his left hand. His right hand wraps over the horn's rim cradling it. His torso slumps forward and his head bows until his brow touches the mouthpiece. Shoulders settle, robes pool around him, and the horn remains held against his heart as the final image.

---

## Class 9: Veilbinder

### Identity
- Weapon: Two chain-linked sickles (left hand holds primary sickle; right hand holds the chain spool that connects to the second sickle which orbits/returns)
- Personality: Dual-wielder with chain mechanic, whirling control
- Idle personality (#109): chain spin — right hand slowly rotates the chain in a small loop at hip level

### States
- **Base**: upright stance, primary sickle held forward in left hand at hip with blade curling forward, secondary sickle hanging by short chain from right hand at right hip, chain slack
- **AttackApex**: primary sickle hooked forward in left hand at full extension; secondary sickle thrown out wide on extended chain in right hand, arms spread cruciform
- **HurtMax**: torso curled inward, both sickles pulled close crossed in front of chest, chain wrapped around forearms
- **DeathFinal**: collapsed on side, both sickles still in hands and tangled across body with chain pooled around her

### Animations

#### 1. Idle (loop, Karar #109 chain spin)
**Chain**: Base → Base (loop)
**Prompt**: The binder stands ready with the primary sickle held forward in her left hand and the secondary sickle dangling from a short chain in her right hand at her hip. Her right wrist slowly rotates the chain in a small lazy loop, the secondary sickle tracing a circle near her thigh. Shoulders rise and fall with breath. Head, hips and feet remain anchored; only the right wrist and the chained sickle's arc drift. The mood is restless and rhythmic, a kept threat.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The binder advances with a fluid prowl, primary sickle leading low in her left hand and secondary sickle trailing at her right hip on its slack chain. Her shoulders roll subtly with each step and the chain swings in light pendulum rhythm. Head stays forward and level, feet alternate in a soft rolling step. The loop reads as a confident predator with leashed weapons.

#### 3. Attack Part 1 — Throw (Base → AttackApex)
**Chain**: Base → AttackApex
**Note**: Karar #120 split (full throw-and-return 12f)
**Prompt**: The binder coils her torso and whips the secondary sickle outward on its chain from her right hand, the blade flying wide in a horizontal arc. Her left arm raises the primary sickle into a high guard ready position. Her shoulders open into a cruciform spread and her weight shifts onto the back foot. The chain extends fully as the throw reaches its apex.

#### 4. Attack Part 2 — Recall and slash (AttackApex → Base)
**Chain**: AttackApex → Base
**Prompt**: The binder yanks the chain back with her right hand, recalling the secondary sickle in a return arc as her left hand sweeps the primary sickle forward in a hooking slash. Both blades close on the centerline before she pulls them back to her hips. Weight transfers onto the front foot then settles. The chain coils back to slack and she returns to the ready stance.

#### 5. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The binder curls inward, pulling both sickles up and crossing them in front of her chest as the chain loops around her forearms. Her head ducks and her shoulders hunch tight. Feet stay planted with knees softening. She uncrosses the blades slowly, lowering the primary back to her left side and letting the chain unspool at her right, returning to the prowling stance.

#### 6. Death (Karar #71 both sickles stay gripped)
**Chain**: Base → DeathFinal
**Prompt**: The binder buckles sideways and collapses onto her side, both sickles still clenched in her hands as the chain pools tangled around her body. The primary stays in her left fist near her chest and the secondary rests in her right hand by her hip. Her head settles to the floor and the chain forms a loose halo. Shoulders go slack, fingers stay locked on the hilts as the final image.

---

## Class 10: Sparkbreech

### Identity
- Weapon: Hand-cannon / arcane pistol (left hand grip; right hand braces underneath or works the hammer)
- Personality: Gun-mage hybrid, percussive caster
- Idle personality (#109): hammer-check — right thumb tests the hammer of the pistol, pulls it half-cock and lets it down

### States
- **Base**: upright stance, hand-cannon held in left hand at hip pointed down-forward, right hand resting on top of the barrel with thumb near hammer
- **AttackApex**: hand-cannon thrust forward at full extension in left hand at shoulder height, right hand fanning the hammer, recoil flare implied
- **HurtMax**: torso recoiled, hand-cannon pulled across chest in left hand, right arm raised to face in flinch
- **DeathFinal**: slumped sitting, knees bent, hand-cannon resting in lap in left hand barrel-up, right hand draped over the grip beside the left

### Animations

#### 1. Idle (loop, Karar #109 hammer-check)
**Chain**: Base → Base (loop)
**Prompt**: The gunner stands ready with the hand-cannon gripped in her left hand at hip level pointed down-forward. Her right thumb slowly draws the hammer back to half-cock, holds it for a beat, then eases it back down with a controlled click. Her shoulders rise and fall in slow breath. Head, hips and feet remain anchored; only the right thumb and brow drift. The mood is wary and precise, a duelist counting heartbeats.

#### 2. Walk (loop, south)
**Chain**: Base → Base (loop)
**Prompt**: The gunner moves forward at a wary pace with the hand-cannon held low in her left hand and her right hand swinging near her belt. Her torso stays upright with subtle shoulder roll and the pistol's weight rocks gently at her hip. Head stays level and scanning, feet alternate in a careful rolling step. The cycle reads as a frontier mage advancing on a target.

#### 3. Attack — single shot (no split, under 12f)
**Chain**: Base → Base
**Prompt**: The gunner raises the hand-cannon in her left hand to shoulder height in a swift extension, her right hand fanning the hammer down with a sharp percussive motion. Her torso rotates slightly to a shooting stance and her head ducks behind the barrel to sight. Weight settles onto the back foot as the implied recoil pushes through her shoulder, then she lowers the pistol back to her hip and squares forward.

#### 4. Hurt
**Chain**: Base → HurtMax → Base
**Prompt**: The gunner recoils backward, the hand-cannon pulled across her chest in her left hand as her right arm rises in a flinching guard near her face. Her head ducks and her shoulders hunch. Feet stay planted with knees softening. She uncurls quickly, lowering the right arm and bringing the pistol back to her hip, head returning to forward gaze.

#### 5. Death (Karar #71 pistol stays gripped)
**Chain**: Base → DeathFinal
**Prompt**: The gunner staggers backward and slides down to a seated slump, knees bending up in front of her. The hand-cannon stays clenched in her left hand and comes to rest in her lap barrel pointing upward. Her right hand drapes over the grip beside her left, both still wrapped around the pistol. Her head tips forward onto her chest. Shoulders settle and the weapon remains held across her lap as the final image.

---

## DECISION SUMMARY (Karar #122 anim variant)

**10 classes** (Warblade, Hexer, Rivenguard, Shrike, Lonebow, Pyrelance, Rotwidow, Hollowcaller, Veilbinder, Sparkbreech) with **4 standardized states** each (Base, AttackApex, HurtMax, DeathFinal) and **5-6 animations apiece** (Idle, Walk, Attack [split per Karar #120 where >=12f], Hurt, Death).

**Rationale:** Per-class personality idle (Karar #109) keeps loops distinct. All deaths satisfy Karar #71 (weapon stays held/cradled/planted). All weapons in left hand (Karar #99). 8/10 classes use Karar #120 split for attack (>=12f single apex). Shrike + Sparkbreech kept single-shot.

**Gen budget impact:** User runs prompts manually through PixelLab "enhance with AI". Per anim ~8 frame × 1 dir initially = ~8 gen. 10 classes × 5 anims × 8 frame = 400 gen for full S-direction set. 8-direction (Karar #114) = 3200 gen. User's 800 gen budget supports either: (a) full S-only for all classes + spare, or (b) ~2 main classes 8-dir + remaining S-only.
