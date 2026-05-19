# RESEARCH DONE — Free Asset Survey + RIMA Tile Angle Solutions

**Model executed:** gemini-3.1-pro-preview (default, no override needed — rate limited, retried, completed successfully)
**Research date:** 2026-05-19
**Hard rules confirmed:** No PixelLab gen, no Branch A re-recommendation.

---

# VERDICT

The single highest-ROI action is the **Transform Squash (Y scale = 0.819)**: stop prompting AI generators for "35° perspective tiles" — they produce sprite-frame baked perspective, not ground-plane foreshortening. Instead, generate or download completely flat 90° top-down textures, then scale the Tilemap parent's Y to `cos(35°) = 0.819` in Unity. The floor will optically match 35° characters instantly with zero shader work and zero new art generation tonight. Second-best is the **ComfyUI + Tilesetter pipeline** for long-term scalability: headless SDXL flat texture generation (autonomous, RTX 5080 ready) piped into Tilesetter's 47-tile Wang assembler ($25, one-time). Third is downloading **32rogues** (CC0, 32px, itch.io) tonight as a control set to test the squash math before committing to any new generation workflow.

---

# 1. Free Asset Survey

| Name / Author | URL | License | Px Size | Style Match (1-5) | Commercial OK? | Notes |
|:---|:---|:---|:---|:---|:---|:---|
| **Free Pixelart Topdown** by bluexel | https://bluexel.itch.io/free-pixelart-topdown-tileset (verify link) | CC0 / Free | 32px | 3/5 | Yes | Bright palette, needs color correction for RIMA dark tone |
| **32rogues** by Seth | https://seth.itch.io/32rogues (verify link) | CC0 / Free | 32px | 4/5 | Yes | Chunky dungeon style, good control test candidate |
| **Basic Caves & Dungeons** by Meaghan | https://meaghan.itch.io/basic-caves-dungeons-tileset-32x32-pixels (verify link) | Free | 32px | 4/5 | Yes | Dark palette, close to RIMA granite/cobble mood |
| **LPC Base Assets** (OpenGameArt) | https://opengameart.org/content/lpc-base-assets | CC-BY-SA 3.0 | 32px | 2/5 | Yes (with credit) | Too bright/generic RPG look, wrong mood |
| **Fantasy Dungeon** by AL_Core | https://al-core.itch.io/fantasy-dungeon-tileset (verify link) | Paid (~$3) | 32px | 5/5 | Yes | Best style match reported — dark stone, organic edges |
| **Kenney Dungeon Pack** | https://kenney.nl/assets/dungeon-pack | CC0 | 16px/32px | 2/5 | Yes | Clean but too sterile/bright for RIMA's painterly tone |
| **Kenney Tiny Town** | https://kenney.nl/assets/tiny-town | CC0 | 16px | 1/5 | Yes | Wrong genre entirely — use only for pipeline testing |

**RIMA compatibility notes:**
- Best immediate candidate: **32rogues** (CC0, 32px, chunky, free, no attribution required)
- Best style match: **Fantasy Dungeon by AL_Core** (~$3)
- All links flagged "verify link" — Gemini knowledge-based, not live-fetched. Confirm on itch.io before download.
- For dark granite/cobble + organic: search itch.io directly with "32px dungeon tileset dark stone" + filter by "CC0" or "Free"

---

# 2. Alternative Generation Tools

| Tool | Seamless Result? | Quality at 32px Chunky | Time / Cost | Autonomous (agent-driveable)? | RIMA Verdict |
|:---|:---|:---|:---|:---|:---|
| **gpt-image-1 (DALL-E 3 via Codex sub)** | No — manual cleanup required | Poor — refuses strict grid/palette limits | ~10s / included in Codex sub | No — prompts need constant human tweaking | LOW ROI. Same root problem as PixelLab: bakes perspective into output. |
| **Local SDXL + ControlNet Tile mode** | YES — via "Asymmetric Tiling" extension or ComfyUI seamless node | High — use Pixel Art XL LoRA, gen at 512, downscale nearest-neighbor | ~3s / Free (RTX 5080) | YES — ComfyUI JSON API can be triggered headlessly by Claude/Codex | HIGHEST ROI for long-term |
| **FLUX dev/schnell local** | No native seamless flag yet | Very high prompt adherence, prefers 1024px — needs post-process | ~6s / Free (local) | Yes (via ComfyUI API) | MEDIUM ROI — good quality, needs seamless node added |
| **Tilesetter ($25 GUI)** | Perfect borders — it is a mathematical assembler | N/A (not a generator — it takes 1 flat texture and builds a 47-tile Wang set) | 1-2 min / $25 one-time | No — GUI only, human clicks required | HIGH ROI as stage 2: pair with ComfyUI for full pipeline |
| **ComfyUI Pixel Art workflow** | Yes with "Make Seamless" custom node | Excellent when combined with SDXL + LoRA | ~3-5s / Free | YES — full headless JSON API | Best autonomous option on RTX 5080 |

