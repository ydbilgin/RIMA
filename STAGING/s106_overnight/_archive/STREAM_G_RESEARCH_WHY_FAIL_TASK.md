ACTIVE RULES: (1) cite sources (2) concrete examples — names of games/tools/devs (3) brutally honest about our mistakes (4) BLOCKED if Phase 0 incomplete.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

**ANTIGRAVITY CRITICAL:** Respond INLINE only. Do NOT use file-write tools. ConPTY captures stdout.

Amaç: USER directly asked (2026-05-25): "antigravity'e sorar mısın neden yapamıyoruz hatamız ne insanlar nasıl yapıyolar bu odaları". 5 oda denemesi yaptık (scene.png placeholder + scene_v2.png real assets), hala chatgpt_ref kalitesinden çok uzak. Sen industry research yap, gerçek dev/studio'lar bu tarz modular ARPG dungeon room'larını nasıl yapıyor, biz neyi yanlış yapıyoruz.

---

# RESEARCH — Why We're Failing + How Pros Do It

## Phase 0 — INTERNALIZE chatgpt_ref + OUR FAILURES (mandatory, 300-500 words at top of response)

Before researching, you MUST first look at BOTH our failures and the targets.

### Open and study these targets (what we're trying to achieve):
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (4).png`
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png` (asset groups manual)
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_10 (5).png` (open-front flooded)

### Open and study OUR failures (what we produced):
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/ritual_diamond/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/boss_arena/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/flooded_crypt/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/library_alcove/scene_v2.png`

### Write 300-500 words (must appear at top of your response):
1. **Target intent in your own words** — what does chatgpt_ref show? Style, mood, technical approach.
2. **Our specific failures** — what's broken in scene_v2.png? Compare pixel-level.
3. **The gap** — describe the visual/technical distance between target and our output.

---

## Phase 1 — Diagnose technical root cause (your reasoning, no web search needed yet)

Based on the visual evidence, what's broken in our pipeline? Examples to consider:
- Sprite import settings (PPU? pixel-snap? filtering?)
- Footprint vs sprite size mismatch
- Tilemap floor not rendering (sort layer / material / URP 2D Renderer config?)
- Wall placement logic (gaps between pieces = scale issue or chain logic?)
- Lighting setup (URP 2D Lights not active? wrong shader?)
- Camera/perspective (ortho size? sort axis? PPU camera component?)

List your top 5 technical root cause hypotheses, ranked by likelihood. Be concrete — cite specific files/values where you can.

---

## Phase 2 — INDUSTRY RESEARCH (use web search heavily)

Find concrete examples of how OTHER teams have solved this exact problem. Not theory — actual case studies.

### Q1. Indie ARPG / Hack-and-slash devs with similar visual style
Find 5-10 indie studios making top-down/3-quarter pixel-art ARPG/dungeon games. For EACH:
- Studio + game name + year
- How they build rooms (handcrafted, procedural, hybrid?)
- What tools they use (Unity Tilemap? Tiled? custom editor? Pyxel? Aseprite?)
- Any GDC talks, dev blogs, postmortems, twitter threads where they explain their pipeline

Examples to investigate: Children of Morta (Dead Mage), Death's Door (Acid Nerve), Eitr (Eneme Entertainment), Wytchwood, Hyper Light Drifter (Heart Machine), Death's Gambit, Tunic (Andrew Shouldice).

### Q2. Modular pixel-art wall composition — the EXACT same problem we have
Search: "modular pixel art walls unity", "tile-based room composition top-down", "wall socket connector pixel art", "dungeon kit modular pixel".

Find:
- Unity Asset Store packs that ship modular wall kits with builder tools (e.g. Brackeys-style)
- Itch.io / OpenGameArt asset packs with documentation on how they're used
- GitHub repos of 2D dungeon room editors (open source)
- YouTube tutorials demonstrating the pipeline (cite specific videos)

