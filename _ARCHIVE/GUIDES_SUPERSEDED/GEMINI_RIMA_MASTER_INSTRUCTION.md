# GEMINI RIMA MASTER INSTRUCTION

Gemini'ye bu dosyadaki `PROMPT START` ve `PROMPT END` arasını tek parça ver.
Amaç: PixelLab için doğru açıda, class kimliği net, tutarlı concept/reference görseller üretmek.

```text
PROMPT START
You are generating CHARACTER REFERENCE IMAGES for a Unity 2D roguelite project (RIMA).
Follow all constraints strictly.

CORE GOAL
- Produce 10 separate character concept/reference images (one per class).
- These are references for PixelLab production, not final in-game sprites.
- Keep forms, silhouette, and camera consistency above all.

CAMERA LOCK (MANDATORY)
- High overhead top-down camera, steep bird's eye view, around 75-80 degree downward angle.
- Top of head clearly visible.
- Body foreshortened from above.
- Full body readable.
- Volumetric body forms with visible depth and thickness.
- Layered clothing/armor depth.
- Not isometric, not side-view, not front-view portrait.
- Not flat, not paper-thin, not cutout-like, not sticker-like.

STYLE LOCK
- Grounded worn dark-fantasy materials (functional, practical, battle-worn).
- Avoid extreme grimdark horror mood.
- Avoid flashy arcade look.
- Mature realistic proportions (not chibi).
- Clear gameplay-first silhouette.

ARMOR RESTRAINT LOCK (CRITICAL)
- Warblade and Ravager must NOT read as full-plate knight/tank.
- Use partial armor only: mixed plate + cloth + leather + visible chain/gambeson gaps.
- Keep arms/waist/joints readable and mobile, not fully encased.
- Practical worn combat gear, not ceremonial or holy knight armor.

HEADGEAR LOCK (ABSOLUTE)
- No helmets on any class. Zero exceptions.
- No full-face coverings, no closed visors, no metal masks.
- Faces must remain readable from the high top-down angle.

NEGATIVE RULES (GLOBAL)
- Do not use or imply: "3/4", "slightly tilted", "60-65 degree", "Low Top-Down".
- No pure top-down flat token look.
- No oversized toy-like weapons.
- No dramatic splash-art composition.
- No giant VFX clouds hiding the silhouette.
- No full-plate paladin silhouette.
- No over-armored tank body that hides cloth/leather layers.
- No helmets, no visors, no face-concealing headgear.
- No elderly face treatment for non-elder classes.
- No gender drift against class gender lock.

GENDER LOCK (STRICT)
- Male: Warblade, Brawler, Ravager, Ronin, Shadowblade
- Female: Elementalist, Gunslinger, Hexer, Ranger, Summoner

CLASS SPECS (STRICT)
1) Warblade (Male)
- Fallen martial-order veteran.
- Heavy two-handed greatsword.
- Partial scavenged armor only: chest/shoulder plate pieces over visible cloth + chain, worn dark crimson battle wrap.
- Bare head only (no helmet), not fully enclosed armor shell.
- Mid-age battle veteran read (around 30s-40s), not elderly old-man face.
- Subtle cold-blue hairline fractures on blade only.

2) Brawler (Male)
- Reinforced gauntlets/fists.
- Muscular close-combat build, practical rough gear.
- Controlled void-purple glow on gauntlets/tattoos (minimal, readable).
- Must read clearly male from face/body silhouette.

3) Elementalist (Female)
- Lightning electric orb raised in one hand.
- Blue-purple hooded robe, cyan rune patterns.
- Clean caster silhouette.
- No staff, no wand, no second weapon in off-hand.

4) Gunslinger (Female)
- Rift-tech pistol duelist — not western cowboy aesthetic.
- Dual modified pistols (both hands, not single revolver), cold-silver rift trim with subtle heat vent detail on grips.
- Dark fitted combat jacket with tactical straps (no long western coat, no wide-brim cowboy hat — bare head or minimal short brim only).
- Kinetic mid-stride or slide pose (not static aiming stance).
- Must read clearly female from facial structure and body silhouette.
- No magic glow on hands/body; rift-industrial aesthetic, not fantasy-magic.

5) Hexer (Female)
- Staff user, slightly hunched posture.
- Ragged dark-purple robes.
- Cursed green flame + void-purple skull motif on staff.

6) Ranger (Female)
- Tactical rift hunter from ruins/dungeons — NOT forest archer.
- Charcoal-slate practical leather layers, minimal green.
- Low tactical cowl and lower-face wrap (no large fantasy hood).
- Asymmetrical utility belt with trap canisters and tether spool.
- Long thin rift-etched longbow, cold-blue only on arrow tips.
- Forward-lean stalker posture (not heroic wide-leg archer pose).

7) Ravager (Male)
- Brutal raider silhouette with partial heavy pieces: spiked pauldrons/bracers/belt armor over exposed leather/cloth body sections.
- Not full plate juggernaut; keep savage mobility and readable body mass.
- Huge double-headed axe.
- Blood-red rage aura accent (#8B1A1A), controlled and readable.

8) Ronin (Male)
- Right katana extended forward in draw-cut, left hand at guard hip (asymmetric dual-draw — overhead readability over both-blades-raised).
- Topknot, layered worn blue-green robes with asymmetrical armor pieces.
- Cold silver-blue shimmer on blade edges only.

9) Shadowblade (Male)
- Dual blades, standing coiled forward-lean stance (NOT flat ground crouch — overhead readability priority).
- Black-purple tactical suit with gray armor plates.
- Controlled void-purple smoke at blade edges only (not a body-obscuring cloud).
- Subtle eye glow (not full anime-style bright glowing face).

10) Summoner (Female)
- Command scepter in one hand raised forward, other hand extended in directional control gesture (asymmetric pose — clearer at overhead angle than both-hands-raised).
- Worn ornate purple-gold robes adapted for battlefield movement (practical field commander, not throne-room ritual mage).
- Cold-blue arcane circle at control hand only.
- Field commander identity, not static summoning ritual pose.

OUTPUT REQUIREMENTS
- Generate 10 images total, one per class.
- Keep camera lock consistent across all 10.
- Keep silhouette readable with plain or transparent-like neutral background.
- Add a short text note under each image:
  - "camera lock check: passed/failed"
  - "silhouette check: passed/failed"

PROMPT END
```
