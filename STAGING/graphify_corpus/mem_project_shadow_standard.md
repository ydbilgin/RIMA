---
name: RIMA Shadow Standard
description: Runtime oval shadow policy (S43). No baked shadows.
type: project
---

# STANDARD
* Type: Separate Unity sprite layer (Oval contact shadow)
* CONSTRAINT: NEVER bake shadow into character/mob sprites.

# REASONS (NO BAKE)
* Inconsistent 8-dir rotation/shimmer
* Cannot scale/fade during states (Dash, Jump, Death)
* Terrain clashing (Marble vs Stone)
* VFX overlap issues

# PIXELLAB PROMPT (MANDATORY)
* Add to description: "Transparent background. Character only - NO baked ground shadow, NO ellipse beneath feet, NO contact shadow. Shadow will be composited in the game engine as a separate runtime sprite."
* Negation: ground shadow, ellipse shadow, soft shadow under feet, contact shadow, drop shadow, baked shadow

# ANGLE QC (SIGHT ONLY)
1. Foreshortening: Feet larger than head top
2. Top Visibility: Hair crown/shoulders visible
3. Body Tilt: Slight forward lean
4. Feet Position: Near bottom of image (camera-close)

# UNITY SETUP
* Asset: Assets/Sprites/VFX/Shadow_Oval.png (32x16)
* Hierarchy: Player > ShadowAnchor (y=-0.4) > SpriteRenderer
* Layer: Ground
* Logic: ShadowController.cs (Scale/Opacity/Fade per state)
