# UI Redesign — 4 Screen DECISION (2026-06-04, council + user)

Council: cx (feasibility/reuse) + ax Gemini 3.1 Pro (deep design) + ax 3.5 Flash (lean, pending) → Opus synth. User answered open questions. Brief: `STAGING/UI_REDESIGN_BRIEF_2026-06-04.md`.

## Locked cross-cutting
- **Eradicate opaque containers** (3.1 Pro's #1 impact): strip heavy 9-slice "boxes"; float text/icons on soft vignettes or directly on world. Translucent, thin, sharp.
- **Color = meaning:** use `ClassAccent(cls)` in HUD/CharSelect instead of generic system cyan where it reinforces identity.
- **Reuse only, NO imagegen for MVP** (cx confirmed death_screen_bg + main_menu_bg imported + on-brand).
- **Build seed: removed everywhere** (user). Also strip `Build:` line from DemoCompleteOverlay for consistency.

## 1. DEATH SCREEN (cx scope M) — FIRST
- REMOVE: `CopyBuildSeedButton` + `CopyBuildSeed()` + listener + `Build:` stat line; **WISHLIST button entirely** (user: "tamamen kaldır"); `NextClassTeaser` (clutter).
- Blackout = **soft vignette gradient**, frozen combat dimly visible beneath (NOT opaque block — 3.1 Pro canon risk).
- Center vertical flow, generous spacing: (a) random canon death line (pixel-serif ~28pt, TextPrimary), **NOT context-aware** (canon: never personal); (b) cyan hairline divider; (c) run stats vertical/small `ODA · KILLS · SÜRE` (drop Build); (d) two equal buttons **TEKRAR DENE [R]** · **ANA MENÜ**, consistent Pack button_9slice + cyan hover, thin cyan hairline border (no opaque panel).
- After seed removal: if compile clean, remove now-dead `RunStats.BuildSeed/GetBuildSeed/SkillToken`. Keep `RunStats.BuildName` only if still referenced; else drop. Update DemoCompleteOverlay to not show Build.

## 2. CHARACTER SELECT right panel (cx scope M) — SECOND
- New API `RimaUITheme.ClassIdentity(ClassType) → (motto, playstyle, resource)` (do NOT widen ClassTagline — breaks center panel). Data = brief Ek-A (NLM 10-class identities).
- Right panel DYNAMIC on select: **motto** (accent color, bold) + **playstyle** (1-2 sentences, TextMuted, wrap) + **resource** (TextPrimary, sharp — e.g. "Rage 0-100 · sadece vurarak dolar"). Typography hierarchy: muted flavor vs primary mechanic (3.1 Pro).
- Accent bar = `ClassAccent(cls)`. **NO skill list** (user + both advisors: clutter without tooltips). Locked class: show identity + "X Echoes gerekli".

## 3. SKILL BAR (cx scope S/M) — THIRD (all local to SkillBarUI.cs)
- **Class-accent glow** (replace fixed cyan ready-glow with `ClassAccent(PrimaryClass)`, cache on OnPrimaryClassSet).
- **Key labels** readable: bigger font, higher alpha, TMP outline/shadow, slightly larger key rect.
- **Cooldown radial-only** (NO numeric seconds — canon "sayı minimal"); darker/clearer overlay + **ready-flash** (track wasOnCooldown).
- **Drop/lighten the 9-slice backing** → hex slots float with crisp drop-shadow (3.1 Pro: backing reads as a box; canon "no horizontal separator/box").

## 4. MAIN MENU (cx scope S/M) — FOURTH
- Tagline: **"Yine geldin."** (3.1 Pro; quiet systemic canon, replaces "THE SEAL BENEATH THE KEEP").
- Reuse `main_menu_bg` (cx: stronger than Pack bg_seal_keep here). No bloom-pulse (clashes with crisp dirty-paper); slow parallax = optional defer.
- Buttons → Pack button_9slice translucent + cyan hover; NEW RUN primary. **Hide SETTINGS completely** (3.1 Pro: disabled "soon" reminds it's software).
- Version label small, bottom-right, ~35% opacity.

## 5. ROOMS / MAP POOL (user: "10-15 oda olabilir random → pool'u genişlet")
- `MapFlowManager.mapsPerRun` 3 → **~12** (range 10-15).
- **Expand map pool 6 → ~12-15** distinct iso shapes (parametric boundary-tracer + auto-cliff like Map04-06; sub-agent). Add to `MapList_Act1.asset` + Build Settings. Coordinate with B-12 (DungeonGraph) — if scene-based stays, just add scenes.
- Separate work-stream from UI; do after UI screens.

## Order (cx, each separate commit, play-verify after Unity D3D11 restart)
Death → CharSelect → SkillBar → MainMenu → (rooms pool expansion).

## Notes
- All runtime UI code → compile-verify now (Unity edits/compiles fine on D3D12); PLAY-verify after user restarts Unity to D3D11.
- 3.5 Flash lean lens pending; fold any scope-cut before finalizing each screen (consensus already = reuse + surgical, low risk of contradiction).
