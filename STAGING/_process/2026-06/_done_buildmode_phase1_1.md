# Build Mode Phase 1.1 — DONE

Fixes (A singleton robustness, B backquote/quote desync, C duplicate-guard).

- `Assets/Scripts/UI/BuildModeController.cs` (~292 -> ~280 lines): static `Instance` auto-prop replaced with lazy find-or-create getter backed by `_instance`; added non-creating `IsActive`; removed redundant `[RuntimeInitializeOnLoadMethod] Bootstrap`; Awake/OnDestroy use `_instance`; Awake duplicate-guard now `Destroy(gameObject)`.
- `Assets/Scripts/UI/DirectorMode.cs` Update() (net +2 lines): backquote gated by `!BuildModeController.IsActive`; quote branch dropped the `!= null` guard (lazy getter never null).

Unity compile: forced script recompile + read_console -> **0 errors, 0 warnings**.
Scope honored: only the 2 listed files touched; scene-reload stale-buildCamera MINOR left deferred. Nothing BLOCKED.
