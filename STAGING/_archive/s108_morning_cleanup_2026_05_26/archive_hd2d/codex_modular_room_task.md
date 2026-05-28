# Modular Room System — Phase 1 (Textures + Prefabs + Sample Composition)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

---

## Goal

Set up a **modular HD-2D dungeon room system** where walls and floor are composed of textured pieces (girinti/çıkıntı = recess/protrusion) instead of giant flat cubes. User wants to compose many varied rooms from these pieces. Reference: `STAGING/concepts/chatgpt_ref/` (ChatGPT mock screenshots — stone keep dungeon, dramatic lighting, modular walls with pillars/buttresses/niches, cracked floor tiles).

Current state: `SampleScene` has 1 huge Cube for Wall_North + 1 for Wall_West + 1 Plane Floor. Player + 4 mobs already placed. Camera at (12, 8, -12), rotation (30, 315, 0), orthographic size 9. Z-sort already configured to Custom Axis (0,1,0).

You will REPLACE the flat walls/floor with **modular textured pieces** built from primitives + generated textures.

---

## STEP 0 — Required reads (read before starting)

1. `CLAUDE.md` (project rules)
2. `CODEX_DISPATCH.md` (codex rules)
3. `CURRENT_STATUS.md` (S103 state)
4. `STAGING/concepts/chatgpt_ref/` — list 8 PNG files, pick 2 reference visual targets
5. `MEMORY/INDEX.md` — only if texture/material decisions need clarity

---

## STEP 1 — Generate 6 textures via gpt-image-1 imagegen

Use `image_gen.imagegen` tool. Output paths under `Assets/Art/Environment/`:

### Wall textures (256w x 512h, tileable horizontally)

**Output:** `Assets/Art/Environment/Walls/stone_wall_a.png`
**Prompt:** "Top-down ARPG perspective pixel art texture of a weathered stone dungeon wall section, tileable horizontally, 256x512 resolution. Dark slate stone (#2A2D34 / #3A3D48 / #1A1C20) with cold grey grout lines, irregular stone block masonry, moss and dampness patches at base, faint crack details. Hard pixel edges minimum 4px clusters, no anti-aliasing, no smooth gradients. Style reference: Octopath Traveler HD-2D dungeon walls, Hyper Light Drifter, Eastward. Cinematic flat texture suitable for 3D wall mesh in Unity URP Lit material."

**Output:** `Assets/Art/Environment/Walls/stone_wall_b_cracked.png`
**Prompt:** Same as above but with prominent cracks and damaged stones — chunks missing, exposed brick interior, more weathering.

**Output:** `Assets/Art/Environment/Walls/stone_wall_c_pillar.png`
**Prompt:** Same palette but designed as a vertical pillar segment — narrow 128x512 with a fluted/carved column shape, Norse-inspired runic accent at midline, dark slate base.

**Output:** `Assets/Art/Environment/Walls/stone_wall_d_niche.png`
**Prompt:** Same palette, 256x512, with a dark recessed arched niche in the center (alcove for statue/torch), surrounding masonry intact, niche interior fully black/very dark.

### Floor textures (256x256, tileable both axes)

**Output:** `Assets/Art/Environment/Floor/stone_floor_a.png`
**Prompt:** "Top-down pixel art texture of cracked stone dungeon floor tile, 256x256 tileable both axes. Cool slate tile palette (#3A3D48 / #4E5260 / #5A5E6E) with darker grout (#2A2D34), large irregular flagstones with hairline cracks, faint dust and wear marks. Hard pixel edges, 4px minimum clusters, no anti-aliasing. Style: Octopath Traveler HD-2D dungeon floor, dark slate base."

