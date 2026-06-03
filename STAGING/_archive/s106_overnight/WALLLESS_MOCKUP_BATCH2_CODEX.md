# Codex Task — Wall-less Mockup Batch 2 (V1 + Boss + Doors + BG Moods + Depth)

ACTIVE RULES: (1) think before generating (2) min words in report, image quality high (3) use $imagegen built-in tool ONLY (NOT scripts/image_gen.py — no OPENAI_API_KEY needed), (4) BLOCKED if $imagegen unavailable.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

# AMAÇ
V1 style (Hybrid Hades Elysium wall-less) RIMA için lock edildi. User şimdi bu stilin ÇEŞİTLİLİĞİNİ görmek istiyor:
- Penitent Sovereign boss arena (2 phase)
- Wall-less geçiş kapıları (floating bridge + rift portal)
- Oda mood'una göre animated bg variations (combat / boss / treasure / ritual / transition)
- DOWNWARD DEPTH (cliff drop shadow + parallax bg + floating island feel)

7 mockup üret, hepsi 1024×576 16:9, V1 style consistent.

# REFERANS
- Önceki V1 mockup: `STAGING/s106_overnight/walless_mockup_v1.png` (open arena, 4 column, cyan rune, brazier — bu kalite + style HEDEFİ)
- Önceki V4 mockup: `STAGING/s106_overnight/walless_mockup_v4.png` (floating island depth) — depth referansı

# LOCKED VISUAL DIRECTION (her image için tut)
- Perspektif: High top-down 3/4 (~70-80°), Hades + Diablo III + Children of Morta tarzı
- Style: Pixel art with painterly shading, hard pixel edges, NO anti-aliasing, NO smooth gradients
- Palette: Floor #3A3D42 weathered cobblestone, Cyan accent #00FFCC rift, Warm orange #E89020 brazier, Deep black void
- Karakter: Chibi pixel art ~2.5 heads tall, large head over short body
- Lighting: Dim ambient + 2-3 focal lights (cyan rune + warm brazier + subtle upper sky)

# 4 KARAKTER (visual identity — kullanırken hatırla)
1. **Warblade** (M, melee tank): Warm tan skin, dark hair, dark gunmetal+black plate armor, **two-handed steel greatsword**, stocky
2. **Ranger** (F, ranged): Light skin, **long blonde hair**, dark teal+slate hunter gear, **longbow always visible**, slim agile
3. **Shadowblade** (M, stealth): Tan skin, black hair, black+dark gray bodysuit with **crimson red accents**, **dual short daggers**, slim fast
4. **Elementalist** (F, magic): Fair skin, **short orange hair swept to one side**, teal+blue mage outfit, **orb/spell light in hand**, petite

# THE BOSS: Penitent Sovereign (Act 1)
- **Scale:** 3-4× player size (massive, ~256px sprite equivalent)
- **Phase 1:** Heavy hunched, bound by his own thick **chains**, kneeling posture. Tarnished armor. **Purple+cyan rift light bleeding from chest** through armor cracks.
- **Phase 2:** Chains BREAK and float around him. Standing upright. Permanent **cyan Rift Tear** at arena center (3m circular hazard).
- **Arena:** Larger floating island (40×30 cells), more cliff edges, central cyan Rift Tear, broken monument wall visible in distant bg (off-island parallax), heavy embers + lightning in void below

# WALL-LESS DOORS / TRANSITIONS (use in specified images)
- **Floating Bridge:** Stone tile bridge spans void between two floating islands, narrow ~3-tile wide, edge cliff drop on both sides
- **Rift Portal:** Standalone cyan glowing circular archway, ~2.5 tile tall, no surrounding wall, energy particles spiraling

# DEPTH-DOWNWARD techniques (CRITICAL — every image)
- Cliff edge → drop shadow band (8-12 px darken) below the floor tiles
- Parallax background → distant ruin silhouette, fog drift, dim glow in void
- Slight side-tilt perspective on 2-3 of the 7 images to emphasize floating island shape
- Drop shadows from columns/props falling slightly diagonally

# THE 7 MOCKUPS

