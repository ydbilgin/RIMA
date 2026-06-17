# CX Council Response - Playtest Bugs, Industry Patterns, Gameplay, Playtest Plan

Advisor: cx / Codex
Date: 2026-06-16
Scope: demo-critical judgement for 2026-06-19 solo-dev demo.
Rule: where internals of shipped commercial games are not public, I mark internals as BELIRSIZ and use visible shipped behavior / public wiki behavior as pattern evidence.

## A - Bug fix + industry patterns

### A0. Priority order

DEMO-KRITIK FIX:
1. B1 reward-flow stall: make clear->reward->draft->grant->door a recoverable transaction with a reconcile watchdog.
2. B3 overlay bleed: one modal authority / scrim gate for M map, draft, pause, settings, director.
3. B2 wave density: data-first bump now; small controller extension only if data bump still feels thin.
4. B4 background void: add persistent ambient background layer and camera framing pass.
5. B6 instrumentation: in-game event log overlay for reward/wave/door/error.

POST-DEMO:
1. Full encounter director with enemy families, room archetypes, heat/budget curves.
2. Full modal-stack UI rewrite.
3. Real boss mechanics beyond telegraph/VFX placeholder.

### B1. Reward-flow stall

RIMA root-cause hypothesis:
- Live path is `RoomRunDirector.HandleEncounterCleared()` -> `RoomClearSequence()` -> `SpawnRewardPickup()` -> `RewardPickup.Collect()` -> `DraftManager.ShowDraft()` -> player pick -> `RoomRunDirector.OpenExitDoors()`.
- `RoomRunDirector` already has lifecycle states (`Combat`, `Cleared`, `RewardTaken`, `DoorOpen`) and guards, but the transaction is split across `RoomRunDirector`, `RewardPickup`, `DraftManager`, `RoomClearVictoryTrigger`, and legacy `RuntimeRoomManager`.
- `RewardPickup.DraftThenOpenExit()` still calls legacy/static exits (`RoomClearVictoryTrigger.ActivateExitDoors()` and `RuntimeRoomManager.Instance.OpenDoorsAfterReward()`) while the live `_Arena` owner is `RoomRunDirector`. That is a risky dual-owner path: doors can open visually/legacy while skill grant failed or while director state is still stale.
- `RewardAutoCollectTimeoutSec = 0f`, so an uncollected reward intentionally waits forever. That matches the newer requirement "reward must not disappear", but it also removes the safety net for missing prompt / unreachable reward / missed trigger.
- The user failure "reward took, doors opened, no skill" is most consistent with `RewardPickup.WasCollected=true` plus `DraftManager.ShowDraft()` returning early / draft pick not producing `OnSkillPicked`, while the director still reaches door-open. The force-kill finding `Cleared + activeReward=null + timeScale=0.3` is a second failure mode: clear sequence either did not reach spawn or got interrupted before `finally RestoreGameplayTimeScale()`.

Demo surgical fix:
- Make `RoomRunDirector` the only authority for room clear transaction. `RewardPickup` should only emit `OnCollected`; it should not open exits or talk to legacy managers.
- Add a `RoomClearReconcile()` coroutine/heartbeat while lifecycle is `Cleared` or `RewardTaken`:
  - if state `Cleared` and no `activeReward` after 1.5 real seconds -> spawn/retry once at safe center; if spawn still fails -> open a forced draft directly via `DraftManager.ShowDraft()` and log.
  - if reward collected and no draft active/pending and no `OnSkillPicked`/gold/heal event after 0.5 real seconds -> show fallback 3-card draft or directly grant fallback gold/heal; log `[REWARD_FLOW_RECOVERED]`.
  - if timeScale is between 0 and 1 for more than 1.5 real seconds outside draft/class-select -> restore to 1 and log `[TIMESCALE_RECOVERED]`.
  - if draft closes or grant event observed -> `MarkRewardTaken()` then open doors.
- Add a transaction id: `roomRewardTxnId = $"{CurrentNodeId}:{clearCounter}"`; log each phase: `CLEARED`, `REWARD_SPAWNED`, `REWARD_COLLECTED`, `DRAFT_OPEN`, `DRAFT_PICKED`, `DOOR_OPEN`.
- Keep F12 panic, but it is a demo rescue, not the primary architecture.

