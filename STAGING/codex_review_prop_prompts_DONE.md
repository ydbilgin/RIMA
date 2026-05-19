# Codex Review - RIMA Prop Production Prompts

Scope: `STAGING/RIMA_MAP_PRODUCTION_SEQUENCE.md`, 12 PixelLab Create Image Pro V3 prop prompts.

Verdict summary: MODIFY overall. The prompt set is close enough to use, but it contains repeat instructions that can confuse PixelLab, several text/rune risks, and some size claims that should be treated as soft composition hints, not binding pixel specs. The tonight P0 batch is usable after small edits. Do not spend limited gens on the current text unchanged if a two minute cleanup is possible.

## Q1 - General Logic and Format Check

The format is mostly compatible with PixelLab Create Image Pro V3. The single block prompt with an inline `Negative Prompt :` section is the right pattern because PixelLab has no separate negative prompt field in this workflow. The camera, style, subject, isolation, and negative prompt order is also sane.

The weak part is the `ABSOLUTE CANVAS` line. Size is already selected in the UI dropdown, and PixelLab's UI already exposes the variant count. Repeating `64x64 pixel canvas`, `64x128 pixel canvas`, or `target final size 64x64` inside the prompt is not useful as a hard control. Worse, it can conflict with the selected UI output when the same prompt is reused for 256x256 generation and downsample. For direct native assets, replace the size mention with composition language: `transparent background, centered isolated prop, full object visible, leave clean transparent padding around silhouette`. For 256->64 prompts, say `designed to remain readable after nearest-neighbor downsample to 64x64`, not `target final size`.

The `Approximate body size 36x36 pixels centered within the 64x64 canvas` style instructions are not binding. PixelLab can understand relative scale, but exact pixel dimensions are soft hints and may be ignored. These lines are useful only as composition guidance. Convert them to proportional language: `small centered object, roughly half the canvas width`, `narrow tall object, full height but not cropped`, or `compact silhouette with clear padding`.

`Single isolated prop` is good but should be stronger. Use `1 prop only, full object visible, centered, transparent background, no extra copies`. This directly attacks the most expensive failure mode: multiple props in one generated image.

## Q2 - Risk Factors

The biggest risk is text-like language. `sigil-like engraved lines`, `rune design`, `rune engravings`, and `faded dirty cream sigil accent` can cause PixelLab to invent readable glyphs, letters, or decorative text. Negative prompts already ban text, letters, captions, and numbers, but positive prompt words like `sigil` and `rune` still invite symbol generation. Safer wording is `abstract non-letter carved marks`, `non-readable ritual grooves`, or `simple geometric carved marks with no letters`.

`Dripped wax frozen in pixel droplets` is understandable, but `frozen` can be misread as ice. Use `solid cooled wax drips in small pixel drops`. That keeps the visual without temperature ambiguity.

`Fits within the canvas with at least 4 pixels transparent padding` will be parsed as intent, not an exact measurement. It helps a little, but it is not reliable. Use `do not crop, leave visible transparent padding around the full silhouette`. Exact pixel padding should be handled during manual selection or cleanup.

`Approximate body size XxY pixels` is imaginary precision. It may steer scale slightly, but it is not a guarantee. Avoid relying on these numbers for production. For tight assets, select the best variant and crop/scale manually if needed.

The downsample prompts have a special risk: `1px outline at final resolution` is aspirational. PixelLab generates at 256x256; after nearest-neighbor downsample, outlines may become too thick, broken, or chunky depending on source thickness. Better phrasing: `bold dark outline that remains about 1 pixel after downsample`.

## Q3 - Output Size Strategy

The strategy is mostly correct, but the 256->64 technique is slightly oversold. It does not automatically make a better prop. It can produce more natural silhouettes and richer organic detail, but it costs variant choice. Direct 64x64 gives 16 variants, which is very valuable when gen budget is tight. The best practical rule: use direct 64x64 for simple readable silhouettes and use 256->64 only when organic shape, glow, fracture, or clustered detail is the main quality target.

Step 1 Wooden Crate: 64x64 standard is correct. A crate is geometric and benefits from native pixel discipline plus 16 variants. 256->64 would likely soften the box language or introduce noisy plank details.

Step 2 Stone Urn: 64x64 standard is correct. It is curved, but still a compact single object where variant choice matters more than high-res detail.

