---
name: weapon-master-spec-10-class
description: 10 class silah master spec — NLM canonical + S99 LATE plan. Weapon decouple Karar #144/#123 (HandAnchor child), 1-dir runtime rotation (8-dir gen YOK), PixelLab S-XL new + Init Image. Mevcut LIVE 2 silah + 8 production queue.
metadata:
  type: project
  source: NLM 30ddffa5 2026-05-27 + project_weapon_pipeline_lock + project_warblade_weapon_animation_plan_s99_late
---

## Ayrık Silah Sistemi (Karar #144 + #123)
1. **Body-Only Sprite:** Karakter bedeni silahsız PixelLab gen
2. **Hand Anchor:** Aseprite'ta elin dominant pixel'ine magenta `#FF00FF` nokta → Unity `HandAnchor`
3. **WeaponHolder:** Silah tek statik sprite olarak HandAnchor child'a → `WeaponEquip.Equip(WeaponDefinition)`
4. **Animation:** Body sprite weaponless animasyon, silah `Transform.rotation` AnimationCurve ile runtime swing

## View Direction Disiplin
- **Silahlar 8-dir gen YAPILMAZ** — 1-dir static + AnimationCurve runtime rotation
- **Karakter body** 8-dir (Karar #114, 5 native + 3 mirror flipX)
- Saat ibresi mantığı: 1 statik sprite + N curve = N saldırı animasyonu

## PixelLab Production Tool
- **Create Image S-XL (new)** ana tool
- `Direction: None`
- `Transparent Background: ON`
- Init Image (`<class>_<weapon>_init.png`) — silah bölgesi crop + padded canvas
- `Style anchor: <class>_south_clean.png` (face cleanup sonrası anchor)
- ~1 gen per silah

## 10 Class Silah Canonical Spec

| # | Class | Silah | Canvas (PixelLab) | Final scale (Unity) | View | PPU | LIVE? | PixelLab ID |
|---|---|---|---|---|---|---|---|---|
| 1 | **Warblade** | Two-handed Greatsword | 256×256 → 192×192 trim | 96×96 | 1-dir | 64 | ✅ LIVE | `441bccf0-9d9c-4bb7-a981-555b132eae00` |
| 2 | **Ronin** | Katana drawn + Sheath (Karar #71, sheath sol bel) | 128×128 | 64×64 | 1-dir | 64 | ✅ LIVE (drawn) | `692f43ce-2c6d-45ea-910d-2b5ec4f6ec99` |
| 3 | **Gunslinger** | Dual Rift-tech Pistols (L+R mirror) | 64×64 | 32×32 | 1-dir runtime rot | 64 | ❌ queue | — |
| 4 | **Ranger** | Compound Bow + Arrow | 128×128 | 64×64 | 1-dir | 64 | ❌ queue (3 bow variant V2 yedek) | — |
| 5 | **Elementalist** | Floating Golden Rune Disc (NO STAFF NO WAND, Karar #146) | 96×96 | 48×48 | 1-dir hover | 64 | ❌ queue | — |
| 6 | **Shadowblade** | Twin Daggers reverse-grip (L+R mirror) | 64×64 | 32×32 | 1-dir | 64 | ❌ queue (15 dagger variant V2 yedek) | — |
| 7 | **Ravager** | Dual Compact Axes (hatchet pair) | 128×128 | 64×64 | 1-dir | 64 | ❌ queue | — |
| 8 | **Hexer** | Grimoire / Cursed Totem / Scepter (variant) | 96×96 | 48×48 | 1-dir | 64 | ❌ queue | — |
| 9 | **Brawler** | Fists / Gauntlets (body part, separate sprite optional) | 96×96 | 48×48 pair | 1-dir | 64 | ❌ queue (or body baked) | — |
| 10 | **Summoner** | Soul Lantern (sol el, NO staff swing, hover lantern) | 96×96 | 48×48 | 1-dir hover | 64 | ❌ queue | — |

## Prompt Template (per weapon)

```
Tool: PixelLab Create Image S-XL (new)
Init Image: <class>_<weapon>_init.png (silah bölgesi crop + padded)
Style Anchor: <class>_south_clean.png

Prompt:
"<weapon_type>, <materials> (e.g., dark steel blade, leather wrap grip, brass fittings), worn battle-tested, high top-down 3/4 view, pixel art, 16-color palette, crisp single-color outline, transparent background. Match style of init image."

Settings:
- Direction: None
- Detail: Highly detailed
- Outline: Single color outline
- Transparent background: ON
- Canvas: per spec table above
- PPU=64 (Unity import target)

Negative:
- no character body
- no hands
- no background
- no extra props
- match init image colors and style
```

## PixelLab Object Inventory Notu
- **Mevcut RIMA Knowledge Base'de spesifik weapon Object ID kayıt YOK** (NLM verdict)
- Silahlar **lokal PNG** olarak yönetilir (`Assets/Sprites/Weapons/<Class>/<weapon>.png`)
- PixelLab cloud library referansı yerine direct local file
- **Mevcut PixelLab inventory 244 obje** — cliff/wall/environment ağırlıklı, weapon-spesifik tag YOK (tarama Track B otonom için yapılabilir, demo zorunluluğu yok)

## Production Queue (Track B / Faz 4)
Öncelik sırası (4-char playtest set):
1. Elementalist orb (mage archetype, NO staff lock)
2. Ranger bow (ranged physical)
3. Shadowblade dagger pair (stealth melee)
4. Ronin katana sheath (drawn LIVE, sheath separate, Karar #71)

Sonra (6-char tail):
5. Gunslinger dual pistols
6. Ravager dual axes
7. Hexer grimoire/scepter
8. Brawler gauntlet (or body-baked decision)
9. Summoner soul lantern
10. Warblade alt weapons (V2 skin)

## Storage Path
- Master: `Assets/Sprites/Weapons/<Class>/<weapon_filename>.png`
- Variant queue: `STAGING/weapon_variants/<class>/`
- WeaponDefinition SO: `Assets/Data/Weapons/<class>_<weapon>.asset`

## Mevcut RIMA Asset Inventory
- `Assets/Sprites/Weapons/Warblade/greatsword_v1.png` ✅
- `Assets/Sprites/Weapons/Ronin/katana_drawn_v1.png` ✅
- Diğer 8 class: STAGING'de mock/proof varsa kullan, yoksa PixelLab Web UI manuel gen (kullanıcı, Faz 4)

## Cross-link
[[canonical-character-roster-v2]] [[project-weapon-pipeline-lock]] [[project-warblade-weapon-animation-plan-s99-late]] [[nine-class-animation-states-demo-phase1-plan]] [[warblade-animation-states-demo-phase1-plan]] [[reference-pixellab-prompt-grammar]] [[feedback-pixellab-mcp-halt-strict]]