### Q3. The "blueprint paint → auto wall generate" workflow specifically
Our user wants: paint walkable cells, system auto-derives wall chain. Find:
- Games that use this exact workflow (Eternal Lands? Roguelike Vault? Project Wingman? Brackeys' free tools?)
- Open source Unity editor windows demonstrating this
- Algorithm references (marching squares? Wang autotile? 4-bit NSEW bitmask?)
- Specific code examples or videos

### Q4. Sprite import + PPU best practices for 64×64 / 128×128 / variable-size pixel art
Our sprites are 256×192 (2-cell) and 128×128 (1-cell). PPU mismatch is suspected cause of scale bug.

Find:
- Unity official guidance for variable-size pixel sprite import
- "Pixel Perfect Camera" + PPU best practices in 2024-2026
- Common mistakes from dev posts / GDC / Twitter / Reddit
- How does Hyper Light Drifter / Death's Door / Children of Morta handle this?

### Q5. Floor tilemap + wall prefab co-existence
Floor on Tilemap + walls as GameObject prefabs is our hybrid approach. Find:
- Game devs who explicitly use this pattern (vs full tilemap or full prefab)
- Common pitfalls (sort layer collision, z-fighting, sprite seam visibility)
- Unity Asset Store examples of this hybrid

### Q6. What Unity Asset Store tools could we BUY/STUDY for inspiration?
Find top 5 Asset Store packs (paid or free) that demo the workflow we want. Specifically:
- DunGen, Edgar, Procedural Dungeon Toolkit?
- 2D Tile Map Editor / Tile-based World Editor?
- Pixel-art specific packs with editor windows?

For each: link to Asset Store page, key features, what we could learn from the source code if we bought it.

---

## Phase 3 — Brutal honest verdict

Based on Phase 1 (our root causes) + Phase 2 (industry methods):

### Are we doing something fundamentally wrong?
- Architecture choice (prefab-stamp vs tilemap vs hybrid)
- Sprite asset prep
- Builder algorithm design
- Visual asset quality (the sprites themselves vs what pros use)

### What's the FASTEST PATH to acceptable chatgpt_ref quality?
- Should we abandon current approach and use Unity Asset Store pack X?
- Should we re-import sprites with different settings?
- Should we redesign the WallChainRoomBuilder algorithm?
- Should we change visual aesthetic target (chatgpt_ref might be unattainable with current asset pack)?

### Specific actionable steps for our user (RIMA project)
Rank top 5 by impact/effort:
1. ...
2. ...

---

## Output format (INLINE only)

```markdown
# Research — Why Are We Failing — Antigravity — 2026-05-25 <time>

## Phase 0 — Intent + Failure analysis (300-500 words)
<your description>

## Phase 1 — Technical root cause hypotheses (top 5)
1. ...
2. ...

## Phase 2 — Industry research findings

### Q1. Indie ARPG devs
<list with citations>

### Q2. Modular wall composition
<findings + sources>

### Q3. Blueprint → auto-wall workflow
<findings>

### Q4. Sprite PPU + import best practices
<findings>

### Q5. Floor tilemap + wall prefab hybrid
<findings>

### Q6. Asset Store tools to study
<5 packs with links>

## Phase 3 — Brutal verdict + recommendations
<2-3 paragraphs of harsh truth>

### Top 5 actionable steps (ranked by impact/effort)
1. ...

## Sources cited (at least 15 specific URLs/refs)
- ...

## Files opened
- ...
```

## Constraints
- ❌ Do NOT regurgitate the previous research report (`agy_research_response.md`) — that was generic. THIS is targeted at WHY we failed + how pros succeed.
- ✅ Use web search heavily — fetch specific GDC talks, dev blogs, Twitter threads
- ✅ Cite at least 15 specific sources
- ✅ Be BRUTALLY honest — if our approach is wrong, say so. If our sprites are bad, say so.

## Estimated time: 20-30 min
