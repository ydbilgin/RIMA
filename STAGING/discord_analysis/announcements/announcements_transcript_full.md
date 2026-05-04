# announcements -- FULL TRANSCRIPT (Complete)
# Date Range: November 2025 - April 2026 (Oldest to Newest)
# Source: 20 screenshots captured 2026-05-02 (capture_001 to capture_020)
# Format: Every visible announcement transcribed verbatim with visual descriptions

---

## TECHNICAL SYNTHESIS -- VERSION HISTORY & FEATURE RELEASES

### PixelLab Version Timeline (Reverse Chronological):
| Version | Date | Key Features |
|---------|------|-------------|
| 0.4.101 | 25.04.2026 | Object Creator, Sidescroller Map Editor, Create Image S-XL (new), Rotate to 8 directions (new) |
| 0.4.92 | 16.03.2026 | Animate with Text (new), 4-16 frames up to 256x256 |
| 0.4.89 | ~Feb 2026 | Tiles Tool (Pro), Pro tool pricing changes |
| 0.4.84 | 16.01.2026 | Create UI Elements, API update (Image S-L, Consistent style, Ref to 8 directions) |
| 0.4.75 | 8.12.2025 | Inpainting (v3), API: Image S-L + Consistent style + Ref to 8 directions |
| 0.4.71 | 28.11.2025 | Create image with 8 rotations (new) |
| 0.4.69 | 23.11.2025 | Animate with Text (first release), 128x128 support |
| 0.4.xx | 08.11.2025 | Improved character model (limb fix, S/N confusion fix), Experimental quadruped |
| 0.4.xx | 10.11.2025 | Create from Reference in Character Creator (bipedal south only) |

### Critical API Endpoints (as of Version 0.4.84):
- Image S-L image tool
- Consistent style tool
- Reference to 8 directions tool
- API Docs: https://api.pixellab.ai/v2/docs

### Key Pricing Info:
- Tier 1: 2000 generations
- Tier 2: 5000 generations
- Tier 3: 10000 generations
- "Animate with Text" costs 40 generations but generates 16 or 4 frames depending on size

---

## ORIGINAL TRANSCRIPTS

---

### [Capture 001] Version 0.4.101 -- Object Creator & Sidescroller (April 25, 2026)

**Kaninen** -- 25.04.2026 17:19
> Big announcement! Version 0.4.101

> We have added two new tools, released object creator and added sidescroller support in the map editor:

> - **Create image S-XL (new)**, which allows you to generate between 32x32 to 512x512 (area), with clean pixel art
> - **Rotate to 8 directions (new)**, with much better understanding of retaining items in the right hand and also seems to keep the pose. You can rotate up to 256x256 sprites.
> - **Object creator**, allows you to easily create new objects/sprites/items, pick out the ones you like and animate them in the online editor just like character creator https://www.pixellab.ai/create-object
> - **Map editor (sidescroller)** works like the high top-down but also allows you to add a background for better immersion!

> I will be adding the new rotation option to character creator / object creator as soon as possible!

> I also made a post about it
> If you'd like to support us by giving it a like or share, it would mean a lot.
> https://x.com/PixelLabAI/status/2048044035760128510

> Hope you have lots of fun trying them all out! @everyone

[ANTIGRAVITY OBSERVATION: Shows 4 attached images:
1. TOP-LEFT: A steampunk/Victorian character with top hat, long coat, mechanical elements -- dark, atmospheric pixel art style. High detail, approximately 128x128 or larger.
2. TOP-RIGHT: A blue-robed wizard/mage character with glowing effects and a staff, dark background. Similar scale and quality.
3. BOTTOM-LEFT: A red demon/orc warrior with armor and a large weapon (possibly axe/halberd). Aggressive pose, detailed armor with gold trim.
4. BOTTOM-RIGHT: An armored knight/warrior in blue-gold armor with a large weapon, standing in a dramatic pose.

All four images demonstrate the quality leap of the S-XL model -- significantly higher fidelity than previous S-M generations. Reactions: 71 fire, 12 star, 16 heart, 8 rocket, 5 skull, 20 red heart, 4 raised hands, 5 clap, 3 gift, 3 fireworks, 4 art, 2 thumbs up, 3 heart.]

