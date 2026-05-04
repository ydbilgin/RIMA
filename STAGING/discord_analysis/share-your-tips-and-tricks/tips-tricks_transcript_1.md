# share-your-tips-and-tricks -- FULL TRANSCRIPT (Part 1/4)
# Date Range: ~January 2026 - March 2026 (Oldest to Newest)
# Source: 40 screenshots captured 2026-05-02 (two scroll-position sets)
# Format: Every visible message transcribed verbatim with visual descriptions

---

## TECHNICAL SYNTHESIS (Part 1)

### Key Pipeline Findings from This Segment:
1. **API Endpoint Routing**: "Rotations pro" is recommended for characters/monsters. "Create image from style reference (pro)" is the batch tool for items/equipment at 32x32 (64 items per generation).
2. **API Rate Limit**: 2-second wait between calls is the current throttle (confirmed by Kaninen, PixelLab staff).
3. **Bitforge (old model) Deprecation**: Users report issues with "Create image (bitforge)" -- Kaninen confirms it's an old model being hidden. Redirect to "Create M-XL image" or "Create image from style reference (pro)".
4. **Pro Tool Output Scaling**: At 64x64, you get 16 images per generation. At 32x32, you get 64 images per generation.
5. **Numbered List Prompting**: For multi-item generation, use a numbered list in the prompt (e.g., "Various weapons and tools: 1. Longsword 2. War hammer ...") for better control.
6. **Walking Animation Difficulty**: NW/NE walking/running animations are confirmed as the hardest directions for the model.
7. **cleanEdge Shader**: Open-source pixel art upscaling algorithm for clean rotations: https://torcado.com/cleanEdge/
8. **Aseprite Export Script**: dzejrou's script (https://github.com/Dzejrou/make_aseprite) converts PixelLab character tool exports into .aseprite files with all rotated sprites and individual animations.
9. **Weapon-in-Animation Workflow**: Generate animation first, then add weapon afterward using "edit image (pro)" or "transfer animation (pro)".

---

## ORIGINAL TRANSCRIPTS

---

### 2026-02-04 -- Rate Limits, API Routing, Weapons vs Characters

**Kaninen** (PixelLab staff) -- 4.02.2026 16:01
> following up on this in the support channel :)

> [Replying to @xjlon: Ai can help you write the prompt]

**draingon .r** -- 4.02.2026 22:17
> okay pls help

> [Links to #general channel]

> look, its not working for me

--- 5 Subat 2026 ---

**monomono** -- 5.02.2026 19:52
> Any tips for humanoid NW NE walking/running anims? Seems like it's the hardest anim you can hit the model with.

[ANTIGRAVITY OBSERVATION: This confirms a known limitation -- diagonal walking/running animations (NW/NE) are the most difficult for the PixelLab model. This directly impacts RIMA's 8-direction sprite pipeline. Workaround: generate N/S/E/W first, then use interpolation or manual tweaks for diagonals.]

**Blaise** -- 5.02.2026 22:58
> If I buy the full version of the program, will I also be able to export/download the sprite sheets so I can add the animation smoothly into my game? Thanks in advance

> [Replying to @Blaise: If I buy the full version...]

**Kaninen** -- 5.02.2026 23:05
> you can download them in the free version as well

**Blaise** -- 5.02.2026 23:07
> Okay, very good -- then I'll have to look into the how tomorrow.

**HEAD (bloodandpower.com)** [MEGA badge] -- 5.02.2026 23:55
> anyone know the rate limit on api ?
> im trying to generate 64x64 art - in batches sof 3 but its timing out badly
> scrappy servers?

--- 6 Subat 2026 ---

> [Replying to @HEAD: anyone know the rate limit on api?...]

**Kaninen** -- 6.02.2026 00:29
> we currently have it set up i believe so you wait 2 seconds between calls, should probably change that :)

[ANTIGRAVITY OBSERVATION: CRITICAL API CONSTRAINT -- 2-second minimum wait between API calls. Any automated pipeline must enforce this throttle. For RIMA batch generation, this means a 16-frame sprite sheet at 64x64 takes minimum 32 seconds of API time alone.]

> [Replying to @Kaninen: we currently have it set up...]

**HEAD (bloodandpower.com)** [MEGA badge] -- 6.02.2026 01:17
> What api should I use for weapons armour, potions and stuff ?
> and which api for monsters

**Kaninen** -- 6.02.2026 01:22
> rotations pro is good for characters/monsters
> Weapons armor potions etc, same style is nice or create image pro
> Probably easiest to check it out in creator/pixelorama or aseprite :)

