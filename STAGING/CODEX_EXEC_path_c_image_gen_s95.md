# Codex Image_Gen Execution — Path C Floor + Walls (S95)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## TASK

Run 5 image_gen calls (image_gen skill) using the 5 prompts defined in `STAGING/CODEX_DISPATCH_path_c_floor_walls_s95.md`. Save outputs to `STAGING/codex_floor_walls_v01/` with exact filenames listed below. Run sequentially (one after another). Do not modify prompt text — copy verbatim from the spec file.

## OUTPUT DIRECTORY

Create if missing:
```
STAGING/codex_floor_walls_v01/
```

## 5 IMAGE_GEN CALLS

For each, copy the prompt text from the section indicated in `STAGING/CODEX_DISPATCH_path_c_floor_walls_s95.md`, then invoke the image_gen skill with that prompt and save the returned PNG to the given path.

| # | Source section in spec | Output filename | Size |
|---|---|---|---|
| 1 | "Prompt 1 — Floor Material A: Cool Granite" | `STAGING/codex_floor_walls_v01/floor_A_granite_v01.png` | 1024x1024 |
| 2 | "Prompt 2 — Floor Material B: Cracked Stone" | `STAGING/codex_floor_walls_v01/floor_B_cracked_v01.png` | 1024x1024 |
| 3 | "Prompt 3 — Floor Material C: Dirt and Rubble" | `STAGING/codex_floor_walls_v01/floor_C_dirt_v01.png` | 1024x1024 |
| 4 | "Prompt 4 — Floor Material D: Ritual Accent" | `STAGING/codex_floor_walls_v01/floor_D_ritual_v01.png` | 1024x1024 |
| 5 | "Prompt 5 — Wall Pieces Set" | `STAGING/codex_floor_walls_v01/walls_set_v01.png` | 1024x1536 |

## EXECUTION RULES

- Copy prompt text VERBATIM from the spec file (between the triple-backtick blocks). Do not paraphrase.
- For floor prompts use square aspect (1024x1024). For wall prompt use portrait 2:3 aspect (1024x1536).
- If image_gen skill returns a different aspect, request closest match and note in result.
- One call at a time. Wait for each to complete before starting next.
- If any call fails (rate limit, content filter, API error), log the error, skip that prompt, continue with the rest. Do not retry more than once per prompt.

## DONE CONDITION

When all 5 calls complete (success or logged failure):

1. Write `STAGING/CODEX_DONE_image_gen_s95.md` with:
   - Per-prompt status: OK / FAILED + reason
   - Output file paths (absolute) for successful ones
   - Total credit/cost reported by image_gen skill (if available)
   - One-line note: ready for Unity slice + import

2. List `STAGING/codex_floor_walls_v01/` contents (ls -la output) at the bottom of DONE file.

## DO NOT

- Do not run any Unity import / slicing — that is the user's next manual step.
- Do not touch `Assets/` directory.
- Do not modify `STAGING/CODEX_DISPATCH_path_c_floor_walls_s95.md` (read-only reference).
- Do not commit anything.
