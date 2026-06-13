# PixelLab YouTube Research — Last 3 Months (2026-03-13 to 2026-06-13)

> Channel: https://www.youtube.com/@PixelLab_AI
> Window: upload_date >= 20260313. Tool: yt-dlp 2026.03.17, auto-subs -> VTT -> cleaned text.
> Found in window: **11 videos + 15 shorts = 26 items**. **All 26 had transcripts** (0 missing).
> Raw cleaned transcripts: `STAGING/_process/2026-06/pixellab_yt/<date>_<id>.md`

---

## (a) Index Table

| Date | Title | Type | Transcript | 1-line topic |
|---|---|---|---|---|
| 2026-06-12 | How to Fix Pixel Art Scale Perfectly (PixelLab Trick) | short | yes | Inpaint V3 next to an existing sprite so the model infers correct scale |
| 2026-06-09 | PixelLab Tutorial: Consistent Characters + Animation | video | yes | Full pipeline: character -> style-ref cast -> states -> custom animation V3 |
| 2026-06-09 | Stop making static pixel art for your indie game! | short | yes | Animate idle/motion instead of shipping static sprites |
| 2026-06-01 | Pixelorama + PixelLab: Create, Edit, Rotate, Animate | video | yes | Built-in browser editor (Pixelorama) with all PixelLab tools inline |
| 2026-05-29 | This AI Website Makes Pixel Art Game Assets | short | yes | General promo / asset-website overview |
| 2026-05-20 | This Feature Saves Hours of Animation Work | short | yes | Animation feature promo (interpolate / animate-with-text) |
| 2026-05-16 | PixelLab Character States: The New Way to Animate Sprites | video | yes | **States feature** — pose-first, then animate; variants, transitions, boss states |
| 2026-05-12 | Making Objects Respond to Players Hits Different | short | yes | Object Creator + states + V3/pro to make interactable objects (grass/statue/chest) |
| 2026-05-11 | This AI tool saves game devs HOURS on pixel art | short | yes | General promo |
| 2026-05-10 | How to Generate & Animate GBA-Style Sprites | video | yes | GBA top-down full game art; mid-stride start-frame trick; rotation->create-character button |
| 2026-05-04 | This auto-rotate feature saves hours | short | yes | Rotate-to-8-directions promo |
| 2026-04-30 | Stop stressing over game assets. try pixellab.ai | short | yes | General promo |
| 2026-04-27 | Introducing Object Creator: Objects, Packs, States & Animations | video | yes | **Object Creator page** — packs, 8-dir objects, object states, object animations |
| 2026-04-24 | This Loot Generation Trick Saves Hours | short | yes | Style-ref pro to generate rarity tiers (common/rare/epic/legendary) of items |
| 2026-04-22 | Stop Wasting Hours Making Game Assets | short | yes | General promo |
| 2026-04-20 | I Tried Using PixelLab for a Game Jam | video | yes | Real game-jam pipeline (card game); style-ref loop, palette lock, manual bg removal |
| 2026-04-15 | Never hand-draw pixels again | short | yes | General promo |
| 2026-04-13 | Perfect character consistency using this method | short | yes | One style image -> whole cast via create-from-style-reference pro |
| 2026-04-09 | How to Make a Full Side-Scroller Asset Pack | video | yes | Side-scroller chars+anims+tilesets+parallax; interpolate for seamless run loop |
| 2026-04-06 | How to Make Animated Pixel Art Scenes | video | yes | Animate character+background+whole scene; layered partial animation; pixel-correction |
| 2026-04-02 | Game dev trick: Create tilesets with a single click | short | yes | **Tileset creator PRO update** — lower/upper terrain + walls + transitions |
| 2026-03-31 | Generate tiles in seconds with PixelLab | short | yes | Tileset generation promo |
| 2026-03-25 | New PixelLab Tool: Animate Between 2 Frames | video | yes | **Interpolate (new)** — first+last frame, transitions, loops; web/Aseprite/Pixelorama |
| 2026-03-18 | This Tool Generates Pixel Animations for You | video | yes | **Animate with Text V3 (new)** — image-ref+prompt, cheaper than pro, big-canvas frames |
| 2026-03-16 | Game dev trick: Turn one character into a whole crowd | short | yes | Style-ref crowd generation promo |
| 2026-03-13 | How to Make Walking Animations for Pixel Art Characters | video | yes | 3 walk-cycle methods: template / animate-with-skeleton / animation-to-animation |

---

## (b) Synthesis — NEW features / model updates / workflows in this window