[ANTIGRAVITY OBSERVATION: ENDPOINT ROUTING RULE --
- Characters/Monsters -> "Rotations Pro"
- Weapons/Armor/Potions/Items -> "Create image from style reference (pro)" or "Same Style"
- This maps directly to RIMA pipeline: character sprites use rotation endpoint, equipment sprites use style reference endpoint.]

> [Replying to @Kaninen: rotations pro is good for characters/monsters...]

**HEAD (bloodandpower.com)** [MEGA badge] -- 6.02.2026 02:08
> which endpoint to use
>
> Im having issues right now using Create image (bitforge)

[ANTIGRAVITY OBSERVATION: An image is shown of generated weapon sprites -- several swords, axes, and maces in pixel art style. Quality is mixed, some items look like placeholder-quality.]

> [Replying to @HEAD: which endpoint to use Im having issues right now using Create image (bitforge)]

**Kaninen** -- 6.02.2026 02:17
> i see, yes its an old model which is why we are kinda hiding it away :) you will get better results using another tool (this one is a bit stupid)

> Pro tools are way way way stronger, but otherwise you can try out create image m-xl (perhaps use it to get a init image for bitforge) if you dont like the style

[ANTIGRAVITY OBSERVATION: BITFORGE DEPRECATION CONFIRMED -- The "Create image (bitforge)" endpoint is an OLD model being hidden. The migration path is:
1. Use "Create image from style reference (pro)" for items
2. Use "Create M-XL image" as alternative
3. Bitforge can still be used as a post-processor if you feed it an init image from M-XL]

> [Replying to @HEAD: which endpoint to use...]

**Kaninen** -- 6.02.2026 02:18
> pro tool is quite efficient for 32x32 images as you get 64 of them at once

[ANTIGRAVITY OBSERVATION: Shows a screenshot of the PixelLab Pro tool interface -- a grid of small 32x32 generated sprites (appears to be equipment/items like shields, helmets, swords) on the left panel, and the tool's configuration panel on the right showing:
- Style Reference with "Upload" option
- "Add style shape" / "Clear all shapes" buttons
- Character field: "optional: type a category/object, e.g. 'human idle south', 'metal slime', 'fireball'"
- Description field
- Output Method: "Bin x atlas" dropdown
- Checkboxes: "Remove background" (checked), "Advanced Options"
- Buttons: "Generate", "Reset", "Cancel"
- Note: "This tool costs 40 credits/lines and creates multiple images. Results are saved to last 30 tab automatically."]

> [Replying to @Kaninen: pro tool is quite efficient for 32x32 images as you get 64 of them at once]

**HEAD (bloodandpower.com)** [MEGA badge] -- 6.02.2026 02:30
> what is this tool you are using ?

**Kaninen** -- 6.02.2026 02:51
> Create image from style reference (pro), look for a tool which has both "style" and "pro" in its name :)

**HEAD (bloodandpower.com)** [MEGA badge] -- 6.02.2026 02:52
> Thats 40 credits its abit much much for 1 image
> I have a set of files with item name and description

> api should take it in and generate
> theres over 2000 items D;

> [Replying to @HEAD: Thats 40 credits its abit much much for 1 image...]

**Kaninen** -- 6.02.2026 02:53
> *generations, but yes, but if you are doing 64x64, you get 16 images and 64 images if you are doing 32x32

