# PixelLab AI YouTube Channel Research
## Production Knowledge for RIMA (Isometric ARPG, 128px, S43)
**Research Date:** 2026-05-02
**Source:** Gemini CLI queries against https://www.youtube.com/@PixelLab_AI

---

> **CONFIDENCE NOTE:** Gemini used model knowledge + web search but hit rate limits on several calls.
> It could not scrape the YouTube channel directly. Video titles cited are reconstructed from
> Gemini's training knowledge, not live crawl results. Treat specific video URLs as unverified.
> Tool behavior descriptions (create_isometric_tile etc.) are consistent with PixelLab API docs
> and are higher-confidence. Flag: MEDIUM overall.

---

## Q1: Isometric Tile Workflow

### Floors (Organic Texture & Variety)
- **Tool: `create_isometric_tile`** with **"Thin" preset** for flat ground surfaces.
- Descriptive prompts outperform generic ones: "stone road with moss" > "stone".
- **Init Image guidance:** Upload a rough sketch to direct where organic variance appears
  (e.g., winding dirt paths, crack placement).
- Lower **Guidance Weight** slightly for more natural, less rigid results.

### Walls / Elevations
- **"Thick" preset** for natural elevation (cliffs, raised ground, ledges).
- **"Block" preset** for man-made structures and walls.
- **`create_tiles_pro`** offers advanced camera controls: adjustable view angle, camera height,
  and depth -- useful for precisely matching a specific isometric projection angle in Unity.

### Consistency Across Tiles
- Upload an existing "style tile" as Init Image or Reference for every new tile in a set.
- Lock **Outline, Shading, and Detail** settings identically across all tiles in a biome.
- Run **Reduce Colors** immediately post-generation to unify the palette and strip muddy gradients.
- Manual edge cleanup required: fix stray edge pixels so tiles snap on the isometric grid.

### Recommended Videos (Gemini-cited, unverified URLs)
- "Tutorial: Creating a pixel art isometric tiles and map with PixelLab" -- covers Thick/Thin/Block presets
- "Create Tiles Pro Tool Announcement" (Short) -- adjustable angle and depth demo
- "How To Create Isometric Animals for Games Using PixelLab" -- perspective and lighting tips

---

## Q2: create_object vs create_map_object

### `create_object`
- Generalized standalone asset generator.
- Input: text prompt only (no environmental context).
- Output: isolated sprite/item/icon.
- Best for: skill icons, inventory items, props that are NOT placed into a specific map scene.

### `create_map_object`
- Optimized for placing assets INTO an existing map.
- Accepts `background_image` and `style_images` to match the shading/palette of the target scene.
- Automatically removes background for game-ready output.
- Best for: trees, chests, barrels, environmental props that must visually blend with floor tiles.

### When to Use Each (RIMA context)
| Need | Tool |
|---|---|
| Skill icon, item icon, UI element | `create_object` |
| Enemy sprite (standalone) | `create_object` |
| Environmental prop on dungeon floor | `create_map_object` |
| Isometric tile (floor or wall) | `create_isometric_tile` or `create_tiles_pro` |

No dedicated video demo was identified by Gemini -- cited from SDK/MCP documentation only.

---

## Q3: Skill Icons / Item Icons

### Workflow: "Loot Generation" Pattern
- Generate a **base item** (simple sword, basic fireball icon).
- Use **Style Reference Pro** to generate rarity variants from the same base:
  prompt: "Create 4 rarity variants: common, rare, epic, legendary. Keep core design but increase detail and glow."
- This produces a visually consistent icon family from a single seed.

