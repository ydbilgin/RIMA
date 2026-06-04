# DECISION — CharacterSelect v2 Layout Pass 2 (user-directed, from rendered feel-test Image #11)
Date: 2026-06-04 · Source: direct user directives on the rendered roster room. Implement as spec'd; verify via council (cx Unity-observe + ax code-review).

## USER DIRECTIVES (verbatim intent)
1. "bu scene ekranındaki hit'e gerek yok" + "hitboxları capsüle ya da karakterlerin boyutu kadar olmalı" → the big full-rect Hit areas (oversized rectangles, overlapping) must go; clickable area = TIGHT to the character (character-sized / capsule-like).
2. "daha ayrık olsun" + "arka ve ön arasında boşluk olmalı" → characters more separated; clear vertical GAP between back and front rows.
3. "10 karakter tam görünmeli" → all 10 fully on-screen (none cut off / behind bg columns / faded off-edge).
4. "kırmızıyla sağda böldüğü yere dikey olsun karakter skilleri" → SKILLS = VERTICAL panel on the RIGHT (red line ≈ x0.84).
5. "geri ekranları da altta olsun" → SEÇ + GERİ buttons stay at the BOTTOM (thin strip).
6. "karakter seçince ... sol tarafa warblade heavy melee rage gibi bilgiler solda açılır pencerede yazsın" → IDENTITY (name + HEAVY·MELEE·RAGE + motto + resource + portrait) = LEFT pop-up panel that appears/updates on select.

## NEW LAYOUT (replaces the bottom 3-box HUD)
- **Roster room area:** x 0.02–0.83 (clear of the right skills strip + bg columns at edges), y above the bottom strip. Characters in TWO rows with a clear vertical gap:
  - **Front row (4 unlocked, scale ~0.95):** y 0.34 · Warblade(.20,.34) Elementalist(.40,.34) Ranger(.58,.34) Shadowblade(.74,.34)
  - **Back row (6 locked, scale ~0.82, dimmed):** y 0.62 (gap ~0.28 from front) · Ronin(.12,.62) Ravager(.255,.62) Gunslinger(.39,.62) Brawler(.525,.62) Summoner(.66,.62) Hexer(.795,.62)
  - All within x 0.12–0.80, y 0.34–0.62 (+sprite height) → fully visible, not behind the far-edge columns, not under the right skills panel.
- **Hitboxes (tight):** remove the oversized full-root Hit rect. Each character clickable area = TIGHT to the visible sprite. Preferred: put the Button on the sprite Image with `alphaHitTestMinimumThreshold ≈ 0.5` (character-shaped click) IF the idle_south sprite textures are Read/Write-enabled; ELSE a tight centered rect (~45% width × 70% height of the root, anchored over the body). No big overlapping rectangles. Front-over-back draw/raycast order preserved.
- **RIGHT vertical skills panel:** x 0.84–0.99, y 0.13–0.97. Framed (panel_frame_9slice + cyan edge). Header "SKILLS" at top. **VERTICAL** skill list (icon + name + short desc) — rewrite the horizontal strip to a vertical layout (VerticalLayoutGroup + vertical ScrollRect if overflow). Same SkillDatabase query/data.
- **LEFT identity pop-up panel:** x 0.01–0.24, y ~0.30–0.86. Framed (panel_frame_9slice + cyan edge + class-accent). Appears/updates on SelectClass: portrait (LoadCanonicalSprite) + class NAME (accent) + "HEAVY · MELEE · RAGE"-style tags + motto (accent) + playstyle (muted) + resource + lock text. "açılır pencere": hidden until first selection, then visible + a quick fade/slide-in on each select (lightweight — CanvasGroup alpha 0→1 over ~0.15s, optional).
- **BOTTOM thin strip:** y 0.0–0.12. **SEÇ** + **GERİ** buttons (centered or bottom-left). Keep locked CTA ("KİLİDİ AÇ — {cost} Echo", disabled) + scene-load wiring.

## KEEP / REUSE
SelectClass data wiring, SkillDatabase query, IsUnlocked/UnlockCost/LockedButtonText, RuntimeRoot_CharSelect + authored-disable, backdrop resilient-load (room_bg), pedestal_seal selection ring + glow/dim, RosterPlacements structure (just new coords + tight hit). The bottom 3-box HUD is REPLACED by left-popup + right-vertical + bottom-buttons.

## VERIFY (council, sequential — as user-established)
1. cx Unity play-observe: 10 chars fully on-screen at new 2-row coords w/ front-back gap; hit areas tight (not full-root); skills VERTICAL right panel (x≥0.84); identity LEFT popup updates on select; SEÇ/GERİ bottom; locked CTA; backdrop=room_bg; authored Root disabled; 0 errors.
2. ax 3.1 code-review vs this spec + nits.
3. Opus synth + commit + (status already current; note in next session block if needed).