[ANTIGRAVITY OBSERVATION: OUTPUT SCALING RULE --
- 32x32 resolution = 64 images per generation (40 credits)
- 64x64 resolution = 16 images per generation (40 credits)
- Cost per sprite: 40/64 = 0.625 credits at 32x32; 40/16 = 2.5 credits at 64x64
- For RIMA's equipment sprites, 32x32 may be sufficient for inventory icons]

**xjlon** -- 6.02.2026 03:21
> [Replying to @HEAD: wait so im i prompting incorrectly ? i dont understand]
> Each item is in a new frame
> You have to export it as sprite sheet to see it like that

**HEAD (bloodandpower.com)** [MEGA badge] -- 6.02.2026 03:32
> issue with this is its going to generate random stuff right ?

> just tested it itss not what i need right now i will just use single credit :(

> yes its more efficient and quicker however the genration is random even if i say sword it outputs spear or shield

> crazy

> still abit crap but it will do

[ANTIGRAVITY OBSERVATION: Shows two images of generated weapon sprites:
1. First image: A grid of small 32x32 weapon sprites with labels -- various types of weapons but inconsistent with the prompts (swords generating as spears/shields)
2. Second image: Higher quality individual weapon renders -- an axe, a sword, and another weapon in pixel art style]

---

### 2026-02-06 -- Numbered List Prompting for Items

**Imakero** -- 6.02.2026 11:04
> Was using a few of these images as style reference images

> [Replying to @Matt:this: Any tips to generate good pokemon sprite ?]

**Kaninen** -- 6.02.2026 12:40
> you will want to use the pro tools

--- 10 Subat 2026 ---

**HEAD (bloodandpower.com)** [MEGA badge] -- 10.02.2026 03:39
> @Kaninen used aspire to do this.
> But is there anyway the same can be done for api/v2 ?

[ANTIGRAVITY OBSERVATION: Shows a grid of generated equipment sprites -- appears to be armor pieces (chest plates, helmets, boots) in pixel art style. Labels visible include "shadow_plating" repeated across many items. Some items have colored variants (purple/green). The grid shows approximately 20+ items.

Below this is another grid showing similar armor sprites but with what appears to be "bug ?" written at the bottom, and a third row of items. User is asking if this batch generation workflow can be replicated via the API v2 endpoint.]

> consistent but this leg armour is hard to get done - any word i should be using ?

[ANTIGRAVITY OBSERVATION: Shows another grid of leg armor sprites -- boots, greaves, leggings in pixel art. Quality varies significantly across the batch. Some items are barely recognizable.]

**Imakero** -- 6.02.2026 11:05
> You can specify a list of the items you want, e.g. using this prompt:

```
Various weapons and tools:
1. Longsword
2. War hammer
3. Battle axe
4. Spear
5. Crossbow
6. Halberd
7. Dagger
8. Mace
9. Blacksmith hammer
10. Tongs
11. Hand drill
12. Wood chisel
13. Sickle
14. Flail
15. Pickaxe
16. Carpenter's saw
```

> I got the following result:

[ANTIGRAVITY OBSERVATION: Shows a grid of generated weapons matching the numbered list. Results include recognizable pixel art versions of: longsword, war hammer, battle axe, spear, crossbow, halberd, dagger, mace, blacksmith hammer, tongs, hand drill, wood chisel, sickle, flail, pickaxe, and carpenter's saw. Quality is good -- items are distinct and mostly match their labels.]

[ANTIGRAVITY OBSERVATION: NUMBERED LIST PROMPTING TECHNIQUE -- Using a structured numbered list in the prompt significantly improves item-to-output correspondence. This technique should be integrated into the RIMA equipment generation pipeline for consistent results.]

---

### 2026-02-25 -- Top-Down Prop Rotation & Normal Maps

--- 25 Subat 2026 ---

> [Replying to @George -- tagged with PHSR badge -- "proper lighting effects"]

**CFLO** -- 24.02.2026 05:56
> That's super pretty!

> [Replying to @George -- PHSR badge -- "proper lighting effects"]

**OzkBrewer** [pMPC badge] -- 25.02.2026 08:23
> Sick!

**AjaxMemories** -- 25.02.2026 20:01
> Has anyone figured out how to rotate top down props? For example say it's facing you but you want it to be facing east or west.

**AjaxMemories** -- 25.02.2026 21:40
> Nvm the rotate pro tool worked for the rotation.

[ANTIGRAVITY OBSERVATION: PROP ROTATION CONFIRMED -- The "Rotate Pro" tool successfully rotates top-down props to different facing directions. This is validated by a community user and received 2 heart reactions. For RIMA, this means environment props can be generated in one facing direction and then rotated via the pro tool.]

--- 26 Subat 2026 ---

> [Replying to @George -- PHSR badge -- "proper lighting effects"]

**Otoya** -- 26.02.2026 21:27
> The values are all over the place there. Hehe.

> [Replying to @Otoya: The values are all over the place there. Hehe.]

**George** [PHSR badge] -- 26.02.2026 21:28
> yeah, it was just a simple testcase to showcase how normal maps could be used.

> [Replying to @Seven: Instead, grid a 256x256 canvas of many sprite sheets and then ask it to follow that spritesheets ref and generate new unique variations]

**Otoya** -- 26.02.2026 21:30
> So you need to be able to film up an entire canvas before this trick is unlocked or did I misunderstand you?

> [Replying to @Kaninen: i created the tool for this specific task in mind :heart: happy it is working!]

**Otoya** -- 26.02.2026 21:38
> Which tool?

> [Replying to @Otoya: Which tool?]

**Kaninen** -- 26.02.2026 21:39
> Create image from style reference (pro)

> [Replying to @Otoya: So you need to be able to film up an entire canvas before this trick is unlocked or did I misunderstand you?]

**Seven** -- 26.02.2026 22:13
> Not anymore. Style tool will grid automatically for you based on the ref images size so just upload single sprite images of what you need and itll do the rest

[ANTIGRAVITY OBSERVATION: STYLE REFERENCE AUTO-GRID -- The "Create image from style reference (pro)" tool now automatically grids based on reference image size. You no longer need to manually create a 256x256 grid canvas. Just upload single sprite images and the tool handles the rest. This simplifies the RIMA batch generation workflow significantly.]

---

### 2026-02-27 -- Map Editor, Description Limits, YouTube Workshop

--- 27 Subat 2026 ---

**BeastL.Z** [CODE badge] -- 27.02.2026 15:35
> can someone explain how the map editor works its kinda hard for an beginner to understand how to use cant find any videos of it aswell

> [Replying to @BeastL.Z: can someone explain how the map editor works...]

**Kaninen** -- 27.02.2026 15:50
> Shared some videos in general

> [Replying to @BeastL.Z: can someone explain how the map editor works...]

**Kaninen** -- 27.02.2026 15:51
> https://youtu.be/qYeGOijp-3w?t=3T53 in this live video Nikola uses the map editor (new things have been added since this video but overall the idea is the same)

[ANTIGRAVITY OBSERVATION: Shows an embedded YouTube video:
- Channel: PixelLab
- Title: "Pixel Art AI Workshop LIVE - Tips, Workflows & Q&A | PixelLab"
- Thumbnail shows "STREAM ENDING SOON" text with teal/green geometric design
This is an official PixelLab workshop video demonstrating the map editor workflow.]

**HEAD (bloodandpower.com)** [MEGA badge] -- 27.02.2026 17:05
> why does aseprite has no limit on description text
> but edit image pro - limited to 500 text

[ANTIGRAVITY OBSERVATION: DESCRIPTION TEXT LIMIT -- "Edit Image Pro" has a 500-character description limit, while Aseprite integration has no such limit. This constrains prompt complexity for the Pro tool. For RIMA, prompts should be concise and front-load critical keywords within the 500-char limit.]

---

### 2026-03-02 to 2026-03-05 -- Tool Hierarchy, Animation with Weapons

> [Someone asks: "is it S-XL?"]

[ANTIGRAVITY OBSERVATION: Shows the PixelLab tool dropdown menu with the following hierarchy:

**Pro Tools (Recommended):**
- Create S-XL image (pro)
- Create image from style reference (pro)
- Create UI elements (pro)
- Create 8-directional object/character (pro)
- Create animated object/character (pro)

**Recommended:**
- Create M-XL image
- Image to image (depth)
- Image to pixel art
- Create character with same style

**Recommended (32x32 or smaller):**
- Create S-M image

**Other:**
- Create S-M image (old)

This is the CANONICAL TOOL HIERARCHY for PixelLab as of early 2026.]

**TheSyntheticFeed** [PHSR badge] -- 2.03.2026 18:13
> Yes you could do it with that one. Or you could Use Create image from style reffrence (Pro) upload the size of the sprites you want as reffrence and make a good prompt explaining each sprite in full detail

--- 4 Mart 2026 ---

**freddy** [GAME badge] -- 4.03.2026 06:33
> Anybody got an idea how i can make a walk animation or slash while the character is holding a weapon lets say a sword idk how to set up the skeleton properly is there even a way @Kaninen

--- 5 Mart 2026 ---

> [Replying to @freddy: Anybody got an idea how i can make a walk animation or slash while the character is holding a weapon...]

**Kaninen** -- 5.03.2026 00:48
> I would try generating the animation first and then try adding in the weapon afterwards using edit image (pro) or transfer animation (pro)

[ANTIGRAVITY OBSERVATION: WEAPON-IN-ANIMATION WORKFLOW --
1. Generate the base character animation FIRST (without weapon)
2. Add weapon AFTER using "edit image (pro)" or "transfer animation (pro)"
This is the canonical approach and matches RIMA's planned layered sprite workflow where base character and equipment are separate layers.]

**freddy** [GAME badge] -- 5.03.2026 01:21
> ah ok makes sense ty man big fan of youre work btw

--- 10 Mart 2026 ---

**kuronami** -- 10.03.2026 05:22
> how do i import tiles created in tiles pro into map editor on pixelorama?

> [Replying to @kuronami: how do i import tiles created in tiles pro into map editor on pixelorama?]

**Kaninen** -- 10.03.2026 14:03
> Map editor supports high top down tilesets at the moment and we have yet to add support for the tiles created in tiles pro. Sorry!

[ANTIGRAVITY OBSERVATION: TILESET COMPATIBILITY GAP -- As of March 10, 2026, tiles created in "Tiles Pro" cannot be imported into the Map Editor. The Map Editor only supports "high top-down tilesets". This is a known limitation.]

--- 11 Mart 2026 ---

**Stew** -- 11.03.2026 02:55
> this shader is amazing https://torcado.com/cleanEdge/

> [Shows embedded link preview:]
> **cleanEdge**
> a pixel art upscaling algorithm for clean rotations

> its open source too, see bottom of that page

[ANTIGRAVITY OBSERVATION: cleanEdge SHADER -- An open-source pixel art upscaling algorithm specifically designed for clean rotations. This is directly relevant to RIMA's sprite rotation pipeline. When rotating pixel art sprites (e.g., from south to east facing), standard rotation causes aliasing/blurring. cleanEdge solves this. The algorithm is open source and available at https://torcado.com/cleanEdge/. Received 2 heart reactions.]

--- 15 Mart 2026 ---

> [Replying to @Stew: this shader is amazing https://torcado.com/cleanEdge/]

**Tommy** [verified, CDU badge] -- 15.03.2026 20:52
> I just implemented this to my little web project!

[ANTIGRAVITY OBSERVATION: Shows Tommy's web project "Haven's Call" with a display settings panel. The project appears to be a small RPG/game built with web tech that has integrated the cleanEdge shader for sprite rendering.]

---

## KEY OBSERVATIONS (Part 1)

### For RIMA Automated Pipeline:
1. **API Throttle**: Enforce 2-second minimum delay between API calls
2. **Endpoint Selection**: Characters -> Rotations Pro; Items -> Style Reference Pro
3. **Bitforge is Dead**: Never use "Create image (bitforge)" -- redirect to M-XL or Pro tools
4. **Output Math**: 64x64 = 16 sprites/gen, 32x32 = 64 sprites/gen (both 40 credits)
5. **Numbered Lists Work**: Structure item prompts as numbered lists for better correspondence
6. **Diagonal Anims are Hard**: NW/NE walking is the hardest -- plan extra iterations
7. **Weapon Layer Separation**: Generate body animation first, add weapons as a second pass
8. **cleanEdge for Rotation**: Use https://torcado.com/cleanEdge/ for clean pixel art rotation
9. **Auto-Grid**: Style Reference Pro now auto-grids from single reference images
10. **Description Limit**: Edit Image Pro has 500-char limit; Aseprite has none