[ANTIGRAVITY OBSERVATION: CRITICAL FOR RIMA --
1. "Create image S-XL (new)" supports 32x32 to 512x512 -- RIMA can use this for high-res concept art
2. "Rotate to 8 directions (new)" RETAINS ITEMS IN RIGHT HAND and KEEPS POSE -- this was a major pain point
3. Object Creator: https://www.pixellab.ai/create-object -- new endpoint for items/props
4. Sidescroller map editor added but RIMA uses top-down]

---

### [Capture 002] Version 0.4.101 (continued -- scrolled down)

[Same announcement continued, showing the 4 character showcase images from a slightly scrolled position. Same visual content as capture_001.]

---

### [Capture 003] Worker Incident & Create from Reference Bug Fix (March 28-30, 2026)

[Shows three separate announcements:]

**Top section**: End of previous announcement image showing pixel art characters including a red demon/orc warrior with horns holding a large curved blade, and an armored knight character. These are examples from the Object Creator output.

--- 28 Mart 2026 ---

**Kaninen** -- 28.03.2026 15:23
> We currently have less workers than intended due to an incident. It will get better in the next 1-3 hours.
>
> Sorry for the inconvenience and thank you for understanding
>
> Should be better now (edited)

[Reactions: 8 image, 5 exploding head, 2 eyes]

--- 30 Mart 2026 ---

**Kaninen** -- 30.03.2026 15:52
> I accidently broke create from reference in Character Creator, looking into it now!
>
> Bug fixed, looking at some additional sizing improvements (edited)

[Reactions: 7 heart, 2 red heart, 2 eyes]

--- 10 Nisan 2026 ---

**Kaninen** -- 10.04.2026 02:07
> Hi everyone! There is a game building competition going on called Vibe Jam and we wanted to support anyone who wants to try it out!
>
> If you decide to join then @ us (PixelLabAI) in your #vibejam X/twitter post we will hook you up with a month free of tier 2.
> https://x.com/PixelLabAI/status/2042377873684562374
> @everyone

[Embedded tweet preview shows PixelLab (@PixelLabAI) post about the Vibe Jam competition]

[ANTIGRAVITY OBSERVATION:
1. INFRASTRUCTURE: PixelLab had a worker shortage incident on March 28 -- service was degraded for 1-3 hours. This means the pipeline should have retry logic.
2. BUG ALERT: "Create from reference" in Character Creator was accidentally broken on March 30 and fixed same day. Shows that features can break -- pipeline should validate outputs.
3. VIBE JAM PROMO: Free month of Tier 2 for participants -- useful for RIMA if entering game jams.]

---

### [Capture 004] Vibe Jam Details & Version 0.4.89 (March-April 2026)

**Kaninen** -- [continuing Vibe Jam post]

[Shows embedded Twitter/X post from PixelLab about Vibe Jam with game building competition details]

--- earlier in the scroll ---

[Partial view of another announcement about version updates]

---

### [Capture 005] Version 0.4.89 -- Tiles Tool & Pro Pricing Changes (circa March 2026)

**Kaninen** -- [date context from scroll position: mid-March 2026]

> New tool: **Tiles tool (pro)**, allows you to easily generate tiles from a bunch of templates. If some are missing that you would like to see please let me know and I will add them

> **Pro tool update**: Pricing changes, 20 generations for smaller sizes (<=256x256), 25 generations for mid size(<=341x341). API pricing has also been lowered

> More information about this has been added to the docs. I will fix it as soon as possible so its shown in the tools.

> Happy generating
> @everyone