**Key insight from Gemini:** The root problem is that all AI generators (PixelLab, DALL-E, gpt-image-1) naturally bake perspective into objects. Prompting for "35° floor tile" gets you an object floating in space viewed from 35°, with visible edges/thickness — ruining seamless tiling. The fix: prompt for "straight top-down satellite view, perfectly flat, no perspective" and apply perspective optically in Unity via Transform.Scale.

---

# 3. Camera Architectural Alternatives

| Approach | Feasibility in URP 2D | Complexity (1=Trivial, 5=Major Rewrite) | Visual Gain (1-5) | Notes |
|:---|:---|:---|:---|:---|
| **A. Full 3D camera (HD-2D / Octopath)** | Low — requires switching to 3D Lit pipelines for environment | 5 | 5 | Real shadows, real depth. Not viable without full pipeline rewrite. |
| **B. Multi-camera layers (char cam + floor cam)** | Medium — Camera Stacking supported in URP | 3 | 3 | Can cause wall clipping artifacts. Complex Z-ordering. |
| **C. 2.5D Billboard sprites (Gungeon method)** | High — floor on XZ plane, orthographic camera angled | 4 — shifts physics to 3D XZ | 5 | Architecturally correct and used in production. Requires physics overhaul. |
| **D. Transform Squash / Y-scale on Tilemap parent** | Perfect — flat orthographic camera stays unchanged | 1 | 4 | Scale Tilemap parent Y to 0.819 (cos 35°). Characters at scale 1. Zero URP changes. **RECOMMENDED TONIGHT.** |
| **E. Shader/Vertex warp on floor tiles only** | Medium — custom URP shader or vertex displacement | 3 | 4 | Elegant but adds shader complexity. Transform squash achieves same result trivially. |
| **F. Y-sort with angled sprites on flat floor** | High — supported natively by Unity Tilemap | 2 | 3 | Works for walls/props, but floor tile mismatch persists. Doesn't solve root problem. |

**Winner for RIMA immediately: Approach D (Transform Squash)**
- No pipeline changes
- No new assets tonight
- Mathematically exact: Y scale = cos(35°) = 0.819
- Characters stay at normal scale
- Works with existing URP 2D Pixel Perfect setup

---

# 4. Hybrid Combinations (Ranked by ROI)

**Rank 1 — Transform Squash + Free Asset Test (Under 2 hours, tonight)**
Download 32rogues (CC0, 32px, free, no credit required). In Unity, create empty GameObject parent, set Transform.Scale.Y = 0.819. Put Tilemap inside. Place Warblade character (unscaled) over floor. Walk around and visually validate the perspective lock. If it reads correctly, this is the floor solution — no new art generation needed. Time: ~45 min including download and Unity setup.

**Rank 2 — ComfyUI Flat Texture + Tilesetter Wang Assembly (Scalable to 50+ rooms)**
Boot ComfyUI on RTX 5080. Use SDXL + Pixel Art XL LoRA + Make Seamless node. Prompt: "straight top-down satellite view of cobblestone, perfectly flat, no perspective, dark granite, pixel art style." Output 512x512. Drop into Tilesetter ($25) — it auto-generates the full 47-tile Wang set with perfect borders. Import to Unity, apply Y-squash. Autonomous: Claude/Codex can drive ComfyUI via JSON API headlessly. Human clicks required only in Tilesetter (one-time per material). Time: ~3-4 hours setup, then ~15 min per new material.

**Rank 3 — Enter the Gungeon Architecture (Full Engineering Overhaul, deferred)**
Switch world to 3D coordinates. Floor on XZ plane. Camera orthographic, tilted 35° on X-axis. Sprites billboard-face camera, Y-stretched by 1/cos(35°) = 1.22. This is architecturally "correct" and what Gungeon does. Zero floor perspective problems ever. Cost: 1-2 weeks engineering. Not tonight. Log as Branch F candidate.

