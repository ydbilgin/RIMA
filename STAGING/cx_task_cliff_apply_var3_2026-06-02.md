ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed scenes only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
The cliff-overflow gap-test (STAGING/cliff_gaptest_report.md) is reviewed. Opus locked the geometry = "VAR3" (edge-anchored with direction-aware vertical tuck), which removed all S/SE/SW overflow and tucks side edges. APPLY this locked geometry to the THREE LIVE scenes, replacing the current inconsistent cliff placement (_IsoGame was +0.85, Map02/03 were -0.10). Unity `execute_code` ONLY — NO new .cs scripts. Then screenshot _IsoGame for verification.

# LOCKED placement formula (VAR3)
Asset folder: `Assets/Sprites/Environment/CliffKit_RefB_pixelified/` (cliff_S, cliff_SE, cliff_SW, cliff_E, cliff_W, cliff_N, cliff_NE, cliff_NW — TopCenter pivot, PPU64). Ground = isometric Tilemap named "Ground", cellSize (0.96, 0.585).
FRONT void-facing dirs, priority order, with cell-index neighbor offset (dc,dr):
  S=(-1,-1), SE=(0,-1), SW=(-1,0), E=(1,-1), W=(-1,1).   (N/NE/NW back-facing, NOT used.)
For each Ground cell that has a tile:
  - scan dirs in [S,SE,SW,E,W]; first dir whose neighbor cell (cell + (dc,dr)) has NO ground tile (ground.GetTile(neighbor)==null):
      cellCenter = ground.GetCellCenterWorld(cell);
      dWorldX = (dc - dr) * 0.48;  dWorldY = (dc + dr) * 0.2925;   // iso world delta
      Dworld = new Vector3(dWorldX, dWorldY, 0);  Dhat = Dworld.normalized;
      float upY = (dWorldY < -0.01f) ? 0.30f : 0.12f;   // S/SE/SW keep 0.30 tuck-up; horizontal E/W use 0.12 so top stays under the side lip
      Vector3 pos = cellCenter + Dworld * 0.5f + (-Dhat * 0.20f) + new Vector3(0, upY, 0);
      // place sprite
      SpriteRenderer.sprite = cliff_<DIR>;
      sortingLayerName = "Floor";
      sortingOrder = -30 + Mathf.RoundToInt(20f - pos.y);
      name = "cliff_<col>_<row>_<DIR>";
      break;  // 1 cliff per cell

# Steps (per scene: _IsoGame.unity, _IsoGame_Map02.unity, _IsoGame_Map03.unity)
1. Open the scene. Find the "Ground" Tilemap. Find/create container `CliffRing` (active) and its child `CliffSprites` (active) — IMPORTANT: both GameObjects MUST be activeSelf=true (a past bug left them inactive → sprites invisible).
2. DELETE all existing children under `CliffRing/CliffSprites` (the old +0.85 / -0.10 cliffs).
3. Re-place cliffs using the LOCKED VAR3 formula above, parented under `CliffRing/CliffSprites`.
4. SAVE the scene.
5. Repeat for all 3 scenes. They MUST end with the SAME formula (equalized).
6. After all 3 saved: open `_IsoGame.unity`, take a GAME-view screenshot to `Assets/Screenshots/cliff_final_isogame.png` framed to show the whole island + edges. (Use manage_camera screenshot. If you also can, one for Map02/Map03: `cliff_final_map02.png` / `cliff_final_map03.png`.)
7. Report per scene: cliff count placed, and the overflow heuristic count (cliffs whose SpriteRenderer bounds.max.y > host cellCenter.y + 0.30 — should be LOW, near the gap-test VAR2's 61 or lower). Confirm CliffRing/CliffSprites are ACTIVE in each.

# Notes
- Do NOT touch `_IsoGame_cliffgaptest.unity` (the test copy — leave it).
- Do NOT create new .cs scripts (execute_code only, avoid recompile). No `using` directives — fully-qualify (UnityEngine.Tilemaps.Tilemap, UnityEditor.SceneManagement.EditorSceneManager, etc.).
- Do NOT commit.

# Output
Per-scene: cliff count + overflow heuristic count + container-active confirm + screenshot paths.
