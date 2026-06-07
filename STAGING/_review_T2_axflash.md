# Static Code Review: T2 Combat Juice, SFX, Execute Prompt, Dash Buffer (Commit `549e185b`)

This review evaluates the implementation of the combat juice tuning, execute prompt, SFX pack, and dash input buffer under commit `549e185b` against the specifications in [TASK_T2_juice_sfx_2026-06-07.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/TASK_T2_juice_sfx_2026-06-07.md), MASTER_PLAN T2, and R5 M1 bullet.

---

## Focus Areas Evaluation

### 1. ExecutePromptDriver
* **Nearest-Target Scan Cost:** 
  > [!WARNING]
  > In [ExecutePromptDriver.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/ExecutePromptDriver.cs) at line 114, `FindNearestExecutableTarget` calls `Physics2D.OverlapCircleAll(pos, detectRadius, enemyLayer)` every frame inside `Update()`. This allocates a new `Collider2D[]` array every single frame, causing garbage collection (GC) pressure.
  >
  > Furthermore, the loop on lines 118–127 calls `GetComponent<Health>()` and `GetComponent<SkillStateTracker>()` every frame for every collider found. Doing `GetComponent` inside a per-frame update loop is a performance anti-pattern.
  >
  > *Fix:* Use `Physics2D.OverlapCircleNonAlloc` with a static pre-allocated array of size 8 or 16 to eliminate per-frame allocations, and check if component querying can be optimized or cached.
