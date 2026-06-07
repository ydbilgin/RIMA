I will start by exploring the workspace to see if there are any existing script files, assets, or configs related to the camera, reward draft UI, or card scaling. This will help ground my critique and recommendations in the actual project implementation.
I will quickly search the workspace to check if there are any references or files related to "RIMA", "PixelPerfectCamera", or "Reward" in case some scripts or logs from previous dispatches are here.
### A — Camera Scroll-Zoom (Pixel-Perfect)

#### 1. The Cheapest Fix That FEELS Good
**Add discrete crisp steps with a short tween.**
*   **Why:** Disabling the pixel-perfect camera mid-zoom causes a jarring "blur/shimmer-then-snap" visual pop. Continuous smooth zoom breaks the pixel grid, causing ugly pixel stretching and flickering during movement.
*   **How:** Keep the `PixelPerfectCamera` component enabled. Change the camera's orthographic size in exact integer scale intervals (e.g., Zoom 1x, 2x, 3x) and tween between them using a fast easing function (e.g., `EaseOutQuad` over **0.15s to 0.2s**). 

#### 2. Default/Min/Max Zoom for an ARPG (PPU 64, Ref 640×360)
*   **Default:** `2x` zoom (effective viewport: `320×180`). Keeps the combat focused, chunky, and readable.
*   **Min (Zoomed Out):** `1x` zoom (effective viewport: `640×360`). Excellent for boss fights, telegraphing wide attacks, or large environments.
*   **Max (Zoomed In):** `3x` zoom (effective viewport: `213×120`). Only for narrative close-ups or character inventory screens.

#### 3. Over-Engineering Critique (DO NOT DO)
*   **Do not:** Write a script that dynamically disables `PixelPerfectCamera`, manipulates camera FOV/orthoSize freely, and tries to re-enable it on settle.
*   **Do not:** Implement sub-pixel camera stabilization logic or custom interpolation matrices. 

---

### B — Reward Draft + Hover + Seç (uGUI)

#### 1. Minimal Fixes
*   **Card Size & Fonts:** Scale cards to **260×380 px** (fits comfortably in a 1920×1080 canvas). Increase body text fonts to **16–20 pt** and header fonts to **24 pt**. Use `HorizontalLayoutGroup` on the card container with `Child Force Expand` disabled to center them natively.
*   **The ONE Correct Fix for Hover Jitter:** 
    *   **The Cause:** The Event Trigger is on the card root. When the card scales up, the cursor target boundaries expand. If the cursor is near the edge, it triggers a feedback loop (`PointerEnter` -> Scale Up -> Cursor exits new boundary -> `PointerExit` -> Scale Down -> Loop).
    *   **The Fix:** Separate **Raycast Hitbox** from **Visuals**. Place the Event Handler/Button components on a static parent GameObject (e.g. `Card_Hitbox`). Nest the artwork and text inside a child GameObject (e.g. `Card_Visuals`). Scale only the child `Card_Visuals` on hover. The hitbox size remains constant.
*   **Fix "Seç":** 
    *   **The Cause:** The selection logic is suspended because `Time.timeScale = 0` during selection, and the confirmation coroutine is waiting on a standard `yield return new WaitForSeconds()`.
    *   **The Fix:** Change the yield statement in the card selection coroutine to `yield return new WaitForSecondsRealtime()`.
*   **Skill Bar:** Resize the slots to **48×48 px** or **64×64 px**. 16/20px is completely unclickable on modern high-DPI displays.

#### 2. Over-Engineering Critique (DO NOT DO)
*   **Do not:** Code 3D rotation animations, card-flip physics, or build a complex UI particle spawning system for the draft selection. Keep it to a fast 2D scale/fade tween.

---

### C — UI Art Production (Image-Gen)

#### 1. What to Build in uGUI (9-Slice/Color Blocks) — DO NOT Generate
*   **HUD Panel & Skill-bar Slot Frame:** Can be built using a single generic **24×24 px** 9-sliced border sprite with a solid-color backdrop.
*   **Rarity Chips:** Use standard uGUI Text/TMP components with an Outline, backed by a simple 9-sliced rounded-corner image colored dynamically via script.
*   **Card Backing:** A flat-color gradient with a 9-sliced frame is faster to iterate and cleaner than generating bloated card art templates.

#### 2. What to Genuinely Generate (The Smallest Asset Set)
To make the UI feel premium, focus image generation *exclusively* on assets that represent gameplay actions:
*   **Skill Icons:** Distinct 32×32 px (upscaled via Point filtering to 64×64 px) pixel-art icons for actions (e.g., Slash, Fireball, Shield).
*   **Card Illustration Windows:** Central square illustrations (e.g., 128×128 px) depicting the upgrade or buff, placed inside the uGUI-rendered card frame.

#### 3. Asset Format
*   **Pack:** Generate them as a cohesive **spritesheet / grid** (e.g., 4x4 layout of icons in one prompt) to ensure color palette harmony, then slice them in Unity. This prevents style drift between generation passes.

