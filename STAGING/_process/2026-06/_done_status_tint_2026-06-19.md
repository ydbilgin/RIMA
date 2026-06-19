## StatusEffectTint — DONE 2026-06-19

**New component:** `Assets/Scripts/VFX/StatusEffectTint.cs` — 165 lines. `[DefaultExecutionOrder(200)]` + `LateUpdate` so it wins over mob `LateUpdate` color resets. Lazy `WireToSES()` in Start+Update handles runtime AddComponent ordering.

**Color mapping (lerp 0.45 base, multiply-blend):**
Frozen=(0.4,0.7,1.0) · Scorch=(1.0,0.45,0.2) · Burning=(1.0,0.35,0.25) · Chill=(0.55,0.75,1.0) · Poison=(0.5,1.0,0.4) · Stunned=(1.0,0.95,0.4) · Weakened=(0.7,0.5,0.9) · RiftMark=(0.6,0.4,1.0). DoT (Burning/Scorch/Poison) pulse: sine 2.5 Hz, lerp 0.30↔0.60. Apply flash: 0.18 s at lerp 0.75.

**Auto-attach:** `StatusEffectSystem.Start` → `if (GetComponent<StatusEffectTint>() == null) gameObject.AddComponent<StatusEffectTint>();` (moved from Awake to avoid disabled-component Unity quirk).

**Screenshots:** `Assets/Screenshots/Playtest_2026-06-19/status_tint_chill_blue-3.png` (FractureImp clearly blue) · `status_tint_burn_red-1.png` (FractureImp clearly red/pink). SR.color verified: Chill=RGBA(0.798,0.888,1.0) · Burning=RGBA(1.0,0.696,0.649).

**Visibility:** CONFIRMED — both tints clearly legible on FractureImp sprite in scene_view. Chill=cool blue cast, Burning=warm red/pink cast, distinct at a glance.

**Compile/console:** 0 compile errors. 1 pre-existing Unity scene-cleanup error (unrelated, from MobDeathResidue test artifact). Play stopped, timeScale=1, scene not saved.
