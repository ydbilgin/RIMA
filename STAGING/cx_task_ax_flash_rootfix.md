# CX TASK — Eliminate the residual `ax dispatch` window flash at the ROOT

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: code / STAGING / memory files.

## Amaç (purpose)
User is bothered by a brief console **window flash** every time `ax dispatch` runs (used by /ask_gemini, /p, /generate_image). Eliminate it at the ROOT, or if truly impossible, make it provably never-visible. Do NOT break output capture, the `ax` public interface, or UnityMCP.

## Full chain already traced by orchestrator (do not re-trace, build on this)
- `ax` = `C:\Users\ydbil\AppData\Roaming\npm\ax.cmd` → `pwsh -File "F:\Antigravity Projeler\AntigravityAuthManager\ax.ps1" %*`
- `ax dispatch` → `ax.ps1` calls `& ax_dispatch.cmd @Rest` (same dir).
- `ax_dispatch.cmd` already jumps to **pythonw.exe** (kills the python.exe console flash) → runs `ax_dispatch.py`.
- `ax_dispatch.py` spawns **agy.exe** under a **pywinpty ConPTY** (`winpty.PtyProcess.spawn`, `PYWINPTY_BACKEND=2`). The ConPTY is a workaround for agy's `text_drip` streaming-capture bug.
- Existing "S110" suppression in `ax_dispatch.py`: pythonw relaunch, `PYWINPTY_BACKEND=2`, a `SetWinEventHook(EVENT_OBJECT_CREATE)` that SW_HIDEs windows matching a **GUESSED class list** (`OpenConsole`, `OpenConsoleWindow`, `ConsoleWindowClass`, …; code comment admits "Tahmin class list … misses"), plus `CREATE_NO_WINDOW` + STARTUPINFO SW_HIDE.
- **Residual flash = the ConPTY `OpenConsole.exe` child window** — either races visible for a few ms before the hook fires, or its actual window class on THIS machine (Windows 11 build 26200, Windows Terminal default) is not in the guessed list.

## Files (edit ONLY these; back up first)
- Primary: `F:\Antigravity Projeler\AntigravityAuthManager\ax_dispatch.py`
- Before editing, COPY the current `ax_dispatch.py` → `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_archive\ax_dispatch_pre_flashfix_2026-06-02.py`.

## Approach — try in priority order, implement the best that works
1. **PREFERRED — drop ConPTY:** Run `agy --help` and the dispatch subcommand help in YOUR shell. Look for any flag that disables the streaming/`text_drip` animation or gives plain/JSON/non-interactive output (e.g. `--no-stream`, `--plain`, `--json`, `--no-color`, `--quiet`, a pipe-friendly mode). If such a mode exists, add an **env-gated** code path `AX_NO_PTY=1` in `ax_dispatch.py` that captures agy via plain `subprocess.run(..., stdout=PIPE, creationflags=CREATE_NO_WINDOW, startupinfo=SW_HIDE)` — NO pywinpty, so NO OpenConsole ever spawns = zero flash. Keep the existing pywinpty path as the default fallback so nothing breaks if the flag is unset.
2. **IF ConPTY is unavoidable — harden the hook by PID, not class:** Replace the guessed-class matching with PID-based hiding: right after `winpty.PtyProcess.spawn`, get the agy child PID; enumerate top-level windows (`EnumWindows` + `GetWindowThreadProcessId`) belonging to that PID *or its OpenConsole child PID* and `ShowWindow(SW_HIDE)` + move offscreen. Add a tight post-spawn poll (e.g. every ~10ms for ~300ms) to catch the window the instant it appears (beat the race). Keep the WinEventHook too (belt-and-suspenders).
3. **Last resort:** run the agy dispatch via a hidden Scheduled Task or `conhost.exe --headless` if pywinpty supports it — only if 1 and 2 fail.

## Constraints
- Output capture MUST still work (pythonw inherits parent stdout for `run_in_background` redirection). Verify the clean answer still reaches stdout.
- Do NOT change `ax.cmd` / `ax.ps1` / `ax_dispatch.cmd` interfaces. Surgical edits to `ax_dispatch.py` only (+ a tiny helper if needed).
- Keep the existing path as fallback (env-gated new path) so a bad change can't break the live pipeline I'm actively using.
- You CANNOT visually confirm "no flash" headlessly — implement + explain, the USER verifies.

## Deliverable (CODEX_DONE.md)
- Whether agy has a no-PTY/no-drip mode (paste the relevant `agy --help` lines).
- What you changed in `ax_dispatch.py` (diff summary + why).
- The EXACT command the user runs to verify (e.g. `set AX_NO_PTY=1` then a dispatch) and what they should observe.
- Confirm output capture still works (paste a captured sample). BLOCKED + reason if agy help can't be read. Do NOT commit.
