# Task: Boss Fight Concept Images — Codex imagegen skill

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
RIMA Act 1 boss fight için 2 farklı görsel yön testi. **Codex imagegen native skill (gpt-image-1 force)** ile 6 PNG üret, `STAGING/concepts/` altına kaydet.

**Implementation note:** Python openai client veya OPENAI_API_KEY YOK. Codex native imagegen skill kullan (S98/S99 imagegen task'larındaki gibi — örnek: `STAGING/codex_imagegen_asset_pack_sheet_v2.md`). Her prompt için ayrı imagegen çağrısı.

## Boss & Karakter
- **Boss:** The Penitent Sovereign — zincirlere bağlı taş bekçi, eğik baş, göğüsten cyan rift ışığı, kırık zincir motifleri
- **Hero 1:** Warblade — genç chibi, longsword
- **Hero 2:** Elementalist — chibi büyücü, elde parlayan orb
- **Stil:** 2D pixel art, top-down chibi, 64px karakterler

---

## Set A — Cute / Stylized

### A1 — `STAGING/concepts/boss_concept_A1_cute_overview.png`
Top-down 2D pixel art, cute chibi style boss fight room. Large open stone floor, warm beige/cream tones, simple cyan glowing cracks. Two small chibi heroes (sword warrior + mage with orb) face large stone golem boss in center. Boss: round chunky body, glowing cyan eyes, broken chains. Soft color palette, clean silhouettes. Stardew Valley / Undertale aesthetic.

### A2 — `STAGING/concepts/boss_concept_A2_cute_closeup.png`
2D pixel art cute chibi top-down, boss fight closeup. Large chained stone boss (round body, cyan glowing chest crack, broken chain arms) looming over small chibi Warblade with tiny sword. Floor: warm cream stone tiles. Props: cute small barrels, round glowing crystals, tiny skulls. Soft warm torch glow + cool cyan boss glow.

### A3 — `STAGING/concepts/boss_concept_A3_cute_action.png`
Top-down 2D pixel art cute chibi, epic boss fight. 2 chibi heroes (sword warrior + mage with orb) vs large chained stone sovereign boss. AoE ground markers as soft rounded circles. Room: round pillars, glowing cyan rune tiles. Palette: warm stone beige + cyan accent + soft orange torch. Hades-meets-Stardew tone.

---

## Set B — Open Top-Down / Minimal Wall

### B1 — `STAGING/concepts/boss_concept_B1_opentd_overview.png`
Pure top-down 2D pixel art, 90-degree overhead view, boss fight arena. Large open stone floor, granite beige with cyan cracks. Walls are just thin dark edge shadow strip — no tall wall sprites. Chibi heroes vs stone sovereign boss in open center. Floor props: rune circles, glowing crystals. Clean readable overhead view, Hades-inspired open arena.

### B2 — `STAGING/concepts/boss_concept_B2_opentd_floor.png`
Pure overhead pixel art, 2D roguelite boss room. Detailed stone floor tiles from directly above — cracked granite, cyan rune inscriptions baked in floor. Thin wall shadow border at edges only. Stone sovereign boss (top-down silhouette, round chunky shape, glowing chest) surrounded by chibi heroes. AoE concentric circles on floor. Torch shadow pools. No perspective distortion.

### B3 — `STAGING/concepts/boss_concept_B3_opentd_wallless.png`
Top-down 2D pixel art, open boss arena — boundary only by floor tile cutoff fading to black void. No wall sprites needed. Stone floor tiles center, fade to darkness at edges. Chibi Warblade (sword raised) vs large stone sovereign boss (top-down silhouette, broken chains radiating out, cyan glowing core). Broken column tops visible as circles from above. Minimal, elegant, Hades arena floor style. 32px pixel art grid.

---

## Tool
**Codex imagegen native skill (gpt-image-1 force).** 6 ayrı imagegen çağrısı — her prompt için bir PNG.
- Hiç Python script yazma
- openai library / API key kullanma
- Native imagegen skill çağrısı (S98 asset_pack_sheet_v2 task'ı bunun referansı)

Resolution önemsiz, test reference. 1024x1024 yeterli.

## Output → CODEX_DONE.md
- 6 PNG dosya listesi (path)
- Eksik varsa hangi prompt failed
