# Review: class-select sprite-path fix (B1) + overlay cross-check (B3) — AGY

ACTIVE RULES: reviewer not writer; real issues only; flag BLOCKED if unclear. Do NOT edit files.
NLM ACCESS: not needed.

## B1 — class-select white-box fix (verify correctness)
Two edits were made to fix a bug where the class-select screen showed white boxes instead of character sprites (the old code loaded `Characters/Anchors/<class>_anchor`, a path that never existed under any Resources folder).

Edits:
- `Assets/Scripts/UI/RimaUITheme.cs` — `AnchorPath()` now returns `"Characters/<Class>/<class>_idle_south"` (e.g. `"Characters/Warblade/warblade_idle_south"`), used by `Resources.Load<Texture2D>`.
- `Assets/Scripts/UI/CharacterSelectScreen.cs` — `CanonicalSpritePath()` (used inside `#if UNITY_EDITOR` by `AssetDatabase.LoadAssetAtPath<Sprite>`) now returns `"Assets/Resources/Characters/<Class>/<class>_idle_south.png"`.

Ground truth: the new sprites exist at `Assets/Resources/Characters/<Class>/<class>_idle_<dir>.png`, 10 classes, PPU64.

Verify and report:
- (a) Are the paths EXACT? Resources.Load takes a Resources-relative path with NO extension; AssetDatabase takes a full `Assets/...png` path. Confirm both forms are right.
- (b) Folder/file CASE: do the actual folder names under `Assets/Resources/Characters/` match `ClassType.ToString()` exactly, and do the files match `<lowercased-enum>_idle_south.png`? List any mismatch (e.g. a class whose enum name != folder name).
- (c) `LoadCanonicalSprite` (CharacterSelectScreen.cs ~line 449) does `Resources.Load<Texture2D>` then `Sprite.Create(tex, fullRect, pivot(0.5,0))`. Any concern for PPU64 sprites rendering in the preview (e.g. the texture must be Read/Write or marked as Sprite — is `Resources.Load<Texture2D>` on a Sprite-imported PNG valid here)?

## B3 — overlay cross-check (light pass, code reviewer covers depth)
Quick sanity on `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` for things a line-by-line reviewer might miss:
- Is F2 already bound anywhere in the runtime input map (collision with the new toggle)?
- `[RuntimeInitializeOnLoadMethod]` + `DontDestroyOnLoad`: any risk of double-instantiation across additive scene loads or domain reloads?

## Output
Concise findings, grouped B1 / B3. State clearly whether B1 is correct as-is.
