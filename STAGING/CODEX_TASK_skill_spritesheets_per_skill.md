# Codex Task — Per-Skill Spritesheets (115 ayrı spritesheet, tek tek)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

---

## CRITICAL — Execution Mode

**v6 başarısız oldu** çünkü Codex `shell command` + `OPENAI_API_KEY` arıyordu. Bu görevde:

**ZORUNLU:** `image_gen` Codex built-in tool kullanılacak. Shell komutu DEĞİL, openai/comfyui API DEĞİL, **doğrudan Codex'in image_gen tool çağrısı**. v6'da Warblade Battle Surge için 1 panel başarıyla üretildi — aynı yöntem 115 skill için tekrarlanacak.

Eğer image_gen tool'unu doğrudan çağıramıyorsan veya iterate edemiyorsan, BLOCKED yaz. Shell workaround arama.

---

## Görev

Her skill için **1 ayrı spritesheet** (4-frame horizontal animation strip) üret. Toplam **115 spritesheet, 10 class**.

## Spritesheet Format — TWO-STAGE PIPELINE

### Stage 1: Codex image_gen (native output)
- image_gen call ile native horizontal strip üret (4 frame yatay)
- Native boyut Codex'in döndürdüğü ne ise OK (örn. 1774×887 acceptable)
- Asıl ŞART: 4 frame yatay strip composition, character consistency, painterly quality

### Stage 2: PIL Post-Process (Python shell, OK BURADA)
- Native PNG'yi oku
- **PIL ile aspect-correct resize** → 1024×256 (LANCZOS interpolation)
- Save to final path

Eğer Codex tek pass'te 1024×256 üretmek için zorlanırsa, BU PIPELINE kullan. v6 attempt'te 1774×887 native quality MÜKEMMEL — sadece downscale gerek.

| Final Parametre | Değer |
|---|---|
| Final boyut | 1024×256 px |
| Frame sayısı | 4 (yatay strip) |
| Frame boyutu | 256×256 px each |
| Layout | Frame 1 → Frame 2 → Frame 3 → Frame 4 (soldan sağa) |
| Animation | Windup → Active → Impact → Recovery |

### PIL Downscale Script (per skill, inline call)
```python
from PIL import Image
img = Image.open("native_output.png")
resized = img.resize((1024, 256), Image.LANCZOS)
resized.save("final/01_battle_surge.png")
```

## Read These Files (input)

