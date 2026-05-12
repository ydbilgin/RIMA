# Batch16 Tier-1 Regeneration Prompts
**2026-05-12 | 4 BLOCKING sprites — must pass before Create Character pipeline**
Based on: rima-qc review + rima-design decisions (Karar #92, #93)

---

## USAGE
Each prompt = Create Image Pro, south-facing (S direction), single frame idle.
Reference image workflow: use dual-blade south-facing reference -> "Match camera angle and south-facing pose exactly. Do not copy colors, armor, or weapon design."
After all 4 PASS rima-qc -> proceed to Create Character -> Custom Animation v3.

---

## 1. ELEMENTALIST (BLOCKING — idle pose wrong)
**Issue:** Hands at sides, no casting gesture. Karar #43 + #93.

### Description
```
64x64 chibi top-down female elementalist, NO WEAPON NO STAFF, asymmetric casting gesture: right hand raised to shoulder height palm forward (channeling), left hand at waist height palm up (gathering), both hands empty fingers visible, light cream/ivory outer robe with dark purple-grey #2A1F35 trim at sleeves and hem, golden #FFF000 accent detailing, warm honey-blonde hair golden-amber tone in neat bun with loose temple strands, straight upright balanced posture, view 35 degree high top-down, pixel art PPU 64, south facing front view
```

### Negative Prompt
```
staff, wand, rod, scepter, orb, crystal, grimoire, book, scroll, weapon, any held object, hands at sides, hands in pockets, hands behind back, hands clasped together, both hands raised symmetrically, T-pose, surrender pose, dark hair, black hair, brown hair, red hair, ginger, platinum, silver, white hair, hair down loose long hair, heavy armor, plate armor, leather armor, short robe, mini dress, hood up covering face, pointy wizard hat, all-dark robe, no trim detail
```

---

## 2. RAVAGER (BLOCKING — single axe instead of dual)
**Issue:** Single large battleaxe generated. Must be dual short compact axes. Karar S61 + anti-pattern.

### Description
```
64x64 chibi top-down male ravager berserker, weight forward berserk-ready stance, arms spread wide and low, DUAL WIELD: one short compact hand axe in EACH hand, one axe in left hand and one axe in right hand, two separate weapons mirrored left and right. Each axe is short-hafted (forearm length), single-headed, hooked head, brutal utilitarian iron. Axes held at hip-to-thigh level, heads angled outward, arms slightly bent and open (not crossed, not centered). Heavy build, scarred leather and fur armor, exposed forearms. Silhouette: two small hook shapes flanking the body at hip line. Dark blood-red rough armor #3A1A0A blood red accent #D43F3F, view 35 degree high top-down, pixel art PPU 64, south facing front view
```

### Negative Prompt
```
single weapon, single axe, two-handed axe, greataxe, battleaxe, long haft, long handle, polearm, halberd, weapon held with both hands, hands together, hands centered, weapon in front of body center, weapon raised overhead, one weapon, axe over shoulder, dual identical mirrored stiff pose, crossed arms, empty hand, only one hand holding weapon, clean armor, bright armor, metallic shine, precise craftsmanship armor
```

---

## 3. RONIN (BLOCKING — gender misfire, female instead of male)
**Issue:** Sprite #6 generated as female. Ronin is locked male. Karar gender lock.

### Description
```
64x64 chibi top-down MALE ronin, slim athletic build, single katana sheathed at LEFT HIP in a black saya (sheath), right hand resting near katana grip in iaido draw-ready position. Traditional asymmetric kimono-style top, hakama pants, tabi-style boots. Tied-back dark hair, focused calm expression. Sheath on LEFT HIP always visible — identity element. Dark navy near-black #1A1A2E kimono style, pale gold #C8A87A katana hilt wrap accent, view 35 degree high top-down, pixel art PPU 64, south facing front view
```

### Negative Prompt
```
female, woman, girl, breasts, feminine features, long flowing hair loose, two swords, dual katana, no sheath, sheath on back, sheath on right hip, greatsword, broadsword, western sword, samurai full plate armor, helmet, face mask, ronin without sheath, sword drawn out of sheath at idle, heavy armor, fur, spikes, casual modern clothes
```

---

## 4. RANGER (BLOCKING — dark hair, no wild aesthetic, wrong identity)
**Issue:** Conventional dark hair, generic archer look. Karar #92 new lock: white hair + battle-worn wild.

### Description
```
64x64 chibi top-down female tactical rift hunter, lean athletic build, weathered skin. Hair: off-white bleached-ivory color (NOT silver not platinum not blue), long, gathered into a low loose ponytail or loose braid behind the head, intentional loose strands escaping at the temples and along the jaw, windswept feel. Armor: dark forest green #2A3520 layered tactical hunter gear with cold blue #7BA7BC accents on hood lining and straps. Asymmetric details: heavier pauldron on right shoulder, leather forearm wrap on LEFT (bow) arm, slightly torn cloak hem, weathered boots. Faint scar across cheek or brow. Weapon: compound bow held in LEFT hand, lowered to thigh-level rest position — bow always in hand. Tactical hunter stance, weight slightly back, eyes forward scanning, alert but composed. View 35 degree high top-down, pixel art PPU 64, south facing front view
```

### Negative Prompt
```
silver hair, platinum hair, blue-tinted hair, ice queen hair, blonde, brown, black, red hair, albino, anime fantasy white, dark hair, orange hair, tribal markings, face paint, war paint, bones in hair, feathers in hair, beaded jewelry, primal warrior, savage, feral, skeletal, gaunt, emaciated, monster features, sharp teeth, fangs, glowing eyes, claws, horns, animal ears, fur tail, pristine clean armor, ceremonial armor, dress, skirt, bare midriff, forest green tights, forest archer fantasy, elf, elven ears, western cowboy archer, run-and-gun pose, bow raised aiming, both hands on bow drawing, bow on back not in hand
```

---

## QC CHECKLIST (rima-qc pass criteria)
After generation, verify each sprite against:
- [ ] Elementalist: right hand raised shoulder-height palm forward, left hand waist-height palm up, NO object in hands, honey-blonde bun visible, cream outer robe
- [ ] Ravager: TWO separate axes visible, one per hand, at hip flanking body (not centered), arms spread, heavy build
- [ ] Ronin: MALE confirmed, black saya/sheath on LEFT HIP visible, right hand near grip, dark kimono
- [ ] Ranger: off-white/bleached ivory hair (not silver/platinum), asymmetric battle-worn armor, compound bow in left hand at thigh rest, scar visible

All 4 PASS -> proceed to Create Character -> Custom Animation v3.
