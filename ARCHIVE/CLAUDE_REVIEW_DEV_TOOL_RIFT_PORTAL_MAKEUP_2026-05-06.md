# Claude Review - Dev Tool, Rift Portal, Makeup VFX Notes - 2026-05-06

Status: draft for Claude review.
Owner intent: capture user feedback and Codex recommendation before production work.

Sources checked:
- Meta Alchemist repost: https://x.com/meta_alchemist/status/2051280891239145669
- Original Nicolas Zullo Codex/game workflow: https://x.com/NicolasZu/status/2045494495546835416/video/1
- Hades Chaos Gate reference: https://hades.fandom.com/wiki/Chaos_Gate
- Hades Chaos Boons reference: https://hades.fandom.com/wiki/Chaos/Boons_(Hades)
- Hades Boons overview: https://hades.fandom.com/wiki/Boons

## 1. Codex-Inside-Game Dev Tool Note

The referenced X video is not evidence that a polished production game was generated from one prompt.
It shows a web game running inside a Codex browser tab, with a Codex-made `Building Designer`
route/tool at `localhost:5173/__dev/building-designer`.

Visible workflow:
- The game is playable inside Codex/browser.
- A dev-only building designer panel edits modular building parts.
- The panel supports part selection, add/duplicate/remove/reset, color palette, size fields, and save.
- A `Save to Real Game` button appears to persist the edited building back into the game.
- The second video shows the edited base/building layout inside the live wave-defense game.

RIMA feasibility:
- A similar tool is not hard if the first scope is narrow and runtime-only.
- Recommended first version: Unity Play Mode dev overlay, not a full editor extension.
- Store output as JSON or ScriptableObject room/building descriptors.
- Use ghost placement, grid snap, rotate, delete, palette/variant selection, and one `Save Descriptor`
  action.
- Do not start with multiplayer-grade undo/redo, arbitrary prefab editing, or final art authoring.

Recommended RIMA tool targets:
- Room blockout designer: playable floor, visual shell, blockers, gate sockets.
- Prop/object placement: trees, crates, pillars, shrine markers, rift scars.
- Encounter preview: spawn anchors, wave markers, elite socket, threat budget labels.
- Makeup/VFX preview: buff aura toggles, speed trail, status pip placement, hit flash preview.
- PixelLab import checker: object scale, isometric footprint, sorting order, player silhouette read.

Codex recommendation:
- Build this as a dev acceleration layer after the active UI/minimap/combat-camera acceptance pass.
- First milestone should be a small `RoomDescriptorEditorOverlay` that edits one descriptor and saves it.
- Claude should decide whether this belongs in Unity runtime UI, Unity Editor tooling, or a small external
  web tool. Codex preference: Unity runtime overlay first, because RIMA's camera, sorting, collision,
  prefabs, and player scale already live there.

## 2. Random Rift Portal Proposal

User request:
- Add very rare "rift portal" spawns like Hades Chaos Gates.
- They should spawn randomly in random places, even with very low probability.
- Passing through gives a strong effect, paired with a permanent or encounter-style negative effect.
- User described it as "like a Chaos Orb."

Hades reference summary:
- Chaos Gates are floor portals that can appear after a room is cleared.
- Entering costs health unless a specific keepsake is equipped.
- They do not appear everywhere; Hades limits them around shops, bosses, and recent Chaos Gates.
- Chaos boons pair a temporary curse/downside with a later blessing.
- Chaos blessings can stack and cannot be sold; many are run-shaping stat/reward modifiers.

RIMA adaptation name options:
- Rift Tear
- Yarik Portal
- Fracture Gate
- Deep Rift
- Echo Rift

