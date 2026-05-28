# Character Idle (Weaponless) Prompts -- LOCK S74

**Date:** 2026-05-14 S74
**Source:** rima-design Opus judgment (sub-agent dispatch a22f8def)
**Status:** LOCKED for PixelLab Create Image Pro batch
**Supersedes:** STAGING/character_idle_weaponless_prompts_LOCK.md (S73) -- K4 prefix updated, all pose/motion/mood content preserved

---

## Honored Kararlar

- Karar #71 -- positive-only spec; grip geometry preserved; no negative phrases
- Karar #80 -- Class Silhouette Bible (10 class identity canon)
- Karar #99 -- silah gorunurlugu/siluet kuralı; hand mention YASAK in prompts
- Karar #100 -- chibi 64x64 proportions, PPU=64
- Karar #108 -- Custom V3 hard rules (4-16 frame, 3 gen/dir for anim; CIP = single south frame here)
- Karar #109 -- per-class ambient idle personality
- Karar #123 -- Yol A weapon decouple (body unarmed; mob exception noted)
- Karar #124 -- Faz 1 weapon scope: Warblade Base + T2 Rift only

---

## K1-K4 LOCKED Decisions (summary)

### K1: Silahsiz Idle Pose per Class
Each class gets a motion-only pose with grip geometry preserved (hands carry the shape of the absent weapon). Pose is south-facing single frame. No weapon in frame.

### K2: Weapon Prompt Approach
Create Image Pro per-weapon (NOT style-sheet / NOT style reference). Precision over speed. Each weapon gets an explicit silhouette pose. Style anchor: Characters/anchors/reference.png.

### K3: Weapon Sprite Sizes (final -- see weapons_pixel_sizes_LOCK_S74.md)
Superseded and moved to dedicated weapon LOCK file. J3 Opus table governs.

### K4: Universal Anchor Reference Instruction (NEW -- replaces S73 K4)
Each class prompt begins with this prefix (J1 from Opus judgment):

> Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose.

---

## 10 Class Weaponless Idle Prompts

> Format: K4 prefix + pose statement + motion verbs + locked parts + mood
> Anchor file: Characters/anchors/{class_name}.png
> Output: 64x64 chibi, south-facing single frame, transparent background

---

### 1. Warblade

- **Anchor:** Characters/anchors/warblade.png
- **Pose:** Low guard stance, both hands curled at chest level (two-hand grip geometry, no weapon)
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The fighter holds a low guard stance, body angled forward slightly, both hands curled at chest level holding empty horizontal space as if cradling a long object. Chest rises and falls in a slow breathing rhythm while shoulders shift subtly with the inhale. Head, hips, and feet remain fixed; only the torso breathing and shoulder roll animate. Mood is grounded, patient, dangerous.

---

### 2. Ranger

- **Anchor:** Characters/anchors/ranger.png
- **Pose:** Tactical hunter stance, one hand curled at thigh (vertical grip geometry), other loose at side
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The hunter stands in a tactical scout pose, one hand curled at the thigh holding empty vertical space, the other hand loose at the side. Torso breathes shallowly, head tilts a degree as if listening to distant motion. Hips, feet, and arms below the wrist remain fixed; only chest, shoulders, and head animate. Mood is alert, patient, predator-still.

---

### 3. Shadowblade

- **Anchor:** Characters/anchors/shadowblade.png
- **Pose:** Slim upright, both hands curled near torso palms inverted (reverse-grip geometry), elbows tucked back
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The assassin stands slim and upright, both hands curled near the torso with palms turned inward and elbows tucked back. Body weight rocks almost imperceptibly forward and back, head dips a fraction with each weight shift. Hips, feet, and lower arms remain fixed; only the body sway and head dip animate. Mood is coiled, predatory, silent.

---

### 4. Elementalist

- **Anchor:** Characters/anchors/elementalist.png
- **Pose:** Open casting stance, one palm upward at chest level (empty palm, disc absent), other loose
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The mage stands in an open casting stance, one palm faced upward at chest level holding empty cradled space, the other hand loose at the side. Fingers flex faintly as if calling power, chest rises slowly with a deep breath. Hips, feet, and the lowered arm remain fixed; only the upper palm fingers, chest, and a subtle head lift animate. Mood is curious, focused, ember-bright.

---

### 5. Ravager

- **Anchor:** Characters/anchors/ravager.png
- **Pose:** Aggressive forward crouch, both hands curled at hip level (dual short-handle geometry), shoulders rolled forward
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The berserker hunches in an aggressive forward crouch, both hands curled at hip level holding empty short-handle space, shoulders rolled forward in barely contained menace. Shoulders rise and clench with each heavy breath while head tilts low and forward. Hips, feet, and curled hands remain fixed; only the shoulders, neck, and head animate. Mood is furious, restrained, about-to-snap.

