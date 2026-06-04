Here is the Council's synthesis and recommended approach for the MainMenu restyle, adhering strictly to the "Vivid Vulnerability" and "No UI" canon.

### 1. Approach: Rebuild at Runtime
**Recommendation:** Rebuild the menu at runtime using (or adapting) `MainMenuScreen.cs`. 
*   **Why:** Code-driven UI is exact. Since you cannot iterate via screenshots, relying on Unity Scene positioning is risky and prone to looking slightly "off." Building the layout in code ensures pixel-perfect alignment, spacing, and styling that perfectly matches your other runtime-built screens.
*   **Action:** Strip the authored UI elements from `MainMenuCanvas` in the scene (keep only the `main_menu_bg` imagery). Have `MainMenuController` trigger the code-driven UI builder on start.

### 2. Subtitle: Hierarchy of Voices
**Recommendation:** Use both, but treat them as distinct layers of communication.
*   **Title:** `RIMA` (Massive, dominant pixel serif).
*   **Subtitle:** `THE RIFT HUNTERS` (Smaller, heavily tracked/spaced out, in the slate #3A3D42 color to sit back in the mix).
*   **The Whisper:** `"Yine geldin."` (Tiny pixel font, floating separately—perhaps just above the button list or in the bottom-left corner). It should not look like a title, but like a quiet observation from the keep itself.

### 3. Premium Layout & Touches
**Recommendation:** Left-aligned, text-only anchor. Let the background art breathe on the right.
*   **Layout:** Stack the logo, subtitle, and buttons in a clean, left-aligned column (e.g., 100px from the left edge). 
*   **3 Premium Touches (Box-free):**
    1.  **Typographic Hover:** Buttons are naked text. On hover, the text color snaps from a muted slate to the sharp cyan (`#00FFCC`), and a tiny, sharp cyan `>` character appears to the left of the word.
    2.  **Void Glow:** Add a subtle `Shadow` component to the main `RIMA` text using void-purple (or cyan with high transparency and an offset). It gives depth without needing a backing plate.
    3.  **Vignette Overlay:** Draw a full-screen, click-through Image over the background art with a radial gradient (transparent in the center, 60% black/void-purple at the edges). This naturally frames the UI text and makes it pop without drawing boxes.

### 4. Settings (AYARLAR)
**Recommendation:** Keep it, wire to a stylized stub.
*   **Action:** Do not hide it. A menu with only "Start" and "Quit" feels unfinished. Wire the button to a code-driven stub that spawns a full-screen, 80% opaque dark overlay with a single, centered line of cyan text: *"Yakında."* Click anywhere to dismiss. It maintains the premium feel without overcommitting to functionality.

### 5. Language
**Recommendation:** Keep it Turkish (**BAŞLA / AYARLAR / ÇIKIŞ**).
*   **Why:** Your tone prompt and quiet statements ("Yine geldin") are already in Turkish. Mixing English UI with Turkish thematic text breaks immersion. The Turkish words provide a stark, sharp aesthetic that fits the dark fantasy vibe perfectly. Stick to the native tongue.