* **Prompt Lifecycle & Scene Change Leak:**
  * **Enemy Death / Status Consumption:** The driver behaves correctly here: when the nearest enemy dies or its state changes, `FindNearestExecutableTarget()` returns `null`, and the update loop simply deactivates the prompt GameObject (`promptLabel.gameObject.SetActive(false)`). This reuses the single prompt label rather than recreating it.
  * **Scene Change Reference Loss:** 
    > [!CAUTION]
    > In [ExecutePromptDriver.cs:53-54](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/ExecutePromptDriver.cs#L53-L54), the prompt GameObject is instantiated and unparented: `go.transform.SetParent(null, true);`.
    >
    > If the Player GameObject survives scene loading (e.g., if persistent or marked `DontDestroyOnLoad`), the unparented `ExecutePrompt_WorldLabel` GameObject in the root of the active scene will be destroyed when the old scene is unloaded. This leaves `promptLabel` pointing to a destroyed object, causing a `MissingReferenceException` in `Update()` on the next scene frame.
    >
    > *Fix:* Parent the label to the Player GameObject itself: `go.transform.SetParent(transform, true);` so its lifecycle is tied to the player, or add a null check in `Update` to recreate it if destroyed.
* **Warm Gold Color Check:** The color respects the palette. It is defined on line 35 as `new Color(1f, 0.92f, 0.55f, 1f)` (warm gold) and comments explicitly state `// warm gold, NOT cyan`.
* **Verdict:** **PASS-WITH-NOTES**

### 2. DeathBlow Wiring & Stacking Freezes
* **Double-Fire Risk on Multi-Hit:** **PASS**. `DeathBlow.Execute()` ([DeathBlow.cs:38](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/Warblade/DeathBlow.cs#L38)) uses a single target returned by `FindExecuteTarget(range)` and calls `DealDamage` and `OnExecuteFired` once. There is no multi-hit or double-fire risk in the execution path.
* **Order of Execution & Stacking Freezes:** 
  > [!NOTE]
  > `ExecutePromptDriver.OnExecuteFired()` is called *after* `SkillRuntime.DealDamage(...)` in [DeathBlow.cs:57](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/Warblade/DeathBlow.cs#L57).
  >
  > 1. `DealDamage` publishes a `HitEvent` via `PublishSkillHit`, which triggers the light hit pause of `0.03s` (HitPauseDriver's `pauseDurationHit`).
  > 2. Immediately after, `OnExecuteFired` invokes `HitPauseDriver.TriggerExecutePause()`, which requests a `0.10s` execute pause.
  > 3. Because `HitPauseDriver` implements pause queuing via `pendingExtraSeconds = Mathf.Max(pendingExtraSeconds, duration)` ([HitPauseDriver.cs:102](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/HitPauseDriver.cs#L102)), the `0.10s` execute pause stacks sequentially with the `0.03s` hit pause, producing a total freeze of `0.13s` instead of the configured `0.10s`.
  >
  > *Screen Shakes* do not stack because `ScreenShakeDriver.Shake` terminates any active shake coroutine before starting a new one ([ScreenShakeDriver.cs:138-141](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs#L138-L141)).
  >
  > *Kill-Pauses* do not stack because skills do not publish `OnKill` events (only basic attacks do, in `BasicAttackBehaviorBase.cs:98`).
* **Overlapping SFX:** 
  > [!WARNING]
  > When `DeathBlow` executes and kills an enemy, `TakeDamage` plays `Sfx.HitImpact` and `Sfx.EnemyDeath`, while `OnExecuteFired` plays `Sfx.ExecutePayoff` simultaneously. Playing all three cues on the same frame creates acoustic clutter and volume spikes.
  >
  > *Fix:* Suppress the normal hit/death SFX when the damage source is an execute.
* **Verdict:** **PASS-WITH-NOTES**

### 3. SFX Wiring & Audio Management
* **TakeDamage DoT/Multi-Hit Sound Spam:**
  > [!WARNING]
  > In [Health.cs:51](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/Health.cs#L51), `TakeDamage` plays `Sfx.HitImpact` on *every single damage tick*. Because status effects (`StatusEffectSystem.cs:209`) and environmental damage zones (`SeamCrawler_Trail.cs:83`) call `TakeDamage` directly, this will cause massive audio spam for ticking damage.
  >
  > *Fix:* Pass a damage type parameter to `TakeDamage` to skip playing the impact sound for periodic ticks/DoTs, or implement a short internal cooldown (ICD) on `HitImpact` audio triggers.
* **ChamberAmbient Leak (Forever Looping):**
  > [!CAUTION]
  > `ChamberAmbient` is played in `Start()` via `TryPlayAmbient()` in [AudioManager.cs:63](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Audio/AudioManager.cs#L63).
  >
  > Because the `AudioManager` GameObject is persistent (`DontDestroyOnLoad`) and there is absolutely no logic in the codebase to stop or pause `ambientSrc`, the chamber ambient track will continue to loop indefinitely across all scenes—including combat arenas, boss fights, and main menus.
  >
  > *Fix:* Add scene load hooks in `AudioManager` to stop the ambient loop when leaving the Chamber, or manage ambient audio via scene-specific managers.
* **Draft Hover SFX Rate-Limit:**
  > [!WARNING]
  > In [SkillOfferUI.cs:743](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/SkillOfferUI.cs#L743), `DraftHover` is played immediately when entering hover. If the player sweeps their mouse quickly over the draft cards, it will trigger repeatedly without rate-limiting, causing audio overlapping noise.
  >
  > *Fix:* Apply a debounce or an internal cooldown (e.g. 0.1s) on hover SFX triggers.
* **Verdict:** **FAIL** (Due to the persistent ChamberAmbient leak and TakeDamage DoT sound spam).

### 4. Dash Input Buffer Window Reduction
* **Buffer Window Regression:**
  > [!CAUTION]
  > In [InputBufferService.cs:10](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/InputBufferService.cs#L10), the `bufferWindow` was shrunk from `0.18s` to `0.08s` (80ms).
  >
  > Because `RequestDash()` ([L28](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/InputBufferService.cs#L28)) and `RequestAttack()` ([L34](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/InputBufferService.cs#L34)) share the same `bufferWindow` variable, **shrinking the window also reduced the attack buffer window to 80ms**.
  >
  > 80ms is extremely tight (less than 5 frames at 60 FPS) for buffering attack chain commands, meaning player inputs will be frequently ignored ("eaten") during commitment recovery. This directly regresses combat input feel. The developer's comment (`// Attack buffer keeps a slightly larger window for leniency`) was never implemented in code.
  >
  > *Fix:* Split the serialized field into two separate settings (e.g., `dashBufferWindow = 0.08f` and `attackBufferWindow = 0.18f`) and apply them in their respective methods.
* **Verdict:** **FAIL**

### 5. New Enum Values Serialized Stability
* **Enum Preservation:** **PASS**. The new enum values ([AudioManager.cs:11-19](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Audio/AudioManager.cs#L11-L19)) are appended after the original cues (`Hit` to `Finisher`). This maintains the integer indices of existing cues, ensuring all pre-existing serialized `Sfx` properties in Unity assets remain stable.
* **Verdict:** **PASS**

### 6. Juice Values vs. FeelToggleSettings
* **Bypass of Global Settings:**
  > [!CAUTION]
  > Direct calls to trigger juice bypass the global settings checks:
  > - `HitPauseDriver.TriggerExecutePause()` ([HitPauseDriver.cs:96](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/HitPauseDriver.cs#L96)) does not check `FeelToggleSettings.HitstopEnabled`.
  > - `ScreenShakeDriver.TriggerExecuteShake()` ([ScreenShakeDriver.cs:126](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs#L126)) and `TriggerKnockdownShake()` ([L129](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs#L129)) do not check `FeelToggleSettings.ShakeEnabled`.
  >
  > Because these are invoked directly from gameplay drivers (e.g., `ExecutePromptDriver` and `KnockdownDriver`) instead of through the event bus handlers, the execute freeze, execute shake, and knockdown shake will still play even if globally disabled in settings.
  >
  > *Fix:* Wrap the internals of `TriggerExecutePause()` with `if (!FeelToggleSettings.HitstopEnabled) return;`, and wrap `TriggerExecuteShake()` / `TriggerKnockdownShake()` with `if (!FeelToggleSettings.ShakeEnabled) return;`.
* **Verdict:** **FAIL**

---

## Summary of Findings

| Severity | File : Line | Description / Proposed Fix |
| :--- | :--- | :--- |
| **High** | [InputBufferService.cs:10](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/InputBufferService.cs#L10) | **Attack Buffer Shrinkage Regression:** Shrunk the global buffer window to 80ms, affecting both dash and attack buffer. Attacks feel "eaten" because 80ms is too short for chaining attacks. <br>*Fix:* Add a separate `attackBufferWindow = 0.18f` variable. |
| **High** | [AudioManager.cs:63](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Audio/AudioManager.cs#L63) | **Persistent ChamberAmbient Loop Leak:** Looping ambient sound is played once at startup and is never stopped, leaking into combat runs, boss fights, and menus. <br>*Fix:* Implement scene load triggers to stop or change the loop when exiting the Chamber. |
| **High** | [HitPauseDriver.cs:96](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/HitPauseDriver.cs#L96) <br> [ScreenShakeDriver.cs:126](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs#L126) | **Global Feel Settings Bypass:** Direct invocations of execute freeze/shake and knockdown shake bypass settings checks, ignoring `FeelToggleSettings`. <br>*Fix:* Add explicit conditional guards checking `FeelToggleSettings.HitstopEnabled` and `FeelToggleSettings.ShakeEnabled`. |
| **Medium** | [ExecutePromptDriver.cs:53](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/ExecutePromptDriver.cs#L53) | **Scene Change Reference Loss / Crash Risk:** Unparented `ExecutePrompt_WorldLabel` is destroyed on scene unload. If Player survives, this causes a `MissingReferenceException` in `Update()`. <br>*Fix:* Parent the label GameObject to the Player GameObject. |
| **Medium** | [Health.cs:51](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/Health.cs#L51) | **TakeDamage DoT Impact Sound Spam:** Plays `Sfx.HitImpact` for every damage tick, spamming audio on poison/bleed status damage or environmental tick trails. <br>*Fix:* Check damage type, or add a short internal cooldown on hit impacts. |
| **Minor** | [ExecutePromptDriver.cs:114](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Combat/ExecutePromptDriver.cs#L114) | **Per-Frame Physics Allocations & Component Lookups:** `Physics2D.OverlapCircleAll` and loops calling `GetComponent` every frame generate garbage and CPU overhead. <br>*Fix:* Use `Physics2D.OverlapCircleNonAlloc` with a pre-allocated array. |
| **Minor** | [DeathBlow.cs:57](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/Warblade/DeathBlow.cs#L57) | **Double Pause / Freeze Stacking:** The sequential call stacks a `0.03s` hit pause and `0.10s` execute pause, resulting in `0.13s` total freeze. <br>*Fix:* Ensure execute pauses override rather than stack with hit pauses, or clear pending pauses on execute fire. |
| **Minor** | [DeathBlow.cs:54](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/Warblade/DeathBlow.cs#L54) <br> [DeathBlow.cs:57](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/Warblade/DeathBlow.cs#L57) | **Acoustic Clutter on Execute Kill:** Plays `HitImpact` + `EnemyDeath` + `ExecutePayoff` simultaneously on the same frame when an execute kills an enemy. <br>*Fix:* Suppress basic hit/death sounds on executing blow. |
| **Minor** | [SkillOfferUI.cs:743](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/SkillOfferUI.cs#L743) | **Draft Hover sound spam:** Rapid mouse sweeps over cards trigger overlapping draft hover SFX. <br>*Fix:* Add a short cooldown/debounce to hover sounds. |

---

## Verdict

**FAIL**
