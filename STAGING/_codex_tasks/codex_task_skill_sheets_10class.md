# CODEX TASK — 10-Class Skill Sheet Gallery

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Offline mode — bağlam bu task'ın içinde.

---

## Hedef

RIMA'nın 10 canonical class'ı için **skill sheet PNG**'leri üret. Her class için 1 PNG:
- Class portrait (mevcut sprite referansı)
- 6-8 skill icon + isim
- Class theme color
- Class name banner

Output: 10 PNG, `STAGING/concepts/skill_sheets/<class>_sheet.png`

## ÖNEMLİ — Imagegen Route

Codex's **built-in imagegen tool** kullan (NOT shell OpenAI API). `b4n5obvio` task'ında çalışmıştı. Source PNG'ler `C:\Users\ydbil\.codex-profiles\<profile>\generated_images\` altında çıkar, post-process ile target path'e kopyala.

## Bağlam — RIMA 10 Canonical Class

PixelLab anchor IDs locked (memory `canonical-character-roster-v2`):

| # | Class | Anchor PNG (existing reference) | Identity (lore) |
|---|---|---|---|
| 1 | Warblade | `Assets/Art/Characters/Warblade/Rotations/warblade_south.png` | Young human, dark armor, iron longsword. Iron/gravity theme. |
| 2 | Ronin | (PixelLab ID `a7957352-cc57-44a1-a9fc-96f1fbd1119a` — no local PNG yet) | Wandering samurai, katana, "Tension" resource (idle gain, movement drain). |
| 3 | Gunslinger | (PixelLab ID `a78545eb-ef10-4e1e-827e-784000e45886`) | Trenchcoat, dual pistols, white tie + teal cap. Long-range marksman. |
| 4 | Ranger | (PixelLab ID `d5b1cf71-...`) | Hooded archer, bow, trap-heavy. Pin/track marks. |
| 5 | Elementalist | (PixelLab ID `4c83c0be-...`) | Robed caster, **orb only** (no staff), elemental magic. |
| 6 | Shadowblade | (PixelLab ID `deee34b5-...`) | Teal robe + red accents, daggers, combo points, stealth/poison. |
| 7 | Ravager | (PixelLab ID `091e9552-...`) | Barbarian, long hair + beard, leather chest, axe. Berserk + rage. |
| 8 | Hexer | (PixelLab ID `e260a1af-...`) | Hooded cursecaster, cursemark forehead, wand. Curse stacking + DoT. |
| 9 | Brawler | (PixelLab ID `d4fa3d13-...`) | Shirtless + wrap gloves, fists. Combo punches + grappling. |
| 10 | Summoner | (PixelLab ID `83039c80-...`) | Robed caster, focus orb in hand. Minion/familiar control. |

## Skill Data (5 Class Coded)

Codex source code'tan oku (paths verilmiş):

### Warblade — `Assets/Scripts/Skills/Warblade/`
Skills: Cleave, GravityCleave, WarStomp, IronCounter, CripplingBlow, BladeRush, IronCrush, IroncladMomentum, BattleSurge, IronCharge, SunderMark, DeepWound, DeathBlow, Earthsplitter (14 skill)
Theme: **iron + gravity + earthbreak**, dark gray + orange ember

### Elementalist — `Assets/Scripts/Skills/Elementalist/`
Skills: Blink, ArcaneBlast, MirrorImage, ArcaneSurge, Combustion, Fireball, GlacialSpike, PrismBeam, FrostWall, SolarFlare, LivingBomb, FrozenOrb, Meteor, Blizzard, ChainLightning (15 skill)
Theme: **elemental prism**, multi-color (cyan ice / orange fire / purple arcane / yellow lightning)

### Ranger — `Assets/Scripts/Skills/Ranger/`
Skills: AimedShot, ConcussiveArrow, BarbedNetShot, ExplosiveTrap, MultiShot, Disengage, BlackArrow, Volley, RapidFire, TetheringArrow, Flare, PointBlank, PinningShot, MarkedDetonate, HuntersStep, BoneTrap, SweepVolley, PredatorsMark, FinalStrike, WirelineTrap (20 skill)
Theme: **forest hunter + traps**, green + brown + arrow feather accent

### Shadowblade — `Assets/Scripts/Skills/Shadowblade/`
Skills: Backstab, Hemorrhage, Rupture, ShadowStep, KidneyShot, Ambush, FanOfKnives, MirageBlade, ToxicEruption, Preparation, Evasion, Vanish, PhaseStep, DeathMark, VeilBurst, Severance, SmokeVeil, ChainCull, ShadowPin, NightAperture, BackstabMark, ShadowClone (22 skill)
Theme: **shadow + poison + combo points**, teal + red + black