Recommended mechanical identity:
- This should not replace `Curse Gate` room type from `TASARIM/ROOM_MECHANICS.md`.
- Treat it as a rare post-clear floor event that can appear inside eligible rooms.
- The player may ignore it and continue normally.
- If entered, it opens a small interstitial rift space or direct offer screen.
- Offer format: choose 1 of 3 `Burden + Gift` pairs.
- Burden duration: 2-4 encounters by default.
- Gift duration: run-permanent by default, but some high-power gifts can be encounter-limited.

Spawn rules for Claude review:
- Eligible after Combat, Elite, and Unknown combat rooms are cleared.
- Not eligible in Shop, Spirit, Boss, tutorial/start, or immediately after boss rooms.
- First possible spawn only after the player has completed 2 combat rooms.
- Base chance: 3%-5% per eligible room.
- Act scaling: Act 1 lower, Act 2/3 slightly higher.
- Max 1 per act or max 2 per full run.
- Minimum spacing: 6-8 rooms between rift portals.
- Spawn point: random valid floor point near room edge or visual-shell seam, never blocking gate sockets,
  reward pickup, enemy spawn anchors, or required path.
- Add a short pre-open cue after room clear: floor crack, whisper sound, cyan/purple smoke, then interact prompt.

Why this is valuable:
- It gives rare "run story" moments without adding another full room type.
- It fits RIMA's rift identity better than generic shrine rewards.
- It creates player risk judgment: enter now, delay build plan, or keep current route.
- It gives high-power effects a fair cost and lets the player opt in.

Balance risk:
- If the gift is profile-permanent, it can break roguelite progression and create save-balance debt.
- Codex recommends "run-permanent" as the default meaning of permanent.
- True account-permanent rewards should be extremely rare, cosmetic/meta-currency bound, and separated from
  combat power unless Claude explicitly approves.

Example Burden + Gift pairs:

| Burden | Duration | Gift | Duration |
|---|---:|---|---:|
| Move speed -15% | 3 encounters | Dash leaves a damaging rift trail | run |
| Reward previews hidden | 4 chambers | All skill offers gain +1 rarity roll | run |
| Next 3 rooms spawn +20% enemies | 3 encounters | Damage +18% when surrounded by 3+ enemies | run |
| Skill cooldowns +20% | 2 encounters | LMB/RMB combo finishers trigger a rift echo hit | run |
| Max HP -12% | run | Gain +1 revive shard or emergency shield proc | run |
| Dash cooldown +30% | 3 encounters | Every 4th dash creates a pull pulse | run |
| Pickups heal 0 HP | 3 encounters | Skill damage lifesteals 2% vs elites/bosses | run |
| Minimap route previews disabled | 3 rooms | Next 3 rewards are upgraded or tagged | 3 rewards |

Claude decisions requested:
- Should Rift Portal be a post-clear surprise, a route node, or both?
- Should the reward be run-permanent by default, or should most gifts be encounter-limited?
- Should it reuse `Curse Gate` logic or become a separate `RiftOpportunity` system?
- Should it appear on minimap after spawn, or remain room-local only?
- Should the offer UI use 1 powerful choice or 3 blind/partial choices?

Codex recommendation:
- Implement separately from `Curse Gate`.
- Use post-clear room-local spawn first.
- Use 3 visible `Burden + Gift` choices.
- Make gifts run-permanent, burdens encounter-limited, with hard caps per run.
- Do not add true profile-permanent combat power until the meta-progression economy is locked.

## 3. Makeup / Micro VFX Pass

User request:
- Think about simple visual "makeup" for everything.
- Example: when the character gets a speed buff, add light wind under/behind the character.
- This should become a deliberate polish layer, not an accidental one-off.

Definition:
- Makeup VFX are small runtime visual cues that make states readable and satisfying.
- They are not baked into PixelLab character animation frames.
- They should be Unity-side VFX/prefabs/particles/sprite overlays so they can turn on/off with state.

Rules:
- One state should have one readable cue family.
- Avoid stacking multiple large auras on the same character.
- Keep player feet, facing, enemy telegraphs, and floor hazards readable.
- Class-owned states keep class-owned colors/shapes.
- Buff makeup should be subtle during combat and clearer on activation.