Industry pattern:
- Hades visible pattern: reward is door-advertised, appears after all enemy waves are defeated, and doors unlock after clear/reward flow. Hades also has variants such as survival encounters where a timer destroys remaining enemies and reward spawns. Internals are BELIRSIZ, but the shipped behavior is a clean room transaction: encounter completion is the gate for reward and exit progression. Source: https://hades.fandom.com/wiki/Chambers_and_Encounters
- Enter the Gungeon visible pattern: room clear reward is probabilistic/weighted, but the "room clear" event is the stable trigger. It also has auto-collection/rescue behavior for some dropped currency/pickups when leaving or when items fall into pits. Internals are BELIRSIZ, but the pattern is "clear event -> deterministic reward roll -> pickup entity with fallback behavior". Source: https://enterthegungeon.fandom.com/wiki/Pickups
- Binding of Isaac visible pattern: room-clear awards are calculated on room clear, with chance influenced by conditions. Internals are BELIRSIZ, but the pattern is a single room-clear resolver, not each pickup deciding progression. Source: https://bindingofisaacrebirth.wiki.gg/wiki/Room_Clear_Awards
- Recommended RIMA pattern name: "authoritative room transaction + reconcile watchdog". Treat reward/draft/door as one state machine owned by `RoomRunDirector`; pickup/UI are presenters.

### B2. Wave size too small

RIMA root-cause hypothesis:
- `EncounterController` supports only opening wave plus second wave. It starts with `activeWave.threatBudget * openingBudgetFraction`, then spends remaining budget as the second wave.
- Current `Act1_Wave_Pilot.asset`: `threatBudget: 10`, `openingBudgetFraction: 0.4`, `nextWaveKillFraction: 0.5`, FractureImp cost 1/count 4, Bruiser cost 4/count 1, Other cost 2/count 2. Depending on spawn budget/weighted picks/max simultaneous, this can read as 1-2 enemies at a time.
- This is mechanically correct but visually under-filled for a Hades-style arena.

Demo surgical fix:
- Data-first pass, today:
  - Combat room 1: budget 14, opening fraction 0.45, kill fraction 0.65, max simultaneous 4-5.
  - Combat room 2/3: budget 18-22, opening fraction 0.35, kill fraction 0.55, max simultaneous 5-6.
  - Elite room: budget 24-28, one bruiser/warden-like big enemy plus 4-6 smalls, max simultaneous 6.
  - Boss room: boss direct plus 2 add waves at 70% and 35% HP if safe; if not implemented, fake via VFX-only telegraphs and spawned minion pairs.
- Code-small pass, if data still feels thin:
  - Extend `EncounterWaveSO` with `List<float> waveBudgetFractions` default `[0.35, 0.35, 0.30]`.
  - Replace `secondWaveSpawned` with `waveIndex`; spawn next wave when current wave dead fraction >= `nextWaveKillFraction` or after a real-time cadence guard.
  - Keep `ThreatBudget` and current spawn points.

Industry pattern:
- Hades visible pattern: most combat chambers have multiple enemy waves; reward spawns after all waves. It also has constant-spawn survival variants. Source: https://hades.fandom.com/wiki/Chambers_and_Encounters
- Risk of Rain 2 pattern: director credits. The director pays enemy credit costs and spawns enemies from a budget scaled by difficulty coefficient. This is the strongest public pattern for RIMA's existing `ThreatBudget`: budget, enemy cost, max simultaneous, director cadence. Source: https://riskofrain2.wiki.gg/wiki/Directors
- Dead Cells visible pattern: difficulty is biome/tier based; enemy/trap scaling differs per tier. RIMA should not scale only by room count; it should have room archetype + depth + elite/boss modifiers. Source: https://deadcells.wiki.gg/wiki/Biomes
- Recommended RIMA pattern name: "budgeted encounter director with wave slices".

### B3. M-overlay bleed / modal stack

RIMA root-cause hypothesis:
- `RunMapOverlay` is IMGUI in `OnGUI()`. Draft/skill/pause are Canvas-based. IMGUI order and Canvas sorting are separate, so draft cards can appear above/below unpredictably and bleed through if there is no single modal owner.
- `UIManager` already knows overlay flags, but `RunMapOverlay` is not integrated with it. It toggles itself on M and draws its own scrim.

Demo surgical fix:
- Make M map obey modal rules:
  - if `DraftManager.IsDraftActive || IsDraftPending`, ignore M or close map.
  - if map opens, tell `UIManager` a map overlay is open, pause/slow time consistently, and block draft/pause/settings beneath it.
  - draw scrim alpha >= 0.92 and consume all IMGUI events while open.
