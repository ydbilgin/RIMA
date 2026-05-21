# CODEX TASK — Compact Comparison Sheets (Hades-style Reward+Door+Map)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: NLM auth expired — bağlam bu task'ın içinde verilmiş, offline mode'da çalış.

---

## Hedef

User threshold konseptleri + ödül + map mark fikirlerini **yan yana** görmek istiyor — full dungeon scene değil, **compact comparison sheets**.

Önceki task (`b8emvmpf4`) bireysel showcase + ingame yapıyor. Bu task **side-by-side galleries** üretir, hızlı karar gate için.

## ÖNEMLİ — Imagegen Route

Önceki `b4n5obvio` task'ta imagegen ÇALIŞMIŞTI. Codex'in **built-in imagegen tool** kullan (NOT shell OpenAI API). Source PNG'ler `C:\Users\ydbil\.codex-profiles\<profile>\generated_images\` altında çıkar, post-process ile `STAGING/concepts/compact_sheets/` altına kopyala (chroma key remove gerekirse).

## Bağlam — RIMA

- 35° izometrik ARPG, 2D pixel art (Hades + Diablo karışımı)
- Lore: "Echo Imprint Cascade" — die, room remembers, each death writes the arena
- Visual signature: cyan rift (floor cracks, wall accents, energy)
- Karakter: chibi pixel art, dark armor, 8-dir, ~64px
- **Hades pattern reference:** Hades'te oda clear sonrası **kapı açılır, üzerinde ödül ikonu görünür** (next room'da ne var: boon/coin/key/health/etc.). Player kapıya bakarak hangi ödül peşinde olacağını seçer. Bu mekanik signature.

## Görev — 4 Compact Comparison Sheet

### Sheet 1: `01_threshold_lineup.png` (~1024×512 or 1536×512)

8 threshold konsepti yan yana, hepsi "active" state, **aynı zemin + aynı ışık** koşullarında:

| Slot | Konsept |
|---|---|
| 1 | C1 Scar Compass Ring (floor compass, broken arc + cyan needle) |
| 2 | C2 Echo Fault Loom (2 anchor + cyan threads horizontal) |
| 3 | C3 Rift Ledger Slabs (raised slabs, V-shape, cyan ink) |
| 4 | C4 Mnemonic Rib Gate (rib fans from floor, jagged) |
| 5 | A1 Echo Anchor Monolith (floating obsidian fragments + cyan column) |
| 6 | A2 Imprint Scar / Floor Rift (floor cracks open to cyan void) |
| 7 | A3 Resonance Mirror (floating cyan liquid mirror) |
| 8 | A4 Chrono-Crypta Wall Seam (suspended wall blocks frozen explosion) |

Format: 4×2 grid OR 8×1 row, each ~256×256 zone. Bottom small label per slot (C1-C4, A1-A4 + 2-word name).

### Sheet 2: `02_hades_style_reward_doors.png` (~1280×640)

**Hades pattern uygulanmış threshold**: aynı 1-2 base konsept üzerinde, ÜSTÜNDE/İÇİNDE **ödül tipi ikonu** beliriyor.

Top 2 konsept seç (Scar Compass Ring + Imprint Scar) ve her birinde **6 farklı reward indicator** göster:
- **Echo Essence** (cyan orb icon)
- **Gold/Currency** (coin stack)
- **Memory Shard** (crystal)
- **Skill Rune** (glyph)
- **Health Orb** (red sphere)
- **Boss Key** (ornate key, glowing)

Format: 2 satır × 6 sütun = 12 mini scene. Her hücrede threshold + üstünde hovering reward icon.

### Sheet 3: `03_reward_drops_gallery.png` (~1024×768)

Floor drops — Hades reward pickup objelerinin RIMA versiyonu. Her biri yerde idle/pulse state:

| Reward | Visual |
|---|---|
| Echo Essence Orb | Cyan orb, slow pulse, hover above floor |
| Memory Shard | Cyan crystal cluster, fractured |
| Gold/Coin Pile | Hades-style gold scatter, but with cyan accent |
| Skill Rune | Carved stone tablet, glyph glowing |
| Health Orb | Red glass sphere, viscous liquid |
| Map Fragment | Floating paper/parchment piece, scar marks |
| Curse Stone | Black stone, cracked, red eye |
| Boss Key | Ornate iron key, cyan rune |

Format: 4×2 grid, 8 reward types. Each ~256×256 zone. Floor stone tile zemin altında.

### Sheet 4: `04_map_progression_marks.png` (~1024×768)

Hades'te oda clear olunca map UI flash eder + bir sonraki oda revealed olur. RIMA için diegetic versiyonlar:

| Mark Idea | Visual |
|---|---|
| **Echo Thread Weave** | Boşluktan cyan thread'ler beliriyor, sonraki room silüetini örüyor (3-frame: weave start / mid / complete) |
| **Scar Path Brand** | Zemine cyan path scar yanıyor, son room'dan next room'a doğru izgi (overhead view) |
| **Fragment Assembly** | Yerde parçalı map fragments, son fragment yerine oturuyor (cyan flash) |
| **Rune Ignite Trail** | Floor rune'ları sıralı yanıyor (3 rune sequence) |
| **Memory Tapestry Reveal** | Asılı (kameraya yakın) dokuma, üzerinde next room icon belirir |
| **Compass Needle Sweep** | Floor pusula iğnesi sweep yapıyor, durduğunda next direction belli olur |

Format: 3×2 grid, 6 mark idea, each ~340×340 zone. Some can show 2-3 mini frames for animation read.

## Stil Tutarlılığı

- Pixel art, 35° iso projection where applicable (orb/coin can be top-down floor pose)
- Renk paleti: dark stone gray base + cyan rift accent (default) + reward-specific color (red health, gold, etc.)
- Floor zemin: cyan-rift stone tile (RIMA b340684f reference style)
- Transparent bg DEĞİL bu sheet'lerde — dark dungeon backdrop OK (karşılaştırma için)
- Each sheet: clean grid layout, mini labels at bottom of each slot
- No full character on these (this is concept gallery, not scene)

## Output Structure

```
STAGING/concepts/compact_sheets/
  01_threshold_lineup.png      (8 thresholds side-by-side)
  02_hades_style_reward_doors.png  (2 thresholds × 6 reward icons)
  03_reward_drops_gallery.png  (8 reward types)
  04_map_progression_marks.png (6 map mark mechanisms)
```

Toplam: **4 PNG comparison sheets**.

## Acceptance Criteria

- ✅ 4 PNG, her biri grid layout, mini-labels
- ✅ Stil tutarlı across sheets
- ✅ Sheet 1: 8 threshold visible + name labels
- ✅ Sheet 2: Hades pattern (reward icon over door) clear
- ✅ Sheet 3: 8 reward drops, isolated, recognizable
- ✅ Sheet 4: 6 map mark mechanism, multi-frame where useful

## BLOCKED if

- imagegen tool erişimi yok
- output write izni yok

## Final Report

`STAGING/CODEX_DONE_compact_sheets.md`:
- 4 PNG list + paths
- Stil tutarlılığı verdict
- Her sheet'in en güçlü ve en zayıf hücreleri
- Sheet 4'te map mark için tavsiye edilen mekanik

## Dispatch

```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_compact_comparison_sheets.md --effort high
```

Run in background. Notify when complete.