[ANTIGRAVITY OBSERVATION: Shows an attached image of tileset examples -- rows of pixel art tiles including:
- Row 1: Earth/ground tiles in brown tones (dirt, rocky ground)
- Row 2: Grass/vegetation tiles in green tones (various grass patterns)
- Row 3: Stone/brick tiles in grey tones (cobblestone, brick walls)
- Row 4: Decorative/special tiles in red/warm tones (lava? blood? flowers?)

Reactions: 100 heart, 47 fire, 10 eyes, 6 rocket, 4 F, 4 R, 4 E, 4 message, 2 question, 1 thumbs up, 1 fist, 3 gift, 4 flags, 2 potato, 2 confetti, 1 badge]

[ANTIGRAVITY OBSERVATION: PRICING UPDATE --
- Pro tool: 20 generations for <=256x256 (was 40)
- Pro tool: 25 generations for mid size <=341x341
- API pricing also lowered
- This is a significant cost reduction for RIMA's pipeline!]

---

### [Capture 005 continued -- bottom] Version 0.4.92 -- Animate with Text (new)

--- 16 Mart 2026 ---

**Kaninen** -- 16.03.2026 22:05
> Version 0.4.92

> New tool: **Animate with text (new)**, allows you to animate your characters with anything from 4-16 frames and supports up to 256x256 and it costs between 1-9 generations.

> It has been added to Aseprite, Pixelorama and Character creator, check it out when you have time

> Incoming additions and updates:
> A new addition to Animate with skeleton and improvements to Tileset (Pro)

> Note: we are investigating the inference stability as well.

> I also made a post about it
> If you'd like to support us by giving it a like or share, it would mean a lot.
> https://x.com/PixelLabAI/status/2033618721634136237

> Happy generating
> @everyone

[Shows a GIF at the bottom -- three pixel art characters (small humanoids) with animation frames visible, demonstrating the "Animate with text" tool output]

[ANTIGRAVITY OBSERVATION: ANIMATE WITH TEXT (NEW) SPECS:
- 4 to 16 frames per animation
- Up to 256x256 resolution
- Cost: 1-9 generations (very cheap!)
- Available in: Aseprite, Pixelorama, Character Creator
- UPCOMING: Animate with skeleton, Tileset Pro improvements
This is the primary animation tool for RIMA's pipeline.]

---

### [Capture 006] Version 0.4.92 continued + Earlier Updates

[Shows more of the Animate with Text announcement from above, with the embedded GIF showing 3 animated pixel characters]

---

### [Capture 007] Version 0.4.84 -- Create UI Elements & Full API Update (January 16, 2026)

**Kaninen** -- 16.01.2026 22:56
> Version 0.4.84

> **Aseprite/Creator/Pixelorama** update:
> New experimental tool: **Create UI elements**
> Its the first version and will be updated again in the near future

> **API Update**
> Added all of the Pro tools in the API, you can find them here https://api.pixellab.ai/v2/docs

> @Pixel Artisan @Pixel Architect @Pixel Architect (OG)

[ANTIGRAVITY OBSERVATION: Shows two attached images:
1. TOP: A collection of UI elements -- RPG-style buttons, frames, health bars, inventory slots, dialog boxes. Multiple golden/brown wooden-framed UI panels and ornate buttons with gems/crystals. A prominent circular red gem/orb element in the center. Shield and weapon stat panels below.
2. BOTTOM: Three UI element examples -- what appears to be a directional arrow button, a treasure chest icon, and a scroll/document element.

Reactions: 37 heart, 8 exploding head, 10 fire, 6 gift, 4 trophy, 4 art, 4 gem, 2 thumbs up]

[ANTIGRAVITY OBSERVATION: MAJOR API UPDATE --
- ALL Pro tools now available in the API
- API docs: https://api.pixellab.ai/v2/docs
- This is the foundation for RIMA's automated pipeline -- all tools accessible programmatically
- "Create UI elements" is experimental but useful for RIMA's HUD/inventory UI generation]

--- 24 Ocak 2026 ---

**Kaninen** -- 24.01.2026 16:22
> We are currently experiencing some problems due to deployment, things will be stable again in an hour! Sorry for the inconvenience
>
> It's back to working again (edited)

---

### [Capture 008-009] More Version 0.4.84 Details & January Stability

[Shows continuation of the Version 0.4.84 announcement and the deployment issue fix notification]

---

### [Capture 010] Version 0.4.75 -- Inpainting v3 & More API Endpoints (December 8, 2025)

[Top section shows the end of a workshop announcement with embedded screenshot of a pixel art town/village scene in a retro-style UI window (Aseprite-like interface with grid overlay)]

--- 8 Aralik 2025 ---

**Kaninen** -- 8.12.2025 19:57
> Version 0.4.75

> New Tool: **Inpainting (v3)**

> Added new inpainting tool, its recommend to extend up/down/left/right rather than sideways like my example image
> Its currently in Aseprite, but I will be adding it to Pixelorama as soon as possible!

> **API Update**
> Added **Image S-L image tool**
> Added **Consistent style tool**
> Added **Reference to 8 directions tool**
> Link to docs: https://api.pixellab.ai/v2/docs

> I also made a post about it
> If you'd like to support us by giving it a like or share, it would mean a lot.
> https://x.com/PixelLabAI/status/1998073017998479372
> @everyone

[ANTIGRAVITY OBSERVATION: CRITICAL API ADDITIONS (v0.4.75):
1. "Image S-L image tool" -- generate images at various sizes
2. "Consistent style tool" -- maintain style across generations
3. "Reference to 8 directions tool" -- the foundation for RIMA's rotation pipeline
All three tools added to API v2 docs at https://api.pixellab.ai/v2/docs

The inpainting v3 recommendation to extend up/down/left/right (not sideways) is important for canvas extension workflows.]

---

### [Capture 012] YouTube Workshop & Version 0.4.71 (November 28, 2025)

[Top: Shows embedded YouTube video thumbnail for "Pixel Art AI Workshop LIVE - Tips, Workflows & Q&A | PixelLab" with play button]

--- 28 Kasim 2025 ---

**Kaninen** -- 28.11.2025 01:19
> Version 0.4.71

> New Tool: **Create image with 8 rotations (new)** (Name pending)

> You can now automatically rotate existing sprites, generate new sprites with 8 rotations based on the style of a reference image 8 directions, or create from a style reference + a single concept image.

> More tools are coming in the next few days -- stay tuned!

> I also made a post about it
> If you'd like to support us by giving it a like or share, it would mean a lot.
> https://x.com/PixelLabAI/status/1994168524630372718
> @everyone

[Embedded tweet preview: PixelLab (@PixelLabAI) -- "Added a new rotation tool! Allows you to rotate existing sprites..."]

[ANTIGRAVITY OBSERVATION: 8-ROTATION TOOL (v0.4.71):
Three use cases:
1. Rotate EXISTING sprites automatically
2. Generate NEW sprites with 8 rotations from a style reference
3. Create from style reference + single concept image
This is the tool RIMA uses for generating 8-directional character sprites from a single reference.]

---

### [Capture 013-014] Earlier Workshop Announcements

[Shows additional workshop promotion content and embedded YouTube video from the same PixelLab workshop series]

---

### [Capture 015] Version 0.4.69 -- Animate with Text (First Release) (November 23, 2025)

**Kaninen** -- 23.11.2025 22:51
> Version 0.4.69 **BIG UPDATE!**

> Added new **Animate with Text**!! It has been added to Creator and Aseprite! It will be ready in Pixelorama in around 1-2 hours! It supports up to 128x128 images, you can use it to generate characters, VFX, scenery and much more

> - 128x128 reference images, creates 4 frames.
> - 32x32 and 64x64 reference images creates 16 frames.

> *Don't worry I have added auto padding for this tool!*

> Updated Tier 1, Tier 2, and Tier 3 **generation limits** to 2000, 5000, and 10000 respectively (for all existing and new users).

> *Note: Animate with Text takes 40 generations but it generates either 16 or 4 frames depending on size. We will be using generated animations for training so we can create a faster, cheaper and better model in the future*

> Made a post about it. If you would like to help out and like or share it we would greatly appreciate the help!
> https://x.com/PixelLabAI/status/1992679739165753353
> @everyone

[Embedded tweet from PixelLab: "Added new Animate with Text!! It supports up to 128x128 images, you can use it to generate characters, VFX, scenery and much more"]

[ANTIGRAVITY OBSERVATION: ANIMATE WITH TEXT v1 SPECS:
- 128x128 -> 4 frames (40 generations)
- 32x32 / 64x64 -> 16 frames (40 generations)
- Auto padding included
- Available: Creator, Aseprite, later Pixelorama
- IMPORTANT NOTE: "We will be using generated animations for training" -- community-generated content feeds back into model improvement

TIER LIMITS:
- Tier 1: 2000 generations
- Tier 2: 5000 generations
- Tier 3: 10000 generations

The v0.4.92 update later improved this to 1-9 generations cost and up to 256x256]

---

### [Captures 016-020] Earliest Announcements (November 8-17, 2025)

[These are already covered in the existing announcements_transcript_4.md file, documenting:]

--- 17 Kasim 2025 ---

**Kaninen** -- UPDATE & LIVE WORKSHOP ANNOUNCEMENT
- Released "Create from Reference" in Character Creator (bipedal south only initially)
- Added "Remove background" to Aseprite and Pixelorama tools
- Added batch option to unzoom for easier downscaling
- Hosted Live Workshop on YouTube with Nikola|PatricioStar

--- 10 Kasim 2025 ---

Workshop follow-up with date/time details, showing deer and bear sprites (quadrupedal testing)

--- 08 Kasim 2025 ---

**Morrokhan** -- UPDATE & LIVE WORKSHOP ANNOUNCEMENT
- Improved character model: fewer limb mix-up issues, less S/N confusion
- Trade-off: reduced text description adherence (another training run planned)
- Experimental quadruped support added
- "Reference character image" feature actively being developed

---

## MASTER VERSION CHANGELOG

```
v0.4.101 (2026-04-25) -- Object Creator, Sidescroller, S-XL (new), 8-Dir Rotation (new)
v0.4.92  (2026-03-16) -- Animate with Text (new): 4-16 frames, up to 256x256, 1-9 gens
v0.4.89  (~2026-03)   -- Tiles Tool (Pro), Pro pricing reduced (20/25 gens)
v0.4.84  (2026-01-16) -- Create UI Elements, ALL Pro tools added to API v2
v0.4.75  (2025-12-08) -- Inpainting v3, API: Image S-L + Consistent Style + Ref to 8 Dir
v0.4.71  (2025-11-28) -- Create image with 8 rotations (new)
v0.4.69  (2025-11-23) -- Animate with Text v1 (128x128 max, 40 gens), Tier limits updated
v0.4.xx  (2025-11-08) -- Improved character model, experimental quadruped
v0.4.xx  (2025-11-10) -- Create from Reference (Character Creator, bipedal south)
```

## SERVICE INCIDENTS LOG
| Date | Issue | Resolution |
|------|-------|-----------|
| 2026-03-28 | Worker shortage (incident) | Fixed within 1-3 hours |
| 2026-03-30 | Create from Reference broken (accidental) | Fixed same day, sizing improvements added |
| 2026-01-24 | Deployment problems | Fixed within 1 hour |

## KEY LINKS
- API Docs: https://api.pixellab.ai/v2/docs
- Object Creator: https://www.pixellab.ai/create-object
- PixelLab X/Twitter: https://x.com/PixelLabAI
- YouTube Workshop: "Pixel Art AI Workshop LIVE - Tips, Workflows & Q&A"
