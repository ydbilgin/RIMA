# Opus rima-design Task — Tile vs Character Angle Mismatch Solution

ACTIVE RULES: (1) think before deciding (2) honest verdict (3) cross-system architectural judgment (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## CRITICAL STRUCTURAL DIAGNOSIS

After 1200+ generations and weeks of tile R&D, user diagnosed root cause:
- **Character sprite** = 35° high top-down (Hades angled view, 10 anchor LOCKED via S86 Karar #100)
- **Tile asset** = 90° pure top-down (flat square cells, all PixelLab variants produced this way)
- **Visual incoherence STRUCTURAL** — no amount of asset gen fixes the angle mismatch

User is sleeping. Solve this architecturally for morning review.

Memory anchor: `memory/project_tile_character_angle_mismatch_s93.md` (just written).

## Mission

Architectural verdict + concrete next-step. Deep cross-system judgment, this is your domain.

## Research questions

### 1. How does Hades actually do it?

Hades has 30-35° angled camera + characters at that angle. **Are their tiles also angled?** Or flat with handpainted depth illusion?

Research:
- Read industry articles/dev interviews on Hades art pipeline
- Look at Hades floor tile examples vs character sprites
- Determine: are tile sprites perspective-baked or flat-with-overlay?

### 2. How does Stardew Valley do it?

Stardew is fully orthographic (no angle). Both character + tiles at same projection. Different from RIMA case.

### 3. How do other 35°-isometric-leaning roguelikes solve it?

- Children of Morta
- Wizard of Legend  
- Crawl
- Hyper Light Drifter
- Death's Door

For each: tile flat or angled? Character angled or flat? Match or mismatch?

### 4. RIMA branches re-evaluated

| Branch | Action | Cost | Risk |
|---|---|---|---|
| A | Regen all tiles at "low top-down" 35° angle (PixelLab `view: "low top-down"`) | ~50-100 gen, week of work | Eski tile pool çöp; new pool style consistency challenge |
| B | Regen all character anchors at 90° pure top-down | YASAK (Karar #100 LOCK, ~5000 gen, eski 10 anchor scrap) | Asla |
| C | Subtle perspective overlay on flat tiles (shadow gradient, fake depth) | Shader work | Belirsiz görsel; might still feel "flat" |
| D | Hades model = flat tiles + 3D-look walls/props perimeter | Mevcut altyapı zaten LIVE (L3 wall + L4 patch + L5 scatter) | Floor still flat, but eye drawn to depth-rich walls |
| E | NEW: Camera tilt — Unity Camera angled, render floor with z-rotation to bake perspective | Camera setup change, single line config | Untested for pixel art readability |
| F | NEW: Mixed (D + camera tilt + selective angled tiles in key areas) | Combination | Highest fidelity but complexity |

Honest assessment of each. Mechanic bank if relevant: `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK`.

## Required output structure

`STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md`:

```
# VERDICT
[Single recommended branch + 1-paragraph rationale]

# 1. Hades art pipeline reverse-engineering
[Evidence-based: are Hades tiles perspective-baked or flat?]

# 2. Comparative analysis other angled-top-down games
[Per game: solution]

# 3. RIMA branches re-evaluated (A through F)
[Per branch: cost, fidelity, risk, recommendation]

# 4. Concrete next step (morning user can execute)
[Single dispatch or action]

# 5. Locks to revoke or update
[If Branch chosen requires updating Karar #100 angle locks, list]
```

Effort: deep. Use web search aggressively for Hades + Children of Morta references. Don't propose visual asset generation (gen frozen tonight).
