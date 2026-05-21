# Codex Task — Skill Sheets v5 (Canonical Sprite + Full Skill Coverage + PixelLab Feasibility)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```

---

## Görev

v4 skill sheet 2 büyük problemle reject edildi:
1. Codex kendi uydurma karakter çizdi — canonical sprite kullanmadı
2. Sadece 4 signature skill gösterdi — class'ların 4-22 skill var, **HEPSİ** görünmeli

v5 = canonical sprite-faithful + full skill coverage + PixelLab feasibility tagged.

## v4 PROBLEM (don't repeat)

- Codex Warblade'i kel/zırhlı bir adam çizdi → canonical Warblade chibi siyah saç + üniforma sprite
- Codex Elementalist'i sarışın robe çizdi → canonical kimliği tutmadı
- 4 skill/class → 14-22 skill'lik class'ların çoğu kaybedildi

## v5 ZORUNLU KOŞULLAR

### A. Canonical Sprite Faithful (HARD RULE)

Her class için canonical sprite path:
```
Assets/Art/Characters/Warblade/Rotations/warblade_south.png
Assets/Art/Characters/Ronin/Rotations/ronin_south.png
Assets/Art/Characters/Gunslinger/Rotations/gunslinger_south.png
Assets/Art/Characters/Ranger/Rotations/ranger_south.png
Assets/Art/Characters/Elementalist/Rotations/elementalist_south.png
Assets/Art/Characters/Shadowblade/Rotations/shadowblade_south.png
Assets/Art/Characters/Ravager/Rotations/ravager_south.png
Assets/Art/Characters/Hexer/Rotations/hexer_south.png
Assets/Art/Characters/Brawler/Rotations/brawler_south.png
Assets/Art/Characters/Summoner/Rotations/summoner_south.png
```

**MANDATORY workflow:**
1. Codex her class için önce canonical south.png'yi OKU (file_path verildi, image_read tool veya manuel inspection)
2. Sprite'ı tarif et: anatomi (chibi 64×64), kıyafet rengi/desen, saç, silah görünür mü, ten rengi, yaş izlenimi
3. Bu tarifi her scene prompt'una **birebir** dahil et
4. **Codex'in kendi uydurması YASAK** — sprite tarifinden sapma = fail