## Mockup 1: COMBAT MOOD bg + Warblade hero shot
- Open arena, V1 style (4 broken columns, cyan rune floor, brazier corner)
- Warblade center, greatsword raised mid-swing
- BG: dark cyan cosmic void + fast cyan particles + faint runic glyphs drifting (Antigravity recipe)
- Depth: cliff edge visible on right, drop shadow visible

## Mockup 2: BOSS PHASE 1 — Penitent Sovereign reveal
- Larger floating island arena, broken monument wall in distant void parallax
- Penitent Sovereign Phase 1: hunched, chains visible, cyan/purple rift light from chest, kneeling near center
- Hero (any class, your pick) in foreground, much smaller, sword/bow drawn
- BG: ominous black abyss + rising volcanic embers + distant dark mountain silhouettes (Antigravity recipe)
- Depth: heavy cliff edges all around, lightning flash in void below

## Mockup 3: BOSS PHASE 2 — Chains broken
- Same arena as #2
- Penitent Sovereign Phase 2: chains FLOATING around him, standing upright, cyan Rift Tear hazard at arena center (3m circular)
- Hero dashing past Rift Tear (motion trail)
- BG: more intense embers + bigger lightning + cyan tear pulse glow
- Depth: cliff drops more pronounced, void looks deeper

## Mockup 4: FLOATING BRIDGE TRANSITION
- Two floating islands separated by void
- Stone bridge ~6 tiles long, ~3 tiles wide, narrow stone tile spans
- Hero (Ranger preferred — longbow at side) crossing bridge mid-step
- BG: deep multi-layered parallax chasm + distant blue mist + dark stone ruins fading away (Antigravity transition recipe)
- Depth: bridge SHOWS void below clearly, drop shadows under each tile

## Mockup 5: RIFT PORTAL — Arena exit
- Smaller arena with central cyan glowing circular archway (~2.5 tile tall, NO walls around it, standalone)
- Hero (Elementalist preferred — orange hair, orb in hand) approaching portal
- 1-2 cyan rune circles on floor leading to portal
- BG: cyan particles spiraling toward portal + dim void
- Depth: arena clearly floating, cliff edges visible, drop shadow

## Mockup 6: TREASURE / SAFE ROOM
- Smaller calm floating island
- Warm brazier on left side, large pool of warm orange light
- Altar/chest in center (use Antigravity statue/altar idea)
- Hero (Shadowblade preferred — daggers sheathed) sitting near brazier, resting moment
- BG: warm golden-brown bg + slowly floating amber dust + soft radiating central sunbeam (Antigravity treasure recipe)
- Depth: calm cliff edges, gentle drop shadow, peaceful

## Mockup 7: RITUAL ROOM — Atmospheric wide shot
- No central character (or tiny character far back)
- Floating island with concentric cyan rune patterns covering the floor
- Multiple small braziers around edges
- Ancient pillar fragments scattered
- BG: mystical deep purple void + thick scrolling fog + shimmering slow-fading ancient magic symbols (Antigravity ritual recipe)
- Depth: maximum depth — slight side-tilt to show floating island shape, deep parallax with multiple void layers, glyph particles drifting up from below

# Çıktı
1. 7 PNG: `STAGING/s106_overnight/walless_v1_batch2_M{1-7}.png`
2. Report memo: `STAGING/s106_overnight/walless_v1_batch2_REPORT.md`
   - Per image: 2 satır (visual strengths + design value)
   - Final notes: hangi 2-3 mockup en güçlü, neden

# Constraints
- $imagegen kullan, scripts/image_gen.py KULLANMA
- Tüm 7 mockup AYNI V1 style — palette/lighting/perspective tutarlı
- Her image 1024×576 16:9
- Pixel art aesthetic mümkünse, design comprehension ana hedef
- Estimated time: 20-30 min (7 image)

# Report format (kısa)
```
# WALLLESS V1 BATCH 2 - <profile> - <time>

## STATUS: DONE | PARTIAL | FAILED

## M1 Combat mood: <2 satır>
## M2 Boss Phase 1: <2 satır>
## M3 Boss Phase 2: <2 satır>
## M4 Floating Bridge: <2 satır>
## M5 Rift Portal: <2 satır>
## M6 Treasure / Safe: <2 satır>
## M7 Ritual Atmospheric: <2 satır>

## Top 3 strongest (visual verdict): M<N>, M<N>, M<N> — <reason>

## Time: <N> min
```
