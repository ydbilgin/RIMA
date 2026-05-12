# Batch Prompt — 15 Sprite (Create Image Pro, Warblade Style Reference)

**Amaç:** Pilot Warblade onaylandıktan sonra kalan 9 karakter + 6 mob batch üretim.
**Tool:** PixelLab Web App → Create Image **Pro** (16 varyasyon kapasitesi, batch mode)
**Canvas:** 64x64 her sprite
**Background:** Transparent ON
**Style Reference:** `warblade_south_v1.png` (pilot çıktısı) — drift kontrolü için
**Seed:** sabit (reproducibility)

---

## ORTAK PROMPT (tüm batch için)

```
Generate 15 character/mob pixel art sprites, ALL identical spec:
- Canvas: 64x64 each
- View: 35° high top-down 3/4 perspective, character facing south (looking at camera)
- Style: 16-bit chunky pixel art matching the reference style — hard edges, no anti-aliasing, no gradients, no embedded glow, no painterly
- Proportions: chibi (large head, compact body)
- Palette: desaturated environment tones with saturated color accents on weapons/armor highlights
- Layout: character centered, ~60% canvas height, padding preserved for VFX
- NO embedded glow/aura/gradient — engine-side VFX is added in Unity

Each sprite has DIFFERENT character (use the reference for style/proportion/quality lock, but new design per cell):
```

---

## SPRITE LISTESİ (1-15)

### Karakter Sınıfları (9 — Warblade pilot'tan ayrı)

**2. ELEMENTALIST**
```
female mage, hooded deep blue robe, calm focused face, tall wooden staff in right hand with glowing CYAN crystal at top (Rift Cyan #00FFCC), runes carved on staff shaft, no orb in left hand, left hand relaxed at side, cream sash belt, simple sandals
Palette: deep blue #2A2E55 robe, cream #E8DCB0 sash, dark wood #4A3520 staff, cyan #00FFCC crystal glow accent
```

**3. SHADOWBLADE**
```
male assassin, dark bandana covering lower face, hood hiding hair, sharp narrow eyes, lean crouched ready stance, BOTH arms angled with DUAL DAGGERS in REVERSE GRIP (blades pointing down behind wrists), tight dark leather armor, soft-soled boots, belt with throwing knives
Palette: deep black-purple #1A0F2A, dark grey #2E2A35, cold blue accent #4A6890, 2-3 shade steps
```

**4. RANGER**
```
female ranger, hooded deep forest green cloak, sharp eyes visible under hood, defined jaw, lean athletic upright stance, SHORT COMPOSITE BOW held diagonally in left hand at chest level (dark wood bow with silver tip), right hand at draw-ready near string, leather armor with shoulder mantle, bracers, belt with pouches, tall brown boots, arrow quiver behind right shoulder
Palette: earthy greens #2B4A2B / #3D5C3D, dark browns #4A3520, leather tan #8B6F3F
```

**5. RAVAGER**
```
male barbarian, wild hair, fierce eyes, grim face, muscular hunched stance, DUAL CLEAVERS one in each hand (large rusty butcher's blades), bloody fur armor over bare chest, leather pants, no boots wraps on feet, scars
Palette: dirty fur browns #5A4530, rusty steel #6A4525, blood red accents #6A1010, skin pale #C9A586
```

**6. RONIN**
```
male samurai, traditional topknot, calm focused face, upright dignified stance, KATANA SHEATHED at left hip in lacquered scabbard, right hand resting near sword hilt (iaido draw-ready), traditional black/grey kimono with hakama pants, geta sandals
Palette: deep black #1A1A1A kimono, grey #4A4A4A trim, gold accent #8B7355 on scabbard, white #E8DCB0 obi
```

**7. GUNSLINGER**
```
female duelist, deep auburn red hair tied back, sharp confident eyes, upright relaxed stance, DUAL PISTOLS holstered at both hips (visible grips), hands hovering near holsters (ready to draw), long dark trench coat, leather vest, belt with ammo pouches, tall boots
Palette: dark charcoal #2E2A35 coat, deep red-brown #5A2A25 hair, brass accents #B5862A on guns, leather brown #5A3D25
```

**8. BRAWLER**
```
male fighter, short hair, athletic muscular build, low ready stance, NO WEAPON — bare fists wrapped in heavy knuckle bandages, fingerless gloves, sleeveless tank top, athletic pants, boxing-style boots
Palette: skin tone #C9A586, white #E8DCB0 wraps, dark blue #2A2E55 pants, brown #4A3520 boots
```

**9. SUMMONER**
```
female mage (NOT chibi face, standard proportions per Karar #34), focused expression, upright graceful posture, TALL CRYSTAL STAFF in right hand (purple-tipped crystal), elegant flowing robes, ornate belt, simple shoes
Palette: deep purple #4A2A5A robe, gold trim #8B7355, purple crystal accent #8B4FB0, cream #E8DCB0 inner robe
```