Örnek Warblade canonical tarif (sen kendin onu sprite'tan çıkar):
```
Chibi pixel character, 64x64, black messy hair, dark steel armor uniform with 
brown leather straps, light skin, neutral expression, holding greatsword.
```

### B. Full Skill Coverage (HARD RULE)

`STAGING/concepts/skill_sheets_v3/skill_enumeration_v3.json` oku — toplam 115 skill, 10 class.

Her class için **TÜM** skill'leri ayrı panelde göster (küçük panel OK):

| Class | Skill# | Grid önerisi | Panel size |
|---|---:|---|---|
| Ronin | 4 | 2×2 | 512×512 each |
| Gunslinger | 8 | 4×2 | 384×384 each |
| Ravager | 8 | 4×2 | 384×384 each |
| Hexer | 8 | 4×2 | 384×384 each |
| Brawler | 8 | 4×2 | 384×384 each |
| Summoner | 8 | 4×2 | 384×384 each |
| Warblade | 14 | 5×3 | 320×320 each (1 empty) |
| Elementalist | 15 | 5×3 | 320×320 each |
| Ranger | 20 | 5×4 | 320×320 each |
| Shadowblade | 22 | 6×4 | 256×256 each (2 empty) |

Sheet toplam boyut: 1600×1600 px standart (Ronin 1024×1024 OK küçük olduğu için).

Atlama YOK. 115/115 skill görünmeli.

### C. PixelLab Feasibility Annotation (HARD RULE)

Her skill için aşağıdaki kategorilerden birini seç + production approach yaz:

| Tag | Tanım | Örnek |
|---|---|---|
| EASY | Tek karakter pose + tek-frame VFX, static effect | Cleave arc, Iron Crush slam, Fireball, Backstab |
| MEDIUM | Karakter + 1 ayrı placed sprite (trap/totem/summon) | Bone Trap, Living Bomb, Summon Wisp, Mirror Image |
| MEDIUM-HARD | Single dynamic path animation (projectile/dash) | Black Arrow, Frozen Orb, Axe Throw, Hex Bolt |
| HARD | İki actor arası canlı bağ (chain/beam/tether) | Shackle Curse, Tethering Arrow, Soul Link, Chain Lightning |
| HARD-COMPOSITE | Multi-actor chain n-jump | Chain Cull multi-target |

Format `STAGING/concepts/skill_sheets_v5/v5_skill_feasibility.json`:
```json
{
  "Warblade": [
    {
      "skill": "Cleave",
      "pixellab": "EASY",
      "approach": "Single character mid-swing + arc VFX sprite, 1 gen + Unity overlay",
      "notes": "n_frames=4-8 enough"
    },
    {
      "skill": "Iron Charge",
      "pixellab": "MEDIUM-HARD",
      "approach": "Character dash sequence + trail VFX, 8-frame anim + Unity LineRenderer",
      "notes": "Dash distance Unity-side"
    },
    ...
  ],
  "Ronin": [...],
  ...
}
```

### D. Scene Composition (HARD RULE)

Her scene'de:
- **Canonical sprite-faithful** karakter (önceki kuralla)
- **Class signature weapon** görünür (Warblade greatsword, Ronin katana, Gunslinger twin pistols, Ranger longbow, Elementalist orb staff, Shadowblade daggers, Ravager axe, Hexer cursed totem, Brawler fists/gauntlet, Summoner tome/sigil)
- **Karşı mob** (Act 1 mob roster — variety daha iyi):
  - Bone Walker, Bone Archer, Cyan Slime, Goblin, Imp Demon, Specter Ghost, Wraith Specter, Skull, Bat, Dungeon Rat, Animated Skull, Husk, Giant Spider, Ground Crawler, Rat King, Cyan Wisp
- **Mob mid-hit reaction** — knocked, sliced, frozen, burned, on fire, electrified, hexed (skill'e uygun)
- **Skill VFX active frame** — skill'in görsel imzası clear
- **30-35° angled iso camera** (RIMA canonical)
- **Act 1 environment hint** — granite floor + subtle cyan rift accent background
- **Skill name caption** panel altında, küçük

Karakter aynı (canonical), düşman farklı her skill'de.

### E. Output Format

1. **10 PNG composite sheet:** `STAGING/concepts/skill_sheets_v5/0X_<class>_v5_all_skills.png` (1024-1600 px scaled by class skill count)
2. **`v5_skill_feasibility.json`** — 115 skill PixelLab annotation
3. **`v5_render_log.md`** — generation params, hangi prompt hangi panel, retry varsa not, canonical sprite tarif sözlüğü (10 class)

### F. Sprite Tarif Sözlüğü (Codex Kendin Doldur)

Her class için canonical south.png'yi inceleyip 1-2 cümle tarif yaz. Bu sözlük v5_render_log.md'ye yapıştırılacak:

```
Warblade: [your description from sprite inspection]
Ronin: [...]
Gunslinger: [...]
Ranger: [...]
Elementalist: [...]
Shadowblade: [...]
Ravager: [...]
Hexer: [...]
Brawler: [...]
Summoner: [...]
```

Bu sözlük her scene prompt'una embed edilecek — canonical drift engelleyici.

## Kısıt

- v4 illustrative style YASAK — RIMA pixel art canonical mood şart
- Codex kendi karakter uydurma YASAK — sprite'tan sap = fail
- Skill atlama YASAK — 115/115 görünmeli
- "Düşmana zincir bağlama" gibi HARD skill'ler için panel'de **statik zincir frame** OK (Unity gameplay-side dinamik anchor sistem ayrı iş)
- Modern UI overlay YASAK
- Stay/Break/Carry image elementi YASAK
- Class signature weapon her panelde görünmeli

## Effort
high
