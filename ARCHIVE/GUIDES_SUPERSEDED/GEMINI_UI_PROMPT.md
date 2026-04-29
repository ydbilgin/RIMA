# RIMA — Gemini UI Konsept Prompt'ları
> Kullanım: Her promptta RIMA_DarkFantasy_Concept.png upload et (style lock).
> Gerçek oyunda UI elementleri konseptteki gibi büyük görünmez — bunlar detay için yakın çekim referanslar.

---

## 1. Ana HUD — v3 (Ultra Minimal + Pixel Art Detay)

```
This is the visual style reference for a dark fantasy pixel art roguelite called RIMA.
Design an in-game HUD overlay in this exact pixel art style.

IMPORTANT CONTEXT: This is a zoomed-in reference mockup to show detail. 
In the actual game at 1080p, all elements are much smaller and peripheral — 
the game world fills the screen. Show the HUD elements large here for clarity,
but note their real proportions in descriptions.

DESIGN PHILOSOPHY:
- Hades-level restraint: every pixel earns its place
- Dark fantasy texture and detail WITHOUT being bulky or decorative for its own sake
- LMB (basic attack) and RMB (secondary) live separately from the skill number bar
- Not sterile-minimal — pixel art texture, iron/bone details, but slim and elegant

─────────────────────────────────────────────────────────────
BOTTOM LEFT CLUSTER — Resource bars + mouse buttons
─────────────────────────────────────────────────────────────

HP BAR:
  - Thin horizontal bar (~160px wide, 8px tall in real game)
  - Background: dark iron trough with faint crack texture
  - Fill: deep blood crimson (#8B0000 range), no glow — just solid
  - Left end: tiny skull icon (12×12px)
  - Right of bar: "480 / 500" in tiny bone-white pixel font

RAGE BAR (directly below HP, same width):
  - Same iron trough backing
  - Fill: cold electric blue (#4AADFF) with faint crackling energy texture in fill
  - Left end: tiny void/lightning rune icon (12×12px)
  - Right of bar: rage value number
  - "RAGE" in tiny grey caps above the bar (1px font essentially)

LMB / RMB SLOTS (below both bars, slightly left-aligned):
  - Two small slots side by side: [LMB] [RMB]
  - Each slot: 32×32px, slightly different frame from skill slots
  - Darker backing, thin iron frame with a subtle corner notch detail
  - Small mouse icon watermark inside when empty
  - "M1" and "M2" labels underneath in tiny grey text
  - These feel like weapon mounts, not skill hotkeys

Subtle dark backing panel behind all three elements — 
barely visible, like a worn iron plate set into the floor edge.

─────────────────────────────────────────────────────────────
BOTTOM CENTER — Skill bar (1–6)
─────────────────────────────────────────────────────────────
  - 6 slots in a row, each 38×38px
  - Dark iron frame — slightly embossed, one pixel border with inner shadow
  - Slot number tiny in top-left corner (pixel font)
  - Empty slot: faint inner dark texture, not bright
  - Active skill icon: slightly brightened, 1px cold blue inner glow
  - On cooldown: icon darkens to ~25%, circular clockwise sweep overlay in dark grey
  - Small horizontal gap between skill bar and LMB/RMB cluster — they are visually distinct

─────────────────────────────────────────────────────────────
BOTTOM RIGHT — Currencies
─────────────────────────────────────────────────────────────
  Two currency lines, stacked, right-aligned, minimal:

  GOLD: 
  - Skull coin icon (14×14px, pixel art) + "1,240" number
  - Warm dull gold color for the number
  - No frame — just icon + text floating

  RIFT SHARDS (secondary currency):
  - Small rift crystal shard icon (14×14px, cold blue-purple) + "89" number
  - Cold blue-white color for the number
  - Directly below gold line

  Both feel like corner watermarks, not UI panels.

─────────────────────────────────────────────────────────────
TOP RIGHT — Minimap
─────────────────────────────────────────────────────────────
  - Small panel (~120×120px), rotated 10–13 degrees clockwise
  - Looks like a physical map fragment placed on a surface — organic, not a clean window
  - Background: dark stone/parchment texture at 60% opacity — dungeon visible faintly through
  - Border: irregular worn parchment edge, slightly torn — NOT a clean rectangle
  - Room nodes: small squares connected by thin lines
  - Current room: faint cold blue glow dot
  - Visited rooms: dim grey squares
  - Unvisited/fog: even dimmer, barely visible
  - No title, no frame label

─────────────────────────────────────────────────────────────
TOP LEFT — Room counter (absolute minimal)
─────────────────────────────────────────────────────────────
  - "Room 4 / ?" — tiny pixel font, low opacity (~60% white)
  - Nothing else at top left

─────────────────────────────────────────────────────────────
STYLE RULES
─────────────────────────────────────────────────────────────
  - Pixel art throughout, consistent with reference image fidelity
  - Materials: worn dark iron, aged bone, cracked stone — no polished or bright surfaces
  - Rift energy (cold blue) used sparingly: active skill glow, rage fill, rift shard icon only
  - The HUD should feel like it's part of the dungeon — organic, not pasted on top
  - Show full dungeon scene behind — HUD elements sit at edges, game world dominates center

Render as a full 16:9 mockup with dungeon visible in center.
```