**Output:** `Assets/Art/Environment/Floor/stone_floor_b_sigil.png`
**Prompt:** Same as floor_a but with a faint carved circular sigil/rune ring etched into one tile — subtle cold blue glow (#7BA7BC) in the runes, hint of ritual magic.

### Import settings (after generation)

For each PNG: Texture Type = Default, Filter Mode = Point, sRGB = ON, Max Size = 512, Wrap Mode = Repeat. Use `AssetDatabase.ImportAsset` with TextureImporter settings, or set defaults via PIPELINE if available.

---

## STEP 2 — Create 6 materials (URP Lit)

For each texture create a URP Lit material:

- `Assets/Materials/Environment/WallMat_StoneA.mat` → `stone_wall_a.png`
- `Assets/Materials/Environment/WallMat_StoneB.mat` → `stone_wall_b_cracked.png`
- `Assets/Materials/Environment/WallMat_PillarC.mat` → `stone_wall_c_pillar.png`
- `Assets/Materials/Environment/WallMat_NicheD.mat` → `stone_wall_d_niche.png`
- `Assets/Materials/Environment/FloorMat_StoneA.mat` → `stone_floor_a.png` (Tiling 4x4 — repeat 4 times per face)
- `Assets/Materials/Environment/FloorMat_SigilB.mat` → `stone_floor_b_sigil.png` (Tiling 1x1)

URP Lit shader path: `Universal Render Pipeline/Lit`. Set Base Map to texture, Smoothness = 0.2, Metallic = 0.

---

## STEP 3 — Create 5 modular prefabs

Use primitive Cube/Plane + materials. Save under `Assets/Prefabs/Environment/`:

1. **WallSegment_Straight.prefab** — Cube primitive, scale (2, 4, 0.5), material WallMat_StoneA, BoxCollider (auto). Pivot bottom-center: child Transform with offset (0, 2, 0) inside an empty parent so the Y=0 of the prefab is the floor.
2. **WallSegment_Cracked.prefab** — Same as above but material WallMat_StoneB.
3. **WallSegment_Niche.prefab** — Same shape but material WallMat_NicheD. (Optional: child empty GO "NicheAnchor" at (0, 2, -0.4) for placing torches later.)
4. **PillarSegment.prefab** — Cube primitive, scale (1, 5, 1), material WallMat_PillarC. Position-agnostic (pivot bottom-center via parent-child setup). Slightly taller than wall (5 vs 4) to create varied skyline.
5. **FloorTile_2x2.prefab** — Plane primitive scaled to (0.2, 1, 0.2) so it covers 2x2 world units (Unity Plane is 10x10 default). Material FloorMat_StoneA.
6. **FloorTile_Sigil.prefab** — Same shape but FloorMat_SigilB.

---

## STEP 4 — Compose Room_Sample in SampleScene

Replace existing flat walls + floor:

1. **Backup current scene:** Save SampleScene as-is first via Ctrl+S equivalent (`EditorSceneManager.SaveScene`).
2. **Delete existing:** `Floor`, `Wall_North`, `Wall_West` GameObjects in SampleScene. KEEP: Main Camera, Directional Light, Global Volume, Player, Mob_Zombie, Mob_Skeleton, Mob_Bat, Boss_Zombie.
3. **Create parent:** Empty GO `Room_Sample` at (0, 0, 0).
4. **Build floor grid:** 8x8 grid of FloorTile_2x2 prefabs as children of `Room_Sample/Floor_Grid`, covering world x∈[-8, 8], z∈[-8, 8] in 2-unit steps. Replace 4 random tiles with FloorTile_Sigil for variation. Skip 2 random corner tiles to create irregular footprint (girinti at floor edges).
5. **Build north wall:** Place WallSegment prefabs along z=8, x∈[-8, 8] in 2-unit steps. Use mix: 60% Straight, 25% Cracked, 15% Niche. Insert PillarSegment at x=-6, x=0, x=+6 (3 pillars). Parent to `Room_Sample/Wall_North`.
6. **Build west wall:** Place WallSegment prefabs along x=-8, z∈[-8, 8] in 2-unit steps, rotated 90° around Y. Same 60/25/15 mix. Insert PillarSegment at z=-6, z=0, z=+6. Parent to `Room_Sample/Wall_West`.
7. **Save SampleScene.**

---

## STEP 5 — Verify

1. Run `read_console` MCP tool — must be clean (no errors, no warnings caused by these changes).
2. Take screenshot via `manage_camera` action=screenshot — save to `Assets/Screenshots/codex_modular_room_v1.png`.
3. Visually confirm: floor has texture (not flat gray), walls show stone with variations, pillars stand taller than walls, layout matches `STAGING/concepts/chatgpt_ref/` aesthetic.

---

## STEP 6 — Commit + report

Commit message:
```
[Codex] [S103 MODULAR ROOM v1] 6 textures + 6 materials + 5 prefabs + Room_Sample composition

- gpt-image-1 textures: 4 wall + 2 floor variants, HD-2D Octopath style
- URP Lit materials for each
- Modular prefabs: WallSegment x3, PillarSegment, FloorTile x2
- Room_Sample replaces flat geometry: 8x8 floor grid + N+W walls with pillars/niches
- ChatGPT_TOPDOWN ref anchored

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

Write summary to `CODEX_DONE.md`:
- STATUS: DONE / FAILED / PARTIAL
- FILES_TOUCHED: <list>
- TEXTURES_GENERATED: <8 paths if successful>
- SCREENSHOT: Assets/Screenshots/codex_modular_room_v1.png
- ISSUES: <or NONE>
- NEXT_SIGNAL: "modular_room_v1_complete"

---

## Constraints

- Total wall+floor LIVE textures: 6 (4 wall + 2 floor). No extras.
- Style: HD-2D Octopath Traveler, dark slate stone, ChatGPT_TOPDOWN anchored.
- Camera reference angle: 30° pitch, -45° yaw, orthographic. Textures viewed at this angle in-game.
- No code comments unless WHY is non-obvious.
- Do NOT modify Player / Mob_* / Boss_* / Main Camera / Directional Light / Global Volume.
- Do NOT touch any files outside Assets/Art/Environment/, Assets/Materials/Environment/, Assets/Prefabs/Environment/, Assets/Screenshots/, Assets/Scenes/SampleScene.unity.
- If imagegen tool unavailable or fails: BLOCKED — report immediately, do not fabricate placeholder textures.
- STOP after STEP 6. Do not generate animations, scripts, or RoomBuilder logic — those are future tasks.
