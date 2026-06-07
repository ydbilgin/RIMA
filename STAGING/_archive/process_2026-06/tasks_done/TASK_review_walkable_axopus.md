# REVIEW TASK: Walkable enforcement (commit 3b800815) — STATIC, READ-ONLY

Rules: evidence file:line · do NOT modify files · do NOT enter play mode/run tests (cite commit's own evidence) · another agent edits designer files — review COMMIT DIFF only (`git show 3b800815`).

Spec: `STAGING/TASK_walkable_enforcement_2026-06-07.md` (audit evidence + 9 items). Repo CWD = RIMA root.

This commit touches the LIVE combat/movement chain (PlayerController, BaseMobBehavior, KnockbackReceiver, KnockdownDriver, WalkabilityMap, EliteAffix, IsoRoomBuilder boundary). Focus — verdict PASS/PASS-WITH-NOTES/FAIL + findings (severity/file:line/fix):
1. **ClampVelocityToWalkable correctness:** next-pos prediction uses what dt (fixedDeltaTime?) and whose position (rb vs transform)? Tunneling risk at high knockback speeds (velocity*dt jumps a whole cell)? Should it subsample or clamp distance?
2. **Feel regressions:** does the clamp make wall-sliding sticky for the PLAYER (velocity zeroed instead of slid along boundary)? Dash path: can clamp kill dash mid-flight in legit situations (dashing parallel to edge)?
3. **Knockdown MoveArc clamp:** arc Y-offset is visual (parabola) — is the clamp applied to GROUND position (correct) or to the visual arc position (would falsely block)? i-frame/get-up state interactions?
4. **InitFromTemplate lifecycle:** chamber + _Arena both get it? Stale map after room transition (old bounds) — is it re-inited BEFORE player teleport (EnsurePlayerAtSpawn ordering)? What about rooms built without RoomRunDirector (Room Browser / Build in Arena editor button)?
5. **Permissive fallback:** WalkabilityMap absent → old behavior. Any scene where map is now PRESENT but template-less (chamber bootstrap?) causing false blocking?
6. **Mob behavior side effects:** mobs whose attack patterns legitimately leave walkable (flying? dash-attack mobs? Bomber lunge)? Any mob subclass overriding FixedUpdate and bypassing the clamp (grep subclasses)?
7. **Prop layer change:** any prop that SHOULDN'T block (decals/floor decor flagged blocking by mistake)?
8. **Tests:** 10 EditMode tests — do they cover diagonal corner-cut and knockback-stop claims meaningfully, or only trivially?

Write full review to `STAGING/_review_walkable_axopus.md`. End with verdict.
