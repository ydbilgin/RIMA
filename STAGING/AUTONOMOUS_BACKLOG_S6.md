# RIMA — Autonomous Backlog (S6) — what runs WITHOUT the user vs what's GATED

> Built 2026-05-30 after BLOCK A+B+C landed (5 commits, build GREEN, 2 cx + 1 agy reviews folded).
> **Rule:** every CODE step = `dotnet build RIMA.Runtime` green + writer≠reviewer (cx OR agy). Every DESIGN item =
> NLM (canon) + ax (research/UX) + cx (code-grounded) synthesis → **Opus final decision**. Animate = USER-GATED, always.
> Order is Opus's call; user can re-prioritize.

---

## ✅ TRACK 1 — CODE / MECHANICAL (fully autonomous, no user)
Each: Opus writes → dotnet green → cx/agy review → commit (push gated).

### 1A. BLOCK D — Conversion (finishes the demo's END) — HIGH, ~3-4h
- **D1** Victory `DemoCompleteOverlay` timeScale → 0 (resolve freeze-vs-slowmo first, see Design 2D). Verify coroutines use unscaled time.
- **D2** Next-class teaser: wire the already-generated `next_class_silhouette.png` instead of the hardcoded string.
- **D3** T4 PlayMode test: boss kill → `DemoCompleteOverlay.Show()` → Wishlist CTA active (the win-condition test).
- **D4** ⛔ Steam URL `/app/0/` — BLOCKED (needs real App ID). Leave a clear TODO + the exact 3 lines.

### 1B. Visual-systems code (controller side autonomous; scene rig gated → Track GATED)
- **RoomLightingController.cs** — per-room mood from `N3_LIGHTING_DESIGN_FINAL.md`, hooks `RoomLoader.OnRoomChanged(Action<int>)`,
  drives URP Light2D refs. Controller CODE = autonomous; the scene Light2D rig flip = gated.
- **HUD anchor pass** (`HUDController` self-builds): BossHealthBar → top-center (not bottom, not auto-under-first-canvas);
  HP+Rage top-left; minimap top-right; low-HP vignette; interaction prompt above bar (CONTROL_SCHEME §5). Placeholder frames exist.
- **Player-hit feedback** (agy FEEL-FIRST, 0-cost): hit vignette 0.6→0 / 0.2s + flash + player-hit-stop 0.08s on `Health.OnDamageTaken`.

### 1C. Controls completion (autonomous code; interactive verify F5-gated)
- **Controls/rebind UI section** in SettingsMenuUI — press-to-bind flow (listen → `TrySetBinding` → guard) + "Reset Controls". Write autonomous; F5 verifies.
- **C5 interact-key** (LOW) — centralize the 4 hardcoded `Key.G` via a registry Interact action (spec §3 excludes it → low value; do only if queue empties).

### 1D. Combat depth code (needs Design 2A/2B first, then autonomous)
- **Boss full 3-phase** — Penitent Sovereign currently 2-phase placeholder; canon chains-break 50% (done) + phase-3 at 33%, attack patterns/telegraphs (design-gated then code).
- **VFXRouter SpriteFlipbookVFX driver** (B2) — additive frame-driver script; entries wiring stays scene-gated.
- **AudioManager hardening** (cx flag) — clips private→`Resources/Audio` auto-load + hit-spam/lethal-double debounce. (Audio assets deferred.)

### 1E. Integration backlog (from `INTEGRATION_BACKLOG_S6.md`, 19-item ROI) — opportunistic
- Pick high-ROI, low-risk, self-contained items as filler between bigger tasks.

---

## ✅ TRACK 2 — DESIGN (autonomous: NLM + ax + cx → Opus decides) — produces locked specs, no code
Each becomes a `STAGING/*_DESIGN_S6.md` lock that then feeds Track 1.

