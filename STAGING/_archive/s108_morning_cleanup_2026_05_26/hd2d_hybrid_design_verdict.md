# HD-2D HYBRID PIVOT — Opus Design Verdict

**Decision-maker:** rima-design (Opus 4.7)
**Date:** 2026-05-23
**Verdict overall:** **REJECT for RIMA. ROUTE to SNAP/future title as STUDIO_KARAR aday.**

---

## DECISION

**RIMA stays on Option C (Hybrid Template + Decor Overlay, 2D rendered).** HD-2D is a legitimate new studio pattern but the wrong tool for **this** project at **this** stage.

**RATIONALE:** Three independent reasons each sufficient to reject.

1. **Sunk-pipeline cost is decisive.** Chibi 64px char + 8-dir + weapon decouple + HandAnchor + LoRA on 335 baked-2D images + RimaWorldPainterWindow + 6-layer painter + S101 wall sheets + 32x22 sub-room v4 lock — every one of these compounds **against** 3D environment. Switching the wall layer to 3D meshes invalidates the painter (which is the only authoring surface), invalidates the S101 sheet pipeline (no place to put them), invalidates the architecture_decision.md anchor-overlay design (anchors are 2D Vector2 localPos), and forces solo dev into Blender/ProBuilder + UV + texel density + Pixel Perfect 3D-projected sprite mixing — a learning curve of weeks, not days, to ship parity with Option C's already-validated path.

2. **The diagnosis is wrong.** User cites "PixelLab output flat top-down, not iso" as motivation. That is a **prompt-and-LoRA-distribution problem, not a 2D-vs-3D problem.** The Option C decision was made **explicitly because** the painted-template approach removes per-tile iso burden — the LoRA bakes the iso angle into the whole-room template at generation time. HD-2D would solve a problem (consistent iso geometry per piece) that Option C **already solves** (iso is baked into the template, never assembled).

3. **KARAR #150 is fresh and the road is open.** S102 is mid-execution on a v4 evidence-based 32x22 dungeon-inside aesthetic with a passing reference image. Pivoting now burns a session of forward motion and re-opens the v3-vs-v4 debate against a path that has no proof slice on the wall yet.

**TRADE-OFF:** Give up: dynamic lights actually casting on geometry, true verticality, camera tilt-on-demand, real shadows from physics. Keep: a finished pipeline that ships rooms this week.

**SYSTEMS AFFECTED:** Wall production, room authoring, lighting, camera, character integration, LoRA scope.

**CONFLICTS WITH LOCKED RULES?**
- KARAR #150 LIVE: HD-2D wall pivot **contradicts** the L3 wall class production target ("24 wall sprites per Act"); 3D walls = mesh kit, not sprite sheets.
- KARAR #143/#147 6-layer painter LIVE: 3D walls **break** the L3 layer's painted authoring; would need a parallel mesh-placement tool.
- STUDIO_KARAR_039a (top-down/oblique 2D default): see §1 interpretation.

---

## §1 — Is HD-2D a violation or a new pattern?

- **STUDIO_KARAR_039a** "top-down/oblique default 2D" reads as **rendering style + pipeline aesthetic**, not just "must not use 3D meshes." Octopath/Triangle Strategy read as **pixel-art aesthetic** because textures, characters, and lighting are tuned that way — the underlying 3D mesh is invisible to the player. So HD-2D **passes the aesthetic clause** of 039a but **fails the pipeline-default clause**: deviation from default, not violation of style.

- **STUDIO_KARAR_049** "3D track = roguelike only" — "track" reads as **production track / studio capability allocation**, not "no 3D meshes anywhere ever in a 2D game." Environment-only 3D under a 2D camera is a **2.5D production technique**, not a 3D track. HD-2D **does not violate 049 strict reading**, but **spirit of 049 is studio focus** — committing a second project (RIMA) to 3D-environment authoring dilutes focus.

- **KARAR #037 Pattern Proof Gate** applies. HD-2D is a new pipeline pattern with cross-game implications. **Proof slice mandatory before adoption.**

**Verdict:** HD-2D is a **NEW PATTERN**, not a violation. Proposed studio name: **STUDIO_KARAR_051a aday — "HD-2D Hybrid Track (2D chars + 3D environment + ortho ref-cam)"**. ADAY status only; needs proof slice on a fresh game, not RIMA.

---

## §2 — RIMA-specific implications