Step 3 Candle: 64x64 standard is correct. The object is small and narrow. High-res generation risks a flame/glow that collapses badly at 64.

Step 4 Debris Pile: optional. Direct 64x64 is better for tonight because 16 variants will help avoid messy or unreadable piles. 256->64 can be tested later if organic rubble shape is disappointing.

Step 5 Column Intact: 64x128 custom is correct. Native tall output preserves vertical read and avoids downsample width loss.

Step 6 Column Broken: 64x128 custom is correct. The break line and lean need vertical canvas more than high-res detail.

Step 7 Brazier: 256->64 is justified but not mandatory. Glow and ember shape can benefit from high-res, but fire may become muddy. If tonight budget is tight, generate direct 64x64 first only if this becomes a required prop. For the listed P1 workflow, 256->64 is acceptable.

Step 8 Banner Torn: 64x128 custom is correct. Tall native output and variant choice matter more than downsample.

Step 9 Stone Altar: 256->64 is optional, not mandatory. The altar is a geometric block; direct 64x64 may be cleaner. Use 256->64 only if the desired old stain and carved mark texture is more important than exact block clarity.

Step 10 Treasure Pile: 256->64 is justified. Coin/gem clusters are organic and benefit from higher-res shape search, but expect cleanup.

Step 11 Hanging Chains: 64x128 custom is correct. Chain links need tall readability; high-res downsample could destroy link spacing.

Step 12 Kneeling Statue: 256->64 is justified. The statue has compact anatomy, fracture, moss, and pose details that are hard to get natively at 64.

Must use 256->64: Treasure Pile and Kneeling Statue are the strongest candidates. Brazier is recommended if glow quality matters. Optional: Debris Pile and Stone Altar. Avoid 256->64 for Crate, Urn, Candle, Columns, Banner, and Chains.

## Q4 - Prompt Bloat Check

The prompts are longer than necessary but not unusable. PixelLab will probably attend most to the beginning and concrete subject description. Repeated global style text adds friction and may make later subject constraints weaker. The easiest cleanup is to keep the camera rule, keep style discipline, simplify canvas language, tighten the subject paragraph, and keep a focused negative prompt.

Cut or reduce:

- `ABSOLUTE CANVAS` size claims because UI controls size.
- Exact body pixel sizes.
- Repeated franchise stack in every line if style is already set once.
- Over-specific padding counts.
- `target final size` wording in 256 prompts.

Must keep:

- High top-down, 30 to 35 degree ARPG camera.
- Explicit `NOT flat front view, NOT side profile, NOT isometric`.
- Pixel art discipline: black outline, hard pixel edges, no anti-aliasing, no smooth gradients.
- Transparent background, centered, full object visible.
- `1 prop only`.
- Negative prompt text ban.

The negative prompt list is adequate but should add `logo, UI, frame, border, decorative frame, sprite sheet, grid, duplicate object, cropped object` across all prompts. For rune/sigil props, add `readable symbols, alphabetic symbols, actual letters`.

## Q5 - PASS/MODIFY/FAIL Per Step

STEP 1 Wooden Crate: MODIFY. Remove `64x64 pixel canvas` and exact `36x36 pixels`. Add `1 prop only, full object visible, no duplicate crates, no border/frame`.

STEP 2 Stone Urn: MODIFY. Replace `subtle sigil-like engraved lines` with `subtle abstract non-letter carved grooves`. Remove exact `32x48 pixels`. Add `no readable symbols` to negative prompt.

STEP 3 Candle: MODIFY. Replace `dripped wax frozen in pixel droplets` with `solid cooled wax drips in small pixel drops`. Remove exact `20x40 pixels`. Consider reducing glow language so it does not become a large aura.

STEP 4 Debris Pile: PASS with one recommended edit. It is usable. Replace canvas line with `transparent background, centered pile, full silhouette visible, clean padding`. For tonight, prefer direct 64x64 because 16 variants reduce risk.

STEP 5 Column Intact: MODIFY. Remove exact `64x128 canvas` and `32x120 pixels`; keep `tall narrow centered prop, full height visible, not cropped`. Add `no duplicated columns`.

STEP 6 Column Broken: MODIFY. Same size cleanup as Step 5. Also clarify `upper portion leaning but still one connected broken column` to avoid generating two separate columns.

STEP 7 Brazier: MODIFY. Current prompt is good but glow can expand too much. Add `small contained flame, glow close to bowl only`. Replace `1px outline at final resolution` with `bold dark outline that remains readable after downsample`.

