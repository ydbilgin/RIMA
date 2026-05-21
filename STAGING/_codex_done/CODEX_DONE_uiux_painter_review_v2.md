# Codex Review - UIUX Painter Redesign v2

## STATUS
NEEDS_REVISION

## Section V - Verification of v1 corrections
1. Panel 1 SetActive -> SceneVisibilityManager
- V1: Y. v2 replaces SetActive with SceneVisibilityManager Hide/Show in the behavior body (spec lines 168-171) and repeats it in the matrix (lines 528).
- V2: N as written. The concept is sound, but Unity reflection shows Hide/Show use parameter name `includeDescendants`; spec line 170 uses named argument `includeChildren`, which will not compile if copied verbatim. Use positional args or `includeDescendants: true`.

2. Panel 1 lock durability claim removal
- V1: Y. The lock tooltip now says editor state under Library and not durable scene-saved lock (lines 172-174).
- V2: Y. This matches the v1 E3 correction and does not overpromise save/reopen durability.

3. Panel 2 cache key expansion
- V1: Y. The key now includes prefab id, category, chosenMode, scaleMult, rotationSteps, custom size/offset hash, and rulesSO dirty count (lines 250-257).
- V2: Y with minor implementation note. The dimensions are the right invalidation inputs. Prefer a typed key/ValueTuple dictionary over `Dictionary<int,...>` to avoid hash collision ambiguity.

4. Panel 4 Ping side-effect fix (PeekTargetParent)
- V1: Y. v2 forbids Panel 4 ping/status from calling GetTargetParent and introduces PeekTargetParent (lines 439-462).
- V2: Y with minor drift risk. The helper removes the auto-create side effect, but GetTargetParent should be refactored to call PeekTargetParent plus create-if-null.

5. Panel 3 ZWSP source-literal cleanup
- V1: Partial. v2 says display-only and local (lines 372-379), but the snippet at line 376 still contains a real U+200B character; shell detection also found U+200B at lines 17 and 531.
- V2: N. The fix must mandate `"_\u200B"`, not a literal invisible character. As written it contradicts the ASCII-safe claim.

6. Gizmo colorblind hardening
- V1: Y. v2 adds alpha/backing outlines and WallBlock cross diagonals (lines 264-276).
- V2: Y. Shape plus outline reduces reliance on red/green color alone.

7. CollisionResolver single source
- V1: Y. v2 defines CollisionResolver.Resolve as the dry-run single source (lines 80-98), uses it in Panel 2 (line 240), badges (line 389), and delegates ConfigureCollider (line 493).
- V2: Y, but implementation must update every placement/preview call site, not only the ConfigureCollider body, so preview, placement, wall replacement, and autoconnect cannot drift.

8. GroupClassifier single source
- V1: Y. v2 defines CanonicalGroups (lines 70-77), uses it for Panel 1 iteration (line 168), and delegates GetOrCreateGroupParent (line 492).
- V2: Y. This is the right small refactor for group name drift.

9. Foldout state preservation (showPrefabSettingsSection rename)
- V1: Y. v2 replaces the old foldout location but preserves `showPrefabSettingsSection` (lines 204, 332, 489).
- V2: Y. Existing source already has this field at RimaUnifiedPainterWindow.cs line 93, so rename preserves state.

10. Dictionary<string,bool> -> SessionState
- V1: Y. v2 specifies runtime dictionary plus SessionState persistence (line 197).
- V2: Y with minor namespace note. SessionState is suitable, but the key should be window-unique.

11. Editor visibility durability tooltip
- V1: Y. v2 explicitly says the picking lock is editor state, not a durable scene lock (lines 172-174, 537).
- V2: Y. This is technically accurate.

12. OnHierarchyChange loop fix (time-polling removed)
- V1: Y. v2 removes time-based scene-org refresh and uses OnHierarchyChange dirty flag plus Repaint (lines 186-192).
- V2: Y. Existing OnEditorUpdate asset-preview repaint logic at source lines 163-173 is unrelated and can remain.

13. Banner OnGUI direct derivation (no extra dirty flag)
- V1: Y. v2 derives banner state from PeekTargetParent, targetTilemap, and activeBiome without adding a dirty flag (lines 425-466, 539).
- V2: Y with minor matrix wording cleanup in S4.

14. BeginDisabledGroup pattern + asmdef placement
- V1: Partial. BeginDisabledGroup is applied in the spec (lines 182, 313, 329, 466). The asmdef fix is not sound: line 284 and line 501 place CollisionRulesSO under `Assets/Editor/MapDesigner/Rules/` and claim CollisionMode is in the same asmdef.
- V2: N. The current painter file is `Assets/Editor/RimaUnifiedPainterWindow.cs` in namespace `RIMA.Editor.MapDesigner` (source lines 13-19), outside `Assets/Editor/MapDesigner/RIMA.MapDesigner.Editor.asmdef`. An asmdef script there cannot safely reference a type in the predefined editor assembly. Put CollisionRulesSO beside the painter under `Assets/Editor/`, move both into the same asmdef, or hoist the enum into a referenced assembly.