- Shortest patch: in `RunMapOverlay.OnGUI`, before toggle/open, check draft and UI flags. While open, draw a full-screen scrim first and call `GUI.ModalWindow`-style input capture or consume mouse/key events except M/Esc.
- Better post-demo: replace IMGUI with Canvas `RunMapCanvas` under one `ModalStackService`.

Industry pattern:
- Common shipped pattern across Hades / Gungeon / Isaac / Dead Cells: only one blocking screen owns input at a time. In-game maps, inventory, pause, and reward choice screens do not visually mix. The exact internal modal implementations are BELIRSIZ.
- Recommended RIMA pattern name: "single modal stack + scrim + input capture".

### B4. Background void

RIMA root-cause hypothesis:
- `BuildPersistentBackgroundIfPresent()` exists, but the arena still reads as a cliff island in black void. Either background controller is not present/wired in `_Arena`, layer order/camera bounds hide it, or art is too empty/dark.
- Fixed demo camera size 5.0 helps consistency but exposes black outside the island.

Demo surgical fix:
- Add one persistent parallax/background band behind all rooms:
  - far layer: desaturated blue/green fog texture or painted ravine.
  - mid layer: 2-3 cliff silhouettes / ruins at low alpha.
  - near edge: floor/cliff rim shadow, not pure black.
- Do not zoom out further until background exists. If camera grows, void gets worse. Keep 5.0 or 4.75 and make combat readable.

Industry pattern:
- Hades and Dead Cells both keep room edges readable with authored biome context, not empty black. Exact internals BELIRSIZ; visible pattern is "combat floor plus ambient context layer".

### B5. UI/gorsel

Demo surgical fix:
- Red low-HP vignette: reduce alpha by 40-60%, make pulse localized to screen edge, not full-screen wash.
- Enemy black blobs: sprite material/tint audit; force a minimum midtone and rim/outline. Silhouette-only is acceptable only for telegraphed shadow enemies, not default mobs.
- MainMenu backdrop: pin one background asset for demo; no random/variant backdrop.
- Settings/Pause/Director overlays: reuse same scrim color/alpha as map.
- Tooltip: anchor to card side with viewport clamp; no vertical strip.

### B6. Instrumentation gap

Demo surgical fix:
- Add small dev-only log panel toggled by F10 or visible in top-left during playtest:
  - `Wave 2/3 spawned: 5 enemies`
  - `Room cleared`
  - `Reward spawned at x,y`
  - `Reward collected`
  - `Draft opened`
  - `Skill picked: Iron Charge`
  - `Doors opened: 2`
  - `RECOVERED: reward respawn`
- Back it by a ring buffer, not Unity console. Console read returning 0 in play mode should not block diagnosis.

## B - Gameplay design

### B1. Hades-style wave system for RIMA

Target feel:
- The player should spend 45-75 seconds in a normal combat room during the demo, not 8-15 seconds.
- A wave should overlap slightly with the previous wave so the arena feels alive, but never spawn on top of the player.
- The player should see escalation: small enemies first, then mixed wave, then one heavier threat or ranged pressure.

Recommended demo room pacing:
- Room 1 combat tutorial:
  - 3 waves.
  - Wave 1: 3 small FractureImp.
  - Wave 2: 3 small + 1 medium.
  - Wave 3: 2 small + 1 bruiser/other.
  - Spawn next wave when 65% of current wave is dead or 8 real seconds after wave start.
- Room 2 combat:
  - 3 waves.
  - Wave 1: 4 small.
  - Wave 2: 4 small + 1 medium.
  - Wave 3: 2 medium + 2 small.
  - 50-60 seconds target.
- Elite room:
  - 4 waves or 3 waves plus elite.
  - Wave 1: 4 small.
  - Wave 2: elite/big spawn with telegraph.
  - Wave 3: small adds while elite remains.
  - Reward should be visibly better.
- Post-boss combined-kit room:
  - 2 dense waves for power fantasy: more enemies but lower HP.

How to fit existing system:
- Keep `EncounterController`, `ThreatBudget`, `EncounterWaveSO`.
- For immediate demo: tune `Act1_Wave_Pilot.asset` and add room-depth difficulty in `DifficultyForCurrentRoom()`.
- For code-small improvement: add wave fractions as data and make `EncounterController` loop through wave slices. Do not rewrite spawn director now.

Concrete values:
- Normal combat budget: 16/20/22 by depth.
- Elite budget: 26-30.
- Opening fraction: 0.35-0.45.
- Next wave kill fraction: 0.55-0.65.
- Max simultaneous:
  - small: 5-6
  - medium: 2
  - bruiser: 1