### NEW MODELS & TOOLS (the big releases)
1. **Animate with Text V3** ("animate with text new") — released ~2026-03-18. Next-gen text->animation: image reference + prompt. Key wins: **much cheaper than the pro model** (V3 = ~1-8 generations depending on sprite size; pro = ~20-40), and it allows **more frames even at large canvases** (e.g. 8 frames at 256x256, up to 16 at 64-128). This is now the channel's *default recommended* animation tool ("cheaper, better, more frames" — repeated in nearly every later video).
2. **Interpolate (new)** — released ~2026-03-25. Give a **first frame + last frame**, the model fills the in-between motion. Available on web, Aseprite, AND Pixelorama. Uses: pose transitions (lay-down -> stand-up), transformations (car -> robot), and **seamless loops** (set the same frame as first AND last to force a perfect walk/run/portal loop). Frame budgets like V3 (16 @ 64-128, ~8 @ 256).
3. **Character States** — released ~2026-05-16. Inside the character creator you can now `create state`: generate the character in a *specific pose first* (mid-walk, fighting, eating, laying down, on-fire), THEN animate FROM that pose. Solves the "every animation forced to start from neutral idle" problem -> cleaner walk/attack starts. States also double as **variants** (alt outfits, damaged/cracked, powered-up, seasonal, NPC variants) and as **interpolation endpoints**. Works on humanoids, animals, and large bosses (tested on a 176px boss).
4. **Object Creator page** — released ~2026-04-27. Dedicated page (separate from characters/maps): prompt single objects OR whole **asset packs**; size slider; style references from gallery or dragged-in; top-down OR side-scroller perspective; per-item description list; **8-direction rotated objects** (e.g. a throne); **object states** (e.g. clean->dirty, normal->cracked); and **object animations** (animate pro or V3).
5. **Tileset creator PRO update** — promoted ~2026-04-02. Pro mode: describe **lower terrain + upper terrain** separately, plus **walls and transitions** with definable height. Generates a full tileset in seconds; supports flat tilesets and dramatic wall/transition sets (lava->volcanic rock, water->sand, grass->cobblestone).
6. **Rotate to 8 directions** + **Rotation Pro** — the older single-rotation tool is now joined by a one-shot "give me a south sprite, get all 8 directions" tool and a smarter pro variant. A **live-pushed "create character" button on the rotation page** now auto-imports all 8 frames + estimates the skeleton -> instant animate (shown live in the 2026-05-10 video).
7. **Pixelorama browser editor integration** — featured ~2026-06-01. The `editor` tab opens Pixelorama (web Aseprite-alternative) with the *entire* PixelLab toolset docked on the left: create-image small-to-XL (+pro), create-from-style-reference, rotation (3 variants), edit image (+pro), inpaint V3, animate-with-text/interpolate, reduce-colors. Desktop + logged-in only, web-only (not the desktop Pixelorama app). Means a no-app, all-in-browser pipeline.
8. **Pixel correction tool** — referenced in scene/jam videos. A cleanup pass that denoises generated art and gently reduces palette noise at adjustable strength (2-5), preserving texture while making output more cohesive. Plus a **reduce-colors / quantize** tool that takes a target color count OR your own palette and processes all animation frames.

### KEY WORKFLOW PATTERNS (repeated across videos = current canonical advice)
- **Style-reference loop**: generate base -> pick the good ones -> feed them BACK as style references -> regenerate. Each pass tightens consistency. Used for casts, loot rarity tiers, card suits.
- **create-from-style-reference pro** is THE consistency tool: one base sprite -> a full cohesive cast / item set in one prompt. You can stack MULTIPLE style images for more context.
- **Clean BEFORE you animate, clean BEFORE you mirror.** Repeated in every video: open in Pixelorama/Aseprite, fix stray pixels, then copy the clean head/face/backpack across animation frames (the face/eyes drift the most). Mirroring a dirty animation propagates the dirt.
- **Mirror symmetric directions** (SE/E/NE -> SW/W/NW) to halve generation cost. Only mirror if the design is symmetric (no one-sided sword/strap/sleeve).
- **Mid-stride start-frame trick** (from their Discord): for natural walk/run, don't start from a standing pose — generate once, grab the middle (legs-spread) frame, and re-run the animation using THAT as the start frame. Cleaner, less "laggy start." States now formalize this.
- **First-frame-on for loops** (walk/run -> keeps loop seamless), **first-frame-off for one-shots** (attacks). Note: "6 frames + first frame kept = 7 total."
- **Use an LLM (Gemini/Claude/ChatGPT) to write the animation prompt** from the character image — descriptive motion prompts beat one-word prompts like "attack." (Strip irrelevant LLM noise like "60-90 frames @ 12fps.")
- **Padding before animating**: bump canvas (e.g. 64->80, or to 128) so secondary motion (cloak, hair, spell) has room.
- **Custom size with narrow width / tall height** (e.g. 32x44) makes the model focus on the character shape instead of wasting square canvas.
- **Watch the canvas-size = scale trap**: dragging a sprite on an oversized canvas as a style ref makes the next char scale up. Shrink the canvas to the true sprite size (e.g. 32x32) before using as reference.
- **Scene/parallax via inpaint V3**: place character -> inpaint the inverse selection -> generate a tiling background -> then edit-image-pro "leave just the trees / mountains / sky, keep composition" to split parallax layers that still tile.
- **Tileset polish trick**: generate tileset -> transform to layout (Wang/15-tile) FIRST -> edit-image-pro "make it prettier, keep the same composition" -> re-import. Big quality jump from a one-line prompt.
- **Inpaint-in-context for scale** (2026-06-12 short): generating a new asset *next to* an existing in-scene sprite gives the model a built-in scale reference.

