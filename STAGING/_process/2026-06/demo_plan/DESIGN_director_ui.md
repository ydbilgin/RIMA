# DESIGN — Director Mode UI Overhaul (G4)

Date: 2026-06-16 · Status: PLAN (read-only design; no code changed) · Demo: 19 Jun 2026
Target file: `Assets/Scripts/UI/DirectorMode.cs` (3091 lines, runtime-built UI)
Two problems: (A) overlay BLEEDS over reward draft = BUG; (B) frames look bad ("cok sacma") = cosmetic.

---

## 0. ROOT CAUSE — BLEED (verified against real code)

DirectorMode overlay canvas: `sortingOrder = 950` (l.700), `ScaleWithScreenSize 1920x1080 match 0.5` (l.702-706). So scaling is NOT the bug — sortingOrder + missing reward-state awareness is.

DirectorMode ALREADY hides itself for ONE case — the dual-class draft (l.2066-2073): comment "Director overlay (sortingOrder 950) ClassSelectionUI'i (190) orter" → calls `SetState(Test)` + `SetOverlayVisible(false)`. It does NOT hide for the **reward / skill draft** (`SkillOfferUI.Show`).

`SkillOfferUI` signals open/close through `UIManager`: `OpenSkillOffer()` (l.91,149) / `CloseSkillOffer()` (l.156). `UIManager` exposes the state with **no new API needed**:
- `UIManager.Instance.IsSkillOfferOpen` (l.38)
- `UIManager.Instance.IsAnyOverlayOpen` = tab|settings|skillOffer|skillCodex|pause (l.41)

So: when a reward draft (or any blocking overlay) is open, the Director overlay at 950 sits on top of it → the bleed. There is NO event on UIManager (grep: only field assigns at l.84/262/269/326), so the gate is **poll in Update()**.

### FIX (MEDIUM, surgical, highest demo value)
DirectorMode already has `Update()` (l.189) and the visibility primitive `SetOverlayVisible(bool)` (l.2080-2086, `overlayCanvasGo.SetActive`).
- Add a guard in `Update()`: when `State == Director` AND `UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen` → ensure overlay hidden (`SetOverlayVisible(false)`) + skip `UpdateFreeCamera/UpdateSpawnTool`; when it closes and still in Director → `SetOverlayVisible(true)`. Track a `bool overlaySuppressedByModal` so we don't fight the existing dual-class/SetState path.
- Belt-and-suspenders: drop overlay `sortingOrder` 950 → ~120 (above HUD ~10/100, BELOW SkillOfferUI/Pause). Reward/Pause/Settings canvases must be > Director. (cx: verify each modal canvas sortingOrder before lowering — do NOT guess.)
- Do NOT touch the dual-class branch (l.2066-2073) — it works; the modal-poll generalizes it. Avoid double-hide races: poll-gate only flips when `IsAnyOverlayOpen` transitions.

Canon confirms intent (NLM): reward draft opens → "Sessiz (Quiet) HUD: Draft acikken combat arayuzu VE Director overlay'e ait kalabalik bilgiler arka planda sessiz duruma gecmeli ve gorunmez olmali." So full-hide (alpha 0 + blocksRaycasts off, which SetActive(false) already gives) is canon-correct, not just a hack.

---

## 1. FRAME REDESIGN ("cok sacma" → on-canon)

NLM canon (visual language) — these are the rules the current frames violate:
- "Opak panel YASAK" (brain-cutting block). Current builders use solid dark fills: `0.05,0.06,0.08,0.80` (TabRail l.795), `0.02,0.025,0.035,0.28` (palette roots) — acceptable bg dim, but the **TopBadge** `0.48,0.18,0.06,0.92` (l.775) = near-opaque flat ember slab = the "cok sacma" offender.
- Canon frame = **thin translucent sharp-edged dark-fractured-stone 9-slice**; "ink-on-paper, opak slab YASAK"; UI breathes WITH the world.
- Palette (NLM, matches MEMORY canon): slate `#3A3D42` (frame skeleton), void-purple `#3A1A4A` (depth/shadow fill), ember `#E89020` (radial-gradient warm bleed BEHIND panels — small accents only), cyan ≤15%.
- Layout: prefer **ribbon / micro-banner** over screen-dominating panels. Room/objective = elegant corner micro-banner, not a giant box.

