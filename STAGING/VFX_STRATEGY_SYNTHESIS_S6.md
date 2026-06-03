# RIMA VFX Strategy — FINAL SYNTHESIS (S6): PixelLab-animated sprites vs engine-native

> 3-source synthesis — **cx** (code-grounded inventory, CODEX_DONE.md) + **ax** (industry/UX research,
> AGY_DONE_ydbilgin.md) + **Opus** (final). Criterion #1 = RIMA-fit (PPU64 pixel-perfect, cyan #00FFCC, demo scope).
> Answers the user's two questions. **No VFX produced here** — this is the decision doc. Any PixelLab *animate* step is
> separately user-gated ([[feedback-never-animate-without-approval]]).

## TL;DR — the answer to "PixelLab-animate vs use directly?"
**Neither universally. HYBRID, split by ROLE.** All three sources converge:
- **Engine-native (the BULK)** for VFX that are **high-frequency, directional, or gameplay-reactive** — they must
  scale/tint/aim/retarget at runtime, which a baked sprite cannot.
- **PixelLab animated sprite (the FEW)** for VFX that are **low-frequency, radial/screen-aligned, and signature
  "hero" moments** — seen rarely, look matters most, no per-frame adaptation needed.
- **Golden rule:** if you bake a PixelLab VFX, design it **RADIAL / direction-agnostic / screen-aligned** so it spawns
  at 0° for any angle. Rotating a pixel sprite to an arbitrary angle = "mixels" (the #1 pixel-VFX mistake).

## Reality check — RIMA's VFX are 100% engine-native TODAY (cx)
Nothing in RIMA uses a sprite-flipbook VFX yet. Current stack (all working, do NOT disturb):
HitSpark/HitImpact = ParticleSystem · SlashArcVFX = LineRenderer · dash = DashEvent→VFXRouter + AfterimageTrail ·
StopDustVFX = ParticleSystem · projectiles/beams = SpriteRenderer primitives + LineRenderer · auras/glows = tint +
ParticleSystem + Light2D · DeathVFX = DeathBurst ParticleSystem · impact-frame/vignette/shake/damage-numbers =
screen-space procedural. `VFXRouter` already pools by tag, places/rotates, plays SFX — so adding a sprite VFX is cheap.

## THE DECISION MATRIX (which VFX → which method)
| VFX | Method | Why (RIMA-fit) |
|---|---|---|
| **Hitspark** | 🟪 **PixelLab sprite** (+ native garnish) | Radial/symmetric → no rotation. The `11127e69` you made = the **best first conversion**. High-freq → tint+scale the ONE sprite (cyan=player, purple=enemy), layer tiny native sparks. |
| **Kill pop / small death burst** | 🟪 **PixelLab sprite** core + native shake | Radial one-shot, carries authored cyan/void identity. |
| **Boss-death sequence** | 🟪 **PixelLab sprite** | Rare, dramatic, tailored to boss sprite — staged bursts/disintegration. |
| **Rift / portal open** | 🟪 **PixelLab sprite** ⭐ | RIMA's **brand element** — worth a hand-authored open/loop/close sheet. Highest art-ROI. |
| **Ultimate / Rift-Break burst** | 🟪 **PixelLab sprite** | Screen-anchored keyframe impact, seen rarely. |
| **Big skill payoff** (Meteor/Earthsplitter/explosion impact) | 🟪 **PixelLab sprite** for the IMPACT only | Structural weight + smoke billows. Keep the moving body native. |
| **Slash arc** | 🟪 **PixelLab painterly flipbook** (CANON CORRECTION 2026-05-30) | ⚠️ NLM canon overrides the earlier "keep native": the detached-weapon design HIDES the weapon during a swing and shows a **painterly slash-arc VFX flipbook** — it's the centerpiece, not optional. So slash **VISUAL = painterly flipbook** (8-dir baked or render-texture-snapped to dodge mixels); **hitbox/timing stays code-driven** (current `SlashArcVFX` LineRenderer = stopgap, keep it driving the hitbox). This trades native's variable-radius control for the authored painterly look — canon's explicit choice. See `RIMA_DIRECTION_LOCK_S6.md` §7. |
| **Dash trail** | 🟩 **KEEP native** (ghosting/AfterimageTrail) | Directional + variable length + aims from move OR cursor. Your `58c183a0` baked-one-orientation = wrong fit. Capture player frame → fading cyan copies. |
| **Knockback / stop dust** | 🟩 **native** ParticleSystem | Variable emission + ground-contact timing + scale. |
| **Projectile trails / beams / chain-lightning** | 🟩 **native** LineRenderer/procedural | Arbitrary start/end, distance, retarget. (PixelLab can do the *endpoint impact*, not the beam.) |
| **Status auras** (burn/freeze/poison/rage) | 🟩 **native** tint + particles + Light2D | Long-lived, must respond to stacks/duration/intensity. |
| **Boss telegraphs** | 🟩 **native** | Readability/range/timing > authored frames. |
| **Screen-space** (impact-frame, vignette, shake, damage #) | 🟩 **native** | Already cheap + correctly screen-space. |
| **Ambient void embers/dust** | 🟩 **native** ParticleSystem | Procedural avoids repeating-pattern fatigue. |

## The other half of the win — make RIMA's NATIVE particles look hand-drawn (ax "Pixelated Particle" stack)
The native VFX won't clash with pixel art **if** you apply 4 constraints to every particle material/system:
1. **Point-filter, low-res textures** (8×8 / 16×16), no compression, no mipmaps.
2. **Pixel-snap in world space:** `snapped = round(worldPos × 64) / 64` (PPU64) in the VFX shader.
3. **Palette-lock:** step-interpolated `Color over Lifetime` (no smooth gradients) → cyan/void only.
4. **Framerate-step the sim to 12–15 FPS** — 60 FPS smooth motion is the #1 thing that makes particles look detached
   from 12 FPS hand-drawn frames (ax "temporal clashing").
This is the single highest-impact, no-PixelLab task for VFX cohesion. (Currently RIMA particles likely run smooth/60.)

## Verdict on YOUR two reference objects
- `11127e69` **Hitspark** → ✅ **KEEP & use.** Radial, on-brand, the canonical first PixelLab VFX. Wire via a tiny
  `SpriteFlipbookVFX` driver registered in `VFXRouter` as `hit_default`.
- `58c183a0` **Dash trail** → ⏸️ **shelve.** Directional baked sprite fights the grid + can't match variable dash.
  Use native afterimage ghosting instead. (Good teaching example of the wrong fit — not wasted, just not for dash.)

## Integration cost (cx) — when we DO add a sprite VFX
- Import: Sprite (Multiple), PPU 64, **Point**, no compression, no mipmaps, sorting layer `VFX`.
- Runtime: a **tiny `SpriteFlipbookVFX` frame-driver** (OnEnable resets frame/time → advances SpriteRenderer.sprite[]
  → returns to pool) — NOT an Animator (heavier to pool). Register in `VFXRouter.entries`; set lifetime ≥ frames/fps.
- Don't rewrite HitImpact + VFXRouter both at once; assign the PixelLab prefab in one path first, play-verify.

## Highest-ROI production order (when greenlit + per-asset animate approval)
1. **Pixelated-particle pass** on existing native VFX (no PixelLab, pure cohesion win). 
2. **Hitspark** sprite (already made — just wire it).
3. **Rift/portal-open** sprite (brand ⭐).
4. **Boss-death** + **ultimate** sprites.
5. Big-skill-payoff impact sprites.
Everything else stays native.

## Risks / do-NOT (cx)
Don't disturb the working juice stack (CombatEventBus/ProcLimiter/ScreenShakeDriver/ImpactFrameDriver/HitFlashDriver/
DamageNumberDriver/VignetteFlashController) · don't replace screen-space feedback with world sprites · don't globally
rotate a directional PixelLab sprite for slash/dash · don't bake long-lived auras/status into flipbooks.
