# CODEX TASK — Overnight: Skill Sheets v2 (Character-Specific + ALL Skills)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen. Output → `STAGING/concepts/skill_sheets_v2/`.

---

## User Feedback (v1 reject)

> "10 tane karakterimi pixellabdan çektin mi? skill sheetlerde kaç skill varsa hepsini yapsın. ve gerçekten karakterlere özel olacak ayrı bi tarzda değil"

v1 sheet'lerde:
- Generic warrior portrait (Warblade için gerçek sprite kullanılmadı)
- Sadece 6 skill (gerçek 14-22)
- Generic stil (class-specific identity yok)

v2 fix:
- **Gerçek character sprite reference** (local PNG veya rich description)
- **TÜM skills**: code'tan enumerate (Warblade 14, Elementalist 15, Ranger 20, Shadowblade 22, Ronin 4)
- 5 class concept skills (Gunslinger/Ravager/Hexer/Brawler/Summoner) — Codex designs 8-10 each
- **Class-specific theme** (palette + iconography + signature visual cue)

## 10 Class Reference

### Has Real Skills (Code'tan enumerate)

#### 1. Warblade (14 skill) — sprite: `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
Identity: Young human, dark armor, iron longsword. Theme: **iron + gravity + earthbreak**.
Skills: Cleave, GravityCleave, WarStomp, IronCounter, CripplingBlow, BladeRush, IronCrush, IroncladMomentum, BattleSurge, IronCharge, SunderMark, DeepWound, DeathBlow, Earthsplitter
Source code: `Assets/Scripts/Skills/Warblade/*.cs`

#### 2. Elementalist (15 skill) — Identity: Robed caster, orb only (no staff), elemental. Theme: **arcane prism multi-color**.
Skills: Blink, ArcaneBlast, MirrorImage, ArcaneSurge, Combustion, Fireball, GlacialSpike, PrismBeam, FrostWall, SolarFlare, LivingBomb, FrozenOrb, Meteor, Blizzard, ChainLightning
Source: `Assets/Scripts/Skills/Elementalist/*.cs`

#### 3. Ranger (20 skill) — Identity: Hooded archer, bow, traps. Theme: **forest hunter + pin marks**.
Skills: AimedShot, ConcussiveArrow, BarbedNetShot, ExplosiveTrap, MultiShot, Disengage, BlackArrow, Volley, RapidFire, TetheringArrow, Flare, PointBlank, PinningShot, MarkedDetonate, HuntersStep, BoneTrap, SweepVolley, PredatorsMark, FinalStrike, WirelineTrap
Source: `Assets/Scripts/Skills/Ranger/*.cs`

#### 4. Shadowblade (22 skill) — Identity: Teal robe + red accents, daggers, stealth/poison. Theme: **shadow + combo points + toxin**.
Skills: Backstab, Hemorrhage, Rupture, ShadowStep, KidneyShot, Ambush, FanOfKnives, MirageBlade, ToxicEruption, Preparation, Evasion, Vanish, PhaseStep, DeathMark, VeilBurst, Severance, SmokeVeil, ChainCull, ShadowPin, NightAperture, BackstabMark, ShadowClone
Source: `Assets/Scripts/Skills/Shadowblade/*.cs`

#### 5. Ronin (4 skill) — Identity: Wandering samurai, katana, Tension resource. Theme: **iaido + sakura + tension drain**.
Skills: Quickdraw, SakuraVeil, IaidoStance, FinalDraw
Source: `Assets/Scripts/Combat/Classes/Ronin/Skills/*.cs`

### Concept Skills (Codex Designs, 8-10 each)

#### 6. Gunslinger (8-10 concept) — Identity: Trenchcoat dual pistols. Theme: **dual fire + trick shots + gunslinger flair, white-teal-gold**.
Concept: Twin Fire / Ricochet Shot / Smoke Round / Quick Reload / Powder Burst / Fan The Hammer / Bullet Time / Dead Eye / Pistol Whip / Six Shooter

#### 7. Ravager (8-10) — Identity: Long hair + beard barbarian, leather chest, axe. Theme: **berserker rage + blood red + brown leather**.
Concept: Berserk / Axe Throw / Earthcrack / Bloodthirst / War Cry / Whirlwind / Reckless Strike / Crimson Roar / Skull Splitter / Frenzied Charge