- **2A. Boss 3-phase design** — phase thresholds (50%/33% done as numbers), attack roster, telegraph language, chains-break beats, phase-2/3 mechanics. Unblocks "Boss full 3-phase" code.
- **2B. Demo mob roster + encounter pacing** — canonical 5-room mob set (FractureImp + ShardWalker + HollowHulk + ?), counts/waves, difficulty curve, which room teaches what. Unblocks encounter wiring.
- **2C. Skill/draft balance** — the 4 Warblade skills + draft offer pool weights; what a 10-min run's power curve feels like.
- **2D. Victory/CTA + conversion flow** — freeze(0) vs slowmo(0.2) decision (work-order says 0; MOMENT_SPEC said 0.2 — resolve), CTA copy, Wishlist beat timing. Unblocks D1.
- **2E. Per-room lighting mood map** — apply N3 recipe per room (R1 intact → boss broken erosion), cyan budget per room. Unblocks RoomLightingController values.
- **2F. Audio design spec** — SFX/music palette + per-event mapping (design NOW, produce later via Sora+Gemini). No production.
- **2G. Readability / color budget (BLOCK H)** — cyan ONLY player/rift/telegraph; enemies+bg muted; juice scaling (big shake/hitstop finishers only).
- **2H. Story/lore beats** — Shattered Keep room monologues already exist (R2-R5); deepen/tighten + boss title-card lines.

---

## ✅ TRACK 3 — ART (semi-autonomous: cx imagegen → Opus/agy QC) — no user unless style-call needed
- **BLOCK E1** — imagegen Group B (~12 UI frames/glyphs, Path B chroma-bbox) + Group C (~20-30 skill icons 64px) via cx → QC → regen fails. Per `IMAGEGEN_ASSET_PACK_PLAN_S6.md`.
- **Python placeholders** — death/low-HP/particles stay Python (cheap, no AI).
- (Mob/boss SPRITES = GATED — art-direction decision, see GATED.)

---

## ⛔ GATED — needs the user (decision or Unity/F5 in the loop)
- **F5 combat-feel playtest** — the REAL lock (hitstop/shake/dash feel, cursor-aim snap, rebind works). Everything feel-tuned waits on this.
- **Unity SCENE wiring (BLOCK G)** — DraftManager.offerUI/offerGenerator + MapFragmentSpawner.fragmentPrefab refs; reassign 2 MapFragment prefabs to Environment script; Player.prefab layer 0→10 + IgnoreLayerCollision; lighting Light2D rig flip (Scene_Lighting GO + Decor_Cliff targets); BossHealthBar anchor; add MapProgressController + RoomTransitionFX GOs; skill icons → SkillIconRegistry; remove stale Gate_Room0_Exit. (Do when Unity MCP stable or hand to user; restart Unity first.)
- **Steam App ID** — unblocks D4 + all `steam://openurl` / `/app/0/` lines.
- **PixelLab Style-Reference** — hero-set pixel-perfect refine, slash-arc painterly flipbook, **boss sprite + 8-dir**, and **ANY animate step** (HARD: never animate without approval).
- **Mob/boss art DIRECTION** — A=archive-restore (0-gen, autonomous-able) / B=PixelLab / C=RTX-local Flux. Needs a "which path" call.
- **Audio real clips** — Sora + Gemini Pro (deferred) or RTX-local → `Resources/Audio/*.wav`.
- **git push** — remote divergence; force-push is the user's call.

---

## Proposed autonomous order (Opus, re-orderable)
1. **Design batch** 2D (victory) + 2A (boss) + 2B (mobs) — quick synthesis, unblocks code. (NLM+ax+cx→Opus)
2. **BLOCK D code** (D1→D3) — finishes the demo END. D4 stays TODO.
3. **RoomLightingController + HUD anchor pass** (controller code; rig stays gated).
4. **Player-hit feedback + juice** (FEEL-FIRST, 0-cost).
5. **TRACK 3 art** (cx imagegen Group B/C) interleaved when cx is free.
6. **Controls/rebind UI** (press-to-bind) + remaining design (2C/2E/2F/2G/2H).
7. **Integration backlog** fillers.
Each block: dotnet green + review + status/work-order checkbox update. Conflict → cx+ax input, Opus decides, ask user only if unresolved.
