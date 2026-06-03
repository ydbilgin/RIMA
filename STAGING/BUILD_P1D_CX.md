ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UnityMCP read_console UNRELIABLE — do NOT block on it. Verify with `dotnet build RIMA.Runtime.csproj --no-restore` (you used this successfully last batch — 0 errors). Do NOT self-certify; Opus reviews + Editor.log.

# BUILD — PHASE 1 Batch D: Victory + Death Wishlist CTA (PURE .cs, self-building UI)

Spec = `STAGING/DESIGN_LOCK_DEMO_S6.md` §4.5 (Death), §4.6 (Victory), §1.5 (copy lines), §9 decisions #2/#4.
The demo's CONVERSION point. C# only — NO scene edits. **Self-build the UI in code** (mirror the existing
self-building pattern in `Assets/Scripts/UI/SkillBarUI.cs` / `UI/Map/MapProgressController.cs` — they create their own
Canvas/children at runtime) so NO manual scene-wiring is needed. Use `RimaUITheme` tokens for colors.

## D1 — Victory screen = enhance existing `Assets/Scripts/Core/DemoCompleteOverlay.cs`
- It already hooks the DemoComplete event and shows "DEMO COMPLETE" + restart. ENHANCE it into the full §4.6 victory:
  - Run-summary (Room reached / Kills / Time / active build name if available) on a translucent stone panel with a
    tarnished-gold `#F2BC3D` edge (gold = reward moment, allowed here).
  - **WISHLIST CTA** (largest button): `[SerializeField] string steamWishlistUrl` defaulting to a PLACEHOLDER
    `"https://store.steampowered.com/app/0/"` (user replaces appid later). On click: try
    `Application.OpenURL("steam://openurl/" + steamWishlistUrl)` then fall back to `Application.OpenURL(steamWishlistUrl)`.
    Cyan `#00FFCC` accent. Label "WISHLIST ON STEAM".
  - Short observational victory line ("The full descent awaits.") + faint next-class teaser placeholder (text/box ok,
    real silhouette image later). Buttons: MAIN MENU / PLAY AGAIN.
- Keep it self-building (no scene refs required). Brief slowMo 0.2 + zoom is optional/nice — only if trivial via existing juice.

## D2 — Death screen CTA + non-blaming copy = enhance `Assets/Scripts/Core/DeathScreenManager.cs`
- Replace "YOU DIED" title with a ROTATING non-blaming canon line (pick one at random): "The rift remembers. You won't."
  / "Not an ending. Just a place where you stopped." (cold-observational, NO blame, NO "try again!" cheer).
- Add a cyan `#00FFCC` **WISHLIST** button (same steamWishlistUrl + OpenURL pattern as D1; factor a tiny shared helper
  if clean, else duplicate the 3 lines) + keep existing run-stats + "Copy Build Seed" optional + "TRY AGAIN [R]".
- Self-build the new buttons under the existing death panel (it already finds/creates its panel). Do NOT regress the
  Batch-A scale/refs fix.

## CONSTRAINTS
- Placeholder steam URL is intentional (§9 #4) — note it. Do NOT invent a real appid.
- No scene edits. No new assembly/asmdef.

## DELIVER (write to DONE file)
Files changed (file:line), what each screen now shows, how UI self-builds, `dotnet build RIMA.Runtime.csproj` result
(0 errors required). List BLOCKED + why. Confirm no scene edits. Concise.
