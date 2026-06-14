# RIMA Control Scheme + HUD Bars — FINAL SYNTHESIS (S6)

> 3-source synthesis: **cx** (code-grounded architecture, CODEX_DONE.md) + **ax** (genre/UX research, AGY_DONE_ydbilgin.md)
> + **Opus** (final decision). Criterion #1 = **RIMA-fit** (surgical, matches existing code, no regression to the
> working cursor-aim). This doc is the LOCK. Implementation is a separate greenlit step — nothing here is coded yet.

---

## 0. The big picture (one line)
RIMA's controls are **already 80% right** (cursor-aim works, combat-facing lock works). The real work is **plumbing
(rebinding) + consistency (labels match real keys) + de-duplication (two settings menus, displayed-vs-actual key
mismatch)** — NOT a redesign. Three concrete bugs surfaced; fixing them is the whole job.

## 1. Cursor-aim attack — KEEP + small polish (both sources agree)
- **Melee = snap-on-press + swing-lock.** Face the cursor the instant you click; lock facing during the active
  swing frames; allow re-aim between combo steps. ✅ Already wired: `PlayerController.FaceCombatTarget()` →
  `GetMouseDirectionOrFallback()`, default `CombatAimMode.TowardsMouse`, and `MeleeChain/CastRhythm/HeatGauge/
  MarkPulse/VeilStrike` + `SkillBase.TryActivate` all call it. **Do NOT touch `FacingDirection`,
  `FaceCombatTarget`, or `combatFacingLockDuration`.**
- **Ranged = continuous cursor tracking.** Standard for kiting (Children of Morta / Hades rail). RIMA gap (cx): Ranger
  `ShotCadence` is **snap-on-release** (charge stored at press, aim sampled at release in `ExecuteArrow`). Acceptable
  for demo; **polish later** = sample aim continuously during charge + faint aim line. Not a blocker.
- **Aim mode stays a toggle** (TowardsMouse default; CharacterFacing for gamepad/accessibility). Already migrated.

## 2. Default keybinds — LOCKED (Opus resolves cx↔ax split)
ax proposed number-row 1-4 for skills; **cx showed the code already uses the Q/E/R/F letter cluster** via
`KeyBindManager`, and ax itself warned the number row is an awkward reach while holding WASD. **Decision: letter
cluster wins** (surgical + ergonomic). The skill bar currently MIS-LABELS these as "1-5" — that label is cosmetic and
gets fixed to show the real key (see §4 Bug-1).

| Action | Default key | Rebindable? | Notes |
|---|---|---|---|
| Move | W A S D | yes (as a group) | movement; block skill collisions unless user also rebinds move |
| Dash | Space | yes | thumb reflex |
| Primary attack | **LMB** | yes (conflict-guarded) | cursor-aimed, bar slot 0 |
| Class secondary | **RMB** | yes (conflict-guarded) | cursor-aimed, bar slot 1 |
| Skill 1 | **Q** | yes | bar slot 2 |
| Skill 2 | **E** | yes | bar slot 3 |
| Skill 3 | **R** | yes | bar slot 4 |
| Skill 4 | **F** | yes | bar slot 5 |
| Skill 5 | **Left Shift** | yes | bar slot 6 — best for a mobility/defensive skill |
| Ultimate / Rift-Break | **V** | yes | rage-gated; already wired; kept distinct (NOT on the 7-slot bar) |
| Map overlay | Tab | **no (reserved)** | |
| Pause | Esc | **no (reserved)** | |
| Interact (chest/forge) | proximity/auto for demo | — | RIMA gates/pickups are auto; avoids F-collision. Add a manual context key only if needed. |

- Warblade's hardcoded **Z/X** secondary slots → fold into the registry as Skill5/extra or retire; don't leave them
  as literals.
- Gamepad parallels: leftStick move, South dash, West attack, East secondary; skills on face/shoulder/dpad later.

## 3. Rebinding architecture — surgical, NO InputActionAsset migration (cx-led)
**Do NOT migrate gameplay to `InputSystem_Actions.inputactions` / PlayerInput.** That asset is generic
(Move/Look/Attack/Jump/Sprint), lacks RIMA's Skill1-5/ClassSecondary/RiftBreak/aim semantics, and a migration would
touch PlayerController + PlayerAttack + all 5 class skill controllers + UI + generated wrappers + tests = high
regression for a demo. Instead, **expand the existing `KeyBindManager` into a gameplay binding registry:**
1. Action ids: `MoveUp/Down/Left/Right, Dash, Attack, ClassSecondary, RiftBreak, Skill1..Skill4` (4 skills — see §7).
2. Default binding paths (the table in §2) + **PlayerPrefs JSON persistence** (`SaveBindingOverridesAsJson`-style).
3. Interactive rebind helper (press-to-bind) + **duplicate/reserved-key guard** (Esc/Tab reserved; LMB/RMB can't be
   silently stolen by a skill slot).