## Section N - New risk (v2-introduced)
- N1: GroupClassifier requires modifying GetOrCreateGroupParent during implementation, but v2 itself is spec-only and does not edit source. Evidence: spec says no code was written (lines 4-5), helper is pseudo-spec (lines 67-77), and implementation writes helpers later (line 104).
- N2: CollisionResolver requires modifying ConfigureCollider during implementation, but v2 itself does not edit source. Evidence: pseudo-spec lines 80-98 and implementation-later note line 104. Existing source remains hardcoded at RimaUnifiedPainterWindow.cs lines 1901-1978.
- N3: `resolveReason` can be safe if computed only on cache miss. If Resolve runs for every IMGUI repaint/palette item and builds strings dynamically, it can create visible GC. Keep reason strings const/static or cache ResolvedCollider by the expanded key before drawing labels/badges.
- N4: PeekTargetParent duplication is acceptable for v2 only if implementation refactors GetTargetParent to use `PeekTargetParent()` plus create-if-null. Otherwise future target resolution changes can drift between read-only status and placement.
- N5: Prefix-only glob via `StartsWith(patternWithoutStar)` is sufficient for the stated examples (`wall_*`, `statue_column_*`). If the UI promises general glob matching, mandate a small wildcard matcher; do not call this full glob support.
- N6: Verified through Unity reflection: `EditorUtility.GetDirtyCount(Object target)` exists and is non-obsolete. It is useful for in-memory SO edit invalidation after SetDirty/inspector edits. For external disk/import changes, AssetDatabase.GetAssetDependencyHash would be more robust, but this cache is editor-session local, so GetDirtyCount is acceptable.
- N7: `painter.groupExpand.{name}` is a generic SessionState namespace. Collision risk is low but real across editor tooling; use `RIMA.UnifiedPainter.groupExpand.{name}` or similar.
- N8: `Selection.activeObject = rulesAsset; EditorGUIUtility.PingObject(rulesAsset);` selects and highlights the asset in a normal unlocked Inspector. It does not guarantee an editable Inspector if the Inspector is locked. For a command labeled Edit, prefer `EditorUtility.OpenPropertyEditor(rulesAsset)` or label it Select/Ping Rules SO.

## Section S - Spec internal consistency
- S1: Yes. Current GetOrCreateGroupParent produces the same six names: default FloorProps plus Walls, Statues, WallMountings, Patches, Mobs (source lines 2294-2316). This matches CanonicalGroups in spec lines 75-76 and Panel 1 mockup lines 154-159.
- S2: Mostly consistent with Karpathy #3 surgical scope, but not literally just two one-line delegations. Existing collision resolution appears in placement/preview/autoconnect paths as well as ConfigureCollider (source lines 1453-1458, 1606-1617, 2613-2614, 2730-2731). Spec should list those callers so implementation remains bounded.
- S3: Confirmed. The existing override field is `customCollisionMode` (source line 41), currently drawn in the collider foldout (source line 576). v2's Override Mode (spec line 216) and Save Rule disabling (line 313) reuse it; no shadow field is introduced.
- S4: Some banner rows are poorly worded. `Parent: explicit + null` is contradictory if explicit means `targetParent != null`; the row appears to mean no explicit parent plus tilemap-parent peek to existing Props_Root. `Parent: null + Props_Root yok` also overlaps tilemap-null unless guarded by tilemap OK. Split the matrix into tilemap, explicit parent, inferred parent exists, inferred parent missing.

## Section G - Per-panel re-verdict
- Panel 1: NEEDS_REVISION - Replace the incorrect named SceneVisibilityManager argument and use a window-unique SessionState key.
- Panel 2: NEEDS_REVISION - Collision resolver concept is good, but asmdef placement can compile-break and the cache key should be typed rather than reduced to int.
- Panel 3: NEEDS_REVISION - The spec still contains a literal ZWSP; mandate `\u200B` escape before implementation.
- Panel 4: LIVE_WITH_MINOR_NOTES - PeekTargetParent fixes the side effect; tighten the state matrix and refactor GetTargetParent through it to avoid drift.

## Section H - Overall verdict
NEEDS_REVISION

## Quotable summary
v2 fixes the main design direction, but it is not implementation-ready: SceneVisibilityManager named args are wrong, the ZWSP cleanup still contains a literal invisible character, and the proposed CollisionRulesSO asmdef placement does not match the current painter assembly.