---

## 2. Yetenek Seçim Ekranı — v2 (Dramatik, Büyük Kartlar)

```
Same dark fantasy pixel art style as the reference image.
Design a SKILL SELECTION / DRAFT SCREEN for a roguelite.
This appears after defeating a boss or opening a special chest.

MOOD: This is a moment of power. The screen should feel weighty and exciting.
Reference: Slay the Spire card presentation meets Diablo loot screen — large, impactful.

─────────────────────────────────────────────────────────────
LAYOUT — Full screen
─────────────────────────────────────────────────────────────

BACKGROUND:
  - Dark dungeon stone, rift energy rising from the floor in slow wisps (cold blue/void purple)
  - Dramatic god-ray light from above (dim, dungeon-appropriate — not bright)
  - Atmospheric depth — very dark edges, faint center illumination

HEADER (top center):
  - "CLAIM YOUR POWER" in large gothic iron-plate pixel font
  - Decorative bone/chain divider line below text
  - Subtle skull motifs flanking the title

3 SKILL CARDS (centered, large, horizontal row):
  Each card is roughly 200×280px — substantial, readable, impressive

  Card anatomy (top to bottom):
  ┌─ TIER BANNER ─────────────────┐  ← thin colored strip across top
  │  [RARE] in matching color      │     grey=Common, blue=Rare, 
  │                                │     purple=Epic, gold=Legendary
  │  ┌──────────────────────┐      │
  │  │   SKILL ICON         │      │  ← Large icon area, 80×80px
  │  │   (pixel art)        │      │     Subtle tier-color inner glow
  │  │   with subtle glow   │      │
  │  └──────────────────────┘      │
  │                                │
  │  SKILL NAME                    │  ← Large bold bone-white text
  │  [🗡 Warblade] [⚔ Melee] [AOE] │  ← Small class + tag icons, iron badges
  │                                │
  │  ───────── iron divider ─────  │
  │                                │
  │  Description text              │  ← 2-3 lines, readable grey-white
  │  readable, 2-3 lines           │     smaller pixel font
  │                                │
  │  CD: 8s                        │  ← Small stat at bottom
  └────────────────────────────────┘

  Card frame: Dark iron with corner skull rivets
  Passive cards: diagonal "PASSIVE" banner across top-left corner (bone color)
  Legendary cards: faint animated shimmer on border (describe as shimmering)

  Cards slightly spread — center card very slightly larger/forward (emphasis on middle choice)

BOTTOM HINT:
  - "Click a card to select — or press 1, 2, 3" in small grey pixel text

OVERALL:
  - The 3 cards should feel large and impactful — this is a key moment
  - Dark atmospheric background lets cards stand out with their tier glows
  - Pixel art, consistent with reference image aesthetic
```

---

## 3. TAB Karakter Sayfası (Non-blocking overlay)

```
Same dark fantasy pixel art style as the reference.
Design a CHARACTER SHEET overlay — appears when TAB is held, game runs behind it.

NOT full-screen — a side panel sliding in from the left (~380px wide, full screen height).
Semi-transparent backing: dark stone texture at 75% opacity, game slightly visible through right edge.

PANEL SECTIONS (top to bottom):

CHARACTER HEADER:
  - Class icon (pixel art sword for Warblade) + "WARBLADE" name
  - Thin bone/iron divider below

STATS SECTION:
  - Iron plate section title: "STATS"
  - Rows of stats, each: [icon] [label] ............. [value]
    HP / Max HP / Rage Cap / Damage / Speed / Defense
  - Alternating very subtle row tints (dark iron vs slightly lighter iron)
  - Values in warm white pixel font

ACTIVE SKILLS SECTION:
  - Iron plate section title: "SKILLS"
  - 2×3 compact grid of skill cards (~72×90px each)
  - Each card: dark iron frame, skill name, small icon, CD badge
  - Tier color as thin left border accent

PASSIVES & TRAITS SECTION:
  - Iron plate section title: "PASSIVES"
  - List rows: [tier dot] [name] [Lv.2/3] ... [short description]
  - Trait rows below with gold stack count ×2

FOOTER:
  - "TAB to close" small grey text

Iron frame along the right edge of the panel — the boundary with the game world.
Pixel art, dark fantasy, feels like a tome or field journal.
```