---

# 5. Reference Games Analysis — What They Actually Do

**Hades (Supergiant Games)**
Does NOT tilt the camera. Uses 3D models exported as 2D sprite sheets. The environment is purely 2D hand-painted art. Perspective is permanently painted into the art by human artists — not computed. Floor has no mechanical angle; the illusion comes from painted depth cues, wall sprites with visible front+top faces, and character proportions. Takeaway: Hades "cheats" with expensive hand art. Not replicable via AI tile gen. The Hades Floor De-emphasis approach (Branch D) is correct — keep floor minimal/dark so the illusion reads from walls+chars.

**Enter the Gungeon (Dodge Roll)**
Entirely 3D physics and world. Camera is orthographic, angled exactly 45°. Floor uses standard 3D planes. Characters are 2D sprites set to billboard mode (always face camera) and mathematically Y-stretched so they look normal through the angled lens. Takeaway: Gungeon's solution is architecturally clean but requires full 3D physics — a rewrite for RIMA.

**Stardew Valley / Zelda LTTP**
Camera is perfectly flat (90° straight down). Floor tiles are completely flat with zero perspective. The illusion of 3/4 angle is created entirely by walls (front face + top face sprites) and character animation design. Takeaway: The flatness of the floor is the key — the "angle" is an illusion from character art, not tile perspective. This is the closest to RIMA's viable path.

**Dead Cells (Motion Twin)**
Sidescroller — not directly comparable. Uses 2D billboards on a 2D plane. Not applicable.

**Hotline Miami**
Flat top-down (90°), no angle at all. Characters match. No mismatch problem.

**Bottom line for RIMA:** The games that work (Hades, Gungeon, Stardew) all solve the floor problem differently — either with hand art, 3D geometry, or by making the floor irrelevant via wall illusion. The Transform Squash approach is closest to what Stardew does: flat tiles that appear angled because of surrounding context + character design.

---

# 6. Concrete Morning Action Items (Ranked, Under 2 Hours Each)

**Action 1 — The Scale Hack Test (45 min)**
1. Open RoomPipelineTest.unity
2. Create empty GameObject "FloorParent", set Transform.Scale.Y = 0.819
3. Move existing floor Tilemap inside FloorParent
4. Place Warblade character sprite over floor (unscaled, normal Y=1)
5. Look at in Scene view and Game view — does perspective read correctly?
6. If yes: this is the floor solution. Commit. Move on to room dressing.
7. If no: note what's still wrong (probably wall tiles need separate treatment)

**Action 2 — Download 32rogues Control Set (15 min)**
- URL: https://seth.itch.io/32rogues (verify before download)
- CC0 — no credit required, commercial use free
- Import to Unity as 32px sprites, use as Tilemap tiles under FloorParent
- Baseline: does human-made flat tile look correct under Y=0.819 squash?
- This confirms the math before spending time on ComfyUI

**Action 3 — ComfyUI Flat Texture Workflow Setup (2 hours)**
- Boot ComfyUI (already installed per system specs / RTX 5080)
- Install "Make Seamless" custom node if not present
- Load SDXL base checkpoint + any Pixel Art LoRA from CivitAI
- Build workflow: SDXL txt2img → Make Seamless → save PNG
- Prompt: "straight top-down satellite view of dark cobblestone floor, perfectly flat, no perspective, no lighting angle, pixel art style, dark grey, cool tones"
- Output at 512x512, then nearest-neighbor downscale to 128x128 or 64x64 in Unity
- Validate: does it tile? Does it look flat under Y=0.819 squash?

---

# GAPS / CONFIDENCE

**CONFIDENCE: MEDIUM**
- Reason: Gemini answered from model knowledge (cutoff), not live web fetch. Asset pack links are best-guess and flagged "verify link." The Transform Squash math (cos 35° = 0.819) is mathematically certain. The game analysis (Hades/Gungeon architecture) is well-documented in public dev talks and is HIGH confidence. ComfyUI/SDXL approach is HIGH confidence based on known community workflows.
- Gaps: Gemini did not cite live URLs — all itch.io links need manual verification. ComfyUI "Make Seamless" node name may differ (search for "seamless tile" in ComfyUI Manager). SDXL Pixel Art LoRA — specific recommended model name not cited (search CivitAI for "pixel art tileset LoRA").
- Not answered: Does Unity Pixel Perfect camera interact badly with non-uniform Y scale? (Needs runtime test — part of Action 1.)
