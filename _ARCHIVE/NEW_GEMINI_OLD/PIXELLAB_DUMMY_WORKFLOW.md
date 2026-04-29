# RIMA PixelLab Dummy-First Workflow (Batch Ready)

Bu dosya, `master_dummy_south_lock1_128.png` kullanarak tum classlari tutarli aci/poz ile uretmek icindir.
Tum classlarda ayni kural: `GLOBAL LOCK + CLASS DELTA + NEGATIVE LOCK`.

## Base
- Base image: `F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\new\new_gemini\master_dummy_south_lock1_128.png`
- Output folder: `F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\new\new_gemini`

## GLOBAL LOCK (Aynen kullan)
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.
```

## NEGATIVE LOCK (Aynen kullan)
```text
no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

## PixelLab Steps (Her class icin ayni)
1. Base image'i yukle.
2. Image Edit / Inpaint moduna gec.
3. Promptu su formatta ver:
- GLOBAL LOCK
- CLASS DELTA
- NEGATIVE LOCK
4. Ciktiyi kontrol et: bas/omuz/ayak yerleri base ile ayni kalmali.
5. Kayma varsa reject edip tekrar uret.

## Batch Prompts (Copy/Paste)

### 1) WARBLADE
Save as: `warblade_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Male fallen warblade from a broken martial order.
Bare head, weathered mid-age face (30s-40s, not old, no gray hair).
Worn dark plate + chain + cloth layers, dark crimson battle wrap.
Greatsword low guard in front of body, two-handed grip, grounded stance.
Subtle cold-blue hairline cracks only on blade metal.
No holy knight read, no full parade plate.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 2) ELEMENTALIST
Save as: `elementalist_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Female battlefield elementalist.
Practical blue-purple layered robes with cyan rune trims.
One raised lightning orb above open palm.
No staff, no wand, no second weapon.
Controlled magic intensity, no giant storm effects.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 3) SHADOWBLADE
Save as: `shadowblade_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Male void assassin.
Dark purple-black tactical armor with gray plates.
Lower-face shadow wrap with eyes visible.
Dual short void blades in low-ready outward angles.
Thin void-purple wisps only on blade edges.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 4) RANGER
Save as: `ranger_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Female tactical rift hunter (not forest elf archer).
Charcoal-slate practical gear, trap canisters and tether spool visible.
Longbow with one nocked arrow.
Cold-blue accent only on arrow tip.
No bright green ranger styling.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 5) GUNSLINGER
Save as: `gunslinger_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Female rift-tech duelist.
Long copper-orange hair clearly visible.
Dark fitted asymmetric duelist coat (not cowboy, no fringe).
Dual pistols at hip-ready, barrels down-forward.
Subtle rift trim on barrel metal only.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 6) HEXER
Save as: `hexer_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Female curse caster.
Ragged dark robes.
Right hand dark staff, left hand iron lantern.
Cursed green and void-purple accents together.
Slight ritual hunch, no body-wide glow.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 7) SUMMONER
Save as: `summoner_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Female battlefield summoner commander.
Ornate but practical purple-gold robes.
One hand holds scepter, other hand open command gesture.
Cold-blue crystal accent only.
No giant runic circle.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 8) BRAWLER
Save as: `brawler_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Male close-pressure brawler.
Bare arms, torn dark green vest, reinforced gauntlets.
Fists-only combat identity, no weapons.
Subtle void-purple tattoo accents.
Athletic, grounded silhouette.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 9) RONIN
Save as: `ronin_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Male exiled ronin.
Layered blue-green robes, visible topknot.
One katana drawn, one sheathed.
Calm pre-strike identity.
Minimal cold-silver edge accent only.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```

### 10) RAVAGER
Save as: `ravager_south_lock.png`
```text
Use the provided dummy image as a strict base reference.
Keep camera angle, head tilt, body proportions, shoulder line, pelvis orientation, leg length, and foot placement unchanged.
Keep full-body framing unchanged.
Maintain the same dark fantasy ARPG readability and anchor-matched 3/4 gameplay view.
Edit only class outfit, weapon, and small accent details.

Male blood berserker with the widest silhouette in roster.
Heavy worn armor fragments, dual large notched axes.
Blood-red scarification accents.
Bare head, brutal grounded identity.
No blue/purple holy magic cues.

no pose change, no camera change, no proportion change, no reframing,
no front portrait, no side view, no chibi,
no extra limbs, no floating weapons, no oversized VFX, no UI, no text, no logo
```
