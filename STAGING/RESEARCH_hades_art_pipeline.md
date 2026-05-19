# Hades + Children of Morta + Death's Door Art Pipeline Research (S93 night)

**Purpose:** Supplement Opus tile angle architecture verdict. Web research findings on how angled top-down pixel art games achieve coherent floor/character look.

## Hades (Supergiant Games)

**Key technique:** Hybrid 2D + 3D pipeline.
- Characters: Start as 3D base meshes in ZBrush, sculpted to portrait reference, rendered to 2D
- 3D artist Paige Carter uses ZBrush sculpting with 2D portrait behind file
- 2 base 3D models, molded into all NPC characters
- Final pipeline: pen-and-ink style 2D output (Mike Mignola inspiration)

**Implication for tile angle:** Hades floors are PAINTED 2D (Jen Zee art direction), not perspective-baked. Coherence achieved via:
- Strong shadows (cast from characters/props)
- HD dynamic lighting on top of 2D
- Tile sits flat but lighting + props sell depth

**Source:**
- [Game Developer: Hades 2D-to-3D character pipeline](https://www.gamedeveloper.com/art/learn-how-supergiant-brought-i-hades-i-hand-painted-characters-to-life)
- [MCV/DEVELOP: Behind the art of Hades](https://mcvuk.com/business-news/behind-the-art-of-hades-we-value-artistic-integrity-and-excellence-in-artistic-craft-at-supergiant-however-were-first-and-foremost-a-game-design-lead-team/)

## Children of Morta (Dead Mage)

**Key technique:** "Stretched HD pixel art"
- Base pixel art created at low res
- Stretched ~4x to fit full HD screen
- HD dynamic lighting applied on top
- High-res dynamic lights instead of pixelated lights — "this made the difference"

**Implication for RIMA:** Lighting + dynamic shadows do as much heavy lifting as tile angle. Even with pure top-down flat tiles, if dynamic HD lighting + character cast shadows are present, depth illusion appears.

**Source:**
- [Game Developer: Children of Morta Postmortem](https://www.gamedeveloper.com/design/postmortem-children-of-morta)
- [TechRaptor: How Dead Mage Created the Art](https://techraptor.net/gaming/previews/how-dead-mage-created-art-for-children-of-morta)
- [Engadget: Power of modern pixel art](https://www.engadget.com/2018-03-22-children-of-morta-hands-on-indie-xbox-gdc.html)

## Isometric pixel art fundamentals (technical)

If RIMA were to fully convert to isometric (Branch A in tile angle architecture):
- 2:1 ratio (2px horizontal per 1px vertical) is fundamental
- Common tile sizes: 32x16, 64x32
- Camera config in 3D: swing 45° one side + 30° down — produces diamond grid
- Light convention: top-left source, top surface lightest, left medium, right darkest
- Edge pixels must match exactly between adjacent tiles

**Source:**
- [Pixnote: Isometric Pixel Art Guide](https://pixnote.net/en/learn/isometric/)
- [Slynyrd: Pixelblog 41 - Isometric Pixel Art](https://www.slynyrd.com/blog/2022/11/28/pixelblog-41-isometric-pixel-art)
- [Sprite-AI: Isometric pixel art for games](https://www.sprite-ai.art/guides/isometric-pixel-art)
- [Clint Bellanger: Isometric Tiles Introduction](https://clintbellanger.net/articles/isometric_intro/)

## Implications for RIMA tile angle verdict

Three honest paths emerge:

1. **Hades model (= Branch D)**: Flat 2D tiles + heavy LIGHTING + 3D-look props/walls cast shadows
   - Tile angle doesn't matter; lighting does the work
   - Requires Unity 2D Lights + shadow caster setup (already in RIMA stack)
   - Maintain existing 35° character angle

2. **Children of Morta model (= variant Branch D)**: Pixel art base + stretched render + HD dynamic lighting
   - Render scale up for crisp light layer
   - Pixel Perfect Camera already in RIMA stack

3. **Full isometric (= Branch A)**: Convert to 2:1 ratio, all assets regen at isometric angle
   - Requires character regen (LOCK violation)
   - Massive scope

**Most likely Opus verdict alignment:** Branch D (Hades model) with Children of Morta lighting layer. Existing infrastructure supports this.

Sources:
- [Hades 2D-to-3D pipeline](https://www.gamedeveloper.com/art/learn-how-supergiant-brought-i-hades-i-hand-painted-characters-to-life)
- [Children of Morta Postmortem](https://www.gamedeveloper.com/design/postmortem-children-of-morta)
- [Isometric pixel art guide](https://pixnote.net/en/learn/isometric/)