### Targets in code
- `[SerializeField] Sprite` skin slots ALREADY exist (l.47-53): `minimapFrame, slotNormal, slotActive, rewardCard, ribbonBase, menuButton, tooltipBox`. Panels use `CreatePanel(... Image.Type.Sliced)` with `minimapFrame` (l.855,1289,1576,1604,1669,1757) and `ribbonBase` (TopBadge, buttons). **So the redesign is mostly an ASSET swap, not a rewrite** — feed proper 9-slice sprites from the D3 modular pack into these slots + tune the flat fill colors.
- `LoadEditorSkin()` (l.178, 764) loads the skin; check it assigns the new sprites (the redesign hooks here, not new code paths).

### Recommendations
1. Replace flat opaque fills with canon: TopBadge `0.48,0.18,0.06,0.92` → translucent slate `#3A3D42` @ ~0.55 alpha + ember radial-gradient sprite behind (not flat ember). Tab/palette bg → void-purple `#3A1A4A` @ low alpha.
2. Feed D3 modular 9-slice (`panel_frame_9slice`, ribbon, button) sprites into `minimapFrame/ribbonBase/menuButton/slotNormal/slotActive` skin slots (`MODULAR_UI_ASSET_PACK.md` manifest). On-canon art direction = fractured-stone, sharp edge, translucent.
3. Convert the TopBadge "DIRECTOR MODE" slab to a thin ribbon/micro-banner (reduce 620x84 dominance; it's a dev tool — keep it discreet).
4. Readability: badge title is 32px (l.778) — OK; ensure tab/inspector body text ≥ ~16px on the 1920x1080 scaler (cx to measure the small ones; some palette labels look small).

REJECT note: package's 4-cardinal / no-flip sprite stuff is sprite-pipeline, not UI — N/A here; 8-direction canon (Karar #114) untouched by this work.

---

## 2. PRIORITY (risk / dependency)
- **P0 BUG (MEDIUM):** modal-poll bleed gate in `Update()` + lower overlay sortingOrder. Demo-critical (this is the user-reported "bleeds over reward draft"). No art dependency → do FIRST.
- **P1 (LOW):** recolor flat opaque fills (TopBadge ember slab → translucent slate + void-purple bg). Code-only, no asset wait.
- **P2 (MEDIUM, art-dependent):** swap 9-slice sprites from D3 pack into existing skin slots; ribbon-ify TopBadge. Blocked on D3 modular asset pack delivery.

---

## CODE TOUCH-POINTS (precise)
- `Update()` l.189-214 — insert modal-poll bleed gate (use `UIManager.Instance.IsAnyOverlayOpen`).
- `SetOverlayVisible(bool)` l.2080-2086 — reuse as-is (the hide primitive).
- `SetState` Director branch l.314-326 — leave; poll-gate coexists (track `overlaySuppressedByModal`).
- Dual-class branch l.2066-2073 — DO NOT TOUCH (already correct).
- `BuildOverlay` l.692-723: canvas `sortingOrder=950` l.700 (lower to ~120 after verifying modal canvases); `ScreenDimmer` l.711.
- `BuildTopBadge` l.773-785: ember slab color l.775 (`0.48,0.18,0.06,0.92`) → recolor + ribbon-ify.
- Skin sprite slots l.47-53 + `LoadEditorSkin()` l.178/764 — asset swap entry point.
- UIManager API (no change): `IsSkillOfferOpen` l.38, `IsAnyOverlayOpen` l.41, set in `OpenSkillOffer/CloseSkillOffer`.
