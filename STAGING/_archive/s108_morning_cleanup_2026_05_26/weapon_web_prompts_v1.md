# Weapon Web Prompts v1 — 11 sprites for PixelLab Web UI

> **Tool:** Create Image S-XL (new)
> **Settings per weapon:**
> - Direction: **None**
> - Canvas size: **32×32**
> - Background: **Transparent**
> - Init image (style anchor): `Assets/Art/Characters/anchors/reference.png` (or `warblade_south.png` as fallback canonical anchor)
> **Naming convention:** `<class>_<weapon>_<L|R|none>.png`
> **Save path:** `Assets/Art/Weapons/<Class>/<filename>.png`

---

## Universal style prefix (paste before every weapon prompt)

```
Pixel art weapon sprite, 32x32 transparent background, dark gritty fantasy aesthetic matching RIMA Shattered Keep canon, weathered look, no character no body, single object centered with handle pivot at lower-center, matte painterly pixel art, dark muted palette, NO outline frame, no UI elements, no background, no shadow ground.
```

---

## 1. Warblade — Greatsword

```
A heavy two-handed greatsword, blade pointing up, dark weathered steel with subtle cyan rift hairline along the fuller (center groove), brass-wrapped grip with worn leather binding, simple cross-guard, blade slightly chipped from battle. Honest brutal weapon, no decorative excess. Pixel art, 32x32 transparent, pivot at grip bottom-center.
```

**Filename:** `Assets/Art/Weapons/Warblade/warblade_greatsword.png`

---

## 2. Ronin — Katana (drawn)

```
A drawn katana, single-edged curved blade pointing up-right at slight diagonal, polished dark steel with a faint cyan reflection along the edge, black silk-wrapped tsuka (handle), small simple iron tsuba (guard), no scabbard visible (scabbard stays on body). Disciplined elegant weapon. Pixel art, 32x32 transparent, pivot at tsuka bottom-center.
```

**Filename:** `Assets/Art/Weapons/Ronin/ronin_katana_drawn.png`

---

## 3a. Gunslinger — Flintlock Pistol LEFT

```
A short flintlock-style dueling pistol, barrel pointing east-right, dark walnut wood grip with brass cap, blackened steel barrel and lock mechanism, ornate but restrained metalwork on the cock and frizzen, compact silhouette. Single weapon, no muzzle flash. Pixel art, 32x32 transparent, pivot at grip bottom-center.
```

**Filename:** `Assets/Art/Weapons/Gunslinger/gunslinger_pistol_L.png`

---

## 3b. Gunslinger — Flintlock Pistol RIGHT

```
A short flintlock-style dueling pistol, barrel pointing west-left (mirror of left pistol), dark walnut wood grip with brass cap, blackened steel barrel and lock mechanism, ornate but restrained metalwork on the cock and frizzen, compact silhouette. Identical otherwise to the left pistol but barrel direction reversed. Pixel art, 32x32 transparent, pivot at grip bottom-center.
```

**Filename:** `Assets/Art/Weapons/Gunslinger/gunslinger_pistol_R.png`

---

## 4. Ranger — Recurve Bow

```
A recurve bow, vertical orientation, string taut from top to bottom, weathered wood limbs with subtle horn tip reinforcements, leather grip wrap at center, ready-to-draw posture (not flexed full), no arrow notched. Hunter aesthetic, no ornate detail. Pixel art, 32x32 transparent, pivot at grip center.
```

**Filename:** `Assets/Art/Weapons/Ranger/ranger_recurve_bow.png`

---

## 5a. Shadowblade — Dagger LEFT

```
A curved dagger, blade pointing up-right, dark blue-tinted steel with a faint violet reflection at the edge tip, black leather-wrapped grip, small pommel ball at the base, simple cross-guard. Stealth weapon, sleek silhouette. Pixel art, 32x32 transparent, pivot at grip bottom-center.
```

**Filename:** `Assets/Art/Weapons/Shadowblade/shadowblade_dagger_L.png`

---

## 5b. Shadowblade — Dagger RIGHT

```
A curved dagger, blade pointing up-left (mirror of left dagger), dark blue-tinted steel with a faint violet reflection at the edge tip, black leather-wrapped grip, small pommel ball at the base, simple cross-guard. Mirror of left dagger. Pixel art, 32x32 transparent, pivot at grip bottom-center.
```

**Filename:** `Assets/Art/Weapons/Shadowblade/shadowblade_dagger_R.png`

---

## 6a. Ravager — Axe LEFT

