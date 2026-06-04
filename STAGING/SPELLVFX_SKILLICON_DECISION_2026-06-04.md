# DECISION — Elementalist Spell VFX + Skill Icon Production
Date: 2026-06-04 · Council: cx (feasibility, yasinderyabilgin, NLM-canon) + ax-3.1-Pro (art/tech) + ax-3.5-Flash (lean) → Opus
For: 2-character gameplay demo (Elementalist + Warblade)

## ONE LINE
Spell VFX: Unity code/particle FIRST (demo), graduate to grayscale-sprite + URP-2D-shader hybrid, PixelLab LAST (gated). Skill icons: 64px (NOT 32), PixelLab is canonical for production, but DEMO REUSES the icons that already exist.

---

## A) ELEMENTALIST SPELL VFX

### Current code reality (cx, grounded)
- `Elementalist_SkillController.cs`: 4 slots, default Fireball/GlacialSpike/ChainLightning/Blink.
- **Fireball** (`Fireball.cs`): already spawns a runtime projectile (`PlayerProjectile` + procedural circle, orange tint). Works, just flat.
- **GlacialSpike** (`GlacialSpike.cs`): BoxCast line, damage/status — **NO visual yet** (clean insertion point).
- **Meteor** (`Meteor.cs`): channel→OverlapCircle AoE — **NO visual yet** (insertion point before/after wait).
- Scaffolding to REUSE: `PlayerProjectile.cs`, `SkillRuntime.SpawnProjectile/SpawnCircleVisual`, `ProjectileFanSpawner`, `SlashArcVFX.cs` (LineRenderer arc precedent), `Combat/Juice/` (hitstop/shake), LightPulse.

### Production phases (all 3 advisors agree)
- **Phase 1 — DEMO (now, zero gen):** pure Unity code/particle VFX.
  - Fireball: keep PlayerProjectile path + cast-flash at hand + colored core + short trail + hit pulse + 2D light.
  - GlacialSpike: LineRenderer icy line + shard burst along the existing BoxCast line.
  - Meteor: channel tell + falling streak + impact ring/crack (SpawnCircleVisual/LineRenderer/particles).
  - Reuse Combat/Juice hitstop+shake on impact.
- **Phase 2 — polish:** "White Core + Shader Tint" hybrid (3.1 Pro's best path): a few **grayscale** pixel shapes (circle/diamond/crescent/puff) → Unity ParticleSystem → **URP 2D Lit pixel-particle shader** mapping grayscale→gradient per spell + 2D Point Light. All 3 spells from the SAME grayscale set via gradient + params. Lightweight `SpriteFlipbookVFX` driver for projectile core/impact (NOT the character Animator — too heavy).
- **Phase 3 — production (GATED):** PixelLab animated sprite-sheets for final projectile/impact art, WITH user approval.

### Spell palette (3.1 Pro — NOT generic D&D colors; harmonize with Rift-cyan)
- Fireball: coral/magenta core `#FF3366` → hot-orange `#FF9933`, sparks cool to cyan `#00FFCC`.
- Glacial Spike: frost-white `#F2FFFF` → mint `#80FFEA` → deep-cyan `#00CCCC`.
- Meteor: void-rock `#0B001A` body, magenta/orange fire wreath, cyan scorch decal on ground.

### Pixel cohesion tech rules (3.1 Pro)
PPU 64, Point filter, no compression. NO soft particles → alpha-cutout shader; trails shrink/step-alpha (no smooth fade). Particle sizes = integer 1/64 multiples. 1px void-purple `#1A0033` outline on sprite cores. Projectiles: NO 8-dir art — rotate GameObject under Pixel Perfect Camera (or 4-dir+flip for spike).

### Demo recommendation: Phase 1 only. RIMA already has enough scaffolding to make Elementalist playable WITHOUT any asset generation.

---

## B) SKILL ICONS (answers user's questions)

### Usage (user is correct)
Skill icons appear in **BOTH** the in-game skill bar (~44-56px slots) AND the char-select right panel (~48px). Same icon image; char-select adds the text description beside it. → They are **active gameplay UI assets**.

### Production route (cx + NLM canon)
- **Canonical = PixelLab** (create_image_pro + STYLE REFERENCE, AI Freedom 0). imagegen = temp/fallback ONLY for active gameplay icons (it adds blur/mixed-pixels at tiny sizes; fine for passive card fallbacks like the existing PassiveIcon pack, not canonical for skill-bar icons).
- User's batch idea (per-class header + bulleted "- <icon>" list, generate any count) = good structure.
- **HARD RULE in prompt: NO TEXT / no letters / no numbers — symbol/silhouette only.** Also: abstract rune/silhouette, high-contrast, class-accent color, transparent, NO gloss/vector/gold/parchment/painterly.

### Size: 64px, NOT 32px
- 64 PPU game, icons render 44-56px → 32px source = ~1.4-1.75x upscale = **non-integer = blurry/distorted pixels.**
- **Generate at 64×64** (NLM canon size; matches PPU64; downscales cleanly to 48-56). 32px too small.

### DEMO: ZERO icon generation
- The 8 skills for Warblade + Elementalist ALREADY have icons in `Assets/Sprites/UI/Icons/` (e.g. `Icon_Elementalist_Fireball/GlacialSpike/Meteor.png`), wired via `SkillIconRegistry.asset` + `SkillDatabase`. → **Demo reuses these. No gen, no gating.**
- **Full production** (eventually ~76 skills across 10 classes): PixelLab **64×64**, **batches of 16 (4×4 sheet** — matches existing PassiveIcon 4×4 slice pattern, clean grid-slice), per-class header + bulleted icon list, style-ref, AI-Freedom 0, **no-text hard rule**, WITH user (gated). Do when scaling past the demo.

---

## ORCHESTRATION NOTE
This is **Track 2 (gameplay demo)** — separate from Track 1 (UI screens / CharSelect rebuild, in flight). Spell VFX Phase-1 + demo wiring is its own implementation task, queued after the UI screens settle (or parallel via 2nd cx profile).
