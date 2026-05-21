# Codex Review — UIUX Painter Redesign v3

## STATUS
NEEDS_REVISION

## Section W — v2 fix verification
### Hard
1. V1 (SceneVisibilityManager): W1=Y, main body uses `includeDescendants: true` for `Hide`, `Show`, `IsHidden`, `DisablePicking`, and `EnablePicking` at lines 172-174. W2=Y, this matches Unity's `SceneVisibilityManager` named argument and avoids the v2 compile error. W3=No new bug found; remaining `includeChildren` mentions are historical review text, not implementation instructions.
2. V5 (ZWSP): W1=Y, line 390 uses the visible ASCII escape text `"_\u200B"`. W2=Y, byte verification returned `0` occurrences of UTF-8 `E2 80 8B`. W3=No.
3. V14 (asmdef): W1=Y in the body, lines 292-298 move the script to `Assets/Editor/CollisionRulesSO.cs` and document the predefined Editor assembly rationale. W2=Y, source confirms the painter is `Assets/Editor/RimaUnifiedPainterWindow.cs` and the MapDesigner asmdef is `autoReferenced: false` with no reference to the predefined Editor assembly. W3=YES: open question 1 at line 543 still recommends `Assets/Editor/MapDesigner/Rules/CollisionRulesSO.cs` "mevcut asmdef altında", contradicting the fixed body and reintroducing the compile-break path.
4. S4 (banner matrix): W1=Y, lines 439-462 split the matrix into Tilemap, Parent, and Biome axes. W2=Y, the tilemap-null guard and parent-only-if-tilemap-OK wording remove the v2 contradictory rows. W3=No.

### Minor
5. V3 (cache key): W1=Y, lines 254-265 specify `readonly struct PreviewCacheKey : IEquatable<PreviewCacheKey>` and `Dictionary<PreviewCacheKey, ResolvedCollider>`. W2=Y, a typed key avoids reducing identity to an `int` hash. W3=Minor wording bug only: line 250 still says "typed ValueTuple key", but the actual pseudo-spec is a struct.
6. N3 (GC): W1=Y, line 270 says `resolveReason` is computed only on cache miss, with const/static strings preferred and rule-pattern interpolation limited to misses. W2=Y, cached strings avoid repaint-time allocation churn. W3=No.
7. N4 (GetTargetParent drift): W1=Y, lines 491-504 mandate `GetTargetParent()` as `PeekTargetParent()` plus create-if-null. W2=Y, this keeps read-only status and paint resolution aligned. W3=No.
8. N7 (SessionState namespace): W1=Y, line 199 uses `RIMA.UnifiedPainter.groupExpand.{name}`. W2=Y, this is window-unique enough for SessionState. W3=No.
9. N8 (OpenPropertyEditor): W1=Y, line 333 uses `EditorUtility.OpenPropertyEditor(rulesAsset)` with Select+Ping fallback and the fallback label "Select Rules SO". W2=Y, Unity 6000.3 docs list `public static void OpenPropertyEditor(Object obj)`, and live editor reflection confirms the method exists. W3=No.

## Section Z — Spec-wide consistency
- Z1: NEEDS_REVISION. The main body is mostly consistent, and no live implementation instruction still uses `includeChildren` or a literal ZWSP. However, line 543 contradicts the V14 fix by reverting the CollisionRulesSO script path to the asmdef folder. Also, stale labels remain (`Final Spec (v2)` at line 110 and v2 output wording at lines 560-566), but those are editorial.
- Z2: The cited caller ranges match the actual source: `PaintPrefab` 1453-1458, `DrawPrefabOutline` 1606-1617, `PaintWallWithConnections` 2613-2614, `UpdateWallConnectionsAt` 2730-2731, and Save/Load 2460-2545. One caller is missing: `ConfigureAssetPackColliders()` at source lines 1799-1801 still calls `GetDefaultCollisionMode` and `ConfigureCollider`, triggered by the "Setup Asset Pack Colliders" button at lines 704-709. If CollisionRulesSO is the single source, this legacy asset-pack path must be explicitly included or explicitly excluded.
- Z3: `HashCode.Combine` is available in the actual Unity target. Project version is Unity 6000.3.6f1; live reflection returned `HashCodeType=True` and `HashCodeCombine=True` even with active API compatibility reported as `NET_Standard_2_0`. It is also compatible with the intended Unity 6 / 2022 LTS / .NET Standard 2.1 target.
- Z4: `EditorUtility.OpenPropertyEditor` exists and is documented for Unity 6000.3 as `public static void OpenPropertyEditor(Object obj);` with description "Open properties editor for an Object." Live reflection also returned `OpenPropertyEditor=True`.

## Section G — Per-panel re-verdict
- Panel 1: LIVE
- Panel 2: NEEDS_REVISION — body is implementation-ready, but the stale open-question script path can send implementation back into the compile-breaking asmdef location; caller list also misses `ConfigureAssetPackColliders`.
- Panel 3: LIVE
- Panel 4: LIVE_WITH_MINOR_NOTES — technically ready; only stale v2 wording elsewhere in the doc should be cleaned.

## Section H — Overall verdict
NEEDS_REVISION

## Quotable summary
v3 fixes the four hard issues in the main body, but it is not ready to hand to implementation while the open questions still recommend the old asmdef CollisionRulesSO path and the CollisionResolver caller list omits `ConfigureAssetPackColliders()`.