---

### 6. Ronin

- **Anchor:** Characters/anchors/ronin.png
- **Pose:** Iaido draw stance, body sideways, one hand curled at right hip (draw-ready), other on empty scabbard at left hip -- scabbard sprite stays on body (Karar #123 exception)
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The duelist stands in an iaido draw posture, body angled three-quarter sideways, one hand curled at the right hip in draw-ready geometry, the other resting on the empty sheath at the left hip (retain a plain sheath silhouette at the left hip). Chest breathes with quiet discipline, head holds a gentle meditative tilt. Hips, feet, and both hand positions remain fixed; only chest, shoulders, and a subtle head sway animate. Mood is composed, lethal-calm, mid-meditation.

---

### 7. Gunslinger

- **Anchor:** Characters/anchors/gunslinger.png
- **Pose:** Duellist micro-crouch, both hands curled at hip level (dual vertical handle geometry), weight on one leg
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The duellist stands in a gentle micro-crouch with weight shifted onto one leg, both hands curled at hip level holding empty vertical handle space. Shoulders roll lightly with breathing, head turns a few degrees as if scanning for a draw moment. Hips, feet, and curled hand geometry remain fixed; only the shoulders, head turn, and chest animate. Mood is cocky, ready, hair-trigger.

---

### 8. Brawler

- **Anchor:** Characters/anchors/brawler.png
- **Pose:** Boxing guard, both fists clenched at chin level (bare-fisted class -- no decouple needed)
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The fighter holds a boxing guard, both fists clenched and raised to chin level, knees softly bent in a fight stance (retain the bare-fisted nature -- clenched fists held in pure boxing readiness). Fists bob in a gentle rhythmic feint, shoulders roll with the bounce, head bobs in counter-rhythm. Hips and feet remain planted; only fists, shoulders, and head animate. Mood is hungry, restless, gleeful.

---

### 9. Summoner

- **Anchor:** Characters/anchors/summoner.png
- **Pose:** Orchestra conductor pose, one hand curled at chest (vertical handle geometry -- lantern absent), other raised palm-open in directing gesture
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The summoner stands in an orchestra-conductor pose, one hand curled at chest level holding empty vertical handle space, the other raised palm-open in a gentle directing gesture. The conducting palm fingers flex faintly as if calling unseen voices, chest rises and falls deeply. Hips, feet, and the chest-held hand remain fixed; only the raised palm fingers, chest, and shoulder lift animate. Mood is reverent, melancholic, communing-with-spirits.

---

### 10. Hexer

- **Anchor:** Characters/anchors/hexer.png
- **Pose:** Curse-bearer stance, one arm bent across chest cradling empty book-shaped space (grimoire geometry kept as passive body posture), other hand curled at side (vertical pole geometry -- staff absent)
- **Note:** Grimoire is a passive body accessory (body posture only), NOT a separate weapon sprite (Karar #18 + #123). Staff is the Hexer weapon sprite.
- **Prompt:**

Reference image shows the Warblade character at the exact 35-degree high top-down camera angle, chibi 64x64 proportions, and true south facing direction that this NEW character must match; replicate ONLY the camera angle, proportions, and facing -- do not copy the warrior's outfit, armor, weapon, build, or pose. The hexer stands in a curse-bearer pose, one arm bent across the chest cradling empty book-shaped space, the other hand curled at the side holding empty vertical pole space. Breathing is slow and heavy, head tilts down toward the cradled space as if reading something invisible. Hips, feet, and both hand geometries remain fixed; only chest, head tilt, and a gentle shoulder shrug animate. Mood is brooding, ancient, mid-incantation.

---

## Generation Count Summary

| Stage | Count | Tool | Notes |
|---|---|---|---|
| Character base (silahsiz south) | 10 class | Create Image Pro | This file |
| Mob base (silahsiz south) | 6 mob | Create Image Pro | See new_mobs_64px_LOCK_S74.md |
| Weapon sprites Faz 1 | 11 | Create Image Pro | See weapons_pixel_sizes_LOCK_S74.md |
| **Total Create Image Pro** | **27** | | |

**Estimated credit:** 27 x ~6 = ~162 credit

---

## QC Checklist

- [ ] No class prompt contains weapon/sword/blade/bow/staff/lantern/pistol/hatchet/katana
- [ ] No class prompt contains "left hand / right hand / his hand / her hand" (Karar #99)
- [ ] Every class prompt begins with the K4 S74 NEW prefix
- [ ] 10 class prompts total
- [ ] Ronin: plain sheath silhouette at left hip noted (Karar #123 exception)
- [ ] Brawler: bare-fisted guard retained in body sprite (no decouple)
- [ ] Hexer: grimoire passive body posture only, staff is weapon sprite
- [ ] Anchor file paths are canonical: Characters/anchors/{class_name}.png