**10. HEXER**
```
female witch, gothic face, dark hair, somber expression, upright stance with SPIRIT LANTERN at left hip belt (small ornate lantern, no flame visible — empty), both hands free at sides ready for spell casting, dark hooded witch robe, leather belt
Palette: deep purple-black #1A0F2A robe, dark grey #2E2A35, lantern brass #8B7355, pale skin
```

### Moblar (6 adet)

**11. GOBLIN WARRIOR**
```
tiny humanoid 64x64 chibi, oversized green head, heavy brow, beady dark eyes, small lower tusks, dark messy hair tufts, short compact body, slight hunch, tiny arms holding RUSTY SHORT SWORD in right hand, short bandy legs, ragged brown tunic, dark belt, tiny dark boots
Palette: green skin #4A6A30, dark brown #4A3520 clothing, rusty steel #6A4525 sword
POSE: aggressive ready, weight forward, sword raised slightly
```

**12. SKELETON WARRIOR**
```
humanoid 64x64 chibi, bone skull head, empty eye sockets with faint blue glow, exposed jaw, bone frame with visible ribs, hunched upright, skeletal arms holding CHIPPED LONGSWORD (right) and BROKEN ROUND SHIELD (left), tattered rusted chainmail rags over bones
Palette: bone white #E8DCB0, rusty steel #6A4525, faded grey rags #5A5550, faint blue eye glow #4A6890
POSE: rigid undead stance, weapons at ready
```

**13. SLIME**
```
amorphous gelatinous creature 64x64 chibi, green-blue blob teardrop shape, NO HEAD/LIMBS — pure blob, two small black dot eyes near top, slight translucent appearance, small inner bubbles
Palette: translucent green-blue #4A8A6A core, darker green #2A5A4A base, white highlight #E8DCB0 (1-2px), 2-3 shade steps
POSE: settled blob, slight wiggle
```

**14. WRAITH**
```
ethereal humanoid 64x64 chibi, hooded skull face, glowing white-cyan eye sockets, NO JAW (mist trails), tattered cloak floating, NO LEGS (mist tail at base instead), skeletal claw hands extended outward, transparent edges
Palette: dark grey-blue cloak #2A2E55, ghostly white-cyan glow #B5E8E0, transparent edges fading to alpha
POSE: floating menacing, claws forward
```

**15. MUSHROOM MONSTER**
```
fungal creature 64x64 chibi, OVERSIZED red mushroom cap with white spots (cap takes top 50% of canvas), small eyes underneath cap, SHORT white stalk body, two short stubby arms, two stumpy legs, glowing spore particles around base
Palette: red cap #8B2020 with white spots #E8DCB0, off-white stalk #C9C0A8, dark mossy green base #1A2B1A, faint yellow spore glow #FFF000
POSE: bouncing ready, slight cap tilt
```

**16. GIANT RAT**
```
small quadruped 64x64 chibi, pointed snout, beady RED eyes, large notched ears, prominent yellow fangs, long crouched body with scruffy fur, hunched back, FOUR short clawed legs, long pink tail trailing behind
Palette: dirty brown fur #5A4530, dark grey shading #2E2A35, pink tail #C97A85, yellow fangs #FFB000, red eyes #8B0000
POSE: low predator ready, weight forward, tail trailing
```

---

## QC Kontrol (batch sonrası)

Her sprite için:
- [ ] 64x64 boyut
- [ ] South facing (kamera-bakan)
- [ ] 35° açı (zemine basma hissi)
- [ ] Style consistency (Warblade pilot ile aynı yoğunluk)
- [ ] Palette uyumu
- [ ] No embedded glow / anti-aliasing
- [ ] Silüet okunabilirlik

Eğer 3+ sprite başarısız → batch revize, prompt nüansla.

## Manuel Cleanup Pass (her sprite ~5-10 dk)

Aseprite veya Photoshop:
1. Silhouette outline kontrol (1px hard edge)
2. Anti-aliasing kalıntıları temizle
3. Embedded glow varsa kaldır (Rift Cyan VFX engine'de eklenecek)
4. Padding doğrulama (karakter ~%60 yükseklik)
5. Transparent BG kontrol

## Sonraki Adım (batch onayı sonrası)

1. Her sprite Unity'e import: PPU=64, Filter=Point, no compression, pivot=bottom-center
2. _IsoGame.unity'de placeholder GameObject'e Warblade sprite ata → Pixel Perfect Camera ile test
3. Bir sonraki: Pilot karakterin (Warblade) Custom Animation V3 ile 8 yön + 6 animasyon (idle/walk/attack/dash/hurt/death)
