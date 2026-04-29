# CLASS SPRITES — PixelLab Prompts
> Her class için Edit Image PRO ile yap. Init image = referans PNG, strength ~180.

## GLOBAL AYARLAR (her seferinde aynı)
```
Tool:       Edit Image PRO
View:       high top-down
Isometric:  ON
Direction:  South
Outline:    single color black outline
Shading:    detailed shading
Detail:     highly detailed
Size:       128×128
Remove BG:  ON
Strength:   180
```

---

## 1 — Warblade
**Init:** `PixelLab_Refs_128/warblade1.png`
```
heavily armored dark warrior, black iron plate armor, long black cape, massive greatsword with blue glowing cracks, isometric high top-down, vibrant Hades art style, pixel art
```

## 2 — Brawler
**Init:** `PixelLab_Refs_128/brawler.png`
```
muscular brawler, light leather armor, bare arms with iron gauntlets and bandaged fists, aggressive stance, isometric high top-down, vibrant Hades art style, pixel art
```

## 3 — Elementalist
**Init:** `PixelLab_Refs_128/elementalist.png`
```
arcane mage, dark robes with glowing runes, hands channeling a floating glowing energy orb, isometric high top-down, vibrant Hades art style, pixel art
```

## 4 — Gunslinger
**Init:** `PixelLab_Refs_128/gunslinger.png`
```
dark gunfighter, long leather duster coat, wide-brim hat, dual revolvers with engravings, isometric high top-down, vibrant Hades art style, pixel art
```

## 5 — Hexer
**Init:** `PixelLab_Refs_128/hexer.png`
```
dark witch, tattered robes with arcane symbols, holding ancient grimoire spellbook with glowing eye on cover, isometric high top-down, vibrant Hades art style, pixel art
```

## 6 — Ranger
**Init:** `PixelLab_Refs_128/ranger.png`
```
hooded hunter, dark green cloak, leather armor, longbow, quiver of arrows on back, isometric high top-down, vibrant Hades art style, pixel art
```

## 7 — Ravager
**Init:** `PixelLab_Refs_128/ravager.png`
```
berserker, spiked bone armor, war paint, wielding oversized heavy battle cleaver, savage stance, isometric high top-down, vibrant Hades art style, pixel art
```

## 8 — Ronin
**Init:** `PixelLab_Refs_128/ronin.png`
```
wandering samurai, torn dark hakama and worn kimono, katana at hip, battle-scarred, isometric high top-down, vibrant Hades art style, pixel art
```

## 9 — Shadowblade
**Init:** `PixelLab_Refs_128/shadowblade.png`
```
shadow assassin, dark form-fitting armor, hood and half-mask, dual curved void-black daggers, isometric high top-down, vibrant Hades art style, pixel art
```

## 10 — Summoner
**Init:** `PixelLab_Refs_128/summoner.png`
```
mystical summoner, ornate ceremonial robes, arms raised, spectral ghostly familiar floating nearby, isometric high top-down, vibrant Hades art style, pixel art
```

---

## Yön Zinciri (S → 8 yön)
Her class için South tamamlanınca diğer yönleri sırayla üret:
`S → SW → W → NW → N → NE → E → SE`
Her yönde: bir önceki yönün çıktısını init_image yap, strength=220.
