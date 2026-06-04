# UI REDESIGN BRIEF — 3 Screens (MainMenu · Settings-wire · CharacterSelect)
Date: 2026-06-04 (b) · Orchestrator: Opus · For: /council (cx + ax-3.1-Pro + ax-3.5-Flash) → Opus synthesis

## GOAL
Redesign 3 front-end screens. **Centerpiece = Character Select** (user gave a hand wireframe + a polished pixel-art reference). Must look premium, fit 16:9 cleanly, NOT cramped, and use OUR OWN 10-class roster + existing sprites. Two smaller jobs: MainMenu composition restyle, and wiring the existing Settings screen to the menu.

## NON-NEGOTIABLE CANON (from RIMA design + project rules)
- World: **The Fracturing / Shattered Keep**. Mood = "Vivid Vulnerability".
- UI philosophy: **"UI yoktur, sadece bilgi vardır"** — avoid flat opaque gray HUD boxes. Prefer ink-on-paper / diegetic, color = meaning.
- Color language: **cyan #00FFCC = Rift energy + ancient seals (emissive)**; void-purple ~#3A1A4A (unlit); warm-orange #E89020 = secondary accent (braziers).
- 10 classes, each has a canonical ACCENT color (RimaUITheme.ClassAccent) + identity (motto / playstyle / resource). Cyan is shared seal/Rift energy.
- Resolution: game runs **16:9** (tested 4K UHD). Reference image #6 is 1024×1024 SQUARE — layout must ADAPT to wide, must be resolution-robust (CanvasScaler Scale-With-Screen-Size, reference 1920×1080).

## KEY DESIGN TENSION TO RESOLVE (council, please weigh in)
The user's favorite reference (#6) uses **framed stone-roster + parchment detail panels**. Our canon says "no opaque boxes". 
PROPOSED RECONCILIATION (critique/improve this): treat panels as **diegetic in-world surfaces** (carved stone tablet for roster, aged parchment for details) — textured, edge-lit with cyan seal-cracks, NOT flat gray HUD chrome. This honors both "framed/readable" AND "ink-on-paper / no flat box". Agree? Better idea?

## CURRENT STATE (grounded from code audit)
- **MainMenu** (`Scripts/UI/MainMenuController.cs`, scene `Scenes/UI/MainMenu.unity`): runtime-built. Ruins background (main_menu_bg) + "Yine geldin." top + bare cyan text buttons BAŞLA/AYARLAR/ÇIKIŞ centered-left + radial vignette + v1.0. User: "güzel değil" — composition feels plain/floating. AYARLAR currently shows a "Yakında." stub overlay.
- **Settings**: A FULLY FUNCTIONAL `SettingsMenuUI.cs` already exists (ESC in-game overlay): Gameplay / Accessibility / Audio sliders / **Controls = click-to-rebind keys** with conflict guard + PlayerPrefs. `KeyBindManager.cs` holds DEFAULTS (WASD move, Dash=Space, Attack=LMB, Secondary=RMB, RiftBreak=V, Skill1-4=Q/E/R/F, CrossEcho=C) + rebind + JSON persist. → Task is just: make MainMenu AYARLAR open this existing settings UI (menu context), replace the "Yakında." stub. (Design input optional here.)
- **CharSelect** (`Scripts/UI/CharacterSelectScreen.cs`, scene `Scenes/UI/CharacterSelect.unity`): code is 3-column (5×2 grid | center portrait | right identity) BUT renders as a single collapsed panel at 4K (CanvasScaler issue). Will be REBUILT to the new wireframe below.

## CHARACTER SELECT — TARGET LAYOUT (user wireframe Image #5 + reference Image #6)
Three columns, full 16:9:
- **LEFT — Class Roster list** (vertical, scrollable if needed): all 10 classes as rows. Each row: small class avatar (REUSE idle_south sprite cropped) + class NAME + accent color. Selected row highlighted (accent glow). Locked classes shown dimmed/locked (4 unlocked default: Warblade, Elementalist, Ranger, Shadowblade; rest need Echo unlock).
- **CENTER — Selected class showcase**: the class's **idle ANIMATION** (we have RuntimeAnimatorController `Characters/{type}/{type}` + idle_south frames) standing on a nice on-brand backdrop (iso ruined-keep / Rift platform, cyan seal-cracks). "şimdilik South poz olabilir" (South idle OK for now). Big, hero-scale, the visual star.
- **RIGHT — Class details (scrollable)**: class name + identity (motto/playstyle/resource from RimaUITheme.ClassIdentity), then **skill cards/icons with names + descriptions scrolling downward**. Bottom: CONFIRM/SEÇ button (accent-colored), GERİ/back.

Available assets: 10× idle_south sprites + 7 directions + animator controllers. RimaUITheme class colors + ClassIdentity. Pack frames (button_9slice, card_frame_9slice, pedestal_seal, bg_seal_keep). PassiveIcon sheet (skill icons may need sourcing — note if generation needed).

## QUESTIONS FOR COUNCIL
1. **CharSelect composition**: best column proportions for 16:9 (e.g., left 22% / center 50% / right 28%?), so it reads premium and NOT cramped. How to frame the center showcase (platform/pedestal? vignette? cyan ground-seal under the character?). How to make the roster list elegant (row height, avatar treatment, selected-state, locked-state)?
2. **Panel treatment**: resolve the canon-vs-reference tension above. Concrete recipe (stone tablet? parchment? edge-lit cyan? 9-slice we already have?).
3. **MainMenu**: a stronger composition over the existing ruins BG — title "RIMA" treatment, button hierarchy/placement, motion/feel — without changing the BG art. Keep Turkish. Keep it premium but minimal (ink-on-paper).
4. **Skill display on CharSelect right panel**: do we have per-skill icons, or should skills be text + accent chips? If icons needed, what's the cheapest on-brand source (PassiveIcon sheet reuse? small generate batch?)?
5. **Assets to GENERATE** (user offered ax generate_image 1024×1024 / cx $imagegen): minimal list — only what REUSE can't cover. Be conservative (REUSE-first rule).

## CONSTRAINTS
- REUSE-first (existing sprites/Pack/theme); generation only where reuse fails.
- PixelLab / new character ANIMATION generation = GATED (with user) — do not assume new char anims.
- Output: concrete, implementable spec (proportions, colors, frame recipe, asset list). Keep it tight.
