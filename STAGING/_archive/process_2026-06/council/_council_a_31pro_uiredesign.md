**DEEP DESIGN LENS: Synthesis & Decisions**

**1. Canon-Coherence & Concrete Improvements per Screen:**
*   **Death Screen:** Canon-coherent (removes clutter, enforces cyan hairline/translucency). *Canon risk:* The 85% void-mor radial blackout might border on an "opaque block" if too heavy. *Improvement:* Ensure the blackout is a soft, vignette-style gradient that leaves the frozen combat area dimly visible beneath, respecting the "combat alanı okunur kalmalı" rule.
*   **Main Menu:** Canon-coherent (quieter tagline, translucent ink-on-paper). *Canon risk:* "Bloom-pulse" could conflict with the sharp, crisp "kirli kâğıt" (dirty paper) aesthetic. *Improvement:* Rely on slow, deep parallax for atmosphere rather than bloom. 
*   **Character Select:** Canon-coherent (focuses on identity and meaning). *Improvement:* Strictly separate the typography: use `TextMuted` for the thematic flavor text, and sharp `TextPrimary` for the mechanical resource (e.g., "Rage 0-100") to establish instant hierarchy.
*   **SkillBar:** Canon-coherent (removes horizontal separators, adds class identity). *Canon risk:* The translucent backing might still read as a "box". *Improvement:* Drop the 9-slice backing entirely. Let the hex slots float directly on the floor with crisp drop-shadows.

**2. Death Screen Decisions:**
*   **Wishlist Button:** **Move to menu only.** A marketing button destroys the "Vivid Vulnerability" tone and the solemn drama of the death state.
*   **Death Line:** **Not context-aware.** Stick to the canon: "denilebilir ama asla kişisel değil" (can be spoken, but never personal). A cold, agnostic, recurring truth hits harder and feels more systemic than a gamey "The goblin killed you" line.

**3. Character Select Right Panel:**
*   **ONLY Identity + Playstyle + Resource.** Do not show the skill list. A list of 5 localized skill names is meaningless visual clutter without full tooltips. Players choose a class based on the fantasy and the mechanical hook (the resource engine). Keep the glance value pure.

**4. SkillBar Decisions:**
*   **Cooldowns:** **Radial-only.** The canon strictly dictates "sayılar minimal" (numbers minimal). Use a high-contrast radial overlay and a sharp flash-on-ready. Numeric seconds turn the UI back into a spreadsheet.
*   **Glow:** **Class-accent glow.** "Renk = anlam" (Color = meaning). Cyan is the universal system accent, but the skills are an extension of the character. Binding the glow to the class color reinforces identity at the bottom of the screen.

**5. Main Menu Decisions:**
*   **Tagline:** **"Yine geldin."** It is the ultimate quiet, systemic observation of the roguelite loop. No epic fluff.
*   **Settings Stub:** **Hide it completely.** A disabled "soon" button reminds the player they are looking at software UI. If it doesn't work, it shouldn't exist.

**6. Single Most Impactful Change (The Synthesis):**
The single most impactful change to elevate perceived quality is the **strict enforcement of "Renk = anlam" (Color = meaning) combined with the total eradication of opaque containers.** By stripping the heavy 9-slice frames from the Death Screen and SkillBar—letting text and icons float on soft vignettes or directly on the game world—and replacing generic system cyan with Class-Accent colors in the HUD and CharSelect, the UI stops feeling like a generic "video game overlay." It transforms into a diegetic, die-cut lens that honors the "UI yoktur, sadece bilgi vardır" philosophy, instantly making the game feel premium, coherent, and deeply immersive.

