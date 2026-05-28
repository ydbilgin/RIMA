# Codex Task — Image Gen: RIMA Multi-Room Compose (Large Atmospheric)

## Amaç

Codex imagegen skill (gpt-image-1) ile **3 farklı odanın yan yana / 3 panel atmospheric concept screenshot** üret. Aynı 24-sprite asset pack'ten compose edilmiş Hades-iso quality oda mockup'ları. User'a paslanır → modüler reuse görsel kanıtı + production quality target onaylama.

**NOT asset.** Visual quality + atmosphere reference.

---

## Output

`STAGING/concepts/multi_room_compose_v1.png`

After generation: `STAGING/concepts/multi_room_compose_v1_NOTES.md`.

---

## Reference materials

- `STAGING/codex_dungeon_asset_pack_proposal.md` Section 4 — 3 sample room JSON (Combat / Corridor / Boss Arena)
- `STAGING/CHATGPT_TOPDOWN/*.png` (6 mockup) — **style target**
- Image #3 ref (önceden user share, Çatlak Mezarlık combat) — **kalite hedef**
- `STAGING/asset_production_master_plan_v3.md` Section 7 — Hades-iso ~70-75° tilt lock

---

## Hedef görsel — 3 Room Concept Panel

### Boyut + format
- **YÜKSEK ÇÖZÜNÜRLÜK:** En az **3072×1024** (3:1 panoramic) veya **2048×1536** (4:3) (3 panel vertical stack)
- Layout: **3 panel side-by-side** (yan yana) OR **3 panel vertical stack** (alt alta) — daha geniş hangisi mantıklıysa
- Her panel ayrı oda mockup'ı, başlık etiketli
- Panel boundary: ince cyan/gold ayırıcı çizgi (1-2px)

### Panel 1 — Room A "Combat - Broken Slab Hall" (16×12)

**Composition:**
- Hades-iso ~70-75° tilt
- Player center-south (Warblade chibi 3-4 head tall)
- 3 enemy spread: close left, mid-right, far north
- 16:9 panel ratio

**Assets visible (from 24-sprite pack):**
- Floor: F01/F02 granite (default), F03 walkway strip vertical x=6-10, F05 rubble NE corner zone, F06 cyan rift at x=8 y=6
- Walls: W01 north edges × 3, W02 east + W02-flipX west, W03 corners (NE/NW/SE/SW), W05 collapsed_stub east side
- Props: P01 column at x=4 y=4, P02 broken column x=11 y=7 flipX, P03 banner on north wall x=7, P04 torches × 2 (left + right wall), P06 urn cluster NE, P07 rubble pile NE
- Decals: D01 moss at wall edges, D02 cracks scattered, D03 blood center, D04 dust NE