- Spawn cadence:
  - first wave immediately after room banner.
  - next wave when dead fraction threshold is reached, but minimum 2.0s after previous wave start.
  - never spawn within 3.0 world units of player.

### B2. Demo boss-mob

Goal:
- One mob should feel like a boss even before full mechanics exist.

Minimum demo boss:
- Scale: 1.45x-1.7x visual only; keep collider controlled. Do not make root scale break hit/collider. Use child visual scale.
- HP: 6-8x normal bruiser, but cap phase duration. If average player DPS makes it >90 sec, reduce HP.
- Posture/stagger: boss should briefly flinch or expose a 1.0s punish window every 25-30% HP.
- Telegraphs:
  - 0.8s ground marker before slam.
  - 0.6s line/cone marker before dash/cleave.
  - VFX placeholder is acceptable if damage mechanic is not ready, but the telegraph must be visible and timed.
- Phase:
  - 100-70%: single telegraph attack loop.
  - 70-35%: add VFX burst + 2 minions once.
  - 35-0%: faster telegraph cadence or red tint, not more raw HP.
- Arena:
  - remove clutter near spawn.
  - keep boss center-biased.
  - add health bar + boss intro sting.

Demo-critical boss skill placeholders:
- The placeholder VFX must not lie. If the VFX implies damage, either damage is real or label it as harmless only during internal testing. For demo, a telegraph without damage still sells boss only if hit reactions, audio, and boss HP bar are present.

### B3. Mob sizes and readability

Recommended ratios for top-down 3/4 ARPG:
- Player: baseline 1.0.
- Small mob: 0.8-0.95 player height.
- Medium mob: 1.05-1.2 player height.
- Bruiser/elite: 1.3-1.5 player height.
- Boss: 1.6-2.0 player height, but collider should be closer to 1.1-1.3 player footprint unless the visual clearly occupies more ground.

Readability rules:
- Mobs need hue/value separation from floor and cliff. Current "black blob" read is below demo bar.
- Every enemy needs at least one of:
  - bright eye/core accent,
  - rim outline,
  - colored attack windup,
  - shadow separate from body.
- Small mobs can be simple, but their facing/attack state must read within 0.2 sec.

## C - Comprehensive playtest plan

### C1. Golden-path checklist

Build/scene startup:
- Test: launch demo path from MainMenu to `_Arena` or dev-direct `_Arena`.
- PASS: player appears, HUD appears, timeScale=1, no blocking overlay, first room label visible, opening kit draft either appears or auto-picks after timeout.

Opening kit:
- Test: pick first opening skill.
- PASS: selected skill appears in skill bar, button works in combat, event log says `Skill picked`.

Combat feel:
- Test: room 1 and room 2 natural play, no force-kill.
- PASS: player uses LMB and at least one Q/E/R/F skill naturally; hit reactions and audio communicate contact; no enemy death leaves stuck collider; player can dodge/kite.

Wave pacing:
- Test: count enemies per room and time-to-clear.
- PASS normal room: 3 waves, 8-18 total enemies depending archetype, 45-75 sec clear for average play.
- PASS elite room: at least one big threat plus adds, 60-100 sec clear.
- FAIL: room clears in under 20 sec with only 1-2 enemies, unless it is an explicit free/chest/shop room.

Reward flow:
- Test: clear room naturally, wait for reward, press G, pick skill.
- PASS: reward appears within 1.5 sec after clear flash, prompt appears, G opens draft, selected reward grants visible effect or currency/heal feedback, doors open only after draft resolves.
- PASS data proof: event log contains `CLEARED -> REWARD_SPAWNED -> REWARD_COLLECTED -> DRAFT_OPEN -> DRAFT_PICKED -> DOOR_OPEN`.
- FAIL: doors open with no reward/draft/grant.

Reward recovery:
- Test: leave reward uncollected for 15 sec.
- PASS: reward remains; no timeScale stuck; event log still shows waiting state.
- Test: force reward spawn obstruction by standing on center / near pickup.
- PASS: prompt still appears or reward relocates to nearest safe cell.

Run-map navigation:
- Test: press M during combat, during draft, after clear, and near doors.
- PASS: map is readable, no draft/card bleed, no input leaks, current node highlighted, chosen door advances to correct child.
- PASS branch: left/right door choice maps to expected child node.

Door flow:
- Test: after skill pick, approach each available door and press G.
- PASS: closed doors have disabled trigger; open doors show prompt and pulse; entering one advances once; no double advance.

