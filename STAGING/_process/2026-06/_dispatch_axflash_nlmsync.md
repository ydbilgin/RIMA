# TASK: NLM batch sync (deterministic — run the script, do NOT improvise)

You are a build agent. Your ONLY job: execute one shell script and report its output.

## Steps
1. Run EXACTLY this command from the project root (Git Bash / bash):
   ```
   bash "STAGING/_process/2026-06/nlm_batch_sync.sh"
   ```
2. Do NOT modify the script. Do NOT add your own nlm commands. Do NOT delete anything beyond what the script does.
3. If `uvx` or `nlm` auth fails (login expired), run `nlm login` once, then re-run the script.
4. Return the script's STDOUT verbatim (the "=== ... ===" log lines), plus a 2-line summary: how many files synced, any orphan warning.

## Constraints
- ASCII-safe output is fine.
- This touches NotebookLM only — it does NOT touch Unity or any code.
- If the script reports "Tum dosyalar guncel", that is success (nothing to sync). Report it as-is.