1. `STAGING/_chatgpt/SKILL_SHEET_v6_CHATGPT_MASTER.md` — Section 4'te 115 skill için visual hint listesi
2. `STAGING/concepts/skill_sheets_v6/canonical_sprite_bible.md` — 10 class detaylı sprite tarifi (Codex v6 attempt'te yazıldı)
3. `STAGING/_chatgpt/sprites_south/01_warblade_south.png` ... `10_summoner_south.png` — canonical sprite reference
4. `STAGING/concepts/skill_sheets_v6/panels/01_warblade_01_battle_surge.png` — v6 quality reference (Codex'in ürettiği örnek)

## Per-Skill Spritesheet Prompt Template

Her skill için image_gen'e şu format gönder:

```
Pixel art 4-frame horizontal animation spritesheet, 1024x256 px (4 panels of 256x256 side by side).

Character (consistent across all 4 frames):
[INSERT CLASS DESCRIPTION FROM canonical_sprite_bible.md — full paragraph]

Weapon: [class signature weapon — Section 3 of SKILL_SHEET_v6_CHATGPT_MASTER.md]

Skill: [SKILL NAME]

Frame 1 (Windup, 256x256, leftmost): Character in pre-action pose, [windup specific to skill]. Mob standing nearby unaware.

Frame 2 (Active, 256x256): Character executing skill, [skill VFX active — derive from visual hint: SKILL_SHEET_v6_CHATGPT_MASTER.md Section 4]. VFX painterly pixel art.

Frame 3 (Impact, 256x256): Mob mid-hit reaction, [impact effect]. Maximum VFX intensity.

Frame 4 (Recovery, 256x256, rightmost): Character returning to neutral pose. Mob in aftermath state (knocked, dead, debuffed, fleeing).

Background (consistent across all frames): dark granite floor with subtle cyan rift accent, dungeon stone walls, atmospheric Act 1 lighting.

Style: 30-35 degree angled isometric perspective, chibi character proportions matching reference sprite, painterly Hades+Diablo synthesis pixel art mood, NOT cinematic illustration, NOT photographic, NOT anime cel-shaded.

Negative: programmatic geometry, primitive shapes, colored squares for mobs, test render look, modern UI overlay, sprite paste artifacts, inconsistent character between frames.
```

## Mob Variety Rule

`STAGING/_chatgpt/SKILL_SHEET_v6_CHATGPT_MASTER.md` Section 2'de Act 1 mob roster (15 mob). Her skill spritesheet için farklı mob seç (rotation). Aynı mob 2 ardışık skill'de KULLANILMASIN.

## Output Structure

```
STAGING/concepts/skill_sheets_v6/spritesheets/
  01_warblade/
    01_battle_surge.png       (1024x256)
    02_blade_rush.png
    03_cleave.png
    ... (14 toplam)
  02_ronin/
    01_final_draw.png         (1024x256)
    02_iaido_stance.png
    03_quickdraw.png
    04_sakura_veil.png
  03_gunslinger/
    01_twin_fire.png
    ... (8 toplam)
  04_ranger/
    ... (20 toplam)
  05_elementalist/
    ... (15 toplam)
  06_shadowblade/
    ... (22 toplam)
  07_ravager/
    ... (8 toplam)
  08_hexer/
    ... (8 toplam)
  09_brawler/
    ... (8 toplam)
  10_summoner/
    ... (8 toplam)
```

Dosya adı convention: `{skill_index_2digit}_{skill_name_slugified_snake_case}.png`

## Quality Gates (Her Spritesheet İçin)

Generate ettikten sonra şunu doğrula:
1. ✅ 4 frame yatay strip (1024×256) — tek panel değil
2. ✅ Karakter 4 frame'de canonical sprite'a sadık
3. ✅ Class weapon her frame'de görünür
4. ✅ Mob spec'e uygun reaction (knocked/sliced/frozen/burned vs.)
5. ✅ VFX painterly pixel art (geometric primitive değil)
6. ✅ Background consistent across 4 frames
7. ✅ Frame 1→4 animation progression okunabilir

Eğer fail → o skill için **regenerate** et, sonraki skill'e geç.

## Execution Order

Sırayla, 10 class boyunca:
1. Warblade 14 skill
2. Ronin 4 skill
3. Gunslinger 8 skill
4. Ranger 20 skill
5. Elementalist 15 skill
6. Shadowblade 22 skill
7. Ravager 8 skill
8. Hexer 8 skill
9. Brawler 8 skill
10. Summoner 8 skill

Toplam: 115 image_gen call, ~30-60 saniye per call.

## Progress Log

`STAGING/concepts/skill_sheets_v6/spritesheets/progress_log.md` yaz, her skill bittikten sonra güncelle:

```
## 01 Warblade (14/14 done)
- [x] 01_battle_surge.png — generated 2026-05-21 14:30
- [x] 02_blade_rush.png — generated 2026-05-21 14:31
- [ ] 03_cleave.png
...

## 02 Ronin (0/4)
- [ ] 01_final_draw.png
...
```

Progress log her ~10 spritesheet sonrası güncellensin (file write efficient).

## Kısıt

- image_gen Codex built-in tool ZORUNLU — shell/API workaround YASAK
- Aynı skill için aynı mob 2 farklı spritesheet'te kullanma (variety)
- Composite YAPMA — final composite sheet üretme, sadece per-skill spritesheets
- Karakter inconsistency 4 frame içinde YASAK — frame 1 ile frame 4 aynı kimlik olmalı
- v5 başarısızlığı tekrar etme — programmatic geometry/primitive shape YASAK

## Effort
high
