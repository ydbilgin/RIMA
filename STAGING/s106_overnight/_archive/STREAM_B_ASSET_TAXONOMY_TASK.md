ACTIVE RULES: (1) think before classifying (2) min effort, no speculation beyond visible features (3) surgical — vision read + JSON only, no file writes (4) BLOCKED if asset content unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

**CRITICAL INLINE-ONLY:** Do NOT use any file-write tool. Respond INLINE only — emit the markdown + JSON directly as your reply text. The dispatcher captures stdout via ConPTY; sandbox file writes are NOT captured.

Files allowed to read:
- `STAGING/s106_overnight/MASTER_CONTEXT.md` (mandatory first read)
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png` (ADIM 1 — asset group reference)
- `Assets/Sprites/AssetPackV3/walls/sheet_1/*.png` (8 typed pieces — baseline for group letter assignment)
- `Assets/Sprites/AssetPackV3/walls/sheet_2/*.png` (9 untyped pieces, piece_01..09)
- `Assets/Sprites/AssetPackV3/walls/sheet_3/*.png` (11 untyped pieces, cell_XX_RxCy_L/R)
- `Assets/Sprites/AssetPackV3/walls/sheet_4/*.png` (16 untyped pieces, piece_01..16 — 4×4 master)
- `Assets/ScriptableObjects/Walls/V2/*.asset` (existing WallPieceData for reference schema)

---

# STREAM B — ASSET TAXONOMY (Antigravity Vision)

## Amaç

Classify the 36 wall PNGs across `sheet_1`, `sheet_2`, `sheet_3`, `sheet_4` into the 7 asset groups A-G per blueprint_room ADIM 1 (PNG 1):
- **A. Connector / Column** — pillars, columns, support pieces, decorative posts
- **B. Rear Wall** — long straight walls, tall walls, dominant back hat
- **C. Side Wall / Step** — side walls, short spans, step pieces, L-turns
- **D. Corner / Turn** — outer/inner/L corner pieces, alcove turns
- **E. Door / Arch / Portal** — arches, gates, passages, portal frames
- **F. Low Front / Open Edge** — low walls, parapets, broken low walls, front fillers
- **G. Seam / Cleanup / Filler** — rubble, small blocks, shadow strips, seam patches

(H. Props are NOT in these sheets — they're elsewhere. Skip H for this task.)

## Procedure

1. **Read ADIM 1** image first (`STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png`). Internalize the 6 group reference samples shown.
2. **Read sheet_1 pieces** (already typed: straight, outer_corner, inner_corner, end, door_l, door_r, alcove, protrusion). Use these as VISUAL CALIBRATION — sheet_1/01_straight ≈ Group B reference, sheet_1/02_outer_corner ≈ Group D, sheet_1/05+06_door ≈ Group E, sheet_1/07_alcove + 08_protrusion ≈ Groups D/F variants.
3. **For each piece in sheet_2/3/4:**
   - Inspect the PNG visually
   - Assign primary group letter (A-G)
   - Estimate `lengthInCells` (1, 2, 3, 4, or "setpiece")
   - Estimate `direction` (rear / side_left / side_right / front / any)
   - Estimate `heightType` (low / normal / tall / setpiece)
   - Note visual offset hint (does the piece have a tall top that extends above its footprint? note offset in cells)
   - Confidence: high / medium / low
   - Brief notes (e.g., "broken section, suggests filler", "stepped variant for stairs")

## Output format (INLINE, paste as a markdown code block)

Emit a SINGLE JSON object as a markdown code-fenced block. NO file writes. Example:

```json
{
  "version": 1,
  "generated_at": "2026-05-25T03:XX:XX+03:00",
  "by": "antigravity-<account>",
  "schema_note": "Group letter A-G per blueprint_room ADIM 1. Confidence reflects visual certainty. Codex will convert these entries to WallPieceData .asset files in Stream B follow-up.",
  "sheet_1": {
    "00_validation": "Already typed — used as visual calibration only",
    "pieces": [
      {
        "file": "Assets/Sprites/AssetPackV3/walls/sheet_1/01_straight.png",
        "group": "B",
        "type_label": "rear_wall_straight",
        "lengthInCells": 1,
        "direction": "rear",
        "heightType": "normal",
        "visualOffset": "0",
        "confidence": "high",
        "notes": "baseline rear wall reference"
      }
    ]
  },
  "sheet_2": {
    "pieces": [
      {
        "file": "Assets/Sprites/AssetPackV3/walls/sheet_2/piece_01.png",
        "group": "B",
        "type_label": "tall_rear_wall_2x",
        "lengthInCells": 2,
        "direction": "rear",
        "heightType": "tall",
        "visualOffset": "1 cell up",
        "confidence": "medium",
        "notes": "elongated vertical, fits 2-cell span"
      }
    ]
  },
  "sheet_3": {
    "pieces": []
  },
  "sheet_4": {
    "pieces": []
  },
  "unknowns": [
    {
      "file": "Assets/Sprites/AssetPackV3/walls/sheet_X/piece_YY.png",
      "reason": "ambiguous — could be C or D, requires further inspection",
      "fallback_group": "G",
      "fallback_notes": "treat as filler until human review"
    }
  ],
  "group_coverage_summary": {
    "A_connector_column": { "found": 0, "files": [] },
    "B_rear_wall":        { "found": 0, "files": [] },
    "C_side_wall_step":   { "found": 0, "files": [] },
    "D_corner_turn":      { "found": 0, "files": [] },
    "E_door_arch_portal": { "found": 0, "files": [] },
    "F_low_front_open":   { "found": 0, "files": [] },
    "G_seam_cleanup_filler": { "found": 0, "files": [] }
  },
  "missing_groups": [
    "List groups where coverage is suspiciously low (e.g. < 2 pieces) and explain what art is missing"
  ],
  "blueprint_room_room_alignment": {
    "library_alcove_room_adim4": "Can current sheet_1+2+3+4 produce this room? List specific missing pieces if any.",
    "open_front_flooded_room_adim5": "Can current sheet_1+2+3+4 produce this room? List specific missing pieces if any."
  }
}
```

After the JSON, add a short markdown "Methodology notes" section (200-400 words):
- What heuristics you used
- Which pieces were hardest to classify and why
- Recommendation for Codex on which pieces should become WallPieceData .assets first (most impactful for 5 test rooms)

## Constraints
- DO NOT use UnityMCP or other tools to spawn/import sprites — only read the existing PNG files.
- DO NOT write any file. The orchestrator extracts your JSON block from your reply.
- DO NOT speculate beyond what you can see. Mark as "unknown" if uncertain.
- DO NOT spawn or modify Unity assets — Codex will do that in a follow-up dispatch.
- "Files opened" header: at end of reply list every PNG file you actually inspected.

## Estimated time: 30-60 min for 36 PNG visual inspection.

If you exceed 90 min total, emit what you have as PARTIAL with a clear "unfinished sheets" note.