### Consistency Technique
- Feed a style reference image (matching the game's UI palette) alongside each icon prompt.
- For weapon icons held by characters, use the **Inpainting Tool** to insert the weapon onto the character sprite post-generation.

### Shorts Tip (Gemini-cited)
- "Lean Prompt" rule: 4-part prompt structure for icons:
  `[Subject], [Action/State], [Environment/Context], [Palette/Style]`
  Example: "fireball skill icon, flaming, dark dungeon UI, 64x64 pixel art, orange and deep red"

### Recommended Videos
- "This Loot Generation Trick Saves Hours" -- rarity variant workflow (title unverified)

---

## Q4: Mob / Enemy Sprites (Non-Humanoid)

### Quadruped Creatures
- Start by generating a base creature (bear, wolf, lizard).
- For animation: use **Animate with Skeleton** tool -- select the **"quadruped" template**
  (typically a 6-frame walk cycle) designed for 4-legged anatomy.
- The "Animation to Animation" feature maps an existing motion template onto a new custom monster.

### Unique Monster Attacks
- **Animate Between 2 Frames:** Define start and end frames of an attack;
  AI interpolates pixel frames in between. Useful for tail swipes, charge lunges, fire breath.
- Custom bone rigs can be created in the browser tool or via the Aseprite plugin.

### Static / Non-Animated Monsters
- For creatures that don't need skeletal animation (slimes, mimics, turrets):
  use **Object Creator** (`create_object`) for frame-by-frame consistency.

### Key Difference from Humanoid Workflow
- Humanoid workflow: `create_character` -> skeleton rig -> animate.
- Non-humanoid: `create_object` (base frames) -> quadruped skeleton rig OR manual interpolation.
- The character generator is optimized for bipedal anatomy; non-humanoids need the object pipeline.

### Recommended Videos
- "How To Create Isometric Animals for Games Using PixelLab" -- quadruped skeleton demo
- "New PixelLab Tool: Animate Between 2 Frames" -- interpolation demo

---

## Q5: generate-8-rotations-v2

### Feature Summary
- API endpoint: `api.pixellab.ai/v2/generate-8-rotations-v2`
- From a single text prompt or a single reference image, generates a character/object in all 8
  directional views simultaneously: S, SE, E, NE, N, NW, W, SW.

### Best Practices
- **Start facing South:** Best quality output when the reference/base image faces South.
- **Perspective keywords:** Use `high top-down` (35-45 deg, RPG iso angle) or
  `low top-down` (20-30 deg, more overhead) for consistent game projection.
- **Image Guidance Weight:** Higher weight = rotated views more faithful to the reference design.
  Lower weight = more creative interpretation of angles.
- **Post-generation cleanup:** Run Reduce Colors to unify palette across all 8 directions.
  Manual fix of perspective artifacts on faces/limbs before animating.

### Isometric Context
- The tool prioritizes diagonal directions (SE, SW, NE, NW) for isometric games.
- Map Workshop can also apply isometric depth/rotations to environmental assets, not just characters.

### Video
- "Tutorial: Generate rotations for your pixel art characters with PixelLab" (title cited by Gemini, URL unverified)

---

## Q6: Recent Features (Feb-May 2026)

> CONFIDENCE: LOW -- Gemini may be confabulating specific announcement dates. Treat as directional.

### February 2026
- **Style Reference Tool:** Upload a sprite to force new assets to match its palette and shading.
- **Consistent Inpainting:** Upgraded masking for modifying specific sprite regions while preserving the rest.

### March 2026
- **One-Click Skeleton Animation:** Text-to-animation via skeletal rig; prompt actions like
  "walking", "attacking" without manual keyframing.
- **8-Directional Rotations:** Confirmed as available; may have been an update/improvement to v2.
- **AI UI Component Generator (Pro):** Generates cohesive pixel-art UI elements (health bars, slots).

### April-May 2026
- **MCP (Model Context Protocol) Integration:** Generate pixel art directly within Claude Code,
  Cursor, and other AI IDEs. Confirmed active for RIMA project.
- **Game Map Expansion:** Procedural tileset expansion and background generation from text.

---

## Q7: Shorts Content

### Prompting Strategy Tips
- **"Lean Prompt" Rule:** Over-describing degrades output quality. Use the 4-part structure:
  `[Subject], [Action], [Environment], [Palette/Style]`
- **Angle Locking:** Use precise keywords to lock perspective:
  - `high top-down` = 35 deg RPG iso
  - `low top-down` = 20 deg overhead
  - `side` = standard sidescroller
- Canvas size: `64x64` or `128x128` for clean characters without artifacting.

### Workflow Tips
- **Rapid Loot Generation:** Use Style Reference Pro with a base item + rarity-tier prompt.
- **Animation-to-Animation:** Upload an existing animation + new character prompt;
  AI retargets the motion timing to the new sprite shape.

### Key Gap
- Gemini could not confirm which specific Shorts are unique vs. excerpted from long videos.
  The Shorts section appears to be primarily "highlight clips" of long-form content.

---

## TOP 5 ACTIONABLE TIPS FOR RIMA

**Use case: Isometric ARPG, 128px characters, dungeon floors/walls, skill icons, mob sprites**

### TIP 1: Use `create_tiles_pro` for Dungeon Floors and Walls
`create_tiles_pro` beats `create_isometric_tile` for batch dungeon tile production because:
- It generates multiple tiles in one prompt (list floor types together).
- It accepts up to 16 style reference images to lock palette across the whole dungeon set.
- It supports adjustable view angle -- match Unity's exact isometric projection.
Lock one "anchor tile" (your best-looking stone floor) as the style reference for all subsequent tiles.

### TIP 2: Workflow for Natural Floor Variety
Do NOT generate one floor tile repeatedly -- use varied prompts from the start:
- "cracked stone floor with moss", "worn flagstone", "dry earth with pebbles"
Each description produces a naturally distinct tile. Then run Reduce Colors on all of them
together to force palette unity. Manual edge-pixel cleanup is required before Unity import.

### TIP 3: Use `create_map_object` for Environmental Props, `create_object` for Icons
- All dungeon props (barrels, pillars, torches, traps) that sit on floor tiles: `create_map_object`
  Pass the floor tile as `background_image` so shading and lighting match automatically.
- All skill icons, item icons, UI elements: `create_object` (no background context needed).

### TIP 4: Non-Humanoid Enemy Pipeline
Do NOT use `create_character` for creatures (also forbidden by S43 rules).
Pipeline: `create_object` (base enemy frames) -> quadruped skeleton in Animate tool
-> "Animate Between 2 Frames" for special attacks.
For slimes, mimics, non-bipedal enemies: treat as animated objects, not characters.

### TIP 5: 8-Rotations for Isometric Mobs -- Start Facing South, Use High Top-Down
For any enemy sprite that needs isometric directional views:
1. Generate a clean South-facing base with `create_object`.
2. Feed into `generate-8-rotations-v2` with `high top-down` perspective keyword.
3. Set Image Guidance Weight high (0.8+) to preserve anatomy across angles.
4. Run Reduce Colors, then manual cleanup on NW/NE/SW/SE diagonal frames (highest artifact rate).
5. Animate from the cleaned 8-direction sheet.

---

## Source Confidence Summary

| Research Area | Confidence | Reason |
|---|---|---|
| Tool differences (create_object vs create_map_object, etc.) | MEDIUM-HIGH | Consistent with PixelLab API/MCP docs |
| Isometric tile presets (Thin/Thick/Block) | MEDIUM-HIGH | Documented in PixelLab official tools |
| 8-rotations-v2 feature | MEDIUM | API endpoint confirmed; best practices from model knowledge |
| Specific video titles | LOW | Gemini could not crawl YouTube live; titles are reconstructed |
| Recent feature announcements (dates) | LOW | Gemini may confabulate specific release months |
| Shorts content tips | MEDIUM | Tips are consistent with PixelLab style; specific Shorts unverified |

**GAPS:**
- No live YouTube scrape was possible (Gemini hit 429 rate limits on initial calls).
- Specific video URLs were not confirmed. All "YouTube Source" attributions should be
  verified by manually searching the channel before citing them in documentation.
- The `create_map_object` endpoint behavior was cited from SDK docs, not a video demo --
  real-world testing is recommended before relying on background_image parameter.