STEP 8 Banner Torn: MODIFY. Replace `sigil or rune design` and `sigil accent` with `abstract non-letter faded emblem shape`. Add `no readable symbol, no logo`.

STEP 9 Stone Altar: MODIFY. Replace `rune engravings` with `abstract non-letter ritual grooves`. Consider direct 64x64 if clean block silhouette matters more than organic stains. Add `no readable symbols`.

STEP 10 Treasure Pile: PASS with cleanup. The high-res strategy is good. Add `no chest, no bag, no container, no duplicate piles` and keep gems limited so it does not become a noisy rainbow pile.

STEP 11 Hanging Chains: MODIFY. `few twist coils midway suggesting kinetic motion` may produce swirling rope or multiple chains. Use `slightly rotated individual links` instead. Add `one continuous chain only`.

STEP 12 Kneeling Statue: MODIFY. The negative prompt says `characters, humans` while the positive prompt says `humanoid figure`; this is not fatal, but it can create conflict. Use `stone statue only, not alive` in the positive and keep `alive creature, living human` in the negative. Avoid `full body statue` in negative because the prompt needs the full visible object; replace with `standing statue`.

No step is FAIL. All are recoverable with small prompt edits.

## Q6 - Variant Separation Recommendation

Do not write `2x2 grid 4 variation` or `16 variation` inside the prompt. PixelLab already controls that, and adding it to the text can cause grid, sprite sheet, or multi-object artifacts.

Do not use `Variation A`, `Variation B`, or separate prop descriptions in one prompt. PixelLab variants are alternate interpretations of one prompt, not a request for four different designed props. If the prompt lists variants, the model may place several variants in one image or mix traits incoherently.

The best use of `-` notation is not variant separation; it is trait grouping. Example:

```
Keep variation within these details only:
- minor chip placement
- subtle wear pattern
- small color value shifts
- slight asymmetry in silhouette
```

This tells PixelLab what may vary without asking it to draw multiple props. For tonight's P0 batch, use natural PixelLab variation and pick the best image. Add trait grouping only if a prop repeatedly generates wrong variation, like multiple crates or readable symbols.

## Q7 - Missing Critical Directives

Add `no sprite sheet, no grid, no border, no frame` to negatives. This directly addresses the common failure where the model reflects UI variant structure inside the image.

Add `full object visible, not cropped` to the positive isolation line. Padding numbers are weaker than a clear anti-crop instruction.

Add `one prop only` instead of only `single isolated prop`. Keep both if token budget allows, but `1 prop only` is clearer.

Add `no readable symbols or letters` to any prompt with marks, sigils, runes, banners, or altars.

Do not add `seamless tile`; these are props, not terrain tiles. That directive would be actively harmful because it may flatten or repeat the prop.

Add asymmetry only where it helps: debris, broken column, torn banner, treasure pile, statue. Do not add it to crate, urn, candle, intact column, brazier, or chains unless variants look too uniform.

## Master Revision List

Apply this globally:

- Replace all `ABSOLUTE CANVAS: [size] pixel canvas...` lines with `ABSOLUTE COMPOSITION: transparent background, 1 prop only, full object visible, centered, clean transparent padding around the silhouette, no border or frame.`
- For 256->64 prompts, append `designed to remain readable after nearest-neighbor downsample to 64x64`.
- Remove all exact body size pixel instructions.
- Replace `Single isolated prop, no characters, no creatures, no weapons.` with `1 prop only, single isolated full object, centered on transparent background, no characters, no creatures, no weapons, no duplicate copies.`
- Add negative terms globally: `sprite sheet, grid, border, frame, cropped object, duplicate object, logo`.
- For Step 2, Step 8, and Step 9, replace rune/sigil wording with `abstract non-letter carved marks` or `abstract faded emblem shape`.
- For Step 3, replace frozen wax wording with cooled wax wording.
- For Step 7, limit flame/glow to the bowl area.
- For Step 11, replace twist coils with slightly rotated chain links.
- For Step 12, resolve the humanoid conflict by saying `stone statue only, not alive` and removing negative wording that bans the needed full visible statue.

Production recommendation for tonight: generate Step 1-4 after the global cleanup. Use direct 64x64 for all four tonight. Do not spend limited gens on 256->64 for Debris Pile unless direct variants fail.
