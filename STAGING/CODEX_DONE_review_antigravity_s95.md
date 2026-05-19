# Codex Review - Antigravity S95 Cleanup

## BasicAttackBehaviorBase.cs
Verdict: PASS
Bulgular: CombatEventBus.PublishHit is called after hp.TakeDamage with attacker, target, damage, physical element, finisher-as-crit, and hitDirection populated. CombatEventBus.PublishKill is called after damage when hp.IsDead, with killer/victim/worldPos/mobFamily. No new null-reference risk from the bus calls; CombatEventBus methods are static and null-safe for missing subscribers. Knockback arrays are null/empty checked before indexing. dotnet build Assembly-CSharp.csproj passed; no compile error observed in this file.

## MarkPulseBehavior.cs
Verdict: PASS_WITH_NOTES
Karar dogru muydu: HAYIR
Oneri: Add CombatEventBus.PublishHit and PublishKill to ApplyMeleeHitWithMarks, then remove the per-hit legacy HitStop/DamagePopup/CameraShake calls from that damage path. Current code does not double-fire bus subscribers because it never publishes to the bus, but that means Ravager hits bypass the new HitPauseDriver, DamageNumberDriver, ScreenShakeDriver, CameraPunchController, VFXRouter, and kill routing. Class identity pulses around Brutal Swing/Blood Pact can remain only if they are intentionally cast/start feedback; the actual damage/kill feedback should go through the bus.

## RimaUnifiedPainterWindow.cs
Verdict: PASS_WITH_NOTES
Bulgular: Props_Root creation has basic null handling and identity transform setup, and the scene file confirms the root object is scene-rooted. GetRecursiveChildren recursively collects descendants and is used by erase, save, eyedropper, wall lookup, wall rebuild, and IsoSorter attach flows. Save and erase are recursive enough for subgroup children. ClearSceneCanvas deletes direct children of Props_Root, which removes subgroup containers and their contents.

Important note: PaintWallWithConnections bypasses GetOrCreateGroupParent and instantiates auto-connected walls directly under Props_Root. Since autoConnectWalls defaults true for walls, normal wall painting does not use the new Walls subgroup. UpdateWallConnectionsAt preserves the current wall parent, so auto-replaced walls also remain outside Walls if the first wall was placed there. LoadMapData also restores objects directly under Props_Root instead of recreating subgroup parents, so saved maps lose the subgroup hierarchy on load. GetTargetParent uses GameObject.Find("Props_Root"), which can pick the wrong root with multiple loaded scenes or duplicate names; scene-context lookup tied to targetTilemap.gameObject.scene would be safer. No memory leak observed; preview cleanup and event unsubscription are present. Editor-only APIs are inside #if UNITY_EDITOR and Assembly-CSharp-Editor.csproj builds successfully with existing warnings.

UI/UX Notlar: Subgroup hierarchy is visible in the Unity Hierarchy once groups are created, but the painter UI does not expose group status, routing, or category counts. Because auto-connected walls currently skip the Walls group, the intended hierarchy is inconsistent for the most common wall workflow.

## PathC_BaseTest.unity
Verdict: PASS
Bulgular: Props_Root exists as a scene root object with m_Father fileID 0. Its transform is identity: position 0,0,0; rotation quaternion 0,0,0,1; scale 1,1,1. Grid is also a separate scene root with m_Father fileID 0, so Props_Root is not a Grid/Tilemap child.

## UI/UX Genel Oneriler (Top 3)
1. Add a compact scene-organization panel showing Props_Root and subgroup counts, with buttons to rebuild missing groups and move misplaced objects into the right group.
2. Make wall auto-connect behavior more transparent: show the active wall rule set, current selected wall variant, and a non-destructive preview of the replacement that will happen after placement.
3. Improve map-file safety: add dirty-state indication, last saved path/time, and clearer Load/Overwrite labels instead of the current ambiguous Fix button.

## Genel Verdict
PASS_WITH_NOTES

## Verification
- dotnet build .\\Assembly-CSharp.csproj --no-restore: PASS, existing warnings only.
- dotnet build .\\Assembly-CSharp-Editor.csproj: PASS after restore, existing warnings only.
- ANTIGRAVITY.md was not found at repo root; review used CODEX_TASK_laurethgame.md scope and listed files.