Overlay/UI:
- Test: pause/settings/map/draft combinations.
- PASS: only one blocking overlay visible; scrim covers gameplay; ESC/M behavior predictable; timeScale restored to 1 after close.

Boss:
- Test: reach boss naturally.
- PASS: boss intro, HP bar, visual scale, at least 2 attack telegraphs, one phase change/add moment, reward/secondary class flow after kill.
- FAIL: boss reads as normal mob with extra HP.

Death:
- Test: let player die in combat and boss.
- PASS: death UI appears, timeScale stable, restart returns to playable scene, no leftover draft/reward/map overlay.

Economy/reward types:
- Test: pick skill, gold, heal, echo if offered.
- PASS: each choice has immediate visible feedback; skill bar/loadout/economy values update.

Background/camera:
- Test: all demo rooms at 16:9 and target laptop resolution.
- PASS: no large black void, player/enemies stay readable, room exits visible, camera does not expose broken edges during follow.

Instrumentation:
- Test: play one full run and export/inspect event log.
- PASS: every room has start, wave, clear, reward, draft, door events. Any recovery event is visible with room/node id.

### C2. "Good play" criteria

Combat:
- The player has a reason to move every 2-4 seconds.
- Hits have readable confirmation: enemy flash/knockback/audio/hitstop.
- Enemy attacks are avoidable after telegraph; damage does not feel random.
- Time-to-kill small enemies: 2-5 basic hits or one skill combo.
- Time-to-kill medium enemies: 6-12 seconds.

Wave rhythm:
- No dead air longer than 2.5 sec between waves unless intentional reward/clear pause.
- New wave appears before the room feels over, but not so early that the player cannot parse threats.
- Peak on-screen enemies: 5-7 for normal room, 6-9 for elite if sprites are readable.

Readability:
- Player, enemies, pickups, doors, telegraphs, and UI should be identifiable in a screenshot with no explanation.
- Red low-HP overlay should signal danger while leaving enemies/telegraphs visible.

Flow:
- Player should always know: fight, take reward, choose card, choose door.
- No step in that chain should require developer knowledge.
- Reward must never feel "fake": if the player picks something, a visible stat/skill/currency/log changes.

Demo-ready threshold:
- Three consecutive natural runs through the demo golden path without F12 panic.
- Zero reward-flow stalls.
- Zero timeScale stuck cases.
- Zero modal bleed cases.
- One successful boss kill plus post-boss room.
- Event log matches observed play.

### C3. Automatic vs manual split

Automatic / MCP execute_code or Unity tests:
- Load `_Arena`; assert `RoomRunDirector`, `DraftManager`, `UIManager`, HUD exist.
- Force all enemy Health deaths; assert lifecycle reaches `Cleared`, reward exists within 2 sec, timeScale returns to 1.
- Programmatically call reward collect path; assert draft opens, then simulate pick, assert doors open.
- Assert `RewardPickup` no longer opens legacy exits directly after refactor.
- Assert `RunMapOverlay` refuses to open while draft active.
- Assert `EncounterController` emits 3+ wave start events for demo wave config if wave-slice code is added.
- Assert boss prefab has Health, boss bar target, collider, visual scale child, at least placeholder telegraph component/VFX reference.

Manual / OBS / player-facing:
- Combat feel: hitstop, juice, control responsiveness.
- Enemy readability on real monitor and recording compression.
- Whether wave density feels exciting or messy.
- Whether boss feels like a boss, not just a large mob.
- Background/camera composition.
- Tooltip placement and overlay polish.
- Whether player understands "G take reward" and door choice without instruction.

## Sources used

- Hades chambers/rewards/waves/doors visible behavior: https://hades.fandom.com/wiki/Chambers_and_Encounters
- Risk of Rain 2 director budget/credits pattern: https://riskofrain2.wiki.gg/wiki/Directors
- Enter the Gungeon room-clear pickup/reward behavior: https://enterthegungeon.fandom.com/wiki/Pickups
- Binding of Isaac room-clear awards: https://bindingofisaacrebirth.wiki.gg/wiki/Room_Clear_Awards
- Dead Cells biome/difficulty scaling: https://deadcells.wiki.gg/wiki/Biomes

## Final recommendation

For the 3-day demo, do not start with a broad rewrite. First make `RoomRunDirector` the sole authority for reward/draft/door, add reconcile logs, and block overlay bleed. Then data-bump waves. If the playtest still feels empty after the data bump, add wave slices to the existing `EncounterController` rather than building a new director.
