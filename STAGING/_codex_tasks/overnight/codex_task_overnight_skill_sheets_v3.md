# CODEX TASK — Skill Sheets v3 (Real PixelLab Character Sprites)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen. Output → `STAGING/concepts/skill_sheets_v3/`.

---

## User Feedback (v2 reject)

v2 skill icons generic text placeholder. v2 portraits except Warblade abstract shape. User: "karakterleri çektikten sonra codexe her karakterin kendi şeyiyle skill sheetlerini görselleştirmesini istiyorum"

## Prerequisite (v3 dispatch SADECE bunlar tamamlandıktan sonra)

`Assets/Art/Characters/{Class}/Rotations/<class>_south.png` — 10 character × south view minimum (north/east/west bonus). Download task `b34sf9gmj` tamamlanmış olmalı.

Eğer download eksik character varsa BLOCKED yaz, dispatch durdurma — sadece available olanlar için v3 üret.

## 10 Class Character Sprite Paths (PixelLab download sonrası)

| # | Class | Sprite Path (Use as Portrait) |
|---|---|---|
| 1 | Warblade | `Assets/Art/Characters/Warblade/Rotations/warblade_south.png` |
| 2 | Ronin | `Assets/Art/Characters/Ronin/Rotations/ronin_south.png` |
| 3 | Gunslinger | `Assets/Art/Characters/Gunslinger/Rotations/gunslinger_south.png` |
| 4 | Ranger | `Assets/Art/Characters/Ranger/Rotations/ranger_south.png` |
| 5 | Elementalist | `Assets/Art/Characters/Elementalist/Rotations/elementalist_south.png` |
| 6 | Shadowblade | `Assets/Art/Characters/Shadowblade/Rotations/shadowblade_south.png` |
| 7 | Ravager | `Assets/Art/Characters/Ravager/Rotations/ravager_south.png` |
| 8 | Hexer | `Assets/Art/Characters/Hexer/Rotations/hexer_south.png` |
| 9 | Brawler | `Assets/Art/Characters/Brawler/Rotations/brawler_south.png` |
| 10 | Summoner | `Assets/Art/Characters/Summoner/Rotations/summoner_south.png` |

**Use ACTUAL PNG referansları** — Codex imagegen'de portrait olarak gerçek sprite'ı UPSCALE veya backdrop ekle, ama character identity korunur. Abstract shape veya generic warrior YASAK.

## Skill Code Enumeration (Read From Source)

### Has Real Skills (Code'tan enumerate, ALL)

| Class | Skill Count | Source Dir |
|---|---|---|
| Warblade | 14 | `Assets/Scripts/Skills/Warblade/` |
| Elementalist | 15 | `Assets/Scripts/Skills/Elementalist/` |
| Ranger | 20 | `Assets/Scripts/Skills/Ranger/` |
| Shadowblade | 22 | `Assets/Scripts/Skills/Shadowblade/` |
| Ronin | 4 | `Assets/Scripts/Combat/Classes/Ronin/Skills/` |

Codex reads each `.cs` file → skill name. ALL skills displayed.

### Concept Skills (Codex Designs, 8-10 each)

5 class skill code yok. Concept skill (v2'den aynı, mümkünse):
- Gunslinger: Twin Fire / Ricochet Shot / Smoke Round / Quick Reload / Powder Burst / Fan The Hammer / Bullet Time / Dead Eye
- Ravager: Berserk / Axe Throw / Earthcrack / Bloodthirst / War Cry / Whirlwind / Reckless Strike / Crimson Roar
- Hexer: Curse Mark / Decay Aura / Necrosis / Hex Bolt / Shackle Curse / Soul Drain / Death Wail / Plague Touch
- Brawler: Jab / Cross / Uppercut / Earth Slam / Body Lock / Power Strike / Iron Stance / Knockout
- Summoner: Summon Wisp / Spirit Bind / Pact Drain / Soul Link / Ethereal Guard / Familiar Strike / Beacon / Sacrifice

## Görsel Spec — v3 İyileştirme

10 PNG, ~1280×960, **v2'den farkı**:

### Portrait Treatment
- Class character sprite (gerçek PNG) **2× upscale** (240×240) drop shadow + background haze
- Karakter pose: south view default (PixelLab anchor)
- Theme color halo around portrait

### Skill Icon Treatment
- Her skill için **UNIQUE ICON** — class theme color + skill action visual (sword arc / arrow trail / spell glyph / etc.)
- 64×64 mini pixel art icons
- Group by skill type: Active (full color) / Passive (muted border) / Ultimate (gold border)
- Text label below icon (skill name, max 12 chars truncate)

### Layout
```
┌─────────────────────────────────────────────────────────┐
│  [CLASS BANNER]              [N skills indicator]       │
│  [theme color frame]                                     │
│                                                          │
│  ┌─────────────┐   ┌──────────────────────────────────┐ │
│  │             │   │  SKILLS — class signature icons  │ │
│  │   ACTUAL    │   │  ┌──┐ ┌──┐ ┌──┐ ┌──┐ ┌──┐      │ │
│  │  CHARACTER  │   │  │S1│ │S2│ │S3│ │S4│ │S5│      │ │
│  │   PORTRAIT  │   │  └──┘ └──┘ └──┘ └──┘ └──┘      │ │
│  │  (sprite)   │   │  ... grid varies by count       │ │
│  │             │   └──────────────────────────────────┘ │
│  └─────────────┘                                         │
│  Theme: ...                                              │
│  Resource: ...                                           │
│  Signature: ...                                          │
└─────────────────────────────────────────────────────────┘
```

## Critical Differences from v2

v2 issue → v3 fix:

| v2 Problem | v3 Solution |
|---|---|
| Generic warrior portrait | Real PixelLab character sprite (upscaled) |
| Text-only "CLE" "GC" icons | Unique 64×64 icon per skill — class signature visual |
| Same icon palette across classes | Per-class theme color LOCK (Warblade orange-gray / Elem multi-prism / etc.) |
| Sadece 6 skill | ALL skills (14/15/20/22/4 + 8-10 concept) |
| Generic abstract shape | Character-identity sprite (PixelLab actual) |

## Output

```
STAGING/concepts/skill_sheets_v3/
  01_warblade_v3_sheet.png
  02_ronin_v3_sheet.png
  03_gunslinger_v3_sheet.png
  04_ranger_v3_sheet.png
  05_elementalist_v3_sheet.png
  06_shadowblade_v3_sheet.png
  07_ravager_v3_sheet.png
  08_hexer_v3_sheet.png
  09_brawler_v3_sheet.png
  10_summoner_v3_sheet.png
```

10 PNG. Eksik sprite (download fail) için skip + log.

## Final Report

`STAGING/_codex_done/CODEX_DONE_skill_sheets_v3.md`:
- 10 PNG list (PASS/SKIP per class with reason)
- Per-class portrait+icon quality verdict
- v2 → v3 improvement summary
- Production cost
- Sabah orchestrator için karar gate: v3 final mi yoksa v4 polish gerek mi

## Dispatch (after b34sf9gmj character download completes)

`--effort high --profile laurethayday --timeout 3600`, background.
