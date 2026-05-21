# Codex Task — Skill Sheets v4 (In-Action Combat Snapshot Format)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```

---

## Görev

v3 skill sheet'leri **flat icon grid** formatında — user bunu reject etti. v4 **in-action combat snapshot** formatında olacak. Codex image generation (gpt-image-1) ile üret.

## v3 Problem (DON'T REPEAT)

- Karakter portresi sol panelde STATİK
- Sağda 14 küçük symbolic icon (kılıç sembolü, vs.)
- Skill ne yaptığını GÖSTERMİYOR
- Mob/boss yok
- Combat anı yok

Reference: `STAGING/concepts/skill_sheets_v3/01_warblade_v3_sheet.png` — bu format YASAK.

## v4 Hedef Format

Her karakter için **1 composite sheet**, layout:

```
┌─────────────────────────────────────────────────────────┐
│ WARBLADE — Signature Skill Demos                        │
├──────────────┬──────────────┬──────────────┬───────────┤
│ SCENE 1      │ SCENE 2      │ SCENE 3      │ SCENE 4   │
│ "Cleave"     │ "Iron Crush" │ "Earthsplit" │ "Blade R" │
│              │              │              │           │
│ [Warblade    │ [Warblade    │ [Warblade    │ [Warblade │
│  swinging    │  slamming    │  ground      │  dashing  │
│  greatsword  │  weapon down │  fracture    │  forward  │
│  in arc,     │  on bone     │  cyan rift   │  past mob │
│  bone walker │  archer mid- │  erupting    │  trailing │
│  splitting]  │  collapse]   │  under skull]│  steel]   │
│              │              │              │           │
│ [skill name] │ [skill name] │ [skill name] │ [skill n] │
└──────────────┴──────────────┴──────────────┴───────────┘
```

## Her Scene'in Gerekli Öğeleri

1. **Karakter sprite** — own canonical class sprite (Warblade greatsword steel, Elementalist robe orb, Ranger bow leather, etc.), mid-attack pose
2. **Karakter silahı görünür** — class signature weapon (Warblade=greatsword, Ronin=katana, Gunslinger=twin pistols, Ranger=longbow, Elementalist=orb staff, Shadowblade=daggers, Ravager=axe, Hexer=cursed totem, Brawler=fists/gauntlet, Summoner=tome/sigil)
3. **Karşı mob/boss** — Act 1 mob roster'dan biri (Bone Walker / Bone Archer / Cyan Slime / Goblin / Imp Demon / Specter Ghost / Wraith Specter / Skull / Bat / Dungeon Rat). Her scene farklı mob daha iyi.
4. **Skill VFX aktif** — skill'in görsel imzası (cleave = wide arc trail, earthsplit = cracking ground, blade rush = dash trail, etc.)
5. **Mob hit reaction** — mob mid-impact (knocked, sliced, knocked back, on fire, frozen, etc.)
6. **30-35° angled iso camera** — RIMA canonical perspective (NOT pure side-view, NOT pure top-down)
7. **Act 1 environment hint** — granite floor + cyan rift accent (subtle, background)

## Karakter Listesi + Skill Seçimi

Her karakter için **4 signature skill** seç (full liste skill_enumeration_v3.json'da, en iconic/visual olanlar):

| # | Class | 4 Signature Skill (öneri, gerekirse listeden değiştir) |
|---:|---|---|
| 01 | Warblade | Cleave / Iron Crush / Earthsplitter / Blade Rush |
| 02 | Ronin | Quickdraw / Iaido Stance / Sakura Veil / Final Draw |
| 03 | Gunslinger | Twin Fire / Ricochet Shot / Fan The Hammer / Dead Eye |
| 04 | Ranger | Aimed Shot / Black Arrow / Bone Trap / Barbed Net Shot |
| 05 | Elementalist | (en iconic 4'ünü seç skill_enumeration_v3.json'dan) |
| 06 | Shadowblade | (en iconic 4'ü) |
| 07 | Ravager | (en iconic 4'ü) |
| 08 | Hexer | (en iconic 4'ü) |
| 09 | Brawler | (en iconic 4'ü) |
| 10 | Summoner | (en iconic 4'ü) |

Skill seçimi kriteri: **visually distinct** olmalı, 4 skill aynı animasyona benzemesin (ör. 2 farklı melee swing değil — 1 melee + 1 ranged + 1 area + 1 utility).

## Render Spec

- **Format:** 1 PNG per character, **1280×1280 px** (4-quadrant grid)
- **Style:** RIMA canonical pixel art mood, NOT full painted illustration — chibi proportions OK (64-128px sprite scale within scene)
- **Palette:** Act 1 granite + cyan rift accent
- **Quality:** Visual reference clarity (her scene 1 saniyede okunabilir)
- **Naming:** `STAGING/concepts/skill_sheets_v4/0X_<classname>_v4_in_action.png` (01-10 sırası)

## Codex Image Generation Prompt Template (per scene)

```
Pixel art game illustration, 30-35 degree angled isometric perspective,
[CLASS NAME] character mid-action using [SKILL NAME]:
  - Character: [class sprite description, own canonical weapon visible]
  - Action: [exact skill animation description, VFX visible]
  - Target: [Act 1 mob name] in mid-hit reaction, [impact effect description]
  - Environment: dark granite floor, cyan rift accent in background, dungeon stone walls
  - Style: chibi proportions, RIMA pixel art canonical, Hades + Diablo synthesis mood
Negative: flat icon, symbolic representation, static portrait, modern UI elements,
photographic, anime cel-shaded
```

Her karakter sheet'i için 4 scene'i tek composite PNG'de birleştir (Codex's image gen 4-panel grid çıkarabilir, veya 4 ayrı scene gen + tile).

## Çıktı

1. 10 PNG: `STAGING/concepts/skill_sheets_v4/01_warblade_v4_in_action.png` ... `10_summoner_v4_in_action.png`
2. `STAGING/concepts/skill_sheets_v4/v4_skill_selection.json` — her karakterin seçilen 4 skill listesi (orchestrator/user override için)
3. `STAGING/concepts/skill_sheets_v4/v4_render_log.md` — generation params, hangi prompt hangi PNG'yi üretti, retry varsa not

## Kısıt

- v3 flat-icon format YASAK
- Free-asset-pack mood YASAK (Hades-clone/Diablo-clone/AD-clone tek başına YASAK — sentez şart)
- Modern UI overlay element YASAK (sadece skill name caption izinli)
- Stay/Break/Carry meta-track image'de YASAK (henüz canonical değil)

## Effort
high
