# T6 + T6.1 Code Review — Opus 4.6

**Commits:** `86b8cfe3` (T6 functional base) · `efc02bd6` (T6.1 visual pass)
**Reviewer:** Claude Opus 4.6 · **Date:** 2026-06-07

---

## 1 · BossIntroController

### MAJOR-1 — Coroutine orphan on scene unload / early boss death

[BossIntroController.cs:49](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/BossIntroController.cs#L49) — `Begin()` creates a standalone GO and immediately launches `RunIntro()`. If the boss dies before the sequence completes (e.g. a lingering damage tick kills during intro), `PenitentSovereign.HandleDeath()` calls `StopAllCoroutines()` on *its own* GO but `BossIntroController` lives on a **separate GO** — its `RunIntro` coroutine keeps running. The dim overlay and title text remain on screen; `onComplete` fires and starts `BossLoop` on an already-dead boss.

**Fix:** BossIntroController should subscribe to the boss's `Health.OnDeath` (or accept a `CancellationToken`-like flag) to abort early. Alternatively, parent the intro GO under the boss so `Destroy(bossGO)` kills both.

### MAJOR-2 — Orphaned Canvas on early Destroy

[BossIntroController.cs:107](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/BossIntroController.cs#L107) — `Destroy(gameObject)` kills the controller, but `EnsureCanvas()` may have created a **"BossIntroCanvas"** GO that is *not* a child of the controller. If the existing canvas at sortingOrder ≥ 150 was reused, no leak; but if a new one was created (L207-213), it is never cleaned up and persists across scenes (no `DontDestroyOnLoad`, so a scene load would clear it, but within the same scene it lingers).

**Fix:** Track the created canvas GO and Destroy it in cleanup.

### MINOR-1 — No double-Begin guard

There is no guard preventing `Begin()` from being called twice (e.g. if two PenitentSovereign instances spawn). Each call creates a new controller GO. Unlikely in practice but worth a static `isRunning` flag.

### NOTE-1 — `WaitForSecondsRealtime` makes intro immune to timeScale

Intentional per "skip edilemez" spec. Good — intro plays even during HitPause. Just noting for awareness.

---

## 2 · PenitentSovereign

### MAJOR-3 — `FragmentFly` is `static` — orphan coroutine on Destroy

[PenitentSovereign.cs:750](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/PenitentSovereign.cs#L750) — `SpawnSealFragments()` calls `StartCoroutine(FragmentFly(...))` on `this`. Line 733: `Destroy(gameObject, 0.5f)` schedules destruction 0.5s later. But `FragmentFly` has `lifetime` up to 0.9s — so the hosting MonoBehaviour can be destroyed mid-flight, killing the coroutine. The `SealFragment` GO is left leaking with its final alpha (never reaches `Destroy(go)` at L792).

**Fix:** Either make fragments self-destroying (add a simple MonoBehaviour to each fragment GO that runs its own coroutine), or extend the Destroy delay to `≥ maxLifetime` (0.9s + margin).

### MAJOR-4 — DeathSequence uses `Time.deltaTime` during HitPause

[PenitentSovereign.cs:683](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/PenitentSovereign.cs#L683) — `HitPauseDriver.TriggerPause(0.20f)` sets `Time.timeScale = 0` (the driver's `pauseTimeScale` field defaults to 0). The DeathSequence fade loop at L696-708 uses `Time.deltaTime`, which is **zero** during the pause. The 2s fade stalls until HitPause ends (0.20s real-time), then continues. Functionally OK because pause is short, but the Fragment coroutines (also `Time.deltaTime`) face the same stall, compressing their visual effect.

No crash risk, but the boss freeze-frame may look jankier than intended if pause is ever tuned longer.

### MINOR-2 — 1.75x scale applied without collider adjustment

[PenitentSovereign.cs:153](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/PenitentSovereign.cs#L153) — `transform.localScale = BossScale (1.75×)`. The `CircleCollider2D` / `BoxCollider2D` on the prefab inherits this scale, making the collision shape 1.75× larger. All `Physics2D.OverlapCircle` attack radii use world-space coordinates (not relative to scale), so **attack hitboxes are correct**. But the boss's *hittable body* is 75% larger than the visual sprite — which is still a placeholder box. Once a real sprite is assigned, verify the collider radius vs. visual bounds.

### MINOR-3 — SyncRimSprite polls at 20Hz forever

[PenitentSovereign.cs:194-201](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/Boss/PenitentSovereign.cs#L194-L201) — Coroutine runs `WaitForSeconds(0.05f)` in a `while` loop for the entire boss lifetime. Cheap but unnecessary when no animation is playing. `HandleDeath` calls `StopAllCoroutines()` so it does stop on death.

### NOTE-2 — Telegraph decal cleanup on attack cancel

Telegraph GOs spawned by `EnemyTelegraph.SpawnCircle/SpawnLine` are **independent** root GOs with `destroyOnComplete = true` and a timer. If the boss dies mid-telegraph (`if (dead) yield break`), the telegraph still self-destructs on its own timer — no leak. ✓

---

## 3 · EnemyTelegraph

### NOTE-3 — "Decals" sorting layer is verified

[TagManager.asset:52-54](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/ProjectSettings/TagManager.asset#L52-L54) — `Decals` layer exists (`uniqueID: 1200000001`), ordered between Floor and Walls. "Ground", "Entities", "VFX" also present. No silent default-fallback risk. ✓

### NOTE-4 — SpawnDecal creates + Destroy lifecycle

[EnemyTelegraph.cs:200-216](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/EnemyTelegraph.cs#L200-L216) — Decal GO is parented to the telegraph GO (`SetParent(transform)`). When `Destroy(gameObject)` fires at L74 on completion, the child decal is destroyed with it. No leak, no pool — clean for the current volume of telegraphs. ✓

### MINOR-4 — `decalSR.transform.localScale` overwritten in Update

[EnemyTelegraph.cs:64](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Enemies/EnemyTelegraph.cs#L64) — `decalSR.transform.localScale = Vector3.one * scale` ignores the initial non-uniform scale set by `SpawnDecal` (e.g. line decal: `scaleX=length, scaleY=width`). The 0.88→1.0 "charge" animation **resets the decal to a uniform square** each frame.

**Fix:** Cache the initial scale and multiply: `decalSR.transform.localScale = initialScale * scale;`

---

## 4 · DeathScreenManager / DemoCompleteOverlay

### MINOR-5 — `RunStats.Instance` null → `EchoWallet.AwardRunIfNeeded(null)`

[DeathScreenManager.cs:163](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/DeathScreenManager.cs#L163) — `RunStats stats` can be null. L163 passes it to `EchoWallet.AwardRunIfNeeded(stats)` — depends on that method's null-tolerance (not verified here). The subsequent `stats != null ?` ternaries on L165-166 are correct. Same pattern in [DemoCompleteOverlay.cs:117](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/DemoCompleteOverlay.cs#L117).

`RunStats.RoomReached` and `RunStats.RunTimeSeconds` at L173/L127 are accessed as **static** properties even when Instance is null — safe only if they return defaults (0/0f) when uninitialized. Verify.

### NOTE-5 — Anchor/autosize approach is resilient

Both panels use percentage-based anchors with TMP auto-sizing (`fontSizeMin/Max`). This will scale correctly across 16:9 / 21:9 / 4:3 ratios. The fixed pixel offsets (`28f, 20f` padding in DemoCompleteOverlay) are inside a percentage-anchored parent, so they remain proportional. No fragility detected. ✓

---

## 5 · RewardPickup

### MINOR-6 — `Resources.Load` on every Awake, no cache

[RewardPickup.cs:29](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/RewardPickup.cs#L29) — `Resources.Load<Sprite>("Props/edge_filler_rift_shard")` runs each time a RewardPickup spawns without a sprite. Only one pickup exists per room, so the cost is negligible. If this ever scales (e.g. multi-pickup rooms), consider a static cached reference.

### NOTE-6 — Prompt canvas guard

[RewardPickup.cs:27](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/RewardPickup.cs#L27) — Sprite fallback only fires `if (sr.sprite == null)`. `EnsurePromptVisuals()` at L39 checks `if (promptCanvas != null) return` so it won't duplicate. The guard chain is correct. ✓

---

## 6 · Prefab YAML (11 enemy prefabs)

All 11 prefabs are **same byte-count** before/after in commit `efc02bd6` — this means only field values changed, no structural additions. The commit message says "placeholder sprites switched to diamond shape". Since byte counts are identical, no GUID/script reference shifts occurred. No collateral damage detected. ✓

---

## Verdict

| Severity | Count | Items |
|----------|-------|-------|
| **MAJOR** | 4 | #1 coroutine orphan, #2 canvas leak, #3 fragment orphan, #4 deltaTime in freeze |
| **MINOR** | 6 | #1 double-Begin, #2 collider scale, #3 rim poll, #4 decal scale reset, #5 RunStats null, #6 Resources.Load cache |
| **NOTE** | 6 | All clear ✓ |

### **CONDITIONAL PASS**

MAJORs #1 and #3 are real leak/crash paths in edge cases (boss dying during intro, fragment orphan on destroy). MAJOR #4 (decal scale flattening) will produce visually broken line/cone telegraphs in the shipped build. These should be fixed before the next playtest milestone. The remaining MAJORs and MINORs are backlog-safe.