Recommended makeup categories:

| Gameplay state | Makeup cue | Notes |
|---|---|---|
| Move speed up | light wind streaks at feet/back, short dust taper | User example. Good universal buff cue. |
| Move speed down | heavy foot shadow, dragging dust, muted trail | Avoid ice look unless Frozen/Chill. |
| Dash buff | short afterimage, sharper foot spark, directional smear | Class color can tint. |
| Attack speed up | brief hand/weapon cadence shimmer | Do not hide weapon silhouette. |
| Damage up | small pulse on hands/weapon/core rift | Activation pulse stronger than idle cue. |
| Defense up/shield | thin rim shield, tiny impact glint on hit | Avoid full bubble unless shield is the mechanic. |
| Vulnerable/defense down | cracked outline or exposed core pip | Do not steal Warblade Sundered shard language. |
| Burn | ember motes and heat shimmer | Elementalist fire identity only when applied by class. |
| Frozen/Chill | cold mist at feet, small frost pips | Do not fully recolor character body. |
| Shock | small intermittent arc on target pip | Keep arcs tiny; no full lightning storm. |
| Marked | target reticle pip, line flash on apply | Ranger Mark must not look like Shadowblade Scar. |
| Scar | violet seam/thread decal, collapses on consume | Shadowblade-owned. |
| Cracked/Shattered | organic body fissure decal | Brawler-owned; not metallic armor shards. |
| Sundered | armor plate fissure/shard decal | Warblade-owned only. |
| Stun | short ring/star pulse above head or center mass | Avoid cartoon stars if tone rejects it. |
| Invulnerable | brief white/cyan rim blink | Should not look like damage shield unless active shield. |
| Low HP | subtle blood-edge vignette or heartbeat pulse | Do not use HP execute color cues on enemies. |
| Resource full | class resource icon pulse + tiny body accent | Prefer HUD first, body cue second. |
| Rift Portal nearby | floor crack smoke + low hum + interact glint | Room-local, not screen-wide. |

Makeup production approach:
- Add a `StateMakeupProfile` data asset per state or buff family.
- Bind profiles to existing status/effect IDs rather than hardcoding per skill.
- Use activation burst + idle loop + expire puff for each major state.
- Let class skills request profile variants through tags, but do not let every skill invent a new cue.
- Add a debug overlay to preview states on the player and a dummy enemy.

First implementation candidates:
- `speed_up_wind_trail`
- `speed_down_drag_dust`
- `damage_up_hand_pulse`
- `shield_rim_glint`
- `marked_target_pip`
- `rift_portal_floor_crack`

Claude decisions requested:
- Should `Makeup VFX` become its own production pass after skill logic, or be required per skill contract?
- Should universal buffs use neutral cyan/white, or class-tinted variants?
- Which states are allowed to show body overlays vs HUD/world pips only?
- Should RIMA define a hard max of simultaneous player makeup loops, e.g. 2 body loops + 2 foot loops?

Codex recommendation:
- Make this a separate Unity-side production pass.
- Add required makeup rows to future visual contracts, but do not block current logic work on full polish.
- Start with universal speed/damage/shield/mark cues because they improve readability fastest.

## 4. Open Review Summary For Claude

Requested Claude review:
- Confirm whether a RIMA internal room/building/prop designer tool should be prioritized.
- Decide the first tool host: Unity runtime overlay vs Unity Editor extension vs web tool.
- Decide Rift Portal placement, frequency, reward duration, and relation to existing Curse Gate.
- Approve or modify the Makeup VFX taxonomy before assets or code are produced.

Codex bottom line:
- The dev tool is feasible and useful if scoped tightly.
- The rift portal idea fits RIMA strongly, but should be separated from the existing route-level Curse Gate.
- Makeup VFX should become a deliberate runtime visual language layer, not baked PixelLab noise.
