# Codex Task — PixelLab Discord Insights Knowledge Base

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Bugün PixelLab Discord'undan ve test'lerden gelen tüm önemli bulguları sistematik bir knowledge base'e dökümante et. Bu doc ileride session'lar arası drift önler + Phase 2-6 production'da hızlı referans olur.

## Output

`STAGING/_research/PIXELLAB_WORKFLOW_INSIGHTS.md`

## İçerik

Aşağıdaki bulgular session'da geldi, bunları kategorize + organize et. Eklemeler/iyileştirmeler yapabilirsin.

### Tool-by-Tool Analysis

**1. Create Image Pro (original)**
- Big canvas (up to 688×384 etc., non-square destek)
- Edit/transform existing image with text instruction
- Asset pack production (multi-tile sheet)
- Use case: Master sheets, tilesets, re-theming

**2. Edit Image (Pro)**
- Mevcut image transform et (örn. grass tileset → lava tileset)
- "Change the X to Y" prompt format
- Whole-sheet re-theme tek call'da
- Tileset adherence freedom slider 925 = smooth tile connections
- Use case: Multi-Act re-theme, damage states, palette shifts

**3. Same Style (Pro)**
- Reference asset → new asset in matching style
- Use case: Variant generation, style anchor

**4. Create Image S-XL (new)** ⭐
- "New model, outline + detail controls"
- Standard sizes: 32/64/128/256/512/768 + non-square presets
- Init Image: LOCKS output dimensions to init size
- Init Image Strength slider (0-1000, 300 = balanced refine)
- Detail control: Low/Medium/Highly detailed
- Outline control: Single color / Selective / Lineless
- Direction param (None for non-character)
- View param (High top-down / Low top-down / Side)
- Blank background checkbox
- Use case: Individual asset generation with controlled style + dimensions

**5. create_object MCP**
- "Unknown tool" hatası alındı bu session — kullanılamadı
- create_object_state (variant chain) MCP'de var
- size: SQUARE only, 32-256, max 256
- n_frames: 1/4/16/64 candidates

**6. create_map_object MCP**
- W × H non-square destekli (32-400)
- BUT: pratikte SQUARE'a clamp ediyor (test: 96×160 → 96×96)
- Single-object centered fill
- detail/outline/shading params
- Use case: Standalone single sprite (props, items)

**7. create_topdown_tileset / create_tiles_pro**
- Wang16 floor terrain transitions
- Multi-variant tile set tek call'da
- Tileset adherence param
- Higher elevation / Lower elevation / Transition descriptions
- Use case: Floor tilemap (NOT walls)

**8. animate_object / animate_character**
- Static sprite → animation frames
- First + end keyframe interpolate
- Use case: Torch flame, brazier fire, rift pulse, banner sway

### Workflow Patterns

**Pattern A — Init-Locked Generation (Create Image S-XL new)**
1. Codex Python ile init image üret (exact target dimensions)
2. Web UI'ya yükle, init image olarak set
3. Width/Height standard preset seç
4. Description: verbose, AI-enhanced style prompts > terse
5. Init Strength 300: balanced refine
6. Generate → output at locked dimensions

**Pattern B — Asset Pack Master Sheet**
1. Big canvas (256×256, 512×512, or non-square)
2. Init image: grid of multiple tile outlines/silhouettes
3. Edit Image Pro: "Transform this sheet into [theme] tileset, each cell becomes [type]"
4. Slice resulting sheet into individual tiles (Python PIL or Unity sprite editor)
5. Cost: 1 call vs N individual calls

**Pattern C — Multi-Act Re-Theme**
1. Act 1 base sheet ready
2. Edit Image Pro: "Change tilesets to bone+rust theme for Act 2"
3. Repeat for Act 3 (void+gold), Act 4 (mirror)
4. 1 sheet × 3 themes = 4 Acts coverage at low gen cost

**Pattern D — 3D Box Reference Method**
1. Codex Python: iso 3D box outline PNG at target dimensions
2. Web UI Edit Image Pro / Create Image S-XL: box → wall transform
3. AI uses box as shape constraint
4. Risk: wireframe leaks into output (mitigate: filled silhouette OR stronger negative prompt)

**Pattern E — Iterative Reference Build**
1. First asset PASS
2. Use as reference for NEXT asset (Same Style Pro)
3. Style consistency chained
4. Risk: late-asset drift if first has flaws

**Pattern F — Overlay Decoration (Hades pattern)**
1. Base sprites pure (no decorative elements baked-in)
2. Decoration as separate overlay sprites (scatter, glow, banner, chain)
3. Unity: child SpriteRenderer on base
4. Runtime flexibility: any base + any overlay = unique look