---

---

## 4. Ana HUD — v4 "Watermark / Fragman" (Anti-Diablo, Ultra Organik)

> **Direktif:** Önceki HUD konseptleri çok Diablo-like geldi. Bu sefer tam tersi yön.
> Referans his: Dead Cells + Risk of Rain 2 + Hades — "UI var ama göremiyorum, oyun var"

```
Same dark fantasy pixel art style as the reference image (RIMA_DarkFantasy_Concept.png).
Design a MINIMAL "watermark-style" HUD for an isometric dark fantasy roguelite.

CORE PHILOSOPHY:
- The dungeon fills 95% of the screen. HUD is nearly invisible until you need it.
- No thick frames, no ornate borders, no Diablo-style orbs or heavy panels.
- Everything feels like it's faintly painted ON the dungeon floor/wall edges — organic, not pasted.
- Think: "graffiti scratched into stone" not "polished UI panel"
- Reference: Dead Cells minimalism + Risk of Rain 2 corner text + Hades restraint

─────────────────────────────────────────────────────────────
BOTTOM LEFT — HP + RAGE (the ONLY heavy elements, but still thin)
─────────────────────────────────────────────────────────────

TWO VERTICAL BARS side by side (not horizontal):
  - Each bar: ~10px wide, ~80px tall — slim vertical strips
  - No outer frame/border — just the fill against a very subtle dark backing strip
  - HP bar (left): deep blood red fill (#6B0000), almost black when low
  - RAGE bar (right): cold electric blue fill (#3A9FE8), crackle texture when full
  - Between the two bars: 2px gap, nothing else
  - Below both bars: "480" and "65" in tiny pixel font (6-7px size) — values only, no labels
  - Above both bars: tiny skull icon (HP) and tiny rune icon (RAGE) — 10×10px maximum
  - The whole thing fits in a 30×100px footprint — tiny

LMB / RMB (directly right of the two bars, same height zone):
  - Two slots: 28×28px each, minimal 1px dark stone frame
  - No glow, no hover state shown — just the icon
  - Tiny "M1" "M2" in 5px font below — barely visible (40% opacity)

─────────────────────────────────────────────────────────────
BOTTOM CENTER — Skill bar (1–6)  
─────────────────────────────────────────────────────────────
  - 6 slots in a row: 34×34px each, 3px gap between
  - Frame: 1px thin line only — no emboss, no shadow, no ornament
  - The slot number (1-6) in top-left as a watermark (20% opacity white)
  - Cooldown: icon darkens, thin clockwise arc overlay — no additional UI
  - Active/ready slot: very faint 1px inner cold blue line — that's it
  - The whole skill bar floats with NO backing panel — icons appear to sit on the dungeon

─────────────────────────────────────────────────────────────
BOTTOM RIGHT — Currency
─────────────────────────────────────────────────────────────
  Two lines, right-aligned, no frame:
  - [skull coin icon 12px] "1,240" — warm dull yellow text
  - [shard icon 12px] "89" — cold blue-white text
  - Text opacity ~70% — watermark feel, not demanding attention

─────────────────────────────────────────────────────────────
TOP RIGHT — Minimap
─────────────────────────────────────────────────────────────
  Same as before — torn parchment fragment, tilted ~12°, organic edges.
  BUT: even more transparent (50% opacity), rooms are just dots and lines.

─────────────────────────────────────────────────────────────
TOP LEFT — Room label
─────────────────────────────────────────────────────────────
  "Crimson Crypts — Room 4" in tiny pixel font.
  50% white opacity. Nothing else.

─────────────────────────────────────────────────────────────
STYLE RULES — STRICT
─────────────────────────────────────────────────────────────
  - FORBIDDEN: thick ornate frames, glowing orbs, heavy backing panels
  - FORBIDDEN: anything that looks like a phone game UI
  - FORBIDDEN: symmetric left-right mirror layout (no matching orbs at both corners)
  - The skill bar should feel like chalk marks on the floor, not a game controller UI
  - Show full dungeon scene in background — a dynamic combat moment
  - HUD elements should feel like they could be missed on first glance — that's correct

Render as full 16:9 mockup. Dungeon combat scene visible, character fighting enemies.
HUD elements at periphery, game world dominant.
```

---

## 5. Pure Gameplay Screenshot — "Panel Kapalı" Referansı

> **Direktif:** `character_menu_concept.png` dosyasındaki arka plan dungeon'ı baz al.
> O panel kapanınca görünen şey bu olacak. Gameplay hedef estetiği budur.

