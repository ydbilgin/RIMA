# Codex Review - Antigravity S95 Cleanup

## BasicAttackBehaviorBase.cs
Verdict: PASS_WITH_NOTES
Bulgular: CombatEventBus.PublishHit is called after hp.TakeDamage with attacker, target, damage, element, finisher-as-crit, and hitDirection populated. PublishKill is called only after damage when hp.IsDead is true, with killer/victim/worldPos/mobFamily populated. The knockback receiver lookup is guarded, and knockbackForce/knockbackDuration arrays are null/empty guarded. No direct compile issue is visible in this file. Remaining note: comboDamage, hitRange, hitRadius, owner, owner.Controller, and profile are still assumed valid; empty profile arrays would still throw, but that appears to be the existing profile contract rather than a new cleanup regression.

## MarkPulseBehavior.cs
Verdict: PASS_WITH_NOTES
Karar dogru muydu: HAYIR
Oneri: MarkPulseBehavior has its own ApplyMeleeHitWithMarks path, so leaving it untouched means Ravager hits still bypass CombatEventBus and still call legacy juice directly: HitStop, DamagePopup, LightPulse, and CameraShake. There is no bus-subscriber double-effect risk right now because this path does not publish hit/kill events. The risk appears when bus events are added later without deleting these direct legacy calls. Recommendation: migrate this custom hit path to publish HitEvent/KillEvent like BasicAttackBehaviorBase, remove direct generic legacy hit feedback from the damage path, and keep only Ravager-specific mark/Blood Pact visuals intentionally outside the generic hit bus if desired.

## RimaUnifiedPainterWindow.cs
Verdict: PASS_WITH_NOTES
Bulgular: Props_Root creation is guarded and uses identity transform when targetParent resolves to a Grid/Tilemap-like parent. GetRecursiveChildren correctly walks descendants for save, erase fallback, object picking fallback, wall search, and rebuild. SaveMapData serializes recursive descendants and skips group containers by SpriteRenderer, so grouped children are included. Erase and wall lookup/rebuild also use recursive traversal. However, the grouping implementation is incomplete in two important paths: PaintWallWithConnections instantiates auto-connected walls directly under Props_Root instead of GetOrCreateGroupParent(..., Walls), and LoadMapData instantiates loaded objects directly under Props_Root, so a save/load roundtrip loses the subgroup hierarchy. ClearSceneCanvas deletes direct children of Props_Root, which deletes group containers and their children, so it is acceptable. Naming coverage matches wall_, statue_, mounting_ and patch/rug/carpet, but mob scanning uses enemy_ while group fallback only checks mob_; category Mob still routes to Mobs, so selected scanned mobs are fine. No obvious Editor callback leak was found; OnDisable unregisters callbacks and preview cleanup is called.
UI/UX Notlar: The subgroup hierarchy is visible in the Unity scene hierarchy only after groups are created. There are no in-window group headers, filters, visibility toggles, lock toggles, counts, or colored scene labels. The UI also warns to assign Target Parent when targetParent is null even though GetTargetParent may auto-create Props_Root from the tilemap/grid path, which can mislead designers.

## PathC_BaseTest.unity
Verdict: PASS
Bulgular: Props_Root exists as a scene root GameObject. Its transform is identity: localPosition (0,0,0), localRotation identity, localScale (1,1,1). m_Father is 0 and SceneRoots includes its transform fileID, so it is not a child of Grid or any Tilemap.

## UI/UX Genel Oneriler (Top 3)
1. Add an in-window placed-object hierarchy panel with group rows for Walls, Statues, WallMountings, Patches, Mobs, and FloorProps, including counts plus visibility/lock/select-all controls.
2. Make target status explicit: show the effective parent that will be used, including auto-created Props_Root, instead of warning only on the serialized targetParent field.
3. Improve palette navigation with subcategory filters and non-clipped names/tooltips; current fixed 92x110 tiles clip long asset names and provide little metadata for large packs.

## Genel Verdict
PASS_WITH_NOTES