#### 8. Hexer (8-10) — Identity: Hooded cursecaster, cursemark forehead, wand. Theme: **curse stacks + DoT + dark purple + sickly green**.
Concept: Curse Mark / Decay Aura / Necrosis / Hex Bolt / Shackle Curse / Soul Drain / Death Wail / Plague Touch / Bone Cage / Anguish Field

#### 9. Brawler (8-10) — Identity: Shirtless + wrap gloves, fists. Theme: **combo punches + grappling + golden glow + earth-brown**.
Concept: Jab / Cross / Uppercut / Earth Slam / Body Lock / Power Strike / Iron Stance / Knockout / Pile Driver / Rolling Thunder

#### 10. Summoner (8-10) — Identity: Robed caster, focus orb. Theme: **minion control + spirit binding + cyan-blue + ethereal white**.
Concept: Summon Wisp / Spirit Bind / Pact Drain / Soul Link / Ethereal Guard / Familiar Strike / Beacon / Sacrifice / Echo Mirror / Wraithgate

## Görsel Spec

**10 PNG output** — her class için 1 sheet, ~1280×960:

Layout:
```
┌─────────────────────────────────────────────────────────┐
│  [CLASS NAME BANNER + theme color frame]                │
│                                                          │
│  ┌──────────────┐    ┌───────────────────────────────┐  │
│  │              │    │  REAL/CONCEPT SKILLS GRID     │  │
│  │   PORTRAIT   │    │  ┌──┐ ┌──┐ ┌──┐ ┌──┐ ┌──┐    │  │
│  │   (chibi)    │    │  │S1│ │S2│ │S3│ │S4│ │S5│    │  │
│  │              │    │  └──┘ └──┘ └──┘ └──┘ └──┘    │  │
│  │              │    │  ┌──┐ ┌──┐ ┌──┐ ┌──┐ ┌──┐    │  │
│  └──────────────┘    │  │S6│ │S7│ │S8│ │S9│ │S10│   │  │
│                      │  └──┘ └──┘ └──┘ └──┘ └──┘    │  │
│  Theme: <text>       │  ... (kaç skill varsa hepsi)  │  │
│  Resource: <text>    │                                │  │
│                      └───────────────────────────────┘  │
│                                                          │
│  [Bottom: skill name labels alt under icons]            │
└─────────────────────────────────────────────────────────┘
```

**Skill icon count per sheet:**
- Warblade 14 → 7×2 grid
- Elementalist 15 → 5×3 grid
- Ranger 20 → 5×4 grid
- Shadowblade 22 → 6×4 grid (2 boş)
- Ronin 4 → 4×1 grid
- Gunslinger/Ravager/Hexer/Brawler/Summoner 8-10 → 5×2 grid

**Critical Requirements:**
- Each class skill icon UNIQUE to that class (palette, motif, signature shape)
- Warblade için ACTUAL local PNG kullan (path verildi)
- Diğer class'lar için identity description rich detail render et
- No generic "warrior" portrait — class identity sprite gibi olmalı
- Skill icons class theme color ile boundary highlight

## Stil

- Pixel art consistent
- Class-specific palette LOCK (Warblade gray+orange, Elementalist multi-color prism, Ranger green+brown, Shadowblade teal+red, Ronin white+red, Gunslinger white-teal-gold, Ravager blood-red+brown, Hexer purple+green, Brawler gold+earth, Summoner cyan+white-ethereal)
- Hades + Diablo + Alabaster Dawn sentez (clone değil — RIMA Style Manifesto)

## Output

```
STAGING/concepts/skill_sheets_v2/
  01_warblade_v2_sheet.png
  02_ronin_v2_sheet.png
  03_gunslinger_v2_sheet.png
  04_ranger_v2_sheet.png
  05_elementalist_v2_sheet.png
  06_shadowblade_v2_sheet.png
  07_ravager_v2_sheet.png
  08_hexer_v2_sheet.png
  09_brawler_v2_sheet.png
  10_summoner_v2_sheet.png
```

## Final Report

`STAGING/CODEX_DONE_overnight_skill_sheets_v2.md`:
- 10 PNG list + alpha analysis
- Class-identity uniqueness verdict (her sheet'in distinct palette/motif kontrol)
- Skill icon count per class doğrulama
- Concept skills validity verdict (5 placeholder class için)
- Production rekommendation

## Dispatch

`--effort high`, explicit profile rotation, `--timeout 3600`, background.