```
Reference image: the dungeon room visible BEHIND the character sheet panel in the provided image.
That background — NOT the panel itself — is the target. Recreate and expand it as a full gameplay moment.

GOAL: A full 16:9 screenshot of RIMA gameplay. No UI, no panels. Pure game world.

SCENE — Combat room, mid-fight:
  - Isometric ~60° view, same angle as reference
  - Stone flagstone floor — same cobblestone texture, moss in cracks, subtle wear marks
  - Stone block walls with visible depth (front face visible — not just top-down)
  - 2-3 torch sconces on walls: warm amber/orange flame, casting dramatic cone shadows
  - One area of cooler blue-white light (rift energy source, cracked floor glowing faintly)
  - Stone pillar(s) as environmental obstacle — same style as reference
  - Floor debris: scattered bones, cracked stone fragments, dried bloodstain
  - 1 hooded player character (dark cloak, small ~32px scale relative to tiles) mid-attack
  - 2-3 enemy silhouettes — dark fantasy creatures, attacking or staggered
  - Particle effects: faint cold blue slash trail from player weapon

LIGHTING:
  - Primary: torch warm amber — most of room lit this way
  - Secondary: rift crack cold blue — one spot of contrast
  - Corners: deep near-black shadow — high contrast atmospheric
  - No flat lighting, no uniform brightness

STYLE:
  - Pixel art, consistent with reference image
  - Dark, gritty, dungeon atmosphere — NOT colorful, NOT bright
  - Environmental storytelling: this room has seen many battles

Render as 16:9. No UI elements at all.
```

---

## 6. HUD Overlay — Gameplay Üzerine Minimal HUD

> **Direktif:** §5 promptuyla ürettiğin (veya `character_menu_concept.png` arka planındaki) gameplay görünümü üzerine HUD elementlerini yerleştir.

```
Take the dungeon gameplay scene (same style as the background in the provided character sheet image).
Now overlay a MINIMAL "watermark-style" HUD. The dungeon still fills 95% of screen.

Use EXACTLY this HUD layout:

BOTTOM LEFT:
  - HP bar: very thin horizontal strip (~160px wide, 6px tall), NO frame, just fill color
    Fill: deep blood red. Background: near-black strip. Skull icon 10×10px on left.
  - Rage bar: identical strip directly below, cold blue fill (#3A9FE8), rune icon left
  - Below both: LMB + RMB slots (28×28px each, 1px stone frame, "M1"/"M2" label)
  - Everything in this cluster fits ~180×90px total footprint

BOTTOM CENTER:
  - 6 skill slots (34×34px), NO backing panel
  - 1px thin border only, slot number watermark (20% opacity) top-left of each slot
  - Slots float directly over the dungeon floor — no panel behind them

BOTTOM RIGHT:
  - Gold: skull coin icon + number, no frame
  - Rift Shards: crystal icon + number, below gold
  - Both at 65% opacity — watermark

TOP RIGHT:
  - Minimap: torn parchment fragment ~100×100px, tilted ~12°, 55% opacity

TOP LEFT:
  - "Room 4 / ?" tiny pixel text, 50% white opacity

OPTIONAL — CHARACTER HP BAR:
  - Thin bar ABOVE the player character sprite (~40px wide, 4px tall)
  - Appears only when below 100% HP
  - Fades out after 3 seconds at full HP
  - Deep red fill, no frame, floats in world space

Show a full combat moment with enemies. HUD elements at periphery only.
```

---

## 7. Karakter HP Barı — Ayrı Konsept

> Tek başına üret — karakterin üstünde floating HP bar nasıl görünmeli?

```
Same dark fantasy pixel art style as reference.
Design a FLOATING HP BAR above a player character sprite in a dungeon scene.

The character is a small hooded figure (~48px tall) on cobblestone floor.

HP BAR OPTIONS — show 3 variations side by side:
  A) Ultra minimal: 4px tall red strip, 44px wide, no border, floats 6px above head
  B) Segmented: 8 small square blocks (each block = 1/8 max HP), red fill, 1px gap between
  C) With tiny skull: 10×10 skull icon on left, thin red bar 36px wide, total compact cluster

For each variation, show:
  - Full HP state (100%)
  - Low HP state (~25%, bar nearly empty / blocks nearly all dark)

Background: dungeon floor, torch light. Character small, bar clearly readable above them.
Style: pixel art, dark fantasy, minimal — NOT a health bar from a mobile game.
```

---

## Notlar

- Gerçek oyunda HUD elemanları konseptteki kadar büyük değil — bunlar detay referansı
- Her üretimden sonra Claude'a göster → QC → beğenilenler STYLE_BIBLE.md'ye eklenir
- Görseller Unity UI için wireframe/referans — direkt asset değil
- Style anchor: RIMA_DarkFantasy_Concept.png (her üretimde upload et)
- **v4 prompt:** Anti-Diablo watermark stili — dikey HP/Rage bar, çerçevesiz skill slot
