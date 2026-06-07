This is an incredible aesthetic foundation. The "Shattered Keep" vibe (void-purple, cyan rift energy, warm-orange accents) paired with a *Hades/Children of Morta* top-down ARPG feel is a proven, highly marketable direction.

However, your current UI scale values (180x260px cards, 20px skill slots) and camera handling are holding the "premium" feel back. A 1080p canvas demands bold, legible, and deeply satisfying UX.

Here is the deep, opinionated UX and art-direction synthesis to elevate this to an Opus-ready state.

***

### A — Camera scroll-zoom (Pixel-Perfect)

**The Problem:** Your current script calculates a continuous fractional target zoom, slides the orthographic size there, and then violently snaps to the nearest integer pixel ratio when it hands control back to the `PixelPerfectCamera` (PPC). That "snap at rest" is what feels cheap and buggy.

**The Ranking for a Modern Pixel-Art ARPG:**
1. **(Recommended) Target-Integer Easing (A better version of your Option 2).** Instead of scrolling to a fractional zoom and snapping to an integer, **make the integer ratio the target from the start.** Scroll increments the target PPC Ratio (1x, 2x, 3x). You smoothly lerp the camera's orthographic size to perfectly match the exact size of that integer. When it settles, you re-enable the PPC. Because you mathematically landed on the exact integer size, there is **zero pop**.
2. **Fully Smooth Ortho (Option 1).** Sacrifice strict pixel-perfect rendering. Let Unity handle subpixels. *Children of Morta* does this! High-action ARPGs benefit massively from unconstrained smooth tracking. Shimmering can be mitigated with a good URP setup.
3. **Discrete Crisp Steps.** Instant hard cuts between 1x, 2x, and 3x. Too retro for what you are building.

**The Concrete Recipe (For Rank 1):**
*   **Default Framing:** 1x (320x180) is claustrophobic for an ARPG. Your **default should be 2x** (640x360). Max zoom out should be 3x (960x540).
*   **The Logic:** 
    *   Track `int _targetPpcRatio`.
    *   On mouse scroll, `_targetPpcRatio = Mathf.Clamp(_targetPpcRatio + scrollDir, 1, 3);`
    *   Calculate `_targetOrthoSize` based on `_targetPpcRatio`.
    *   Lerp `_currentOrthoSize` towards `_targetOrthoSize` (`ZoomLerpSpeed = 15f` for a snappy, premium feel).
    *   When `Mathf.Abs(_currentOrthoSize - _targetOrthoSize) < 0.001f`, set `_ppc.assetsPPU = ...` and enable it. It will lock in seamlessly.

***

### B — Reward draft + HUD redesign (UX)

**The Problem:** The cards are microscopic (~9% of a 1080p screen height), and the jitter loop is destroying the UX. The "SEÇ" button click is failing because the `HoverScale` on the root `RectTransform` moves the UI element completely out from under the mouse, instantly triggering an `OnPointerExit` which cancels the click event.

#### 1. The Ideal Reward Draft Screen
*   **Scale up massively:** On a 1920x1080 canvas, cards should be **360x540px** (exactly 2x your current size).
*   **Spacing:** `CardGap` should be **80px**, not 20px. 
*   **Hierarchy & Fonts:** 
    *   Title: 42px (Gold)
    *   Desc: 24px (Light Gray, line-height 1.2)
    *   "SEÇ" Button: 200x56px, Font 24px.
*   **The Jitter Fix (CRITICAL):** Keep your root `Card_0` GameObject at exactly 360x540. It holds the `GraphicRaycaster` and a transparent Image (alpha 0) to act as the unmoving hit-box. Move all visuals (Background, Icons, Text, Button) into a child GameObject named `Visuals`. **Apply your `HoverScale` (1.05x) ONLY to the `Visuals` transform.** The hit-box never moves; the jitter loop dies.

#### 2. Premium Hover Treatments (Cyan/Void Aesthetic)
Implement **Treatment A** combined with **Treatment B** for maximum impact:
*   **Treatment A: The Phantom Lift:** On hover, scale `Visuals` to 1.05x and tween its anchored Y position **UP by 20px**. Simultaneously, tween the non-hovered cards' CanvasGroup alpha down to 0.4 and scale down to 0.95x. This makes the chosen card feel incredibly important.
*   **Treatment B: 3D Void Tilt:** (Hearthstone/Marvel Snap style). Map the mouse's local position over the card to the `localRotation` of the `Visuals` container. A subtle pitch/yaw of +/- 8 degrees makes the card feel like a physical, premium object floating in the void.
*   **Confirm Effect:** When clicked, the selected card flashes pure white, the other two dissolve into void-purple dust (particle system), and the selected card flies to the center of the screen, shrinking into the player character.

#### 3. Skill Bar Redesign
Your current hexes (20px and 16px) are unreadable.
*   **Size:** Primary (LMB/RMB) should be **72x72px**. Secondary (Q/E/R/F) should be **56x56px**.
*   **Layout:** Bottom-center of the screen. ARPG players need their eyes near the action.
*   **Placement:** Cluster them. Place the two large 72px Primary hexes in the middle, and flank them with two 56px Secondary hexes on the left, and two on the right. Wrap a curved Health bar over the left side of this cluster, and a curved Mana/Energy bar over the right side.

***

### C — UI art production plan

**Rule of thumb:** Do not generate monolithic UI screens. Generate modular, reusable pieces. Use standard Unity uGUI for 9-slice borders, text, and simple glows.

#### Asset Manifest for Generation

| Asset | Dimensions | Aesthetic Prompt / Description | Priority |
| :--- | :--- | :--- | :--- |
| **Card Backing (Relic Slab)** | 360x540 | Dark, obsidian stone slab. A jagged void-purple fissure runs faintly through the center. Subtle cyan runic etchings at the four corners. Minimalist, premium, 2D top-down perspective. | HIGH |
| **Skill Slot Frame (Hex)** | 128x128 | Ashen/obsidian hexagon border. Slight edge wear/chipping. The inner rim has a faint warm-orange reflection. Center is totally transparent. | HIGH |
| **Tier Chip (Ribbon)** | 128x64 | A ragged, metallic bookmark/ribbon that will drape over the top-left edge of the skill card. Needs a neutral dark metallic tone so we can tint it Cyan/Gold via uGUI. | MED |
| **HUD Pedestal** | 800x200 | A dark stone plinth/altar piece intended for the bottom-center of the screen. Symmetrical, spreading outward like wings. Faint void-purple ambient occlusion in the crevices. | MED |
| **Icon: Strike (LMB)** | 128x128 | A sweeping, aggressive sword slash silhouette in stark white, backed by a burst of cyan rift energy over a void-purple background. Flat, graphic, readable at small sizes. | HIGH |
| **Icon: Dash (Space)** | 128x128 | Silhouette of a figure shattering into multiple afterimages, cyan motion trails. | HIGH |
| **Icon: Slam (AoE)** | 128x128 | A heavy gauntlet or hammer striking the ground, with warm-orange shockwaves radiating outward. | HIGH |

#### Production Pipeline Notes:
1.  **Generate the Card Backing and Hex Frame first.**
2.  Import to Unity and slice the Card Backing if necessary (though for a 360x540 fixed aspect, a raw sprite works beautifully).
3.  **Do not generate text or simple glowing boxes.** Handle the card's outer cyan "Hover Glow" using a blurred white rounded rectangle sprite in Unity, tinted cyan `#00FFCC` via the Image component, utilizing additive blending (`Universal Render Pipeline/2D/Sprite-Lit-Default` with Additive blend mode).

