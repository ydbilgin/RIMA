# DONE — Editor Consolidation Implement (2026-06-13)

Per `EDITOR_CONSOLIDATION_DECISION_2026-06-13.md` safe order. RoomTemplateSO path KEPT, no RoomData merge.

1. KEY GUARD: `InPlayToolKeyRegistry` (RegisterExclusive/Release/OwnerOf/Owns/ClearAll); 2nd owner FAILS w/ clear error; same-owner re-register OK; DisableDomainReload-safe.
2. F2 SOLE OWNER + LEGACY RETIRE: BuildModeController registers F2+Quote, Update polls only if owner. InPlayMapPaintOverlay Bootstrap + f2Key poll gated behind `RIMA_LEGACY_MAPPAINT` (OFF; file kept).
3. SAVE: BuildModeController.SaveWorkingTemplate + SaveForValidation — EditorUtility.CopySerialized working->source + restore name/hideFlags + SetDirty + SaveAssets (#if UNITY_EDITOR; player no-op). Source untouched pre-Save (DontSave clone kept).
4. TMP: LiberationSans SDF-Fallback wired into TMP Settings m_fallbackFontAssets AND Jersey10 fallbackFontAssetTable (execute_code/SerializedObject); verified both non-empty. Labels stay ASCII.
5. POLISH: BuildModeUiStyle slate ramp brightened (panel/button/border/muted/card/surface; ember/void/selected unchanged). Iso-grid overlay = pooled world-space LineRenderer diamonds via neighbour-midpoint of grid.GetCellCenterWorld (no rect math), shown on enter / hidden on exit.
   - OUT OF SCOPE (flagged): "Expand Bounds" resize tool deferred per decision item 7 (core+tests first).

VERIFY: 0 compile errors (read_console). EditMode BuildMode group 25/25 PASS. PlayMode BuildMode group 8/8 PASS. (Full EditMode 566: 11 failures are PRE-EXISTING + unrelated — V15g/h missing PNGs, MCPSceneLoad, PerfAntiPattern, PrefabHealth, SubRoom DontDestroyOnLoad; none touch BuildMode/registry/TMP/palette.) Did NOT manually enter Play Mode.
