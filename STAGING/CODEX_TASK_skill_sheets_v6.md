# Codex Task — Skill Sheets v6 (Demo Scope, Game-Asset Quality, NO Programmatic Compositing)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

---

## CRITICAL — Why v5 FAILED

v5 BÜYÜK BAŞARISIZLIK. Codex:
1. Canonical sprite'ı **literal olarak paste** etti (PIL/ImageMagick programmatic compositing)
2. Üzerine **geometrik primitive arc** çizdi (line draw, color square)
3. Düşmanı **renkli kare** yaptı (programmatic shape)
4. Her panel **aynı pose**, sadece arc rengi + mob rengi farklı

Sonuç: Hiç oyun görseli değil, test-render gibi.

**HARD RULE v6:** PROGRAMMATIC COMPOSITING YASAK. PIL/ImageMagick ile sprite paste + primitive draw YOK. Tüm pixel art Codex `image_gen` tool ile ÜRETİLECEK. v4 quality baseline (paint illustration mood, action pose, mob sprite, VFX) AMA canonical sprite identity korunarak.

---

## Görev

Demo scope için 4 class × full skill list, **gerçek oyun görseli kalitesi** spritesheet-style composite. Her skill ayrı illustration panel.

## Scope — TÜM 10 CLASS, 115 SKILL

User talebi: bütün skill'leri görmek. Demo scope iptal, full coverage.

| Class | Skill# |
|---|---:|
| Warblade | 14 |
| Ronin | 4 |
| Gunslinger | 8 |
| Ranger | 20 |
| Elementalist | 15 |
| Shadowblade | 22 |
| Ravager | 8 |
| Hexer | 8 |
| Brawler | 8 |
| Summoner | 8 |
| **TOPLAM** | **115** |

10 class × N skill = 10 composite sheet. Diğer class'lar dahil — Karar #150 priority sadece quality prioritization, scope full.

## Workflow (HARD RULE — bu sıra)

### Step 1 — Canonical Sprite Bible Yaz
Her 10 class için canonical south.png'yi `image_read` ile incele, **çok detaylı text description** yaz (1 paragraf, 5-8 cümle):
- Anatomi (chibi proportions, kafa/vücut oranı)
- Yaş izlenimi (genç/orta/yaşlı)
- Saç (renk, stil, uzunluk)
- Yüz hatları (ten rengi, ifade)
- Kıyafet (renk paleti, malzeme, detay)
- Silah (varsa görünen)
- Ten tonu, vücut tipi
- Özel görsel imza (cape, kapüşon, accessories)

Yazılır `STAGING/concepts/skill_sheets_v6/canonical_sprite_bible.md`:
```
## Warblade
[7-cümle detaylı tarif]

## Ronin
## Gunslinger
## Ranger
## Elementalist
## Shadowblade
## Ravager
## Hexer
## Brawler
## Summoner
[hepsi için aynı]
```

Skill action description hint'leri için reference:
`STAGING/_chatgpt/SKILL_SHEET_v6_CHATGPT_MASTER.md` Section 4 — orchestrator 115 skill için per-skill visual hint zaten yazdı, oradan oku.