```
A heavy single-bit axe, blade pointing up-right, dark iron axe head with crude rivets, blood-stained edge with weathered patina, thick wooden haft wrapped in worn leather at the grip, brutal silhouette. Barbarian aesthetic, no ornate detail. Pixel art, 32x32 transparent, pivot at haft bottom-center.
```

**Filename:** `Assets/Art/Weapons/Ravager/ravager_axe_L.png`

---

## 6b. Ravager — Axe RIGHT

```
A heavy single-bit axe, blade pointing up-left (mirror of left axe), dark iron axe head with crude rivets, blood-stained edge with weathered patina, thick wooden haft wrapped in worn leather at the grip, brutal silhouette. Mirror of left axe. Pixel art, 32x32 transparent, pivot at haft bottom-center.
```

**Filename:** `Assets/Art/Weapons/Ravager/ravager_axe_R.png`

---

## 7. Hexer — Hex Staff

```
A mid-length hex staff, vertical orientation, dark gnarled wood shaft with binding wraps along its length, a bound dark crystal at the top end emitting faint cyan inner glow, leather grip wrap at lower third. Witch/warlock aesthetic, mysterious silhouette. Pixel art, 32x32 transparent, pivot at grip center.
```

**Filename:** `Assets/Art/Weapons/Hexer/hexer_staff.png`

---

## 8. Summoner — Soul Lantern

```
A handheld soul lantern, vertical orientation, iron frame with hanging chain at the top, weathered curved metal cage holding glass panes, pale ghost-green flame inside the glass (soft glow, not bright), worn but still functioning, mystical silhouette readable as light source. Pixel art, 32x32 transparent, pivot at chain-handle center.
```

**Filename:** `Assets/Art/Weapons/Summoner/summoner_soul_lantern.png`

---

## 9. Elementalist — Arcane Orb (NEW per user direction 2026-05-22)

```
A handheld arcane orb, round crystalline ball roughly fist-sized, internal swirling cyan-violet energy with subtle inner spark patterns, faint cool glow emanating from within (cyan RGB 93,239,255 dominant + minor violet accent), held with small brass or dark metal claw-grip cradle at the bottom, mid-mystic aesthetic readable as elemental focus. Pixel art, 32x32 transparent, pivot at cradle bottom-center.
```

**Filename:** `Assets/Art/Weapons/Elementalist/elementalist_arcane_orb.png`

**Note (Karar #144 exception override):** NLM canonical previously stated Elementalist disc = Unity VFX only (no sprite). User direction 2026-05-22 supersedes — item-as-sprite first, VFX/animate layered on top in Unity later. This decision documented in `STAGING/weapon_pipeline_v1.md` Section 11 (Karar amendment record).

---

## QC checklist per weapon

After generating each sprite:

1. **Background fully transparent?** No leftover color/checkerboard bleed.
2. **Centered with grip at bottom-center?** For pivot setup ease.
3. **Silhouette readable at 32×32 zoom-out?** No detail-loss to ambiguity.
4. **Style consistency with character anchor?** Same palette, weathering level.
5. **No outline frame?** No black border around the canvas edge.
6. **Single object only?** No accessory clutter, no smoke/sparks/VFX.

If any FAIL → regen with prompt tweak. PixelLab MCP fallback also works once timeout resolved.

---

## Bulk production estimate

- **Per weapon:** ~1-2 gen credit (Create Image S-XL is cheap)
- **Total 12 weapons:** ~18-30 gen credit
- **User time:** ~3-5 min per weapon (prompt + generate + QC + maybe 1 regen) = ~35-65 min total

After all 12 sprites land, Codex picks up: import + custom pivot + WeaponPrefab batch build + WeaponDatabase entry populate.

---

## Generation order recommendation

Priority for vertical slice (Phase K test):
1. **Warblade greatsword** (primary class for Phase E-K test)
2. Ronin katana (second test character)
3. Ranger recurve bow (ranged class test)
4. Shadowblade dagger L+R (dual-wield test)
5. Hexer staff (caster test, no projectile yet)
6. Elementalist arcane orb (caster + VFX layer test)
7. Gunslinger pistol L+R (ranged dual-wield)
8. Ravager axe L+R (heavy dual-wield)
9. Summoner soul lantern (special — light source mechanic post-MVP)

**MVP test set:** Items 1-4 (5 sprites). Sufficient for Phase K vertical slice. Rest can roll in post-K polish.

**Total class roster coverage:** 9 / 10 classes have weapon sprites. Brawler remains sprite-less (clenched fists embedded in body sprite per Karar #144 Brawler exception).
