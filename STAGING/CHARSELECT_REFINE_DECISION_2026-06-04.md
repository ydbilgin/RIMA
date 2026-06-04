# DECISION — CharacterSelect v2 Layout Refinement
Date: 2026-06-04 · Council: cx (feasibility/lines) + ax-3.1-Pro (design) + ax-3.5-Flash (lean) → Opus
Brief: STAGING/CHARSELECT_REFINE_BRIEF_2026-06-04.md

## UNANIMOUS DECISIONS
1. **Skills = bottom HUD RIGHT zone, NOT a vertical right side-panel.** All 3 agree. cx raycast analysis: a side-panel's viewport (raycastTarget=true) would block the right-side characters (x up to .92) and needs layering/rewrite (current skill helper is horizontal-only). Bottom-right = least-code + keeps the wide room clear. → just arrange bottom strip as **Identity | Details/Action | Skills** (skills = rightmost zone).
2. **Framed boxes (user's main ask):** convert the 3 bottom zones (IdentityZone/SkillZone/ActionZone, currently flat MakeRect at lines 427/431/435) → framed `MakePanel` with **`panel_frame_9slice` (Image.Type.Sliced)** + dark translucent tint + cyan edge, same anchors/offsets. Keep inner content/wiring. Outer strip keeps SmallPanelFrame/bar_frame. Buttons keep button_9slice.
3. **Character spread (wider, 2 clean rows, cx concrete coords):** update `RosterPlacements` (lines 65-77) only — no new layout system (anti-over-engineering, 3.5).
   - **Front (unlocked, size 300x410, scale ~0.98):** Warblade(.22,.44) · Elementalist(.41,.39) · Ranger(.59,.39) · Shadowblade(.78,.44)
   - **Back (locked, size 260x360, scale ~0.84):** Ronin(.08,.61) · Ravager(.25,.57) · Gunslinger(.42,.54) · Brawler(.58,.54) · Summoner(.75,.57) · Hexer(.92,.61)
   - X spans .08–.92 (was .20–.80) → spacious, click-targets separated. Front-over-back sibling order already handled (descending anchor.y at line 201).
4. **Left box = selected character portrait + identity** (ax-3.1): since the roster IS the room, the old "Classes-recap" is redundant — the left box shows the SELECTED char's portrait (idle_south) + name (accent) + tagline + resource (ClassIdentity). Small, premium.

## KEPT (no touch — cx)
RoomLayer/BottomHUDStrip split (0.25), RefreshSkillList(631-654), BuildSkillRow(657-707), MakeSkillStripArea(886-930, horizontal skill cards), BuildRoomCharacter click/lock/seal, SelectClass data wiring, backdrop resilient-load, EnsureSkillDatabase, IsUnlocked/LockedButtonText, scene-load.

## IMPLEMENTATION (touch only CharacterSelectScreen.cs)
1. Add const `PackPanelFrame = "UI/RIMA/Pack/panel_frame_9slice"` near lines 30-33.
2. BuildSkillDetailPanel (413-507): IdentityZone/SkillZone/ActionZone → framed MakePanel (panel_frame_9slice Sliced + dark tint + thin cyan edge child, raycastTarget=false). Order zones so SKILLS = rightmost. Add selected portrait Image to IdentityZone (LoadCanonicalSprite(selected), updated in SelectClass).
3. RosterPlacements (65-77): replace coords with the wider two-row table above (+ scales/sizes).
4. SelectClass: also refresh the new left-box portrait (LoadCanonicalSprite). Keep everything else.

## ANTI-OVER-ENGINEERING (3.5)
No dynamic-spacing layout engine, no skill scroll-paging beyond existing horizontal strip, no panel animation transitions, no custom scaling — hardcoded coords + CanvasScaler only.

## POST-IMPLEMENTATION VERIFICATION (user-directed: council members verify in sequence)
1. **cx QC** — enter Unity play, observe: authored Root disabled, 10 chars at NEW wide coords, 3 framed boxes (panel_frame_9slice) in bottom bar, skills rightmost, left-box portrait updates on select, Elementalist skills + Ronin locked-CTA, backdrop=room_bg, 0 console errors → PASS/observations.
2. **ax-3.1 QC** — review the implemented CharacterSelectScreen.cs against this decision + flag any nit → approve.
3. **Opus** synth + own play-verify → commit → update CURRENT_STATUS.md + memory.