- **Irregular dungeon shape:** HD-2D solves it cleanly. **BUT** Option C also solves it (whole-room template = whatever silhouette the LoRA paints). RIMA's KARAR #150 v4 already established that 32x22 painted-template silhouette works. Option C wins on cost.
- **Option C status under HD-2D:** Superseded for environment, **but the anchor/decor model survives**. The 6 templates + 28 decor count becomes "6 mesh rooms + 28 sprite decals" — but the 6 mesh rooms cost dramatically more than 6 LoRA gens.
- **LoRA plan:** Still makes sense **for decor + characters** under either path. **Texture LoRA for 3D walls** is a separate, smaller training set and **adds work**, not subtracts.
- **Cross-genre transplant (KARAR #017):** HD-2D is **applicable studio-wide but expensive per game.** Not a free transplant — every game pays the Blender/ProBuilder + texture-mapping cost. Worth it for a **future title designed around it from day one**.

---

## §3 — New feature opportunities from 3D environment

| Feature | Worth dev cost for RIMA? | Why |
|---|---|---|
| Dynamic point lights cast on geometry (torches, rifts lighting walls) | **No** — Light2D + decals deliver 80% feel at 5% cost | Cost/value bad |
| True verticality / multi-floor sub-rooms | **No** — KARAR #150 sub-room is single-elevation | Out of design scope |
| Camera rotation moments (boss reveal) | **No** — Karar #100 8-dir char only has 5 rotated sprite sets; rotating camera breaks dir math | Breaks lock |
| Parallax depth on walls | **Already achievable in 2D** via 6-layer painter L1/L2 background | Already solved |
| Real physics shadows / falling debris | **Marginal** — combat juice spec already covers debris with 2D physics | Diminishing return |
| Floor-as-ramp / stair platforming | **No** — RIMA not a platformer; combat grounded on flat sub-room | Out of genre |
| Tilt cam during specific encounters | **No** — Karar #148 already locks Branch D+E camera tilt as 2D vfx | Out of scope |

**Net new value HD-2D delivers to RIMA that Option C cannot:** **near-zero**.

---

## §4 — Risks specific to RIMA

- **Style coherence:** Octopath works because of **pixel-snapped textures + ortho cam + zero perspective foreshortening + heavy DoF/bloom unifying both**. Solo dev hitting that polish bar on first attempt is months-long calibration. **High risk.**
- **Performance:** Not a concern.
- **Pipeline / Blender learning:** Solo dev not currently a Blender user. **Weeks of ramp.** Highest risk.
- **Texel density at iso:** Pixel-perfect texture mapping at 85-90° tilt requires UV at exact texel multiples on every face. **Repeatable production discipline solo dev does not yet have.** High risk.
- **Ortho vs perspective:** Locked ortho. Mixing 2D sprites in perspective cam = parallax mismatch hell.

---

## §5 — Comparison verdict

| Criterion | Option C (Template + Decor 2D) | HD-2D Hybrid |
|---|---|---|
| Solo dev time to first playable | **Days** | **Weeks** |
| Quality bar match to chatgpt_ref | **Direct** — chatgpt_ref *is* painted 2D | **Indirect** — must reverse-engineer painted look in 3D textures |
| Irregular dungeon flexibility | High | Highest (overkill) |
| Procgen integration | Stable | Unsolved — mesh-room procgen separate problem |
| Studio asset reuse | Decor sprites portable | Walls/textures portable; chars stay 2D either way |

**Verdict:** **Option C wins decisively** on every metric except theoretical ceiling.

---

## §6 — STUDIO_KARAR proposal (for the new pattern, NOT for RIMA)

**STUDIO_KARAR_051a (ADAY)** — *HD-2D Hybrid Production Track*

- **Locks (if approved later):** A title may elect HD-2D Hybrid track at project start, requiring: orthographic camera with fixed tilt 60-90°, 3D mesh environment authored in Blender/ProBuilder, pixel-snapped tileable textures at fixed texel density, 2D pixel-art characters as sprites in 3D scene, no character-mesh hybrid permitted.
- **Deprecates:** Nothing immediately. Coexists with KARAR_039a as deviation track.
- **Requires:** KARAR_037 proof slice (1 fully playable room, 1 char, 1 enemy, ortho cam at locked tilt) before any second title adopts.
- **Cross-ref:** STUDIO_KARAR_039a (deviation, not violation), STUDIO_KARAR_049 (does not extend "3D track" definition; this is a 2.5D production track), STUDIO_KARAR_048 Farever (orthogonal — different art direction), STUDIO_KARAR_037 (proof gate applies).
- **Pilot game recommendation:** **NOT RIMA.** A future title where the design pillar **starts from** vertical/lit/rotating environments. **SNAP** is the natural candidate if its design brief favors environment-as-character; otherwise hold for next concept.

---

## ORCHESTRATOR NEXT STEP

1. **No change to RIMA pipeline.** Continue Option C path.
2. Save short memory note: `memory/project_hd2d_pivot_rejected_for_rima_2026_05_23.md`.
3. If user wants STUDIO_KARAR_051a aday formally registered: **separate dispatch** to studio constitution doc.
4. **Codex tech review (parallel):** read verdict when returns. If Codex flags 2D-implementation blocker, revisit.

**Files referenced:**
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/architecture_decision.md` (Option C, current path)
- `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/MASTER_KARAR_BELGESI.md` (KARAR #150 lines 373-423)
