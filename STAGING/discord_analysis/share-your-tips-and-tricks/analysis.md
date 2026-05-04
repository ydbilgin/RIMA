# #share-your-tips-and-tricks Channel Index

### [2026-04-29] Run Cycle Animation Workflow & Interpolation
- Participants: Brian, Kaninen
- Type: text
- Summary: Brian shares a workflow for run cycles: feed an idle pose into "Animate with text" (prompt: "running fast, alternating arms and legs..."). Pick the best extreme pose (high knee), delete others, flip, and interpolate. Works for N/S, struggles on E/W. Kaninen suggests animating a "mid pose" first to use as a reference, and using "edit image (pro)" to create poses to interpolate from.
- Flag for User: [None]
- File: screenshots/capture_001_20260502_191824.png

### [2026-04-27] Forcing True South (Front-Facing) Generation
- Participants: Tampa, Kibyra
- Type: text + image
- Summary: Tampa struggles to generate a true "South" (front-facing) mage; 9/10 results are SW or SE. Kibyra suggests that on 256x256, adding prompt keywords like "fully visible top to bottom body" helps force the correct perspective and visibility.
- Flag for User: [None]
- File: screenshots/capture_005_20260502_191837.png

### [2026-04-22 to 2026-04-23] 100-Pose Spritesheet Tool & Tileset Blending
- Participants: Betty, Stew, YumYum
- Type: text + image
- Summary: Stew shares a custom tool (`https://lysle.net/satoshi/skeleton`) and a video showing how to generate ~100 pose spritesheets in an hour combining the PixelLab API and their tool. YumYum shares a trick for blending tilesets (e.g., grass to snow): place the target tile in the center, surround it with the other tile in Aseprite, then use "Edit Image Pro" to merge textures seamlessly.
- Flag for User: [None]
- File: screenshots/capture_010_20260502_191857.png

### Flagged Media
*   `capture_010_20260502_191857.png` / `capture_012_20260502_191905.png`: Shows the run cycle reference and someone mentioning manual interpolation between frames.
*   `capture_015_20260502_191915.png`: The "grass-to-snow" tileset blending problem, which mentions using "Edit Image Pro" for transitions.
*   `capture_016_20260502_191918.png`: A discussion about forcing "True South" and managing Z-depth overlaps, crucial for our camera angle.
*   `capture_001_20260502_192147.png`: Shows an image of a pond created with tiles and a reference to a YouTube Godot VFX tutorial.
*   `capture_005_20260502_192203.png`: Contains the URL `https://torcado.com/cleanEdge/` which could be useful for rotating our own top-down sprites without losing pixel art cleanliness.
*   `capture_010_20260502_192219.png`: Mentions the 256x256 sprite sheet grid technique for "Create image from style reference (pro)".
*   `capture_015_20260502_192235.png`: `HEAD` posted a large grid of generated weapons/armor. User asked how to maintain consistency for leg armor in API/v2. Might be a useful reference for item generation.

### [2026-04-12] 8-Direction Generation via Creator Tab
- Participants: NikolaiPatricioStar, Dyamonic, ToyotaTacoma
- Type: text + video (streamable)
- Summary: NikolaiPatricioStar shares a video (`https://streamable.com/65qzvf`) demonstrating how to do 8-direction generation directly in the website's creator tab or via Pixelorama. Dyamonic advises using creator templates or v3 animation inside the creator.
- Flag for User: [None]
- File: screenshots/capture_015_20260502_191915.png

### [2026-04-07] Armor Sets & Forcing "Forward" Animation
- Participants: CalatZ, mark3448, Kaninen
- Type: text + image
- Summary: CalatZ discusses the high cost but better results of filling out an entire sprite sheet at once with the same seed for armor sets (sword, shield, hat). mark3448 asks how to make "animate with text" actually walk forward (north); keywords like "forward" or "north upward" resulted in walking backwards. Kaninen suggests using custom (v3) / animate with text (new) and the prompt enhancer.
- Flag for User: [None]
- File: screenshots/capture_020_20260502_191932.png

### Run 2: (Approx. 2026-02-04 to 2026-04-05)
*   **Media Indexed:** `capture_001_20260502_192147.png` to `capture_020_20260502_192253.png`
*   **Topics Discussed:**
    *   **Map Building & Tilesets:** Discussing layer-based map creation vs image-based map. `quark2world` mentions that water is tricky and refers to Godot shaders/textures (linking to a VFX tutorial video).
    *   **Pixelorama Integration:** `Kaninen` states that the Map Editor currently supports high top-down tilesets, but support for tiles created in "tiles pro" is yet to be added (as of March 10).
    *   **Clean Rotations:** `Stew` shared an open-source pixel art upscaling algorithm for clean rotations: `cleanEdge` (https://torcado.com/cleanEdge/).
    *   **Top-Down Prop Rotation:** `AjaxMemories` confirmed that the "rotate pro tool" works for rotating top-down props (e.g., from south-facing to east/west).
    *   **Normal Maps & Variations:** `Seven` suggested gridding a 256x256 canvas of many sprite sheets and using it as a style reference to generate new unique variations. `Kaninen` noted that the "Create image from style reference (pro)" tool was created for this specific task.
    *   **Sprite Directions:** `monomono` mentioned that humanoid NW/NE walking/running animations are the hardest animations to hit the model with.
    *   **API Rate Limits:** `HEAD` asked about API rate limits for batch generation. `Kaninen` confirmed a current setup of a 2-second wait between calls.