### Ronin — `Assets/Scripts/Combat/Classes/Ronin/Skills/`
Skills: RoninQuickdraw, RoninSakuraVeil, RoninIaidoStance, RoninFinalDraw (4 skill — minimal)
Theme: **tension + iaido + sakura**, white + red + black-steel

## Skill Data (5 Class Concept — Codex Design)

Code'da skill yok. Class identity'den 6-8 concept skill öner (sadece visual + isim + 1-line description). Mekanikleri PRD değil, visual concept yeterli.

### Gunslinger
Theme: **dual pistols, trick shots, gunslinger flair**, white-teal-gold
Concept skills önerisi: Twin Fire, Ricochet Shot, Smoke Round, Quick Reload, Powder Burst, Fan The Hammer, Bullet Time, Dead Eye

### Ravager
Theme: **berserker rage + axe smash**, blood red + brown leather + steel
Concept skills: Berserk, Axe Throw, Earthcrack, Bloodthirst, War Cry, Whirlwind, Reckless Strike, Crimson Roar

### Hexer
Theme: **curse stacks + DoT decay**, dark purple + sickly green + cursemark glow
Concept skills: Curse Mark, Decay Aura, Necrosis, Hex Bolt, Shackle Curse, Soul Drain, Death Wail, Plague Touch

### Brawler
Theme: **combo punches + grappling**, golden glow knuckles + earth-brown + impact orange
Concept skills: Jab, Cross, Uppercut, Earth Slam, Body Lock, Power Strike, Iron Stance, Knockout

### Summoner
Theme: **minion control + spirit binding**, cyan-blue + ghost-white + ethereal
Concept skills: Summon Wisp, Spirit Bind, Pact Drain, Soul Link, Ethereal Guard, Familiar Strike, Beacon, Sacrifice

## Skill Sheet Layout (Her Class İçin)

Önerilen layout (Codex sanatsal serbestlik kullanabilir):

```
┌─────────────────────────────────────────────────┐
│  [CLASS NAME BANNER — theme color border]       │
│                                                  │
│  ┌──────────┐    ┌─────┐ ┌─────┐ ┌─────┐        │
│  │          │    │skill│ │skill│ │skill│        │
│  │ PORTRAIT │    │  1  │ │  2  │ │  3  │        │
│  │  (chibi) │    │     │ │     │ │     │        │
│  │          │    └─────┘ └─────┘ └─────┘        │
│  │          │    ┌─────┐ ┌─────┐ ┌─────┐        │
│  └──────────┘    │skill│ │skill│ │skill│        │
│                  │  4  │ │  5  │ │  6  │        │
│  Class:          │     │ │     │ │     │        │
│  Theme:          └─────┘ └─────┘ └─────┘        │
│  Resource:                                      │
│                                                  │
│  [Bottom: skill name labels]                    │
└─────────────────────────────────────────────────┘
```

- Resolution: 1024×768 per sheet
- Style: pixel art consistent, dark theme background, class theme accent
- Portrait: existing PNG referansı kullanılabilir (Warblade'in PNG'i var, diğerleri için PixelLab ID açıklayıcı sprite tarif et)
- Skill icons: 6 (focus) — most signature 6 skills (not all 14-22). Codex prioritize eder.
- Icon style: 64×64 mini pixel art, theme color glow

## Output Structure

```
STAGING/concepts/skill_sheets/
  01_warblade_sheet.png
  02_ronin_sheet.png
  03_gunslinger_sheet.png
  04_ranger_sheet.png
  05_elementalist_sheet.png
  06_shadowblade_sheet.png
  07_ravager_sheet.png
  08_hexer_sheet.png
  09_brawler_sheet.png
  10_summoner_sheet.png
```

Toplam: **10 PNG**.

## Acceptance Criteria

- ✅ 10 sheet PNG
- ✅ Her sheet'te class portrait + 6 skill icon + class name + theme color
- ✅ Stil tutarlı across 10 sheet
- ✅ Real skills (5 class) ile concept skills (5 class) ayrımı net (concept skill banner'da "concept" işareti olabilir)
- ✅ Class identity reads instantly (lore-aligned)

## BLOCKED if

- imagegen tool erişimi yok
- skill source files okunamıyor (`Assets/Scripts/Skills/`)
- output write izni yok

## Final Report

`STAGING/CODEX_DONE_skill_sheets.md`:
- 10 PNG list
- Her sheet için 1-2 cümle verdict (clean / muddy / readable)
- Hangi class en güçlü skill set'e sahip görsel olarak
- Concept skill 5 class için recommendation: hangi skill'ler "must-keep" hangileri "drop"

## Dispatch

```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_skill_sheets_10class.md --effort high
```

Background. Notify on complete.
