# RIMA — CharacterSelect Layout Pass 3 — INDEPENDENT CODE QC (Gemini 3.1 Pro High)

Independent QC code-review. READ the actual code (don't trust summaries):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\UI\CharacterSelectScreen.cs
Against spec:
- F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_LAYOUT2_DECISION_2026-06-04.md (base) + the layout-3 deltas: narrow panels (identity x0.012-0.175, skills x0.86-0.988), chars centered (front .34/.47/.61/.73 @y.34, back .27/.375/.48/.585/.69/.79 @y.62) so NO panel occlusion, and a Unity-UI selection VFX (animated glow, canvas-compatible since ScreenSpaceOverlay, BEHIND the character sprite).

Verify + find real issues:
1. **No-occlusion:** are char placements within x 0.175-0.86 (clear of both panels) accounting for sprite WIDTH (size 250 front / 200 back at PPU/scale)? Any char whose sprite edge still overlaps a panel?
2. **Selection VFX:** is it UI-Image based (NOT a raw ParticleSystem in overlay canvas)? Is it sibling-ordered BEHIND the character sprite (so it doesn't cover the char)? Activated only for the selected entry, disabled for others? Animated (rotate/pulse/mote drift) via existing AnimateRoomSelection without a heavy new system? Any per-frame alloc / leak risk?
3. **Sensible sizes:** identity popup compact (content fits, no overflow), skill rows compact vertical (204x76), nothing oversized?
4. **Kept intact:** SelectClass data/portrait, SkillDatabase vertical query, locked CTA (KİLİDİ AÇ disabled), backdrop resilient-load, tight-hit, scene-load — all undisturbed?
5. **Bugs/risks:** null-refs, raycastTarget on VFX images (must be false so they don't block char clicks), coroutine/Update cleanup on selection change, any over-engineering.
6. **Verdict:** PASS / PASS-WITH-NITS / FAIL + top 3 findings as `file:line — issue — fix`.

Cite real line numbers. Terse.