### Sizing Standards

| PixelLab tier | Items/batch | Standard sizes |
|---|---|---|
| 32-40 px | 64 (max n_frames) | Tiny decals, pickups, runes |
| 48-80 px | 16 (max n_frames) | Small props, decorations |
| 88-168 px | 4 (max n_frames) | Walls, large props, features |

**Create Image S-XL (new) preset sizes:** 32, 64, 128, 256, 512, 768
**Non-square presets:** 344×192, 384×216, 512×288, 632×424, 424×632, 688×384

**RIMA standardı (LOCK):**
- Tile (modular wall block): 32×64
- Wall low (inner divider): 96×96 or 128×128
- Wall tall (perimeter): 128×256
- Wall feature (archway): 256×256
- Wall narrow (endcap/column): 128×256 (with horizontal margin)
- Floor tile: 64×64

### Known Limitations / FAIL Patterns

**FAIL 1: create_object grid prompt**
- "2x2 grid of 4 different walls" prompt → AI tek obje üretir
- create_object SINGLE OBJECT için tasarlanmış
- Use Edit Image Pro instead for multi-item

**FAIL 2: Dimension clamp in create_map_object**
- W=96 H=160 → output 96×96 (clamped to smaller dim)
- Use Web UI Create Image S-XL (new) for non-square

**FAIL 3: Wireframe leak in Edit Image Pro**
- 3D box outline → output preserves wireframe lines
- Mitigate: filled silhouette OR explicit negative prompt OR manual cleanup

**FAIL 4: Pixel art density mismatch in expanded canvas**
- Init image at smaller size + expand canvas → AI may produce smooth content in expansion
- Mitigate: Init Strength 300+, explicit "sharp pixel art, no anti-aliasing" prompt, init image at exact target size (pad to fit)

**FAIL 5: Style drift across multiple independent calls**
- 4 separate gens → 4 slightly different styles
- Mitigate: Same Style Pro reference chain OR master sheet single call

**FAIL 6: Text-to-tileset weak performance**
- "Generate grass-bush transition tileset" → unusable output
- Model needs proper tile examples in training data
- Use Edit Image Pro with existing tileset reference instead

**FAIL 7: AI-enhanced prompt changes lore-specific details**
- "Refine and expand cyan rift wall" → AI-enhanced version replaced cyan with moss
- Mitigate: keep proprietary descriptions out of AI-enhance, manually iterate

### Prompt Engineering Best Practices

1. **No proprietary names** (RIMA, Shattered Keep) — AI doesn't know
2. **Use familiar style anchors** (Hades, Diablo, Souls, Stardew)
3. **Verbose > Terse** — AI-enhanced verbose prompts give richer output
4. **Pixel art explicit guarantee** at end of prompt:
   ```
   The output must be true pixel art with sharp 1:1 pixel boundaries throughout
   the entire canvas. Every pixel must be a solid color block aligned to the
   same pixel grid as the init image. NO anti-aliasing, NO smooth gradients,
   NO blurry edges.
   ```
5. **Negative prompts** at end:
   - blurry, anti-aliased, modern brick, photo-realistic, white background
6. **Palette specification** with hex codes
7. **Single concept per call** — don't ask AI to do multiple things

### Production Cost Estimates

| Production Type | Cost (gen) | Items |
|---|---|---|
| Single asset (create_map_object) | ~30 | 1 |
| Edit Image Pro re-theme | ~30 | Whole sheet re-themed |
| Same Style Pro variant | ~30 | 1 variant |
| Asset pack master sheet | ~30-50 | 16-30 small tiles |
| State variant chain | ~15 each | N states per base |
| Animation (animate_object) | ~20-30 | 4-8 frames |

**RIMA Phase 1 budget (Act 1 walls + decor):**
- Naive per-piece: 24 walls × 30 = 720 gen
- Asset pack master sheet: 1-2 sheets × 50 = 100 gen
- Multi-Act re-theme (Act 2/3/4): 3 × 30 = 90 gen
- Total Phase 1 + Multi-Act: ~300 gen for all 4 Acts walls

### Future Research Questions

- [ ] Same Style Pro vs Edit Image Pro: which is better for variant generation?
- [ ] Init Image Strength sweet spot for wall expansion (200, 300, 400 tested?)
- [ ] animate_object: keyframe quality + interpolation fidelity test
- [ ] create_tiles_pro for wall tilesets (officially floor-only, but...)
- [ ] Pixelorama integration: better slider control via desktop app
- [ ] Best master sheet layout for 16-tile modular wall pack

## Effort

medium — Mevcut session bulgular zaten dağınık halde, organize + structure et.

## Output Confirmation

Doc path + section count + key insights count.