---

## (c) RIMA Actionable — what our PixelLab MCP pipeline should adopt / watch

RIMA uses the PixelLab MCP heavily (`create_character`, `animate_character`, `create_topdown_tileset`, `create_sidescroller_tileset`, `create_map_object`, `create_tiles_pro`, `create_object_state`, `create_character_state`, style-reference tools). Mapping the above to our pipeline:

1. **Adopt Character States as the default animation entry point.** The MCP exposes `create_character_state` — we are likely under-using it. For RIMA's mobs/bosses (spire_choirling, shard_walker, penitent_bruiser, hollow_hulk, etc.) generate a **mid-walk / fighting / cast / hit / death** STATE first, then animate from it. This directly fixes "idle-start" jank on attack and locomotion. Bonus: states give us **damaged/cracked/powered-up enemy variants** and **boss phase looks** for free (tested up to 176px bosses) — relevant to our signature/Echo/boss bespoke pillar.

2. **Switch animation generations to V3, not pro, by default.** V3 is ~3-5x cheaper in PixelLab credits and supports more frames at our sizes. For routine RIMA sprite work this is a direct credit-burn reduction on every `animate_character` call. Reserve pro only where V3 visibly fails (e.g. heavy crumble/impact, per the 2026-05-12 short). Note: there's a PixelLab balance/credit cost — relevant since we autodelete-download map objects in 8h anyway.

3. **Use Interpolate for transitions and seamless loops** (likely `select_object_frames` / animate with start+end on the MCP). Two RIMA wins: (a) **seamless run/walk loops** by setting the same frame as first+last — kills the one-frame hitch we'd otherwise clean by hand; (b) **transition states** (idle->cast, ground->stand, transform) for bosses without hand-animating betweens.

4. **Make create-from-style-reference the backbone of RIMA cast consistency.** We already have a fixed Act 1 visual canon (slate #3A3D42, void #3A1A4A, ember #E89020, cyan <=15%). Pipe ONE canon-correct anchor sprite (or a small set) as the style reference for every new mob/NPC so the whole Shattered Keep cast stays cohesive — and run the **style-ref feedback loop** (regenerate using the approved outputs as the new refs). The "loot rarity tiers from one item" trick (2026-04-24) maps cleanly to RIMA relics/skill-icon variants.

5. **Tighten our cleanup + scale discipline (no new tooling needed, but enforce it).**
   - Always **clean before animate / before mirror**; copy the clean face/eyes across frames (drift is the #1 artifact). We already have the `pixel-cleanup` and `pixelify` skills + the project rule "PixelLab decals always transparent / alpha-verify" — keep cleanup in the loop.
   - **Mirror SE/E/NE -> SW/W/NW** on symmetric mobs to halve MCP generations (top-down RIMA characters are mostly symmetric).
   - **Beware the canvas-size scale trap**: when using an existing RIMA sprite as a style ref, set the canvas to the true sprite size first or new mobs come out oversized. This matches our known PPU32 discipline.
   - **Pad the canvas** before animating cloaks/spells (matches our VFX "engine juice" approach in `SkillVfx.cs`).

6. **Tilesets: use PRO mode with explicit lower/upper terrain + transitions.** Our `create_topdown_tileset` / `create_sidescroller_tileset` MCP calls should specify terrain pair + wall/transition height, then run the **transform-to-layout-then-edit-pro-"make prettier, keep composition"** polish pass. This is a cheap quality lift for Shattered Keep floors/walls and ties into our existing prop/Poisson placement system.

7. **WATCH (don't necessarily adopt now):**
   - **Object Creator packs + object states/animations** — good fit for RIMA destructibles/props (the 2026-05-12 short literally shows interactable grass/statue/chest with states + V3). Could replace bespoke prop animation work; revisit post-demo when the prop system gets wired.
   - **Inpaint-in-context for scale** — useful if we ever generate props directly into a RIMA scene screenshot rather than in isolation.
   - **LLM-written animation prompts** — we already have Claude/Gemini in the loop; we can auto-generate richer `animate_*` prompts from the mob's design notes instead of one-word actions.
   - **Pixel-correction / quantize-to-palette** — a palette-lock pass to force generated mobs onto the Act 1 canon palette before Unity import (we already enforce palette canon manually).

8. **Demo note:** none of these change the 9/9 demo systems; this is asset-pipeline efficiency + consistency for *post-demo* content production. Lowest-risk, highest-value immediate adoptions: **(1) States-first animation, (2) V3-by-default, (3) style-ref anchor for cast consistency.**