### Step 2 — Per-Skill Panel Generate
Her 115 skill için **ayrı ayrı 1 panel** `image_gen` ile üret. Panel boyutu: **320×320 px** (composite'a uyum için).

Her panel prompt template (her zaman bu yapıyı kullan):

```
Pixel art game illustration, 30-35 degree angled isometric perspective.
Character: [CANONICAL SPRITE BIBLE'dan o class'ın tam paragrafı yapıştır]
Action: [CLASS] character executing [SKILL NAME] - [skill specific action description].
Target: [Act 1 mob name: Bone Walker / Bone Archer / Cyan Slime / Goblin / Imp Demon / Specter Ghost / Wraith Specter / Skull / Bat / Dungeon Rat / Animated Skull / Husk / Giant Spider / Ground Crawler / Rat King / Cyan Wisp] in mid-hit reaction, [impact visual: knocked back / sliced through / frozen mid-pose / burning / electrified / bleeding / hexed].
Environment: dark granite floor with cyan rift accent, dungeon stone walls, atmospheric.
Style: RIMA pixel art canonical mood, chibi character proportions (small head-body ratio matches sprite reference), painterly Hades+Diablo synthesis lighting, NOT cinematic illustration, NOT flat icon, NOT photographic.
Negative: programmatic geometry, primitive shapes, colored squares, test render look, AI-generated artifacts, modern UI elements, anime cel-shading, sprite paste look.
```

**KRİTİK:** Her panel kendi başına Codex image-gen ile üretilecek. NO PIL PASTE, NO PRIMITIVE DRAW.

Karşı mob variety: aynı class'ın 14-22 panel'inde **mob variety korunsun** (her panel farklı veya rotation). Bone Walker spammine girmesin.

### Step 3 — Composite Sheet
115 panel üretildikten sonra, Python (PIL OK BURADA — sadece final composite için) ile her class'a göre grid composite:

| Class | Skill# | Grid | Sheet boyut |
|---|---:|---|---|
| Warblade | 14 | 5×3 (1 boş) | 1600×960 |
| Ronin | 4 | 2×2 | 640×640 |
| Gunslinger | 8 | 4×2 | 1280×640 |
| Ranger | 20 | 5×4 | 1600×1280 |
| Elementalist | 15 | 5×3 | 1600×960 |
| Shadowblade | 22 | 6×4 (2 boş) | 1920×1280 |
| Ravager | 8 | 4×2 | 1280×640 |
| Hexer | 8 | 4×2 | 1280×640 |
| Brawler | 8 | 4×2 | 1280×640 |
| Summoner | 8 | 4×2 | 1280×640 |

Üst banner: class adı + skill count + canonical sprite path
Her panel altı: skill name caption (12-16px, beyaz)
PixelLab feasibility tag: panel sağ-üst köşesinde küçük (EASY/MEDIUM/HARD)

**Composite kuralı:** SADECE panel'leri grid'e diz + caption ekle + banner ekle. Sprite paste, primitive draw, geometric arc YOK.

### Step 4 — Output

`STAGING/concepts/skill_sheets_v6/`:
- `canonical_sprite_bible.md` (10 class detaylı sprite tarifi)
- `01_warblade_v6.png` (1600×960, 14 panel composite)
- `02_ronin_v6.png` (640×640, 4 panel)
- `03_gunslinger_v6.png` (1280×640, 8 panel)
- `04_ranger_v6.png` (1600×1280, 20 panel)
- `05_elementalist_v6.png` (1600×960, 15 panel)
- `06_shadowblade_v6.png` (1920×1280, 22 panel)
- `07_ravager_v6.png` (1280×640, 8 panel)
- `08_hexer_v6.png` (1280×640, 8 panel)
- `09_brawler_v6.png` (1280×640, 8 panel)
- `10_summoner_v6.png` (1280×640, 8 panel)
- `panels/` alt klasör: ham 115 panel PNG (atılır sonra ama backup için tut)
- `v6_skill_feasibility.json` (115 skill PixelLab annotation, v5'tekiyle aynı format)
- `v6_render_log.md` (her panel için kullanılan prompt + retry varsa not)

## Kalite Standardı

Her panel'in `v4_quality` baseline'ında olması ZORUNLU:
- Karakter pose ACTION ANIM (idle DEĞİL)
- Silah görünür ve mid-swing/active state
- VFX painterly pixel art (geometric arc DEĞİL)
- Mob proper pixel art mob (colored square DEĞİL)
- Environment Act 1 atmosphere (cyan rift + granite)
- 30-35° iso perspective (top-down değil, side değil)

**v4 örneği reference:** `STAGING/concepts/skill_sheets_v4/01_warblade_v4_in_action.png` — bu kalite seviyesi MINIMUM. Tek fark: v4 4 skill, v6 14-22 skill + canonical sprite identity preserved.

## Kısıt

- Programmatic compositing (sprite paste + primitive draw) YASAK — v5'in başarısızlığını tekrar etme
- Codex'in kendi karakter uydurması YASAK — canonical sprite bible'a sadık kal
- Skill atlama YASAK — 71/71 panel
- Modern UI overlay YASAK
- "Düşmana zincir bağlama" gibi HARD skill için statik impact frame OK (animasyon ayrı iş)
- v6 full scope = 10 class × 115 skill
- PIL composite SADECE final grid layout için (panel paste değil, panel place)

## Effort
high