4. `OnBindingsChanged` event.
5. Replace local `AddBinding` literals in `PlayerController`, `PlayerAttack`, `Warblade_/Elementalist_/Ranger_/
   Shadowblade_SkillController`, `RoninController` with registry calls.

## 4. The three bugs this pass must fix
- **Bug-1 (display ≠ reality):** `SkillBarUI.SlotKeys = {LMB,RMB,1,2,3,4,5}` is hardcoded, but skills actually fire on
  Q/E/R/F(/Z-X). → make slot labels **binding-driven** from the registry. (One-string-array → registry lookup.)
- **Bug-2 (two settings menus):** `UI/SettingsMenuUI.cs` builds an aim toggle on key `setting_aim_mode` that is **NOT**
  `PlayerController.AttackAimMode`; `Core/SettingsMenu.cs` has the REAL DashMode + AttackAimMode toggles but is a
  separate ESC/timeScale owner. → make **UIManager/SettingsMenuUI canonical**, wire its aim toggle to
  `PlayerController.AttackAimMode`, add DashMode + a compact Controls/rebind section, **retire Core/SettingsMenu**.
- **Bug-3 (fake slot risk):** if a class has <5 real skills, don't render empty "1-5" rows as if bindable. Bar should
  reflect `GetActiveSlotCount()` (it already does) AND label from real bindings.

## 5. HUD bars — canonical layout (cx + Opus, Ashen-Glyph aesthetic)
`HUDController` owns the combat-HUD canvas + all top-level anchors. Use the S6 placeholder frames I generated.

| Zone | Element | Owner | Placeholder asset |
|---|---|---|---|
| **Top-left** (stacked) | HP bar + class resource (Rage) | HUDController | translucent stone frame + cyan/red fills |
| **Top-right** | Minimap | HUDController.BuildMiniMap | `minimap_frame_stone.png` |
| **Top-center** (just below margin) | Boss health bar | BossHealthBar (Show/Hide only) | `boss_skull_glyph.png` + bar |
| **Bottom-center** | Skill bar (7 hex slots, cooldown sweep, **binding-driven labels**) | SkillBarUI | `hex_slot_mask` + `icon_frame_hex` |
| **Bottom-center, above bar** | Interaction prompt | UI prompt | `RIMA_UI_PromptFrame` |
| **Full-screen additive** | Low-HP vignette | HUDController | `lowhp_vignette.png` |

Rules: **BossHealthBar must NOT self-create under the first arbitrary Canvas** and must not sit at the bottom
competing with the skill bar — anchor it top-center under the HUD canvas. `CharacterHPBar` = optional world-space
feedback, not the canonical player HP HUD. Interaction prompt must not overlap the skill bar.

## 6. Risks — what NOT to touch (cx)
Do NOT: wholesale switch to PlayerInput/InputActionAsset this pass · change `FacingDirection`/`FaceCombatTarget`/
`combatFacingLockDuration`/dash-cancel while adding rebinding · keep two settings menus writing different pref keys ·
hardcode visual key labels after the registry exists · make Esc/Tab rebindable until overlay routing is redesigned ·
let a binding change require editing every class controller again.

## 7. RESOLVED (user, 2026-05-30): **4 active skills = Q / E / R / F**
- Skill bar = **LMB + RMB + 4 hex = 6 slots** (drop the old 7th "5" slot and the Shift-slot from §2).
- **Left Shift skill is REMOVED.** Skills are exactly Q, E, R, F.
- Ultimate / Rift-Break stays **V** (separate from the bar). Dash stays Space.
- `SkillBarUI.SlotCount` 7 → 6; `SlotKeys` becomes registry-driven `{Attack, ClassSecondary, Q, E, R, F}` labels.
- Warblade's hardcoded Z/X secondary slots → retire (no longer mapped to bar slots).

## 8. Suggested implementation order (when greenlit — separate step)
1. KeyBindManager → registry + PlayerPrefs persistence + guard (foundation).
2. Repoint PlayerController/PlayerAttack/skill controllers to registry calls.
3. SkillBarUI labels binding-driven (kills Bug-1).
4. Unify settings menu + wire aim/dash toggles + Controls/rebind section (kills Bug-2).
5. HUD anchor pass (BossHealthBar top-center, bars use placeholders).
6. F5 play-verify: rebind a key, confirm label + behavior update; confirm cursor-aim still snaps.