**Lighting:**
- Global dim teal-blue (#20242A, intensity 0.65)
- 2 warm torch halos (orange #FFA060, radius 4, flicker)
- 1 cool cyan rift halo (#5DEFFF, intensity 0.8, radius 4)
- Dual-tone contrast: warm corners + cool center

**Mood:** Balanced combat tension, post-incursion lived-in feel

### Panel 2 — Room B "Corridor - Hairline Rift" (8×24)

**Composition:**
- Hades-iso ~70-75° tilt
- Narrow vertical room (corridor)
- Player walking north (mid-corridor)
- 2 patrol enemies (1 at y=10 area, 1 at y=18 area)
- Long aspect ratio (rendering as compressed vertical or proper tall)

**Assets visible:**
- Floor: F03 walkway (default), F01 granite center strip x=1-7, F06 cyan rift accents at x=4 y=6 and x=4 y=17
- Walls: W02 east + west long edges (4 segments each side), W04 inner corners at south end, W04 inner corners at north end, H01 archway at north exit
- Props: P04 torches × 2 (mid corridor), P06 urn at x=2 y=10, P07 rubble pile x=5 y=19
- Decals: D02 crack at x=4 y=8, D04 dust at x=3 y=13, D05 glyph at x=4 y=17

**Lighting:**
- Global very dim (#191D23, intensity 0.5)
- 2 warm torch halos, smaller radius (3)
- 1 cool cyan rift halo at archway hint
- Tight tunnel feel, claustrophobic but with rhythmic light points

**Mood:** Tense traversal, narrow tactical space

### Panel 3 — Room C "Boss Arena - Rift Court" (24×18)

**Composition:**
- Hades-iso ~70-75° tilt
- Wide dramatic boss arena
- Boss prominent at x=12 y=14 (placeholder, large humanoid silhouette, ~3× player scale)
- Player at south entry x=12 y=4 area
- 16:9 wide panel

**Assets visible:**
- Floor: F01/F02 granite default, F05 rubble NW corner zone, F04 walkway central strip x=9-14, F06 cyan rift 4×4 center zone x=10-13 y=7-10, F06 accents at x=6 y=9 and x=18 y=9
- Walls: W01 north edges × 3 segments, W03 corners NE/SE/NW/SW, W05 collapsed_stubs at x=3 y=5 and x=20 y=5 flipX, H01 archway at south door x=12 y=17
- Props: P01 columns × 2 (x=5 y=5, x=19 y=5), P02 broken columns × 2 (x=7 y=13, x=17 y=13 flipX), P03 banners × 2 (x=8 y=16, x=16 y=16 flipX), P05 braziers × 2 (x=4 y=15, x=20 y=15), P06 urn cluster NW, P07 rubble pile NE
- Decals: D01 moss NW, D02 crack center, D03 blood at x=14 y=11, D04 dust NE, D05 glyph circle at x=12 y=9 (boss summon ring)

**Lighting:**
- Global very dark (#15191F, intensity 0.45)
- Strong cyan rift center halo (#5DEFFF, intensity 2.0, radius 8 — dominant)
- 2 warm brazier halos (#FFA060, intensity 1.2, radius 4, flicker)
- Maximum drama, focal cyan core, rim warm light

**Mood:** Boss confrontation, ritual dread, climactic encounter

### Style — overall multi-panel
- Each panel rendered at **concept art quality** — like AAA game promotional screenshot
- Pixel art aesthetic with painterly polish + atmospheric depth
- Visible pixel grain on individual sprites, but soft organic blending on lighting/decals
- Each panel **distinct atmosphere** — Combat balanced, Corridor narrow tense, Boss dramatic cyan
- Panels separated by subtle vertical divider lines (cyan or gold thin)
- Each panel has small label/title at top (one line)

### Quality target (CRITICAL)
- Match `STAGING/CHATGPT_TOPDOWN/*.png` reference quality (similar polish, atmosphere, color grading)
- NOT raw 16-pixel-sprite-zoom — render at concept-art polish like a game studio reveal
- Show **modular reuse evidence** — same wall/column/torch sprite visible in multiple panels but different placement/lighting making each room feel unique

---

## Negative directives
- NO single oversized scene (must be 3 distinct panels)
- NO different art style between panels (all 3 same Hades-iso aesthetic)
- NO modern UI elements, NO HUD, NO health bars (except subtle thin enemy HP hint OK)
- NO bright cartoon palette
- NO photorealistic
- NO pure 90° flat top-down — must be Hades-iso ~70-75°

---

## Tool

Codex imagegen skill (gpt-image-1 force). Aim for highest detail mode + largest resolution.

If first gen quality insufficient, regen up to 2 times with prompt refinement.

Save to: `STAGING/concepts/multi_room_compose_v1.png`

---

## After image gen — NOTES.md

`STAGING/concepts/multi_room_compose_v1_NOTES.md`:

### Per-panel quality grading
| Panel | Tile read | Wall height/perspective | Lighting dual-tone | Prop density | Atmosphere match | Overall |
|---|---|---|---|---|---|---|
| A Combat | PASS/TWEAK/FAIL | | | | | A/B/C/F |
| B Corridor | | | | | | |
| C Boss | | | | | | |

### Modular reuse verification
- Same sprite visible in multiple panels? List: (e.g. P04 torch in A + B, W01 north wall in A + C)
- Reuse READABLE without looking like literal copy-paste? PASS/TWEAK/FAIL

### vs ChatGPT_TOPDOWN reference
- Atmosphere match: AT/NEAR/BELOW reference quality
- Polish match: AT/NEAR/BELOW
- Hades-iso tilt accuracy: PASS/TWEAK/FAIL

### Recommendation
- Concept quality enough to greenlight PixelLab production with this 24-asset pack? YES/NO/REGEN_X
- If NO, what specific element fails

---

## Commit
```
[Codex] [S98 IMAGEGEN] Multi-room compose v1 — 3 panel atmospheric scenes

- Codex imagegen (gpt-image-1) 3-panel concept of asset pack composing
- Panel A: Combat Broken Slab Hall (16×12)
- Panel B: Corridor Hairline Rift (8×24)
- Panel C: Boss Arena Rift Court (24×18)
- Modular reuse evidence — same sprite library across all panels
- PNG: STAGING/concepts/multi_room_compose_v1.png
- NOTES: STAGING/concepts/multi_room_compose_v1_NOTES.md
```

Wall clock: ~15-20 min (3 scenes, higher detail).
