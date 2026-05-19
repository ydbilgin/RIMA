# Codex Task — Wall Inventory Curation (Wave A.5)

ACTIVE RULES: (1) think before deciding (2) objective verdict (3) RIMA theme criteria explicit (4) BLOCKED if asset access fails.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Curate the wall inventory at `STAGING/RIMA_WALL_INVENTORY_AND_CANON.md`. Per user directive: **"doğru ürün kalsın, mantıksız ürün kalmasın"** (keep correct products, drop nonsensical ones).

This is **Wave A.5** in the escalation chain: Sonnet inventory → **Codex curation (you)** → Opus design → Codex review → Opus final.

## Source files (READ FIRST)

1. `STAGING/RIMA_WALL_INVENTORY_AND_CANON.md` — full inventory + canon answers
2. `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` — visual anchor (theme reference)

For each PixelLab object listed in inventory, download/view the preview via:
```
mcp__pixellab__get_object(object_id="<uuid>", include_preview=true)
```

PixelLab objects to inspect (verdict each):
- `b2703abf-63a2-41e9-bc90-a3e7cf5fcdd4` (priority hero — 384x216)
- `a3f9fcf1-7858-411f-9508-63500c2281f6` (priority hero — 192x128 corner)
- `1d73e775-7024-4826-bab0-3a7600a53fdd` (priority hero — 192x128 wall+arch)
- `06338801-a660-4309-8037-e2bd6034af9b` (keep_wall_v2 tile 32x32)
- `76693f8f-92c6-4b0f-be5c-cd059fe99ce6` (keep_wall_v2 tile 32x32)
- `825ddbdd-c38c-434a-aba7-a4c0712794f8` (keep_wall_v2 tile 32x32)
- `f053b5f0-d8f8-46b4-b751-6f676d72a385` (keep_wall_v2 tile 32x32)
- `2c1ebaac-d9c7-45bb-8d7e-b7e794b3cd25` (alabaster_wall tile 32x32)
- `56cc237f-9ddb-4b2d-afb2-0d805158e3ff` (alabaster_wall tile 32x32)
- `7f603f7d-e7dd-4cde-9323-d1c293ee4ae2` (alabaster_wall tile 32x32)
- `eb3fcf85-6b7a-468e-bb35-00bfd0790916` (alabaster_wall tile 32x32)
- `ab0f5ab4-4e6a-4e94-b8ab-3d9d5e31747f` (alabaster_layered batch — single perimeter wall)

File assets to evaluate (already in Wave 4 scene):
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painterly_wall_01-12`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/wall_edge_stone.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/wall_decoration_vines.png`

## Curation criteria (RIMA theme rubric)

Score each asset on:

1. **Projection match** — 3D Hades-style 35° side-face reveal (concept ✓) vs flat top-down (concept ✗)
2. **Tone match** — Cool dark blue-gray with moss accents (concept ✓) vs warm tan brown or amber (concept ✗ unless it's an accent)
3. **Detail density** — Painterly hand-painted with stone texture variation (concept ✓) vs flat single-tone vs noisy/overdetailed
4. **Scale viability** — Can serve as a hero wall section (384×216, 192×128) or only as tile (32×32) or oversize unusable
5. **Fracturing theme support** — Has cyan/violet rift accent potential vs purely mundane stone

## Output

`STAGING/CODEX_DONE_wall_inventory_curation.md`:

```markdown
# Codex Curation Verdict — RIMA Wall Inventory

## Summary
[1 paragraph: how many KEEP / DROP / MAYBE]

## PixelLab object verdicts

| Object ID | Verdict | Reason | Action |
|---|---|---|---|
| b2703abf... | KEEP/DROP/MAYBE | [1-line] | delete_object / import_to_unity / leave |
| ... | ... | ... | ... |

## File asset verdicts

| File | Verdict | Reason | Action |
|---|---|---|---|
| painterly_wall_01 | KEEP/DROP | [1-line] | keep_in_scene / delete_file / archive |
| ... | ... | ... | ... |

## Priority recommendation for Opus

1. **HERO walls (top choice):** [list IDs/files]
2. **Backup walls (if hero gen needed):** [list]
3. **Tile band candidates (32×32, floor-wall transition):** [list]
4. **DELETE from library entirely:** [list with delete_object IDs]

## Rationale (for Opus design context)

[2-3 paragraphs explaining curation logic — what concept demanded vs what existed, why specific choices]

## Gaps remaining for Opus

[List unresolved questions Codex couldn't answer — these become Opus design decisions in Wave B]

## Cost projection

- Assets to delete from PixelLab library: X objects (no gen cost, just cleanup)
- Assets to import to Unity from PixelLab: X objects (download + import overhead, ~5 min)
- Assets to potentially regen if Opus says so: X (gen cost estimate)
```

## Hard rules

- DO call `mcp__pixellab__get_object` for EACH PixelLab ID to actually SEE the sprite — don't trust descriptions alone
- DO compare to concept image directly
- DO mark DROP only if asset genuinely fails 2+ criteria (be conservative — drop is destructive)
- DO note "delete_object" for DROP verdicts (Codex can execute these via `mcp__pixellab__delete_object` if user approves later)
- DO NOT execute delete_object yet — just RECOMMEND in the output
- DO NOT modify Unity scene
- DO NOT make design decisions (that's Opus in Wave B) — only curate inventory

## Effort

~25-30 min. ~15 mcp get_object calls + visual inspection + NLM optional + output write.