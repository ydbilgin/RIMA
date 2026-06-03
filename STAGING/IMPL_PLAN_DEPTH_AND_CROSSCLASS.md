# IMPL PLAN — Depth Bundle (Sundered Beat) + Cross-Class Echo
**2026-05-31 · Opus, code-grounded (read SkillData/IronCharge/DeathBlow/SunderMark/Earthsplitter/CripplingBlow, SkillStateTracker, SkillRuntime, CombatEventBus, CombatHandler, Beat3CommitTrigger, Health, BaseMobBehavior, RageSystem, DraftManager, SkillOfferUI/Generator, SkillDatabase, Warblade/Shadowblade controllers, CrossClass/* infra).**
Each step = small, independently Unity-compilable. Writer ≠ reviewer per project rules. Compile in Unity (not just dotnet) after every step.

---

## GROUND-TRUTH CORRECTIONS to the S6 report (read before planning)
The S6 FINAL REPORT is partly **stale**. Verified in code 2026-05-31:
- **State system ALREADY EXISTS & WORKS.** `SkillStateTracker` (`Assets/Scripts/Skills/SkillStateTracker.cs`) has `Sundered`, `Broken` consts + duration/stack/expiry. `SunderMark` applies Sundered, `Earthsplitter` applies Broken, **`DeathBlow` already reads `state.Has(Sundered/Broken)` to gate execution.** A1's "minimal state" is mostly built — the gap is (a) no auto-convert Broken×3→Sundered (canon), (b) **no visual tell**, (c) chains use a crude `CooldownPercent>0.5` proxy instead of a real `ChainWindowTracker`.
- **Layer-mask bug is REAL but PARTIAL.** IronCharge/SunderMark/DeathBlow already use `GetMask("Enemy")`. **Still on the stale `GetMask("Default")` (will MISS enemies post-refactor):** `CripplingBlow.cs:54`, `DeepWound.cs:24`, `IronCounter.cs:69`, `Blink.cs:44`. This is the step-0 fix and it is small + surgical, not a play-test mystery.
- **CommitBeat path is LIVE.** `CombatHandler.OnCommitBeat()` already `PublishCommitBeat(...)` (drives juice). Payload = {worldPos, attacker, beatIndex} — **no target**, so it can't apply state on the 3rd-hit. That's the real A1/A2 wiring gap.
- **KillEvent exists but is NOT published.** `CombatEventBus.OnKill`/`KillEvent` defined; `VFXRouter` subscribes. **Nothing calls `PublishKill`.** Enemy death currently flows `Health.OnDeath` → `RuntimeRoomManager.OnEnemyDied(enemy)` (RuntimeRoomManager.cs:795). That is the hook for A3 execute-heal/refund.
- **Cross-class infra ALREADY EXISTS but is the WRONG shape for Feature B.** `CrossClass/CrossClassSkillManager.cs` = passive-trigger stub (`ApplyEffect` = `Debug.Log`), `CrossClassGhostEffect.cs` = a fade-up ghost ON the player, `CrossClassSkillData.cs` = effect-type SO. **Feature B is a different mechanic** (transient travelling actor that *performs an existing skill*), so it is a NEW actor that should REUSE `CrossClassSkillData`'s SO pattern + `SourceClass` enum, but NOT route through the stub `ApplyEffect`.

## CANON (NLM, queried 2026-05-31) — overrides loosely-specified bits
- **No HP<30% generic execute** anywhere; all executes are state-gated. Warblade Death Blow gate = Broken/Sundered/Staggered. **A3: the DeathBlow `hpThreshold` fallback should be removed/demoted** (it currently allows lowHP execute with no state — violates canon).
- **Broken** = public state, **red crack overlay**. **3 stacks Broken auto-convert → Sundered** (internal, **orange-red split crack**). Sunder Mark applies Sundered directly. (A1 + A2.)
- **Heal payoff is NOT on DeathBlow.** Canon: Death Blow empties Rage; heal comes from **Battle Surge passive (+5% HP per Rage spent)** — so executing (which spends the whole bar via Battle Surge) is the heal loop. (A3: wire the *Rage-spent → heal* hook, not a DeathBlow-special heal. `BattleSurge.cs` already exists — verify it heals on spend.)
- **Echo = "Shadow Echo" system** (canon term; user doc says "Sundered Echo" — keep user's lore phrasing in copy if desired, mechanic is identical). **Translucent CYAN silhouette, max 0.3 alpha, 0.4s lifetime, then vanish.** ⚠️ User spec says "black silhouette" — **reviewer must reconcile** (recommend: black body fill + cyan rim, alpha ~0.3, per the memory `feedback` note "cold cyan rim on black silhouette"; canon's 0.3-alpha + cyan is the binding constraint).
- **Echo positioning by archetype (canon, exact):** Melee → ghost spawns **ON the enemy**; Ranged → **next to player (~24px / ~0.4u over-shoulder)**; Zone → at cursor/floor point; Self-buff → overlaps player. 50 total Shadow Echoes = 5 curated/class (demo: a small curated subset).

---

# FEATURE A — DEPTH BUNDLE (Sundered Beat: BREAK → EXECUTE)
Build order is **value-first / risk-ascending**. A0 + A1-tell are the visible signature and lowest risk.

### A0 — Hit-layer fix (pure bugfix, do FIRST) — touches-shared, LOW risk
- **Modify:** `Assets/Scripts/Skills/Warblade/CripplingBlow.cs:54`, `Assets/Scripts/Skills/Warblade/DeepWound.cs:24`, `Assets/Scripts/Skills/Warblade/IronCounter.cs:69`, `Assets/Scripts/Skills/Elementalist/Blink.cs:44` — change `LayerMask.GetMask("Default")` → `GetMask("Enemy")` (Blink: probably wants `Default` for wall-cast — verify; the WB three are enemy-finds).
- **Reused:** matches the already-migrated IronCharge/SunderMark/DeathBlow pattern.
- **Risk/unknown:** confirm "Enemy" layer is the post-refactor name (it is — BaseMobBehavior.Awake sets `gameObject.layer = NameToLayer("Enemy")`). Blink may legitimately want a wall mask — leave if so.
- **Verify:** in-play, CripplingBlow/DeepWound actually hit a mob.

### A1 — Sundered VISUAL TELL + auto-convert (★ START HERE — signature, mostly-new, LOW risk)
The single highest-value visible change. State already exists; make it *seen*.
- **Create:** `Assets/Scripts/Combat/Juice/BrokenStateVisual.cs` — a MonoBehaviour added to any enemy with an active Broken/Sundered state. Reads `SkillRuntime.State(go)` each frame (or subscribes — see A1b); shows:
  - **Broken** → red crack overlay sprite (child SpriteRenderer, sorting layer "VFX" or on the enemy's own sort point) + faint pulse.
  - **Sundered** → orange-red split-crack overlay + brighter cyan-edged glow (cyan = RIMA seal-energy brand).
  - On *enter* Sundered: one-shot **shatter shards** burst + crack SFX.
- **Create:** `Assets/Resources/VFX/BrokenCrack.png` / `SunderedCrack.png` overlay sprites — placeholder Python/solid first (note "replace w/ PixelLab crack decal"); do NOT block on art.
- **Reused systems (named):** `VFXRouter` (add pool entries `status_Broken`, `status_Sundered`, `shatter` driven by `CombatEventBus.OnStatusApplied` — `StatusEvent{statusId}` already routes `status_<id>`); `RIMA.Audio.AudioManager.Play(...)` for crack SFX; sorting via "Entities"/Pivot per project rule (NOT manual order).
- **Data model:** no SO. Add to `SkillStateTracker` an optional `event Action<string> OnStateEntered` (fires when a key goes 0→active) so the visual is event-driven not poll-driven (cleaner; see A1b).
- **Risk/unknown:** SkillStateTracker is auto-added by `SkillRuntime.State()` lazily — BrokenStateVisual must also lazily attach or be added on first state apply. Overlay sprite must follow the enemy's facing/scale.
- **Pure-new?** Yes except a tiny additive edit to SkillStateTracker (A1b).

### A1b — `SkillStateTracker` enter/exit events + Broken×3→Sundered auto-convert — touches-shared, LOW-MED risk
- **Modify:** `Assets/Scripts/Skills/SkillStateTracker.cs` — add `event Action<string,int> OnStateEntered` (key, stacks) fired in `Apply` when key newly created OR stacks cross a threshold; `event Action<string> OnStateExpired` fired in `Update` expiry. In `Apply`, when `key==Broken` and resulting stacks `>=3`, auto `Apply(Sundered, ...)` + `Remove(Broken)` (canon).
- **Reused:** existing dict/stack logic.
- **Risk:** SkillStateTracker is consumed by DeathBlow, Shadowblade, Ranger, MarkPulse/HeatGauge/VeilStrike. Events are additive (no behavior change for existing readers). The auto-convert changes Broken semantics — verify Earthsplitter (applies Broken ×1, maxStacks 3) + repeated casts now escalate to Sundered as intended.

### A2 — CommitBeat payload carries target + applies state (the BREAK delivery) — touches-shared, MED risk
Make the 3rd-hit commit beat actually *break* the enemy it lands on.
- **Modify:** `Assets/Scripts/Combat/CombatEventBus.cs` — extend `CommitBeatEvent` with `GameObject target;` (and optionally `string roleTag`). Additive field; existing subscribers (HitPause/Shake/VFX) ignore it.
- **Modify:** `Assets/Scripts/Combat/CombatHandler.cs` `OnCommitBeat()` — resolve the enemy currently being struck (nearest enemy in basic-attack reach, or pass-through from the melee behavior) and on commit apply `SkillRuntime.State(target)?.Apply(Broken, 6f, 1, 3)` + publish `StatusEvent{statusId="Broken"}` (drives A1 tell) + set `target` on the published `CommitBeatEvent`.
- **Reused:** `SkillRuntime.State`, `SkillStateTracker`, `CombatEventBus.OnStatusApplied`→VFXRouter (A1).
- **Risk/unknown:** **Where does CombatHandler learn the struck target?** `Beat3CommitTrigger` only passes the animator owner. Options: (1) CombatHandler does its own small OverlapCircle in facing dir on commit (mirrors basic-attack reach — verify the basic-attack hit code, `MeleeChainBehavior`/`BasicAttackBehaviorBase`, for the exact reach/mask to stay consistent), (2) the melee behavior caches "last hit target" and CombatHandler reads it. Recommend (1) for decoupling. Must use `GetMask("Enemy")`.
- **Pure-new?** No — modifies the live commit-beat path. **Review-critical.**

### A3 — Execute payoff: Rage refund + heal-on-spend + canon execute gate — touches-shared, LOW-MED risk
- **Modify:** `Assets/Scripts/Skills/Warblade/DeathBlow.cs` —
  - (canon) demote/remove the `lowHp` fallback in `FindExecuteTarget` so execute REQUIRES Broken/Sundered (keep `hpThreshold` field but gate behind a `[SerializeField] bool allowLowHpExecute=false`).
  - On a successful execute, **partial Rage refund** (e.g. `rage.AddRage(refundOnExecute)`) BEFORE/after the existing `rage.TrySpend(CurrentRage)` cash-out — sequence so Battle Surge's per-spend heal still triggers. (M204.)
- **Verify (don't necessarily modify):** `Assets/Scripts/Skills/Warblade/BattleSurge.cs` — confirm it heals +5%HP per Rage spend (canon heal loop). If it hooks `RageSystem.TrySpend`/`OnRageChanged`, the DeathBlow cash-out already drives the heal. If not wired, add the hook in BattleSurge (RageSystem exposes `TrySpend`/`TryConsume`/`OnRageChanged`).
- **Optional (nice-to-have):** publish `CombatEventBus.PublishKill(...)` from `RuntimeRoomManager.OnEnemyDied` (RuntimeRoomManager.cs:795) so kill VFX (`VFXRouter.HandleKill`) finally fires + future on-kill hooks have a real event. Low risk, isolated.
- **Reused:** `RageSystem` (`AddRage`, `TrySpend`, `OnRageChanged`), `Health.Heal` (respects `healMultiplier`), `CombatEventBus`.
- **Risk:** ordering of refund vs cash-out vs Battle-Surge heal; make sure refund can't create an infinite execute (gate by state being consumed). Consider consuming the Broken/Sundered stack on execute (`state.Consume(Sundered)`).

### A4 — `ChainWindowTracker` (named chain windows) — NEW system, MED risk
Replace the crude `CooldownPercent>0.5` chain proxy with real named windows.
- **Create:** `Assets/Scripts/Skills/ChainWindowTracker.cs` — a player-side MonoBehaviour: `OpenWindow(string id, float duration)`, `bool IsOpen(string id)`, internal dict like SkillStateTracker. Canon windows: `IronChargeNextHit`, `CripplingExecute`, `BladestormChain`, `SunderExecute`.
- **Modify (small, per-skill):** `IronCharge.cs` (open `IronChargeNextHit` after charge), `CripplingBlow.cs` (open `CripplingExecute`; also CONSUME `IronChargeNextHit` for its heal-reduction chain instead of reading `ironCharge.CooldownPercent`), `DeathBlow.cs` (read `IsOpen("CripplingExecute")`/`SunderExecute` for the ×6 chain instead of `cripplingBlow.CooldownPercent>0.5`), `IronCrush.cs` (Bladestorm chain), `SunderMark.cs` (replace `deathBlow.CooldownPercent>0.5` proxy).
- **Reused:** mirrors `SkillStateTracker` design; lives on player root so `GetComponentInParent<ChainWindowTracker>()` resolves like `rage`.
- **Data model:** none (string consts on the tracker, mirroring `SkillStateTracker`'s const pattern).
- **Risk/unknown:** these per-skill edits touch every WB chain skill — do as ONE reviewed step, verify each chain still triggers in-play. Pure-new tracker = low; the per-skill rewiring = review-critical.

### A5 — Draft chain-UI (M68: "pairs with" cyan icon + curated pool) — touches-shared (UI + generator), LOW risk
Make interlocks visible — "can't combo what you can't see."
- **Modify (data):** `Assets/Scripts/Skills/SkillData.cs` — add `public string[] statesProduced;` + `public string[] statesConsumed;` (string keys matching SkillStateTracker consts) OR a lighter `public string[] chainsWith;` (skillName list). Seed in `SkillDatabase.cs` for the 12 WB skills (e.g. SunderMark.statesProduced=["Sundered"], DeathBlow.statesConsumed=["Sundered","Broken"]).
- **Modify (UI):** `Assets/Scripts/UI/SkillOfferUI.cs` `BuildSkillCard`/`BuildRewardCard` — if the offered skill `statesConsumed` matches a state `statesProduced` by an already-owned active (query `DraftManager`), draw a small cyan "⟂ pairs with X" chip (reuse the existing TierChip pattern + `RimaUITheme.Cyan`).
- **Modify (query):** `DraftManager.cs` — expose `IReadOnlyList<SkillData> CurrentActiveSkills` (currently private `currentActiveSkills`) so the UI can compute interlocks.
- **Modify (pool):** `SkillOfferGenerator.cs` / `SkillDatabase.GetPool` — for the demo, optionally restrict the offered WB pool to the ~6-8 high-interlock Common skills (freeze, don't delete; the report's step 6). Light flag, not a rewrite.
- **Reused:** `SkillOfferUI` card builder, `RimaUITheme`, `DraftManager`.
- **Risk:** purely additive data + cosmetic UI. Lowest-risk of the bundle except it depends on A-data seeding.

### A6 — OWNS/AVOIDS gate (process, NOT runtime) — pure-doc, ZERO code risk
- **No code.** Add an "OWNS / AVOIDS" section to each dispatch task .md (e.g. "Echo OWNS the transient actor; AVOIDS touching SkillBase.TryActivate / RageSystem.Modify"). Enforced by the `rima-conventions` review skill + writer≠reviewer. Keep it a checklist line in the per-step dispatch, not a class.

---

# FEATURE B — CROSS-CLASS SHADOW ECHO (transient actor performs a guest skill)
The hard constraint that shapes everything: **every skill is a `SkillBase` MonoBehaviour that resolves `player`/`rage`/`resource` via `GetComponentInParent<...>()` in Awake/TryActivate, and reads `player.FacingDirection`.** So the echo CANNOT simply host a guest `SkillBase` on its own GO (it would find no player). Two viable reuse strategies — **B2 picks one**.

### B1 — Echo binding data + slot plumbing — pure-new + small shared, LOW risk
- **Reuse/extend:** `Assets/Scripts/CrossClass/CrossClassSkillData.cs` — add fields: `public System.Type guestSkillType;` (NonSerialized, set at build like SkillDatabase does) OR `public string guestSkillName;` resolved via SkillDatabase; `public SkillTag archetype;` (Melee/Ranged/AOE — drives positioning, read from the guest skill's tags, NOT hardcoded per spec); keep `sourceClass`, `ghostColor`, `cooldown`.
- **Create:** `Assets/Scripts/CrossClass/EchoBinding.cs` (tiny struct/SO) OR just reuse a `CrossClassSkillData` slot. The skill-bar slot needs an `isCrossClass + guestSkillRef` — bind to a dedicated action (ult-adjacent). For Warblade, `Warblade_SkillController` already has 6 slots + secondary Z/X; **bind the Echo to a new action (e.g. C) or the ult slot** rather than overloading a native slot.
- **Modify:** `Warblade_SkillController.cs` — add an optional `CrossClassEcho echoSlot` + bind one InputAction (reuse `KeyBindManager` pattern). Keep minimal; do not disturb the 6-slot array.
- **Reused:** `CrossClassSkillData`, `SourceClass` enum, `KeyBindManager`, `SkillDatabase` (to map guest skill → Type).
- **Risk:** low; additive. Decide binding key with reviewer (ult-adjacent vs new key C).

### B2 — `CrossClassEcho` transient actor (★ core of Feature B) — NEW, MED risk
- **Create:** `Assets/Scripts/CrossClass/CrossClassEcho.cs` — a transient actor spawned on activation. Lifecycle: spawn silhouette GO → **puff-in** → move (by archetype) → **perform guest skill** → **puff-out** → despawn. Lifetime ≈ skill cast + small tail (canon = ~0.4s ghost; user = "skill duration + small tail" — reconcile: 0.4s is the *visible silhouette*, the skill effect can outlive it).
- **Silhouette visual:** instantiate guest idle sprite from `Resources/Characters/<Guest>/<guest>_idle_south.png` (already in repo, see CLAUDE gitStatus) via `Resources.Load<Sprite>`; SpriteRenderer with unlit material (`Sprites/Default`), **black fill + cyan rim, alpha ~0.3** (reconcile user "black" vs canon "cyan 0.3" — recommend black tint w/ cyan rim shader/outline). **Sorting: layer "Entities", sortingOrder 0, spriteSortPoint=Pivot** (Custom-Axis Y-sort per project rule — NEVER manual order). NO new art gen (recolor existing).
- **Positioning by archetype (canon):** read `archetype` from binding (B1). Melee → spawn ON nearest enemy to cursor, dash-to/strike. Ranged → spawn ~0.4u over player's shoulder toward cursor, strike from afar. Zone → at cursor. Self-buff → on player. Targeting reuses cursor-aim (`PlayerController.FacingDirection` / mouse world, like native skills).
- **Reused:** `Resources/Characters/*` sprites, `PlayerController` aim, `SkillRuntime` (DealDamage/EnemiesInCircle/etc.), `VFXRouter` (puff VFX via a `puff_in`/`puff_out` pool entry or `SkillRuntime.SpawnCircleVisual` black-smoke placeholder), `CombatEventBus`.
- **Risk/unknown #1 (BIGGEST):** **how to invoke the guest skill's real behavior** without reimplementing 10 skills — see B3.
- **Pure-new?** The actor file is new; it's review-critical only where it invokes guest behavior (B3) and where it touches layers/sort.

### B3 — Guest-skill invocation strategy (the crux) — NEW + touches SkillBase, MED-HIGH risk
The echo must *perform an existing skill*. `SkillBase.Execute()` is `protected` and resolves player refs from parent. Three options, pick with reviewer:
- **Option A (RECOMMENDED — "borrow under player"):** The guest `SkillBase` component lives **on the player** (added on bind, like DraftManager already does via `AddComponent(skill.skillType)`), so it resolves `player`/`resource` correctly. The Echo actor is **purely cosmetic** (silhouette + movement + puff). On the strike frame, the Echo calls a NEW public method on the guest skill, e.g. `SkillBase.ExecuteFromEcho(Vector2 originOverride, Vector2 aimOverride)`, which runs the same effect from the echo's world position. **Requires:** add a small public entry on `SkillBase` (a `public void ExecuteAt(Vector3 origin, Vector2 aim)` that sets a transient origin override then calls `Execute()`), OR refactor `Execute()` to read an overridable origin. **This is the shared-code touch** — additive, opt-in, default = current behavior.
- **Option B ("echo hosts skill, inject player"):** put the guest SkillBase on the echo GO and give SkillBase a way to accept an injected `PlayerController`/`RageSystem` (e.g. `Init(PlayerController, RageSystem, PlayerResourceBase)`). More invasive to SkillBase.Awake; higher risk.
- **Option C ("reimplement light"):** echo calls only `SkillRuntime` primitives (DealDamage in a circle/line by archetype) — no real skill reuse. Lowest fidelity, violates the "reuse existing behavior" goal. Fallback only.
- **Recommendation:** **Option A.** It reuses the exact damage/break/Sundered logic (so a guest Earthsplitter Echo really applies Broken on layer "Enemy"), keeps SkillBase changes additive, and the echo stays a cosmetic actor. The skill components already get AddComponent'd onto the player in DraftManager — same pattern.
- **Reused:** existing guest skill `Execute()` logic, `SkillRuntime`, layer "Enemy".
- **Risk/unknown #2:** many guest skills hardcode origin = `transform.position` (= player) and `player.FacingDirection`. For the echo to strike *from afar / on the enemy*, those need the origin/aim override. Audit which curated guest skills behave correctly with an origin override (start with 3-4 simple ones: Fireball, Aimed Shot, a melee like Backstab). Self-positioning skills (Blink/dashes) are bad echo candidates — curate them out.

### B4 — Puff VFX + "{Class} Echo!" UI pop — pure-new, LOW risk
- **Create:** puff-in/puff-out = small black smoke burst, cyan-tinted edge, pixelated-particle rules (memory). Reuse `VFXRouter` pool (`echo_puff` tag) or a tiny coroutine scaling `SkillRuntime.SpawnCircleVisual` dark sprite. Placeholder first.
- **Create/Modify (UI):** a 1-frame icon + "{Class} Echo!" text pop at screen edge (canon). Reuse `DamagePopup`/HUD text pattern or a tiny `EchoPopupUI`. Low priority cosmetic.
- **Risk:** none structural.

### B5 — Echo acquisition (draft "Echo of <Class>" card) — touches-shared (draft), LOW-MED risk
- **Modify:** `DraftManager.cs` + `SkillOfferUI.cs` + `RewardOffer` (find its def) — add a `RewardType.Echo` (or reuse skill card) that, when picked, binds a `CrossClassSkillData` to the player's Echo slot (B1) instead of a native slot. For the demo, can be granted once (class-select secondary pick already fires `OnSecondaryClassSelected` → could grant the matching Echo).
- **Reused:** existing draft offer/pick flow, `CrossClassSkillManager` (could repurpose its `slot1/slot2` for *Echo* bindings rather than passive effects — but keep the new actor path separate from its stub `ApplyEffect`).
- **Risk:** touches the draft selection switch; keep the Echo path isolated from the gold/heal/skill branches.

---

## RECOMMENDED BUILD ORDER (each line: step — files — risk)
1. **A0 hit-layer fix** — CripplingBlow.cs, DeepWound.cs, IronCounter.cs, (Blink.cs verify) — *touches-shared, LOW* (pure bugfix; do first, unblocks combat).
2. **A1 Sundered/Broken VISUAL TELL** — new BrokenStateVisual.cs + crack sprites + VFXRouter entries — *pure-new, LOW* (★ the visible signature; highest value).
3. **A1b StateTracker events + Broken×3→Sundered** — SkillStateTracker.cs — *touches-shared, LOW-MED* (additive events; canon auto-convert).
4. **A2 CommitBeat carries target + applies Broken** — CombatEventBus.cs, CombatHandler.cs — *touches-shared, MED* (review-critical: where target comes from).
5. **A3 execute payoff + canon gate** — DeathBlow.cs, BattleSurge.cs(verify), RuntimeRoomManager.cs(PublishKill, optional) — *touches-shared, LOW-MED* (ordering of refund/spend/heal).
6. **A4 ChainWindowTracker** — new ChainWindowTracker.cs + IronCharge/CripplingBlow/DeathBlow/IronCrush/SunderMark — *new+touches-shared, MED* (per-skill rewire = review-critical).
7. **A5 draft chain-UI + data** — SkillData.cs, SkillDatabase.cs, SkillOfferUI.cs, DraftManager.cs, SkillOfferGenerator.cs — *touches-shared, LOW* (additive data + cosmetic).
8. **B1 Echo binding data + slot** — CrossClassSkillData.cs, Warblade_SkillController.cs, EchoBinding — *pure-new + small shared, LOW*.
9. **B2 CrossClassEcho actor** — new CrossClassEcho.cs — *new, MED* (sort/layer + positioning).
10. **B3 guest-skill invocation (Option A: SkillBase.ExecuteAt)** — SkillBase.cs + CrossClassEcho.cs — *new + touches SkillBase, MED-HIGH* (★ the crux).
11. **B4 puff VFX + Echo UI pop** — new VFX/EchoPopupUI — *pure-new, LOW*.
12. **B5 Echo acquisition (draft card)** — DraftManager.cs, SkillOfferUI.cs, RewardOffer — *touches-shared, LOW-MED*.
13. **A6 OWNS/AVOIDS** — per-dispatch .md checklist — *no code*.

## TOP UNKNOWNS A REVIEWER MUST CHECK
1. **A2 — where does the commit beat learn its struck target?** `Beat3CommitTrigger` only passes the animator owner; `CommitBeatEvent` has no target today. Reviewer must confirm the chosen source (recommend CombatHandler does its own `OverlapCircle(GetMask("Enemy"))` matching the basic-attack reach — verify reach/mask against `MeleeChainBehavior`/`BasicAttackBehaviorBase` so the break lands exactly where the swing hits).
2. **B3 — guest-skill reuse without reimplementing 10 skills.** SkillBase.Execute() is protected + resolves player via parent + uses `transform.position`/`player.FacingDirection` as origin/aim. Option A (skill lives on player, echo calls `ExecuteAt(origin, aim)` override) is recommended but requires an additive SkillBase entry AND an audit that curated guest skills honor an origin override (many hardcode player origin). Reviewer must approve the SkillBase touch and the curated-skill shortlist (exclude self-positioning skills like Blink/dashes).
3. **Silhouette color: user "black" vs NLM canon "cyan, 0.3 alpha, 0.4s".** Reconcile before B2 art (recommend black fill + cyan rim @ ~0.3 alpha, 0.4s visible). Also confirm A3 canon: **DeathBlow heal is via Battle Surge per-Rage-spend, NOT a DeathBlow-special heal** — verify BattleSurge actually wires the +5%HP-per-spend loop, else A3 must add that hook.
