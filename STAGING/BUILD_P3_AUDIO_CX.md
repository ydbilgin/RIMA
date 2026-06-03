ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
Verify with `dotnet build RIMA.Runtime.csproj --no-restore` (0 errors). UnityMCP UNRELIABLE — don't block on it. Opus reviews.

# BUILD — PHASE 3 Audio: Resources/Audio override loader + missing hooks (PURE .cs)

Spec = `STAGING/DESIGN_LOCK_DEMO_S6.md` §5 P3 + Placeholder Registry G2. AudioManager already auto-bootstraps and plays
PROCEDURAL SFX (Hit/Death/Gate/Draft). Real clips are DEFERRED (Sora+Gemini). This batch only: (a) let real clips
drop in later with zero code change, (b) wire the few missing call-sites. C# only.

## F1 — Resources/Audio override loader (`Assets/Scripts/Audio/AudioManager.cs`)
- At init, for each `Sfx` enum value, TRY `Resources.Load<AudioClip>("Audio/" + sfxName)`; if found, use the real clip;
  else keep the existing procedural clip. So dropping `Assets/Resources/Audio/Hit.wav` (etc.) later auto-overrides with
  NO code change. Keep the procedural generation fully intact as fallback.
- Optional simple BGM: if `Resources.Load<AudioClip>("Audio/music_demo")` exists, play it looped at low volume; else silent.
  Do NOT generate procedural music.

## F2 — Missing call-site hooks (per §5 P3; keep audio centralized in AudioManager, do NOT use VFXRouter.soundEffect)
- `Play(Sfx.Dash)` at the existing `PublishDash` site (find it in PlayerController/dash path).
- Distinct crit/finisher audio accent on the `isFinisher` branch (reuse an existing Sfx or add one `Sfx` entry +
  procedural variant if trivial; do not over-engineer).
- `Play(Sfx.Shatter)` on the `PublishKill` site. (If `Sfx.Shatter` doesn't exist, add it + a simple procedural variant.)
- If a site is ambiguous, list what you found and mark that sub-item BLOCKED rather than guess.

## CONSTRAINTS
- Do NOT add real audio assets (none exist yet — that's the deferred Sora+Gemini pass). Loader must no-op gracefully when
  Resources/Audio is empty.
- No scene edits, no new asmdef.

## DELIVER (write to DONE file)
Files changed (file:line), how the Resources override works, which hooks added, any new Sfx entries,
`dotnet build RIMA.Runtime.csproj` result. List BLOCKED + why. Concise.
